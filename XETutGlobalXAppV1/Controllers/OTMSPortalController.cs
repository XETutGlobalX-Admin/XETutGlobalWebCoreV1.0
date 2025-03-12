using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Data.Common;
using XEtutGlobalX.Modal.Material;
using XEtutGlobalX.Modal.XETutGlobalX_DB.Response;
using XEtutGlobalX.Modal.XETutGlobalX_DB.Response.Courses;
using XEtutGlobalX.Modal.XETutGlobalX_DB.Response.UserProfile;
using XETutGlobalX.DAL.XETutGlobalX_DB;
using static XETutGlobalXAppV1.AppCode.XETutGlobalX_ConnectDB.DBConnect_Mode;

namespace XETutGlobalXAppV1.Controllers
{
    public class OTMSPortalController : Controller
    {
        #region Global Variables
        List<WebloginValidation> webloginValidation = new();
        private readonly IConfiguration _config;
        #endregion

        public OTMSPortalController(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Home()
        {
            #region Home Internal Variables
            int login_UserKey = 0;
            string sGuid = string.Empty;
            #endregion

            if (HttpContext.Session.GetString("webloginValidation") == null)
            {
                return RedirectToAction("LoginX","XETutGlobalX");
            }
            webloginValidation = JsonConvert.DeserializeObject<List<WebloginValidation>>(HttpContext.Session.GetString("webloginValidation")!)!;
            if(webloginValidation != null)
            {
                foreach(WebloginValidation validation in webloginValidation)
                {
                    login_UserKey = validation.UserKey;
                    sGuid = validation.GUID!;
                }
            }
            DataSet dsUserProfile = GetLoginUserDetails(login_UserKey, sGuid);
            dsUserProfile.Tables[0].TableName = "UserProfile";
            string UP_json = JsonConvert.SerializeObject(dsUserProfile);
            HttpContext.Session.SetString("UserProfileModel", UP_json);
            clsUserProfileModel upModel = JsonConvert.DeserializeObject<clsUserProfileModel>(UP_json)!;
            upModel.countryStateDetails = GetCountryStateCity();
            if (upModel != null)
            {
                if(upModel.UserProfile != null)
                {
                    foreach(var  user in upModel.UserProfile)
                    {
                        HttpContext.Session.SetString("LoggedInType", (user.LoginType!=null?user.LoginType:""));
                    }
                }
            }
            
            return View("Home", upModel);
        }
        public IActionResult Courses()
        {
            #region Home Internal Variables
            int login_UserKey = 0;
            string sGuid = string.Empty;
            clsCoursesModel clsCoursesModel = new();
            #endregion

            if (HttpContext.Session.GetString("webloginValidation") == null)
            {
                return RedirectToAction("LoginX", "XETutGlobalX");
            }
            webloginValidation = JsonConvert.DeserializeObject<List<WebloginValidation>>(HttpContext.Session.GetString("webloginValidation")!)!;
            if (webloginValidation != null)
            {
                foreach (WebloginValidation validation in webloginValidation)
                {
                    login_UserKey = validation.UserKey;
                    sGuid = validation.GUID!;
                }
            }
            DataSet dsUserProfile = GetLoginUserDetails(login_UserKey, sGuid);
            dsUserProfile.Tables[0].TableName = "UserProfile";
            string UP_json = JsonConvert.SerializeObject(dsUserProfile);
            HttpContext.Session.SetString("UserProfileModel", UP_json);
            clsUserProfileModel upModel = JsonConvert.DeserializeObject<clsUserProfileModel>(UP_json)!;
            upModel.countryStateDetails = GetCountryStateCity();
            if (upModel != null)
            {
                if (upModel.UserProfile != null)
                {
                    foreach (var user in upModel.UserProfile)
                    {
                        HttpContext.Session.SetString("LoggedInType", (user.LoginType != null ? user.LoginType : ""));
                    }
                }
            }
            clsCoursesModel.clsUserProfileModel = upModel;
            return View("Courses", clsCoursesModel);
        }
        public CountryStateDetails GetCountryStateCity()
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
                countryStateDetails = null!;
            }
            return countryStateDetails;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="login_UserKey"></param>
        /// <param name="sGuid"></param>
        public DataSet GetLoginUserDetails(int login_UserKey, string sGuid)
        {
            #region Internal Variables
           
            string DBConnect = string.Empty;
            string DataSource = string.Empty;
            string InitialCatalog = string.Empty;
            string IntegratedSecurity = string.Empty;
            string Encrypt = string.Empty;
            string TrustServerCertificate = string.Empty;
            DataSet dsUserProfile = new();
            XETutGlobalX.DAL.XETutGlobalX_DB.UserProfile profile = new();
            #endregion
            try
            {
                DBConnectMode mode = _config.GetValue<DBConnectMode>("DBSetting:mode")!;
                switch (mode)
                {
                    case DBConnectMode.Simple:
                        DBConnect = _config.GetValue<string>("DBSetting:ConnectionString")!;
                        dsUserProfile = profile.GetLoginUserDetails((XETutGlobalX.DAL.App_Code.DBConnectMode.DBConnect_Mode)mode, login_UserKey.ToString() + "||" + sGuid, DBConnect);
                        break;
                    case DBConnectMode.Complex:
                        DataSource = _config.GetValue<string>("DBSetting:DataSource")!;
                        InitialCatalog = _config.GetValue<string>("DBSetting:DataSource")!;
                        IntegratedSecurity = _config.GetValue<string>("DBSetting:IntegratedSecurity")!;
                        Encrypt = _config.GetValue<string>("DBSetting:Encrypt")!;
                        TrustServerCertificate = _config.GetValue<string>("DBSetting:TrustServerCertificate")!;
                        DBConnect = DataSource + ";" + InitialCatalog + ";" + IntegratedSecurity + ";" + Encrypt + ";" + TrustServerCertificate + ";";
                        dsUserProfile = profile.GetLoginUserDetails((XETutGlobalX.DAL.App_Code.DBConnectMode.DBConnect_Mode)mode, login_UserKey.ToString() + "||" + sGuid, DBConnect);
                        break;
                }
            }
            catch (Exception ex)
            { 
                dsUserProfile = null!;
            }
            return dsUserProfile;
        }
    }
}
