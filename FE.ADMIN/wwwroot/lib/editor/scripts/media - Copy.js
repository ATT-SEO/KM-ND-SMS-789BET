$(document).ready(function(){
    $('.tab_btn .btn').click(function(){
        var id = $(this).attr('href');
        if(id=='#tab-1' && $(this).hasClass('red')) {$('#btn_tab_1').click(); return false;}
        $('.tab_btn .btn.red').removeClass('red').addClass('blue');
        $('.tab_content').addClass('hide');
        $(this).addClass('red').removeClass('blue');
        $(id).removeClass('hide');
        return false;
    });

    $('#videoList').on('click', '.oneVideo .btn_Insert', function () {
      var id = $(this).attr('data-id');

      $.ajax({
      type: "GET", url: "/Tinymce/InsertVideo", data:  {id: id}, dataType: "html",
      success: function(msg){
        insertContent(msg);
      },
          error: function(request,error) {
              console.log(arguments); alert ( " Can't do because: " + error );
          }
    });

      return false;
    });

    $('#form-youtube').submit(function(){
        debugger
        var youtube = $('input[name=youtube]').val();
        var id = youtube_parser(youtube);
        var content = '<iframe width="100%" height="400px" class="exp_video" src="https://www.youtube.com/embed/'+id+'?rel=0" frameborder="0" allowfullscreen></iframe>';
        insertContent(content);
        return false;
    });

    $('#form-embed').submit(function(){
        var embed = $('textarea[name=embed]').val();
        insertContent(embed);
        return false;
    });

    $('#btn_tab_1').click(function(){
        $('#frm_upload input[type=file]').click();
        return false;
    });
    $('#frm_upload input[type=file]').change(function(){
        $(this).parents('form').submit();
    });

    $('#frm_upload').submit(function () {
        $('#frm_upload').addClass('hide');
        $('.uploadbar').removeClass('hide');

        $(this).ajaxSubmit({
            uploadProgress: function (event, position, total, percentComplete) {
                $('.uploadbar .bar').css('width', percentComplete + '%');
                $('#lbl_upload').text(parseInt(percentComplete) + '%');

                if (percentComplete == 100) {
                    $('.uploadbar').css('margin-top', '60px');

                    setTimeout(function () {
                        $('.progressbar').removeClass('hide');

                        setTimeout(function () {
                            $('.progressbar .bar').css('width', '100%');
                        }, 200);
                    }, 1000);
                }
            },
            success: function (responseText) {
                $('.uploadbar').css('margin-top', '15px');
                $('.progress-striped').removeClass('progress-striped');

                // Call insertContent only if the upload was successful
                insertContent(responseText);
            }
        });

        //editor.windowManager.close();
        return false;
    });

});

var editor = top.tinymce.activeEditor, selection = editor.selection, dom = editor.dom;
function insertContent(content) {
    editor.execCommand('mceInsertContent', false, content);
    editor.windowManager.close();
}
function youtube_parser(url){
    var regExp = /^.*((youtu.be\/)|(v\/)|(\/u\/\w\/)|(embed\/)|(watch\?))\??v?=?([^#\&\?]*).*/;
    var match = url.match(regExp);
    return (match&&match[7].length==11)? match[7] : false;
}
