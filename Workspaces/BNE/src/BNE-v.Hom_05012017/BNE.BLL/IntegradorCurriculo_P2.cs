//-- Data: 13/03/2013 18:33
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using BNE.BLL.Integracoes.IntegradorCurriculo;

namespace BNE.BLL
{
    public partial class IntegradorCurriculo // Tabela: TAB_Integrador_Curriculo
    {

        #region Consultas

        #region Spcarregarintegradorativo
        private const string Spcarregarintegradorativo = @"
        SELECT  IC.*
        FROM    TAB_Integrador_Curriculo IC
                INNER JOIN TAB_Filial F ON IC.Idf_Filial = F.Idf_Filial
        WHERE   F.Num_CNPJ = @Num_CNPJ 
                AND IC.Flg_Inativo = 0
        ";
        #endregion

        #endregion

        #region CarregarIntegradorAtivo
        public static bool CarregarIntegradorAtivo(decimal numeroCNPJ, out IntegradorCurriculo objIntegradorCurriculo, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Num_CNPJ", SqlDbType = SqlDbType.Decimal, Size = 14, Value = numeroCNPJ }
                };

            bool retorno;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spcarregarintegradorativo, parms))
            {
                objIntegradorCurriculo = new IntegradorCurriculo();
                retorno = SetInstance(dr, objIntegradorCurriculo);
            }
            return retorno;
        }
        #endregion

        #region RecuperarDadosCurriculo
        /// <summary>
        /// Método responsável por recuperar os dados pessoais de um currículo
        /// </summary>
        /// <param name="numeroCNPJ"></param>
        /// <param name="senhaUsuario"></param>
        /// <param name="nomeCliente"></param>
        /// <param name="idCurriculo"></param>
        /// <returns></returns>
        public static RetornoIntegrador RecuperarDadosCurriculo(string numeroCNPJ, string senhaUsuario, string nomeCliente, int idCurriculo)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        decimal cnpj = Custom.Helper.ConverterCPFCNPJStringDecimal(numeroCNPJ);

                        IntegradorCurriculo objIntegradorCurriculo;
                        if (CarregarIntegradorAtivo(cnpj, out objIntegradorCurriculo, trans)) //Se existe o integrador
                        {
                            string senhaRequest = senhaUsuario.Trim();
                            string senhaIntegrador = objIntegradorCurriculo.SenhaIntegradorCurriculo.Trim();

                            if (senhaIntegrador.Equals(senhaRequest)) //Se a senha passada bate
                            {
                                var objCurriculoDTO = Curriculo.CarregarCurriculoDTO(idCurriculo);

                                var objCurriculoIntegracao = new Integracoes.IntegradorCurriculo.Curriculo
                                    {
                                        CodigoCurriculo = objCurriculoDTO.IdCurriculo,
                                        NumeroCPF = Custom.Helper.FormatarCPF(objCurriculoDTO.NumeroCPF),
                                        NomeCompleto = objCurriculoDTO.NomeCompleto,
                                        Email = objCurriculoDTO.Email,
                                        EnderecoNumeroCEP = objCurriculoDTO.NumeroCEP,
                                        EnderecoLogradouro = TratarTexto(objCurriculoDTO.Logradouro),
                                        EnderecoNumero = TratarTexto(objCurriculoDTO.NumeroEndereco),
                                        EnderecoComplemento = TratarTexto(objCurriculoDTO.Complemento),
                                        DataNascimento = objCurriculoDTO.DataNascimento,
                                        NumeroCelular = Custom.Helper.FormatarTelefone(objCurriculoDTO.NumeroDDDCelular, objCurriculoDTO.NumeroCelular),
                                        NumeroResidencial = Custom.Helper.FormatarTelefone(objCurriculoDTO.NumeroDDDTelefone, objCurriculoDTO.NumeroTelefone),
                                        DataAtualizacaoCurriculo = objCurriculoDTO.DataAtualizacaoCurriculo
                                    };

                                var objRequisicaoIntegradorCurriculo = new RequisicaoIntegradorCurriculo
                                    {
                                        Curriculo = new Curriculo(objCurriculoDTO.IdCurriculo),
                                        DataIntegracao = DateTime.Now,
                                        IntegradorCurriculo = objIntegradorCurriculo,
                                        NomeCliente = nomeCliente
                                    };

                                objRequisicaoIntegradorCurriculo.Save(trans);

                                trans.Commit();

                                return new RetornoIntegrador
                                {
                                    Curriculo = objCurriculoIntegracao
                                };
                            }
                            else
                            {
                                var exception = new Exception("Acesso não autorizado. Senha não confere!");
                                var customMessage = "Tentativa de acesso pelo CNPJ: " + cnpj.ToString(CultureInfo.CurrentCulture);

                                EL.GerenciadorException.GravarExcecao(exception, customMessage);

                                return new RetornoIntegrador
                                {
                                    Erro = exception.Message
                                };
                            }
                        }
                        else
                        {
                            var exception = new Exception("Acesso não autorizado. Integrador inexistente!");
                            var customMessage = "Tentativa de acesso pelo CNPJ: " + cnpj.ToString(CultureInfo.CurrentCulture);

                            EL.GerenciadorException.GravarExcecao(exception, customMessage);

                            return new RetornoIntegrador
                            {
                                Erro = exception.Message
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        EL.GerenciadorException.GravarExcecao(ex);
                        return new RetornoIntegrador
                        {
                            Erro = "Ocorreu um erro em seu acesso!"
                        };
                    }
                }
            }
        }
        #endregion

        #region TratarTexto
        private static string TratarTexto(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return " ";
                
            return texto;
        }
        #endregion

    }
}