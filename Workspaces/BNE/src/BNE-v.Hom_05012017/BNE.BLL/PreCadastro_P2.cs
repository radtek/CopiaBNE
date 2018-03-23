using BNE.BLL.Custom;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BNE.BLL
{
    public partial class PreCadastro
    {
        #region Consultas

        #region SpVerificaCadastro
        private const string SpVerificaCadastro = @"Select idf_Pessoa_fisica from tab_pessoa_fisica with(nolock) 
                    where nme_Pessoa = @Nome and Eml_Pessoa = @Email";

        #endregion

        #region SpRetornarIdPreCadastro
        private const string SpRetornarIdPreCadastro = @"Select Idf_Pre_Cadastro from BNE_Pre_Cadastro with(nolock) where nme_Pessoa = @Nome and Eml_Pessoa = @Email ";

        #endregion

        #region SpCarregarCadastroAllin
        private const string SpCarregarCadastroAllin = @"select pc.eml_pessoa, pc.nme_pessoa, f.des_funcao, c.nme_cidade, c.sig_estado
                 from bne_pre_cadastro pc with(Nolock)
                    JOIN plataforma.tab_funcao f WITH ( NOLOCK ) ON f.idf_funcao = pc.idf_funcao
                    JOIN plataforma.tab_Cidade c WITH ( NOLOCK ) ON c.idf_cidade = pc.idf_Cidade
	                where pc.idf_pre_cadastro = @idf_Pre_Cadastro";
        #endregion

        #endregion

        #region Métodos

        #region Verifica se existe cadastro
        /// <summary>
        /// Se ja existir no bne um cadastro com o email e nome cadastrado na popup é inativado da table do preCadastro
        /// </summary>
        /// <param name="email"></param>
        /// <param name="nome"></param>
        public static void VerificaCadastro(PreCadastro objPre, int idfOrigem)
        {
            var parms = new List<SqlParameter>{
                new SqlParameter { ParameterName = "@Email", SqlDbType = SqlDbType.VarChar, Value = objPre.email},
                new SqlParameter { ParameterName = "@Nome", SqlDbType = SqlDbType.VarChar, Value = objPre.nome}
            };
            // se ja possuir email e nome igual cadastrados no bne é inativado o pre cadastro pra não receber mais jornal do preCadastro
            if (DataAccessLayer.ExecuteScalar(CommandType.Text, SpVerificaCadastro, parms) != null)
                Delete(objPre.email, objPre.nome, idfOrigem);


           // else
               // BufferAdicionarPopupAllin.Add(objPre.idPreCadastro);
            

        }
        #endregion


        #region CarregarCadastroAllin

        public static DataTable CarregarCadastroAllin(int idPreCadastro)
        {
            var parms = new List<SqlParameter>
				{
					new SqlParameter { ParameterName = "@idf_Pre_Cadastro", SqlDbType = SqlDbType.Int, Size = 4, Value = idPreCadastro }
				};

            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, SpCarregarCadastroAllin, parms).Tables[0];
        }
        #endregion


        #region RetornarIdPreCadastro
        public static int RetornarIdPreCadastro(string EmlPessoa, string NmePessoa)
        {
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("@Nome", SqlDbType.VarChar, 100));
                parms.Add(new SqlParameter("@Email", SqlDbType.VarChar, 100));

                parms[0].Value = NmePessoa;
                parms[1].Value = EmlPessoa;

                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpRetornarIdPreCadastro, parms))
                {
                    if (dr.Read())
                        return Convert.ToInt32(dr["idf_Pre_Cadastro"]);
                }
                return 0;
            }
            catch (Exception ex )
            {
                return 0;
            }
            
           
        }
        #endregion
        #endregion
    }
}
