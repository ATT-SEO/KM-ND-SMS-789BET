$(document).ready(function(){
    
    var handleUniform = function () {
        if (!jQuery().uniform) {
            return;
        }
        var test = $("input[type=checkbox]:not(.toggle), input[type=radio]:not(.toggle, .star)");
        if (test.size() > 0) {
            test.each(function () {
                if ($(this).parents(".checker").size() == 0) {
                    $(this).show();
                    $(this).uniform();
                }
            });
        }
    }
    handleUniform();
    
    $('#btn_insert_content').click(function(){
        
        var val = $('.nav_radio input[type=radio]:checked').val();
        var border = $('#border').val();
        var background = $('#background').val();

        if(val=='left') $('blockquote').css('float', 'left').css('margin', '0 13px 0 0');
        else if(val=='right') $('blockquote').css('float', 'right').css('margin', '0 0 0 13px');
        else $('blockquote').css('margin', '0px auto').css('padding', '8px 2%').css('width', '96%');

        $('blockquote').css('background', background).css('border-color',border);
        $('blockquote').addClass('expNoEdit');
       
	   var arrayOfLines = $('textarea[name=info]').val().split('\n');
       
	   var html = '';
	   html += '<div class="expEdit">'; 
        $.each(arrayOfLines, function(index, item) {
            html += '<p>'+item+'</p>';       
        });
        $('#js_wrap_quote blockquote').html(html);
          html += '</div>'; 
        top.tinymce.activeEditor.execCommand('mceInsertContent', false, $('#js_wrap_quote').html());
        top.tinymce.activeEditor.windowManager.close();
        return false;
    });
});