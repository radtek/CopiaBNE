//-- Data: 25/04/2011 16:58
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class Rastreador // Tabela: BNE_Rastreador
	{
		#region Atributos
		private int _idRastreador;
		private Filial _filial;
		private Funcao _funcao;
		private Cidade _cidade;
		private string _descricaoPalavraChave;
		private decimal? _valorSalarioInicio;
		private decimal? _valorSalarioFim;
		private Int16? _quantidadeExperiencia;
		private Escolaridade _escolaridade;
		private int? _descricaoIdadeInicio;
		private int? _descricaoIdadeFim;
		private Sexo _sexo;
		private string _descricaoBairro;
		private CategoriaHabilitacao _categoriaHabilitacao;
		private string _razaoSocial;
		private AreaBNE _areaBNE;
		private Fonte _fonte;
		private Deficiencia _deficiencia;
		private bool? _flagRegiaoMetropolitana;
		private bool? _flagInativo;
		private DateTime? _dataCadastro;
		private Curso _curso;
		private CursoFonte _cursoFonte;
		private Raca _raca;
		private EstadoCivil _estadoCivil;
		private TipoVeiculo _tipoVeiculo;
		private string _descricaoCEPFim;
		private bool? _flagFilhos;
		private string _descricaoCEPInicio;
		private Curso _cursoOutrosCursos;
		private Fonte _fonteOutrosCursos;
		private Estado _estado;
		private Origem _origem;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdRastreador
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdRastreador
		{
			get
			{
				return this._idRastreador;
			}
		}
		#endregion 

		#region Filial
		/// <summary>
		/// Campo opcional.
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

		#region Funcao
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Funcao Funcao
		{
			get
			{
				return this._funcao;
			}
			set
			{
				this._funcao = value;
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
		/// Tamanho do campo: 50.
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

		#region DescricaoIdadeInicio
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public int? DescricaoIdadeInicio
		{
			get
			{
				return this._descricaoIdadeInicio;
			}
			set
			{
				this._descricaoIdadeInicio = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoIdadeFim
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public int? DescricaoIdadeFim
		{
			get
			{
				return this._descricaoIdadeFim;
			}
			set
			{
				this._descricaoIdadeFim = value;
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
		/// Tamanho do campo: 50.
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

		#region FlagRegiaoMetropolitana
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public bool? FlagRegiaoMetropolitana
		{
			get
			{
				return this._flagRegiaoMetropolitana;
			}
			set
			{
				this._flagRegiaoMetropolitana = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagInativo
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public bool? FlagInativo
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

		#region DescricaoCEPFim
		/// <summary>
		/// Tamanho do campo: 8.
		/// Campo opcional.
		/// </summary>
		public string DescricaoCEPFim
		{
			get
			{
				return this._descricaoCEPFim;
			}
			set
			{
				this._descricaoCEPFim = value;
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

		#region DescricaoCEPInicio
		/// <summary>
		/// Tamanho do campo: 8.
		/// Campo opcional.
		/// </summary>
		public string DescricaoCEPInicio
		{
			get
			{
				return this._descricaoCEPInicio;
			}
			set
			{
				this._descricaoCEPInicio = value;
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

		#endregion

		#region Construtores
		public Rastreador()
		{
		}
		public Rastreador(int idRastreador)
		{
			this._idRastreador = idRastreador;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Rastreador (Idf_Filial, Idf_Funcao, Idf_Cidade, Des_Palavra_Chave, Vlr_Salario_Inicio, Vlr_Salario_Fim, Qtd_Experiencia, Idf_Escolaridade, Des_Idade_Inicio, Des_Idade_Fim, Idf_Sexo, Des_Bairro, Idf_Categoria_Habilitacao, Raz_Social, Idf_Area_BNE, Idf_Fonte, Idf_Deficiencia, Flg_Regiao_Metropolitana, Flg_Inativo, Dta_Cadastro, Idf_Curso, Idf_Curso_Fonte, Idf_Raca, Idf_Estado_Civil, Idf_Tipo_Veiculo, Des_CEP_Fim, Flg_Filhos, Des_CEP_Inicio, Idf_Curso_Outros_Cursos, Idf_Fonte_Outros_Cursos, Idf_Estado, Idf_Origem) VALUES (@Idf_Filial, @Idf_Funcao, @Idf_Cidade, @Des_Palavra_Chave, @Vlr_Salario_Inicio, @Vlr_Salario_Fim, @Qtd_Experiencia, @Idf_Escolaridade, @Des_Idade_Inicio, @Des_Idade_Fim, @Idf_Sexo, @Des_Bairro, @Idf_Categoria_Habilitacao, @Raz_Social, @Idf_Area_BNE, @Idf_Fonte, @Idf_Deficiencia, @Flg_Regiao_Metropolitana, @Flg_Inativo, @Dta_Cadastro, @Idf_Curso, @Idf_Curso_Fonte, @Idf_Raca, @Idf_Estado_Civil, @Idf_Tipo_Veiculo, @Des_CEP_Fim, @Flg_Filhos, @Des_CEP_Inicio, @Idf_Curso_Outros_Cursos, @Idf_Fonte_Outros_Cursos, @Idf_Estado, @Idf_Origem);SET @Idf_Rastreador = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Rastreador SET Idf_Filial = @Idf_Filial, Idf_Funcao = @Idf_Funcao, Idf_Cidade = @Idf_Cidade, Des_Palavra_Chave = @Des_Palavra_Chave, Vlr_Salario_Inicio = @Vlr_Salario_Inicio, Vlr_Salario_Fim = @Vlr_Salario_Fim, Qtd_Experiencia = @Qtd_Experiencia, Idf_Escolaridade = @Idf_Escolaridade, Des_Idade_Inicio = @Des_Idade_Inicio, Des_Idade_Fim = @Des_Idade_Fim, Idf_Sexo = @Idf_Sexo, Des_Bairro = @Des_Bairro, Idf_Categoria_Habilitacao = @Idf_Categoria_Habilitacao, Raz_Social = @Raz_Social, Idf_Area_BNE = @Idf_Area_BNE, Idf_Fonte = @Idf_Fonte, Idf_Deficiencia = @Idf_Deficiencia, Flg_Regiao_Metropolitana = @Flg_Regiao_Metropolitana, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro, Idf_Curso = @Idf_Curso, Idf_Curso_Fonte = @Idf_Curso_Fonte, Idf_Raca = @Idf_Raca, Idf_Estado_Civil = @Idf_Estado_Civil, Idf_Tipo_Veiculo = @Idf_Tipo_Veiculo, Des_CEP_Fim = @Des_CEP_Fim, Flg_Filhos = @Flg_Filhos, Des_CEP_Inicio = @Des_CEP_Inicio, Idf_Curso_Outros_Cursos = @Idf_Curso_Outros_Cursos, Idf_Fonte_Outros_Cursos = @Idf_Fonte_Outros_Cursos, Idf_Estado = @Idf_Estado, Idf_Origem = @Idf_Origem WHERE Idf_Rastreador = @Idf_Rastreador";
		private const string SPDELETE = "DELETE FROM BNE_Rastreador WHERE Idf_Rastreador = @Idf_Rastreador";
		private const string SPSELECTID = "SELECT * FROM BNE_Rastreador WHERE Idf_Rastreador = @Idf_Rastreador";
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
			parms.Add(new SqlParameter("@Idf_Rastreador", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Funcao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Palavra_Chave", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Vlr_Salario_Inicio", SqlDbType.Money, 8));
			parms.Add(new SqlParameter("@Vlr_Salario_Fim", SqlDbType.Money, 8));
			parms.Add(new SqlParameter("@Qtd_Experiencia", SqlDbType.Int, 2));
			parms.Add(new SqlParameter("@Idf_Escolaridade", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Idade_Inicio", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Idade_Fim", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Sexo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Bairro", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Idf_Categoria_Habilitacao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Raz_Social", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Idf_Area_BNE", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Fonte", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Deficiencia", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Flg_Regiao_Metropolitana", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Idf_Curso", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curso_Fonte", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Raca", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Estado_Civil", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Tipo_Veiculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_CEP_Fim", SqlDbType.VarChar, 8));
			parms.Add(new SqlParameter("@Flg_Filhos", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Des_CEP_Inicio", SqlDbType.VarChar, 8));
			parms.Add(new SqlParameter("@Idf_Curso_Outros_Cursos", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Fonte_Outros_Cursos", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Estado", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Origem", SqlDbType.Int, 4));
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
			parms[0].Value = this._idRastreador;

			if (this._filial != null)
				parms[1].Value = this._filial.IdFilial;
			else
				parms[1].Value = DBNull.Value;


			if (this._funcao != null)
				parms[2].Value = this._funcao.IdFuncao;
			else
				parms[2].Value = DBNull.Value;


			if (this._cidade != null)
				parms[3].Value = this._cidade.IdCidade;
			else
				parms[3].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoPalavraChave))
				parms[4].Value = this._descricaoPalavraChave;
			else
				parms[4].Value = DBNull.Value;


			if (this._valorSalarioInicio.HasValue)
				parms[5].Value = this._valorSalarioInicio;
			else
				parms[5].Value = DBNull.Value;


			if (this._valorSalarioFim.HasValue)
				parms[6].Value = this._valorSalarioFim;
			else
				parms[6].Value = DBNull.Value;


			if (this._quantidadeExperiencia.HasValue)
				parms[7].Value = this._quantidadeExperiencia;
			else
				parms[7].Value = DBNull.Value;


			if (this._escolaridade != null)
				parms[8].Value = this._escolaridade.IdEscolaridade;
			else
				parms[8].Value = DBNull.Value;


			if (this._descricaoIdadeInicio.HasValue)
				parms[9].Value = this._descricaoIdadeInicio;
			else
				parms[9].Value = DBNull.Value;


			if (this._descricaoIdadeFim.HasValue)
				parms[10].Value = this._descricaoIdadeFim;
			else
				parms[10].Value = DBNull.Value;


			if (this._sexo != null)
				parms[11].Value = this._sexo.IdSexo;
			else
				parms[11].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoBairro))
				parms[12].Value = this._descricaoBairro;
			else
				parms[12].Value = DBNull.Value;


			if (this._categoriaHabilitacao != null)
				parms[13].Value = this._categoriaHabilitacao.IdCategoriaHabilitacao;
			else
				parms[13].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._razaoSocial))
				parms[14].Value = this._razaoSocial;
			else
				parms[14].Value = DBNull.Value;


			if (this._areaBNE != null)
				parms[15].Value = this._areaBNE.IdAreaBNE;
			else
				parms[15].Value = DBNull.Value;


			if (this._fonte != null)
				parms[16].Value = this._fonte.IdFonte;
			else
				parms[16].Value = DBNull.Value;


			if (this._deficiencia != null)
				parms[17].Value = this._deficiencia.IdDeficiencia;
			else
				parms[17].Value = DBNull.Value;


			if (this._flagRegiaoMetropolitana.HasValue)
				parms[18].Value = this._flagRegiaoMetropolitana;
			else
				parms[18].Value = DBNull.Value;


			if (this._flagInativo.HasValue)
				parms[19].Value = this._flagInativo;
			else
				parms[19].Value = DBNull.Value;


			if (this._curso != null)
				parms[21].Value = this._curso.IdCurso;
			else
				parms[21].Value = DBNull.Value;


			if (this._cursoFonte != null)
				parms[22].Value = this._cursoFonte.IdCursoFonte;
			else
				parms[22].Value = DBNull.Value;


			if (this._raca != null)
				parms[23].Value = this._raca.IdRaca;
			else
				parms[23].Value = DBNull.Value;


			if (this._estadoCivil != null)
				parms[24].Value = this._estadoCivil.IdEstadoCivil;
			else
				parms[24].Value = DBNull.Value;


			if (this._tipoVeiculo != null)
				parms[25].Value = this._tipoVeiculo.IdTipoVeiculo;
			else
				parms[25].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoCEPFim))
				parms[26].Value = this._descricaoCEPFim;
			else
				parms[26].Value = DBNull.Value;


			if (this._flagFilhos.HasValue)
				parms[27].Value = this._flagFilhos;
			else
				parms[27].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoCEPInicio))
				parms[28].Value = this._descricaoCEPInicio;
			else
				parms[28].Value = DBNull.Value;


			if (this._cursoOutrosCursos != null)
				parms[29].Value = this._cursoOutrosCursos.IdCurso;
			else
				parms[29].Value = DBNull.Value;


			if (this._fonteOutrosCursos != null)
				parms[30].Value = this._fonteOutrosCursos.IdFonte;
			else
				parms[30].Value = DBNull.Value;


			if (this._estado != null)
				parms[31].Value = this._estado.IdEstado;
			else
				parms[31].Value = DBNull.Value;


			if (this._origem != null)
				parms[32].Value = this._origem.IdOrigem;
			else
				parms[32].Value = DBNull.Value;


			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[20].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de Rastreador no banco de dados.
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
						this._idRastreador = Convert.ToInt32(cmd.Parameters["@Idf_Rastreador"].Value);
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
		/// Método utilizado para inserir uma instância de Rastreador no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idRastreador = Convert.ToInt32(cmd.Parameters["@Idf_Rastreador"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de Rastreador no banco de dados.
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
		/// Método utilizado para atualizar uma instância de Rastreador no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de Rastreador no banco de dados.
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
		/// Método utilizado para salvar uma instância de Rastreador no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de Rastreador no banco de dados.
		/// </summary>
		/// <param name="idRastreador">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idRastreador)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Rastreador", SqlDbType.Int, 4));

			parms[0].Value = idRastreador;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de Rastreador no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRastreador">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idRastreador, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Rastreador", SqlDbType.Int, 4));

			parms[0].Value = idRastreador;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de Rastreador no banco de dados.
		/// </summary>
		/// <param name="idRastreador">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idRastreador)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Rastreador where Idf_Rastreador in (";

			for (int i = 0; i < idRastreador.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idRastreador[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idRastreador">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idRastreador)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Rastreador", SqlDbType.Int, 4));

			parms[0].Value = idRastreador;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRastreador">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idRastreador, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Rastreador", SqlDbType.Int, 4));

			parms[0].Value = idRastreador;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Ras.Idf_Rastreador, Ras.Idf_Filial, Ras.Idf_Funcao, Ras.Idf_Cidade, Ras.Des_Palavra_Chave, Ras.Vlr_Salario_Inicio, Ras.Vlr_Salario_Fim, Ras.Qtd_Experiencia, Ras.Idf_Escolaridade, Ras.Des_Idade_Inicio, Ras.Des_Idade_Fim, Ras.Idf_Sexo, Ras.Des_Bairro, Ras.Idf_Categoria_Habilitacao, Ras.Raz_Social, Ras.Idf_Area_BNE, Ras.Idf_Fonte, Ras.Idf_Deficiencia, Ras.Flg_Regiao_Metropolitana, Ras.Flg_Inativo, Ras.Dta_Cadastro, Ras.Idf_Curso, Ras.Idf_Curso_Fonte, Ras.Idf_Raca, Ras.Idf_Estado_Civil, Ras.Idf_Tipo_Veiculo, Ras.Des_CEP_Fim, Ras.Flg_Filhos, Ras.Des_CEP_Inicio, Ras.Idf_Curso_Outros_Cursos, Ras.Idf_Fonte_Outros_Cursos, Ras.Idf_Estado, Ras.Idf_Origem FROM BNE_Rastreador Ras";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de Rastreador a partir do banco de dados.
		/// </summary>
		/// <param name="idRastreador">Chave do registro.</param>
		/// <returns>Instância de Rastreador.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static Rastreador LoadObject(int idRastreador)
		{
			using (IDataReader dr = LoadDataReader(idRastreador))
			{
				Rastreador objRastreador = new Rastreador();
				if (SetInstance(dr, objRastreador))
					return objRastreador;
			}
			throw (new RecordNotFoundException(typeof(Rastreador)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de Rastreador a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRastreador">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de Rastreador.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static Rastreador LoadObject(int idRastreador, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idRastreador, trans))
			{
				Rastreador objRastreador = new Rastreador();
				if (SetInstance(dr, objRastreador))
					return objRastreador;
			}
			throw (new RecordNotFoundException(typeof(Rastreador)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de Rastreador a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idRastreador))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de Rastreador a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idRastreador, trans))
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
		/// <param name="objRastreador">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, Rastreador objRastreador)
		{
			try
			{
				if (dr.Read())
				{
					objRastreador._idRastreador = Convert.ToInt32(dr["Idf_Rastreador"]);
					if (dr["Idf_Filial"] != DBNull.Value)
						objRastreador._filial = new Filial(Convert.ToInt32(dr["Idf_Filial"]));
					if (dr["Idf_Funcao"] != DBNull.Value)
						objRastreador._funcao = new Funcao(Convert.ToInt32(dr["Idf_Funcao"]));
					if (dr["Idf_Cidade"] != DBNull.Value)
						objRastreador._cidade = new Cidade(Convert.ToInt32(dr["Idf_Cidade"]));
					if (dr["Des_Palavra_Chave"] != DBNull.Value)
						objRastreador._descricaoPalavraChave = Convert.ToString(dr["Des_Palavra_Chave"]);
					if (dr["Vlr_Salario_Inicio"] != DBNull.Value)
						objRastreador._valorSalarioInicio = Convert.ToDecimal(dr["Vlr_Salario_Inicio"]);
					if (dr["Vlr_Salario_Fim"] != DBNull.Value)
						objRastreador._valorSalarioFim = Convert.ToDecimal(dr["Vlr_Salario_Fim"]);
					if (dr["Qtd_Experiencia"] != DBNull.Value)
						objRastreador._quantidadeExperiencia = Convert.ToInt16(dr["Qtd_Experiencia"]);
					if (dr["Idf_Escolaridade"] != DBNull.Value)
						objRastreador._escolaridade = new Escolaridade(Convert.ToInt32(dr["Idf_Escolaridade"]));
					if (dr["Des_Idade_Inicio"] != DBNull.Value)
						objRastreador._descricaoIdadeInicio = Convert.ToInt32(dr["Des_Idade_Inicio"]);
					if (dr["Des_Idade_Fim"] != DBNull.Value)
						objRastreador._descricaoIdadeFim = Convert.ToInt32(dr["Des_Idade_Fim"]);
					if (dr["Idf_Sexo"] != DBNull.Value)
						objRastreador._sexo = new Sexo(Convert.ToInt32(dr["Idf_Sexo"]));
					if (dr["Des_Bairro"] != DBNull.Value)
						objRastreador._descricaoBairro = Convert.ToString(dr["Des_Bairro"]);
					if (dr["Idf_Categoria_Habilitacao"] != DBNull.Value)
						objRastreador._categoriaHabilitacao = new CategoriaHabilitacao(Convert.ToInt32(dr["Idf_Categoria_Habilitacao"]));
					if (dr["Raz_Social"] != DBNull.Value)
						objRastreador._razaoSocial = Convert.ToString(dr["Raz_Social"]);
					if (dr["Idf_Area_BNE"] != DBNull.Value)
						objRastreador._areaBNE = new AreaBNE(Convert.ToInt32(dr["Idf_Area_BNE"]));
					if (dr["Idf_Fonte"] != DBNull.Value)
						objRastreador._fonte = new Fonte(Convert.ToInt32(dr["Idf_Fonte"]));
					if (dr["Idf_Deficiencia"] != DBNull.Value)
						objRastreador._deficiencia = new Deficiencia(Convert.ToInt32(dr["Idf_Deficiencia"]));
					if (dr["Flg_Regiao_Metropolitana"] != DBNull.Value)
						objRastreador._flagRegiaoMetropolitana = Convert.ToBoolean(dr["Flg_Regiao_Metropolitana"]);
					if (dr["Flg_Inativo"] != DBNull.Value)
						objRastreador._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					if (dr["Dta_Cadastro"] != DBNull.Value)
						objRastreador._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					if (dr["Idf_Curso"] != DBNull.Value)
						objRastreador._curso = new Curso(Convert.ToInt32(dr["Idf_Curso"]));
					if (dr["Idf_Curso_Fonte"] != DBNull.Value)
						objRastreador._cursoFonte = new CursoFonte(Convert.ToInt32(dr["Idf_Curso_Fonte"]));
					if (dr["Idf_Raca"] != DBNull.Value)
						objRastreador._raca = new Raca(Convert.ToInt32(dr["Idf_Raca"]));
					if (dr["Idf_Estado_Civil"] != DBNull.Value)
						objRastreador._estadoCivil = new EstadoCivil(Convert.ToInt32(dr["Idf_Estado_Civil"]));
					if (dr["Idf_Tipo_Veiculo"] != DBNull.Value)
						objRastreador._tipoVeiculo = new TipoVeiculo(Convert.ToInt32(dr["Idf_Tipo_Veiculo"]));
					if (dr["Des_CEP_Fim"] != DBNull.Value)
						objRastreador._descricaoCEPFim = Convert.ToString(dr["Des_CEP_Fim"]);
					if (dr["Flg_Filhos"] != DBNull.Value)
						objRastreador._flagFilhos = Convert.ToBoolean(dr["Flg_Filhos"]);
					if (dr["Des_CEP_Inicio"] != DBNull.Value)
						objRastreador._descricaoCEPInicio = Convert.ToString(dr["Des_CEP_Inicio"]);
					if (dr["Idf_Curso_Outros_Cursos"] != DBNull.Value)
						objRastreador._cursoOutrosCursos = new Curso(Convert.ToInt32(dr["Idf_Curso_Outros_Cursos"]));
					if (dr["Idf_Fonte_Outros_Cursos"] != DBNull.Value)
						objRastreador._fonteOutrosCursos = new Fonte(Convert.ToInt32(dr["Idf_Fonte_Outros_Cursos"]));
					if (dr["Idf_Estado"] != DBNull.Value)
						objRastreador._estado = new Estado(Convert.ToInt32(dr["Idf_Estado"]));
					if (dr["Idf_Origem"] != DBNull.Value)
						objRastreador._origem = new Origem(Convert.ToInt32(dr["Idf_Origem"]));

					objRastreador._persisted = true;
					objRastreador._modified = false;

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
				dr.Dispose();
			}
		}
		#endregion

        #region SetInstanceNonDispose
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objRastreador">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Elias</remarks>
        private static bool SetInstanceNonDispose(IDataReader dr, Rastreador objRastreador)
        {
            if (dr.Read())
            {
                objRastreador._idRastreador = Convert.ToInt32(dr["Idf_Rastreador"]);
                if (dr["Idf_Filial"] != DBNull.Value)
                    objRastreador._filial = new Filial(Convert.ToInt32(dr["Idf_Filial"]));
                if (dr["Idf_Funcao"] != DBNull.Value)
                    objRastreador._funcao = new Funcao(Convert.ToInt32(dr["Idf_Funcao"]));
                if (dr["Idf_Cidade"] != DBNull.Value)
                    objRastreador._cidade = new Cidade(Convert.ToInt32(dr["Idf_Cidade"]));
                if (dr["Des_Palavra_Chave"] != DBNull.Value)
                    objRastreador._descricaoPalavraChave = Convert.ToString(dr["Des_Palavra_Chave"]);
                if (dr["Vlr_Salario_Inicio"] != DBNull.Value)
                    objRastreador._valorSalarioInicio = Convert.ToDecimal(dr["Vlr_Salario_Inicio"]);
                if (dr["Vlr_Salario_Fim"] != DBNull.Value)
                    objRastreador._valorSalarioFim = Convert.ToDecimal(dr["Vlr_Salario_Fim"]);
                if (dr["Qtd_Experiencia"] != DBNull.Value)
                    objRastreador._quantidadeExperiencia = Convert.ToInt16(dr["Qtd_Experiencia"]);
                if (dr["Idf_Escolaridade"] != DBNull.Value)
                    objRastreador._escolaridade = new Escolaridade(Convert.ToInt32(dr["Idf_Escolaridade"]));
                if (dr["Des_Idade_Inicio"] != DBNull.Value)
                    objRastreador._descricaoIdadeInicio = Convert.ToInt32(dr["Des_Idade_Inicio"]);
                if (dr["Des_Idade_Fim"] != DBNull.Value)
                    objRastreador._descricaoIdadeFim = Convert.ToInt32(dr["Des_Idade_Fim"]);
                if (dr["Idf_Sexo"] != DBNull.Value)
                    objRastreador._sexo = new Sexo(Convert.ToInt32(dr["Idf_Sexo"]));
                if (dr["Des_Bairro"] != DBNull.Value)
                    objRastreador._descricaoBairro = Convert.ToString(dr["Des_Bairro"]);
                if (dr["Idf_Categoria_Habilitacao"] != DBNull.Value)
                    objRastreador._categoriaHabilitacao = new CategoriaHabilitacao(Convert.ToInt32(dr["Idf_Categoria_Habilitacao"]));
                if (dr["Raz_Social"] != DBNull.Value)
                    objRastreador._razaoSocial = Convert.ToString(dr["Raz_Social"]);
                if (dr["Idf_Area_BNE"] != DBNull.Value)
                    objRastreador._areaBNE = new AreaBNE(Convert.ToInt32(dr["Idf_Area_BNE"]));
                if (dr["Idf_Fonte"] != DBNull.Value)
                    objRastreador._fonte = new Fonte(Convert.ToInt32(dr["Idf_Fonte"]));
                if (dr["Idf_Deficiencia"] != DBNull.Value)
                    objRastreador._deficiencia = new Deficiencia(Convert.ToInt32(dr["Idf_Deficiencia"]));
                if (dr["Flg_Regiao_Metropolitana"] != DBNull.Value)
                    objRastreador._flagRegiaoMetropolitana = Convert.ToBoolean(dr["Flg_Regiao_Metropolitana"]);
                if (dr["Flg_Inativo"] != DBNull.Value)
                    objRastreador._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                if (dr["Dta_Cadastro"] != DBNull.Value)
                    objRastreador._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                if (dr["Idf_Curso"] != DBNull.Value)
                    objRastreador._curso = new Curso(Convert.ToInt32(dr["Idf_Curso"]));
                if (dr["Idf_Curso_Fonte"] != DBNull.Value)
                    objRastreador._cursoFonte = new CursoFonte(Convert.ToInt32(dr["Idf_Curso_Fonte"]));
                if (dr["Idf_Raca"] != DBNull.Value)
                    objRastreador._raca = new Raca(Convert.ToInt32(dr["Idf_Raca"]));
                if (dr["Idf_Estado_Civil"] != DBNull.Value)
                    objRastreador._estadoCivil = new EstadoCivil(Convert.ToInt32(dr["Idf_Estado_Civil"]));
                if (dr["Idf_Tipo_Veiculo"] != DBNull.Value)
                    objRastreador._tipoVeiculo = new TipoVeiculo(Convert.ToInt32(dr["Idf_Tipo_Veiculo"]));
                if (dr["Des_CEP_Fim"] != DBNull.Value)
                    objRastreador._descricaoCEPFim = Convert.ToString(dr["Des_CEP_Fim"]);
                if (dr["Flg_Filhos"] != DBNull.Value)
                    objRastreador._flagFilhos = Convert.ToBoolean(dr["Flg_Filhos"]);
                if (dr["Des_CEP_Inicio"] != DBNull.Value)
                    objRastreador._descricaoCEPInicio = Convert.ToString(dr["Des_CEP_Inicio"]);
                if (dr["Idf_Curso_Outros_Cursos"] != DBNull.Value)
                    objRastreador._cursoOutrosCursos = new Curso(Convert.ToInt32(dr["Idf_Curso_Outros_Cursos"]));
                if (dr["Idf_Fonte_Outros_Cursos"] != DBNull.Value)
                    objRastreador._fonteOutrosCursos = new Fonte(Convert.ToInt32(dr["Idf_Fonte_Outros_Cursos"]));
                if (dr["Idf_Estado"] != DBNull.Value)
                    objRastreador._estado = new Estado(Convert.ToInt32(dr["Idf_Estado"]));
                if (dr["Idf_Origem"] != DBNull.Value)
                    objRastreador._origem = new Origem(Convert.ToInt32(dr["Idf_Origem"]));

                objRastreador._persisted = true;
                objRastreador._modified = false;

                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

		#endregion

	}
}