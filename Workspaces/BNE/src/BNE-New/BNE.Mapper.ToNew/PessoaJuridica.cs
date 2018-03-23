using System;

namespace BNE.Mapper.ToNew
{
    public class PessoaJuridica : MapperBase
    {
        public PessoaJuridica()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Models.PessoaJuridica.PessoaJuridica, BNE.PessoaJuridica.Domain.Command.CriarOuAtualizarPessoaJuridicaDoVelho>();
                cfg.CreateMap<Models.PessoaJuridica.Usuario, BNE.PessoaJuridica.Domain.Command.UsuarioPessoaJuridicaDoVelho>();
            });
        }
        public bool Map(Models.PessoaJuridica.PessoaJuridica commonObject)
        {
            try
            {
                var domainPessoaJuridica = DomainPessoaJuridica;

                var command = AutoMapper.Mapper.Map<Models.PessoaJuridica.PessoaJuridica, BNE.PessoaJuridica.Domain.Command.CriarOuAtualizarPessoaJuridicaDoVelho>(commonObject);

                return domainPessoaJuridica.SalvarPessoaJuridicaDoVelho(command);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
