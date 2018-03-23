using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BNE.BLL.Assincronos
{
    public class MensagemAssincronoLogCRM
    {
        #region Atributos

        public string ObjName { get; set; }
        public string Property { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }

        #endregion


        #region DiffFields
        /// <summary>
        /// Método para comparar diferenças entre dois objetos.
        /// </summary>
        /// <typeparam name="T1">Tipo do Objeto</typeparam>
        /// <param name="oldObj">Objeto Original</param>
        /// <param name="newObj">Objeto que você quer comparar</param>
        /// <param name="typeObj">Opcional, é usado quando a chamada é feita recursivamente</param>
        /// <returns>Array de AlteracaoLog</returns>
        public static MensagemAssincronoLogCRM[] DiffFields<T>(T oldObj, T newObj, Type typeObj = null)
        {
            var diffList = new List<MensagemAssincronoLogCRM>();
            var type = typeObj ?? typeof(T);
            //var reg = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.GetField);
            //var reg2 = type.GetProperties();

            //var propriedades = type.GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.GetField).Where(a => a.MemberType == MemberTypes.Property || a.MemberType == MemberTypes.Field);
            foreach (var prop in type.GetFields().Where(t => t.IsStatic == false))
            {
                var nome = prop.Name;
                //verificando se é um array ou lista 
                if (prop.GetValue(newObj).GetType().IsGenericType && prop.GetValue(oldObj).GetType().IsGenericType)
                {
                    //Tipo da lista ou tipo da classe do objecto
                    Type tipo = (Type)GetGenericCollectionItemType(prop.GetValue(newObj).GetType());

                    var oldObjField = (IList)prop.GetValue(oldObj);
                    var newObjField = (IList)prop.GetValue(newObj);
                    var tipoObjeto = prop.GetValue(newObj).GetType();

                    var countNewObjField = (int)tipoObjeto.GetProperty("Count").GetValue(newObjField, null);
                    var countOldObjField = (int)tipoObjeto.GetProperty("Count").GetValue(oldObjField, null);


                    if (countNewObjField > countOldObjField)
                    {
                        for (int i = 0; i < countNewObjField; i++)
                        {
                            if (i >= countOldObjField)
                            {
                                var instanciaNula = Activator.CreateInstance(tipo);
                                diffList.AddRange(DiffFields(instanciaNula, newObjField[i], newObjField[i].GetType()));
                            }
                            else
                            {
                                diffList.AddRange(DiffFields(oldObjField[i], newObjField[i], newObjField[i].GetType()));
                            }
                        }
                    }
                    else
                        if (countOldObjField > countNewObjField)
                        {
                            for (int i = 0; i < countOldObjField; i++)
                            {
                                if (i >= countNewObjField)
                                {
                                    var instanciaNula = Activator.CreateInstance(tipo);
                                    diffList.AddRange(DiffFields(oldObjField[i], instanciaNula, oldObjField[i].GetType()));
                                }
                                else
                                {
                                    diffList.AddRange(DiffFields(oldObjField[i], newObjField[i], newObjField[i].GetType()));
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < countNewObjField; i++)
                            {
                                diffList.AddRange(DiffFields(oldObjField[i], newObjField[i], newObjField[i].GetType()));
                            }
                        }

                }
                else
                {
                    var _newValue = prop.GetValue(newObj);
                    var _oldValue = prop.GetValue(oldObj);
                    diffList.AddRange(DiffFields(_oldValue, _newValue, _oldValue.GetType()));
                }
            }
            return diffList.ToArray();
        }
        #endregion

        #region DiffProperties
        public static MensagemAssincronoLogCRM DiffProperty<T>(T oldObj, T newObj, Type typeObj = null)
        {
            var type = typeObj ?? typeof(T);

            foreach (var prop in type.GetProperties())
            {
                if (!prop.Name.Substring(2).Equals(type.Name)) continue;

                var _newValue = prop.GetValue(newObj, null);
                var _oldValue = prop.GetValue(oldObj, null);
                _newValue = _newValue == null ? "" : _newValue;
                _oldValue = _oldValue == null ? "" : _oldValue;
                if (!_newValue.Equals(_oldValue))
                {
                    if (_newValue.GetType().IsClass)
                    {
                        return DiffProperty(_oldValue, _newValue, _newValue.GetType());
                    }
                    return new MensagemAssincronoLogCRM
                    {
                        ObjName = type.Name,
                        Property = GetDisplayName(prop),
                        OldValue = _oldValue == null ? "" : _oldValue.ToString(),
                        NewValue = _newValue == null ? "" : _newValue.ToString()
                    };
                }
            }
            return null;
        }


        public static MensagemAssincronoLogCRM[] DiffProperties<T>(T oldObj, T newObj, Type typeObj = null)
        {
            var diffList = new List<MensagemAssincronoLogCRM>();
            var type = typeObj ?? typeof(T);

            foreach (var prop in type.GetProperties())
            {
                string name = GetDisplayName(prop);

                if (name.Equals("IgnoreData")) continue;

                var _newValue = prop.GetValue(newObj, null);
                var _oldValue = prop.GetValue(oldObj, null);

                _newValue = _newValue == null ? "" : _newValue;
                _oldValue = _oldValue == null ? "" : _oldValue;

                if (_newValue.GetType().Name == "String")// Problema com Classes não primitivas
                {
                    _newValue = _newValue.ToString().TrimEnd().TrimStart();
                    _oldValue = _oldValue.ToString().TrimEnd().TrimStart();
                }

                if (!_newValue.Equals(_oldValue))
                {
                    string[] objIgnore = { "String", "Boolean", "Int16", "Int32", "Int64", "DateTime", "Double" };

                    if (_newValue.GetType().IsClass && !objIgnore.Contains(_newValue.GetType().Name))
                    {
                        var msgAss = DiffProperty(_oldValue, _newValue, _newValue.GetType());
                        if (msgAss != null)
                            diffList.Add(msgAss);
                    }
                    else
                    {
                        diffList.Add(new MensagemAssincronoLogCRM
                        {
                            ObjName = type.Name,
                            Property = GetDisplayName(prop),
                            OldValue = _oldValue == null ? "" : _oldValue.ToString(),
                            NewValue = _newValue == null ? "" : _newValue.ToString()
                        });
                    }

                }
            }
            return diffList.ToArray();

        }

        private static string GetDisplayName(MemberInfo type)
        {
            var nameType = (DisplayAttribute)type.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();

            if (nameType != null)
                return nameType.Name;
            return type.Name;
        }

        #endregion

        #region FieldChanged
        public static List<MensagemAssincronoLogCRM> FieldChanged(Type type, object newObj, object oldObj)
        {

            var fields = type.GetFields().Where(t => t.IsStatic == false);
            var diffList = new List<MensagemAssincronoLogCRM>();
            foreach (var prop in fields)
            {
                var _newValue = prop.GetValue(newObj);
                var _oldValue = prop.GetValue(oldObj);
                diffList.AddRange(DiffFields(_oldValue, _newValue, _oldValue.GetType()));
            }
            return diffList.ToList();
        }
        #endregion

        #region GetGenericCollectionItemType
        static Type GetGenericCollectionItemType(Type type)
        {
            if (type.IsGenericType)
            {
                var args = type.GetGenericArguments();
                if (args.Length == 1 &&
                    typeof(ICollection<>).MakeGenericType(args).IsAssignableFrom(type))
                {
                    return args[0];
                }
            }
            return null;
        }
        #endregion

        #region StringListaDeItensModificados
        public static String StringListaDeItensModificados(MensagemAssincronoLogCRM[] obj)
        {
            if (obj.Length > 0)
            {
                string modificados = "No(a) " + (obj.GetValue(0) as MensagemAssincronoLogCRM).ObjName + ": <br/> ";
                modificados += string.Join("<br/> ", obj.Where(p => p != null).Select(m => m.ToString()));
                return modificados + "<br/><br/>";
            }

            return string.Empty;
        }

        public static String StringListaDeItensModificados(params object[] objects)
        {
            if (objects.Length == 0) return "<br/>";

            string modificados = "No(a) " + ((objects[0] as MensagemAssincronoLogCRM[]).GetValue(0) as MensagemAssincronoLogCRM).ObjName + ": <br/> ";

            for (int x = 0; x < objects.Length; x++)
                modificados += string.Join("<br/> ", (objects[x] as MensagemAssincronoLogCRM[]).Where(p => p != null).Select(m => m.ToString()));
            return modificados + "<br/><br/>";
        }
        #endregion

        #region CURRICULO
        public static void SalvarModificacoesCurriculoCRM(string descricao, Curriculo objCurriculo, UsuarioFilialPerfil objUsuarioGerador = null, string nomeProcessoSistema = null)
        {
            try
            {
                Thread t = new Thread(() => ExecutaCurriculo(descricao, objCurriculo, objUsuarioGerador, nomeProcessoSistema));
                t.Start();
            }
            catch (ThreadAbortException ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }

        }

        private static void ExecutaCurriculo(string descricao, Curriculo objCurriculo, UsuarioFilialPerfil objUsuarioGerador, string nomeProcessoSistema)
        {
            if (objUsuarioGerador == null)
                CurriculoObservacao.SalvarCRM(descricao, objCurriculo, nomeProcessoSistema);
            else if (string.IsNullOrEmpty(nomeProcessoSistema))
                CurriculoObservacao.SalvarCRM(descricao, objCurriculo, objUsuarioGerador);
        }
        #endregion

        #region FILIAL
        public static void SalvarModificacoesFilialCRM(string descricao, Filial objFilial, UsuarioFilialPerfil objUsuarioGerador = null, string nomeProcessoSistema = null)
        {
            try
            {
                Thread t = new Thread(() => ExecutaFilial(descricao, objFilial, objUsuarioGerador, nomeProcessoSistema));
                t.Start();
            }
            catch (ThreadAbortException ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
        }

        private static void ExecutaFilial(string descricao, Filial objFilial, UsuarioFilialPerfil objUsuarioGerador, string nomeProcessoSistema)
        {
            if (objUsuarioGerador == null)
                FilialObservacao.SalvarCRM(descricao, objFilial, nomeProcessoSistema);
            else if (string.IsNullOrEmpty(nomeProcessoSistema))
                FilialObservacao.SalvarCRM(descricao, objFilial, objUsuarioGerador);
        }
        #endregion


        #region ToString
        public override string ToString()
        {
            if (OldValue.Equals("true") || OldValue.Equals("false"))
            {
                if (OldValue.Equals("true"))
                {
                    NewValue = "Não";
                    OldValue = "Sim";
                }
                else
                {
                    NewValue = "Sim";
                    OldValue = "Não";
                }
            }
            return Property + ": " + OldValue + " => " + NewValue + " ";
        }
        #endregion
    }
}
