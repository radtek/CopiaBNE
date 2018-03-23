using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.Dashboard.Business
{
    public enum ServerStatus
    {
        up = 0,
        inactive = 1,
        warning = 2,
        down = 3
    }

    public class ServerItem
    {
        public DateTime date;
        public Decimal percent;
    }

    public class Message
    {
        public DateTime data;
        public String message;
    }

    public class Server
    {
        //public List<ServerItem> _cpu = new List<ServerItem>();
        //public List<ServerItem> _ram = new List<ServerItem>();

        public string applicationName;
        public string ip;
        public string color;
        public decimal lastCpu;
        public decimal lastRam;
        public bool solrCheck = false;
        public bool sqlCheck = false;
        public decimal lastSolr;
        public decimal lastSql;
        public List<Message> messages = new List<Message>();
        //public List<ServerItem> cpu {
        //    get {
        //        if(this._cpu == null)
        //            this._cpu = new List<ServerItem>();

        //        return this._cpu;
        //    }
        //    set { this._cpu = new List<ServerItem>(); }
        //}
        //public List<ServerItem> ram {
        //    get {
        //        if(this._ram == null)
        //            this._ram = new List<ServerItem>();

        //        return this._ram;
        //    }
        //    set { this._ram = new List<ServerItem>(); }
        //}
        public DateTime lastCommunication;
        public ServerStatus status = ServerStatus.up;
    }
}
