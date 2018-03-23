//-- Data: 16/11/2010 15:04
//-- Autor: Vinicius Maciel

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class Funcao // Tabela: plataforma.TAB_Funcao
	{
		#region Atributos
		private int _idFuncao;
		private Funcao _funcaoAgrupadora;
		private string _descricaoFuncao;
		private FuncaoCategoria _funcaoCategoria;
		private ClasseSalarial _classeSalarial;
		private Escolaridade _escolaridade;
		private AreaBNE _areaBNE;
		private bool? _flagMenorAprendiz;
		private string _descricaoJob;
		private string _descricaoExperiencia;
		private decimal? _valorPisoProfissional;
		private string _descricaoCursos;
		private string _descricaoCompetencias;
		private bool _flagInativo;
		private DateTime _dataCadastro;
		private string _descricaoFuncaoPesquisa;
		private string _codigoFuncaoFolha;
		private string _codigoCBO;
		private bool _flagConselho;
		private bool _flagCursoEspecializacao;
		private bool? _flagRAISAnalfabeto;
		private bool? _flagRAISNivelSuperior;
		private bool _flagCategoriaSindical;
		private bool _flagAuditada;
		private Int16? _quantidadeCargaHorariaDiaria;
		private string _descricaoLocalTrabalho;
		private string _descricaoEPI;
		private string _descricaoPPRA;
		private string _descricaoPCMSO;
		private string _descricaoEquipamentos;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdFuncao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdFuncao
		{
			get
			{
				return this._idFuncao;
			}
			set
			{
				this._idFuncao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FuncaoAgrupadora
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Funcao FuncaoAgrupadora
		{
			get
			{
				return this._funcaoAgrupadora;
			}
			set
			{
				this._funcaoAgrupadora = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoFuncao
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoFuncao
		{
			get
			{
				return this._descricaoFuncao;
			}
			set
			{
				this._descricaoFuncao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FuncaoCategoria
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public FuncaoCategoria FuncaoCategoria
		{
			get
			{
				return this._funcaoCategoria;
			}
			set
			{
				this._funcaoCategoria = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ClasseSalarial
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public ClasseSalarial ClasseSalarial
		{
			get
			{
				return this._classeSalarial;
			}
			set
			{
				this._classeSalarial = value;
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

		#region FlagMenorAprendiz
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public bool? FlagMenorAprendiz
		{
			get
			{
				return this._flagMenorAprendiz;
			}
			set
			{
				this._flagMenorAprendiz = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoJob
		/// <summary>
		/// Tamanho do campo: 2000.
		/// Campo opcional.
		/// </summary>
		public string DescricaoJob
		{
			get
			{
				return this._descricaoJob;
			}
			set
			{
				this._descricaoJob = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoExperiencia
		/// <summary>
		/// Tamanho do campo: 2000.
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

		#region ValorPisoProfissional
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public decimal? ValorPisoProfissional
		{
			get
			{
				return this._valorPisoProfissional;
			}
			set
			{
				this._valorPisoProfissional = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoCursos
		/// <summary>
		/// Tamanho do campo: 2000.
		/// Campo opcional.
		/// </summary>
		public string DescricaoCursos
		{
			get
			{
				return this._descricaoCursos;
			}
			set
			{
				this._descricaoCursos = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoCompetencias
		/// <summary>
		/// Tamanho do campo: 2000.
		/// Campo opcional.
		/// </summary>
		public string DescricaoCompetencias
		{
			get
			{
				return this._descricaoCompetencias;
			}
			set
			{
				this._descricaoCompetencias = value;
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
		/// Campo obrigatório.
		/// </summary>
		public DateTime DataCadastro
		{
			get
			{
				return this._dataCadastro;
			}
		}
		#endregion 

		#region DescricaoFuncaoPesquisa
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoFuncaoPesquisa
		{
			get
			{
				return this._descricaoFuncaoPesquisa;
			}
			set
			{
				this._descricaoFuncaoPesquisa = value;
				this._modified = true;
			}
		}
		#endregion 

		#region CodigoFuncaoFolha
		/// <summary>
		/// Tamanho do campo: 10.
		/// Campo opcional.
		/// </summary>
		public string CodigoFuncaoFolha
		{
			get
			{
				return this._codigoFuncaoFolha;
			}
			set
			{
				this._codigoFuncaoFolha = value;
				this._modified = true;
			}
		}
		#endregion 

		#region CodigoCBO
		/// <summary>
		/// Tamanho do campo: 6.
		/// Campo obrigatório.
		/// </summary>
		public string CodigoCBO
		{
			get
			{
				return this._codigoCBO;
			}
			set
			{
				this._codigoCBO = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagConselho
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagConselho
		{
			get
			{
				return this._flagConselho;
			}
			set
			{
				this._flagConselho = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagCursoEspecializacao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagCursoEspecializacao
		{
			get
			{
				return this._flagCursoEspecializacao;
			}
			set
			{
				this._flagCursoEspecializacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagRAISAnalfabeto
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public bool? FlagRAISAnalfabeto
		{
			get
			{
				return this._flagRAISAnalfabeto;
			}
			set
			{
				this._flagRAISAnalfabeto = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagRAISNivelSuperior
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public bool? FlagRAISNivelSuperior
		{
			get
			{
				return this._flagRAISNivelSuperior;
			}
			set
			{
				this._flagRAISNivelSuperior = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagCategoriaSindical
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagCategoriaSindical
		{
			get
			{
				return this._flagCategoriaSindical;
			}
			set
			{
				this._flagCategoriaSindical = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagAuditada
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagAuditada
		{
			get
			{
				return this._flagAuditada;
			}
			set
			{
				this._flagAuditada = value;
				this._modified = true;
			}
		}
		#endregion 

		#region QuantidadeCargaHorariaDiaria
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Int16? QuantidadeCargaHorariaDiaria
		{
			get
			{
				return this._quantidadeCargaHorariaDiaria;
			}
			set
			{
				this._quantidadeCargaHorariaDiaria = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoLocalTrabalho
		/// <summary>
		/// Tamanho do campo: 2000.
		/// Campo opcional.
		/// </summary>
		public string DescricaoLocalTrabalho
		{
			get
			{
				return this._descricaoLocalTrabalho;
			}
			set
			{
				this._descricaoLocalTrabalho = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoEPI
		/// <summary>
		/// Tamanho do campo: 2000.
		/// Campo opcional.
		/// </summary>
		public string DescricaoEPI
		{
			get
			{
				return this._descricaoEPI;
			}
			set
			{
				this._descricaoEPI = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoPPRA
		/// <summary>
		/// Tamanho do campo: 2000.
		/// Campo opcional.
		/// </summary>
		public string DescricaoPPRA
		{
			get
			{
				return this._descricaoPPRA;
			}
			set
			{
				this._descricaoPPRA = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoPCMSO
		/// <summary>
		/// Tamanho do campo: 2000.
		/// Campo opcional.
		/// </summary>
		public string DescricaoPCMSO
		{
			get
			{
				return this._descricaoPCMSO;
			}
			set
			{
				this._descricaoPCMSO = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoEquipamentos
		/// <summary>
		/// Tamanho do campo: 2000.
		/// Campo opcional.
		/// </summary>
		public string DescricaoEquipamentos
		{
			get
			{
				return this._descricaoEquipamentos;
			}
			set
			{
				this._descricaoEquipamentos = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public Funcao()
		{
		}
		public Funcao(int idFuncao)
		{
			this._idFuncao = idFuncao;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO plataforma.TAB_Funcao (Idf_Funcao, Idf_Funcao_Agrupadora, Des_Funcao, Idf_Funcao_Categoria, Idf_Classe_Salarial, Idf_Escolaridade, Idf_Area_BNE, Flg_Menor_Aprendiz, Des_Job, Des_Experiencia, Vlr_Piso_Profissional, Des_Cursos, Des_Competencias, Flg_Inativo, Dta_Cadastro, Des_Funcao_Pesquisa, Cod_Funcao_Folha, Cod_CBO, Flg_Conselho, Flg_Curso_Especializacao, Flg_RAIS_Analfabeto, Flg_RAIS_Nivel_Superior, Flg_Categoria_Sindical, Flg_Auditada, Qtd_Carga_Horaria_Diaria, Des_Local_Trabalho, Des_EPI, Des_PPRA, Des_PCMSO, Des_Equipamentos) VALUES (@Idf_Funcao, @Idf_Funcao_Agrupadora, @Des_Funcao, @Idf_Funcao_Categoria, @Idf_Classe_Salarial, @Idf_Escolaridade, @Idf_Area_BNE, @Flg_Menor_Aprendiz, @Des_Job, @Des_Experiencia, @Vlr_Piso_Profissional, @Des_Cursos, @Des_Competencias, @Flg_Inativo, @Dta_Cadastro, @Des_Funcao_Pesquisa, @Cod_Funcao_Folha, @Cod_CBO, @Flg_Conselho, @Flg_Curso_Especializacao, @Flg_RAIS_Analfabeto, @Flg_RAIS_Nivel_Superior, @Flg_Categoria_Sindical, @Flg_Auditada, @Qtd_Carga_Horaria_Diaria, @Des_Local_Trabalho, @Des_EPI, @Des_PPRA, @Des_PCMSO, @Des_Equipamentos);";
		private const string SPUPDATE = "UPDATE plataforma.TAB_Funcao SET Idf_Funcao_Agrupadora = @Idf_Funcao_Agrupadora, Des_Funcao = @Des_Funcao, Idf_Funcao_Categoria = @Idf_Funcao_Categoria, Idf_Classe_Salarial = @Idf_Classe_Salarial, Idf_Escolaridade = @Idf_Escolaridade, Idf_Area_BNE = @Idf_Area_BNE, Flg_Menor_Aprendiz = @Flg_Menor_Aprendiz, Des_Job = @Des_Job, Des_Experiencia = @Des_Experiencia, Vlr_Piso_Profissional = @Vlr_Piso_Profissional, Des_Cursos = @Des_Cursos, Des_Competencias = @Des_Competencias, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro, Des_Funcao_Pesquisa = @Des_Funcao_Pesquisa, Cod_Funcao_Folha = @Cod_Funcao_Folha, Cod_CBO = @Cod_CBO, Flg_Conselho = @Flg_Conselho, Flg_Curso_Especializacao = @Flg_Curso_Especializacao, Flg_RAIS_Analfabeto = @Flg_RAIS_Analfabeto, Flg_RAIS_Nivel_Superior = @Flg_RAIS_Nivel_Superior, Flg_Categoria_Sindical = @Flg_Categoria_Sindical, Flg_Auditada = @Flg_Auditada, Qtd_Carga_Horaria_Diaria = @Qtd_Carga_Horaria_Diaria, Des_Local_Trabalho = @Des_Local_Trabalho, Des_EPI = @Des_EPI, Des_PPRA = @Des_PPRA, Des_PCMSO = @Des_PCMSO, Des_Equipamentos = @Des_Equipamentos WHERE Idf_Funcao = @Idf_Funcao";
		private const string SPDELETE = "DELETE FROM plataforma.TAB_Funcao WHERE Idf_Funcao = @Idf_Funcao";
		private const string SPSELECTID = "SELECT * FROM plataforma.TAB_Funcao WHERE Idf_Funcao = @Idf_Funcao";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Funcao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Funcao_Agrupadora", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Funcao", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Idf_Funcao_Categoria", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Classe_Salarial", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Escolaridade", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Area_BNE", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Flg_Menor_Aprendiz", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Des_Job", SqlDbType.VarChar, 2000));
			parms.Add(new SqlParameter("@Des_Experiencia", SqlDbType.VarChar, 2000));
			parms.Add(new SqlParameter("@Vlr_Piso_Profissional", SqlDbType.Money, 8));
			parms.Add(new SqlParameter("@Des_Cursos", SqlDbType.VarChar, 2000));
			parms.Add(new SqlParameter("@Des_Competencias", SqlDbType.VarChar, 2000));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Des_Funcao_Pesquisa", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Cod_Funcao_Folha", SqlDbType.Char, 10));
			parms.Add(new SqlParameter("@Cod_CBO", SqlDbType.Char, 6));
			parms.Add(new SqlParameter("@Flg_Conselho", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Flg_Curso_Especializacao", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Flg_RAIS_Analfabeto", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Flg_RAIS_Nivel_Superior", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Flg_Categoria_Sindical", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Flg_Auditada", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Qtd_Carga_Horaria_Diaria", SqlDbType.Int, 2));
			parms.Add(new SqlParameter("@Des_Local_Trabalho", SqlDbType.VarChar, 2000));
			parms.Add(new SqlParameter("@Des_EPI", SqlDbType.VarChar, 2000));
			parms.Add(new SqlParameter("@Des_PPRA", SqlDbType.VarChar, 2000));
			parms.Add(new SqlParameter("@Des_PCMSO", SqlDbType.VarChar, 2000));
			parms.Add(new SqlParameter("@Des_Equipamentos", SqlDbType.VarChar, 2000));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Vinicius Maciel</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idFuncao;

			if (this._funcaoAgrupadora != null)
				parms[1].Value = this._funcaoAgrupadora.IdFuncao;
			else
				parms[1].Value = DBNull.Value;

			parms[2].Value = this._descricaoFuncao;
			parms[3].Value = this._funcaoCategoria.IdFuncaoCategoria;

			if (this._classeSalarial != null)
				parms[4].Value = this._classeSalarial.IdClasseSalarial;
			else
				parms[4].Value = DBNull.Value;


			if (this._escolaridade != null)
				parms[5].Value = this._escolaridade.IdEscolaridade;
			else
				parms[5].Value = DBNull.Value;


			if (this._areaBNE != null)
				parms[6].Value = this._areaBNE.IdAreaBNE;
			else
				parms[6].Value = DBNull.Value;


			if (this._flagMenorAprendiz.HasValue)
				parms[7].Value = this._flagMenorAprendiz;
			else
				parms[7].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoJob))
				parms[8].Value = this._descricaoJob;
			else
				parms[8].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoExperiencia))
				parms[9].Value = this._descricaoExperiencia;
			else
				parms[9].Value = DBNull.Value;


			if (this._valorPisoProfissional.HasValue)
				parms[10].Value = this._valorPisoProfissional;
			else
				parms[10].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoCursos))
				parms[11].Value = this._descricaoCursos;
			else
				parms[11].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoCompetencias))
				parms[12].Value = this._descricaoCompetencias;
			else
				parms[12].Value = DBNull.Value;

			parms[13].Value = this._flagInativo;
			parms[15].Value = this._descricaoFuncaoPesquisa;

			if (!String.IsNullOrEmpty(this._codigoFuncaoFolha))
				parms[16].Value = this._codigoFuncaoFolha;
			else
				parms[16].Value = DBNull.Value;

			parms[17].Value = this._codigoCBO;
			parms[18].Value = this._flagConselho;
			parms[19].Value = this._flagCursoEspecializacao;

			if (this._flagRAISAnalfabeto.HasValue)
				parms[20].Value = this._flagRAISAnalfabeto;
			else
				parms[20].Value = DBNull.Value;


			if (this._flagRAISNivelSuperior.HasValue)
				parms[21].Value = this._flagRAISNivelSuperior;
			else
				parms[21].Value = DBNull.Value;

			parms[22].Value = this._flagCategoriaSindical;
			parms[23].Value = this._flagAuditada;

			if (this._quantidadeCargaHorariaDiaria.HasValue)
				parms[24].Value = this._quantidadeCargaHorariaDiaria;
			else
				parms[24].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoLocalTrabalho))
				parms[25].Value = this._descricaoLocalTrabalho;
			else
				parms[25].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoEPI))
				parms[26].Value = this._descricaoEPI;
			else
				parms[26].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoPPRA))
				parms[27].Value = this._descricaoPPRA;
			else
				parms[27].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoPCMSO))
				parms[28].Value = this._descricaoPCMSO;
			else
				parms[28].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoEquipamentos))
				parms[29].Value = this._descricaoEquipamentos;
			else
				parms[29].Value = DBNull.Value;


			if (!this._persisted)
			{
				this._dataCadastro = DateTime.Now;
			}
			parms[14].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de Funcao no banco de dados.
		/// </summary>
		/// <remarks>Vinicius Maciel</remarks>
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
		/// Método utilizado para inserir uma instância de Funcao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Vinicius Maciel</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de Funcao no banco de dados.
		/// </summary>
		/// <remarks>Vinicius Maciel</remarks>
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
		/// Método utilizado para atualizar uma instância de Funcao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Vinicius Maciel</remarks>
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
		/// Método utilizado para salvar uma instância de Funcao no banco de dados.
		/// </summary>
		/// <remarks>Vinicius Maciel</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de Funcao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Vinicius Maciel</remarks>
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
		/// Método utilizado para excluir uma instância de Funcao no banco de dados.
		/// </summary>
		/// <param name="idFuncao">Chave do registro.</param>
		/// <remarks>Vinicius Maciel</remarks>
		public static void Delete(int idFuncao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Funcao", SqlDbType.Int, 4));

			parms[0].Value = idFuncao;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de Funcao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idFuncao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Vinicius Maciel</remarks>
		public static void Delete(int idFuncao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Funcao", SqlDbType.Int, 4));

			parms[0].Value = idFuncao;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de Funcao no banco de dados.
		/// </summary>
		/// <param name="idFuncao">Lista de chaves.</param>
		/// <remarks>Vinicius Maciel</remarks>
		public static void Delete(List<int> idFuncao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from plataforma.TAB_Funcao where Idf_Funcao in (";

			for (int i = 0; i < idFuncao.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idFuncao[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idFuncao">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		private static IDataReader LoadDataReader(int idFuncao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Funcao", SqlDbType.Int, 4));

			parms[0].Value = idFuncao;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idFuncao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		private static IDataReader LoadDataReader(int idFuncao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Funcao", SqlDbType.Int, 4));

			parms[0].Value = idFuncao;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Fun.Idf_Funcao, Fun.Idf_Funcao_Agrupadora, Fun.Des_Funcao, Fun.Idf_Funcao_Categoria, Fun.Idf_Classe_Salarial, Fun.Idf_Escolaridade, Fun.Idf_Area_BNE, Fun.Flg_Menor_Aprendiz, Fun.Des_Job, Fun.Des_Experiencia, Fun.Vlr_Piso_Profissional, Fun.Des_Cursos, Fun.Des_Competencias, Fun.Flg_Inativo, Fun.Dta_Cadastro, Fun.Des_Funcao_Pesquisa, Fun.Cod_Funcao_Folha, Fun.Cod_CBO, Fun.Flg_Conselho, Fun.Flg_Curso_Especializacao, Fun.Flg_RAIS_Analfabeto, Fun.Flg_RAIS_Nivel_Superior, Fun.Flg_Categoria_Sindical, Fun.Flg_Auditada, Fun.Qtd_Carga_Horaria_Diaria, Fun.Des_Local_Trabalho, Fun.Des_EPI, Fun.Des_PPRA, Fun.Des_PCMSO, Fun.Des_Equipamentos FROM plataforma.TAB_Funcao Fun";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region SetInstance
		/// <summary>
		/// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
		/// </summary>
		/// <param name="dr">Cursor de leitura do banco de dados.</param>
		/// <param name="objFuncao">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		private static bool SetInstance(IDataReader dr, Funcao objFuncao)
		{
			try
			{
				if (dr.Read())
				{
					objFuncao._idFuncao = Convert.ToInt32(dr["Idf_Funcao"]);
					if (dr["Idf_Funcao_Agrupadora"] != DBNull.Value)
						objFuncao._funcaoAgrupadora = new Funcao(Convert.ToInt32(dr["Idf_Funcao_Agrupadora"]));
					objFuncao._descricaoFuncao = Convert.ToString(dr["Des_Funcao"]);
					objFuncao._funcaoCategoria = new FuncaoCategoria(Convert.ToInt32(dr["Idf_Funcao_Categoria"]));
					if (dr["Idf_Classe_Salarial"] != DBNull.Value)
						objFuncao._classeSalarial = new ClasseSalarial(Convert.ToInt32(dr["Idf_Classe_Salarial"]));
					if (dr["Idf_Escolaridade"] != DBNull.Value)
						objFuncao._escolaridade = new Escolaridade(Convert.ToInt32(dr["Idf_Escolaridade"]));
					if (dr["Idf_Area_BNE"] != DBNull.Value)
						objFuncao._areaBNE = new AreaBNE(Convert.ToInt32(dr["Idf_Area_BNE"]));
					if (dr["Flg_Menor_Aprendiz"] != DBNull.Value)
						objFuncao._flagMenorAprendiz = Convert.ToBoolean(dr["Flg_Menor_Aprendiz"]);
					if (dr["Des_Job"] != DBNull.Value)
						objFuncao._descricaoJob = Convert.ToString(dr["Des_Job"]);
					if (dr["Des_Experiencia"] != DBNull.Value)
						objFuncao._descricaoExperiencia = Convert.ToString(dr["Des_Experiencia"]);
					if (dr["Vlr_Piso_Profissional"] != DBNull.Value)
						objFuncao._valorPisoProfissional = Convert.ToDecimal(dr["Vlr_Piso_Profissional"]);
					if (dr["Des_Cursos"] != DBNull.Value)
						objFuncao._descricaoCursos = Convert.ToString(dr["Des_Cursos"]);
					if (dr["Des_Competencias"] != DBNull.Value)
						objFuncao._descricaoCompetencias = Convert.ToString(dr["Des_Competencias"]);
					objFuncao._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objFuncao._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objFuncao._descricaoFuncaoPesquisa = Convert.ToString(dr["Des_Funcao_Pesquisa"]);
					if (dr["Cod_Funcao_Folha"] != DBNull.Value)
						objFuncao._codigoFuncaoFolha = Convert.ToString(dr["Cod_Funcao_Folha"]);
					objFuncao._codigoCBO = Convert.ToString(dr["Cod_CBO"]);
					objFuncao._flagConselho = Convert.ToBoolean(dr["Flg_Conselho"]);
					objFuncao._flagCursoEspecializacao = Convert.ToBoolean(dr["Flg_Curso_Especializacao"]);
					if (dr["Flg_RAIS_Analfabeto"] != DBNull.Value)
						objFuncao._flagRAISAnalfabeto = Convert.ToBoolean(dr["Flg_RAIS_Analfabeto"]);
					if (dr["Flg_RAIS_Nivel_Superior"] != DBNull.Value)
						objFuncao._flagRAISNivelSuperior = Convert.ToBoolean(dr["Flg_RAIS_Nivel_Superior"]);
					objFuncao._flagCategoriaSindical = Convert.ToBoolean(dr["Flg_Categoria_Sindical"]);
					objFuncao._flagAuditada = Convert.ToBoolean(dr["Flg_Auditada"]);
					if (dr["Qtd_Carga_Horaria_Diaria"] != DBNull.Value)
						objFuncao._quantidadeCargaHorariaDiaria = Convert.ToInt16(dr["Qtd_Carga_Horaria_Diaria"]);
					if (dr["Des_Local_Trabalho"] != DBNull.Value)
						objFuncao._descricaoLocalTrabalho = Convert.ToString(dr["Des_Local_Trabalho"]);
					if (dr["Des_EPI"] != DBNull.Value)
						objFuncao._descricaoEPI = Convert.ToString(dr["Des_EPI"]);
					if (dr["Des_PPRA"] != DBNull.Value)
						objFuncao._descricaoPPRA = Convert.ToString(dr["Des_PPRA"]);
					if (dr["Des_PCMSO"] != DBNull.Value)
						objFuncao._descricaoPCMSO = Convert.ToString(dr["Des_PCMSO"]);
					if (dr["Des_Equipamentos"] != DBNull.Value)
						objFuncao._descricaoEquipamentos = Convert.ToString(dr["Des_Equipamentos"]);

					objFuncao._persisted = true;
					objFuncao._modified = false;

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

		#endregion
	}
}