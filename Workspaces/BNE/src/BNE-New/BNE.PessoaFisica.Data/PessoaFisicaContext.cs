using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.SqlServer;
using BNE.Global.Data.Configuration;
using BNE.Global.Model;
using BNE.PessoaFisica.Data.Configuration;
using BNE.PessoaFisica.Domain.Model;
using IdiomaConfiguration = BNE.PessoaFisica.Data.Configuration.IdiomaConfiguration;

namespace BNE.PessoaFisica.Data
{
    public class PessoaFisicaContext : DbContext
    {
        public PessoaFisicaContext() : base("PessoaFisica")
        {
            var instance = SqlProviderServices.Instance;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            #region Configuration PessoaFisica

            modelBuilder.Configurations.Add(new PreCurriculoConfiguration());
            modelBuilder.Configurations.Add(new TipoCurriculoConfiguration());
            modelBuilder.Configurations.Add(new FuncaoPretendidaConfiguration());
            modelBuilder.Configurations.Add(new CurriculoConfiguration());
            modelBuilder.Configurations.Add(new PessoaFisicaConfiguration());
            modelBuilder.Configurations.Add(new EmailConfiguration());
            modelBuilder.Configurations.Add(new TelefoneConfiguration());
            modelBuilder.Configurations.Add(new EstadoCivilConfiguration());
            modelBuilder.Configurations.Add(new ExperienciaProfissionalConfiguration());
            modelBuilder.Configurations.Add(new NivelCursoConfiguration());
            modelBuilder.Configurations.Add(new CursoConfiguration());
            modelBuilder.Configurations.Add(new SituacaoFormacaoConfiguration());
            modelBuilder.Configurations.Add(new InstituicaoEnsinoConfiguration());
            modelBuilder.Configurations.Add(new FormacaoConfiguration());
            modelBuilder.Configurations.Add(new NivelIdiomaConfiguration());
            modelBuilder.Configurations.Add(new IdiomaConfiguration());
            modelBuilder.Configurations.Add(new EnderecoConfiguration());

            modelBuilder.Configurations.Add(new CidadePretendidaConfiguration());
            modelBuilder.Configurations.Add(new TipoVeiculoConfiguration());
            modelBuilder.Configurations.Add(new VeiculoConfiguration());
            modelBuilder.Configurations.Add(new FotoConfiguration());
            modelBuilder.Configurations.Add(new CurriculoAnexoConfiguration());
            modelBuilder.Configurations.Add(new CurriculoDisponibilidadeConfiguration());
            modelBuilder.Configurations.Add(new SituacaoCurriculoConfiguration());
            modelBuilder.Configurations.Add(new CurriculoOrigemConfiguration());
            modelBuilder.Configurations.Add(new ParametroConfiguration());
            modelBuilder.Configurations.Add(new CurriculoParametroConfiguration());
            modelBuilder.Configurations.Add(new CodigoConfirmacaoEmailConfiguration());

            #endregion

            #region Configuration Global

            modelBuilder.Configurations.Add(new GrauEscolaridadeConfiguration());
            modelBuilder.Configurations.Add(new EscolaridadeConfiguration());
            modelBuilder.Configurations.Add(new RankingEmailConfiguration());
            modelBuilder.Configurations.Add(new DeficienciaConfiguration());
            modelBuilder.Configurations.Add(new SexoConfiguration());
            modelBuilder.Configurations.Add(new RamoAtividadeConfiguration());
            modelBuilder.Configurations.Add(new Global.Data.Configuration.IdiomaConfiguration());
            modelBuilder.Configurations.Add(new DisponibilidadeGlobalConfiguration());
            modelBuilder.Configurations.Add(new TipoTelefoneConfiguration());
            modelBuilder.Configurations.Add(new CidadeConfiguration());
            modelBuilder.Configurations.Add(new EstadoConfiguration());

            modelBuilder.Configurations.Add(new TipoFuncaoGlobalConfiguration());
            modelBuilder.Configurations.Add(new CategoriaCargoGlobalConfiguration());
            modelBuilder.Configurations.Add(new CargoGlobalConfiguration());
            modelBuilder.Configurations.Add(new FuncaoSinonimoConfiguration());
            modelBuilder.Configurations.Add(new FuncaoConfiguration());
            modelBuilder.Configurations.Add(new TipoOrigemGlobalConfiguration());
            modelBuilder.Configurations.Add(new OrigemGlobalConfiguration());

            #endregion

            //Determina que as colunas string serão Varchar.
            modelBuilder.Properties<string>().Configure(c => c.HasColumnType("Varchar"));

            base.OnModelCreating(modelBuilder);
        }

        public virtual void Commit()
        {
            SaveChanges();
        }

        #region Model PessoaFisica

        public DbSet<PreCurriculo> PreCurriculo { get; set; }

        public DbSet<TipoCurriculo> TipoCurriculo { get; set; }
        public DbSet<FuncaoPretendida> FuncaoPretendida { get; set; }
        public DbSet<Curriculo> Curriculo { get; set; }
        public DbSet<Domain.Model.PessoaFisica> PessoaFisica { get; set; }
        public DbSet<Email> PessoaFisicaEmail { get; set; }
        public DbSet<Telefone> Telefone { get; set; }
        public DbSet<EstadoCivil> EstadoCivil { get; set; }
        public DbSet<ExperienciaProfissional> ExperienciaProfissional { get; set; }
        public DbSet<NivelCurso> NivelCurso { get; set; }
        public DbSet<Curso> Curso { get; set; }
        public DbSet<SituacaoFormacao> SituacaoFormacao { get; set; }
        public DbSet<InstituicaoEnsino> InstituicaoEnsino { get; set; }
        public DbSet<Formacao> Formacao { get; set; }
        public DbSet<NivelIdioma> NivelIdioma { get; set; }
        public DbSet<Idioma> Idioma { get; set; }
        public DbSet<CidadePretendida> CidadePretendida { get; set; }
        public DbSet<TipoVeiculo> TipoVeiculo { get; set; }
        public DbSet<Veiculo> Veiculo { get; set; }
        public DbSet<Foto> Foto { get; set; }
        public DbSet<CurriculoAnexo> CurriculoAnexo { get; set; }
        public DbSet<CurriculoDisponibilidade> CurriculoDisponibilidade { get; set; }
        public DbSet<Endereco> Endereco { get; set; }
        public DbSet<SituacaoCurriculo> SituacaoCurriculo { get; set; }
        public DbSet<CurriculoOrigem> CurriculoOrigem { get; set; }
        public DbSet<Parametro> Parametro { get; set; }
        public DbSet<CurriculoParametro> CurriculoParametro { get; set; }
        public DbSet<CodigoConfirmacaoEmail> CodigoConfirmacaoEmail { get; set; }

        #endregion

        #region Model Global

        public DbSet<GrauEscolaridadeGlobal> GrauEscolaridade { get; set; }
        public DbSet<EscolaridadeGlobal> Escolaridade { get; set; }
        public DbSet<RankingEmail> RankingEmail { get; set; }
        public DbSet<DeficienciaGlobal> Deficiencia { get; set; }
        public DbSet<Sexo> Sexo { get; set; }
        public DbSet<RamoAtividadeGlobal> RamoAtividade { get; set; }
        public DbSet<IdiomaGlobal> IdiomaGlobal { get; set; }
        public DbSet<DisponibilidadeGlobal> DisponibilidadeGlobal { get; set; }
        public DbSet<TipoTelefoneGlobal> TipoTelefone { get; set; }
        public DbSet<Cidade> Cidade { get; set; }
        public DbSet<Estado> Estado { get; set; }
        public DbSet<TipoFuncaoGlobal> TipoFuncao { get; set; }
        public DbSet<CargoGlobal> CargoGlobal { get; set; }
        public DbSet<CategoriaCargoGlobal> TipoFuncaoGlobal { get; set; }
        public DbSet<Funcao> FuncaoGlobal { get; set; }
        public DbSet<FuncaoSinonimo> FuncaoSinonimoGlobal { get; set; }
        public DbSet<TipoOrigemGlobal> TipoOrigemGlobal { get; set; }
        public DbSet<OrigemGlobal> OrigemGlobal { get; set; }

        #endregion
    }
}