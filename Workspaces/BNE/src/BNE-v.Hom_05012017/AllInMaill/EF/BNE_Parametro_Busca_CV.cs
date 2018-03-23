namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Parametro_Busca_CV")]
    public partial class BNE_Parametro_Busca_CV
    {
        [Key]
        public int Idf_Parametro_Busca_CV { get; set; }

        public bool Flg_Idf_Curriculo { get; set; }

        public bool Flg_Num_CPF { get; set; }

        public bool Flg_Eml_Pessoa { get; set; }

        public bool Flg_Idf_Funcao { get; set; }

        public bool Flg_Idf_Cidade { get; set; }

        public bool Flg_Sig_Estado { get; set; }

        public bool Flg_Peso_Escolaridade { get; set; }

        public bool Flg_Idf_Sexo { get; set; }

        public bool Flg_Idade_Min { get; set; }

        public bool Flg_Idade_Max { get; set; }

        public bool Flg_Salario_Min { get; set; }

        public bool Flg_Salario_Max { get; set; }

        public bool Flg_Meses_Exp { get; set; }

        public bool Flg_Nme_Pessoa { get; set; }

        public bool Flg_Des_Bairro { get; set; }

        public bool Flg_Des_Logradouro { get; set; }

        public bool Flg_Num_CEP_Min { get; set; }

        public bool Flg_Num_CEP_Max { get; set; }

        public bool Flg_Experiencia_Em { get; set; }

        public bool Flg_Des_Curso { get; set; }

        public bool Flg_Nme_Fonte { get; set; }

        public bool Flg_Des_Curso_Outros { get; set; }

        public bool Flg_Nme_Fonte_Outros { get; set; }

        public bool Flg_Nme_Empresa { get; set; }

        public bool Flg_Idf_Area_BNE { get; set; }

        public bool Flg_Idf_Categoria_Habilitacao { get; set; }

        public bool Flg_Idf_Tipo_Veiculo { get; set; }

        public bool Flg_Num_DDD { get; set; }

        public bool Flg_Num_Telefone { get; set; }

        public bool Flg_Idf_Deficiencia { get; set; }

        public bool Flg_Des_MetaBusca { get; set; }

        public bool Flg_Des_Metabusca_Rapida { get; set; }

        public bool Flg_Idf_Origem { get; set; }

        public bool Flg_Idf_Estado_Civil { get; set; }

        public bool Flg_Idf_Filial { get; set; }

        public bool Flg_Idfs_Idioma { get; set; }

        public bool Flg_Idfs_Disponibilidade { get; set; }

        public bool Flg_Idf_Raca { get; set; }

        public bool Flg_Flg_Filhos { get; set; }

        public bool Flg_Idf_Vaga { get; set; }

        public bool Flg_Idf_Rastreador { get; set; }

        public bool Flg_Inativo { get; set; }

        [StringLength(200)]
        public string Nme_SP_Busca { get; set; }
    }
}
