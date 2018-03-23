using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL.Integracoes.SalarioBR
{
    public class DadosRSM
    {

        #region Consultas

        #region spselectdadoscurriculo
        private const string Spselectdadoscurriculo = @"
        SELECT  C.Vlr_Pretensao_Salarial ,
                FP.Idf_Funcao ,
                Cid.Sig_Estado ,
                FP.Qtd_Experiencia AS FuncaoPretendidaQtdExp ,
                EP.Qtd_Experiencia AS ExpProfissionalQtdExp
        FROM    BNE_Curriculo C WITH(NOLOCK)
                CROSS APPLY ( SELECT TOP 1 IDF_FUNCAO, Qtd_Experiencia FROM BNE_Funcao_Pretendida FP WITH ( NOLOCK ) WHERE C.Idf_Curriculo = FP.Idf_Curriculo ORDER BY FP.Idf_Funcao_Pretendida ) AS FP
                OUTER APPLY ( SELECT SUM(DATEDIFF(DAY, EP.Dta_Admissao, ISNULL(EP.Dta_Demissao, GETDATE()))/30) Qtd_Experiencia FROM BNE_Experiencia_Profissional EP WITH ( NOLOCK ) WHERE EP.Idf_Pessoa_Fisica = C.Idf_Pessoa_Fisica AND EP.Idf_Funcao = FP.Idf_Funcao ) AS EP
                INNER JOIN plataforma.TAB_Cidade Cid WITH(NOLOCK) ON C.Idf_Cidade_Endereco = Cid.Idf_Cidade
        WHERE   C.Idf_Curriculo = @Idf_Curriculo
        ";
        #endregion

        #endregion

        public int IdentificadorFuncaoPretendida { get; set; }
        public string SiglaEstado { get; set; }
        public int IdentificadorNivel
        {
            get
            {
                var qtdExp = 0;
                if (QuantidadeExperienciaFuncaoPretendida.HasValue)
                    qtdExp = QuantidadeExperienciaFuncaoPretendida.Value;
                else if (QuantidadeExperienciaProfissional.HasValue)
                    qtdExp = QuantidadeExperienciaProfissional.Value;

                if (qtdExp < 24)
                    return 1;

                if (qtdExp >= 24 && qtdExp < 48)
                    return 2;

                if (qtdExp >= 48 && qtdExp < 72)
                    return 3;

                if (qtdExp >= 72 && qtdExp < 96)
                    return 4;

                if (qtdExp >= 96)
                    return 5;

                return 0;
            }
        }
        public decimal ValorPretensaoSalarial { get; set; }
        private int? QuantidadeExperienciaFuncaoPretendida { get; set; }
        private int? QuantidadeExperienciaProfissional { get; set; }

        #region RecuperarRSM
        public static DadosRSM RecuperarRSM(Curriculo objCurriculo)
        {
            var objDadosRSM = new DadosRSM();
            using (IDataReader dr = RetornarDataReader(objCurriculo))
            {
                if (!SetInstance(dr, objDadosRSM))
                    objDadosRSM = null;
            }
            return objDadosRSM;
        }
        #endregion

        #region RetornarDataReader
        private static IDataReader RetornarDataReader(Curriculo objCurriculo)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objCurriculo.IdCurriculo}
            };

            return DataAccessLayer.ExecuteReader(CommandType.Text, Spselectdadoscurriculo, parms);
        }
        #endregion

        #region SetInstance
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objDadosRSM">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance(IDataReader dr, DadosRSM objDadosRSM)
        {
            try
            {
                if (dr.Read())
                {
                    objDadosRSM.IdentificadorFuncaoPretendida = Convert.ToInt32(dr["Idf_Funcao"]);
                    objDadosRSM.ValorPretensaoSalarial = Convert.ToDecimal(dr["Vlr_Pretensao_Salarial"]);
                    objDadosRSM.SiglaEstado = dr["Sig_Estado"].ToString();

                    if (dr["FuncaoPretendidaQtdExp"] != DBNull.Value)
                        objDadosRSM.QuantidadeExperienciaFuncaoPretendida = Convert.ToInt32(dr["FuncaoPretendidaQtdExp"]);

                    if (dr["ExpProfissionalQtdExp"] != DBNull.Value)
                        objDadosRSM.QuantidadeExperienciaProfissional = Convert.ToInt32(dr["ExpProfissionalQtdExp"]);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            finally
            {
                dr.Dispose();
            }
        }

        #endregion

    }
}
