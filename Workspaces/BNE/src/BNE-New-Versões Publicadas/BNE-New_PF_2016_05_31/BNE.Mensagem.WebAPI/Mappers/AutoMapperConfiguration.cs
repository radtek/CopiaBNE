namespace BNE.Mensagem.WebAPI.Mappers
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            AutoMapper.Mapper.Initialize(n =>
            {
                   n.AddProfile<WebApiModelToDomain>();
            });
        }
    }
}