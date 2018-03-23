using System;
using BNE.Core.Common;
using BNE.PessoaJuridica.Data.Repositories;
using BNE.PessoaJuridica.Domain.Exceptions;

namespace BNE.PessoaJuridica.Domain
{
    public class Usuario
    {

        private readonly IUsuarioRepository _usuarioRepository;
        private readonly Global.Domain.Sexo _sexo;
        private readonly TelefoneComercial _telefoneComercial;
        private readonly TelefoneCelular _telefoneCelular;

        public Usuario(IUsuarioRepository usuarioRepository, Global.Domain.Sexo sexo, TelefoneComercial telefoneComercial, TelefoneCelular telefoneCelular)
        {
            _usuarioRepository = usuarioRepository;

            _sexo = sexo;

            _telefoneComercial = telefoneComercial;
            _telefoneCelular = telefoneCelular;
        }

        #region GetByCPF
        private Model.Usuario GetByCPF(decimal numeroCPF)
        {
            return _usuarioRepository.Get(n => n.CPF == numeroCPF);
        }
        #endregion

        #region Salvar

        /// <summary>
        /// Salva um usuário dados os parametros
        /// </summary>
        /// <param name="numeroCPF"></param>
        /// <param name="nome"></param>
        /// <param name="dataNascimento"></param>
        /// <param name="sexo"></param>
        /// <param name="dddCelular"></param>
        /// <param name="numeroCelular"></param>
        /// <param name="dddComercial"></param>
        /// <param name="numeroComercial"></param>
        /// <param name="ramal"></param>
        /// <param name="IP"></param>
        /// <param name="dataCadastro">Usado na migração do velho para o novo</param>
        /// <returns></returns>
        public Model.Usuario Salvar(decimal numeroCPF, string nome, DateTime dataNascimento, string sexo, string dddCelular, decimal numeroCelular, string dddComercial, decimal numeroComercial, decimal ramal, string IP, DateTime? dataCadastro = null)
        {
            var data = dataCadastro ?? DateTime.Now;
            var objUsuario = GetByCPF(numeroCPF);

            if (objUsuario == null)
            {
                objUsuario = new Model.Usuario { DataCadastro = data, CPF = numeroCPF, Guid = Guid.NewGuid(), IP = IP};
                Adicionar(objUsuario);
            }
            else
            {
                if (objUsuario.DataNascimento.Date != dataNascimento.Date)
                    throw new DataDeNascimentoNaoConfere();

                Atualizar(objUsuario);
            }

            objUsuario.Nome = nome;
            objUsuario.DataAlteracao = data;
            objUsuario.DataNascimento = dataNascimento;
            objUsuario.Sexo = string.IsNullOrWhiteSpace(sexo) ? null : _sexo.GetByChar(sexo);

            _telefoneCelular.SalvarTelefone(objUsuario, string.IsNullOrWhiteSpace(dddCelular) ? (byte?)null : Convert.ToByte(dddCelular), numeroCelular);
            _telefoneComercial.SalvarTelefone(objUsuario, string.IsNullOrWhiteSpace(dddComercial) ? (byte?)null : Convert.ToByte(dddComercial), numeroComercial, ramal);
            //_email.SalvarEmail(objUsuario, email);

            return objUsuario;
        }
        #endregion

        #region Atualizar
        private void Atualizar(Model.Usuario objUsuario)
        {
            _usuarioRepository.Update(objUsuario);
        }
        #endregion

        #region Adicionar
        private void Adicionar(Model.Usuario objUsuario)
        {
            _usuarioRepository.Add(objUsuario);
        }
        #endregion

        #region GerarHashAcessoLoginAutomatico
        /// <summary>
        /// Gera o token de acesso para o bne
        /// </summary>
        /// <param name="objUsuario">Usuário</param>
        /// <param name="urlDestino">URL de destino</param>
        /// <returns></returns>
        public string GerarHashAcessoLoginAutomatico(Model.Usuario objUsuario, string urlDestino)
        {
            if (urlDestino == null)
                urlDestino = string.Empty;

            var parametros = new
            {
                NumeroCPF = objUsuario.CPF,
                DataNascimento = objUsuario.DataNascimento.Date,
                Url = urlDestino
            };
            string json = Utils.ToJSON(parametros);
            return Utils.ToBase64(json);
        }
        #endregion

    }
}
