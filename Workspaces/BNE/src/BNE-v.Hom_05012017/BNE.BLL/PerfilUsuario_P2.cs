//-- Data: 26/03/2010 09:18
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;
namespace BNE.BLL
{
    [Serializable]
    public partial class PerfilUsuario // Tabela: TAB_Perfil_Usuario
    {
        #region Consultas
        private const string SPVERIFICAPERFILUSUARIO = @"   SELECT  COUNT(PU.Idf_Perfil_Usuario)
                                                            FROM    TAB_Perfil_Usuario PU
                                                            WHERE   PU.Idf_Perfil = @Idf_Perfil
                                                                    AND PU.Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial";

        private const string SPSELECTPERFILUSUARIO = @"   SELECT  *
                                                            FROM    TAB_Perfil_Usuario PU
                                                            WHERE   PU.Idf_Perfil = @Idf_Perfil
                                                                    AND PU.Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial";
        private const string SPSELECTPERFILUSUARIOPORUSUARIO = @"SELECT  PU.idf_perfil_usuario ,
                                                                        P.Des_Perfil ,
                                                                        F.Nme_Fantasia
                                                                FROM    BNE.TAB_Perfil_Usuario PU
                                                                        JOIN TAB_Perfil P ON PU.Idf_Perfil = P.Idf_Perfil
                                                                        JOIN TAB_Usuario_Filial_Perfil UFP ON PU.Idf_Usuario_Filial_Perfil = UFP.Idf_Usuario_Filial_Perfil
                                                                        JOIN TAB_Filial F ON UFP.Idf_Filial = F.Idf_Filial
                                                                        JOIN BNE_Usuario U ON UFP.Idf_Usuario = U.Idf_Usuario
                                                                WHERE   U.Idf_Usuario = @Idf_Usuario";
        private const string SPFLAGPERFILUSUARIO = @"SELECT Flg_Inativo FROM BNE.TAB_Perfil_Usuario WHERE idf_perfil_usuario = @idf_perfil_usuario";


        #region SELPORUSUARIOFILIALPERFIL

        private const string SELPORUSUARIOFILIALPERFIL = @"
                                                            SELECT
                                                                *
	                                                        FROM TAB_Perfil_Usuario
	                                                        WHERE 
                                                                Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil
	                                                            AND Flg_Inativo = 0";

        #endregion


        #endregion

        #region Métodos

        #region VerificarPerfilUsuario
        /// <summary>
        /// Método responsavel por verificar se o usuario de determinada filial possui determinado perfil
        /// </summary>
        /// <param name="idUsuarioFilial"></param>
        /// <param name="idPerfil"></param>
        public static bool VerificarPerfilUsuario(int idUsuarioFilial, int idPerfil)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Usuario_Filial", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Perfil", SqlDbType.Int, 4));
            parms[0].Value = idUsuarioFilial;
            parms[1].Value = idPerfil;

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPVERIFICAPERFILUSUARIO, parms)) > 0;
        }

        #endregion

        #region CarregarPorPerfilUsuario
        /// <summary>
        /// Método responsavel por carregar uma instancia de PerfilUsuario, passando o Código Identificado de um Perfil e o código Identificador de uma Pessoa Fisica
        /// </summary>
        /// <param name="idUsuarioFilial"></param>
        /// <param name="idPerfil"></param>
        public static bool CarregarPorPerfilUsuario(int idUsuarioFilial, int idPerfil, out PerfilUsuario objPerfilUsuario, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Usuario_Filial", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Perfil", SqlDbType.Int, 4));
            parms[0].Value = idUsuarioFilial;
            parms[1].Value = idPerfil;

            IDataReader dr;

            if (trans != null)
                dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPSELECTPERFILUSUARIO, parms);
            else
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPERFILUSUARIO, parms);

            objPerfilUsuario = new PerfilUsuario();
            if (SetInstance(dr, objPerfilUsuario))
                return true;

            if (!dr.IsClosed)
                dr.Close();

            dr.Dispose();

            objPerfilUsuario = null;
            return false;
        }
        /// <summary>
        /// Método responsavel por carregar uma instancia de PerfilUsuario, passando o Código Identificado de um Perfil e o código Identificador de uma Pessoa Fisica
        /// </summary>
        /// <param name="idUsuarioFilial"></param>
        /// <param name="idPerfil"></param>
        /// <param name="trans">Transação, pode passar como NULL</param>
        public static PerfilUsuario CarregarPorPerfilUsuario(int idUsuarioFilial, int idPerfil, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Usuario_Filial", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Perfil", SqlDbType.Int, 4));
            parms[0].Value = idUsuarioFilial;
            parms[1].Value = idPerfil;

            IDataReader dr;

            if (trans != null)
                dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPSELECTPERFILUSUARIO, parms);
            else
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPERFILUSUARIO, parms);

            PerfilUsuario objPerfilUsuario = new PerfilUsuario();
            if (SetInstance(dr, objPerfilUsuario))
                return objPerfilUsuario;

            if (!dr.IsClosed)
                dr.Close();

            dr.Dispose();

            throw (new RecordNotFoundException(typeof(PerfilUsuario)));
        }

        #endregion

        #region ListarUsuarioFilialPerfil
        /// <summary>
        /// Carrega todos os "PerfilUsuario" de um determinado "UsuarioFilialPerfil"
        /// que estejam ativos.
        /// </summary>
        /// <param name="pIdUsuarioFilialPerfil">Identificador do usuario filial perfil</param>
        /// <returns>Lista com os "PerfilUsuario" ativos</returns>
        /// <remarks>Renan Prado</remarks>
        public static List<PerfilUsuario> ListarUsuarioFilialPerfil(int pIdUsuarioFilialPerfil)
        {
            #region Variáveis

            List<PerfilUsuario> lst = new List<PerfilUsuario>();
            List<SqlParameter> parms = new List<SqlParameter>();

            #endregion

            #region Filtros

            parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int));
            parms[0].Value = pIdUsuarioFilialPerfil;

            #endregion

            using (IDataReader iDr = DataAccessLayer.ExecuteReader(CommandType.Text, SELPORUSUARIOFILIALPERFIL, parms))
            {
                while (iDr.Read())
                {
                    PerfilUsuario objPerfilUsuario = new PerfilUsuario();
                    objPerfilUsuario._idfperfilusuario = Convert.ToInt32(iDr["idf_perfil_usuario"]);
                    objPerfilUsuario._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(iDr["Idf_Usuario_Filial_Perfil"]));
                    objPerfilUsuario._flagInativo = Convert.ToBoolean(iDr["Flg_Inativo"]);
                    objPerfilUsuario._perfil = new Perfil(Convert.ToInt32(iDr["Idf_Perfil"]));
                    objPerfilUsuario._dataCadastro = Convert.ToDateTime(iDr["Dta_Cadastro"]);
                    objPerfilUsuario._dataInicio = Convert.ToDateTime(iDr["Dta_Inicio"]);
                    if (iDr["Dta_Fim"] != DBNull.Value)
                        objPerfilUsuario._dataFim = Convert.ToDateTime(iDr["Dta_Fim"]);

                    objPerfilUsuario._persisted = true;
                    objPerfilUsuario._modified = false;
                    
                    lst.Add(objPerfilUsuario);
                }
            }

            return lst;
        }

        #endregion

        #region ListarPerfilUsuario
        /// <summary>
        /// Seleciona o PerfilUsuario por usuário
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns>IDataReader</returns>
        public static IDataReader ListarPerfilUsuario(int idUsuario)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Usuario", SqlDbType.Int, 4));
            parms[0].Value = idUsuario;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPERFILUSUARIOPORUSUARIO, parms);
        }

        #endregion

        #region CarregarFlagPerfilUsuario

        public static DataTable CarregarFlagPerfilUsuario(int idPerfilUsuario)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@idf_perfil_usuario", SqlDbType.Int, 4));
            parms[0].Value = idPerfilUsuario;

            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, SPFLAGPERFILUSUARIO, parms).Tables[0];
        }

        #endregion

        #endregion

    }
}