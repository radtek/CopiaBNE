//-- Data: 12/01/2016 14:52
//-- Autor: Gieyson Stelmak

using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
	public partial class PesquisaCurriculoCurriculos // Tabela: TAB_Pesquisa_Curriculo_Curriculos
	{

        #region Inserção em Massa
        /// <summary>
        /// Crie uma tabela para inserir em massa.
        /// Passe um DataTable nulo para e a classe em populada. 
        /// Ela irá instanciar um novo DataTable já com colunas definidas apartir dos parâmetros sql definidos na classe.
        /// Os valores setados nas propriedades são transformados em uma linha na tabela.
        /// </summary>
        /// <param name="dt"></param>
        public void AddBulkTable(ref DataTable dt)
        {
            DataAccessLayer.AddBulkTable(ref dt, this);
        }
        /// <summary>
        /// Realiza inserção em massa.
        /// </summary>
        /// <param name="dt">Tabela criada pelo método AddBulkTable</param>
        /// <param name="trans">Transação</param>
        public static void SaveBulkTable(DataTable dt, SqlTransaction trans = null)
        {
            DataAccessLayer.SaveBulkTable(dt, "TAB_Pesquisa_Curriculo_Curriculos", trans);
        }
        #endregion

	}
}