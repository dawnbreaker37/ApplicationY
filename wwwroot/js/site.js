let botNavbarH = 0;
let fullWidth = 0;
let fullHeigth = 0;
let lastContainerName = undefined;
let currentContainerName = null;

window.onload = function () {
    botNavbarH = $("#MainBotOffNavbar").innerHeight();
    fullWidth = window.innerWidth;
    fullHeigth = window.innerHeight;
    displayCorrect(fullWidth);
    animatedOpen(false, "Preload_Container", true);
    navBarBtnSelector(document.location.href);
}
window.onresize = function () {
    botNavbarH = $("#MainBotOffNavbar").innerHeight();
    fullWidth = window.innerWidth;
    fullHeigth = window.innerHeight;
    displayCorrect(fullWidth);
}

$("#UserName").on("change", function (event) {
    event.preventDefault();
    $("#IUNU_Val").val($(this).val());
    let url = $("#IsUserNameUnique_Form").attr("action");
    let data = $("#IsUserNameUnique_Form").serialize();

    $.get(url, data, function (response) {
        if (response.success) {
            $("#SignInSbmt_Btn").attr("disabled", false);
            $("#IUNU_Badge").removeClass("text-danger");
            $("#IUNU_Badge").addClass("text-primary");
            $("#IUNU_Badge").html(" <i class='fas fa-check-double'></i>");
        }
        else {
            $("#SignInSbmt_Btn").attr("disabled", true);
            $("#IUNU_Badge").removeClass("text-primary");
            $("#IUNU_Badge").addClass("text-danger");
            $("#IUNU_Badge").html(" <i class='fas fa-times-circle'></i>");
        }
    });
});

$("#SignIn_Form").on("submit", function (event) {
    event.preventDefault();
    let url = $(this).attr("action");
    let data = $(this).serialize();
    $.post(url, data, function (response) {
        if (response.success) {
            $("#CodeText_Label").text(response.code);
            animatedOpen(false, "SignInComplete_Container", true);
            $("#Email").val("");
            $("#UserName").val("");
            $("#Password").val("");
            $("#PasswordConfirm").val("");
        }
        else {
            openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null,  3.5);
        }
    });
});
$("#LogIn_Form").on("submit", function (event) {
    event.preventDefault();
    let url = $(this).attr("action");
    let data = $(this).serialize();

    $.post(url, data, function (response) {
        if (response.success) {
            $("#LogIn_Password").attr("readonly", true);
            $("#LogIn_UserName").attr("readonly", true);
            $("#LogInSbmt_Btn").attr("disabled", true);
            openModal(response.alert, "<i class='fas fa-home'></i> Relocate To Home Page", null, 0, "/Home/Index", null, null, 3.75);
        }
        else {
            $("#LogIn_Password").val("");
            openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 4.25);
        }
    });
});
$("#SendCode_Form").on("submit", function (event) {
    event.preventDefault();
    let url = $(this).attr("action");
    let data = $(this).serialize();

    $.post(url, data, function (response) {
        if (response.success) {
            $("#PR_Username").val(response.email);
            openModal(response.text, null, null, null, null, null, null, 3.75);
            setTimeout(function () {
                $("#ChangeRecoveringType_Btn").click();
            }, 350);
        }
        else {
            openModal(response.text, null, null, null, null, null, null, 3.75);
        }
    });
});
$("#PasswordRecovering_Form").on("submit", function (event) {
    event.preventDefault();
    let url = $(this).attr("action");
    let data = $(this).serialize();

    $.post(url, data, function (response) {
        if (response.success) {
            $("#UP_Token").val(response.token);
            $("#UP_ReserveCode").val(response.reserveCode);
            $("#TypeOfSearch_Val").val($("#SearchByUsername_Val").val());
            $("#CN_EmailOrUsername").val($("#PR_Username").val());
            animatedOpen(false, "CreateNewPassword_Container", false, false);
        }
        else {
            animatedClose(false, "PasswordRecovering_Container");
            openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 4);
        }
    });
});
$("#UpdatePassword_Form").on("submit", function (event) {
    event.preventDefault();
    let url = $(this).attr("action");
    let data = $(this).serialize();

    $.post(url, data, function (response) {
        if (response.success) {
            openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 4);
            animatedClose(true, "smallside-box-container");
        }
        else {
            openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 4);
            $("#NewPassword_Confirm").val("");
        }
    });
});

$("#VerifyAccount_Form").on("submit", function (event) {
    event.preventDefault();
    let url = $(this).attr("action");
    let data = $(this).serialize();

    $.post(url, data, function (response) {
        if (response.success) {
            openModal(response.alert, " <i class='fas fa-close'></i> Close", "About Verified Accounts Features", 2, null, 1, "AboutVerification_Container", 4);
            animatedClose(false, "Verification_Container");
        }
        else {
            $("#VAF_Code").val("");
            $("#VAF_Id").val("");
            $("#VAF_Email").val("");
            openModal(response.alert, " <i class='fas fa-close'></i> Close", null, 2, null, null, null, 4);
            animatedOpen(false, "Preload_Container", true, true);
        }
    });
});
$("#SendEmailConfirmationCode_Form").on("submit", function (event) {
    event.preventDefault();
    let url = $(this).attr("action");
    let data = $(this).serialize();

    $.post(url, data, function (response) {
        if (response.success) {
            openModal(response.alert, " <i class='fas fa-times'></i> Close", null, 2, null, null, null, 4);
            setTimeout(function () {
                $("#VAF_Token").val(response.result);
                $("#VAF_Id").val(response.id);
                $("#VAF_Email").val(response.email);
                animatedOpen(false, "Verification_Container", true, true);
            }, 3500);
        }
        else {
            $("#VAF_Token").val("");
            $("#VAF_Id").val("");
            $("#VAF_Email").val("");
            openModal(response.alert, " <i class='fas fa-times'></i> Close", null, 2, null, null, null, 4);
        }
    });
});

$("#ChangeRecoveringType_Btn").on("click", function () {
    let currentTypeName = $("#PR_CurrentTypeVal").val();
    animatedClose(false, "PasswordRecovering_Container");
    if (currentTypeName == 0) {
        $("#PR_Username").attr("placeholder", "Enter your email here");
        $("#PR_Username_Lbl").text("Email");
        $("#PR_CurrentTypeVal").val(1);
        $("#PR_ByUsername").val(false);
        $(this).text("Continue With Username");
        //smallSideModal("You've changed type of account search from username by email", 6);
    }
    else {
        $("#PR_CurrentTypeVal").val(0);
        $("#PR_Username").attr("placeholder", "Enter your username here");
        $("#PR_Username_Lbl").text("Username");
        $("#PR_ByUsername").val(true);
        $(this).text("Continue With Email");
        //smallSideModal("You've changed type of account search from email by username", 6);
    }
/*    setTimeout(function () {*/
        animatedOpen(false, "PasswordRecovering_Container", false, false);
/*    }, 8000);*/
});

$(document).on("click", ".btn-back", function (event) {
    openLastContainer(lastContainerName);
});
$(document).on("click", ".btn-close", function (event) {
    let container = getTrueName(event.target.id);
    animatedClose(false, container);
});
$(document).on("click", ".btn-open-container", function (event) {
    let container = getTrueName(event.target.id);
    animatedOpen(false, container, true, true);
});
$(document).on("click", ".smallside-btn-open-container", function (event) {
    let container = getTrueName(event.target.id);
    smallBarAnimatedOpenAndClose(true);
    animatedOpen(false, container, false, false);
});
$(document).on("click", ".smallside-btn-close", function () {
    smallBarAnimatedOpenAndClose(false);
});
$(document).on("mouseover", ".smallside-modal-open-by-hover", function (event) {
    let alert = $("#" + event.target.id).attr("data-content");
    if (alert != undefined || alert != null) {
        if (fullWidth >= 717) openModal(alert, " <i class='fas fa-times'></i> Close", null, 2, null, null, null, 7.5);
        else openModal(alert, " <i class='fas fa-times'></i> Close", null, 2, null, null, null, 7.5);
    } 
});

function getTrueName(fullName) {
    let separatorIndex = fullName.lastIndexOf("-");
    let substringedName = fullName.substring(0, separatorIndex);

    return substringedName;
}

function openLastContainer(element) {
    if (element == undefined) {
        return false;
    }
    else if (element == null) {
        animatedOpen(false, "Preload_Container", true);
    }
    else {
        animatedOpen(false, element, true);
    }
}

function openModal(text, btn1Txt, btn2Txt, btn1WhatToDo, btn1Action, btn2WhatToDo, btn2Action, fadeOutTimer) {
    $("#MMC_Text").html(text);
    $("#MainModal_Container").fadeIn(0);
    $("#MainModal_Container").css("z-index", "1000");
    $("#MainModal_Container").css("bottom", "24px");
    setTimeout(function () {
        $("#MainModal_Container").css("bottom", "8px");
    }, 500);
    setTimeout(function () {
        $("#MainModal_Container").css("bottom", "-1200px");
        $("#MainModal_Container").fadeOut(300);
        $("#MainModal_Container").css("z-index", 0);
    }, (fadeOutTimer + 0.45) * 1000);

    if (btn1Txt == null) $("#MMC_FirstBtn").html("Close");
    else $("#MMC_FirstBtn").html(btn1Txt);
    if (btn2Txt == null) $("#MMC_SecondBtn").html(null);
    else $("#MMC_SecondBtn").html(btn2Txt);

    if (btn1WhatToDo != null) {
        $("#MMC_FirstBtn").fadeIn(100);
        switch (btn1WhatToDo) {
            case 0:
                $("#MMC_FirstBtn").on("click", function () {
                    document.location.href = btn1Action;
                });
                break;
            case 1:
                $("#MMC_FirstBtn").on("click", function () {
                    animatedOpen(false, btn1Action, true);
                });
                break;
            case 2:
                $("#MMC_FirstBtn").on("click", function () {
                    animatedClose(false, "MainModal_Container");
                });
                break;
            default:
                $("#MMC_FirstBtn").on("click", function () {
                    document.location.href = btn1Action;
                });
                break;
        }
    }
    else $("#MMC_FirstBtn").fadeOut(100);

    if (btn2WhatToDo != null) {
        $("#MMC_SecondBtn").fadeIn(100);
        switch (btn1WhatToDo) {
            case 0:
                $("#MMC_SecondBtn").on("click", function () {
                    document.location.href = btn2Action;
                });
                break;
            case 1:
                $("#MMC_SecondBtn").on("click", function () {
                    animatedOpen(false, btn2Action, true);
                });
                break;
            case 2:
                $("#MMC_SecondBtn").on("click", function () {
                    animatedClose(false, "MainModal_Container");
                });
                break;
            default:
                $("#MMC_SecondBtn").on("click", function () {
                    document.location.href = btn2Action;
                });
                break;
        }
    }
    else {
        $("#MMC_SecondBtn").fadeOut(100);
    }
}

function navBarBtnSelector(href) {
    if (href.toLowerCase().includes("create")) {
        $("#SearchLink_Btn").fadeOut(0);
        $("#SecondReserve_Btn").fadeIn(0);
        $("#SecondReserve_Btn").addClass("btn-open-container");
        $("#SecondReserve_Btn").html(" <i class='fas fa-sign-in-alt'></i> <br/>Sign or Log In");
        $("#SecondReserve_Btn").attr("id", "Preload_Container-OpenY");
    }
    else if ((href.toLowerCase().includes("index") || href.toLowerCase()[href.length - 1] == "/") && (fullWidth < 717)) {
        $("#HomeLink_Btn").fadeOut(0);
        $("#FirstReserve_Btn").fadeIn(0);
        $("#FirstReserve_Btn").addClass("smallside-btn-open-container");
        $("#FirstReserve_Btn").html(" <i class='fas fa-bars'></i> <br/>Menu");
    }
    else if (href.toLowerCase().includes("profile") && fullWidth < 717) {
        $("#HomeLink_Btn").fadeOut(0);
        $("#FirstReserve_Btn").fadeIn(0);
        $("#FirstReserve_Btn").addClass("smallside-btn-open-container");
        $("#FirstReserve_Btn").html(" <i class='fas fa-bars'></i> <br/>Menu");
    }
}

function displayCorrect(width) {
    let neededH = fullHeigth - 24;
    $(".main-container").css("height", neededH + "px");
    $(".main-container").css("max-height", neededH + "px");
    $(".smallside-box-container").css("max-height", neededH + "px");

    if (width < 717) {
        $(".main-container").css("width", "100%");
        $(".main-container").css("left", 0);
        $(".smallside-box-container").css("width", "99%");
        $(".smallside-box-container").css("left", "2px");
        $(".smallside-modal-container").css("left", "2.4%");
        $(".smallside-modal-container").css("width", "95%");

        $("#MainBotOffNavbar").css("width", "100%");
        $("#MainBotOffNavbar").css("left", 0);
        $("#Main_SideBar").css("left", "-1200px");
        $("#Main_SideBar").css("width", "100%");
        $("#MainBotOffNavbar_Box").css("width", "100%");
        $("#Main_SideBar").fadeOut(0);
    }
    else {
        let leftBarW = $("#Main_SideBar").innerWidth() + 3;
        let leftW = fullWidth - leftBarW - 3;
        let smallSideContainerW = leftBarW - 5;

        $("#Main_SideBar").fadeIn(0);
        $("#Main_SideBar").css("left", 0);
        $(".main-container").css("width", leftW + "px");
        $(".main-container").css("left", leftBarW + "px");
        $(".smallside-box-container").css("width", smallSideContainerW + "px");
        $(".smallside-box-container").css("left", "2px");
        $(".smallside-modal-container").css("left", "10px");
        $(".smallside-modal-container").css("width", smallSideContainerW - 19 + "px");

        $("#MainBotOffNavbar").css("width", leftW + "px");
        $("#MainBotOffNavbar_Box").css("width", leftW + "px");
        $("#MainBotOffNavbar").css("left", leftBarW + "px");
    }
}

function smallBarAnimatedOpenAndClose(open) {
    if (fullWidth < 717) {
        if (open) {
            animatedClose(true, "main-container");
            setTimeout(function () {
                $("#Main_SideBar").fadeIn(100);
                $("#Main_SideBar").css("left", 0);
            }, 150);
        }
        else {
            $("#Main_SideBar").fadeOut(750);
            $("#Main_SideBar").css("left", "-1200px");
        }
    }
}

function animatedOpen(forAll, element, sticky, closeOtherContainers) {
    if (closeOtherContainers) animatedClose(true, "main-container");
    else animatedClose(true, "smallside-box-container");
    setTimeout(function () {
        if (forAll) {
            if (!closeOtherContainers) {
                if (currentContainerName != null) lastContainerName = currentContainerName;
                currentContainerName = null;
            }
            $("." + element).fadeIn(25);
            $("." + element).css("border-radius", "10px");
            if (sticky) {
                $("." + element).css("bottom", "20px");
                setTimeout(function () {
                    $("." + element).css("bottom", 0);
                    $("." + element).css("border-radius", "8px 8px 2px 2px");
                }, 550);
            }
            else {
                $("." + element).css("bottom", 0);
                $("." + element).css("border-radius", "8px 8px 2px 2px");
            }
        }
        else {
            if (!closeOtherContainers) {
                if (currentContainerName != null) lastContainerName = currentContainerName;
                currentContainerName = element;
            }
            $("#" + element).css("border-radius", "10px");
            $("#" + element).fadeIn(25);
            if (sticky) {
                $("#" + element).css("bottom", "20px");
                setTimeout(function () {
                    $("#" + element).css("bottom", 0);
                    $("#" + element).css("border-radius", "8px 8px 1px 1px");
                }, 550);
            }
            else {
                $("#" + element).css("bottom", 0);
                $("#" + element).css("border-radius", "8px 8px 1px 1px");
            }
        }
    }, 175);
}

function animatedClose(forAll, elemet) {
    if (forAll) {
        /*        $("." + elemet).css("bottom", "24px");*/
        $("." + elemet).css("bottom", "-1200px");
        $("." + elemet).fadeOut(250);
    }
    else {
        /*        $("#" + elemet).css("bottom", "24px");*/
        $("#" + elemet).css("bottom", "-1200px");
        $("#" + elemet).fadeOut(250);
    }
}