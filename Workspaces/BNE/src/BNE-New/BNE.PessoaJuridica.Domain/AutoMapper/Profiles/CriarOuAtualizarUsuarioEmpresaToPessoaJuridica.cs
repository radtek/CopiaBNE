using AutoMapper;

namespace BNE.PessoaJuridica.Domain.AutoMapper.Profiles
{
    public class CriarOuAtualizarUsuarioEmpresaToPessoaJuridica : Profile
    {
        /// <summary>
        ///     Configuracao do mapeamento.
        /// </summary>
        public CriarOuAtualizarUsuarioEmpresaToPessoaJuridica()
        {
            CreateMap<Command.CriarOuAtualizarUsuarioEmpresa, Mapper.Models.PessoaJuridica.PessoaJuridica>();
        }
    }
}