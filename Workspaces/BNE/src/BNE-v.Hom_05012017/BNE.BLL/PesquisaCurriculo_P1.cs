//-- Data: 28/01/2016 11:09
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;
using Microsoft.SqlServer.Types;

namespace BNE.BLL
{
	public partial class PesquisaCurriculo // Tabela: TAB_Pesquisa_Curriculo
	{
		#region Atributos
		private int _idPesquisaCurriculo;
		private UsuarioFilialPerfil _usuarioFilialPerfil;
		private Curriculo _curriculo;
		private string _descricaoIP;
		private Cidade _cidade;
		private string _descricaoPalavraChave;
		private Estado _estado;
		private Escolaridade _escolaridade;
		private Sexo _sexo;
		private decimal? _numeroSalarioMin;
		private decimal? _numeroSalarioMax;
		private Int16? _quantidadeExperiencia;
		private string _descricaoCodCPFNome;
		private EstadoCivil _estadoCivil;
		private string _descricaoBairro;
		private string _descricaoLogradouro;
		private string _numeroCEPMin;
		private string _numeroCEPMax;
		private string _descricaoExperiencia;
		private Curso _cursoTecnicoGraduacao;
		private Fonte _fonteTecnicoGraduacao;
		private string _razaoSocial;
		private AreaBNE _areaBNE;
		private CategoriaHabilitacao _categoriaHabilitacao;
		private string _numeroDDDTelefone;
		private string _numeroTelefone;
		private string _emailPessoa;
		private Deficiencia _deficiencia;
		private DateTime? _dataCadastro;
		private TipoVeiculo _tipoVeiculo;
		private Curso _cursoOutrosCursos;
		private Fonte _fonteOutrosCursos;
		private Raca _raca;
		private bool? _flagFilhos;
		private bool _flagPesquisaAvancada;
		private string _descricaoCursoTecnicoGraduacao;
		private string _descricaoCursoOutrosCursos;
		private int? _quantidadeCurriculoRetorno;
		private string _idEscolaridadeWebStagio;
		private bool? _flagAprendiz;
		private string _descricaoFiltroExcludente;
		private string _descricaoZona;
		private SqlGeography _GeoBuscaBairro;
		private SqlGeography _GeoBuscaCidade;
		private Funcao _funcaoExercida;
		private string _descricaoFuncaoExercida;
		private Origem _origem;
		private string _descricaoAvaliacao;
		private Int16? _numeroIdadeMin;
		private Int16? _numeroIdadeMax;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdPesquisaCurriculo
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdPesquisaCurriculo
		{
			get
			{
				return this._idPesquisaCurriculo;
			}
		}
		#endregion 

		#region UsuarioFilialPerfil
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public UsuarioFilialPerfil UsuarioFilialPerfil
		{
			get
			{
				return this._usuarioFilialPerfil;
			}
			set
			{
				this._usuarioFilialPerfil = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Curriculo
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Curriculo Curriculo
		{
			get
			{
				return this._curriculo;
			}
			set
			{
				this._curriculo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoIP
		/// <summary>
		/// Tamanho do campo: 15.
		/// Campo opcional.
		/// </summary>
		public string DescricaoIP
		{
			get
			{
				return this._descricaoIP;
			}
			set
			{
				this._descricaoIP = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Cidade
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Cidade Cidade
		{
			get
			{
				return this._cidade;
			}
			set
			{
				this._cidade = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoPalavraChave
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string DescricaoPalavraChave
		{
			get
			{
				return this._descricaoPalavraChave;
			}
			set
			{
				this._descricaoPalavraChave = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Estado
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Estado Estado
		{
			get
			{
				return this._estado;
			}
			set
			{
				this._estado = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Escolaridade
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Escolaridade Escolaridade
		{
			get
			{
				return this._escolaridade;
			}
			set
			{
				this._escolaridade = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Sexo
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Sexo Sexo
		{
			get
			{
				return this._sexo;
			}
			set
			{
				this._sexo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroSalarioMin
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public decimal? NumeroSalarioMin
		{
			get
			{
				return this._numeroSalarioMin;
			}
			set
			{
				this._numeroSalarioMin = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroSalarioMax
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public decimal? NumeroSalarioMax
		{
			get
			{
				return this._numeroSalarioMax;
			}
			set
			{
				this._numeroSalarioMax = value;
				this._modified = true;
			}
		}
		#endregion 

		#region QuantidadeExperiencia
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Int16? QuantidadeExperiencia
		{
			get
			{
				return this._quantidadeExperiencia;
			}
			set
			{
				this._quantidadeExperiencia = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoCodCPFNome
		/// <summary>
		/// Tamanho do campo: 200.
		/// Campo opcional.
		/// </summary>
		public string DescricaoCodCPFNome
		{
			get
			{
				return this._descricaoCodCPFNome;
			}
			set
			{
				this._descricaoCodCPFNome = value;
				this._modified = true;
			}
		}
		#endregion 

		#region EstadoCivil
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public EstadoCivil EstadoCivil
		{
			get
			{
				return this._estadoCivil;
			}
			set
			{
				this._estadoCivil = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoBairro
		/// <summary>
		/// Tamanho do campo: -1.
		/// Campo opcional.
		/// </summary>
		public string DescricaoBairro
		{
			get
			{
				return this._descricaoBairro;
			}
			set
			{
				this._descricaoBairro = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoLogradouro
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string DescricaoLogradouro
		{
			get
			{
				return this._descricaoLogradouro;
			}
			set
			{
				this._descricaoLogradouro = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroCEPMin
		/// <summary>
		/// Tamanho do campo: 8.
		/// Campo opcional.
		/// </summary>
		public string NumeroCEPMin
		{
			get
			{
				return this._numeroCEPMin;
			}
			set
			{
				this._numeroCEPMin = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroCEPMax
		/// <summary>
		/// Tamanho do campo: 8.
		/// Campo opcional.
		/// </summary>
		public string NumeroCEPMax
		{
			get
			{
				return this._numeroCEPMax;
			}
			set
			{
				this._numeroCEPMax = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoExperiencia
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string DescricaoExperiencia
		{
			get
			{
				return this._descricaoExperiencia;
			}
			set
			{
				this._descricaoExperiencia = value;
				this._modified = true;
			}
		}
		#endregion 

		#region CursoTecnicoGraduacao
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Curso CursoTecnicoGraduacao
		{
			get
			{
				return this._cursoTecnicoGraduacao;
			}
			set
			{
				this._cursoTecnicoGraduacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FonteTecnicoGraduacao
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Fonte FonteTecnicoGraduacao
		{
			get
			{
				return this._fonteTecnicoGraduacao;
			}
			set
			{
				this._fonteTecnicoGraduacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region RazaoSocial
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string RazaoSocial
		{
			get
			{
				return this._razaoSocial;
			}
			set
			{
				this._razaoSocial = value;
				this._modified = true;
			}
		}
		#endregion 

		#region AreaBNE
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public AreaBNE AreaBNE
		{
			get
			{
				return this._areaBNE;
			}
			set
			{
				this._areaBNE = value;
				this._modified = true;
			}
		}
		#endregion 

		#region CategoriaHabilitacao
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public CategoriaHabilitacao CategoriaHabilitacao
		{
			get
			{
				return this._categoriaHabilitacao;
			}
			set
			{
				this._categoriaHabilitacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroDDDTelefone
		/// <summary>
		/// Tamanho do campo: 2.
		/// Campo opcional.
		/// </summary>
		public string NumeroDDDTelefone
		{
			get
			{
				return this._numeroDDDTelefone;
			}
			set
			{
				this._numeroDDDTelefone = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroTelefone
		/// <summary>
		/// Tamanho do campo: 10.
		/// Campo opcional.
		/// </summary>
		public string NumeroTelefone
		{
			get
			{
				return this._numeroTelefone;
			}
			set
			{
				this._numeroTelefone = value;
				this._modified = true;
			}
		}
		#endregion 

		#region EmailPessoa
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string EmailPessoa
		{
			get
			{
				return this._emailPessoa;
			}
			set
			{
				this._emailPessoa = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Deficiencia
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Deficiencia Deficiencia
		{
			get
			{
				return this._deficiencia;
			}
			set
			{
				this._deficiencia = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DataCadastro
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DataCadastro
		{
			get
			{
				return this._dataCadastro;
			}
		}
		#endregion 

		#region TipoVeiculo
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public TipoVeiculo TipoVeiculo
		{
			get
			{
				return this._tipoVeiculo;
			}
			set
			{
				this._tipoVeiculo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region CursoOutrosCursos
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Curso CursoOutrosCursos
		{
			get
			{
				return this._cursoOutrosCursos;
			}
			set
			{
				this._cursoOutrosCursos = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FonteOutrosCursos
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Fonte FonteOutrosCursos
		{
			get
			{
				return this._fonteOutrosCursos;
			}
			set
			{
				this._fonteOutrosCursos = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Raca
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Raca Raca
		{
			get
			{
				return this._raca;
			}
			set
			{
				this._raca = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagFilhos
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public bool? FlagFilhos
		{
			get
			{
				return this._flagFilhos;
			}
			set
			{
				this._flagFilhos = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagPesquisaAvancada
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagPesquisaAvancada
		{
			get
			{
				return this._flagPesquisaAvancada;
			}
			set
			{
				this._flagPesquisaAvancada = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoCursoTecnicoGraduacao
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string DescricaoCursoTecnicoGraduacao
		{
			get
			{
				return this._descricaoCursoTecnicoGraduacao;
			}
			set
			{
				this._descricaoCursoTecnicoGraduacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoCursoOutrosCursos
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string DescricaoCursoOutrosCursos
		{
			get
			{
				return this._descricaoCursoOutrosCursos;
			}
			set
			{
				this._descricaoCursoOutrosCursos = value;
				this._modified = true;
			}
		}
		#endregion 

		#region QuantidadeCurriculoRetorno
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public int? QuantidadeCurriculoRetorno
		{
			get
			{
				return this._quantidadeCurriculoRetorno;
			}
			set
			{
				this._quantidadeCurriculoRetorno = value;
				this._modified = true;
			}
		}
		#endregion 

		#region IdEscolaridadeWebStagio
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo opcional.
		/// </summary>
		public string IdEscolaridadeWebStagio
		{
			get
			{
				return this._idEscolaridadeWebStagio;
			}
			set
			{
				this._idEscolaridadeWebStagio = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagAprendiz
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public bool? FlagAprendiz
		{
			get
			{
				return this._flagAprendiz;
			}
			set
			{
				this._flagAprendiz = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoFiltroExcludente
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string DescricaoFiltroExcludente
		{
			get
			{
				return this._descricaoFiltroExcludente;
			}
			set
			{
				this._descricaoFiltroExcludente = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoZona
		/// <summary>
		/// Tamanho do campo: 20.
		/// Campo opcional.
		/// </summary>
		public string DescricaoZona
		{
			get
			{
				return this._descricaoZona;
			}
			set
			{
				this._descricaoZona = value;
				this._modified = true;
			}
		}
		#endregion 

		#region GeoBuscaBairro
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public SqlGeography GeoBuscaBairro
		{
			get
			{
				return this._GeoBuscaBairro;
			}
			set
			{
				this._GeoBuscaBairro = value;
				this._modified = true;
			}
		}
		#endregion 

		#region GeoBuscaCidade
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public SqlGeography GeoBuscaCidade
		{
			get
			{
				return this._GeoBuscaCidade;
			}
			set
			{
				this._GeoBuscaCidade = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FuncaoExercida
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Funcao FuncaoExercida
		{
			get
			{
				return this._funcaoExercida;
			}
			set
			{
				this._funcaoExercida = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoFuncaoExercida
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo opcional.
		/// </summary>
		public string DescricaoFuncaoExercida
		{
			get
			{
				return this._descricaoFuncaoExercida;
			}
			set
			{
				this._descricaoFuncaoExercida = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Origem
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Origem Origem
		{
			get
			{
				return this._origem;
			}
			set
			{
				this._origem = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoAvaliacao
		/// <summary>
		/// Tamanho do campo: 200.
		/// Campo opcional.
		/// </summary>
		public string DescricaoAvaliacao
		{
			get
			{
				return this._descricaoAvaliacao;
			}
			set
			{
				this._descricaoAvaliacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroIdadeMin
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Int16? NumeroIdadeMin
		{
			get
			{
				return this._numeroIdadeMin;
			}
			set
			{
				this._numeroIdadeMin = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroIdadeMax
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Int16? NumeroIdadeMax
		{
			get
			{
				return this._numeroIdadeMax;
			}
			set
			{
				this._numeroIdadeMax = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public PesquisaCurriculo()
		{
		}
		public PesquisaCurriculo(int idPesquisaCurriculo)
		{
			this._idPesquisaCurriculo = idPesquisaCurriculo;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Pesquisa_Curriculo (Idf_Usuario_Filial_Perfil, Idf_Curriculo, Des_IP, Idf_Cidade, Des_Palavra_Chave, Sig_Estado, Idf_Escolaridade, Idf_Sexo, Num_Salario_Min, Num_Salario_Max, Qtd_Experiencia, Des_Cod_CPF_Nome, Idf_Estado_Civil, Des_Bairro, Des_Logradouro, Num_CEP_Min, Num_CEP_Max, Des_Experiencia, Idf_Curso_Tecnico_Graduacao, Idf_Fonte_Tecnico_Graduacao, Raz_Social, Idf_Area_BNE, Idf_Categoria_Habilitacao, Num_DDD_Telefone, Num_Telefone, Eml_Pessoa, Idf_Deficiencia, Dta_Cadastro, Idf_Tipo_Veiculo, Idf_Curso_Outros_Cursos, Idf_Fonte_Outros_Cursos, Idf_Raca, Flg_Filhos, Flg_Pesquisa_Avancada, Des_Curso_Tecnico_Graduacao, Des_Curso_Outros_Cursos, Qtd_Curriculo_Retorno, Idf_Escolaridade_WebStagio, Flg_Aprendiz, Des_Filtro_Excludente, Des_Zona, Geo_Busca_Bairro, Geo_Busca_Cidade, Idf_Funcao_Exercida, Des_Funcao_Exercida, Idf_Origem, Des_Avaliacao, Num_Idade_Min, Num_Idade_Max) VALUES (@Idf_Usuario_Filial_Perfil, @Idf_Curriculo, @Des_IP, @Idf_Cidade, @Des_Palavra_Chave, @Sig_Estado, @Idf_Escolaridade, @Idf_Sexo, @Num_Salario_Min, @Num_Salario_Max, @Qtd_Experiencia, @Des_Cod_CPF_Nome, @Idf_Estado_Civil, @Des_Bairro, @Des_Logradouro, @Num_CEP_Min, @Num_CEP_Max, @Des_Experiencia, @Idf_Curso_Tecnico_Graduacao, @Idf_Fonte_Tecnico_Graduacao, @Raz_Social, @Idf_Area_BNE, @Idf_Categoria_Habilitacao, @Num_DDD_Telefone, @Num_Telefone, @Eml_Pessoa, @Idf_Deficiencia, @Dta_Cadastro, @Idf_Tipo_Veiculo, @Idf_Curso_Outros_Cursos, @Idf_Fonte_Outros_Cursos, @Idf_Raca, @Flg_Filhos, @Flg_Pesquisa_Avancada, @Des_Curso_Tecnico_Graduacao, @Des_Curso_Outros_Cursos, @Qtd_Curriculo_Retorno, @Idf_Escolaridade_WebStagio, @Flg_Aprendiz, @Des_Filtro_Excludente, @Des_Zona, @Geo_Busca_Bairro, @Geo_Busca_Cidade, @Idf_Funcao_Exercida, @Des_Funcao_Exercida, @Idf_Origem, @Des_Avaliacao, @Num_Idade_Min, @Num_Idade_Max);SET @Idf_Pesquisa_Curriculo = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE TAB_Pesquisa_Curriculo SET Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil, Idf_Curriculo = @Idf_Curriculo, Des_IP = @Des_IP, Idf_Cidade = @Idf_Cidade, Des_Palavra_Chave = @Des_Palavra_Chave, Sig_Estado = @Sig_Estado, Idf_Escolaridade = @Idf_Escolaridade, Idf_Sexo = @Idf_Sexo, Num_Salario_Min = @Num_Salario_Min, Num_Salario_Max = @Num_Salario_Max, Qtd_Experiencia = @Qtd_Experiencia, Des_Cod_CPF_Nome = @Des_Cod_CPF_Nome, Idf_Estado_Civil = @Idf_Estado_Civil, Des_Bairro = @Des_Bairro, Des_Logradouro = @Des_Logradouro, Num_CEP_Min = @Num_CEP_Min, Num_CEP_Max = @Num_CEP_Max, Des_Experiencia = @Des_Experiencia, Idf_Curso_Tecnico_Graduacao = @Idf_Curso_Tecnico_Graduacao, Idf_Fonte_Tecnico_Graduacao = @Idf_Fonte_Tecnico_Graduacao, Raz_Social = @Raz_Social, Idf_Area_BNE = @Idf_Area_BNE, Idf_Categoria_Habilitacao = @Idf_Categoria_Habilitacao, Num_DDD_Telefone = @Num_DDD_Telefone, Num_Telefone = @Num_Telefone, Eml_Pessoa = @Eml_Pessoa, Idf_Deficiencia = @Idf_Deficiencia, Dta_Cadastro = @Dta_Cadastro, Idf_Tipo_Veiculo = @Idf_Tipo_Veiculo, Idf_Curso_Outros_Cursos = @Idf_Curso_Outros_Cursos, Idf_Fonte_Outros_Cursos = @Idf_Fonte_Outros_Cursos, Idf_Raca = @Idf_Raca, Flg_Filhos = @Flg_Filhos, Flg_Pesquisa_Avancada = @Flg_Pesquisa_Avancada, Des_Curso_Tecnico_Graduacao = @Des_Curso_Tecnico_Graduacao, Des_Curso_Outros_Cursos = @Des_Curso_Outros_Cursos, Qtd_Curriculo_Retorno = @Qtd_Curriculo_Retorno, Idf_Escolaridade_WebStagio = @Idf_Escolaridade_WebStagio, Flg_Aprendiz = @Flg_Aprendiz, Des_Filtro_Excludente = @Des_Filtro_Excludente, Des_Zona = @Des_Zona, Geo_Busca_Bairro = @Geo_Busca_Bairro, Geo_Busca_Cidade = @Geo_Busca_Cidade, Idf_Funcao_Exercida = @Idf_Funcao_Exercida, Des_Funcao_Exercida = @Des_Funcao_Exercida, Idf_Origem = @Idf_Origem, Des_Avaliacao = @Des_Avaliacao, Num_Idade_Min = @Num_Idade_Min, Num_Idade_Max = @Num_Idade_Max WHERE Idf_Pesquisa_Curriculo = @Idf_Pesquisa_Curriculo";
		private const string SPDELETE = "DELETE FROM TAB_Pesquisa_Curriculo WHERE Idf_Pesquisa_Curriculo = @Idf_Pesquisa_Curriculo";
		private const string SPSELECTID = "SELECT * FROM TAB_Pesquisa_Curriculo WITH(NOLOCK) WHERE Idf_Pesquisa_Curriculo = @Idf_Pesquisa_Curriculo";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_IP", SqlDbType.Char, 15));
			parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Palavra_Chave", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Sig_Estado", SqlDbType.Char, 2));
			parms.Add(new SqlParameter("@Idf_Escolaridade", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Sexo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Num_Salario_Min", SqlDbType.Decimal, 9));
			parms.Add(new SqlParameter("@Num_Salario_Max", SqlDbType.Decimal, 9));
			parms.Add(new SqlParameter("@Qtd_Experiencia", SqlDbType.SmallInt, 8));
			parms.Add(new SqlParameter("@Des_Cod_CPF_Nome", SqlDbType.VarChar, 200));
			parms.Add(new SqlParameter("@Idf_Estado_Civil", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Bairro", SqlDbType.VarChar));
			parms.Add(new SqlParameter("@Des_Logradouro", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Num_CEP_Min", SqlDbType.Char, 8));
			parms.Add(new SqlParameter("@Num_CEP_Max", SqlDbType.Char, 8));
			parms.Add(new SqlParameter("@Des_Experiencia", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Idf_Curso_Tecnico_Graduacao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Fonte_Tecnico_Graduacao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Raz_Social", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Idf_Area_BNE", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Categoria_Habilitacao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Num_DDD_Telefone", SqlDbType.Char, 2));
			parms.Add(new SqlParameter("@Num_Telefone", SqlDbType.Char, 10));
			parms.Add(new SqlParameter("@Eml_Pessoa", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Idf_Deficiencia", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Idf_Tipo_Veiculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curso_Outros_Cursos", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Fonte_Outros_Cursos", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Raca", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Flg_Filhos", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Flg_Pesquisa_Avancada", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Des_Curso_Tecnico_Graduacao", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Des_Curso_Outros_Cursos", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Qtd_Curriculo_Retorno", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Escolaridade_WebStagio", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Flg_Aprendiz", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Des_Filtro_Excludente", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Des_Zona", SqlDbType.VarChar, 20));
            parms.Add(new SqlParameter { ParameterName = "@Geo_Busca_Bairro", UdtTypeName = "Geography" });
            parms.Add(new SqlParameter { ParameterName = "@Geo_Busca_Cidade", UdtTypeName = "Geography" });
            parms.Add(new SqlParameter("@Idf_Funcao_Exercida", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Funcao_Exercida", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Idf_Origem", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Avaliacao", SqlDbType.VarChar, 200));
			parms.Add(new SqlParameter("@Num_Idade_Min", SqlDbType.Int, 1));
			parms.Add(new SqlParameter("@Num_Idade_Max", SqlDbType.Int, 1));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idPesquisaCurriculo;

			if (this._usuarioFilialPerfil != null)
				parms[1].Value = this._usuarioFilialPerfil.IdUsuarioFilialPerfil;
			else
				parms[1].Value = DBNull.Value;


			if (this._curriculo != null)
				parms[2].Value = this._curriculo.IdCurriculo;
			else
				parms[2].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoIP))
				parms[3].Value = this._descricaoIP;
			else
				parms[3].Value = DBNull.Value;


			if (this._cidade != null)
				parms[4].Value = this._cidade.IdCidade;
			else
				parms[4].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoPalavraChave))
				parms[5].Value = this._descricaoPalavraChave;
			else
				parms[5].Value = DBNull.Value;


			if (this._estado != null)
				parms[6].Value = this._estado.SiglaEstado;
			else
				parms[6].Value = DBNull.Value;


			if (this._escolaridade != null)
				parms[7].Value = this._escolaridade.IdEscolaridade;
			else
				parms[7].Value = DBNull.Value;


			if (this._sexo != null)
				parms[8].Value = this._sexo.IdSexo;
			else
				parms[8].Value = DBNull.Value;


			if (this._numeroSalarioMin.HasValue)
				parms[9].Value = this._numeroSalarioMin;
			else
				parms[9].Value = DBNull.Value;


			if (this._numeroSalarioMax.HasValue)
				parms[10].Value = this._numeroSalarioMax;
			else
				parms[10].Value = DBNull.Value;


			if (this._quantidadeExperiencia.HasValue)
				parms[11].Value = this._quantidadeExperiencia;
			else
				parms[11].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoCodCPFNome))
				parms[12].Value = this._descricaoCodCPFNome;
			else
				parms[12].Value = DBNull.Value;


			if (this._estadoCivil != null)
				parms[13].Value = this._estadoCivil.IdEstadoCivil;
			else
				parms[13].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoBairro))
				parms[14].Value = this._descricaoBairro;
			else
				parms[14].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoLogradouro))
				parms[15].Value = this._descricaoLogradouro;
			else
				parms[15].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroCEPMin))
				parms[16].Value = this._numeroCEPMin;
			else
				parms[16].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroCEPMax))
				parms[17].Value = this._numeroCEPMax;
			else
				parms[17].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoExperiencia))
				parms[18].Value = this._descricaoExperiencia;
			else
				parms[18].Value = DBNull.Value;


			if (this._cursoTecnicoGraduacao != null)
				parms[19].Value = this._cursoTecnicoGraduacao.IdCurso;
			else
				parms[19].Value = DBNull.Value;


			if (this._fonteTecnicoGraduacao != null)
				parms[20].Value = this._fonteTecnicoGraduacao.IdFonte;
			else
				parms[20].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._razaoSocial))
				parms[21].Value = this._razaoSocial;
			else
				parms[21].Value = DBNull.Value;


			if (this._areaBNE != null)
				parms[22].Value = this._areaBNE.IdAreaBNE;
			else
				parms[22].Value = DBNull.Value;


			if (this._categoriaHabilitacao != null)
				parms[23].Value = this._categoriaHabilitacao.IdCategoriaHabilitacao;
			else
				parms[23].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroDDDTelefone))
				parms[24].Value = this._numeroDDDTelefone;
			else
				parms[24].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroTelefone))
				parms[25].Value = this._numeroTelefone;
			else
				parms[25].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._emailPessoa))
				parms[26].Value = this._emailPessoa;
			else
				parms[26].Value = DBNull.Value;


			if (this._deficiencia != null)
				parms[27].Value = this._deficiencia.IdDeficiencia;
			else
				parms[27].Value = DBNull.Value;


			if (this._tipoVeiculo != null)
				parms[29].Value = this._tipoVeiculo.IdTipoVeiculo;
			else
				parms[29].Value = DBNull.Value;


			if (this._cursoOutrosCursos != null)
				parms[30].Value = this._cursoOutrosCursos.IdCurso;
			else
				parms[30].Value = DBNull.Value;


			if (this._fonteOutrosCursos != null)
				parms[31].Value = this._fonteOutrosCursos.IdFonte;
			else
				parms[31].Value = DBNull.Value;


			if (this._raca != null)
				parms[32].Value = this._raca.IdRaca;
			else
				parms[32].Value = DBNull.Value;


			if (this._flagFilhos.HasValue)
				parms[33].Value = this._flagFilhos;
			else
				parms[33].Value = DBNull.Value;

			parms[34].Value = this._flagPesquisaAvancada;

			if (!String.IsNullOrEmpty(this._descricaoCursoTecnicoGraduacao))
				parms[35].Value = this._descricaoCursoTecnicoGraduacao;
			else
				parms[35].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoCursoOutrosCursos))
				parms[36].Value = this._descricaoCursoOutrosCursos;
			else
				parms[36].Value = DBNull.Value;


			if (this._quantidadeCurriculoRetorno.HasValue)
				parms[37].Value = this._quantidadeCurriculoRetorno;
			else
				parms[37].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._idEscolaridadeWebStagio))
				parms[38].Value = this._idEscolaridadeWebStagio;
			else
				parms[38].Value = DBNull.Value;


			if (this._flagAprendiz.HasValue)
				parms[39].Value = this._flagAprendiz;
			else
				parms[39].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoFiltroExcludente))
				parms[40].Value = this._descricaoFiltroExcludente;
			else
				parms[40].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoZona))
				parms[41].Value = this._descricaoZona;
			else
				parms[41].Value = DBNull.Value;


			if (this._GeoBuscaBairro != null)
				parms[42].Value = this._GeoBuscaBairro;
			else
                parms[42].Value = SqlGeography.Null;


            if (this._GeoBuscaCidade != null)
				parms[43].Value = this._GeoBuscaCidade;
			else
                parms[43].Value = SqlGeography.Null;


			if (this._funcaoExercida != null)
				parms[44].Value = this._funcaoExercida.IdFuncao;
			else
				parms[44].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoFuncaoExercida))
				parms[45].Value = this._descricaoFuncaoExercida;
			else
				parms[45].Value = DBNull.Value;


			if (this._origem != null)
				parms[46].Value = this._origem.IdOrigem;
			else
				parms[46].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoAvaliacao))
				parms[47].Value = this._descricaoAvaliacao;
			else
				parms[47].Value = DBNull.Value;


			if (this._numeroIdadeMin.HasValue)
				parms[48].Value = this._numeroIdadeMin;
			else
				parms[48].Value = DBNull.Value;


			if (this._numeroIdadeMax.HasValue)
				parms[49].Value = this._numeroIdadeMax;
			else
				parms[49].Value = DBNull.Value;


			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[28].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de PesquisaCurriculo no banco de dados.
		/// </summary>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert()
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);

			using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
			{
				conn.Open();
				using (SqlTransaction trans = conn.BeginTransaction())
				{
					try
					{
						SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
						this._idPesquisaCurriculo = Convert.ToInt32(cmd.Parameters["@Idf_Pesquisa_Curriculo"].Value);
						cmd.Parameters.Clear();
						this._persisted = true;
						this._modified = false;
						trans.Commit();
					}
					catch
					{
						trans.Rollback();
						throw;
					}
				}
			}
		}
		/// <summary>
		/// Método utilizado para inserir uma instância de PesquisaCurriculo no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idPesquisaCurriculo = Convert.ToInt32(cmd.Parameters["@Idf_Pesquisa_Curriculo"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de PesquisaCurriculo no banco de dados.
		/// </summary>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Update()
		{
			if (this._modified)
			{
				List<SqlParameter> parms = GetParameters();
				SetParameters(parms);
				DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPUPDATE, parms);
				this._modified = false;
			}
		}
		/// <summary>
		/// Método utilizado para atualizar uma instância de PesquisaCurriculo no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Update(SqlTransaction trans)
		{
			if (this._modified)
			{
				List<SqlParameter> parms = GetParameters();
				SetParameters(parms);
				DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPUPDATE, parms);
				this._modified = false;
			}
		}
		#endregion

		#region Save
		/// <summary>
		/// Método utilizado para salvar uma instância de PesquisaCurriculo no banco de dados.
		/// </summary>
		/// <remarks>Gieyson Stelmak</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de PesquisaCurriculo no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public void Save(SqlTransaction trans)
		{
			if (!this._persisted)
				this.Insert(trans);
			else
				this.Update(trans);
		}
		#endregion

		#region Delete
		/// <summary>
		/// Método utilizado para excluir uma instância de PesquisaCurriculo no banco de dados.
		/// </summary>
		/// <param name="idPesquisaCurriculo">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idPesquisaCurriculo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaCurriculo;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de PesquisaCurriculo no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPesquisaCurriculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idPesquisaCurriculo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaCurriculo;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de PesquisaCurriculo no banco de dados.
		/// </summary>
		/// <param name="idPesquisaCurriculo">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idPesquisaCurriculo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from TAB_Pesquisa_Curriculo where Idf_Pesquisa_Curriculo in (";

			for (int i = 0; i < idPesquisaCurriculo.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idPesquisaCurriculo[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idPesquisaCurriculo">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idPesquisaCurriculo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaCurriculo;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPesquisaCurriculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idPesquisaCurriculo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaCurriculo;

			return DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar uma consulta paginada do banco de dados.
		/// </summary>
		/// <param name="colunaOrdenacao">Nome da coluna pela qual será ordenada.</param>
		/// <param name="direcaoOrdenacao">Direção da ordenação (ASC ou DESC).</param>
		/// <param name="paginaCorrente">Número da página que será exibida.</param>
		/// <param name="tamanhoPagina">Quantidade de itens em cada página.</param>
		/// <param name="totalRegistros">Quantidade total de registros na tabela.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		public static IDataReader LoadDataReader(string colunaOrdenacao, string direcaoOrdenacao, int paginaCorrente, int tamanhoPagina, out int totalRegistros)
		{
			int inicio = ((paginaCorrente - 1) * tamanhoPagina) + 1;
			int fim = paginaCorrente * tamanhoPagina;

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Pes.Idf_Pesquisa_Curriculo, Pes.Idf_Usuario_Filial_Perfil, Pes.Idf_Curriculo, Pes.Des_IP, Pes.Idf_Cidade, Pes.Des_Palavra_Chave, Pes.Sig_Estado, Pes.Idf_Escolaridade, Pes.Idf_Sexo, Pes.Num_Salario_Min, Pes.Num_Salario_Max, Pes.Qtd_Experiencia, Pes.Des_Cod_CPF_Nome, Pes.Idf_Estado_Civil, Pes.Des_Bairro, Pes.Des_Logradouro, Pes.Num_CEP_Min, Pes.Num_CEP_Max, Pes.Des_Experiencia, Pes.Idf_Curso_Tecnico_Graduacao, Pes.Idf_Fonte_Tecnico_Graduacao, Pes.Raz_Social, Pes.Idf_Area_BNE, Pes.Idf_Categoria_Habilitacao, Pes.Num_DDD_Telefone, Pes.Num_Telefone, Pes.Eml_Pessoa, Pes.Idf_Deficiencia, Pes.Dta_Cadastro, Pes.Idf_Tipo_Veiculo, Pes.Idf_Curso_Outros_Cursos, Pes.Idf_Fonte_Outros_Cursos, Pes.Idf_Raca, Pes.Flg_Filhos, Pes.Flg_Pesquisa_Avancada, Pes.Des_Curso_Tecnico_Graduacao, Pes.Des_Curso_Outros_Cursos, Pes.Qtd_Curriculo_Retorno, Pes.Idf_Escolaridade_WebStagio, Pes.Flg_Aprendiz, Pes.Des_Filtro_Excludente, Pes.Des_Zona, Pes.Geo_Busca_Bairro, Pes.Geo_Busca_Cidade, Pes.Idf_Funcao_Exercida, Pes.Des_Funcao_Exercida, Pes.Idf_Origem, Pes.Des_Avaliacao, Pes.Num_Idade_Min, Pes.Num_Idade_Max FROM TAB_Pesquisa_Curriculo Pes";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de PesquisaCurriculo a partir do banco de dados.
		/// </summary>
		/// <param name="idPesquisaCurriculo">Chave do registro.</param>
		/// <returns>Instância de PesquisaCurriculo.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static PesquisaCurriculo LoadObject(int idPesquisaCurriculo)
		{
			using (IDataReader dr = LoadDataReader(idPesquisaCurriculo))
			{
				PesquisaCurriculo objPesquisaCurriculo = new PesquisaCurriculo();
				if (SetInstance(dr, objPesquisaCurriculo))
					return objPesquisaCurriculo;
			}
			throw (new RecordNotFoundException(typeof(PesquisaCurriculo)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de PesquisaCurriculo a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPesquisaCurriculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de PesquisaCurriculo.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static PesquisaCurriculo LoadObject(int idPesquisaCurriculo, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idPesquisaCurriculo, trans))
			{
				PesquisaCurriculo objPesquisaCurriculo = new PesquisaCurriculo();
				if (SetInstance(dr, objPesquisaCurriculo))
					return objPesquisaCurriculo;
			}
			throw (new RecordNotFoundException(typeof(PesquisaCurriculo)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de PesquisaCurriculo a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idPesquisaCurriculo))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de PesquisaCurriculo a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idPesquisaCurriculo, trans))
			{
				return SetInstance(dr, this);
			}
		}
		#endregion

		#region SetInstance
		/// <summary>
		/// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
		/// </summary>
		/// <param name="dr">Cursor de leitura do banco de dados.</param>
		/// <param name="objPesquisaCurriculo">Instância a ser manipulada.</param>
		/// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, PesquisaCurriculo objPesquisaCurriculo, bool dispose = true)
		{
			try
			{
				if (dr.Read())
				{
					objPesquisaCurriculo._idPesquisaCurriculo = Convert.ToInt32(dr["Idf_Pesquisa_Curriculo"]);
					if (dr["Idf_Usuario_Filial_Perfil"] != DBNull.Value)
						objPesquisaCurriculo._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));
					if (dr["Idf_Curriculo"] != DBNull.Value)
						objPesquisaCurriculo._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
					if (dr["Des_IP"] != DBNull.Value)
						objPesquisaCurriculo._descricaoIP = Convert.ToString(dr["Des_IP"]);
					if (dr["Idf_Cidade"] != DBNull.Value)
						objPesquisaCurriculo._cidade = new Cidade(Convert.ToInt32(dr["Idf_Cidade"]));
					if (dr["Des_Palavra_Chave"] != DBNull.Value)
						objPesquisaCurriculo._descricaoPalavraChave = Convert.ToString(dr["Des_Palavra_Chave"]);
					if (dr["Sig_Estado"] != DBNull.Value)
                        objPesquisaCurriculo._estado = new Estado(Convert.ToString(dr["Sig_Estado"]));
					if (dr["Idf_Escolaridade"] != DBNull.Value)
						objPesquisaCurriculo._escolaridade = new Escolaridade(Convert.ToInt32(dr["Idf_Escolaridade"]));
					if (dr["Idf_Sexo"] != DBNull.Value)
						objPesquisaCurriculo._sexo = new Sexo(Convert.ToInt32(dr["Idf_Sexo"]));
					if (dr["Num_Salario_Min"] != DBNull.Value)
						objPesquisaCurriculo._numeroSalarioMin = Convert.ToDecimal(dr["Num_Salario_Min"]);
					if (dr["Num_Salario_Max"] != DBNull.Value)
						objPesquisaCurriculo._numeroSalarioMax = Convert.ToDecimal(dr["Num_Salario_Max"]);
					if (dr["Qtd_Experiencia"] != DBNull.Value)
						objPesquisaCurriculo._quantidadeExperiencia = Convert.ToInt16(dr["Qtd_Experiencia"]);
					if (dr["Des_Cod_CPF_Nome"] != DBNull.Value)
						objPesquisaCurriculo._descricaoCodCPFNome = Convert.ToString(dr["Des_Cod_CPF_Nome"]);
					if (dr["Idf_Estado_Civil"] != DBNull.Value)
						objPesquisaCurriculo._estadoCivil = new EstadoCivil(Convert.ToInt32(dr["Idf_Estado_Civil"]));
					if (dr["Des_Bairro"] != DBNull.Value)
						objPesquisaCurriculo._descricaoBairro = Convert.ToString(dr["Des_Bairro"]);
					if (dr["Des_Logradouro"] != DBNull.Value)
						objPesquisaCurriculo._descricaoLogradouro = Convert.ToString(dr["Des_Logradouro"]);
					if (dr["Num_CEP_Min"] != DBNull.Value)
						objPesquisaCurriculo._numeroCEPMin = Convert.ToString(dr["Num_CEP_Min"]);
					if (dr["Num_CEP_Max"] != DBNull.Value)
						objPesquisaCurriculo._numeroCEPMax = Convert.ToString(dr["Num_CEP_Max"]);
					if (dr["Des_Experiencia"] != DBNull.Value)
						objPesquisaCurriculo._descricaoExperiencia = Convert.ToString(dr["Des_Experiencia"]);
					if (dr["Idf_Curso_Tecnico_Graduacao"] != DBNull.Value)
						objPesquisaCurriculo._cursoTecnicoGraduacao = new Curso(Convert.ToInt32(dr["Idf_Curso_Tecnico_Graduacao"]));
					if (dr["Idf_Fonte_Tecnico_Graduacao"] != DBNull.Value)
						objPesquisaCurriculo._fonteTecnicoGraduacao = new Fonte(Convert.ToInt32(dr["Idf_Fonte_Tecnico_Graduacao"]));
					if (dr["Raz_Social"] != DBNull.Value)
						objPesquisaCurriculo._razaoSocial = Convert.ToString(dr["Raz_Social"]);
					if (dr["Idf_Area_BNE"] != DBNull.Value)
						objPesquisaCurriculo._areaBNE = new AreaBNE(Convert.ToInt32(dr["Idf_Area_BNE"]));
					if (dr["Idf_Categoria_Habilitacao"] != DBNull.Value)
						objPesquisaCurriculo._categoriaHabilitacao = new CategoriaHabilitacao(Convert.ToInt32(dr["Idf_Categoria_Habilitacao"]));
					if (dr["Num_DDD_Telefone"] != DBNull.Value)
						objPesquisaCurriculo._numeroDDDTelefone = Convert.ToString(dr["Num_DDD_Telefone"]);
					if (dr["Num_Telefone"] != DBNull.Value)
						objPesquisaCurriculo._numeroTelefone = Convert.ToString(dr["Num_Telefone"]);
					if (dr["Eml_Pessoa"] != DBNull.Value)
						objPesquisaCurriculo._emailPessoa = Convert.ToString(dr["Eml_Pessoa"]);
					if (dr["Idf_Deficiencia"] != DBNull.Value)
						objPesquisaCurriculo._deficiencia = new Deficiencia(Convert.ToInt32(dr["Idf_Deficiencia"]));
					if (dr["Dta_Cadastro"] != DBNull.Value)
						objPesquisaCurriculo._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					if (dr["Idf_Tipo_Veiculo"] != DBNull.Value)
						objPesquisaCurriculo._tipoVeiculo = new TipoVeiculo(Convert.ToInt32(dr["Idf_Tipo_Veiculo"]));
					if (dr["Idf_Curso_Outros_Cursos"] != DBNull.Value)
						objPesquisaCurriculo._cursoOutrosCursos = new Curso(Convert.ToInt32(dr["Idf_Curso_Outros_Cursos"]));
					if (dr["Idf_Fonte_Outros_Cursos"] != DBNull.Value)
						objPesquisaCurriculo._fonteOutrosCursos = new Fonte(Convert.ToInt32(dr["Idf_Fonte_Outros_Cursos"]));
					if (dr["Idf_Raca"] != DBNull.Value)
						objPesquisaCurriculo._raca = new Raca(Convert.ToInt32(dr["Idf_Raca"]));
					if (dr["Flg_Filhos"] != DBNull.Value)
						objPesquisaCurriculo._flagFilhos = Convert.ToBoolean(dr["Flg_Filhos"]);
					objPesquisaCurriculo._flagPesquisaAvancada = Convert.ToBoolean(dr["Flg_Pesquisa_Avancada"]);
					if (dr["Des_Curso_Tecnico_Graduacao"] != DBNull.Value)
						objPesquisaCurriculo._descricaoCursoTecnicoGraduacao = Convert.ToString(dr["Des_Curso_Tecnico_Graduacao"]);
					if (dr["Des_Curso_Outros_Cursos"] != DBNull.Value)
						objPesquisaCurriculo._descricaoCursoOutrosCursos = Convert.ToString(dr["Des_Curso_Outros_Cursos"]);
					if (dr["Qtd_Curriculo_Retorno"] != DBNull.Value)
						objPesquisaCurriculo._quantidadeCurriculoRetorno = Convert.ToInt32(dr["Qtd_Curriculo_Retorno"]);
					if (dr["Idf_Escolaridade_WebStagio"] != DBNull.Value)
						objPesquisaCurriculo._idEscolaridadeWebStagio = Convert.ToString(dr["Idf_Escolaridade_WebStagio"]);
					if (dr["Flg_Aprendiz"] != DBNull.Value)
						objPesquisaCurriculo._flagAprendiz = Convert.ToBoolean(dr["Flg_Aprendiz"]);
					if (dr["Des_Filtro_Excludente"] != DBNull.Value)
						objPesquisaCurriculo._descricaoFiltroExcludente = Convert.ToString(dr["Des_Filtro_Excludente"]);
					if (dr["Des_Zona"] != DBNull.Value)
						objPesquisaCurriculo._descricaoZona = Convert.ToString(dr["Des_Zona"]);
					if (dr["Geo_Busca_Bairro"] != DBNull.Value)
						objPesquisaCurriculo._GeoBuscaBairro = (SqlGeography)(dr["Geo_Busca_Bairro"]);
					if (dr["Geo_Busca_Cidade"] != DBNull.Value)
						objPesquisaCurriculo._GeoBuscaCidade = (SqlGeography)(dr["Geo_Busca_Cidade"]);
					if (dr["Idf_Funcao_Exercida"] != DBNull.Value)
						objPesquisaCurriculo._funcaoExercida = new Funcao(Convert.ToInt32(dr["Idf_Funcao_Exercida"]));
					if (dr["Des_Funcao_Exercida"] != DBNull.Value)
						objPesquisaCurriculo._descricaoFuncaoExercida = Convert.ToString(dr["Des_Funcao_Exercida"]);
					if (dr["Idf_Origem"] != DBNull.Value)
						objPesquisaCurriculo._origem = new Origem(Convert.ToInt32(dr["Idf_Origem"]));
					if (dr["Des_Avaliacao"] != DBNull.Value)
						objPesquisaCurriculo._descricaoAvaliacao = Convert.ToString(dr["Des_Avaliacao"]);
					if (dr["Num_Idade_Min"] != DBNull.Value)
						objPesquisaCurriculo._numeroIdadeMin = Convert.ToInt16(dr["Num_Idade_Min"]);
					if (dr["Num_Idade_Max"] != DBNull.Value)
						objPesquisaCurriculo._numeroIdadeMax = Convert.ToInt16(dr["Num_Idade_Max"]);

					objPesquisaCurriculo._persisted = true;
					objPesquisaCurriculo._modified = false;

					return true;
				}
				else
				{
					return false;
				}
			}
			catch 
			{
				throw;
			}
			finally
			{
				if (dispose)
					dr.Dispose();
			}
		}
		#endregion

		#endregion
	}
}