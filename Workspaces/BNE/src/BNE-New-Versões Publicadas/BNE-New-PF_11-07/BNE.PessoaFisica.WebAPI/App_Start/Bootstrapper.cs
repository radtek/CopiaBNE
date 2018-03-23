using BNE.PessoaFisica.WebAPI.Autofac;
using BNE.PessoaFisica.WebAPI.Mappers;

namespace BNE.PessoaFisica.WebAPI
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