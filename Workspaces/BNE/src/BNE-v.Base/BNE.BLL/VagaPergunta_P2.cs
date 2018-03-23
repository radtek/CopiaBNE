//-- Data: 17/11/2010 10:02
//-- Autor: Vinicius Maciel

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class VagaPergunta // Tabela: BNE_Vaga_Pergunta
    {

        #region Consultas

        #region Spselectperguntas
        private const string Spselectperguntas = "SELECT Idf_vaga_Pergunta, Des_Vaga_Pergunta, Flg_resposta FROM BNE_Vaga_Pergunta WHERE Idf_vaga = @Idf_Vaga AND Flg_Inativo = 0 ORDER BY Idf_Vaga_Pergunta";
        #endregion

        #region Spinativar
        private const string Spinativar = @"UPDATE BNE_Vaga_Pergunta SET Flg_Inativo = 1 WHERE Idf_Vaga_Pergunta = @Idf_Vaga_Pergunta";
        #endregion

        #endregion

        #region Construtores
        public VagaPergunta(int idVagaPergunta, string desVagaPergunta, bool flagResposta)
        {
            this._idVagaPergunta = idVagaPergunta;
            this._descricaoVagaPergunta = desVagaPergunta;
            this._flagResposta = flagResposta;
            this._persisted = true;
        }
        #endregion

        #region Métodos

        #region RecuperarPerguntas
        public static DataTable RecuperarPerguntas(int idVaga)
        {
            return RecuperarPerguntas(idVaga, null);
        }

        public static DataTable RecuperarPerguntas(int idVaga, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4));
            parms[0].Value = idVaga;

            DataTable dt = null;
            IDataReader dr = null;

            if (trans != null)
                dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectperguntas, parms);
            else
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectperguntas, parms);

            while (dr.Read())
                dt = DataTablePerguntas(dt, Convert.ToInt32(dr["Idf_Vaga_Pergunta"]), dr["Des_Vaga_Pergunta"].ToString(), Convert.ToBoolean(dr["Flg_resposta"]), (int)Enumeradores.TipoPergunta.RespostaObjetiva);

            if (!dr.IsClosed)
                dr.Close();

            dr.Dispose();

            return dt;
        }
        public static List<VagaPergunta> RecuperarListaPerguntas(int idVaga, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4));
            parms[0].Value = idVaga;

            List<VagaPergunta> listVagaPergunta = new List<VagaPergunta>();
            IDataReader dr = null;

            if (trans != null)
                dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectperguntas, parms);
            else
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectperguntas, parms);

            while (dr.Read())
                listVagaPergunta.Add(new VagaPergunta(Convert.ToInt32(dr["Idf_Vaga_Pergunta"]), dr["Des_Vaga_Pergunta"].ToString(), Convert.ToBoolean(dr["Flg_resposta"])));

            if (!dr.IsClosed)
                dr.Close();

            dr.Dispose();

            return listVagaPergunta;
        }
        #endregion

        #region DataTablePerguntas
        public static DataTable DataTablePerguntas(DataTable dt, int? idVagaPergunta, string pergunta, bool resposta, int idfTipoResposta)
        {
            string respostaDr = resposta ? "Sim" : "Não";

            if (dt == null)
            {
                dt = new DataTable();
                dt.Columns.Add("Idf_Vaga_Pergunta");
                dt.Columns.Add("Des_Vaga_Pergunta");
                dt.Columns.Add("Flg_Resposta");
                dt.Columns.Add("Idf_Tipo_Resposta");
            }

            DataRow dr = dt.NewRow();

            string idfVagaPergunta = idVagaPergunta.HasValue ? idVagaPergunta.Value.ToString() : Guid.NewGuid().ToString().Replace("-", "");

            dr["Idf_Vaga_Pergunta"] = idfVagaPergunta;
            dr["Des_Vaga_Pergunta"] = pergunta;
            dr["Flg_Resposta"] = respostaDr;
            dr["Idf_Tipo_Resposta"] = idfTipoResposta;

            dt.Rows.Add(dr);

            return dt;
        }
        #endregion

        #region Inativar
        /// <summary>
        /// Método utilizado para excluir uma instância de VagaPergunta no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idVagaPergunta">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Inativar(int idVagaPergunta, SqlTransaction trans)
        {
            var parms = new List<SqlParameter> { new SqlParameter { ParameterName = "@Idf_Vaga_Pergunta", SqlDbType = SqlDbType.Int, Size = 4, Value = idVagaPergunta } };

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, Spinativar, parms);
        }
        #endregion

        #endregion

    }
}