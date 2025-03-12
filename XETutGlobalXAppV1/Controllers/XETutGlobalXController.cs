using Microsoft.AspNetCore.Mvc;
using XEtutGlobalX.Modal.Material;
using Newtonsoft.Json;
using System.Net.NetworkInformation;
using System.Data.Common;
using System.Net;
using System.Net.Mail;
using static XETutGlobalXAppV1.AppCode.XETutGlobalX_ConnectDB.DBConnect_Mode;
using System.Data;
using XEtutGlobalX.Modal.XETutGlobalX_DB.Request;
using XETutGlobalX.DAL.XETutGlobalX_DB;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using Azure;
using XEtutGlobalX.Modal.XETutGlobalX_DB.Response;
using Microsoft.AspNetCore.Http;
using static XEtutGlobalX.Modal.XETutGlobalX_DB.Response.WebloginValidation;
using XEtutGlobalX.Modal.XETutGlobalX_DB.Request.UserProfile;
using static System.Runtime.InteropServices.JavaScript.JSType;
using XEtutGlobalX.Modal.XETutGlobalX_DB.Response.UserProfile;
namespace XETutGlobalXAppV1.Controllers
{
    
    public class XETutGlobalXController : Controller
    {
        private readonly IConfiguration _config;
        public XETutGlobalXController(IConfiguration config)
        {
            _config = config;
        }
        public string HashPassword(string password)
        {
            // Generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            // Combine the salt and password hash
            return $"{Convert.ToBase64String(salt)}:{hashed}";
        }
        /// <summary>
        /// This Function is used for Load LoginX Views.
        /// </summary>
        /// <returns></returns>
        public IActionResult LoginX()
        {
            return View("LoginX");
        }
        [HttpPost]
        public IActionResult BindCity(string StateCode, string CountryCode)
        {
            bool status = true;
            CityDetails cityDetails = new CityDetails();
            List<CitySet> citySets = new List<CitySet>();
            object citySet = new();
            try
            {
                using (StreamReader r = new StreamReader("Material-Json/cities.json"))
                {
                    string json = r.ReadToEnd();

                    cityDetails.citySets = JsonConvert.DeserializeObject<List<CitySet>>(json!)!;
                    //countryState = countryStateDetails.countries;
                    //User item = JsonConvert.DeserializeObject<User>(json);
                }
                if (cityDetails.citySets != null)
                {
                    for (int i = 0; i < cityDetails.citySets.Count; i++)
                    {
                        if (cityDetails.citySets[i].state_code == StateCode && cityDetails.citySets[i].country_code == CountryCode)
                        {
                            citySets.Add(new CitySet
                            {
                                code = cityDetails.citySets[i].name,
                                name = cityDetails.citySets[i].name,
                                

                            });
                        }
                    }
                    citySet = citySets;
                }
            }
            catch (Exception ex)
            {
                status = false;
                return Json(new
                {
                    IsSuccess = status,
                    Error = ex.Message
                });
            }
            return Json(new
            {
                IsSuccess = status,
                citySet = citySet
            });
        }
        [HttpPost]
        public IActionResult BindState(string CountryCode)
        {
            bool status = true;
            List<State> states = new List<State>();
            CountryStateDetails countryStateDetails = new CountryStateDetails();
            object countryState = new();
            try
            {
                using (StreamReader r = new StreamReader("Material-Json/countries+states.json"))
                {
                    string json = r.ReadToEnd();

                    countryStateDetails.countries = JsonConvert.DeserializeObject<List<Country>>(json!)!;
                    countryState = countryStateDetails.countries;
                    //User item = JsonConvert.DeserializeObject<User>(json);
                }
                if(countryStateDetails.countries != null)
                {
                    for(int i = 0; i < countryStateDetails.countries.Count; i++)
                    {
                        if (countryStateDetails.countries[i].iso2 == CountryCode)
                        {
                            states = countryStateDetails.countries[i].states;
                        }
                    }
                    countryState = states;
                }
            }
            catch (Exception ex)
            {
                status = false;
                return Json(new
                {
                    IsSuccess = status,
                    Error = ex.Message
                });
            }
            return Json(new
            {
                IsSuccess = status,
                stateDetails = countryState
            });
        }
        [HttpGet]
        public IActionResult GetAdminDesignations()
        {
            bool status = true;
            List<Designation> designations = new List<Designation>();
            List<Designation> admin_designations = new List<Designation>();
            object obj_admin_designations = new();
            try
            {
                if (HttpContext.Session.GetString("DesignationList") != null)
                {
                    string json = HttpContext.Session.GetString("DesignationList")!;
                    designations = JsonConvert.DeserializeObject<List<Designation>>(json)!;
                }
                else
                {
                    BindDesignation();
                    string json = HttpContext.Session.GetString("DesignationList")!;
                    designations = JsonConvert.DeserializeObject<List<Designation>>(json)!;
                }
                if (designations != null)
                {
                    foreach (Designation designation in designations)
                    {
                        if (designation != null)
                        {
                            if (designation.Designation_Code == "CEO" || designation.Designation_Code == "CTO")
                            {
                                admin_designations.Add(designation);
                                obj_admin_designations = admin_designations;
                            }
                           
                        }
                        else
                        {
                            obj_admin_designations = null!;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                status = false;
                return Json(new
                {
                    IsSuccess = status,
                    Error = ex.Message
                });
            }
            
            return Json(new {
                IsSuccess = true,
                admin_designations = obj_admin_designations,
            
            });
        }
        [HttpPost]
        public IActionResult BindDesignation()
        {
            bool status = true;
            string DBConnect = string.Empty;
            string DataSource = string.Empty;
            string InitialCatalog = string.Empty;
            string IntegratedSecurity = string.Empty;
            string Encrypt = string.Empty;
            string TrustServerCertificate= string.Empty;
            object objdesignations = new();
            DataSet dsDesignation = new();
            List<Designation> designations = new List<Designation>();
            XETutGlobalX.DAL.XETutGlobalX_DB.clsDesignation clsDesignation = new();
            try
            {
                DBConnectMode mode = _config.GetValue<DBConnectMode>("DBSetting:mode")!;
  
                switch (mode)
                {
                    case DBConnectMode.Simple:
                        DBConnect = _config.GetValue<string>("DBSetting:ConnectionString")!;
                        dsDesignation = clsDesignation.GetDesignation(DBConnect, (XETutGlobalX.DAL.App_Code.DBConnectMode.DBConnect_Mode)mode);
                        break;
                    case DBConnectMode.Complex:
                        DataSource = _config.GetValue<string>("DBSetting:DataSource")!;
                        InitialCatalog = _config.GetValue<string>("DBSetting:DataSource")!;
                        IntegratedSecurity = _config.GetValue<string>("DBSetting:IntegratedSecurity")!;
                        Encrypt = _config.GetValue<string>("DBSetting:Encrypt")!;
                        TrustServerCertificate = _config.GetValue<string>("DBSetting:TrustServerCertificate")!;
                        DBConnect = DataSource + ";" + InitialCatalog + ";" + IntegratedSecurity + ";" + Encrypt +";" +  TrustServerCertificate + ";";
                        dsDesignation = clsDesignation.GetDesignation(DBConnect, (XETutGlobalX.DAL.App_Code.DBConnectMode.DBConnect_Mode)mode);
                        break;
                }
                if (dsDesignation != null)
                {
                    if(dsDesignation.Tables!=null)
                    {
                        if (dsDesignation.Tables.Count > 0)
                        {
                            if (dsDesignation.Tables[0].Rows.Count > 0)
                            {
                                string json = JsonConvert.SerializeObject(dsDesignation.Tables[0]);
                                designations = JsonConvert.DeserializeObject<List<Designation>>(json)!;
                                HttpContext.Session.SetString("DesignationList", json);
                            }
                        }
                    }
                }
                objdesignations = designations;




            }
            catch (Exception ex)
            {
                status = false;
                return Json(new
                {
                    IsSuccess = status,
                    Error = ex.Message
                });
            }
            return Json(new
            {
                IsSuccess = status,
                designations = objdesignations
            });
        }
        [HttpPost]
        public IActionResult GetCountryStateCity()
        {
            bool status = true;

           


            CountryStateDetails countryStateDetails = new CountryStateDetails();
            object countryState = new();
            try
            {
                using (StreamReader r = new StreamReader("Material-Json/countries+states.json"))
                {
                    string json = r.ReadToEnd();
                    
                    countryStateDetails.countries = JsonConvert.DeserializeObject<List<Country>>(json!)!;
                    countryState = countryStateDetails.countries;
                    //User item = JsonConvert.DeserializeObject<User>(json);
                }
            }
            catch (Exception ex)
            {
                status = false;
                return Json(new
                {
                    IsSuccess = status,
                    Error = ex.Message
                });
            }
            return Json(new
            {
                IsSuccess = status,
                countryStateDetails = countryState
            });
        }
        public IActionResult RegXter()
        {
            
            return View();
        }
        [HttpPost]
        public IActionResult ChechOtp(int parameter) 
        {  
            bool status = true;
            bool isOTPMatched;
            string message = "";
            string RedirectToSystemURL = "";

            try
            {
                if (HttpContext.Session.GetString("GeneratedOTPCode") != null && HttpContext.Session.GetString("webloginValidation") != null)
                {
                    status = true;
                    if (parameter == Convert.ToInt32(HttpContext.Session.GetString("GeneratedOTPCode")))
                    {
                        isOTPMatched = true;
                        message = XETutGlobalXAppV1.AppCode.Common.OtpSuccessMessage;
                        RedirectToSystemURL = Url.Action("Home", "OTMSPortal")!;
                    }
                    else
                    {
                        isOTPMatched = false;
                        message = XETutGlobalXAppV1.AppCode.Common.OtpErrorMessage;
                    }
                }
                else
                {
                    status = false;
                    return Json(new
                    {
                        IsSuccess = status,
                        URL = Url.Action("LoginX", "XETutGlobalX")
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    IsSuccess = status,
                    isExceptionExist = true,
                    Exceptionmessage = ex.Message,
                    message = XETutGlobalXAppV1.AppCode.Common.OtpExceptionMessage
                });
            }
            
            return Json(new
            {
                IsSuccess = status,
                isOTP_Matched= isOTPMatched,
                Message = message,
                redirectToSystemURL = RedirectToSystemURL
            }); 
        }
        [HttpPost]
        public IActionResult LoginProcess(string login_Input)
        {
            DataSet dsResponse = new();
            LoginResultSet loginResultSet = new LoginResultSet();
            List<WebloginValidation> webloginValidation = new List<WebloginValidation>();
            object obj_webloginValidation = new();
            bool status = true;
            string[] loginInputSet;
            string userName = string.Empty;
            string emailID = string.Empty;
            string password = string.Empty;
            string DBConnect = string.Empty;
            string RediectURL = string.Empty;
            string response = string.Empty;
            string DataSource = string.Empty;
            string InitialCatalog = string.Empty;
            string IntegratedSecurity = string.Empty;
            string Encrypt = string.Empty;
            string TrustServerCertificate = string.Empty;
            string fromEmail = string.Empty;
            string fromPassword = string.Empty;
            string sendEmailResponse = string.Empty;
            SendOTPMailResponse sendOTPMailResponse= new SendOTPMailResponse();
            if (string.IsNullOrEmpty(login_Input)) 
            { 
                status = false; 
            } 
            else 
            { 
                loginInputSet = login_Input.Split('~');
                userName = loginInputSet[0];
                emailID = loginInputSet[1];
                password = loginInputSet[2];
                XETutGlobalX.DAL.XETutGlobalX_DB.Authentication authentication = new();
                DBConnectMode mode = _config.GetValue<DBConnectMode>("DBSetting:mode")!;
                fromEmail= _config.GetValue<string>("EmailConnection:fromEmail")!;
                fromPassword= _config.GetValue<string>("EmailConnection:fromPassword")!;

                switch (mode)
                {
                    case DBConnectMode.Simple:
                        DBConnect = _config.GetValue<string>("DBSetting:ConnectionString")!;

                        dsResponse = authentication.LoginProcess(userName,password,emailID, DBConnect, (XETutGlobalX.DAL.App_Code.DBConnectMode.DBConnect_Mode)mode, fromEmail, fromPassword);
                        break;
                    case DBConnectMode.Complex:
                        DataSource = _config.GetValue<string>("DBSetting:DataSource")!;
                        InitialCatalog = _config.GetValue<string>("DBSetting:DataSource")!;
                        IntegratedSecurity = _config.GetValue<string>("DBSetting:IntegratedSecurity")!;
                        Encrypt = _config.GetValue<string>("DBSetting:Encrypt")!;
                        TrustServerCertificate = _config.GetValue<string>("DBSetting:TrustServerCertificate")!;
                        DBConnect = DataSource + ";" + InitialCatalog + ";" + IntegratedSecurity + ";" + Encrypt + ";" + TrustServerCertificate + ";";
                        dsResponse = authentication.LoginProcess(userName, password, emailID, DBConnect, (XETutGlobalX.DAL.App_Code.DBConnectMode.DBConnect_Mode)mode, fromEmail, fromPassword);
                        break;
                }
                if (dsResponse != null)
                {
                    if (dsResponse.Tables[0].Rows.Count > 0)
                    {
                        foreach(DataRow row in dsResponse.Tables[0].Rows)
                        {
                            if (row["Result"].ToString() == "User successfully logged in")
                            {
                                if (row["SecurityCode"] != null)
                                {
                                    if (row["SecurityCode"].ToString() != "")
                                    {
                                        HttpContext.Session.SetString("GeneratedOTPCode", row["SecurityCode"].ToString()!);
                                        sendEmailResponse = authentication.SendOtpEmail(fromEmail, fromPassword, emailID, row["SecurityCode"].ToString()!);
                                    }
                                    else
                                    {
                                        sendEmailResponse = null!;
                                    }
                                }
                                else
                                {
                                    sendEmailResponse = null!;
                                }
                                if (sendEmailResponse != null)
                                {
                                    sendOTPMailResponse = JsonConvert.DeserializeObject<SendOTPMailResponse>(sendEmailResponse)!;
                                }
                                else
                                {
                                    sendOTPMailResponse = null!;
                                }
                                webloginValidation.Add(new WebloginValidation
                                {
                                    LoginStatus = true,
                                    GUID = row["GUID"].ToString(),
                                    Message = row["Result"].ToString(),
                                    UserKey = Convert.ToInt32(row["UserKey"]),
                                    sendOTPMailResponse = sendOTPMailResponse
                                });
                                obj_webloginValidation = webloginValidation;
                                HttpContext.Session.SetString("webloginValidation", JsonConvert.SerializeObject(webloginValidation));
                            }
                            else
                            {
                                webloginValidation.Add(new WebloginValidation
                                {
                                    LoginStatus = false,
                                    GUID = null,
                                    Message = row["Result"].ToString(),
                                    UserKey = 0
                                });
                                obj_webloginValidation = webloginValidation;
                            }
                            
                        }
                    }
                }
            }
            return Json(new
            {
                IsSuccess = status,
                Login_Response = response,
                webloginValidation = obj_webloginValidation
            });
        }
        /// <summary>
        /// Method Name: RegistrationProcess
        /// Purpose: This function is used for register new user in UserLoginDetails...
        /// Flow: 
        /// Developer Name: Mandeep Singh - CEO - Chief Executive Officer,  GMETECH Tech. Group
        /// </summary>
        /// <param name="registration_Input"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RegistrationProcess(string Registration_Input)
        {
            bool IsEmailIDExist = false;
            bool IsUsernameExist = false;
            string New_UID = string.Empty;
            bool status = true;
            DataSet dsResponse = new();
            string DBConnect = string.Empty;
            string RediectURL = string.Empty;
           
            string DataSource = string.Empty;
            string InitialCatalog = string.Empty;
            string IntegratedSecurity = string.Empty;
            string Encrypt = string.Empty;
            string TrustServerCertificate = string.Empty;
            LoginCredential loginCredential = new();
            List<Registration_RequestBody> registration_RequestBody = new ();
            clsRegistration_RQ clsRegistration_RQ = new clsRegistration_RQ();
            try
            {
                if (Registration_Input != null)
                {
                    registration_RequestBody = JsonConvert.DeserializeObject<List<Registration_RequestBody>>(Registration_Input)!;
                    clsRegistration_RQ = XETutGlobalXAppV1.AppCode.XETutGlobalX.Authentication.Authentication_Process.RegisterUserRQ(registration_RequestBody);
                    //HashPassword
                    if (clsRegistration_RQ != null)
                    {
                        if (clsRegistration_RQ._Request != null)
                        {
                            if(clsRegistration_RQ._Request.RequestBody != null)
                            {
                                foreach(var item in clsRegistration_RQ._Request.RequestBody)
                                {
                                    if (item.LoginCredential != null)
                                    {
                                        loginCredential = item.LoginCredential;
                                    }
                                }
                            }
                        }
                    }
                    HashPassword(loginCredential.Item.password);
                    XETutGlobalX.DAL.XETutGlobalX_DB.Authentication authentication = new();
                    DBConnectMode mode = _config.GetValue<DBConnectMode>("DBSetting:mode")!;
                    switch (mode)
                    {
                        case DBConnectMode.Simple:
                            DBConnect = _config.GetValue<string>("DBSetting:ConnectionString")!;
                            dsResponse=authentication.clsRegistrationProcess(clsRegistration_RQ, DBConnect, (XETutGlobalX.DAL.App_Code.DBConnectMode.DBConnect_Mode)mode);
                            break;
                        case DBConnectMode.Complex:
                            DataSource = _config.GetValue<string>("DBSetting:DataSource")!;
                            InitialCatalog = _config.GetValue<string>("DBSetting:DataSource")!;
                            IntegratedSecurity = _config.GetValue<string>("DBSetting:IntegratedSecurity")!;
                            Encrypt = _config.GetValue<string>("DBSetting:Encrypt")!;
                            TrustServerCertificate = _config.GetValue<string>("DBSetting:TrustServerCertificate")!;
                            DBConnect = DataSource + ";" + InitialCatalog + ";" + IntegratedSecurity + ";" + Encrypt + ";" + TrustServerCertificate + ";";
                            dsResponse = authentication.clsRegistrationProcess(clsRegistration_RQ, DBConnect, (XETutGlobalX.DAL.App_Code.DBConnectMode.DBConnect_Mode)mode);
                            break;
                    }
                    
                }
                if (dsResponse != null)
                {
                    if (dsResponse.Tables[0]!=null)
                    {
                        if (dsResponse.Tables[0].Rows.Count > 0)
                        {
                            if (dsResponse.Tables[0].Rows[0]["Result"] != null)
                            {
                                if (dsResponse.Tables[0].Rows[0]["Result"].ToString() == "EmailID Already Exist")
                                {
                                    IsEmailIDExist = true;
                                }
                                if (dsResponse.Tables[0].Rows[0]["Result"].ToString() == "UserName Already Exist")
                                {
                                    IsUsernameExist = true;
                                }
                                if (dsResponse.Tables[0].Rows[0]["UID"] != null)
                                {
                                    New_UID = dsResponse.Tables[0].Rows[0]["UID"].ToString()!;
                                    RediectURL = Url.Action("LoginX", "XETutGlobalX")!;
                                }
                                else
                                {
                                    New_UID = "";
                                    RediectURL = "";
                                }
                            }
                        }
                    }    
                }
            }
            catch(Exception ex)
            {
                return Json(new { 
                    
                    IsSuccess = true, 
                    IsExceptionExist= true,
                    ErrorLog=ex.Message
                });
            }
            return Json(new 
            { 
                IsSuccess = status,
                IsEmailIDExist =IsEmailIDExist,
                IsUsernameExist = IsUsernameExist,
                New_UID=New_UID,
                Rediect_URL=RediectURL,
            });
        }
        [HttpPost]
        public IActionResult CheckCredentails(string input)
        {
            string response = string.Empty;
            bool IsValid = true;
            // Checking lower alphabet in string
            int n = input.Length;
            bool hasLower = false, hasUpper = false,
                    hasDigit = false, specialChar = false;
            HashSet<char> set = new HashSet<char>(
                new char[] { '!', '@', '#', '$', '%', '^', '&',
                          '*', '(', ')', '-', '+' });
            foreach (char i in input.ToCharArray())
            {
                if (char.IsLower(i))
                    hasLower = true;
                if (char.IsUpper(i))
                    hasUpper = true;
                if (char.IsDigit(i))
                    hasDigit = true;
                if (set.Contains(i))
                    specialChar = true;
            }

            // Strength of password
            Console.Write("Strength of password:- ");
            if (hasDigit && hasLower && hasUpper && specialChar
                && (n >= 8))
            {
                response = "Strong";
            }
               
            else if ((hasLower || hasUpper || specialChar)
                     && (n >= 6))
            {
                response = "Moderate";
            }
            else
            {
                response = "Weak";
            }
            if(response.Length > 0)
            {
                if(response == "Weak")
                {
                    IsValid = false;
                }
            }
            return Json(new {
                IsValidPassword = IsValid, 
                Response = response
            }); 
        }

        [HttpPost]
        public IActionResult Update_UserProfile(string input)
        {
            DataSet dsResponse = new DataSet();
            List<clsUpdateUserProfile> clsUpdateUserProfile = new();
            List<PersonalInfo> personalInfo = new();
            List<ContactInfo> contactInfo = new();
            HttpStatusCode httpStatus = HttpStatusCode.OK;
            string DBConnect = string.Empty;
            string DataSource = string.Empty;
            string InitialCatalog = string.Empty;
            string IntegratedSecurity = string.Empty;
            string Encrypt = string.Empty;
            string TrustServerCertificate = string.Empty;
            bool isSuccess = false;
            string Success_url = string.Empty;
            string response = string.Empty;
            if (HttpContext.Session.GetString("webloginValidation") == null)
            {

                return Json(new
                {
                    is_Success = isSuccess,
                    is_SessionExpired = true,
                    is_ExceptionExist = false,
                    message = "Session Expired",
                    url = Url.Action("LoginX", "XETutGlobalX")
                    //url = RedirectToAction("LoginX", "XETutGlobalX")
                });
            }
            else
            {
                try
                {

                    if (input != null)
                    {
                        clsUpdateUserProfile = JsonConvert.DeserializeObject<List<clsUpdateUserProfile>>(input)!;
                        foreach (var profiles in clsUpdateUserProfile)
                        {
                            if (profiles != null)
                            {
                                personalInfo = profiles.PersonalInfo!;
                                contactInfo = profiles.ContactInfo!;
                            }

                        }
                        if (personalInfo != null && contactInfo != null)
                        {
                            XETutGlobalX.DAL.XETutGlobalX_DB.UserProfile userProfile = new();
                            try
                            {
                                DBConnectMode mode = _config.GetValue<DBConnectMode>("DBSetting:mode")!;
                                switch (mode)
                                {
                                    case DBConnectMode.Simple:
                                        DBConnect = _config.GetValue<string>("DBSetting:ConnectionString")!;
                                        dsResponse = userProfile.UpdateLoginUserProfile(personalInfo, contactInfo, "", (XETutGlobalX.DAL.App_Code.DBConnectMode.DBConnect_Mode)mode, DBConnect);
                                        break;
                                    case DBConnectMode.Complex:
                                        DataSource = _config.GetValue<string>("DBSetting:DataSource")!;
                                        InitialCatalog = _config.GetValue<string>("DBSetting:DataSource")!;
                                        IntegratedSecurity = _config.GetValue<string>("DBSetting:IntegratedSecurity")!;
                                        Encrypt = _config.GetValue<string>("DBSetting:Encrypt")!;
                                        TrustServerCertificate = _config.GetValue<string>("DBSetting:TrustServerCertificate")!;
                                        DBConnect = DataSource + ";" + InitialCatalog + ";" + IntegratedSecurity + ";" + Encrypt + ";" + TrustServerCertificate + ";";
                                        dsResponse = userProfile.UpdateLoginUserProfile(personalInfo, contactInfo, "", (XETutGlobalX.DAL.App_Code.DBConnectMode.DBConnect_Mode)mode, DBConnect);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                dsResponse = null!;
                                //dsUserProfile = null!;
                            }

                        }
                        if (dsResponse != null)
                        {
                            if (dsResponse.Tables != null)
                            {
                                if (dsResponse.Tables.Count > 0)
                                {
                                    if (dsResponse.Tables[0].Rows.Count > 0)
                                    {
                                        if (dsResponse.Tables[0].Rows[0]["uid"] != null && dsResponse.Tables[0].Rows[0]["uid"].ToString() != "")
                                        {
                                            response = dsResponse.Tables[0].Rows[0]["uid"].ToString()!;
                                            Success_url = Url.Action("Home", "OTMSPortal")!;
                                        }
                                        else
                                        {
                                            response = dsResponse.Tables[0].Rows[0]["ErrorResult"].ToString()!;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            httpStatus = HttpStatusCode.InternalServerError;
                        }
                        isSuccess = true;
                    }

                }
                catch (Exception e)
                {
                    httpStatus = HttpStatusCode.InternalServerError;
                    return Json(new
                    {
                        is_Success = isSuccess,
                        is_ExceptionExist = true,
                        message = e.Message,
                        status = httpStatus
                    });
                }
            }
            
            return Json(new
            {
                status = httpStatus,
                is_Success = isSuccess,
                is_ExceptionExist = false,
                response = response,
                message = "",
                url = Success_url
            });

        }
    }
}
