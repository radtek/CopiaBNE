//-- Data: 05/10/2011 11:31
//-- Autor: Gieyson Stelmak

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
	public partial class EmailDestinatario // Tabela: BNE_Email_Destinatario
    {

        #region Consultas
        
        

        #endregion

        #region SalvarEmail
        /// <summary>
        /// 
        /// </summary>
        public static EmailDestinatario SalvarEmail(DataRow drEmail, SqlTransaction trans)
        {
            EmailDestinatario objEmailDestinatario = new EmailDestinatario();

            int idEmail;
            if (Int32.TryParse(drEmail["Idf_Email_Destinatario"].ToString(), out idEmail))
            {
                if (idEmail > 0)
                {
                    objEmailDestinatario = new EmailDestinatario(idEmail);
                    objEmailDestinatario.CompleteObject(trans);
                    EmailDestinatario.SetInstance_DataRow(drEmail, objEmailDestinatario, false);
                }
                else
                    EmailDestinatario.SetInstance_DataRow(drEmail, objEmailDestinatario, true);
            }
            else
                EmailDestinatario.SetInstance_DataRow(drEmail, objEmailDestinatario, true);

            objEmailDestinatario.Save(trans);

            return objEmailDestinatario;
        }
        #endregion

        #region SetInstance_DataRow
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um DataRow e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objEmailDestinatarioCidade">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance_DataRow(DataRow dr, EmailDestinatario objEmailDestinatario, bool newObject)
        {
            try
            {
                if (dr["Idf_Email_Destinatario"] != DBNull.Value)
                    objEmailDestinatario._idEmailDestinatario = Convert.ToInt32(dr["Idf_Email_Destinatario"]);

                objEmailDestinatario._descricaoEmail = Convert.ToString(dr["Des_Email"]);
                objEmailDestinatario._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                objEmailDestinatario._dataAlteracao = Convert.ToDateTime(dr["Dta_Alteracao"]);
                objEmailDestinatario._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                objEmailDestinatario._usuarioGerador = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Gerador"]));
                objEmailDestinatario._nomePessoa = Convert.ToString(dr["Nme_Pessoa"]);
                if (dr["Num_DDD_Telefone"] != DBNull.Value)
                    objEmailDestinatario._numeroDDDTelefone = Convert.ToString(dr["Num_DDD_Telefone"]);
                if (dr["Num_Telefone"] != DBNull.Value)
                    objEmailDestinatario._numeroTelefone = Convert.ToString(dr["Num_Telefone"]);

                objEmailDestinatario._persisted = !newObject;
                objEmailDestinatario._modified = true;

                return true;
            }
            catch
            {
                throw;
            }
        }
        #endregion

	}
}