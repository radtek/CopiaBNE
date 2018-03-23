//-- Data: 02/03/2010 09:20
//-- Autor: Gieyson Stelmak

using System.Data.SqlClient;
namespace BNE.BLL
{
	public partial class MiniCurriculo // Tabela: BNE_Mini_Curriculo
    {

        #region SalvarMiniCurriculo
        /// <summary>
        /// Método responsável por salvar um mini currículo 
        /// </summary>
        /// <param name="objPessoaFisca">Objeto da classe PessoaFisica</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public void SalvarMiniCurriculo(PessoaFisica objPessoaFisca, PessoaFisicaTemp objPessoaFiscaTemp)
        {
            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        //Pessoa Física
                        if (objPessoaFisca != null)
                        {
                            objPessoaFisca.Save(trans);
                            this.PessoaFisica = objPessoaFisca;
                            this.Save(trans);
                        }
                        else if (objPessoaFiscaTemp != null)
                        {
                            objPessoaFiscaTemp.Save(trans);
                            this.PessoaFisicaTemp = objPessoaFiscaTemp;
                            this.Save(trans);
                        }

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        #endregion

	}
}
