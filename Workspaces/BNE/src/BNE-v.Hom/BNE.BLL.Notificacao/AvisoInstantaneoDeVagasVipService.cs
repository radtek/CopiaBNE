using BNE.BLL.Notificacao.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL.Notificacao
{
    public class AvisoInstantaneoDeVagasVipService
    {
        /// <summary>
        ///     Le uma tabela do banco e retorna os Jornal de Vagas que estão aguardando serem processados
        /// </summary>
        /// <returns>IEnumerable de ProcessamentoJornalVagas</returns>
        public List<ProcessamentoAvisoVagasVip> GetAllToProcess()
        {
            List <ProcessamentoAvisoVagasVip> _list = new List<ProcessamentoAvisoVagasVip>();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, "JornalVaga_RecuperarAguardandoProcessamento_VIP", null))            
            {
                while (dr.Read())
                {
                    var objProcessamentoJornalVagasVip = new ProcessamentoAvisoVagasVip();
                    if (objProcessamentoJornalVagasVip.SetInstance(dr))
                        _list.Add(objProcessamentoJornalVagasVip);
                }
            }

            return _list;
        }

        public void AtualizarDataInicio(ProcessamentoAvisoVagasVip objProcessamentoJornalVagas)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Processamento_Jornal_Vagas", SqlDbType = SqlDbType.Int, Size = 4, Value = objProcessamentoJornalVagas.IdProcessamentoJornalVagas},
                new SqlParameter {ParameterName = "@Data", SqlDbType = SqlDbType.DateTime, Value = objProcessamentoJornalVagas.DataInicioProcessamento}
            };
            DataAccessLayer.ExecuteNonQuery(CommandType.StoredProcedure, "JornalVaga_AtualizarDataInicioProcessamento_VIP", parms);
        }

        public void AtualizarDataFim(ProcessamentoAvisoVagasVip objProcessamentoJornalVagas)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Processamento_Jornal_Vagas", SqlDbType = SqlDbType.Int, Size = 4, Value = objProcessamentoJornalVagas.IdProcessamentoJornalVagas},
                new SqlParameter {ParameterName = "@Data", SqlDbType = SqlDbType.DateTime, Value = objProcessamentoJornalVagas.DataFimProcessamento}
            };
            DataAccessLayer.ExecuteNonQuery(CommandType.StoredProcedure, "JornalVaga_AtualizarDataFimProcessamento_VIP", parms);
        }

        public List<Vaga> RecuperarVagas(ProcessamentoAvisoVagasVip objProcessamentoJornalVagas)
        {
            var list = new List<Vaga>();
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@idfs_Vagas", SqlDbType = SqlDbType.VarChar, Value = objProcessamentoJornalVagas.CodigoVagas}
            };
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, "JornalVaga_RecuperarVagas", parms))
            {
                while (dr.Read())
                {
                    var dto = new Vaga();
                    dto.ReadFromDataReader(dr);
                    list.Add(dto);
                }
            }
            return list;
        }

        public List<Curriculo> RecuperarCurriculos(ProcessamentoAvisoVagasVip objProcessamentoJornalVagas)
        {
            var list = new List<Curriculo>();
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@idfs_curriculos", SqlDbType = SqlDbType.VarChar, Value = objProcessamentoJornalVagas.CodigoCurriculos}
            };
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, "JornalVaga_RecuperarCurriculos", parms))
            {
                while (dr.Read())
                {
                    var vip = Convert.ToBoolean(dr["Flg_VIP"]);
                    if (vip)
                    {
                        list.Add(new Curriculo
                        {
                            IdCurriculo = Convert.ToInt32(dr["Idf_Curriculo"]),
                            Nome = dr["Nme_Pessoa"].ToString(),
                            Email = dr["Eml_Pessoa"].ToString(),
                            Funcao = dr["Des_Funcao"].ToString(),
                            Cidade = dr["Nme_Cidade"].ToString(),
                            Estado = dr["Sig_Estado"].ToString(),
                            QuantidadeQuemMeViu15Dias = Convert.ToInt32(dr["QtdQuemMeViu15"]),
                            QuantidadeQuemMeViu30Dias = Convert.ToInt32(dr["QtdQuemMeViu30"]),
                            VIP = vip,
                            CPF = Convert.ToDecimal(dr["Num_CPF"]),
                            DataNascimento = Convert.ToDateTime(dr["Dta_Nascimento"]),
                            DDD = dr["Num_DDD_Celular"].ToString(),
                            Celular = dr["Num_Celular"].ToString()
                        });
                    }
                }
            }
            return list;
        }
    }
}
