using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using BNE.Web.Services.Solr.Models;

namespace BNE.Web.Services.Solr.Database
{
    public class CidadeContext : DbContext
    {
        public CidadeContext() : base("name=Cidade") { }

        public DbSet<CidadeNaoEncontrada> CidadesNaoEncontradas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<CidadeNaoEncontrada>().ToTable("LOG_Cidade_Nao_Encontrada", "cidade");
            modelBuilder.Entity<CidadeNaoEncontrada>().HasKey(t => t.IdCidadeNaoEncontrada);
            modelBuilder.Entity<CidadeNaoEncontrada>().Property(t => t.IdCidadeNaoEncontrada).HasColumnName("Idf_Cidade_Nao_Encontrada");
            modelBuilder.Entity<CidadeNaoEncontrada>().Property(t => t.DescricaoConteudoBuscado).HasColumnName("Des_Conteudo_Buscado").IsRequired();
            modelBuilder.Entity<CidadeNaoEncontrada>().Property(t => t.DescricaoOrigem).HasColumnName("Des_Origem");
            modelBuilder.Entity<CidadeNaoEncontrada>().Property(t => t.DataCadastro).HasColumnName("Dta_Cadastro");

            base.OnModelCreating(modelBuilder);
        }
    }
}