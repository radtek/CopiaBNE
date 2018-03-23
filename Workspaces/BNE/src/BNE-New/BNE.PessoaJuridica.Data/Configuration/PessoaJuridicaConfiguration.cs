using System.Data.Entity.ModelConfiguration;

namespace BNE.PessoaJuridica.Data.Configuration
{
    public class PessoaJuridicaConfiguration : EntityTypeConfiguration<Domain.Model.PessoaJuridica>
    {
        public PessoaJuridicaConfiguration()
        {
            ToTable("PessoaJuridica", "pessoajuridica");
            HasKey(n => n.Id);
            Property(n => n.Id).HasColumnName("IdPessoaJuridica");
            Property(n => n.RazaoSocial).HasMaxLength(120);
            Property(n => n.NomeFantasia).HasMaxLength(100);
            Property(n => n.Site).HasMaxLength(150);
            Property(n => n.SituacaoCadastral).HasMaxLength(20);
            Property(n => n.IP).HasMaxLength(40).IsRequired();
            Property(n => n.QuantidadeFuncionario).IsRequired();
            Property(n => n.DataAbertura).HasColumnType("DATE").IsOptional();
            Property(n => n.CNPJ).HasPrecision(14, 0).IsRequired();
            Property(n => n.DataAlteracao).IsRequired();
            Property(n => n.DataCadastro).IsRequired();
            HasOptional(n => n.CNAE).WithMany().Map(n => n.MapKey("CNAE"));
            HasOptional(n => n.NaturezaJuridica).WithMany().Map(n => n.MapKey("NaturezaJuridica"));
            HasOptional(n => n.Endereco).WithMany().Map(n => n.MapKey("IdEndereco"));
        }
    }
}