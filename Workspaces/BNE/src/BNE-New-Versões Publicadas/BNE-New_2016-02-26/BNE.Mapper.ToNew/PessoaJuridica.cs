using System;

namespace BNE.Mapper.ToNew
{
    public class PessoaJuridica : MapperBase
    {

        public bool Map(Models.PessoaJuridica.PessoaJuridica commonObject)
        {
            try
            {
                AutoMapper.Mapper.CreateMap<Models.PessoaJuridica.PessoaJuridica, BNE.PessoaJuridica.Domain.Command.CriarOuAtualizarPessoaJuridicaDoVelho>();
                AutoMapper.Mapper.CreateMap<Models.PessoaJuridica.Usuario, BNE.PessoaJuridica.Domain.Command.UsuarioPessoaJuridicaDoVelho>();

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
