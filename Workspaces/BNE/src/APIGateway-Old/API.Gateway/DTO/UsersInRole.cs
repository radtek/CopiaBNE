using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Gateway.DTO
{
    public class UsersInRole
    {
        public string Id { get; set; }
        public List<string> EnrolledUsers { get; set; }
        public List<string> RemovedUsers { get; set; }
    }
}