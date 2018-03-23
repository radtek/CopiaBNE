using System.Data.Entity.ModelConfiguration;

namespace BNE.Global.Data.Configuration
{
    public class FuncaoConfiguration : EntityTypeConfiguration<Model.Funcao>
    {
        public FuncaoConfiguration()
        {
            ToTable("Funcao", "global");
            this.Property(p => p.Id).HasColumnName("IdFuncao");
            this.HasKey(p => p.Id);
            this.Property(p => p.Descricao).HasMaxLength(100).IsRequired();
            this.Property(p=>p.Prioridade).IsRequired();
            this.Property(p=>p.DataAlteracao).IsOptional();
            this.Property(p=>p.DataCadastro).IsRequired();
            this.Property(p=>p.FlgInativo).IsRequired();

            this.HasRequired(p => p.CargoGlobal).WithMany().Map(p=>p.MapKey("IdCargoGlobal"));
        }
    }
}