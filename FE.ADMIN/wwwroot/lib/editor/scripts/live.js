$(document).ready(function () {
  $("#btn_insert_content").click(function () {
    var content = $("textarea[name=info]").val(),
        hours = $("input[name=hours]").val(),
        min = $("input[name=min]").val(),
      border = $("#border").val(),
      time = hours + ':' + min;
      
    return (
      $(".live-inner").css("border-color", border),
      $(".time").html(time),
      $(".content").html(content),
      
      top.tinymce.activeEditor.execCommand(
        "mceInsertContent",
        !1,
        $("#js_wrap_live").html()
      ),
      top.tinymce.activeEditor.windowManager.close(),
      !1
    );
  });
});
