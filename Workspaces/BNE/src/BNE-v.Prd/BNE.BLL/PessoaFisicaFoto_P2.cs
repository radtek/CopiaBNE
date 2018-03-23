//-- Data: 06/09/2013 09:20
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class PessoaFisicaFoto // Tabela: TAB_Pessoa_Fisica_Foto
    {

        #region Consultas

        #region Spselectfoto
        private const string Spselectfoto = @"
        SELECT  * 
        FROM    TAB_Pessoa_Fisica_Foto
        WHERE   Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica";
        #endregion Spselectfoto

        #region Sprecuperarfotoporcpf
        private const string Sprecuperarfotoporcpf = @"
        SELECT  PF.Idf_Sexo, IIF(PFF.Flg_Inativo = 0, PFF.Img_Pessoa, NULL) AS Img_Pessoa
        FROM    TAB_Pessoa_Fisica PF WITH(NOLOCK)
                LEFT JOIN TAB_Pessoa_Fisica_Foto PFF WITH(NOLOCK) ON PFF.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
        WHERE   PF.Num_CPF = @NumeroCPF
        ";
        #endregion Sprecuperarfotoporcpf

        #region SprecuperarfotoporcurriculoId
        private const string SprecuperarfotoporcurriculoId = @"
        SELECT  TOP 1 PFF.Img_Pessoa
        FROM    TAB_Pessoa_Fisica_Foto PFF WITH(NOLOCK)
                INNER JOIN TAB_Pessoa_Fisica PF WITH(NOLOCK) ON PFF.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
		        INNER JOIN BNE.BNE_Curriculo CR WITH(NOLOCK) ON CR.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
        WHERE   CR.Idf_Curriculo = @CurriculoId
                AND PFF.Flg_Inativo = 0
        ORDER BY PFF.Dta_Alteracao DESC";
        #endregion SprecuperarfotoporcurriculoId

        #endregion

        #region Métodos

        #region CarregarFoto
        /// <summary>
        /// Método responsável por carregar uma instancia de PessoaFisicaFoto
        /// </summary>
        /// <param name="idPessoaFisica">Identificador Pessoa Fisica</param>
        /// <param name="objPessoaFisicaFoto">PessoaFisicaFoto</param>
        /// <returns>True se idPessoaFisica tiver valor</returns>
        public static bool CarregarFoto(int idPessoaFisica, out PessoaFisicaFoto objPessoaFisicaFoto)
        {
            var parms = new List<SqlParameter> { new SqlParameter { ParameterName = "@Idf_Pessoa_Fisica", SqlDbType = SqlDbType.Int, Size = 4, Value = idPessoaFisica } };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectfoto, parms))
            {
                objPessoaFisicaFoto = new PessoaFisicaFoto();
                if (SetInstance(dr, objPessoaFisicaFoto))
                    return true;

                if (!dr.IsClosed)
                    dr.Close();
            }
            objPessoaFisicaFoto = null;
            return false;
        }
        #endregion

        #region RecuperarArquivo

        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static byte[] RecuperarArquivo(decimal numeroCPF)
        {
            bool homem;
            return RecuperarArquivo(numeroCPF, out homem);
        }
        public static byte[] RecuperarArquivo(decimal numeroCPF, out bool homem)
        {
            homem = true;
            var parms = new List<SqlParameter> { new SqlParameter { ParameterName = "@NumeroCPF", SqlDbType = SqlDbType.Decimal, Size = 11, Value = numeroCPF } };

            using (var resultado = DataAccessLayer.ExecuteReader(CommandType.Text, Sprecuperarfotoporcpf, parms))
            {
                if (resultado.Read())
                {
                    int sexo;
                    if (Int32.TryParse(resultado["Idf_Sexo"].ToString(), out sexo))
                    {
                        homem = sexo == (int)Enumeradores.Sexo.Masculino;
                    }
                    if (resultado["Img_Pessoa"] != DBNull.Value)
                    {
                        return (byte[])resultado["Img_Pessoa"];
                    }
                }
            }
            return null;
        }

        public static byte[] RecuperarFotoPorCurriculoId(int curriculoId)
        {
            var parms = new List<SqlParameter> { new SqlParameter { ParameterName = "@CurriculoId", SqlDbType = SqlDbType.Int, Value = curriculoId } };

            object res = DataAccessLayer.ExecuteScalar(CommandType.Text, SprecuperarfotoporcurriculoId, parms);

            if (Convert.IsDBNull(res) && res == null)
                return null;

            return (byte[])res;
        }

        #endregion

        #endregion

    }
}