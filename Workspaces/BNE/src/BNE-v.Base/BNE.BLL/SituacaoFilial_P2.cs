//-- Data: 08/04/2010 11:17
//-- Autor: Gieyson Stelmak

using System;
using System.Data;

namespace BNE.BLL
{
	public partial class SituacaoFilial : ICloneable // Tabela: TAB_Situacao_Filial
    {

        #region Clone
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion

        #region Consultas

        private const string SELECTSITUACAOFILIAL = @"  
        SELECT  Idf_Situacao_Filial, 
                Des_Situacao_Filial 
        FROM    TAB_Situacao_Filial WITH ( NOLOCK )
        WHERE   Flg_Inativo = 0";

        #endregion

        #region Metodos

        #region SetInstance
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objSituacaoFilial">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool SetInstance(IDataReader dr, SituacaoFilial objSituacaoFilial)
        {
            try
            {
                if (dr.Read())
                {
                    objSituacaoFilial._idSituacaoFilial = Convert.ToInt32(dr["Idf_Situacao_Filial"]);
                    objSituacaoFilial._descricaoSituacaoFilial = Convert.ToString(dr["Des_Situacao_Filial"]);
                    objSituacaoFilial._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                    objSituacaoFilial._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);

                    objSituacaoFilial._persisted = true;
                    objSituacaoFilial._modified = false;

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                dr.Dispose();
            }
        }
        #endregion

        #region Listar

        public static IDataReader Listar()
        {
            return DataAccessLayer.ExecuteReader(CommandType.Text, SELECTSITUACAOFILIAL, null);
        }

        #endregion

        #endregion

    }
}