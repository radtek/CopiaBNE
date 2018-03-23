//-- Data: 08/11/2011 12:00
//-- Autor: Jhonatan Taborda

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
	public partial class IntencaoFilial // Tabela: BNE_Intencao_Filial
	{

		private const string Spselectporfilialcurriculo = "SELECT * FROM BNE_Intencao_Filial WITH(NOLOCK) WHERE Idf_Filial = @Idf_Filial AND Idf_Curriculo = @Idf_Curriculo";
		private const string Spselectporfilial = "SELECT * FROM BNE_Intencao_Filial WITH(NOLOCK) WHERE Idf_Filial = @Idf_Filial ";

        #region [spCarregarPorCurriculo]
        private const string spCarregarPorCurriculo = @" DECLARE @FirstRec INT
                DECLARE @LastRec INT
                DECLARE @iSelect VARCHAR(8000)
                DECLARE @iSelectCount VARCHAR(8000)
                DECLARE @iSelectPag VARCHAR(8000)
                SET @FirstRec = ( @CurrentPage - 1 ) * @PageSize
                SET @LastRec = ( @CurrentPage * @PageSize + 1 )
                SET @iSelect = '
                SELECT ROW_NUMBER() OVER (ORDER BY intf.dta_cadastro DESC) AS RowID,
                        f.raz_social, intf.dta_cadastro 
			        from  bne.BNE_Intencao_Filial intf with(nolock)
			        join bne.tab_filial f with(nolock) on f.idf_Filial = intf.idf_Filial
			        where intf.flg_inativo = 0  
			        and intf.Idf_Curriculo = '  + CONVERT(VARCHAR, @Idf_Curriculo)
			        if(@Dta_Inicio is not null and @Dta_Fim is not null)
			         begin
			         SET @iSelect = @iSelect + ' AND  intf.dta_cadastro BETWEEN convert(date, ''' + @Dta_Inicio + ''')
                                           AND     '''+ @Dta_Fim +''''
			         end
				
                SET @iSelectCount = 'Select Count(*) From ( ' + @iSelect + ' ) As TblTempCount'
                SET @iSelectPag = 'Select * From ( ' + @iSelect + ' ) As TblTempPag	Where RowID > ' + CONVERT(VARCHAR, @FirstRec) + ' And RowID < ' + CONVERT(VARCHAR, @LastRec)
                EXECUTE (@iSelectCount)
                EXECUTE (@iSelectPag)";
        #endregion
        #region CarregarPorFilialCurriculo
        /// <summary>
        /// Método responsável por carregar uma instancia de Intencao_Filial através do
        /// identificar de uma filial e um currículo
        /// </summary>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorFilialCurriculo(int idCurriculo, int idFilial, out IntencaoFilial objIntencaoFilial)
		{
			return CarregarPorFilialCurriculo(idCurriculo, idFilial, null, out objIntencaoFilial);
		}
		public static bool CarregarPorFilialCurriculo(int idCurriculo, int idFilial, SqlTransaction trans, out IntencaoFilial objIntencaoFilial)
		{
			var parms = new List<SqlParameter>
				{
					new SqlParameter{ ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = idFilial}, 
					new SqlParameter{ ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = idCurriculo} 
				};

			using (IDataReader dr = trans != null ? DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectporfilialcurriculo, parms) : DataAccessLayer.ExecuteReader(CommandType.Text, Spselectporfilialcurriculo, parms))
			{
				objIntencaoFilial = new IntencaoFilial();
				if (SetInstance(dr, objIntencaoFilial))
					return true;

				if (!dr.IsClosed)
					dr.Close();

				dr.Dispose();
			}

			objIntencaoFilial = null;
			return false;
		}
		#endregion

		#region CarregarPorPessoaFisica
		/// <summary>
		/// Método responsável por carregar todas as Intencao_Filial através do
		/// identificar de uma filial
		/// </summary>
		/// <remarks>Jhonatan Taborda</remarks>
		public static DataTable CarregarPorFilial(int idFilial, out IntencaoFilial objIntencaoFilial)
		{
			var dtZero = new DataTable();
			var parms = new List<SqlParameter>
				{
					new SqlParameter{ ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = idFilial}, 
				};

			using (DataTable dt = DataAccessLayer.ExecuteReaderDs(CommandType.Text, Spselectporfilial, parms).Tables[0])
			{
				objIntencaoFilial = new IntencaoFilial();
				if (dt.Rows.Count > 0)
					return dt;			
			}
			objIntencaoFilial = null;
			return dtZero;
		}
		#endregion

		#region	InsereIntencaoOrigem
		/// <summary>
		/// Método utilizado para inserir em massa todas as IntencaoFilial na CurriculoOrigem
		/// </summary>
		/// <remarks>Jhonatan Taborda</remarks>
		public static void InsereIntencaoOrigem(DataTable dt)
		{
			BulkInsert(dt);				
		}
        #endregion

        #region RecuperarQuemMeViu
        /// <summary>
        /// </summary>
        /// <param name="idCurriculo">Id do Curriculo da pessoa fisica logada</param>
        /// <param name="paginaCorrente">pagina corrente</param>
        /// <param name="tamanhoPagina">Tamanho de registros por pagina</param>
        /// <param name="totalRegistros">Total de registros encontrados</param>
        /// <returns>DataTable com os registros encontrados</returns>
        public static DataTable CarregarPorCurriculo(int idCurriculo, int paginaCorrente, int tamanhoPagina, out int totalRegistros, DateTime? DtaInicio = null, DateTime? DtaFim = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = idCurriculo},
                    new SqlParameter{ ParameterName = "@CurrentPage", SqlDbType = SqlDbType.Int, Size = 4, Value = paginaCorrente},
                    new SqlParameter{ ParameterName = "@PageSize", SqlDbType = SqlDbType.Int, Size = 4, Value = tamanhoPagina}

                };
            if (DtaInicio.HasValue)
                parms.Add(new SqlParameter { ParameterName = "@Dta_Inicio", SqlDbType = SqlDbType.VarChar, Value = DtaInicio.Value.ToString("MM/dd/yyyy") });
            else
                parms.Add(new SqlParameter { ParameterName = "@Dta_Inicio", SqlDbType = SqlDbType.VarChar, Value = DBNull.Value });

            if (DtaFim.HasValue)
                parms.Add(new SqlParameter { ParameterName = "@Dta_Fim", SqlDbType = SqlDbType.VarChar, Value = Convert.ToDateTime(DtaFim.Value).AddHours(23).AddMinutes(59).ToString("MM/dd/yyyy HH:mm")});
            else
                parms.Add(new SqlParameter { ParameterName = "@Dta_Fim", SqlDbType = SqlDbType.VarChar, Value = DBNull.Value });


            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spCarregarPorCurriculo, parms))
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
    }
}