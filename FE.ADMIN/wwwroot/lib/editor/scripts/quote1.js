// $(document).ready(function(){(function(){if(jQuery().uniform){var a=$("input[type=checkbox]:not(.toggle), input[type=radio]:not(.toggle, .star)");0<a.size()&&a.each(function(){0==$(this).parents(".checker").size()&&($(this).show(),$(this).uniform())})}})();$(".nav_radio input[type=radio]").change(function(){var a=$(this).val();"left"==a?$(".quote-inner").css("float","right").css("margin","0 13px 0 0"):"right"==a&&$(".quote-inner").css("float","right").css("margin","0 0 0 13px")});$("#btn_insert_content").click(function(){var a=$("textarea[name=info]").val(),b=$("input[name=fullname]").val();$("#js_wrap_quote blockquote.quote").html(a);b?$("#js_wrap_quote p.expEdit").html(b):$("#js_wrap_quote p.expEdit").remove();top.tinymce.activeEditor.execCommand("mceInsertContent",!1,$("#js_wrap_quote").html());top.tinymce.activeEditor.windowManager.close();return!1})});

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
    
    $('.nav_radio input[type=radio]').change(function(){
        var val = $(this).val();
        if(val=='left'){ 
            $('.quote-inner').css('float', 'left').css('margin', '0 13px 0 0');
            $('.quote-inner-box').addClass('flleft');
            $('.quote-inner-box').css({
                'width': '35%',
            });
        }else if(val=='center'){
            $('.quote-inner').css('float', 'none').addClass('quote_center');
            $('.quote-inner-box').removeClass('flleft');$('.quote-inner-box').removeClass('flright');
            $('.quote-inner-box').css({
                'width': '100%',
            });
        }else if(val=='right'){
            $('.quote-inner').css('float', 'right').css('margin', '0 0 0 13px');
            $('.quote-inner-box').addClass('flright');
            $('.quote-inner-box').css({
                'width': '35%',
            });
        }
    });
    $('#btn_insert_content').click(function(){
        var info = $('textarea[name=info]').val();
        var fullname = $('input[name=fullname]').val();
        $('#js_wrap_quote blockquote.quote').html(info);
        if(fullname) $('#js_wrap_quote p.expEdit').html(fullname);
        else $('#js_wrap_quote p.expEdit').remove();
        
        top.tinymce.activeEditor.execCommand('mceInsertContent', false, $('#js_wrap_quote').html());
        top.tinymce.activeEditor.windowManager.close();
        return false;
    });
    ////dành cho box ngang -chèn box thông tin---
    $('.nav_radio_box input[name=type_border]').change(function(){
        var val = $(this).val();
        if(val=='border'){
            $('.quote-inner blockquote').css('background', 'none');
            $('.quote-inner').addClass('padding_top');
            $('.quote-inner').css({
                'border-left': '3px solid #0f3a97',
                'padding': '5px 0 0',
                'width': '100%',
            });
        }
        else if(val=='noboder'){
            $('.quote-inner blockquote').css('background', 'url(https://media.lyluanchinhtrivatruyenthong.vn/uploads/images/quote.png) no-repeat');
            $('.quote-inner blockquote').css('background-size', '13px');
            $('.quote-inner').removeClass('padding_top');
            $('.quote-inner').css('border-left', 'none');
        };
    });
});