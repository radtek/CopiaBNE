using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.Solr.Vagas
{
    public class VagasSolr : SolrResponse<VagasSolr.DocVaga>
    {
        public class DocVaga : Docs
        {
            public int Id { get; set; }
            public string Cod_Vaga { get; set; }
            public int Qtd_Vaga { get; set; }
            public string Des_Atribuicoes { get; set; }
            public string Des_Beneficio { get; set; }
            public string Des_Deficiencia { get; set; }
            public string Des_Requisito { get; set; }
            public decimal Vlr_Salario_De { get; set; }
            public decimal Vlr_Salario_Para { get; set; }
            public string Url_Vaga { get; set; }
            public bool Flg_Confidencial { get; set; }
            public bool Flg_Vaga_Rapida { get; set; }
            public bool Flg_Vaga_Premium { get; set; }
            public bool Flg_BNE_Recomenda { get; set; }
            public bool Flg_Vaga_Massa { get; set; }
            public bool Flg_Liberada { get; set; }
            public bool Flg_Inativo { get; set; }
            public bool Flg_Auditada { get; set; }
            public bool Flg_Vaga_Arquivada { get; set; }
            public DateTime Dta_Cadastro { get; set; }
            public DateTime Dta_Abertura { get; set; }
            public DateTime Dta_Auditoria { get; set; }
            public DateTime Dta_Prazo { get; set; }
            public string Nme_Bairro { get; set; }
            public bool Flg_Plano { get; set; }
            public bool Flg_Oferece_Cursos { get; set; }

            public int[] Idf_Tipo_Vinculo { get; set; }
            public string[] Des_Tipo_Vinculo { get; set; }

            public int Idf_Escolaridade { get; set; }
            public string Des_BNE { get; set; }
            public string[] Des_Curso { get; set; }

            public int Idf_Origem { get; set; }
            public string Des_Origem { get; set; }

            public int Idf_Funcao { get; set; }
            public string Des_Funcao { get; set; }
            public int[] Idfs_Funcoes_Sinonimo { get; set; }

            public int Idf_Cidade { get; set; }
            public string Nme_Cidade { get; set; }
            public string Sig_Estado { get; set; }

            public int Idf_Filial { get; set; }
            public int Idf_Usuario_Filial_Perfil { get; set; }
            public string Geo_Localizacao { get; set; }
            public int Idf_Natureza_Juridica { get; set; }
            public string Des_Natureza_Juridica { get; set; }
            public string Raz_Social { get; set; }
            public string Nme_Empresa { get; set; }
            public string Des_Area_BNE { get; set; }
            public string Eml_Vaga { get; set; }
            public bool Flg_Empresa_Em_Auditoria { get; set; }
            public bool Flg_Receber_Todos_CV { get; set; }
            public bool Flg_Receber_Cada_CV { get; set; }
            public bool Campanha { get; set; }

        }

    }
}
