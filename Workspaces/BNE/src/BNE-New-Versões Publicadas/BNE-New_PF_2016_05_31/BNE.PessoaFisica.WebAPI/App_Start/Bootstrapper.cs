using BNE.PessoaFisica.WebAPI.Autofac;
using BNE.PessoaFisica.WebAPI.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BNE.PessoaFisica.WebAPI.App_Start
{
    public static class Bootstrapper
    {
        public static void Run()
        {
            AutofacConfiguration.Configure();
            AutoMapperConfiguration.Configure();
        }
    }
}