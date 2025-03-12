// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
    return new bootstrap.Tooltip(tooltipTriggerEl)
})
function LoadCountryState_Control() {
    debugger;
    //GetCountryStateCity
    var serviceURL = '/XETutGlobalX/GetCountryStateCity';
    $.ajax({
        type: "POST",
        url: serviceURL,
        data: { },
        //contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            debugger;
            let country_row = '';
            let phone_code_row = '';
            let selectedCountry;
            if (sessionStorage.getItem("SelectedCountry") != null) {
                selectedCountry = sessionStorage.getItem("SelectedCountry");
                
            }
            else {
                selectedCountry = '0';
                $("#drp_State").html("<option selected value='0'>Please Choose Country First</option>")
            }
            

            
            if (data.isSuccess != false) {
                if (data.countryStateDetails != null) {
                    country_row = country_row + "<option selected value='0'>Select Country</option>"
                    
                    for (let index = 0; index < data.countryStateDetails.length; index++) {
                        if (selectedCountry == data.countryStateDetails[index].iso2) {
                            country_row = country_row + "<option selected value='" + data.countryStateDetails[index].iso2 + "'>" + data.countryStateDetails[index].name + "</option>";
                        }
                        else {
                            country_row = country_row + "<option value='" + data.countryStateDetails[index].iso2 + "'>" + data.countryStateDetails[index].name + "</option>";
                        }
                        
                        phone_code_row = phone_code_row + "<option id='opt_" + data.countryStateDetails[index].emoji + "' value='" + data.countryStateDetails[index].phone_code + "'>" + "<ing src='/flags/flags/1x1/" + getFlagEmoji(data.countryStateDetails[index].iso2) + ".svg'/>" +" " + data.countryStateDetails[index].phone_code + "</option>";
                        $("#drp_PhoneCode").html(phone_code_row);
                        //$("#opt_" + data.countryStateDetails[index].emoji).html("<flag-emoji code='" + data.countryStateDetails[index].emoji + "'></flag-emoji>");
                    }
                    $("#drp_Country").html(country_row);
                    
                }
            }
        },
        error: errorFunc
    });
}
function EnabledOtherDesignation() {
    if ($("#drp_Designation").val() == "OTH") {
        $("#txtOtherDesignationDiv").removeClass("d-none");
    }
    else {
        $("#txtOtherDesignationDiv").addClass("d-none");
    }
}
function BindDesignationDropDown() {
    var serviceURL = '/XETutGlobalX/BindDesignation';
    $.ajax({
        type: "POST",
        url: serviceURL,
        data: {},
        //contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            debugger;
            let desi_row = '';
            let userAdminType;
            if (data.isSuccess != false) {
                if (data.designations != null) {
                    
                    desi_row = desi_row + "<option selected value='0'>Select Designation</option>"

                    for (let index = 0; index < data.designations.length; index++) {
                        if (data.designations[index].designation_Code == "CEO" || data.designations[index].designation_Code == "CTO") {
                            userAdminType = "<option value='A'>Admin</option>";
                        }
                        desi_row = desi_row + "<option value='" + data.designations[index].designation_Code + "'>" + data.designations[index].designation_Name + "</option>";
                    }
                    $("#drp_Designation").html(desi_row);
                    $("#drpUserType").append(userAdminType);
                }
            }
        },
        error: errorFunc
    });
}
function BindCity(obj) {
    debugger;
    //citySet
    let controlid = obj.id;
    let stateCode = $("#" + controlid).val();
    let countryCode = $("#drp_Country").val();
    var serviceURL = '/XETutGlobalX/BindCity';
    $.ajax({
        type: "POST",
        url: serviceURL,
        data: { StateCode: stateCode, CountryCode: countryCode },
        //contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            debugger;
            let city_row = '';
            if (data.isSuccess != false) {
                if (data.citySet != null) {
                    //drp_State
                    if (data.citySet != null) {
                        city_row = city_row + "<option selected value='0'>Select City</option>"

                        for (let index = 0; index < data.citySet.length; index++) {
                            city_row = city_row + "<option value='" + data.citySet[index].code + "'>" + data.citySet[index].name + "</option>";
                        }
                        $("#drp_City").html(city_row);
                    }
                }
            }
        },
        error: errorFunc
    });
}
function CheckCredentails(obj) {
    debugger;
    let controlid = $("#" + obj.id);

    let password_field = controlid.val();
    let serviceURL = '/XETutGlobalX/CheckCredentails';
    if (password_field != "") {
        $.ajax({
            type: "POST",
            url: serviceURL,
            data: { input: password_field },
            //contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                success_Check_Credentials(data, obj.id);
            },
            error: error_Check_Credentials
        });
    }
    
}

function success_Check_Credentials(data, controlid) {
    debugger;
    if (data.isValidPassword) {
        //
        sessionStorage.setItem("IsValidCredential" + controlid, "true");
    }
    else {
        if (data.response == "Weak") {
            sessionStorage.setItem("IsValidCredential" + controlid, "false");
            
        }
        
        
    }
    
}
function error_Check_Credentials(data) {

}
function RegisterUser() {
    debugger;
    const input_param = [];
    let designation;
    let Othdesignation;
    let designationName
    //drp_AdminDesignation
    if ($("#drpUserType").val() == "A") {
        switch ($("#drp_AdminDesignation").val()) {
            
            default:
                designation = $("#drp_AdminDesignation").val();
                designationName = $("#drp_AdminDesignation option:selected").text();
                break;
        }
    }
    else {
        switch ($("#drp_Designation").val()) {
            case "OTH":
                designation = $("#drp_Designation").val();
                designationName = $("#txtOtherDesignation").val();
                break;
            default:
                designation = $("#drp_Designation").val();
                designationName = $("#drp_Designation option:selected").text();
                break;
        }
    }
   
    if ($("#txtFirstName").val() == "") {
        toastr.error("First Name field Required!");
        $("#txtFirstName").focus();
        return false;
    }
    if ($("#txtLastName").val() == "") {
        toastr.error("Last Name field Required!");
        $("#txtLastName").focus();
        return false;
    }
    if ($("#txtEmailId").val() == "") {
        toastr.error("EmailID field Required!");
        $("#txtEmailId").focus();
        return false;
    }
    if ($("#drpUserType").val() == "0") {
        toastr.error("User Type field Required!");
        $("#drpUserType").focus();
        return false;
    }
    if ($("#drpUserType").val() == "S") {

    }
    else {
        if ($("#drp_Designation").val() == "0") {
            toastr.error("Designation field Required!");
            $("#drp_Designation").focus();
            return false;
        }
    }
    if ($("#txtUserName").val() == "") {
        toastr.error("Username field Required!");
        $("#txtUserName").focus();
        return false;
    }

    


    if ($("#txtPassword").val() == "") {
        toastr.error("Password field Required!");
        $("#txtPassword").focus();
        return false;
    }
    if ($("#txtCofirmPassword").val() == "") {
        toastr.error("Cofirm Password field Required!");
        $("#txtCofirmPassword").focus();
        return false;
    }
    if (sessionStorage.getItem("IsValidCredential" + "txtPassword") == "true") {
        if (sessionStorage.getItem("IsValidCredential" + "txtCofirmPassword") == "true") {
            if ($("#txtPassword").val() == $("#txtCofirmPassword").val()) {

            }
            else {
                toastr.error("Password Not Matched");
                return false;
            }
        }
        else {
            toastr.error("Please follow the hint and make sure password must match");
            return false;
        }
    }
    else {
        toastr.error("Please follow the hint and make sure password must match");
        return false;
    }

    input_param.push({
        personal:{
            item: {
                FirstName: $("#txtFirstName").val(),
                MiddleName: $("#txtMiddleName").val(),
                LastName: $("#txtLastName").val(),
                Gender: $("#drpGender").val(),
                EmailId: $("#txtEmailId").val(),
                dateOfBirth: $("#dateOfBirth").val(),
                desig_Code: designation,
                desig_Name: designationName
            }
        },
        contact: {
            item: {
                country: $("#drp_Country").val(),
                state: $("#drp_State").val(),
                city: $("#drp_City").val(),
                postalCode: $("#txt_Postal_Zip_Code").val(),
                currentAddress: $("#txt_Current_Address").val(),
                phonecountrycode: $(".iti__a11y-text").html(),
                phoneNumber: $("#txt_Phone_Number").val()
            }
        },
        loginCredential: {
            item: {
                userName: $("#txtUserName").val(),
                password: $("#txtPassword").val(),
                confirm_password:$("#txtCofirmPassword").val()
            }
        }
    })
    HitToAPI('RegisterProcess', input_param);
}

function PasswordMasking(id, txtid) {
    debugger;
    
    if ($("#" + txtid).attr("type") == "password") {
        $("#" + txtid).attr('type', 'text');
        $("#" + id).removeClass("fa-solid fa-eye");
        $("#" + id).addClass("fa-solid fa-eye-slash");

    }
    else {
        $("#" + txtid).attr('type', 'password');
        $("#" + id).addClass("fa-solid fa-eye");
        //$("#" + id).removeClass("fa-solid fa-eye-slash");
    }
}
function getFlagEmoji(countryCode) {
    return countryCode.toUpperCase().replace(/./g, char =>
        String.fromCodePoint(127397 + char.charCodeAt())
    );
}
//function getFlagEmoji(countryCode) {
//    const codePoints = countryCode
//        .toUpperCase()
//        .split('')
//        .map(char => 127397 + char.charCodeAt());
//    return String.fromCodePoint(...codePoints);
//}
function BindState(obj) {
    debugger;
    let controlid = obj.id;
    let countrycode = $("#" + controlid).val();
    var serviceURL = '/XETutGlobalX/BindState';
    $.ajax({
        type: "POST",
        url: serviceURL,
        data: { CountryCode: countrycode },
        //contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            debugger;
            let state_row = '';
            if (data.isSuccess != false) {
                if (data.stateDetails != null) {
                    //drp_State
                    if (data.stateDetails != null) {
                        state_row = state_row + "<option selected value='0'>Select State</option>"

                        for (let index = 0; index < data.stateDetails.length; index++) {
                            state_row = state_row + "<option value='" + data.stateDetails[index].state_code + "'>" + data.stateDetails[index].name + "</option>";
                        }
                        $("#drp_State").html(state_row);
                    }
                }
            }
        },
        error: errorFunc
    });
}
function ShowHideDesignation(id) {
    debugger;
    if ($("#" + id.id).val() != "0") {
        if ($("#" + id.id).val() == "E") {
            $("#Designation_Div").removeClass("d-none");
            $("#drp_Designation_Div").removeClass("d-none");
            $("#drp_AdminDesignation_Div").addClass("d-none");

        }
        else if ($("#" + id.id).val() == "A") {
            $("#Designation_Div").removeClass("d-none");
            
            var serviceURL = '/XETutGlobalX/GetAdminDesignations';
            $.ajax({
                type: "GET",
                url: serviceURL,
                data: {},
                //contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    debugger;
                    let desi_row = '';
                    let userAdminType;
                    if (data.isSuccess != false) {
                        if (data.admin_designations != null) {

                            desi_row = desi_row + "<option selected value='0'>Select Designation</option>"

                            for (let index = 0; index < data.admin_designations.length; index++) {
                               
                                desi_row = desi_row + "<option value='" + data.admin_designations[index].designation_Code + "'>" + data.admin_designations[index].designation_Name + "</option>";
                            }
                            $("#drp_Designation_Div").addClass("d-none");
                            $("#drp_AdminDesignation_Div").removeClass("d-none");
                            $("#drp_AdminDesignation").html(desi_row);
                            
                        }
                    }
                },
                error: errorFunc
            });
        }
        else {
            $("#Designation_Div").addClass("d-none");
            $("#drp_Designation_Div").addClass("d-none");
            $("#drp_AdminDesignation_Div").addClass("d-none");
        }
    }
    else {
        $("#Designation_Div").addClass("d-none");
    }
    
}

/****************************************************
 * Function: loginValidation
 * Purpose: This Function is used to validate login request and send this request to database using store procedure.
 * Flow:
 * Author/Developer: Mandeep Singh - Owner(GMETECH Technology Group)
 * 
 ***************************************************/
function loginValidation(id) {
    var username_ID = $("#txtusername");
    var email_ID = $("#txtemail");
    var password_ID = $("#txtpassword");
    let errormsg;
    if (username_ID.val() != "") {
        if (email_ID.val() != "") {
            if (validateEmail(email_ID.val()) == false) {
                return false;
            }
            else {
                if (password_ID.val() != "") {
                    //Start Login Process Logic
                    
                    $("#Loader_spin").removeClass("d-none");
                    $("#spnloading").removeClass("d-none");

                    $("#spnLoginLabel").addClass("d-none");
                    $("#" + id.id).attr("disabled", true);

                    LoginProcess(username_ID.val(), email_ID.val(), password_ID.val());
                    //End Login Process Logic
                }
                else {
                    errormsg = "Password Field is Mandatory for login Process!";
                    $("#txtpassword").focus();
                    toastr.error(errormsg);
                    
                }
            }
            
            
        }
        else {
            if (password_ID.val() == "") {
                errormsg = "Both Email ID and Password Field are Mandatory for login Process!";
                $("#txtpassword").focus();
                $("#txtemail").focus();
                toastr.error(errormsg);
                
            }
            else {
                errormsg = "Email ID Field is Mandatory for login Process!";
                $("#txtemail").focus();
                toastr.error(errormsg);
                
            }
            
        }
        
    }
    else {
        if (email_ID.val() == "") {
            if (password_ID.val() == "") {
                errormsg = "Username, Email ID and Password Field are Mandatory for login Process!";
                toastr.error(errormsg);
                
            } else {
                errormsg = "Username and Email ID Field are Mandatory for login Process!";
                toastr.error(errormsg);
                
            }
            
        }
        else {
            errormsg = "Username Field is Mandatory for login Process!";
            toastr.error(errormsg);
        }
        
    }

}
function LoginProcess(username, emailid, password) {
    var loginserviceURL = '/XETutGlobalX/LoginProcess';
    let logininput = username + "~" + emailid + "~" + password;
    $.ajax({
        type: "POST",
        url: loginserviceURL,
        data: { login_Input: logininput },
        //contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: successFunc,
        error: errorFunc
    });
}

function VerifyOtp(input1,input2,input3,input4,input5,input6) {
    debugger;
    let input_Otp;
    if (input1 != undefined && input2 != undefined && input3 != undefined && input4 != undefined && input5 != undefined && input6 != undefined) {
        if ($("#" + input1).val() == "") {
            toastr.error("Please enter first value of send OTP")
            input1.focus();
            return false;
        }
        if ($("#" + input2).val() == "") {
            toastr.error("Please enter second value of send OTP")
            input2.focus();
            return false;
        }
        if ($("#" + input3).val() == "") {
            toastr.error("Please enter third value of send OTP")
            input3.focus();
            return false;
        }
        if ($("#" + input4).val() == "") {
            toastr.error("Please enter forth value of send OTP")
            input4.focus();
            return false;
        }
        if ($("#" + input5).val() == "") {
            toastr.error("Please enter fifth value of send OTP")
            input5.focus();
            return false;
        }
        if ($("#" + input6).val() == "") {
            toastr.error("Please enter last value of send OTP")
            input6.focus();
            return false;
        }

        input_Otp = parseInt($("#" + input1).val() + $("#" + input2).val() + $("#" + input3).val() + $("#" + input4).val() + $("#" + input5).val() + $("#" + input6).val());
        var ChechOtpserviceURL = '/XETutGlobalX/ChechOtp';
        $.ajax({
            type: "POST",
            url: ChechOtpserviceURL,
            data: { parameter: input_Otp },
            //contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: checkOtp_successFunc,
            error: errorFunc
        });
    }
}
function checkOtp_successFunc(data) {
    debugger;
    if (data != null) {
        if(data.isSuccess != null) {
            if (data.isSuccess == true) {
                if (data.isExceptionExist) {
                    console.log(data.exceptionmessage);
                    toastr.error(data.message);
                    return false;
                }
                if (data.isOTP_Matched) {
                    toastr.success(data.message);
                    //window.location.href = data.redirectToSystemURL;
                    window.open(data.redirectToSystemURL, '_blank');
                    window.location.reload();
                }
                else {
                    toastr.error(data.message);
                    return false;
                }
            }
            else {
                alert("OTP Expired!");
                window.location.href = data.url;
            }
        }
    }
}
function successFunc(data, status) {
    debugger;
    if (data != null) {
        $("#Loader_spin").addClass("d-none");
        $("#btnXETutGlobalX_Login").removeAttr("disabled");
        $("#spnloading").addClass("d-none");

        $("#spnLoginLabel").removeClass("d-none");
        if (data.isSuccess != null) {
            if (data.isSuccess == true) {
                //alert(data);
                if (data.webloginValidation != null) {
                    if (data.webloginValidation[0].loginStatus == true) {
                        if (data.webloginValidation[0].sendOTPMailResponse.status == true) {
                            $("#login-fill-div").addClass("d-none");
                            $("#otp-verification-div").removeClass("d-none");

                        }
                        //toastr.success(data.webloginValidation[0].message);
                    }
                    else {
                        $("#login-fill-div").removeClass("d-none");
                        $("#otp-verification-div").addClass("d-none");

                        toastr.error(data.webloginValidation[0].message);
                    }
                }
                
            }
            else {
                toastr.error('Login process fail due to some critical reason!');
                //alert('error');
            }
        }
    }
    
}

function errorFunc() {
    toastr.error('Error in Processing');
    //alert('error');
}
function validateEmail(emailAddress) {

    if (emailAddress.includes("@") && emailAddress.includes(".")) {
        
        
        return true;
    } else {
        toastr.error("Please enter Valid Email Id!");
        $("#txtemail").focus();
        
        return false;
    }
}
function ClearField() {
    $("#txtusername").val("");
    $("#txtemail").val("");
    $("#txtpassword").val("");
}
function ShowPassword() {
    var password_ID = document.getElementById("txtpassword");
    if (password_ID.type === "password") {
        password_ID.type = "text";
    } else {
        password_ID.type = "password";
    }
}