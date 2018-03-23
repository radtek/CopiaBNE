using AutoMapper;

namespace BNE.PessoaJuridica.Domain.AutoMapper.Profiles
{
    public class UsuarioPessoaJuridicaToUsuario : Profile
    {
        /// <summary>
        ///     Configuracao do mapeamento.
        /// </summary>
        public UsuarioPessoaJuridicaToUsuario()
        {
            CreateMap<Command.UsuarioPessoaJuridica, Mapper.Models.PessoaJuridica.Usuario>();
        }
    }
}