using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BNE.Comum.Domain.Localizable
{
    public class Translation
    {
        /// <summary>
        /// Linguagem padrão salva no banco. Quando a linguagem padrão é usada, a tradução não será realizada.
        /// </summary>
        const string defaultLanguage = "pt-BR";

        /// <summary>
        /// Traduz a entidade, substituindo os valores das propriedades pelas definidas no banco.
        /// </summary>
        /// <param name="entity">Objeto a ser traduzido.</param>
        /// <param name="context">Contexto do banco de dados.</param>
        public static void Translate(object entity, DbContext context)
        {
            Type t = entity.GetType();

            //Se a cultura da Thread é igual à padrão, não realiza a tradução
            if (Thread.CurrentThread.CurrentCulture.IetfLanguageTag == defaultLanguage)
            {
                t.GetProperty("TranslationState").SetValue(entity, Model.Localizable.Enum.TranslationState.Translated);
                return;
            }

            //Obtendo o valor da primary key do objeto
            var keys = GetKeyNames(entity, context);
            var pk = t.GetProperty(keys[0].ToString()).GetValue(entity).ToString();

            //Obtendo tipo do objeto
            var type = entity.GetType().FullName;

            //Recuperando linguagem da thread.
            var l = Thread.CurrentThread.CurrentCulture.IetfLanguageTag;

            //Buscando traduções no banco
            var ts = context.Set<BNE.Comum.Model.Localizable.Translation>()
                        .Where(lt => lt.Type == type && lt.PrimaryKeyValue == pk && lt.LanguageCode == l).ToList();

            //realizando tradução
            var count = 0;
            foreach (var item in ts)
            {
                t.GetProperty(item.FieldName).SetValue(entity, item.Text);
                count++;
            }

            //Definindo status da tradução
            if (count == 0) t.GetProperty("TranslationState").SetValue(entity, Model.Localizable.Enum.TranslationState.NoTranslationAvailable);
            if (count < ts.Count()) t.GetProperty("TranslationState").SetValue(entity, Model.Localizable.Enum.TranslationState.PartialTranslated);
            if (count >= ts.Count()) t.GetProperty("TranslationState").SetValue(entity, Model.Localizable.Enum.TranslationState.Translated);
            
        }

        /// <summary>
        /// Obtem o nome das primary keys presentes no objeto
        /// </summary>
        /// <param name="entity">Entidade para a qual os nomes das PK's devem ser recuperadas.</param>
        /// <param name="context">Contexto</param>
        /// <returns>Array com os nomes das PK's</returns>
        public static string[] GetKeyNames(object entity, DbContext context) //where T : class
        {
            Type t = entity.GetType();

            //retrieve the base type
            while (t.BaseType != typeof(object))
            {
                t = t.BaseType;
            }

            ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;

            //create method CreateObjectSet with the generic parameter of the base-type
            MethodInfo method = typeof(ObjectContext)
                                      .GetMethod("CreateObjectSet", Type.EmptyTypes)
                                      .MakeGenericMethod(t);
            dynamic objectSet = method.Invoke(objectContext, null);
            IEnumerable<dynamic> keyMembers = objectSet.EntitySet.ElementType.KeyMembers;
            string[] keyNames = keyMembers.Select(k => (string)k.Name).ToArray();
            return keyNames;
        }
    }
}
