using BNE.EL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BNE.BLL
{
    public partial class VagaDeficiencia
    {

        #region Consultas

        #region SpSelectPorDescricao
        private const string SpSelectPorDescricao = @"select * from bne_deficiencia_detalhe with(nolock)
                    where des_deficiencia_detalhe like @Descricao";
        #endregion

        #region spSelectDeficiencias
        private const string spSelectDeficiencias = @"select dd.*, vd.idf_vaga_deficiencia, d.des_deficiencia from BNE.bne_vaga_deficiencia vd with(nolock)
		left join bne_deficiencia_detalhe dd on dd.idf_deficiencia_detalhe = vd.idf_deficiencia_detalhe
		join plataforma.tab_deficiencia d on d.idf_deficiencia = vd.idf_deficiencia
		 where vd.idf_Vaga = @idf_Vaga";

        #endregion

        #region spExisteDeficiencia

        private const string spExisteDeficiencia = @" select top 1 idf_vaga_deficiencia from BNE.bne_vaga_deficiencia with(nolock)
                                                    where idf_vaga = @idf_vaga";
        #endregion

        #region SPDELETEALL
        private const string SPDELETEALL = "DELETE FROM BNE.bne_vaga_deficiencia WHERE idf_Vaga =  @idf_Vaga";
        #endregion

        #endregion

        #region Metodos

        #region CarregarPorDescricao
        /// <summary>
        /// Carrega uma instancia de Deficiencia a partir de sua descricao
        /// </summary>
        /// <param name="descricao">Descricao da Deficiencia</param>
        /// <returns>Instancia de Deficiencia</returns>
        public static List<VagaDeficiencia> CarregarPorDescricao(string descricao)
        {
            #region Cache
            //if (HabilitaCache)
            //{
            //    var deficiencia = Deficiencias.FirstOrDefault(f => f.Descricao.NormalizarStringLINQ().Equals(descricao.NormalizarStringLINQ()));
            //    if (deficiencia != null)
            //        return new Deficiencia { IdDeficiencia = deficiencia.Identificador, DescricaoDeficiencia = deficiencia.Descricao };

            //    return null;
            //}
            #endregion

            List<VagaDeficiencia> list = new List<VagaDeficiencia>();
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Descricao", SqlDbType = SqlDbType.VarChar, Size = 50, Value = descricao }
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpSelectPorDescricao, parms))
            {
                while (dr.Read())
                {
                    VagaDeficiencia objVagaDeficiencia = new VagaDeficiencia();
                    if (SetInstance(dr, objVagaDeficiencia))
                        list.Add(objVagaDeficiencia);
                }
                if (!dr.IsClosed)
                    dr.Close();
            }
            return list;

        }
        #endregion


        #region listaDeficienciaVaga
        public static List<VagaDeficiencia> listaDeficienciaVaga(int? idVaga)
        {
            List<VagaDeficiencia> lista = new List<VagaDeficiencia>();
            if (idVaga == null)
                return lista;

            var parms = new List<SqlParameter> {
                new SqlParameter("@idf_Vaga",  SqlDbType.Int, 4)
            };

            parms[0].Value = idVaga;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spSelectDeficiencias, parms))
            {

                while (dr.Read())
                    lista.Add(VagaDeficiencia.LoadObject(Convert.ToInt32(dr["idf_vaga_deficiencia"])));

                if (!dr.IsClosed)
                    dr.Close();
            }

            return lista;
        }
        #endregion

        #region ViewDeficiencias
        public static string ViewDeficiencias(int idVaga)
        {
            string deficiencias = null;
            var parms = new List<SqlParameter> {
                new SqlParameter("@idf_Vaga",  SqlDbType.Int, 4)
            };

            parms[0].Value = idVaga;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spSelectDeficiencias, parms))
            {

                while (dr.Read())
                    deficiencias += !String.IsNullOrEmpty(dr["des_Deficiencia_Detalhe"].ToString()) ? dr["des_deficiencia_Detalhe"].ToString() + ", " : dr["Des_deficiencia"].ToString() + ", ";
               
                if (!dr.IsClosed)
                    dr.Close();
            }
            if(!String.IsNullOrEmpty(deficiencias))
                deficiencias = deficiencias.Remove(deficiencias.Length - 2);

            return deficiencias;
        }
        #endregion

        #region ListaDeficienciaDetalhe
        public static DataTable ListaDeficienciaDetalhe(int idVaga)
        {
            List<DeficienciaDetalhe> lista = new List<DeficienciaDetalhe>();
           
             var parms = new List<SqlParameter> {
                new SqlParameter("@idf_Vaga",  SqlDbType.Int, 4)
            };

            parms[0].Value = idVaga;

            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, spSelectDeficiencias, parms).Tables[0];
           
        }
        #endregion

        #region ExisteDeficiencia
        public static bool ExisteDeficiencia(int idVaga)
        {
            var parms = new List<SqlParameter>{
                new SqlParameter("@idf_Vaga", SqlDbType.Int, 4)
            };
            parms[0].Value = idVaga;

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

        #region DeleteAll
        /// <summary>
        /// Deleta todas as deficiencias 
        /// </summary>
        /// <param name="idVaga"></param>
        public static void DeleteAll(int idVaga)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@idf_Vaga", SqlDbType.Int, 4));

            parms[0].Value = idVaga;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETEALL, parms);
        }
        #endregion

        #endregion

    }
}
