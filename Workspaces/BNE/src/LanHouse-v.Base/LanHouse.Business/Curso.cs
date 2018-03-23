using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanHouse.Entities.BNE;
using System.Collections;

namespace LanHouse.Business
{
    public class Curso
    {
        #region ListarCursos
        public static IList ListarCursos(string nomeCurso, int limiteRegistros)
        {
            using (var entity = new LanEntities())
            {
                var query = (
                    from esc in entity.TAB_Curso
                    where esc.Flg_Inativo == false
                    && esc.Des_Curso.ToLower().StartsWith(nomeCurso.ToLower())
                    orderby esc.Des_Curso ascending
                    select new { id = esc.Idf_Curso, text = esc.Des_Curso }).Take(limiteRegistros);

                return query.ToList();
            }
        }
        #endregion

        #region Carregar id curso por descricao

        public static int CarregarIdCursoporDescricao(string desCurso)
        {
            using(var entity = new LanEntities())
            {
                var query = (
                    from cur in entity.TAB_Curso
                    where cur.Des_Curso.ToLower() == desCurso.ToLower()
                    select cur.Idf_Curso).FirstOrDefault();

                return query;
            }
        }

        #endregion
    }
}
