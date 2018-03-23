//-- Data: 08/04/2010 11:17
//-- Autor: Gieyson Stelmak

using System;
using System.Data;
namespace BNE.BLL
{
	public partial class TipoPagamento // Tabela: BNE_Tipo_Pagamento
	{
        #region SetInstance
        /// <summary>
        /// Método auxiliar utilizado para vincular as colunas com os atributos da classe, definindo se a leitura do DataReader e o Dispose devem ser excutado.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objTipoContrato">Instância a ser manipulada.</param>
        /// <param name="executeDispose">Define se o dispose no DataReader deve ser executado</param>
        /// <param name="executeRead">Define se o read no DataReader deve ser executado</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Francisco Ribas</remarks>
        internal static bool SetInstance(IDataReader dr, TipoPagamento objTipoPagamento, Boolean executeDispose, Boolean executeRead)
        {
            try
            {
                if (!executeRead || dr.Read())
                {
                    objTipoPagamento._idTipoPagamento = Convert.ToInt32(dr["Idf_Tipo_Pagamento"]);
                    objTipoPagamento._descricaoTipoPagamaneto = Convert.ToString(dr["Des_Tipo_Pagamaneto"]);
                    objTipoPagamento._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                    objTipoPagamento._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);

                    objTipoPagamento._persisted = true;
                    objTipoPagamento._modified = false;

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
                if (executeDispose)
                    dr.Dispose();
            }
        }
        #endregion
	}
}