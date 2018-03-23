using BNE.Vaga.WebAPI.Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BNE.Vaga.WebAPI.App_Start
{
    public static class Bootstrapper
    {
        public static void Run()
        {
            AutofacConfiguration.Configure();
        }
    }
}