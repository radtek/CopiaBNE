//-- Data: 05/10/2011 11:31
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
    public partial class GrupoCidade // Tabela: BNE_Grupo_Cidade
    {

        #region Consultas
        private const string SPPESQUISARGRUPO = @"
        DECLARE @ParmDefinition NVARCHAR(1000)
        SET @ParmDefinition = N'@Pesquisa VARCHAR(100), @Idf_Grupo_Cidade INT, @CurrentPage INT, @PageSize INT ';

        DECLARE @FirstRec INT
        DECLARE @LastRec INT
        DECLARE @iSelect NVARCHAR(MAX)
        DECLARE @iSelectCount NVARCHAR(MAX)
        DECLARE @iSelectPag NVARCHAR(MAX)
        SET @FirstRec = ( @CurrentPage * @PageSize + 1 )
        SET @LastRec = ( @CurrentPage * @PageSize ) + @PageSize
        SET @iSelect = '
            SELECT 
                ROW_NUMBER() OVER (ORDER BY Email ASC) AS RowID ,
                Idf_Grupo_Cidade ,
                SUBSTRING(Email, 0, LEN(Email)) AS Email ,
                SUBSTRING(Nme_Cidade, 0, LEN(Nme_Cidade)) AS Nme_Cidade ,
                Nme_Pessoa ,
                Nme_Responsavel
				Nme_Filial
            FROM	( 
				        SELECT    
						        (	SELECT	ED.Des_Email + '', ''
							        FROM	BNE.BNE_Email_Destinatario_Cidade EDC WITH ( NOLOCK )
									        INNER JOIN BNE.BNE_Email_Destinatario ED WITH ( NOLOCK ) ON EDC.Idf_Email_Destinatario = ED.Idf_Email_Destinatario
							        WHERE   EDC.Flg_Inativo = 0
							        AND		ED.Flg_Inativo = 0
									        AND EDC.Idf_Grupo_Cidade = GC.Idf_Grupo_Cidade
							        FOR XML PATH('''')
						        ) AS Email ,
						        (	SELECT  ED.Nme_Pessoa + '', ''
							        FROM    BNE.BNE_Email_Destinatario_Cidade EDC WITH ( NOLOCK )
									        INNER JOIN BNE.BNE_Email_Destinatario ED WITH ( NOLOCK ) ON EDC.Idf_Email_Destinatario = ED.Idf_Email_Destinatario
							        WHERE   EDC.Flg_Inativo = 0
									        AND ED.Flg_Inativo = 0
									        AND EDC.Idf_Grupo_Cidade = GC.Idf_Grupo_Cidade
							        FOR XML PATH('''')
						        ) AS Nme_Pessoa ,
						        (	SELECT  ED.Nme_Pessoa + '', ''
							        FROM    BNE.BNE_Email_Destinatario_Cidade EDC WITH ( NOLOCK )
									        INNER JOIN BNE.BNE_Email_Destinatario ED WITH ( NOLOCK ) ON EDC.Idf_Email_Destinatario = ED.Idf_Email_Destinatario
							        WHERE   EDC.Flg_Inativo = 0
									        AND ED.Flg_Inativo = 0
									        AND EDC.Idf_Grupo_Cidade = GC.Idf_Grupo_Cidade
									        AND EDC.Flg_Responsavel = 1
							        FOR XML PATH('''')
						        ) AS Nme_Responsavel ,
						        (   SELECT  C.Nme_Cidade + ''/'' + C.Sig_Estado + '', ''
							        FROM    BNE.BNE_Lista_Cidade LC WITH ( NOLOCK )
                                            INNER JOIN plataforma.TAB_Cidade C WITH ( NOLOCK ) ON LC.Idf_Cidade = C.Idf_Cidade
							        WHERE   LC.Idf_Grupo_Cidade = GC.Idf_Grupo_Cidade
							        FOR XML PATH('''')
						        ) AS Nme_Cidade ,
						        GC.Idf_Grupo_Cidade,
								Filial.Nme_Fantasia AS Nme_Filial
						        --F.Raz_Social ,
				        FROM	BNE.BNE_Grupo_Cidade GC WITH ( NOLOCK )
                                --LEFT JOIN BNE.TAB_Filial F WITH ( NOLOCK ) ON GC.Idf_Filial = F.Idf_Filial
								OUTER apply ( SELECT GF.Nme_Fantasia FROM BNE.BNE_Email_Destinatario_Cidade EDC WITH(NOLOCK) INNER JOIN BNE.BNE_Gerente_Filial GF WITH(NOLOCK) ON GF.idf_Filial = EDC.idf_Filial WHERE EDC.Idf_Grupo_Cidade = GC.Idf_Grupo_Cidade AND EDC.Flg_Responsavel = 1 AND EDC.Flg_Inativo = 0 ) AS Filial
                        WHERE   GC.Flg_Inativo = 0 '

        IF ( @Idf_Grupo_Cidade IS NOT NULL ) 
            BEGIN
                SET @iSelect = @iSelect + ' AND Idf_Grupo_Cidade = @Idf_Grupo_Cidade '
            END                      
						
        SET @iSelect = @iSelect + ' ) AS temp '

        IF ( @Pesquisa IS NOT NULL ) 
            BEGIN
				SET @iSelect = @iSelect + ' WHERE (Email LIKE ''%'' + @Pesquisa + ''%'' OR Nme_Pessoa LIKE ''%'' + @Pesquisa + ''%'' OR Nme_Cidade LIKE ''%'' + @Pesquisa + ''%'') '
            END                    
	       
        SET @iSelectCount = 'SELECT COUNT(1) FROM ( ' + @iSelect
            + ' ) AS TblTempCount'
        SET @iSelectPag = 'SELECT * FROM ( ' + @iSelect
            + ' ) AS TblTempPag WHERE RowID >= ' + CONVERT(VARCHAR, @FirstRec)
            + ' AND RowID <= ' + CONVERT(VARCHAR, @LastRec)

        EXEC sp_executesql @iSelectCount, @ParmDefinition,
            @Pesquisa = @Pesquisa, @Idf_Grupo_Cidade = @Idf_Grupo_Cidade,
            @CurrentPage = @CurrentPage, @PageSize = @PageSize;
        EXEC sp_executesql @iSelectPag, @ParmDefinition, @Pesquisa = @Pesquisa,
            @Idf_Grupo_Cidade = @Idf_Grupo_Cidade, @CurrentPage = @CurrentPage,
            @PageSize = @PageSize;";

        private const string SPLISTARCIDADE = @"
        SELECT  C.*
        FROM    BNE_Grupo_Cidade GC WITH ( NOLOCK )
                INNER JOIN BNE_Lista_Cidade LC WITH ( NOLOCK ) ON GC.Idf_Grupo_Cidade = LC.Idf_Grupo_Cidade
                INNER JOIN plataforma.TAB_Cidade C WITH ( NOLOCK ) ON LC.Idf_Cidade = C.Idf_Cidade
        WHERE   GC.Idf_Grupo_Cidade = @Idf_Grupo_Cidade";

        private const string SPLISTARGRUPOSPORCIDADE = @"
        SELECT  GC.*
        FROM    BNE_Grupo_Cidade GC WITH ( NOLOCK )
                INNER JOIN BNE_Lista_Cidade LC WITH ( NOLOCK ) ON GC.Idf_Grupo_Cidade = LC.Idf_Grupo_Cidade
        WHERE   LC.Idf_Cidade = @Idf_Cidade
		AND GC.Flg_Inativo = 0";

        #endregion

        #region PesquisarGrupoCidade
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <returns>DataTable</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static DataTable PesquisarGrupoCidade(int paginaCorrente, int tamanhoPagina, out int totalRegistros, int? idGrupoCidade, string pesquisa)
        {
            var parms = new List<SqlParameter>
                            {
                                new SqlParameter("@CurrentPage", SqlDbType.Int, 4),
                                new SqlParameter("@PageSize", SqlDbType.Int, 4)
                            };

            parms[0].Value = paginaCorrente;
            parms[1].Value = tamanhoPagina;

            var parm = new SqlParameter("@Idf_Grupo_Cidade", SqlDbType.Int, 4);
            if (idGrupoCidade.HasValue)
                parm.Value = idGrupoCidade.Value;
            else
                parm.Value = DBNull.Value;
            parms.Add(parm);

            parm = new SqlParameter("@Pesquisa", SqlDbType.VarChar, 100);
            if (String.IsNullOrEmpty(pesquisa))
                parm.Value = DBNull.Value;
            else
                parm.Value = pesquisa;
            parms.Add(parm);

            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPPESQUISARGRUPO, parms))
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
        /// <remarks>Gieyson Stelmak</remarks>
        public void Inativar()
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);

            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        List<EmailDestinatarioCidade> lista = EmailDestinatarioCidade.ListarPorGrupoCidade(this, trans);

                        foreach (EmailDestinatarioCidade objEmailDestinatarioCidade in lista)
                        {
                            objEmailDestinatarioCidade.Inativar(trans);
                        }

                        this._flagInativo = true;
                        this._modified = true;
                        this.Save(trans);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        #endregion

        #region ListarCidades
        public List<Cidade> ListarCidades()
        {
            var lista = new List<Cidade>();

            var parms = new List<SqlParameter> { new SqlParameter("@Idf_Grupo_Cidade", SqlDbType.Int, 4) };
            parms[0].Value = this._idGrupoCidade;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPLISTARCIDADE, parms))
            {
                while (dr.Read())
                {
                    var objCidade = new Cidade();
                    if (Cidade.SetInstance_NotDispose(dr, objCidade))
                        lista.Add(objCidade);
                }

                if (!dr.IsClosed)
                    dr.Close();
            }

            return lista;
        }
        #endregion

        #region ListarGruposPorCidade
        public static IEnumerable<GrupoCidade> ListarGruposPorCidade(Cidade objCidade)
        {
            var lista = new List<GrupoCidade>();

            var parms = new List<SqlParameter> { new SqlParameter("@Idf_Cidade", SqlDbType.Int, 4) };
            parms[0].Value = objCidade.IdCidade;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPLISTARGRUPOSPORCIDADE, parms))
            {
                while (dr.Read())
                {
                    var objGrupoCidade = new GrupoCidade();
                    if (GrupoCidade.SetInstance_NonDispose(dr, objGrupoCidade))
                        lista.Add(objGrupoCidade);
                }

                if (!dr.IsClosed)
                    dr.Close();
            }

            return lista;
        }
        #endregion

        #region Salvar
        public void Salvar(IEnumerable<int> listCidade, DataTable dtEmail, IEnumerable<int> listEmailExcluir)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.Save(trans);

                        DataTable dt = null;

                        foreach (int idCidade in listCidade)
                        {
                            ListaCidade objListaCidade;
                            if (!ListaCidade.CarregarPorCidadeGrupo(new Cidade(idCidade), this, trans, out objListaCidade))
                            {
                                objListaCidade = new ListaCidade { Cidade = new Cidade(idCidade), GrupoCidade = this, FlagInativo = false };
                                objListaCidade.AddBulkTable(ref dt);
                            }
                            else if (objListaCidade.FlagInativo)
                            {
                                objListaCidade.FlagInativo = false;
                                objListaCidade.Save(trans);
                            }
                        }

                        ListaCidade.SaveBulkTable(dt, trans);

                        foreach (int idEmailDestinatarioCidade in listEmailExcluir)
                        {
                            EmailDestinatarioCidade objEmailDestinatarioCidade = EmailDestinatarioCidade.LoadObject(idEmailDestinatarioCidade, trans);
                            objEmailDestinatarioCidade.Inativar(trans);
                        }

                        foreach (DataRow drEmail in dtEmail.Rows)
                        {
                            EmailDestinatario objEmailDestinatario = EmailDestinatario.SalvarEmail(drEmail, trans);
                            EmailDestinatarioCidade.SalvarEmail(drEmail, trans, objEmailDestinatario, this);
                        }

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        #endregion

        #region SetInstance_NonDispose
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objGrupoCidade">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance_NonDispose(IDataReader dr, GrupoCidade objGrupoCidade)
        {
            objGrupoCidade._idGrupoCidade = Convert.ToInt32(dr["Idf_Grupo_Cidade"]);
            objGrupoCidade._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
            objGrupoCidade._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
            if (dr["Idf_Filial"] != DBNull.Value)
                objGrupoCidade._filial = new Filial(Convert.ToInt32(dr["Idf_Filial"]));

            objGrupoCidade._persisted = true;
            objGrupoCidade._modified = false;

            return true;
        }
        #endregion

    }
}