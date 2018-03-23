using System.Data.Entity.ModelConfiguration;
using BNE.Global.Model;

namespace BNE.Global.Data.Configuration
{
    public class OrigemGlobalConfiguration : EntityTypeConfiguration<OrigemGlobal>
    {
        public OrigemGlobalConfiguration()
        {
            ToTable("Origem", "global");
            Property(p => p.Id).HasColumnName("IdOrigem");
            HasKey(p => p.Id);
            Property(p => p.Ativo).IsRequired();
            Property(p => p.DataCadastro).IsRequired();
            Property(p => p.Descricao).HasMaxLength(100).IsOptional();
            Property(p => p.URL).HasMaxLength(120).IsOptional();

            HasRequired(p => p.TipoOrigem).WithMany().Map(p => p.MapKey("IdTipoOrigem"));
        }
    }
}