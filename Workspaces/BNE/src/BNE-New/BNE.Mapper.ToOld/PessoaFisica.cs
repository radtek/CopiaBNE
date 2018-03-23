using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.EL;
using BNE.Mapper.Models.PessoaFisica;
using BNE.Mapper.Models.Vaga;
using Curriculo = BNE.BLL.Curriculo;
using ExperienciaProfissional = BNE.Mapper.Models.PessoaFisica.ExperienciaProfissional;
using Formacao = BNE.Mapper.Models.PessoaFisica.Formacao;
using FuncaoPretendida = BNE.BLL.FuncaoPretendida;
using Indicacao = BNE.Mapper.Models.Indicacao.Indicacao;
using OrigemCandidatura = BNE.BLL.Enumeradores.OrigemCandidatura;
using StatusCandidatura = BNE.BLL.Enumeradores.StatusCandidatura;
using Vaga = BNE.BLL.Vaga;

namespace BNE.Mapper.ToOld
{
    public class PessoaFisica
    {
        public bool IndicarAmigos(Indicacao model)
        {
            var indicacaoAmigo = true;
            var cpf = Convert.ToDecimal(model.CPF);

            var objPessoaFisica = new BLL.PessoaFisica(BLL.PessoaFisica.CarregarIdPorCPF(cpf));
            var objCurriculo = new Curriculo(RecuperarIdCurriculoPorCPF(cpf));

            var objIndicacao = new BLL.Indicacao(objCurriculo, objPessoaFisica)
            {
                DataCadastro = DateTime.Now
            };

            foreach (var item in model.listaAmigos)
            {
                objIndicacao.AdicionarIndicado(item.Nome, "", "", item.Email);
            }

            var retorno = objIndicacao.Indicar();

            if (retorno)
            {
                retorno = SalvarCandidatura(cpf, model.IdVaga, indicacaoAmigo, null);
            }

            return retorno;
        }

        public bool MapFormacao(Formacao commandObject, int idVaga, bool Candidatar)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        var objFormacao = new BLL.Formacao
                        {
                            DataCadastro = commandObject.DataCadastro,
                            DataAlteracao = commandObject.DataAlteracao,
                            AnoConclusao = commandObject.AnoConclusao,
                            DescricaoCurso = commandObject.NomeCurso,
                            DescricaoFonte = commandObject.NomeInstituicao,
                            FlagInativo = !commandObject.Ativo,
                            Escolaridade = new Escolaridade(commandObject.IdEscolaridade),
                            Cidade = commandObject.IdCidade > 0 ? new Cidade(commandObject.IdCidade) : null,
                            Fonte = commandObject.IdInstituicaoEnsino > 0 ? new Fonte(commandObject.IdInstituicaoEnsino) : null,
                            Curso = commandObject.IdCurso > 0 ? new Curso(commandObject.IdCurso) : null,
                            PessoaFisica = new BLL.PessoaFisica(BLL.PessoaFisica.CarregarIdPorCPF(commandObject.Cpf, trans))
                        };

                        objFormacao.SalvarMigracao(trans);

                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
            if (Candidatar)
                SalvarCandidatura(commandObject.Cpf, idVaga, false, null);

            return true;
        }

        public bool MapCurriculoParametro(ExperienciaProfissional commandObject, int idVaga, bool candidatar)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        var parametro = BLL.Enumeradores.Parametro.CurriculoSemExperiencia;

                        Curriculo objCurriculo;
                        if (Curriculo.CarregarPorCpf(commandObject.Cpf, out objCurriculo, trans))
                        {
                            ParametroCurriculo cvParametro;
                            if (!ParametroCurriculo.CarregarParametroPorCurriculo(parametro, objCurriculo, out cvParametro, trans))
                            {
                                cvParametro = new ParametroCurriculo
                                {
                                    Parametro = new Parametro((int)parametro),
                                    Curriculo = objCurriculo,
                                    DataCadastro = DateTime.Now
                                };
                            }

                            cvParametro.DataAlteracao = DateTime.Now;
                            cvParametro.ValorParametro = Parametro.RecuperaValorParametro(parametro, trans);

                            cvParametro.SalvarMigracao(trans);

                            trans.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            if (candidatar)
                SalvarCandidatura(commandObject.Cpf, idVaga, false, null);

            return true;
        }

        public bool MapExperienciaProfissional(ExperienciaProfissional commandObject, int idVaga, bool salvarExperiencia, bool candidatar)
        {
            if (salvarExperiencia)
            {
                using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
                {
                    conn.Open();

                    using (var trans = conn.BeginTransaction())
                    {
                        try
                        {
                            Funcao objFuncao;
                            if (!Funcao.CarregarPorDescricao(commandObject.FuncaoExercida, out objFuncao, trans))
                            {
                                objFuncao = null;
                            }

                            var objExperiencia = new BLL.ExperienciaProfissional
                            {
                                DataCadastro = commandObject.DataCadastro,
                                DataAdmissao = commandObject.DataEntrada.Value.ToString("dd/MM/yyyy") != "01/01/0001" ? commandObject.DataEntrada.Value : DateTime.Now,
                                DataDemissao = commandObject.DataSaida,
                                FlagImportado = commandObject.FlgImportado,
                                FlagInativo = !commandObject.Ativo,
                                RazaoSocial = commandObject.NomeEmpresa,
                                DescricaoAtividade = commandObject.AtividadesExercidas,
                                DescricaoFuncaoExercida = commandObject.FuncaoExercida,
                                VlrSalario = commandObject.UltimoSalario,
                                Funcao = objFuncao,
                                AreaBNE = commandObject.IdRamoEmpresa == 0 ? null : new AreaBNE(commandObject.IdRamoEmpresa),
                                PessoaFisica = new BLL.PessoaFisica(BLL.PessoaFisica.CarregarIdPorCPF(commandObject.Cpf, trans))
                            };

                            objExperiencia.SalvarMigracao(trans);

                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                }
            }
            if (candidatar)
                SalvarCandidatura(commandObject.Cpf, idVaga, false, null);

            return true;
        }

        public bool Map(Models.PessoaFisica.PessoaFisica commandObject, out bool candidatura, bool Candidatar)
        {
            candidatura = false;
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        var novo = false;
                        BLL.PessoaFisica objPessoaFisica;
                        if (!BLL.PessoaFisica.CarregarPorCPF(commandObject.CPF, out objPessoaFisica))
                        {
                            novo = true;
                            objPessoaFisica = new BLL.PessoaFisica
                            {
                                CPF = commandObject.CPF,
                                DataCadastro = commandObject.DataCadastro,
                                NomePessoa = commandObject.Nome,
                                NomePessoaPesquisa = commandObject.Nome,
                                FlagInativo = commandObject.FlgInativo,
                                FlagEmailConfirmado = false,
                                FlagCelularConfirmado = false,
                                NumeroDDDCelular = commandObject.DDDCelular.ToString(),
                                NumeroCelular = commandObject.NumeroCelular.ToString(),
                                DataNascimento = commandObject.DataNascimento,
                                FlagWhatsApp = commandObject.FlgWhatsApp

                            };
                        }

                        #region [ Endereco Pessoa ]
                        Cidade cidade = null;
                        Endereco endereco = null;

                        if (commandObject.IdCidade > 0)
                        {
                            cidade = new Cidade(commandObject.IdCidade);
                            endereco = new Endereco { DataCadastro = DateTime.Now };

                            endereco.Cidade = cidade;
                            endereco.FlagInativo = false;
                            endereco.DataAlteracao = DateTime.Now;
                            endereco.NumeroCEP = null;
                            endereco.DescricaoLogradouro = null;
                            endereco.DescricaoBairro = null;
                            endereco.DescricaoComplemento = null;
                            endereco.NumeroEndereco = null;
                            endereco.SalvarMigracao(trans);
                        }
                        #endregion

                        objPessoaFisica.DataAlteracao = commandObject.DataAlteracao != null ? commandObject.DataAlteracao.Value : commandObject.DataCadastro;
                        objPessoaFisica.EmailPessoa = commandObject.Email;
                        objPessoaFisica.Escolaridade = commandObject.IdEscolaridade > 0 ? new Escolaridade(commandObject.IdEscolaridade) : null;
                        objPessoaFisica.Sexo = commandObject.IdSexo > 0 ? new Sexo(commandObject.IdSexo) : null;
                        objPessoaFisica.Cidade = cidade;
                        objPessoaFisica.Endereco = endereco;
                        objPessoaFisica.SalvarMigracao(trans);

                        #region Curriculo
                        var objTipoCurriculo = new TipoCurriculo(1);
                        var objSituacaoCurriculo = new SituacaoCurriculo(commandObject.Curriculo.IdSituacaoCurriculo);

                        Curriculo objCurriculo;
                        if (!Curriculo.CarregarPorCpf(commandObject.CPF, out objCurriculo))
                        {
                            objCurriculo = new Curriculo
                            {
                                PessoaFisica = objPessoaFisica,
                                DataCadastro = commandObject.Curriculo.DataCadastro,
                                FlagMSN = false
                            };
                        }

                        objCurriculo.ValorPretensaoSalarial = commandObject.Curriculo.PretensaoSalarial;
                        objCurriculo.TipoCurriculo = objTipoCurriculo;
                        objCurriculo.SituacaoCurriculo = objSituacaoCurriculo;
                        objCurriculo.DataAtualizacao = commandObject.Curriculo.DataAtualizacao != null ?
                            commandObject.Curriculo.DataAtualizacao.Value : commandObject.Curriculo.DataCadastro;

                        objCurriculo.FlagInativo = commandObject.Curriculo.FlgInativo;
                        objCurriculo.DescricaoIP = "novo -> velho";

                        //objCurriculo.FlagVIP = commandObject.Curriculo.FlgVIP;

                        //salvar curriculo na transação
                        objCurriculo.SalvarMigracao(trans);

                        #region CurriculoOrigem
                        var objOrigem = new Origem(commandObject.Curriculo.IdOrigem);

                        if (!CurriculoOrigem.ExisteCurriculoNaOrigem(objCurriculo, objOrigem))
                        {
                            var objCurriculoOrigem = new CurriculoOrigem
                            {
                                Curriculo = objCurriculo,
                                DataCadastro = DateTime.Now,
                                DataAlteracao = objCurriculo.DataAtualizacao,
                                Origem = objOrigem
                            };

                            //salvar Origem curriculo na transação
                            objCurriculoOrigem.SalvarMigracao(trans);
                        }
                        #endregion

                        #region CurriculoParametro
                        ParametroCurriculo parametroCurriculo;

                        if (!ParametroCurriculo.CarregarParametroPorCurriculo(BLL.Enumeradores.Parametro.QuantidadeCandidaturaDegustacao, objCurriculo, out parametroCurriculo, trans))
                        {
                            var parametro = new Parametro((int)BLL.Enumeradores.Parametro.QuantidadeCandidaturaDegustacao);
                            var valorParametro = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.QuantidadeCandidaturaDegustacao, trans);

                            //BH a degustação cai para 1 task:31173
                            if (commandObject.Curriculo.CurriculoParametro != null && EstaNaRegiaoDeCampanhaBH(commandObject.IdCidade))
                            {
                                valorParametro = commandObject.Curriculo.CurriculoParametro.Valor;
                            }

                            parametroCurriculo = new ParametroCurriculo
                            {
                                DataCadastro = DateTime.Now,
                                Curriculo = objCurriculo,
                                FlagInativo = false,
                                Parametro = parametro,
                                ValorParametro = valorParametro
                            };
                        }

                        parametroCurriculo.ValorParametro = (Convert.ToInt32(parametroCurriculo.ValorParametro)).ToString();

                        var numeroCandidaturas = parametroCurriculo.ValorParametro;
                        #endregion

                        #region Usuario
                        Usuario objUsuario;
                        if (!Usuario.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objUsuario, trans))
                        {
                            objUsuario = new Usuario
                            {
                                PessoaFisica = objPessoaFisica,
                                DataCadastro = objPessoaFisica.DataCadastro,
                                FlagInativo = false,
                                SenhaUsuario = "00000000",
                                DataAlteracao = objPessoaFisica.DataCadastro,
                                DescricaoSessionID = "novo -> velho"
                            };

                            objUsuario.SalvarMigracao(trans);
                        }
                        #endregion

                        #region UsuarioFilialPerfil
                        UsuarioFilialPerfil objUsuarioFilialPerfil;
                        if (!UsuarioFilialPerfil.CarregarUsuarioFilialPerfilCandidatoAtivo(objPessoaFisica, trans, out objUsuarioFilialPerfil))
                        {
                            objUsuarioFilialPerfil = new UsuarioFilialPerfil
                            {
                                PessoaFisica = objPessoaFisica,
                                DataCadastro = objPessoaFisica.DataCadastro,
                                DataAlteracao = objPessoaFisica.DataCadastro,
                                FlagInativo = false,
                                SenhaUsuarioFilialPerfil = objPessoaFisica.DataNascimento.ToString("dd/MM/yyyy"),
                                DescricaoIP = "novo -> velho",
                                FlagUsuarioResponsavel = false,
                                Perfil = new Perfil(3)
                            };

                            objUsuarioFilialPerfil.SalvarMigracao(trans);
                        }
                        #endregion

                        #region FuncaoPretendida
                        Funcao objFuncao = null;
                        FuncaoPretendida objFuncaoPretendida;
                        var existeFuncao = false;

                        if (commandObject.Curriculo.FuncaoPretendida.idFuncao != null && commandObject.Curriculo.FuncaoPretendida.idFuncao > 0)
                        {
                            objFuncao = new Funcao(commandObject.Curriculo.FuncaoPretendida.idFuncao.Value);

                            if (objFuncao != null)
                                existeFuncao = FuncaoPretendida.CarregarPorCurriculoFuncao(objCurriculo, objFuncao, null, out objFuncaoPretendida);
                        }
                        if (!existeFuncao)
                        {
                            objFuncaoPretendida = new FuncaoPretendida
                            {
                                DataCadastro = commandObject.Curriculo.DataCadastro,
                                DescricaoFuncaoPretendida = commandObject.Curriculo.FuncaoPretendida.Descricao,
                                QuantidadeExperiencia = commandObject.Curriculo.FuncaoPretendida.TempoExperiencia,
                                Curriculo = objCurriculo,
                                Funcao = objFuncao
                            };
                            objFuncaoPretendida.SalvarMigracao(trans);
                        }

                        #endregion

                        #endregion

                        #region Cadidatar
                        //Se o cadastro for a partir de uma candidatura
                        if (commandObject.IdVaga > 0 && Candidatar)
                        {
                            var vaga = Vaga.LoadObject(commandObject.IdVaga, trans);
                            var idStatusVagaCandidato = ValidarStatusVaga(vaga);

                            //Cadastro novo pela vaga premium não deixar candidatar automatico por não ser vip

                            VagaCandidato objVagaCandidato;

                            if (!VagaCandidato.CarregarPorVagaCurriculo(vaga.IdVaga, objCurriculo.IdCurriculo, out objVagaCandidato, trans))
                            {
                                objVagaCandidato = new VagaCandidato(objCurriculo, vaga);
                                objVagaCandidato.DataCadastro = DateTime.Now;
                                objVagaCandidato.FlagInativo = false;
                                objVagaCandidato.FlagAutoCandidatura = false;
                                objVagaCandidato.StatusCurriculoVaga = new StatusCurriculoVaga(idStatusVagaCandidato);
                                objVagaCandidato.StatusCandidatura = new BLL.StatusCandidatura((int)StatusCandidatura.Candidatado);
                                objVagaCandidato.OrigemCandidatura = new BLL.OrigemCandidatura((int)OrigemCandidatura.SitePF);
               
                                objVagaCandidato.SalvarMigracao(trans);
                                candidatura = true;
                            }

                            //Enviar Cv para empresa
                            IntencaoFilial objIntencao;
                            if (!IntencaoFilial.CarregarPorFilialCurriculo(objCurriculo.IdCurriculo, vaga.Filial.IdFilial, trans, out objIntencao))
                            {
                                objIntencao = new IntencaoFilial
                                {
                                    Curriculo = objCurriculo,
                                    Filial = vaga.Filial,
                                    FlagInativo = false,
                                    DataCadastro = DateTime.Now
                                };

                                objIntencao.SalvarMigracao(trans);
                            }

                            //Registrar visualização da vaga
                            VagaVisualizada.SalvarVisualizacaoVaga(vaga, objCurriculo, trans);

                            //Enviar SMS para usuário da empresa
                            vaga.CampanhaRecrutamentoNotificarCandidatura(objCurriculo, trans);

                            //Descontar a candidatura
                            if (!vaga.Filial.PossuiPlanoAtivo() && !vaga.FlagVagaArquivada && !vaga.ExistePerguntas()) //Se existem perguntas cairá em outro fluxo para descontar as candidaturas.
                            {
                                numeroCandidaturas = (Convert.ToInt32(numeroCandidaturas) - 1).ToString();
                            }
                        }
                        #endregion

                        parametroCurriculo.ValorParametro = numeroCandidaturas;
                        parametroCurriculo.DataAlteracao = DateTime.Now.Date;
                        parametroCurriculo.DataCadastro = DateTime.Now;
                        parametroCurriculo.SalvarMigracao(trans);

                        trans.Commit();
                        if (novo)  //Task 45613
                            objCurriculo.AdicionarAtividadeAssincronoSalvarAlertasCurriculo(objPessoaFisica.EmailPessoa, objCurriculo.IdCurriculo);

                        return true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        //validar status vaga
        //1- Sem envio, quando a vaga estiver fechada ou as flags de receber CV na vaga forem false.
        //2- aguardando envio, quando a flag receber todos for true e a flag receber cada for false.
        //3- Enviado, quando a flag receber cada for true e/ou a flag receber todos for false.
        private int ValidarStatusVaga(Vaga vaga)
        {
            var result = 1;

            if (vaga.FlagReceberCadaCV.HasValue && vaga.FlagReceberCadaCV.Value && vaga.FlagReceberTodosCV.HasValue && vaga.FlagReceberTodosCV.Value)
            {
                result = (int)BLL.Enumeradores.StatusCurriculoVaga.AguardoEnvio;
            }
            else if (vaga.FlagReceberCadaCV != null && vaga.FlagReceberCadaCV.Value)
            {
                result = (int)BLL.Enumeradores.StatusCurriculoVaga.Enviado;
            }
            else if (vaga.FlagReceberTodosCV.HasValue && vaga.FlagReceberTodosCV.Value)
            {
                result = (int)BLL.Enumeradores.StatusCurriculoVaga.AguardoEnvio;
            }

            return result;
        }

        public PodeCandidatar PodeCandidatarSe(decimal cpf, int idVaga)
        {
            var objCurriculo = new Curriculo(RecuperarIdCurriculoPorCPF(cpf));

            if (CurriculoVIP(objCurriculo.IdCurriculo))
                return new PodeCandidatar { CurriculoVIP = true, IdCurriculo = objCurriculo.IdCurriculo };

            return new PodeCandidatar { CurriculoVIP = false, IdCurriculo = objCurriculo.IdCurriculo, QuantidadeCandidaturaDegustacao = RecuperarSaldoCandidatura(objCurriculo.IdCurriculo) };
        }

        public bool SalvarCandidatura(decimal cpf, int idVaga, bool indicacaoAmigo, List<Tuple<int, bool?, string>> listaPerguntas)
        {
            var vaga = Vaga.LoadObject(idVaga);

            var result = PodeCandidatarSe(cpf, idVaga);

            if (vaga.FlagVagaArquivada)
            {
                return MapCandidatar(result, idVaga, true, listaPerguntas, true);
            }

            //Se a empresa tem plano a candidatura para vaga é livre e não desconta degustação
            if (!vaga.Filial.PossuiPlanoAtivo())
            {
                //se tem saldo de degustação ou é indicação de amigo
                if (result.QuantidadeCandidaturaDegustacao > 0 || result.CurriculoVIP || indicacaoAmigo)
                {
                    if (indicacaoAmigo) //se indicação de amigo passa como vaga livre para não descontar candidatura
                        return MapCandidatar(result, idVaga, true, listaPerguntas, true);
                    return MapCandidatar(result, idVaga, false, listaPerguntas, false);
                }

                return false;
            }
            return MapCandidatar(result, idVaga, true, listaPerguntas, false);
        }

        public bool MapCandidatar(PodeCandidatar result, int idVaga, bool vagaLivre, List<Tuple<int, bool?, string>> listaPerguntas, bool oportunidade)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        #region Cadidatar
                        var vaga = Vaga.LoadObject(idVaga, trans);
                        var idStatusVagaCandidato = ValidarStatusVaga(vaga);

                        VagaCandidato objVagaCandidato;
                        if (!VagaCandidato.CarregarPorVagaCurriculo(idVaga, result.IdCurriculo, out objVagaCandidato, trans))
                        {
                            objVagaCandidato = new VagaCandidato(new Curriculo(result.IdCurriculo), vaga);
                            objVagaCandidato.DataCadastro = DateTime.Now;
                            objVagaCandidato.FlagInativo = false;
                            objVagaCandidato.FlagAutoCandidatura = false;
                            objVagaCandidato.StatusCurriculoVaga = new StatusCurriculoVaga(idStatusVagaCandidato);
                            objVagaCandidato.StatusCandidatura = new BLL.StatusCandidatura((int)StatusCandidatura.Candidatado);
                            objVagaCandidato.OrigemCandidatura = new BLL.OrigemCandidatura((int)OrigemCandidatura.SitePF);
                            objVagaCandidato.SalvarMigracao(trans);
                        }

                        //Enviar Cv para empresa
                        IntencaoFilial objIntensao;
                        if (!IntencaoFilial.CarregarPorFilialCurriculo(result.IdCurriculo, vaga.Filial.IdFilial, trans, out objIntensao))
                        {
                            objIntensao = new IntencaoFilial
                            {
                                Curriculo = new Curriculo(result.IdCurriculo),
                                Filial = vaga.Filial,
                                FlagInativo = false,
                                DataCadastro = DateTime.Now
                            };

                            objIntensao.SalvarMigracao(trans);
                        }
                        #endregion

                        #region SalvarRepostas das perguntas
                        if (listaPerguntas != null)
                        {
                            foreach (var pergunta in listaPerguntas)
                            {
                                var objVagaCandidaturaPerunta = new VagaCandidatoPergunta
                                {
                                    VagaPergunta = new VagaPergunta(pergunta.Item1),
                                    FlagResposta = pergunta.Item2,
                                    DescricaoResposta = pergunta.Item3,
                                    VagaCandidato = objVagaCandidato
                                };
                                objVagaCandidaturaPerunta.Save(trans);
                            }
                        }
                        #endregion

                        //Enviar SMS para usuário da empresa
                        vaga.CampanhaRecrutamentoNotificarCandidatura(new Curriculo(result.IdCurriculo), trans);

                        //Atualizar Saldo Degustação
                        if (!vagaLivre && !oportunidade && !result.CurriculoVIP && result.QuantidadeCandidaturaDegustacao > 0)
                        {
                            ParametroCurriculo objParametroCurriculo;
                            if (ParametroCurriculo.CarregarParametroPorCurriculo(BLL.Enumeradores.Parametro.QuantidadeCandidaturaDegustacao, new Curriculo(result.IdCurriculo), out objParametroCurriculo, trans))
                            {
                                objParametroCurriculo.ValorParametro = (result.QuantidadeCandidaturaDegustacao - 1).ToString();
                                objParametroCurriculo.DataAlteracao = DateTime.Now;
                                objParametroCurriculo.SalvarMigracao(trans);
                            }
                        }

                        trans.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        GerenciadorException.GravarExcecao(ex, "ERRO - bne novo metodo - MapCandidatar");
                        return false;
                    }
                }
            }
        }

        #region ExistePessoaFisica
        /// <summary>
        ///     Verifica no banco velho se existe pessoa fisica
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public bool ExistePessoaFisica(decimal cpf)
        {
            int idPessoaFisica;
            return BLL.PessoaFisica.ExistePessoaFisica(cpf, out idPessoaFisica);
        }
        #endregion

        #region PessoaFisicaIntiva
        public bool PessoaFisicaInativa(decimal cpf)
        {
            return new BLL.PessoaFisica().Inativo(cpf);
        }
        #endregion

        #region ExisteCurriculo
        public bool ExisteCurriculo(int idPessoaFisica)
        {
            int idCurriculo;
            return Curriculo.ExisteCurriculo(new BLL.PessoaFisica(idPessoaFisica), out idCurriculo);
        }
        #endregion

        #region RecuperarIdCurriculoPorCPF
        public int RecuperarIdCurriculoPorCPF(decimal numeroCPF)
        {
            int idPessoaFisica;
            if (BLL.PessoaFisica.ExistePessoaFisica(numeroCPF, out idPessoaFisica))
            {
                return Curriculo.RecuperarIdPorPessoaFisica(new BLL.PessoaFisica(idPessoaFisica));
            }
            return 0;
        }
        #endregion

        #region CarregarIdSeExistePessoaFisica
        /// <summary>
        ///     Verifica no banco velho se existe pessoa fisica
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public int CarregarIdSeExistePessoaFisica(decimal cpf)
        {
            int idPessoaFisica;
            if (BLL.PessoaFisica.ExistePessoaFisica(cpf, out idPessoaFisica))
            {
                return idPessoaFisica;
            }

            return 0;
        }
        #endregion

        #region CarregarNomePessoaFisica
        /// <summary>
        ///     Verifica no banco velho se existe pessoa fisica
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public string CarregarPessoaFisica(decimal cpf)
        {
            return new BLL.PessoaFisica(BLL.PessoaFisica.CarregarIdPorCPF(cpf)).RecuperarNomePessoa();
        }
        #endregion

        #region EstaNaRegiaoDeCampanhaBH
        /// <summary>
        /// </summary>
        /// <param name="idCidade"></param>
        /// <returns></returns>
        public bool EstaNaRegiaoDeCampanhaBH(int idCidade)
        {
            return IndiqueTresAmigos.EstaNaRegiaoDeCampanha(idCidade);
        }
        #endregion

        #region CarregarPlanoPremium
        /// <summary>
        ///     Carregar o plano vip de acordo com a funcao e o plano da vaga premium
        /// </summary>
        /// <param name="cpf">Número do CPF</param>
        /// <returns></returns>
        public PlanoPremium CarregarPlanoPremium(decimal cpf)
        {
            int idCurriculo;
            Curriculo.CarregarIdPorCpf(cpf, out idCurriculo); //Recupera o id do currículo
            if (idCurriculo > 0)
            {
                //TODO Melhorar para trazer apenas o id da função categoria
                var objFuncaoCategoria = FuncaoCategoria.RecuperarCategoriaPorCurriculo(new Curriculo(idCurriculo));

                var planoPremium = Recuperar(objFuncaoCategoria);

                return planoPremium;
            }
            return null;
        }
        #endregion

        #region CarregarVagaSQL
        public Models.Vaga.Vaga CarregarVagaSQL(int idVaga)
        {
            var vaga = Vaga.LoadObject(idVaga);
            Models.Vaga.Vaga vagaSQL = null;

            if (vaga != null)
            {
                vaga.Cidade.CompleteObject();
                vaga.Funcao.CompleteObject();
                vaga.Funcao.AreaBNE.CompleteObject();

                var objTipoVinculo = VagaTipoVinculo.ListarTipoVinculoPorVaga(idVaga, null);

                vagaSQL = new Models.Vaga.Vaga();

                vagaSQL.IdVaga = vaga.IdVaga;
                vagaSQL.IdFuncao = vaga.Funcao.IdFuncao;
                vagaSQL.Funcao = vaga.Funcao.DescricaoFuncao;
                vagaSQL.IdCidade = vaga.Cidade.IdCidade;
                vagaSQL.SalarioDe = vaga.ValorSalarioDe != null ? vaga.ValorSalarioDe.Value : 0;
                vagaSQL.SalarioAte = vaga.ValorSalarioPara != null ? vaga.ValorSalarioPara.Value : 0;
                vagaSQL.Cidade = vaga.Cidade.NomeCidade;
                vagaSQL.UF = vaga.Cidade.Estado.SiglaEstado;
                vagaSQL.CodigoVaga = vaga.CodigoVaga;
                vagaSQL.Atribuicoes = vaga.DescricaoAtribuicoes;
                vagaSQL.Beneficios = vaga.DescricaoBeneficio;
                vagaSQL.Requisitos = vaga.DescricaoRequisito;
                vagaSQL.DataAnuncio = vaga.DataAbertura.Value;
                vagaSQL.FlgAuditada = vaga.FlagAuditada != null ? vaga.FlagAuditada.Value : false;
                vagaSQL.NomeEmpresa = vaga.NomeEmpresa;
                vagaSQL.Descricao = vaga.DescricaoFuncao;
                vagaSQL.FlgArquivada = vaga.FlagVagaArquivada;
                vagaSQL.FlgInativo = vaga.FlagInativo;
                vagaSQL.FlgConfidencial = vaga.FlagConfidencial;
                vagaSQL.DescricaoAreaBNEPesquisa = vaga.Funcao.AreaBNE.DescricaoAreaBNEPesquisa;
                vagaSQL.FlgDeficiencia = vaga.FlagDeficiencia != null ? vaga.FlagDeficiencia.Value : false;
                vagaSQL.Bairro = vaga.NomeBairro != null ? Helper.AjustarString(vaga.NomeBairro) : string.Empty;
                //descriçaõ de deficiencia no modelo novo descomentar quando subir toda a alteração
                //vagaSQL.DescricaoDeficiencia = vagaSQL.FlgDeficiencia == true ? BLL.VagaDeficiencia.ViewDeficiencias(vaga.IdVaga) : string.Empty;
                if (vaga.Deficiencia != null)
                {
                    vaga.Deficiencia.CompleteObject();
                    vagaSQL.DescricaoDeficiencia = vaga.Deficiencia.DescricaoDeficiencia;
                    vagaSQL.Idf_Deficiencia = vaga.Deficiencia.IdDeficiencia;
                }


                //vagaSQL.Perguntas = GetPergunta(vaga.IdVaga);



                if (objTipoVinculo.Count > 0)
                {
                    foreach (var item in objTipoVinculo)
                    {
                        switch (item.TipoVinculo.IdTipoVinculo)
                        {
                            case 4:
                                vagaSQL.IdTipoVinculo = 4;
                                vagaSQL.eEstagio = true;
                                vagaSQL.eEfetivo = false;
                                break;
                            case 1:
                                vagaSQL.IdTipoVinculo = 1;
                                vagaSQL.eAprendiz = true;
                                break;
                            default:
                                vagaSQL.IdTipoVinculo = 3;
                                vagaSQL.eEfetivo = true;
                                break;
                        }
                    }
                }
                else
                {
                    vagaSQL.IdTipoVinculo = 3;
                    vagaSQL.eEfetivo = true;
                }
            }
            return vagaSQL;
        }
        #endregion

        #region CurriculoVIP
        public bool CurriculoVIP(int curriculo)
        {
            if (curriculo > 0)
            {
                return new Curriculo(curriculo).VIP();
            }

            return false;
        }
        #endregion

        #region RecuperarSaldoCandidatura
        public int RecuperarSaldoCandidatura(int idCurriculo)
        {
            if (idCurriculo > 0)
            {
                var objCurriculo = new Curriculo(idCurriculo);

                var valor = ParametroCurriculo.RecuperarValorParametroPorCurriculo(BLL.Enumeradores.Parametro.QuantidadeCandidaturaDegustacao, objCurriculo);
                if (string.IsNullOrWhiteSpace(valor))
                {
                    return 0;
                }
                return Convert.ToInt32(valor);
            }

            return 0;
        }
        #endregion

        #region GetLinksPaginasSemelhantes
        public string[] GetLinksPaginasSemelhantes(string funcao, string cidade, string siglaEstado, string areaBNE)
        {
            var urls = new string[4];

            urls[0] = SitemapHelper.MontarUrlVagasPorFuncao(funcao);
            urls[1] = SitemapHelper.MontarUrlVagasPorCidade(cidade, siglaEstado);
            urls[2] = SitemapHelper.MontarUrlVagasPorFuncaoCidade(funcao, cidade, siglaEstado);
            urls[3] = SitemapHelper.MontarUrlVagasPorArea(areaBNE);

            return urls;
        }
        #endregion

        #region [ CarregarInformacoesCurriculo ]
        public DataTable CarregarInformacoesCurriculo(decimal cpf, int idVaga)
        {
            return Curriculo.CarregarInformacoesCurriculo(idVaga, cpf);
        }
        #endregion

        #region SalvarVisualizacaoVaga
        public static void SalvarVisualizacaoVaga(int idVaga, int idCurriculo)
        {
            VagaVisualizada.SalvarVisualizacaoVaga(new Vaga(idVaga), new Curriculo(idCurriculo));
        }
        #endregion

        #region Perguntas
        public List<Pergunta> GetPergunta(int idVaga)
        {
            var lista = VagaPergunta.RecuperarListaPerguntas(idVaga, null);
            var listaPergunta = new List<Pergunta>();
            if (lista.Count > 0)
            {
                foreach (var pergunta in lista)
                {
                    listaPergunta.Add(new Pergunta
                    {
                        descricaoVagaPergunta = pergunta.DescricaoVagaPergunta,
                        idVagaPergunta = pergunta.IdVagaPergunta,
                        tipoResposta = pergunta.TipoResposta.IdTipoResposta
                    });
                }
            }
            return listaPergunta;
        }
        #endregion

        #region AjustarString
        public static string AjustarString(string entrada)
        {
            return Helper.AjustarString(entrada);
        }
        #endregion

        public PlanoPremium Recuperar(FuncaoCategoria objFuncaoCategoria = null)
        {
            var objPlanoPremium = new PlanoPremium();
            if (objFuncaoCategoria != null)
            {
                objPlanoPremium.PrecoVip = new Plano(Plano.RecuperarCodigoPlanoMensalPorFuncaoCategoria(objFuncaoCategoria)).RecuperarValor();
            }
            else
            {
                objPlanoPremium.PrecoVip = new BLL.Plano(Convert.ToInt32(Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.PlanoVIPCategoria130Dias))).RecuperarValor();
            }
            objPlanoPremium.PrecoCandidatura = new Plano(Convert.ToInt32(Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.PlanoCandidaturaPremium))).RecuperarValor();

            return objPlanoPremium;
        }

        public string RecuperarNomePessoaPorCPFDataNascimento(decimal cpf, DateTime dataNascimento)
        {
            return new BLL.PessoaFisica().RecuperarNomePessoaPorCPFDataNascimento(cpf, dataNascimento);
        }

        public struct PodeCandidatar
        {
            public int IdCurriculo { get; set; }
            public bool CurriculoVIP { get; set; }
            public int QuantidadeCandidaturaDegustacao { get; set; }
            public ParametroCurriculo ParametroCurriculo { get; set; }
        }


        #region [DadosDaEmpresa]
        public Models.PessoaFisica.DadosEmpresaModel DadosDaEmpresa(int? idCurriculo, int idVaga)
        {
            var objDadosEmpresa = new Models.PessoaFisica.DadosEmpresaModel();

            var objVaga = BLL.Vaga.LoadObject(idVaga);

            if (objVaga.FlagConfidencial)
            {
                objDadosEmpresa.VagaConfidencial = true;
                objDadosEmpresa.MensagemEmpresaConfidencial = "Esta empresa optou por fazer um processo de recrutamento sigiloso.";
            }
            else
            {
                if (idCurriculo.HasValue)
                {
                    var objCurriculo = new Curriculo(idCurriculo.Value);
                    var objFuncaoCategoria = FuncaoCategoria.RecuperarCategoriaPorCurriculo(objCurriculo);
                    objDadosEmpresa.ValorPlanoVIP = new Plano(Plano.RecuperarCodigoPlanoMensalPorFuncaoCategoria(objFuncaoCategoria)).RecuperarValor();
                    objDadosEmpresa.CurriculoVIP = objCurriculo.VIP();
                }
                else
                {
                    objDadosEmpresa.CurriculoVIP = false;
                }

                objVaga.Filial.CompleteObject();
                objVaga.Filial.Endereco.CompleteObject();
                objVaga.Filial.Endereco.Cidade.CompleteObject();
                objDadosEmpresa.NumeroCNPJ = objVaga.Filial.NumeroCNPJ.Value;

                //Verificando se a visualização é para uma vaga específica
                //utilizado para somente exibir a empresa e telefone da vaga se o tipo da origem for parceiro (não há filial cadastrada)
                Origem origem = objVaga.Origem;
                if (origem.TipoOrigem == null)
                {
                    origem.CompleteObject();
                }

                if (origem != null && origem.TipoOrigem.IdTipoOrigem == (int)BLL.Enumeradores.TipoOrigem.Parceiro)
                {
                    //TODO: Charan => Criar um parametro para a Origem Sine
                    objDadosEmpresa.VagaSine = objVaga.Filial.IdFilial == 158198;

                    objDadosEmpresa.NomeEmpresa = objVaga.NomeEmpresa;
                    if (String.IsNullOrEmpty(objVaga.NumeroDDD) || String.IsNullOrEmpty(objVaga.NumeroTelefone))
                        objDadosEmpresa.NumeroTelefone = "Não Informado";
                    else if (objVaga.FlagVagaArquivada)
                        objDadosEmpresa.NumeroTelefone = null;
                    else
                        objDadosEmpresa.NumeroTelefone = Helper.FormatarTelefone(objVaga.NumeroDDD, objVaga.NumeroTelefone);

                    //Buscar na API do SINE os dados, dataCadastro empresa, QTD vagas
                    try
                    {
//                        BLL.VagaIntegracao objVagaIntegracao;
//                        BLL.VagaIntegracao.RecuperarIntegradorPorVaga(objVaga.IdVaga, out objVagaIntegracao);

//#if DEBUG
//                        string urlApiSine = String.Format("http://localhost:61291/v1.0/");
//#else
//                            string urlApiSine = Parametro.RecuperaValorParametro(Enumeradores.Parametro.SineApi);
//#endif

//                        var service = new HttpClient();

//                        System.Net.Http.HttpResponseMessage response = service.GetAsync(urlApiSine + "User/GetEstatisticasUsuario?idVaga=" + objVagaIntegracao.CodigoVagaIntegrador).Result;

//                        if (response.IsSuccessStatusCode)
//                        {
//                            var retorno = response.Content.ReadAsStringAsync().Result;
//                            dynamic result = JsonConvert.DeserializeObject(retorno);

//                            objDadosEmpresa.DataCadastro = result.DataCadastro;
//                            objDadosEmpresa.QuantidadeVagasDivulgadas = result.TotalVagasAnunciadas;
//                        }
                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex, "Erro ao consultar dados da empresa no Sine");
                    }
                }
                else
                {
                    if (objVaga.FlagVagaArquivada)
                        objDadosEmpresa.DesAreaBne = Filial.AreaBne(objVaga.Filial.IdFilial);

                    objDadosEmpresa.NomeEmpresa = objVaga.Filial.RazaoSocial;
                    objDadosEmpresa.DataCadastro = objVaga.Filial.DataCadastro;
                    objDadosEmpresa.QuantidadeFuncionarios = objVaga.Filial.QuantidadeFuncionarios;
                    objDadosEmpresa.Cidade = Helper.FormatarCidade(objVaga.Filial.Endereco.Cidade.NomeCidade, objVaga.Filial.Endereco.Cidade.Estado.SiglaEstado);
                    objDadosEmpresa.Bairro = objVaga.Filial.Endereco.DescricaoBairro;
                    objDadosEmpresa.QuantidadeCurriculosVisualizados = objVaga.Filial.RecuperarQuantidadeCurriculosVisualizados();
                    objDadosEmpresa.QuantidadeVagasDivulgadas = objVaga.Filial.RecuperarQuantidadeVagasDivuldadas();
                    objDadosEmpresa.NumeroTelefone = objVaga.FlagVagaArquivada ? null : Helper.FormatarTelefone(objVaga.Filial.NumeroDDDComercial, objVaga.Filial.NumeroComercial);
                }
            }
            return objDadosEmpresa;
        }
        #endregion
    }

}