using System.Data.Entity.ModelConfiguration;

namespace BNE.PessoaJuridica.Data.Configuration
{
    public class CNAEConfiguration : EntityTypeConfiguration<Model.CNAE>
    {

        public CNAEConfiguration()
        {
            ToTable("CNAE", "pessoajuridica");
            this.HasKey(n => n.Codigo);
            this.Property(n => n.Codigo).HasMaxLength(7);
            this.Property(n => n.Descricao).HasMaxLength(175).IsRequired();
        }

    }
}
