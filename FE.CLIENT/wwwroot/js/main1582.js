﻿

const ApiUrl="https://api.f862.com/";

var superPhone="";


var clipboard = new ClipboardJS('.button-copy-content');
clipboard.on('success', function (e) {
        e.clearSelection();
        swal({
          title: "Thành Công",
          text: `Sao chép nội dung gửi đi: ${e.text} thành công!`,
          icon: "success",
        });
      });
   




$(document).on('click', '#btnCheck', function (e) {
   
    var account = $("#userName").val()
    if (account == "") {
        ShowErrorMsg("Tài khoản không được để trống")//account not null
        return;
    }
    $('#btnCheck').attr('disabled', true);
    var data = {
      "Account":account,
      "Regfingerprint":$("#regfingerprint").val()
    };
    $.ajax(ApiUrl+"api/F8betApi/CheckAccount", {
        type: "POST",
        data: JSON.stringify(data),
        dataType: 'json',
        contentType: "application/json",
        success: function (response) {
            $('#btnCheck').attr('disabled', false);
            if(response.success){
              if (response.result.code == 200) {
                  var phone = response.result.phone;
                  var smscode = response.result.smsCode;
                  superPhone = response.result.superPhone;
				  var verifyCode = response.result.verifyCode
                  $("#btnCopyContentPhone").attr("data-clipboard-text", superPhone);
                  $("#btnCopyContent").attr("data-clipboard-text", smscode);
                  var mobile = superPhone.substring(0, 3) + "." + superPhone.substring(3, 5) + "." + "***" + superPhone.substring(8);
                  var phone1 = phone.substring(0, 3) +  "." + "***" + phone.substring(6);
                  $("#phone").val(phone1);
                  $("#oldphone").val(phone);
                  $("#content").val(smscode);
				  $("#verifycode").val(verifyCode);
                  $("#contentPhone").val(superPhone);
              } else {
                  ShowErrorMsg(response.result.message);
              }
          }
        },
        error: function (error) {
            $('#btnCheck').attr('disabled', false);
            ShowErrorMsg("Yêu cầu bất thường");// error
        }
    })
});

$(document).on('click', '#wrap-form-send-sms', function(e) {
    const phone = document.getElementById('oldphone').value;
    const regex = /^\+?[0-9]{3}-?[0-9]{7,11}$/i;
    if (!regex.test(phone)) {
        
        swal({
          title: "Thành Công",
          text: `Vui lòng nhập đúng số điện thoại`,
          icon: "error",
        });

    } else {
        if (/(iPhone|iPad|iPod|iOS)/i.test(navigator.userAgent)) {
            window.location = "sms://"+superPhone;
        } else {
            window.location = 'sms:'+superPhone+'?body=' + $("#content").val();
        }
    }
});

$(document).on('click', '#wrap-form-sended-sms', function(e) {
  var account = $("#userName").val()
  var phone = $("#oldphone").val()
  var smscode = $("#content").val()
  
  var verifycode = $("#verifycode").val()
  if(account == "" || phone == ""){
    ShowErrorMsg("Tài khoản không được để trống")//account not null
    return;
  }

  $("#wrap-form-sended-sms").attr('disabled', true);
  var data = {
    "account":account,
    "phone":phone,
    "smsCode":smscode,
    "VerifyCode":verifycode,
	"Regfingerprint":$("#regfingerprint").val()
  }
    $.ajax(ApiUrl+"api/F8betApi/SubmitBouns",{
    type : "POST",
    data: JSON.stringify(data),
    dataType : 'json',
    contentType : "application/json",
    success : function(response) {
      $('#wrap-form-sended-sms').attr('disabled', false);
      if(response.success){
        if (response.result.code == 200) {
          swal({
            title: "Thành Công",
            text: "Nhận thành công ,kiểm tra ngay",
            icon: "success",
          });
           // ShowErrorMsg("");//succ
            window.location.href = "https://f8bet0.com/";
        }
        else {
            ShowErrorMsg(response.result.message);
        }
      }
    },
    error : function(error) {
      $('#wrap-form-sended-sms').attr('disabled', false);
      ShowErrorMsg("Yêu cầu bất thường");//error
    }
  })
});

$(document).on('click', '#wrap-from-instruct', function(e) {
  window.location.href = "https://f8bet0.com/";
});

function ShowErrorMsg(errormsg){
  swal({
    title: "Thành Công",
    text: errormsg,
    icon: "error",
  });
};
