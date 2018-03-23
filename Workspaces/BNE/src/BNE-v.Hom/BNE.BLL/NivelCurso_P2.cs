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
        /// M�dodos e atributos auxiliares � migra��o de dados para o novo
        /// dom�nio.
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