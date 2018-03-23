using System;
using System.Collections.Generic;
using System.Web;
using AutoMapper;
using BNE.Core.ExtensionsMethods;
using BNE.Data.Services.Interfaces;
using BNE.PessoaJuridica.Domain.Command;
using BNE.PessoaJuridica.Domain.Repositories;

namespace BNE.PessoaJuridica.Domain.Services
{
    public class UsuarioAdicionalService
    {

        private readonly IMapper _mapper;

        private readonly IUsuarioAdicionalRepository _usuarioAdicionalRepository;
        public PessoaJuridicaService PessoaJuridica { get; set; }
        public UsuarioService Usuario { get; set; }
        public UsuarioPessoaJuridicaService UsuarioPessoaJuridica { get; set; }
        public ParametroService Parametro { get; set; }

        private readonly IIdentityServerService _identityServerService;

        public UsuarioAdicionalService(
            IUsuarioAdicionalRepository usuarioAdicionalRepository, 
            UsuarioService usuario, 
            UsuarioPessoaJuridicaService usuarioPessoaJuridica, 
            ParametroService parametro, 
            IMapper mapper, 
            IIdentityServerService identityServerService)
        {
            _usuarioAdicionalRepository = usuarioAdicionalRepository;
            _mapper = mapper;
            _identityServerService = identityServerService;

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
                var commonObject = _mapper.Map<CriarOuAtualizarUsuarioEmpresa, Mapper.Models.PessoaJuridica.PessoaJuridica>(command);
                var usuario = _mapper.Map<CriarOuAtualizarUsuarioEmpresa, Mapper.Models.PessoaJuridica.Usuario>(command);

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

                //TODO: _unitOfWork.Commit();

                _identityServerService.CreateUserAccount(objUsuario.Nome, objUsuario.CPF, objUsuario.DataNascimento);

                return objUsuario;
            }
            throw new Exception("CNPJ não existe!");
        }

        public string RecuperarLink(Model.PessoaJuridica objPessoaJuridica, string email, string nome)
        {
            var model = new
            {
                URL = Parametro.RecuperarValor(Model.Enumeradores.Parametro.EnderecoWebPessoaJuridica),
                NumeroCNPJ = objPessoaJuridica.CNPJ,
                Email = HttpUtility.UrlPathEncode(email),
                Nome = HttpUtility.UrlPathEncode(nome)
            };
            return model.ToString("http://{URL}/usuario-adicional/{Email}/{NumeroCNPJ}/{Nome}");
        }
    }
}
