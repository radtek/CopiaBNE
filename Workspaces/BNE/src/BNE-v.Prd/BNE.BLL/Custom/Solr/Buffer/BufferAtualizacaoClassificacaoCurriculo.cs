using BNE.Domain.Events.CrossDomainEvents;
using BNE.Domain.Events.Handler;

namespace BNE.BLL.Custom.Solr.Buffer
{
    public class BufferAtualizacaoClassificacaoCurriculo
    {
        private static BufferAtualizacao _buffer;

        private static BufferAtualizacao Buffer
        {
            get { return _buffer ?? (_buffer = new BufferAtualizacao(Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLSolrAtualizaClassificacao))); }
        }

        public static void Update(Curriculo objCurriculo)
        {
            Buffer.Add(objCurriculo);
            try
            {
                if (objCurriculo.PessoaFisica == null || string.IsNullOrWhiteSpace(objCurriculo.PessoaFisica.EmailPessoa))
                {
                    if (objCurriculo.PessoaFisica == null)
                        objCurriculo.PessoaFisica = new PessoaFisica(PessoaFisica.RecuperarIdPorCurriculo(objCurriculo));
                }

                DomainEventsHandler.Handle(new OnAtualizarCurriculo(objCurriculo.IdCurriculo, objCurriculo.PessoaFisica.EmailPessoa));
            }
            catch (System.Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Erro ao lançar evento de dominio OnAtualizarCurriculo");
            }

        }
    }
}