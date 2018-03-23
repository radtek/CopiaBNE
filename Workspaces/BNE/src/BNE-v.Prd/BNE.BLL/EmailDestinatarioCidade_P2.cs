//-- Data: 05/10/2011 11:31
//-- Autor: Gieyson Stelmak

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
    public partial class EmailDestinatarioCidade // Tabela: BNE_Email_Destinatario_Cidade
    {

        #region Consultas
        private const string SPPESQUISAREMAILPORGRUPO = @"
        DECLARE @ParmDefinition NVARCHAR(1000)
        SET @ParmDefinition = N'@Idf_Grupo_Cidade INT, @CurrentPage INT, @PageSize INT ' ;

        DECLARE @FirstRec INT
        DECLARE @LastRec INT
        DECLARE @iSelect NVARCHAR(MAX)
        DECLARE @iSelectCount NVARCHAR(MAX)
        DECLARE @iSelectPag NVARCHAR(MAX)
        SET @FirstRec = ( @CurrentPage * @PageSize + 1 )
        SET @LastRec = ( @CurrentPage * @PageSize ) + @PageSize
        SET @iSelect = '
            SELECT  ROW_NUMBER() OVER (ORDER BY Nme_Pessoa ASC) AS RowID ,
                    EDC.Idf_Email_Destinatario_Cidade ,
                    EDC.Flg_Responsavel ,
                    EDC.Flg_Inativo ,
                    EDC.Dta_Cadastro ,
                    EDC.Dta_Alteracao ,
                    EDC.Idf_Grupo_Cidade ,
                    ED.Idf_Email_Destinatario ,
                    ED.Nme_Pessoa ,
                    ED.Des_Email ,
                    ED.Num_DDD_Telefone ,
                    ED.Num_Telefone ,
                    ED.Idf_Usuario_Gerador,
					EDC.idf_Filial 
		    FROM	BNE_Email_Destinatario_Cidade EDC WITH ( NOLOCK )
			    	INNER JOIN BNE_Email_Destinatario ED WITH ( NOLOCK ) ON EDC.Idf_Email_Destinatario = ED.Idf_Email_Destinatario
	        WHERE   EDC.Idf_Grupo_Cidade = @Idf_Grupo_Cidade
                    AND EDC.Flg_Inativo = 0 
                    AND ED.Flg_Inativo = 0'
    
        SET @iSelectCount = 'SELECT COUNT(1) FROM ( ' + @iSelect + ' ) AS TblTempCount'
        SET @iSelectPag = 'SELECT * FROM ( ' + @iSelect  + ' ) AS TblTempPag WHERE RowID >= ' + CONVERT(VARCHAR, @FirstRec) + ' AND RowID <= ' + CONVERT(VARCHAR, @LastRec)

        EXEC sp_executesql @iSelectCount, @ParmDefinition, @Idf_Grupo_Cidade = @Idf_Grupo_Cidade, @CurrentPage = @CurrentPage, @PageSize = @PageSize;
        EXEC sp_executesql @iSelectPag, @ParmDefinition, @Idf_Grupo_Cidade = @Idf_Grupo_Cidade, @CurrentPage = @CurrentPage, @PageSize = @PageSize;";

        private const string SPLISTARPORGRUPOCIDADE = @" 
        SELECT  * 
        FROM    BNE_Email_Destinatario_Cidade EDC WITH ( NOLOCK ) 
        WHERE   EDC.Idf_Grupo_Cidade = @Idf_Grupo_Cidade ";

        private const string SQL_RECUPERAR_USUARIO_RESPONSAVEL_CIDADE = @"
        SELECT  EDC.*
        FROM	BNE.BNE_Email_Destinatario ED WITH ( NOLOCK )
		        INNER JOIN BNE.BNE_Email_Destinatario_Cidade EDC WITH ( NOLOCK ) ON EDC.Idf_Email_Destinatario = ED.Idf_Email_Destinatario
		        INNER JOIN BNE.BNE_Grupo_Cidade GC WITH ( NOLOCK ) ON EDC.Idf_Grupo_Cidade = GC.Idf_Grupo_Cidade
		        INNER JOIN BNE.BNE_Lista_Cidade LC WITH ( NOLOCK ) ON GC.Idf_Grupo_Cidade = LC.Idf_Grupo_Cidade 
        WHERE   EDC.Flg_Inativo = 0
		        AND GC.Flg_Inativo = 0
		        AND LC.Flg_Inativo = 0
		        AND Flg_Responsavel = 1
		        AND LC.Idf_Cidade = @Idf_Cidade
        ORDER BY EDC.Dta_Cadastro ASC
        ";
        
        #endregion

        #region PesquisarEmailPorGrupo
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <returns>DataTable</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static DataTable PesquisarEmailPorGrupo(int paginaCorrente, int tamanhoPagina, out int totalRegistros, int idGrupoCidade)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@CurrentPage", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@PageSize", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Grupo_Cidade", SqlDbType.Int, 4));

            parms[0].Value = paginaCorrente;
            parms[1].Value = tamanhoPagina;
            parms[2].Value = idGrupoCidade;

            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPPESQUISAREMAILPORGRUPO, parms))
                {
                    if (dr.Read())
                        totalRegistros = Convert.ToInt32(dr[0]);

                    dr.NextResult();
                    dt = new DataTable();
                    dt.Load(dr);

                    if (!dr.IsClosed)
                        dr.Close();
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

        #region Inativar
        /// <summary>
        /// Método utilizado para inativar uma instância de email destinatario cidade no banco de dados.
        /// </summary>
        /// <remarks>Jhonatan Taborda</remarks>
        public void Inativar(SqlTransaction trans)
        {
            this._emailDestinatario.CompleteObject(trans);
			this._emailDestinatario.FlagInativo = true;
            this._emailDestinatario.Save(trans);

			this._modified = true;
            this._flagInativo = true;
            this.Save(trans);
        }
        #endregion

        #region ListarPorGrupoCidade
        public static List<EmailDestinatarioCidade> ListarPorGrupoCidade(GrupoCidade objGrupoCidade, SqlTransaction trans)
        {
            List<EmailDestinatarioCidade> lista = new List<EmailDestinatarioCidade>();

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Grupo_Cidade", SqlDbType.Int, 4));
            parms[0].Value = objGrupoCidade.IdGrupoCidade;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPLISTARPORGRUPOCIDADE, parms))
            {
                EmailDestinatarioCidade objEmailDestinatarioCidade;

                while (dr.Read())
                {
                    objEmailDestinatarioCidade = new EmailDestinatarioCidade();
                    if (SetInstance_NonDispose(dr, objEmailDestinatarioCidade))
                        lista.Add(objEmailDestinatarioCidade);
                }

                if (!dr.IsClosed)
                    dr.Close();
            }

            return lista;
        }
        #endregion

        #region SetInstance_NonDispose
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objEmailDestinatarioCidade">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance_NonDispose(IDataReader dr, EmailDestinatarioCidade objEmailDestinatarioCidade)
        {
            try
            {
                objEmailDestinatarioCidade._idEmailDestinatarioCidade = Convert.ToInt32(dr["Idf_Email_Destinatario_Cidade"]);
                objEmailDestinatarioCidade._grupoCidade = new GrupoCidade(Convert.ToInt32(dr["Idf_Grupo_Cidade"]));
                objEmailDestinatarioCidade._emailDestinatario = new EmailDestinatario(Convert.ToInt32(dr["Idf_Email_Destinatario"]));
                objEmailDestinatarioCidade._flagResponsavel = Convert.ToBoolean(dr["Flg_Responsavel"]);
                objEmailDestinatarioCidade._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                objEmailDestinatarioCidade._dataAlteracao = Convert.ToDateTime(dr["Dta_Alteracao"]);
                objEmailDestinatarioCidade._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                objEmailDestinatarioCidade._usuarioGerador = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Gerador"]));

                objEmailDestinatarioCidade._persisted = true;
                objEmailDestinatarioCidade._modified = false;

                return true;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region SalvarEmail
        /// <summary>
        /// Altera os dados de um determinado holerite verba já existente no holerite.
        /// </summary>
        public static void SalvarEmail(DataRow drEmail, SqlTransaction trans, EmailDestinatario objEmailDestinatario, GrupoCidade objGrupoCidade)
        {
            EmailDestinatarioCidade objEmailDestinatarioCidade = new EmailDestinatarioCidade();
           
            int idEmail;
            if (Int32.TryParse(drEmail["Idf_Email_Destinatario_Cidade"].ToString(), out idEmail))
            {
                if (idEmail > 0)
                {
                    objEmailDestinatarioCidade = new EmailDestinatarioCidade(idEmail);
                    objEmailDestinatarioCidade.CompleteObject(trans);
                    EmailDestinatarioCidade.SetInstance_DataRow(drEmail, objEmailDestinatarioCidade, false);
                }
                else
                    EmailDestinatarioCidade.SetInstance_DataRow(drEmail, objEmailDestinatarioCidade, true);
            }
            else
                EmailDestinatarioCidade.SetInstance_DataRow(drEmail, objEmailDestinatarioCidade, true);

            objEmailDestinatarioCidade.GrupoCidade = objGrupoCidade;
            objEmailDestinatarioCidade.EmailDestinatario = objEmailDestinatario;
            objEmailDestinatarioCidade.Save(trans);
        }
        #endregion

        #region SetInstance_DataRow
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um DataRow e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objEmailDestinatarioCidade">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance_DataRow(DataRow dr, EmailDestinatarioCidade objEmailDestinatarioCidade, bool newObject)
        {
            try
            {
                if (dr["Idf_Email_Destinatario_Cidade"] != DBNull.Value)
                    objEmailDestinatarioCidade._idEmailDestinatarioCidade = Convert.ToInt32(dr["Idf_Email_Destinatario_Cidade"]);

                if (dr["Idf_Grupo_Cidade"] != DBNull.Value)
                    objEmailDestinatarioCidade._grupoCidade = new GrupoCidade(Convert.ToInt32(dr["Idf_Grupo_Cidade"]));
                if (dr["Idf_Email_Destinatario"] != DBNull.Value)
                    objEmailDestinatarioCidade._emailDestinatario = new EmailDestinatario(Convert.ToInt32(dr["Idf_Email_Destinatario"]));
                objEmailDestinatarioCidade._flagResponsavel = Convert.ToBoolean(dr["Flg_Responsavel"]);
                objEmailDestinatarioCidade._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                objEmailDestinatarioCidade._dataAlteracao = Convert.ToDateTime(dr["Dta_Alteracao"]);
                objEmailDestinatarioCidade._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                objEmailDestinatarioCidade._usuarioGerador = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Gerador"]));

                if (dr["idf_Filial"] != DBNull.Value)
    				objEmailDestinatarioCidade._idfFilial = new Filial(Convert.ToInt32(dr["idf_Filial"]));

                objEmailDestinatarioCidade._persisted = !newObject;
                objEmailDestinatarioCidade._modified = true;

                return true;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region CarregarResponsavelCidade
        /// <summary>
        /// 
        /// </summary>
        public static bool CarregarResponsavelCidade(Cidade objCidade, out EmailDestinatarioCidade objEmailDestinatarioCidade)
        {
            return CarregarResponsavelCidade(objCidade, out objEmailDestinatarioCidade, null);
        }
        public static bool CarregarResponsavelCidade(Cidade objCidade, out EmailDestinatarioCidade objEmailDestinatarioCidade, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 4));
            parms[0].Value = objCidade.IdCidade;

            IDataReader dr;
            if (trans != null)
                dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, SQL_RECUPERAR_USUARIO_RESPONSAVEL_CIDADE, parms);
            else
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, SQL_RECUPERAR_USUARIO_RESPONSAVEL_CIDADE, parms);

            bool retorno = false;
            objEmailDestinatarioCidade = new EmailDestinatarioCidade();
            
            if (SetInstance(dr, objEmailDestinatarioCidade))
                retorno = true;
            else
                objEmailDestinatarioCidade = null;

            if (!dr.IsClosed)
                dr.Close();

            dr.Dispose();

            return retorno;
        }
        #endregion

    }
}