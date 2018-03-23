//-- Data: 02/03/2010 09:17
//-- Autor: Gieyson Stelmak

using BNE.EL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
	public partial class Raca // Tabela: TAB_Raca
    {

        #region Consulta

        private const string SELECTRACA = @"Select *
                                            From
                                            plataforma.Tab_Raca";
        private const string SELECTRACADESCRICAO = @"Select *
                                            From
                                            plataforma.Tab_Raca
                                            WHERE Des_Raca = @desRaca";

        #endregion

        #region Métodos
        
        #region CarregarPorDescricao
        public static Raca CarregarPorDescricao(string descricao)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@desRaca", SqlDbType.VarChar, 100));
            parms[0].Value = descricao;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SELECTRACADESCRICAO, parms))
            {
                Raca objRaca = new Raca();
                if (SetInstance(dr, objRaca))
                    return objRaca;

                if (!dr.IsClosed)
                    dr.Close();
            }
            throw (new RecordNotFoundException(typeof(PessoaFisica)));
        }
        #endregion

        /// <summary>
        /// Método que retorna a consulta de Des_Raca para o dropdownlist ser carregado
        /// </summary>
        /// <returns></returns>
        public static IDataReader Listar()
        {
            return DataAccessLayer.ExecuteReader(CommandType.Text,SELECTRACA,null);
        }
        #endregion

        #region [ Migration Mapping ]
        /// <summary>
        /// Médodos e atributos auxiliares à migração de dados para o novo
        /// domínio.
        /// </summary>
        public DateTime MigrationDataCadastro
        {
            set
            {
                this._dataCadastro = value;
            }
            get { return this._dataCadastro; }
        }
        #endregion
    }
}