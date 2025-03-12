using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XEtutGlobalX.Modal.Material;

namespace XEtutGlobalX.Modal.XETutGlobalX_DB.Response.UserProfile
{
   
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class clsUserProfileModel
    {
        public List<UserProfile>? UserProfile { get; set; }
        public CountryStateDetails? countryStateDetails {  get; set; }
    }

    public class UserProfile
    {
        public int UID { get; set; }
        public string? FName { get; set; }
        public string? MName { get; set; }
        public string? LName { get; set; }
        public string? Gender { get; set; }
        public string? LoginType { get; set; }
        public object? ProfilePhoto { get; set; }
        public string? UserName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DOB { get; set; }
        public string? Designation_Code { get; set; }
        public string? Designation { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Pincode { get; set; }
        public string? Address { get; set; }
        public string? phone_code { get; set; }
        public string? Contact { get; set; }
        public string? EmailID { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public bool IsPasswordChanged { get; set; }
        public int WrongAttemptCount { get; set; }
    }


}
