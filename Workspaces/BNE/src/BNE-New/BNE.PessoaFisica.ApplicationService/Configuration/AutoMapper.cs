using System;
using System.Runtime.InteropServices;
using AutoMapper;

namespace BNE.PessoaFisica.ApplicationService.Configuration
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
                //ApplicationServiceCommand to DomainCommand
                cfg.CreateMap<Curso.Command.GetCursoCommand, Domain.Command.GetCursoCommand>();
                cfg.CreateMap<Curriculo.Command.SalvarCurriculoCommand, Domain.Command.SalvarCandidatarCurriculoCommand>()
                    .ForMember(dest => dest.CPF, opt => opt.MapFrom(s => Convert.ToDecimal(Core.Common.Utils.LimparMascaraCPFCNPJCEP(s.CPF))));
                cfg.CreateMap<PreCurriculo.Command.SalvarPreCurriculoCommand, Domain.Command.SalvarPreCurriculoCommand>()
                    .ForMember(n => n.DDDCelular, opts => opts.MapFrom(s => string.IsNullOrWhiteSpace(s.Celular) ? string.Empty : Core.Common.Utils.LimparMascaraTelefone(s.Celular).Substring(0, 2)))
                    .ForMember(n => n.NumeroCelular, opts => opts.MapFrom(s => string.IsNullOrWhiteSpace(s.Celular) ? string.Empty : Core.Common.Utils.LimparMascaraTelefone(s.Celular).Substring(2)));
                cfg.CreateMap<Curriculo.Command.SalvarCurriculoCommand, Domain.Command.SalvarCurriculoCommand>()
                    .ForMember(dest => dest.CPF, opt => opt.MapFrom(s => Convert.ToDecimal(Core.Common.Utils.LimparMascaraCPFCNPJCEP(s.CPF))));
                cfg.CreateMap<Plano.Command.GetPlanoCommand, Domain.Command.GetPlanoCommand>();
                cfg.CreateMap<Curriculo.Command.Pergunta, Domain.Command.SalvarCandidatarPreCurriculoSalvarPerguntaCommand>();
                cfg.CreateMap<Curriculo.Command.SalvarExperienciaProfissionalCommand, Domain.Command.SalvarExperienciaProfissionalCommand>();
                cfg.CreateMap<Curriculo.Command.SalvarFormacaoCommand, Domain.Command.SalvarFormacaoCommand>();
                cfg.CreateMap<PessoFisica.Command.IndicarAmigosCommand, Domain.Command.SalvarIndicarAmigoCommand>()
                    .ForMember(dest => dest.CPF, opt => opt.MapFrom(s => Convert.ToDecimal(Core.Common.Utils.LimparMascaraCPFCNPJCEP(s.CPF))));
                cfg.CreateMap<PessoFisica.Command.IndicarAmigosAmigoIndicadoCommand, Domain.Command.SalvarIndicarAmigoIndicadoCommand>();

                //DomainModel to ApplicationServiceModel
                cfg.CreateMap<Domain.Model.DadosEmpresa, DadosEmpresa.Model.DadosEmpresaResponse>();
                cfg.CreateMap<Domain.Model.PreCurriculo, PreCurriculo.Model.PreCurriculoResponse>();
                cfg.CreateMap<Domain.Model.Curso, Curso.Model.CursoResponse>();
                cfg.CreateMap<Domain.Model.Vaga, Vaga.Model.VagaResponse>();
                cfg.CreateMap<Domain.Model.Plano, Plano.Model.PlanoResponse>();
                cfg.CreateMap<Domain.Aggregates.InformacaoCurriculo, Curriculo.Model.InformacaoCurriculoResponse>();
                cfg.CreateMap<Domain.Aggregates.InformacaoCurriculo, Curriculo.Model.InformacaoCurriculoResponse>();
                cfg.CreateMap<Domain.Aggregates.Candidatura, Curriculo.Model.CandidaturaResponse>();
                cfg.CreateMap<Domain.Model.VagaPergunta, VagaPergunta.Model.VagaPerguntaResponse>();

                cfg.CreateMap<SolrService.Model.Vaga, Domain.Services.SOLR.Vaga>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url))
                    .ForAllOtherMembers(c => c.Ignore());

                Domain.Configuration.AutoMapper.Register(cfg);

            });
            return config.CreateMapper();
        }
    }
}