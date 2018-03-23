using System.Data.Entity.ModelConfiguration;

namespace BNE.PessoaJuridica.Data.Configuration
{
    public class NaturezaJuridicaConfiguration : EntityTypeConfiguration<Model.NaturezaJuridica>
    {

        public NaturezaJuridicaConfiguration()
        {
            ToTable("NaturezaJuridica", "pessoajuridica");
            this.HasKey(n => n.Codigo);
            this.Property(n => n.Codigo).HasMaxLength(4);
            this.Property(n => n.Descricao).HasMaxLength(75).IsRequired();
        }

    }
}
