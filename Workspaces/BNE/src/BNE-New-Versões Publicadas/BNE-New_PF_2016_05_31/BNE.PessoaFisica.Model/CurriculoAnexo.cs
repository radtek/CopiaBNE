using System;

namespace BNE.PessoaFisica.Model
{
    public class CurriculoAnexo
    {
        public int Id { get; set; }
        public string UrlArquivo { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual Curriculo Curriculo { get; set; }
    }
}