using API.Gateway.DTO;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Routing;

namespace API.Gateway.Auth
{
    public class ModelFactory
    {
        private UrlHelper _UrlHelper;

        public ModelFactory() 
        {
            _UrlHelper = new UrlHelper();
        }

        //Code removed for brevity
        public RoleReturn Create(IdentityRole appRole)
        {
            return new RoleReturn
            {
                Url = _UrlHelper.Link("GetRoleById", new { id = appRole.Id }),
                Id = appRole.Id,
                Name = appRole.Name
            };
        }
    }
}