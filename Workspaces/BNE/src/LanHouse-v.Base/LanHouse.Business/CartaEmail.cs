using LanHouse.Entities.BNE;
using LanHouse.Business.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business
{
    public class CartaEmail
    {
        #region CarregarCartaporId
        /// <summary>
        /// Carregar o valor de uma carta pelo ID
        /// </summary>
        /// <param name="idCarta"></param>
        /// <returns></returns>
        public static string CarregarCartaporId(int idCarta)
        {
            string valorCarta = string.Empty;

            using(var context = new LanEntities())
            {
                valorCarta = (
                    from c in context.BNE_Carta_Email
                    where c.Idf_Carta_Email == idCarta
                    select c.Vlr_Carta_Email).SingleOrDefault();
            }

            return valorCarta;
        }

        #endregion

        #region RetornarConteudoBNE
        public static String RetornarConteudoBNE(Enumeradores.CartaEmail enumerador, out string assunto)
        {
            return MontarConteudo(null, enumerador, true, out assunto);
        }
        #endregion

        #region MontarConteudo
        private static String MontarConteudo(string texto, Enumeradores.CartaEmail? enumerador, bool adicionarRodape, out string assunto)
        {
            assunto = string.Empty;

            var conteudos = new List<Int32>
                {
                    (int)Enumeradores.CartaEmail.ConteudoPadraoBNE
                };

            if (enumerador.HasValue)
                conteudos.Add((int)enumerador.Value);

            if (adicionarRodape)
                conteudos.Add((int)Enumeradores.CartaEmail.ConteudoPadraoBNERodape);

            var valoresConteudos = ListarCartas(conteudos);

            if (enumerador.HasValue)
            {
                assunto = valoresConteudos[enumerador.Value].Assunto;
                if (string.IsNullOrEmpty(texto)) //Se não foi passado o texto, então recupera do enumerador
                    texto = valoresConteudos[enumerador.Value].Conteudo;
            }

            string rodape = string.Empty;

            if (adicionarRodape)
                rodape = valoresConteudos[Enumeradores.CartaEmail.ConteudoPadraoBNERodape].Conteudo;

            var parametrosTemplate = new
            {
                UrlSite = string.Concat("http://", new Parametro().GetById(Convert.ToInt32(Enumeradores.Parametro.URLAmbiente)).Vlr_Parametro),
                UrlImagens = new Parametro().GetById(Convert.ToInt32(Enumeradores.Parametro.URLImagens)).Vlr_Parametro,
                Conteudo = texto,
                Rodape = rodape
            };

            return parametrosTemplate.ToString(valoresConteudos[Enumeradores.CartaEmail.ConteudoPadraoBNE].Conteudo);
        }
        #endregion

        #region ListarCartas
        /// <summary>
        /// Lista as cartas através de uma lista de ids
        /// </summary>
        /// <param name="idsCartas"></param>
        /// <returns>Dicionario de conteudos</returns>
        private static Dictionary<Enumeradores.CartaEmail, DTO.CartaEmail> ListarCartas(List<Int32> idsCartas)
        {
            var itensConteudos = new Dictionary<Enumeradores.CartaEmail, DTO.CartaEmail>();
            List<BNE_Carta_Email> lstCartas;
            using(var entity = new LanEntities())
            {
                lstCartas = (from cartas in entity.BNE_Carta_Email
                             where idsCartas.Contains(cartas.Idf_Carta_Email)
                             select cartas).ToList();

            }

            Enumeradores.CartaEmail cartaEmail;
            foreach(BNE_Carta_Email carta in lstCartas)
            {
                cartaEmail = (Enumeradores.CartaEmail)Enum.Parse(typeof(Enumeradores.CartaEmail), carta.Idf_Carta_Email.ToString());
                itensConteudos.Add(cartaEmail, new DTO.CartaEmail { Conteudo = carta.Vlr_Carta_Email, Assunto = carta.Des_Assunto });

            }

            return itensConteudos;
        }
        #endregion
    }
}
