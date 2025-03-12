using XEtutGlobalX.Modal.XETutGlobalX_DB.Request;
using XETutGlobalX.DAL.XETutGlobalX_DB;

namespace XETutGlobalXAppV1.AppCode.XETutGlobalX.Authentication
{
    public class Authentication_Process
    {
        public static clsRegistration_RQ RegisterUserRQ(List<Registration_RequestBody> registration_RequestBody)
        {
            clsRegistration_RQ clsRegistration_RQ = new clsRegistration_RQ();
            Registration_Request registration_Request = new Registration_Request();
            registration_Request.RequestBody = registration_RequestBody;
            clsRegistration_RQ._Request = registration_Request;
            return clsRegistration_RQ;
        }
    }

}
