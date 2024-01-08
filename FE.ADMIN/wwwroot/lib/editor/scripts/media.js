$(document).ready(function () {
    $('.tab_btn .btn').click(function () {
        var id = $(this).attr('href');
        if (id == '#tab-1' && $(this).hasClass('red')) { $('#btn_tab_1').click(); return false; }
        $('.tab_btn .btn.red').removeClass('red').addClass('blue');
        $('.tab_content').addClass('hide');
        $(this).addClass('red').removeClass('blue');
        $(id).removeClass('hide');
        return false;
    });

    $('#videoList').on('click', '.oneVideo .btn_Insert', function () {
        var id = $(this).attr('data-id');

        $.ajax({
            type: "GET", url: "/Tinymce/InsertVideo", data: { id: id }, dataType: "html",
            success: function (msg) {
                insertContent(msg);
            },
            error: function (request, error) {
                console.log(arguments); alert(" Can't do because: " + error);
            }
        });

        return false;
    });

    $('#form-youtube').submit(function () {
        debugger
        var youtube = $('input[name=youtube]').val();
        var id = youtube_parser(youtube);
        var content = '<iframe width="100%" height="400px" class="exp_video" src="https://www.youtube.com/embed/' + id + '?rel=0" frameborder="0" allowfullscreen></iframe>';
        insertContent(content);
        return false;
    });

    $('#form-embed').submit(function () {
        var embed = $('textarea[name=embed]').val();
        insertContent(embed);
        return false;
    });

    $('#btn_tab_1').click(function () {
        $('#frm_upload input[type=file]').click();
        return false;
    });
    $('#frm_upload input[type=file]').change(function () {
        // Lấy file từ input
        var file = $(this)[0].files[0];

        // Hiển thị progress bar và bắt đầu tải
        $('#frm_upload').addClass('hide');
        $('.uploadbar').removeClass('hide');

        // Slice và upload video
        sliceVideoAndUpload(file);

        // Gửi form sau khi đã chuẩn bị dữ liệu
        $('#frm_upload').submit();

        return false;
    });

    $('#frm_upload').submit(function (e) {
        e.preventDefault(); // Ngăn chặn sự kiện mặc định của form
        return false;
    });
    function generateRandomString(length) {
        const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
        let result = '';
        const charactersLength = characters.length;

        for (let i = 0; i < length; i++) {
            result += characters.charAt(Math.floor(Math.random() * charactersLength));
        }

        return result;
    }

    const fileIdValue = generateRandomString(10);
    async function uploadVideoPartsSequentially(file, sliceSize) {
        const totalParts = Math.ceil(file.size / sliceSize);

        async function uploadPart(partNumber) {
            const start = partNumber * sliceSize;
            const end = Math.min(start + sliceSize, file.size);
            const blob = file.slice(start, end);

            const formData = new FormData();
            formData.append('fileId', fileIdValue);
            formData.append('chunk', blob);
            formData.append('chunkIndex', partNumber);
            formData.append('totalParts', totalParts);

            try {
                const response = await $.ajax({
                    url: '/Tinymce/UploadChunk',
                    type: 'POST',
                    data: formData,
                    contentType: false,
                    processData: false
                });

                const percentComplete = ((partNumber + 1) / totalParts) * 100;
                console.log(`Part ${partNumber + 1} uploaded successfully. Progress: ${parseInt(percentComplete)}%`);
                $('.uploadbar .bar').css('width', parseInt(percentComplete) + '%');
                $('#lbl_upload').text(parseInt(percentComplete) + '%');
                if (partNumber + 1 === totalParts) {
                    console.log('All parts uploaded successfully.');
                    $('.uploadbar').css('margin-top', '15px');
                    $('.progress-striped').removeClass('progress-striped');
                    $('.uploadbar .bar').css('width', parseInt(percentComplete) + '%');
                    $('#lbl_upload').text(parseInt(percentComplete) + '%');
                    setTimeout(function () {
                        $('.progressbar').removeClass('hide');
                        setTimeout(function () {
                            $('.progressbar .bar').css('width', '100%');
                        }, 200);
                    }, 1000);

                    insertContent(response);
                } else {
                    // Upload next part recursively
                    await new Promise(resolve => setTimeout(resolve, 500));
                    await uploadPart(partNumber + 1);
                }
            } catch (error) {
                console.error(`Error uploading video part ${partNumber}:`, error);
            }
        }

        // Start uploading the first part
        await uploadPart(0);
    }
    async function sliceVideoAndUpload(file) {
        const sliceSize = 10 * 1024 * 1024; // Kích thước của mỗi phần (đơn vị byte)

        // Gọi hàm để cắt và gửi từng phần lên server
        await uploadVideoPartsSequentially(file, sliceSize);
    }
});

var editor = top.tinymce.activeEditor, selection = editor.selection, dom = editor.dom;
function insertContent(content) {
    editor.execCommand('mceInsertContent', false, content);
    editor.windowManager.close();
}
function youtube_parser(url) {
    var regExp = /^.*((youtu.be\/)|(v\/)|(\/u\/\w\/)|(embed\/)|(watch\?))\??v?=?([^#\&\?]*).*/;
    var match = url.match(regExp);
    return (match && match[7].length == 11) ? match[7] : false;
}
