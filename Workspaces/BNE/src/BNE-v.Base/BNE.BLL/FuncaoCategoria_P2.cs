//-- Data: 02/03/2010 09:20
//-- Autor: Gieyson Stelmak

using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using BNE.EL;

namespace BNE.BLL
{
	public partial class FuncaoCategoria // Tabela: TAB_Funcao_Categoria
    {

        #region Consultas

        #region Spselectfuncaocategoriaporcurriculo
        private const string Spselectfuncaocategoriaporcurriculo = @"
        SELECT  TOP 1 FC.* 
        FROM    BNE_Funcao_Pretendida FP WITH(NOLOCK)
                INNER JOIN plataforma.TAB_Funcao F WITH(NOLOCK) ON FP.Idf_Funcao = F.Idf_Funcao
                INNER JOIN plataforma.TAB_Funcao_Categoria FC WITH(NOLOCK) ON FC.Idf_Funcao_Categoria = F.Idf_Funcao_Categoria
        WHERE   FP.Idf_Curriculo = @Idf_Curriculo
        ORDER BY FP.Idf_Funcao_Pretendida ASC 
        ";
        #endregion

        #endregion

        #region Métodos

        #region RecuperarCategoria
        public static FuncaoCategoria RecuperarCategoriaPorCurriculo(Curriculo objCurriculo)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objCurriculo.IdCurriculo}
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectfuncaocategoriaporcurriculo, parms))
            {
                var objFuncaoCategoria = new FuncaoCategoria();
                if (SetInstance(dr, objFuncaoCategoria))
                    return objFuncaoCategoria;
            }
            throw (new RecordNotFoundException(typeof(FuncaoCategoria)));
        }
        #endregion

        #endregion

    }
}