using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.Data.Configuration
{
    public class UsuarioPessoaFisicaConfiguration : EntityTypeConfiguration<Model.UsuarioPessoaFisica>
    {
        public UsuarioPessoaFisicaConfiguration()
        {
            this.Property(n => n.CPF).IsRequired().HasPrecision(11,0).HasColumnName("CPF");
            this.Property(n => n.DataNascimento).HasColumnType("date");
        }
    }
}
