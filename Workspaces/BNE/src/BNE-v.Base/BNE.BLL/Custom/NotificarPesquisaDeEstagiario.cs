using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using BNE.BLL.Custom.Email;
using System.Reflection;

namespace BNE.BLL.Custom
{
    public sealed class NotificarPesquisaDeEstagiario
    {
        private readonly int _filialId;
        private readonly PesquisaCurriculo _pesquisaCurriculo;
        private readonly List<PesquisaCurriculoIdioma> _listPesquisaCurriculoIdioma;
        private readonly List<PesquisaCurriculoDisponibilidade> _listPesquisaCurriculoDisponibilidade;

        public NotificarPesquisaDeEstagiario(int filialId, PesquisaCurriculo pesquisaCurriculo,
                                             List<PesquisaCurriculoIdioma> listPesquisaCurriculoIdioma,
                                             List<PesquisaCurriculoDisponibilidade> listPesquisaCurriculoDisponibilidade)
        {

            this._filialId = filialId;
            this._pesquisaCurriculo = pesquisaCurriculo;
            this._listPesquisaCurriculoIdioma = listPesquisaCurriculoIdioma;
            this._listPesquisaCurriculoDisponibilidade = listPesquisaCurriculoDisponibilidade;
        }

        public bool EnviarSeAplicavel()
        {
            if (_filialId <= 0)
                return false;

            ParametroFilial parametroFilial;
            if (ParametroFilial.CarregarParametroPorFilial(Enumeradores.Parametro.FilialParceiraWebEstagios,
                                                           new Filial(_filialId), out parametroFilial))
            {
                if ("true".Equals((parametroFilial.ValorParametro ?? string.Empty).Trim(),
                                  StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            var emailNotificacao =
                Parametro.LoadObject((int)Enumeradores.Parametro.EmailWebEstagiosPesquisaPorCurriculoDeEstagiarios);

            if (emailNotificacao == null)
                return false;

            if (string.IsNullOrWhiteSpace(emailNotificacao.ValorParametro))
                return false;

            const Enumeradores.CartaEmail cartaParam = Enumeradores.CartaEmail.ConteudoWebEstagiosIntegracaoPesquisaPorEstagiario;

            string assunto;
            string conteudo = CartaEmail.RetornarConteudoBNE(cartaParam, out assunto);

            if (string.IsNullOrWhiteSpace(conteudo))
                return false;

            var filial = new Filial(_filialId);

            filial.CompleteObject();
            filial.Endereco.CompleteObject();
            filial.Endereco.Cidade.CompleteObject();

            var dados = GerarConteudoDadosCurriculo(filial, _pesquisaCurriculo, _listPesquisaCurriculoDisponibilidade,
                                                    _listPesquisaCurriculoIdioma);

            string mensagemEmail = dados.ToString(conteudo);

            string emailRemetenteSistema = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens);

            foreach (var item in emailNotificacao.ValorParametro.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                EmailSenderFactory.Create(TipoEnviadorEmail.Fila)
                          .Enviar(assunto, mensagemEmail, emailRemetenteSistema, item);
            }

            return true;
        }

        private ExpandoObject GerarConteudoDadosCurriculo(Filial filial, PesquisaCurriculo pesquisaCurriculo,
                                                   List<PesquisaCurriculoDisponibilidade>
                                                       listPesquisaCurriculoDisponibilidade,
                                                   List<PesquisaCurriculoIdioma> listPesquisaCurriculoIdioma)
        {
            var expando = new ExpandoObject();

            PreencheConteudoEstatico(filial, pesquisaCurriculo, listPesquisaCurriculoDisponibilidade,
                                  listPesquisaCurriculoIdioma, ref  expando);

            PreencheConteudoDinamicamente(filial, pesquisaCurriculo, ref expando);

            return expando;
        }

        private void PreencheConteudoDinamicamente(Filial filial, PesquisaCurriculo pesquisaCurriculo,
                                           ref ExpandoObject expando)
        {
            var typeFilial = filial.GetType();
            foreach (var fp in typeFilial.GetProperties())
            {
                ((IDictionary<string, object>)expando)[fp.Name] = fp.GetValue(filial, null);

                if (fp.PropertyType != typeof(Endereco))
                {
                    continue;
                }

                var enderecoObj = ((IDictionary<string, object>)expando)[fp.Name];
                if (enderecoObj == null)
                    continue;

                foreach (var ep in typeof(Endereco).GetProperties())
                {
                    ((IDictionary<string, object>)expando)[typeof(Filial).Name + ep.Name] =
                        ep.GetValue(((IDictionary<string, object>)expando)[fp.Name], null);
                }
            }

            var typeCurriculo = pesquisaCurriculo.GetType();
            foreach (var cp in typeCurriculo.GetProperties())
            {
                ((IDictionary<string, object>)expando)[cp.Name] = cp.GetValue(pesquisaCurriculo, null);

            }
        }

        private void PreencheConteudoEstatico(Filial filial,
                                            PesquisaCurriculo pesquisaCurriculo,
                                           List<PesquisaCurriculoDisponibilidade> listPesquisaCurriculoDisponibilidade,
                                           List<PesquisaCurriculoIdioma> listPesquisaCurriculoIdioma,
                                           ref ExpandoObject expando)
        {
            var container = ((IDictionary<string, object>)expando);

            container["NomeEmpresa"] = filial.NomeFantasia;
            container["FlagCliente"] = "NÃO";

            var relacionamentosDaPesquisa = new StringBuilder();

            if (listPesquisaCurriculoDisponibilidade == null
                || listPesquisaCurriculoDisponibilidade.Count == 0)
            {
                container["PesquisaCurriculoDisponibilidade"] = "Não Aplicável";
            }
            else
            {
                var descricaoDisponibilidades =
                    listPesquisaCurriculoDisponibilidade.Select(
                        a => Disponibilidade.LoadObject(a.Disponibilidade.IdDisponibilidade))
                                                        .Where(a => a != null)
                                                        .Select(a => a.DescricaoDisponibilidade)
                                                        .Aggregate((a, b) => a + ", " + b);

                container["PesquisaCurriculoDisponibilidade"] = descricaoDisponibilidades;

                relacionamentosDaPesquisa.AppendFormat("Disponibilidades=\"{0}\"", descricaoDisponibilidades);
            }

            if (listPesquisaCurriculoIdioma == null
         || listPesquisaCurriculoIdioma.Count == 0)
            {
                container["PesquisaCurriculoIdioma"] = "Não Aplicável";
            }
            else
            {
                if (relacionamentosDaPesquisa.Length > 0)
                    relacionamentosDaPesquisa.Append(" | ");

                var descricaoIdiomas =
                    listPesquisaCurriculoIdioma.Select(a => Idioma.LoadObject(a.Idioma.IdIdioma))
                                               .Where(a => a != null)
                                               .Select(a => a.DescricaoIdioma)
                                               .Aggregate((a, b) => a + ", " + b);

                container["PesquisaCurriculoIdioma"] = descricaoIdiomas;

                relacionamentosDaPesquisa.AppendFormat("Idioma=\"{0}\"", descricaoIdiomas);
            }

            var formatarPesquisa = FormatarPesquisa(pesquisaCurriculo);
            if (formatarPesquisa.Length <= 0)
            {
                container["PesquisaFormatada"] = relacionamentosDaPesquisa.ToString();
                return;
            }

            if (relacionamentosDaPesquisa.Length > 0)
            {
                container["PesquisaFormatada"] = formatarPesquisa + " | " +
                                                 relacionamentosDaPesquisa + ".";
            }
            else
            {
                container["PesquisaFormatada"] = formatarPesquisa;
            }
        }

        private string FormatarPesquisa(PesquisaCurriculo pesquisaCurriculo)
        {
            var texto = new StringBuilder();

            FormatarPesquisaObjeto("", pesquisaCurriculo, ref texto);

            return texto.ToString();
        }

        private void FormatarPesquisaObjeto(string prefixo, object objPesquisa, ref StringBuilder totalTexto)
        {
            var typeCurriculo = objPesquisa.GetType();

            var container = new List<Tuple<string, object>>();
            foreach (var cp in typeCurriculo.GetProperties())
            {
                if (cp.Name.IndexOf("FlagPesquisaAvancada", StringComparison.OrdinalIgnoreCase) > -1
                    || cp.Name.StartsWith("Id", StringComparison.OrdinalIgnoreCase)
                    || cp.Name.EndsWith("Id", StringComparison.OrdinalIgnoreCase))
                    continue;

                if (cp.PropertyType == typeof(DateTime)
                    || cp.PropertyType == typeof(DateTime?))
                    continue;

                var valor = cp.GetValue(objPesquisa, null);

                if (valor == null)
                    continue;

                if (cp.PropertyType.IsValueType)
                {
                    if (valor.Equals(Activator.CreateInstance(cp.PropertyType)))
                        continue;
                }

                if (cp.PropertyType == typeof(string))
                {
                    var valorTexto = valor.ToString();
                    if (!string.IsNullOrWhiteSpace(valorTexto))
                    {
                        AcrescentarValor(prefixo, cp, valor, ref totalTexto);
                    }
                    continue;
                }

                if (!cp.PropertyType.IsValueType)
                {
                    container.Add(Tuple.Create(cp.Name, valor));
                    continue;
                }

                AcrescentarValor(prefixo, cp, valor, ref totalTexto);
            }

            foreach (var item in container)
            {
                FormatarPesquisaObjeto(item.Item1, item.Item2, ref totalTexto);
            }
        }

        private static void AcrescentarValor(string prefixo, PropertyInfo prop, object valor, ref StringBuilder builder)
        {
            if (builder.Length > 0)
            {
                builder.Insert(0, " | ");
            }

            if (prop.Name.IndexOf(prefixo, StringComparison.OrdinalIgnoreCase) == -1)
            {
                builder.Insert(0, string.Format("{0}-{1}=\"{2}\"", prefixo, prop.Name, valor));
            }
            else
            {
                builder.Insert(0, string.Format("{0}=\"{1}\"", prop.Name, valor));
            }
        }
    }
}
