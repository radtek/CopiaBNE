using AutoMapper;

namespace BNE.PessoaFisica.WebAPI.Mappers
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(n =>
            {
                n.AddProfile<WebApiModelToDomain>();
            });
        }
    }
}