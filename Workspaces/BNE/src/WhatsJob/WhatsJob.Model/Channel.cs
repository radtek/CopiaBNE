using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsJob.Model
{
    public class Channel
    {

        public string Number { get; set; }

        public Boolean Ativo { get; set; }

        public string Password { get; set; }

        public string NickName { get; set; }

        public string NextChallenge { get; set; }

    }
}
