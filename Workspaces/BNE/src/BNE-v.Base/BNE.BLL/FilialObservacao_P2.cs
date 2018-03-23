//-- Data: 19/07/2010 15:07
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class FilialObservacao // Tabela: TAB_Filial_Observacao
    {

        #region Consultas

        #region SpInativarFilialObservacao
        private const string SpInativarFilialObservacao = @"UPDATE TAB_Filial_Observacao SET Flg_Inativo = 1 WHERE Idf_Filial_Observacao = @Idf_Filial_Observacao";
        #endregion

        #region SpSelectObservacaoFilial
        private const string SpSelectObservacaoFilial = @"
        DECLARE @FirstRec INT
        DECLARE @LastRec INT
                                                            
        SET @FirstRec = ( @CurrentPage * @PageSize + 1 )
        SET @LastRec = ( @CurrentPage * @PageSize ) + @PageSize

        CREATE TABLE #temp_results_observacao_filial 
        (
	        RowID INT NOT NULL,
            Dta_Cadastro DATETIME NOT NULL,
            Des_Observacao VARCHAR(max) NOT NULL,
            Nme_Usuario VARCHAR(100) NOT NULL,
            Idf_Filial_Observacao INT NOT NULL ,
            Idf_Usuario_Filial_Perfil INT NULL ,
            Flg_Sistema BIT NOT NULL,
            Dta_Proxima_Acao DATETIME NULL,
            Des_Proxima_Acao VARCHAR(100) NULL
        )

        INSERT INTO #temp_results_observacao_filial
        SELECT  
                ROW_NUMBER() OVER (ORDER BY FO.Dta_Cadastro DESC) AS RowID,  
                FO.Dta_Cadastro ,
                FO.Des_Observacao ,
                ISNULL(PF.Nme_Pessoa, 'Processo do Sistema') AS Nme_Pessoa ,
                FO.Idf_Filial_Observacao ,
                FO.Idf_Usuario_Filial_Perfil ,
                FO.Flg_Sistema,
                FO.Dta_Proxima_Acao,
                FO.Des_Proxima_Acao
        FROM    BNE.TAB_Filial_Observacao FO WITH(NOLOCK)
		        LEFT JOIN BNE.TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) ON UFP.Idf_Usuario_Filial_Perfil = FO.Idf_Usuario_Filial_Perfil
		        LEFT JOIN BNE.TAB_Pessoa_Fisica PF WITH(NOLOCK) ON UFP.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
        WHERE   FO.Idf_Filial = @Idf_Filial
                AND FO.Flg_Inativo = 0

        SELECT COUNT (rowid) FROM #temp_results_observacao_filial
        SELECT * FROM #temp_results_observacao_filial WHERE RowID >= @FirstRec AND RowID <= @LastRec

        DROP TABLE #temp_results_observacao_filial
        ";
        #endregion

        #region Spselectdescricao
        private const string Spselectdescricao = @"
        SELECT  Des_Observacao
        FROM    TAB_Filial_Observacao WITH(NOLOCK)
        WHERE   Idf_Filial_Observacao = @Idf_Filial_Observacao
        ";
        #endregion

        #endregion

        #region Métodos

        #region Inativar
        /// <summary>
        /// Método utilizado para excluir uma instância de FilialObservacao no banco de dados.
        /// </summary>
        /// <param name="idFilialObservacao">Chave do registro.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Inativar(int idFilialObservacao)
        {
            Inativar(idFilialObservacao, null);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de FilialObservacao no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idFilialObservacao">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Inativar(int idFilialObservacao, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Filial_Observacao", SqlDbType = SqlDbType.Int, Size = 4, Value = idFilialObservacao }
                };

            if (trans != null)
                DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SpInativarFilialObservacao, parms);
            else
                DataAccessLayer.ExecuteNonQuery(CommandType.Text, SpInativarFilialObservacao, parms);
        }
        #endregion

        #region ListarObservacoes
        /// <summary>
        /// Recupera um DataTable paginado com as observações ativas de uma filial
        /// </summary>
        /// <param name="objFilial"></param>
        /// <param name="paginaAtual"></param>
        /// <param name="tamanhoPagina"></param>
        /// <param name="totalRegistros"></param>
        /// <returns></returns>
        public static DataTable ListarObservacoes(Filial objFilial, int paginaAtual, int tamanhoPagina, out int totalRegistros)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@CurrentPage", SqlDbType = SqlDbType.Int, Size = 4, Value = paginaAtual },
                    new SqlParameter { ParameterName = "@PageSize", SqlDbType = SqlDbType.Int, Size = 4, Value = tamanhoPagina },
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = objFilial.IdFilial }
                };

            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpSelectObservacaoFilial, parms))
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
                    new SqlParameter { ParameterName = "@Idf_Filial_Observacao", SqlDbType = SqlDbType.Int, Size = 4, Value = _idFilialObservacao }
                };

            return Convert.ToString(DataAccessLayer.ExecuteScalar(CommandType.Text, Spselectdescricao, parms));
        }
        #endregion

        #region SalvarCRM
        /// <summary>
        /// Salva as informações no CRM
        /// </summary>
        public static void SalvarCRM(string descricao, Filial objFilial, string nomeProcessoSistema, SqlTransaction trans = null)
        {
            SalvarCRM(descricao, objFilial, null, nomeProcessoSistema, trans);
        }
        public static void SalvarCRM(string descricao, Filial objFilial, UsuarioFilialPerfil objUsuarioGerador, SqlTransaction trans = null)
        {
            SalvarCRM(descricao, objFilial, objUsuarioGerador, null, trans);
        }
        private static void SalvarCRM(string descricao, Filial objFilial, UsuarioFilialPerfil objUsuarioGerador, string nomeProcessoSistema, SqlTransaction trans = null)
        {
            if (!string.IsNullOrWhiteSpace(nomeProcessoSistema))
                descricao = string.Concat(descricao, " Efetuado pelo processo:", nomeProcessoSistema);

            var objFilialObservacao = new FilialObservacao
            {
                DescricaoObservacao = descricao,
                Filial = objFilial,
                FlagSistema = true,
                UsuarioFilialPerfil = objUsuarioGerador
            };

            if (trans != null)
                objFilialObservacao.Save(trans);
            else
                objFilialObservacao.Save();
        }
        #endregion

        #endregion

    }
}