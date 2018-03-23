//-- Data: 16/04/2013 16:57
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
	public partial class CadastroParceiro // Tabela: BNE_Cadastro_Parceiro
    {

        #region Consultas

//        #region Sppossuicadastro
//        private const string Sppossuicadastro = @"
//        SELECT  COUNT(*)
//        FROM    BNE_Cadastro_Parceiro CP
//        WHERE   CP.Idf_Curriculo = @Idf_Curriculo
//                AND CP.Idf_Parceiro_Tecla = @Idf_Parceiro
//        ";
//        #endregion

        #region Sprecuperarloginsenha
        private const string Sprecuperarloginsenha = @"
        SELECT  CP.*
        FROM    BNE_Cadastro_Parceiro CP
        WHERE   CP.Idf_Curriculo = @Idf_Curriculo
                AND CP.Idf_Parceiro_Tecla = @Idf_Parceiro
        ";
        #endregion

        #endregion

        //#region PossuiCadastroParceiro
        //public static bool PossuiCadastroParceiro(Curriculo objCurriculo, ParceiroTecla objParceiro)
        //{
        //    var parms = new List<SqlParameter>
        //        {
        //            new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objCurriculo.IdCurriculo },
        //            new SqlParameter { ParameterName = "@Idf_Parceiro", SqlDbType = SqlDbType.Int, Size = 4, Value = objParceiro.IdParceiroTecla }
        //        };

        //    return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Sppossuicadastro, parms)) > 0;
        //}
        //#endregion

        #region PossuiCadastroParceiro
        public static bool PossuiCadastroParceiro(Curriculo objCurriculo, ParceiroTecla objParceiro, out CadastroParceiro objCadastroParceiro, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objCurriculo.IdCurriculo },
                    new SqlParameter { ParameterName = "@Idf_Parceiro", SqlDbType = SqlDbType.Int, Size = 4, Value = objParceiro.IdParceiroTecla }
                };

            bool retorno = false;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, Sprecuperarloginsenha, parms))
            {
                objCadastroParceiro = new CadastroParceiro();
                if (SetInstance(dr, objCadastroParceiro))
                    retorno = true;
            }

            return retorno;
        }
        #endregion

        //#region DadosAcesso
        //public class DadosAcesso
        //{
        //    public string Login { get; set; }    
        //    public string Senha { get; set; }    
        //}
        //#endregion

    }
}