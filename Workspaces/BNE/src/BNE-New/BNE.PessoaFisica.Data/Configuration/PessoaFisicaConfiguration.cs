using System.Data.Entity.ModelConfiguration;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class PessoaFisicaConfiguration : EntityTypeConfiguration<Domain.Model.PessoaFisica>
    {
        public PessoaFisicaConfiguration()
        {
            ToTable("PessoaFisica", "pessoafisica");
            Property(p => p.Id).HasColumnName("IdPessoaFisica");
            HasKey(p => p.Id);
            Property(p => p.Nome).HasMaxLength(100).IsRequired();
            Property(p => p.CPF).HasPrecision(11, 0).IsRequired();
            Property(p => p.DataNascimento).HasColumnType("DATE").IsRequired();
            Property(p => p.DataCadastro).IsRequired();
            Property(p => p.DataAlteracao).IsOptional();
            Property(p => p.RG).HasMaxLength(20).IsOptional();
            Property(p => p.OrgaoEmissor).HasMaxLength(50).IsOptional();
            Property(p => p.QtdFilhos).IsOptional();
            Property(p => p.FlgPossuiFilhos).IsOptional();
            Property(p => p.CNH).HasMaxLength(15).IsOptional();
            Property(p => p.CategoriaHabilitacao).HasMaxLength(2).IsOptional();


            HasOptional(p => p.Cidade).WithMany().Map(p => p.MapKey("IdCidade"));
            HasOptional(p => p.Sexo).WithMany().Map(p => p.MapKey("SiglaSexo"));
            HasOptional(p => p.Endereco).WithMany().Map(p => p.MapKey("IdEndereco"));
            HasOptional(p => p.DeficienciaGlobal).WithMany().Map(p => p.MapKey("IdDeficienciaGlobal"));
            HasOptional(p => p.EscolaridadeGlobal).WithMany().Map(p => p.MapKey("IdEscolaridadeGlobal"));
            HasOptional(p => p.EstadoCivil).WithMany().Map(p => p.MapKey("IdEstadoCivil"));
        }
    }
}