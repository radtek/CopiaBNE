using System;
using System.ComponentModel.DataAnnotations;

namespace Sample.Models
{
    public class OneSignalToken
    {
        [Required]
        public string User { get; set; }
        public Guid? Token { get; set; }
    }
}