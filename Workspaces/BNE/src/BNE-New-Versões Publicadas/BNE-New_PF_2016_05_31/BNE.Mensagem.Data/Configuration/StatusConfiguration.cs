using System.Data.Entity.ModelConfiguration;

namespace BNE.Mensagem.Data.Configuration
{
    class StatusConfiguration : EntityTypeConfiguration<Model.Status>
    {

        public StatusConfiguration()
        {
            ToTable("Status", "mensagem");
            this.Property(n => n.Id).HasColumnName("IdStatus");
            this.Property(n => n.Descricao).HasMaxLength(20).IsRequired();

        }

    }
}
