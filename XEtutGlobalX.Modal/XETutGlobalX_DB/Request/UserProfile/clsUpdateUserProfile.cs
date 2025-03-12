using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XEtutGlobalX.Modal.XETutGlobalX_DB.Request.UserProfile
{
    public class clsUpdateUserProfile
    {
        public List<PersonalInfo>? PersonalInfo { get; set; }
        public List<ContactInfo>? ContactInfo { get; set; }
    }



    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ContactInfo
    {
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? PinCode { get; set; }
        public string? CurrentAddr { get; set; }
        public string? Phone_Code { get; set; }

        public string? Phone_Number { get; set; }
    }

    public class PersonalInfo
    {
        public string? UserID { get; set; }
        public string? profilePhoto { get; set; }
        public string? firstName { get; set; }
        public string? middleName { get; set; }
        public string? lastName { get; set; }
        public string? Gender { get; set; }
        public string? login_emailID { get; set; }
        public string? DOB { get; set; }
        public string? LoggedInType { get; set; }
        public string? LoggedDesignation { get; set; }
    }

    
}
