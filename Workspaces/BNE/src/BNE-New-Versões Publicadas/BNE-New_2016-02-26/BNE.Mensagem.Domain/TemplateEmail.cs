using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using BNE.Mensagem.Data.Repositories;
using BNE.Core.ExtensionsMethods;
using BNE.Mensagem.Domain.Exceptions;

namespace BNE.Mensagem.Domain
{
    public class TemplateEmail
    {

        private readonly ITemplateEmailRepository _templateEmailRepository;

        public TemplateEmail(ITemplateEmailRepository templateEmailRepository)
        {
            _templateEmailRepository = templateEmailRepository;
        }

        public string HTML(Model.TemplateEmail objTemplateEmail, dynamic parametros)
        {
            return RecuperarHTML(objTemplateEmail.Sistema, objTemplateEmail.Nome, parametros, true);
        }
        public string HTML(Model.Sistema objSistema, string nometemplate, ExpandoObject parametros)
        {
            return RecuperarHTML(objSistema, nometemplate, parametros, true);
        }
        private string RecuperarHTML(Model.Sistema objSistema, string nometemplate, ExpandoObject parametros, bool validarParametros, string conteudo = null)
        {
            string html = conteudo ?? "{Conteudo}";

            var template = RecuperarTemplate(objSistema, nometemplate);

            var conteudoValidacao = ((object)parametros).ToString(template.Conteudo);

            if (template.TemplateSistema != null)
            {
                dynamic parametrosTemplateTemplateSistema = new ExpandoObject();
                parametrosTemplateTemplateSistema.UrlAmbiente = string.Concat("http://", objSistema.UrlSite);
                parametrosTemplateTemplateSistema.UrlImagens = string.Concat("http://", objSistema.UrlImagens);

                html = RecuperarHTML(objSistema, template.TemplateSistema.Nome, parametrosTemplateTemplateSistema, false, html);
            }

            if (validarParametros)
                ValidarParametros(conteudoValidacao);

            return MontarHTML(objSistema, html, conteudoValidacao);
        }

        private string MontarHTML(Model.Sistema objSistema, string template, string conteudo)
        {
            var parametrosTemplateTemplateSistema = new
            {
                UrlAmbiente = string.Concat("http://", objSistema.UrlSite),
                UrlImagens = string.Concat("http://", objSistema.UrlImagens),
                Conteudo = conteudo
            };

            return parametrosTemplateTemplateSistema.ToString(template);
        }

        public Model.TemplateEmail RecuperarTemplate(Model.Sistema objSistema, string nometemplate)
        {
            var template = _templateEmailRepository.GetMany(n => n.Sistema.Id == objSistema.Id && n.Nome == nometemplate).OrderByDescending(n => n.Versao).FirstOrDefault();
            return template;
        }

        public string Assunto(Model.TemplateEmail templateEmail, ExpandoObject parametros)
        {
            var conteudoValidacao = parametros != null ? ((object)parametros).ToString(templateEmail.Assunto) : templateEmail.Assunto;

            ValidarParametros(conteudoValidacao);

            return conteudoValidacao;
        }

        public void ValidarParametros(string conteudo)
        {
            var regex = new Regex(@"({)([^}]+)(})", RegexOptions.IgnoreCase);

            var mc = regex.Matches(conteudo);
            if (mc.Count > 0)
                throw new SemParametroException(mc.Cast<Match>().Select(n => n.Value).ToList());
        }

    }
}
