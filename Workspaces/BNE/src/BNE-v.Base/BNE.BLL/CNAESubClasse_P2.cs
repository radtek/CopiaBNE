//-- Data: 02/03/2010 09:20
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
    public partial class CNAESubClasse : ICloneable // Tabela: TAB_CNAE_Sub_Classe
    {

        #region Clone
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion

        #region Consultas
        private const string Spselectcodsubclasse = @" 
        SELECT  Idf_CNAE_Sub_Classe, 
                Cod_CNAE_Sub_Classe,
                Des_CNAE_Sub_Classe, 
                Idf_CNAE_Classe 
        FROM    plataforma.TAB_CNAE_Sub_Classe WITH(NOLOCK)
        WHERE   Cod_CNAE_Sub_Classe LIKE @Cod_CNAE_Sub_Classe";
        #endregion

        #region Métodos

        #region ListarPorCodigo
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader ListarPorCodigo(string codigoCNAE, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ParameterName =  "@Cod_CNAE_Sub_Classe", SqlDbType = SqlDbType.Char, Size = 7, Value = codigoCNAE}
                };

            if (trans != null)
                return DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectcodsubclasse, parms);
            
            return DataAccessLayer.ExecuteReader(CommandType.Text, Spselectcodsubclasse, parms);
        }
        #endregion

        #region CarregarPorCodigo
        /// <summary>
        /// Método utilizado para retornar uma instância de CNAESubClasse a partir do banco de dados.
        /// </summary>
        /// <param name="codigoCNAE">Chave do registro.</param>
        /// <param name="objCNAESubClasse">Objeto CNAE Sub Classe </param>
        /// <param name="trans">Objeto Transação </param>
        /// <returns>Instância de CNAESubClasse.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorCodigo(string codigoCNAE, out CNAESubClasse objCNAESubClasse, SqlTransaction trans = null)
        {
            IDataReader dr = trans != null ? ListarPorCodigo(codigoCNAE, trans) : ListarPorCodigo(codigoCNAE);

            objCNAESubClasse = new CNAESubClasse();
            if (SetInstance(dr, objCNAESubClasse))
                return true;

            objCNAESubClasse = null;
            return false;
        }
        #endregion

        #endregion

    }
}