using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.BLL.Notificacao.DTO;

namespace BNE.BLL.Notificacao
{
    public class JornalVagasService
    {
        /// <summary>
        ///     Le uma tabela do banco e retorna os Jornal de Vagas que estão aguardando serem processados
        /// </summary>
        /// <returns>IEnumerable de ProcessamentoJornalVagas</returns>
        public IEnumerable<ProcessamentoJornalVagas> GetAllToProcess()
        {
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, "JornalVaga_RecuperarAguardandoProcessamento", null))
            {
                while (dr.Read())
                {
                    var objProcessamentoJornalVagas = new ProcessamentoJornalVagas();
                    objProcessamentoJornalVagas.SetInstance(dr);
                    yield return objProcessamentoJornalVagas;
                }
            }
        }

        public void AtualizarDataInicio(ProcessamentoJornalVagas objProcessamentoJornalVagas)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Processamento_Jornal_Vagas", SqlDbType = SqlDbType.Int, Size = 4, Value = objProcessamentoJornalVagas.IdProcessamentoJornalVagas},
                new SqlParameter {ParameterName = "@Data", SqlDbType = SqlDbType.DateTime, Value = objProcessamentoJornalVagas.DataInicioProcessamento}
            };
            DataAccessLayer.ExecuteNonQuery(CommandType.StoredProcedure, "JornalVaga_AtualizarDataInicioProcessamento", parms);
        }

        public void AtualizarDataFim(ProcessamentoJornalVagas objProcessamentoJornalVagas)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Processamento_Jornal_Vagas", SqlDbType = SqlDbType.Int, Size = 4, Value = objProcessamentoJornalVagas.IdProcessamentoJornalVagas},
                new SqlParameter {ParameterName = "@Data", SqlDbType = SqlDbType.DateTime, Value = objProcessamentoJornalVagas.DataFimProcessamento}
            };
            DataAccessLayer.ExecuteNonQuery(CommandType.StoredProcedure, "JornalVaga_AtualizarDataFimProcessamento", parms);
        }

        public List<Vaga> RecuperarVagas(ProcessamentoJornalVagas objProcessamentoJornalVagas)
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

        public List<Curriculo> RecuperarCurriculos(ProcessamentoJornalVagas objProcessamentoJornalVagas)
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
                        VIP = Convert.ToBoolean(dr["Flg_VIP"]),
                        CPF = Convert.ToDecimal(dr["Num_CPF"]),
                        DataNascimento = Convert.ToDateTime(dr["Dta_Nascimento"])
                    });
                }
            }
            return list;
        }
    }
}