using BNE.PessoaJuridica.WebAPI.Autofac;
using BNE.PessoaJuridica.WebAPI.Mappers;

namespace BNE.PessoaJuridica.WebAPI
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