using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BNE.Services.Base.ProcessosAssincronos
{
    /// <summary>
    ///     Os parâmetros de execução
    /// </summary>
    [Serializable]
    public class ParametroExecucao
    {
        public ParametroExecucao()
        {
            Valores = new List<ParametroExecucaoValor>();
            ExibirParametro = true;
        }

        /// <summary>
        ///     Parametro
        /// </summary>
        public string Parametro { get; set; }

        /// <summary>
        ///     Descrição do parâmetro - Usado em tela
        /// </summary>
        public string DesParametro { get; set; }

        public List<ParametroExecucaoValor> Valores { get; set; }

        /// <summary>
        ///     Valor do primeiro parâmetro
        /// </summary>
        [XmlIgnore]
        public string Valor
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

        /// <summary>
        ///     Descrição do valor primeiro do parâmetro - Usado em tela
        /// </summary>
        [XmlIgnore]
        public string DesValor
        {
            get
            {
                if (Valores.Count == 0)
                {
                    Valores.Add(new ParametroExecucaoValor());
                }

                var vals = new string[Valores.Count];
                for (var i = 0; i < Valores.Count; i++)
                {
                    vals[i] = Valores[i].DescricaoValor;
                }

                return string.Join(", ", vals);
            }
            set
            {
                if (Valores.Count == 0)
                {
                    Valores.Add(new ParametroExecucaoValor());
                }
                Valores[0].DescricaoValor = value;
            }
        }

        /// <summary>
        ///     Retorna o valor do parâmetro em int
        /// </summary>
        [XmlIgnore]
        public int? ValorInt
        {
            get
            {
                int dummy;
                if (int.TryParse(Valor, out dummy))
                    return dummy;

                return null;
            }
            set { Valor = Convert.ToString(value); }
        }

        /// <summary>
        ///     Retorna o valor do parâmetro em decimal
        /// </summary>
        [XmlIgnore]
        public decimal? ValorDecimal
        {
            get
            {
                decimal dummy;
                if (decimal.TryParse(Valor, out dummy))
                    return dummy;

                return null;
            }
            set { Valor = Convert.ToString(value); }
        }

        /// <summary>
        ///     Retorna o valor do parâmetro em booleano
        /// </summary>
        [XmlIgnore]
        public bool ValorBool
        {
            get
            {
                bool dummy;
                if (bool.TryParse(Valor, out dummy))
                    return dummy;

                return "1".Equals(Valor);
            }
            set { Valor = Convert.ToString(value); }
        }

        public bool ExibirParametro { get; set; }

        public void Adicionar(string valor, string desValor)
        {
            Valores.Add(new ParametroExecucaoValor {Valor = valor, DescricaoValor = desValor});
        }
    }
}