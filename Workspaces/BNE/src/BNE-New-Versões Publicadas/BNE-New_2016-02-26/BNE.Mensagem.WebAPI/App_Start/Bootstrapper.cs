using BNE.Mensagem.WebAPI.Autofac;
using BNE.Mensagem.WebAPI.Mappers;

namespace BNE.Mensagem.WebAPI
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