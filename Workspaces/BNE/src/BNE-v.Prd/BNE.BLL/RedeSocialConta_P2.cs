//-- Data: 29/04/2010 09:17
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.BLL.Enumeradores;

namespace BNE.BLL
{
    public partial class RedeSocialConta
    {

        private const string SPSELECTDIVULGADORAS = "SELECT Idf_Rede_Social_Conta FROM BNE_Rede_Social_Conta WHERE Flg_Vaga = 1";

        #region RecuperarListRedeSocialContaDivulgadoras
        /// <summary>
        /// Método responsável por retornar uma lista com as Rede social conta divulgadoras de vaga
        /// </summary>
        /// <returns></returns>
        public static List<RedeSocialConta> RecuperarListRedeSocialContaDivulgadoras()
        {
            List<RedeSocialConta> listRedeSocialConta = new List<RedeSocialConta>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTDIVULGADORAS, null))
            {
                while (dr.Read())
                {
                    listRedeSocialConta.Add(RedeSocialConta.LoadObject(Convert.ToInt32(dr[0])));
                }
                if (!dr.IsClosed)
                    dr.Close();
            }

            return listRedeSocialConta;
        }
        #endregion

        #region EnviarMensagemVaga
        public void EnviarMensagemVaga(VagaRedeSocial objVagaRedeSocial)
        {
            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        objVagaRedeSocial.Save(trans);
                        this.EnviarMensagem(this.RedeSocialcs);

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

        #region EnviarMensagem
        private void EnviarMensagem(RedeSocialCS objRedeSocialCS)
        {
            switch ((Enumeradores.RedeSocial)Enum.Parse(typeof(Enumeradores.RedeSocial), objRedeSocialCS.IdRedeSocialCS.ToString()))
            {
                case Enumeradores.RedeSocial.Twitter:

                    break;
                case Enumeradores.RedeSocial.Orkut:

                    break;
                case Enumeradores.RedeSocial.FaceBook:
                   
                    break;
                case Enumeradores.RedeSocial.MySpace:
                    
                    break;
                case Enumeradores.RedeSocial.MSN:
                    
                    break;
                case Enumeradores.RedeSocial.GTalk:
                    
                    break;
                case Enumeradores.RedeSocial.Skype:
                    
                    break;
            }
        }
        #endregion

    }
}
