//-- Data: 12/09/2011 15:25
//-- Autor: Elias

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
	public partial class EnvioEmailSalBR // Tabela: TAB_Envio_Email_Sal_BR
	{
        #region Consulta

            private const string SELECT_CARREGAR_POR_DES_EMAIL = "SELECT * FROM BNE.TAB_Envio_Email_Sal_BR WHERE Des_Email = @Des_Email";
    
        #endregion

        #region Métodos Públicos e estáticos

            public static void SalvarOuAtualizarEmail(string email)
            {
                var registro = new EnvioEmailSalBR();

                // -> Carrega registro se existir
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SELECT_CARREGAR_POR_DES_EMAIL, new List<SqlParameter>() { new SqlParameter("@Des_Email", SqlDbType.VarChar, 100) { Value = email } }))
                {
                    SetInstance(dr, registro);

                    if (!dr.IsClosed)
                        dr.Close();
                }

                // -> Atualiza dados.
                registro.DataCadastro = DateTime.Now;
                registro.DescricaoEmail = email;

                registro.Save();
            }

        #endregion
	}
}