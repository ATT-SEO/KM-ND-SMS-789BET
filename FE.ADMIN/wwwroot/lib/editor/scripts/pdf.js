var editor = top.tinymce.activeEditor, selection = editor.selection, dom = editor.dom;
var selectedElm = selection.getNode();
//tinyMCE.activeEditor.dom.addClass('someid', 'someclass');
//var anchorElm = dom.getParent(selectedElm, 'div.expEdit');
$(document).ready(function(){
    
    var url = $('#my-awesome-dropzone').attr('action');
    Dropzone.options.myAwesomeDropzone = {
        paramName: "file",
        maxFilesize: 20,
        uploadMultiple: false
    };
    var myDropzone = new Dropzone("#my-awesome-dropzone", { url: url});
    var total_file = 0; 
    var success_file = 0;

    // myDropzone.on("sending", function(file, xhr, formData) {
    //     var fn = encodeURI(file.name)
    //     var fn1 = slugify(file.name);
        
    //     formData.append("encFilename", fn1);
    // });
    myDropzone.on("addedfile", function(file) {total_file++;});
    myDropzone.on("success", function(file, response) {
        success_file++;
        if(success_file==total_file) {
            if(success_file==1) {
                editor.execCommand('mceInsertContent', false, response);
                editor.undoManager.add();
                editor.save();
                editor.windowManager.close();
            }
            else {
                window.location.href = '/tinymce/pdf?news_id='+news_id;
            }
            
        }
    });
    
    
    $.fn.editable.defaults.inputclass = 'm-wrap';
    $.fn.editable.defaults.url = '/tinymce/image';
    $.fn.editable.defaults.name = 'updateTitle';
    $.fn.editable.defaults.emptytext = 'Empty';
    $.fn.editableform.buttons = '<button type="submit" class="btn blue editable-submit"><i class="icon-ok"></i></button>';
    $.fn.editableform.buttons += '<button type="button" class="btn editable-cancel"><i class="icon-remove"></i></button>';
    $('.editable').editable();
    
    $('.tab_btn .btn').click(function(){
        var id = $(this).attr('href');
        if(id=='#tab-1' && $(this).hasClass('red')) $('#my-awesome-dropzone').click();
        $('.tab_btn .btn.red').removeClass('red').addClass('blue');
        $(this).addClass('red').removeClass('blue');
        $('.tab_content').addClass('hide');
        $(id).removeClass('hide');
        
        if(id=='#tab-2' || id=='#tab-3') {
            if($('.dz-image-preview').length>0) {
                var is_done = true;
                $('.dz-image-preview').each(function(){if(!$(this).hasClass('dz-success') && !$(this).hasClass('dz-error')) is_done=false;});
                if(is_done) {window.location.href = '/tinymce/image?news_id='+news_id+id.replace('#tab-', '&tab='); return true;}
            }
        }
        
        return false;
    });
    
    $('.btn_Insert').click(function(){
        if(!$(this).parents('.meet-our-team').hasClass('s0')) {$(this).parents('.meet-our-team').find('svg:eq(0)').click(); return false;}
        var src = $(this).attr('href');
        var caption = $(this).parent().find('.editable').text();
        var width = $(this).attr('data-width');
        var height = $(this).attr('data-height');
        var _width = width; var _height = height;
        if(width>max_width) {
            _width = max_width;
            _height = parseInt(height/(width/max_width));
        }
        var img = '<img src="'+src+'" width="'+_width+'" height="'+_height+'" alt="'+caption+'" />';
        var content = '';
        
        if(caption && caption!='Empty') img += '<figcaption><p>'+caption+'</p></figcaption>';
        content = '<figure class="expNoEdit">'+img+'</figure>';
        
        editor.execCommand('mceInsertContent', false, content);
        editor.undoManager.add();
        editor.save();        
        
        editor.windowManager.close();
        return false;
    });
    
    var array_image = [];
    $('body.image .meet-our-team svg').click(function(){
        var obj = $(this).parents('.meet-our-team');
        var id = obj.attr('id').replace('oneImage_', '');
        if(obj.hasClass('active')) {
            obj.removeClass('active');
            array_image = jQuery.grep(array_image, function(value) {return value != id;});
            if($('body.image').find('.meet-our-team.active').size()>0) obj.addClass('s1');
            else {
                $('body.image .meet-our-team').removeClass('active').removeClass('s1').addClass('s0');
                array_image = [];
                $('#btn_insert_content').hide();
            }
        }
        else {
            if($('body.image').find('.meet-our-team.active').size()==0) {
                $('body.image .meet-our-team').removeClass('s0').addClass('s1');
                $('#btn_insert_content').show();
            }
            obj.removeClass('s1').addClass('active');
            array_image.push(id);
        }
        return false;
    });
    
    $('#btn_insert_content').click(function(){
        var content = '';
        $.each(array_image, function( key, value ) {
            var e = $('#oneImage_'+value).find('.btn_Insert');
            var src = e.attr('href');
            var caption = e.parent().find('.editable').text();
            var width = e.attr('data-width');
            var height = e.attr('data-height');
            var _width = width; var _height = height;
            if(width>max_width) {
                _width = max_width;
                _height = parseInt(height/(width/max_width));
            }
            var img = '<img src="'+src+'" width="'+_width+'" height="'+_height+'" alt="'+caption+'" />';
            if(caption && caption!='Empty') img += '<figcaption><p>'+caption+'</p></figcaption>';
            content += '<figure class="expNoEdit">'+img+'</figure>';
        });
        editor.execCommand('mceInsertContent', false, content);
        editor.undoManager.add();
        editor.save();        
        
        editor.windowManager.close();
        return false;
    });
});

function slugify (text) {
    const a = 'àáäâãèéëêìíïîòóöôùúüûñçßÿœæŕśńṕẃǵǹḿǘẍźḧ·/_,:;'
    const b = 'aaaaaeeeeiiiioooouuuuncsyoarsnpwgnmuxzh------'
    const p = new RegExp(a.split('').join('|'), 'g')
  
    return text.toString().toLowerCase()
      .replace(/\s+/g, '-')           // Replace spaces with -
      .replace(p, c =>
          b.charAt(a.indexOf(c)))     // Replace special chars
      .replace(/&/g, '-and-')         // Replace & with 'and'
      .replace(/[^\w\-]+/g, '')       // Remove all non-word chars
      .replace(/\-\-+/g, '-')         // Replace multiple - with single -
      .replace(/^-+/, '')             // Trim - from start of text
      .replace(/-+$/, '')             // Trim - from end of text
  }