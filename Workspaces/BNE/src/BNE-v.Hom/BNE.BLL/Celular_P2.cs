//-- Data: 18/09/2013 15:11
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class Celular // Tabela: BNE_Celular
    {

        #region Consultas

        #region SpverificaIMEI
        private const string SpverificaIMEI = "SELECT COUNT(*) FROM BNE_Celular WITH(NOLOCK) WHERE Cod_Imei_Celular = @imei";
        #endregion

        #region Spatualizatoken
        private const string Spatualizatoken = "UPDATE BNE_Celular SET Cod_Token_Celular = @Token WHERE Idf_Celular = @Idf_Celular";
        #endregion
        
        #endregion

        //#region Construtores
        //public Celular(string imei)
        //{
        //    _codigoImeiCelular = imei;
        //}
        //#endregion

        #region Métodos

        #region VerificaIMEIEstaCadastrado
        /// <summary>
        /// Valida se este IMEI está cadastrado na base
        /// </summary>
        /// <param name="imei"></param>
        public static bool VerificaIMEIEstaCadastrado(string imei)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@imei", SqlDbType = SqlDbType.VarChar, Size = 200, Value = imei }
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SpverificaIMEI, parms)) > 0;
        }
        #endregion

        #region AtualizarToken
        /// <summary>
        /// Atualiza o token de um celular
        /// </summary>
        /// <param name="tokenGCM"></param>
        public void AtualizarToken(string tokenGCM)
        {
            object tokenValue = string.Empty;

            if (!string.IsNullOrWhiteSpace(tokenGCM))
                tokenValue = tokenGCM;

            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Celular", SqlDbType = SqlDbType.Int, Size = 4, Value = this._idCelular },
                    new SqlParameter{ ParameterName = "@Token", SqlDbType = SqlDbType.VarChar, Size = 200, Value = tokenValue }
                };

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, Spatualizatoken, parms);
        }
        #endregion

        #endregion

    }
}