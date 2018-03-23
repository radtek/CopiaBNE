using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BNE.PessoaFisica.Domain.Model;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class NivelCursoConfiguration : EntityTypeConfiguration<NivelCurso>
    {
        public NivelCursoConfiguration()
        {
            ToTable("NivelCurso", "pessoafisica");
            Property(p => p.Id).HasColumnName("IdNivelCurso");
            HasKey(p => p.Id);
            Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(p => p.Descricao).HasMaxLength(50).IsRequired();

            HasRequired(p => p.GrauEscolaridadeGlobal).WithMany().Map(p => p.MapKey("IdGrauEscolaridadeGlobal"));
        }
    }
}