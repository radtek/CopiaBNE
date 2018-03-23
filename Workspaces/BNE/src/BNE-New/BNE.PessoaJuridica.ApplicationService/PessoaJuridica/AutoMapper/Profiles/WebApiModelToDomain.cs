using System;
using AutoMapper;
using BNE.PessoaJuridica.ApplicationService.PessoaJuridica.Command;

namespace BNE.PessoaJuridica.ApplicationService.PessoaJuridica.AutoMapper.Profiles
{
    public class WebApiModelToDomain : Profile
    {
        public WebApiModelToDomain()
        {
            CreateMap<CadastroEmpresa, Domain.Command.CadastroPessoaJuridica>()
                .ForMember(n => n.NumeroCNPJ, opt => opt.MapFrom(s => Convert.ToDecimal(Core.Common.Utils.LimparMascaraCPFCNPJCEP(s.NumeroCNPJ))))
                .ForMember(n => n.CEP, opts => opts.ResolveUsing(s => string.IsNullOrWhiteSpace(s.CEP) ? 0 : Int32.Parse(Core.Common.Utils.LimparMascaraCPFCNPJCEP(s.CEP))))
                .ForMember(n => n.NumeroDDDComercial, opts => opts.MapFrom(s => Core.Common.Utils.LimparMascaraTelefone(s.NumeroComercial).Substring(0, 2)))
                .ForMember(n => n.NumeroComercial, opts => opts.MapFrom(s => Convert.ToDecimal(Core.Common.Utils.LimparMascaraTelefone(s.NumeroComercial).Substring(2))));

            CreateMap<CadastroUsuarioEmpresa, Domain.Command.UsuarioPessoaJuridica>()
                .ForMember(dest => dest.NumeroCPF, opt => opt.MapFrom(s => Convert.ToDecimal(Core.Common.Utils.LimparMascaraCPFCNPJCEP(s.NumeroCPF))))
                .ForMember(dest => dest.NumeroDDDCelular, opt => opt.MapFrom(s => Core.Common.Utils.LimparMascaraTelefone(s.NumeroCelular).Substring(0, 2)))
                .ForMember(dest => dest.NumeroCelular, opt => opt.MapFrom(s => Convert.ToDecimal(Core.Common.Utils.LimparMascaraTelefone(s.NumeroCelular).Substring(2))))
                .ForMember(dest => dest.NumeroDDDComercial, opt => opt.ResolveUsing(s => s.NumeroComercial == null ? string.Empty : Core.Common.Utils.LimparMascaraTelefone(s.NumeroComercial).Substring(0, 2)))
                .ForMember(dest => dest.NumeroComercial, opt => opt.ResolveUsing(s => s.NumeroComercial == null ? Decimal.Zero : Convert.ToDecimal(Core.Common.Utils.LimparMascaraTelefone(s.NumeroComercial).Substring(2))));

            CreateMap<CadastroUsuarioAdicionalEmpresa, Domain.Command.UsuarioAdicionalPessoaJuridica>();

            CreateMap<CadastroUsuarioEmpresa, Domain.Command.CriarOuAtualizarUsuarioEmpresa>()
                .ForMember(dest => dest.NumeroCPF, opt => opt.MapFrom(s => Convert.ToDecimal(Core.Common.Utils.LimparMascaraCPFCNPJCEP(s.NumeroCPF))))
                .ForMember(dest => dest.NumeroDDDCelular, opt => opt.MapFrom(s => Core.Common.Utils.LimparMascaraTelefone(s.NumeroCelular).Substring(0, 2)))
                .ForMember(dest => dest.NumeroCelular, opt => opt.MapFrom(s => Convert.ToDecimal(Core.Common.Utils.LimparMascaraTelefone(s.NumeroCelular).Substring(2))))
                .ForMember(dest => dest.NumeroDDDComercial, opt => opt.ResolveUsing(s => s.NumeroComercial == null ? string.Empty : Core.Common.Utils.LimparMascaraTelefone(s.NumeroComercial).Substring(0, 2)))
                .ForMember(dest => dest.NumeroComercial, opt => opt.ResolveUsing(s => s.NumeroComercial == null ? Decimal.Zero : Convert.ToDecimal(Core.Common.Utils.LimparMascaraTelefone(s.NumeroComercial).Substring(2))));
        }
    }
}