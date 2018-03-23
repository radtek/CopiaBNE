//-- Data: 24/06/2013 16:17
//-- Autor: Gieyson Stelmak


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
    public partial class ConsultorR1 // Tabela: BNE_Consultor_R1
    {

        #region Consultas

        #region Spconsultorporcidade
        private const string Spconsultorporcidade = @"SELECT C.* FROM BNE_Consultor_R1 C WITH(NOLOCK) INNER JOIN BNE_Consultor_Cidade_R1 CC WITH(NOLOCK) ON C.Idf_Consultor_R1 = CC.Idf_Consultor_R1 WHERE CC.Idf_Cidade = @Idf_Cidade";
        #endregion

        #endregion

        #region RecuperarConsultorR1
        public static ConsultorR1 RecuperarConsultorR1(Cidade objCidade, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Cidade", SqlDbType = SqlDbType.Int, Size = 4, Value = objCidade.IdCidade }
                };

            var listaConsultores = new List<ConsultorR1>();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spconsultorporcidade, parms))
            {
                while (dr.Read())
                {
                    var objConsultorR1 = new ConsultorR1();
                    if (SetInstanceNonDispose(dr, objConsultorR1))
                        listaConsultores.Add(objConsultorR1);
                }
            }
            if (listaConsultores.Count.Equals(0))
                return null;

            if (listaConsultores.Count.Equals(1))
                return listaConsultores[0];

            var random = new Random((int)DateTime.Now.Ticks);
            var posicao = random.Next(0, listaConsultores.Count);
            return listaConsultores[posicao];
        }
        #endregion

        #region SetInstance
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objConsultorR1">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstanceNonDispose(IDataReader dr, ConsultorR1 objConsultorR1)
        {
            objConsultorR1._idConsultorR1 = Convert.ToInt32(dr["Idf_Consultor_R1"]);
            objConsultorR1._nomeConsultor = Convert.ToString(dr["Nme_Consultor"]);
            objConsultorR1._descricaoEmail = Convert.ToString(dr["Des_Email"]);

            objConsultorR1._persisted = true;
            objConsultorR1._modified = false;

            return true;
        }
        #endregion

    }
}