//-- Data: 12/01/2016 14:52
//-- Autor: Gieyson Stelmak

using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
	public partial class PesquisaCurriculoCurriculos // Tabela: TAB_Pesquisa_Curriculo_Curriculos
	{

        #region Inser��o em Massa
        /// <summary>
        /// Crie uma tabela para inserir em massa.
        /// Passe um DataTable nulo para e a classe em populada. 
        /// Ela ir� instanciar um novo DataTable j� com colunas definidas apartir dos par�metros sql definidos na classe.
        /// Os valores setados nas propriedades s�o transformados em uma linha na tabela.
        /// </summary>
        /// <param name="dt"></param>
        public void AddBulkTable(ref DataTable dt)
        {
            DataAccessLayer.AddBulkTable(ref dt, this);
        }
        /// <summary>
        /// Realiza inser��o em massa.
        /// </summary>
        /// <param name="dt">Tabela criada pelo m�todo AddBulkTable</param>
        /// <param name="trans">Transa��o</param>
        public static void SaveBulkTable(DataTable dt, SqlTransaction trans = null)
        {
            DataAccessLayer.SaveBulkTable(dt, "TAB_Pesquisa_Curriculo_Curriculos", trans);
        }
        #endregion

	}
}