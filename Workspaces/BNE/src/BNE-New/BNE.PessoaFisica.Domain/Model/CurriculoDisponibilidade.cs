namespace BNE.PessoaFisica.Domain.Model
{
    public class CurriculoDisponibilidade
    {
        public int IdCurriculo { get; set; }
        public byte IdDisponibilidadeGlobal { get; set; }

        public virtual Global.Model.DisponibilidadeGlobal DisponibilidadeGlobal { get; set; }
        public virtual Curriculo Curriculo { get; set; }

    }
}