using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Employer.Componentes.UI.Web.Extensions
{
    /// <summary>
    /// Classe utilitária para carregar lista de sugestão
    /// </summary>
    public static class ListaSugestoesExtensions
    {
        /// <summary>
        /// Carrega uma lista de sugestão
        /// </summary>
        /// <param name="lista">Lista de sugestão</param>
        /// <param name="Dicionario">Dicionário</param>
        public static void CarregarListaSugestoes(this ListaSugestoes lista, Dictionary<string, string> Dicionario)
        {
            lista.DataSource = Dicionario;
            lista.CampoChave = "key";
            lista.CampoDescricao = "value";
            lista.DataBind();
        }

        /// <summary>
        /// Carrega uma lista de sugestão
        /// </summary>
        /// <param name="lista">Lista de sugestão</param>
        /// <param name="DataSource">Dados</param>
        /// <param name="DataKeyField">Campo chave</param>
        /// <param name="DataValueField">Campo valor</param>
        public static void CarregarListaSugestoes(this ListaSugestoes lista, object DataSource, string DataKeyField, string DataValueField)
        {
            lista.DataSource = DataSource;
            lista.CampoChave = DataKeyField;
            lista.CampoDescricao = DataValueField;
            lista.DataBind();
        }
    }
}
