using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BNE.BLL
{
    public partial class PessoaFisicaDeficiencia
    {

        #region Consultas

        #region spSelectPessoaFisicaDeficiencia
        private const string spSelectPessoaFisicaDeficiencia = "";
        #endregion

        #region spSelectDeficiencias
        private const string spSelectDeficiencias = @"
        select * from BNE.BNE_Pessoa_Fisica_Deficiencia with(nolock) where idf_pessoa_fisica = @idf_Pessoa_Fisica";

        #endregion

        #region spViewDeficiencia
        private const string spViewDeficiencia =@"  select dd.idf_deficiencia_detalhe, dd.des_deficiencia_detalhe,
		       pd.idf_deficiencia, pd.idf_pessoa_fisica_deficiencia, d.des_deficiencia 
		       from BNE.BNE_Pessoa_Fisica_Deficiencia pd with(nolock)
              left join bne_deficiencia_detalhe dd on dd.idf_deficiencia_detalhe = pd.idf_deficiencia_detalhe
              join plataforma.tab_deficiencia d on d.idf_deficiencia = pd.idf_deficiencia
               where pd.idf_pessoa_fisica = @idf_Pessoa_Fisica ";
	    #endregion

        #region spExisteDeficiencia

        private const string spExisteDeficiencia = @" select top 1 idf_Pessoa_fisica_deficiencia from BNE.bne_Pessoa_Fisica_deficiencia with(nolock)
                                                    where idf_Pessoa_fisica = @idf_Pessoa_fisica";
        #endregion

        #region SPDELETEALL
        private const string SPDELETEALL = "DELETE FROM BNE.BNE_Pessoa_Fisica_Deficiencia WHERE idf_Pessoa_Fisica =  @idf_Pessoa_Fisica";
        #endregion
        
        #endregion

        #region Metodos

        #region ListaPessoaFisicaDeficiencia

        public static List<PessoaFisicaDeficiencia> ListaPessoaFisicaDeficiencia(int idPessoaFisica)
        {
            List<PessoaFisicaDeficiencia> listaDeficiencia = new List<PessoaFisicaDeficiencia>();

            var parms = new List<SqlParameter>{
               new SqlParameter("@idf_Pessoa_Fisica", SqlDbType.Int, 4)
            };
            parms[0].Value = idPessoaFisica;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spSelectPessoaFisicaDeficiencia, parms))
            {
                while (dr.Read())
                    listaDeficiencia.Add(PessoaFisicaDeficiencia.LoadObject(Convert.ToInt32(dr["Idf_Pessoa_Fisica_Deficiencia"])));
                if (!dr.IsClosed)
                    dr.Close();
            }

            return listaDeficiencia;

        }
        #endregion

        #region ExisteDeficiencia
        public static bool ExisteDeficiencia(int idPessoaFisica)
        {
            var parms = new List<SqlParameter>{
                new SqlParameter("@idf_Pessoa_Fisica", SqlDbType.Int, 4)
            };
            parms[0].Value = idPessoaFisica;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spExisteDeficiencia, parms))
            {
                if (dr.Read())
                    return true;

                if (!dr.IsClosed)
                    dr.Close();
            }
            return false;
        }
        #endregion

        #region ListaDeficiencia
        public static List<PessoaFisicaDeficiencia> listaDeficiencia(int idPessoaFisica)
        {
            List<PessoaFisicaDeficiencia> lista = new List<PessoaFisicaDeficiencia>();
            var parms = new List<SqlParameter> {
                new SqlParameter("@idf_Pessoa_Fisica",  SqlDbType.Int, 4)
            };

            parms[0].Value = idPessoaFisica;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spSelectDeficiencias, parms))
            {

                while (dr.Read())
                    lista.Add(PessoaFisicaDeficiencia.LoadObject(Convert.ToInt32(dr["idf_Pessoa_Fisica_Deficiencia"])));

                if (!dr.IsClosed)
                    dr.Close();
            }

            return lista;
        }
        #endregion

        #region ViewDeficiencias
        public static string ViewDeficiencias(int idPessoaFisica)
        {
            string deficiencias = null;
            var parms = new List<SqlParameter> {
                new SqlParameter("@idf_Pessoa_Fisica",  SqlDbType.Int, 4)
            };

            parms[0].Value = idPessoaFisica;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spViewDeficiencia, parms))
            {

                while (dr.Read())
                    deficiencias += !String.IsNullOrEmpty(dr["des_Deficiencia_Detalhe"].ToString()) ? dr["des_deficiencia_Detalhe"].ToString() + ", " : dr["Des_deficiencia"].ToString() + ", ";

                if (!dr.IsClosed)
                    dr.Close();
            }
            if (!String.IsNullOrEmpty(deficiencias))
                deficiencias = deficiencias.Remove(deficiencias.Length - 2);

            return deficiencias;
        }
        #endregion

        #region DeleteAll
        /// <summary>
        /// Deleta todas as deficiencias 
        /// </summary>
        /// <param name="idPessoaFisica"></param>
        public static void DeleteAll(int idPessoaFisica)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@idf_Pessoa_Fisica", SqlDbType.Int, 4));

            parms[0].Value = idPessoaFisica;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETEALL, parms);
        }
        #endregion

        #endregion

     
    }
}
