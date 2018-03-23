//-- Data: 13/09/2016 22:34
//-- Autor: Francisco Ribas

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace BNE.BLL
{
	public partial class VagaCurso // Tabela: BNE_Vaga_Curso
	{
        #region Consultas

        private const string Spdeleteporvaga   = "DELETE FROM BNE.BNE_Vaga_Curso WHERE Idf_Vaga = @Idf_Vaga";
        private const string Spselectidporvaga = @"SELECT *, c.Des_Curso AS DescricaoCursoCadastrado 
                                                    FROM BNE.BNE_Vaga_Curso vc
                                                    LEFT JOIN BNE.TAB_Curso c ON vc.Idf_Curso = c.Idf_Curso
                                                    WHERE Idf_Vaga = @Idf_Vaga";

        #endregion Consultas

        /// <summary>
        /// Metodo para instanciar um array de VagaCurso, baseada em uma lista de descrições.
        /// </summary>
        /// <param name="cursos"></param>
        /// <returns></returns>
        /// <remarks>Francisco Ribas</remarks>
        public static List<VagaCurso> ResolverVagaCurso(List<string> cursos, Vaga objVaga)
        {
            var listaVagaCurso = new List<VagaCurso>();
            foreach (var nomeCurso in cursos)
            {
                if (!string.IsNullOrEmpty(nomeCurso))
                {
                    var objVagaCurso = new VagaCurso() { Vaga = objVaga };
                    BLL.Curso objCurso;

                    // Se o curso é encontrado através do nome informado, define o curso como relacionamento
                    // Senão, considera o próprio nome de curso é considerado na descrição
                    if (BLL.Curso.CarregarPorNome(nomeCurso, out objCurso))
                        objVagaCurso.Curso = objCurso;
                    else
                        objVagaCurso.DescricaoCurso = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(nomeCurso.ToLower());

                    listaVagaCurso.Add(objVagaCurso);
                }
            }
            return listaVagaCurso;
        }

        #region DeletePorVaga
        /// <summary>
        /// Método utilizado para excluir todas as VagaCurso ligadas a uma vaga dentro de uma transação
        /// </summary>
        /// <param name="idVaga">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Francisco Ribas</remarks>
        public static void DeletePorVaga(int idVaga, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = idVaga }
                };

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, Spdeleteporvaga, parms);
        }
        #endregion

        #region ListarCursosPorVaga
        /// <summary>
        /// Método responsável por retornar uma List com todas as instâncias de VagaCurso 
        /// </summary>
        /// <param name="idVaga">Código identificador de uma vaga</param>
        /// <returns></returns>
        public static List<VagaCurso> ListarCursoPorVaga(int idVaga, SqlTransaction trans = null)
        {
            var listVagaCurso = new List<VagaCurso>();

            using (var dr = ListarIDPorVaga(idVaga, trans))
            {
                var objVagaVagaCurso = new VagaCurso();
                while (SetInstance(dr, objVagaVagaCurso, false))
                {
                    listVagaCurso.Add(objVagaVagaCurso);
                    objVagaVagaCurso = new VagaCurso();
                }

                if (!dr.IsClosed)
                    dr.Close();
            }

            return listVagaCurso;
        }
        /// <summary>
        /// Método responsável por retornar uma IDataReader com todas os ids de VagaTipoVinculo
        /// </summary>
        /// <param name="idVaga">Código identificador de uma vaga</param>
        /// <returns></returns>
        private static IDataReader ListarIDPorVaga(int idVaga, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = idVaga }
                };

            return trans == null ? DataAccessLayer.ExecuteReader(CommandType.Text, Spselectidporvaga, parms)
                : DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectidporvaga, parms);
        }
        #endregion

        #region [ListaCursoParaCartaEmail]
        /// <summary>
        /// Retorna a string de cursos para enviar nos e-mails.
        /// </summary>
        /// <param name="IdVaga"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static string ListaCursoParaCartaEmail(string Funcao, int IdVaga, SqlTransaction trans = null)
        {
            List<VagaCurso> listaCurso = ListarCursoPorVaga(IdVaga, trans);
            var retorno = Funcao;
          
            if(listaCurso.Count > 0)
            {
                List<string> lista = new List<string>();
                foreach (var item in listaCurso)
                    lista.Add(item.Curso != null ? item.Curso.DescricaoCurso : item.DescricaoCurso);

                retorno = $"estágio em {string.Join(",", lista)}";
            }
         

            return retorno;
        }
        #endregion
    }
}