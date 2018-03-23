//-- Data: 27/01/2011 11:34
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Configuration;

namespace BNE.BLL
{
    public partial class Usuario // Tabela: BNE_Usuario
    {

        #region Consultas

        #region SpSelectPorPessoaFisica
        private const string SpSelectPorPessoaFisica = @"
        SELECT  *
        FROM    BNE_Usuario WITH(NOLOCK)
        WHERE   Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica
                AND Flg_Inativo = 0";
        #endregion

        #region Sprecuperarusuariosfilial
        private const string Sprecuperarusuariosfilial = @"
        SELECT	U.Idf_Usuario, U.Idf_Pessoa_Fisica, U.Dta_Ultima_Atividade, U.Des_Session_Id
        FROM	TAB_Pessoa_Fisica PF WITH(NOLOCK)
		        INNER JOIN BNE_Usuario U WITH(NOLOCK) ON PF.Idf_Pessoa_Fisica = U.Idf_Pessoa_Fisica
		        INNER JOIN TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) ON PF.Idf_Pessoa_Fisica = UFP.Idf_Pessoa_Fisica
        WHERE	UFP.Idf_Filial = @Idf_Filial
		        AND PF.Flg_Inativo = 0
		        AND UFP.Flg_Inativo = 0
		        AND U.Flg_Inativo = 0
        ";
        #endregion

        #endregion

        #region Métodos

        #region CarregarPorPessoaFisica
        /// <summary>
        /// Método responsável por carregar uma instancia de Usuario através do
        /// identificar de uma pessoa física.
        /// </summary>
        /// <param name="idPessoaFisica">Identificador da Pessoa Física</param>
        /// <param name="objUsuario"> </param>
        /// <returns>Boolean</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorPessoaFisica(int idPessoaFisica, out Usuario objUsuario, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Pessoa_Fisica", SqlDbType = SqlDbType.Int, Size = 4, Value = idPessoaFisica }
                };

            using (IDataReader dr = trans == null ? DataAccessLayer.ExecuteReader(CommandType.Text, SpSelectPorPessoaFisica, parms)
                                                : DataAccessLayer.ExecuteReader(trans, CommandType.Text, SpSelectPorPessoaFisica, parms))
            {
                objUsuario = new Usuario();
                if (SetInstance(dr, objUsuario))
                    return true;

                if (!dr.IsClosed)
                    dr.Close();
            }
            objUsuario = null;
            return false;
        }
        #endregion

        #region UsuarioPodeLogarFilial
        /// <summary>
        /// Implementa da regra para saber se um usuário pode logar-se em determinada filial
        /// </summary>
        /// <param name="objFilial"></param>
        /// <param name="objPessoaFisica"></param>
        /// <returns></returns>
        public static bool UsuarioPodeLogarFilial(Filial objFilial, PessoaFisica objPessoaFisica)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Pessoa_Fisica", SqlDbType = SqlDbType.Int, Size = 4, Value = objPessoaFisica.IdPessoaFisica },
                    new SqlParameter{ ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = objFilial.IdFilial }
                };

            var lista = new List<Usuario>();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Sprecuperarusuariosfilial, parms))
            {
                while (dr.Read())
                {
                    lista.Add(new Usuario(Convert.ToInt32(dr["Idf_Usuario"])) { DataUltimaAtividade = dr["Dta_Ultima_Atividade"] != DBNull.Value ? Convert.ToDateTime(dr["Dta_Ultima_Atividade"]) : (DateTime?)null, PessoaFisica = new PessoaFisica(Convert.ToInt32(dr["Idf_Pessoa_Fisica"])), DescricaoSessionID = dr["Des_Session_Id"] != DBNull.Value ? dr["Des_Session_Id"].ToString() : string.Empty });
                }
            }

            if (lista.Count.Equals(0))
                return false;

            var sessionState = (SessionStateSection)ConfigurationManager.GetSection("system.web/sessionState");
            var listaLogados = lista.Where(u => u.DataUltimaAtividade != null && u.DataUltimaAtividade.Value.AddTicks(sessionState.Timeout.Ticks) > DateTime.Now).ToList();

            if (listaLogados.Count(lg => lg.PessoaFisica.IdPessoaFisica == objPessoaFisica.IdPessoaFisica) > 0)
                return true;

            var quantidadeAcessosAdquiridos = objFilial.RecuperarQuantidadeAcessosAdquiridos();

            return listaLogados.Count() < quantidadeAcessosAdquiridos;
        }
        #endregion

        #endregion
    }
}