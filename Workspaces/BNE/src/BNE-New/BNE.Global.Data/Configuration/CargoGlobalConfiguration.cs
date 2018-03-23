using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BNE.Global.Model;

namespace BNE.Global.Data.Configuration
{
    public class CargoGlobalConfiguration : EntityTypeConfiguration<CargoGlobal>
    {
        public CargoGlobalConfiguration()
        {
            ToTable("Cargo", "global");
            HasKey(p => p.Id);
            Property(p => p.Id).HasColumnName("IdCargo").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(p => p.Descricao).HasMaxLength(50).IsRequired();
            Property(p => p.Prioridade).IsRequired();
            Property(p => p.FlgInativo).IsRequired();
            Property(p => p.DataCadastro).IsRequired();
            Property(p => p.DataAlteracao).IsOptional();

            HasRequired(p => p.RamoAtividadeGlobal).WithMany().Map(p => p.MapKey("IdRamoAtividadeGlobal"));
            HasRequired(p => p.CategoriaCargoGlobal).WithMany().Map(p => p.MapKey("IdCategoriaCargoGlobal"));
        }
    }
}