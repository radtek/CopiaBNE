using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using BNE.Data.Infrastructure;
using BNE.Logger.Interface;
using BNE.PessoaJuridica.Data.Repositories;
using BNE.PessoaJuridica.Domain.Exceptions;

namespace BNE.PessoaJuridica.Domain
{
    public class PessoaJuridica
    {

        private readonly IPessoaJuridicaRepository _pessoaJuridicaRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        private readonly Mensagem.Email _emailService;

        private readonly Global.Domain.Cidade _cidade;
        private readonly Global.Domain.Bairro _bairro;
        public Usuario Usuario { get; set; }
        public NaturezaJuridica NaturezaJuridica { get; set; }
        public CNAE CNAE { get; set; }
        public Endereco Endereco { get; set; }
        public UsuarioPessoaJuridica UsuarioPessoaJuridica { get; set; }
        public TelefoneComercial TelefoneComercial { get; set; }
        public Email Email { get; set; }
        public UsuarioAdicional UsuarioAdicional { get; set; }
        public NCall Ncall { get; set; }
        public Parametro Parametro { get; set; }

        public PessoaJuridica(IPessoaJuridicaRepository pessoaJuridicaRepository, Global.Domain.Cidade cidade, Global.Domain.Bairro bairro, Endereco endereco, UsuarioPessoaJuridica usuarioPessoaJuridica, TelefoneComercial telefoneComercial, Email email, Usuario usuario, NaturezaJuridica naturezaJuridica, CNAE cnae, Mensagem.Email emailService, IUnitOfWork unitOfWork, ILogger logger)
        {
            _pessoaJuridicaRepository = pessoaJuridicaRepository;
            _unitOfWork = unitOfWork;

            _logger = logger;

            _cidade = cidade;
            _bairro = bairro;

            Usuario = usuario;
            NaturezaJuridica = naturezaJuridica;
            CNAE = cnae;
            Endereco = endereco;
            UsuarioPessoaJuridica = usuarioPessoaJuridica;
            TelefoneComercial = telefoneComercial;
            Email = email;

            _emailService = emailService;
        }

        #region SalvarPessoaJuridica
        public Model.Usuario SalvarPessoaJuridica(Command.CriarOuAtualizarPessoaJuridica command)
        {
            try
            {
                var baseData = DateTime.Now;

                if (!BNE.Core.Validacao.CNPJ.Validar(command.NumeroCNPJ))
                    throw new CNPJInvalido();

                if(!BNE.Core.Validacao.CPF.Validar(command.Usuario.NumeroCPF))
                    throw new CPFInvalido();

                #region Pessoa Juridica

                Model.PessoaJuridica objPessoaJuridica;

                if (command.Id != 0)
                    objPessoaJuridica = _pessoaJuridicaRepository.GetById(command.Id);
                else
                    objPessoaJuridica = CarregarPorCNPJ(command.NumeroCNPJ) ?? new Model.PessoaJuridica { Guid = Guid.NewGuid(), IP = command.IP };

                if (objPessoaJuridica.Endereco == null)
                {
                    objPessoaJuridica.Endereco = new Model.Endereco();
                }

                if (objPessoaJuridica.Id != 0)
                {
                    _pessoaJuridicaRepository.Update(objPessoaJuridica);
                }
                else
                {
                    objPessoaJuridica.DataCadastro = baseData;
                    _pessoaJuridicaRepository.Add(objPessoaJuridica);
                }

                Email.SalvarEmail(objPessoaJuridica, command.Email);
                objPessoaJuridica.CNPJ = command.NumeroCNPJ;
                objPessoaJuridica.RazaoSocial = command.RazaoSocial;
                objPessoaJuridica.NomeFantasia = command.NomeFantasia;
                objPessoaJuridica.Site = command.Site;
                objPessoaJuridica.SituacaoCadastral = command.SituacaoCadastral;
                objPessoaJuridica.CNAE = CNAE.GetByCodigo(command.CNAE);
                objPessoaJuridica.NaturezaJuridica = NaturezaJuridica.GetByCodigo(command.NaturezaJuridica);
                objPessoaJuridica.QuantidadeFuncionario = command.QuantidadeFuncionario;
                objPessoaJuridica.DataAbertura = command.DataAbertura;
                objPessoaJuridica.DataAlteracao = baseData;

                TelefoneComercial.SalvarTelefone(objPessoaJuridica, Convert.ToByte(command.NumeroDDDComercial), command.NumeroComercial);

                Endereco.SalvarEndereco(objPessoaJuridica, command, baseData);
                #endregion

                #region Usuario
                var objUsuario = Usuario.Salvar(command.Usuario.NumeroCPF, command.Usuario.Nome, command.Usuario.DataNascimento, command.Usuario.Sexo, command.Usuario.NumeroDDDCelular, command.Usuario.NumeroCelular, command.Usuario.NumeroDDDComercial, command.Usuario.NumeroComercial, command.Usuario.NumeroRamal, command.IP);
                #endregion

                //Testando se o usuário pode editar a empresa atual
                if (objPessoaJuridica.Id != 0)
                {
                    var obj = UsuarioPessoaJuridica.GetByUsuarioPessoaJuridica(objUsuario, objPessoaJuridica);
                    if (obj == null || !UsuarioPessoaJuridica.UsuarioPodeEditarEmpresa(obj))
                        throw new SemPermissaoParaEditar();
                }

                #region UsuarioPessoaJuridica
                var objUsuarioPessoaJuridica = UsuarioPessoaJuridica.Salvar(objUsuario, objPessoaJuridica, Model.Enumeradores.Perfil.Master, command.Usuario.Funcao, command.Usuario.NumeroDDDCelular, command.Usuario.NumeroCelular, command.Usuario.NumeroDDDComercial, command.Usuario.NumeroComercial, command.Usuario.NumeroRamal, command.Usuario.Email, command.IP);
                #endregion

                #region UsuariosAdicionais
                foreach (var objUsuarioAdicional in command.UsuariosAdicionais)
                {
                    UsuarioAdicional.Adicionar(new Model.UsuarioAdicional
                    {
                        Nome = objUsuarioAdicional.Nome,
                        Email = objUsuarioAdicional.Email,
                        PessoaJuridica = objPessoaJuridica
                    });
                }
                #endregion

                #region Mapeamento para o velho
                try
                {
                    AutoMapper.Mapper.CreateMap<Command.CriarOuAtualizarPessoaJuridica, Mapper.Models.PessoaJuridica.PessoaJuridica>();
                    AutoMapper.Mapper.CreateMap<Command.UsuarioPessoaJuridica, Mapper.Models.PessoaJuridica.Usuario>();

                    var commonObject = AutoMapper.Mapper.Map<Command.CriarOuAtualizarPessoaJuridica, Mapper.Models.PessoaJuridica.PessoaJuridica>(command);
                    var usuario = AutoMapper.Mapper.Map<Command.UsuarioPessoaJuridica, Mapper.Models.PessoaJuridica.Usuario>(command.Usuario);

                    //Ajustando propriedades que não foram mapeadas.
                    commonObject.DataAlteracao = baseData;
                    commonObject.DataCadastro = objPessoaJuridica.DataCadastro;

                    usuario.UsuarioMaster = true;
                    usuario.Email = string.Empty;
                    usuario.EmailComercial = command.Usuario.Email;
                    usuario.DataCadastro = objUsuario.DataCadastro;
                    usuario.DataAlteracao = baseData;
                    usuario.UsuarioInativo = false;
                    if (objUsuarioPessoaJuridica.FuncaoSinonimo != null)
                        usuario.IdFuncaoVelho = objUsuarioPessoaJuridica.FuncaoSinonimo.IdSinonimoSubstituto;

                    commonObject.Usuarios = new List<Mapper.Models.PessoaJuridica.Usuario> { usuario };

                    new Mapper.ToOld.PessoaJuridica().Map(commonObject);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "MAPEAMENTO");
                }
                #endregion

                _unitOfWork.Commit();

                #region Email - UsuariosAdicionais
                if (command.UsuariosAdicionais != null)
                {
                    foreach (var objUsuarioAdicional in command.UsuariosAdicionais)
                    {
                        var model = new
                        {
                            Nome = Core.Common.Utils.RetornarPrimeiroNome(objUsuarioAdicional.Nome),
                            NomeResponsavel = command.Usuario.Nome,
                            NomeEmpresa = command.RazaoSocial,
                            Link = UsuarioAdicional.RecuperarLink(objPessoaJuridica, objUsuarioAdicional.Email)
                        };
                        _emailService.EnviarEmail(Model.Enumeradores.Email.InclusaoUsuarioAdicionalEmpresa, "bne@bne.com.br", objUsuarioAdicional.Email, model);
                    }
                }
                #endregion

                return objUsuario;
            }
            catch (Exception ex)
            {
                var customError = string.Empty;

                var exception = ex as DbEntityValidationException;
                if (exception != null)
                    customError = string.Join("; ", exception.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage));

                _logger.Error(ex, customError);
                throw;
            }
        }
        #endregion

        #region SalvarPessoaJuridicaDoVelho
        /// <summary>
        /// Método utilizado pelo mapeamento
        /// </summary>
        /// <param name="objCriarOuAtualizarPessoaJuridica"></param>
        /// <returns></returns>
        public bool SalvarPessoaJuridicaDoVelho(Command.CriarOuAtualizarPessoaJuridicaDoVelho objCriarOuAtualizarPessoaJuridica)
        {
            try
            {
                Model.PessoaJuridica objPessoaJuridica;

                if (objCriarOuAtualizarPessoaJuridica.Id != 0)
                    objPessoaJuridica = _pessoaJuridicaRepository.GetById(objCriarOuAtualizarPessoaJuridica.Id);
                else
                    objPessoaJuridica = CarregarPorCNPJ(objCriarOuAtualizarPessoaJuridica.NumeroCNPJ) ?? new Model.PessoaJuridica { Endereco = new Model.Endereco(), Guid = Guid.NewGuid() };

                objPessoaJuridica.CNPJ = objCriarOuAtualizarPessoaJuridica.NumeroCNPJ;
                objPessoaJuridica.RazaoSocial = objCriarOuAtualizarPessoaJuridica.RazaoSocial;
                objPessoaJuridica.NomeFantasia = objCriarOuAtualizarPessoaJuridica.NomeFantasia;
                objPessoaJuridica.Site = objCriarOuAtualizarPessoaJuridica.Site;
                objPessoaJuridica.SituacaoCadastral = objCriarOuAtualizarPessoaJuridica.SituacaoCadastral;
                objPessoaJuridica.CNAE = CNAE.GetByCodigo(objCriarOuAtualizarPessoaJuridica.CNAE);
                objPessoaJuridica.NaturezaJuridica = NaturezaJuridica.GetByCodigo(objCriarOuAtualizarPessoaJuridica.NaturezaJuridica);
                objPessoaJuridica.IP = objCriarOuAtualizarPessoaJuridica.IP;

                if (objCriarOuAtualizarPessoaJuridica.DataAbertura > objPessoaJuridica.DataAbertura)
                    objPessoaJuridica.DataAbertura = objCriarOuAtualizarPessoaJuridica.DataAbertura;

                objPessoaJuridica.DataAlteracao = objCriarOuAtualizarPessoaJuridica.DataAlteracao;

                if (objPessoaJuridica.Id != 0)
                    _pessoaJuridicaRepository.Update(objPessoaJuridica);
                else
                {
                    objPessoaJuridica.DataCadastro = objCriarOuAtualizarPessoaJuridica.DataCadastro;
                    _pessoaJuridicaRepository.Add(objPessoaJuridica);
                }

                objPessoaJuridica.DataCadastro = objCriarOuAtualizarPessoaJuridica.DataCadastro;

                TelefoneComercial.SalvarTelefone(objPessoaJuridica, Convert.ToByte(objCriarOuAtualizarPessoaJuridica.NumeroDDDComercial), objCriarOuAtualizarPessoaJuridica.NumeroComercial);

                var objCidade = _cidade.GetByNomeUF(objCriarOuAtualizarPessoaJuridica.Cidade);

                if (objCidade == null)
                    return false;

                objPessoaJuridica.Endereco.CEP = objCriarOuAtualizarPessoaJuridica.CEP;
                objPessoaJuridica.Endereco.Cidade = objCidade;

                var objBairro = _bairro.GetByNome(objCidade, objCriarOuAtualizarPessoaJuridica.Bairro);
                if (objBairro != null)
                {
                    objPessoaJuridica.Endereco.Bairro = objBairro;
                    objPessoaJuridica.Endereco.DescricaoBairro = null;
                }
                else
                {
                    objPessoaJuridica.Endereco.Bairro = null;
                    objPessoaJuridica.Endereco.DescricaoBairro = objCriarOuAtualizarPessoaJuridica.Bairro;
                }

                objPessoaJuridica.Endereco.Complemento = objCriarOuAtualizarPessoaJuridica.Complemento;
                objPessoaJuridica.Endereco.Logradouro = objCriarOuAtualizarPessoaJuridica.Logradouro;
                objPessoaJuridica.Endereco.Numero = objCriarOuAtualizarPessoaJuridica.Numero;
                objPessoaJuridica.Endereco.DataAlteracao = objCriarOuAtualizarPessoaJuridica.DataAlteracao;

                if (objPessoaJuridica.Endereco.Id != 0)
                    Endereco.Atualizar(objPessoaJuridica.Endereco);
                else
                {
                    objPessoaJuridica.Endereco.DataCadastro = objCriarOuAtualizarPessoaJuridica.DataCadastro;
                    Endereco.Adicionar(objPessoaJuridica.Endereco);
                }

                foreach (var objUsuarioVelho in objCriarOuAtualizarPessoaJuridica.Usuarios)
                {
                    #region Usuario
                    var objUsuario = Usuario.Salvar(objUsuarioVelho.NumeroCPF, objUsuarioVelho.Nome, objUsuarioVelho.DataNascimento, objUsuarioVelho.Sexo.ToString(), objUsuarioVelho.NumeroDDDCelular, objUsuarioVelho.NumeroCelular, objUsuarioVelho.NumeroDDDComercial, objUsuarioVelho.NumeroComercial, objUsuarioVelho.NumeroRamal, objUsuarioVelho.IP, objUsuarioVelho.DataCadastro);
                    #endregion

                    #region UsuarioPessoaJuridica
                    var perfil = objUsuarioVelho.UsuarioMaster ? Model.Enumeradores.Perfil.Master : Model.Enumeradores.Perfil.Selecionador;
                    var objUsuarioPessoaJuridica = UsuarioPessoaJuridica.Salvar(objUsuario, objPessoaJuridica, perfil, objUsuarioVelho.Funcao, objUsuarioVelho.NumeroDDDCelular, objUsuarioVelho.NumeroCelular, objUsuarioVelho.NumeroDDDComercial, objUsuarioVelho.NumeroComercial, objUsuarioVelho.NumeroRamal, objUsuarioVelho.EmailComercial, objUsuarioVelho.IP, objUsuarioVelho.DataCadastro, objUsuarioVelho.IdFuncaoVelho);

                    objUsuarioPessoaJuridica.Ativo = !objUsuarioVelho.UsuarioInativo;
                    #endregion
                }

                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }
        #endregion        

        #region CarregarPorCNPJ
        /// <summary>
        /// Carrega uma pessoa juridica dado um cnpj.
        /// </summary>
        /// <param name="numeroCNPJ"></param>
        /// <returns></returns>
        public Model.PessoaJuridica CarregarPorCNPJ(decimal numeroCNPJ)
        {
            return _pessoaJuridicaRepository.Get(n => n.CNPJ == numeroCNPJ);
        }
        #endregion

        #region ExisteCNPJ
        /// <summary>
        /// Existe pessoa juridica dado um cnpj.
        /// </summary>
        /// <param name="numeroCNPJ"></param>
        /// <returns></returns>
        public bool ExisteCNPJ(decimal numeroCNPJ)
        {
            return _pessoaJuridicaRepository.GetMany(n => n.CNPJ == numeroCNPJ).Any();
        }
        #endregion

        #region Click2Call
        /// <summary>
        /// Chama o processo de click2call, como está no cadastro de pj deverá chamar a fila de boas vindas.
        /// </summary>
        /// <param name="numero"></param>
        public void Click2Call(string numero, string nome)
        {
            if (Ncall.FilaBoasVindasDisponivel())
                Ncall.LigarBoasVindas(numero, nome);
            else if (Ncall.FilaCIADisponivel())
                Ncall.LigarCIA(numero, nome);
        }
        #endregion

        #region SalvarSolicitacaoReceita
        /// <summary>
        /// Fluxo para tratar as tentativas de cadastro de pj quando o site da receita federal estiver fora do ar apra recuperar os dados do cartão de cnpj
        /// </summary>
        /// <param name="command"></param>
        public void SalvarSolicitacaoReceita(Command.CriarSolicitacaoReceita command)
        {
            try
            {
                var destinatario = Parametro.RecuperarValor(Model.Enumeradores.Parametro.EmailDestinatarioNotificaoReceitaOnline);
                var model = new
                {
                    CNPJ = command.CNPJ,
                    Nome = command.Nome,
                    Email = command.Email,
                    Telefone = command.Telefone,
                    Data = command.DataCadastro
                };
                _emailService.EnviarEmail(Model.Enumeradores.Email.SolicitacaoNotificacaoReceita, destinatario, command.Email, model);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
        #endregion

        #region GerarHashCompraDePlano
        /// <summary>
        /// Gera uma hash para o usuário entra logado e pagar por um plano pre definido
        /// </summary>
        /// <param name="objUsuario"></param>
        /// <returns></returns>
        public string GerarHashCompraDePlano(Model.Usuario objUsuario)
        {
            var urlCompra = Parametro.RecuperarValor(Model.Enumeradores.Parametro.UrlCompraPlano);
            var plano = Parametro.RecuperarValor(Model.Enumeradores.Parametro.IdentificadorPlanoCompraPlano);

            return Usuario.GerarHashAcessoLoginAutomatico(objUsuario, string.Concat(urlCompra, plano));
        }
        #endregion

    }
}
