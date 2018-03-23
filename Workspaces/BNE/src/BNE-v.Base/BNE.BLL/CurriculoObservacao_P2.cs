//-- Data: 06/03/2013 13:30
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
	public partial class CurriculoObservacao // Tabela: BNE_Curriculo_Observacao
	{

        #region Consultas

        #region SpInativarCurriculoObservacao
        private const string SpInativarCurriculoObservacao = @"UPDATE BNE_Curriculo_Observacao SET Flg_Inativo = 1 WHERE Idf_Curriculo_Observacao = @Idf_Curriculo_Observacao";
        #endregion

        #region SpSelectObservacaoCurriculo
        private const string SpSelectObservacaoCurriculo = @"
        DECLARE @FirstRec INT
        DECLARE @LastRec INT
                                                            
        SET @FirstRec = ( @CurrentPage * @PageSize + 1 )
        SET @LastRec = ( @CurrentPage * @PageSize ) + @PageSize

        CREATE TABLE #temp_results_observacao_curriculo 
        (
	        RowID INT NOT NULL,
            Dta_Cadastro DATETIME NOT NULL,
            Des_Observacao VARCHAR(2000) NOT NULL,
            Nme_Usuario VARCHAR(100) NOT NULL,
            Idf_Curriculo_Observacao INT NOT NULL ,
            Idf_Usuario_Filial_Perfil INT NULL ,
            Flg_Sistema BIT NOT NULL
        )

        INSERT INTO #temp_results_observacao_curriculo
        SELECT  
                ROW_NUMBER() OVER (ORDER BY CO.Dta_Cadastro DESC) AS RowID,  
                CO.Dta_Cadastro ,
                CO.Des_Observacao ,
                ISNULL(PF.Nme_Pessoa, 'Processo do Sistema') AS Nme_Pessoa ,
                CO.Idf_Curriculo_Observacao ,
                CO.Idf_Usuario_Filial_Perfil ,
                CO.Flg_Sistema
        FROM    BNE.BNE_Curriculo_Observacao CO WITH(NOLOCK)
		        LEFT JOIN BNE.TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) ON UFP.Idf_Usuario_Filial_Perfil = CO.Idf_Usuario_Filial_Perfil
		        LEFT JOIN BNE.TAB_Pessoa_Fisica PF WITH(NOLOCK) ON UFP.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
        WHERE   CO.Idf_Curriculo = @Idf_Curriculo
                AND CO.Flg_Inativo = 0

        SELECT COUNT (rowid) FROM #temp_results_observacao_curriculo
        SELECT * FROM #temp_results_observacao_curriculo WHERE RowID >= @FirstRec AND RowID <= @LastRec

        DROP TABLE #temp_results_observacao_curriculo
        ";
        #endregion

        #region Spselectdescricao
        private const string Spselectdescricao = @"
        SELECT  Des_Observacao
        FROM    BNE_Curriculo_Observacao WITH(NOLOCK)
        WHERE   Idf_Curriculo_Observacao = @Idf_Curriculo_Observacao
        ";
        #endregion

        #endregion

        #region Métodos

        #region Inativar
        /// <summary>
        /// Método utilizado para excluir uma instância de CurriculoObservacao no banco de dados.
        /// </summary>
        /// <param name="idCurriculoObservacao">Chave do registro.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Inativar(int idCurriculoObservacao)
        {
            Inativar(idCurriculoObservacao, null);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de CurriculoObservacao no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idCurriculoObservacao">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Inativar(int idCurriculoObservacao, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo_Observacao", SqlDbType = SqlDbType.Int, Size = 4, Value = idCurriculoObservacao }
                };

            if (trans != null)
                DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SpInativarCurriculoObservacao, parms);
            else
                DataAccessLayer.ExecuteNonQuery(CommandType.Text, SpInativarCurriculoObservacao, parms);
        }
        #endregion

        #region ListarObservacoes
        /// <summary>
        /// Recupera um DataTable paginado com as observações ativas de um curriculo
        /// </summary>
        /// <param name="objCurriculo"></param>
        /// <param name="paginaAtual"></param>
        /// <param name="tamanhoPagina"></param>
        /// <param name="totalRegistros"></param>
        /// <returns></returns>
        public static DataTable ListarObservacoes(Curriculo objCurriculo, int paginaAtual, int tamanhoPagina, out int totalRegistros)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@CurrentPage", SqlDbType = SqlDbType.Int, Size = 4, Value = paginaAtual },
                    new SqlParameter { ParameterName = "@PageSize", SqlDbType = SqlDbType.Int, Size = 4, Value = tamanhoPagina },
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objCurriculo.IdCurriculo }
                };

            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpSelectObservacaoCurriculo, parms))
                {
                    if (dr.Read())
                        totalRegistros = Convert.ToInt32(dr[0]);

                    dr.NextResult();
                    dt = new DataTable();
                    dt.Load(dr);
                }
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }

            return dt;
        }
        #endregion

        #region RecuperarDescricao
        /// <summary>
        /// Recupera a descrição da Observação
        /// </summary>
        /// <returns></returns>
        public string RecuperarDescricao()
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo_Observacao", SqlDbType = SqlDbType.Int, Size = 4, Value = _idCurriculoObservacao }
                };

            return Convert.ToString(DataAccessLayer.ExecuteScalar(CommandType.Text, Spselectdescricao, parms));
        }
        #endregion

        #region SalvarCRM
        /// <summary>
        /// Salva as informações no CRM
        /// </summary>
        public static void SalvarCRM(string descricao, Curriculo objCurriculo, string nomeProcessoSistema, SqlTransaction trans)
        {
            SalvarCRM(descricao, objCurriculo, null, nomeProcessoSistema, trans);
        }
        public static void SalvarCRM(string descricao, Curriculo objCurriculo, UsuarioFilialPerfil objUsuarioGerador, SqlTransaction trans)
        {
            SalvarCRM(descricao, objCurriculo, objUsuarioGerador, null, trans);
        }
        private static void SalvarCRM(string descricao, Curriculo objCurriculo, UsuarioFilialPerfil objUsuarioGerador, string nomeProcessoSistema, SqlTransaction trans)
        {
            if (!string.IsNullOrWhiteSpace(nomeProcessoSistema))
                descricao = string.Concat(descricao, " Efetuado pelo processo:", nomeProcessoSistema);

            var objCurriculoObservacao = new CurriculoObservacao
            {
                DescricaoObservacao = descricao,
                Curriculo = objCurriculo,
                FlagSistema = true,
                UsuarioFilialPerfil = objUsuarioGerador
            };
            objCurriculoObservacao.Save(trans);
        }
        #endregion

        #endregion

	}
}