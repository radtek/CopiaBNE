namespace BNE.PessoaFisica.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alteracao : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "global.Cargo",
                c => new
                    {
                        IdCargo = c.Short(nullable: false),
                        Prioridade = c.Short(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 50, unicode: false),
                        DataCadastro = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                        FlgInativo = c.Boolean(nullable: false),
                        IdCategoriaCargoGlobal = c.Byte(nullable: false),
                        IdRamoAtividadeGlobal = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdCargo)
                .ForeignKey("global.CategoriaCargo", t => t.IdCategoriaCargoGlobal, cascadeDelete: true)
                .ForeignKey("global.RamoAtividade", t => t.IdRamoAtividadeGlobal, cascadeDelete: true)
                .Index(t => t.IdCategoriaCargoGlobal)
                .Index(t => t.IdRamoAtividadeGlobal);
            
            CreateTable(
                "global.CategoriaCargo",
                c => new
                    {
                        IdCategoriaCargo = c.Byte(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 30, unicode: false),
                    })
                .PrimaryKey(t => t.IdCategoriaCargo);
            
            CreateTable(
                "global.RamoAtividade",
                c => new
                    {
                        IdRamoAtividade = c.Int(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.IdRamoAtividade);
            
            CreateTable(
                "global.Cidade",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(maxLength: 50, unicode: false),
                        Localizacao = c.Geography(),
                        UF = c.String(nullable: false, maxLength: 2, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("global.Estado", t => t.UF, cascadeDelete: true)
                .Index(t => t.UF);
            
            CreateTable(
                "global.Estado",
                c => new
                    {
                        UF = c.String(nullable: false, maxLength: 2, unicode: false),
                        Nome = c.String(nullable: false, maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.UF);
            
            CreateTable(
                "pessoafisica.CidadePretendida",
                c => new
                    {
                        IdCidadePretendida = c.Long(nullable: false, identity: true),
                        DataCadastro = c.DateTime(nullable: false),
                        IdCidadeGlobal = c.Int(nullable: false),
                        IdPessoaFisica = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdCidadePretendida)
                .ForeignKey("global.Cidade", t => t.IdCidadeGlobal, cascadeDelete: true)
                .ForeignKey("pessoafisica.PessoaFisica", t => t.IdPessoaFisica, cascadeDelete: true)
                .Index(t => t.IdCidadeGlobal)
                .Index(t => t.IdPessoaFisica);
            
            CreateTable(
                "pessoafisica.PessoaFisica",
                c => new
                    {
                        IdPessoaFisica = c.Int(nullable: false, identity: true),
                        CPF = c.Decimal(nullable: false, precision: 11, scale: 0),
                        Nome = c.String(nullable: false, maxLength: 100, unicode: false),
                        DataNascimento = c.DateTime(nullable: false, storeType: "date"),
                        DataCadastro = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                        RG = c.String(maxLength: 20, unicode: false),
                        OrgaoEmissor = c.String(maxLength: 50, unicode: false),
                        FlgPossuiFilhos = c.Boolean(),
                        QtdFilhos = c.Byte(),
                        CategoriaHabilitacao = c.String(maxLength: 2, unicode: false),
                        CNH = c.String(maxLength: 15, unicode: false),
                        IdCidade = c.Int(),
                        IdDeficienciaGlobal = c.Byte(),
                        IdEndereco = c.Long(),
                        IdEscolaridadeGlobal = c.Short(),
                        IdEstadoCivil = c.Byte(),
                        SiglaSexo = c.String(maxLength: 1, fixedLength: true, unicode: false),
                    })
                .PrimaryKey(t => t.IdPessoaFisica)
                .ForeignKey("global.Cidade", t => t.IdCidade)
                .ForeignKey("global.Deficiencia", t => t.IdDeficienciaGlobal)
                .ForeignKey("pessoafisica.Endereco", t => t.IdEndereco)
                .ForeignKey("global.Escolaridade", t => t.IdEscolaridadeGlobal)
                .ForeignKey("pessoafisica.EstadoCivil", t => t.IdEstadoCivil)
                .ForeignKey("global.Sexo", t => t.SiglaSexo)
                .Index(t => t.IdCidade)
                .Index(t => t.IdDeficienciaGlobal)
                .Index(t => t.IdEndereco)
                .Index(t => t.IdEscolaridadeGlobal)
                .Index(t => t.IdEstadoCivil)
                .Index(t => t.SiglaSexo);
            
            CreateTable(
                "global.Deficiencia",
                c => new
                    {
                        IdDeficiencia = c.Byte(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 20, unicode: false),
                        CodCaged = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdDeficiencia);
            
            CreateTable(
                "pessoafisica.Endereco",
                c => new
                    {
                        IdEndereco = c.Long(nullable: false, identity: true),
                        DescricaoBairro = c.String(maxLength: 8000, unicode: false),
                        Logradouro = c.String(maxLength: 150, unicode: false),
                        Numero = c.String(maxLength: 20, unicode: false),
                        Complemento = c.String(maxLength: 50, unicode: false),
                        CEP = c.Int(nullable: false),
                        DataAlteracao = c.DateTime(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        Geolocalizacao = c.Geography(),
                        DataAtualizacaoGeolocalizacao = c.DateTime(),
                        Bairro_Id = c.Int(),
                        Cidade_Id = c.Int(),
                    })
                .PrimaryKey(t => t.IdEndereco)
                .ForeignKey("dbo.Bairro", t => t.Bairro_Id)
                .ForeignKey("global.Cidade", t => t.Cidade_Id)
                .Index(t => t.Bairro_Id)
                .Index(t => t.Cidade_Id);
            
            CreateTable(
                "dbo.Bairro",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(maxLength: 8000, unicode: false),
                        Localizacao = c.Geography(),
                        Cidade_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("global.Cidade", t => t.Cidade_Id)
                .Index(t => t.Cidade_Id);
            
            CreateTable(
                "global.Escolaridade",
                c => new
                    {
                        IdEscolaridade = c.Short(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 50, unicode: false),
                        DescricaoBNE = c.String(nullable: false, maxLength: 50, unicode: false),
                        Abreviacao = c.String(nullable: false, maxLength: 8, unicode: false),
                        IdGrauEscolaridadeGlobal = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.IdEscolaridade)
                .ForeignKey("global.GrauEscolaridade", t => t.IdGrauEscolaridadeGlobal, cascadeDelete: true)
                .Index(t => t.IdGrauEscolaridadeGlobal);
            
            CreateTable(
                "global.GrauEscolaridade",
                c => new
                    {
                        IdGrauEscolaridade = c.Short(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.IdGrauEscolaridade);
            
            CreateTable(
                "pessoafisica.EstadoCivil",
                c => new
                    {
                        IdEstadoCivil = c.Byte(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.IdEstadoCivil);
            
            CreateTable(
                "global.Sexo",
                c => new
                    {
                        Sigla = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        Descricao = c.String(nullable: false, maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.Sigla);
            
            CreateTable(
                "pessoafisica.CodigoConfirmacaoEmail",
                c => new
                    {
                        IdCodigoConfirmacaoEmail = c.Int(nullable: false, identity: true),
                        DataCriacao = c.DateTime(nullable: false),
                        DataUtilizacao = c.DateTime(),
                        Email = c.String(nullable: false, maxLength: 100, unicode: false),
                        Codigo = c.String(nullable: false, maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.IdCodigoConfirmacaoEmail);
            
            CreateTable(
                "pessoafisica.Curriculo",
                c => new
                    {
                        IdCurriculo = c.Int(nullable: false, identity: true),
                        PretensaoSalarial = c.Decimal(nullable: false, precision: 10, scale: 2),
                        DataCadastro = c.DateTime(nullable: false),
                        DataAtualizacao = c.DateTime(),
                        DataModificacao = c.DateTime(),
                        FlgDisponivelViagem = c.Boolean(nullable: false),
                        FlgVIP = c.Boolean(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        Observacao = c.String(maxLength: 2000, unicode: false),
                        Conhecimento = c.String(maxLength: 2000, unicode: false),
                        IdPessoaFisica = c.Int(nullable: false),
                        IdSituacaoCurriculo = c.Int(nullable: false),
                        IdTipoCurriculo = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.IdCurriculo)
                .ForeignKey("pessoafisica.PessoaFisica", t => t.IdPessoaFisica, cascadeDelete: true)
                .ForeignKey("pessoafisica.SituacaoCurriculo", t => t.IdSituacaoCurriculo, cascadeDelete: true)
                .ForeignKey("pessoafisica.TipoCurriculo", t => t.IdTipoCurriculo, cascadeDelete: true)
                .Index(t => t.IdPessoaFisica)
                .Index(t => t.IdSituacaoCurriculo)
                .Index(t => t.IdTipoCurriculo);
            
            CreateTable(
                "pessoafisica.SituacaoCurriculo",
                c => new
                    {
                        IdSituacaoCurriculo = c.Int(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.IdSituacaoCurriculo);
            
            CreateTable(
                "pessoafisica.TipoCurriculo",
                c => new
                    {
                        IdTipoCurriculo = c.Short(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.IdTipoCurriculo);
            
            CreateTable(
                "pessoafisica.CurriculoAnexo",
                c => new
                    {
                        IdCurriculoAnexo = c.Int(nullable: false, identity: true),
                        UrlArquivo = c.String(nullable: false, maxLength: 8000, unicode: false),
                        DataCadastro = c.DateTime(nullable: false, storeType: "date"),
                        Ativo = c.Boolean(nullable: false),
                        IdCurriculo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdCurriculoAnexo)
                .ForeignKey("pessoafisica.Curriculo", t => t.IdCurriculo, cascadeDelete: true)
                .Index(t => t.IdCurriculo);
            
            CreateTable(
                "pessoafisica.CurriculoDisponibilidade",
                c => new
                    {
                        IdCurriculo = c.Int(nullable: false),
                        IdDisponibilidadeGlobal = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => new { t.IdCurriculo, t.IdDisponibilidadeGlobal })
                .ForeignKey("pessoafisica.Curriculo", t => t.IdCurriculo, cascadeDelete: true)
                .ForeignKey("global.Disponibilidade", t => t.IdDisponibilidadeGlobal, cascadeDelete: true)
                .Index(t => t.IdCurriculo)
                .Index(t => t.IdDisponibilidadeGlobal);
            
            CreateTable(
                "global.Disponibilidade",
                c => new
                    {
                        IdDisponibilidade = c.Byte(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 30, unicode: false),
                    })
                .PrimaryKey(t => t.IdDisponibilidade);
            
            CreateTable(
                "pessoafisica.CurriculoOrigem",
                c => new
                    {
                        IdCurriculoOrigem = c.Int(nullable: false, identity: true),
                        DataCadastro = c.DateTime(nullable: false),
                        IdCurriculo = c.Int(nullable: false),
                        IdOrigem = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdCurriculoOrigem)
                .ForeignKey("pessoafisica.Curriculo", t => t.IdCurriculo, cascadeDelete: true)
                .ForeignKey("global.Origem", t => t.IdOrigem, cascadeDelete: true)
                .Index(t => t.IdCurriculo)
                .Index(t => t.IdOrigem);
            
            CreateTable(
                "global.Origem",
                c => new
                    {
                        IdOrigem = c.Int(nullable: false, identity: true),
                        Descricao = c.String(maxLength: 100, unicode: false),
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        URL = c.String(maxLength: 120, unicode: false),
                        IdTipoOrigem = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.IdOrigem)
                .ForeignKey("global.TipoOrigem", t => t.IdTipoOrigem, cascadeDelete: true)
                .Index(t => t.IdTipoOrigem);
            
            CreateTable(
                "global.TipoOrigem",
                c => new
                    {
                        IdTipoOrigem = c.Byte(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 30, unicode: false),
                    })
                .PrimaryKey(t => t.IdTipoOrigem);
            
            CreateTable(
                "pessoafisica.CurriculoParametro",
                c => new
                    {
                        IdCurriculoParametro = c.Int(nullable: false, identity: true),
                        Valor = c.String(maxLength: 8000, unicode: false),
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        IdCurriculo = c.Int(nullable: false),
                        IdParametro = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.IdCurriculoParametro)
                .ForeignKey("pessoafisica.Curriculo", t => t.IdCurriculo, cascadeDelete: true)
                .ForeignKey("pessoafisica.Parametro", t => t.IdParametro, cascadeDelete: true)
                .Index(t => t.IdCurriculo)
                .Index(t => t.IdParametro);
            
            CreateTable(
                "pessoafisica.Parametro",
                c => new
                    {
                        IdParametro = c.Short(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 70, unicode: false),
                        Valor = c.String(nullable: false, maxLength: 8000, unicode: false),
                    })
                .PrimaryKey(t => t.IdParametro);
            
            CreateTable(
                "pessoafisica.Curso",
                c => new
                    {
                        IdCurso = c.Int(nullable: false, identity: true),
                        CodigoCurso = c.String(maxLength: 50, unicode: false),
                        Descricao = c.String(nullable: false, maxLength: 100, unicode: false),
                        FlgMEC = c.Boolean(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        FlgAuditado = c.Boolean(nullable: false),
                        IdNivelCurso = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.IdCurso)
                .ForeignKey("pessoafisica.NivelCurso", t => t.IdNivelCurso, cascadeDelete: true)
                .Index(t => t.IdNivelCurso);
            
            CreateTable(
                "pessoafisica.NivelCurso",
                c => new
                    {
                        IdNivelCurso = c.Byte(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 50, unicode: false),
                        IdGrauEscolaridadeGlobal = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.IdNivelCurso)
                .ForeignKey("global.GrauEscolaridade", t => t.IdGrauEscolaridadeGlobal, cascadeDelete: true)
                .Index(t => t.IdGrauEscolaridadeGlobal);
            
            CreateTable(
                "pessoafisica.ExperienciaProfissional",
                c => new
                    {
                        IdExperienciaProfissional = c.Long(nullable: false, identity: true),
                        NomeEmpresa = c.String(nullable: false, maxLength: 100, unicode: false),
                        DataEntrada = c.DateTime(nullable: false, storeType: "date"),
                        DataSaida = c.DateTime(storeType: "date"),
                        DataCadastro = c.DateTime(nullable: false),
                        AtividadesExercidas = c.String(maxLength: 8000, unicode: false),
                        FuncaoExercida = c.String(maxLength: 100, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                        FlgImportado = c.Boolean(),
                        UltimoSalario = c.Decimal(precision: 10, scale: 2),
                        IdPessoaFisica = c.Int(nullable: false),
                        IdRamoAtividadeGlobal = c.Int(),
                    })
                .PrimaryKey(t => t.IdExperienciaProfissional)
                .ForeignKey("pessoafisica.PessoaFisica", t => t.IdPessoaFisica, cascadeDelete: true)
                .ForeignKey("global.RamoAtividade", t => t.IdRamoAtividadeGlobal)
                .Index(t => t.IdPessoaFisica)
                .Index(t => t.IdRamoAtividadeGlobal);
            
            CreateTable(
                "pessoafisica.Formacao",
                c => new
                    {
                        IdFormacao = c.Int(nullable: false, identity: true),
                        AnoConclusao = c.Short(),
                        DataCadastro = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                        CargaHoraria = c.Short(),
                        Ativo = c.Boolean(nullable: false),
                        NomeInstituicao = c.String(maxLength: 200, unicode: false),
                        NomeCurso = c.String(maxLength: 200, unicode: false),
                        IdCidade = c.Int(),
                        IdCurso = c.Int(),
                        IdEscolaridadeGlobal = c.Short(nullable: false),
                        IdInstituicaoEnsino = c.Int(),
                        IdPessoaFisica = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdFormacao)
                .ForeignKey("global.Cidade", t => t.IdCidade)
                .ForeignKey("pessoafisica.Curso", t => t.IdCurso)
                .ForeignKey("global.Escolaridade", t => t.IdEscolaridadeGlobal, cascadeDelete: true)
                .ForeignKey("pessoafisica.InstituicaoEnsino", t => t.IdInstituicaoEnsino)
                .ForeignKey("pessoafisica.PessoaFisica", t => t.IdPessoaFisica, cascadeDelete: true)
                .Index(t => t.IdCidade)
                .Index(t => t.IdCurso)
                .Index(t => t.IdEscolaridadeGlobal)
                .Index(t => t.IdInstituicaoEnsino)
                .Index(t => t.IdPessoaFisica);
            
            CreateTable(
                "pessoafisica.InstituicaoEnsino",
                c => new
                    {
                        IdInstituicaoEnsino = c.Int(nullable: false, identity: true),
                        Sigla = c.String(nullable: false, maxLength: 20, unicode: false),
                        Nome = c.String(nullable: false, maxLength: 100, unicode: false),
                        FlgMEC = c.Boolean(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdInstituicaoEnsino);
            
            CreateTable(
                "pessoafisica.Foto",
                c => new
                    {
                        IdFoto = c.Long(nullable: false, identity: true),
                        Url = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        DataAtualizacao = c.DateTime(),
                        IdPessoaFisica = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdFoto)
                .ForeignKey("pessoafisica.PessoaFisica", t => t.IdPessoaFisica, cascadeDelete: true)
                .Index(t => t.IdPessoaFisica);
            
            CreateTable(
                "global.Funcao",
                c => new
                    {
                        IdFuncao = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 100, unicode: false),
                        Prioridade = c.Short(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                        FlgInativo = c.Boolean(nullable: false),
                        IdCargoGlobal = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.IdFuncao)
                .ForeignKey("global.Cargo", t => t.IdCargoGlobal, cascadeDelete: true)
                .Index(t => t.IdCargoGlobal);
            
            CreateTable(
                "pessoafisica.FuncaoPretendida",
                c => new
                    {
                        IdFuncaoPretendida = c.Long(nullable: false, identity: true),
                        IdFuncao = c.Int(),
                        DataCadastro = c.DateTime(nullable: false),
                        Descricao = c.String(maxLength: 50, unicode: false),
                        TempoExperiencia = c.Short(nullable: false),
                        IdCurriculo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdFuncaoPretendida)
                .ForeignKey("pessoafisica.Curriculo", t => t.IdCurriculo, cascadeDelete: true)
                .Index(t => t.IdCurriculo);
            
            CreateTable(
                "global.FuncaoSinonimo",
                c => new
                    {
                        IdFuncaoSinonimo = c.Int(nullable: false, identity: true),
                        IdSinonimoSubstituto = c.Int(),
                        NomeSinonimo = c.String(maxLength: 100, unicode: false),
                        DataCadastro = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                        CodigoCBO = c.String(nullable: false, maxLength: 6, unicode: false),
                        DescricaoPesquisa = c.String(nullable: false, maxLength: 100, unicode: false),
                        DescricaoJob = c.String(maxLength: 2000, unicode: false),
                        Atribuicoes = c.String(maxLength: 8000, unicode: false),
                        Responsabilidades = c.String(maxLength: 8000, unicode: false),
                        Beneficio = c.String(maxLength: 8000, unicode: false),
                        FlgInativo = c.Boolean(nullable: false),
                        FlgAuditada = c.Boolean(nullable: false),
                        IdEscolaricadeGlobal = c.Short(nullable: false),
                        IdFuncaoGlobal = c.Int(nullable: false),
                        IdTipoFuncaoGlobal = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.IdFuncaoSinonimo)
                .ForeignKey("global.Escolaridade", t => t.IdEscolaricadeGlobal, cascadeDelete: true)
                .ForeignKey("global.Funcao", t => t.IdFuncaoGlobal, cascadeDelete: true)
                .ForeignKey("global.TipoFuncao", t => t.IdTipoFuncaoGlobal, cascadeDelete: true)
                .Index(t => t.IdEscolaricadeGlobal)
                .Index(t => t.IdFuncaoGlobal)
                .Index(t => t.IdTipoFuncaoGlobal);
            
            CreateTable(
                "global.TipoFuncao",
                c => new
                    {
                        IdTipoFuncao = c.Byte(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 30, unicode: false),
                    })
                .PrimaryKey(t => t.IdTipoFuncao);
            
            CreateTable(
                "pessoafisica.Idioma",
                c => new
                    {
                        IdIdioma = c.Int(nullable: false, identity: true),
                        Ativo = c.Boolean(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        IdIdiomaGlobal = c.Byte(nullable: false),
                        IdNivelIdioma = c.Byte(nullable: false),
                        IdPessoaFisica = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdIdioma)
                .ForeignKey("global.Idioma", t => t.IdIdiomaGlobal, cascadeDelete: true)
                .ForeignKey("pessoafisica.NivelIdioma", t => t.IdNivelIdioma, cascadeDelete: true)
                .ForeignKey("pessoafisica.PessoaFisica", t => t.IdPessoaFisica, cascadeDelete: true)
                .Index(t => t.IdIdiomaGlobal)
                .Index(t => t.IdNivelIdioma)
                .Index(t => t.IdPessoaFisica);
            
            CreateTable(
                "global.Idioma",
                c => new
                    {
                        IdIdioma = c.Byte(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 30, unicode: false),
                    })
                .PrimaryKey(t => t.IdIdioma);
            
            CreateTable(
                "pessoafisica.NivelIdioma",
                c => new
                    {
                        IdNivelIdioma = c.Byte(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.IdNivelIdioma);
            
            CreateTable(
                "pessoafisica.Email",
                c => new
                    {
                        Endereco = c.String(nullable: false, maxLength: 100, unicode: false),
                        IdPessoaFisica = c.Int(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.Endereco, t.IdPessoaFisica })
                .ForeignKey("pessoafisica.PessoaFisica", t => t.IdPessoaFisica, cascadeDelete: true)
                .Index(t => t.IdPessoaFisica);
            
            CreateTable(
                "pessoafisica.PreCurriculo",
                c => new
                    {
                        IdPreCurriculo = c.Int(nullable: false, identity: true),
                        Nome = c.String(maxLength: 100, unicode: false),
                        Email = c.String(maxLength: 100, unicode: false),
                        DDDCelular = c.String(maxLength: 2, unicode: false),
                        Celular = c.String(maxLength: 10, unicode: false),
                        IdFuncao = c.Int(),
                        DescricaoFuncao = c.String(maxLength: 50, unicode: false),
                        IdCidade = c.Int(),
                        TempoExperiencia = c.Short(),
                        PretensaoSalarial = c.Decimal(precision: 10, scale: 2),
                        IdVaga = c.Int(),
                        IdCurriculo = c.Int(),
                        DataCadastro = c.DateTime(nullable: false),
                        SiglaSexo = c.String(maxLength: 1, fixedLength: true, unicode: false),
                    })
                .PrimaryKey(t => t.IdPreCurriculo)
                .ForeignKey("global.Sexo", t => t.SiglaSexo)
                .Index(t => t.SiglaSexo);
            
            CreateTable(
                "global.RankingEmail",
                c => new
                    {
                        IdRankingEmail = c.Int(nullable: false, identity: true),
                        DescricaoEmail = c.String(nullable: false, maxLength: 30, unicode: false),
                    })
                .PrimaryKey(t => t.IdRankingEmail);
            
            CreateTable(
                "pessoafisica.SituacaoFormacao",
                c => new
                    {
                        IdSituacaoFormacao = c.Byte(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.IdSituacaoFormacao);
            
            CreateTable(
                "pessoafisica.Telefone",
                c => new
                    {
                        IdTelefone = c.Long(nullable: false, identity: true),
                        FalarCom = c.String(maxLength: 50, unicode: false),
                        DDD = c.Byte(nullable: false),
                        Numero = c.Decimal(nullable: false, precision: 10, scale: 0),
                        Ramal = c.Decimal(precision: 5, scale: 0),
                        IdPessoaFisica = c.Int(),
                        IdTipoTelefone = c.Int(),
                    })
                .PrimaryKey(t => t.IdTelefone)
                .ForeignKey("pessoafisica.PessoaFisica", t => t.IdPessoaFisica)
                .Index(t => t.IdPessoaFisica);
            
            CreateTable(
                "global.TipoTelefone",
                c => new
                    {
                        IdTipoTelefone = c.Short(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.IdTipoTelefone);
            
            CreateTable(
                "pessoafisica.TipoVeiculo",
                c => new
                    {
                        IdTipoVeiculo = c.Byte(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 30, unicode: false),
                    })
                .PrimaryKey(t => t.IdTipoVeiculo);
            
            CreateTable(
                "pessoafisica.Veiculo",
                c => new
                    {
                        IdVeiculo = c.Int(nullable: false, identity: true),
                        Modelo = c.String(maxLength: 50, unicode: false),
                        Ano = c.Short(nullable: false),
                        IdPessoaFisica = c.Int(nullable: false),
                        IdTipoVeiculo = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.IdVeiculo)
                .ForeignKey("pessoafisica.PessoaFisica", t => t.IdPessoaFisica, cascadeDelete: true)
                .ForeignKey("pessoafisica.TipoVeiculo", t => t.IdTipoVeiculo, cascadeDelete: true)
                .Index(t => t.IdPessoaFisica)
                .Index(t => t.IdTipoVeiculo);
            
        }
        
        public override void Down()
        {
            DropForeignKey("pessoafisica.Veiculo", "IdTipoVeiculo", "pessoafisica.TipoVeiculo");
            DropForeignKey("pessoafisica.Veiculo", "IdPessoaFisica", "pessoafisica.PessoaFisica");
            DropForeignKey("pessoafisica.Telefone", "IdPessoaFisica", "pessoafisica.PessoaFisica");
            DropForeignKey("pessoafisica.PreCurriculo", "SiglaSexo", "global.Sexo");
            DropForeignKey("pessoafisica.Email", "IdPessoaFisica", "pessoafisica.PessoaFisica");
            DropForeignKey("pessoafisica.Idioma", "IdPessoaFisica", "pessoafisica.PessoaFisica");
            DropForeignKey("pessoafisica.Idioma", "IdNivelIdioma", "pessoafisica.NivelIdioma");
            DropForeignKey("pessoafisica.Idioma", "IdIdiomaGlobal", "global.Idioma");
            DropForeignKey("global.FuncaoSinonimo", "IdTipoFuncaoGlobal", "global.TipoFuncao");
            DropForeignKey("global.FuncaoSinonimo", "IdFuncaoGlobal", "global.Funcao");
            DropForeignKey("global.FuncaoSinonimo", "IdEscolaricadeGlobal", "global.Escolaridade");
            DropForeignKey("pessoafisica.FuncaoPretendida", "IdCurriculo", "pessoafisica.Curriculo");
            DropForeignKey("global.Funcao", "IdCargoGlobal", "global.Cargo");
            DropForeignKey("pessoafisica.Foto", "IdPessoaFisica", "pessoafisica.PessoaFisica");
            DropForeignKey("pessoafisica.Formacao", "IdPessoaFisica", "pessoafisica.PessoaFisica");
            DropForeignKey("pessoafisica.Formacao", "IdInstituicaoEnsino", "pessoafisica.InstituicaoEnsino");
            DropForeignKey("pessoafisica.Formacao", "IdEscolaridadeGlobal", "global.Escolaridade");
            DropForeignKey("pessoafisica.Formacao", "IdCurso", "pessoafisica.Curso");
            DropForeignKey("pessoafisica.Formacao", "IdCidade", "global.Cidade");
            DropForeignKey("pessoafisica.ExperienciaProfissional", "IdRamoAtividadeGlobal", "global.RamoAtividade");
            DropForeignKey("pessoafisica.ExperienciaProfissional", "IdPessoaFisica", "pessoafisica.PessoaFisica");
            DropForeignKey("pessoafisica.Curso", "IdNivelCurso", "pessoafisica.NivelCurso");
            DropForeignKey("pessoafisica.NivelCurso", "IdGrauEscolaridadeGlobal", "global.GrauEscolaridade");
            DropForeignKey("pessoafisica.CurriculoParametro", "IdParametro", "pessoafisica.Parametro");
            DropForeignKey("pessoafisica.CurriculoParametro", "IdCurriculo", "pessoafisica.Curriculo");
            DropForeignKey("pessoafisica.CurriculoOrigem", "IdOrigem", "global.Origem");
            DropForeignKey("global.Origem", "IdTipoOrigem", "global.TipoOrigem");
            DropForeignKey("pessoafisica.CurriculoOrigem", "IdCurriculo", "pessoafisica.Curriculo");
            DropForeignKey("pessoafisica.CurriculoDisponibilidade", "IdDisponibilidadeGlobal", "global.Disponibilidade");
            DropForeignKey("pessoafisica.CurriculoDisponibilidade", "IdCurriculo", "pessoafisica.Curriculo");
            DropForeignKey("pessoafisica.CurriculoAnexo", "IdCurriculo", "pessoafisica.Curriculo");
            DropForeignKey("pessoafisica.Curriculo", "IdTipoCurriculo", "pessoafisica.TipoCurriculo");
            DropForeignKey("pessoafisica.Curriculo", "IdSituacaoCurriculo", "pessoafisica.SituacaoCurriculo");
            DropForeignKey("pessoafisica.Curriculo", "IdPessoaFisica", "pessoafisica.PessoaFisica");
            DropForeignKey("pessoafisica.CidadePretendida", "IdPessoaFisica", "pessoafisica.PessoaFisica");
            DropForeignKey("pessoafisica.PessoaFisica", "SiglaSexo", "global.Sexo");
            DropForeignKey("pessoafisica.PessoaFisica", "IdEstadoCivil", "pessoafisica.EstadoCivil");
            DropForeignKey("pessoafisica.PessoaFisica", "IdEscolaridadeGlobal", "global.Escolaridade");
            DropForeignKey("global.Escolaridade", "IdGrauEscolaridadeGlobal", "global.GrauEscolaridade");
            DropForeignKey("pessoafisica.PessoaFisica", "IdEndereco", "pessoafisica.Endereco");
            DropForeignKey("pessoafisica.Endereco", "Cidade_Id", "global.Cidade");
            DropForeignKey("pessoafisica.Endereco", "Bairro_Id", "dbo.Bairro");
            DropForeignKey("dbo.Bairro", "Cidade_Id", "global.Cidade");
            DropForeignKey("pessoafisica.PessoaFisica", "IdDeficienciaGlobal", "global.Deficiencia");
            DropForeignKey("pessoafisica.PessoaFisica", "IdCidade", "global.Cidade");
            DropForeignKey("pessoafisica.CidadePretendida", "IdCidadeGlobal", "global.Cidade");
            DropForeignKey("global.Cidade", "UF", "global.Estado");
            DropForeignKey("global.Cargo", "IdRamoAtividadeGlobal", "global.RamoAtividade");
            DropForeignKey("global.Cargo", "IdCategoriaCargoGlobal", "global.CategoriaCargo");
            DropIndex("pessoafisica.Veiculo", new[] { "IdTipoVeiculo" });
            DropIndex("pessoafisica.Veiculo", new[] { "IdPessoaFisica" });
            DropIndex("pessoafisica.Telefone", new[] { "IdPessoaFisica" });
            DropIndex("pessoafisica.PreCurriculo", new[] { "SiglaSexo" });
            DropIndex("pessoafisica.Email", new[] { "IdPessoaFisica" });
            DropIndex("pessoafisica.Idioma", new[] { "IdPessoaFisica" });
            DropIndex("pessoafisica.Idioma", new[] { "IdNivelIdioma" });
            DropIndex("pessoafisica.Idioma", new[] { "IdIdiomaGlobal" });
            DropIndex("global.FuncaoSinonimo", new[] { "IdTipoFuncaoGlobal" });
            DropIndex("global.FuncaoSinonimo", new[] { "IdFuncaoGlobal" });
            DropIndex("global.FuncaoSinonimo", new[] { "IdEscolaricadeGlobal" });
            DropIndex("pessoafisica.FuncaoPretendida", new[] { "IdCurriculo" });
            DropIndex("global.Funcao", new[] { "IdCargoGlobal" });
            DropIndex("pessoafisica.Foto", new[] { "IdPessoaFisica" });
            DropIndex("pessoafisica.Formacao", new[] { "IdPessoaFisica" });
            DropIndex("pessoafisica.Formacao", new[] { "IdInstituicaoEnsino" });
            DropIndex("pessoafisica.Formacao", new[] { "IdEscolaridadeGlobal" });
            DropIndex("pessoafisica.Formacao", new[] { "IdCurso" });
            DropIndex("pessoafisica.Formacao", new[] { "IdCidade" });
            DropIndex("pessoafisica.ExperienciaProfissional", new[] { "IdRamoAtividadeGlobal" });
            DropIndex("pessoafisica.ExperienciaProfissional", new[] { "IdPessoaFisica" });
            DropIndex("pessoafisica.NivelCurso", new[] { "IdGrauEscolaridadeGlobal" });
            DropIndex("pessoafisica.Curso", new[] { "IdNivelCurso" });
            DropIndex("pessoafisica.CurriculoParametro", new[] { "IdParametro" });
            DropIndex("pessoafisica.CurriculoParametro", new[] { "IdCurriculo" });
            DropIndex("global.Origem", new[] { "IdTipoOrigem" });
            DropIndex("pessoafisica.CurriculoOrigem", new[] { "IdOrigem" });
            DropIndex("pessoafisica.CurriculoOrigem", new[] { "IdCurriculo" });
            DropIndex("pessoafisica.CurriculoDisponibilidade", new[] { "IdDisponibilidadeGlobal" });
            DropIndex("pessoafisica.CurriculoDisponibilidade", new[] { "IdCurriculo" });
            DropIndex("pessoafisica.CurriculoAnexo", new[] { "IdCurriculo" });
            DropIndex("pessoafisica.Curriculo", new[] { "IdTipoCurriculo" });
            DropIndex("pessoafisica.Curriculo", new[] { "IdSituacaoCurriculo" });
            DropIndex("pessoafisica.Curriculo", new[] { "IdPessoaFisica" });
            DropIndex("global.Escolaridade", new[] { "IdGrauEscolaridadeGlobal" });
            DropIndex("dbo.Bairro", new[] { "Cidade_Id" });
            DropIndex("pessoafisica.Endereco", new[] { "Cidade_Id" });
            DropIndex("pessoafisica.Endereco", new[] { "Bairro_Id" });
            DropIndex("pessoafisica.PessoaFisica", new[] { "SiglaSexo" });
            DropIndex("pessoafisica.PessoaFisica", new[] { "IdEstadoCivil" });
            DropIndex("pessoafisica.PessoaFisica", new[] { "IdEscolaridadeGlobal" });
            DropIndex("pessoafisica.PessoaFisica", new[] { "IdEndereco" });
            DropIndex("pessoafisica.PessoaFisica", new[] { "IdDeficienciaGlobal" });
            DropIndex("pessoafisica.PessoaFisica", new[] { "IdCidade" });
            DropIndex("pessoafisica.CidadePretendida", new[] { "IdPessoaFisica" });
            DropIndex("pessoafisica.CidadePretendida", new[] { "IdCidadeGlobal" });
            DropIndex("global.Cidade", new[] { "UF" });
            DropIndex("global.Cargo", new[] { "IdRamoAtividadeGlobal" });
            DropIndex("global.Cargo", new[] { "IdCategoriaCargoGlobal" });
            DropTable("pessoafisica.Veiculo");
            DropTable("pessoafisica.TipoVeiculo");
            DropTable("global.TipoTelefone");
            DropTable("pessoafisica.Telefone");
            DropTable("pessoafisica.SituacaoFormacao");
            DropTable("global.RankingEmail");
            DropTable("pessoafisica.PreCurriculo");
            DropTable("pessoafisica.Email");
            DropTable("pessoafisica.NivelIdioma");
            DropTable("global.Idioma");
            DropTable("pessoafisica.Idioma");
            DropTable("global.TipoFuncao");
            DropTable("global.FuncaoSinonimo");
            DropTable("pessoafisica.FuncaoPretendida");
            DropTable("global.Funcao");
            DropTable("pessoafisica.Foto");
            DropTable("pessoafisica.InstituicaoEnsino");
            DropTable("pessoafisica.Formacao");
            DropTable("pessoafisica.ExperienciaProfissional");
            DropTable("pessoafisica.NivelCurso");
            DropTable("pessoafisica.Curso");
            DropTable("pessoafisica.Parametro");
            DropTable("pessoafisica.CurriculoParametro");
            DropTable("global.TipoOrigem");
            DropTable("global.Origem");
            DropTable("pessoafisica.CurriculoOrigem");
            DropTable("global.Disponibilidade");
            DropTable("pessoafisica.CurriculoDisponibilidade");
            DropTable("pessoafisica.CurriculoAnexo");
            DropTable("pessoafisica.TipoCurriculo");
            DropTable("pessoafisica.SituacaoCurriculo");
            DropTable("pessoafisica.Curriculo");
            DropTable("pessoafisica.CodigoConfirmacaoEmail");
            DropTable("global.Sexo");
            DropTable("pessoafisica.EstadoCivil");
            DropTable("global.GrauEscolaridade");
            DropTable("global.Escolaridade");
            DropTable("dbo.Bairro");
            DropTable("pessoafisica.Endereco");
            DropTable("global.Deficiencia");
            DropTable("pessoafisica.PessoaFisica");
            DropTable("pessoafisica.CidadePretendida");
            DropTable("global.Estado");
            DropTable("global.Cidade");
            DropTable("global.RamoAtividade");
            DropTable("global.CategoriaCargo");
            DropTable("global.Cargo");
        }
    }
}
