using System.Collections;
using System.Data.Entity.ModelConfiguration;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class PessoaFisicaConfiguration : EntityTypeConfiguration<Model.PessoaFisica>
    {
        public PessoaFisicaConfiguration()
        {
            ToTable("PessoaFisica","pessoafisica");
            this.Property(p => p.Id).HasColumnName("IdPessoaFisica");
            this.HasKey(p => p.Id);
            this.Property(p => p.Nome).HasMaxLength(100).IsRequired();
            this.Property(p => p.CPF).HasPrecision(11,0).IsRequired();
            this.Property(p => p.DataNascimento).HasColumnType("DATE").IsRequired();
            this.Property(p => p.DataCadastro).IsRequired();
            this.Property(p => p.DataAlteracao).IsOptional();
            this.Property(p => p.RG).HasMaxLength(20).IsOptional();
            this.Property(p => p.OrgaoEmissor).HasMaxLength(50).IsOptional();
            this.Property(p => p.QtdFilhos).IsOptional();
            this.Property(p => p.FlgPossuiFilhos).IsOptional();
            this.Property(p => p.CNH).HasMaxLength(15).IsOptional();
            this.Property(p => p.CategoriaHabilitacao).HasMaxLength(2).IsOptional();

            this.HasOptional(p => p.Cidade).WithMany().Map(p => p.MapKey("IdCidade"));
            this.HasOptional(p => p.Sexo).WithMany().Map(p => p.MapKey("SiglaSexo"));
            this.HasOptional(p => p.Endereco).WithMany().Map(p => p.MapKey("IdEndereco"));
            this.HasOptional(p => p.DeficienciaGlobal).WithMany().Map(p => p.MapKey("IdDeficienciaGlobal"));
            this.HasOptional(p => p.EscolaridadeGlobal).WithMany().Map(p => p.MapKey("IdEscolaridadeGlobal"));
            this.HasOptional(p => p.EstadoCivil).WithMany().Map(p => p.MapKey("IdEstadoCivil"));
        }
    }
}