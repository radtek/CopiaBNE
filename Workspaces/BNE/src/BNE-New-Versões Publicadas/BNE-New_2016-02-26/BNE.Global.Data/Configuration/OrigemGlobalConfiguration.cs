using System.Data.Entity.ModelConfiguration;

namespace BNE.Global.Data.Configuration
{
    public class OrigemGlobalConfiguration : EntityTypeConfiguration<Model.OrigemGlobal>
    {
        public OrigemGlobalConfiguration()
        {
            ToTable("Origem", "global");
            this.Property(p => p.Id).HasColumnName("IdOrigem");
            this.HasKey(p => p.Id);
            this.Property(p => p.Ativo).IsRequired();
            this.Property(p => p.DataCadastro).IsRequired();
            this.Property(p => p.Descricao).HasMaxLength(100).IsOptional();
            this.Property(p => p.URL).HasMaxLength(120).IsOptional();

            this.HasRequired(p => p.TipoOrigem).WithMany().Map(p=>p.MapKey("IdTipoOrigem"));
        }
    }
}