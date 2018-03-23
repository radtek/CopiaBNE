using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.BLL;

namespace Bne.Web.Services.API.Business
{
    public static class Celular
    {
        #region QRY_PF_POR_CURRICULO
        private const string QRY_PF_POR_CURRICULO = @" 
        SELECT  cv.Idf_Pessoa_Fisica 
        FROM    BNE_Curriculo cv with(nolock) 
        WHERE   cv.Idf_Curriculo = @Idf_Curriculo; 
        ";
        #endregion

        #region QRY_VALIDAR_CELULAR
        private const string QRY_VALIDAR_CELULAR = @" 
        UPDATE BNE.TAB_Pessoa_Fisica SET Flg_Celular_Confirmado = 1, Dta_Alteracao = GETDATE() WHERE Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica AND Num_DDD_Celular = @Num_DDD_Celular AND Num_Celular = @Num_Celular
        ";
        #endregion

        public static bool Validar(int Curriculo, string NumCompleto)
        {
            var parms1 = new List<SqlParameter> {new SqlParameter("@Idf_Curriculo", SqlDbType.Int) {Value = Curriculo}};
            var idPessoaFisica = (int) DataAccessLayer.ExecuteScalar(CommandType.Text, QRY_PF_POR_CURRICULO, parms1);

            if (idPessoaFisica != null)
            {
                var NumDDD = NumCompleto.Substring(0, 2);
                var NumCelular = NumCompleto.Substring(2);

                var parms2 = new List<SqlParameter>
                {
                    new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int) {Value = idPessoaFisica},
                    new SqlParameter("@Num_DDD_Celular", SqlDbType.VarChar) {Value = NumDDD},
                    new SqlParameter("@Num_Celular", SqlDbType.VarChar) {Value = NumCelular}
                };
                return (DataAccessLayer.ExecuteNonQuery(CommandType.Text, QRY_VALIDAR_CELULAR, parms2) > 0);
            }
            return false;
        }
    }
}