using System;

namespace BNE.PessoaFisica.Domain.Model
{
    public class CurriculoParametro
    {
        public int Id { get; set; }
        public string Valor { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual Curriculo Curriculo { get; set; }
        public virtual Parametro Parametro { get; set; }

        /// <summary>
        /// Desconta a candidatura, até hoje é unico parametro, foi portado para a nova arquitetura e a logica ficou para esse parametro apenas
        /// </summary>
        public void Descontar()
        {
            int garbage;
            if (int.TryParse(Valor, out garbage))
            {
                if (garbage > 0)
                {
                    Valor = (garbage - 1).ToString();
                }
            }
        }
    }
}