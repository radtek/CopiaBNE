//-- Data: 28/04/2014 09:41
//-- Autor: Francisco Ribas

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class PlanoAdquirido // Tabela: BNE_Plano_Adquirido
    {
        #region Atributos
        private int _idPlanoAdquirido;
        private Plano _plano;
        private UsuarioFilialPerfil _usuarioFilialPerfil;
        private DateTime _dataCadastro;
        private DateTime _dataInicioPlano;
        private DateTime _dataFimPlano;
        private PlanoSituacao _planoSituacao;
        private Filial _filial;
        private int _quantidadeSMS;
        private decimal _valorBase;
        private int? _quantidadePrazoBoleto;
        private bool _flagBoletoRegistrado;
        private bool _flagNotaAntecipada;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdPlanoAdquirido
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdPlanoAdquirido
        {
            get
            {
                return this._idPlanoAdquirido;
            }
        }
        #endregion

        #region Plano
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public Plano Plano
        {
            get
            {
                return this._plano;
            }
            set
            {
                this._plano = value;
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

        #region DataInicioPlano
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public DateTime DataInicioPlano
        {
            get
            {
                return this._dataInicioPlano;
            }
            set
            {
                this._dataInicioPlano = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataFimPlano
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public DateTime DataFimPlano
        {
            get
            {
                return this._dataFimPlano;
            }
            set
            {
                this._dataFimPlano = value;
                this._modified = true;
            }
        }
        #endregion

        #region PlanoSituacao
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public PlanoSituacao PlanoSituacao
        {
            get
            {
                return this._planoSituacao;
            }
            set
            {
                this._planoSituacao = value;
                this._modified = true;
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

        #region QuantidadeSMS
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public int QuantidadeSMS
        {
            get
            {
                return this._quantidadeSMS;
            }
            set
            {
                this._quantidadeSMS = value;
                this._modified = true;
            }
        }
        #endregion

        #region ValorBase
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public decimal ValorBase
        {
            get
            {
                return this._valorBase;
            }
            set
            {
                this._valorBase = value;
                this._modified = true;
            }
        }
        #endregion

        #region QuantidadePrazoBoleto
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public int? QuantidadePrazoBoleto
        {
            get
            {
                return this._quantidadePrazoBoleto;
            }
            set
            {
                this._quantidadePrazoBoleto = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagBoletoRegistrado
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagBoletoRegistrado
        {
            get
            {
                return this._flagBoletoRegistrado;
            }
            set
            {
                this._flagBoletoRegistrado = value;
                this._modified = true;
            }
        }
        #endregion

        #region NotaAntecipada
        public bool FlgNotaAntecipada 
        {
            get 
            {
                return this._flagNotaAntecipada;
            }
            set 
            {
                this._flagNotaAntecipada = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Construtores
        public PlanoAdquirido()
        {
        }
        public PlanoAdquirido(int idPlanoAdquirido)
        {
            this._idPlanoAdquirido = idPlanoAdquirido;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Plano_Adquirido (Idf_Plano, Idf_Usuario_Filial_Perfil, Dta_Cadastro, Dta_Inicio_Plano, Dta_Fim_Plano, Idf_Plano_Situacao, Idf_Filial, Qtd_SMS, Vlr_Base, Qtd_Prazo_Boleto, Flg_Boleto_Registrado, Flg_Nota_Antecipada) VALUES (@Idf_Plano, @Idf_Usuario_Filial_Perfil, @Dta_Cadastro, @Dta_Inicio_Plano, @Dta_Fim_Plano, @Idf_Plano_Situacao, @Idf_Filial, @Qtd_SMS, @Vlr_Base, @Qtd_Prazo_Boleto, @Flg_Boleto_Registrado, @Flg_Nota_Antecipada);SET @Idf_Plano_Adquirido = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE BNE_Plano_Adquirido SET Idf_Plano = @Idf_Plano, Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil, Dta_Cadastro = @Dta_Cadastro, Dta_Inicio_Plano = @Dta_Inicio_Plano, Dta_Fim_Plano = @Dta_Fim_Plano, Idf_Plano_Situacao = @Idf_Plano_Situacao, Idf_Filial = @Idf_Filial, Qtd_SMS = @Qtd_SMS, Vlr_Base = @Vlr_Base, Qtd_Prazo_Boleto = @Qtd_Prazo_Boleto, Flg_Boleto_Registrado = @Flg_Boleto_Registrado, Flg_Nota_Antecipada = @Flg_Nota_Antecipada WHERE Idf_Plano_Adquirido = @Idf_Plano_Adquirido";
        private const string SPDELETE = "DELETE FROM BNE_Plano_Adquirido WHERE Idf_Plano_Adquirido = @Idf_Plano_Adquirido";
        private const string SPSELECTID = "SELECT * FROM BNE_Plano_Adquirido WITH(NOLOCK) WHERE Idf_Plano_Adquirido = @Idf_Plano_Adquirido";
        #endregion

        #region Métodos

        #region GetParameters
        /// <summary>
        /// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
        /// </summary>
        /// <returns>Lista de parâmetros SQL.</returns>
        /// <remarks>Francisco Ribas</remarks>
        private List<SqlParameter> GetParameters()
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano_Adquirido", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Plano", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Dta_Inicio_Plano", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Dta_Fim_Plano", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Idf_Plano_Situacao", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Qtd_SMS", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Vlr_Base", SqlDbType.Decimal, 5));
            parms.Add(new SqlParameter("@Qtd_Prazo_Boleto", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Flg_Boleto_Registrado", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Nota_Antecipada", SqlDbType.Bit, 1));
            return (parms);
        }
        #endregion

        #region SetParameters
        /// <summary>
        /// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
        /// </summary>
        /// <param name="parms">Lista de parâmetros SQL.</param>
        /// <remarks>Francisco Ribas</remarks>
        private void SetParameters(List<SqlParameter> parms)
        {
            parms[0].Value = this._idPlanoAdquirido;
            parms[1].Value = this._plano.IdPlano;
            parms[2].Value = this._usuarioFilialPerfil.IdUsuarioFilialPerfil;
            parms[4].Value = this._dataInicioPlano;
            parms[5].Value = this._dataFimPlano;
            parms[6].Value = this._planoSituacao.IdPlanoSituacao;

            if (this._filial != null)
                parms[7].Value = this._filial.IdFilial;
            else
                parms[7].Value = DBNull.Value;

            parms[8].Value = this._quantidadeSMS;
            parms[9].Value = this._valorBase;

            if (this._quantidadePrazoBoleto.HasValue)
                parms[10].Value = this._quantidadePrazoBoleto;
            else
                parms[10].Value = DBNull.Value;

            parms[11].Value = this._flagBoletoRegistrado;
            parms[12].Value = this._flagNotaAntecipada;

            if (!this._persisted)
            {
                parms[0].Direction = ParameterDirection.Output;
                this._dataCadastro = DateTime.Now;
            }
            else
            {
                parms[0].Direction = ParameterDirection.Input;
            }
            parms[3].Value = this._dataCadastro;
        }
        #endregion

        #region Insert
        /// <summary>
        /// Método utilizado para inserir uma instância de PlanoAdquirido no banco de dados.
        /// </summary>
        /// <remarks>Francisco Ribas</remarks>
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
                        this._idPlanoAdquirido = Convert.ToInt32(cmd.Parameters["@Idf_Plano_Adquirido"].Value);
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
        /// Método utilizado para inserir uma instância de PlanoAdquirido no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Francisco Ribas</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idPlanoAdquirido = Convert.ToInt32(cmd.Parameters["@Idf_Plano_Adquirido"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de PlanoAdquirido no banco de dados.
        /// </summary>
        /// <remarks>Francisco Ribas</remarks>
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
        /// Método utilizado para atualizar uma instância de PlanoAdquirido no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Francisco Ribas</remarks>
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
        /// Método utilizado para salvar uma instância de PlanoAdquirido no banco de dados.
        /// </summary>
        /// <remarks>Francisco Ribas</remarks>
        public void Save()
        {
            if (!this._persisted)
                this.Insert();
            else
                this.Update();
        }
        /// <summary>
        /// Método utilizado para salvar uma instância de PlanoAdquirido no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Francisco Ribas</remarks>
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
        /// Método utilizado para excluir uma instância de PlanoAdquirido no banco de dados.
        /// </summary>
        /// <param name="idPlanoAdquirido">Chave do registro.</param>
        /// <remarks>Francisco Ribas</remarks>
        public static void Delete(int idPlanoAdquirido)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano_Adquirido", SqlDbType.Int, 4));

            parms[0].Value = idPlanoAdquirido;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de PlanoAdquirido no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPlanoAdquirido">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Francisco Ribas</remarks>
        public static void Delete(int idPlanoAdquirido, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano_Adquirido", SqlDbType.Int, 4));

            parms[0].Value = idPlanoAdquirido;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de PlanoAdquirido no banco de dados.
        /// </summary>
        /// <param name="idPlanoAdquirido">Lista de chaves.</param>
        /// <remarks>Francisco Ribas</remarks>
        public static void Delete(List<int> idPlanoAdquirido)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from BNE_Plano_Adquirido where Idf_Plano_Adquirido in (";

            for (int i = 0; i < idPlanoAdquirido.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idPlanoAdquirido[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idPlanoAdquirido">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Francisco Ribas</remarks>
        private static IDataReader LoadDataReader(int idPlanoAdquirido)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano_Adquirido", SqlDbType.Int, 4));

            parms[0].Value = idPlanoAdquirido;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPlanoAdquirido">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Francisco Ribas</remarks>
        private static IDataReader LoadDataReader(int idPlanoAdquirido, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano_Adquirido", SqlDbType.Int, 4));

            parms[0].Value = idPlanoAdquirido;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Pla.Idf_Plano_Adquirido, Pla.Idf_Plano, Pla.Idf_Usuario_Filial_Perfil, Pla.Dta_Cadastro, Pla.Dta_Inicio_Plano, Pla.Dta_Fim_Plano, Pla.Idf_Plano_Situacao, Pla.Idf_Filial, Pla.Qtd_SMS, Pla.Vlr_Base, Pla.Qtd_Prazo_Boleto, Pla.Flg_Boleto_Registrado FROM BNE_Plano_Adquirido Pla";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de PlanoAdquirido a partir do banco de dados.
        /// </summary>
        /// <param name="idPlanoAdquirido">Chave do registro.</param>
        /// <returns>Instância de PlanoAdquirido.</returns>
        /// <remarks>Francisco Ribas</remarks>
        public static PlanoAdquirido LoadObject(int idPlanoAdquirido)
        {
            using (IDataReader dr = LoadDataReader(idPlanoAdquirido))
            {
                PlanoAdquirido objPlanoAdquirido = new PlanoAdquirido();
                if (SetInstance(dr, objPlanoAdquirido))
                    return objPlanoAdquirido;
            }
            throw (new RecordNotFoundException(typeof(PlanoAdquirido)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de PlanoAdquirido a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPlanoAdquirido">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de PlanoAdquirido.</returns>
        /// <remarks>Francisco Ribas</remarks>
        public static PlanoAdquirido LoadObject(int idPlanoAdquirido, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idPlanoAdquirido, trans))
            {
                PlanoAdquirido objPlanoAdquirido = new PlanoAdquirido();
                if (SetInstance(dr, objPlanoAdquirido))
                    return objPlanoAdquirido;
            }
            throw (new RecordNotFoundException(typeof(PlanoAdquirido)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de PlanoAdquirido a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Francisco Ribas</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idPlanoAdquirido))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de PlanoAdquirido a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Francisco Ribas</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idPlanoAdquirido, trans))
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
        /// <param name="objPlanoAdquirido">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Francisco Ribas</remarks>
        private static bool SetInstance(IDataReader dr, PlanoAdquirido objPlanoAdquirido)
        {
            try
            {
                if (dr.Read())
                {
                    objPlanoAdquirido._idPlanoAdquirido = Convert.ToInt32(dr["Idf_Plano_Adquirido"]);
                    objPlanoAdquirido._plano = new Plano(Convert.ToInt32(dr["Idf_Plano"]));
                    objPlanoAdquirido._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));
                    objPlanoAdquirido._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                    objPlanoAdquirido._dataInicioPlano = Convert.ToDateTime(dr["Dta_Inicio_Plano"]);
                    objPlanoAdquirido._dataFimPlano = Convert.ToDateTime(dr["Dta_Fim_Plano"]);
                    objPlanoAdquirido._planoSituacao = new PlanoSituacao(Convert.ToInt32(dr["Idf_Plano_Situacao"]));
                    if (dr["Idf_Filial"] != DBNull.Value)
                        objPlanoAdquirido._filial = new Filial(Convert.ToInt32(dr["Idf_Filial"]));
                    objPlanoAdquirido._quantidadeSMS = Convert.ToInt32(dr["Qtd_SMS"]);
                    objPlanoAdquirido._valorBase = Convert.ToDecimal(dr["Vlr_Base"]);
                    if (dr["Qtd_Prazo_Boleto"] != DBNull.Value)
                        objPlanoAdquirido._quantidadePrazoBoleto = Convert.ToInt32(dr["Qtd_Prazo_Boleto"]);
                    objPlanoAdquirido._flagBoletoRegistrado = Convert.ToBoolean(dr["Flg_Boleto_Registrado"]);
                    if(dr["Flg_Nota_Antecipada"] != DBNull.Value)
                        objPlanoAdquirido._flagNotaAntecipada = Convert.ToBoolean(dr["Flg_Nota_Antecipada"]);

                    objPlanoAdquirido._persisted = true;
                    objPlanoAdquirido._modified = false;

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