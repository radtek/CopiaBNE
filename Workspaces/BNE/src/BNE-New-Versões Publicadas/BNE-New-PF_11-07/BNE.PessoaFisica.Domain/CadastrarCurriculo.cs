using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.PessoaFisica.Domain
{
    class CadastrarCurriculo
    {
        public bool PostMiniCurriculo(Command.PreCurriculo preCurriculo, out bool candidatura)
        {
            candidatura = false;
            try
            {
                var idPessoaFisica = PessoaFisicaJaExiste(preCurriculo.CPF, preCurriculo.DataNascimento);

                if (idPessoaFisica > 0)
                {
                    if (CurriculoExiste(idPessoaFisica))
                    {
                        return Candidatar(preCurriculo, true).Candidatou;
                    }
                    //Todo: Retornar mensagem de CV Bloqueado
                    return false;
                }

                var baseData = DateTime.Now; //Data base para todas as datas de cadastros

                var tipoCurriculo = _tipoCurriculo.GetById(1);
                var situacaoCurriculo = _situacaoCurriculo.GetById(1);

                // TODO: pegar origem conforme o STC ou parceiro BNE
                var origem = _origem.GetById(1); // Origem 1 é o BNE.

                Model.PessoaFisica objPessoaFisica;
                byte dddCelular;
                decimal celular;
                #region PessoaFisica
                try
                {
                    var objSexo = _sexo.GetByChar(preCurriculo.Sexo);
                    objPessoaFisica = new Model.PessoaFisica
                    {
                        Nome = preCurriculo.Nome,
                        CPF = preCurriculo.CPF,
                        DataCadastro = baseData,
                        DataNascimento = preCurriculo.DataNascimento,
                        Sexo = objSexo
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

                    dddCelular = byte.Parse(preCurriculo.Celular.Replace("(", "").Replace(")", "").Replace("-", "").Substring(0, 2));
                    celular = decimal.Parse(preCurriculo.Celular.Replace("(", "").Replace(")", "").Replace("-", "").Substring(3));

                    _telefoneCelular.SalvarTelefone(objPessoaFisica, dddCelular, celular);

                    objPessoaFisica.Cidade = _cidade.GetById(preCurriculo.IdCidade);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Erro ao criar Pessoa física");
                    throw ex;
                }
                #endregion

                #region Curriculo
                Model.Curriculo objCurriculo;
                try
                {
                    objCurriculo = new Model.Curriculo
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

                    _curriculo.SalvarMiniCurriculo(objCurriculo);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Erro ao criar Currículo");
                    throw ex;
                }
                #endregion

                #region Funcao Pretendida
                Model.FuncaoPretendida objFuncaoPretendida;
                try
                {
                    short tempoExperiencia = 0;

                    if (preCurriculo.TempoExperienciaAnos != null && preCurriculo.TempoExperienciaMeses != null)
                        tempoExperiencia = short.Parse(((preCurriculo.TempoExperienciaAnos * 12) + preCurriculo.TempoExperienciaMeses).ToString());

                    objFuncaoPretendida = new Model.FuncaoPretendida
                    {
                        DataCadastro = baseData,
                        IdFuncao = preCurriculo.IdFuncao,
                        Curriculo = objCurriculo,
                        Descricao = preCurriculo.DescricaoFuncao,
                        TempoExperiencia = tempoExperiencia
                    };

                    _funcaoPretendida.Salvar(objFuncaoPretendida);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Erro ao criar Função Pretendida");
                    throw ex;
                }
                #endregion

                #region CurriculoOrigem
                Model.CurriculoOrigem objCurriculoOrigem;
                try
                {
                    objCurriculoOrigem = new Model.CurriculoOrigem
                    {
                        DataCadastro = baseData,
                        Curriculo = objCurriculo,
                        OrigemGlobal = origem
                    };

                    _curriculoOrigem.Salvar(objCurriculoOrigem);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Erro ao criar CurriculoOrigem");
                    throw ex;
                }
                #endregion

                #region CurriculoParametro
                Model.CurriculoParametro objCurriculoParametro;
                try
                {
                    objCurriculoParametro = new Model.CurriculoParametro
                    {
                        Curriculo = objCurriculo,
                        DataCadastro = baseData,
                        Ativo = true,
                        Valor = _curriculo.EstaNaRegiaoDeCampanhaBH(objPessoaFisica.Cidade.Id) ? _parametro.GetById(Model.Enumeradores.Parametro.QuantidadeCandidaturaDegustacaoBH).Valor : ((int)Model.Enumeradores.Parametro.QuantidadeCandidaturaDegustacao).ToString(),
                        Parametro = _parametro.GetById(Model.Enumeradores.Parametro.QuantidadeCandidaturaDegustacao)
                    };

                    _curriculoParametro.Salvar(objCurriculoParametro);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Erro ao criar CurriculoParametro");
                    throw ex;
                }
                #endregion

                //Chamar o mapeamento
                var commandObject = MapearNovoToAntigo(objCurriculo, baseData, objCurriculoOrigem, objFuncaoPretendida, objPessoaFisica, objCurriculoParametro, preCurriculo.Email, preCurriculo.IdVaga, dddCelular, celular);


                new Mapper.ToOld.PessoaFisica().Map(commandObject, out candidatura, preCurriculo.Candidatar);


                _unitOfWork.Commit();

                #region Atualizar PreCurriculo com o Id do Currículo
                try
                {
                    var preCV = PreCurriculo.Get(preCurriculo.Id);
                    preCV.IdCurriculo = objCurriculo.Id;

                    PreCurriculo.SetarCurriculonoPreCurriculo(preCV);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Erro ao criar Atualizar PreCurriculo com o Id do Currículo");
                    throw ex;
                }
                #endregion

                //atualizar número de candidaturas
                try
                {
                    if (preCurriculo.Candidatar)
                        DescontarCandidatura(objCurriculoParametro.Curriculo.Id);
                    //objCurriculoParametro.Valor = "2"; //descontar uma candidatura
                    //_curriculoParametro.Atualizar(objCurriculoParametro);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Erro ao criar atualizar número de candidaturas");
                    throw ex;
                }

                #region Enviar Carta de Confirmação de e-mail.
                try
                {
                    if (preCurriculo.Email != null)
                    {
                        EnviarCartaConfirmacaoDeEmail(objPessoaFisica.Nome, objPessoaFisica.CPF, objPessoaFisica.DataNascimento, objCurriculo.Id, objPessoaFisica.Id, preCurriculo.Email);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Erro ao criar Enviar Carta de Confirmação de e-mail");
                    throw ex;
                }
                #endregion

                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao criar mini CV");
                throw ex;
            }
        }
    }
}
