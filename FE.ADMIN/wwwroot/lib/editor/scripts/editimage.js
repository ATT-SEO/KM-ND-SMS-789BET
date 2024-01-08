var ed = top.tinymce.activeEditor, dom = ed.dom, n = ed.selection.getNode();
if(n.nodeName == 'FIGURE' && $(n).hasClass('expNoEdit')) {
    var src = $(n).find('img').attr('src');
    var caption = $(n).find('figcaption').text();
    var src_mb = $(n).find('img.img_is_mb').attr('src');
}
else {
    alert('Không tìm thấy hình ảnh');
    ed.windowManager.close();
}
$(document).ready(function(){



    //var new_width = $(n).find('img').attr('width');
    //var new_height = $(n).find('img').attr('height');

    var new_image = src, jcrop_api;
    var  class_pc =img_src_mb=img_mb= '';
    var crop_x, crop_y, crop_w, crop_h, pre_w, pre_h;
    $('#img_preview').attr('src', src);
    $('#img_mobile').attr('src', src_mb);
    $('#caption').val(caption);

    var myImg = document.querySelector("#img_preview");
    console.log(myImg);
    var new_width = myImg.naturalWidth;
    var new_height = myImg.naturalHeight;
    console.log(new_width , new_height);

    if($(n).hasClass('right')) $('.nav_radio input[value=right]').prop( "checked", true );
    else if($(n).hasClass('left')) $('.nav_radio input[value=left]').prop( "checked", true );
    else if($(n).hasClass('full')) $('.nav_radio input[value=full]').prop( "checked", true );
    else if($(n).hasClass('center')) $('.nav_radio input[value=center]').prop( "checked", true );
    else if($(n).hasClass('large')) $('.nav_radio input[value=large]').prop( "checked", true );
    else $('.nav_radio input[value=none]').prop( "checked", true );

    if(src_mb != undefined){
        $('.checkmb').show();
        $('#check-img-mobile').prop( "checked", true );
    }

    var old_class = $('.nav_radio input[name=align]:checked').val();
    $('#resize_w').val(new_width);
    $('#resize_h').val(new_height);

    $('#btn_insert_content').click(function(){
        /*$(n).removeClass('expNoEdit');
        $(n).find('img').attr('src', new_image);
        $(n).find('img').attr('width', new_width);
        $(n).find('img').attr('height', new_height);
        var new_class = $('.nav_radio input[name=align]:checked').val();
        if(old_class!=new_class) {
            if(old_class!='' && old_class!='none') $(n).removeClass(old_class);
            if(new_class!='' && new_class!='none') $(n).addClass(new_class);
        }
        $(n).addClass('expNoEdit');*/

        if ($('#check-img-mobile').is(":checked"))
        {
            img_src_mb = $('#img_mobile').attr('src');
            img_mb = '<img src="'+img_src_mb+'" class="img_is_mb" alt="'+caption+'" width="100%" height="auto" />';
            class_pc = "img_is_pc";
        }

        var new_class = $('.nav_radio input[name=align]:checked').val();
        if(new_class=='none') new_class = '';
        caption = $('#caption').val();
        var _caption = '';
        if(caption) _caption = '<figcaption class="expEdit">'+caption+'</figcaption>';
        ed.execCommand('mceInsertContent', false, '<figure class="expNoEdit '+new_class+'"><img src="'+new_image+'" class="'+class_pc+'" alt="'+caption+'" width="'+new_width+'" height="'+new_height+'" />'+img_mb+''+_caption+'</figure>');
        ed.undoManager.add();
        ed.save();
        ed.windowManager.close();
        return false;
    });
    $('#btn_cancel').click(function(){
        ed.windowManager.close();
        return false;
    });

    $('#btn_resize').click(function(){
        $('.tools .button-group').hide();
        $('.tools .resize').css('display', 'inline-block').find('input:eq(0)').focus();
    });

    var resize_tyle = $('#resize_h').val()/$('#resize_w').val();
    // $('#resize_w').keyup(function(){
    //     var val = ($(this).val())*(resize_tyle);
    //     $('#resize_h').val(parseInt(val));
    // });
    // $('#resize_h').keyup(function(){
    //     var val = ($(this).val())/(resize_tyle);
    //     $('#resize_w').val(parseInt(val));
    // });

    $('#btn_crop').click(function(){
        $('.tools .button-group').hide();
        $('.tools .crop').css('display', 'inline-block');
        $('#img_preview').Jcrop({
          onChange:   showCoords,
          onSelect:   showCoords,
          onRelease:  clearCoords
        },function(){
            jcrop_api = this;
        });

    });
    $('#btn_paint').click(function(){
        $('.tools .button-group').hide();
        $('.tools .paint').css('display', 'inline-block');
        $.ajax({
    		type: "POST",
    		url: "/admin/editor/editimage",
    		data:  {action: 'paint', image: $('#img_preview').attr('src')},
    		dataType: "html",
    		success: function(msg){
    			if(parseInt(msg)!=0) {$('#img_preview').attr('src', msg); new_image = msg;}
                else alert('ERR');
                $('.tools .button-group').show();
                $('.tools .paint').hide();
    		},
            error: function(request,error) {
                console.log(arguments);
                alert ( " Can't do because: " + error );
                $('.tools .button-group').show();
                $('.tools .paint').hide();
            }
    	});
    });
    $('.tools .resize .btn_ok').click(function(){
        $('.tools .button-group').show();
        $('.tools .resize').hide();
        new_width = $('#resize_w').val();
        new_height = $('#resize_h').val();
        src = $('#img_preview').attr('src').replace(/\/resize\/([0-9]*)x([0-9]*)\//gm, "/").replace('/uploads/', '/resize/'+new_width+'x'+new_height+'/uploads/');
        $('#img_preview').attr('src', src);
        new_image = src;
    });

    $('.tools .crop .btn_ok').click(function(){
        $('.tools .button-group').show();
        $('.tools .crop').hide();
        jcrop_api.destroy();
        pre_w = $('#img_preview').width();
        pre_h = $('#img_preview').height();

        crop_x = parseInt($('#crop_x').val()*new_width/pre_w);
        crop_y = parseInt($('#crop_y').val()*new_height/pre_h);
        crop_w = parseInt($('#crop_w').val()*new_width/pre_w);
        crop_h = parseInt($('#crop_h').val()*new_height/pre_h);
        new_width = crop_w; new_height = crop_h;
        $('#resize_w').val(new_width);
        $('#resize_h').val(new_height);
        var csrfToken = $('meta[name="csrf-token"]').attr("content");
        $.ajax({
    		type: "POST",
    		url: "/admin/editor/editimage",
    		data:  {_csrf_backend: csrfToken,action: 'crop',x: crop_x, y: crop_y, w: crop_w, h: crop_h, image: $('#img_preview').attr('src')},
    		dataType: "html",
    		success: function(msg){
    			if(parseInt(msg)!=0) {$('#img_preview').attr('src', msg);new_image=msg;}
                else alert('ERR');
    		},
            error: function(request,error) {
                console.log(arguments);
                alert ( " Can't do because: " + error );
            }
    	});
    });

    $('.tools .resize .btn_cancel').click(function(){
        $('.tools .button-group').show();
        $('.tools .resize').hide();
    });
    $('.tools .crop .btn_cancel').click(function(){
        $('.tools .button-group').show();
        $('.tools .crop').hide();
        jcrop_api.destroy();
    });
    function showCoords(c) {
        $('#crop_x').val(c.x);
        $('#crop_y').val(c.y);
        $('#crop_w').val(c.w);
        $('#crop_h').val(c.h);
    }
    function clearCoords() {
        $('#coords input').val('');
    }


    $('#img-mobille').change(function() {
        if(this.checked) {
          alert(1);
        }else{
            alert(3);

        }
    });
});
