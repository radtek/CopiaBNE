using System.Data.Entity.ModelConfiguration;

namespace BNE.Mensagem.Data.Configuration
{
    class AnexoConfiguration : EntityTypeConfiguration<Model.Anexo>
    {

        public AnexoConfiguration()
        {
            ToTable("Anexo", "mensagem");
            this.Property(n => n.Id).HasColumnName("IdAnexo");
            this.Property(n => n.Nome).HasMaxLength(50).IsRequired();
            this.Property(n => n.Url).HasMaxLength(1000).IsRequired();
        }

    }
}
