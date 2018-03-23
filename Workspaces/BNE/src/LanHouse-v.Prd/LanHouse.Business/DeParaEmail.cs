using LanHouse.Entities.BNE;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business
{
    public class DeParaEmail
    {
        #region ListarSugestaoEmails

        /// <summary>
        /// Listar sugestão de email ao digitar @ no campo email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="limiteRegistros"></param>
        /// <returns></returns>
        public static IList ListarSugestaoEmails(string email, int limiteRegistros)
        {
            using (var entity = new LanEntities())
            {
                if (email.Contains("@"))
                {
                    string sufixo = email.Substring(email.IndexOf('@'));
                    email = email.Replace(sufixo, string.Empty);

                    var query = (
                        from eml in entity.BNE_Ranking_Email1
                        where eml.Des_Email.ToLower().StartsWith(sufixo)
                        orderby eml.idf_Ranking_email ascending
                        select new { id= eml.idf_Ranking_email, text = email + eml.Des_Email}).Distinct().Take(limiteRegistros);

                    return query.ToList();
                }

                return null;
            }
        }

        #endregion
    }
}
