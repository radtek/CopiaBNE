//-- Data: 25/07/2013 18:01
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class TipoParceiro // Tabela: TAB_Tipo_Parceiro
    {
        #region Consultas
        private const string SpListar = @"
SELECT Idf_Tipo_Parceiro,
       Des_Tipo_Parceiro
FROM BNE.TAB_Tipo_Parceiro
ORDER BY Idf_Tipo_Parceiro
";
        #endregion

        #region Métodos
        public static SqlDataReader Listar()
        {
            return DataAccessLayer.ExecuteReader(CommandType.Text, SpListar, null);
        }
        #endregion
    }
}