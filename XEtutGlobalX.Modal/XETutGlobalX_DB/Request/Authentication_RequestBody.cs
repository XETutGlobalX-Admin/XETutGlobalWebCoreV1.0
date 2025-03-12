using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XEtutGlobalX.Modal.XETutGlobalX_DB.Request
{
    public class Authentication_RequestBody
    {
        public string? UserName { get; set; }
        public string? EmailID { get; set; }
        public string? Password { get; set; }
    }
}
