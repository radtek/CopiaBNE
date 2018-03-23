namespace AdminLTE_Application
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model : DbContext
    {
        public Model()
            : base("name=DW_CRM2012")
        {
        }

        public virtual DbSet<CRM_Acao> CRM_Acao { get; set; }
        public virtual DbSet<CRM_Atendimento> CRM_Atendimento { get; set; }
        public virtual DbSet<CRM_Empresa> CRM_Empresa { get; set; }
        public virtual DbSet<CRM_Fato> CRM_Fato { get; set; }
        public virtual DbSet<CRM_Log_Carga> CRM_Log_Carga { get; set; }
        public virtual DbSet<CRM_Situacao_Atendimento> CRM_Situacao_Atendimento { get; set; }
        public virtual DbSet<CRM_Tab_Area_BNE> CRM_Tab_Area_BNE { get; set; }
        public virtual DbSet<CRM_Tipo_Vendedor> CRM_Tipo_Vendedor { get; set; }
        public virtual DbSet<CRM_Vendedor> CRM_Vendedor { get; set; }
        public virtual DbSet<CRM_Vendedor_Cidade> CRM_Vendedor_Cidade { get; set; }
        public virtual DbSet<CRM_Vendedor_Empresa> CRM_Vendedor_Empresa { get; set; }
        public virtual DbSet<CRM_Vendedor_Cidade1> CRM_Vendedor_Cidade1 { get; set; }
        public virtual DbSet<CRM_Vendedor_Empresa1> CRM_Vendedor_Empresa1 { get; set; }
        public virtual DbSet<CRM_Log_Carga1> CRM_Log_Carga1 { get; set; }
        public virtual DbSet<CRM_Vendedor_Cidade2> CRM_Vendedor_Cidade2 { get; set; }
        public virtual DbSet<CRM_Vendedor_Empresa2> CRM_Vendedor_Empresa2 { get; set; }
        public virtual DbSet<CRM_Vendedor_Cidade3> CRM_Vendedor_Cidade3 { get; set; }
        public virtual DbSet<CRM_Vendedor_Empresa3> CRM_Vendedor_Empresa3 { get; set; }
        public virtual DbSet<VW_EMPRESA_COM_PLANO> VW_EMPRESA_COM_PLANO { get; set; }
        public virtual DbSet<VW_EMPRESA_SEM_PLANO> VW_EMPRESA_SEM_PLANO { get; set; }
        public virtual DbSet<VWAtendimentos> VWAtendimentos { get; set; }
        public virtual DbSet<VWEmpresa> VWEmpresas { get; set; }
        public virtual DbSet<dbParametro> tbParametro { get; set; }
        public virtual DbSet<VW_Tanque_Empresa> VWTanqueEmpresas { get; set; }
        public virtual DbSet<VW_Banco_Empresas> VWbancoEmpresas { get; set; }
        public virtual DbSet<VW_BANCO_EMPRESAS_RESERVA_TECNICA> VWbancoEmpresasReservaTecnica { get; set; }
        public virtual DbSet<VWVagas> VWVaga { get; set; }
        public virtual DbSet<VWVagaObservacao> VWVagaObservacao {get;set;}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CRM_Acao>()
                .Property(e => e.Des_Acao)
                .IsUnicode(false);

            modelBuilder.Entity<CRM_Atendimento>()
                .Property(e => e.Des_Atendimento)
                .IsUnicode(false);

            modelBuilder.Entity<CRM_Empresa>()
                .Property(e => e.Num_CNPJ)
                .HasPrecision(14, 0);

            modelBuilder.Entity<CRM_Empresa>()
                .Property(e => e.Raz_Social)
                .IsUnicode(false);

            modelBuilder.Entity<CRM_Empresa>()
                .Property(e => e.Des_Plano)
                .IsUnicode(false);

            modelBuilder.Entity<CRM_Empresa>()
                .Property(e => e.Nme_Cidade)
                .IsUnicode(false);

            modelBuilder.Entity<CRM_Empresa>()
                .Property(e => e.Sig_Estado)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CRM_Empresa>()
                .HasMany(e => e.CRM_Fato)
                .WithRequired(e => e.CRM_Empresa)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CRM_Empresa>()
                .HasMany(e => e.CRM_Vendedor_Empresa)
                .WithRequired(e => e.CRM_Empresa)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CRM_Empresa>()
                .HasMany(e => e.CRM_Vendedor_Empresa1)
                .WithRequired(e => e.CRM_Empresa)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CRM_Empresa>()
                .HasMany(e => e.CRM_Vendedor_Empresa2)
                .WithRequired(e => e.CRM_Empresa)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CRM_Empresa>()
                .HasMany(e => e.CRM_Vendedor_Empresa3)
                .WithRequired(e => e.CRM_Empresa)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CRM_Fato>()
                .Property(e => e.Num_CNPJ)
                .HasPrecision(14, 0);

            modelBuilder.Entity<CRM_Fato>()
                .Property(e => e.Num_CPF)
                .HasPrecision(11, 0);

            modelBuilder.Entity<CRM_Situacao_Atendimento>()
                .Property(e => e.Des_Situacao_atendimento)
                .IsUnicode(false);

            modelBuilder.Entity<CRM_Tab_Area_BNE>()
                .Property(e => e.Des_Area_BNE)
                .IsUnicode(false);

            modelBuilder.Entity<CRM_Tipo_Vendedor>()
                .Property(e => e.Des_Tipo_Vendedor)
                .IsUnicode(false);

            modelBuilder.Entity<CRM_Vendedor>()
                .Property(e => e.Num_CPF)
                .HasPrecision(11, 0);

            modelBuilder.Entity<CRM_Vendedor>()
                .Property(e => e.Nme_Vendedor)
                .IsUnicode(false);

            modelBuilder.Entity<CRM_Vendedor>()
                .Property(e => e.Eml_Vendedor)
                .IsUnicode(false);

            modelBuilder.Entity<CRM_Vendedor>()
                .Property(e => e.Sen_Vendedor)
                .IsUnicode(false);

            modelBuilder.Entity<CRM_Vendedor>()
                .HasMany(e => e.CRM_Vendedor_Cidade)
                .WithRequired(e => e.CRM_Vendedor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CRM_Vendedor>()
                .HasMany(e => e.CRM_Vendedor_Cidade1)
                .WithRequired(e => e.CRM_Vendedor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CRM_Vendedor>()
                .HasMany(e => e.CRM_Vendedor_Cidade2)
                .WithRequired(e => e.CRM_Vendedor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CRM_Vendedor>()
                .HasMany(e => e.CRM_Vendedor_Cidade3)
                .WithRequired(e => e.CRM_Vendedor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CRM_Vendedor>()
                .HasMany(e => e.CRM_Vendedor_Empresa)
                .WithRequired(e => e.CRM_Vendedor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CRM_Vendedor>()
                .HasMany(e => e.CRM_Vendedor_Empresa1)
                .WithRequired(e => e.CRM_Vendedor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CRM_Vendedor>()
                .HasMany(e => e.CRM_Vendedor_Empresa2)
                .WithRequired(e => e.CRM_Vendedor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CRM_Vendedor>()
                .HasMany(e => e.CRM_Vendedor_Empresa3)
                .WithRequired(e => e.CRM_Vendedor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CRM_Vendedor_Cidade>()
                .Property(e => e.Num_CPF)
                .HasPrecision(11, 0);

            modelBuilder.Entity<CRM_Vendedor_Empresa>()
                .Property(e => e.Num_CPF)
                .HasPrecision(11, 0);

            modelBuilder.Entity<CRM_Vendedor_Empresa>()
                .Property(e => e.Num_CNPJ)
                .HasPrecision(14, 0);

            modelBuilder.Entity<CRM_Vendedor_Empresa>()
                .Property(e => e.Des_Obs_VendedorEmpresa)
                .IsUnicode(false);

            modelBuilder.Entity<CRM_Vendedor_Cidade1>()
                .Property(e => e.Num_CPF)
                .HasPrecision(11, 0);

            modelBuilder.Entity<CRM_Vendedor_Empresa1>()
                .Property(e => e.Num_CPF)
                .HasPrecision(11, 0);

            modelBuilder.Entity<CRM_Vendedor_Empresa1>()
                .Property(e => e.Num_CNPJ)
                .HasPrecision(14, 0);

            modelBuilder.Entity<CRM_Vendedor_Empresa1>()
                .Property(e => e.Des_Obs_VendedorEmpresa)
                .IsUnicode(false);

            modelBuilder.Entity<CRM_Vendedor_Cidade2>()
                .Property(e => e.Num_CPF)
                .HasPrecision(11, 0);

            modelBuilder.Entity<CRM_Vendedor_Empresa2>()
                .Property(e => e.Num_CPF)
                .HasPrecision(11, 0);

            modelBuilder.Entity<CRM_Vendedor_Empresa2>()
                .Property(e => e.Num_CNPJ)
                .HasPrecision(14, 0);

            modelBuilder.Entity<CRM_Vendedor_Empresa2>()
                .Property(e => e.Des_Obs_VendedorEmpresa)
                .IsUnicode(false);

            modelBuilder.Entity<CRM_Vendedor_Cidade3>()
                .Property(e => e.Num_CPF)
                .HasPrecision(11, 0);

            modelBuilder.Entity<CRM_Vendedor_Empresa3>()
                .Property(e => e.Num_CPF)
                .HasPrecision(11, 0);

            modelBuilder.Entity<CRM_Vendedor_Empresa3>()
                .Property(e => e.Num_CNPJ)
                .HasPrecision(14, 0);

            modelBuilder.Entity<CRM_Vendedor_Empresa3>()
                .Property(e => e.Des_Obs_VendedorEmpresa)
                .IsUnicode(false);

          

            modelBuilder.Entity<VW_EMPRESA_COM_PLANO>()
                .Property(e => e.Num_CPF)
                .HasPrecision(11, 0);

            modelBuilder.Entity<VW_EMPRESA_COM_PLANO>()
                .Property(e => e.Des_Atendimento)
                .IsUnicode(false);

            modelBuilder.Entity<VW_EMPRESA_COM_PLANO>()
                .Property(e => e.Num_CNPJ)
                .HasPrecision(14, 0);

            modelBuilder.Entity<VW_EMPRESA_COM_PLANO>()
                .Property(e => e.Raz_Social)
                .IsUnicode(false);

            modelBuilder.Entity<VW_EMPRESA_COM_PLANO>()
                .Property(e => e.Des_Area_BNE)
                .IsUnicode(false);

            modelBuilder.Entity<VW_EMPRESA_COM_PLANO>()
                .Property(e => e.Sig_Estado)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VW_EMPRESA_COM_PLANO>()
                .Property(e => e.Nme_Cidade)
                .IsUnicode(false);

            modelBuilder.Entity<VW_EMPRESA_SEM_PLANO>()
                .Property(e => e.Num_CPF)
                .HasPrecision(11, 0);

            modelBuilder.Entity<VW_EMPRESA_SEM_PLANO>()
                .Property(e => e.Num_CNPJ)
                .HasPrecision(14, 0);

            modelBuilder.Entity<VW_EMPRESA_SEM_PLANO>()
                .Property(e => e.Raz_Social)
                .IsUnicode(false);

            modelBuilder.Entity<VW_EMPRESA_SEM_PLANO>()
                .Property(e => e.Sig_Estado)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VW_EMPRESA_SEM_PLANO>()
                .Property(e => e.Des_Area_BNE)
                .IsUnicode(false);

            modelBuilder.Entity<VW_EMPRESA_SEM_PLANO>()
                .Property(e => e.Nme_Cidade)
                .IsUnicode(false);

            modelBuilder.Entity<VW_EMPRESA_SEM_PLANO>()
                .Property(e => e.Des_Atendimento)
                .IsUnicode(false);

            modelBuilder.Entity<VWAtendimentos>()
                .Property(e => e.CNPJ)
                .HasPrecision(14, 0);

            modelBuilder.Entity<VWAtendimentos>()
                .Property(e => e.Tipo_Atendimento)
                .IsUnicode(false);

            modelBuilder.Entity<VWAtendimentos>()
                .Property(e => e.CPF)
                .HasPrecision(11, 0);

            modelBuilder.Entity<VWEmpresa>()
                .Property(e => e.Raz_Social)
                .IsUnicode(false);

            modelBuilder.Entity<VWEmpresa>()
                .Property(e => e.Num_CNPJ)
                .HasPrecision(14, 0);

            modelBuilder.Entity<VWEmpresa>()
                .Property(e => e.End_Site)
                .IsUnicode(false);

            modelBuilder.Entity<VWEmpresa>()
                .Property(e => e.Num_DDD_Comercial)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VWEmpresa>()
                .Property(e => e.Num_Comercial)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VWEmpresa>()
                .Property(e => e.Des_Natureza_Juridica)
                .IsUnicode(false);

            modelBuilder.Entity<VWEmpresa>()
                .Property(e => e.Des_Plano)
                .IsUnicode(false);

            modelBuilder.Entity<VWEmpresa>()
                .Property(e => e.Sig_Estado)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VWEmpresa>()
                .Property(e => e.Nme_Cidade)
                .IsUnicode(false);

            modelBuilder.Entity<VWEmpresa>()
                .Property(e => e.Des_Situacao_Filial)
                .IsUnicode(false);

            modelBuilder.Entity<VWEmpresa>()
                .Property(e => e.Des_URL)
                .IsUnicode(false);

            modelBuilder.Entity<dbParametro>().Property(e => e.Id_Parametro);

            modelBuilder.Entity<VW_Tanque_Empresa>().Property(e => e.Num_CNPJ);

            modelBuilder.Entity<VW_Banco_Empresas>()
              .Property(e => e.Sig_Estado)
              .IsFixedLength()
              .IsUnicode(false);

            modelBuilder.Entity<VW_BANCO_EMPRESAS_RESERVA_TECNICA>()
                .Property(e => e.Sig_Estado)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VWVagas>()
              .Property(e => e.num_cnpj);

            modelBuilder.Entity<VWVagaObservacao>()
             .Property(e => e.Idf_Vaga_Observacao);

        }
    }
}
