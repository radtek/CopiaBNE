//-- Data: 19/07/2010 14:26
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class CursoFonte // Tabela: TAB_Curso_Fonte
	{
		#region Atributos
		private int _idCursoFonte;
		private Fonte _fonte;
		private Curso _curso;
		private string _descricaoCurso;
		private bool _flagManha;
		private bool _flagTarde;
		private bool _flagNoite;
		private Int64? _quantidadeCargaHoraria;
		private string _descricaoPagamento;
		private string _descricaoContato;
		private bool _flagInativo;
		private DateTime _dataCadastro;
		private DateTime _dataAtualizacao;
		private string _descricaoObs;
		private string _descricaoDuracao;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdCursoFonte
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdCursoFonte
		{
			get
			{
				return this._idCursoFonte;
			}
		}
		#endregion 

		#region Fonte
		/// <summary>
		/// Campo obrigatório.
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

		#region Curso
		/// <summary>
		/// Campo obrigatório.
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

		#region DescricaoCurso
		/// <summary>
		/// Tamanho do campo: 100.
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

		#region FlagManha
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagManha
		{
			get
			{
				return this._flagManha;
			}
			set
			{
				this._flagManha = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagTarde
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagTarde
		{
			get
			{
				return this._flagTarde;
			}
			set
			{
				this._flagTarde = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagNoite
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagNoite
		{
			get
			{
				return this._flagNoite;
			}
			set
			{
				this._flagNoite = value;
				this._modified = true;
			}
		}
		#endregion 

		#region QuantidadeCargaHoraria
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Int64? QuantidadeCargaHoraria
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

		#region DescricaoPagamento
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo opcional.
		/// </summary>
		public string DescricaoPagamento
		{
			get
			{
				return this._descricaoPagamento;
			}
			set
			{
				this._descricaoPagamento = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoContato
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo opcional.
		/// </summary>
		public string DescricaoContato
		{
			get
			{
				return this._descricaoContato;
			}
			set
			{
				this._descricaoContato = value;
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

		#region DataAtualizacao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public DateTime DataAtualizacao
		{
			get
			{
				return this._dataAtualizacao;
			}
			set
			{
				this._dataAtualizacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoObs
		/// <summary>
		/// Tamanho do campo: 1000.
		/// Campo opcional.
		/// </summary>
		public string DescricaoObs
		{
			get
			{
				return this._descricaoObs;
			}
			set
			{
				this._descricaoObs = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoDuracao
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string DescricaoDuracao
		{
			get
			{
				return this._descricaoDuracao;
			}
			set
			{
				this._descricaoDuracao = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public CursoFonte()
		{
		}
		public CursoFonte(int idCursoFonte)
		{
			this._idCursoFonte = idCursoFonte;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Curso_Fonte (Idf_Fonte, Idf_Curso, Des_Curso, Flg_Manha, Flg_Tarde, Flg_Noite, Qtd_Carga_Horaria, Des_Pagamento, Des_Contato, Flg_Inativo, Dta_Cadastro, Dta_Atualizacao, Des_Obs, Des_Duracao) VALUES (@Idf_Fonte, @Idf_Curso, @Des_Curso, @Flg_Manha, @Flg_Tarde, @Flg_Noite, @Qtd_Carga_Horaria, @Des_Pagamento, @Des_Contato, @Flg_Inativo, @Dta_Cadastro, @Dta_Atualizacao, @Des_Obs, @Des_Duracao);SET @Idf_Curso_Fonte = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE TAB_Curso_Fonte SET Idf_Fonte = @Idf_Fonte, Idf_Curso = @Idf_Curso, Des_Curso = @Des_Curso, Flg_Manha = @Flg_Manha, Flg_Tarde = @Flg_Tarde, Flg_Noite = @Flg_Noite, Qtd_Carga_Horaria = @Qtd_Carga_Horaria, Des_Pagamento = @Des_Pagamento, Des_Contato = @Des_Contato, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro, Dta_Atualizacao = @Dta_Atualizacao, Des_Obs = @Des_Obs, Des_Duracao = @Des_Duracao WHERE Idf_Curso_Fonte = @Idf_Curso_Fonte";
		private const string SPDELETE = "DELETE FROM TAB_Curso_Fonte WHERE Idf_Curso_Fonte = @Idf_Curso_Fonte";
		private const string SPSELECTID = "SELECT * FROM TAB_Curso_Fonte WHERE Idf_Curso_Fonte = @Idf_Curso_Fonte";
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
			parms.Add(new SqlParameter("@Idf_Curso_Fonte", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Fonte", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curso", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Curso", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Flg_Manha", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Flg_Tarde", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Flg_Noite", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Qtd_Carga_Horaria", SqlDbType.BigInt, 8));
			parms.Add(new SqlParameter("@Des_Pagamento", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Des_Contato", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Dta_Atualizacao", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Des_Obs", SqlDbType.VarChar, 1000));
			parms.Add(new SqlParameter("@Des_Duracao", SqlDbType.VarChar, 100));
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
			parms[0].Value = this._idCursoFonte;
			parms[1].Value = this._fonte.IdFonte;
			parms[2].Value = this._curso.IdCurso;

			if (!String.IsNullOrEmpty(this._descricaoCurso))
				parms[3].Value = this._descricaoCurso;
			else
				parms[3].Value = DBNull.Value;

			parms[4].Value = this._flagManha;
			parms[5].Value = this._flagTarde;
			parms[6].Value = this._flagNoite;

			if (this._quantidadeCargaHoraria.HasValue)
				parms[7].Value = this._quantidadeCargaHoraria;
			else
				parms[7].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoPagamento))
				parms[8].Value = this._descricaoPagamento;
			else
				parms[8].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoContato))
				parms[9].Value = this._descricaoContato;
			else
				parms[9].Value = DBNull.Value;

			parms[10].Value = this._flagInativo;
            parms[12].Value = DateTime.Now;

			if (!String.IsNullOrEmpty(this._descricaoObs))
				parms[13].Value = this._descricaoObs;
			else
				parms[13].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoDuracao))
				parms[14].Value = this._descricaoDuracao;
			else
				parms[14].Value = DBNull.Value;


			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[11].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de CursoFonte no banco de dados.
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
						this._idCursoFonte = Convert.ToInt32(cmd.Parameters["@Idf_Curso_Fonte"].Value);
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
		/// Método utilizado para inserir uma instância de CursoFonte no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idCursoFonte = Convert.ToInt32(cmd.Parameters["@Idf_Curso_Fonte"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de CursoFonte no banco de dados.
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
		/// Método utilizado para atualizar uma instância de CursoFonte no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de CursoFonte no banco de dados.
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
		/// Método utilizado para salvar uma instância de CursoFonte no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de CursoFonte no banco de dados.
		/// </summary>
		/// <param name="idCursoFonte">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCursoFonte)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curso_Fonte", SqlDbType.Int, 4));

			parms[0].Value = idCursoFonte;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de CursoFonte no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCursoFonte">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCursoFonte, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curso_Fonte", SqlDbType.Int, 4));

			parms[0].Value = idCursoFonte;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de CursoFonte no banco de dados.
		/// </summary>
		/// <param name="idCursoFonte">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idCursoFonte)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from TAB_Curso_Fonte where Idf_Curso_Fonte in (";

			for (int i = 0; i < idCursoFonte.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idCursoFonte[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCursoFonte">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCursoFonte)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curso_Fonte", SqlDbType.Int, 4));

			parms[0].Value = idCursoFonte;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCursoFonte">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCursoFonte, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curso_Fonte", SqlDbType.Int, 4));

			parms[0].Value = idCursoFonte;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cur.Idf_Curso_Fonte, Cur.Idf_Fonte, Cur.Idf_Curso, Cur.Des_Curso, Cur.Flg_Manha, Cur.Flg_Tarde, Cur.Flg_Noite, Cur.Qtd_Carga_Horaria, Cur.Des_Pagamento, Cur.Des_Contato, Cur.Flg_Inativo, Cur.Dta_Cadastro, Cur.Dta_Atualizacao, Cur.Des_Obs, Cur.Des_Duracao FROM TAB_Curso_Fonte Cur";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de CursoFonte a partir do banco de dados.
		/// </summary>
		/// <param name="idCursoFonte">Chave do registro.</param>
		/// <returns>Instância de CursoFonte.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CursoFonte LoadObject(int idCursoFonte)
		{
			using (IDataReader dr = LoadDataReader(idCursoFonte))
			{
				CursoFonte objCursoFonte = new CursoFonte();
				if (SetInstance(dr, objCursoFonte))
					return objCursoFonte;
			}
			throw (new RecordNotFoundException(typeof(CursoFonte)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de CursoFonte a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCursoFonte">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de CursoFonte.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CursoFonte LoadObject(int idCursoFonte, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCursoFonte, trans))
			{
				CursoFonte objCursoFonte = new CursoFonte();
				if (SetInstance(dr, objCursoFonte))
					return objCursoFonte;
			}
			throw (new RecordNotFoundException(typeof(CursoFonte)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de CursoFonte a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idCursoFonte))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de CursoFonte a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idCursoFonte, trans))
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
		/// <param name="objCursoFonte">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, CursoFonte objCursoFonte)
		{
			try
			{
				if (dr.Read())
				{
					objCursoFonte._idCursoFonte = Convert.ToInt32(dr["Idf_Curso_Fonte"]);
					objCursoFonte._fonte = new Fonte(Convert.ToInt32(dr["Idf_Fonte"]));
					objCursoFonte._curso = new Curso(Convert.ToInt32(dr["Idf_Curso"]));
					if (dr["Des_Curso"] != DBNull.Value)
						objCursoFonte._descricaoCurso = Convert.ToString(dr["Des_Curso"]);
					objCursoFonte._flagManha = Convert.ToBoolean(dr["Flg_Manha"]);
					objCursoFonte._flagTarde = Convert.ToBoolean(dr["Flg_Tarde"]);
					objCursoFonte._flagNoite = Convert.ToBoolean(dr["Flg_Noite"]);
					if (dr["Qtd_Carga_Horaria"] != DBNull.Value)
						objCursoFonte._quantidadeCargaHoraria = Convert.ToInt64(dr["Qtd_Carga_Horaria"]);
					if (dr["Des_Pagamento"] != DBNull.Value)
						objCursoFonte._descricaoPagamento = Convert.ToString(dr["Des_Pagamento"]);
					if (dr["Des_Contato"] != DBNull.Value)
						objCursoFonte._descricaoContato = Convert.ToString(dr["Des_Contato"]);
					objCursoFonte._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objCursoFonte._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objCursoFonte._dataAtualizacao = Convert.ToDateTime(dr["Dta_Atualizacao"]);
					if (dr["Des_Obs"] != DBNull.Value)
						objCursoFonte._descricaoObs = Convert.ToString(dr["Des_Obs"]);
					if (dr["Des_Duracao"] != DBNull.Value)
						objCursoFonte._descricaoDuracao = Convert.ToString(dr["Des_Duracao"]);

					objCursoFonte._persisted = true;
					objCursoFonte._modified = false;

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