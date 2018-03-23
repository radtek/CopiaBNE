//-- Data: 18/09/2013 15:11
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class CelularSelecionador // Tabela: BNE_Celular_Selecionador
    {

        #region Consultas

        #region Spverificacelularliberadoporimei
        private const string Spverificacelularliberadoporimei = @"
        SELECT  COUNT(*)
        FROM    BNE_Celular C WITH (NOLOCK)
                INNER JOIN BNE_Celular_Selecionador CS WITH (NOLOCK) ON C.Idf_Celular = CS.Idf_Celular
        WHERE   C.Cod_Imei_Celular = @imei
		        AND CONVERT(VARCHAR, CS.Dta_Inicio_Utilizacao, 112) <= CONVERT(VARCHAR, GETDATE(), 112)
		        AND (CONVERT(VARCHAR, CS.Dta_Fim_Utilizacao, 112) IS NULL OR CONVERT(VARCHAR, CS.Dta_Fim_Utilizacao, 112) >= CONVERT(VARCHAR, GETDATE(), 112))";
        #endregion

        #region Spverificacelularliberadoporselecionador
        private const string Spverificacelularliberadoporselecionadorgeral = @"
        SELECT  COUNT(*)
        FROM    BNE_Celular C WITH (NOLOCK)
                INNER JOIN BNE_Celular_Selecionador CS WITH (NOLOCK) ON C.Idf_Celular = CS.Idf_Celular
        WHERE   CS.Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil
		        AND CONVERT(VARCHAR, CS.Dta_Inicio_Utilizacao, 112) <= CONVERT(VARCHAR, GETDATE(), 112)
		        AND (CONVERT(VARCHAR, CS.Dta_Fim_Utilizacao, 112) IS NULL OR CONVERT(VARCHAR, CS.Dta_Fim_Utilizacao, 112) >= CONVERT(VARCHAR, GETDATE(), 112))";

        private const string Spverificacelularliberadoporselecionadorparatanque = @"
        SELECT  COUNT(*)
        FROM    BNE_Celular C WITH (NOLOCK)
                INNER JOIN BNE_Celular_Selecionador CS WITH (NOLOCK) ON C.Idf_Celular = CS.Idf_Celular
        WHERE   CS.Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil
		        AND CONVERT(VARCHAR, CS.Dta_Inicio_Utilizacao, 112) <= CONVERT(VARCHAR, GETDATE(), 112)
		        AND (CONVERT(VARCHAR, CS.Dta_Fim_Utilizacao, 112) IS NULL OR CONVERT(VARCHAR, CS.Dta_Fim_Utilizacao, 112) >= CONVERT(VARCHAR, GETDATE(), 112))
                AND CS.Flg_Utiliza_Servico_Tanque = 1";
        #endregion

        #region Sprecuperarcelularselecionadorliberadoporimei
        private const string Sprecuperarcelularselecionadorliberadoporimei = @"
        SELECT  CS.*
        FROM    BNE_Celular C WITH (NOLOCK)
                INNER JOIN BNE_Celular_Selecionador CS WITH (NOLOCK) ON C.Idf_Celular = CS.Idf_Celular
        WHERE   C.Cod_Imei_Celular = @imei
		        AND CONVERT(VARCHAR, CS.Dta_Inicio_Utilizacao, 112) <= CONVERT(VARCHAR, GETDATE(), 112)
		        AND (CONVERT(VARCHAR, CS.Dta_Fim_Utilizacao, 112) IS NULL OR CONVERT(VARCHAR, CS.Dta_Fim_Utilizacao, 112) >= CONVERT(VARCHAR, GETDATE(), 112))";
        #endregion

        #region Sprecuperarcelularselecionadorliberadoporselecionador
        private const string Sprecuperarcelularselecionadorliberadoporselecionador = @"
        SELECT  CS.*
        FROM    BNE_Celular C WITH (NOLOCK)
                INNER JOIN BNE_Celular_Selecionador CS WITH (NOLOCK) ON C.Idf_Celular = CS.Idf_Celular
        WHERE   CS.Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil
		        AND CONVERT(VARCHAR, CS.Dta_Inicio_Utilizacao, 112) <= CONVERT(VARCHAR, GETDATE(), 112)
		        AND (CONVERT(VARCHAR, CS.Dta_Fim_Utilizacao, 112) IS NULL OR CONVERT(VARCHAR, CS.Dta_Fim_Utilizacao, 112) >= CONVERT(VARCHAR, GETDATE(), 112))";
        #endregion

        #region SpRecuperarCelularSelecionadorTanque
        private const string SpRecuperarCelularSelecionadorTanque = @"
        SELECT  CS.*
        FROM    BNE_Celular_Selecionador CS WITH (NOLOCK)
        WHERE   CS.Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil
		        AND CS.Flg_Utiliza_Servico_Tanque = 1";
        #endregion

        #region Sprecuperarnomeselecionador
        private const string Sprecuperarnomeselecionador = @"
        SELECT  PF.Nme_Pessoa
        FROM    BNE_Celular_Selecionador CS WITH (NOLOCK)
                INNER JOIN TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) ON CS.Idf_Usuario_Filial_Perfil = UFP.Idf_Usuario_Filial_Perfil
                INNER JOIN TAB_Pessoa_Fisica PF WITH(NOLOCK) ON UFP.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
        WHERE   CS.Idf_Celular_Selecionador = @Idf_Celular_Selecionador";
        #endregion

        #region Sprecuperartokenselecionador
        private const string Sprecuperartokenselecionador = @"
        SELECT  C.Cod_Token_Celular
        FROM    BNE_Celular_Selecionador CS WITH (NOLOCK)
                INNER JOIN BNE_Celular C WITH(NOLOCK) ON CS.Idf_Celular = C.Idf_Celular
        WHERE   CS.Idf_Celular_Selecionador = @Idf_Celular_Selecionador";
        #endregion

        #region SpRecuperarCelularSelecionador
        private const string SpRecuperarCelularSelecionador = @"
        SELECT  TOP 1 CS.*
        FROM    BNE_Celular_Selecionador CS WITH (NOLOCK)
        WHERE   CS.Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil
		        AND CS.Flg_Utiliza_Servico_Tanque = 1
                AND (CS.Dta_Fim_Utilizacao IS NULL OR CS.Dta_Fim_Utilizacao > GetDate())
        ";
        #endregion

        #endregion

        #region VerificaCelularEstaLiberado
        /// <summary>
        /// Valida se este IMEI está cadastrado na base
        /// </summary>
        /// <param name="imei"></param>
        public static bool VerificaCelularEstaLiberado(string imei)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@imei", SqlDbType = SqlDbType.VarChar, Size = 200, Value = imei }
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spverificacelularliberadoporimei, parms)) > 0;
        }
        public static bool VerificaCelularEstaLiberado(UsuarioFilialPerfil objUsuarioFilialPerfil)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = objUsuarioFilialPerfil.IdUsuarioFilialPerfil }
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spverificacelularliberadoporselecionadorgeral, parms)) > 0;
        }

        public static bool VerificaCelularEstaLiberadoParaTanque(UsuarioFilialPerfil objUsuarioFilialPerfil)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = objUsuarioFilialPerfil.IdUsuarioFilialPerfil }
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spverificacelularliberadoporselecionadorparatanque, parms)) > 0;
        }
        #endregion

        #region RecuperarNomeSelecionador
        /// <summary>
        /// Recupera o nome do usuário filial ligado a um celular selecionador
        /// </summary>
        /// <returns></returns>
        public string RecuperarNomeSelecionador()
	    {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Celular_Selecionador", SqlDbType = SqlDbType.Int, Size = 4, Value = _idCelularSelecionador }
                };

            return Convert.ToString(DataAccessLayer.ExecuteScalar(CommandType.Text, Sprecuperarnomeselecionador, parms));
	    }
        #endregion

        #region RecuperarToken
        /// <summary>
        /// Recupera o nome do usuário filial ligado a um celular selecionador
        /// </summary>
        /// <returns></returns>
        public string RecuperarToken()
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Celular_Selecionador", SqlDbType = SqlDbType.Int, Size = 4, Value = _idCelularSelecionador }
                };

            return Convert.ToString(DataAccessLayer.ExecuteScalar(CommandType.Text, Sprecuperartokenselecionador, parms));
        }
        #endregion

        #region RecuperarCelularSelecionador
        /// <summary>
        /// Retorna uma instancia liberada de celular de selecionador através de seu IMEI
        /// </summary>
        /// <param name="imei"></param>
        /// <returns></returns>
        public static CelularSelecionador RecuperarCelularSelecionador(string imei)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@imei", SqlDbType = SqlDbType.VarChar, Size = 200, Value = imei }
                };

            var objCelularSelecionador = new CelularSelecionador();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Sprecuperarcelularselecionadorliberadoporimei, parms))
            {
                if (!SetInstance(dr, objCelularSelecionador))
                    objCelularSelecionador = null;
            }

            return objCelularSelecionador;
        }
        public static CelularSelecionador RecuperarCelularSelecionador(int IdUsuarioFilialPerfil)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = IdUsuarioFilialPerfil }
                };

            var objCelularSelecionador = new CelularSelecionador();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Sprecuperarcelularselecionadorliberadoporselecionador, parms))
            {
                if (!SetInstance(dr, objCelularSelecionador))
                    objCelularSelecionador = null;
            }

            return objCelularSelecionador;
        }

        public static CelularSelecionador RecuperarCelularSelecionadorTanque(int IdUsuarioFilialPerfil)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = IdUsuarioFilialPerfil }
                };

            var objCelularSelecionador = new CelularSelecionador();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpRecuperarCelularSelecionadorTanque, parms))
            {
                if (!SetInstance(dr, objCelularSelecionador))
                    objCelularSelecionador = null;
            }

            return objCelularSelecionador;
        }

        

        #endregion

        #region RecuperarCelularSelecionadorByCodigo
        public static CelularSelecionador RecuperarCelularSelecionadorByCodigo(int UsuarioFilialPerfil)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 8, Value = UsuarioFilialPerfil }
                };

            var objCelularSelecionador = new CelularSelecionador();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpRecuperarCelularSelecionador, parms))
            {
                if (!SetInstance(dr, objCelularSelecionador))
                    objCelularSelecionador = null;
            }

            return objCelularSelecionador;
        }
        #endregion RecuperarCelularSelecionador
    }
}