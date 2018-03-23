using System;

namespace BNE.PessoaJuridica.WebAPI.Mappers
{
    public class WebApiModelToDomain : AutoMapper.Profile
    {
        protected override void Configure()
        {
            AutoMapper.Mapper.CreateMap<Models.CadastroModel, Domain.Command.CriarOuAtualizarPessoaJuridica>()
                .ForAllMembers(c => c.IgnoreIfSourceIsNull());
            AutoMapper.Mapper.CreateMap<Models.CadastroModel, Domain.Command.CriarOuAtualizarPessoaJuridica>()
                .ForMember(n => n.NumeroCNPJ, opt => opt.MapFrom(s => Convert.ToDecimal(Core.Common.Utils.LimparMascaraCPFCNPJCEP(s.NumeroCNPJ))))
                .ForMember(n => n.CEP, opts => opts.ResolveUsing(s => string.IsNullOrWhiteSpace(s.CEP) ? 0 : Int32.Parse(Core.Common.Utils.LimparMascaraCPFCNPJCEP(s.CEP))))
                .ForMember(n => n.NumeroDDDComercial, opts => opts.MapFrom(s => Core.Common.Utils.LimparMascaraTelefone(s.NumeroComercial).Substring(0, 2)))
                .ForMember(n => n.NumeroComercial, opts => opts.MapFrom(s => Convert.ToDecimal(Core.Common.Utils.LimparMascaraTelefone(s.NumeroComercial).Substring(2))));

            AutoMapper.Mapper.CreateMap<Models.CadastroUsuarioModel, Domain.Command.UsuarioPessoaJuridica>()
                .ForAllMembers(c => c.IgnoreIfSourceIsNull());

            AutoMapper.Mapper.CreateMap<Models.CadastroUsuarioModel, Domain.Command.UsuarioPessoaJuridica>()
                .ForMember(dest => dest.NumeroCPF, opt => opt.MapFrom(s => Convert.ToDecimal(Core.Common.Utils.LimparMascaraCPFCNPJCEP(s.NumeroCPF))))
                .ForMember(dest => dest.NumeroDDDCelular, opt => opt.MapFrom(s => Core.Common.Utils.LimparMascaraTelefone(s.NumeroCelular).Substring(0, 2)))
                .ForMember(dest => dest.NumeroCelular, opt => opt.MapFrom(s => Convert.ToDecimal(Core.Common.Utils.LimparMascaraTelefone(s.NumeroCelular).Substring(2))))
                .ForMember(dest => dest.NumeroDDDComercial, opt => opt.ResolveUsing(s => s.NumeroComercial == null ? string.Empty : Core.Common.Utils.LimparMascaraTelefone(s.NumeroComercial).Substring(0, 2)))
                .ForMember(dest => dest.NumeroComercial, opt => opt.ResolveUsing(s => s.NumeroComercial == null ? Decimal.Zero : Convert.ToDecimal(Core.Common.Utils.LimparMascaraTelefone(s.NumeroComercial).Substring(2))));

            AutoMapper.Mapper.CreateMap<Models.CadastroUsuarioAdicionalModel, Domain.Command.UsuarioAdicionalPessoaJuridica>();

            AutoMapper.Mapper.CreateMap<Models.CadastroUsuarioModel, Domain.Command.CriarOuAtualizarUsuarioEmpresa>()
                .ForAllMembers(c => c.IgnoreIfSourceIsNull());
            AutoMapper.Mapper.CreateMap<Models.CadastroUsuarioModel, Domain.Command.CriarOuAtualizarUsuarioEmpresa>()
                .ForMember(dest => dest.NumeroCPF, opt => opt.MapFrom(s => Convert.ToDecimal(Core.Common.Utils.LimparMascaraCPFCNPJCEP(s.NumeroCPF))))
                .ForMember(dest => dest.NumeroDDDCelular, opt => opt.MapFrom(s => Core.Common.Utils.LimparMascaraTelefone(s.NumeroCelular).Substring(0, 2)))
                .ForMember(dest => dest.NumeroCelular, opt => opt.MapFrom(s => Convert.ToDecimal(Core.Common.Utils.LimparMascaraTelefone(s.NumeroCelular).Substring(2))))
                .ForMember(dest => dest.NumeroDDDComercial, opt => opt.ResolveUsing(s => s.NumeroComercial == null ? string.Empty : Core.Common.Utils.LimparMascaraTelefone(s.NumeroComercial).Substring(0, 2)))
                .ForMember(dest => dest.NumeroComercial, opt => opt.ResolveUsing(s => s.NumeroComercial == null ? Decimal.Zero : Convert.ToDecimal(Core.Common.Utils.LimparMascaraTelefone(s.NumeroComercial).Substring(2))));

            AutoMapper.Mapper.CreateMap<Models.CadastroReceita, Domain.Command.CriarSolicitacaoReceita>();
        }
    }

    public static class ConditionExtensions
    {
        public static void IgnoreIfSourceIsNull<T>(this AutoMapper.IMemberConfigurationExpression<T> expression)
        {
            expression.Condition(IgnoreIfSourceIsNull);
        }

        static bool IgnoreIfSourceIsNull(AutoMapper.ResolutionContext context)
        {
            if (!context.IsSourceValueNull)
            {
                return true;
            }
            var result = context.GetContextPropertyMap().ResolveValue(context.Parent);
            return result.Value != null;
        }
    }
}