using System.Data.Entity.ModelConfiguration;

namespace BNE.PessoaJuridica.Data.Configuration
{
    public class PessoaJuridicaConfiguration : EntityTypeConfiguration<Model.PessoaJuridica>
    {

        public PessoaJuridicaConfiguration()
        {
            ToTable("PessoaJuridica", "pessoajuridica");
            this.HasKey(n => n.Id);
            this.Property(n => n.Id).HasColumnName("IdPessoaJuridica");
            this.Property(n => n.RazaoSocial).HasMaxLength(120);
            this.Property(n => n.NomeFantasia).HasMaxLength(100);
            this.Property(n => n.Site).HasMaxLength(150);
            this.Property(n => n.SituacaoCadastral).HasMaxLength(20);
            this.Property(n => n.IP).HasMaxLength(15).IsRequired();
            this.Property(n => n.QuantidadeFuncionario).IsRequired();
            this.Property(n => n.DataAbertura).HasColumnType("DATE").IsOptional();
            this.Property(n => n.CNPJ).HasPrecision(14, 0).IsRequired();
            this.Property(n => n.DataAlteracao).IsRequired();
            this.Property(n => n.DataCadastro).IsRequired();
            this.HasOptional(n => n.CNAE).WithMany().Map(n => n.MapKey("CNAE"));
            this.HasOptional(n => n.NaturezaJuridica).WithMany().Map(n => n.MapKey("NaturezaJuridica"));
            this.HasOptional(n => n.Endereco).WithMany().Map(n => n.MapKey("IdEndereco"));
        }

    }
}
