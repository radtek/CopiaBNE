using BNE.EL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BNE.BLL
{
    public partial class DeficienciaDetalhe
    {
  	    #region Atributos
        private int _idfDeficienciaDetalhe;
		private Deficiencia _deficiencia;
        private string _desDeficienciaDetalhe;
        private bool _flgInativo;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

        #region IdfDeficienciaDetalhe
		/// <summary>
		/// 
		/// Campo obrigatório.
		/// </summary>
		public int IdfDeficienciaDetalhe
		{
			get
			{
				return this._idfDeficienciaDetalhe;
			}
			set
			{
				this._idfDeficienciaDetalhe = value;
				this._modified = true;
			}
		}
		#endregion 

        #region Deficiencia
		/// <summary>
		/// 
		/// Campo obrigatório.
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

		#region DesDeficienciaDetalhe
		/// <summary>
		/// 
		/// Campo obrigatório.
		/// </summary>
		public string DesDeficienciaDetalhe
		{
			get
			{
				return this._desDeficienciaDetalhe;
			}
			set
			{
				this._desDeficienciaDetalhe = value;
				this._modified = true;
			}
		}
		#endregion 

        #region FlgInativo
		/// <summary>
		/// 
		/// Campo obrigatório.
		/// </summary>
		public bool FlgInativo
		{
			get
			{
				return this._flgInativo;
			}
			set
			{
				this._flgInativo = value;
				this._modified = true;
			}
		}
		#endregion 		

		#endregion

		#region Construtores
		public DeficienciaDetalhe()
		{
		}

        public DeficienciaDetalhe(int idDeficienciaDetalhe)
        {
            this._idfDeficienciaDetalhe = idDeficienciaDetalhe;
            this._persisted = true;
        }
		#endregion

		#region Consultas
        private const string SPINSERT = "insert into bne.bne_pessoa_fisica_deficiencia (idf_pessoa_fisica, idf_deficiencia) values(@idf_Pessoa_Fisica, @idf_Deficiencia);";
        private const string SPDELETE = "DELETE FROM BNE.BNE_Pessoa_Fisica_Deficiencia WHERE Idf_Deficiencia = @Idf_Deficiencia and idf_Pessoa_Fisica =  @idf_Pessoa_Fisica";
        private const string SPSELECTID = "SELECT * FROM bne_Deficiencia_Detalhe WHERE Idf_Deficiencia_Detalhe = @Idf_Deficiencia_Detalhe";
		#endregion

		#region Métodos

        //#region GetParameters
        ///// <summary>
        ///// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
        ///// </summary>
        ///// <returns>Lista de parâmetros SQL.</returns>
        ///// <remarks>Gieyson Stelmak</remarks>
        //private List<SqlParameter> GetParameters()
        //{
        //    List<SqlParameter> parms = new List<SqlParameter>();
        //    parms.Add(new SqlParameter("@idf_Pessoa_Fisica", SqlDbType.Int, 4));
        //    parms.Add(new SqlParameter("@Idf_Deficiencia", SqlDbType.Int, 4));
        //    return(parms);
        //}
        //#endregion

        //#region SetParameters
        ///// <summary>
        ///// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
        ///// </summary>
        ///// <param name="parms">Lista de parâmetros SQL.</param>
        ///// <remarks>Gieyson Stelmak</remarks>
        //private void SetParameters(List<SqlParameter> parms)
        //{
        //    parms[0].Value = this._pessoaFisica.IdPessoaFisica;
        //    parms[1].Value = this._deficiencia.IdDeficiencia;
        //}
        //#endregion

        //#region Insert
        ///// <summary>
        ///// Método utilizado para inserir uma instância de Deficiencia no banco de dados.
        ///// </summary>
        ///// <remarks>Gieyson Stelmak</remarks>
        //private void Insert()
        //{
        //    List<SqlParameter> parms = GetParameters();
        //    SetParameters(parms);

        //    using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
        //    {
        //        conn.Open();
        //        using (SqlTransaction trans = conn.BeginTransaction())
        //        {
        //            try
        //            {
        //                SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
        //                cmd.Parameters.Clear();
        //                this._persisted = true;
        //                this._modified = false;
        //                trans.Commit();
        //            }
        //            catch
        //            {
        //                trans.Rollback();
        //                throw;
        //            }
        //        }
        //    }
        //}
        ///// <summary>
        ///// Método utilizado para inserir uma instância de Deficiencia no banco de dados, dentro de uma transação.
        ///// </summary>
        ///// <param name="trans">Transação existente no banco de dados.</param>
        ///// <remarks>Gieyson Stelmak</remarks>
        //private void Insert(SqlTransaction trans)
        //{
        //    List<SqlParameter> parms = GetParameters();
        //    SetParameters(parms);
        //    SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
        //    cmd.Parameters.Clear();
        //    this._persisted = true;
        //    this._modified = false;
        //}
        //#endregion

        //#region Save
        ///// <summary>
        ///// Método utilizado para salvar uma instância de Deficiencia no banco de dados.
        ///// </summary>
        ///// <remarks>Gieyson Stelmak</remarks>
        //public void Save()
        //{
        //    if (!this._persisted)
        //        this.Insert();
			
        //}
        ///// <summary>
        ///// Método utilizado para salvar uma instância de Deficiencia no banco de dados, dentro de uma transação.
        ///// </summary>
        ///// <param name="trans">Transação existente no banco de dados.</param>
        ///// <remarks>Gieyson Stelmak</remarks>
        //public void Save(SqlTransaction trans)
        //{
        //    if (!this._persisted)
        //        this.Insert(trans);
			
        //}
        //#endregion

        //#region Delete
        ///// <summary>
        ///// Método utilizado para excluir uma instância de Deficiencia no banco de dados.
        ///// </summary>
        ///// <param name="idDeficiencia">Chave do registro.</param>
        ///// <remarks>Gieyson Stelmak</remarks>
        //public static void Delete(int idPessoaFisica, int idDeficiencia)
        //{
        //    List<SqlParameter> parms = new List<SqlParameter>();
        //    parms.Add(new SqlParameter("@Idf_Deficiencia", SqlDbType.Int, 4));
        //    parms.Add(new SqlParameter("@idf_Pessoa_Fisica", SqlDbType.Int, 4));

        //    parms[0].Value = idDeficiencia;
        //    parms[1].Value = idPessoaFisica;

        //    DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        //}
        ///// <summary>
        ///// Método utilizado para excluir uma instância de Deficiencia no banco de dados, dentro de uma transação.
        ///// </summary>
        ///// <param name="idDeficiencia">Chave do registro.</param>
        ///// <param name="trans">Transação existente no banco de dados.</param>
        ///// <remarks>Gieyson Stelmak</remarks>
        //public static void Delete(int idDeficiencia, SqlTransaction trans)
        //{
        //    List<SqlParameter> parms = new List<SqlParameter>();
        //    parms.Add(new SqlParameter("@Idf_Deficiencia", SqlDbType.Int, 4));

        //    parms[0].Value = idDeficiencia;

        //    DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        //}
        ///// <summary>
        ///// Método utilizado para excluir uma lista de Deficiencia no banco de dados.
        ///// </summary>
        ///// <param name="idDeficiencia">Lista de chaves.</param>
        ///// <remarks>Gieyson Stelmak</remarks>
        //public static void Delete(List<int> idDeficiencia)
        //{
        //    List<SqlParameter> parms = new List<SqlParameter>();
        //    string query = "delete from plataforma.TAB_Deficiencia where Idf_Deficiencia in (";

        //    for (int i = 0; i < idDeficiencia.Count; i++)
        //    {
        //        string nomeParametro = "@parm" + i.ToString();

        //        if (i > 0)
        //        {
        //            query += ", ";
        //        }
        //        query += nomeParametro;
        //        parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
        //        parms[i].Value = idDeficiencia[i];
        //    }

        //    query += ")";

        //    DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        //}
        //#endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idPessoaFisicaDeficiencia">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        private static IDataReader LoadDataReader(int idDeficienciaDetalhe)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@idf_Deficiencia_Detalhe", SqlDbType.Int, 4));

            parms[0].Value = idDeficienciaDetalhe;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idDeficienciaDetalhe">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idDeficienciaDetalhe, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@idf_Deficiencia_Detalhe", SqlDbType.Int, 4));

            parms[0].Value = idDeficienciaDetalhe;

            return DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPSELECTID, parms);
        }

        #endregion


        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de AcaoPublicacao a partir do banco de dados.
        /// </summary>
        /// <param name="idDeficienciaDetalhe">Chave do registro.</param>
        /// <returns>Instância de AcaoPublicacao.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static DeficienciaDetalhe LoadObject(int idDeficienciaDetalhe)
        {
            using (IDataReader dr = LoadDataReader(idDeficienciaDetalhe))
            {
                DeficienciaDetalhe objDeficienciaDetalhe = new DeficienciaDetalhe();
                if (SetInstance(dr, objDeficienciaDetalhe))
                    return objDeficienciaDetalhe;
            }
            throw (new RecordNotFoundException(typeof(PessoaFisicaDeficiencia)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de AcaoPublicacao a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idDeficienciaDetalhe">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de AcaoPublicacao.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static DeficienciaDetalhe LoadObject(int idDeficienciaDetalhe, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idDeficienciaDetalhe, trans))
            {
                DeficienciaDetalhe objDeficienciaDetalhe = new DeficienciaDetalhe();
                if (SetInstance(dr, objDeficienciaDetalhe))
                    return objDeficienciaDetalhe;
            }
            throw (new RecordNotFoundException(typeof(PessoaFisicaDeficiencia)));
        }
        #endregion

        #region SetInstance
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objDeficienciaDetalhe">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance(IDataReader dr, DeficienciaDetalhe objDeficienciaDetalhe)
        {
            try
            {
                if (dr.Read())
                {
                    objDeficienciaDetalhe._idfDeficienciaDetalhe = Convert.ToInt32(dr["Idf_Deficiencia_Detalhe"]);
                    objDeficienciaDetalhe.Deficiencia = new Deficiencia(Convert.ToInt32(dr["Idf_Deficiencia"]));
                    objDeficienciaDetalhe._desDeficienciaDetalhe = dr["Des_Deficiencia_Detalhe"].ToString();
                    objDeficienciaDetalhe._flgInativo = Convert.ToBoolean(dr["Flg_Inativo"]);

                    objDeficienciaDetalhe._persisted = true;
                    objDeficienciaDetalhe._modified = false;

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
