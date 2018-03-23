using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace BNE.Services.AsyncServices.Base.ProcessosAssincronos
{
    [Serializable]
    public class ParametroExecucaoValor
    {
        public String Valor { get; set; }
    }

    /// <summary>
    /// Os parâmetros de execução
    /// </summary>
    [Serializable]
    public class ParametroExecucao
    {
        public ParametroExecucao()
        {
            Valores = new List<ParametroExecucaoValor>();
        }

        #region Parametro
        /// <summary>
        /// Parametro
        /// </summary>
        public String Parametro { get; set; }
        #endregion

        #region Valores
        public List<ParametroExecucaoValor> Valores { get; set; }
        #endregion

        #region Valor
        /// <summary>
        /// Valor do primeiro parâmetro
        /// </summary>
        [JsonIgnore]
        public String Valor
        {
            get
            {
                if (Valores.Count == 0)
                {
                    Valores.Add(new ParametroExecucaoValor());
                }
                return Valores[0].Valor;
            }
            set
            {
                if (Valores.Count == 0)
                {
                    Valores.Add(new ParametroExecucaoValor());
                }
                Valores[0].Valor = value;
            }
        }
        #endregion

        #region ValorInt
        /// <summary>
        /// Retorna o valor do parâmetro em int
        /// </summary>
        [JsonIgnore]
        public int? ValorInt
        {
            get
            {
                int dummy;
                if (int.TryParse(Valor, out dummy))
                    return dummy;

                return null;
            }
            set
            {
                Valor = Convert.ToString(value);
            }
        }
        #endregion

        #region ValorDecimal
        /// <summary>
        /// Retorna o valor do parâmetro em decimal
        /// </summary>
        [JsonIgnore]
        public Decimal? ValorDecimal
        {
            get
            {
                Decimal dummy;
                if (Decimal.TryParse(Valor, out dummy))
                    return dummy;

                return null;
            }
            set
            {
                Valor = Convert.ToString(value);
            }
        }
        #endregion

        #region ValorBool
        /// <summary>
        /// Retorna o valor do parâmetro em booleano
        /// </summary>
        [JsonIgnore]
        public bool ValorBool
        {
            get
            {
                Boolean dummy;
                if (Boolean.TryParse(Valor, out dummy))
                    return dummy;

                return "1".Equals(Valor);
            }
            set
            {
                Valor = Convert.ToString(value);
            }
        }
        #endregion

        #region Adicionar
        public void Adicionar(String valor, String desValor)
        {
            Valores.Add(new ParametroExecucaoValor { Valor = valor });
        }
        #endregion
    }

    #region ParametroExecucaoCollection
    /// <summary>
    /// Coleção dos parâmetros de execução
    /// </summary>
    [Serializable]
    public class ParametroExecucaoCollection : Collection<ParametroExecucao>
    {
        #region Métodos

        #region Add
        /// <summary>
        /// Adiciona um ítem a coleção
        /// </summary>
        /// <param name="parametro">O parametro</param>
        /// <param name="desParametro">A descrição do parâmetro</param>
        /// <param name="valor">O valor do parâmetro</param>
        /// <param name="desValor">A descrição do valor do parâmetro</param>
        public void Add(String parametro, String desParametro, String valor, String desValor)
        {
            var objParametro = new ParametroExecucao
            {
                Parametro = parametro,
                Valor = valor
            };

            Add(objParametro);
        }
        public void Add(String parametro, String desParametro, String valor, String desValor, bool exibirParametro)
        {
            var objParametro = new ParametroExecucao
            {
                Parametro = parametro,
                Valor = valor
            };

            Add(objParametro);
        }
        #endregion

        #endregion

        #region Propriedades

        #region this
        /// <summary>
        /// Retorna a instância do parâmetro a partir do valor da sua propriedade "parâmetro".
        /// Caso o parâmetro não exista, cria uma nova instância e a retorna
        /// </summary>
        /// <param name="parametro">O valor da propriedade "parâmetro"</param>
        /// <returns>A instância do parâmetro</returns>
        public ParametroExecucao this[String parametro]
        {
            get
            {
                foreach (ParametroExecucao parm in this)
                {
                    if (parm.Parametro == parametro)
                        return parm;
                }

                var p = new ParametroExecucao
                {
                    Parametro = parametro
                };
                Add(p);

                return p;
            }
        }
        #endregion

        #endregion
    }
    #endregion

}