using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using BNE.Web.Vip.Models;


namespace BNE.Web.Vip.Controllers
{
    public class zMeController : BaseAPIController
    {

        public GetViewModel Get()
        {
            return new GetViewModel() { Hometown = "HomeTown" };
        }

        public GetViewModel Valid(int test)
        {
            return new GetViewModel() { Hometown = "HomeTown" };
        }
    }
}