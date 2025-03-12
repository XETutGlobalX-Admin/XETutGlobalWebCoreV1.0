using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XEtutGlobalX.Modal.XETutGlobalX_DB.Request
{
    public class Registration_RequestBody
    {
        public Personal? Personal { get; set; }
        public Contact? Contact { get; set; }
        public LoginCredential? LoginCredential { get; set; }
    }
    // Registration_RequestBody myDeserializedClass = JsonConvert.DeserializeObject<Registration_RequestBody>(myJsonResponse);
    public class Contact
    {
        public Item? Item { get; set; }
    }

    public class Item
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }

        public string EmailId { get; set; }
        public string dateOfBirth { get; set; }
        public string desig_Code { get; set; }
        public string desig_Name { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string postalCode { get; set; }
        public string currentAddress { get; set; }
        public string phonecountrycode { get; set; }
        public string phoneNumber { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string confirm_password { get; set; }
    }

    public class LoginCredential
    {
        public Item? Item { get; set; }
    }

    public class Personal
    {
        public Item? Item { get; set; }
    }

    
}
