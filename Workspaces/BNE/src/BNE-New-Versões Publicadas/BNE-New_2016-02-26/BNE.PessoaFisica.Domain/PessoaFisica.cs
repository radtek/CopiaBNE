using BNE.Data.Infrastructure;
using BNE.PessoaFisica.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BNE.Core.Common;
using BNE.ExceptionLog.Interface;
using System.Configuration;
using BNE.Global.Domain;
using Newtonsoft.Json;

namespace BNE.PessoaFisica.Domain
{
    public class PessoaFisica
    {
        private readonly BNE.PessoaFisica.Data.Repositories.IPessoaFisicaRepository _pessoaFisicaRepository;

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        private readonly Domain.TipoCurriculo _tipoCurriculo;
        private readonly Domain.SituacaoCurriculo _situacaoCurriculo;
        private readonly Domain.Curriculo _curriculo;
        private readonly Domain.Email _email;
        public Domain.PreCurriculo PreCurriculo { get; set; }
        private readonly Domain.FuncaoPretendida _funcaoPretendida;
        private readonly Domain.CurriculoOrigem _curriculoOrigem;
        private readonly Domain.CurriculoParametro _curriculoParametro;
        private readonly Domain.TelefoneCelular _telefoneCelular;
        private readonly Domain.Mensagem.Email _emailService;
        private readonly Domain.CodigoConfirmacaoEmail _codigoConfirmacaoEmail;
        private readonly Domain.ExperienciaProfissional _experienciaProfissional;
        private readonly Domain.Formacao _formacao;
        private readonly Domain.InstituicaoEnsino _instituicaoEnsino;
        private readonly Domain.Curso _curso;
        private readonly Domain.Parametro _parametro;

        private readonly Global.Domain.RamoAtividade _ramoAtividade;
        private readonly Global.Domain.Origem _origem;
        private readonly Global.Domain.Escolaridade _escolaridade;
        private readonly Global.Domain.Cidade _cidade;
        private readonly Global.Domain.FuncaoSinonimo _funcao;
        private readonly Global.Domain.Sexo _sexo;
        private readonly Global.Domain.Deficiencia _deficiencia;



        public PessoaFisica(IPessoaFisicaRepository PessoaFisicaRepository, IUnitOfWork unitOfWork, TipoCurriculo tipoCurriculo, SituacaoCurriculo situacaoCurriculo, ILogger logger,
            Curriculo curriculo,Email email, FuncaoPretendida funcaoPretendida, CurriculoOrigem curriculoOrigem, Global.Domain.Origem origem, TelefoneCelular telefoneCelular,
            CurriculoParametro curriculoParametro, Domain.Mensagem.Email emailService, CodigoConfirmacaoEmail codigoConfirmacaoEmail, ExperienciaProfissional experienciaProfissional,
            RamoAtividade ramoAtividade, Formacao formacao, Global.Domain.Escolaridade escolaridade, Global.Domain.Cidade cidade, Domain.InstituicaoEnsino instituicaoEnsino,
            Domain.Curso curso, Domain.Parametro parametro,Global.Domain.FuncaoSinonimo funcao, Global.Domain.Sexo sexo,Global.Domain.Deficiencia deficiencia)
        {
            _pessoaFisicaRepository = PessoaFisicaRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;

            _tipoCurriculo = tipoCurriculo;
            _situacaoCurriculo = situacaoCurriculo;
            _curriculo = curriculo;
            _email = email;
            _funcaoPretendida = funcaoPretendida;
            _curriculoOrigem = curriculoOrigem;
            _origem = origem;
            _telefoneCelular = telefoneCelular;
            _curriculoParametro = curriculoParametro;
            _emailService = emailService;
            _codigoConfirmacaoEmail = codigoConfirmacaoEmail;
            _experienciaProfissional = experienciaProfissional;
            _ramoAtividade = ramoAtividade;
            _formacao = formacao;
            _escolaridade = escolaridade;
            _cidade = cidade;
            _instituicaoEnsino = instituicaoEnsino;
            _curso = curso;
            _parametro = parametro;
            _funcao = funcao;
            _sexo = sexo;
            _deficiencia = deficiencia;
        }

        public Model.PessoaFisica GetById(int id)
        {
            return _pessoaFisicaRepository.GetById(id);
        }

        /// <summary>
        /// Validar se o CPF e Data Nascimento já existem
        /// </summary>
        /// <param name="cpf"></param>
        /// <param name="dataNascimento"></param>
        /// <returns></returns>
        public string CarregarNomePessoaFisicaJaExiste(decimal cpf, DateTime dataNascimento)
        {
            string nomePessoa = string.Empty;

            try
            {
                //Chcecar PF no Banco novo
                var pessoaFisica = _pessoaFisicaRepository.GetMany(p => p.CPF == cpf && p.DataNascimento == dataNascimento.Date).FirstOrDefault();

                if (pessoaFisica == null)
                {
                    //Checar PF no banco velho, mas o ID é diferente
                    nomePessoa = new Mapper.ToOld.PessoaFisica().CarregarPessoaFisica(cpf);
                }

                return pessoaFisica == null ? nomePessoa : pessoaFisica.Nome;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return "";
            }
        }

        public Model.PessoaFisica CarregarPessoaFisicaJaExiste(decimal cpf, DateTime dataNascimento)
        {
            try
            {
                //Chcecar PF no Banco novo
                var pessoaFisica = _pessoaFisicaRepository.GetMany(p => p.CPF == cpf && p.DataNascimento == dataNascimento.Date).FirstOrDefault();

                if (pessoaFisica == null)
                {
                    //Checar PF no banco velho, mas o ID é diferente
                    //se existir mapear para o novo
                }

                return pessoaFisica;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return null;
            }
        }

        public int PessoaFisicaJaExiste(decimal cpf, DateTime dataNascimento)
        {
            try
            {
                //Chcecar PF no Banco novo
                var pessoaFisica = _pessoaFisicaRepository.GetMany(p => p.CPF == cpf && p.DataNascimento == dataNascimento.Date).FirstOrDefault();

                if (pessoaFisica == null)
                {
                    //Checar PF no banco velho, mas o ID é diferente
                    int idPessoaFisica = new Mapper.ToOld.PessoaFisica().CarregarIdSeExistePessoaFisica(cpf);

                    return idPessoaFisica;
                }

                return pessoaFisica.Id;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return -1;
            }
        }

        #region CurriculoExiste
        public bool CurriculoExiste(int idPessoaFisica)
        {
            try
            {
                if(_curriculo.CurriculoAtivo(idPessoaFisica))
                {
                    return true;
                }

                return new Mapper.ToOld.PessoaFisica().ExisteCurriculo(idPessoaFisica);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return false;
            }
        }
        #endregion

        #region GetLinksPaginasSemelhantes
        public string[] GetLinksPaginasSemelhantes(string funcao, string cidade, string siglaEstado, string areaBNE)
        {
            return new Mapper.ToOld.PessoaFisica().GetLinksPaginasSemelhantes(funcao, cidade, siglaEstado, areaBNE);
        }
        #endregion

        #region ChecarJaEnviei
        public bool ChecarJaEnviei(int idVaga, decimal cpf)
        {
            return new Mapper.ToOld.PessoaFisica().ChecarJaEnviei(idVaga, cpf);
        }

        #endregion

        public string GetCadidaturasDegustacao(decimal cpf, DateTime dataNascimento)
        {
            string saldo = "0";

            Model.PessoaFisica objPessoa = RetornarFisicaJaExiste(cpf, dataNascimento);
            //Model.Curriculo objCurriculo = null;
            //Model.CurriculoParametro objCurriculoParametro = null;

            //if(objPessoa == null)
            //{
                var pf = new Mapper.ToOld.PessoaFisica().CarregarPessoaFisica(cpf);

                if(pf != null)
                {
                    Tuple<bool,int> result = new Mapper.ToOld.PessoaFisica().ECurriculoVIP(cpf);

                    if (result.Item1)
                        saldo = "VIP";
                    else
                        saldo = new Mapper.ToOld.PessoaFisica().GetParametroCurriculo(result.Item2);
                }
            //}
            //else
            //{
            //    objCurriculo = _curriculo.GetByIdPessoaFisica(objPessoa.Id);

            //    if (!objCurriculo.FlgVIP)
            //    {
            //        objCurriculoParametro = _curriculoParametro.GetByIdCurriculo(objCurriculo.Id);
            //        saldo = objCurriculoParametro.Valor;
            //    }
            //    else
            //    {
            //        saldo = "VIP";
            //    }
            //}

            return saldo;
        }

        /// <summary>
        /// Validar se o CPF e Data Nascimento já existem
        /// </summary>
        /// <param name="cpf"></param>
        /// <param name="dataNascimento"></param>
        /// <returns></returns>
        public Model.PessoaFisica RetornarFisicaJaExiste(decimal cpf, DateTime dataNascimento)
        {
            try
            {
                //Chcecar PF no Banco novo
                var pessoaFisica = _pessoaFisicaRepository.GetMany(p => p.CPF == cpf && p.DataNascimento == dataNascimento.Date).FirstOrDefault();

                if (pessoaFisica == null)
                {
                    //checar PF no Banco antigo
                    bool existe = new Mapper.ToOld.PessoaFisica().ExistePessoaFisica(cpf);
                }

                return pessoaFisica;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return null;
            }
        }

        #region TemExperiencia
        public bool TemExperiencia(decimal cpf, DateTime dataNascimento)
        {
            bool retorno = false;

            int idPessoaFisica = PessoaFisicaJaExiste(cpf, dataNascimento);

            if (idPessoaFisica > 0)
            {
                if (_experienciaProfissional.GetByIdPessoa(idPessoaFisica) > 0)
                    retorno = true; 

                //Checar experiencia na base BNE_IMP
                retorno = new Mapper.ToOld.PessoaFisica().TemExperienciaProfissional(cpf, dataNascimento);
            }

            return retorno;
        }
        #endregion

        #region TemFormacao
        public bool TemFormacao(decimal cpf, DateTime dataNascimento)
        {
            bool retorno = false;

            int idPessoaFisica = PessoaFisicaJaExiste(cpf, dataNascimento);

            if (idPessoaFisica > 0)
            {
                if (_formacao.GetByIdPessoa(idPessoaFisica) > 0)
                    retorno = true;

                //Checar formação na base BNE_IMP
                retorno = new Mapper.ToOld.PessoaFisica().TemFormacao(cpf, dataNascimento);
            }

            return retorno;
        }
        #endregion

        #region Efetuar a candidatura
        /// <summary>
        /// Efetuar a candidatura e retorna o nome do candidato
        /// </summary>
        /// <param name="preCurriculo"></param>
        /// <returns></returns>
        public string Candidatar(Command.PreCurriculo preCurriculo, bool? jaExiste = false)
        {
            try
            {
                bool pessoaInativa = false;

                string nomePessoaFisica = "";

                if (!jaExiste.Value)
                {
                    if (preCurriculo.DataNascimento != null)
                    {
                        nomePessoaFisica = CarregarNomePessoaFisicaJaExiste(preCurriculo.CPF, preCurriculo.DataNascimento.Value.Date);

                        if (nomePessoaFisica != "")
                        {
                            pessoaInativa = new Mapper.ToOld.PessoaFisica().PessoaFisicaInativa(preCurriculo.CPF);

                            if (!pessoaInativa)
                                new Mapper.ToOld.PessoaFisica().SalvarCandidatura(preCurriculo.CPF, preCurriculo.IdVaga);
                            else
                                nomePessoaFisica = "Inativa";
                        }
                    }
                }

                return nomePessoaFisica;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region CadastrarMiniCV
        public bool CadastrarMiniCV(Command.PessoaFisica pessoaFisica)
        {
            var objPessoaFisica = CarregarPessoaFisicaJaExiste(pessoaFisica.CPF, pessoaFisica.DataNascimento);

            var baseData = DateTime.Now;
            Model.TipoCurriculo tipoCurriculo = _tipoCurriculo.GetById(1);
            Model.SituacaoCurriculo situacaoCurriculo = _situacaoCurriculo.GetById(1);

            //TODO: Charan => pegar origem conforme o STC ou parceiro BNE
            Global.Model.OrigemGlobal origem = _origem.GetById(1); // Origem 1 é o BNE.

            #region PessoaFisica
            if (objPessoaFisica == null)
            {
                objPessoaFisica = new Model.PessoaFisica
                {
                    DataCadastro = baseData,
                };
            }

            var objSexo = _sexo.GetByChar(pessoaFisica.Sexo);
            

            objPessoaFisica.Nome = pessoaFisica.Nome;
            objPessoaFisica.CPF = Convert.ToDecimal(Utils.LimparMascaraCPFCNPJCEP(pessoaFisica.CPF.ToString()));
            objPessoaFisica.DataNascimento = pessoaFisica.DataNascimento;
            objPessoaFisica.Sexo = objSexo;
            objPessoaFisica.DeficienciaGlobal = pessoaFisica.IdDeficiencia != null ? _deficiencia.GetById(pessoaFisica.IdDeficiencia.Value) : null;
            objPessoaFisica.DataAlteracao = baseData;
            objPessoaFisica.EscolaridadeGlobal = pessoaFisica.IdEscolaridade > 0 ? _escolaridade.GetById(pessoaFisica.IdEscolaridade) : null;

            #region Cidade
            var objCidade = _cidade.GetByNomeUF(pessoaFisica.Cidade);
            objPessoaFisica.Cidade = objCidade;
            #endregion

            var objEmailPessoaFisica = new Model.Email();

            if (pessoaFisica.Email != null)
            {
                objEmailPessoaFisica.Endereco = pessoaFisica.Email;
                objEmailPessoaFisica.PessoaFisica = objPessoaFisica;
                objEmailPessoaFisica.DataCadastro = baseData;
                
                _pessoaFisicaRepository.Add(objPessoaFisica);
                _email.SalvarEmail(objEmailPessoaFisica);
            }

            #region Telefone            
            byte dddCelular = Byte.Parse(pessoaFisica.Celular.Replace("(", "").Replace(")", "").Replace("-", "").Substring(0, 2));
            decimal celular = decimal.Parse(pessoaFisica.Celular.Replace("(", "").Replace(")", "").Replace("-", "").Substring(3));

            _telefoneCelular.SalvarTelefone(objPessoaFisica, dddCelular, celular);
            #endregion

            

            #endregion

            #region Curriculo
            var objCurriculo = new Model.Curriculo
            {
                PessoaFisica = objPessoaFisica,
                FlgVIP = false,
                Ativo = false,
                FlgDisponivelViagem = false,
                TipoCurriculo = tipoCurriculo,
                SituacaoCurriculo = situacaoCurriculo,
                PretensaoSalarial = pessoaFisica.PretensaoSalarial.Value,
                DataCadastro = baseData
            };

            var cvSalvo = _curriculo.SalvarMiniCurriculo(objCurriculo);

            #endregion

            #region Funcao Pretendida
            Int16 tempoExperiencia = 0;

            if (pessoaFisica.TempoExperienciaAnos != null && pessoaFisica.TempoExperienciaMeses != null)
                tempoExperiencia = Int16.Parse(((pessoaFisica.TempoExperienciaAnos * 12) + pessoaFisica.TempoExperienciaMeses).ToString());

            //TODO Charan => verificar as funções, por ex.: Desenvolvedor PHP na retorna nessa consulta
            var objFuncao = _funcao.GetByNome(pessoaFisica.Funcao);

            var objFuncaoPretendida = new Model.FuncaoPretendida
            {
                DataCadastro = baseData,
                IdFuncao = objFuncao.Id,
                Curriculo = objCurriculo,
                Descricao = pessoaFisica.Funcao,
                TempoExperiencia = tempoExperiencia
            };

            _funcaoPretendida.Salvar(objFuncaoPretendida);
            #endregion

            #region CurriculoOrigem
            var objCurriculoOrigem = new Model.CurriculoOrigem
            {
                DataCadastro = baseData,
                Curriculo = objCurriculo,
                OrigemGlobal = origem
            };

            _curriculoOrigem.Salvar(objCurriculoOrigem);
            #endregion

            #region CurriculoParametro
            var objCurriculoParametro = new Model.CurriculoParametro
            {
                Curriculo = objCurriculo,
                DataCadastro = baseData,
                Ativo = true,
                Valor = Model.Enumeradores.Parametro.QuantidadeCandidaturaDegustacao.ToString(),
                Parametro = _parametro.GetById(Model.Enumeradores.Parametro.QuantidadeCandidaturaDegustacao)
            };

            _curriculoParametro.Salvar(objCurriculoParametro);
            #endregion

            //Chamar o mapeamento
            Mapper.Models.PessoaFisica.PessoaFisica commandObject = MapearNovoToAntigo(objCurriculo, baseData, objCurriculoOrigem, objFuncaoPretendida, objPessoaFisica, objEmailPessoaFisica.Endereco ,0, dddCelular, celular);
            new Mapper.ToOld.PessoaFisica().Map(commandObject);

            _unitOfWork.Commit();


            #region Enviar Carta de Confirmação de e-mail.
            if (pessoaFisica.Email != null)
            {
                EnviarCartaConfirmacaoDeEmail(objPessoaFisica.Nome, objPessoaFisica.CPF, objPessoaFisica.DataNascimento, objCurriculo.Id, objPessoaFisica.Id, objEmailPessoaFisica.Endereco);
            }
            #endregion

            return true;

        }
        #endregion

        #region PostMiniCurriculo
        /// <summary>
        /// Criar o mini curriculo a partir do pré-cadastro
        /// </summary>
        /// <param name="preCurriculo"></param>
        /// <returns></returns>
        public bool PostMiniCurriculo(Command.PreCurriculo preCurriculo)
        {
            try
            {
                int idPessoaFisica = PessoaFisicaJaExiste(preCurriculo.CPF, preCurriculo.DataNascimento.Value);

                if (idPessoaFisica > 0)
                {
                    if (CurriculoExiste(idPessoaFisica))
                    {
                        Candidatar(preCurriculo,true);
                        return true;
                    }
                    else // Curriculo bloqueado
                    {
                        //Todo: Retornar mensagem de CV Bloqueado
                        return false;
                    }
                }
                        
                var baseData = DateTime.Now; //Data base para todas as datas de cadastros

                Model.TipoCurriculo tipoCurriculo = _tipoCurriculo.GetById(1);
                Model.SituacaoCurriculo situacaoCurriculo = _situacaoCurriculo.GetById(1);

                // TODO: pegar origem conforme o STC ou parceiro BNE
                Global.Model.OrigemGlobal origem = _origem.GetById(1); // Origem 1 é o BNE.

                #region PessoaFisica
                var objPessoaFisica = new Model.PessoaFisica
                {
                    Nome = preCurriculo.Nome,
                    CPF = Convert.ToDecimal(Utils.LimparMascaraCPFCNPJCEP(preCurriculo.CPF.ToString())),
                    DataCadastro = baseData,
                    DataNascimento = preCurriculo.DataNascimento.Value
                };

                if (preCurriculo.Email != null)
                {
                    var objEmailPessoaFisica = new Model.Email
                    {
                        Endereco = preCurriculo.Email,
                        PessoaFisica = objPessoaFisica,
                        DataCadastro = baseData
                    };

                    _pessoaFisicaRepository.Add(objPessoaFisica);
                    _email.SalvarEmail(objEmailPessoaFisica);
                }

                byte dddCelular = Byte.Parse(preCurriculo.Celular.Replace("(", "").Replace(")", "").Replace("-", "").Substring(0, 2));
                decimal celular = decimal.Parse(preCurriculo.Celular.Replace("(", "").Replace(")", "").Replace("-", "").Substring(3));

                _telefoneCelular.SalvarTelefone(objPessoaFisica, dddCelular, celular);

                objPessoaFisica.Cidade = _cidade.GetById(preCurriculo.IdCidade);

                #endregion

                #region Curriculo
                var objCurriculo = new Model.Curriculo
                {
                    PessoaFisica = objPessoaFisica,
                    FlgVIP = false,
                    Ativo = false,
                    FlgDisponivelViagem = false,
                    TipoCurriculo = tipoCurriculo,
                    SituacaoCurriculo = situacaoCurriculo,
                    PretensaoSalarial = preCurriculo.PretensaoSalarial,
                    DataCadastro = baseData
                };

                var cvSalvo = _curriculo.SalvarMiniCurriculo(objCurriculo);

                #endregion

                #region Funcao Pretendida
                Int16 tempoExperiencia = 0;

                if(preCurriculo.TempoExperienciaAnos != null && preCurriculo.TempoExperienciaMeses != null)
                   tempoExperiencia = Int16.Parse(((preCurriculo.TempoExperienciaAnos * 12) + preCurriculo.TempoExperienciaMeses).ToString());

                var objFuncaoPretendida = new Model.FuncaoPretendida
                {
                    DataCadastro = baseData,
                    IdFuncao = preCurriculo.IdFuncao,
                    Curriculo = objCurriculo,
                    Descricao = preCurriculo.DescricaoFuncao,
                    TempoExperiencia = tempoExperiencia
                };

                _funcaoPretendida.Salvar(objFuncaoPretendida);
                #endregion

                #region CurriculoOrigem
                var objCurriculoOrigem = new Model.CurriculoOrigem
                {
                    DataCadastro = baseData,
                    Curriculo = objCurriculo,
                    OrigemGlobal = origem
                };

                _curriculoOrigem.Salvar(objCurriculoOrigem);
                #endregion

                #region CurriculoParametro
                var objCurriculoParametro = new Model.CurriculoParametro
                {
                    Curriculo = objCurriculo,
                    DataCadastro = baseData,
                    Ativo =true,
                    Valor = (Model.Enumeradores.Parametro.QuantidadeCandidaturaDegustacao -1).ToString(),
                    Parametro = _parametro.GetById(Model.Enumeradores.Parametro.QuantidadeCandidaturaDegustacao)
                };

                _curriculoParametro.Salvar(objCurriculoParametro);
                #endregion

                //Chamar o mapeamento
                Mapper.Models.PessoaFisica.PessoaFisica commandObject = MapearNovoToAntigo(objCurriculo, baseData, objCurriculoOrigem, objFuncaoPretendida, objPessoaFisica, preCurriculo.Email,preCurriculo.IdVaga, dddCelular, celular);

                new Mapper.ToOld.PessoaFisica().Map(commandObject);


                _unitOfWork.Commit();


                #region Atualizar PreCurriculo com o Id do Currículo
                var preCV = PreCurriculo.Get(preCurriculo.Id);
                preCV.IdCurriculo = objCurriculo.Id;

                PreCurriculo.SetarCurriculonoPreCurriculo(preCV);
                #endregion

                //atualizar número de candidaturas
                objCurriculoParametro.Valor = "2"; //descontar uma candidatura
                _curriculoParametro.Atualizar(objCurriculoParametro);

                #region Enviar Carta de Confirmação de e-mail.
                if (preCurriculo.Email != null)
                {
                    EnviarCartaConfirmacaoDeEmail(objPessoaFisica.Nome, objPessoaFisica.CPF, objPessoaFisica.DataNascimento, objCurriculo.Id, objPessoaFisica.Id, preCurriculo.Email);
                }
                #endregion

                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex,"Erro ao criar mini CV");
                throw;
            }
        }
        #endregion 
        

        #region Enviar Carta de Confirmação de e-mail.
        private void EnviarCartaConfirmacaoDeEmail(string nome, decimal cpf, DateTime dataNascimento, int idCurriculo, int idPessoaFisica, string email)
        {
            var confirmacao = new
            {
                NomeCandidato = nome,
                cpf = cpf,
                dataNascimento = dataNascimento.ToString("dd/MM/yyyy"),
                Link = ConfigurationManager.AppSettings["EnderecoBNE"] +
                    "/confirmar-email?IdCV=" + idCurriculo.ToString() + "&codigo=" +
                    _codigoConfirmacaoEmail.GerarCodigoValidacaoEmail(idPessoaFisica, email)
            };

            _emailService.EnviarEmail(Model.Enumeradores.Email.TemplateConfirmacaoEmailCandidato, "atendimento@bne.com.br", email, confirmacao);
        }
        #endregion

        #region PostFormacao

        public bool PostFormacao(Command.Formacao formacao)
        {
            DateTime baseData = DateTime.Now;

            try
            {
                BNE.PessoaFisica.Model.PessoaFisica pessoa = RetornarFisicaJaExiste(formacao.Cpf, formacao.DataNascimento.Date);

                var objFormacao = new Model.Formacao
                {
                    AnoConclusao = formacao.AnoConclusao,
                    CargaHoraria = formacao.CargaHoraria,
                    NomeInstituicao = formacao.NomeInstituicao,
                    NomeCurso = formacao.NomeCurso,
                    DataCadastro = baseData,
                    DataAlteracao = baseData,
                    Ativo = true,
                    PessoaFisica = pessoa,
                    EscolaridadeGlobal = _escolaridade.GetById(formacao.IdEscolaridade),
                    Cidade = _cidade.GetByNomeUF(formacao.NomeCidade),
                    InstituicaoEnsino = _instituicaoEnsino.GetByName(formacao.NomeInstituicao),
                    Curso = _curso.GetByName(formacao.NomeCurso)
                };

                _formacao.Salvar(objFormacao);

                //Mapear para banco antigo
                Mapper.Models.PessoaFisica.Formacao commandObject = MapearFormacao(objFormacao);
                new Mapper.ToOld.PessoaFisica().MapFormacao(commandObject, formacao.IdVaga);

                _unitOfWork.Commit();

                //Descontar candidatura
                string result = GetCadidaturasDegustacao(formacao.Cpf, formacao.DataNascimento);

                if (result != "0" && result != "VIP")
                {
                    DescontarCandidatura(_curriculo.GetByIdPessoaFisica(pessoa.Id).Id);
                    _unitOfWork.Commit();
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao inserir Formação candidato", JsonConvert.SerializeObject(formacao));
                return false;
            }

        }
        #endregion

        #region DescontarCandidatura
        private void DescontarCandidatura(int idCurriculo)
        {
            Model.CurriculoParametro objCurriculoParametro = _curriculoParametro.GetByIdCurriculo(idCurriculo);

            if(objCurriculoParametro != null)
            {
                objCurriculoParametro.Valor = (Convert.ToInt32(objCurriculoParametro.Valor) - 1).ToString();
                _curriculoParametro.Atualizar(objCurriculoParametro);
            }
        }
        #endregion

        #region PostExperienciaProfissional
        public bool PostExperienciaProfissional(Command.ExperienciaProfissional experienciaProfissional)
        {
            DateTime baseData = DateTime.Now;
            bool salvarExperiencia = experienciaProfissional.NomeEmpresa != null && experienciaProfissional.FuncaoExercida != null;

            try
            {
                BNE.PessoaFisica.Model.PessoaFisica pessoa = RetornarFisicaJaExiste(experienciaProfissional.Cpf, experienciaProfissional.DataNascimento.Date);

                var objExperiencia = new Model.ExperienciaProfissional
                {
                    AtividadesExercidas = experienciaProfissional.AtividadesExercidas,
                    DataEntrada = experienciaProfissional.DataEntrada,
                    DataSaida = experienciaProfissional.DataSaida,
                    NomeEmpresa = experienciaProfissional.NomeEmpresa,
                    Ativo = true,
                    FlgImportado = false,
                    UltimoSalario = experienciaProfissional.UltimoSalario,
                    DataCadastro = baseData,
                    FuncaoExercida = experienciaProfissional.FuncaoExercida,
                    RamoAtividadeGlobal = _ramoAtividade.GetById(experienciaProfissional.IdRamoEmpresa),
                    PessoaFisica = pessoa == null ? new Model.PessoaFisica { CPF = experienciaProfissional.Cpf } : pessoa,
                };

                if (pessoa != null && salvarExperiencia)
                {
                    _experienciaProfissional.Salvar(objExperiencia);

                    //Descontar candidatura
                    DescontarCandidatura(_curriculo.GetByIdPessoaFisica(pessoa.Id).Id);
                }

                

                //Mapear para Banco antigo
                Mapper.Models.PessoaFisica.ExperienciaProfissional commandObject = MapearExperienciaProfissional(objExperiencia);

                try
                {
                    new Mapper.ToOld.PessoaFisica().MapExperienciaProfissional(commandObject, experienciaProfissional.IdVaga, salvarExperiencia);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "commandObject: ", JsonConvert.SerializeObject(commandObject.IdRamoEmpresa));
                    throw;
                }


                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao persistir Experiencia profissional", JsonConvert.SerializeObject(experienciaProfissional));
                throw;
            }
        }
        #endregion

        #region MapearFormacao
        private Mapper.Models.PessoaFisica.Formacao MapearFormacao(Model.Formacao objFormacao)
        {
            try
            {
                var command = new Command.Formacao
                {
                    DataCadastro = objFormacao.DataCadastro,
                    DataAlteracao = objFormacao.DataAlteracao,
                    NomeCurso = objFormacao.NomeCurso,
                    NomeInstituicao = objFormacao.NomeInstituicao,
                    AnoConclusao = objFormacao.AnoConclusao,
                    Ativo = objFormacao.Ativo,
                    IdCidade = objFormacao.Cidade == null ? 0 : objFormacao.Cidade.Id,
                    IdInstituicaoEnsino = objFormacao.InstituicaoEnsino == null? 0 : objFormacao.InstituicaoEnsino.Id,
                    IdCurso = objFormacao.Curso == null? 0 : objFormacao.Curso.Id,
                    IdEscolaridade = objFormacao.EscolaridadeGlobal.Id,
                    IdPessoa = objFormacao.PessoaFisica.Id,
                    Cpf = objFormacao.PessoaFisica.CPF
                };

                AutoMapper.Mapper.CreateMap<Command.Formacao, Mapper.Models.PessoaFisica.Formacao>();

                var commandObject = AutoMapper.Mapper.Map<Command.Formacao, Mapper.Models.PessoaFisica.Formacao>(command);

                return commandObject;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro no mapeamento de Experiência profissional NEWBNE to OLD");
                throw;
            }
        }

        #endregion

        #region MapearExperienciaProfissional
        private Mapper.Models.PessoaFisica.ExperienciaProfissional MapearExperienciaProfissional(Model.ExperienciaProfissional objExperiencia)
        {
            try
            {
                var command = new Command.ExperienciaProfissional
                {
                    DataEntrada = objExperiencia.DataEntrada != null ? objExperiencia.DataEntrada.Value : DateTime.Now,
                    DataSaida = objExperiencia.DataSaida,
                    DataCadastro = objExperiencia.DataCadastro,
                    AtividadesExercidas = objExperiencia.AtividadesExercidas,
                    FlgImportado =false,
                    UltimoSalario = objExperiencia.UltimoSalario,
                    NomeEmpresa = objExperiencia.NomeEmpresa,
                    FuncaoExercida = objExperiencia.FuncaoExercida,
                    Ativo = objExperiencia.Ativo,
                    Cpf = objExperiencia.PessoaFisica.CPF,
                    //IdPessoa = objExperiencia.PessoaFisica.Id,
                    IdRamoEmpresa = objExperiencia.RamoAtividadeGlobal != null ? objExperiencia.RamoAtividadeGlobal.Id : 0
                };

                AutoMapper.Mapper.CreateMap<Command.ExperienciaProfissional, Mapper.Models.PessoaFisica.ExperienciaProfissional>();

                var commandObject = AutoMapper.Mapper.Map<Command.ExperienciaProfissional, Mapper.Models.PessoaFisica.ExperienciaProfissional>(command);

                return commandObject;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro no mapeamento de Experiência profissional NEWBNE to OLD");
                throw;
            }
        }

        #endregion

        #region MapearNovoToAntigo
        private Mapper.Models.PessoaFisica.PessoaFisica MapearNovoToAntigo(Model.Curriculo objCurriculo, DateTime baseData,
                                        Model.CurriculoOrigem objCurriculoOrigem,
                                        Model.FuncaoPretendida objFuncaoPretendida,
                                        Model.PessoaFisica objPessoaFisica,
                                        string email, int idVaga, byte dddCelular, decimal celular)
        {
            try
            {
                var commandCV = new Command.Curriculo
                {
                    FlgVIP = objCurriculo.FlgVIP,
                    FlgInativo = objCurriculo.Ativo,
                    FlgDisponivelViagem = objCurriculo.FlgDisponivelViagem,
                    PretensaoSalarial = objCurriculo.PretensaoSalarial,
                    Observacao = objCurriculo.Observacao,
                    Conhecimento = objCurriculo.Conhecimento,
                    IdTipoCurriculo = objCurriculo.TipoCurriculo.Id,
                    IdSituacaoCurriculo = objCurriculo.SituacaoCurriculo.Id,
                    DataCadastro = baseData,
                    DataAtualizacao = objCurriculo.DataAtualizacao,
                    DataModificacao = objCurriculo.DataModificacao,
                    IdOrigem = objCurriculoOrigem.OrigemGlobal.Id
                };

                var cmdFuncaoPretendida = new Command.FuncaoPretendida
                {
                    TempoExperiencia = objFuncaoPretendida.TempoExperiencia,
                    Descricao = objFuncaoPretendida.Descricao,
                    idFuncao = objFuncaoPretendida.IdFuncao
                };

                var command = new Command.CriarAtualizarPessoaFisica
                {
                    CPF = objPessoaFisica.CPF,
                    Nome = objPessoaFisica.Nome,
                    Email = email,
                    DataNascimento = objPessoaFisica.DataNascimento,
                    DataCadastro = baseData,
                    DDDCelular = dddCelular,
                    Celular = celular,
                    IdVaga = idVaga,
                    IdSexo = objPessoaFisica.Sexo != null? objPessoaFisica.Sexo.Sigla == "M" ? 1 : 2 : 0,
                    IdCidade = objPessoaFisica.Cidade.Id,
                    IdEscolaridade = objPessoaFisica.EscolaridadeGlobal != null ? objPessoaFisica.EscolaridadeGlobal.Id : 0,
                    IdDeficiencia = objPessoaFisica.DeficienciaGlobal != null? objPessoaFisica.DeficienciaGlobal.Id : 0
                };

                //Criar o Map
                AutoMapper.Mapper.CreateMap<Command.CriarAtualizarPessoaFisica, Mapper.Models.PessoaFisica.PessoaFisica>();
                AutoMapper.Mapper.CreateMap<Command.Curriculo, Mapper.Models.PessoaFisica.Curriculo>();
                AutoMapper.Mapper.CreateMap<Command.FuncaoPretendida, Mapper.Models.PessoaFisica.FuncaoPretendida>();


                //inserir objeto Curriculo na PessoaFisica
                command.Curriculo = commandCV;
                command.Curriculo.FuncaoPretendida = cmdFuncaoPretendida;

                //Consumir o Map
                var commandObject = AutoMapper.Mapper.Map<Command.CriarAtualizarPessoaFisica, Mapper.Models.PessoaFisica.PessoaFisica>(command);

                return commandObject;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro no mapeamento NEWBNE to OLD");
                throw;
            }
        }

        #endregion
        
        #region GerarHashAcesso
        public string GerarHashAcessoLoginAutomatico(decimal cpf, DateTime dataNascimento, string urlDestino)
        {
            if(urlDestino == null)
            {
                urlDestino = string.Empty;
            }

            var parametros = new 
            {
                NumeroCPF = cpf,
                DataNascimento = dataNascimento,
                Url = urlDestino
            };

            string json = Utils.ToJSON(parametros);
            return Utils.ToBase64(json);
        }
        #endregion
    }
}