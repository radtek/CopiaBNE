using AutoMapper;

namespace BNE.PessoaJuridica.Domain.AutoMapper.Profiles
{
    public class CadastroPessoaJuridicaToPessoaJuridica : Profile
    {
        /// <summary>
        ///     Configuracao do mapeamento.
        /// </summary>
        public CadastroPessoaJuridicaToPessoaJuridica()
        {
            CreateMap<Command.CadastroPessoaJuridica, Mapper.Models.PessoaJuridica.PessoaJuridica>();
        }
    }
}