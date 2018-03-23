//-- Data: 02/03/2010 09:20
//-- Autor: Gieyson Stelmak

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
	public partial class PerfilPermissao // Tabela: TAB_Perfil_Permissao
	{
        #region SPCOPIAPERMISSAODEUMPERFILPARAOUTRO
        private const string SPCOPIAPERMISSAODEUMPERFILPARAOUTRO = @"
        INSERT INTO TAB_Perfil_Permissao (
	        Idf_Perfil,
	        Idf_Permissao
        ) 
        (	SELECT @Idf_Perfil_Destino AS Idf_Perfil, 
		           TAB_Perfil_Permissao.Idf_Permissao 
	          FROM TAB_Perfil_Permissao
	         WHERE TAB_Perfil_Permissao.Idf_Perfil = @Idf_Perfil_Origem 
	           AND TAB_Perfil_Permissao.Idf_Permissao NOT IN (SELECT idf_permissao FROM TAB_Perfil_Permissao WHERE idf_perfil = @Idf_perfil_destino))";
        #endregion

        #region CopiarPermissoesDeUmPerfilParaOutro
        /// <summary>
        /// Método responsável por copiar as permissões vinculadas a um perfil (origem) para o perfil (destino). 
        /// </summary>
        /// <param name="pIdPerfilOrigem">Identificador do perfil que contém as permissões a serem copiadas.</param>
        /// <param name="pIdPerfildestino">Identificador do perfil que serão persistidas as permissões.</param>
        /// <remarks>Renan Prado</remarks>
        public static void CopiarPermissoesDeUmPerfilParaOutro(int pIdPerfilOrigem, int pIdPerfildestino)
        {
            #region Variáveis

            List<SqlParameter> parms = new List<SqlParameter>();

            #endregion

            #region Filtros

            parms.Add(new SqlParameter("@Idf_Perfil_Origem", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Perfil_Destino", SqlDbType.Int, 4));

            parms[0].Value = pIdPerfilOrigem;
            parms[1].Value = pIdPerfildestino;

            #endregion

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPCOPIAPERMISSAODEUMPERFILPARAOUTRO, parms);
        }

        #endregion

        #region SELPORSISTEMAGRID

        private const string SELPORSISTEMAGRID = @"
	        SELECT plataforma.TAB_Categoria_Permissao.Des_Categoria_Permissao, 
		           plataforma.TAB_Permissao.Idf_Permissao,
		           plataforma.TAB_Permissao.Des_Permissao
	          FROM plataforma.TAB_Categoria_Permissao
        INNER JOIN plataforma.TAB_Permissao
		        ON plataforma.TAB_Categoria_Permissao.Idf_Categoria_Permissao = plataforma.TAB_Permissao.Idf_Categoria_Permissao
	         WHERE 1 = 1
	           AND plataforma.TAB_Categoria_Permissao.Flg_Inativo = 0
	           AND plataforma.TAB_Permissao.Flg_Inativo = 0
          ORDER BY plataforma.TAB_Categoria_Permissao.Des_Categoria_Permissao ASC,
		           plataforma.TAB_Permissao.Des_Permissao ASC";

        #endregion

        #region DELPORPERFIL

        private const string DELPORPERFIL = @" DELETE FROM TAB_Perfil_Permissao WHERE Idf_Perfil = @Idf_Perfil";

        #endregion

        #region INSPERMISSOESPORPERFIL

        private const string INSPERMISSOESPORPERFIL = @"
        INSERT INTO TAB_Perfil_Permissao (
	        Idf_Perfil,
	        Idf_Permissao
        ) 
        (	SELECT @Idf_Perfil AS Idf_Perfil, 
		           plataforma.TAB_Permissao.Idf_Permissao 
	          FROM plataforma.TAB_Permissao
	         WHERE plataforma.TAB_Permissao.Flg_Inativo = 0
	           AND plataforma.TAB_Permissao.Idf_Permissao IN ";

        #endregion

        #region SELASSOCIACAO

        private const string SELASSOCIACAO = @"
            SELECT COUNT(*) FROM TAB_Perfil_Permissao
            WHERE Idf_Perfil = @Idf_Perfil AND Idf_Permissao = @Idf_Permissao";
        #endregion

        #region ListarGrid
        /// <summary>
        /// Lista os categorias de permissão e permissões por sistema de forma paginada para serem exibidos em gridview
        /// </summary>
        /// <param name="pIdSistema">Identificador do sistema</param>
        /// <param name="colunaOrdenacao">Coluna de ordenação</param>
        /// <param name="direcaoOrdenacao">Direção da ordenação</param>
        /// <param name="paginaCorrente">Página que deseja visualizar</param>
        /// <param name="tamanhoPagina">Quantidades de resgistros por página</param>
        /// <param name="totalRegistros">Quantidade de registros encontrados</param>
        /// <returns>Cursor de leitura do banco de dados</returns>
        /// <remarks>Renan Prado</remarks>
        public static IDataReader ListarGrid(int pIdSistema, string colunaOrdenacao, string direcaoOrdenacao,
            int paginaCorrente, int tamanhoPagina, out int totalRegistros)
        {

            #region Variáveis

            List<SqlParameter> parms = new List<SqlParameter>();

            #endregion

            #region Filtros

            parms.Add(new SqlParameter("@Idf_Sistema", SqlDbType.Int, 4));
            parms[0].Value = pIdSistema;

            #endregion

            totalRegistros = 0;
            int inicio = ((paginaCorrente - 1) * tamanhoPagina) + 1;
            int fim = paginaCorrente * tamanhoPagina;

            #region SPSELECTCOUNT
            string SPSELECTCOUNT = " SELECT COUNT(*) FROM (" + SELPORSISTEMAGRID + ") AS temp";
            #endregion

            #region SPSELECTPAG
            string SPSELECTPAG =
                    "SELECT *\n" +
                    "FROM (\n" +
                    "       SELECT \n" +
                    "           ROW_NUMBER() OVER (ORDER BY   " + colunaOrdenacao + " " + direcaoOrdenacao + "  ) AS RowID, " +
                    "           * \n" +
                    "       FROM (\n" +
                                    SELPORSISTEMAGRID +
                                ") AS temp \n" +
                    "   ) as x \n" +
                    "WHERE RowId BETWEEN " + inicio + " AND " + fim;
            #endregion

            totalRegistros = (int)DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, parms);
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, parms);
        }

        /// <summary>
        /// Lista os categorias de permissão e permissões por sistema.
        /// </summary>
        /// <returns>Cursor de leitura do banco de dados</returns>
        /// <remarks>Renan Prado</remarks>
        public static IDataReader ListarGrid()
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            return DataAccessLayer.ExecuteReader(CommandType.Text, SELPORSISTEMAGRID, parms);
        }
        #endregion

        #region Salvar
        /// <summary>
        /// Método utilizado para salvar uma lista de permissões a um perfil.
        /// </summary>
        /// <remarks>Renan Prado</remarks>
        public static void Salvar(int pIdPerfil, List<int> pLstPermissoes)
        {
            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        #region Variáveis

                        List<SqlParameter> parms = new List<SqlParameter>();
                        string query = string.Empty;

                        #endregion

                        #region Filtros

                        parms.Add(new SqlParameter("@Idf_Perfil", SqlDbType.Int, 4));
                        parms[0].Value = pIdPerfil;

                        #endregion

                        foreach (int id in pLstPermissoes)
                        {
                            if (string.IsNullOrEmpty(query))
                                query += " (" + id;
                            else
                                query += "," + id;
                        }
                        query += "))";

                        DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, DELPORPERFIL, parms);
                        DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, INSPERMISSOESPORPERFIL + query, parms);

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

        #region VerificaAssociacaoPerfilComPermissao
        /// <summary>
        /// Verifica se a associação entre o perfil e a permissão existe.
        /// </summary>
        /// <param name="pIdPermissao">Identificador da permissão</param>
        /// <param name="pIdPerfil">Identificador do perfil</param>
        /// <returns>status da verificação</returns>
        /// <remarks>Renan Prado</remarks>
        public static bool VerificaAssociacaoPerfilComPermissao(int pIdPermissao, int pIdPerfil)
        {
            #region Variáveis

            List<SqlParameter> parms = new List<SqlParameter>();
            int quantidade = 0;

            #endregion

            #region Filtros

            parms.Add(new SqlParameter("@Idf_Perfil", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Permissao", SqlDbType.Int, 4));

            parms[0].Value = pIdPerfil;
            parms[1].Value = pIdPermissao;

            #endregion

            quantidade = (int)DataAccessLayer.ExecuteScalar(CommandType.Text, SELASSOCIACAO, parms);
            return quantidade > 0;
        }

        #endregion
	}
}