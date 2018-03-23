//-- Data: 03/02/2011 16:47
//-- Autor: Elias Junior

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
	public partial class VagaCandidatoPergunta // Tabela: BNE_Vaga_Candidato_Pergunta
	{
        #region Consultas
        private const string SPSELECTRESPOSTAS = @"
        SELECT  VCP.Idf_Vaga_Pergunta, VCP.Flg_Resposta
        FROM    BNE.BNE_Vaga_Candidato_Pergunta VCP
                INNER JOIN BNE.BNE_Vaga_Candidato VC ON VCP.Idf_Vaga_Candidato = VC.Idf_Vaga_Candidato
                INNER JOIN BNE.BNE_Vaga_Pergunta VP ON VCP.Idf_Vaga_Pergunta = VP.Idf_Vaga_Pergunta
        WHERE   VP.Idf_Vaga = @Idf_Vaga
                AND VC.Idf_Curriculo = @Idf_Curriculo";
        #endregion

        #region Construtores
        public VagaCandidatoPergunta(int idVagaPergunta, bool flgResposta)
        {
            this._vagaPergunta = new VagaPergunta(idVagaPergunta);
            this._flagResposta = flgResposta;
            this._persisted = true;
        }
        #endregion

        public static List<VagaCandidatoPergunta> RecuperarListaResposta(int idVaga, int idCurriculo, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
            parms[0].Value = idVaga;
            parms[1].Value = idCurriculo;

            List<VagaCandidatoPergunta> listVagaPergunta = new List<VagaCandidatoPergunta>();
            IDataReader dr = null;

            if (trans != null)
                dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPSELECTRESPOSTAS, parms);
            else
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTRESPOSTAS, parms);

            while (dr.Read())
                listVagaPergunta.Add(new VagaCandidatoPergunta(Convert.ToInt32(dr["Idf_Vaga_Pergunta"]), Convert.ToBoolean(dr["Flg_resposta"])));

            if (!dr.IsClosed)
                dr.Close();

            dr.Dispose();

            return listVagaPergunta;
        }

	}
}