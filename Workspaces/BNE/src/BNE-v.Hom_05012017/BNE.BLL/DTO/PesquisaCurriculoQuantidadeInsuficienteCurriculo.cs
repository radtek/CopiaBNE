using System.Text;

namespace BNE.BLL.DTO
{
    public class PesquisaCurriculoQuantidadeInsuficienteCurriculo
    {
        public string NumeroCEPMaximo { get; set; }
        public string Funcao { get; set; }
        public string Cidade { get; set; }
        public string PalavraChave { get; set; }
        public string SiglaEstado { get; set; }
        public decimal? SalarioMinimo { get; set; }
        public decimal? SalarioMaximo { get; set; }
        public short? IdadeMaxima { get; set; }
        public short? IdadeMinima { get; set; }
        public string NumeroCEPMinimo { get; set; }
        public bool? PossuiFilhos { get; set; }
        public string TipoVeiculo { get; set; }
        public string CategoriaHabilitacao { get; set; }
        public string Deficiencia { get; set; }
        public string Raca { get; set; }
        public string AreaEmpresa { get; set; }
        public string EstadoCivil { get; set; }
        public string Empresa { get; set; }
        public string Sexo { get; set; }
        public string Escolaridade { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string CursoTecnicoGraduacao { get; set; }
        public string NomeFonteTecnicoGraduacao { get; set; }
        public string CursoOutros { get; set; }
        public string NomeFonteOutros { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            var props = this.GetType().GetProperties();

            foreach (var prop in props)
            {
                var value = prop.GetValue(this, null);
                if (value != null)
                {
                    sb.AppendFormat("{0}: {1}, ", prop.Name, value);
                }
            }

            return sb.ToString();
        }
    }
}
