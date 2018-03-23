//-- Data: 02/03/2010 09:54
//-- Autor: Gieyson Stelmak

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
	public partial class CategoriaHabilitacao // Tabela: TAB_Categoria_Habilitacao
    {
        #region Consulta
        private const string SELECTCATEGORIAHABILITACAO = @"Select 
                                                                Idf_Categoria_Habilitacao,
                                                                Des_Categoria_Habilitacao
                                                            From
                                                            plataforma.Tab_Categoria_Habilitacao";

        private const string SELECTCATEGORIAHABILITACAODescricao = @"SELECT * FROM plataforma.TAB_Categoria_Habilitacao
                                                            WHERE Flg_Inativo = 0 AND Des_Categoria_Habilitacao = @desCategoria";
        #endregion

        #region Métodos

        #region CarregarPorDescricao
        public static bool CarregarPorDescricao(string descricao, out CategoriaHabilitacao objCategoriaHabilitacao)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@desCategoria", SqlDbType = SqlDbType.VarChar, Size = 100, Value = descricao },
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SELECTCATEGORIAHABILITACAODescricao, parms))
            {
                objCategoriaHabilitacao = new CategoriaHabilitacao();
                if (SetInstance(dr, objCategoriaHabilitacao))
                    return true;

                if (!dr.IsClosed)
                    dr.Close();
            }

            return false;
        }
        #endregion

        public static IDataReader Listar()
        {
            return DataAccessLayer.ExecuteReader(CommandType.Text,SELECTCATEGORIAHABILITACAO,null);
        }
        #endregion

    }
}