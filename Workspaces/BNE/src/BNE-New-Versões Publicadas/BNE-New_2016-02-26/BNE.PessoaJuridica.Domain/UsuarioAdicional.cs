using System;
using System.Web;
using BNE.Core.ExtensionsMethods;
using BNE.Data.Infrastructure;
using BNE.PessoaJuridica.Data.Repositories;
using BNE.PessoaJuridica.Domain.Command;
using System.Collections.Generic;

namespace BNE.PessoaJuridica.Domain
{
    public class UsuarioAdicional
    {

        private readonly IUnitOfWork _unitOfWork;

        private readonly IUsuarioAdicionalRepository _usuarioAdicionalRepository;
        public PessoaJuridica PessoaJuridica { get; set; }
        public Usuario Usuario { get; set; }
        public UsuarioPessoaJuridica UsuarioPessoaJuridica { get; set; }
        public Parametro Parametro { get; set; }

        public UsuarioAdicional(IUsuarioAdicionalRepository usuarioAdicionalRepository, Usuario usuario, UsuarioPessoaJuridica usuarioPessoaJuridica, Parametro parametro, IUnitOfWork unitOfWork)
        {
            _usuarioAdicionalRepository = usuarioAdicionalRepository;
            _unitOfWork = unitOfWork;

            Usuario = usuario;
            UsuarioPessoaJuridica = usuarioPessoaJuridica;
            Parametro = parametro;
        }

        public void Adicionar(Model.UsuarioAdicional objUsuarioAdicional)
        {
            _usuarioAdicionalRepository.Add(objUsuarioAdicional);
        }

        public Model.Usuario SalvarUsuarioAdicional(CriarOuAtualizarUsuarioEmpresa command)
        {
            if (PessoaJuridica.ExisteCNPJ(command.NumeroCNPJ))
            {
                var objPessoaJuridica = PessoaJuridica.CarregarPorCNPJ(command.NumeroCNPJ);

                #region Usuario
                var objUsuario = Usuario.Salvar(command.NumeroCPF, command.Nome, command.DataNascimento, command.Sexo.ToString(), command.NumeroDDDCelular, command.NumeroCelular, command.NumeroDDDComercial, command.NumeroComercial, command.NumeroRamal, command.IP);
                #endregion

                #region UsuarioPessoaJuridica
                var objUsuarioPessoaJuridica = UsuarioPessoaJuridica.Salvar(objUsuario, objPessoaJuridica, Model.Enumeradores.Perfil.Selecionador, command.Funcao, command.NumeroDDDCelular, command.NumeroCelular, command.NumeroDDDComercial, command.NumeroComercial, command.NumeroRamal, command.Email, command.IP);
                #endregion

                #region Mapeamento para o velho
                AutoMapper.Mapper.CreateMap<Command.CriarOuAtualizarUsuarioEmpresa, Mapper.Models.PessoaJuridica.PessoaJuridica>();
                AutoMapper.Mapper.CreateMap<Command.CriarOuAtualizarUsuarioEmpresa, Mapper.Models.PessoaJuridica.Usuario>();

                var commonObject = AutoMapper.Mapper.Map<CriarOuAtualizarUsuarioEmpresa, Mapper.Models.PessoaJuridica.PessoaJuridica>(command);
                var usuario = AutoMapper.Mapper.Map<CriarOuAtualizarUsuarioEmpresa, Mapper.Models.PessoaJuridica.Usuario>(command);

                usuario.UsuarioMaster = false;
                usuario.Email = string.Empty;
                usuario.EmailComercial = command.Email;
                usuario.DataCadastro = objUsuario.DataCadastro;
                usuario.DataAlteracao = objUsuario.DataAlteracao;
                usuario.UsuarioInativo = false;
                if (objUsuarioPessoaJuridica.FuncaoSinonimo != null)
                    usuario.IdFuncaoVelho = objUsuarioPessoaJuridica.FuncaoSinonimo.IdSinonimoSubstituto;

                commonObject.Usuarios = new List<Mapper.Models.PessoaJuridica.Usuario> { usuario };

                new Mapper.ToOld.PessoaJuridica().MapUsuarios(commonObject);
                #endregion

                _unitOfWork.Commit();

                return objUsuario;
            }
            throw new Exception("CNPJ não existe!");
        }

        public string RecuperarLink(Model.PessoaJuridica objPessoaJuridica, string email)
        {
            var model = new
            {
                URL = Parametro.RecuperarValor(Model.Enumeradores.Parametro.EnderecoWebPessoaJuridica),
                NumeroCNPJ = objPessoaJuridica.CNPJ,
                Email = HttpUtility.UrlEncode(email)
            };
            return model.ToString("http://{URL}/usuario-adicional/{Email}/{NumeroCNPJ}");
        }
    }
}
