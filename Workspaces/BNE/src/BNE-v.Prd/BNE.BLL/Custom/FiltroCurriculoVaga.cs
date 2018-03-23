using System;
using System.Collections.Generic;

namespace BNE.BLL.Custom
{
    [Serializable]
    public class FiltroCurriculoVaga
    {
        public FiltroCurriculoVaga()
        {
            this.ListCidades = new List<ListCidade>();
            this.ListFuncoes = new List<ListFuncao>();
            this.ListEscolaridade = new List<EscolaridadeFiltro>();
        }
        public List<ListFuncao> ListFuncoes { get; set; }
        public List<ListCidade> ListCidades { get; set; }
        public int? IdadeMinima { get; set; }
        public int? IdadeMaxima { get; set; }
        public decimal? SalarioMinimo { get; set; }
        public decimal? SalarioMaximo { get; set; }
        public List<EscolaridadeFiltro> ListEscolaridade { get; set; }
        public int? Sexo { get; set; }
        public string PalavraChave { get; set; }
        public bool? visualizados { get; set; }
        public bool PCD { get; set; }
        public bool RespostaCorreta { get; set; }
    }

    [Serializable]
    public class ListFuncao
    {
        public ListFuncao(int IdFuncao, string desFuncao)
        {
            this.DesFuncao = desFuncao;
            this.IdFuncao = IdFuncao;
        }
        public int IdFuncao { get; set; }
        public string DesFuncao { get; set; }
    }

    [Serializable]
    public class ListCidade
    {
        public ListCidade(int idCidade, string desCidade)
        {
            this.DesCidade = desCidade;
            this.IdCidade = idCidade;
        }
        public int IdCidade { get; set; }
        public string DesCidade { get; set; }
    }

    [Serializable]
    public class EscolaridadeFiltro
    {
        public EscolaridadeFiltro(int idEscolaridade, string desEscolaridade)
        {
            this.IdEscolaridade = idEscolaridade;
            this.DesEscolaridade = desEscolaridade;
        }
        public int IdEscolaridade { get; set; }
        public string DesEscolaridade { get; set; }
    }
}
