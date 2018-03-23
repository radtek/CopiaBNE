using System;
using BNE.PessoaJuridica.Domain.Repositories;

namespace BNE.PessoaJuridica.Domain.Services
{
    public class EmailService
    {

        private readonly IEmailRepository _emailRepository;

        public EmailService(IEmailRepository emailRepository)
        {
            _emailRepository = emailRepository;
        }

        public Model.Email GetByPessoaJuridica(Model.PessoaJuridica objPessoaJuridica)
        {
            return _emailRepository.Get(n => n.PessoaJuridica.Id == objPessoaJuridica.Id);
        }

        public Model.Email GetByUsuario(Model.Usuario objUsuario)
        {
            return _emailRepository.Get(n => n.Usuario.Id == objUsuario.Id);
        }

        public Model.Email GetByUsuarioPessoaJuridica(Model.UsuarioPessoaJuridica objUsuarioPessoaJuridica)
        {
            return _emailRepository.Get(n => n.UsuarioPessoaJuridica.Usuario.Id == objUsuarioPessoaJuridica.Usuario.Id && n.UsuarioPessoaJuridica.PessoaJuridica.Id == objUsuarioPessoaJuridica.PessoaJuridica.Id);
        }

        public void Atualizar(Model.Email objEmail)
        {
            _emailRepository.Update(objEmail);
        }

        public void Adicionar(Model.Email objEmail)
        {
            _emailRepository.Add(objEmail);
        }

        public void Excluir(Model.Email objEmail)
        {
            _emailRepository.Delete(objEmail);
        }

        public Model.Email SalvarEmail(Model.PessoaJuridica objPessoaJuridica, string email)
        {
            return Salvar(objPessoaJuridica, email);
        }

        public Model.Email SalvarEmail(Model.Usuario objUsuario, string email)
        {
            return Salvar(objUsuario, email);
        }

        public Model.Email SalvarEmail(Model.UsuarioPessoaJuridica objUsuarioPessoaJuridica, string email)
        {
            return Salvar(objUsuarioPessoaJuridica, email);
        }

        private Model.Email Salvar(object objGeneric, string email)
        {
            if (email == null)
                email = string.Empty;

            email = email.Trim();

            Model.Email objEmail = null;

            var objPessoaJuridica = objGeneric as Model.PessoaJuridica;
            if (objPessoaJuridica != null)
                objEmail = GetByPessoaJuridica(objPessoaJuridica);

            var objUsuario = objGeneric as Model.Usuario;
            if (objUsuario != null)
                objEmail = GetByUsuario(objUsuario);

            var objUsuarioPessoaJuridica = objGeneric as Model.UsuarioPessoaJuridica;
            if (objUsuarioPessoaJuridica != null)
                objEmail = GetByUsuarioPessoaJuridica(objUsuarioPessoaJuridica);

            if (objEmail == null) //Se não existe e-mail
            {
                if (!string.IsNullOrWhiteSpace(email)) //O email foi informado
                {
                    objEmail = new Model.Email
                    {
                        DataCadastro = DateTime.Now,
                        Endereco = email,
                        PessoaJuridica = objPessoaJuridica,
                        Usuario = objUsuario,
                        UsuarioPessoaJuridica = objUsuarioPessoaJuridica
                    };
                    Adicionar(objEmail);
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(email)) //Email não informado então está deletando a informação
                {
                    Excluir(objEmail);
                    objEmail = null;
                }
                else if (objEmail.Endereco != email)//Atualizando
                {
                    objEmail.Endereco = email;
                    Atualizar(objEmail);
                }
            }

            return objEmail;
        }

    }
}
