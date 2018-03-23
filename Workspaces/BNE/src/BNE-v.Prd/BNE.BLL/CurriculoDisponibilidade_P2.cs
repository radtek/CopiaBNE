//-- Data: 06/04/2011 17:26
//-- Autor: Vinicius Maciel

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BNE.BLL
{
    public partial class CurriculoDisponibilidade // Tabela: BNE_Curriculo_Disponibilidade
    {
        #region Querys

        private const string LISTARPORCURRICULO = @"SELECT * FROM BNE_Curriculo_Disponibilidade WHERE Idf_Curriculo = @Idf_Curriculo";

        private const string DELETEPORCURRICULO = @"DELETE BNE_Curriculo_Disponibilidade WHERE Idf_Curriculo = @Idf_Curriculo";

        private const string ATULIZAR_LISTA_CURRICULO = @"
            DECLARE @Idf_Disponibilidade INT
            DECLARE @handle INT
            EXEC sp_xml_preparedocument @handle OUTPUT, @Xml

            -- Deletando disponibilidades
            DELETE FROM BNE.BNE_Curriculo_Disponibilidade 
            WHERE Idf_Curriculo = @Idf_Curriculo
            AND Idf_Disponibilidade NOT IN (SELECT * FROM OPENXML (@handle, '/ArrayOfInt/int') WITH (id INT '.'))

            -- Incluindo disponibilidades
            DECLARE incluir_cursor CURSOR  
                FOR SELECT id FROM OPENXML (@handle, '/ArrayOfInt/int') WITH (id INT '.')
			            WHERE NOT EXISTS(SELECT 1 FROM BNE.BNE_Curriculo_Disponibilidade 
								            WHERE Idf_Curriculo = @Idf_Curriculo AND
								            Idf_Disponibilidade = id)

            OPEN incluir_cursor  
            FETCH NEXT FROM incluir_cursor 
	            INTO @Idf_Disponibilidade

            WHILE @@FETCH_STATUS = 0  
            BEGIN 
	            INSERT INTO BNE.BNE_Curriculo_Disponibilidade
	                    ( Idf_Curriculo ,
	                      Idf_Disponibilidade
	                    )
	            VALUES  ( @Idf_Curriculo , -- Idf_Curriculo - int
	                      @Idf_Disponibilidade  -- Idf_Disponibilidade - int
	                    )

	            FETCH NEXT FROM incluir_cursor 
	            INTO @Idf_Disponibilidade
            END

            CLOSE incluir_cursor;  
            DEALLOCATE incluir_cursor;  

            EXEC sp_xml_removedocument @handle";

        #endregion

        #region Metodos

        #region Listar
        /// <summary>
        /// Lista CurriculoDisponibilidade por Curriculo
        /// </summary>
        /// <param name="idCurriculo"></param>
        /// <returns></returns>
        public static List<CurriculoDisponibilidade> Listar(int idCurriculo)
        {
            //TODO: Melhorar código, tirar load object interno
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
            parms[0].Value = idCurriculo;

            List<CurriculoDisponibilidade> lstCurriculoDisponibilidade = new List<CurriculoDisponibilidade>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, LISTARPORCURRICULO, parms))
            {
                while (dr.Read())
                    lstCurriculoDisponibilidade.Add(LoadObject(Convert.ToInt32(dr["Idf_Curriculo_Disponibilidade"])));

                if (!dr.IsClosed)
                    dr.Dispose();

                dr.Close();
            }


            return lstCurriculoDisponibilidade;
        }

        #endregion

        #region DeletePorCurriculo
        /// <summary>
        /// Deleta Curriculo disponibilidade por id curriculo na transação
        /// </summary>
        /// <param name="idCurriculo"></param>
        /// <param name="trans"></param>
        public static void DeletePorCurriculo(int idCurriculo, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
            parms[0].Value = idCurriculo;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, DELETEPORCURRICULO, parms);
        }

        #endregion

        #region AtualizaCurriculo

        /// <summary>
        /// Atualiza as disponibilidades por curriculo
        /// </summary>
        /// <param name="idCurriculo"></param>
        /// <param name="lstDisponibilidades"></param>
        /// <param name="trans"></param>
        public static void AtualizaCurriculo(int idCurriculo, List<CurriculoDisponibilidade> lstDisponibilidades, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Xml", SqlDbType.Xml));
            parms[0].Value = idCurriculo;

            XmlSerializer xs = new XmlSerializer(typeof(List<int>));
            MemoryStream ms = new MemoryStream();
            xs.Serialize(ms, lstDisponibilidades.Select(dc => dc.Disponibilidade.IdDisponibilidade).ToList());
            string resultXML = UTF8Encoding.UTF8.GetString(ms.ToArray());

            parms[1].Value = resultXML;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, ATULIZAR_LISTA_CURRICULO, parms);
        }

        #endregion

        #endregion
    }
}