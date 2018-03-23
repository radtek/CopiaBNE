using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Gateway.DTO
{
    public class RemoteResult
    {
        private string _creationDate;
        public string CreationDate { get { return _creationDate; } }

        private object _content;
        public object Content { get { return _content; } }

        private string _message;
        public string Message { get { return _message; } }


        public RemoteResult(object content, string message)
        {
            _creationDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            _content = content;
            _message = message;
        }
    }
}