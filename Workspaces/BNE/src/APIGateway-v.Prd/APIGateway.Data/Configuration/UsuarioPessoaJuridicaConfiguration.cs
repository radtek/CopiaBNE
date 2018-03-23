using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.Data.Configuration
{
    public class UsuarioPessoaJuridicaConfiguration : EntityTypeConfiguration<Model.UsuarioPessoaJuridica>
    {
        public UsuarioPessoaJuridicaConfiguration()
        {
            this.Property(n => n.CPF).IsRequired().HasPrecision(11, 0).HasColumnName("CPF");
            this.Property(n => n.CNPJ).IsRequired().HasPrecision(14, 0);
        }
    }
}
