//-- Data: 08/07/2010 12:31
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class Formacao // Tabela: BNE_Formacao
	{
		#region Atributos
		private int _idFormacao;
		private PessoaFisica _pessoaFisica;
		private Escolaridade _escolaridade;
		private Curso _curso;
		private Fonte _fonte;
		private Int16? _anoConclusao;
		private Int16? _quantidadeCargaHoraria;
		private Int16? _numeroPeriodo;
		private SituacaoFormacao _situacaoFormacao;
		private DateTime? _dataAlteracao;
		private DateTime _dataCadastro;
		private bool _flagInativo;
		private bool _flagNacional;
		private string _descricaoEndereco;
		private string _descricaoCurso;
		private Cidade _cidade;
		private string _descricaoFonte;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdFormacao
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdFormacao
		{
			get
			{
				return this._idFormacao;
			}
		}
		#endregion 

		#region PessoaFisica
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public PessoaFisica PessoaFisica
		{
			get
			{
				return this._pessoaFisica;
			}
			set
			{
				this._pessoaFisica = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Escolaridade
		/// <summary>
		/// Campo obrigatório.
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

		#region AnoConclusao
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Int16? AnoConclusao
		{
			get
			{
				return this._anoConclusao;
			}
			set
			{
				this._anoConclusao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region QuantidadeCargaHoraria
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Int16? QuantidadeCargaHoraria
		{
			get
			{
				return this._quantidadeCargaHoraria;
			}
			set
			{
				this._quantidadeCargaHoraria = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroPeriodo
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Int16? NumeroPeriodo
		{
			get
			{
				return this._numeroPeriodo;
			}
			set
			{
				this._numeroPeriodo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region SituacaoFormacao
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public SituacaoFormacao SituacaoFormacao
		{
			get
			{
				return this._situacaoFormacao;
			}
			set
			{
				this._situacaoFormacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DataAlteracao
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DataAlteracao
		{
			get
			{
				return this._dataAlteracao;
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

		#region FlagNacional
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagNacional
		{
			get
			{
				return this._flagNacional;
			}
			set
			{
				this._flagNacional = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoEndereco
		/// <summary>
		/// Tamanho do campo: 300.
		/// Campo opcional.
		/// </summary>
		public string DescricaoEndereco
		{
			get
			{
				return this._descricaoEndereco;
			}
			set
			{
				this._descricaoEndereco = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoCurso
		/// <summary>
		/// Tamanho do campo: 200.
		/// Campo opcional.
		/// </summary>
		public string DescricaoCurso
		{
			get
			{
				return this._descricaoCurso;
			}
			set
			{
				this._descricaoCurso = value;
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

		#region DescricaoFonte
		/// <summary>
		/// Tamanho do campo: 200.
		/// Campo opcional.
		/// </summary>
		public string DescricaoFonte
		{
			get
			{
				return this._descricaoFonte;
			}
			set
			{
				this._descricaoFonte = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public Formacao()
		{
		}
		public Formacao(int idFormacao)
		{
			this._idFormacao = idFormacao;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Formacao (Idf_Pessoa_Fisica, Idf_Escolaridade, Idf_Curso, Idf_Fonte, Ano_Conclusao, Qtd_Carga_Horaria, Num_Periodo, Idf_Situacao_Formacao, Dta_Alteracao, Dta_Cadastro, Flg_Inativo, Flg_Nacional, Des_Endereco, Des_Curso, Idf_Cidade, Des_Fonte) VALUES (@Idf_Pessoa_Fisica, @Idf_Escolaridade, @Idf_Curso, @Idf_Fonte, @Ano_Conclusao, @Qtd_Carga_Horaria, @Num_Periodo, @Idf_Situacao_Formacao, @Dta_Alteracao, @Dta_Cadastro, @Flg_Inativo, @Flg_Nacional, @Des_Endereco, @Des_Curso, @Idf_Cidade, @Des_Fonte);SET @Idf_Formacao = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Formacao SET Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica, Idf_Escolaridade = @Idf_Escolaridade, Idf_Curso = @Idf_Curso, Idf_Fonte = @Idf_Fonte, Ano_Conclusao = @Ano_Conclusao, Qtd_Carga_Horaria = @Qtd_Carga_Horaria, Num_Periodo = @Num_Periodo, Idf_Situacao_Formacao = @Idf_Situacao_Formacao, Dta_Alteracao = @Dta_Alteracao, Dta_Cadastro = @Dta_Cadastro, Flg_Inativo = @Flg_Inativo, Flg_Nacional = @Flg_Nacional, Des_Endereco = @Des_Endereco, Des_Curso = @Des_Curso, Idf_Cidade = @Idf_Cidade, Des_Fonte = @Des_Fonte WHERE Idf_Formacao = @Idf_Formacao";
		private const string SPDELETE = "DELETE FROM BNE_Formacao WHERE Idf_Formacao = @Idf_Formacao";
		private const string SPSELECTID = "SELECT * FROM BNE_Formacao WHERE Idf_Formacao = @Idf_Formacao";
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
			parms.Add(new SqlParameter("@Idf_Formacao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Escolaridade", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curso", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Fonte", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Ano_Conclusao", SqlDbType.Int, 2));
			parms.Add(new SqlParameter("@Qtd_Carga_Horaria", SqlDbType.Int, 2));
			parms.Add(new SqlParameter("@Num_Periodo", SqlDbType.Int, 2));
			parms.Add(new SqlParameter("@Idf_Situacao_Formacao", SqlDbType.Int, 2));
			parms.Add(new SqlParameter("@Dta_Alteracao", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Flg_Nacional", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Des_Endereco", SqlDbType.VarChar, 300));
			parms.Add(new SqlParameter("@Des_Curso", SqlDbType.VarChar, 200));
			parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Fonte", SqlDbType.VarChar, 200));
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
			parms[0].Value = this._idFormacao;
			parms[1].Value = this._pessoaFisica.IdPessoaFisica;
			parms[2].Value = this._escolaridade.IdEscolaridade;

			if (this._curso != null)
				parms[3].Value = this._curso.IdCurso;
			else
				parms[3].Value = DBNull.Value;


			if (this._fonte != null)
				parms[4].Value = this._fonte.IdFonte;
			else
				parms[4].Value = DBNull.Value;


			if (this._anoConclusao.HasValue)
				parms[5].Value = this._anoConclusao;
			else
				parms[5].Value = DBNull.Value;


			if (this._quantidadeCargaHoraria.HasValue)
				parms[6].Value = this._quantidadeCargaHoraria;
			else
				parms[6].Value = DBNull.Value;


			if (this._numeroPeriodo.HasValue)
				parms[7].Value = this._numeroPeriodo;
			else
				parms[7].Value = DBNull.Value;


			if (this._situacaoFormacao != null)
				parms[8].Value = this._situacaoFormacao.IdSituacaoFormacao;
			else
				parms[8].Value = DBNull.Value;

			parms[11].Value = this._flagInativo;
			parms[12].Value = this._flagNacional;

			if (!String.IsNullOrEmpty(this._descricaoEndereco))
				parms[13].Value = this._descricaoEndereco;
			else
				parms[13].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoCurso))
				parms[14].Value = this._descricaoCurso;
			else
				parms[14].Value = DBNull.Value;


			if (this._cidade != null)
				parms[15].Value = this._cidade.IdCidade;
			else
				parms[15].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoFonte))
				parms[16].Value = this._descricaoFonte;
			else
				parms[16].Value = DBNull.Value;


			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			this._dataAlteracao = DateTime.Now;
			parms[9].Value = this._dataAlteracao;
			parms[10].Value = this._dataCadastro;
		}
		#endregion

		#region Save
		/// <summary>
		/// Método utilizado para salvar uma instância de Formacao no banco de dados.
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
		/// Método utilizado para salvar uma instância de Formacao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de Formacao no banco de dados.
		/// </summary>
		/// <param name="idFormacao">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idFormacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Formacao", SqlDbType.Int, 4));

			parms[0].Value = idFormacao;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de Formacao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idFormacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idFormacao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Formacao", SqlDbType.Int, 4));

			parms[0].Value = idFormacao;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de Formacao no banco de dados.
		/// </summary>
		/// <param name="idFormacao">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idFormacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Formacao where Idf_Formacao in (";

			for (int i = 0; i < idFormacao.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idFormacao[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idFormacao">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idFormacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Formacao", SqlDbType.Int, 4));

			parms[0].Value = idFormacao;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idFormacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idFormacao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Formacao", SqlDbType.Int, 4));

			parms[0].Value = idFormacao;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, For.Idf_Formacao, For.Idf_Pessoa_Fisica, For.Idf_Escolaridade, For.Idf_Curso, For.Idf_Fonte, For.Ano_Conclusao, For.Qtd_Carga_Horaria, For.Num_Periodo, For.Idf_Situacao_Formacao, For.Dta_Alteracao, For.Dta_Cadastro, For.Flg_Inativo, For.Flg_Nacional, For.Des_Endereco, For.Des_Curso, For.Idf_Cidade, For.Des_Fonte FROM BNE_Formacao For";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de Formacao a partir do banco de dados.
		/// </summary>
		/// <param name="idFormacao">Chave do registro.</param>
		/// <returns>Instância de Formacao.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static Formacao LoadObject(int idFormacao)
		{
			using (IDataReader dr = LoadDataReader(idFormacao))
			{
				Formacao objFormacao = new Formacao();
				if (SetInstance(dr, objFormacao))
					return objFormacao;
			}
			throw (new RecordNotFoundException(typeof(Formacao)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de Formacao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idFormacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de Formacao.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static Formacao LoadObject(int idFormacao, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idFormacao, trans))
			{
				Formacao objFormacao = new Formacao();
				if (SetInstance(dr, objFormacao))
					return objFormacao;
			}
			throw (new RecordNotFoundException(typeof(Formacao)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de Formacao a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idFormacao))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de Formacao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idFormacao, trans))
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
		/// <param name="objFormacao">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, Formacao objFormacao)
		{
			try
			{
				if (dr.Read())
				{
					objFormacao._idFormacao = Convert.ToInt32(dr["Idf_Formacao"]);
					objFormacao._pessoaFisica = new PessoaFisica(Convert.ToInt32(dr["Idf_Pessoa_Fisica"]));
					objFormacao._escolaridade = new Escolaridade(Convert.ToInt32(dr["Idf_Escolaridade"]));
					if (dr["Idf_Curso"] != DBNull.Value)
						objFormacao._curso = new Curso(Convert.ToInt32(dr["Idf_Curso"]));
					if (dr["Idf_Fonte"] != DBNull.Value)
						objFormacao._fonte = new Fonte(Convert.ToInt32(dr["Idf_Fonte"]));
					if (dr["Ano_Conclusao"] != DBNull.Value)
						objFormacao._anoConclusao = Convert.ToInt16(dr["Ano_Conclusao"]);
					if (dr["Qtd_Carga_Horaria"] != DBNull.Value)
						objFormacao._quantidadeCargaHoraria = Convert.ToInt16(dr["Qtd_Carga_Horaria"]);
					if (dr["Num_Periodo"] != DBNull.Value)
						objFormacao._numeroPeriodo = Convert.ToInt16(dr["Num_Periodo"]);
					if (dr["Idf_Situacao_Formacao"] != DBNull.Value)
						objFormacao._situacaoFormacao = new SituacaoFormacao(Convert.ToInt16(dr["Idf_Situacao_Formacao"]));
					if (dr["Dta_Alteracao"] != DBNull.Value)
						objFormacao._dataAlteracao = Convert.ToDateTime(dr["Dta_Alteracao"]);
					objFormacao._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objFormacao._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objFormacao._flagNacional = Convert.ToBoolean(dr["Flg_Nacional"]);
					if (dr["Des_Endereco"] != DBNull.Value)
						objFormacao._descricaoEndereco = Convert.ToString(dr["Des_Endereco"]);
					if (dr["Des_Curso"] != DBNull.Value)
						objFormacao._descricaoCurso = Convert.ToString(dr["Des_Curso"]);
					if (dr["Idf_Cidade"] != DBNull.Value)
						objFormacao._cidade = new Cidade(Convert.ToInt32(dr["Idf_Cidade"]));
					if (dr["Des_Fonte"] != DBNull.Value)
						objFormacao._descricaoFonte = Convert.ToString(dr["Des_Fonte"]);

					objFormacao._persisted = true;
					objFormacao._modified = false;

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