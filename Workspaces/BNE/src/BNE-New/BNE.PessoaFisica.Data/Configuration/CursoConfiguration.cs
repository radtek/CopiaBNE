using System.Data.Entity.ModelConfiguration;
using BNE.PessoaFisica.Domain.Model;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class CursoConfiguration : EntityTypeConfiguration<Curso>
    {
        public CursoConfiguration()
        {
            ToTable("Curso", "pessoafisica");
            Property(p => p.Id).HasColumnName("IdCurso");
            HasKey(p => p.Id);
            Property(p => p.CodigoCurso).HasMaxLength(50).IsOptional();
            Property(p => p.Descricao).HasMaxLength(100).IsRequired();
            Property(p => p.Ativo).IsRequired();
            Property(p => p.FlgAuditado).IsRequired();
            Property(p => p.FlgMEC).IsRequired();

            HasRequired(p => p.NivelCurso).WithMany().Map(p => p.MapKey("IdNivelCurso"));
        }
    }
}