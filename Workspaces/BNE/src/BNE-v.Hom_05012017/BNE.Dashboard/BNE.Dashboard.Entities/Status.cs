using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.Dashboard.Entities
{
    public class Status
    {

        public string Name { get; set; }
        public int StatusId { get; set; }

        public static readonly Status OK = new Status(1, "OK");
        public static readonly Status ERROR = new Status(2, "ERROR");

        private Status(int value, String name)
        {
            this.Name = name;
            this.StatusId = value;
        }

        public Status() { }

        public override String ToString()
        {
            return Name;
        }

    }
}
