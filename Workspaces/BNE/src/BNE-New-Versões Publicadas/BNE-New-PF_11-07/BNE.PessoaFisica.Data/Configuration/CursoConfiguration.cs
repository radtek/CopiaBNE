using BNE.PessoaFisica.Model;
using System.Data.Entity.ModelConfiguration;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class CursoConfiguration : EntityTypeConfiguration<Curso>
    {
        public CursoConfiguration()
        {
            ToTable("Curso", "pessoafisica");
            this.Property(p => p.Id).HasColumnName("IdCurso");
            this.HasKey(p => p.Id);
            this.Property(p => p.CodigoCurso).HasMaxLength(50).IsOptional();
            this.Property(p => p.Descricao).HasMaxLength(100).IsRequired();
            this.Property(p => p.Ativo).IsRequired();
            this.Property(p => p.FlgAuditado).IsRequired();
            this.Property(p => p.FlgMEC).IsRequired();

            this.HasRequired(p => p.NivelCurso).WithMany().Map(p=>p.MapKey("IdNivelCurso"));
        }
    }
}