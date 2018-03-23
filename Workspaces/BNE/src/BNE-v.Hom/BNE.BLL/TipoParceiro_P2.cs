//-- Data: 25/07/2013 18:01
//-- Autor: Gieyson Stelmak

using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class TipoParceiro // Tabela: TAB_Tipo_Parceiro
    {

        #region Consultas
        private const string SpListar = @"
        SELECT  Idf_Tipo_Parceiro,
                Des_Tipo_Parceiro
        FROM    TAB_Tipo_Parceiro
        ORDER BY Idf_Tipo_Parceiro
        ";
        #endregion

        #region Métodos

        #region Clone
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion

        #region Listar
        public static SqlDataReader Listar()
        {
            return DataAccessLayer.ExecuteReader(CommandType.Text, SpListar, null);
        }
        #endregion

        #endregion

    }
}