//-- Data: 19/06/2013 15:27
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class SimulacaoR1 // Tabela: BNE_Simulacao_R1
    {
        public static SimulacaoR1 Simular(Funcao objFuncao, Cidade objCidade, decimal salarioInicial, decimal salarioFinal, short numeroVagas, UsuarioFilialPerfil objUsuarioFilialPerfil, string nome, string numeroDDD, string numeroFone, string email)
        {
            SimulacaoR1 objSimulacao;

            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {

                        #region Media Salarial
                        //Média salarial para a função e cidade da solicitação
                        decimal mediaSalarialSalarioBR = Decimal.Zero;
                        try
                        {
                            var pesquisa = Custom.SalarioBr.PesquisaSalarial.EfetuarPesquisa(objFuncao, objCidade.Estado);

                            if (pesquisa != null && (pesquisa.ValorTrainee != Decimal.Zero && pesquisa.ValorMaster != Decimal.Zero))
                                mediaSalarialSalarioBR = ((pesquisa.ValorTrainee + pesquisa.ValorMaster)/2);
                            else
                                mediaSalarialSalarioBR = 20000;
                        }
                        catch (Exception ex)
                        {
                            mediaSalarialSalarioBR = 20000;
                            EL.GerenciadorException.GravarExcecao(ex);
                        }
                        #endregion

                        #region Números de dias para atender a solicitação
                        var parms = new List<SqlParameter>
                        {
                            new SqlParameter { ParameterName = "@Idf_Funcao", SqlDbType = SqlDbType.Int, Size = 4, Value = objFuncao.IdFuncao },
                            new SqlParameter { ParameterName = "@Idf_Cidade", SqlDbType = SqlDbType.Int, Size = 4, Value = objCidade.IdCidade },
                            new SqlParameter { ParameterName = "@Vlr_Salario_Inicial", SqlDbType = SqlDbType.Decimal, Size = 9, Value = salarioInicial },
                            new SqlParameter { ParameterName = "@Vlr_Salario_Final", SqlDbType = SqlDbType.Decimal, Size = 4, Value = salarioFinal },
                            new SqlParameter { ParameterName = "@Num_Vagas", SqlDbType = SqlDbType.Int, Size = 4, Value = numeroVagas } ,
                            new SqlParameter { ParameterName = "@Vlr_Media_Salarial", SqlDbType = SqlDbType.Decimal, Size = 4, Value = mediaSalarialSalarioBR }
                        };

                        int numeroDias = Convert.ToInt32(DataAccessLayer.ExecuteScalar(trans, CommandType.StoredProcedure, "SP_Calcular_Dias_Atendimento_R1", parms));
                        #endregion

                        #region Parametros de Cálculo
                        var parametros = new List<Enumeradores.Parametro>
                        {
                            Enumeradores.Parametro.R1TaxaReaisAberturaProcesso,
                            Enumeradores.Parametro.R1TaxaPercentualAberturaProcesso,
                            Enumeradores.Parametro.R1BonusPercentualSolicitacaoOnline,
                            Enumeradores.Parametro.R1MargemPercentualTotalServico
                        };

                        Dictionary<Enumeradores.Parametro, string> valoresParametros = Parametro.ListarParametros(parametros);

                        decimal valorReaisAberturaProcesso = Convert.ToDecimal(valoresParametros[Enumeradores.Parametro.R1TaxaReaisAberturaProcesso]);
                        decimal valorPercentualAberturaProcesso = Convert.ToDecimal(valoresParametros[Enumeradores.Parametro.R1TaxaPercentualAberturaProcesso]);
                        decimal valorBonusPercentualSolicitacaoOnline = Convert.ToDecimal(valoresParametros[Enumeradores.Parametro.R1BonusPercentualSolicitacaoOnline]);
                        decimal valorMargemPercentualTotalServico = Convert.ToDecimal(valoresParametros[Enumeradores.Parametro.R1MargemPercentualTotalServico]);
                        #endregion

                        #region Cálculo
                        decimal valorTotalServico = salarioFinal * (valorMargemPercentualTotalServico / 100) * numeroVagas; //Percentual final sobre o valor do maior salário
                        decimal valorBonus = salarioFinal * (valorBonusPercentualSolicitacaoOnline / 100) * numeroVagas; //Percentual de bonificação
                        decimal valorTaxaAbertura = valorReaisAberturaProcesso > decimal.Zero ? valorReaisAberturaProcesso * numeroVagas : salarioFinal * (valorPercentualAberturaProcesso / 100) * numeroVagas; //Se a taxa de abertura for maior que zero então aplica o valor percentual
                        decimal valorServicoPrestado = valorTotalServico - valorTaxaAbertura + valorBonus;
                        #endregion

                        #region Objeto simulacao
                        objSimulacao = new SimulacaoR1
                            {
                                Cidade = objCidade,
                                Funcao = objFuncao,
                                ValorSalarioMin = salarioInicial,
                                ValorSalarioMax = salarioFinal,
                                NumeroVagas = numeroVagas,
                                UsuarioFilialPerfil = objUsuarioFilialPerfil,
                                NomePessoa = nome,
                                NumeroDDDTelefone = numeroDDD,
                                NumeroTelefone = numeroFone,
                                EmailPessoa = email,
                                ValorTaxaAbertura = valorTaxaAbertura,
                                ValorServicoPrestado = valorServicoPrestado,
                                ValorBonusSolicitacaoOnline = valorBonus,
                                ValorTotal = valorTotalServico,
                                ValorMargemPercentualServico = valorMargemPercentualTotalServico,
                                QuantidadeDiasAtendimento = numeroDias,
                                ConsultorR1 = ConsultorR1.RecuperarConsultorR1(objCidade, trans)
                            };
                        objSimulacao.Save(trans);
                        #endregion

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return objSimulacao;
        }
        
    }

}