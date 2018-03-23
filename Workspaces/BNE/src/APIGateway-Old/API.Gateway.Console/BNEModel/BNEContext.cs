namespace API.Gateway.Console.BNEModel
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class BNEContext : DbContext
    {
        public BNEContext()
            : base("name=BNEContext")
        {
        }

        public virtual DbSet<BNE_Usuario> BNE_Usuario { get; set; }
        public virtual DbSet<TAB_Pessoa_Fisica> TAB_Pessoa_Fisica { get; set; }
        public virtual DbSet<TAB_Usuario_Filial_Perfil> TAB_Usuario_Filial_Perfil { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BNE_Usuario>()
                .Property(e => e.Sen_Usuario)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Usuario>()
                .Property(e => e.Des_Session_ID)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Num_CPF)
                .HasPrecision(11, 0);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Nme_Pessoa)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Ape_Pessoa)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Nme_Mae)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Nme_Pai)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Num_RG)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Nme_Orgao_Emissor)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Sig_UF_Emissao_RG)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Num_PIS)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Num_CTPS)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Des_Serie_CTPS)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Sig_UF_CTPS)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Num_DDD_Telefone)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Num_Telefone)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Num_DDD_Celular)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Num_Celular)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Eml_Pessoa)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Nme_Pessoa_Pesquisa)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Sig_Estado)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Des_IP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .HasMany(e => e.BNE_Usuario)
                .WithRequired(e => e.TAB_Pessoa_Fisica)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .HasMany(e => e.TAB_Usuario_Filial_Perfil)
                .WithRequired(e => e.TAB_Pessoa_Fisica)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Usuario_Filial_Perfil>()
                .Property(e => e.Des_IP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Usuario_Filial_Perfil>()
                .Property(e => e.Sen_Usuario_Filial_Perfil)
                .IsUnicode(false);
        }
    }
}
