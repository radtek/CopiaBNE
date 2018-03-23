using AutoMapper;

namespace BNE.PessoaFisica.Domain.Configuration
{
    public static class AutoMapper
    {

        public static void Register(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Command.SalvarCurriculoCommand, Mapper.Models.PessoaFisica.Curriculo>();
            cfg.CreateMap<Command.SalvarCurriculoCommand, Mapper.Models.PessoaFisica.PessoaFisica>();
            cfg.CreateMap<Command.SalvarExperienciaProfissionalCommand, Mapper.Models.PessoaFisica.ExperienciaProfissional>();
            cfg.CreateMap<Command.SalvarFormacaoCommand, Mapper.Models.PessoaFisica.Formacao>();
            cfg.CreateMap<Command.SalvarIndicarAmigoCommand, Mapper.Models.Indicacao.Indicacao>();
            cfg.CreateMap<Command.SalvarIndicarAmigoIndicadoCommand, Mapper.Models.Indicacao.AmigoIndicado>();
        }
    }
}
