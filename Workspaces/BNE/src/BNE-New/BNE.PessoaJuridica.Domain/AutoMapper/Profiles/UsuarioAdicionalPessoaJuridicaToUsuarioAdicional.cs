using AutoMapper;

namespace BNE.PessoaJuridica.Domain.AutoMapper.Profiles
{
    public class UsuarioAdicionalPessoaJuridicaToUsuarioAdicional : Profile
    {
        /// <summary>
        ///     Configuracao do mapeamento.
        /// </summary>
        public UsuarioAdicionalPessoaJuridicaToUsuarioAdicional()
        {
            CreateMap<Command.UsuarioAdicionalPessoaJuridica, Mapper.Models.PessoaJuridica.UsuarioAdicional>();
        }
    }
}