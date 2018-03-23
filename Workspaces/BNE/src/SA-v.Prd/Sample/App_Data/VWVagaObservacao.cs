namespace AdminLTE_Application
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("VWVagaObservacao")]
    public partial class VWVagaObservacao
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Dta_Cadastro { get; set; }

        [StringLength(400)]
        public string Des_Observacao { get; set; }

        [StringLength(70)]
        public string Nme_Pessoa { get; set; }
            
           
        public int Idf_Vaga { get; set; }


        [Key]
        public int Idf_Vaga_Observacao { get; set; }

   

      

        

      

    }
}
