using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySQL_Client
{
    class CustomerModel
    {
        public string firstname { get; set; } = null!;
        public string lastname { get; set; } = null!;
        public int age { get; set; } = 0;
        public string city { get; set; } = null!;
        public string firma { get; set; } = null!;
    }
}
