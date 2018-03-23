using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BNE.Web.Services.Vagas.DTO
{
    public class CurriculoCurtoDTO
    {
        public bool Vip { get; set; }
        public string Nome { get; set; }
        public string Sexo { get; set; }
        public string EstadoCivil { get; set; }
        public int Idade { get; set; }
        public string Escolaridade { get; set; }
        public decimal Pretensao { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Funcoes { get; set; }
        public string Experiencia { get; set; }
        public string Carteira { get; set; }
        public int IDCurriculo { get; set; }
    }
}