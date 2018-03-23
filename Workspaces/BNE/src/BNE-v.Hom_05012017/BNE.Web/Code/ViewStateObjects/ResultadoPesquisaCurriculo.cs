using System;

namespace BNE.Web.Code.ViewStateObjects
{
    [Serializable]
    public class ResultadoPesquisaCurriculo
    {

        public TipoPesquisa Tipo { get; set; }

        public ResultadoPesquisaCurriculoVaga Vaga { get; set; }
        public ResultadoPesquisaCurriculoRastreador Rastreador { get; set; }
        public ResultadoPesquisaCurriculoCurriculo Curriculo { get; set; }

        public bool PorVaga()
        {
            return Tipo == TipoPesquisa.Vaga;
        }
        public bool PorRastreador()
        {
            return Tipo == TipoPesquisa.Rastreador;
        }
        public bool PorCurriculo()
        {
            return Tipo == TipoPesquisa.Curriculo;
        }


        public ResultadoPesquisaCurriculo()
        {
            Tipo = TipoPesquisa.Nenhum;
        }
        public ResultadoPesquisaCurriculo(ResultadoPesquisaCurriculoVaga vaga)
        {
            Tipo = TipoPesquisa.Vaga;
            Vaga = vaga;
        }
        public ResultadoPesquisaCurriculo(ResultadoPesquisaCurriculoRastreador rastreador)
        {
            Tipo = TipoPesquisa.Rastreador;
            Rastreador = rastreador;
        }
        public ResultadoPesquisaCurriculo(ResultadoPesquisaCurriculoCurriculo curriculo)
        {
            Tipo = TipoPesquisa.Curriculo;
            Curriculo = curriculo;
        }

        public enum TipoPesquisa
        {
            Nenhum,
            Vaga,
            Curriculo,
            Rastreador
        }

    }

    [Serializable]
    public class ResultadoPesquisaCurriculoVaga
    {
        public int IdVaga { get; set; }
        public bool Campanha { get; set; }
        public bool BancoCurriculo { get; set; }
        public bool InscritosNaoLidos { get; set; }
    }

    [Serializable]
    public class ResultadoPesquisaCurriculoRastreador
    {
        public int IdRastreadorCurriculo { get; set; }
        public DateTime? DataVisualizacao { get; set; }
    }

    [Serializable]
    public class ResultadoPesquisaCurriculoCurriculo
    {
        public int IdPesquisaCurriculo { get; set; }
    }

}