//-- Data: 29/06/2015 17:58
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;
using Microsoft.SqlServer.Types;

namespace BNE.BLL
{
    public partial class Cidade // Tabela: plataforma.TAB_Cidade
    {
        #region Atributos
        private int _idCidade;
        private string _nomeCidade;
        private bool _flagInativo;
        private DateTime _dataCadastro;
        private Estado _estado;
        private decimal _taxaISS;
        private int? _idTipoBase;
        private decimal? _codigoRais;
        private SqlGeography _GeoLocalizacao;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdCidade
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public int IdCidade
        {
            get
            {
                return this._idCidade;
            }
            set
            {
                this._idCidade = value;
                this._modified = true;
            }
        }
        #endregion

        #region NomeCidade
        /// <summary>
        /// Tamanho do campo: 80.
        /// Campo obrigatório.
        /// </summary>
        public string NomeCidade
        {
            get
            {
                return this._nomeCidade;
            }
            set
            {
                this._nomeCidade = value;
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

        #region Estado
        /// <summary>
        /// Campo obrigatório.
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

        #region TaxaISS
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public decimal TaxaISS
        {
            get
            {
                return this._taxaISS;
            }
            set
            {
                this._taxaISS = value;
                this._modified = true;
            }
        }
        #endregion

        #region IdTipoBase
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public int? IdTipoBase
        {
            get
            {
                return this._idTipoBase;
            }
            set
            {
                this._idTipoBase = value;
                this._modified = true;
            }
        }
        #endregion

        #region CodigoRais
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public decimal? CodigoRais
        {
            get
            {
                return this._codigoRais;
            }
            set
            {
                this._codigoRais = value;
                this._modified = true;
            }
        }
        #endregion

        #region GeoLocalizacao
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public SqlGeography GeoLocalizacao
        {
            get
            {
                return this._GeoLocalizacao;
            }
            set
            {
                this._GeoLocalizacao = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Construtores
        public Cidade()
        {
        }
        public Cidade(int idCidade)
        {
            this._idCidade = idCidade;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO plataforma.TAB_Cidade (Idf_Cidade, Nme_Cidade, Flg_Inativo, Dta_Cadastro, Sig_Estado, Txa_ISS, Idf_Tipo_Base, Cod_Rais, Geo_Localizacao) VALUES (@Idf_Cidade, @Nme_Cidade, @Flg_Inativo, @Dta_Cadastro, @Sig_Estado, @Txa_ISS, @Idf_Tipo_Base, @Cod_Rais, @Geo_Localizacao);";
        private const string SPUPDATE = "UPDATE plataforma.TAB_Cidade SET Nme_Cidade = @Nme_Cidade, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro, Sig_Estado = @Sig_Estado, Txa_ISS = @Txa_ISS, Idf_Tipo_Base = @Idf_Tipo_Base, Cod_Rais = @Cod_Rais, Geo_Localizacao = @Geo_Localizacao WHERE Idf_Cidade = @Idf_Cidade";
        private const string SPDELETE = "DELETE FROM plataforma.TAB_Cidade WHERE Idf_Cidade = @Idf_Cidade";
        private const string SPSELECTID = "SELECT * FROM plataforma.TAB_Cidade WITH(NOLOCK) WHERE Idf_Cidade = @Idf_Cidade";
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
			parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Nme_Cidade", SqlDbType.VarChar, 80));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Sig_Estado", SqlDbType.Char, 2));
			parms.Add(new SqlParameter("@Txa_ISS", SqlDbType.Float, 8));
			parms.Add(new SqlParameter("@Idf_Tipo_Base", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Cod_Rais", SqlDbType.Decimal, 5));
			parms.Add(new SqlParameter { ParameterName = "@Geo_Localizacao", UdtTypeName = "Geography" });
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
            parms[0].Value = this._idCidade;
            parms[1].Value = this._nomeCidade;
            parms[2].Value = this._flagInativo;
            parms[4].Value = this._estado.SiglaEstado;
            parms[5].Value = this._taxaISS;

            if (this._idTipoBase.HasValue)
                parms[6].Value = this._idTipoBase;
            else
                parms[6].Value = DBNull.Value;


            if (this._codigoRais.HasValue)
                parms[7].Value = this._codigoRais;
            else
                parms[7].Value = DBNull.Value;


            if (this._GeoLocalizacao != null)
                parms[8].Value = this._GeoLocalizacao;
            else
                parms[8].Value = SqlGeography.Null;


            if (!this._persisted)
            {
                this._dataCadastro = DateTime.Now;
            }
            parms[3].Value = this._dataCadastro;
        }
        #endregion

        #region Insert
        /// <summary>
        /// Método utilizado para inserir uma instância de Cidade no banco de dados.
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
        /// Método utilizado para inserir uma instância de Cidade no banco de dados, dentro de uma transação.
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
        /// Método utilizado para atualizar uma instância de Cidade no banco de dados.
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
        /// Método utilizado para atualizar uma instância de Cidade no banco de dados, dentro de uma transação.
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

        #region Delete
        /// <summary>
        /// Método utilizado para excluir uma instância de Cidade no banco de dados.
        /// </summary>
        /// <param name="idCidade">Chave do registro.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idCidade)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 4));

            parms[0].Value = idCidade;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de Cidade no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idCidade">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idCidade, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 4));

            parms[0].Value = idCidade;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de Cidade no banco de dados.
        /// </summary>
        /// <param name="idCidade">Lista de chaves.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(List<int> idCidade)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from plataforma.TAB_Cidade where Idf_Cidade in (";

            for (int i = 0; i < idCidade.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idCidade[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idCidade">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idCidade)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 4));

            parms[0].Value = idCidade;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idCidade">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idCidade, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 4));

            parms[0].Value = idCidade;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cid.Idf_Cidade, Cid.Nme_Cidade, Cid.Flg_Inativo, Cid.Dta_Cadastro, Cid.Sig_Estado, Cid.Txa_ISS, Cid.Idf_Tipo_Base, Cid.Cod_Rais, Cid.Geo_Localizacao FROM plataforma.TAB_Cidade Cid";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #endregion

    }
}