using System.Data.Entity.ModelConfiguration;
using BNE.PessoaFisica.Domain.Model;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class EnderecoConfiguration : EntityTypeConfiguration<Endereco>
    {
        public EnderecoConfiguration()
        {
            ToTable("Endereco", "pessoafisica");
            HasKey(n => n.Id);
            Property(n => n.Id).HasColumnName("IdEndereco");
            Property(n => n.Logradouro).HasMaxLength(150);
            Property(n => n.Numero).HasMaxLength(20);
            Property(n => n.Complemento).HasMaxLength(50);
            Property(p => p.Geolocalizacao).HasColumnType("Geography").IsOptional();
            Property(n => n.DataAtualizacaoGeolocalizacao).IsOptional();
        }
    }
}