let botNavbarH = 0;
let fullWidth = 0;
let fullHeigth = 0;
let lastContainerName = undefined;
let currentContainerName = null;

window.onload = function () {
    let currentUrl = document.location.href;
    botNavbarH = $("#MainBotOffNavbar").innerHeight();
    fullWidth = window.innerWidth;
    fullHeigth = window.innerHeight;
    displayCorrect(fullWidth);
    animatedOpen(false, "Preload_Container", true);
    navBarBtnSelector(document.location.href);

    if (currentUrl.toLowerCase().includes("/profile")) {
        textDecoder("AY_Description", true, true);
    }
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
$("#SearchName").on("change", function (event) {
    event.preventDefault();
    $("#ISNU_Searchname").val($(this).val());

    let url = $("#IsSearchnameUnique_Form").attr("action");
    let data = $("#IsSearchnameUnique_Form").serialize();

    $.get(url, data, function (response) {
        if (response.success) {
            $("#SearchName_Lbl").html(" <i class='fas fa-check-double text-primary'></i> Searchname");
            $("#EditFormSbmt_Btn").attr("disabled", false);
            $("#EA_SearchnameLbl").text("Is used for searching and relocating to your page");
        }
        else {
            $("#SearchName_Lbl").html(" <i class='fas fa-times-circle text-danger'></i> Searchname");
            $("#EditFormSbmt_Btn").prop("disabled", true);
            $("#EA_SearchnameLbl").html(" <i class='fas fa-times-circle text-danger'></i> " + response.alert);
            setTimeout(function () {
                $("#EA_SearchnameLbl").text("Is used for searching and relocating to your page");
            }, 5000);
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

$("#ChangePassword_First_Form").on("submit", function (event) {
    event.preventDefault();

    let url = $(this).attr("action");
    let data = $(this).serialize();
    $.post(url, data, function (response) {
        if (response.success) {
            $("#PCS_UserId").val(response.userId);
            $("#PCS_CurrentPassword").val(response.currentPassword);
            $("#PCS_NewPassword").val(response.newPassword);
            animatedClose(false, "PasswordChange_Container");
            openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.25);
            setTimeout(function () {
                animatedOpen(false, "PasswordChangeSubmit_Container", true, false);
            }, 3550);
        }
        else {
            $("#CurrentPassword").val("");
            animatedClose(false, "PasswordChange_Container");
            openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.75);
            setTimeout(function () {
                animatedOpen(false, "PasswordChange_Container", true, false);
            }, 4100);
        }
    });
});
$("#ChangePassword_Final_Form").on("submit", function (event) {
    event.preventDefault();

    let url = $(this).attr("action");
    let data = $(this).serialize();
    $.post(url, data, function (response) {
        if (response.success) {
            $("#PasswordChange_Container-Open").attr("disabled", true);
            $("#PasswordUpdateDate_Lbl").html(" <span class='text-dark'> <i class='fas fa-redo-alt'></i> Elapsed time from last update:</span> Few seconds ago")
            animatedClose(false, "PasswordChangeSubmit_Container");
            openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.25);
        }
        else {
            $("#CurrentPassword").val("");
            animatedClose(false, "PasswordChangeSubmit_Container");
            openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.25);
            setTimeout(function () {
                animatedOpen(false, "PasswordChangeSubmit_Container", true, false);
            }, 3550);
        }
    });
});

$("#ChangeEmailViaReserveCode_Form").on("submit", function (event) {
    event.preventDefault();
    let url = $(this).attr("action");
    let data = $(this).serialize();

    $.post(url, data, function (response) {
        if (response.success) {
            animatedClose(false, "EmailChange_Container");
            openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3);
            $("#AY_Email").html(' <i class="fas fa-inbox text-dark"></i> <span class="text-dark">Inbox:</span> ' + response.email);
            $("#EmailChange_Container-Open").fadeOut(300);
        }
        else {
            animatedClose(false, "EmailChange_Container");
            openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3);
            $("#EmailChange_Container-Open").fadeOut(300);
        }
    });
});

$("#EditMainInfo_Form").on("submit", function (event) {
    event.preventDefault();

    let url = $(this).attr("action");
    let data = $(this).serialize();
    $.post(url, data, function (response) {
        if (response.success) {
            let userName = $("#RealUserName").val();
            $("#RealSearchName").val(response.result.searchName);
            $("#SearchName").val(response.result.searchName);
            $("#PseudoName").val(response.result.pseudoName);           
            if (response.result.pseudoName != null) {
                $("#AYA_Pseudoname").html(" <i class='fas fa-user-circle text-primary'></i> " + response.result.pseudoName);
                $("#AYA_Searchname").html(" <i class='fas fa-at text-primary'></i> " + response.result.searchName);
                $("#AY_Username").text(response.result.pseudoName);
                $("#AY_SearchName").html(" <i class='fas fa-at text-dark'></i>, <i class='fas fa-hashtag text-dark'></i> <span class='text-dark'>Searchname, Username:</span> " + response.result.searchName + ", " + userName);
            }
            else {
                $("#AY_Username").text(response.result.userName);
                $("#AY_SearchName").html(" <i class='fas fa-at text-dark'></i> <span class='text-dark'>Searchname:</span> " + response.result.searchName);
            }

            animatedClose(false, "EditAccount_Container");
            openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.75);
        }
        else {
            openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.25);
        }
    });
});
$("#EditDescription_Form").on("submit", function (event) {
    event.preventDefault();
    let url = $(this).attr("action");
    let data = $(this).serialize();

    $.post(url, data, function (response) {
        if (response.success) {
            animatedClose(false, "EditDescription_Container");
            openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.25);
            $("#Description").val(response.description);
            $("#AY_Description").html(response.description);
            textDecoder("AY_Description", true, true);
        }
        else {
            animatedClose(false, "EditDescription_Container");
            openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.5);
        }
    });
});
$("#EditLinks_Form").on("submit", function (event) {
    event.preventDefault();
    let url = $(this).attr("action");
    let data = $(this).serialize();

    $.post(url, data, function (response) {
        if (response.success) {
            animatedClose(false, "LinkEdit_Container");
            openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.25);
            if (response.link1Tag != null) $("#AY_Link1").html("<small class='card-text text-dark'> <i class='fas fa-globe text-primary'></i> Official <span class='text-primary'>" + response.link1Tag + "</span> Link</small>");
            else $("#AY_Link1").html("<small class='card-text text-dark'> <i class='fas fa-globe text-primary'></i> Unknow Link</small>");
            $("#AY_Link1").attr("href", response.link1);
            if (response.link2Tag != null) $("#AY_Link2").html("<small class='card-text text-dark'> <i class='fas fa-globe text-primary'></i> Official <span class='text-primary'>" + response.link2Tag + "</span> Link</small>");
            else $("#AY_Link2").html("<small class='card-text text-dark'> <i class='fas fa-globe text-primary'></i> Unknow Link</small>");
            $("#AY_Link2").attr("href", response.link2);
        }
        else {
            animatedClose(false, "LinkEdit_Container");
            openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.25);
        }
    });
});

$("#EditPersonalInfo_Form").on("submit", function (event) {
    event.preventDefault();
    let url = $(this).attr("action");
    let data = $(this).serialize();

    $.post(url, data, function (response) {
        if (response.success) {
            animatedClose(false, "EditPersonalInfo_Container");
            openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.25);

            if (response.countryId != 0) {
                $("#AY_Country").text("Your country: " + response.country);
                $("#AY_CountryAdditional").text("Your country: " + response.country);
                $("#CurrentCountry_Badge").html("<i class='fas fa-globe-americas'></i> " + response.country);
                $("#CurrentCountry_Badge").removeClass("bg-light text-dark");
                $("#CurrentCountry_Badge").addClass("bg-primary text-light");
            }
            if (response.result.isCompany) {
                $("#AY_IsCompany").html("<i class='far fa-building text-primary'></i>  You are a company");
            }
            else {
                $("#AY_IsCompany").html("<i class='fas fa-user-tie text-primary'></i>  You are a personal user");
            }
            if (response.result.createdAt != null) {
                let dateTimeVal = convertDateAndTime(response.result.createdAt, true);
                dateTimeVal = dateTimeVal.substring(0, dateTimeVal.lastIndexOf(","));
                $("#AY_CreatedDate").html(" <i class='fas fa-clock text-primary'></i> Signed or created at: " + dateTimeVal);
            }
        }
        else {
            animatedClose(false, "EditPersonalInfo_Container");
            openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.75);
        }
    });
});

$("#SendCode_Form").on("submit", function (event) {
    event.preventDefault();
    let url = $(this).attr("action");
    let data = $(this).serialize();

    $.post(url, data, function (response) {
        if (response.success) {
            $(".modal").modal("hide");
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

$(document).on("submit", "#RemoveAllNotifications_Form", function (event) {
    event.preventDefault();

    let url = $("#RemoveAllNotifications_Form").attr("action");
    let data = $("#RemoveAllNotifications_Form").serialize();
    $.post(url, data, function (response) {
        if (response.success) {
            $("#StaticBackdrop_Container").modal("hide");
            setTimeout(function () {
                $("#SB_C-Body").empty();
            }, 300);
            openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.5);
        }
        else {
            $("#StaticBackdrop_Container").modal("hide");
            openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.5);
        }
    });
});

$("#CreateProject_Form").on("submit", function (event) {
    event.preventDefault();
    let url = $(this).attr("action");
    let data = $(this).serialize();

    $.post(url, data, function (response) {
        if (response.success) {
            $("#LookAtProject_Btn").attr("href", "/Project/ProjectInfo/" + response.link);
            animatedClose(false, "Preload_Container");
            openModal(response.alert, "Go To Project Page", " <i class='fas fa-times text-danger'></i> Close", 0, "/Project/ProjectInfo/" + response.link, 2, null, 3.75);
            animatedOpen(false, "Completed_Container");
        }
        else {
            openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.5);
        }
    });
});

$("#GetNotifications_Btn").on("click", function (event) {
    event.preventDefault();
    let url = $("#GetNotifications_Form").attr("action");
    let data = $("#GetNotifications_Form").serialize();
    $.get(url, data, function (response) {
        if (response.success) {
            $("#SB_C-Body").empty();
            $("#SB_C-Title").text("Notifications ∙ " + response.count.toLocaleString());
            if (response.count != 0) {
                let formDiv = $("<div class='mt-2'></div>");
                let formTag = $("<form method='post' action='/Notifications/RemoveAllNotifications' id='RemoveAllNotifications_Form'></form>");
                let formInput = $("<input type='hidden' name='Id' />");
                let formSbmtBtn = $("<button type='submit' class='btn btn-outline-danger border-0 btn-sm float-end ms-1 btn-standard-with-no-colour'>Remove All</button>");
                formTag.append(formInput);
                formTag.append(formSbmtBtn);
                formDiv.append(formTag);
                formInput.val(response.userId);

                $.each(response.notifications, function (index) {
                    let div = $("<div class='bordered-container p-2 mt-1 mb-1'></div>");
                    let dateTime = $("<small class='card-text text-muted float-end ms-1'></small>");
                    let title = $("<h6 class='h6 text-truncate'></h6>");
                    let separatorDiv = $("<div class='mt-3'></div>");
                    let separatorDiv2 = $("<div class='mt-1'></div>");
                    let text = $("<small class='card-text'></small>");
                    let removeBtn = $("<button type='button' class='btn btn-text btn-sm remove-notification'>&times; Remove</button>");

                    dateTime.html(convertDateAndTime(response.notifications[index].sentAt, false));
                    title.html(response.notifications[index].title);
                    text.html(response.notifications[index].description);

                    removeBtn.attr("id", "RemoveNotification-" + response.notifications[index].id);
                    div.attr("id", "Notification_Container-" + response.notifications[index].id);

                    div.append(dateTime);
                    div.append(title);
                    div.append(separatorDiv);
                    div.append(text);
                    div.append(separatorDiv2);
                    div.append(removeBtn);
                    $("#SB_C-Body").append(div);
                });
                $("#SB_C-Body").append(formDiv);
            }
            else {
                let div = $("<div class='box-container p-2 mt-1 mb-1 text-center'></div>");
                let anima = $("<h3 class='display-4'> <i class='far fa-bell-slash'></i> </h3>");
                let title = $("<h5 class='h5 safe-font mt-2'>No notifications</h5>");
                let text = $("<small class='card-text'>You haven't received any notifications yet</small>");

                div.append(anima);
                div.append(title);
                div.append(text);
                $("#SB_C-Body").append(div);
            }

            openStaticBackdrop(null, false);
        }
        else {
            openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.5);
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

$("#AddOptionSbmt_Btn").on("click", function () {
    let type = $("#AddOption_Type_Val").val();
    let value1 = $("#AddOption_Val1").val();
    let value2 = $("#AddOption_Val2").val();
    let whereAndFrom = $("#AddOption_WhereAndFrom_Val").val();

    if (type != 0) {
        animatedOpen(false, "EditDescription_Container", true, false);
        addOptionToText(whereAndFrom, type, value1, value2);
        $("#AddOption_Val1").val("");
        $("#AddOption_Val2").val("");
        $("#AddOptionSbmt_Btn").attr("disabled", true);
    }
    else openModal("Can't add this type of text. Please, try other type", " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 2.75);
});
$("#AddOption_Val1").on("keyup", function () {
    let value = $(this).val();
    let type = $("#AddOption_Type_Val").val();
    if (value != "" && type != 2) {
        $("#AddOptionSbmt_Btn").attr("disabled", false);
    }
    else $("#AddOptionSbmt_Btn").attr("disabled", true);
});
$("#AddOption_Val2").on("keyup change", function () {
    let value = $(this).val();
    let type = $("#AddOption_Type_Val").val();
    if (type == 2 && value == "") {
        $("#AddOptionSbmt_Btn").attr("disabled", true);
    }
    else $("#AddOptionSbmt_Btn").attr("disabled", false);
});

$("#EPI_Date_Day").on("keyup change", function () {
    let dayValue = $(this).val();
    let monthValue = $("#EPI_Date_Month").val();
    let yearValue = $("#EPI_Date_Year").val();

    if (dayValue < 10) dayValue = "0" + dayValue;
    else if (dayValue > 31 || dayValue <= 0) {
        dayValue = "01";
        $("#EPI_Date_Day").val(1);
    }
    if (monthValue < 10) monthValue = "0" + monthValue;
    else if (monthValue > 12 || monthValue <= 0) {
        monthValue = "01";
        $("#EPI_Date_Month").val(1);
    }

    $("#EPI_CreatedAt").val(dayValue + "." + monthValue + "." + yearValue + " 00:00:00");
    $("#EPI_DateInfo").text(dayValue + "/" + monthValue + "/" + yearValue);
});
$("#EPI_Date_Month").on("keyup change", function () {
    let dayValue = $("#EPI_Date_Day").val();
    let monthValue = $(this).val();
    let yearValue = $("#EPI_Date_Year").val();

    if (dayValue < 10) dayValue = "0" + dayValue;
    else if (dayValue > 31 || dayValue <= 0) {
        dayValue = "01";
        $("#EPI_Date_Day").val(1);
    }
    if (monthValue < 10) monthValue = "0" + monthValue;
    else if (monthValue > 12 || monthValue <= 0) {
        monthValue = "01";
        $("#EPI_Date_Month").val(1);
    }
  
    $("#EPI_CreatedAt").val(dayValue + "." + monthValue + "." + yearValue + " 00:00:00");
    $("#EPI_DateInfo").text(dayValue + "/" + monthValue + "/" + yearValue);
});
$("#EPI_Date_Year").on("keyup change", function () {
    let dayValue = $("#EPI_Date_Day").val();
    let monthValue = $("#EPI_Date_Month").val();
    let yearValue = $(this).val();
    let dT = new Date();
    let currentYear = dT.getFullYear();

    if (yearValue > currentYear) yearValue = currentYear;
    $("#EPI_Date_Year").val(yearValue);
    if (dayValue < 10) dayValue = "0" + dayValue;
    else if (dayValue > 31 || dayValue <= 0) {
        dayValue = "01";
        $("#EPI_Date_Day").val(1);
    }
    if (monthValue < 10) monthValue = "0" + monthValue;
    else if (monthValue > 12 || monthValue <= 0) {
        monthValue = "01";
        $("#EPI_Date_Month").val(1);
    }

    $("#EPI_CreatedAt").val(dayValue + "." + monthValue + "." + yearValue + " 00:00:00");
    $("#EPI_DateInfo").text(dayValue + "/" + monthValue + "/" + yearValue);
});

$("#Link1Tag").on("keyup change", function () {
    $("#FirstLinkTest_Btn").html($(this).val());
});
$("#Link2Tag").on("keyup change", function () {
    $("#SecondLinkTest_Btn").html($(this).val());
});
$("#Link1").on("keyup change", function () {
    $("#FirstLinkTest_Btn").attr("href", $(this).val());
});
$("#Link2").on("keyup change", function () {
    $("#SecondLinkTest_Btn").attr("href", $(this).val());
});

$(document).on("click", ".remove-notification", function (event) {
    let trueId = getTrueId(event.target.id);
    if (trueId != 0 || trueId != null || trueId != undefined) {
        let userId = $("#PageInfo_UserId").val();
        $("#RN_Id_Val").val(trueId);
        $("#RN_UserId_Val").val(userId);

        let url = $("#RemoveNotification_Form").attr("action");
        let data = $("#RemoveNotification_Form").serialize();
        $.post(url, data, function (response) {
            if (response.success) {
                if (response.count > 0) {
                    $("#SB_C-Title").text("Notifications ∙ " + response.count.toLocaleString());
                    slideToLeftAnimation("Notification_Container-" + trueId);
                    //$("#Notification_Container-" + response.id).fadeOut(300);
                }
                else {
                    $("#SB_C-Title").text("Notifications ∙ " + response.count.toLocaleString());
                    slideToLeftAnimation("Notification_Container-" + trueId);                  
                    setTimeout(function () {
                        $("#SB_C-Body").empty();
                        let div = $("<div class='box-container p-2 mt-1 mb-1 text-center'></div>");
                        let anima = $("<h3 class='display-4'> <i class='far fa-bell-slash'></i> </h3>");
                        let title = $("<h5 class='h5 safe-font mt-2'>No notifications</h5>");
                        let text = $("<small class='card-text'>You haven't received any notifications yet</small>");

                        div.append(anima);
                        div.append(title);
                        div.append(text);
                        $("#SB_C-Body").append(div);
                    }, 500);
                }
            }
            else {
                $("#StaticBackdrop_Container").modal("hide");
                openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.5);
            }
        });
    }
});
$("#TextPart").on("keyup", function () {
    lengthCounter("TextPart", 6000);
});
$("#Project_Description").on("keyup", function () {
    lengthCounter("Project_Description", 1600);
});
$("#ProjectLink1").on("keyup change", function () {
    let value = $(this).val();
    if (value != "") {
        if (!value.includes("http") || !value.includes("www")) {
            value = "https://www." + value;
        }
        $("#Link1Example_Btn").html(" <i class='fas fa-link'></i> Click to go by this link");
        $("#Link1Example_Btn").attr("href", value);
        $("#Link1_Link").text(value);
        $("#Link1_Link").fadeIn(300);
    }
    else {
        $("#Link1Example_Btn").html(" <i class='fas fa-globe'></i> No entered first link");
        $("#Link1Example_Btn").attr("href", "#");
        $("#Link1_Link").text("No link");
        $("#Link1_Link").fadeOut(300);
    }
});
$("#ProjectLink2").on("keyup change", function () {
    let value = $(this).val();
    if (value != "") {
        if (!value.includes("http") || !value.includes("www")) {
            value = "https://www." + value;
        }
        $("#Link2Example_Btn").html(" <i class='fas fa-link'></i> Click to go by this link");
        $("#Link2Example_Btn").attr("href", value);
        $("#Link2_Link").text(value);
        $("#Link2_Link").fadeIn(300);
    }
    else {
        $("#Link2Example_Btn").html(" <i class='fas fa-globe'></i> No entered first link");
        $("#Link2Example_Btn").attr("href", "#");
        $("#Link2_Link").text("No link");
        $("#Link2_Link").fadeOut(300);
    }
});
$("#YoutubeLink").on("keyup change", function () {
    let value = $(this).val();
    let trueLink = youtubeLinkCorrector(value);
    if (trueLink[trueLink.length - 1] != "/") {
        $("#NoVideo_Box").fadeOut(300);
        setTimeout(function () {
            $("#YT_VideoPlayer").attr("src", trueLink);
            $("#YT_VideoPlayer_Box").fadeIn(300);
        }, 200);
    }
    else {
        $("#YT_VideoPlayer_Box").fadeOut(300);
        $("#YT_VideoPlayer").attr("src", null);
        setTimeout(function () {
            $("#NoVideo_Box").fadeIn(300);
        }, 200);
    }
    $("#YT_VideoLink_Lbl").text("Link: " + value);
    $("#YT_VideoLink_Btn").attr("href", value);

    openModal("Watch to your selected video from Youtube to be sure that you've entered the true one", " <i class='fab fa-youtube text-danger'></i> Check Video", " <i class='fas fa-times text-danger'></i> Close", 4, "YTPreview_Container", 2, null, 5.5);
});
$("#ProjectPrice").on("change", function () {
    let value = $(this).val();
    let percentageFromFull = value / 10000000 * 100;
    let finalValue = parseInt(value).toLocaleString("en-US", { style: "currency", currency: "USD" });

    $("#ProjectPrice_Badge").html(finalValue);
    $("#ProjectPricePercentage_Badge").html(percentageFromFull.toFixed(2) + "%");
});
$("#IncreaseStep_Btn").on("click", function () {
    let currentStep = $("#ProjectPrice").attr("step");
    currentStep = parseInt(currentStep);

    if (currentStep >= 100 && currentStep < 1000) {
        currentStep += 100;
    }
    else if (currentStep >= 1000 && currentStep < 10000) {
        currentStep += 500;
    }
    else if (currentStep >= 10000 && currentStep < 25000) {
        currentStep += 1000;
    }
    else if (currentStep >= 25000 && currentStep < 50000) {
        currentStep += 5000;
    }
    else if (currentStep >= 50000 && currentStep < 100000) {
        currentStep += 10000;
    }
    else if (currentStep >= 100000 && currentStep < 1000000) {
        currentStep += 10000;
    }
    else if (currentStep >= 10000000) {
        currentStep += 0;
    }
    else {
        currentStep += 100000;
    }
    $("#ProjectPrice").attr("step", currentStep);
    currentStep = currentStep.toLocaleString("en-US", { style: "currency", currency: "USD" });

    $("#StepOfRange_Span").text("Step: " + currentStep);
});
$("#DecreaseStep_Btn").on("click", function () {
    let currentStep = $("#ProjectPrice").attr("step");
    currentStep = parseInt(currentStep);

    if (currentStep <= 100) {
        currentStep -= 0;
    }
    else if (currentStep > 100 && currentStep < 1000) {
        currentStep -= 100;
    }
    else if (currentStep >= 1000 && currentStep < 10000) {
        currentStep -= 500;
    }
    else if (currentStep >= 10000 && currentStep < 25000) {
        currentStep -= 1000;
    }
    else if (currentStep >= 25000 && currentStep < 50000) {
        currentStep -= 5000;
    }
    else if (currentStep >= 50000 && currentStep < 100000) {
        currentStep -= 10000;
    }
    else if (currentStep >= 100000 && currentStep < 1000000) {
        currentStep -= 10000;
    }
    else {
        currentStep -= 100000;
    }
    $("#ProjectPrice").attr("step", currentStep);
    currentStep = currentStep.toLocaleString("en-US", { style: "currency", currency: "USD" });

    $("#StepOfRange_Span").text("Step: " + currentStep);
});

$("#PreviewTheProject_Btn").on("click", function () {
    previewCombinizer(true);
});

$(document).on("click", ".btn-back", function () {
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
$(document).on("click", ".smallside-btn-close-n-open", function (event) {
    let trueId = getTrueName(event.target.id);
    animatedClose(true, "smallside-box-container");
    smallBarAnimatedOpenAndClose(false);
    animatedOpen(false, trueId, true, true);
});
$(document).on("mouseover", ".smallside-modal-open-by-hover", function (event) {
    let alert = $("#" + event.target.id).attr("data-content");
    if (alert != undefined || alert != null) {
        openModal(alert, " <i class='fas fa-times'></i> Close", null, 2, null, null, null, Infinity);
    } 
});
$(document).on("click", ".btn-static-bar-open", function (event) {
    let trueId = getTrueName(event.target.id);
    let currentBtnBottom = $("#" + trueId + "-Open").css("bottom");
    let currentBarHeight = $("#" + trueId).innerHeight();

    currentBtnBottom = parseInt(currentBtnBottom) + currentBarHeight + 2;
    $("#" + trueId).fadeIn(0);
    $("#" + trueId).css("bottom", botNavbarH + 2 + "px");
    $("#" + trueId + "-Open").css("bottom", currentBtnBottom + 18 + "px");
    $("#" + trueId + "-Open").attr("disabled", true);
    $("#" + trueId + "-Open").css("transform", "rotate(-45deg)");
    setTimeout(function () {
        $("#" + trueId + "-Open").css("bottom", currentBtnBottom + "px");
        $("#" + trueId + "-Open").css("transform", "rotate(180deg)");
    }, 325);
});
$(document).on("click", ".btn-status-bar-close", function (event) {
    let trueId = getTrueName(event.target.id);
    let currentBtnBottom = $("#" + trueId + "-Open").css("bottom");
    currentBtnBottom = parseInt(currentBtnBottom) + 18;

    $("#" + trueId).css("z-index", "-1000");
    $("#" + trueId).css("bottom", "-1200px");
    setTimeout(function () {
        $("#" + trueId).css("z-index", "0");
        $("#" + trueId + "-Open").css("bottom", currentBtnBottom + "px");
        $("#" + trueId + "-Open").css("transform", "rotate(360deg)");
    }, 50);
    setTimeout(function () {
        $("#" + trueId).fadeOut(500);
        $("#" + trueId + "-Open").css("bottom", botNavbarH + 9 + "px");
        $("#" + trueId + "-Open").attr("disabled", false);
    }, 275);
});
$(document).on("click", ".btn-static-backdrop-open", function (event) {
    openStaticBackdrop(event.target.id, true);
});

$(document).on("click", ".add-option", function (event) {
    let type = getTrueId(event.target.id);
    let whereAndFrom = $("#" + event.target.id).attr("data-html");
    type = parseInt(type);

    smallBarAnimatedOpenAndClose(true);
    animatedOpen(false, "AddOption_Container", false, false);
    $("#AddOption_Type_Val").val(type);
    $("#AddOption_WhereAndFrom_Val").val(whereAndFrom);
    switch (type) {
        case 1:
            $("#AddOption_Badge").html("Bold Text");
            $("#AddOption_Val2").attr("disabled", true);
            break;
        case 2:
            $("#AddOption_Badge").html("Link Text");
            $("#AddOption_Val2").attr("disabled", false);
            break;
        case 3:
            $("#AddOption_Badge").html("Underlined Text");
            $("#AddOption_Val2").attr("disabled", true);
            break;
        case 4:
            $("#AddOption_Badge").html("Italic Styled Text");
            $("#AddOption_Val2").attr("disabled", true);
            break;
        default:
            $("#AddOption_Badge").html("Bold Text");
            $("#AddOption_Val2").attr("disabled", true);
            break;
    }
});

function textDecoder(element, isFromText, needsTheReplacement) {
    let value;
    if (isFromText) value = $("#" + element).text();
    else value = $("#" + element).val();

    value = value.replaceAll("[[", "<span class='fw-500'>");
    value = value.replaceAll("]]", "</span>");
    value = value.replaceAll("[{", "<a class='text-decoration-none text-primary'");
    value = value.replaceAll("}]", "</a>");
    value = value.replaceAll("{[", "<span class='text-decoration-underline'>");
    value = value.replaceAll("[^", "<span class='fst-italic'>");

    if (needsTheReplacement) {
        if (isFromText) $("#" + element).html(value);
        else $("#" + element).val(value);
    }

    return value;
}

function previewCombinizer(previewingProject) {
    if (previewingProject) {
        let title = $("#Name").val();
        let smallerDescription = textDecoder("Project_Description", null, false);
        let longerDescription = textDecoder("TextPart", null, false);
        let link1 = $("#ProjectLink1").val();
        let link2 = $("#ProjectLink1").val();
        let ytLink = $("#YoutubeLink").val();
        let projectPrice = $("#ProjectPrice").val();
        
        $("#Preview_Title").text(title);
        $("#Preview_ShortDescription").html(smallerDescription);
        $("#Preview_LongDescription").html(longerDescription);
        if (link1 != null) {
            $("#Preview_LinkBtn1").attr("href", link1);
            $("#Preview_LinkText1").text("Link: " + link1);
        }
        if (link2 != null) {
            $("#Preview_LinkBtn2").attr("href", link2);
            $("#Preview_LinkText2").text("Link: " + link2);
        }

        if (projectPrice != 0) {
            $("#Preview_TagPrice").text(parseFloat(projectPrice).toLocaleString("en-US", { style: "currency", currency: "USD" }));
            $("#Preview_TagPriceDescription").text("needs this project for his start");
        }
        else {
            $("#Preview_TagPrice").text("No investments");
            $("#Preview_TagPriceDescription").text("There's no needed investments for this project. Anyway, the owner of project can add the investments part later");
        }
    }

    animatedOpen(false, "Preview_Container", true, true);
}

function lengthCounter(element, maxValue) {
    let text = $("#" + element).val();
    let length = text.length;
    let leftSymbols = maxValue - length;

    $("#" + element + "-LengthSpan").text(length + "/" + maxValue + ", -" + leftSymbols);
}

function addOptionToText(element, type, value1, value2) {
    let textBefore;
    let textAfter;
    let text = $("#" + element).val();
    let cursorPosition = $("#" + element).prop("selectionStart");
    textBefore = text.substring(0, cursorPosition);
    textAfter = text.substring(cursorPosition);
    type = parseInt(type);

    switch (type) {
        case 1:
            textBefore = textBefore + "[[" + value1 + "]]";
            break;
        case 2:
            textBefore = textBefore + "[{ href='" + value2 + "'>" + value1 + "}]";
            break;
        case 3:
            textBefore = textBefore + "{[" + value1 + "]]";
            break;
        case 4:
            textBefore = textBefore + "[^" + value1 + "]]";
            break;
        default:
            textBefore = textBefore + " [[" + value1 + "]]";
            break;
    }

    $("#" + element).val(textBefore + textAfter);
}

function convertDateAndTime(value, asShort) {
    let dT = new Date(value);
    let day = 0;
    let month = 0;
    let hr = 0;
    let min = 0;
    let fullAlert;

    if (dT.getHours() < 10) hr = "0" + dT.getHours();
    else hr = dT.getHours();
    if (dT.getMinutes() < 10) min = "0" + dT.getMinutes();
    else min = dT.getMinutes();

    if (dT.getDate() < 10) day = "0" + dT.getDate();
    else day = dT.getDate();

    if (asShort) {
        month = parseInt(dT.getMonth()) + 1;
        if (dT.getMonth() < 10) month = "0" + month;
        else month = month + 1;

        fullAlert = day + "/" + month + "/" + dT.getFullYear() + ", at " + hr + ":" + min;
    }
    else {
        let months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
        month = months[dT.getMonth()];

        fullAlert = day + " " + month + ", at " + hr + ":" + min;
    }

    return fullAlert;
}

function getTrueId(fullName) {
    return fullName.substring(fullName.lastIndexOf("-") + 1);
}

function getTrueName(fullName) {
    let separatorIndex = fullName.lastIndexOf("-");
    let substringedName = fullName.substring(0, separatorIndex);

    return substringedName;
}

function youtubeLinkCorrector(initialLink) {
    let trueLink;
    if (initialLink.includes("watch")) {
        trueLink = initialLink.substring(initialLink.lastIndexOf("=") + 1);
    }
    else {
        trueLink = initialLink.substring(initialLink.lastIndexOf("/") + 1);
    }
    trueLink = "https://www.youtube.com/embed/" + trueLink;

    return trueLink;
}

function openLastContainer(element) {
    if (element == undefined) {
        animatedOpen(false, "Preload_Container", true, true);
    }
    else if (element == null) {
        return false;
    }
    else {
        animatedOpen(false, element, true, true);
    }
}

function openModal(text, btn1Txt, btn2Txt, btn1WhatToDo, btn1Action, btn2WhatToDo, btn2Action, fadeOutTimer) {
    setTimeout(function () {
        let currentContainerB = $("#" + currentContainerName).css("bottom");
        let currentContainerH = $("#" + currentContainerName).innerHeight();
        let containersFullHeight = parseInt(currentContainerB) + parseInt(currentContainerH);
        let windowFullHeight = fullHeigth;

        $("#MMC_Text").html(text);
        $("#MainModal_Container").fadeIn(0);
        $("#MainModal_Container").css("z-index", "1000");

        let modalH = $("#MainModal_Container").innerHeight();
        windowFullHeight -= modalH + 8;

        if (containersFullHeight > 0 && containersFullHeight < windowFullHeight) {
            $("#MainModal_Container").css("bottom", containersFullHeight + 24 + "px");
            setTimeout(function () {
                $("#MainModal_Container").css("bottom", containersFullHeight + 8 + "px");
            }, 500);
        }
        else {
            animatedClose(true, "smallside-box-container");
            $("#MainModal_Container").css("bottom", "24px");
            setTimeout(function () {
                $("#MainModal_Container").css("bottom", "8px");
            }, 500);
        }
    }, 150);

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
                    animatedOpen(false, btn1Action, true, true);
                    $("#MainModal_Container").css("bottom", "-1200px");
                    $("#MainModal_Container").fadeOut(300);
                });
                break;
            case 2:
                $("#MMC_FirstBtn").on("click", function () {
                    animatedClose(false, "MainModal_Container");
                });
                break;
            case 4:
                $("#MMC_FirstBtn").on("click", function () {
                    smallBarAnimatedOpenAndClose(true);
                    animatedOpen(false, btn1Action, false, false);
                    $("#MainModal_Container").css("bottom", "-1200px");
                    $("#MainModal_Container").fadeOut(300);
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

function slideToLeftAnimation(element) {
    let elementCurrentLeft = $("#" + element).css("left");
    elementCurrentLeft = parseInt(elementCurrentLeft);

    $("#" + element).css("position", "relative");
    $("#" + element).css("border", "none");
    $("#" + element).css("margin-left", elementCurrentLeft + 16 + "px");
    setTimeout(function () {
        $("#" + element).css("margin-left", "-1200px");
    }, 350);
    $("#" + element).fadeOut(500);
}

function navBarBtnSelector(href) {
    if (href.toLowerCase().includes("create")) {
        $("#SearchLink_Btn").fadeOut(0);
        $("#SecondReserve_Btn").fadeIn(0);
        $("#SecondReserve_Btn").addClass("btn-open-container");
        $("#SecondReserve_Btn").html(" <i class='fas fa-sign-in-alt text-muted'></i> <br/>Enter");
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
    $(".btn-right-bottom-fixed").css("bottom", botNavbarH + 10 + "px");

    if (width < 717) {
        $(".main-container").css("width", "100%");
        $(".main-container").css("left", 0);
        $(".static-bar").css("width", "100%");
        $(".static-bar").css("left", 0);
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
        $("#Main_SideBar").css("width", "35%");
        let leftBarW = $("#Main_SideBar").innerWidth() + 3;
        let leftW = fullWidth - leftBarW - 3;
        let smallSideContainerW = leftBarW - 5;

        $("#Main_SideBar").fadeIn(0);
        $("#Main_SideBar").css("left", 0);
        $(".main-container").css("width", leftW + "px");
        $(".main-container").css("left", leftBarW + "px");
        $(".static-bar").css("width", leftW + "px");
        $(".static-bar").css("left", leftBarW + "px");
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
    smallBarAnimatedOpenAndClose(false);
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

function openStaticBackdrop(id, truncate) {
    $(".modal").modal("hide");
    if (id == null || id == undefined || id == "") {
        $("#StaticBackdrop_Container").modal("toggle");
    }
    else {
        if (truncate) id = getTrueName(id);
        $("#" + id).modal("toggle");
    }
}

function animatedClose(forAll, elemet) {
    if (forAll) {
        $("." + elemet).css("bottom", "-1200px");
        $("." + elemet).fadeOut(250);
    }
    else {
        $("#" + elemet).css("bottom", "-1200px");
        $("#" + elemet).fadeOut(250);
    }
}

$(document).on("mouseover", ".info-popover", function (event) {
    $("#" + event.target.id).popover("show");
});
$(document).on("mouseout", ".info-popover", function () {
    $(".info-popover").popover("hide");
});
$(document).on("click", ".form-check-input", function (event) {
    let trueId = event.target.id;
    let value = $("#" + trueId).val();
    value = value == "false" ? false : true;

    if (value) {
        $("#" + trueId).val(false);
    }
    else {
        $("#" + trueId).val(true);
    }
});

$(document).on("change", ".datalist-select", function (event) {
    //let value = $(this).val();
    //let text = $(this).text();
    //console.log(value);
});