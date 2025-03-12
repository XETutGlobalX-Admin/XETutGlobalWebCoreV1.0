
function HitToAPI(endpoints, input_param) {
    debugger;
    let serviceURL;

    switch (endpoints) {
        case "RegisterProcess":
            sessionStorage.removeItem("BackToLoginPageURL");
            serviceURL = '/XETutGlobalX/RegistrationProcess';
            $.ajax({
                type: "POST",
                url: serviceURL,
                data: { Registration_Input: JSON.stringify(input_param) },
                //contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: successRegisterProcessFunc,
                error: errorRegisterProcessFunc
            });
            break;
        case "UpdateUPProcess":
            serviceURL = '/XETutGlobalX/Update_UserProfile';
            $.ajax({
                type: "POST",
                url: serviceURL,
                data: { input: JSON.stringify(input_param) },
                //contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: successUserProfileFunc,
                error: errorUserProfileFunc
            });
            break;
        case "SubMit_Help_Desk_form":
            serviceURL = '/Home/SubmitHelpDesk';
            $.ajax({
                type: "POST",
                url: serviceURL,
                data: { formdata: input_param },
                //contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: successHelpDeskFunc,
                error: errorHelpDeskFunc
            });
            break;
    }
}
function successHelpDeskFunc(data) {
    debugger;
}
function successUserProfileFunc(data) {
    debugger;
    if (data != undefined) {
        if (data.is_Success == false) {
            if (data.is_SessionExpired == true) {
                
                alert(data.message);
                window.location.href = data.url;
            }
        }
        else {
            if (data.status != 200 && data.is_ExceptionExist == true) {
                toastr.error(data.message);
            }
            if (data.status == 200 && data.is_Success == true) {
                if (data.response == "User ID Not Found") {
                    toastr.error(data.response);
                    return false;
                }
                else if (data.response == "InValid User ID") {
                    toastr.error(data.response);
                    return false;
                }
                else if (data.response == "InValid Input") {
                    toastr.error(data.response);
                    return false;
                }
                else {
                    toastr.success("Record Updated");
                    toastr.options.timeOut = 1000000;
                    //const myTimeout = setTimeout(RedirectToHome(data), 5000);

                   
                    
                    
                }
            }
        }
    }
}
function RedirectToHome(url) {
    window.location.href = url;
}
function errorUserProfileFunc(error) {
    debugger;
    toastr.error("Something Goes Wrong...");
    return false;
} 
function errorHelpDeskFunc(error) {
    debugger;
    toastr.error("Something Goes Wrong...");
    return false;
} 

function displaySelectedImage(event, elementId) {
    debugger;
    const selectedImage = document.getElementById(elementId);
    const fileInput = event.target;

    if (fileInput.files && fileInput.files[0]) {
        const reader = new FileReader();

        reader.onload = function (e) {
            selectedImage.src = e.target.result;
        };

        reader.readAsDataURL(fileInput.files[0]);
    }
}
function UpdateProfile() {
    debugger;
    let profile_avtaar = $("#profile_Avtaar").attr("src");
    let updated_profile_avtaar;
    let personalInfo = [];
    let contactInfo = [];

    switch (profile_avtaar) {
        case "/Images/profile.png":
            updated_profile_avtaar = "";
            break;
        case "/Images/woman.png":
            updated_profile_avtaar = "";
            break;
        default:
            updated_profile_avtaar = profile_avtaar;
            break;
    }
    let txt_userprofile_FName = $("#txt_userprofile_FName").val();
    let txt_userprofile_MName = $("#txt_userprofile_MName").val();
    let txt_userprofile_LName = $("#txt_userprofile_LName").val();
    let drpGender = $("#drpGender").val();
    let txt_login_emailID = $("#txt_login_emailID").val();
    let txtDOB = $("#txtDOB").val();
    let hdn_login_UserID = $("#hdn_login_UserID").val();
    let txtLoggedInType = $("#txtLoggedInType").val();
    let txtLoggedDesignation = $("#txtLoggedDesignation").val();
    
    let drp_Country = $("#drp_Country").val()
    //'IN'
    let drp_State = $("#drp_State").val()
    //'HR'
    let drp_City = $("#drp_City").val()
    //'Panipat'
    let txtCurrentAddr = $("#txtCurrentAddr").val();
    let txt_Phone_Number = $("#txt_Phone_Number").val();
    let txtLogin_PinCode = $("#txtLogin_PinCode").val();
    personalInfo.push({
        UserID: hdn_login_UserID,
        profilePhoto: updated_profile_avtaar,
        firstName: txt_userprofile_FName,
        middleName: txt_userprofile_MName,
        lastName: txt_userprofile_LName,
        Gender: drpGender,
        login_emailID: txt_login_emailID,
        DOB: txtDOB,
        LoggedInType: txtLoggedInType,
        LoggedDesignation: txtLoggedDesignation,
        
    });
    contactInfo.push({
        Country: drp_Country,
        State: drp_State,
        City: drp_City,
        PinCode: txtLogin_PinCode,
        CurrentAddr: txtCurrentAddr,
        Phone_Code: $(".iti__a11y-text").html(),
        Phone_Number: txt_Phone_Number
    });
    const inputParameter = [];
    inputParameter.push({
        "PersonalInfo": personalInfo,
        "ContactInfo": contactInfo
    });
    UpdateProfileProcess(inputParameter);
}
function UpdateProfileProcess(inputParameter) {
    debugger;
    //Update_UserProfile
    HitToAPI("UpdateUPProcess", inputParameter);
}
function HidemyMessageModal(modalobj) {
    $("#" + modalobj).modal('hide');
    if (sessionStorage.getItem("BackToLoginPageURL") != null) {
        window.location.href = sessionStorage.getItem("BackToLoginPageURL");
    }
    
}
function successRegisterProcessFunc(data) {
    if (data != null) {
        if (data.isSuccess==true) {
            if (data.isEmailIDExist) {
                toastr.error("EmailID already Exist!");

                return false;
            }
            if (data.isUsernameExist) {
                toastr.error("Username already Exist!");
                return false;
            }
            if (data.new_UID != "") {
                $("#myMessageModal").modal('show');
                $("#spnmyMessageModaBody").html("<span>Thank You</span>")
                sessionStorage.setItem("BackToLoginPageURL", data.rediect_URL);
                //window.location.href = data.rediect_URL;
            }    
                
                
        }
    }
}
function errorRegisterProcessFunc(error) {
    toastr.error("Something Goes Wrong...");
    return false;
}
