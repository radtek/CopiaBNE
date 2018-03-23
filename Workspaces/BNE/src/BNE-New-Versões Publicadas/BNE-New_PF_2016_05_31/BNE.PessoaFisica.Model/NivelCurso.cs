namespace BNE.PessoaFisica.Model
{
    public class NivelCurso
    {
        public byte Id { get; set; }
        public string Descricao { get; set; }

        public virtual Global.Model.GrauEscolaridadeGlobal GrauEscolaridadeGlobal { get; set; }
    }
}