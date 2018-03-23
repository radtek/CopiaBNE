using System.Data.Entity.ModelConfiguration;

namespace BNE.PessoaJuridica.Data.Configuration
{
    public class EnderecoConfiguration : EntityTypeConfiguration<Model.Endereco>
    {

        public EnderecoConfiguration()
        {
            ToTable("Endereco", "pessoajuridica");
            this.HasKey(n => n.Id);
            this.Property(n => n.Id).HasColumnName("IdEndereco");
            this.Property(n => n.Logradouro).HasMaxLength(150).IsRequired();
            this.Property(n => n.Numero).HasMaxLength(20);
            this.Property(n => n.Complemento).HasMaxLength(50);
            this.Property(p => p.Geolocalizacao).HasColumnType("Geography").IsOptional();
            this.Property(n => n.DataAtualizacaoGeolocalizacao).IsOptional();
            this.Property(n => n.DescricaoBairro).HasMaxLength(100);
            this.HasOptional(n => n.Bairro).WithMany().Map(n => n.MapKey("IdBairro"));
            this.HasRequired(n => n.Cidade).WithMany().Map(n => n.MapKey("IdCidade"));
        }
    }
}