﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bne.Web.Services.API.DTO
{
    public class MailMessage
    {
        public string to { get; set; }
        public string from { get; set; }
        public string subject { get; set; }
        public string message { get; set; }
    }
}