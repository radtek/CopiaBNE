using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LanHouse.Business
{
    public class RssRead
    {
        #region LerRssNoticiasAzulzinho

        /// <summary>
        /// Ler o Rss das notícias do Blog Azulzinho
        /// Retornar as 2 últimas notícias
        /// </summary>
        /// <returns></returns>
        public static List<DTO.RssPost> LerRssNoticiasAzulzinho()
        {
            List<DTO.RssPost> lista = new List<DTO.RssPost>();

            var rssFeed = new Uri("http://noticias.azulzinho.com.br/?feed=rss2");

            var request = (HttpWebRequest)WebRequest.Create(rssFeed);
            request.Method = "GET";
            var response = (HttpWebResponse)request.GetResponse();

            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var feedContents = reader.ReadToEnd();

                var document = XDocument.Parse(feedContents);

                var post = (from p in document.Descendants("item")
                            select new DTO.RssPost
                            {
                                titulo = p.Element("title").Value,
                                descricao = p.Element("description").Value,
                                categoria = p.Element("category").Value,

                                link = p.Element("link").Value,
                                dataPost = Convert.ToDateTime(p.Element("pubDate").Value).ToString("dd/MM/yyyy"),
                            }).Take(2).ToList();

                lista = post;
            }

            return lista;
        }

        #endregion
    }
}
