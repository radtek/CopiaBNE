
namespace BNE.Mensagem.WebAPI.Mappers
{
    public class WebApiModelToDomain : AutoMapper.Profile
    {
        protected override void Configure()
        {


            AutoMapper.Mapper.CreateMap<Models.EnviarSMS, Domain.Command.EnviarSMS>();
            //TODO: Não consegui resolver com automapper tipos dinamicos
            AutoMapper.Mapper.CreateMap<Models.EnviarEmail, Domain.Command.EnviarEmail>()
                .ForMember(s => s.Parametros, options => options.Ignore());

            //AutoMapper.Mapper.CreateMap<Newtonsoft.Json.Linq.JObject, Newtonsoft.Json.Linq.JObject>().ConvertUsing(value =>
            //{
            //    if (value == null)
            //        return null;

            //    return new Newtonsoft.Json.Linq.JObject(value);
            //});

            //.ForMember(s => s.Parametros, m => m.MapFrom(q => AutoMapper.Mapper.Map<JObject, JObject>(q.Parametros)));

            //.ConvertUsing<JObject, JObject>(value =>
            //{
            //    if (value == null)
            //        return null;

            //    return new JObject(value);
            //});

            //AutoMapper.Mapper.DynamicMap<JObject, JObject>().ConvertUsing(value =>
            //{
            //    if (value == null)
            //        return null;

            //    return new JObject(value);
            //});

            //AutoMapper.Mapper.CreateMap<JObject, JObject>().ConvertUsing(value =>
            //{
            //    if (value == null)
            //        return null;

            //    return new JObject(value);
            //});

        }

        private object ResolveJObject(AutoMapper.ResolutionResult result)
        {
            var source = result.Context.SourceValue as Models.EnviarEmail;

            if (result.Context.IsSourceValueNull || source == null || !(source.Parametros is Newtonsoft.Json.Linq.JObject))
            {
                return null;
            }
            var sourceValue = source.Parametros as Newtonsoft.Json.Linq.JObject;

            return result.Context.Engine.Map<Newtonsoft.Json.Linq.JObject>(sourceValue);
        }
    }
}