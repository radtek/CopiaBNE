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
        private const string Spselectperguntas = "SELECT Idf_Vaga_Pergunta, Des_Vaga_Pergunta, Flg_Resposta, Idf_Tipo_Resposta, Idf_Vaga FROM BNE_Vaga_Pergunta WITH(NOLOCK) WHERE Idf_vaga = @Idf_Vaga AND Flg_Inativo = 0 ORDER BY Idf_Vaga_Pergunta";
        #endregion

        #region Spinativar
        private const string Spinativar = @"UPDATE BNE_Vaga_Pergunta SET Flg_Inativo = 1 WHERE Idf_Vaga_Pergunta = @Idf_Vaga_Pergunta";
        #endregion

        #endregion

        #region Construtores
        public VagaPergunta(int idVagaPergunta, string desVagaPergunta, bool? flagResposta, TipoResposta tipoResposta)
        {
            this._idVagaPergunta = idVagaPergunta;
            this._descricaoVagaPergunta = desVagaPergunta;
            this._flagResposta = flagResposta;
            this._tipoResposta = tipoResposta;
            this._persisted = true;
        }
        #endregion

        #region Métodos

        #region RecuperarPerguntas
        public static DataTable RecuperarPerguntas(Vaga objVaga)
        {
            return RecuperarPerguntas(objVaga, null);
        }

        public static DataTable RecuperarPerguntas(Vaga objVaga, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = objVaga.IdVaga }
                };

            DataTable dt = null;
            IDataReader dr;

            if (trans != null)
                dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectperguntas, parms);
            else
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectperguntas, parms);

            while (dr.Read())
            {
                Enumeradores.TipoResposta tipoResposta;
                Enum.TryParse(Convert.ToString(dr["Idf_Tipo_Resposta"]), out tipoResposta);

                bool? flagResposta = null;
                if (dr["Flg_resposta"] != DBNull.Value)
                {
                    flagResposta = Convert.ToBoolean(dr["Flg_resposta"]);
                }

                dt = DataTablePerguntas(dt, Convert.ToInt32(dr["Idf_Vaga_Pergunta"]), dr["Des_Vaga_Pergunta"].ToString(), flagResposta, tipoResposta);
            }

            if (!dr.IsClosed)
                dr.Close();

            dr.Dispose();

            return dt;
        }
        public static List<VagaPergunta> RecuperarListaPerguntas(int idVaga, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = idVaga }
                };

            List<VagaPergunta> listVagaPergunta = new List<VagaPergunta>();
            IDataReader dr;

            if (trans != null)
                dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectperguntas, parms);
            else
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectperguntas, parms);

            while (dr.Read())
            {
                bool? flagResposta = null;

                if (dr["Flg_Resposta"] != DBNull.Value)
                    flagResposta = Convert.ToBoolean(dr["Flg_Resposta"]);

                listVagaPergunta.Add(new VagaPergunta(Convert.ToInt32(dr["Idf_Vaga_Pergunta"]), dr["Des_Vaga_Pergunta"].ToString(), flagResposta, new TipoResposta(Convert.ToInt32(dr["Idf_Tipo_Resposta"]))));
            }

            if (!dr.IsClosed)
                dr.Close();

            dr.Dispose();

            return listVagaPergunta;
        }
        #endregion

        #region DataTablePerguntas
        public static DataTable DataTablePerguntas(DataTable dt, int? idVagaPergunta, string pergunta, bool? resposta, Enumeradores.TipoResposta tipoResposta)
        {
            string respostaObjetiva = resposta.HasValue && resposta.Value ? "Sim" : "Não";

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
            dr["Flg_Resposta"] = tipoResposta == Enumeradores.TipoResposta.RespostaDescritiva ? "Descritiva" : respostaObjetiva;
            dr["Idf_Tipo_Resposta"] = (int)tipoResposta;

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
        public static void Inativar(int idVagaPergunta,Vaga vaga,int?idUsuarioFilialPerfil,Enumeradores.VagaLog? Processo, SqlTransaction trans)
        {
            LogAlteracaoVaga objAlteracao = new LogAlteracaoVaga();
            objAlteracao.DescricaoAlteracao = "Inativou pergunta" + idVagaPergunta;
            objAlteracao.Vaga = vaga;
            if(idUsuarioFilialPerfil.HasValue)
                objAlteracao.UsuarioFilialPerfil = new UsuarioFilialPerfil(idUsuarioFilialPerfil.Value);
            if (Processo.HasValue)
                objAlteracao.NomeServico = Processo.ToString();
            objAlteracao.Save(trans);
                
            var parms = new List<SqlParameter> { new SqlParameter { ParameterName = "@Idf_Vaga_Pergunta", SqlDbType = SqlDbType.Int, Size = 4, Value = idVagaPergunta } };

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, Spinativar, parms);
        }
        #endregion

        #endregion

    }
}