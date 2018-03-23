using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BNE.Mapper.ToOld
{
    public class PessoaFisica
    {
        public bool MapFormacao(Models.PessoaFisica.Formacao commandObject, int idVaga)
        {
            using (SqlConnection conn = new SqlConnection(BLL.DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
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
                            Escolaridade = BLL.Escolaridade.LoadObject(commandObject.IdEscolaridade),
                            Cidade = commandObject.IdCidade > 0 ? BLL.Cidade.LoadObject(commandObject.IdCidade) : null,
                            Fonte = commandObject.IdInstituicaoEnsino> 0 ? BLL.Fonte.LoadObject(commandObject.IdInstituicaoEnsino): null,
                            Curso = commandObject.IdCurso > 0 ? BLL.Curso.LoadObject(commandObject.IdCurso) : null,
                            PessoaFisica = BLL.PessoaFisica.CarregarPorCPF(commandObject.Cpf),
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

            SalvarCandidatura(commandObject.Cpf, idVaga);
            return true;
        }

        public bool MapExperienciaProfissional(Models.PessoaFisica.ExperienciaProfissional commandObject, int idVaga, bool salvarExperiencia)
        {
            if (salvarExperiencia)
            {
                using (SqlConnection conn = new SqlConnection(BLL.DataAccessLayer.CONN_STRING))
                {
                    conn.Open();

                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            BLL.Funcao objFuncao;
                            if (!BLL.Funcao.CarregarPorDescricao(commandObject.FuncaoExercida, out objFuncao, trans))
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
                                AreaBNE = commandObject.IdRamoEmpresa == 0 ? null : BLL.AreaBNE.LoadObject(commandObject.IdRamoEmpresa),
                                PessoaFisica = BLL.PessoaFisica.CarregarPorCPF(commandObject.Cpf),
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

            SalvarCandidatura(commandObject.Cpf, idVaga);

            return true;
        }

        public bool Map(Models.PessoaFisica.PessoaFisica commandObject)
        {
            using (SqlConnection conn = new SqlConnection(BLL.DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using(SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        bool novo = false;
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

                        objPessoaFisica.DataAlteracao = commandObject.DataAlteracao != null ? commandObject.DataAlteracao.Value : commandObject.DataCadastro;
                        objPessoaFisica.EmailPessoa = commandObject.Email;
                        objPessoaFisica.DataNascimento = commandObject.DataNascimento;
                        objPessoaFisica.Escolaridade = commandObject.IdEscolaridade > 0 ? BLL.Escolaridade.LoadObject(commandObject.IdEscolaridade): null;
                        objPessoaFisica.Sexo = commandObject.IdSexo > 0 ? BLL.Sexo.LoadObject(commandObject.IdSexo): null;
                        objPessoaFisica.Cidade = commandObject.IdCidade >0 ? BLL.Cidade.LoadObject(commandObject.IdCidade) : null;
                        //objPessoaFisica.Deficiencia = commandObject.IdDeficiencia > 0 ? BLL.Deficiencia.LoadObject(commandObject.IdDeficiencia) : null;

                        objPessoaFisica.SalvarMigracao(trans);

                        #region Curriculo
                        BLL.TipoCurriculo objTipoCurriculo = BLL.TipoCurriculo.LoadObject(1);
                        BLL.SituacaoCurriculo objSituacaoCurriculo = BLL.SituacaoCurriculo.LoadObject(commandObject.Curriculo.IdSituacaoCurriculo);

                        BLL.Curriculo objCurriculo;
                        if (!BLL.Curriculo.CarregarPorCpf(commandObject.CPF, out objCurriculo))
                        {
                            objCurriculo = new BLL.Curriculo
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
                        BLL.CurriculoOrigem objCurriculoOrigem;
                        BLL.Origem objOrigem = BLL.Origem.LoadObject(commandObject.Curriculo.IdOrigem);

                        if (!BLL.CurriculoOrigem.ExisteCurriculoNaOrigem(objCurriculo, objOrigem))
                        {
                            objCurriculoOrigem = new BLL.CurriculoOrigem
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
                        BLL.ParametroCurriculo parametroCurriculo;

                        if(!BLL.ParametroCurriculo.CarregarParametroPorCurriculo(BLL.Enumeradores.Parametro.QuantidadeCandidaturaDegustacao,objCurriculo, out parametroCurriculo,trans))
                        {
                            var parametro = BLL.Parametro.LoadObject((int)BLL.Enumeradores.Parametro.QuantidadeCandidaturaDegustacao, trans);

                            parametroCurriculo = new BLL.ParametroCurriculo
                            {
                                DataCadastro = objCurriculo.DataCadastro,
                                Curriculo = objCurriculo,
                                FlagInativo = false,
                                Parametro = parametro,
                                ValorParametro = parametro.ValorParametro
                            };
                        }

                        //parametroCurriculo.ValorParametro = (Convert.ToInt32(parametroCurriculo.ValorParametro) - 1).ToString();
                        //parametroCurriculo.SalvarMigracao(trans);

                        #endregion

                        #region Usuario
                        BLL.Usuario objUsuario;
                        if(!BLL.Usuario.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica,out objUsuario,trans))
                        {
                            objUsuario = new BLL.Usuario
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
                        BLL.UsuarioFilialPerfil objUsuarioFilialPerfil;
                        if (!BLL.UsuarioFilialPerfil.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objUsuarioFilialPerfil, trans))
                        {
                            objUsuarioFilialPerfil = new BLL.UsuarioFilialPerfil()
                            {
                                PessoaFisica = objPessoaFisica,
                                DataCadastro = objPessoaFisica.DataCadastro,
                                DataAlteracao = objPessoaFisica.DataCadastro,
                                FlagInativo = false,
                                SenhaUsuarioFilialPerfil = objPessoaFisica.DataNascimento.ToString("dd/MM/yyyy"),
                                DescricaoIP = "novo -> velho",
                                FlagUsuarioResponsavel = false,
                                Perfil = BLL.Perfil.LoadObject(3)
                            };

                            objUsuarioFilialPerfil.SalvarMigracao(trans);
                        }
                        #endregion

                        


                        #region FuncaoPretendida
                        BLL.Funcao objFuncao = null;
                        BLL.FuncaoPretendida objFuncaoPretendida;
                        var existeFuncao = false;

                        if (commandObject.Curriculo.FuncaoPretendida.idFuncao != null && commandObject.Curriculo.FuncaoPretendida.idFuncao > 0)
                        {
                            objFuncao = BLL.Funcao.LoadObject(commandObject.Curriculo.FuncaoPretendida.idFuncao.Value);

                            if (objFuncao != null)
                                existeFuncao = BLL.FuncaoPretendida.CarregarPorCurriculoFuncao(objCurriculo, objFuncao, out objFuncaoPretendida);
                        }

                        objFuncaoPretendida = new BLL.FuncaoPretendida
                        {
                            DataCadastro = commandObject.Curriculo.DataCadastro,
                            DescricaoFuncaoPretendida = commandObject.Curriculo.FuncaoPretendida.Descricao,
                            QuantidadeExperiencia = commandObject.Curriculo.FuncaoPretendida.TempoExperiencia,
                            Curriculo = objCurriculo,
                            Funcao = objFuncao
                        };
                        #endregion

                        objFuncaoPretendida.SalvarMigracao(trans);

                        #endregion

                        #region Cadidatar
                        //Se o cadastro for a partir de uma candidatura
                        if (commandObject.IdVaga > 0)
                        {
                            int idStatusVagaCandidato = 1;
                            bool EnviarCVEmpresaAgora = false;
                            BLL.Vaga vaga = BLL.Vaga.LoadObject(commandObject.IdVaga);

                            //validar status
                            //1- Sem envio, quando a vaga estiver fechada ou as flags de receber CV na vaga forem false.
                            //2- aguardando envio, quando a flag receber todos for true e a flag receber cada for false.
                            //3- Enviado, quando a flag receber cada for true e/ou a flag receber todos for false.
                            if (vaga.FlagReceberCadaCV.HasValue && vaga.FlagReceberCadaCV.Value && vaga.FlagReceberTodosCV.HasValue && vaga.FlagReceberTodosCV.Value)
                            {
                                idStatusVagaCandidato = 2;
                                EnviarCVEmpresaAgora = true;
                            }
                            else if (vaga.FlagReceberCadaCV != null && vaga.FlagReceberCadaCV.Value)
                            {
                                idStatusVagaCandidato = 3;
                            }
                            else if (vaga.FlagReceberTodosCV.HasValue && vaga.FlagReceberTodosCV.Value)
                            {
                                idStatusVagaCandidato = 2;
                            }

                            BLL.VagaCandidato objVagaCandidato;
                            if(!BLL.VagaCandidato.CarregarPorVagaCurriculo(vaga.IdVaga,objCurriculo.IdCurriculo,out objVagaCandidato))
                            {
                                BLL.StatusCurriculoVaga objStatusCurriculoVaga = BLL.StatusCurriculoVaga.LoadObject(idStatusVagaCandidato);

                                objVagaCandidato = new BLL.VagaCandidato
                                {
                                    Curriculo = objCurriculo,
                                    Vaga = vaga,
                                    DataCadastro = objCurriculo.DataCadastro,
                                    FlagInativo = false,
                                    FlagAutoCandidatura = false,
                                    StatusCurriculoVaga = objStatusCurriculoVaga
                                };

                                objVagaCandidato.SalvarMigracao(trans);
                            }

                            //Descontar a candidatura
                            string numeroCandidaturas = parametroCurriculo.ValorParametro;

                            if(!vaga.Filial.PossuiPlanoAtivo() && !vaga.FlagVagaArquivada)
                            {
                                numeroCandidaturas = (Convert.ToInt32(numeroCandidaturas) - 1).ToString();
                            }

                            parametroCurriculo.ValorParametro = numeroCandidaturas;
                            parametroCurriculo.SalvarMigracao(trans);

                            //Enviar Cv para empresa
                            BLL.IntencaoFilial objIntencao;
                            if (!BLL.IntencaoFilial.CarregarPorFilialCurriculo(objCurriculo.IdCurriculo, vaga.Filial.IdFilial, trans, out objIntencao))
                            {
                                objIntencao = new BLL.IntencaoFilial
                                {
                                    Curriculo = objCurriculo,
                                    Filial = vaga.Filial,
                                    FlagInativo = false,
                                    DataCadastro = objCurriculo.DataCadastro
                                };

                                objIntencao.SalvarMigracao(trans);
                            }


                            //Registrar visualização da vaga
                            BLL.VagaVisualizada.SalvarVisualizacaoVaga(vaga, objCurriculo, trans);

                            //Enviar SMS para usuário da empresa
                            vaga.CampanhaRecrutamentoNotificarCandidatura(objCurriculo);

                            #region Enviar Curriculo para empresa
                            if (EnviarCVEmpresaAgora)
                            {

                            }
                            #endregion
                        }
                        #endregion

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

        public Tuple<BLL.Curriculo, BLL.ParametroCurriculo, bool> PodeCandidatarSe(decimal cpf)
        {
            int valorParametro = 1;
            BLL.Curriculo objCurriculo;
            BLL.Curriculo.CarregarPorCpf(cpf, out objCurriculo);

            BLL.ParametroCurriculo objParametroCuriculo;
            BLL.ParametroCurriculo.CarregarParametroPorCurriculo(BLL.Enumeradores.Parametro.QuantidadeCandidaturaDegustacao, objCurriculo, out objParametroCuriculo);

            //se nao for VIP retorna o saldo de candidaturas
            if(!objCurriculo.FlagVIP)
                valorParametro = objParametroCuriculo != null ? Convert.ToInt32(objParametroCuriculo.ValorParametro): 0;

            return new Tuple<BLL.Curriculo, BLL.ParametroCurriculo, bool>(objCurriculo, objParametroCuriculo, valorParametro > 0);
        }

        public bool SalvarCandidatura(decimal cpf, int idVaga)
        {
            //Se a empresa tem plano a candidatura para vaga é livre e não desconta degustação
            var vaga = BLL.Vaga.LoadObject(idVaga);

            var result = PodeCandidatarSe(cpf);

            if(vaga.FlagVagaArquivada)
            {
                return MapCandidatar(result, idVaga,false,true);
            }

            else if (!vaga.Filial.PossuiPlanoAtivo())
            {
                //se tem saldo de degustação
                if (result.Item3)
                {
                    return MapCandidatar(result, idVaga, false,false);
                }
    
                return false;
            }
            else
            {
                return MapCandidatar(result, idVaga, true,false);
            }
        }

        public bool MapCandidatar(Tuple<BLL.Curriculo, BLL.ParametroCurriculo, bool> result, int idVaga, bool vagaLivre, bool oportunidade)
        {
            using (SqlConnection conn = new SqlConnection(BLL.DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        #region Cadidatar
                        int idStatusVagaCandidato = 1;
                        bool EnviarCVEmpresaAgora = false;
                        BLL.Vaga vaga = BLL.Vaga.LoadObject(idVaga);

                        //validar status
                        //1- Sem envio, quando a vaga estiver fechada ou as flags de receber CV na vaga forem false.
                        //2- aguardando envio, quando a flag receber todos for true e a flag receber cada for false.
                        //3- Enviado, quando a flag receber cada for true e/ou a flag receber todos for false.
                        if (vaga.FlagReceberCadaCV.HasValue && vaga.FlagReceberCadaCV.Value && vaga.FlagReceberTodosCV.HasValue && vaga.FlagReceberTodosCV.Value)
                        {
                            idStatusVagaCandidato = 2;
                            EnviarCVEmpresaAgora = true;
                        }
                        else if (vaga.FlagReceberCadaCV != null && vaga.FlagReceberCadaCV.Value)
                        {
                            idStatusVagaCandidato = 3;
                        }
                        else if (vaga.FlagReceberTodosCV.HasValue && vaga.FlagReceberTodosCV.Value)
                        {
                            idStatusVagaCandidato = 2;
                        }

                        BLL.StatusCurriculoVaga objStatusCurriculoVaga = BLL.StatusCurriculoVaga.LoadObject(idStatusVagaCandidato);

                        BLL.VagaCandidato objVagaCandidato = new BLL.VagaCandidato
                        {
                            Curriculo = result.Item1,
                            Vaga = vaga,
                            DataCadastro = DateTime.Now,
                            FlagInativo = false,
                            FlagAutoCandidatura = false,
                            StatusCurriculoVaga = objStatusCurriculoVaga
                        };

                        objVagaCandidato.SalvarMigracao(trans);

                        //Enviar Cv para empresa
                        BLL.IntencaoFilial objIntensao;
                        if (!BLL.IntencaoFilial.CarregarPorFilialCurriculo(result.Item1.IdCurriculo, vaga.Filial.IdFilial, trans, out objIntensao))
                        {
                            objIntensao = new BLL.IntencaoFilial
                            {
                                Curriculo = result.Item1,
                                Filial = vaga.Filial,
                                FlagInativo = false,
                                DataCadastro = DateTime.Now
                            };

                            objIntensao.SalvarMigracao(trans);
                        }

                        #endregion

                        //Enviar SMS para usuário da empresa
                        vaga.CampanhaRecrutamentoNotificarCandidatura(result.Item1);

                        #region Enviar Curriculo para empresa
                        if (EnviarCVEmpresaAgora)
                        {

                        }
                        #endregion

                        //Atualizar Saldo Degustação
                        if (!vagaLivre && !oportunidade)
                        {
                            result.Item2.ValorParametro = (Convert.ToInt32(result.Item2.ValorParametro) - 1).ToString();
                            result.Item2.SalvarMigracao(trans);
                        }

                        trans.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }            
        }

        #region ExistePessoaFisica
        /// <summary>
        /// Verifica no banco velho se existe pessoa fisica
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
            var ativa = BLL.PessoaFisica.CarregarPorCPF(cpf).FlagInativo.Value;
            return ativa;
        }
        #endregion

        #region ExisteCurriculo
        public bool ExisteCurriculo(int idPessoaFisica)
        {
            BLL.Curriculo objCurriculo;
            return BLL.Curriculo.CarregarPorPessoaFisica(idPessoaFisica, out objCurriculo);
        }
        #endregion

        #region CarregarIdSeExistePessoaFisica
        /// <summary>
        /// Verifica no banco velho se existe pessoa fisica
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

        #region CarregarIdSeExistePessoaFisica
        /// <summary>
        /// Verifica no banco velho se existe pessoa fisica
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public string CarregarPessoaFisica(decimal cpf)
        {
            var pessoaFisica = BLL.PessoaFisica.CarregarPorCPF(cpf);

            return pessoaFisica.NomeCompleto;
        }
        #endregion

        #region TemExperienciaProfissional
        public bool TemExperienciaProfissional(decimal cpf, DateTime dataNascimento)
        {
            BLL.ExperienciaProfissional experiencia = BLL.ExperienciaProfissional.CarregarUltimaExperienciaProfissional(BLL.PessoaFisica.CarregarIdPorCPF(cpf));
            return (experiencia != null && experiencia.IdExperienciaProfissional > 0);
        }
        #endregion

        #region TemFormacao
        public bool TemFormacao(decimal cpf, DateTime dataNascimento)
        {
            return BLL.Formacao.ExisteFormacaoInformada(BLL.PessoaFisica.CarregarIdPorCPF(cpf));
        }
        #endregion

        #region CarregarVagaSQL
        public Models.Vaga.Vaga CarregarVagaSQL(int idVaga)
        {

            var vaga = BLL.Vaga.LoadObject(idVaga);
            Models.Vaga.Vaga vagaSQL = null;
            
            if(vaga != null)
            {
                vaga.Cidade.CompleteObject();
                vaga.Funcao.CompleteObject();
                vaga.Funcao.AreaBNE.CompleteObject();

                List<BLL.VagaTipoVinculo> objTipoVinculo = BLL.VagaTipoVinculo.ListarTipoVinculoPorVaga(idVaga, null);
                
                vagaSQL = new Models.Vaga.Vaga();
                
                vagaSQL.IdVaga = vaga.IdVaga;
                vagaSQL.IdFuncao = vaga.Funcao.IdFuncao;
                vagaSQL.Funcao = vaga.Funcao.DescricaoFuncao;
                vagaSQL.IdCidade = vaga.Cidade.IdCidade;
                vagaSQL.SalarioDe = vaga.ValorSalarioDe != null? vaga.ValorSalarioDe.Value : 0;
                vagaSQL.SalarioAte = vaga.ValorSalarioPara != null ? vaga.ValorSalarioPara.Value : 0;
                vagaSQL.Cidade = vaga.Cidade.NomeCidade;
                vagaSQL.UF = vaga.Cidade.Estado.SiglaEstado;
                vagaSQL.CodigoVaga = vaga.CodigoVaga;
                vagaSQL.Atribuicoes = vaga.DescricaoAtribuicoes;
                vagaSQL.Beneficios = vaga.DescricaoBeneficio;
                vagaSQL.Requisitos = vaga.DescricaoRequisito;
                vagaSQL.DataAnuncio = vaga.DataAbertura.Value;
                vagaSQL.FlgAuditada = vaga.FlagAuditada != null? vaga.FlagAuditada.Value : false;
                vagaSQL.NomeEmpresa = vaga.NomeEmpresa;
                vagaSQL.Descricao = vaga.DescricaoFuncao;
                vagaSQL.FlgArquivada = vaga.FlagVagaArquivada;
                vagaSQL.FlgInativo = vaga.FlagInativo;
                vagaSQL.DescricaoAreaBNEPesquisa = vaga.Funcao.AreaBNE.DescricaoAreaBNEPesquisa;
                vagaSQL.FlgDeficiencia = vaga.FlagDeficiencia !=null ? vaga.FlagDeficiencia.Value : false ;
                //descriçaõ de deficiencia no modelo novo descomentar quando subir toda a alteração
                //vagaSQL.DescricaoDeficiencia = vagaSQL.FlgDeficiencia == true ? BLL.VagaDeficiencia.ViewDeficiencias(vaga.IdVaga) : string.Empty;
                vagaSQL.DescricaoDeficiencia = string.Empty;

                if(objTipoVinculo.Count > 0)
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

        #region ECurriculoVIP(int idPessoaFisica)
        public Tuple<bool,int> ECurriculoVIP(decimal cpf)
        {
            bool retorno = false;

            BLL.Curriculo objCurriculo;
            if (BLL.Curriculo.CarregarPorCpf(cpf, out objCurriculo))
            {
                retorno = objCurriculo.FlagVIP;
            }

            return new Tuple<bool,int>(retorno,objCurriculo.IdCurriculo);
        }
        #endregion

        #region GetParametroCurriculo
        public string GetParametroCurriculo(int idCurriculo)
        {
            BLL.Curriculo objCurriculo = BLL.Curriculo.LoadObject(idCurriculo);
            BLL.ParametroCurriculo objParametroCurriculo;
            BLL.ParametroCurriculo.CarregarParametroPorCurriculo(BLL.Enumeradores.Parametro.QuantidadeCandidaturaDegustacao, objCurriculo, out objParametroCurriculo);

            return objParametroCurriculo != null ? objParametroCurriculo.ValorParametro : "0";
        }
        #endregion

        #region GetLinksPaginasSemelhantes
        public string[] GetLinksPaginasSemelhantes(string funcao,string cidade, string siglaEstado,string areaBNE)
        {
            string[] urls = new string[4];

            urls[0] = BLL.Custom.SitemapHelper.MontarUrlVagasPorFuncao(funcao);
            urls[1] = BLL.Custom.SitemapHelper.MontarUrlVagasPorCidade(cidade,siglaEstado);
            urls[2] = BLL.Custom.SitemapHelper.MontarUrlVagasPorFuncaoCidade(funcao,cidade,siglaEstado);
            urls[3] = BLL.Custom.SitemapHelper.MontarUrlVagasPorArea(areaBNE);

            return urls;
        }
        #endregion

        #region ChecarJaEnviei
        public bool ChecarJaEnviei(int idVaga, decimal cpf)
        {
            bool result = false;
            BLL.Curriculo objCurriculo;
            if (BLL.Curriculo.CarregarPorCpf(cpf, out objCurriculo))
            {
                BLL.VagaCandidato objVagaCandidato;
                result = BLL.VagaCandidato.CarregarPorVagaCurriculo(idVaga, objCurriculo.IdCurriculo, out objVagaCandidato);
            }
            return result;
        }
        #endregion

    }
}