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
    else if (currentUrl.toLowerCase().includes("/project/info")) {
        textDecoder("Preview_LongDescription", true, true);
        textDecoder("CategoryMainIcon_Lbl", true, true);
        setTimeout(function () {
            $("#CategoryMainIcon_Lbl").removeClass("d-none");
        }, 75);
    }
    else if (currentUrl.toLowerCase().includes("/user/info")) {
        textDecoder("UserInfo_LinkTag1_Lbl", true, true);
        textDecoder("UserInfo_LinkTag2_Lbl", true, true);
        textDecoder("ProfileDescription_Lbl", true, true);
    }
    else if (currentUrl.toLowerCase().includes("/project/create")) {
        let lastId = $("#LastCategoryId_Val").val();
        for (let i = 1; i <= lastId; i++) {
            textDecoder("CategoryIcon-" + i, true, true);
        }
    }
    else if (currentUrl.toLowerCase().includes("/project/edit")) {
        let lastId = $("#LastCategoryId_Val").val();
        for (let i = 1; i <= lastId; i++) {
            textDecoder("CategoryIcon-" + i, true, true);
        }
    }
    else if (currentUrl.toLowerCase().includes("/search")) {
        let lastId = $("#LastCategoryId_Val").val();
        for (let i = 1; i <= lastId; i++) {
            textDecoder("CategoryIcon-" + i, true, true);
        }
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
                let dateTimeVal = convertDateAndTime(response.result.createdAt, true, true);
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

$("#EditProfilePhoto_Form").on("submit", function (event) {
    event.preventDefault();
    let file = $("#ProfilePhoto").get(0).files[0];
    let url = $(this).attr("action");
    let formData = new FormData();

    formData.append("id", $("#EPP_Id_Val").val());
    formData.append("profilePhoto", file);
    $.ajax({
        type: "Post",
        url: url,
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            if (response.success) {
                $("#AY_ProfilePhoto_Img").attr("src", "/ProfilePhotos/" + response.url);
                $("#AY_ProfilePhoto_Img").removeClass("d-none");
                animatedClose(false, "AddPhoto_Container");
                openModal(response.alert, "Done", null, 2, null, null, null, 3.5, "<i class='far fa-image text-primary'></i>");
            }
            else {
                openModal(response.alert, "Done", null, 2, null, null, null, 3.5, "<i class='fas fa-times-circle text-danger'></i>");
            }
        }
    });
});

$("#GetShortUserInfo_Form").on("submit", function (event) {
    event.preventDefault();
    let url = $(this).attr("action");
    let data = $(this).serialize();
    $.get(url, data, function (response) {
        if (response.success) {
            $("#USI_UsernameTitle_Span").html(response.result.pseudoName);
            $("#USI_Username_Lbl").html(response.result.pseudoName);
            $("#USI_Searchname_Lbl").html("@" + response.result.searchName);
            $("#USI_Id_Val").val(response.result.id);
            $("#USI_SubscribersCount_Val").val(response.result.subscribersCount);
            $("#USI_SubscribersInfo_Btn").html(" <i class='fas fa-bell'></i> " + response.result.subscribersCount + " subscriber(s)");
            if (response.result.description != null) $("#USI_Description_Lbl").text(response.result.description);
            else $("#USI_Description_Lbl").text("No user description");
            $("#USI_Userpage_Link").attr("href", "/User/Info/" + response.result.searchName);
            $("#USI_Userpage_Link").html(" <i class='fas fa-chevron-right'></i> " + response.result.pseudoName + "'s Page");
            $("#USI_Inbox_Lbl").html(" <i class='fas fa-inbox text-primary'></i> " + response.result.email);
            $("#USI_Date_Lbl").html(" <i class='far fa-calendar-alt text-primary'></i> Created At: " + convertDateAndTime(response.result.createdAt, false, false));
            if (response.result.link1 != null) {
                $("#USI_Link1Tag_Lbl").text(response.result.link1Tag);
                $("#USI_Link1_Lbl").html(" <i class='fas fa-external-link-alt text-primary'></i> " + response.result.link1);
            }
            else {
                $("#USI_Link1_Lbl").html(" <i class='fas fa-external-link-alt text-primary'></i> No external first link");
                $("#USI_Link1Tag_Lbl").text("No Link Tag");
            }
            if (response.result.link2 != null) {
                $("#USI_Link2Tag_Lbl").text(response.result.link2Tag);
                $("#USI_Link2_Lbl").html(" <i class='fas fa-external-link-alt text-primary'></i> " + response.result.link2Tag);
            }
            else {
                $("#USI_Link2Tag_Lbl").text("No Link Tag");
                $("#USI_Link2_Lbl").html(" <i class='fas fa-external-link-alt text-primary'></i> No external second link");
            }

            if (response.result.country.name != null) {
                $("#USI_CountryBadge").html(" <i class='fas fa-globe-americas'></i> " + response.result.country.name);
            }
            else $("#USI_CountryBadge").html(" <i class='fas fa-globe-americas'></i> No Country Info");
            $("#USI_ProjectsCount_Lbl").html(response.result.projectsCount.toLocaleString() + " project(s) published");

            textDecoder("USI_Description_Lbl", true, true);
            smallBarAnimatedOpenAndClose(true);
            animatedOpen(false, "UserShortInfo_Container", false, false);
        }
        else {
            openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.25);
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
    let formData = new FormData();
    let files = $("#ImagesFiles").get(0).files;
    let count = files.length;

    formData.append("name", $("#Name").val());
    formData.append("userId", $("#UserId").val());
    formData.append("description", $("#Project_Description").val());
    formData.append("textPart", $("#TextPart").val());
    formData.append("link1", $("#ProjectLink1").val());
    formData.append("link2", $("#ProjectLink2").val());
    formData.append("youtubeLink", $("#YoutubeLink").val());
    formData.append("categoryId", $("#CategoryId").val());
    formData.append("projectPrice", $("#ProjectPrice").val());
    formData.append("targetPriceChangeAnnotation", $("#TargetPriceChangeAnnotation").val());
    formData.append("donationLink", $("#DonationLink").val());
    if ($("#Duration").val() != 0) formData.append("duration", $("#Duration").val());
    else formData.append("duration", 0);
    for (let i = 0; i < count; i++) {
        formData.append("imagesFiles", files[i]);
    }

    $.ajax({
        type: "POST",
        url: url,
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response.success) {
                $("#LookAtProject_Btn").attr("href", "/Project/Info/" + response.link);
                animatedOpen(false, "Completed_Container", true, true);
                openModal(response.alert, "Go To Project", " <i class='fas fa-times text-danger'></i> Close", 0, "/Project/Info/" + response.link, 2, null, 3.75);
            }
            else {
                openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.5);
            }
        }
    });
});
$("#EditProject_Form").on("submit", function (event) {
    event.preventDefault();
    let url = $(this).attr("action");
    let data = $(this).serialize();

    $.post(url, data, function (response) {
        if (response.success) {
            $("#LookAtProject_Btn").attr("href", "/Project/Info/" + response.link);
            animatedOpen(false, "Completed_Container", true, true);
            openModal(response.alert, "Go To Project", " <i class='fas fa-times text-danger'></i> Close", 0, "/Project/Info/" + response.link, 2, null, 3.75);
        }
        else {
            animatedOpen(false, "Preload_Container", true, true);
            openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.25);
        }
    });
});

$("#EditProjectImages_Form").on("submit", function (event) {
    event.preventDefault();

    let files = $("#Images").get(0).files[0];
    let url = $(this).attr("action");
    let formData = new FormData();

    formData.append("id", $("#EPI_Val_Id").val());
    formData.append("images", files);
    for (let i = 1; i <= 6; i++) {
        if ($("#Images").get(0).files[i] != undefined) {
            formData.append("images", $("#Images").get(0).files[i]);
        }
    }

    $.ajax({
        type: "POST",
        data: formData,
        url: url,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response.success) {
/*                animatedClose(false, "AddImages_Container");*/
                $("#HaveImages_Box").empty();

                let result = response.result;
                let count = result.length;
                for (let i = 0; i < count; i++) {
                    let div = $("<div class='box-container bg-transparent me-2 d-inline-block'></div>");
                    let imgTag = $("<img class='image-preview-box' />");
                    imgTag.attr("src", "/ProjectPhotos/" + result[i]);

                    div.append(imgTag);
                    $("#HaveImages_Box").append(div);
                }
                setTimeout(function () {
                    $("#HaveImages_Box").fadeIn(0);
                    $("#NoImages_Box").fadeOut(0);
                    animatedOpen(false, "AddImages_Container", false, false);
                }, 250);
                setTimeout(function () {
                    openModal(response.alert, "Done", null, 2, null, null, null, 3.75, "<i class='far fa-images text-primary'></i>");
                }, 750);
            }
            else {
                $("#Images").val(null);
                $("#Images-Get").text("Click to Upload");
                $("#ChosenFileName_Lbl").text("You can upload up to 6 images per project");
                openModal(response.alert, "Done", null, 2, null, null, null, 3.5, "<i class='far fa-times-circle text-danger'></i>");
            }
        }
    });
});

$("#EditProjectAudios_Form").on("submit", function (event) {
    event.preventDefault();

    let url = $(this).attr("action");
    let formData = new FormData();
    let files = $("#Audios").get(0).files;

    formData.append("id", $("#EPA_Val_Id").val());
    for (let i = 0; i < 4; i++) {
        if (files[i] != undefined) {
            formData.append("audios", files[i]);
        }
    }

    $.ajax({
        type: "POST",
        url: url,
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response.success) {
                openModal(response.alert, "Done", null, 2, null, null, null, 3.75, "<i class='fas fa-music text-primary'></i>");
            }
            else {
                openModal(response.alert, "Done", null, 2, null, null, null, 3.75, "<i class='fas fa-times-circle text-danger'></i>");
            }
        }
    });
});

$(document).on("click", ".btn-remove-img", function (event) {
    let trueId = getTrueId(event.target.id);
    if (trueId != "") {
        $("#RI_ImageId_Val").val(trueId);
        let url = $("#RemoveImage_Form").attr("action");
        let data = $("#RemoveImage_Form").serialize();

        $.post(url, data, function (response) {
            if (response.success) {
                let count = $("#CurrentImgs_Count").val();
                count = parseInt(count) - 1;
                if (count == 0) {
                    animatedClose(false, "AddImages_Container");
                    $("#NoImages_Box").fadeIn(100);
                    $("#HaveImages_Box").fadeOut(100);
                    setTimeout(function () {
                        animatedOpen(false, "AddImages_Container", false, false);
                    }, 300);
                }
                else {
                    $("#ProjectImg-" + response.result).fadeOut(300);
                    setTimeout(function () {
                        $("#ProjectImg-" + response.result).addClass("d-none");
                    }, 225);
                }
            }
            else {
                openModal(response.alert, "Got It", null, 2, null, null, null, 3.5, "<i class='fas fa-times-circle text-danger'></i>");
            }
        });
    }
});

$("#RemoveProject_Form").on("submit", function (event) {
    event.preventDefault();
    let url = $(this).attr("action");
    let data = $(this).serialize();

    $.post(url, data, function (response) {
        if (response.success) {
            let currentCount = $("#ProjectsCount_Span").text();
            currentCount = parseInt(currentCount);
            smallBarAnimatedOpenAndClose(false);
            animatedClose(false, "RemoveProject_Container");
            openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.5);
            currentCount--;
            if (currentCount > 0) {
                animatedOpen(false, "Preload_Container", true, true);
                $("#ProjectsCount_Span").text(currentCount);
                $("#ProjectContainer-" + response.id).fadeOut(350);
            }
            else {
                animatedOpen(false, "Preload_Container", true, true);
                $("#ProjectsCount_Lbl").fadeOut(325);
                $("#HaveProjects_Box").fadeOut(325);
                $("#NoProjects_Box").fadeIn(650);
            }
        }
        else {
            smallBarAnimatedOpenAndClose(false);
            animatedClose(false, "RemoveProject_Container");
            openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.25);
        }
    });
});

$("#AddUpdate_Form").on("submit", function (event) {
    event.preventDefault();
    let url = $(this).attr("action");
    let data = $(this).serialize();

    $.post(url, data, function (response) {
        animatedClose(false, "AddUpdate_Container");
        openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.75);
    });
});

$(document).on("click", ".get-release-notes", function (event) {
    let trueId = getTrueId(event.target.id);
    if (trueId != "") {
        $("#GUF_ProjectId_Val").val(trueId);
        $("#GetUpdates_Form").submit();
    }
    else {
        $("#GUF_ProjectId_Val").val(trueId);
    }
});
$("#GetUpdates_Form").on("submit", function (event) {
    event.preventDefault();
    let url = $(this).attr("action");
    let data = $(this).serialize();

    $.get(url, data, function (response) {
        if (response.success) {
            if (response.count > 0) {
                $("#ReleaseNotes_Count_Lbl").text(response.count.toLocaleString());
                $("#ReleaseNotes_Box").empty();
                $.each(response.result, function (index) {
                    let div = $("<div class='release-note-container mt-2 p-2 pt-0 pb-0'></div>");
                    let title = $("<h6 class='h6 mt-1'></h6>");
                    let text = $("<small class='card-text white-space-on'></small>");
                    let sentAt = $("<small class='card-text safe-font text-primary'></small>");
                    let separatorOne = $("<div class='mt-2'></div>");

                    title.text(response.result[index].title);
                    text.html(response.result[index].description);
                    sentAt.html(" <i class='far fa-clock'></i> " + convertDateAndTime(response.result[index].releaseDate, false, true));

                    div.append(sentAt);
                    div.append(title);
                    div.append(text);
                    $("#ReleaseNotes_Box").append(div);
                });
            }
            else {
                let div = $("<div class='box-container mt-3 p-2 text-center'></div>");
                let icon = $("<h2 class='h2'> <i class='fas fa-sync-alt'></i> </h2>");
                let title = $("<h6 class='h6 safe-font'>No Release Notes</h6>");
                let text = $("<small class='card-text text-muted'>There's no any release notes for this project yet</small>");

                div.append(icon);
                div.append(title);
                div.append(text);
                $("#ReleaseNotes_Box").append(div);
            }
            smallBarAnimatedOpenAndClose(true);
            animatedOpen(false, "ReleaseNotes_Container", false, false);
        }
        else {
            openModal(response.alert, " <i class='fas fa-close text-danger'></i> Close", null, 2, null, null, null, 3.5);
        }
    });
});

$("#GetFullProject_Form").on("submit", function (event) {
    event.preventDefault();
    let url = $(this).attr("action");
    let data = $(this).serialize();

    $.get(url, data, function (response) {
        if (response.success) {
            let fullText;

            $("#GSU_Id_Val").val(response.result.userId);
            $("#GCF_ProjectId_Val").val(response.result.id);
            $("#SCF_ProjectId_Val").val(response.result.id);
            $("#GUF_ProjectId_Val").val(response.result.id);

            $("#RFL_ProjectId_Val").val(response.result.id);
            $("#LTP_ProjectId_Val").val(response.result.id);

            $("#Preview_Title").text(response.result.name);
            $("#Preview_ShortDescription").text(response.result.description);
            $("#Preview_ViewsCnt").text(response.result.views);
            $("#Preview_ProjectPageLink").attr("href", "/Project/Info/" + response.result.id);
            $("#Preview_ProjectPageLink2").attr("href", "/Project/Info/" + response.result.id);
            if (response.getUsername) $("#GCF_UserPseudoName_Lbl").text(response.result.userName);
            if (response.result.categoryName != null) $("#Preview_Category").text(response.result.categoryName);
            else $("#Preview_Category").text("Unknown");
            if (response.result.textPart1 != null) fullText = response.result.textPart1;
            if (response.result.textPart2 != null) fullText += response.result.textPart2;
            if (response.result.textPart3 != null) fullText += response.result.textPart3;

            if (response.images.length > 0) {
                $("#PreviewImages_Box").fadeIn(350);
                $("#ImagesCount_Lbl").text(response.images.length);
                $("#Images_Box").empty();
                $.each(response.images, function (index) {
                    let div = $("<div class='box-container bg-transparent d-inline-block me-2'></div>");
                    let img = $("<img class='image-main-box' alt='Cannot display this image' />");
                    img.attr("src", "/ProjectPhotos/" + response.images[index].name);
                    div.append(img);
                    div.attr("id", "ImgBox-" + response.images[index].id);
                    $("#Images_Box").append(div);
                });
            }
            else {
                $("#Images_Box").empty();
                $("#PreviewImages_Box").fadeOut(350);
            }

            if (response.audios.length > 0) {
                $("#AudiosExample_Box").empty();
                let count = response.audios.length;
                $("#AudiosCount_Lbl").text(count);
                $.each(response.audios, function (index) {
                    let mainDiv = $("<div class='mt-1 mb-1'></div>");
                    let secondDiv = $("<div class='box-container bg-light p-2'></div>");
                    let thirdDiv = $("<div class='d-inline-block mt-1'></div>");
                    let durationDiv = $("<div class='box-container bg-secondary rounded-pill mt-1 ms-1 standard-transition'></div>");
                    let durationProgress = $("<div class='box-container bg-dark audio-progress rounded-pill'></div>");
                    let audioTag = $("<audio preload='metadata'></audio>");
                    let sourceOne = $("<source type='audio/mp3' />");
                    let sourceTwo = $("<source type='audio/ogg' />");
                    let audioSrcInput = $("<input type='hidden' />");
                    let audioFileName = $("<h6 class='h6 safe-font ms-1'></h6>");
                    let audioDuration_Lbl = $("<small class='card-text pt-4 fw-500'></small>");

                    let backwardBtn = $("<button type='button' class='btn btn-light btn-standard-with-no-colour border-0 btn-sm btn-audio-backward me-1' id='AudioBackwardBtn-" + response.audios[index].id + "'> <i class='fas fa-backward btn-audio-backward' id='AudioBackwardIcon-" + response.audios[index].id + "'></i> </button>");
                    let playBtn = $("<button type='button' class='btn btn-light btn-standard-with-no-colour border-0 btn-sm btn-play-audio me-1' id='PlayAudioBtn-" + response.audios[index].id + "'> <i class='fas fa-play btn-play-audio' id='PlayAudioIcon-" + response.audios[index].id + "'></i> </button>");
                    let pauseBtn = $("<button type='button' class='btn btn-light btn-standard-with-no-colour border-0 btn-sm btn-pause-audio me-1 d-none' id='PauseAudioBtn-" + response.audios[index].id + "'> <i class='fas fa-pause btn-pause-audio' id='PauseAudioIcon-" + response.audios[index].id + "'></i> </button>");
                    let muteBtn = $("<button type='button' class='btn btn-light btn-standard-with-no-colour border-0 btn-sm btn-mute-audio me-1' id='MuteAudioBtn-" + response.audios[index].id + "'> <i class='fas fa-volume-up btn-mute-audio' id='MuteAudioIcon-" + response.audios[index].id + "'></i> </button>");
                    let unMuteBtn = $("<button type='button' class='btn btn-light btn-standard-with-no-colour border-0 btn-sm btn-unmute-audio me-1 d-none' id='UnmuteAudioBtn-" + response.audios[index].id + "'> <i class='fas fa-volume-mute btn-mute-audio' id='UnmuteAudioIcon-" + response.audios[index].id + "'></i> </button>");

                    audioSrcInput.val("/ProjectAudios/" + response.audios[index].name);
                    audioFileName.text(response.audios[index].name);
                    audioDuration_Lbl.text("00:00/00:00");
                    sourceOne.attr("src", "/ProjectAudios/" + response.audios[index].name);
                    sourceTwo.attr("src", "/ProjectAudios/" + response.audios[index].name);

                    audioTag.attr("id", "AudioControl-" + response.audios[index].id);
                    audioDuration_Lbl.attr("id", "AudioDuration-" + response.audios[index].id);
                    audioFileName.attr("id", "AudioFileName-" + response.audios[index].id);
                    durationProgress.attr("id", "AudioDurationProgress-" + response.audios[index].id);
                    audioSrcInput.attr("id", "AudioSrc-" + response.audios[index].id);

                    audioTag.append(sourceOne);
                    audioTag.append(sourceTwo);

                    durationDiv.append(durationProgress);
                    secondDiv.append(audioFileName);
                    secondDiv.append(audioSrcInput);
                    secondDiv.append(audioTag);
                    thirdDiv.append(backwardBtn);
                    thirdDiv.append(pauseBtn);
                    thirdDiv.append(playBtn);
                    thirdDiv.append(muteBtn);
                    thirdDiv.append(unMuteBtn);
                    thirdDiv.append(audioDuration_Lbl);
                    thirdDiv.append(durationDiv);
                    secondDiv.append(thirdDiv);
                    mainDiv.append(secondDiv);

                    $("#AudiosExample_Box").append(mainDiv);
                });
                $("#PreviewAudios_Box").fadeIn(350);
            }
            else {
                $("#PreviewAudios_Box").fadeOut(300);
                $("#AudiosExample_Box").empty();
            }

            if (response.result.deadline != null) {
                let todaysDate = new Date();
                let deadlineDay = new Date(response.result.deadline);
                let daysLeft = 0;

                if (todaysDate.getFullYear() < deadlineDay.getFullYear()) {
                    let yearsPast = deadlineDay.getFullYear() - todaysDate.getFullYear();
                    if (yearsPast > 1) {
                        daysLeft += ((yearsPast - 1) * 365) + (deadlineDay.getMonth()) * 30 + deadlineDay.getDate() + todaysDate.getDate() + ((11 - todaysDate.getMonth()) * 30);
                    }
                    else {
                        daysLeft = ((deadlineDay.getMonth()) * 30) + deadlineDay.getDate() + todaysDate.getDate() + ((11 - todaysDate.getMonth()) * 30);
                    }
                }
                else {
                    daysLeft += 365 - (((deadlineDay.getMonth()) + 1) * 30) + deadlineDay.getDate();
                }

                $("#DeadlineDate_Lbl").text(convertDateAndTime(deadlineDay, false, false));
                $("#DeadlineDaysLeft_Lbl").text(daysLeft);
            }

            if (response.isLiked) {
                $("#AlreadyLiked_Box").removeClass("d-none");
                $("#NotLiked_Box").addClass("d-none");
            }
            else {
                $("#AlreadyLiked_Box").addClass("d-none");
                $("#NotLiked_Box").removeClass("d-none");
            }

            $("#Preview_PublishedDate").text(convertDateAndTime(response.result.createdAt, true, true));
            $("#Preview_LastUpdatedDate").text(convertDateAndTime(response.result.lastUpdatedAt, true, true));
            $("#Preview_LikesCnt").text(response.likesCount.toLocaleString());

            if (response.result.youtubeLink != null) {
                $("#YT_VideoPlayer_MainBox").removeClass("d-none");
                $("#YT_VideoPlayer").attr("src", youtubeLinkCorrector(response.result.youtubeLink));
                $("#YT_VideoLink_Lbl").html(" <i class='fas fa-link'></i> Link: https://www.youtube.com/watch?v=" + response.result.youtubeLink);
                $("#YT_VideoLink_Btn").attr("href", "https://www.youtube.com/watch?v=" + response.result.youtubeLink);
            }
            else {
                $("#YT_VideoPlayer_MainBox").addClass("d-none");
                $("#YT_VideoPlayer").attr("src", null);
                $("#YT_VideoLink_Lbl").html(" <i class='fas fa-link'></i> Link: Undefined");
                $("#YT_VideoLink_Btn").attr("href", "#");
            }

            if (response.result.donationLink != null) {
                $("#InvestToProject_Box").removeClass("d-none");
                $("#Preview_DonationLink_Btn").attr("href", response.result.donationLink);
            }
            else {
                $("#InvestToProject_Box").addClass("d-none");
            }

            if (response.result.priceChangeAnnotation != null) {
                $("#Preview_TagPriceAnnotation").removeClass("d-none");
                $("#Preview_TagPriceAnnotation").html(" <i class='fas fa-quote-right text-primary'></i> " + response.result.priceChangeAnnotation);
            }
            if (response.result.pastTargetPrice != 0) {
                $("#Preview_PrevTagPrice").removeClass("d-none");
                $("#Preview_TagPriceAnnotationSeparator").removeClass("d-none");
                let percentageChange = parseInt(response.result.targetPrice) / parseInt(response.result.pastTargetPrice) * 100;
                if (percentageChange < 100) {
                    percentageChange = (100 - percentageChange) * -1;
                    $("#Preview_PrevTagPrice").addClass("text-danger");
                    $("#Preview_PrevTagPrice").html("<span class='text-decoration-line-through'>" + response.result.pastTargetPrice.toLocaleString("en-US", { style: "currency", currency: "USD" }) + "</span>, " + percentageChange.toLocaleString() + "%");
                }
                else {
                    percentageChange = (100 - percentageChange) * -1;
                    $("#Preview_PrevTagPrice").addClass("text-success");
                    $("#Preview_PrevTagPrice").html("<span class='text-decoration-line-through'>" + response.result.pastTargetPrice.toLocaleString("en-US", { style: "currency", currency: "USD" }) + "</span>, +" + percentageChange.toLocaleString() + "%");
                }
            }
            else {
                $("#Preview_PrevTagPrice").addClass("d-none");
            }

            $("#Preview_LongDescription").text(fullText);
            let value = textDecoder("Preview_LongDescription", true, null);
            $("#Preview_LongDescription").html(value);

            if (response.result.targetPrice != 0) {
                $("#InverToProject_Box").fadeIn(300);
                $("#Preview_TagPrice").text(parseInt(response.result.targetPrice).toLocaleString("en-US", { style: "currency", currency: "USD" }));
            }
            else {
                $("#InverToProject_Box").fadeOut(300);
                $("#Preview_TagPrice").text("No target price");
            }

            if (response.result.link1 != null) {
                $("#Preview_LinkBtn1").attr("href", response.result.link1);
                $("#Preview_LinkBtn1").removeClass("disabled");
                $("#Preview_LinkBtn1").html(' <i class="fas fa-external-link-alt"></i> Click To Relocate');
                $("#Preview_LinkText1").text(response.result.link1);
            }
            else {
                $("#Preview_LinkBtn1").attr("href", "#");
                $("#Preview_LinkBtn1").addClass("disabled");
                $("#Preview_LinkBtn1").html(' <i class="fas fa-external-link-alt"></i> No Link');
                $("#Preview_LinkText1").text("No external first link");
            }
            if (response.result.link2 != null) {
                $("#Preview_LinkBtn2").attr("href", response.result.link2);
                $("#Preview_LinkBtn2").removeClass("disabled");
                $("#Preview_LinkBtn2").html(' <i class="fas fa-external-link-alt"></i> Click To Relocate');
                $("#Preview_LinkText2").text(response.result.link2);
            }
            else {
                $("#Preview_LinkBtn2").attr("href", "#");
                $("#Preview_LinkBtn2").addClass("disabled");
                $("#Preview_LinkBtn2").html(' <i class="fas fa-external-link-alt"></i> No Link');
                $("#Preview_LinkText2").text("No external second link");
            }
            animatedClose(false, "SettingsOfProject_Container");
            if (fullWidth < 717) {
                smallBarAnimatedOpenAndClose(true);
                animatedOpen(false, "ProjectView_Container", true, false);
            }
            else {
                smallBarAnimatedOpenAndClose(false);
                animatedOpen(false, "ProjectView_Container", true, true);
            }
        }
        else {
            openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3);
        }
    });
});

$("#LikeTheProject_Form").on("submit", function (event) {
    event.preventDefault();
    let url = $(this).attr("action");
    let data = $(this).serialize();

    $.post(url, data, function (response) {
        if (response.success) {
            $("#NotLiked_Box").addClass("d-none");
            $("#AlreadyLiked_Box").removeClass("d-none");
            setTimeout(function () {
                $("#RemoveLikeSbmt_Btn").html(' <i class="far fa-grin-hearts text-primary"></i><br/>Liked');
                $("#RemoveLikeSbmt_Btn").css("transform", "scale(1.25)");
            }, 75);
            setTimeout(function () {
                $("#RemoveLikeSbmt_Btn").html(' <i class="fas fa-heart text-primary"></i><br/>Liked');
                $("#RemoveLikeSbmt_Btn").css("transform", "scale(1)");
            }, 625);
        }
        else {
            openModal(response.alert, "<i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.25);
        }
    });
});
$("#RemoveFromLiked_Form").on("submit", function (event) {
    event.preventDefault();
    let url = $(this).attr("action");
    let data = $(this).serialize();

    $.post(url, data, function (response) {
        if (response.success) {
            let curUrl = document.location.href;
            if (curUrl.toLowerCase().includes("/liked")) {
                let currentCount = $("#LikedProjectsCount_Span").text();
                currentCount = parseInt(currentCount);
                currentCount--;

                $("#LikedProjectsCount_Span").text(currentCount);
                if (currentCount > 0) slideToLeftAnimation("LikedProject_Box-" + response.projectId);
                else {
                    $("#HaveLikedProjects_Container").fadeOut(300);
                    setTimeout(function () {
                        $("#NoLikedProjects_Container").fadeIn(150);
                    }, 400);
                }
                openModal("Selected project has been successfully removed from your liked list", " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.5);
            }
            else {
                $("#AlreadyLiked_Box").addClass("d-none");
                $("#NotLiked_Box").removeClass("d-none");
                setTimeout(function () {
                    $("#LikeSbmt_Btn").html(' <i class="fas fa-heart-broken text-danger"></i><br/>Like');
                    $("#LikeSbmt_Btn").css("transform", "scale(1.25)");
                }, 75);
                setTimeout(function () {
                    $("#LikeSbmt_Btn").css("transform", "scale(1)");
                }, 575);
                setTimeout(function () {
                    $("#LikeSbmt_Btn").html(' <i class="far fa-heart"></i><br/>Like');
                }, 4000);
            }
        }
        else {
            openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.25);
        }
    });
});
$("#RemoveAllLiked_Form").on("submit", function (event) {
    event.preventDefault();
    let url = $(this).attr("action");
    let data = $(this).serialize();

    $.post(url, data, function (response) {
        if (response.success) {
            let div = $("<div class='box-container mt-4 p-2 text-center' id='LikedProjectsRemove_Box'></div>");
            let icon = $("<h1 class='display-4 text-primary'> <i class='fas fa-check'></i> </h1>");
            let title = $("<h3 class='h3 safe-font'>Projects Removed</h3>");
            let text = $("<p class='card-text text-muted'>All liked projects have been successfully removed</h3>");

            div.append(icon);
            div.append(title);
            div.append(text);
            $("#HaveLikedProjects_Container").fadeOut(300);
            $("#LikedProjectsCount_Span").text("0");
            setTimeout(function () {
                $("#HaveLikedProjects_Container").empty();
                $("#HaveLikedProjects_Container").append(div);
                $("#HaveLikedProjects_Container").fadeIn(150);
            }, 400);
            setTimeout(function () {
                slideToLeftAnimation("LikedProjectsRemove_Box");
            }, 2000);
            setTimeout(function () {
                $("#LikedProjectsRemove_Box").fadeOut(350);
            }, 2400);
            setTimeout(function () {
                $("#NoLikedProjects_Container").fadeIn(0);
            }, 2700);
        }
        else {
            openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.25);
        }
    });
});

$(document).on("click", ".subscribe-for-user", function (event) {
    let subscriberId = $("#USI_Id_Val").val();
    if (subscriberId) {
        $("#SubOption_UserId_Val").val(subscriberId);
        let url = $("#SubscribtionOptionWithoutKnowing_Form").attr("action");
        let data = $("#SubscribtionOptionWithoutKnowing_Form").serialize();
        $.post(url, data, function (response) {
            if (response.success) {
                let subsCount = $("#USI_SubscribersCount_Val").val();
                if (response.result) {
                    subsCount++;
                    openModal(response.alert, "Done", null, 2, null, null, null, 4, " <i class='fas fa-bell text-primary bell-icon'></i> ");
                }
                else {
                    subsCount--;
                    openModal(response.alert, "Done", null, 2, null, null, null, 4, " <i class='fas fa-times-circle circular-icons text-danger'></i> ");
                }
                $("#USI_SubscribersCount_Val").val(subsCount);
                $("#USI_SubscribersInfo_Btn").html(" <i class='fas fa-bell'></i> " + subsCount + " subscriber(s)");
            }
            else {
                openModal(response.alert, "Done", null, 2, null, null, null, 3.5);
            }
        });
    }
});
$("#SubscribtionOption_Form").on("submit", function (event) {
    event.preventDefault();
    let url = $(this).attr("action");
    let data = $(this).serialize();

    $.post(url, data, function (response) {
        let currentCount = $("#SubscribersCount_Lbl").text();
        if (parseInt(currentCount)) currentCount = parseInt(currentCount);
        else currentCount = 0;

        if (response.success) {
            let curUrl = document.location.href;
            $("#IsSubscribed_Val").val(response.result);
            if (response.result) {
                currentCount++;
                if (!curUrl.toLowerCase().includes("/user/info")) $("#SubscribtionForm_Btn").html(' <i class="fas fa-check-double text-primary"></i> <br/>Subscribed');
                else $("#SubscribtionForm_Btn").html(' <i class="fas fa-bell bell-icon text-primary"></i> ');
                openModal(response.alert, "Done", null, 2, null, null, null, 4, " <i class='fas fa-bell text-primary bell-icon'></i> ");
            }
            else {
                currentCount--;
                if (!curUrl.toLowerCase().includes("/user/info")) $("#SubscribtionForm_Btn").html(' <i class="far fa-bell"></i> <br />Subscribe');
                else $("#SubscribtionForm_Btn").html(' <i class="far fa-bell"></i> ');
                openModal(response.alert, "Done", null, 2, null, null, null, 4, " <i class='fas fa-times-circle circular-icons text-danger'></i> ");
            }
            $("#SubscribersCount_Lbl").text(currentCount);
        }
        else openModal(response.alert, "Done", null, 2, null, null, null, 3.5);
    });
});

$("#LockTheProject_Form").on("submit", function (event) {
    event.preventDefault();
    let url = $(this).attr("action");
    let data = $(this).serialize();

    $.post(url, data, function (response) {
        if (response.success) {
            animatedClose(false, "SettingsOfProject_Container");
            openModal(response.alert, "Got It!", null, 2, null, null, null, 3.75, "<i class='fas fa-lock text-danger'></i>");
            setTimeout(function () {
                $("#LockProject_Box").addClass("d-none");
                $("#UnlockProject_Box").removeClass("d-none");
            }, 300);
            $("#ProjectLockInfo-" + response.id).html("<small class='card-text text-muted'> <i class='fas fa-lock text-danger'></i> Lock Info: Locked</small>");
            $("#ProjectIsLocked-" + response.id).val(true);
        }
        else {
            openModal(response.alert, "<i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.25);
        }
    });
});
$("#UnlockTheProject_Form").on("submit", function (event) {
    event.preventDefault();
    let url = $(this).attr("action");
    let data = $(this).serialize();

    $.post(url, data, function (response) {
        if (response.success) {
            animatedClose(false, "SettingsOfProject_Container");
            openModal(response.alert, "Got It!", null, 2, null, null, null, 3.75, "<i class='fas fa-lock-open text-primary'></i>");
            setTimeout(function () {
                $("#UnlockProject_Box").addClass("d-none");
                $("#LockProject_Box").removeClass("d-none");
            }, 300);
            $("#ProjectLockInfo-" + response.id).html("<small class='card-text text-muted'> <i class='fas fa-lock text-primary'></i> Lock Info: Unlocked</small>");
            $("#ProjectIsLocked-" + response.id).val(false);
        }
        else {
            openModal(response.alert, "<i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.25);
        }
    });
});

$("#PinTheProject_Form").on("submit", function (event) {
    event.preventDefault();

    let url = $(this).attr("action");
    let data = $(this).serialize();
    $.post(url, data, function (response) {
        if (response.success) {
            $("#IsPinnedIcon-" + response.result).fadeIn(350);
            $("#ProjectIsPinned-" + response.result).val(true);
            $("#UTP_ProjectId_Val").val(response.result);
            animatedClose(false, "SettingsOfProject_Container");
            $("#PinTheProject_Box").addClass("d-none");
            $("#UnpinTheProject_Box").removeClass("d-none");
            animatedOpen(false, "SettingsOfProject_Container", false, false);
            setTimeout(function () {
                openModal(response.alert, "Done", null, 2, null, null, null, 3.5, "<i class='fas fa-thumbtack thumbtack-icon fa-rotate-90 text-primary'></i> ");
            }, 450);
        }
        else openModal(response.alert, "Done", null, 2, null, null, null, 3.5, "<i class='fas fa-times-circle text-danger'></i>");
    });
});
$("#UnpinTheProject_Form").on("submit", function (event) {
    event.preventDefault();

    let url = $(this).attr("action");
    let data = $(this).serialize();
    $.post(url, data, function (response) {
        if (response.success) {
            $("#IsPinnedIcon-" + response.result).fadeOut(350);
            $("#ProjectIsPinned-" + response.result).val(false);
            $("#PTP_ProjectId_Val").val(response.result);
            animatedClose(false, "SettingsOfProject_Container");
            $("#PinTheProject_Box").removeClass("d-none");
            $("#UnpinTheProject_Box").addClass("d-none");
            animatedOpen(false, "SettingsOfProject_Container", false, false);

            setTimeout(function () {
                openModal(response.alert, "Done", null, 2, null, null, null, 3.5, "<i class='fas fa-unlink text-danger'></i>");
            }, 450);
        }
        else openModal(response.alert, "Done", null, 2, null, null, null, 3.5, "<i class='fas fa-times-circle text-danger'></i>");
    });
});

$("#SendMessage_Form").on("submit", function (event) {
    event.preventDefault();
    let url = $(this).attr("action");
    let data = $(this).serialize();

    $.post(url, data, function (response) {
        if (response.success) {
            $("#MsgSendSbmt_Btn").css("transform", "rotate(360deg)");
            $("#MsgSendSbmt_Btn").html(' <i class="fas fa-check"></i> ');
            $("#MsgSendSbmt_Btn").attr("disabled", true);
            $("#Text").attr("disabled", true);
            setTimeout(function () {
                $("#Text").attr("disabled", false);
                $("#Text").val("");
                $("#MsgSendSbmt_Btn").attr("disabled", false);
                $("#MsgSendSbmt_Btn").css("transform", "rotate(-360deg)");
                $("#MsgSendSbmt_Btn").html(" <i class='fas fa-arrow-up'></i> ");
            }, 2500);
        }
        else {
            $("#MsgSendSbmt_Btn").css("transform", "rotate(360deg)");
            $("#MsgSendSbmt_Btn").html(' <i class="fas fa-times"></i> ');
            $("#MsgSendSbmt_Btn").attr("disabled", true);
            $("#MsgSendSbmt_Btn").removeClass("bg-primary");
            $("#MsgSendSbmt_Btn").addClass("bg-danger");
            $("#Text").attr("disabled", true);
            setTimeout(function () {
                $("#MsgSendSbmt_Btn").removeClass("bg-danger");
                $("#MsgSendSbmt_Btn").addClass("bg-primary");
                $("#MsgSendSbmt_Btn").css("transform", "rotate(-360deg)");
                $("#Text").attr("disabled", false);
                $("#Text").val("");
                $("#MsgSendSbmt_Btn").attr("disabled", false);
                $("#MsgSendSbmt_Btn").html(" <i class='fas fa-arrow-up'></i> ");
            }, 3250);
            openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.5);
        }
    });
});

$("#SendReply_Form").on("submit", function (event) {
    event.preventDefault();
    let url = $(this).attr("action");
    let data = $(this).serialize();

    $.post(url, data, function (response) {
        if (response.success) {
            let count = $("#RTC_Count").val();

            $("#RTCC_TextValue").val("");
            $("#RTCSbmt_Btn").attr("disabled", true);
            $("#RTCSbmt_Btn").html(" <i class='fas fa-check'></i> Sent");
            setTimeout(function () {
                $("#RTCSbmt_Btn").html(" <i class='fas fa-arrow-up'></i> ");
                $("#RTCSbmt_Btn").attr("disabled", false);
            }, 2750);

            let div = $("<div class='box-container mt-2 p-2 border-top border-straight'></div>");
            let title = $("<h6 class='h6'></h6>");
            let text = $("<small class='card-text white-space-on'></small>");
            let sentAt = $("<small class='card-text text-muted float-end ms-1'></small>");

            title.html("You");
            text.html(response.text);
            sentAt.text("Few seconds ago");

            div.append(sentAt);
            div.append(title);
            div.append(text);
            if (count > 0) {
                $("#RepliesList_Box").prepend(div);
            }
            else {
                $("#RepliesList_Box").empty();
                $("#RepliesList_Box").append(div);
            }
        }
        else {
            $("#RTCC_TextValue").val("");
            openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.25);
        }
    });
});

$("#SendComment_Form").on("submit", function (event) {
    event.preventDefault();
    let url = $(this).attr("action");
    let data = $(this).serialize();

    $.post(url, data, function (response) {
        if (response.success) {
            let currentCount = $("#CommentsCount_Val").val();
            currentCount++;

            let textVal = $("#SendComment_Text").val();
            let div = $("<div class='box-container mt-2 p-2 border-top'></div>");
            let title = $("<h6 class='h6'></h6>");
            let text = $("<small class='card-text'></small>");
            let sentAt = $("<small class='card-text text-muted float-end ms-1'></small>");

            title.text("You");
            text.html(textVal);
            sentAt.text(convertDateAndTime(response.date, true, true));

            div.append(sentAt);
            div.append(title);
            div.append(text);

            $("#CommentsCount_Val").val(currentCount);
            $("#CommentsCount_Span").html(currentCount.toLocaleString());
            $("#CommentsCountMain_Span").html(currentCount.toLocaleString());
            if (currentCount <= 1) {
                $("#CommentsMain_Box").empty();
                $("#CommentsMain_Box").append(div);
            }
            else {
                $("#CommentsMain_Box").prepend(div);
            }

            $("#SendCommentSbmt_Btn").html(" <i class='fas fa-check'></i> ");
            $("#SendCommentSbmt_Btn").attr("disabled", true);
            $("#SendComment_Text").attr("disabled", true);
            $("#SendComment_Text").attr("rows", 1);
            $("#SendComment_Text").val("");
            setTimeout(function () {
                $("#SendCommentSbmt_Btn").html(" <i class='fas fa-arrow-up'></i> ");
                $("#SendCommentSbmt_Btn").attr("disabled", false);
                $("#SendComment_Text").attr("disabled", false);
            }, 4500);
        }
        else {
            $("#SendComment_Text").attr("rows", 1);
            $("#SendComment_Text").val("");
            openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.5);
        }
    });
});

$("#GetComments_Form").on("submit", function (event) {
    event.preventDefault();
    let url = $(this).attr("action");
    let data = $(this).serialize();
    $.get(url, data, function (response) {
        if (response.success) {
            $("#CommentsMain_Box").empty();
            if (response.count > 0) {
                $.each(response.result, function (index) {
                    let div = $("<div class='box-container mt-2 p-2 border-top border-straight'></div>");
                    let title = $("<h6 class='h6'></h6>");
                    let text = $("<small class='card-text white-space-on'></small>");
                    let sentAt = $("<small class='card-text text-muted float-end ms-1'></small>");
                    let separatorOne = $("<div class='mt-2'></div>");
                    let getAndSendRepliesBtn = $("<button type='button' class='btn btn-text btn-sm send-reply-to-comment'></button>")

                    title.text(response.result[index].senderName);
                    text.html(response.result[index].text);
                    sentAt.text(convertDateAndTime(response.result[index].sentAt, true, true));
                    title.attr("id", "GetAndSendReplyTitle-" + response.result[index].id);
                    text.attr("id", "GetAndSendReplyText-" + response.result[index].id);
                    getAndSendRepliesBtn.attr("id", "GetAndSendReply-" + response.result[index].id);
                    getAndSendRepliesBtn.html(" <i class='fas fa-reply'></i> Replies (" + response.result[index].repliesCount.toLocaleString() + ")");

                    div.append(sentAt);
                    div.append(title);
                    div.append(text);
                    div.append(separatorOne);
                    div.append(getAndSendRepliesBtn);

                    $("#CommentsMain_Box").append(div);
                    $("#CommentsCount_Val").val(response.count);
                    $("#CommentsCount_Span").text(response.count.toLocaleString());
                });
            }
            else {
                let div = $("<div class='box-container mt-2 p-3 text-center'></div>");
                let title = $("<h3 class='h3 text-primary'></h3>");
                let titleText = $("<h6 class='h6 safe-font'></h6>");
                let text = $("<small class='card-text text-muted'></small>");

                title.html(" <i class='fas fa-comment-slash'></i> ");
                titleText.text("No recently sent comments");
                text.text("Try to look up for them later");

                div.append(title);
                div.append(titleText);
                div.append(text);

                $("#CommentsMain_Box").append(div);
                $("#CommentsCount_Val").val(0);
                $("#CommentsCount_Span").text(" out of comments");
            }
            smallBarAnimatedOpenAndClose(true);
            animatedOpen(false, "Comments_Container", false, false);
        }
        else {
            openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.5);
        }
    });
});

$("#SearchEverywhere_Keyword").on("change", function (event) {
    event.preventDefault();
    animatedOpen(false, "Waiter_Container", true, true);

    let url = $("#SearchEverywhere_Form").attr("action");
    let data = $("#SearchEverywhere_Form").serialize();
    $.get(url, data, function (response) {
        if (response.success) {
            setTimeout(function () {
                animatedOpen(false, "Preload_Container", true, true);

                $("#UserResults_Box").empty();
                $("#ProjectResults_Box").empty();
                if (response.projectsCount > 0) {
                    $.each(response.projectResult, function (index) {
                        let div = $("<div class='box-container bg-light p-2 me-2 d-inline-block mw-responsive'></div>");
                        let name = $("<h3 class='h3 safe-font text-primary get-project-short-info'></h3>");
                        let targetPrice = $("<small class='card-text text-primary safe-font fs-6'></small>");
                        let userName = $("<small class='card-text'></small>");
                        let prevTargetPrice = $("<small class='card-text text-muted text-decoration-line-through'></small>");
                        let separatorOne = $("<div class='mt-3'></div>");
                        let separatorZero = $("<div></div>");
                        let separatorTwo = $("<div class='mt-2'></div>");
                        let row = $("<div class='row'></div>");
                        let row2 = $("<div class='row mt-2'></div>");
                        let col1 = $("<div class='col'></div>");
                        let col1Div = $("<div class='box-container bg-white p-2 text-center'></div>");
                        let col1Title = $("<p class='card-text fw-500'>Created</p>");
                        let col1Small1 = $("<small class='card-text text-muted safe-font'></small>");

                        let col2 = $("<div class='col'></div>");
                        let col2Div = $("<div class='box-container bg-white p-2 text-center'></div>");
                        let col2Title = $("<p class='card-text fw-500'>Modified</p>");
                        let col2Small1 = $("<small class='card-text text-muted safe-font'></small>");

                        let col1Row2Col = $("<div class='col'></div>");
                        let col1Row2Div = $("<div class='box-container bg-white p-2 text-center'></div>");
                        let col1Row2Title = $("<p class='card-text fw-500'>Views</p>");
                        let col1Row2Small1 = $("<small class='card-text text-muted safe-font'>0</p>");

                        name.html(response.projectResult[index].name);
                        userName.text(response.projectResult[index].userName);
                        name.attr("id", "SGPFI-" + response.projectResult[index].id);

                        if (response.projectResult[index].targetPrice != 0) {
                            targetPrice.html(response.projectResult[index].targetPrice.toLocaleString("en-US", { style: "currency", currency: "USD" }));
                            if (response.projectResult[index].pastTargetPrice != 0) {
                                prevTargetPrice.html(response.projectResult[index].pastTargetPrice.toLocaleString("en-US", { style: "currency", currency: "USD" }));
                            }
                            else {
                                prevTargetPrice.text("No Prev. Price");
                                prevTargetPrice.removeClass("text-decoration-line-through");
                            }
                        }
                        else {
                            targetPrice.html("No Target Price");
                            targetPrice.addClass("text-muted");
                            targetPrice.removeClass("text-primary");
                            prevTargetPrice.html("No Prev. Price");
                            prevTargetPrice.removeClass("text-decoration-line-through");
                        }

                        col1Small1.text(convertDateAndTime(response.projectResult[index].createdAt, false, true));
                        col2Small1.text(convertDateAndTime(response.projectResult[index].lastUpdatedAt, false, true));

                        col1Div.append(col1Title);
                        col1Div.append(col1Small1);
                        col1.append(col1Div);

                        col2Div.append(col2Title);
                        col2Div.append(col2Small1);
                        col2.append(col2Div);

                        row.append(col1);
                        row.append(col2);

                        col1Row2Small1.text(response.projectResult[index].views.toLocaleString());
                        col1Row2Div.append(col1Row2Title);
                        col1Row2Div.append(col1Row2Small1);
                        col1Row2Col.append(col1Row2Div);
                        row2.append(col1Row2Col);

                        div.append(name);
                        div.append(targetPrice);
                        div.append(separatorZero);
                        div.append(prevTargetPrice);
                        div.append(separatorOne);
                        div.append(userName);
                        div.append(separatorTwo);
                        div.append(row);
                        div.append(row2);

                        $("#ProjectResults_Box").append(div);
                    });
                }
                else {
                    let div = $("<div class='text-center'></div>");
                    let title = $("<h2 class='h2 safe-font'>No Project Found</h3>'");
                    let text = $("<small class='text-muted'>There's no project found with similar name, description and target price</small>");
                    div.append(title);
                    div.append(text);
                    $("#ProjectResults_Box").append(div);
                    $("#ProjectResults_Box").removeClass("horizontal-scrolled-container");
                    $("#ProjectResults_Box").addClass("box-container");
                }

                if (response.usersCount > 0) {
                    $.each(response.userResult, function (index) {
                        let div = $("<div class='box-container bg-light p-2 me-2 d-inline-block mw-responsive'></div>");
                        let name = $("<h4 class='h4 safe-font text-primary get-user-short-info'></h4>");
                        let searchName = $("<small class='card-text text-muted fw-500'></small>");
                        let separatorOne = $("<div class='mt-3'></div>");
                        let separatorTwo = $("<div class='mt-3'></div>");
                        let row = $("<div class='row'></div>");
                        let col1 = $("<div class='col'></div>");
                        let col1Div = $("<div class='box-container bg-white p-2 text-center'></div>");
                        let col1Title = $("<p class='card-text fw-500'>Projects</p>");
                        let col1Small1 = $("<small class='card-text'></small>");
                        let col1SmallSeparator = $("<div></div>");
                        let col1Small2 = $("<p class='card-text text-muted'></small>");

                        let col2 = $("<div class='col'></div>");
                        let col2Div = $("<div class='box-container bg-white p-2 text-center'></div>");
                        let col2Title = $("<p class='card-text fw-500'>Status</p>");
                        let col2Small1 = $("<small class='card-text'></small>");
                        let col2SmallSeparator = $("<div></div>");
                        let col2Small2 = $("<p class='card-text text-muted'></small>");

                        let goToPageBtn = $("<a href='#' class='btn btn-text btn-sm me-3'> <i class='fas fa-info'></i> Go To Page</button>");
                        let showCardBtn = $("<button type='button' class='btn btn-text btn-sm'> <i class='far fa-address-card'></i> Show Card</button>");

                        goToPageBtn.attr("href", "/User/Info/" + response.userResult[index].searchName);

                        if (response.userResult[index].countryFullName != null) name.html(response.userResult[index].pseudoName + "<small><span class='badge bg-white text-dark warning-badge fw-normal ms-5'> <i class='fas fa-globe-americas text-primary'></i> " + response.userResult[index].countryFullName + "</span></small>");
                        else name.html(response.userResult[index].pseudoName + "<small><span class='badge bg-white text-dark warning-badge fw-normal ms-5'> <i class='fas fa-globe-americas text-primary'></i> Country Unknown</span></small>");

                        name.attr("id", "SUSI_Val-" + response.userResult[index].id);
                        searchName.text("@" + response.userResult[index].searchName);
                        if (response.userResult[index].projectsCount != 0) {
                            col1Small1.text(response.userResult[index].projectsCount);
                            col1Small2.text("created");
                        }
                        else {
                            col1Small1.text("No Created Projects");
                            col1Small2.text("or they may be closed");
                        }

                        if (response.userResult[index].isCompany) {
                            col2Small1.text("This's a company");
                            col2Small2.text("business account");
                        }
                        else {
                            col2Small1.text("He/She is an user");
                            col2Small2.text("individual account");
                        }

                        col1Div.append(col1Title);
                        col1Div.append(col1Small1);
                        col1Div.append(col1SmallSeparator);
                        col1Div.append(col1Small2);
                        col1.append(col1Div);

                        col2Div.append(col2Title);
                        col2Div.append(col2Small1);
                        col2Div.append(col2SmallSeparator);
                        col2Div.append(col2Small2);
                        col2.append(col2Div);

                        row.append(col1);
                        row.append(col2);

                        div.append(name);
                        div.append(searchName);
                        div.append(separatorOne);
                        div.append(row);
                        div.append(separatorTwo);
                        div.append(goToPageBtn);
                        div.append(showCardBtn);

                        $("#UserResults_Box").append(div);
                        $("#UserResults_Box").addClass("horizontal-scrolled-container");
                        $("#UserResults_Box").removeClass("box-container");
                    });
                }
                else {
                    let div = $("<div class='text-center'></div>");
                    let title = $("<h2 class='h2 safe-font'>No User Found</h3>'");
                    let text = $("<small class='text-muted'>There's no user found with similar name, email, description or country in his badge</small>");
                    div.append(title);
                    div.append(text);
                    $("#UserResults_Box").append(div);
                    $("#UserResults_Box").removeClass("horizontal-scrolled-container");
                    $("#UserResults_Box").addClass("box-container");
                }
                $("#SearchResults_Box").fadeIn(0);
            }, 750);
        }
        else {
            animatedOpen(false, "Preload_Container", true, true);
            openModal("An error occured while trying to search. Please, check your internet connection and then try again", " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.5);
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
            $("#SB_C-Title").html(" <i class='fas fa-times btn-close-liquid-container'></i>  Notifications ∙ " + response.count.toLocaleString());
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

                    dateTime.html(convertDateAndTime(response.notifications[index].sentAt, false, true));
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
                let div = $("<div class='box-container p-2 mt-1 mb-1 text-center bg-transparent'></div>");
                let anima = $("<h3 class='display-4'> <i class='far fa-bell-slash'></i> </h3>");
                let title = $("<h5 class='h5 safe-font mt-2'>No notifications</h5>");
                let text = $("<small class='card-text'>You haven't received any notifications yet</small>");

                div.append(anima);
                div.append(title);
                div.append(text);
                $("#SB_C-Body").append(div);
            }

            bubbleAnimation("NotificationsLiquid_Container", true);
        }
        else {
            openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.5);
        }
    });
});
$("#GetMessages_Btn").on("click", function (event) {
    event.preventDefault();
    let url = $("#GetAllMessages_Form").attr("action");
    let data = $("#GetAllMessages_Form").serialize();
    $.get(url, data, function (response) {
        if (response.success) {
            $("#SB_C-Body").empty();
            if (response.count > 0) {
                $("#SB_C-Title").html(" <i class='fas fa-times'></i> Received Messages ∙ " + response.count.toLocaleString());

                $.each(response.result, function (index) {
                    let div = $("<div class='box-container p-2 pe-3 mt-1 mb-1 bordered-container'></div>");
                    let senderName = $("<h6 class='h6 text-primary safe-font'></h6>");
                    let text = $("<p class='card-text'></p>");
                    let dateAndTime = $("<small class='card-text text-muted ms-1 float-end'></small>");
                    let isChecked = $("<small class='card-text float-end ms-2 is-checked-icon'></small>");
                    let separatorOne = $("<div class='mt-3'></div>");

                    let dropdown = $("<div class='dropdown'></div>");
                    let dropdownOpenBtn = $("<button type='button' class='btn btn-sm btn-outline-primary btn-standard-with-no-colour border-0 float-end ms-1' data-bs-toggle='dropdown' aria-expanded='false'> <i class='fas fa-ellipsis-h'></i> </button>");
                    let dropdownUl = $("<ul class='dropdown-menu shadow-sm p-1'></div>");
                    let dropdownLi0 = $("<li></li>");
                    let dropdownLi1 = $("<li></li>");
                    let dropdownLi2 = $("<li></li>");
                    let dropdownLi3 = $("<li></li>");
                    //let removeBtn = $("<button type='button' class='btn btn-sm dropdown-item text-danger select-to-remove-the-msg'> <i class='fas fa-times-circle'></i> Remove</button>")
                    let markAsReadBtn = $("<button type='button' class='dropdown-item select-to-mark mb-1'> <i class='fas fa-check-double'></i> Mark as Read</button>");
                    let selectToReply = $("<button type='button' class='dropdown-item select-to-reply mb-1'> <i class='fas fa-reply'></i> Reply</button>");
                    let msgInfoBtn = $("<button type='button' class='dropdown-item select-for-info mb-1'> <i class='fas fa-info-circle'></i> About This Message</button>");
                    let msgHideBtn = $("<button type='button' class='dropdown-item select-to-hide'> <i class='fas fa-minus-circle text-danger'></i> Hide</button>");

                    text.html(response.result[index].text);
                    dateAndTime.text(convertDateAndTime(response.result[index].sentAt), true, true);
                    if (response.result[index].isChecked) {
                        isChecked.html(" <i class='fas fa-check-double text-primary'></i> ");
                        markAsReadBtn.fadeOut(0);
                    }
                    else isChecked.html(" <i class='fas fa-check text-muted'></i> ");
                    senderName.text(response.result[index].senderName);

                    div.attr("id", "AllMessages-" + response.result[index].id);
                    senderName.attr("id", "SentFrom-" + response.result[index].id);
                    text.attr("id", "AllMessagesText-" + response.result[index].id);
                    dateAndTime.attr("id", "AllMessagesDate-" + response.result[index].id);
                    isChecked.attr("id", "AllMessagesIsChecked-" + response.result[index].id);
                    markAsReadBtn.attr("id", "MessageMarkAsRead-" + response.result[index].id);
                    msgInfoBtn.attr("id", "MessageInfoBtn-" + response.result[index].id);
                    selectToReply.attr("id", "MessageReplyToBtn-" + response.result[index].id);
                    msgHideBtn.attr("id", "MessageToHideBtn-" + response.result[index].id);

                    dropdownLi0.append(msgInfoBtn);
                    dropdownLi1.append(markAsReadBtn);
                    dropdownLi2.append(selectToReply);
                    dropdownLi3.append(msgHideBtn);
                    dropdownUl.append(dropdownLi0);
                    dropdownUl.append(dropdownLi1);
                    dropdownUl.append(dropdownLi2);
                    dropdownUl.append(dropdownLi3);
                    dropdown.append(dropdownOpenBtn);
                    dropdown.append(dropdownUl);

                    div.append(dropdown);
                    div.append(senderName);
                    div.append(separatorOne);
                    div.append(isChecked);
                    div.append(dateAndTime);
                    div.append(text);
                    $("#SB_C-Body").append(div);
                });
                let checkAll_SbmtBtn = $("<button type='button' class='btn btn-text mt-1 btn-sm select-to-mark' id='MarkAllMessagesAsRead--256'> <i class='fas fa-check-double text-primary'></i> Mark All as Read</button>");
                $("#SB_C-Body").append(checkAll_SbmtBtn);           
            }
            else {
                let div = $("<div class='p-3 text-center'></div>");
                let iconTitle = $("<h1 class='h1'> <i class='fas fa-comment-slash'></i> </h1>");
                let textTitle = $("<h3 class='h3 safe-font'>No Messages</h3>");
                let text = $("<small class='card-text text-muted'>You haven't get any messages. All of them will lately will appear here to edit, check or remove</small>");
                div.append(iconTitle);
                div.append(textTitle);
                div.append(text);
                $("#SB_C-Body").append(div);
                $("#SB_C-Title").html(" <i class='fas fa-times'></i> Received Messages ∙ " + response.count.toLocaleString());
            }
            bubbleAnimation("NotificationsLiquid_Container", true);
        }
        else {
            openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.5);
        }
    });
});

$(document).on("submit", "#RemoveAllSentMessages_Form", function (event) {
    event.preventDefault();
    let url = $(this).attr("action");
    let data = $(this).serialize();
    $.post(url, data, function (response) {
        if (response.success) {
            bubbleAnimation("NotificationsLiquid_Container", false);
            setTimeout(function () {
                $("#SB_C-Title").html(" <i class='fas fa-times'></i> No Sent Messages");
                $("#SB_C-Body").empty();
                let div = $("<div class='p-3 text-center'></div>");
                let iconTitle = $("<h1 class='h1'> <i class='fas fa-comment-slash'></i> </h1>");
                let textTitle = $("<h3 class='h3 safe-font'>No Sent Messages</h3>");
                let text = $("<small class='card-text text-muted'>All sent messages have been successfully removed</small>");
                div.append(iconTitle);
                div.append(textTitle);
                div.append(text);
                $("#SB_C-Body").append(div);
            }, 600);
            setTimeout(function () {
                bubbleAnimation("NotificationsLiquid_Container", true);
            }, 1250);
        }
        else {
            bubbleAnimation("NotificationsLiquid_Container", false);
            setTimeout(function () {
                openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.25);
            }, 500);
        }
    })
});

$("#GetSentMessages_Btn").on("click", function (event) {
    event.preventDefault();
    let url = $("#GetAllSentMessages_Form").attr("action");
    let data = $("#GetAllSentMessages_Form").serialize();
    let userId = $("#PageInfo_UserId").val();
    if (parseInt(userId)) {
        $.get(url, data, function (response) {
            if (response.success) {
                $("#SB_C-Body").empty();
                if (response.count > 0) {
                    $("#SB_C-Title").html(" <i class='fas fa-times'></i> Sent Messages ∙ " + response.count.toLocaleString());

                    $.each(response.result, function (index) {
                        let div = $("<div class='box-container p-2 pe-3 mt-1 mb-1 bordered-container'></div>");
                        let senderName = $("<h6 class='h6 text-primary safe-font'></h6>");
                        let text = $("<p class='card-text'></p>");
                        let dateAndTime = $("<small class='card-text text-muted ms-1 float-end'></small>");
                        let isChecked = $("<small class='card-text float-end ms-2 is-checked-icon'></small>");
                        let separatorOne = $("<div class='mt-3'></div>");

                        let dropdown = $("<div class='dropdown'></div>");
                        let dropdownOpenBtn = $("<button type='button' class='btn btn-sm btn-outline-primary btn-standard-with-no-colour border-0 float-end ms-1' data-bs-toggle='dropdown' aria-expanded='false'> <i class='fas fa-ellipsis-h'></i> </button>");
                        let dropdownUl = $("<ul class='dropdown-menu shadow-sm p-1'></div>");
                        let dropdownLi0 = $("<li></li>");
                        let dropdownLi1 = $("<li></li>");
                        let removeBtn = $("<button type='button' class='btn btn-sm dropdown-item text-danger select-to-remove-the-msg'> <i class='fas fa-times-circle'></i> Remove</button>")
                        let msgInfoBtn = $("<button type='button' class='dropdown-item select-for-info mb-1'> <i class='fas fa-info-circle'></i> About This Message</button>");

                        text.html(response.result[index].text);
                        dateAndTime.text(convertDateAndTime(response.result[index].sentAt), true, true);
                        if (response.result[index].isChecked) {
                            isChecked.html(" <i class='fas fa-check-double text-primary'></i> ");
                        }
                        else isChecked.html(" <i class='fas fa-check text-muted'></i> ");
                        senderName.text("Sent By You");
                        
                        div.attr("id", "AllSentMessages-" + response.result[index].id);
                        senderName.attr("id", "SentFrom-" + response.result[index].id);
                        text.attr("id", "AllMessagesText-" + response.result[index].id);
                        dateAndTime.attr("id", "AllMessagesDate-" + response.result[index].id);
                        isChecked.attr("id", "AllMessagesIsChecked-" + response.result[index].id);
                        removeBtn.attr("id", "MessageRemove-" + response.result[index].id);
                        msgInfoBtn.attr("id", "MessageInfoBtn-" + response.result[index].id);

                        dropdownLi0.append(msgInfoBtn);
                        dropdownLi1.append(removeBtn);
                        dropdownUl.append(dropdownLi0);
                        dropdownUl.append(dropdownLi1);
                        dropdown.append(dropdownOpenBtn);
                        dropdown.append(dropdownUl);

                        div.append(dropdown);
                        div.append(senderName);
                        div.append(separatorOne);
                        div.append(isChecked);
                        div.append(dateAndTime);
                        div.append(text);
                        $("#SB_C-Body").append(div);
                    });
                    let removeAllMsgsForm = $("<div><form method='post' action='/Messages/RemoveSentMessage' id='RemoveAllSentMessages_Form'><input type='hidden' name='Id' value='-256' /> <input type='hidden' name='UserId' value='" + userId + "' /> <button type='submit' class= 'btn btn-text btn-sm' > <i class='fas fa-trash-alt text-danger'></i> Remove All</button ></form ></div>");
                    $("#SB_C-Body").append(removeAllMsgsForm);
                }
                else {
                    let div = $("<div class='p-3 text-center'></div>");
                    let iconTitle = $("<h1 class='h1'> <i class='fas fa-comment-slash'></i> </h1>");
                    let textTitle = $("<h3 class='h3 safe-font'>No Messages</h3>");
                    let text = $("<small class='card-text text-muted'>You haven't get any messages. All of them will lately will appear here to edit, check or remove</small>");
                    div.append(iconTitle);
                    div.append(textTitle);
                    div.append(text);
                    $("#SB_C-Body").append(div);
                    $("#SB_C-Title").html(" <i class='fas fa-times'></i> Sent Messages ∙ " + response.count.toLocaleString());
                }
                bubbleAnimation("NotificationsLiquid_Container", true);
            }
            else {
                openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.5);
            }
        });
    }
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

$("#GetMailOfCreator_Btn").on("click", function () {
    let email = $("#ProjectCreator_Email").val();
    if (email != "" || email != null) {
        let usersMailLink;
        let icon;
        let bgColor;

        let trueMail = email.substring(email.indexOf("@") + 1);
        switch (trueMail) {
            case "gmail.com":
                usersMailLink = "https://mail.google.com";
                icon = "@gmail.com, <i class='fab fa-google'></i> Google";
                bgColor = "bg-primary text-light";
                break;
            case "yahoo.com":
                usersMailLink = "https://mail.yahoo.com/";
                icon = "@yahoo.com, <i class='fab fa-yahoo'></i> Yahoo!";
                bgColor = "bg-deep-purple text-light";
                break;
            case "outlook.com":
                usersMailLink = "https://outlook.live.com/mail/";
                icon = "@outlook.com, <i class='fab fa-microsoft'></i> Microsoft";
                bgColor = "bg-info text-light";
                break;
            case "icloud.com":
                usersMailLink = "https://www.icloud.com/";
                icon = "@icloud.com, <i class='fab fa-apple'></i> Apple";
                bgColor = "bg-light text-dark";
                break;
            case "yandex.ru":
                usersMailLink = "https://360.yandex.com/mail/";
                icon = "@yandex.ru, <i class='fab fa-yandex-international'></i> Yandex";
                bgColor = "bg-warning text-light";
                break;
            case "mail.com":
                usersMailLink = "https://www.mail.com/";
                icon = "@mail.com, <i class='fas fa-envelope-open'></i> Mail.com";
                bgColor = "bg-info text-light";
                break;
            case "aol.com":
                usersMailLink = "https://mail.aol.com/";
                icon = "@aol.com, <i class='fab fa-yahoo'></i> Yahoo!";
                bgColor = "bg-primary text-light";
                break;
            case "mail.ru":
                usersMailLink = "https://mail.ru/";
                icon = "@mail.ru, <i class='fab fa-vk'></i> VK";
                bgColor = "bg-primary text-light";
                break;
            default:
                usersMailLink = "#";
                icon = "Not founded mail service";
                bgColor = "bg-light text-dark";
                break;
        }

        if (usersMailLink != "#") {
            openModal("By clicking the bottom button you're going to be relocated to author's e-mail service web-site or application to send him / her your message. If you don't want do that just ignore and close this widget and write him via our message service. It's much faster and easier! <div class= 'border-top mt-2 pt-2'></div> <small><span class='p-1 " + bgColor + " warning-badge'>" + icon + "</span></small>", " <i class= 'fas fa-times text-danger' ></i> Close", "Send Via " + icon, 2, null, 0, usersMailLink, Infinity);
        }
        else {
            openModal("User's email is hidden or it's an unexpected by us email service application. Anyway, we recommend you to text him via direct messages of our application or relocate manually to this user's mail service web-site/application", "Got It!", null, 2, null, null, null, Infinity);
        }
    }
});

$("#AddOptionSbmt_Btn").on("click", function () {
    let type = $("#AddOption_Type_Val").val();
    let value1 = $("#AddOption_Val1").val();
    let value2 = $("#AddOption_Val2").val();
    let value3 = $("#AddOption_Val3").val();
    let whereAndFrom = $("#AddOption_WhereAndFrom_Val").val();

    if (type != 0) {
        animatedOpen(false, "EditDescription_Container", true, false);
        if (value3 == null) addOptionToText(whereAndFrom, type, value1, value2);
        else addOptionToText(whereAndFrom, type, value1, value3);
        $("#AddOption_Val1").val(null);
        $("#AddOption_Val2").val(null);
        $("#AddOption_Val3").val(null);
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

$(document).on("click", ".get-project-short-info", function (event) {
    let trueId = getTrueId(event.target.id);
    if (trueId != "") {
        $("#GFP_Id_Val").val(trueId);
        $("#GetFullProject_Form").submit();
    }
    else openModal("Unable to get information about selected project. Please, try again later", " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.5);
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

$(document).on("click", ".send-reply-to-comment", function (event) {
    let trueId = getTrueId(event.target.id);
    if (trueId != "") {
        let userName = $("#GetAndSendReplyTitle-" + trueId).text();
        let text = $("#GetAndSendReplyText-" + trueId).text();
        $("#RTC_User").html(userName);
        $("#RTC_Username").html(userName);
        $("#RTC_TextValue").html(text);
        $("#RTC_CommentId_Val").val(trueId);
        $("#GAR_Id_Val").val(trueId);

        let url = $("#GetAllReplies_Form").attr("action");
        let data = $("#GetAllReplies_Form").serialize();
        $.get(url, data, function (response) {
            if (response.success) {
                $("#RepliesList_Box").empty();
                $("#RTC_Count").val(response.count);
                if (response.count > 0) {
                    $.each(response.result, function (index) {
                        let div = $("<div class='box-container mt-2 p-2 border-top border-straight'></div>");
                        let title = $("<h6 class='h6'></h6>");
                        let text = $("<small class='card-text white-space-on'></small>");
                        let sentAt = $("<small class='card-text text-muted float-end ms-1'></small>");

                        title.html(response.result[index].username);
                        text.html(response.result[index].text);
                        sentAt.text(convertDateAndTime(response.result[index].sentAt, true, true));

                        div.append(sentAt);
                        div.append(title);
                        div.append(text);
                        $("#RepliesList_Box").append(div);
                    });
                }
                else {
                    let div = $("<div class='box-container mt-2 p-3 text-center'></div>");
                    let titleText = $("<h4 class='h4 safe-font'></h4>");
                    let text = $("<small class='card-text text-muted'></small>");

                    titleText.text("No Replies");
                    text.text("Try to be the first one who has replied to this comment");
                    div.append(titleText);
                    div.append(text);
                    $("#RepliesList_Box").append(div);
                }
            }
            else {
                let div = $("<div class='box-container mt-2 p-3 text-center'></div>");
                let titleText = $("<h4 class='h4 safe-font'></h4>");
                let text = $("<small class='card-text text-muted'></small>");

                titleText.text("No Replies");
                text.text("Try to be the first one who is going to reply to this comment");
                div.append(titleText);
                div.append(text);
                $("#RepliesList_Box").append(div);
            }
        });

        smallBarAnimatedOpenAndClose(true);
        animatedOpen(false, "ReplyToComment_Container", false, false);
    }
    else {
        openModal("You can't send reply for this comment", null, null, null, null, null, null, 2.75);
    }
});

$(document).on("click", ".select-to-hide", function (event) {
    let trueId = getTrueId(event.target.id);
    if (trueId != "") {
        let btnText = $("#SB_C-Title").text();
        let currentCount = btnText.substring(btnText.lastIndexOf(" ") + 1);
        currentCount = parseInt(currentCount);
        currentCount--;
        if (currentCount > 0) {
            slideToLeftAnimation("AllMessages-" + trueId);
            $("#SB_C-Title").html(" <i class='fas fa-times'></i> Received Messages ∙ " + currentCount.toLocaleString());
        }
        else {
            slideToLeftAnimation("AllMessages-" + trueId);
            $("#MarkAllMessagesAsRead--256").fadeOut(350);
            setTimeout(function () {
                let div = $("<div class='p-3 text-center'></div>");
                let iconTitle = $("<h1 class='h1'> <i class='fas fa-minus-circle'></i> </h1>");
                let textTitle = $("<h3 class='h3 safe-font'>No Messages</h3>");
                let text = $("<small class='card-text text-muted'>You've hidden all your received messages. They'll recover by next opening</small>");
                div.append(iconTitle);
                div.append(textTitle);
                div.append(text);
                $("#SB_C-Body").append(div);
                $("#SB_C-Title").html(" <i class='fas fa-times'></i> No Messages ∙ All Hidden");
            }, 350);
        }
    }
});

$(document).on("click", ".select-to-remove-the-msg", function (event) {
    let trueId = getTrueId(event.target.id);
    let userId = $("#PageInfo_UserId").val();
    if (parseInt(trueId) && parseInt(userId)) {
        $("#RSM_Id_Val").val(trueId);
        $("#RSM_UserId_Val").val(userId);
        let url = $("#RemoveSentMessage_Form").attr("action");
        let data = $("#RemoveSentMessage_Form").serialize();
        $.post(url, data, function (response) {
            if (response.success) {
                let currentCount = $("#SB_C-Title").text().substring($("#SB_C-Title").text().lastIndexOf(" ") + 1);
                currentCount = parseInt(currentCount);
                currentCount--;
                if (currentCount > 0) {
                    $("#SB_C-Title").html(" <i class='fas fa-times'></i> Sent Messages ∙ " + currentCount.toLocaleString());
                    slideToLeftAnimation("AllSentMessages-" + response.id);
                }
                else {
                    bubbleAnimation("NotificationsLiquid_Container", false);
                    setTimeout(function () {
                        $("#SB_C-Title").html(" <i class='fas fa-times'></i> No Sent Messages");
                        $("#SB_C-Body").empty();
                        let div = $("<div class='p-3 text-center'></div>");
                        let iconTitle = $("<h1 class='h1'> <i class='fas fa-comment-slash'></i> </h1>");
                        let textTitle = $("<h3 class='h3 safe-font'>No Messages</h3>");
                        let text = $("<small class='card-text text-muted'>You haven't get any messages. All of them will lately will appear here to edit, check or remove</small>");
                        div.append(iconTitle);
                        div.append(textTitle);
                        div.append(text);
                        $("#SB_C-Body").append(div);
                    }, 600);
                    setTimeout(function () {
                        bubbleAnimation("NotificationsLiquid_Container", true);
                    }, 1250);
                }
            }
            else {
                bubbleAnimation("NotificationsLiquid_Container", false);
                setTimeout(function () {
                    openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.25);
                }, 500);
            }
        });
    }
});
$(document).on("click", ".select-for-info", function (event) {
    let trueId = getTrueId(event.target.id);
    if (trueId != 0) {
        $("#GMI_Id_Val").val(trueId);
        let url = $("#GetMessageInfo_Form").attr("action");
        let data = $("#GetMessageInfo_Form").serialize();

        $.get(url, data, function (response) {
            if (response.success) {
                let sentBy = $("#SentFrom-" + trueId).text();
                let div = $("<div class='box-container p-3 mt-2'></div>");
                let header = $("<h5 class='h5 text-primary safe-font mb-2'></h5>");
                let separatorFirst = $("<div class='mt-1'></div>");
                let separatorSecond = $("<div class='mt-1'></div>");
                let sentDate = $("<small class='card-text'></small>");
                let isChecked = $("<small class='card-text'></small>");
                let projectName = $("<small class='card-text'></p>");
                header.html(sentBy);
                projectName.html("<span class='text-muted'>Project: </span>" + response.result.projectName);
                sentDate.html("<span class='text-muted'>Sent at: </span>" + convertDateAndTime(response.result.sentAt, true, true));
                if (response.result.isChecked) {
                    isChecked.html(" <i class='fas fa-check-double text-primary'></i> This message has been checked");
                }
                else {
                    isChecked.html(" <i class='fas fa-check text-muted'></i> This message isn't checked");
                }

                div.append(header);
                div.append(projectName);
                div.append(separatorSecond);
                div.append(sentDate);
                div.append(separatorFirst);
                div.append(isChecked);

                bubbleAnimation("NotificationsLiquid_Container", false);
                setTimeout(function () {
                    $("#SB_C-Title").html(" <i class='fas fa-times'></i> Message Info");
                    $("#SB_C-Body").empty();
                    $("#SB_C-Body").append(div);
                    bubbleAnimation("NotificationsLiquid_Container", true);
                }, 800);
            }
            else {
                closeContainerBubbleAnimation("NotificationsLiquid_Container", false);
                openModal(response.alert, "<i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.25);
            }
        });
    }
});

$(document).on("click", ".copy-the-main-link", function () {
    copyToClipboard(null, false, true);
});

$(document).on("click", ".remove-from-liked", function (event) {
    let trueId = getTrueId(event.target.id);
    if (trueId != "") {
        $("#RFL_ProjectId_Val").val(trueId);
        $("#RemoveFromLiked_Form").submit();
    }
});

$(document).on("click", ".btn-audio-backward", function (event) {
    let trueId = getTrueId(event.target.id);
    let curUrl = document.location.href;
    if (trueId != null) {
        let audio = document.getElementById("AudioControl-" + trueId);

        if (audio != null) {
            audio.currentTime = 0;
            $("#AudioBackwardBtn-" + trueId).css("transform", "rotate(180deg)");
            setTimeout(function () {
                $("#AudioBackwardBtn-" + trueId).css("transform", "rotate(0)");
            }, 2500);
        }
    }
});
$(document).on("click", ".btn-mute-audio", function (event) {
    let trueId = getTrueId(event.target.id);
    let curUrl = document.location.href;
    if (trueId != "") {
        let audio = document.getElementById("AudioControl-" + trueId);

        if (audio != null) {
            audio.muted = true;
            $("#MuteAudioBtn-" + trueId).addClass("d-none");
            $("#UnmuteAudioBtn-" + trueId).removeClass("d-none");
        }
    }
});
$(document).on("click", ".btn-unmute-audio", function (event) {
    let trueId = getTrueId(event.target.id);
    let curUrl = document.location.href;
    if (trueId != null) {
        let audio = document.getElementById("AudioControl-" + trueId);

        if (audio != null) {
            audio.muted = false;
            $("#MuteAudioBtn-" + trueId).removeClass("d-none");
            $("#UnmuteAudioBtn-" + trueId).addClass("d-none");
        }
    }
});
$(document).on("click", ".btn-play-audio", function (event) {
    let trueId = getTrueId(event.target.id);
    let curUrl = document.location.href;
    if (trueId != null) {
        let audio = document.getElementById("AudioControl-" + trueId);
        
        if (audio != null) {
            let duration = 0;
            let durationMin = 0;
            let durationSec = 0;
            let currentTime = 0;
            let currentTimeMin = 0;
            let currentTimeSec = 0;
            let currentDurationPerc = 0;
            let audioName = $("#AudioFileName-" + trueId).text();
            setTimeout(function () {
                duration = parseInt(audio.duration);
                if (duration < 60) {
                    durationMin = 0;
                    durationSec = duration;
                }
                else {
                    durationMin = parseInt(duration / 60);
                    durationSec = duration - (durationMin * 60);
                }

                if (durationSec < 10) durationSec = "0" + durationSec;
                if (durationMin < 10) durationMin = "0" + durationMin;
                $("#AudioDuration-" + trueId).text("00:00/" + durationMin + ":" + durationSec);
            }, 100);
            setInterval(function () {
                currentTime = parseInt(audio.currentTime);
                if (currentTime < 60) {
                    currentTimeMin = 0;
                    currentTimeSec = currentTime;
                }
                else {
                    currentTimeMin = parseInt(currentTime / 60);
                    currentTimeSec = currentTime - (currentTimeMin * 60);
                }

                if (currentTimeSec < 10) currentTimeSec = "0" + currentTimeSec;
                if (currentTimeMin < 10) currentTimeMin = "0" + currentTimeMin;
                currentDurationPerc = currentTime / duration * 100;

                $("#AudioDurationProgress-" + trueId).css("width", currentDurationPerc + "%");
                $("#AudioFileNameDuration_Lbl").text(currentTimeMin + ":" + currentTimeSec);
                $("#AudioDuration-" + trueId).text(currentTimeMin + ":" + currentTimeSec + "/" + durationMin + ":" + durationSec);
            }, 1000);

            audio.play();
            $("#PlayAudioBtn-" + trueId).css("transform", "rotate(360deg)");
            setTimeout(function () {
                $("#AudioFileName_Lbl").text(audioName);

                $("#PlayAudioBtn-" + trueId).addClass("d-none");
                $("#PauseAudioBtn-" + trueId).removeClass("d-none");
                $("#PlayAudioBtn-" + trueId).css("transform", "rotate(0)");
                $("#Preload_Container-Hanging").removeClass("d-none");
            }, 350);
            $("#AudioDuration-" + trueId).text(duration);
        }
    }
});
$(document).on("click", ".btn-pause-audio", function (event) {
    let trueId = getTrueId(event.target.id);
    let curUrl = document.location.href;
    if (trueId != null) {
        let audio = document.getElementById("AudioControl-" + trueId);
        if (audio != null) {
            audio.pause();
            $("#PauseAudioBtn-" + trueId).css("transform", "rotate(360deg)");
            setTimeout(function () {
                $("#PlayAudioBtn-" + trueId).removeClass("d-none");
                $("#PauseAudioBtn-" + trueId).addClass("d-none");
                $("#PauseAudioBtn-" + trueId).css("transform", "rotate(0)");
            }, 350);
        }
    }
});

$("#Name").on("keyup", function () {
    let value = $(this).val();
    if (value.length > 0) $("#PreviewTheProject_Btn").attr("disabled", false);
    else $("#PreviewTheProject_Btn").attr("disabled", true);
});
$("#TextPart").on("keyup", function () {
    lengthCounter("TextPart", 6000);
});
$("#RTM_Text").on("keyup", function () {
    let value = lengthCounter("RTM_Text", 1750);
    if (value < 1) $("#ReplyToMsgSend_Btn").attr("disabled", true);
    else $("#ReplyToMsgSend_Btn").attr("disabled", false);
});
$("#Text").on("keyup", function () {
    let value = lengthCounter("Text", 2500);
    if (value < 1) $("#MsgSendSbmt_Btn").attr("disabled", true);
    else $("#MsgSendSbmt_Btn").attr("disabled", false);
});
$(".send-msg-form-control").on("keyup", function (event) {
    let id = event.target.id;
    if (id != "" || id != undefined) {
        let length = $("#" + id).val().length;
        rowsCountAutoCorrection(id, length);
    } 
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
    let currentPrice = $("#CurrentPrice").val();

    $("#ProjectPrice_Badge").html(finalValue);
    $("#ProjectPricePercentage_Badge").html(percentageFromFull.toFixed(2) + "%");
    let newPricePercentage = 0;

    if (currentPrice != undefined) {
        if (currentPrice != value) {
            currentPrice = parseFloat(currentPrice);
            value = parseFloat(value);
            if (currentPrice != 0) {
                if (value > currentPrice) newPricePercentage = value / currentPrice * 100;
                else newPricePercentage = (100 - (value / currentPrice * 100)) * -1;
            }
            else newPricePercentage = value / 10000000 * 100;

            if (newPricePercentage > 0) {
                $("#UpdatedPrice_Badge").addClass("bg-success");
                $("#UpdatedPrice_Badge").removeClass("bg-danger");
                $("#UpdatedPrice_Badge").text("+" + newPricePercentage.toFixed(2) + "%");
            }
            else {
                $("#UpdatedPrice_Badge").addClass("bg-danger");
                $("#UpdatedPrice_Badge").removeClass("bg-success");
                $("#UpdatedPrice_Badge").text(newPricePercentage.toFixed(2) + "%");
            }         
            $("#TPCAnnotation_Box").removeClass("d-none");
        }
        else {
            $("#TPCAnnotation_Box").addClass("d-none");
            $("#TargetPriceChangeAnnotation").val("");
            $("#TargetPriceAnnotation_Collapse").addClass("collapsed");
            $("#TargetPriceAnnotation_Collapse").removeClass("show");
            $("#TargetPriceAnnotation_Collapse").removeClass("collapsed");

            $("#UpdatedPrice_Badge").addClass("bg-success");
            $("#UpdatedPrice_Badge").removeClass("bg-danger");
            $("#UpdatedPrice_Badge").text("Same Target");
        }
    }
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

$("#StartTheGame_Btn").on("click", function () {
    let type = $("#GameSpeedType_Val").val();
    gameOne(type);
});
$("#GameSpeedIncrease_Btn").on("click", function () {
    let type = $("#GameSpeedType_Val").val();
    if (type < 4) {
        type++;
        openModal("Game speed has been increased", "Done", null, 2, null, null, null, 2.5, " <i class='fas fa-tachometer-alt text-primary'></i> ");
    }
    else {
        type = 0;
        openModal("Game speed has been decreased to base speed (1.5 sec. interval)", "Done", null, 2, null, null, null, 2.5, " <i class='fas fa-backward text-danger'></i> ");
    }
    $("#GameSpeedType_Val").val(type);
});

$(document).on("click", ".get-user-short-info", function (event) {
    let trueId = getTrueId(event.target.id);
    if (trueId != "") {
        $("#GSU_Id_Val").val(trueId);
        $("#GetShortUserInfo_Form").submit();
    }
    else openModal("Unable to get some information about this project. Please, try again later", " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.5);
});

$(document).on("click", ".select-to-reply", function (event) {
    let trueId = getTrueId(event.target.id);
    if (trueId != 0 || trueId != "") {
        let userId = $("#PageInfo_UserId").val();
        let neededWidth = fullWidth;
        if (fullWidth >= 717) neededWidth = (fullWidth * 0.35) - 10;

        bubbleAnimation("NotificationsLiquid_Container", false);
        smallBarAnimatedOpenAndClose(true);

        let messageSenderName = $("#SentFrom-" + trueId).text();
        let messageText = $("#AllMessagesText-" + trueId).text();

        let msgInfoBox = $("<div class='box-container bg-primary p-2'><small class='card-text text-light'> <i class='fas fa-reply'></i> Replying To...</small></div>");
        let replyingTo = $("<h6 class='h6 text-light mt-2' id='ReplyingTo_Lbl'></h6>");
        let replyingMsgTxt = $("<small class='card-text text-light text-truncate' id='ReplyingToTxt_Lbl'></small>");
        msgInfoBox.append(replyingTo);
        msgInfoBox.append(replyingMsgTxt);

        let msgReplyMainContainer = $("<div class='smallside-box-container p-3 shadow-standard' id = 'MsgReply_Container' ></div > ");
        let msgReplySecondaryBox = $("<div class='mt-4'></div>");
        let msgReplyTitle = $("<h5 class='h5 text-truncate'>Reply</h5>");
        let msgReplyCloseBtn = $("<button type='button' class='btn btn-close float-end ms-1' id='MsgReply_Container-Close' aria-label='Close'></button>");
        let replyForm = $("<form method='post' action='/Messages/Reply' id='ReplyToMessage_Form'></form>");
        let replyFormDiv = $("<div class='mt-1'></div>");
        let replyFormDivRowBox = $("<div class='row'></div>'");
        let replyFormDivLgCol = $("<div class='col col-10'></div>");
        let replyFormDivSmCol = $("<div class='col col-2'></div>");
        let replyUserId_Input = $("<input type='hidden' name='UserId' id='RTM_UserId_Val' value='0' />");
        let replyMsgId_Input = $("<input type='hidden' name='MessageId' id='RTM_MsgId_Val' value='0' />");
        let replyProjectId_Input = $("<input type='hidden' name='ProjectId' value='-256' />");
        let replyMsgText_Input = $("<textarea class='form-control w-100 form-control-sm send-msg-form-control' rows='1' id='RTM_Text' name='Text' maxlength='1750' placeholder='Reply here'></textarea>");
        let replySubmit_Btn = $("<button type='submit' class='btn btn-primary btn-sm w-100 mt-1 btn-standard-with-no-colour' id='ReplyToMsgSend_Btn'>Reply</button>");

        replyingTo.text(messageSenderName);
        replyingMsgTxt.text(messageText);
        replyMsgId_Input.val(trueId);
        replyUserId_Input.val(userId);

        replyForm.append(replyUserId_Input);
        replyForm.append(replyMsgId_Input);
        replyFormDivLgCol.append(replyMsgText_Input);
        replyFormDivLgCol.append(replyProjectId_Input);
        replyFormDivSmCol.append(replySubmit_Btn);
        replyFormDivRowBox.append(replyFormDivLgCol);
        replyFormDivRowBox.append(replyFormDivSmCol);
        replyForm.append(replyFormDivRowBox);
        replyFormDiv.append(replyForm);

        msgReplySecondaryBox.append(msgInfoBox);
        msgReplySecondaryBox.append(replyFormDiv);
        msgReplyMainContainer.append(msgReplyCloseBtn);
        msgReplyMainContainer.append(msgReplyTitle);
        msgReplyMainContainer.append(msgReplySecondaryBox);
        $("#MsgReply_Container").remove();
        $("#Main_SideBar").append(msgReplyMainContainer);
        $("#MsgReply_Container").css("width", neededWidth + "px");

        animatedOpen(false, "MsgReply_Container", false, false);
    }
});

$(document).on("click", ".category-select-container", function (event) {
    let trueId = getTrueId(event.target.id);
    if (trueId != "") {
        $("#CategoryId").val(trueId);
        $(".category-select-container").css("border", "1px solid rgb(240, 240, 240");
        $("#CategoryValue-" + trueId).css("border", "1px solid #0d6efd");
    }
    else {
        $("#CategoryId").val(0);
        $(".category-select-container").css("border", "1px solid rgb(240, 240, 240");
    }
});

$(document).on("click", ".select-to-mark", function (event) {
    let trueId = getTrueId(event.target.id);
    let userId = $("#PageInfo_UserId").val();
    $("#MaR_Id_Val").val(trueId);
    $("#MaR_UserId_Val").val(userId);

    if (userId != 0 && trueId != "") {
        event.preventDefault();
        let url = $("#MarkAsRead_Form").attr("action");
        let data = $("#MarkAsRead_Form").serialize();
        $.post(url, data, function (response) {
            if (response.success) {
                if (response.id == -256) {
                    $(".is-checked-icon").html(" <i class='fas fa-check-double text-primary'></i> ");
                    $(".select-to-mark").html(" <i class='fas fa-check-double text-primary'></i> Marked as Read");
                    $(".select-to-mark").attr("disabled", true);
                }
                else {
                    $("#AllMessagesIsChecked-" + response.id).html(" <i class='fas fa-check-double text-primary'></i> ");
                    $("#MessageMarkAsRead-" + response.id).attr("disabled", true);
                    $("#MessageMarkAsRead-" + response.id).html(" <i class='fas fa-check-double text-primary'></i> Marked as Read");
                }
            }
            else {
                bubbleAnimation("NotificationsLiquid_Container", false);
                openModal(response.alert, " <i class='fas fa-times text-danger'></i> Close", null, 2, null, null, null, 3.25);
            }
        });
    }
});

$(document).on("click", ".select-to-remove", function (event) {
    let trueId = getTrueId(event.target.id);
    if (trueId != 0 || trueId != "") {
        $("#RP_ProjectId").val(trueId);
        smallBarAnimatedOpenAndClose(true);
        animatedOpen(false, "RemoveProject_Container", false, false);
    }
    else {
        openModal("Unable to remove this project right now. Please, try again later", null, null, null, null, null, null, 3);
    }
});
$(document).on("click", ".project-box", function (event) {
    let trueId = getTrueId(event.target.id);
    if (trueId != 0 && trueId != "") {
        let selectedProjectName = $("#ProjectName-" + trueId).text();
        let isPinned = $("#ProjectIsPinned-" + trueId).val();
        let isLocked = $("#ProjectIsLocked-" + trueId).val();
        isLocked = isLocked == "false" ? false : true;
        isPinned = isPinned == "false" ? false : true;

        $("#SelectedProjectName_Lbl").text(selectedProjectName);
        $("#GPI_Id").val(trueId);
        $(".have-an-edit").attr("href", "/Project/Edit/" + trueId);
        $(".have-a-lock").attr("id", "HaveALock-" + trueId);
        $(".select-to-remove").attr("id", "HaveARemove-" + trueId);

        if (isPinned) {
            $("#UTP_ProjectId_Val").val(trueId);
            $("#PinTheProject_Box").addClass("d-none");
            $("#UnpinTheProject_Box").removeClass("d-none");
        }
        else {
            $("#PTP_ProjectId_Val").val(trueId);
            $("#PinTheProject_Box").removeClass("d-none");
            $("#UnpinTheProject_Box").addClass("d-none");
        }

        if (isLocked) {
            $("#LockProject_Box").addClass("d-none");
            $("#UnlockTheProject_Box").removeClass("d-none");
        }
        else {
            $("#LockProject_Box").removeClass("d-none");
            $("#UnlockTheProject_Box").addClass("d-none");
        }
        $("#LTP_Id_Val").val(trueId);
        $("#ULTP_Id_Val").val(trueId);

        smallBarAnimatedOpenAndClose(true);
        animatedOpen(false, "SettingsOfProject_Container", false, false);
    }
});

$(document).on("click", ".btn-back", function () {
    let isSmallSideContainer = $("#" + lastContainerName).hasClass("smallside-box-container");
    if (isSmallSideContainer) openLastContainer(lastContainerName, true);
    else openLastContainer(lastContainerName, false);
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

    setTimeout(function () {
        $("#" + trueId).fadeIn(0);
        $("#" + trueId).css("z-index", -1000);
        $("#" + trueId).css("bottom", botNavbarH + 2 + "px");
    }, 150);
    setTimeout(function () {
        $("#" + trueId + "-Open").css("transform", "rotate(360deg)");
        $("#" + trueId + "-Open").html(" <i class='fas fa-chevron-down'></i> ");
        $("#" + trueId + "-Open").attr("disabled", true);
        $("#" + trueId + "-Open").css("bottom", currentBtnBottom + 56 + "px");
    }, 300);
    setTimeout(function () {
        $("#" + trueId + "-Open").css("bottom", currentBtnBottom + 2 + "px");
        $("#" + trueId).css("z-index", 0);
    }, 550);
});
$(document).on("click", ".btn-status-bar-close", function (event) {
    let trueId = getTrueName(event.target.id);
    let currentBtnBottom = $("#" + trueId + "-Open").css("bottom");
    currentBtnBottom = parseInt(currentBtnBottom) + 18;

    $("#" + trueId).css("z-index", "-1000");
    $("#" + trueId).css("bottom", "-1200px");
    setTimeout(function () {
        $("#" + trueId).css("z-index", "0");
        $("#" + trueId + "-Open").css("bottom", currentBtnBottom + 28 + "px");
        $("#" + trueId + "-Open").css("transform", "rotate(-360deg)");
    }, 50);
    setTimeout(function () {
        $("#" + trueId).fadeOut(500);
        $("#" + trueId + "-Open").css("bottom", botNavbarH - 32 + "px");
        $("#" + trueId + "-Open").attr("disabled", false);
        $("#" + trueId + "-Open").html(" <i class='fas fa-bars btn-static-bar-open' id='StatusBar_Container-OpenX'></i> ");
    }, 350);
    setTimeout(function () {
        $("#" + trueId + "-Open").css("bottom", botNavbarH + 9 + "px");
    }, 700);
});
$(document).on("click", ".btn-static-backdrop-open", function (event) {
    openStaticBackdrop(event.target.id, true);
});

$(document).on("click", ".btn-open-liquid-container", function (event) {
    let trueId = getTrueName(event.target.id);
    bubbleAnimation(trueId, true);
});
$(document).on("click", ".btn-close-liquid-container", function (event) {
    let trueId = getTrueName(event.target.id);
    bubbleAnimation(trueId, false);
});

$(document).on("click", ".add-option", function (event) {
    let type = getTrueId(event.target.id);
    let whereAndFrom = $("#" + event.target.id).attr("data-html");
    type = parseInt(type);

    smallBarAnimatedOpenAndClose(true);
    animatedOpen(false, "AddOption_Container", false, false);
    $("#AddOption_Type_Val").val(type);
    $("#AddOption_WhereAndFrom_Val").val(whereAndFrom);
    $("#ColorOption_Box").addClass("d-none");
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
        case 5:
            $("#AddOption_Badge").html("Colored Text");
            $("#AddOption_Val2").attr("disabled", true);
            $("#ColorOption_Box").removeClass("d-none");
            break;
        case 6:
            $("#AddOption_Badge").html("Included Text");
            $("#AddOption_Val2").attr("disabled", true);
            break;
        case 7:
            $("#AddOption_Badge").html("Heading Text");
            $("#AddOption_Val2").attr("disabled", true);
            break;
        case 8:
            $("#AddOption_Badge").html("Alternative Font Family");
            $("#AddOption_Val2").attr("disabled", true);
            break;
        default:
            $("#AddOption_Badge").html("Bold Text");
            $("#AddOption_Val2").attr("disabled", true);
            break;
    }
});

$(document).on("click", ".text-full-delete", function (event) {
    let trueId = getTrueId(event.target.id);
    if (trueId != "") {
        $("#" + trueId).val("");
        openModal("Content from textbox has been deleted", "Got It", null, 2, null, null, null, 2.5);
    }
});

$(document).on("click", ".boolean-change", function (event) {
    let trueName = getTrueName(event.target.id);
    if (trueName != "") {
        let value = $("#" + trueName).val();
        value = value.toLowerCase();
        value = value == "false" ? true : false;
        $("#" + trueName).val(value);
        if (value) $("#" + event.target.id).html(" <i class='fas fa-check text-primary'></i> With Files");
        else $("#" + event.target.id).html(" <i class='fas fa-times-circle text-danger'></i> Without Files");
    }
});

$(".game-hole").on("click", function () {
    gameScoreIncrease();
});

$(".main-container").on("scroll", function (event) {
    let trueId = event.target.id;
    if (trueId != "") {
        let mainContainerWidth = $("#" + trueId).innerWidth();
        let scrollHeight = parseInt($("#" + trueId).scrollTop());
        let containerH = $("#" + trueId + "-Header").innerHeight();
        let containerTop = $("#" + trueId + "-Header").css("top");
        containerTop = parseInt(containerTop) + containerH;

        if (scrollHeight >= 28) {
            $("#" + trueId + "-Header").css("position", "fixed");
            $("#" + trueId + "-Header").css("z-index", "1000");
            $("#" + trueId + "-Header").css("width", mainContainerWidth + "px");
            //$("#" + trueId + "-Header").css("background-color", "transparent");
            //$("#" + trueId + "-Header").css("backdrop-filter", "blur(5px)");
            $("#" + trueId + "-Header").addClass("shadow-sm");
            $("#" + trueId + "-Hanging").fadeIn(50);
            $("#" + trueId + "-Hanging").css("top", containerTop + "px");
        }
        else {
            $("#" + trueId + "-Header").css("position", "relative");
            $("#" + trueId + "-Header").css("z-index", "0");
            $("#" + trueId + "-Header").removeClass("shadow-sm");
            $("#" + trueId + "-Header").css("backdrop-filter", "none");
            $("#" + trueId + "-Hanging").css("top", "-1200px");
            $("#" + trueId + "-Hanging").fadeOut(250);
        }
    }
});
$(".smallside-box-container").on("scroll", function (event) {
    let trueId = event.target.id;
    if (trueId != "") {
        let mainContainerWidth = $("#" + trueId).innerWidth();
        let scrollHeight = parseInt($("#" + trueId).scrollTop());
        if (scrollHeight >= 28) {
            $("#" + trueId + "-Header").css("position", "fixed");
            $("#" + trueId + "-Header").css("z-index", "1000");
            $("#" + trueId + "-Header").css("width", mainContainerWidth + "px");
            $("#" + trueId + "-Header").css("background-color", "transparent");
            $("#" + trueId + "-Header").css("backdrop-filter", "blur(5px)");
            $("#" + trueId + "-Header").addClass("shadow-sm");
        }
        else {
            $("#" + trueId + "-Header").css("position", "relative");
            $("#" + trueId + "-Header").css("z-index", "0");
            $("#" + trueId + "-Header").removeClass("shadow-sm");
            $("#" + trueId + "-Header").css("backdrop-filter", "none");
        }
    }
});

$(".upload-file-btn").on("click", function (event) {
    let trueName = getTrueName(event.target.id);
    if (trueName != "") {
        getFile(trueName);
    }
});

function getFile(element) {
    $("#" + element).click();
}
function sub(event) {
    let file = event.value;
    let allFiles = $("#" + event.id).get(0).files;
    let filesFullName = "";
    let count = allFiles.length;

    if (count > 6) {
        openModal("We're sorry, but at this moment max count of uploaded images is 6. You're not allowed to upload more than 6 images per project due to server load capacity", "Got It", null, 2, null, null, null, 4, '<i class="fas fa-exclamation bell-icon text-warning"></i>');
    }
    else {
        for (let i = 0; i < count; i++) {
            let val = allFiles[i];
            if (val != undefined) filesFullName += val.name + ", ";
        }
        filesFullName = filesFullName.substring(0, filesFullName.lastIndexOf(","));

        $("#" + event.id + "-Get").text(filesFullName);
        if (count == 1) {
            $("#ChosenAudioName_Lbl").html(count + " audio selected: " + filesFullName);
            $("#ChosenFileName_Lbl").html(count + " image selected: " + filesFullName);
        }
        else {
            $("#ChosenFileName_Lbl").html(count + " images selected: " + filesFullName);
            $("#ChosenAudioName_Lbl").html(count + " audios selected: " + filesFullName);
        }
    }
}

function textDecoder(element, isFromText, needsTheReplacement) {
    let value;

    if (isFromText) value = $("#" + element).text();
    else value = $("#" + element).val();

    value = value.replaceAll("[[", "<span class='fw-500'>");
    value = value.replaceAll("[!", "<span class='fs-3'>");
    value = value.replaceAll("[-", "<span class='safe-font'>");
    value = value.replaceAll("]]", "</span>");
    value = value.replaceAll("[{", "<a class='text-decoration-none text-primary'");
    value = value.replaceAll("}]", "</a>");
    value = value.replaceAll("{[", "<span class='text-decoration-underline'>");
    value = value.replaceAll("[^", "<span class='fst-italic'>");
    value = value.replaceAll('[&', '<span style="');
    value = value.replaceAll("[=", "<div class='bordered-container p-2 mt-2'><p class='card-text'>");
    value = value.replaceAll("=]", "</p></div>");

    if (needsTheReplacement) {
        if (isFromText) $("#" + element).html(value);
        else $("#" + element).val(value);
    }

    return value;
}

function copyToClipboard(element, isFromText, copyTheMainLink) {
    let value;
    if (!copyTheMainLink) {
        if (isFromText) {
            value = $("#" + element).text();
        }
        else value = $("#" + element).val();
        navigator.clipboard.writeText(value);

        openModal("The link has been successfully copied to clipboard", "Done", null, 2, null, null, null, 4, "<i class='fas fa-clone text-primary'></i>");

        return value;
    }
    else {
        value = document.location.href;
        navigator.clipboard.writeText(value);
        openModal("The link has been successfully copied to clipboard", "Done", null, 2, null, null, null, 4, "<i class='fas fa-clone text-primary'></i>");

        return value;
    }
}

function previewCombinizer(previewingProject) {
    if (previewingProject) {
        let previousPricePercentage = 0;
        let title = $("#Name").val();
        let smallerDescription = textDecoder("Project_Description", null, false);
        let longerDescription = textDecoder("TextPart", null, false);
        let link1 = $("#ProjectLink1").val();
        let link2 = $("#ProjectLink1").val();
        let ytLink = $("#YoutubeLink").val();
        let projectPrice = $("#CurrentPrice").val();
        let currentPrice = $("#ProjectPrice").val();
        projectPrice = parseInt(projectPrice);
        currentPrice = parseInt(currentPrice);

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
            if (projectPrice != currentPrice) {
                if (currentPrice > projectPrice) previousPricePercentage = currentPrice / projectPrice * 100;
                else previousPricePercentage = (100 - currentPrice / projectPrice * 100) * -1;

                $("#Preview_PrevTagPrice").fadeIn(300);
                if (previousPricePercentage > 0) {
                    $("#Preview_TagPrice").text(parseFloat(currentPrice).toLocaleString("en-US", { style: "currency", currency: "USD" }));
                    $("#Preview_PrevTagPrice").html("<span class='text-decoration-line-through'>" + parseFloat(projectPrice).toLocaleString("en-US", { style: "currency", currency: "USD" }) + "</span>, +" + previousPricePercentage.toFixed(1) + "%");
                    $("#Preview_PrevTagPrice").removeClass("text-danger");
                    $("#Preview_PrevTagPrice").addClass("text-success");
                    $("#Preview_TagPriceDescription").text("needs this project for his start");
                }
                else {
                    $("#Preview_TagPrice").text(parseFloat(currentPrice).toLocaleString("en-US", { style: "currency", currency: "USD" }));
                    $("#Preview_PrevTagPrice").html("<span class='text-decoration-line-through'>" + parseFloat(projectPrice).toLocaleString("en-US", { style: "currency", currency: "USD" }) + "</span>, " + previousPricePercentage.toFixed(1) + "%");
                    $("#Preview_PrevTagPrice").addClass("text-danger");
                    $("#Preview_PrevTagPrice").removeClass("text-sucess")
                }
            }
            else {
                $("#Preview_PrevTagPrice").fadeOut(300);
                $("#Preview_TagPrice").text(parseFloat(projectPrice).toLocaleString("en-US", { style: "currency", currency: "USD" }));
            }
        }
        else {
            $("#Preview_TagPrice").text("No investments");
            $("#Preview_PrevTagPrice").fadeOut(300);
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

    return length;
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
        case 5:
            textBefore = textBefore + '[& color:' + value2 + ';">' + value1 + ']]';
            break;
        case 6:
            if (cursorPosition != 0) textBefore = textBefore + "\n[=" + value1 + "=]";
            else textBefore = textBefore + "[=" + value1 + "=]";
            break;
        case 7:
            textBefore = textBefore + "[!" + value1 + "]]";
            break;
        case 8:
            textBefore = textBefore + "[-" + value1 + "]]";
            break;
        default:
            textBefore = textBefore + " [[" + value1 + "]]";
            break;
    }

    $("#" + element).val(textBefore + textAfter);
    smallBarAnimatedOpenAndClose(false);
    if (fullWidth < 717) {
        animatedOpen(false, "Preload_Container", true, true);
    }
}

function convertDateAndTime(value, asShort, showTimeInfo) {
    let dT = new Date(value);
    let dTCurrent = new Date();
    let day = 0;
    let month = 0;
    let hr = 0;
    let min = 0;
    let daysFrom = 0;
    let monthsFrom = 0;
    let yearsFrom = 0;
    let fullAlert;
    let year = 0;
    let monthName;

    if (dT.getHours() < 10) hr = "0" + dT.getHours();
    else hr = dT.getHours();
    if (dT.getMinutes() < 10) min = "0" + dT.getMinutes();
    else min = dT.getMinutes();

    if (dT.getFullYear() == dTCurrent.getFullYear()) year = "";
    else year = " " + dT.getFullYear();
    //if (dT.getFullYear() == dTCurrent.getFullYear()) year = "";
    //else {
    //    if (asShort) year = "/" + dT.getFullYear();
    //    else year = " " + dT.getFullYear();
    //}

    if (dT.getDate() < 10) day = "0" + dT.getDate();
    else day = dT.getDate();

    if (asShort) {
        if (dTCurrent.getFullYear() == dT.getFullYear()) {
            yearsFrom = 0;
        }
        else {
            yearsFrom = dTCurrent.getFullYear() - dT.getFullYear();
        }
        if (dTCurrent.getMonth() == dT.getMonth()) {
            monthsFrom = 0;
            daysFrom = dTCurrent.getDate() - dT.getDate();
        }
        else {
            daysFrom = dTCurrent.getDate() - dT.getDate();
            monthsFrom = dTCurrent.getMonth() - dT.getMonth();
        }
        if (daysFrom < 0) daysFrom *= -1;
        if (monthsFrom < 0) monthsFrom *= -1;

        if (yearsFrom > 0) {
            if (monthsFrom > 0) {
                fullAlert = yearsFrom + "yr, " + monthsFrom + "m ago";
            }
            else {
                if (daysFrom > 0) {
                    fullAlert = yearsFrom + "yr, " + daysFrom + "d ago";
                }
                else {
                    fullAlert = yearsFrom + "yr ago";
                }
            }
        }
        else {
            if (monthsFrom > 0) {
                fullAlert = monthsFrom + "m, " + daysFrom + "d ago";
            }
            else {
                if (daysFrom == 1) fullAlert = daysFrom + " day ago, at " + hr + ":" + min;
                else fullAlert = daysFrom + " days ago, at " + hr + ":" + min;
            }
        }
    }
    else {
        let months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
        monthName = months[dT.getMonth()];

        if (showTimeInfo) fullAlert = day + " " + monthName + year + ", at " + hr + ":" + min;
        else fullAlert = day + " " + monthName + year;
    }

    return fullAlert;
}

function getTrueId(fullName) {
    return fullName.substring(fullName.indexOf("-") + 1);
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

function rowsCountAutoCorrection(element, length) {
    let currentRowsCount = $("#" + element).attr("rows");
    if (fullWidth < 717) {
        if (length >= 39) {
            if (length / currentRowsCount > 40) {
                if (currentRowsCount < 9) {
                    currentRowsCount++;
                    $("#" + element).attr("rows", currentRowsCount);
                }
            }
        }
        else {
            $("#" + element).attr("rows", 1);
        }
    }
    else {
        if (length >= 49) {
            if (length / currentRowsCount > 50) {
                if (currentRowsCount < 12) {
                    currentRowsCount++;
                    $("#" + element).attr("rows", currentRowsCount);
                }
            }
        }
        else {
            $("#" + element).attr("rows", 1);
        }
    }
}

function openLastContainer(element, isSmallSideContainer) {
    if (element == undefined) {
        animatedOpen(false, "Preload_Container", true, true);
    }
    else if (element == null) {
        return false;
    }
    else {
        if (!isSmallSideContainer) animatedOpen(false, element, true, true);
        else {
            smallBarAnimatedOpenAndClose(true);
            animatedOpen(false, element, false, false);
        }
    }
}

function openModal(text, btn1Txt, btn2Txt, btn1WhatToDo, btn1Action, btn2WhatToDo, btn2Action, fadeOutTimer, icon) {
    setTimeout(function () {
        let currentContainerB = $("#" + currentContainerName).css("bottom");
        let currentContainerType = $("#" + currentContainerName).hasClass("smallside-box-container");
        let currentContainerH = $("#" + currentContainerName).innerHeight();
        let containersFullHeight = parseInt(currentContainerB) + parseInt(currentContainerH);
        let windowFullHeight = fullHeigth;

        $("#MMC_Text").html(text);
        $("#MainModal_Container").fadeIn(0);
        $("#MainModal_Container").css("z-index", "1000");

        if (icon == null) $("#MMC_Icon").html(" <i class='fas fa-info-circle'></i> ");
        else $("#MMC_Icon").html(" " + icon + " ");

        let modalH = $("#MainModal_Container").innerHeight();
        windowFullHeight -= modalH + 8;

        if (containersFullHeight > 0 && containersFullHeight < windowFullHeight && currentContainerType) {
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
                    closeContainerBubbleAnimation("MainModal_Container", true);
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
                    closeContainerBubbleAnimation("MainModal_Container", true);
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
    if (href.toLowerCase().includes("account/create")) {
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
    else if ((href.toLowerCase().includes("profile") || href.toLowerCase().includes("create") || href.toLowerCase().includes("edit")) && (fullWidth < 717)) {
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
    $(".bubble-container").css("max-height", neededH + "px");
    $(".smallside-box-container").css("max-height", neededH + "px");
    $(".btn-right-bottom-fixed").css("bottom", botNavbarH + 10 + "px");

    if (width < 717) {
        $(".main-container").css("width", "100%");
        $(".main-container").css("left", 0);
        $(".hanging-container").css("left", 0);
        $(".hanging-container").css("width", "100%");
        $(".static-bar").css("width", "100%");
        $(".static-bar").css("left", 0);
        $(".smallside-box-container").css("width", "99%");
        $(".smallside-box-container").css("left", "2px");
        $(".smallside-modal-container").css("left", "2.4%");
        $(".smallside-modal-container").css("width", "95%");
        $(".card-box-container").css("width", "100%");
        $(".card-box-container").css("left", "1px");

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
        $(".hanging-container").css("width", leftW + "px");
        $(".hanging-container").css("left", leftBarW + "px");
        $(".static-bar").css("width", leftW + "px");
        $(".static-bar").css("left", leftBarW + "px");
        $(".smallside-box-container").css("width", smallSideContainerW + "px");
        $(".smallside-box-container").css("left", "2px");
        $(".smallside-modal-container").css("left", "10px");
        $(".smallside-modal-container").css("width", smallSideContainerW - 19 + "px");
        $(".card-box-container").css("width", smallSideContainerW + "px");
        $(".card-box-container").css("left", "1px");

        $("#MainBotOffNavbar").css("width", leftW + "px");
        $("#MainBotOffNavbar_Box").css("width", leftW + "px");
        $("#MainBotOffNavbar").css("left", leftBarW + "px");
    }
}

function hide(element) {
    $("#" + element).css("top", "-1200px");
    $("#" + element).fadeOut(350);
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

function containerBubbleAnimation(element) {
    let currentContainerH = $("#" + element).innerHeight();
    let neededH = fullHeigth - currentContainerH - 20;

    if (fullWidth >= 1024) $("#" + element).css("width", "75%");
    else $("#" + element).css("width", "95%");

    $("#" + element).css("left", "50%");
    $("#" + element).css("transform", "translateX(-50%)");
    $("#" + element).css("bottom", neededH + "px");
    $("#" + element).css("background-color", "transparent");
    $("#" + element).css("backdrop-filter", "blur(5px)");
    $("#" + element).css("border", "1px solid rgb(240, 240, 240)");
}
function closeContainerBubbleAnimation(element, isForSmallSide) {
    let smallSideContainerW = $("#Main_SideBar").innerWidth() - 19;

    setTimeout(function () {
        $("#" + element).css("border", "1px solid transparent");
        $("#" + element).css("transform", "none");
        $("#" + element).css("background-color", "#f8f9fa");
    }, 250);
    animatedClose(false, element);
    if (isForSmallSide) {
        animatedClose(false, element);
        setTimeout(function () {
            if (fullWidth < 717) {
                $("#" + element).css("left", "2.4%");
                $("#" + element).css("width", "95%");
            }
            else {
                $("#" + element).css("left", "10px");
                $("#" + element).css("width", smallSideContainerW + "px");
            }
        }, 300);
    }
}

function bubbleAnimation(element, isForOpening) {
    if (isForOpening) {
        $(".container").css("filter", "blur(4px)");
        $(".smallside-modal-container").css("filter", "blur(4px)");
        $(".static-bar").css("filter", "blur(4px)");
        $(".main-container").not("bubble-container box-container").css("filter", "blur(4px)");
        $("#" + element).fadeIn(300);
        $("#" + element).css("z-index", "1000");
        $("#" + element).css("top", "55px");
        setTimeout(function () {
            $("#" + element).css("width", "75%");
        }, 150);
        setTimeout(function () {
            $("#" + element).css("top", "10px");
            $("#" + element).css("width", "70%");
        }, 450);
        setTimeout(function () {
            $("#" + element).css("top", "25px");
        }, 900);
    }
    else {
        $(".bubble-container").css("top", "60px");
        $(".bubble-container").css("width", "75%");
        setTimeout(function () {
            $(".container").css("filter", "none");
            $(".main-container").not("bubble-container box-container").css("filter", "none");
            $(".smallside-modal-container").css("filter", "none");
            $(".static-bar").css("filter", "none");

            $(".bubble-container").css("width", "70%");
            $(".bubble-container").css("z-index", "-1");
        }, 300);
        setTimeout(function () {
            $(".bubble-container").css("top", "-1200px");
            $(".bubble-container").fadeOut(300);
        }, 500);
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
            $("#" + element + "-CardBox").fadeIn(25);
            if (sticky) {
                $("#" + element).css("bottom", "20px");
                setTimeout(function () {
                    $("#" + element).css("bottom", 0);
                    $("#" + element + "-CardBox").css("bottom", 0);
                    $("#" + element).css("border-radius", "8px 8px 1px 1px");
                }, 550);
            }
            else {
                $("#" + element).css("bottom", 0);
                $("#" + element + "-CardBox").css("bottom", 0);
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

function gameScoreIncrease() {
    let score = $("#Score_Val").val();
    let speed = $("#Speed_Span").text();
    speed = parseFloat(speed);
    score = parseInt(score);
    score = (score + 1) * speed;

    if (score < 10) {
        speed += 0.05;
        $("#ScoreTable_Lbl").text("00" + score.toLocaleString());
    }
    else if (score >= 10 && score < 100) {
        speed += 0.15;
        $("#ScoreTable_Lbl").text("0" + score.toLocaleString());
    }
    else if (score >= 100 && score < 250) {
        speed += 0.25;
        $("#ScoreTable_Lbl").text(score.toLocaleString());
    }
    else {
        speed += 0.3;
        $("#ScoreTable_Lbl").text(score.toLocaleString());
    }
    $("#Score_Val").val(score);
    $("#Speed_Span").text(speed.toFixed(2));
    $("#SpeedIcon_Span").html("<i class='fas fa-chevron-up text-success'></i>");

    $("#StartTheGame_Btn").attr("disabled", true);
    $("#GameSpeedIncrease_Btn").attr("disabled", true);
}
function gameOne(type) {
    let cellNumber = 0;
    let speed = 0.1;
    let duration = 0;
    let interval = 0;
    let score = 0;

    if (type == 0) interval = 2400;
    else if (type == 1) interval = 1700;
    else if (type == 2) interval = 1200;
    else if (type == 3) interval = 900;
    else interval = 650;

    setInterval(function () {
        duration += 0.1;
        score = parseInt($("#Score_Val").val());
        $("#GameDuration_Span").text(duration.toFixed(1) + " sec.");
    }, 100); 

    setInterval(function () {
        let speed = $("#Speed_Span").text();
        speed = parseFloat(speed);
        if (speed > 0.1 && speed < 2) {
            speed -= 0.05;
        }
        else if (speed >= 2 && speed < 9) {
            speed -= 0.15;
        }
        else if (speed >= 9 && speed < 16) {
            speed -= 0.25;
        }
        else if (speed >= 16 && speed < 25) {
            speed -= 0.3;
        }
        else if (speed >= 25) {
            speed -= 0.45;
        }

        $("#Speed_Span").text(speed.toFixed(2));
        $("#SpeedIcon_Span").html("<i class='fas fa-chevron-down text-danger'></i>");
    }, 1250);

    $("#GameInterval_Span").text((interval / 1000).toFixed(2));

    setInterval(function () {
        $(".game-hole").css("pointer-events", "none");
        $(".game-hole").css("background-color", "#f8f9fa");
        cellNumber = 1 + parseInt(Math.random() * 24);
        $("#GameCell-" + cellNumber).css("pointer-events", "auto");
        $("#GameCell-" + cellNumber).css("background-color", "#0d6efd");
    }, interval);

    return score;
}

function animatedClose(forAll, elemet) {
    if (forAll) {
        $("." + elemet).css("bottom", "-1200px");
        $(".card-box-container").css("bottom", "-1200px");
        $("." + elemet).fadeOut(250);
        $(".card-box-container").fadeOut(250);
    }
    else {
        $("#" + elemet).css("bottom", "-1200px");
        $("#" + elemet + "-CardBox").css("bottom", "-1200px");
        $("#" + elemet).fadeOut(250);
        $("#" + elemet + "-CardBox").fadeOut(300);
    }
}

$("#MMC_Text").on("click", function () {
    containerBubbleAnimation("MainModal_Container");
});

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