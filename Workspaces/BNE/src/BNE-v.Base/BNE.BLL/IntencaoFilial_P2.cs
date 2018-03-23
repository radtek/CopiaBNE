//-- Data: 08/11/2011 12:00
//-- Autor: Jhonatan Taborda

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
	public partial class IntencaoFilial // Tabela: BNE_Intencao_Filial
	{

        private const string Spselectporfilialcurriculo = "SELECT * FROM BNE_Intencao_Filial WITH(NOLOCK) WHERE Idf_Filial = @Idf_Filial AND Idf_Curriculo = @Idf_Curriculo";
        private const string Spselectporfilial = "SELECT * FROM BNE_Intencao_Filial WITH(NOLOCK) WHERE Idf_Filial = @Idf_Filial ";

        #region CarregarPorFilialCurriculo
        /// <summary>
        /// M�todo respons�vel por carregar uma instancia de Intencao_Filial atrav�s do
        /// identificar de uma filial e um curr�culo
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
		/// M�todo respons�vel por carregar todas as Intencao_Filial atrav�s do
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
		/// M�todo utilizado para inserir em massa todas as IntencaoFilial na CurriculoOrigem
		/// </summary>
		/// <remarks>Jhonatan Taborda</remarks>
		public static void InsereIntencaoOrigem(DataTable dt)
		{
			BulkInsert(dt);				
		}		
		#endregion
	}
}