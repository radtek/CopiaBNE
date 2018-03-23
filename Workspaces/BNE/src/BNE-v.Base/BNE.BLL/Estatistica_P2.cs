//-- Data: 04/05/2010 09:15
//-- Autor: Gieyson Stelmak

using System.Data;
using BNE.EL;
namespace BNE.BLL
{
	public partial class Estatistica // Tabela: BNE_Estatistica
    {

        private const string SPSELECTESTATISTICA = @"SELECT 
                                                            TOP 1 *
                                                    FROM BNE_Estatistica
                                                    ORDER BY Dta_Cadastro DESC
                                                         ";

        #region RecuperarEstatistica
        /// <summary>
        /// Método responsável por retornar a última estatística do site de curriculos e vagas.
        /// </summary>
        /// <returns></returns>
        public static Estatistica RecuperarEstatistica()
        {
            Estatistica objEstatistica;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTESTATISTICA, null))
            {
                objEstatistica = new Estatistica();
                if (SetInstance(dr, objEstatistica))
                    return objEstatistica;
            }
            throw (new RecordNotFoundException(typeof(Estatistica)));
        }
        #endregion

    }
}