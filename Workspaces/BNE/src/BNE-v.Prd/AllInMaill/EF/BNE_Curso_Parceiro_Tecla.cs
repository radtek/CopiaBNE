namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Curso_Parceiro_Tecla")]
    public partial class BNE_Curso_Parceiro_Tecla
    {
        public BNE_Curso_Parceiro_Tecla()
        {
            BNE_Inscricao_Curso = new HashSet<BNE_Inscricao_Curso>();
        }

        [Key]
        public int Idf_Curso_Parceiro_Tecla { get; set; }

        public int Idf_Curso_Tecla { get; set; }

        public int Idf_Parceiro_Tecla { get; set; }

        [Required]
        [StringLength(1000)]
        public string Des_URL_Curso_Tecla { get; set; }

        public DateTime Dta_Inclusao { get; set; }

        public bool Flg_Inativo { get; set; }

        [Required]
        [StringLength(3000)]
        public string Des_Curso { get; set; }

        [Required]
        [StringLength(3000)]
        public string Des_Conteudo { get; set; }

        [Required]
        [StringLength(500)]
        public string Des_Caminho_Imagem_Banner { get; set; }

        [Required]
        [StringLength(500)]
        public string Des_Caminho_Imagem_Miniatura { get; set; }

        [Required]
        [StringLength(150)]
        public string Des_Titulo_Curso { get; set; }

        [Required]
        [StringLength(250)]
        public string Des_Publico_Alvo { get; set; }

        [StringLength(150)]
        public string Des_Instrutor_Curso { get; set; }

        [StringLength(200)]
        public string Des_Assinatura_Instrutor_Curso { get; set; }

        public bool Flg_Certificado { get; set; }

        public int Qtd_Carga_Horaria { get; set; }

        public decimal? Vlr_Curso_Sem_Desconto { get; set; }

        public decimal Vlr_Curso { get; set; }

        public int? Qtd_Parcela { get; set; }

        public decimal? Vlr_Curso_Parcela { get; set; }

        public int Idf_Curso_Modalidade_Tecla { get; set; }

        public virtual BNE_Curso_Modalidade_Tecla BNE_Curso_Modalidade_Tecla { get; set; }

        public virtual BNE_Parceiro_Tecla BNE_Parceiro_Tecla { get; set; }

        public virtual BNE_Curso_Tecla BNE_Curso_Tecla { get; set; }

        public virtual ICollection<BNE_Inscricao_Curso> BNE_Inscricao_Curso { get; set; }
    }
}
