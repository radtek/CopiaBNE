namespace BNE.PessoaFisica.Domain.Model
{
    public class Curso
    {
        public int Id { get; set; }
        public string CodigoCurso { get; set; }
        public string Descricao { get; set; }
        public bool FlgMEC { get; set; }
        public bool Ativo { get; set; }
        public bool FlgAuditado { get; set; }
        
        public virtual NivelCurso NivelCurso { get; set; }
    }
}