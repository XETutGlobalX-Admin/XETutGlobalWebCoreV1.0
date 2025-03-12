using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XEtutGlobalX.Modal.Material
{
    public class Designation
    {
        public int Des_ID { get; set; }
        public string? Designation_Code { get; set; }
        public string? Designation_Name { get; set; }
        public int PriorityNumber { get; set; }
        public bool IsVisible { get; set; }
    }
}
