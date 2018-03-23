//-- Data: 03/08/2016 17:54
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL.Notificacao
{
	public partial class LogErroJornal // Tabela: alerta.log_erro_jornal
	{
		#region Atributos
		private DateTime? _dataCadastro;
		private string _IdfsVagas;
		private string _IdfsCvs;
		private string _descricaoMensagem;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

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

		#region IdfsVagas
		/// <summary>
		/// Tamanho do campo: -1.
		/// Campo opcional.
		/// </summary>
		public string IdfsVagas
		{
			get
			{
				return this._IdfsVagas;
			}
			set
			{
				this._IdfsVagas = value;
				this._modified = true;
			}
		}
		#endregion 

		#region IdfsCvs
		/// <summary>
		/// Tamanho do campo: -1.
		/// Campo opcional.
		/// </summary>
		public string IdfsCvs
		{
			get
			{
				return this._IdfsCvs;
			}
			set
			{
				this._IdfsCvs = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoMensagem
		/// <summary>
		/// Tamanho do campo: -1.
		/// Campo opcional.
		/// </summary>
		public string DescricaoMensagem
		{
			get
			{
				return this._descricaoMensagem;
			}
			set
			{
				this._descricaoMensagem = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO alerta.log_erro_jornal (Dta_Cadastro, Idfs_Vagas, Idfs_Cvs, Des_Mensagem) VALUES (@Dta_Cadastro, @Idfs_Vagas, @Idfs_Cvs, @Des_Mensagem);";
		private const string SPUPDATE = "UPDATE alerta.log_erro_jornal SET Dta_Cadastro = @Dta_Cadastro, Idfs_Vagas = @Idfs_Vagas, Idfs_Cvs = @Idfs_Cvs, Des_Mensagem = @Des_Mensagem";
		private const string SPDELETE = "DELETE FROM alerta.log_erro_jornal";
		private const string SPSELECTID = "SELECT * FROM alerta.log_erro_jornal WITH(NOLOCK) ";
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
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Idfs_Vagas", SqlDbType.VarChar));
			parms.Add(new SqlParameter("@Idfs_Cvs", SqlDbType.VarChar));
			parms.Add(new SqlParameter("@Des_Mensagem", SqlDbType.VarChar));
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

			if (!String.IsNullOrEmpty(this._IdfsVagas))
				parms[1].Value = this._IdfsVagas;
			else
				parms[1].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._IdfsCvs))
				parms[2].Value = this._IdfsCvs;
			else
				parms[2].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoMensagem))
				parms[3].Value = this._descricaoMensagem;
			else
				parms[3].Value = DBNull.Value;


			if (!this._persisted)
			{
				this._dataCadastro = DateTime.Now;
			}
			parms[0].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de LogErroJornal no banco de dados.
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
		/// Método utilizado para inserir uma instância de LogErroJornal no banco de dados, dentro de uma transação.
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
		/// Método utilizado para atualizar uma instância de LogErroJornal no banco de dados.
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
		/// Método utilizado para atualizar uma instância de LogErroJornal no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de LogErroJornal no banco de dados.
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
		/// Método utilizado para salvar uma instância de LogErroJornal no banco de dados, dentro de uma transação.
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

		#region SetInstance
		/// <summary>
		/// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
		/// </summary>
		/// <param name="dr">Cursor de leitura do banco de dados.</param>
		/// <param name="objLogErroJornal">Instância a ser manipulada.</param>
		/// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, LogErroJornal objLogErroJornal, bool dispose = true)
		{
			try
			{
				if (dr.Read())
				{
					if (dr["Dta_Cadastro"] != DBNull.Value)
						objLogErroJornal._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					if (dr["Idfs_Vagas"] != DBNull.Value)
						objLogErroJornal._IdfsVagas = Convert.ToString(dr["Idfs_Vagas"]);
					if (dr["Idfs_Cvs"] != DBNull.Value)
						objLogErroJornal._IdfsCvs = Convert.ToString(dr["Idfs_Cvs"]);
					if (dr["Des_Mensagem"] != DBNull.Value)
						objLogErroJornal._descricaoMensagem = Convert.ToString(dr["Des_Mensagem"]);

					objLogErroJornal._persisted = true;
					objLogErroJornal._modified = false;

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