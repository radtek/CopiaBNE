using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace BNE.Global.Data.Configuration
{
    public class CargoGlobalConfiguration : EntityTypeConfiguration<Model.CargoGlobal>
    {
        public CargoGlobalConfiguration()
        {
            ToTable("Cargo", "global");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasColumnName("IdCargo").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(p => p.Descricao).HasMaxLength(50).IsRequired();
            this.Property(p => p.Prioridade).IsRequired();
            this.Property(p => p.FlgInativo).IsRequired();
            this.Property(p => p.DataCadastro).IsRequired();
            this.Property(p => p.DataAlteracao).IsOptional();

            this.HasRequired(p => p.RamoAtividadeGlobal).WithMany().Map(p => p.MapKey("IdRamoAtividadeGlobal"));
            this.HasRequired(p => p.CategoriaCargoGlobal).WithMany().Map(p => p.MapKey("IdCategoriaCargoGlobal"));
        }
    }
}