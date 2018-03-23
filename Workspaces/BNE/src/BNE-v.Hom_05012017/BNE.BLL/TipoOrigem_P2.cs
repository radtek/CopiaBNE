//-- Data: 01/08/2011 16:37
//-- Autor: Vinicius Maciel

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
	public partial class TipoOrigem // Tabela: BNE_Tipo_Origem
    {
        #region Consultas

        private const string SPLISTAR = "SELECT * FROM BNE_Tipo_Origem WHERE Flg_Inativo = 0 AND Idf_Tipo_Origem <> 3";

        #endregion

        #region Metodos        

        #region Listar

        public static IDataReader Listar()
        {
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPLISTAR, null);
        }

        #endregion

        #endregion
    }
}