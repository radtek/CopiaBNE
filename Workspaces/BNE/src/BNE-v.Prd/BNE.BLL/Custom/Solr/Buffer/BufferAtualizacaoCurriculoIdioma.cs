using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BNE.BLL.Custom.Solr.Buffer
{
    public static class BufferAtualizacaoCurriculoIdioma
    {

        private static BufferAtualizacao _buffer;

        private static BufferAtualizacao Buffer
        {
            get { return _buffer ?? (_buffer = new BufferAtualizacao(Parametro.RecuperaValorParametro(Enumeradores.Parametro.UrlSolrAtualizaCV_idioma))); }
        }

        public static void Update(Curriculo objCurriculo)
        {
            Buffer.Add(objCurriculo);
        }
    }
}
