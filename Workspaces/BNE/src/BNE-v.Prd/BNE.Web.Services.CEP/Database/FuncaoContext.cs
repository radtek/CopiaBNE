using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using BNE.Web.Services.Solr.Models;

namespace BNE.Web.Services.Solr.Database
{
    public class FuncaoContext : DbContext
    {
        public FuncaoContext() : base("name=Funcao") { }

        public DbSet<FuncaoNaoEncontrada> FuncoesNaoEncontradas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<FuncaoNaoEncontrada>().ToTable("LOG_Funcao_Nao_Encontrada", "funcao");
            modelBuilder.Entity<FuncaoNaoEncontrada>().HasKey(t => t.IdFuncaoNaoEncontrada);
            modelBuilder.Entity<FuncaoNaoEncontrada>().Property(t => t.IdFuncaoNaoEncontrada).HasColumnName("Idf_Funcao_Nao_Encontrada");
            modelBuilder.Entity<FuncaoNaoEncontrada>().Property(t => t.DescricaoConteudoBuscado).HasColumnName("Des_Conteudo_Buscado").IsRequired();
            modelBuilder.Entity<FuncaoNaoEncontrada>().Property(t => t.DescricaoOrigem).HasColumnName("Des_Origem");
            modelBuilder.Entity<FuncaoNaoEncontrada>().Property(t => t.DataCadastro).HasColumnName("Dta_Cadastro");

            base.OnModelCreating(modelBuilder);
        }
    }
}