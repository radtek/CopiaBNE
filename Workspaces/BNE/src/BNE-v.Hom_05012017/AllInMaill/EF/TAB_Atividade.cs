namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Atividade")]
    public partial class TAB_Atividade
    {
        [Key]
        public int Idf_Atividade { get; set; }

        public int Idf_Plugins_Compatibilidade { get; set; }

        public int Idf_Status_Atividade { get; set; }

        public int? Idf_Usuario_Gerador { get; set; }

        [Column(TypeName = "xml")]
        public string Des_Parametros_Entrada { get; set; }

        [Column(TypeName = "xml")]
        public string Des_Parametros_Saida { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        [StringLength(255)]
        public string Des_Caminho_Arquivo_Upload { get; set; }

        [StringLength(255)]
        public string Des_Caminho_Arquivo_Gerado { get; set; }

        [Column(TypeName = "text")]
        public string Des_Erro { get; set; }

        public DateTime Dta_Agendamento { get; set; }

        public DateTime? Dta_Execucao { get; set; }

        public bool Flg_Inativo { get; set; }

        public int? Idf_Tipo_Saida { get; set; }

        public int? Idf_Filial { get; set; }

        public virtual BNE_Usuario BNE_Usuario { get; set; }

        public virtual TAB_Tipo_Saida TAB_Tipo_Saida { get; set; }

        public virtual TAB_Plugins_Compatibilidade TAB_Plugins_Compatibilidade { get; set; }

        public virtual TAB_Filial TAB_Filial { get; set; }

        public virtual TAB_Status_Atividade TAB_Status_Atividade { get; set; }
    }
}
