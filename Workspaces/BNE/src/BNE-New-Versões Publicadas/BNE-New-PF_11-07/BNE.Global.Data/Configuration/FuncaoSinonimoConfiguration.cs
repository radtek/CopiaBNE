using System.Data.Entity.ModelConfiguration;

namespace BNE.Global.Data.Configuration
{
    public class FuncaoSinonimoConfiguration : EntityTypeConfiguration<Model.FuncaoSinonimo>
    {

        public FuncaoSinonimoConfiguration()
        {
            ToTable("FuncaoSinonimo", "global");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasColumnName("IdFuncaoSinonimo");
            this.Property(p => p.NomeSinonimo).HasMaxLength(100);
            this.Property(p => p.DescricaoPesquisa).HasMaxLength(100).IsRequired();
            this.Property(p => p.IdSinonimoSubstituto).IsOptional();
            this.Property(p => p.CodigoCBO).HasMaxLength(6).IsRequired();
            this.Property(p => p.Atribuicoes).IsMaxLength().IsOptional();
            this.Property(p => p.Responsabilidades).IsMaxLength().IsOptional();
            this.Property(p => p.DescricaoJob).HasMaxLength(2000).IsOptional();
            this.Property(p => p.Beneficio).IsMaxLength().IsOptional();
            this.Property(p => p.FlgAuditada).IsRequired();
            this.Property(p => p.FlgInativo).IsRequired();

            this.Property(p => p.DataCadastro).IsRequired();
            this.Property(p => p.DataAlteracao).IsOptional();

            this.HasRequired(p => p.TipoFuncaoGlobal).WithMany().Map(p => p.MapKey("IdTipoFuncaoGlobal"));
            this.HasRequired(p => p.EscolaricadeGlobal).WithMany().Map(p => p.MapKey("IdEscolaricadeGlobal"));
            this.HasRequired(p => p.FuncaoGlobal).WithMany().Map(p => p.MapKey("IdFuncaoGlobal"));

        }
    }
}