using System;

namespace BNE.PessoaFisica.ApplicationService.Curriculo.Model
{
    public class InformacaoCurriculoResponse
    {
        public bool CurriculoVIP { get; set; }
        public int IdCurriculo { get; set; }
        public bool EmpresaBloqueada { get; set; }
        public bool JaEnvioCvParaVaga { get; set; }
        public bool IndicadoBNE { get; set; }
        public bool EstaNaRegiaoBH { get; set; }
        public bool TemExperienciaProfissional { get; set; }
        public bool TemFormacao { get; set; }
        public int SaldoCandidatura { get; set; }
        public bool DisseQueNaoTemExperiencia { get; set; }
        public DateTime? DataNaoTemExperiencia { get; set; }
        
    }
}