using AutoMapper;
using System;

namespace BNE.PessoaFisica.WebAPI.Mappers
{
    public class WebApiModelToDomain : Profile
    {
        protected override void Configure()
        {
            AutoMapper.Mapper.CreateMap<Models.PreCurriculo, Domain.Command.PreCurriculo>();
            AutoMapper.Mapper.CreateMap<Models.PessoaFisica, Domain.Command.PessoaFisica>()
                .ForMember(dest => dest.CPF, opt => opt.MapFrom(s => Convert.ToDecimal(Core.Common.Utils.LimparMascaraCPFCNPJCEP(s.CPF))));

            AutoMapper.Mapper.CreateMap<Models.Formacao, Domain.Command.Formacao>();

            AutoMapper.Mapper.CreateMap<Models.ExperienciaProfissional, Domain.Command.ExperienciaProfissional>()
                .ForMember(dest => dest.Cpf, opt => opt.MapFrom(s => Convert.ToDecimal(Core.Common.Utils.LimparMascaraCPFCNPJCEP(s.CPF))));
        }
    }
}