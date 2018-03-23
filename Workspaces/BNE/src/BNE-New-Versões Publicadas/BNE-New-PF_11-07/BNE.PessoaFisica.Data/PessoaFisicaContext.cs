using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace BNE.PessoaFisica.Data
{
    public class PessoaFisicaContext : DbContext
    {
        public PessoaFisicaContext() : base("PessoaFisica") 
        {
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        #region Model PessoaFisica
        public DbSet<Model.PreCurriculo> PreCurriculo { get; set; }

        public DbSet<Model.TipoCurriculo> TipoCurriculo { get; set; }
        public DbSet<Model.FuncaoPretendida> FuncaoPretendida { get; set; }
        public DbSet<Model.Curriculo> Curriculo { get; set; }
        public DbSet<Model.PessoaFisica> PessoaFisica { get; set; }
        public DbSet<Model.Email> PessoaFisicaEmail { get; set; }
        public DbSet<Model.Telefone> Telefone { get; set; }
        public DbSet<Model.EstadoCivil> EstadoCivil { get; set; }
        public DbSet<Model.ExperienciaProfissional> ExperienciaProfissional { get; set; }
        public DbSet<Model.NivelCurso> NivelCurso { get; set; }
        public DbSet<Model.Curso> Curso { get; set; }
        public DbSet<Model.SituacaoFormacao> SituacaoFormacao { get; set; }
        public DbSet<Model.InstituicaoEnsino> InstituicaoEnsino { get; set; }
        public DbSet<Model.Formacao> Formacao { get; set; }
        public DbSet<Model.NivelIdioma> NivelIdioma { get; set; }
        public DbSet<Model.Idioma> Idioma { get; set; }
        public DbSet<Model.CidadePretendida> CidadePretendida { get; set; }
        public DbSet<Model.TipoVeiculo> TipoVeiculo { get; set; }
        public DbSet<Model.Veiculo> Veiculo { get; set; }
        public DbSet<Model.Foto> Foto { get; set; }
        public DbSet<Model.CurriculoAnexo> CurriculoAnexo { get; set; }
        public DbSet<Model.CurriculoDisponibilidade> CurriculoDisponibilidade { get; set; }
        public DbSet<Model.Endereco> Endereco { get; set; }
        public DbSet<Model.SituacaoCurriculo> SituacaoCurriculo { get; set; }
        public DbSet<Model.CurriculoOrigem> CurriculoOrigem { get; set; }
        public DbSet<Model.Parametro> Parametro { get; set; }
        public DbSet<Model.CurriculoParametro> CurriculoParametro { get; set; }
        public DbSet<Model.CodigoConfirmacaoEmail> CodigoConfirmacaoEmail { get; set; }




        #endregion

        #region Model Global
        public DbSet<Global.Model.GrauEscolaridadeGlobal> GrauEscolaridade { get; set; }
        public DbSet<Global.Model.EscolaridadeGlobal> Escolaridade { get; set; }
        public DbSet<Global.Model.RankingEmail> RankingEmail { get; set; }
        public DbSet<Global.Model.DeficienciaGlobal> Deficiencia { get; set; }
        public DbSet<Global.Model.Sexo> Sexo { get; set; }
        public DbSet<Global.Model.RamoAtividadeGlobal> RamoAtividade { get; set; }
        public DbSet<Global.Model.IdiomaGlobal> IdiomaGlobal { get; set; }
        public DbSet<Global.Model.DisponibilidadeGlobal> DisponibilidadeGlobal { get; set; }
        public DbSet<Global.Model.TipoTelefoneGlobal> TipoTelefone { get; set; }
        public DbSet<Global.Model.Cidade> Cidade { get; set; }
        public DbSet<Global.Model.Estado> Estado { get; set; }

        public DbSet<Global.Model.TipoFuncaoGlobal> TipoFuncao { get; set; }
        public DbSet<Global.Model.CargoGlobal> CargoGlobal { get; set; }
        public DbSet<Global.Model.CategoriaCargoGlobal> TipoFuncaoGlobal { get; set; }
        public DbSet<Global.Model.Funcao> FuncaoGlobal { get; set; }
        public DbSet<Global.Model.FuncaoSinonimo> FuncaoSinonimoGlobal { get; set; }
        public DbSet<Global.Model.TipoOrigemGlobal> TipoOrigemGlobal { get; set; }
        public DbSet<Global.Model.OrigemGlobal> OrigemGlobal { get; set; }



        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            #region Configuration PessoaFisica
            modelBuilder.Configurations.Add(new Configuration.PreCurriculoConfiguration());
            modelBuilder.Configurations.Add(new Configuration.TipoCurriculoConfiguration());
            modelBuilder.Configurations.Add(new Configuration.FuncaoPretendidaConfiguration());
            modelBuilder.Configurations.Add(new Configuration.CurriculoConfiguration());
            modelBuilder.Configurations.Add(new Configuration.PessoaFisicaConfiguration());
            modelBuilder.Configurations.Add(new Configuration.EmailConfiguration());
            modelBuilder.Configurations.Add(new Configuration.TelefoneConfiguration());
            modelBuilder.Configurations.Add(new Configuration.EstadoCivilConfiguration());
            modelBuilder.Configurations.Add(new Configuration.ExperienciaProfissionalConfiguration());
            modelBuilder.Configurations.Add(new Configuration.NivelCursoConfiguration());
            modelBuilder.Configurations.Add(new Configuration.CursoConfiguration());
            modelBuilder.Configurations.Add(new Configuration.SituacaoFormacaoConfiguration());
            modelBuilder.Configurations.Add(new Configuration.InstituicaoEnsinoConfiguration());
            modelBuilder.Configurations.Add(new Configuration.FormacaoConfiguration());
            modelBuilder.Configurations.Add(new Configuration.NivelIdiomaConfiguration());
            modelBuilder.Configurations.Add(new Configuration.IdiomaConfiguration());
            modelBuilder.Configurations.Add(new Configuration.EnderecoConfiguration());

            modelBuilder.Configurations.Add(new Configuration.CidadePretendidaConfiguration());
            modelBuilder.Configurations.Add(new Configuration.TipoVeiculoConfiguration());
            modelBuilder.Configurations.Add(new Configuration.VeiculoConfiguration());
            modelBuilder.Configurations.Add(new Configuration.FotoConfiguration());
            modelBuilder.Configurations.Add(new Configuration.CurriculoAnexoConfiguration());
            modelBuilder.Configurations.Add(new Configuration.CurriculoDisponibilidadeConfiguration());
            modelBuilder.Configurations.Add(new Configuration.SituacaoCurriculoConfiguration());
            modelBuilder.Configurations.Add(new Configuration.CurriculoOrigemConfiguration());
            modelBuilder.Configurations.Add(new Configuration.ParametroConfiguration());
            modelBuilder.Configurations.Add(new Configuration.CurriculoParametroConfiguration());
            modelBuilder.Configurations.Add(new Configuration.CodigoConfirmacaoEmailConfiguration());

            #endregion

            #region Configuration Global
            modelBuilder.Configurations.Add(new Global.Data.Configuration.GrauEscolaridadeConfiguration());
            modelBuilder.Configurations.Add(new Global.Data.Configuration.EscolaridadeConfiguration());
            modelBuilder.Configurations.Add(new Global.Data.Configuration.RankingEmailConfiguration());
            modelBuilder.Configurations.Add(new Global.Data.Configuration.DeficienciaConfiguration());
            modelBuilder.Configurations.Add(new Global.Data.Configuration.SexoConfiguration());
            modelBuilder.Configurations.Add(new Global.Data.Configuration.RamoAtividadeConfiguration());
            modelBuilder.Configurations.Add(new Global.Data.Configuration.IdiomaConfiguration());
            modelBuilder.Configurations.Add(new Global.Data.Configuration.DisponibilidadeGlobalConfiguration());
            modelBuilder.Configurations.Add(new Global.Data.Configuration.TipoTelefoneConfiguration());
            modelBuilder.Configurations.Add(new Global.Data.Configuration.CidadeConfiguration());
            modelBuilder.Configurations.Add(new Global.Data.Configuration.EstadoConfiguration());

            modelBuilder.Configurations.Add(new Global.Data.Configuration.TipoFuncaoGlobalConfiguration());
            modelBuilder.Configurations.Add(new Global.Data.Configuration.CategoriaCargoGlobalConfiguration());
            modelBuilder.Configurations.Add(new Global.Data.Configuration.CargoGlobalConfiguration());
            modelBuilder.Configurations.Add(new Global.Data.Configuration.FuncaoSinonimoConfiguration());
            modelBuilder.Configurations.Add(new Global.Data.Configuration.FuncaoConfiguration());
            modelBuilder.Configurations.Add(new Global.Data.Configuration.TipoOrigemGlobalConfiguration());
            modelBuilder.Configurations.Add(new Global.Data.Configuration.OrigemGlobalConfiguration());


            #endregion

            //Determina que as colunas string serão Varchar.
            modelBuilder.Properties<string>().Configure(c => c.HasColumnType("Varchar"));

            base.OnModelCreating(modelBuilder);
        }

        public virtual void Commit()
        {
            SaveChanges();
        }
    }
}