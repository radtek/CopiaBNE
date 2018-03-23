//-- Data: 14/07/2010 12:21
//-- Autor: Gieyson Stelmak

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

    }

}