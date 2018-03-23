using System;
using BNE.PessoaJuridica.Data.Repositories;

namespace BNE.PessoaJuridica.Domain
{
    public class UsuarioPessoaJuridica
    {

        private readonly IUsuarioPessoaJuridicaRepository _usuarioPessoaJuridicaRepository;
        private readonly Perfil _perfil;
        private readonly TelefoneComercial _telefoneComercial;
        private readonly TelefoneCelular _telefoneCelular;
        private readonly Email _email;
        private readonly Global.Domain.FuncaoSinonimo _funcaoSinonimo;

        public UsuarioPessoaJuridica(IUsuarioPessoaJuridicaRepository usuarioPessoaJuridicaRepository, Global.Domain.FuncaoSinonimo funcaoSinonimo, Perfil perfil, TelefoneComercial telefoneComercial, TelefoneCelular telefoneCelular, Email email)
        {
            _usuarioPessoaJuridicaRepository = usuarioPessoaJuridicaRepository;

            _funcaoSinonimo = funcaoSinonimo;

            _perfil = perfil;
            _telefoneComercial = telefoneComercial;
            _telefoneCelular = telefoneCelular;
            _email = email;
        }

        public Model.UsuarioPessoaJuridica GetByUsuarioPessoaJuridica(Model.Usuario objUsuario, Model.PessoaJuridica objPessoaJuridica)
        {
            return _usuarioPessoaJuridicaRepository.Get(n => n.IdUsuario == objUsuario.Id && n.IdPessoaJuridica == objPessoaJuridica.Id);
        }

        private void Atualizar(Model.UsuarioPessoaJuridica objUsuarioPessoaJuridica)
        {
            _usuarioPessoaJuridicaRepository.Update(objUsuarioPessoaJuridica);
        }

        private void Adicionar(Model.UsuarioPessoaJuridica objUsuarioPessoaJuridica)
        {
            _usuarioPessoaJuridicaRepository.Add(objUsuarioPessoaJuridica);
        }

        public bool UsuarioPodeEditarEmpresa(Model.UsuarioPessoaJuridica objUsuarioPessoaJuridica)
        {
            return objUsuarioPessoaJuridica.Perfil.Id == (int)Model.Enumeradores.Perfil.Master;
        }

        public Model.UsuarioPessoaJuridica Salvar(Model.Usuario objUsuario, Model.PessoaJuridica objPessoaJuridica, Model.Enumeradores.Perfil perfil, string funcao, string dddCelular, decimal numeroCelular, string dddComercial, decimal numeroComercial, decimal numeroRamal, string email, string IP, DateTime? dataCadastro = null, int? idFuncaoVelho = null)
        {
            var data = dataCadastro ?? DateTime.Now;

            var objUsuarioPessoaJuridica = GetByUsuarioPessoaJuridica(objUsuario, objPessoaJuridica);

            if (objUsuarioPessoaJuridica == null)
            {
                objUsuarioPessoaJuridica = new Model.UsuarioPessoaJuridica
                {
                    PessoaJuridica = objPessoaJuridica,
                    Usuario = objUsuario,
                    Perfil = _perfil.GetById((int)perfil),
                    DataCadastro = data,
                    Guid = Guid.NewGuid(),
                    IP = IP
                };
                Adicionar(objUsuarioPessoaJuridica);
            }
            else
                Atualizar(objUsuarioPessoaJuridica);

            objUsuarioPessoaJuridica.DataAlteracao = data;
            //objUsuarioPessoaJuridica.PessoaJuridica = objPessoaJuridica;
            objUsuarioPessoaJuridica.FuncaoSinonimo = null;
            objUsuarioPessoaJuridica.Funcao = null;

            var objFuncao = _funcaoSinonimo.GetByNome(funcao);
            if (objFuncao != null)
                objUsuarioPessoaJuridica.FuncaoSinonimo = objFuncao;
            else if (idFuncaoVelho.HasValue)
            {
                objFuncao = _funcaoSinonimo.GetByIdSubstituto(idFuncaoVelho.Value);
                if (objFuncao != null)
                    objUsuarioPessoaJuridica.FuncaoSinonimo = objFuncao;
                else
                    objUsuarioPessoaJuridica.Funcao = funcao;
            }
            else
                objUsuarioPessoaJuridica.Funcao = funcao;

            _email.SalvarEmail(objUsuarioPessoaJuridica, email);
            _telefoneCelular.SalvarTelefone(objUsuarioPessoaJuridica, string.IsNullOrWhiteSpace(dddCelular) ? (byte?)null : Convert.ToByte(dddCelular), numeroCelular);
            _telefoneComercial.SalvarTelefone(objUsuarioPessoaJuridica, string.IsNullOrWhiteSpace(dddComercial) ? (byte?)null : Convert.ToByte(dddComercial), numeroComercial, numeroRamal);

            return objUsuarioPessoaJuridica;
        }
    }
}
