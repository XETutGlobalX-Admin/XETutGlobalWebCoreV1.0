using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XEtutGlobalX.Modal.XETutGlobalX_DB.Response
{
    public class WebloginValidation
    {
        public bool LoginStatus { get; set; }
        public string? Message { get; set; }
        public int UserKey { get; set; }
        public string? GUID { get; set; }
        public SendOTPMailResponse? sendOTPMailResponse { get; set; }
        
    }
    public class SendOTPMailResponse
    {
        public bool Status { get; set; }
        public string? Message { get; set; }

    }
}
