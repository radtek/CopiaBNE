//-- Data: 04/04/2013 15:26
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class TipoVinculoIntegracao // Tabela: plataforma.TAB_Tipo_Vinculo_Integracao
	{
		#region Atributos
		private int _idTipoVinculoIntegracao;
		private string _descricaoTipoVinculo;
		private bool _flagPrazoDeterminado;
		private Int16? _quantidadePrazoPadrao;
		private bool _flagPrazoVariavel;
		private bool _flagExperiencia;
		private bool _flagInativo;
		private DateTime _dataCadastro;
		private Int16? _codigoCategoriaTrabalhador;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdTipoVinculoIntegracao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdTipoVinculoIntegracao
		{
			get
			{
				return this._idTipoVinculoIntegracao;
			}
			set
			{
				this._idTipoVinculoIntegracao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoTipoVinculo
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoTipoVinculo
		{
			get
			{
				return this._descricaoTipoVinculo;
			}
			set
			{
				this._descricaoTipoVinculo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagPrazoDeterminado
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagPrazoDeterminado
		{
			get
			{
				return this._flagPrazoDeterminado;
			}
			set
			{
				this._flagPrazoDeterminado = value;
				this._modified = true;
			}
		}
		#endregion 

		#region QuantidadePrazoPadrao
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Int16? QuantidadePrazoPadrao
		{
			get
			{
				return this._quantidadePrazoPadrao;
			}
			set
			{
				this._quantidadePrazoPadrao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagPrazoVariavel
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagPrazoVariavel
		{
			get
			{
				return this._flagPrazoVariavel;
			}
			set
			{
				this._flagPrazoVariavel = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagExperiencia
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagExperiencia
		{
			get
			{
				return this._flagExperiencia;
			}
			set
			{
				this._flagExperiencia = value;
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

		#region CodigoCategoriaTrabalhador
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Int16? CodigoCategoriaTrabalhador
		{
			get
			{
				return this._codigoCategoriaTrabalhador;
			}
			set
			{
				this._codigoCategoriaTrabalhador = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public TipoVinculoIntegracao()
		{
		}
		public TipoVinculoIntegracao(int idTipoVinculoIntegracao)
		{
			this._idTipoVinculoIntegracao = idTipoVinculoIntegracao;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO plataforma.TAB_Tipo_Vinculo_Integracao (Idf_Tipo_Vinculo_Integracao, Des_Tipo_Vinculo, Flg_Prazo_Determinado, Qtd_Prazo_Padrao, Flg_Prazo_Variavel, Flg_Experiencia, Flg_Inativo, Dta_Cadastro, Cod_Categoria_Trabalhador) VALUES (@Idf_Tipo_Vinculo_Integracao, @Des_Tipo_Vinculo, @Flg_Prazo_Determinado, @Qtd_Prazo_Padrao, @Flg_Prazo_Variavel, @Flg_Experiencia, @Flg_Inativo, @Dta_Cadastro, @Cod_Categoria_Trabalhador);";
		private const string SPUPDATE = "UPDATE plataforma.TAB_Tipo_Vinculo_Integracao SET Des_Tipo_Vinculo = @Des_Tipo_Vinculo, Flg_Prazo_Determinado = @Flg_Prazo_Determinado, Qtd_Prazo_Padrao = @Qtd_Prazo_Padrao, Flg_Prazo_Variavel = @Flg_Prazo_Variavel, Flg_Experiencia = @Flg_Experiencia, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro, Cod_Categoria_Trabalhador = @Cod_Categoria_Trabalhador WHERE Idf_Tipo_Vinculo_Integracao = @Idf_Tipo_Vinculo_Integracao";
		private const string SPDELETE = "DELETE FROM plataforma.TAB_Tipo_Vinculo_Integracao WHERE Idf_Tipo_Vinculo_Integracao = @Idf_Tipo_Vinculo_Integracao";
		private const string SPSELECTID = "SELECT * FROM plataforma.TAB_Tipo_Vinculo_Integracao WITH(NOLOCK) WHERE Idf_Tipo_Vinculo_Integracao = @Idf_Tipo_Vinculo_Integracao";
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
			parms.Add(new SqlParameter("@Idf_Tipo_Vinculo_Integracao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Tipo_Vinculo", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Flg_Prazo_Determinado", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Qtd_Prazo_Padrao", SqlDbType.Int, 2));
			parms.Add(new SqlParameter("@Flg_Prazo_Variavel", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Flg_Experiencia", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Cod_Categoria_Trabalhador", SqlDbType.Int, 2));
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
			parms[0].Value = this._idTipoVinculoIntegracao;
			parms[1].Value = this._descricaoTipoVinculo;
			parms[2].Value = this._flagPrazoDeterminado;

			if (this._quantidadePrazoPadrao.HasValue)
				parms[3].Value = this._quantidadePrazoPadrao;
			else
				parms[3].Value = DBNull.Value;

			parms[4].Value = this._flagPrazoVariavel;
			parms[5].Value = this._flagExperiencia;
			parms[6].Value = this._flagInativo;

			if (this._codigoCategoriaTrabalhador.HasValue)
				parms[8].Value = this._codigoCategoriaTrabalhador;
			else
				parms[8].Value = DBNull.Value;


			if (!this._persisted)
			{
				this._dataCadastro = DateTime.Now;
			}
			parms[7].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de TipoVinculoIntegracao no banco de dados.
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
		/// Método utilizado para inserir uma instância de TipoVinculoIntegracao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
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
		/// Método utilizado para atualizar uma instância de TipoVinculoIntegracao no banco de dados.
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
		/// Método utilizado para atualizar uma instância de TipoVinculoIntegracao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de TipoVinculoIntegracao no banco de dados.
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
		/// Método utilizado para salvar uma instância de TipoVinculoIntegracao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de TipoVinculoIntegracao no banco de dados.
		/// </summary>
		/// <param name="idTipoVinculoIntegracao">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idTipoVinculoIntegracao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Vinculo_Integracao", SqlDbType.Int, 4));

			parms[0].Value = idTipoVinculoIntegracao;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de TipoVinculoIntegracao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTipoVinculoIntegracao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idTipoVinculoIntegracao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Vinculo_Integracao", SqlDbType.Int, 4));

			parms[0].Value = idTipoVinculoIntegracao;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de TipoVinculoIntegracao no banco de dados.
		/// </summary>
		/// <param name="idTipoVinculoIntegracao">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idTipoVinculoIntegracao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from plataforma.TAB_Tipo_Vinculo_Integracao where Idf_Tipo_Vinculo_Integracao in (";

			for (int i = 0; i < idTipoVinculoIntegracao.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idTipoVinculoIntegracao[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idTipoVinculoIntegracao">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idTipoVinculoIntegracao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Vinculo_Integracao", SqlDbType.Int, 4));

			parms[0].Value = idTipoVinculoIntegracao;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTipoVinculoIntegracao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idTipoVinculoIntegracao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Vinculo_Integracao", SqlDbType.Int, 4));

			parms[0].Value = idTipoVinculoIntegracao;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Tip.Idf_Tipo_Vinculo_Integracao, Tip.Des_Tipo_Vinculo, Tip.Flg_Prazo_Determinado, Tip.Qtd_Prazo_Padrao, Tip.Flg_Prazo_Variavel, Tip.Flg_Experiencia, Tip.Flg_Inativo, Tip.Dta_Cadastro, Tip.Cod_Categoria_Trabalhador FROM plataforma.TAB_Tipo_Vinculo_Integracao Tip";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de TipoVinculoIntegracao a partir do banco de dados.
		/// </summary>
		/// <param name="idTipoVinculoIntegracao">Chave do registro.</param>
		/// <returns>Instância de TipoVinculoIntegracao.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static TipoVinculoIntegracao LoadObject(int idTipoVinculoIntegracao)
		{
			using (IDataReader dr = LoadDataReader(idTipoVinculoIntegracao))
			{
				TipoVinculoIntegracao objTipoVinculoIntegracao = new TipoVinculoIntegracao();
				if (SetInstance(dr, objTipoVinculoIntegracao))
					return objTipoVinculoIntegracao;
			}
			throw (new RecordNotFoundException(typeof(TipoVinculoIntegracao)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de TipoVinculoIntegracao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTipoVinculoIntegracao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de TipoVinculoIntegracao.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static TipoVinculoIntegracao LoadObject(int idTipoVinculoIntegracao, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idTipoVinculoIntegracao, trans))
			{
				TipoVinculoIntegracao objTipoVinculoIntegracao = new TipoVinculoIntegracao();
				if (SetInstance(dr, objTipoVinculoIntegracao))
					return objTipoVinculoIntegracao;
			}
			throw (new RecordNotFoundException(typeof(TipoVinculoIntegracao)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de TipoVinculoIntegracao a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idTipoVinculoIntegracao))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de TipoVinculoIntegracao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idTipoVinculoIntegracao, trans))
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
		/// <param name="objTipoVinculoIntegracao">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, TipoVinculoIntegracao objTipoVinculoIntegracao)
		{
			try
			{
				if (dr.Read())
				{
					objTipoVinculoIntegracao._idTipoVinculoIntegracao = Convert.ToInt32(dr["Idf_Tipo_Vinculo_Integracao"]);
					objTipoVinculoIntegracao._descricaoTipoVinculo = Convert.ToString(dr["Des_Tipo_Vinculo"]);
					objTipoVinculoIntegracao._flagPrazoDeterminado = Convert.ToBoolean(dr["Flg_Prazo_Determinado"]);
					if (dr["Qtd_Prazo_Padrao"] != DBNull.Value)
						objTipoVinculoIntegracao._quantidadePrazoPadrao = Convert.ToInt16(dr["Qtd_Prazo_Padrao"]);
					objTipoVinculoIntegracao._flagPrazoVariavel = Convert.ToBoolean(dr["Flg_Prazo_Variavel"]);
					objTipoVinculoIntegracao._flagExperiencia = Convert.ToBoolean(dr["Flg_Experiencia"]);
					objTipoVinculoIntegracao._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objTipoVinculoIntegracao._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					if (dr["Cod_Categoria_Trabalhador"] != DBNull.Value)
						objTipoVinculoIntegracao._codigoCategoriaTrabalhador = Convert.ToInt16(dr["Cod_Categoria_Trabalhador"]);

					objTipoVinculoIntegracao._persisted = true;
					objTipoVinculoIntegracao._modified = false;

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