//-- Data: 22/06/2010 12:18
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
    public partial class Rastreador // Tabela: BNE_Rastreador
    {

        #region Consultas

        #region Spselectporfilial
        private const string Spselectporfilial = @"
        DECLARE @FirstRec INT
        DECLARE @LastRec INT
        DECLARE @iSelect VARCHAR(8000)
        DECLARE @iSelectCount VARCHAR(8000)
        DECLARE @iSelectPag VARCHAR(8000)
        SET @FirstRec = ( @CurrentPage - 1 ) * @PageSize
        SET @LastRec = ( @CurrentPage * @PageSize + 1 )
        SET @iSelect = '
            SELECT 
                ROW_NUMBER() OVER (ORDER BY Dta_Cadastro DESC) AS RowID ,
                *
            FROM 
            (
                SELECT 
	                R.Idf_Rastreador ,
	                F.Des_Funcao ,
	                R.Des_Palavra_Chave ,
	                ( SELECT 
		                COUNT (RCSub.Idf_Rastreador_Curriculo) 
	                  FROM BNE_Rastreador_Curriculo RCSub 
	                  WHERE RCSub.Idf_Rastreador = R.Idf_Rastreador ) as Qtd_Curriculo ,
	                R.Dta_Cadastro,
                    C.Nme_Cidade ,
                    C.Sig_Estado
                FROM BNE_Rastreador R WITH(NOLOCK)
                    LEFT JOIN plataforma.TAB_Funcao F WITH(NOLOCK) ON F.Idf_Funcao = R.Idf_Funcao
                    LEFT JOIN plataforma.TAB_Cidade C WITH(NOLOCK) ON C.Idf_Cidade = R.Idf_Cidade
                WHERE R.Idf_Filial = ' + CONVERT(VARCHAR, @Idf_Filial) + '
                    AND R.Flg_Inativo = 0
            ) as temp '

        SET @iSelectCount = 'Select Count(1) From ( ' + @iSelect + ' ) As TblTempCount'
        SET @iSelectPag = 'Select * From ( ' + @iSelect  + ' ) As TblTempPag	Where RowID > ' + CONVERT(VARCHAR, @FirstRec) + ' And RowID < ' + CONVERT(VARCHAR, @LastRec)
        EXECUTE (@iSelectCount)
        EXECUTE (@iSelectPag)";
        #endregion

        #region Spselectitens
        private const string Spselectitens = @"
        SELECT  R.*
        FROM    BNE_Rastreador R WITH(NOLOCK)
        WHERE   R.Flg_Inativo = 0";
        #endregion

        #region Sprastreadoresprogramados
        private const string Sprastreadoresprogramados = @"
        SELECT  COUNT(*) 
        FROM BNE_Rastreador WITH(NOLOCK)
        WHERE   Idf_Filial = @Idf_Filial 
                AND Flg_Inativo = 0";
        #endregion

        #region Spcvsencontrados
        private const string Spcvsencontrados = @"
        SELECT  COUNT(RC.Idf_Curriculo)
        FROM    BNE_Rastreador R WITH(NOLOCK)
                JOIN BNE_Rastreador_Curriculo RC WITH(NOLOCK) ON R.Idf_Rastreador = RC.Idf_Rastreador
        WHERE   Idf_Filial = @Idf_Filial 
                AND CONVERT(VARCHAR,RC.Dta_Cadastro,103) = CONVERT(VARCHAR,GETDATE(),103)
                AND R.Flg_Inativo = 0";
        #endregion

        #endregion

        #region Métodos

        #region ListarRastreadorFilialDT
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <returns>DataTable</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static DataTable ListarRastreadorFilialDT(int idFilial, int paginaCorrente, int tamanhoPagina, out int totalRegistros)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = idFilial },
                    new SqlParameter { ParameterName = "@CurrentPage", SqlDbType = SqlDbType.Int, Size = 4, Value = paginaCorrente },
                    new SqlParameter { ParameterName = "@PageSize", SqlDbType = SqlDbType.Int, Size = 4, Value = tamanhoPagina }
                };

            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectporfilial, parms))
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

        #region RecuperarItens
        public static List<Rastreador> RecuperarItens()
        {
            var list = new List<Rastreador>();
            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectitens, null))
            {
                var objRastreador = new Rastreador();
                while (SetInstanceNonDispose(dr, objRastreador))
                {
                    list.Add(objRastreador);
                    objRastreador = new Rastreador();
                }

                if (!dr.IsClosed)
                    dr.Close();
            }
            return list;
        }
        #endregion

        #region QuantidadeRastreadoresProgramadosPorEmpresa
        /// <summary>
        /// Retorna a quantidade dos rastreadores programados pela empresa
        /// </summary>
        /// <param name="idFilial"></param>
        /// <returns></returns>
        public static int QuantidadeRastreadoresProgramadosPorEmpresa(int idFilial)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = idFilial }
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Sprastreadoresprogramados, parms));
        }
        #endregion

        #region QuantidadeCvsEncontradosRastreador
        public static int QuantidadeCvsEncontradosRastreador(int idFilial)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = idFilial }
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spcvsencontrados, parms));
        }
        #endregion

        #region Salvar
        public void Salvar(List<RastreadorIdioma> listRastreadorIdioma, List<RastreadorDisponibilidade> listRastreadorDisponibilidade)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        Save(trans);
                        
                        RastreadorCurriculo.DeletePorRastreador(_idRastreador, trans);

                        RastreadorIdioma.DeletePorRastreador(_idRastreador, trans);
                        foreach (RastreadorIdioma objRastreadorIdioma in listRastreadorIdioma)
                        {
                            objRastreadorIdioma.Save(trans);
                        }

                        RastreadorDisponibilidade.DeletePorRastreador(_idRastreador, trans);
                        foreach (RastreadorDisponibilidade objRastreadorDisponibilidade in listRastreadorDisponibilidade)
                        {
                            objRastreadorDisponibilidade.Save(trans);
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
      
        #endregion

    }
}