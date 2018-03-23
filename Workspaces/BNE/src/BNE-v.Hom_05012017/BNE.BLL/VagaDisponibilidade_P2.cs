//-- Data: 18/11/2010 13:23
//-- Autor: Vinicius Maciel

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class VagaDisponibilidade // Tabela: BNE_Vaga_Disponibilidade
    {

        #region Consultas
        private const string SPDELETEPORVAGA = "DELETE FROM BNE_Vaga_Disponibilidade WHERE Idf_Vaga = @Idf_Vaga";
        private const string Spselectporvaga = @"SELECT * FROM BNE_Vaga_Disponibilidade vd WITH (NOLOCK)
                                                    JOIN BNE.Tab_Disponibilidade d WITH(NOLOCK) ON vd.Idf_Disponibilidade = d.Idf_Disponibilidade
                                                     WHERE Idf_Vaga = @Idf_Vaga";
        private const string Spselectidsdisponibilidadeporvaga = @"SELECT Idf_Disponibilidade FROM BNE_Vaga_Disponibilidade WITH (NOLOCK) WHERE Idf_Vaga = @Idf_Vaga";
        #endregion

        #region Métodos

        #region DeletePorVaga
        /// <summary>
        /// Método utilizado para excluir todas as VagaDisponibilidade ligadas a uma vaga dentro de uma transação
        /// </summary>
        /// <param name="idVaga">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson</remarks>
        public static void DeletePorVaga(int idVaga, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4));

            parms[0].Value = idVaga;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETEPORVAGA, parms);
        }
        #endregion

        #region ListarDisponibilidadesPorVaga
        public static List<int> ListarIdentificadoresDisponibilidadePorVaga(Vaga objVaga)
        {
            var lista = new List<int>();

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = objVaga.IdVaga }
                };

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectidsdisponibilidadeporvaga, parms))
            {
                while (dr.Read())
                    lista.Add(Convert.ToInt32(dr["Idf_Disponibilidade"]));
            }

            return lista;
        }
        #endregion

        #region ListarDisponibilidadesPorVaga
        public static List<VagaDisponibilidade> ListarDisponibilidadesPorVaga(Vaga objVaga)
        {
            var listVagaDisponibilidade = new List<VagaDisponibilidade>();

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = objVaga.IdVaga }
                };

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectporvaga, parms))
            {
                while (dr.Read())
                {
                    var objVagaDisponibilidade = new VagaDisponibilidade();
                    objVagaDisponibilidade._idVagaDisponibilidade = Convert.ToInt32(dr["Idf_Vaga_Disponibilidade"]);
                    objVagaDisponibilidade._disponibilidade = new Disponibilidade(Convert.ToInt32(dr["Idf_Disponibilidade"]));
                    if (dr["Des_Disponibilidade"] != null)
                        objVagaDisponibilidade._disponibilidade.DescricaoDisponibilidade = dr["Des_Disponibilidade"].ToString();
                    objVagaDisponibilidade._vaga = new Vaga(Convert.ToInt32(dr["Idf_Vaga"]));
                    objVagaDisponibilidade._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

                    objVagaDisponibilidade._persisted = true;
                    objVagaDisponibilidade._modified = false;
                    listVagaDisponibilidade.Add(objVagaDisponibilidade);
                }
            }

            return listVagaDisponibilidade;
        }
        #endregion

        #endregion

    }
}