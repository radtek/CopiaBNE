//-- Data: 28/01/2016 17:26
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;
using Microsoft.SqlServer.Types;

namespace BNE.BLL
{
	public partial class RastreadorCurriculo // Tabela: BNE_Rastreador_Curriculo
	{
		#region Atributos
		private int _idRastreadorCurriculo;
		private Filial _filial;
		private Cidade _cidade;
		private string _descricaoPalavraChave;
		private decimal? _valorSalarioInicio;
		private decimal? _valorSalarioFim;
		private Escolaridade _escolaridade;
		private Sexo _sexo;
		private string _descricaoBairro;
		private CategoriaHabilitacao _categoriaHabilitacao;
		private AreaBNE _areaBNE;
		private Fonte _fonte;
		private Deficiencia _deficiencia;
		private bool _flagInativo;
		private DateTime? _dataCadastro;
		private Curso _curso;
		private CursoFonte _cursoFonte;
		private Raca _raca;
		private EstadoCivil _estadoCivil;
		private TipoVeiculo _tipoVeiculo;
		private bool? _flagFilhos;
		private Curso _cursoOutrosCursos;
		private Fonte _fonteOutrosCursos;
		private Origem _origem;
		private UsuarioFilialPerfil _usuarioFilialPerfil;
		private string _descricaoIP;
		private Estado _estado;
		private decimal? _numeroSalarioMin;
		private decimal? _numeroSalarioMax;
		private string _numeroCEPMin;
		private string _numeroCEPMax;
		private string _descricaoExperiencia;
		private Curso _cursoTecnicoGraduacao;
		private Fonte _fonteTecnicoGraduacao;
		private string _descricaoCursoTecnicoGraduacao;
		private string _descricaoCursoOutrosCursos;
		private string _idEscolaridadeWebStagio;
		private bool? _flagAprendiz;
		private string _descricaoFiltroExcludente;
		private string _descricaoZona;
		private SqlGeography _GeoBuscaBairro;
		private SqlGeography _GeoBuscaCidade;
		private Funcao _funcaoExercida;
		private string _descricaoFuncaoExercida;
		private string _descricaoLogradouro;
		private bool? _flagNotificaDia;
		private bool? _flagNotificaHora;
		private DateTime? _dataVisualizacao;
		private Int16? _numeroIdadeMax;
		private Int16? _numeroIdadeMin;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdRastreadorCurriculo
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdRastreadorCurriculo
		{
			get
			{
				return this._idRastreadorCurriculo;
			}
		}
		#endregion 

		#region Filial
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Filial Filial
		{
			get
			{
				return this._filial;
			}
			set
			{
				this._filial = value;
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

		#region ValorSalarioInicio
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public decimal? ValorSalarioInicio
		{
			get
			{
				return this._valorSalarioInicio;
			}
			set
			{
				this._valorSalarioInicio = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorSalarioFim
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public decimal? ValorSalarioFim
		{
			get
			{
				return this._valorSalarioFim;
			}
			set
			{
				this._valorSalarioFim = value;
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

		#region Fonte
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Fonte Fonte
		{
			get
			{
				return this._fonte;
			}
			set
			{
				this._fonte = value;
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

		#region FlagInativo
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagInativo
		{
			get
			{
				return this._flagInativo;
			}
			set
			{
				this._flagInativo = value;
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

		#region Curso
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Curso Curso
		{
			get
			{
				return this._curso;
			}
			set
			{
				this._curso = value;
				this._modified = true;
			}
		}
		#endregion 

		#region CursoFonte
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public CursoFonte CursoFonte
		{
			get
			{
				return this._cursoFonte;
			}
			set
			{
				this._cursoFonte = value;
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

		#region UsuarioFilialPerfil
		/// <summary>
		/// Campo obrigatório.
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

		#region FlagNotificaDia
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public bool? FlagNotificaDia
		{
			get
			{
				return this._flagNotificaDia;
			}
			set
			{
				this._flagNotificaDia = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagNotificaHora
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public bool? FlagNotificaHora
		{
			get
			{
				return this._flagNotificaHora;
			}
			set
			{
				this._flagNotificaHora = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DataVisualizacao
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DataVisualizacao
		{
			get
			{
				return this._dataVisualizacao;
			}
			set
			{
				this._dataVisualizacao = value;
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

		#endregion

		#region Construtores
		public RastreadorCurriculo()
		{
		}
		public RastreadorCurriculo(int idRastreadorCurriculo)
		{
			this._idRastreadorCurriculo = idRastreadorCurriculo;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Rastreador_Curriculo (Idf_Filial, Idf_Cidade, Des_Palavra_Chave, Vlr_Salario_Inicio, Vlr_Salario_Fim, Idf_Escolaridade, Idf_Sexo, Des_Bairro, Idf_Categoria_Habilitacao, Idf_Area_BNE, Idf_Fonte, Idf_Deficiencia, Flg_Inativo, Dta_Cadastro, Idf_Curso, Idf_Curso_Fonte, Idf_Raca, Idf_Estado_Civil, Idf_Tipo_Veiculo, Flg_Filhos, Idf_Curso_Outros_Cursos, Idf_Fonte_Outros_Cursos, Idf_Origem, Idf_Usuario_Filial_Perfil, Des_IP, Sig_Estado, Num_Salario_Min, Num_Salario_Max, Num_CEP_Min, Num_CEP_Max, Des_Experiencia, Idf_Curso_Tecnico_Graduacao, Idf_Fonte_Tecnico_Graduacao, Des_Curso_Tecnico_Graduacao, Des_Curso_Outros_Cursos, Idf_Escolaridade_WebStagio, Flg_Aprendiz, Des_Filtro_Excludente, Des_Zona, Geo_Busca_Bairro, Geo_Busca_Cidade, Idf_Funcao_Exercida, Des_Funcao_Exercida, Des_Logradouro, Flg_Notifica_Dia, Flg_Notifica_Hora, Dta_Visualizacao, Num_Idade_Max, Num_Idade_Min) VALUES (@Idf_Filial, @Idf_Cidade, @Des_Palavra_Chave, @Vlr_Salario_Inicio, @Vlr_Salario_Fim, @Idf_Escolaridade, @Idf_Sexo, @Des_Bairro, @Idf_Categoria_Habilitacao, @Idf_Area_BNE, @Idf_Fonte, @Idf_Deficiencia, @Flg_Inativo, @Dta_Cadastro, @Idf_Curso, @Idf_Curso_Fonte, @Idf_Raca, @Idf_Estado_Civil, @Idf_Tipo_Veiculo, @Flg_Filhos, @Idf_Curso_Outros_Cursos, @Idf_Fonte_Outros_Cursos, @Idf_Origem, @Idf_Usuario_Filial_Perfil, @Des_IP, @Sig_Estado, @Num_Salario_Min, @Num_Salario_Max, @Num_CEP_Min, @Num_CEP_Max, @Des_Experiencia, @Idf_Curso_Tecnico_Graduacao, @Idf_Fonte_Tecnico_Graduacao, @Des_Curso_Tecnico_Graduacao, @Des_Curso_Outros_Cursos, @Idf_Escolaridade_WebStagio, @Flg_Aprendiz, @Des_Filtro_Excludente, @Des_Zona, @Geo_Busca_Bairro, @Geo_Busca_Cidade, @Idf_Funcao_Exercida, @Des_Funcao_Exercida, @Des_Logradouro, @Flg_Notifica_Dia, @Flg_Notifica_Hora, @Dta_Visualizacao, @Num_Idade_Max, @Num_Idade_Min);SET @Idf_Rastreador_Curriculo = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Rastreador_Curriculo SET Idf_Filial = @Idf_Filial, Idf_Cidade = @Idf_Cidade, Des_Palavra_Chave = @Des_Palavra_Chave, Vlr_Salario_Inicio = @Vlr_Salario_Inicio, Vlr_Salario_Fim = @Vlr_Salario_Fim, Idf_Escolaridade = @Idf_Escolaridade, Idf_Sexo = @Idf_Sexo, Des_Bairro = @Des_Bairro, Idf_Categoria_Habilitacao = @Idf_Categoria_Habilitacao, Idf_Area_BNE = @Idf_Area_BNE, Idf_Fonte = @Idf_Fonte, Idf_Deficiencia = @Idf_Deficiencia, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro, Idf_Curso = @Idf_Curso, Idf_Curso_Fonte = @Idf_Curso_Fonte, Idf_Raca = @Idf_Raca, Idf_Estado_Civil = @Idf_Estado_Civil, Idf_Tipo_Veiculo = @Idf_Tipo_Veiculo, Flg_Filhos = @Flg_Filhos, Idf_Curso_Outros_Cursos = @Idf_Curso_Outros_Cursos, Idf_Fonte_Outros_Cursos = @Idf_Fonte_Outros_Cursos, Idf_Origem = @Idf_Origem, Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil, Des_IP = @Des_IP, Sig_Estado = @Sig_Estado, Num_Salario_Min = @Num_Salario_Min, Num_Salario_Max = @Num_Salario_Max, Num_CEP_Min = @Num_CEP_Min, Num_CEP_Max = @Num_CEP_Max, Des_Experiencia = @Des_Experiencia, Idf_Curso_Tecnico_Graduacao = @Idf_Curso_Tecnico_Graduacao, Idf_Fonte_Tecnico_Graduacao = @Idf_Fonte_Tecnico_Graduacao, Des_Curso_Tecnico_Graduacao = @Des_Curso_Tecnico_Graduacao, Des_Curso_Outros_Cursos = @Des_Curso_Outros_Cursos, Idf_Escolaridade_WebStagio = @Idf_Escolaridade_WebStagio, Flg_Aprendiz = @Flg_Aprendiz, Des_Filtro_Excludente = @Des_Filtro_Excludente, Des_Zona = @Des_Zona, Geo_Busca_Bairro = @Geo_Busca_Bairro, Geo_Busca_Cidade = @Geo_Busca_Cidade, Idf_Funcao_Exercida = @Idf_Funcao_Exercida, Des_Funcao_Exercida = @Des_Funcao_Exercida, Des_Logradouro = @Des_Logradouro, Flg_Notifica_Dia = @Flg_Notifica_Dia, Flg_Notifica_Hora = @Flg_Notifica_Hora, Dta_Visualizacao = @Dta_Visualizacao, Num_Idade_Max = @Num_Idade_Max, Num_Idade_Min = @Num_Idade_Min WHERE Idf_Rastreador_Curriculo = @Idf_Rastreador_Curriculo";
		private const string SPDELETE = "DELETE FROM BNE_Rastreador_Curriculo WHERE Idf_Rastreador_Curriculo = @Idf_Rastreador_Curriculo";
		private const string SPSELECTID = "SELECT * FROM BNE_Rastreador_Curriculo WITH(NOLOCK) WHERE Idf_Rastreador_Curriculo = @Idf_Rastreador_Curriculo";
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
			parms.Add(new SqlParameter("@Idf_Rastreador_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Palavra_Chave", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Vlr_Salario_Inicio", SqlDbType.Money, 8));
			parms.Add(new SqlParameter("@Vlr_Salario_Fim", SqlDbType.Money, 8));
			parms.Add(new SqlParameter("@Idf_Escolaridade", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Sexo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Bairro", SqlDbType.VarChar));
			parms.Add(new SqlParameter("@Idf_Categoria_Habilitacao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Area_BNE", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Fonte", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Deficiencia", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Idf_Curso", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curso_Fonte", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Raca", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Estado_Civil", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Tipo_Veiculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Flg_Filhos", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Idf_Curso_Outros_Cursos", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Fonte_Outros_Cursos", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Origem", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_IP", SqlDbType.Char, 15));
			parms.Add(new SqlParameter("@Sig_Estado", SqlDbType.Char, 2));
			parms.Add(new SqlParameter("@Num_Salario_Min", SqlDbType.Decimal, 9));
			parms.Add(new SqlParameter("@Num_Salario_Max", SqlDbType.Decimal, 9));
			parms.Add(new SqlParameter("@Num_CEP_Min", SqlDbType.Char, 8));
			parms.Add(new SqlParameter("@Num_CEP_Max", SqlDbType.Char, 8));
			parms.Add(new SqlParameter("@Des_Experiencia", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Idf_Curso_Tecnico_Graduacao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Fonte_Tecnico_Graduacao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Curso_Tecnico_Graduacao", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Des_Curso_Outros_Cursos", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Idf_Escolaridade_WebStagio", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Flg_Aprendiz", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Des_Filtro_Excludente", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Des_Zona", SqlDbType.VarChar, 20));
            parms.Add(new SqlParameter { ParameterName = "@Geo_Busca_Bairro", UdtTypeName = "Geography" });
            parms.Add(new SqlParameter { ParameterName = "@Geo_Busca_Cidade", UdtTypeName = "Geography" });
			parms.Add(new SqlParameter("@Idf_Funcao_Exercida", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Funcao_Exercida", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Des_Logradouro", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Flg_Notifica_Dia", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Flg_Notifica_Hora", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Dta_Visualizacao", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Num_Idade_Max", SqlDbType.Int, 1));
			parms.Add(new SqlParameter("@Num_Idade_Min", SqlDbType.Int, 1));
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
			parms[0].Value = this._idRastreadorCurriculo;
			parms[1].Value = this._filial.IdFilial;

			if (this._cidade != null)
				parms[2].Value = this._cidade.IdCidade;
			else
				parms[2].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoPalavraChave))
				parms[3].Value = this._descricaoPalavraChave;
			else
				parms[3].Value = DBNull.Value;


			if (this._valorSalarioInicio.HasValue)
				parms[4].Value = this._valorSalarioInicio;
			else
				parms[4].Value = DBNull.Value;


			if (this._valorSalarioFim.HasValue)
				parms[5].Value = this._valorSalarioFim;
			else
				parms[5].Value = DBNull.Value;


			if (this._escolaridade != null)
				parms[6].Value = this._escolaridade.IdEscolaridade;
			else
				parms[6].Value = DBNull.Value;


			if (this._sexo != null)
				parms[7].Value = this._sexo.IdSexo;
			else
				parms[7].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoBairro))
				parms[8].Value = this._descricaoBairro;
			else
				parms[8].Value = DBNull.Value;


			if (this._categoriaHabilitacao != null)
				parms[9].Value = this._categoriaHabilitacao.IdCategoriaHabilitacao;
			else
				parms[9].Value = DBNull.Value;


			if (this._areaBNE != null)
				parms[10].Value = this._areaBNE.IdAreaBNE;
			else
				parms[10].Value = DBNull.Value;


			if (this._fonte != null)
				parms[11].Value = this._fonte.IdFonte;
			else
				parms[11].Value = DBNull.Value;


			if (this._deficiencia != null)
				parms[12].Value = this._deficiencia.IdDeficiencia;
			else
				parms[12].Value = DBNull.Value;

			parms[13].Value = this._flagInativo;

			if (this._curso != null)
				parms[15].Value = this._curso.IdCurso;
			else
				parms[15].Value = DBNull.Value;


			if (this._cursoFonte != null)
				parms[16].Value = this._cursoFonte.IdCursoFonte;
			else
				parms[16].Value = DBNull.Value;


			if (this._raca != null)
				parms[17].Value = this._raca.IdRaca;
			else
				parms[17].Value = DBNull.Value;


			if (this._estadoCivil != null)
				parms[18].Value = this._estadoCivil.IdEstadoCivil;
			else
				parms[18].Value = DBNull.Value;


			if (this._tipoVeiculo != null)
				parms[19].Value = this._tipoVeiculo.IdTipoVeiculo;
			else
				parms[19].Value = DBNull.Value;


			if (this._flagFilhos.HasValue)
				parms[20].Value = this._flagFilhos;
			else
				parms[20].Value = DBNull.Value;


			if (this._cursoOutrosCursos != null)
				parms[21].Value = this._cursoOutrosCursos.IdCurso;
			else
				parms[21].Value = DBNull.Value;


			if (this._fonteOutrosCursos != null)
				parms[22].Value = this._fonteOutrosCursos.IdFonte;
			else
				parms[22].Value = DBNull.Value;


			if (this._origem != null)
				parms[23].Value = this._origem.IdOrigem;
			else
				parms[23].Value = DBNull.Value;

			parms[24].Value = this._usuarioFilialPerfil.IdUsuarioFilialPerfil;

			if (!String.IsNullOrEmpty(this._descricaoIP))
				parms[25].Value = this._descricaoIP;
			else
				parms[25].Value = DBNull.Value;


			if (this._estado != null)
				parms[26].Value = this._estado.SiglaEstado;
			else
				parms[26].Value = DBNull.Value;


			if (this._numeroSalarioMin.HasValue)
				parms[27].Value = this._numeroSalarioMin;
			else
				parms[27].Value = DBNull.Value;


			if (this._numeroSalarioMax.HasValue)
				parms[28].Value = this._numeroSalarioMax;
			else
				parms[28].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroCEPMin))
				parms[29].Value = this._numeroCEPMin;
			else
				parms[29].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroCEPMax))
				parms[30].Value = this._numeroCEPMax;
			else
				parms[30].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoExperiencia))
				parms[31].Value = this._descricaoExperiencia;
			else
				parms[31].Value = DBNull.Value;


			if (this._cursoTecnicoGraduacao != null)
				parms[32].Value = this._cursoTecnicoGraduacao.IdCurso;
			else
				parms[32].Value = DBNull.Value;


			if (this._fonteTecnicoGraduacao != null)
				parms[33].Value = this._fonteTecnicoGraduacao.IdFonte;
			else
				parms[33].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoCursoTecnicoGraduacao))
				parms[34].Value = this._descricaoCursoTecnicoGraduacao;
			else
				parms[34].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoCursoOutrosCursos))
				parms[35].Value = this._descricaoCursoOutrosCursos;
			else
				parms[35].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._idEscolaridadeWebStagio))
				parms[36].Value = this._idEscolaridadeWebStagio;
			else
				parms[36].Value = DBNull.Value;


			if (this._flagAprendiz.HasValue)
				parms[37].Value = this._flagAprendiz;
			else
				parms[37].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoFiltroExcludente))
				parms[38].Value = this._descricaoFiltroExcludente;
			else
				parms[38].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoZona))
				parms[39].Value = this._descricaoZona;
			else
				parms[39].Value = DBNull.Value;


			if (this._GeoBuscaBairro != null)
				parms[40].Value = this._GeoBuscaBairro;
			else
                parms[40].Value = SqlGeography.Null;


            if (this._GeoBuscaCidade != null)
				parms[41].Value = this._GeoBuscaCidade;
			else
				parms[41].Value = SqlGeography.Null;


			if (this._funcaoExercida != null)
				parms[42].Value = this._funcaoExercida.IdFuncao;
			else
				parms[42].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoFuncaoExercida))
				parms[43].Value = this._descricaoFuncaoExercida;
			else
				parms[43].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoLogradouro))
				parms[44].Value = this._descricaoLogradouro;
			else
				parms[44].Value = DBNull.Value;


			if (this._flagNotificaDia.HasValue)
				parms[45].Value = this._flagNotificaDia;
			else
				parms[45].Value = DBNull.Value;


			if (this._flagNotificaHora.HasValue)
				parms[46].Value = this._flagNotificaHora;
			else
				parms[46].Value = DBNull.Value;


			if (this._dataVisualizacao.HasValue)
				parms[47].Value = this._dataVisualizacao;
			else
				parms[47].Value = DBNull.Value;


			if (this._numeroIdadeMax.HasValue)
				parms[48].Value = this._numeroIdadeMax;
			else
				parms[48].Value = DBNull.Value;


			if (this._numeroIdadeMin.HasValue)
				parms[49].Value = this._numeroIdadeMin;
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
			parms[14].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de RastreadorCurriculo no banco de dados.
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
						this._idRastreadorCurriculo = Convert.ToInt32(cmd.Parameters["@Idf_Rastreador_Curriculo"].Value);
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
		/// Método utilizado para inserir uma instância de RastreadorCurriculo no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idRastreadorCurriculo = Convert.ToInt32(cmd.Parameters["@Idf_Rastreador_Curriculo"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de RastreadorCurriculo no banco de dados.
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
		/// Método utilizado para atualizar uma instância de RastreadorCurriculo no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de RastreadorCurriculo no banco de dados.
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
		/// Método utilizado para salvar uma instância de RastreadorCurriculo no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de RastreadorCurriculo no banco de dados.
		/// </summary>
		/// <param name="idRastreadorCurriculo">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idRastreadorCurriculo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Rastreador_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idRastreadorCurriculo;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de RastreadorCurriculo no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRastreadorCurriculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idRastreadorCurriculo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Rastreador_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idRastreadorCurriculo;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de RastreadorCurriculo no banco de dados.
		/// </summary>
		/// <param name="idRastreadorCurriculo">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idRastreadorCurriculo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Rastreador_Curriculo where Idf_Rastreador_Curriculo in (";

			for (int i = 0; i < idRastreadorCurriculo.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idRastreadorCurriculo[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idRastreadorCurriculo">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idRastreadorCurriculo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Rastreador_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idRastreadorCurriculo;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRastreadorCurriculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idRastreadorCurriculo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Rastreador_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idRastreadorCurriculo;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Ras.Idf_Rastreador_Curriculo, Ras.Idf_Filial, Ras.Idf_Cidade, Ras.Des_Palavra_Chave, Ras.Vlr_Salario_Inicio, Ras.Vlr_Salario_Fim, Ras.Idf_Escolaridade, Ras.Idf_Sexo, Ras.Des_Bairro, Ras.Idf_Categoria_Habilitacao, Ras.Idf_Area_BNE, Ras.Idf_Fonte, Ras.Idf_Deficiencia, Ras.Flg_Inativo, Ras.Dta_Cadastro, Ras.Idf_Curso, Ras.Idf_Curso_Fonte, Ras.Idf_Raca, Ras.Idf_Estado_Civil, Ras.Idf_Tipo_Veiculo, Ras.Flg_Filhos, Ras.Idf_Curso_Outros_Cursos, Ras.Idf_Fonte_Outros_Cursos, Ras.Idf_Origem, Ras.Idf_Usuario_Filial_Perfil, Ras.Des_IP, Ras.Sig_Estado, Ras.Num_Salario_Min, Ras.Num_Salario_Max, Ras.Num_CEP_Min, Ras.Num_CEP_Max, Ras.Des_Experiencia, Ras.Idf_Curso_Tecnico_Graduacao, Ras.Idf_Fonte_Tecnico_Graduacao, Ras.Des_Curso_Tecnico_Graduacao, Ras.Des_Curso_Outros_Cursos, Ras.Idf_Escolaridade_WebStagio, Ras.Flg_Aprendiz, Ras.Des_Filtro_Excludente, Ras.Des_Zona, Ras.Geo_Busca_Bairro, Ras.Geo_Busca_Cidade, Ras.Idf_Funcao_Exercida, Ras.Des_Funcao_Exercida, Ras.Des_Logradouro, Ras.Flg_Notifica_Dia, Ras.Flg_Notifica_Hora, Ras.Dta_Visualizacao, Ras.Num_Idade_Max, Ras.Num_Idade_Min FROM BNE_Rastreador_Curriculo Ras";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de RastreadorCurriculo a partir do banco de dados.
		/// </summary>
		/// <param name="idRastreadorCurriculo">Chave do registro.</param>
		/// <returns>Instância de RastreadorCurriculo.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static RastreadorCurriculo LoadObject(int idRastreadorCurriculo)
		{
			using (IDataReader dr = LoadDataReader(idRastreadorCurriculo))
			{
				RastreadorCurriculo objRastreadorCurriculo = new RastreadorCurriculo();
				if (SetInstance(dr, objRastreadorCurriculo))
					return objRastreadorCurriculo;
			}
			throw (new RecordNotFoundException(typeof(RastreadorCurriculo)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de RastreadorCurriculo a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRastreadorCurriculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de RastreadorCurriculo.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static RastreadorCurriculo LoadObject(int idRastreadorCurriculo, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idRastreadorCurriculo, trans))
			{
				RastreadorCurriculo objRastreadorCurriculo = new RastreadorCurriculo();
				if (SetInstance(dr, objRastreadorCurriculo))
					return objRastreadorCurriculo;
			}
			throw (new RecordNotFoundException(typeof(RastreadorCurriculo)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de RastreadorCurriculo a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idRastreadorCurriculo))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de RastreadorCurriculo a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idRastreadorCurriculo, trans))
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
		/// <param name="objRastreadorCurriculo">Instância a ser manipulada.</param>
		/// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, RastreadorCurriculo objRastreadorCurriculo, bool dispose = true)
		{
			try
			{
				if (dr.Read())
				{
					objRastreadorCurriculo._idRastreadorCurriculo = Convert.ToInt32(dr["Idf_Rastreador_Curriculo"]);
					objRastreadorCurriculo._filial = new Filial(Convert.ToInt32(dr["Idf_Filial"]));
					if (dr["Idf_Cidade"] != DBNull.Value)
						objRastreadorCurriculo._cidade = new Cidade(Convert.ToInt32(dr["Idf_Cidade"]));
					if (dr["Des_Palavra_Chave"] != DBNull.Value)
						objRastreadorCurriculo._descricaoPalavraChave = Convert.ToString(dr["Des_Palavra_Chave"]);
					if (dr["Vlr_Salario_Inicio"] != DBNull.Value)
						objRastreadorCurriculo._valorSalarioInicio = Convert.ToDecimal(dr["Vlr_Salario_Inicio"]);
					if (dr["Vlr_Salario_Fim"] != DBNull.Value)
						objRastreadorCurriculo._valorSalarioFim = Convert.ToDecimal(dr["Vlr_Salario_Fim"]);
					if (dr["Idf_Escolaridade"] != DBNull.Value)
						objRastreadorCurriculo._escolaridade = new Escolaridade(Convert.ToInt32(dr["Idf_Escolaridade"]));
					if (dr["Idf_Sexo"] != DBNull.Value)
						objRastreadorCurriculo._sexo = new Sexo(Convert.ToInt32(dr["Idf_Sexo"]));
					if (dr["Des_Bairro"] != DBNull.Value)
						objRastreadorCurriculo._descricaoBairro = Convert.ToString(dr["Des_Bairro"]);
					if (dr["Idf_Categoria_Habilitacao"] != DBNull.Value)
						objRastreadorCurriculo._categoriaHabilitacao = new CategoriaHabilitacao(Convert.ToInt32(dr["Idf_Categoria_Habilitacao"]));
					if (dr["Idf_Area_BNE"] != DBNull.Value)
						objRastreadorCurriculo._areaBNE = new AreaBNE(Convert.ToInt32(dr["Idf_Area_BNE"]));
					if (dr["Idf_Fonte"] != DBNull.Value)
						objRastreadorCurriculo._fonte = new Fonte(Convert.ToInt32(dr["Idf_Fonte"]));
					if (dr["Idf_Deficiencia"] != DBNull.Value)
						objRastreadorCurriculo._deficiencia = new Deficiencia(Convert.ToInt32(dr["Idf_Deficiencia"]));
					objRastreadorCurriculo._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					if (dr["Dta_Cadastro"] != DBNull.Value)
						objRastreadorCurriculo._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					if (dr["Idf_Curso"] != DBNull.Value)
						objRastreadorCurriculo._curso = new Curso(Convert.ToInt32(dr["Idf_Curso"]));
					if (dr["Idf_Curso_Fonte"] != DBNull.Value)
						objRastreadorCurriculo._cursoFonte = new CursoFonte(Convert.ToInt32(dr["Idf_Curso_Fonte"]));
					if (dr["Idf_Raca"] != DBNull.Value)
						objRastreadorCurriculo._raca = new Raca(Convert.ToInt32(dr["Idf_Raca"]));
					if (dr["Idf_Estado_Civil"] != DBNull.Value)
						objRastreadorCurriculo._estadoCivil = new EstadoCivil(Convert.ToInt32(dr["Idf_Estado_Civil"]));
					if (dr["Idf_Tipo_Veiculo"] != DBNull.Value)
						objRastreadorCurriculo._tipoVeiculo = new TipoVeiculo(Convert.ToInt32(dr["Idf_Tipo_Veiculo"]));
					if (dr["Flg_Filhos"] != DBNull.Value)
						objRastreadorCurriculo._flagFilhos = Convert.ToBoolean(dr["Flg_Filhos"]);
					if (dr["Idf_Curso_Outros_Cursos"] != DBNull.Value)
						objRastreadorCurriculo._cursoOutrosCursos = new Curso(Convert.ToInt32(dr["Idf_Curso_Outros_Cursos"]));
					if (dr["Idf_Fonte_Outros_Cursos"] != DBNull.Value)
						objRastreadorCurriculo._fonteOutrosCursos = new Fonte(Convert.ToInt32(dr["Idf_Fonte_Outros_Cursos"]));
					if (dr["Idf_Origem"] != DBNull.Value)
						objRastreadorCurriculo._origem = new Origem(Convert.ToInt32(dr["Idf_Origem"]));
					objRastreadorCurriculo._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));
					if (dr["Des_IP"] != DBNull.Value)
						objRastreadorCurriculo._descricaoIP = Convert.ToString(dr["Des_IP"]);
					if (dr["Sig_Estado"] != DBNull.Value)
                        objRastreadorCurriculo._estado = new Estado(Convert.ToString(dr["Sig_Estado"]));
					if (dr["Num_Salario_Min"] != DBNull.Value)
						objRastreadorCurriculo._numeroSalarioMin = Convert.ToDecimal(dr["Num_Salario_Min"]);
					if (dr["Num_Salario_Max"] != DBNull.Value)
						objRastreadorCurriculo._numeroSalarioMax = Convert.ToDecimal(dr["Num_Salario_Max"]);
					if (dr["Num_CEP_Min"] != DBNull.Value)
						objRastreadorCurriculo._numeroCEPMin = Convert.ToString(dr["Num_CEP_Min"]);
					if (dr["Num_CEP_Max"] != DBNull.Value)
						objRastreadorCurriculo._numeroCEPMax = Convert.ToString(dr["Num_CEP_Max"]);
					if (dr["Des_Experiencia"] != DBNull.Value)
						objRastreadorCurriculo._descricaoExperiencia = Convert.ToString(dr["Des_Experiencia"]);
					if (dr["Idf_Curso_Tecnico_Graduacao"] != DBNull.Value)
						objRastreadorCurriculo._cursoTecnicoGraduacao = new Curso(Convert.ToInt32(dr["Idf_Curso_Tecnico_Graduacao"]));
					if (dr["Idf_Fonte_Tecnico_Graduacao"] != DBNull.Value)
						objRastreadorCurriculo._fonteTecnicoGraduacao = new Fonte(Convert.ToInt32(dr["Idf_Fonte_Tecnico_Graduacao"]));
					if (dr["Des_Curso_Tecnico_Graduacao"] != DBNull.Value)
						objRastreadorCurriculo._descricaoCursoTecnicoGraduacao = Convert.ToString(dr["Des_Curso_Tecnico_Graduacao"]);
					if (dr["Des_Curso_Outros_Cursos"] != DBNull.Value)
						objRastreadorCurriculo._descricaoCursoOutrosCursos = Convert.ToString(dr["Des_Curso_Outros_Cursos"]);
					if (dr["Idf_Escolaridade_WebStagio"] != DBNull.Value)
						objRastreadorCurriculo._idEscolaridadeWebStagio = Convert.ToString(dr["Idf_Escolaridade_WebStagio"]);
					if (dr["Flg_Aprendiz"] != DBNull.Value)
						objRastreadorCurriculo._flagAprendiz = Convert.ToBoolean(dr["Flg_Aprendiz"]);
					if (dr["Des_Filtro_Excludente"] != DBNull.Value)
						objRastreadorCurriculo._descricaoFiltroExcludente = Convert.ToString(dr["Des_Filtro_Excludente"]);
					if (dr["Des_Zona"] != DBNull.Value)
						objRastreadorCurriculo._descricaoZona = Convert.ToString(dr["Des_Zona"]);
					if (dr["Geo_Busca_Bairro"] != DBNull.Value)
						objRastreadorCurriculo._GeoBuscaBairro = (SqlGeography)(dr["Geo_Busca_Bairro"]);
					if (dr["Geo_Busca_Cidade"] != DBNull.Value)
						objRastreadorCurriculo._GeoBuscaCidade = (SqlGeography)(dr["Geo_Busca_Cidade"]);
					if (dr["Idf_Funcao_Exercida"] != DBNull.Value)
						objRastreadorCurriculo._funcaoExercida = new Funcao(Convert.ToInt32(dr["Idf_Funcao_Exercida"]));
					if (dr["Des_Funcao_Exercida"] != DBNull.Value)
						objRastreadorCurriculo._descricaoFuncaoExercida = Convert.ToString(dr["Des_Funcao_Exercida"]);
					if (dr["Des_Logradouro"] != DBNull.Value)
						objRastreadorCurriculo._descricaoLogradouro = Convert.ToString(dr["Des_Logradouro"]);
					if (dr["Flg_Notifica_Dia"] != DBNull.Value)
						objRastreadorCurriculo._flagNotificaDia = Convert.ToBoolean(dr["Flg_Notifica_Dia"]);
					if (dr["Flg_Notifica_Hora"] != DBNull.Value)
						objRastreadorCurriculo._flagNotificaHora = Convert.ToBoolean(dr["Flg_Notifica_Hora"]);
					if (dr["Dta_Visualizacao"] != DBNull.Value)
						objRastreadorCurriculo._dataVisualizacao = Convert.ToDateTime(dr["Dta_Visualizacao"]);
					if (dr["Num_Idade_Max"] != DBNull.Value)
						objRastreadorCurriculo._numeroIdadeMax = Convert.ToInt16(dr["Num_Idade_Max"]);
					if (dr["Num_Idade_Min"] != DBNull.Value)
						objRastreadorCurriculo._numeroIdadeMin = Convert.ToInt16(dr["Num_Idade_Min"]);

					objRastreadorCurriculo._persisted = true;
					objRastreadorCurriculo._modified = false;

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