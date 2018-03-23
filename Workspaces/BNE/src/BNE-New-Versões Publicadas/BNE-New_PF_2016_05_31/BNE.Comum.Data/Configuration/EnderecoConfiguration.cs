using System.Data.Entity.ModelConfiguration;

namespace BNE.Comum.Data.Configuration
{
    public class EnderecoConfigurationOld : EntityTypeConfiguration<Model.EnderecoComum>
    {

        public EnderecoConfiguration()
        {
            this.HasKey(n => n.Id);
            this.Property(n => n.Logradouro).HasMaxLength(150);
            this.Property(n => n.Numero).HasMaxLength(20);
            this.Property(n => n.Complemento).HasMaxLength(50);
            this.Property(p => p.Geolocalizacao).HasColumnType("Geography").IsOptional();
            this.Property(n => n.DataAtualizacaoGeolocalizacao).IsOptional();
        }
    }
}