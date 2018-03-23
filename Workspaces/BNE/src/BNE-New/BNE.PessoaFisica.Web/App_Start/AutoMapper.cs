using AutoMapper;

namespace BNE.PessoaFisica.Web
{
    /// <summary>
    /// 
    /// </summary>
    public static class AutoMapper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IMapper Register()
        {
            var config = new MapperConfiguration(cfg =>
            {
                //Web Models -> API Models
                cfg.CreateMap<Models.Curriculo, API.Models.SalvarCurriculoCommand>();
                cfg.CreateMap<Models.ExperienciaProfissional, API.Models.SalvarExperienciaProfissionalCommand>();
                cfg.CreateMap<Models.Formacao, API.Models.SalvarFormacaoCommand>();
                cfg.CreateMap<Models.Curriculo, Models.PreCurriculo>();
                cfg.CreateMap<Models.PreCurriculo, API.Models.SalvarPreCurriculoCommand>();
                cfg.CreateMap<Models.Vaga, API.Models.SalvarCurriculoVagaCommand>();
                cfg.CreateMap<Models.Pergunta, API.Models.Pergunta>();
                cfg.CreateMap<Models.Indicacao, API.Models.IndicarAmigosCommand>();
                cfg.CreateMap<Models.AmigoIndicado, API.Models.IndicarAmigosAmigoIndicadoCommand>();
                cfg.CreateMap<Models.DadosEmpresa, API.Models.DadosEmpresaCommand>();

                //API Models -> Web Models
                cfg.CreateMap<API.Models.VagaResponse, Models.Vaga>()
                    .ForMember(dest => dest.DataAnuncio, opt => opt.MapFrom(s => s.DataAnuncio.Value));
                cfg.CreateMap<API.Models.InformacaoCurriculoResponse, Models.InformacaoCurriculo>()
                    .ForMember(dest => dest.DataNaoTemExperiencia, opt => opt.MapFrom(s => s.DataNaoTemExperiencia));

                cfg.CreateMap<API.Models.CandidaturaDegustacao, Models.CandidaturaDegustacao>();
                cfg.CreateMap<API.Models.PlanoResponse, Models.PlanoPremium>();
                cfg.CreateMap<API.Models.VagaPerguntaResponse, Models.Pergunta>();
                cfg.CreateMap<API.Models.NavegacaoVagaResponse, Models.NavegacaoVaga>()
                    .ForMember(dest => dest.UrlVagaAnterior, opt => opt.MapFrom(s => s.URLVagaAnterior))
                    .ForMember(dest => dest.UrlVagaProxima, opt => opt.MapFrom(s => s.URLVagaProxima));
            });

            return config.CreateMapper();
        }
    }
}