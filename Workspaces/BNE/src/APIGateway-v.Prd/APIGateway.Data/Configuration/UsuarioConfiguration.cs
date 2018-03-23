using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.Data.Configuration 
{
    public class UsuarioConfiguration : EntityTypeConfiguration<Model.Usuario>
    {
        public UsuarioConfiguration()
        {
            ToTable("Usuario", "ApiGateway");
            this.HasKey(n => n.Id);
            this.Ignore(n => n.ForwardHeaders);
            this.Ignore(n => n.PerfilDeAcesso);
            this.Map<Model.UsuarioPessoaFisica>(m => m.Requires("Type").HasValue("UsuarioPessoaFisica"))
                .Map<Model.UsuarioSistemaCliente>(m => m.Requires("Type").HasValue("UsuarioSistemaCliente"))
                .Map<Model.UsuarioPessoaJuridica>(m => m.Requires("Type").HasValue("UsuarioPessoaJuridica"));
        }
    }
}
