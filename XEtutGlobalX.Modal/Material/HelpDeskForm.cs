using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XEtutGlobalX.Modal.Material
{
    public class HelpDeskForm
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Root
        {
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string? BirthDate { get; set; }
            public string? Query { get; set; }
            public string? Address { get; set; }
            public string? Phone { get; set; }
            public string? Email { get; set; }
        }


    }
}
