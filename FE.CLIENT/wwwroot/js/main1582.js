var superPhone = "";

var clipboard = new ClipboardJS('.button-copy-content');
clipboard.on('success', function (e) {
    e.clearSelection();
    var showsText = e.trigger.querySelector('.shows-text');
    showsText.style.display = 'inline';
    setTimeout(function () {
        showsText.style.display = 'none';
    }, 3000);
});

function showLoadingSpinner() {
    document.getElementById("loadingOverlay").style.display = "flex";
}

function hideLoadingSpinner() {
    document.getElementById("loadingOverlay").style.display = "none";
}
$(document).on('click', '#btnCheck', function (e) {
    var account = $("#account").val()
    if (account == "") {
        ShowErrorMsg("Tài khoản không được để trống") //account not null
        return;
    }
    $('#btnCheck').attr('disabled', true);
    var data = {
        "Account": account,
        "Regfingerprint": $("#regfingerprint").val(),
        "RecaptchaToken": ""
    };
    showLoadingSpinner();
    grecaptcha.ready(function () {
        grecaptcha.execute('6Lfrm6MpAAAAAPPoX7zioJPgJqCgqObe1Usqfgko', {
            action: 'submit_form'
        }).then(function (token) {
            data.RecaptchaToken = token;
            $.ajax("/Account/CheckAccount", {
                type: "POST",
                data: JSON.stringify(data),
                //dataType: 'json',
                contentType: "application/json",
                success: function (response) {
                    $('#btnCheck').attr('disabled', false);
                    hideLoadingSpinner();
                    if (response?.isSuccess ) {
                        console.log(response);
                        if (response.code == 200) {
                            if (response.isSuccess == true) {
                                document.body.style.overflow = 'hidden';
                                $('#message-success').html(response.message.replace(/\n/g, '<br>'));
                                window.confettiful = new Confettiful(document.querySelector(".js-container"));
                                return;
                            }
                        }
                        if (response.code == 202) {
                            ShowSuccessMsg(response.message, "Kiểm tra thành công");
                            return;
                        }
                        if (response.code == 250) {
                            ShowErrorMsg(response.message);
                            return;
                        }
                    } else if (response?.isSuccess == false) {
                        ShowErrorMsg(response.message); // error
                        return;
                    }else {
                        $("#step-1").removeClass('pt-page-current');
                        $("#step-2").html(response);
                        $("#step-2").addClass('pt-page-current pt-page-moveFromTop');
                    }
                },
                error: function (error) {
                    hideLoadingSpinner();
                    $('#btnCheck').attr('disabled', false);
                    ShowErrorMsg("Yêu cầu bất thường"); // error
                }
            });
        });
    })
});
$(document).on('click', '#wrap-form-send-sms', function (e) {
    superPhone = $("#contentPhone").val();
    if (/(iPhone|iPad|iPod|iOS)/i.test(navigator.userAgent)) {
        window.location = "sms://" + superPhone;
    } else {
        window.location = 'sms:' + superPhone + '?body=' + $("#content").val();
    }
    //const phone = document.getElementById('oldphone').value;
    //const regex = /^\+?[0-9]{3}-?[0-9]{7,11}$/i;
    //if (!regex.test(phone)) {
    //    swal({
    //        title: "Thất bại",
    //        text: `Vui lòng nhập đúng số điện thoại`,
    //        icon: "error",
    //    });

    //} else {
    //    if (/(iPhone|iPad|iPod|iOS)/i.test(navigator.userAgent)) {
    //        window.location = "sms://" + superPhone;
    //    } else {
    //        window.location = 'sms:' + superPhone + '?body=' + $("#content").val();
    //    }
    //}
});
$(document).on('click', '#wrap-form-sended-sms', function (e) {
    var account = $("#account").val();
    var smscode = $("#content").val();
    var verifycode = $("#verifycode").val()
    if (account == "") {
        ShowErrorMsg("Tài khoản không được để trống") //account not null
        return;
    }
    $("#wrap-form-sended-sms").attr('disabled', true);
    var data = {
        "Account": account,
        "SmsCode": smscode,
        "VerifyCode": verifycode,
        "Regfingerprint": $("#regfingerprint").val()
    }
    showLoadingSpinner();
    var countdown = 30;

    var countdownInterval = setInterval(function () {
        if (countdown <= 0) {
            clearInterval(countdownInterval);
            $.ajax("/Account/SubmitBouns", {
                type: "POST",
                data: JSON.stringify(data),
                dataType: 'json',
                contentType: "application/json",
                success: function (response) {
                    $('#wrap-form-sended-sms').attr('disabled', false);
                    hideLoadingSpinner();
                    console.log(response);
                    if (response.success) {
                        if (response.result.code == 200) {
                            if (response.result.status == 2) {
                                document.body.style.overflow = 'hidden';
                                $('#message-success').html(response.result.message.replace(/\n/g, '<br>'));
                                window.confettiful = new Confettiful(document.querySelector(".js-container"));
                            } else {
                                ShowSuccessMsg(response.result.message);
                            }
                        } else {
                            ShowErrorMsg(response.result.message);
                        }
                        $("#back-homeme").css('display', 'inline');
                    }
                },
                error: function (error) {
                    hideLoadingSpinner();
                    $('#wrap-form-sended-sms').attr('disabled', false);
                    ShowErrorMsg("Yêu cầu bất thường"); //error
                }
            });
        } else {
            $('#text-time-count').html("Hệ thống đang xác nhận. Vui lòng chờ trong " + countdown + " giây...");
        }
        countdown--;
    }, 1000);
});
$(document).on('click', '#back-homeme', function (e) {
    $("#step-2").removeClass('pt-page-current pt-page-moveFromTop');
    $("#step-2").html();
    $("#step-1").addClass('pt-page-current');
});

$(document).on('click', '#wrap-from-instruct', function (e) {
    window.location.href = "/";
});

function ShowErrorMsg(errormsg) {
    swal({
        title: "Thất bại",
        text: errormsg,
        icon: "error",
    });
};

function ShowSuccessMsg(successMsg, title = "Thành Công") {
    swal({
        title: title,
        text: successMsg,
        icon: "success",
    });
};

function Confettiful(el) {
    $("#show-congratulations").show();
    this.el = el;
    this.containerEl = null;
    this.confettiFrequency = 3;
    this.confettiColors = ["#EF2964", "#00C09D", "#2D87B0", "#48485E", "#EFFF1D"];
    this.confettiAnimations = ["slow", "medium", "fast"];
    this._setupElements();
    this._renderConfetti();
}
Confettiful.prototype._setupElements = function () {
    const containerEl = document.createElement("div");
    const elPosition = this.el.style.position;

    if (elPosition !== "relative" && elPosition !== "absolute") {
        this.el.style.position = "relative";
    }

    containerEl.classList.add("confetti-container");
    this.el.appendChild(containerEl);
    this.containerEl = containerEl;
};

Confettiful.prototype._renderConfetti = function () {
    this.confettiInterval = setInterval(() => {
        const confettiEl = document.createElement("div");
        const confettiSize = Math.floor(Math.random() * 3) + 7 + "px";
        const confettiBackground = this.confettiColors[Math.floor(Math.random() * this.confettiColors.length)];
        const confettiLeft = Math.floor(Math.random() * this.el.offsetWidth) + "px";
        const confettiAnimation = this.confettiAnimations[Math.floor(Math.random() * this.confettiAnimations.length)];

        confettiEl.classList.add("confetti", "confetti--animation-" + confettiAnimation);
        confettiEl.style.left = confettiLeft;
        confettiEl.style.width = confettiSize;
        confettiEl.style.height = confettiSize;
        confettiEl.style.backgroundColor = confettiBackground;

        confettiEl.removeTimeout = setTimeout(function () {
            confettiEl.parentNode.removeChild(confettiEl);
        }, 3000);

        this.containerEl.appendChild(confettiEl);
    }, 25);
};