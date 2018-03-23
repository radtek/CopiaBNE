using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BNE.PessoaFisica.Web.Models
{
    public class RamoAtividade
    {
        public int IdRamoAtividade { get; set; }
        public string Descricao { get; set; }

        public List<RamoAtividade> Listar()
        {
            return new List<RamoAtividade>
            {
                new RamoAtividade{IdRamoAtividade=0, Descricao="A Empresa trabalha com"},

                new RamoAtividade{IdRamoAtividade=22, Descricao="Acessórios"},
                new RamoAtividade{IdRamoAtividade=45, Descricao="Administração Pública "},
                new RamoAtividade{IdRamoAtividade=44, Descricao="Administrativo"},
                new RamoAtividade{IdRamoAtividade=1, Descricao="Agronegócios"},
                new RamoAtividade{IdRamoAtividade=25, Descricao="Água e Esgoto"},
                new RamoAtividade{IdRamoAtividade=3, Descricao="Alimentos"},
                new RamoAtividade{IdRamoAtividade=30, Descricao="Arte e Cultura"},
                new RamoAtividade{IdRamoAtividade=50, Descricao="Associações e Diversos"},
                new RamoAtividade{IdRamoAtividade=4, Descricao="Bebidas"},
                new RamoAtividade{IdRamoAtividade=57, Descricao="Biologia"},
                new RamoAtividade{IdRamoAtividade=27, Descricao="Comércio"},
                new RamoAtividade{IdRamoAtividade=31, Descricao="Comunicação"},
                new RamoAtividade{IdRamoAtividade=26, Descricao="Construção"},
                new RamoAtividade{IdRamoAtividade=37, Descricao="Consultoria"},
                new RamoAtividade{IdRamoAtividade=55, Descricao="Contabilidade"},
                new RamoAtividade{IdRamoAtividade=46, Descricao="Educação"},
                new RamoAtividade{IdRamoAtividade=18, Descricao="Elétrico"},
                new RamoAtividade{IdRamoAtividade=17, Descricao="Eletrônico"},
                new RamoAtividade{IdRamoAtividade=24, Descricao="Energia"},
                new RamoAtividade{IdRamoAtividade=49, Descricao="Esporte"},
                new RamoAtividade{IdRamoAtividade=2, Descricao="Extração"},
                new RamoAtividade{IdRamoAtividade=12, Descricao="Farmacêutico"},
                new RamoAtividade{IdRamoAtividade=34, Descricao="Financeiro"},
                new RamoAtividade{IdRamoAtividade=47, Descricao="Forças Armadas"},
                new RamoAtividade{IdRamoAtividade=5, Descricao="Fumo"},
                new RamoAtividade{IdRamoAtividade=9, Descricao="Gráfica"},
                new RamoAtividade{IdRamoAtividade=29, Descricao="Hotelaria e Turismo"},
                new RamoAtividade{IdRamoAtividade=35, Descricao="Imobiliária"},
                new RamoAtividade{IdRamoAtividade=33, Descricao="Informática"},
                new RamoAtividade{IdRamoAtividade=53, Descricao="Internacional"},
                new RamoAtividade{IdRamoAtividade=36, Descricao="Jurídico"},
                new RamoAtividade{IdRamoAtividade=43, Descricao="Limpeza"},
                new RamoAtividade{IdRamoAtividade=40, Descricao="Locação"},
                new RamoAtividade{IdRamoAtividade=28, Descricao="Logística"},
                new RamoAtividade{IdRamoAtividade=7, Descricao="Madeira"},
                new RamoAtividade{IdRamoAtividade=23, Descricao="Manutenção"},
                new RamoAtividade{IdRamoAtividade=38, Descricao="Marketing"},
                new RamoAtividade{IdRamoAtividade=19, Descricao="Mecânico"},
                new RamoAtividade{IdRamoAtividade=16, Descricao="Metal Mecânico"},
                new RamoAtividade{IdRamoAtividade=15, Descricao="Metalurgia"},
                new RamoAtividade{IdRamoAtividade=14, Descricao="Minerais"},
                new RamoAtividade{IdRamoAtividade=21, Descricao="Móveis"},
                new RamoAtividade{IdRamoAtividade=58, Descricao="Não Informado"},
                new RamoAtividade{IdRamoAtividade=8, Descricao="Papel "},
                new RamoAtividade{IdRamoAtividade=10, Descricao="Petróleo"},
                new RamoAtividade{IdRamoAtividade=13, Descricao="Plástico"},
                new RamoAtividade{IdRamoAtividade=56, Descricao="Produção"},
                new RamoAtividade{IdRamoAtividade=11, Descricao="Químico"},
                new RamoAtividade{IdRamoAtividade=41, Descricao="Recursos Humanos"},
                new RamoAtividade{IdRamoAtividade=39, Descricao="Saúde"},
                new RamoAtividade{IdRamoAtividade=42, Descricao="Segurança"},
                new RamoAtividade{IdRamoAtividade=52, Descricao="Serviços Domésticos"},
                new RamoAtividade{IdRamoAtividade=51, Descricao="Serviços Pessoais"},
                new RamoAtividade{IdRamoAtividade=48, Descricao="Social"},
                new RamoAtividade{IdRamoAtividade=32, Descricao="Telecomunicações"},
                new RamoAtividade{IdRamoAtividade=6, Descricao="Têxteis"},
                new RamoAtividade{IdRamoAtividade=20, Descricao="Veículos"},
            };
        }
    }
}