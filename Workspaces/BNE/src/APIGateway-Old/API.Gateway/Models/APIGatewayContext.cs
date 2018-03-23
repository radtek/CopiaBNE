namespace API.Gateway.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class APIGatewayContext : DbContext
    {
        public APIGatewayContext()
            : base("name=APIGatewayContext")
        {
        }

        public virtual DbSet<AccessToken> AccessToken { get; set; }
        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<Endpoint> Endpoint { get; set; }
        public virtual DbSet<KeyToken> KeyToken { get; set; }
        public virtual DbSet<Metodo> Metodo { get; set; }
        public virtual DbSet<Perfil> Perfil { get; set; }
        public virtual DbSet<Quota> Quota { get; set; }
        public virtual DbSet<Requisicao> Requisicao { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<WebApi> WebApi { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccessToken>()
                .Property(e => e.Token)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.Des_Cliente)
                .IsUnicode(false);

            modelBuilder.Entity<Endpoint>()
                .Property(e => e.VersionAPI)
                .IsUnicode(false);

            modelBuilder.Entity<Endpoint>()
                .Property(e => e.Nme_Api)
                .IsUnicode(false);

            modelBuilder.Entity<Endpoint>()
                .Property(e => e.Controller)
                .IsUnicode(false);

            modelBuilder.Entity<Endpoint>()
                .Property(e => e.Action)
                .IsUnicode(false);

            modelBuilder.Entity<Endpoint>()
                .HasMany(e => e.Quota)
                .WithRequired(e => e.Endpoint)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Endpoint>()
                .HasMany(e => e.Requisicao)
                .WithRequired(e => e.Endpoint)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<KeyToken>()
                .Property(e => e.KeyValue)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Metodo>()
                .Property(e => e.Des_Metodo)
                .IsUnicode(false);

            modelBuilder.Entity<Metodo>()
                .HasMany(e => e.Requisicao)
                .WithRequired(e => e.Metodo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Perfil>()
                .Property(e => e.Des_Perfil)
                .IsUnicode(false);

            modelBuilder.Entity<Perfil>()
                .HasMany(e => e.Usuario)
                .WithRequired(e => e.Perfil)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Requisicao>()
                .Property(e => e.Conteudo)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Num_CPF)
                .HasPrecision(11, 0);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Senha)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.AccessToken)
                .WithRequired(e => e.Usuario)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Requisicao)
                .WithRequired(e => e.Usuario)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<WebApi>()
                .Property(e => e.Nme_Api)
                .IsUnicode(false);

            modelBuilder.Entity<WebApi>()
                .Property(e => e.Location)
                .IsUnicode(false);

            modelBuilder.Entity<WebApi>()
                .HasMany(e => e.Endpoint)
                .WithRequired(e => e.WebApi)
                .WillCascadeOnDelete(false);
        }
    }
}
