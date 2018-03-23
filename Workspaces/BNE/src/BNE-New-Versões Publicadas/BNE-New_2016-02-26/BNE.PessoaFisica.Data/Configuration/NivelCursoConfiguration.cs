using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class NivelCursoConfiguration : EntityTypeConfiguration<Model.NivelCurso>
    {
        public NivelCursoConfiguration()
        {
            ToTable("NivelCurso", "pessoafisica");
            this.Property(p => p.Id).HasColumnName("IdNivelCurso");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(p=>p.Descricao).HasMaxLength(50).IsRequired();

            this.HasRequired(p => p.GrauEscolaridadeGlobal).WithMany().Map(p => p.MapKey("IdGrauEscolaridadeGlobal"));
        }
    }
}