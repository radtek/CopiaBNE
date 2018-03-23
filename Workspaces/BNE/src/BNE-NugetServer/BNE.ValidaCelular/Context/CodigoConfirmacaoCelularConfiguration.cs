using BNE.ValidaCelular.Model;
using System.Data.Entity.ModelConfiguration;

namespace BNE.ValidaCelular.Context
{
    internal sealed class CodigoConfirmacaoCelularConfiguration : EntityTypeConfiguration<CodigoConfirmacaoCelular>
    {

        #region CodigoConfirmacaoCelularConfiguration
        public CodigoConfirmacaoCelularConfiguration()
        {
            ToTable("TAB_Codigo_Confirmacao_Celular");
            Property(t => t.Id).HasColumnName("Idf_Codigo_Confirmacao_Celular");
            Property(t => t.DataUtilizacao).HasColumnName("Dta_Utilizacao").IsOptional();
            Property(t => t.DataCriacao).HasColumnName("Dta_Criacao").IsRequired();
            Property(t => t.CodigoConfirmacao).HasColumnName("Cod_Confirmacao").IsRequired().HasMaxLength(11);
            Property(t => t.NumeroDDDCelular).HasColumnName("Num_DDD_Celular").IsRequired().HasMaxLength(2);
            Property(t => t.NumeroCelular).HasColumnName("Num_Celular").IsRequired().HasMaxLength(10);
        }
        #endregion CodigoConfirmacaoCelularConfiguration

    }
}
