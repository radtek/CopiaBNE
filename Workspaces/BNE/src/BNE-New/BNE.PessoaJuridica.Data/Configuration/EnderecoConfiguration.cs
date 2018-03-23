using System.Data.Entity.ModelConfiguration;
using BNE.PessoaJuridica.Domain.Model;

namespace BNE.PessoaJuridica.Data.Configuration
{
    public class EnderecoConfiguration : EntityTypeConfiguration<Endereco>
    {
        public EnderecoConfiguration()
        {
            ToTable("Endereco", "pessoajuridica");
            HasKey(n => n.Id);
            Property(n => n.Id).HasColumnName("IdEndereco");
            Property(n => n.Logradouro).HasMaxLength(150).IsRequired();
            Property(n => n.Numero).HasMaxLength(20);
            Property(n => n.Complemento).HasMaxLength(50);
            Property(p => p.Geolocalizacao).HasColumnType("Geography").IsOptional();
            Property(n => n.DataAtualizacaoGeolocalizacao).IsOptional();
            Property(n => n.DescricaoBairro).HasMaxLength(100);
            HasOptional(n => n.Bairro).WithMany().Map(n => n.MapKey("IdBairro"));
            HasRequired(n => n.Cidade).WithMany().Map(n => n.MapKey("IdCidade"));
        }
    }
}