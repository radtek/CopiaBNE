namespace AdminLTE_Application
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class VWVagas
    {
        public decimal num_cnpj { get; set; }

        [Key]
        public int Codigo { get; set; }

        public int qtd_Vaga { get; set; }

        [StringLength(100)]
        public string Funcao { get; set; }

        [StringLength(100)]
        public string Cidade { get; set; }

        [StringLength(2)]
        public string UF { get; set; }

        [StringLength(100)]
        public string eml_Vaga { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? Dta_Abertura { get; set; }

        [StringLength(400)]
        public string url { get; set; }

        public int Num_Cvs_Recebidos { get; set; }

        public int Num_Cvs_Recebidos_Nao_Lidos { get; set; }

        public int Qtd_Curriculos_Perfil { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? Dta_Cadastro { get; set; }

        [StringLength(50)]
        public string Origem { get; set; }

        [StringLength(30)]
        public string Status_vaga { get; set; }

        [StringLength(20)]
        public string Publicacao_Imediata { get; set; }

        [StringLength(50)]
        public string Campanha { get; set; }

        public bool Ativa { get; set; }

        public int Qtd_Visualizacao_Completa_VIP { get; set; }
        public int Qtd_Visualizacao_Completa_Nao_VIP { get; set; }
        public int Qtd_Visualizacao_Parcial_VIP { get; set; }
        public int Qtd_Visualizacao_Parcial_Nao_VIP { get; set; }

        [StringLength(120)]
        public string Nome { get; set; }

        [StringLength(120)]
        public string TelefoneVagaRapida { get; set; }

        [StringLength(120)]
        public string nme_Contato { get; set; }

        [StringLength(20)]
        public string TelefoneUsuarioFilial { get; set; }

        public bool Confidencial { get; set; }

    }
}
