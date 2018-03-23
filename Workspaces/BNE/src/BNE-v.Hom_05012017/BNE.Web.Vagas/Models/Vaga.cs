using BNE.Web.Vagas.Code.Helpers.SEO;
using BNE.BLL.Custom;
using System;
using System.Collections.Generic;
using BNE.BLL.Common;

namespace BNE.Web.Vagas.Models
{
    [Serializable]
    public class Vaga
    {
        public int IdentificadorVaga { get; set; }
        public string Codigo { get; set; }
        public string Funcao { get; set; }
        public string Cidade { get; set; }
        public string NomeCidade { get; set; }
        public string NomeEstado { get; set; }
        public string SiglaEstado { get; set; }
        public string Empresa { get; set; }
        public decimal? SalarioInicial { get; set; }
        public decimal? SalarioFinal { get; set; }
        public int QuantidadeVaga { get; set; }
        public string Atribuicao { get; set; }
        public string Beneficio { get; set; }
        public string Requisito { get; set; }
        public string Escolaridade { get; set; }
        public DateTime DataAnuncio { get; set; }
        public string URL { get; set; }
        public string URLIconeFacebook { get; set; }
        public int? IdentificadorDeficiencia { get; set; }
        public string DescricaoDeficiencia { get; set; }
        public string Disponibilidade { get; set; }
        public string TipoVinculo { get; set; }
        public int IdentificadorOrigem { get; set; }
        public int TipoOrigem { get; set; }
        public bool BNERecomenda { get; set; }
        public bool Candidatou { get; set; }
        public string AreaBNE { get; set; }
        public bool FlagInativo { get; set; }
        public bool FlagVagaArquivada { get; set; }
        public bool VagaLivre { get; set; }
        public string MediaSalarial { get; set; }
        public int IdFilial { get; set; }
        public bool Premium { get; set; }//**Nâo é campo da tabela**
        public string Bairro { get; set; }
        public bool MobileAcess { get; set; }//**Nâo é campo da tabela**

        //public string DescricaoDeficiencia { get; set; }

        //public List<DeficienciaModel> Deficiencias {get; set;}

        public SEOLink LinkVaga
        {
            get
            {
                if (ContratoDeEstagio && "Estagiário" != this.Funcao)
                {
                    return new SEOLink()
                    {
                        Descricao = this.ToString("Estágio para {Funcao}"),
                        Title = this.ToString("Vaga de estágio para {Funcao} em {Cidade}"),
                        URL = this.URL
                    };
                }
                if (this.FlagVagaArquivada == true)
                {
                    return new SEOLink()
                    {
                        Descricao = this.ToString("Oportunidade de {Funcao}"),
                        Title = this.ToString("Oportunidade de emprego para {Funcao} em {Cidade}"),
                        URL = this.URL
                    };
                }
                return new SEOLink()
                {

                    Descricao = this.ToString("Vaga de {Funcao}"),
                    Title = this.ToString("Vaga de emprego para {Funcao} em {Cidade}"),
                    URL = this.URL
                };
            }
        }

        public Boolean ContratoDeEstagio
        {
            get
            {
                return !string.IsNullOrEmpty(this.TipoVinculo) &&
                        this.TipoVinculo.IndexOf("Estágio", StringComparison.OrdinalIgnoreCase) > -1;
            }
        }

        public Vaga ShallowCopy()
        {
            return (Vaga)this.MemberwiseClone();
        }

   
    }

    [Serializable]
    public class DeficienciaModel
    {
        public int idDeficienciaDetalhe { get; set; }
        public int idDeficiencia { get; set; }
        public string DesDeficienciaDetalhe { get; set; }
        public bool FlgInativo { get; set; }
    }
}