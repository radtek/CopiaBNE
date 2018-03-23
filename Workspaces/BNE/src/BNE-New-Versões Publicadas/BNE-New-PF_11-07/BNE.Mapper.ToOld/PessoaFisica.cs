using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.EL;
using BNE.Mapper.Models.Vaga;
using ExperienciaProfissional = BNE.Mapper.Models.PessoaFisica.ExperienciaProfissional;
using Formacao = BNE.Mapper.Models.PessoaFisica.Formacao;
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
            BLL.PessoaFisica objPessoaFisica;
            BLL.PessoaFisica.CarregarPorCPF(Convert.ToDecimal(model.CPF), out objPessoaFisica);

            Curriculo objCurriculo;
            Curriculo.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objCurriculo);

            var objIndicacao = new BLL.Indicacao(objCurriculo, objPessoaFisica);

            objIndicacao.DataCadastro = DateTime.Now;

            foreach (var item in model.listaAmigos)
            {
                objIndicacao.AdicionarIndicado(item.Nome, "", "", item.Email);
            }

            var retorno = objIndicacao.Indicar();

            if (retorno)
            {
                retorno = SalvarCandidatura(objPessoaFisica.CPF, model.IdVaga, indicacaoAmigo, null);
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
                            Escolaridade = Escolaridade.LoadObject(commandObject.IdEscolaridade, trans),
                            Cidade = commandObject.IdCidade > 0 ? Cidade.LoadObject(commandObject.IdCidade, trans) : null,
                            Fonte = commandObject.IdInstituicaoEnsino > 0 ? Fonte.LoadObject(commandObject.IdInstituicaoEnsino, trans) : null,
                            Curso = commandObject.IdCurso > 0 ? Curso.LoadObject(commandObject.IdCurso, trans) : null,
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
                        var valorParametro = Parametro.RecuperaValorParametro(parametro, trans);

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
                            cvParametro.ValorParametro = valorParametro;

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
                                AreaBNE = commandObject.IdRamoEmpresa == 0 ? null : AreaBNE.LoadObject(commandObject.IdRamoEmpresa, trans),
                                PessoaFisica = BLL.PessoaFisica.CarregarPorCPF(commandObject.Cpf, trans)
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
                                NumeroCelular = commandObject.Celular.ToString()
                            };
                        }

                        #region [ Endereco Pessoa ]
                        Cidade cidade = null;
                        Endereco endereco = null;

                        if (commandObject.IdCidade > 0)
                        {
                            cidade = Cidade.LoadObject(commandObject.IdCidade);
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
                        objPessoaFisica.DataNascimento = commandObject.DataNascimento;
                        objPessoaFisica.Escolaridade = commandObject.IdEscolaridade > 0 ? Escolaridade.LoadObject(commandObject.IdEscolaridade) : null;
                        objPessoaFisica.Sexo = commandObject.IdSexo > 0 ? Sexo.LoadObject(commandObject.IdSexo) : null;
                        objPessoaFisica.Cidade = cidade;
                        objPessoaFisica.Endereco = endereco;
                        objPessoaFisica.SalvarMigracao(trans);

                        #region Curriculo
                        var objTipoCurriculo = TipoCurriculo.LoadObject(1);
                        var objSituacaoCurriculo = SituacaoCurriculo.LoadObject(commandObject.Curriculo.IdSituacaoCurriculo);

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

                        objCurriculo.FlagVIP = commandObject.Curriculo.FlgVIP;

                        //salvar curriculo na transação
                        objCurriculo.SalvarMigracao(trans);

                        #region CurriculoOrigem
                        CurriculoOrigem objCurriculoOrigem;
                        var objOrigem = Origem.LoadObject(commandObject.Curriculo.IdOrigem);

                        if (!CurriculoOrigem.ExisteCurriculoNaOrigem(objCurriculo, objOrigem))
                        {
                            objCurriculoOrigem = new CurriculoOrigem
                            {
                                Curriculo = objCurriculo,
                                DataCadastro = objCurriculo.DataCadastro,
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
                            var parametro = Parametro.LoadObject((int)BLL.Enumeradores.Parametro.QuantidadeCandidaturaDegustacao, trans);

                            //BH a degustação cai para 1 task:31173
                            if (commandObject.Curriculo.CurriculoParametro != null && EstaNaRegiaoDeCampanhaBH(commandObject.IdCidade))
                            {
                                parametro.ValorParametro = commandObject.Curriculo.CurriculoParametro.Valor;
                            }

                            parametroCurriculo = new ParametroCurriculo
                            {
                                DataCadastro = objCurriculo.DataCadastro,
                                Curriculo = objCurriculo,
                                FlagInativo = false,
                                Parametro = parametro,
                                ValorParametro = parametro.ValorParametro
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
                                Perfil = Perfil.LoadObject(3)
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
                            objFuncao = Funcao.LoadObject(commandObject.Curriculo.FuncaoPretendida.idFuncao.Value);

                            if (objFuncao != null)
                                existeFuncao = FuncaoPretendida.CarregarPorCurriculoFuncao(objCurriculo, objFuncao, out objFuncaoPretendida);
                        }

                        objFuncaoPretendida = new FuncaoPretendida
                        {
                            DataCadastro = commandObject.Curriculo.DataCadastro,
                            DescricaoFuncaoPretendida = commandObject.Curriculo.FuncaoPretendida.Descricao,
                            QuantidadeExperiencia = commandObject.Curriculo.FuncaoPretendida.TempoExperiencia,
                            Curriculo = objCurriculo,
                            Funcao = objFuncao
                        };

                        objFuncaoPretendida.SalvarMigracao(trans);
                        #endregion

                        #endregion

                        #region Cadidatar
                        //Se o cadastro for a partir de uma candidatura
                        if (commandObject.IdVaga > 0 && Candidatar)
                        {
                            var vaga = Vaga.LoadObject(commandObject.IdVaga, trans);
                            var idStatusVagaCandidato = validarStatusVaga(vaga);

                            //Cadastro novo pela vaga premium não deixar candidatar automatico por não ser vip
                            if (!ParametroVaga.Premium((int)BLL.Enumeradores.Parametro.VagaPremium, vaga.IdVaga))
                            {
                                VagaCandidato objVagaCandidato;

                                if (!VagaCandidato.CarregarPorVagaCurriculo(vaga.IdVaga, objCurriculo.IdCurriculo, out objVagaCandidato, trans))
                                {
                                    objVagaCandidato = new VagaCandidato
                                    {
                                        Curriculo = objCurriculo,
                                        Vaga = vaga,
                                        DataCadastro = objCurriculo.DataCadastro,
                                        FlagInativo = false,
                                        FlagAutoCandidatura = false,
                                        StatusCurriculoVaga = new StatusCurriculoVaga(idStatusVagaCandidato),
                                        StatusCandidatura = new BLL.StatusCandidatura((int)StatusCandidatura.Candidatado),
                                        OrigemCandidatura = new BLL.OrigemCandidatura((int)OrigemCandidatura.Site)
                                    };
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
                                        DataCadastro = objCurriculo.DataCadastro
                                    };

                                    objIntencao.SalvarMigracao(trans);
                                }

                                //Registrar visualização da vaga
                                VagaVisualizada.SalvarVisualizacaoVaga(vaga, objCurriculo, trans);

                                //Enviar SMS para usuário da empresa
                                vaga.CampanhaRecrutamentoNotificarCandidatura(objCurriculo, trans);
                            }

                            //Descontar a candidatura
                            if (!vaga.Filial.PossuiPlanoAtivo() && !vaga.FlagVagaArquivada)
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
        private int validarStatusVaga(Vaga vaga)
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

        public Tuple<Curriculo, ParametroCurriculo, bool> PodeCandidatarSe(decimal cpf, int idVaga)
        {
            var valorParametro = 1;

            Curriculo objCurriculo;
            Curriculo.CarregarPorCpf(cpf, out objCurriculo);

            ParametroCurriculo objParametroCuriculo;
            ParametroCurriculo.CarregarParametroPorCurriculo(BLL.Enumeradores.Parametro.QuantidadeCandidaturaDegustacao, objCurriculo, out objParametroCuriculo);
            //se nao for VIP retorna o saldo de candidaturas
            if (!objCurriculo.FlagVIP)
                valorParametro = objParametroCuriculo != null ? Convert.ToInt32(objParametroCuriculo.ValorParametro) : 0;

            return new Tuple<Curriculo, ParametroCurriculo, bool>(objCurriculo, objParametroCuriculo, valorParametro > 0);
        }

        public bool SalvarCandidatura(decimal cpf, int idVaga, bool indicacaoAmigo, List<Tuple<int, bool?, string>> listaPerguntas)
        {
            var vaga = Vaga.LoadObject(idVaga);

            var result = PodeCandidatarSe(cpf, idVaga);

            //não vip, não deixar candidatar a vaga premium
            if (!result.Item1.FlagVIP && ParametroVaga.Premium((int)BLL.Enumeradores.Parametro.VagaPremium, idVaga))
                return false;

            if (vaga.FlagVagaArquivada)
            {
                return MapCandidatar(result, idVaga, true, listaPerguntas, true);
            }

            //Se a empresa tem plano a candidatura para vaga é livre e não desconta degustação
            if (!vaga.Filial.PossuiPlanoAtivo())
            {
                //se tem saldo de degustação ou é indicação de amigo
                if (result.Item3 || indicacaoAmigo)
                {
                    if (indicacaoAmigo) //se indicação de amigo passa como vaga livre para não descontar candidatura
                        return MapCandidatar(result, idVaga, true, listaPerguntas, true);
                    return MapCandidatar(result, idVaga, false, listaPerguntas, false);
                }

                return false;
            }
            return MapCandidatar(result, idVaga, true, listaPerguntas, false);
        }

        public bool MapCandidatar(Tuple<Curriculo, ParametroCurriculo, bool> result, int idVaga, bool vagaLivre, List<Tuple<int, bool?, string>> listaPerguntas, bool oportunidade)
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
                        var idStatusVagaCandidato = validarStatusVaga(vaga);

                        VagaCandidato objVagaCandidato;
                        if (!VagaCandidato.CarregarPorVagaCurriculo(idVaga, result.Item1.IdCurriculo, out objVagaCandidato, trans))
                        {
                            objVagaCandidato = new VagaCandidato
                            {
                                Curriculo = result.Item1,
                                Vaga = vaga,
                                DataCadastro = DateTime.Now,
                                FlagInativo = false,
                                FlagAutoCandidatura = false,
                                StatusCurriculoVaga = new StatusCurriculoVaga(idStatusVagaCandidato),
                                StatusCandidatura = new BLL.StatusCandidatura((int)StatusCandidatura.Candidatado),
                                OrigemCandidatura = new BLL.OrigemCandidatura((int)OrigemCandidatura.Site)
                            };

                            objVagaCandidato.SalvarMigracao(trans);
                        }

                        //Enviar Cv para empresa
                        IntencaoFilial objIntensao;
                        if (!IntencaoFilial.CarregarPorFilialCurriculo(result.Item1.IdCurriculo, vaga.Filial.IdFilial, trans, out objIntensao))
                        {
                            objIntensao = new IntencaoFilial
                            {
                                Curriculo = result.Item1,
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
                        vaga.CampanhaRecrutamentoNotificarCandidatura(result.Item1, trans);

                        //Atualizar Saldo Degustação
                        if (!vagaLivre && !oportunidade)
                        {
                            result.Item2.ValorParametro = (Convert.ToInt32(result.Item2.ValorParametro) - 1).ToString();
                            result.Item2.DataAlteracao = DateTime.Now;
                            result.Item2.SalvarMigracao(trans);
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
                var objFuncaoCategoria = FuncaoCategoria.RecuperarCategoriaPorCurriculo(new Curriculo(idCurriculo));

                var planoPremium = new PlanoPremium
                {
                    PrecoVip = new Plano(Plano.RecuperarCodigoPlanoMensalPorFuncaoCategoria(objFuncaoCategoria)).RecuperarValor().ToString(),
                    PrecoCandidatura = new Plano(Convert.ToInt32(Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.PlanoCandidaturaPremium))).RecuperarValor().ToString()
                };

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

                #region Premium
                vagaSQL.FlgPremium = ParametroVaga.Premium((int)BLL.Enumeradores.Parametro.VagaPremium, vaga.IdVaga);
                //se for premium carregar os plano para mostrar na modal
                if (vagaSQL.FlgPremium)
                {
                    vagaSQL.PlanoPremium = new PlanoPremium
                    {
                        PrecoVip = Plano.LoadObject(Convert.ToInt32(Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.PlanoVIPCategoria130Dias))).ValorBase.ToString(),
                        PrecoCandidatura = Plano.LoadObject(Convert.ToInt32(Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.PlanoCandidaturaPremium))).ValorBase.ToString()
                    };
                }
                #endregion

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

        #region ECurriculoVIP
        public Tuple<bool, int> ECurriculoVIP(decimal cpf)
        {
            var retorno = false;

            int IdCurriculo;
            if (Curriculo.CarregarIdPorCpf(cpf, out IdCurriculo))
            {
                retorno = new Curriculo(IdCurriculo).VIP();
            }

            return new Tuple<bool, int>(retorno, IdCurriculo);
        }
        #endregion

        #region GetParametroCurriculo
        public string GetParametroCurriculo(int idCurriculo)
        {
            if (idCurriculo < 1)
                return "0";

            var objCurriculo = new Curriculo(idCurriculo);
            ParametroCurriculo objParametroCurriculo;
            ParametroCurriculo.CarregarParametroPorCurriculo(BLL.Enumeradores.Parametro.QuantidadeCandidaturaDegustacao, objCurriculo, out objParametroCurriculo);

            return objParametroCurriculo != null ? objParametroCurriculo.ValorParametro : "0";
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
        public DataTable CarregarInformacoesCurriculo(int idVaga, decimal cpf)
        {
            return Curriculo.CarregarInformacoesCurriculo(idVaga, cpf);
        }
        #endregion

        #region SalvarVisualizacaoVaga
        public static void SalvarVisualizacaoVaga(int idVaga, int idCurriculo)
        {
            try
            {
                VagaVisualizada.SalvarVisualizacaoVaga(new Vaga(idVaga), new Curriculo(idCurriculo));
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex, "Erro - Gravar visualização da vaga metodo SalvarVisualizacaoVaga - BNE novo");
            }
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

        public string RecuperarNomePessoaPorCPFDataNascimento(decimal cpf, DateTime dataNascimento)
        {
            return new BLL.PessoaFisica().RecuperarNomePessoaPorCPFDataNascimento(cpf, dataNascimento);
        }
    }
}