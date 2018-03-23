//-- Data: 14/07/2010 12:21
//-- Autor: Gieyson Stelmak

using System;
using System.Data;
namespace BNE.BLL
{
    public partial class NivelCurso // Tabela: TAB_Nivel_Curso
    {

        #region Consultas

        private const string LISTARNIVELCURSO = "SELECT * FROM TAB_Nivel_Curso";

        #endregion

        #region Metodos

        #region ListarNivelCurso

        public static IDataReader ListarNivelCurso()
        {
            return DataAccessLayer.ExecuteReader(CommandType.Text, LISTARNIVELCURSO, null);
        }

        #endregion

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