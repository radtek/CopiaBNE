using AutoMapper;

namespace BNE.PessoaJuridica.Domain.AutoMapper.Profiles
{
    public class CriarOuAtualizarUsuarioEmpresaToUsuario : Profile
    {
        /// <summary>
        ///     Configuracao do mapeamento.
        /// </summary>
        public CriarOuAtualizarUsuarioEmpresaToUsuario()
        {
            CreateMap<Command.CriarOuAtualizarUsuarioEmpresa, Mapper.Models.PessoaJuridica.Usuario>();
        }
    }
}