//-- Data: 02/03/2010 09:20
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
    public partial class NaturezaJuridica : ICloneable // Tabela: TAB_Natureza_Juridica
    {

        #region Clone
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion

        #region Consultas
        private const string SPSELECT = "SELECT Nat.Idf_Natureza_Juridica,Nat.Cod_Natureza_Juridica,Nat.Des_Natureza_Juridica,Nat.Flg_Inativo,Nat.Dta_Cadastro FROM plataforma.TAB_Natureza_Juridica Nat WHERE 1 = 1";
        private const string SPSELECTCODIGO = "SELECT * FROM plataforma.TAB_Natureza_Juridica WHERE Cod_Natureza_Juridica = @Cod_Natureza_Juridica";
        #endregion

        #region Métodos

        #region Listar
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Clóvis Jr.</remarks>
        public static IDataReader Listar()
        {
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECT, null);
        }
        #endregion

        #region ListarPorCodigo
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader ListarPorCodigo(string codigoNatureza, SqlTransaction trans = null)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Cod_Natureza_Juridica", SqlDbType.Char, 4));

            parms[0].Value = codigoNatureza;

            IDataReader dr = null;
            if (trans != null)
                dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPSELECTCODIGO, parms);
            else
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTCODIGO, parms);

            return dr;
        }
        #endregion

        #region CarregarPorCodigo
        /// <summary>
        /// Método utilizado para retornar uma instância de NaturezaJuridica a partir do banco de dados.
        /// </summary>
        /// <param name="codigoNatureza">Chave do registro.</param>
        /// <returns>Instância de NaturezaJuridica.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorCodigo(string codigoNatureza, out NaturezaJuridica objNaturezaJuridica, SqlTransaction trans = null)
        {
            IDataReader dr = null;
            if (trans != null)
                dr = ListarPorCodigo(codigoNatureza, trans);
            else
                dr = ListarPorCodigo(codigoNatureza, null);

            objNaturezaJuridica = new NaturezaJuridica();
            if (SetInstance(dr, objNaturezaJuridica))
                return true;

            if (!dr.IsClosed)
                dr.Close();

            objNaturezaJuridica = null;
            return false;
        }
        #endregion

        #endregion

    }
}