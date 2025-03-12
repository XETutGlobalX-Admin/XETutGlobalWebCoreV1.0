using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using XEtutGlobalX.Modal.XETutGlobalX_DB.Response.UserProfile;
using XEtutGlobalX.Modal.XETutGlobalX_DB.Response;
using XEtutGlobalX.Modal.Material;

namespace XETutGlobalXAppV1.Controllers
{
    public class ProfileManagementController : Controller
    {
        #region Global Variables
        List<WebloginValidation> webloginValidation = new();
        private readonly IConfiguration _config;
        #endregion
        public ProfileManagementController(IConfiguration config)
        {
            _config = config;
        }
        public IActionResult UserProfile()
        {
            #region UserProfile Internal Variables
            int login_UserKey = 0;
            string sGuid = string.Empty;
            clsUserProfileModel upModel = new();
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
            if (HttpContext.Session.GetString("UserProfileModel") != null)
            {
                upModel = JsonConvert.DeserializeObject<clsUserProfileModel>(HttpContext.Session.GetString("UserProfileModel")!)!;
            }
            upModel.countryStateDetails = GetCountryStateCity();

            return View("UserProfile", upModel);
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
    }
}
