using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWDB.Core
{
    public class Product
    {
        public string ProductName { get; set; }
        public int ProductID { get; set; }
        public string Tag { get; set; }
        public int TagID { get; set; }
        public int FamilyID { get; set; }
        public int Year { get; set; }
        public string Family { get; set; }
    }
    
}
