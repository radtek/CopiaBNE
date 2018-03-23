using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.Data.Configuration
{
    public class UsuarioSistemaClienteConfiguration : EntityTypeConfiguration<Model.UsuarioSistemaCliente>
    {
        public UsuarioSistemaClienteConfiguration()
        {
            this.HasRequired(n => n.SistemaCliente);
            this.HasMany<Model.Header>(n => n.Headers);

            //Mapeando Perfil para string
            this.Property(n => n.PerfilString).HasColumnName("Perfil").HasMaxLength(50).IsRequired();
            this.Ignore(n => n.Perfil);
        }
    }
}
