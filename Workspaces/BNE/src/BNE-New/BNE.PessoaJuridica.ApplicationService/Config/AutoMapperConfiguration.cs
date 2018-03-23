using AutoMapper;
using BNE.PessoaJuridica.ApplicationService.PessoaJuridica.AutoMapper.Profiles;
using BNE.PessoaJuridica.Domain.AutoMapper.Profiles;

namespace BNE.PessoaJuridica.ApplicationService.Config
{
    public static class AutoMapperConfiguration
    {

        private static MapperConfiguration _instance;

        /// <summary>
        ///     Obtem uma Instancia do MapperConfiguration
        /// </summary>
        /// <returns></returns>
        public static MapperConfiguration GetInstance()
        {
            return _instance ?? (_instance = Configure());
        }

        public static MapperConfiguration Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<WebApiModelToDomain>();
                cfg.AddProfile<CadastroPessoaJuridicaToPessoaJuridica>();
                cfg.AddProfile<UsuarioAdicionalPessoaJuridicaToUsuarioAdicional>();
                cfg.AddProfile<UsuarioPessoaJuridicaToUsuario>();
                cfg.AddProfile<CriarOuAtualizarUsuarioEmpresaToPessoaJuridica>();
                cfg.AddProfile<CriarOuAtualizarUsuarioEmpresaToUsuario>();
            });
            return config;
        }
    }
}