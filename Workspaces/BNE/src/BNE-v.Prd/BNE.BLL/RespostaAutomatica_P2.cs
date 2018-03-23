//-- Data: 18/03/2011 10:58
//-- Autor: Elias

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
	public partial class RespostaAutomatica // Tabela: BNE_Resposta_Automatica
    {
        #region Consultas

        #region SPSELECTTODASRESPOSTASAUTOMATICAS

        private const string SPSELECTTODASRESPOSTASAUTOMATICAS = @"
         SELECT	Idf_Resposta_Automatica 
	            ,Substring(Nme_Resposta_Automatica, 0, 40) + ' ...' as Nme_Resposta_Automatica
				,Dta_Cadastro
				FROM BNE_Resposta_Automatica order by dta_cadastro desc";

        #endregion

        #region SPSELECTRESPOSTASAUTOMATICAS

        private const string SPSELECTRESPOSTASAUTOMATICAS = @"
         DECLARE @FirstRec INT
        DECLARE @LastRec INT
        DECLARE @iSelect VARCHAR(8000)
        DECLARE @iSelectCount VARCHAR(8000)
        DECLARE @iSelectPag VARCHAR(8000)
        SET @FirstRec = ( @CurrentPage - 1 ) * @PageSize
        SET @LastRec = ( @CurrentPage * @PageSize + 1 )
        SET @iSelect = '
	        SELECT  ROW_NUMBER() OVER (ORDER BY Dta_Cadastro DESC) AS RowID ,
		        * FROM  (    
							 SELECT	Idf_Resposta_Automatica 
									,Substring(Nme_Resposta_Automatica, 0, 40) + '' ...'' as Nme_Resposta_Automatica
									,Dta_Cadastro
							FROM BNE_Resposta_Automatica ) as temp '

        SET @iSelectCount = 'Select Count(1) From ( ' + @iSelect + ' ) As TblTempCount'
        SET @iSelectPag = 'Select * From ( ' + @iSelect  + ' ) As TblTempPag	Where RowID > ' + CONVERT(VARCHAR, @FirstRec) + ' And RowID < ' + CONVERT(VARCHAR, @LastRec)
        EXECUTE (@iSelectCount)
        EXECUTE (@iSelectPag)";

        #endregion

        #region SPSELECTRESPOSTASAUTOMATICASFILTRO

        private const string SPSELECTRESPOSTASAUTOMATICASFILTRO = @"
        DECLARE @FirstRec INT
        DECLARE @LastRec INT
        DECLARE @iSelect VARCHAR(8000)
        DECLARE @iSelectCount VARCHAR(8000)
        DECLARE @iSelectPag VARCHAR(8000)
        SET @FirstRec = ( @CurrentPage - 1 ) * @PageSize
        SET @LastRec = ( @CurrentPage * @PageSize + 1 )
        SET @iSelect = '
	        SELECT  ROW_NUMBER() OVER (ORDER BY Dta_Cadastro DESC) AS RowID ,
		        * FROM  (   SELECT	Idf_Resposta_Automatica 
									,Substring(Nme_Resposta_Automatica, 0, 40) + '' ...'' as Nme_Resposta_Automatica
									,Dta_Cadastro
							FROM BNE_Resposta_Automatica 
                             	@Filtro ) as temp '

        SET @iSelectCount = 'Select Count(1) From ( ' + @iSelect + ' ) As TblTempCount'
        SET @iSelectPag = 'Select * From ( ' + @iSelect  + ' ) As TblTempPag	Where RowID > ' + CONVERT(VARCHAR, @FirstRec) + ' And RowID < ' + CONVERT(VARCHAR, @LastRec)
        EXECUTE (@iSelectCount)
        EXECUTE (@iSelectPag)";

        #endregion

        #endregion

        #region Metodos

        #region ListarTodas

        public static DataTable ListarTodas()
        {
            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, SPSELECTTODASRESPOSTASAUTOMATICAS, null).Tables[0];
        }
        #endregion

        #region ListarRespostasAutomaticasDT

        public static DataTable ListarRespostasAutomaticasDT(int paginaCorrente, int tamanhoPagina, out int totalRegistros)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@CurrentPage", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@PageSize", SqlDbType.Int, 4));

            parms[0].Value = paginaCorrente;
            parms[1].Value = tamanhoPagina;

            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTRESPOSTASAUTOMATICAS, parms))
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

        #region ListarRespostasAutomaticasDT

        public static DataTable ListarRespostasAutomaticasDT(string filtro, int paginaCorrente, int tamanhoPagina, out int totalRegistros)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@CurrentPage", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@PageSize", SqlDbType.Int, 4));
         
            String consulta = String.Empty;
            
            int filtroNum = 0;
            if (Int32.TryParse(filtro, out filtroNum))
            {
                consulta = SPSELECTRESPOSTASAUTOMATICASFILTRO.Replace("@Filtro", " where idf_Resposta_Automatica = " + filtroNum.ToString());
            }
            else
                consulta = SPSELECTRESPOSTASAUTOMATICASFILTRO.Replace("@Filtro", " where Nme_Resposta_Automatica like  ''%" + filtro + "%''");


            parms[0].Value = paginaCorrente;
            parms[1].Value = tamanhoPagina;
   


            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, consulta, parms))
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

        #endregion
    }
}