using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class PesquisaVagaDisponibilidade // Tabela: TAB_Pesquisa_Vaga_Disponibilidade
    {
        #region Atributos

        private Disponibilidade _disponibilidade;
        private PesquisaVaga _pesquisaVaga;

        private bool _persisted;
        private bool _modified;

        #endregion

        #region Propriedades

        #region IdPesquisaVagaDisponibilidade

        /// <summary>
        ///     Campo obrigatório.
        ///     Campo auto-numerado.
        /// </summary>
        public int IdPesquisaVagaDisponibilidade { get; private set; }

        #endregion

        #region Disponibilidade

        /// <summary>
        ///     Campo opcional.
        /// </summary>
        public Disponibilidade Disponibilidade
        {
            get { return _disponibilidade; }
            set
            {
                _disponibilidade = value;
                _modified = true;
            }
        }

        #endregion

        #region PesquisaVaga

        /// <summary>
        ///     Campo opcional.
        /// </summary>
        public PesquisaVaga PesquisaVaga
        {
            get { return _pesquisaVaga; }
            set
            {
                _pesquisaVaga = value;
                _modified = true;
            }
        }

        #endregion

        #endregion

        #region Construtores

        public PesquisaVagaDisponibilidade()
        {
        }

        public PesquisaVagaDisponibilidade(int idPesquisaVagaDisponibilidade)
        {
            IdPesquisaVagaDisponibilidade = idPesquisaVagaDisponibilidade;
            _persisted = true;
        }

        #endregion

        #region Consultas

        private const string SPINSERT = "INSERT INTO TAB_Pesquisa_Vaga_Disponibilidade (Idf_Disponibilidade, Idf_Pesquisa_Vaga) VALUES (@Idf_Disponibilidade, @Idf_Pesquisa_Vaga);SET @Idf_Pesquisa_Vaga_Disponibilidade = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE TAB_Pesquisa_Vaga_Disponibilidade SET Idf_Disponibilidade = @Idf_Disponibilidade, Idf_Pesquisa_Vaga = @Idf_Pesquisa_Vaga WHERE Idf_Pesquisa_Vaga_Disponibilidade = @Idf_Pesquisa_Vaga_Disponibilidade";
        private const string SPDELETE = "DELETE FROM TAB_Pesquisa_Vaga_Disponibilidade WHERE Idf_Pesquisa_Vaga_Disponibilidade = @Idf_Pesquisa_Vaga_Disponibilidade";
        private const string SPSELECTID = "SELECT * FROM TAB_Pesquisa_Vaga_Disponibilidade WITH(NOLOCK) WHERE Idf_Pesquisa_Vaga_Disponibilidade = @Idf_Pesquisa_Vaga_Disponibilidade";

        #endregion

        #region Métodos

        #region GetParameters

        /// <summary>
        ///     Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
        /// </summary>
        /// <returns>Lista de parâmetros SQL.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private List<SqlParameter> GetParameters()
        {
            var parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pesquisa_Vaga_Disponibilidade", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Disponibilidade", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Pesquisa_Vaga", SqlDbType.Int, 4));
            return parms;
        }

        #endregion

        #region SetParameters

        /// <summary>
        ///     Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
        /// </summary>
        /// <param name="parms">Lista de parâmetros SQL.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void SetParameters(List<SqlParameter> parms)
        {
            parms[0].Value = IdPesquisaVagaDisponibilidade;

            if (_disponibilidade != null)
            {
                parms[1].Value = _disponibilidade.IdDisponibilidade;
            }
            else
            {
                parms[1].Value = DBNull.Value;
            }


            if (_pesquisaVaga != null)
            {
                parms[2].Value = _pesquisaVaga.IdPesquisaVaga;
            }
            else
            {
                parms[2].Value = DBNull.Value;
            }


            if (!_persisted)
            {
                parms[0].Direction = ParameterDirection.Output;
            }
            else
            {
                parms[0].Direction = ParameterDirection.Input;
            }
        }

        #endregion

        #region Insert

        /// <summary>
        ///     Método utilizado para inserir uma instância de PesquisaVagaDisponibilidade no banco de dados.
        /// </summary>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Insert()
        {
            var parms = GetParameters();
            SetParameters(parms);

            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        var cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
                        IdPesquisaVagaDisponibilidade = Convert.ToInt32(cmd.Parameters["@Idf_Pesquisa_Vaga_Disponibilidade"].Value);
                        cmd.Parameters.Clear();
                        _persisted = true;
                        _modified = false;
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
        ///     Método utilizado para inserir uma instância de PesquisaVagaDisponibilidade no banco de dados, dentro de uma
        ///     transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Insert(SqlTransaction trans)
        {
            var parms = GetParameters();
            SetParameters(parms);
            var cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            IdPesquisaVagaDisponibilidade = Convert.ToInt32(cmd.Parameters["@Idf_Pesquisa_Vaga_Disponibilidade"].Value);
            cmd.Parameters.Clear();
            _persisted = true;
            _modified = false;
        }

        #endregion

        #region Update

        /// <summary>
        ///     Método utilizado para atualizar uma instância de PesquisaVagaDisponibilidade no banco de dados.
        /// </summary>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Update()
        {
            if (_modified)
            {
                var parms = GetParameters();
                SetParameters(parms);
                DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPUPDATE, parms);
                _modified = false;
            }
        }

        /// <summary>
        ///     Método utilizado para atualizar uma instância de PesquisaVagaDisponibilidade no banco de dados, dentro de uma
        ///     transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Update(SqlTransaction trans)
        {
            if (_modified)
            {
                var parms = GetParameters();
                SetParameters(parms);
                DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPUPDATE, parms);
                _modified = false;
            }
        }

        #endregion

        #region Save

        /// <summary>
        ///     Método utilizado para salvar uma instância de PesquisaVagaDisponibilidade no banco de dados.
        /// </summary>
        /// <remarks>Gieyson Stelmak</remarks>
        public void Save()
        {
            if (!_persisted)
            {
                Insert();
            }
            else
            {
                Update();
            }
        }

        /// <summary>
        ///     Método utilizado para salvar uma instância de PesquisaVagaDisponibilidade no banco de dados, dentro de uma
        ///     transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public void Save(SqlTransaction trans)
        {
            if (!_persisted)
            {
                Insert(trans);
            }
            else
            {
                Update(trans);
            }
        }

        #endregion

        #region Delete

        /// <summary>
        ///     Método utilizado para excluir uma instância de PesquisaVagaDisponibilidade no banco de dados.
        /// </summary>
        /// <param name="idPesquisaVagaDisponibilidade">Chave do registro.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idPesquisaVagaDisponibilidade)
        {
            var parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pesquisa_Vaga_Disponibilidade", SqlDbType.Int, 4));

            parms[0].Value = idPesquisaVagaDisponibilidade;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }

        /// <summary>
        ///     Método utilizado para excluir uma instância de PesquisaVagaDisponibilidade no banco de dados, dentro de uma
        ///     transação.
        /// </summary>
        /// <param name="idPesquisaVagaDisponibilidade">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idPesquisaVagaDisponibilidade, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pesquisa_Vaga_Disponibilidade", SqlDbType.Int, 4));

            parms[0].Value = idPesquisaVagaDisponibilidade;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }

        /// <summary>
        ///     Método utilizado para excluir uma lista de PesquisaVagaDisponibilidade no banco de dados.
        /// </summary>
        /// <param name="idPesquisaVagaDisponibilidade">Lista de chaves.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(List<int> idPesquisaVagaDisponibilidade)
        {
            var parms = new List<SqlParameter>();
            var query = "delete from TAB_Pesquisa_Vaga_Disponibilidade where Idf_Pesquisa_Vaga_Disponibilidade in (";

            for (var i = 0; i < idPesquisaVagaDisponibilidade.Count; i++)
            {
                var nomeParametro = "@parm" + i;

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idPesquisaVagaDisponibilidade[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }

        #endregion

        #region LoadDataReader

        /// <summary>
        ///     Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idPesquisaVagaDisponibilidade">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idPesquisaVagaDisponibilidade)
        {
            var parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pesquisa_Vaga_Disponibilidade", SqlDbType.Int, 4));

            parms[0].Value = idPesquisaVagaDisponibilidade;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }

        /// <summary>
        ///     Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPesquisaVagaDisponibilidade">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idPesquisaVagaDisponibilidade, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pesquisa_Vaga_Disponibilidade", SqlDbType.Int, 4));

            parms[0].Value = idPesquisaVagaDisponibilidade;

            return DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPSELECTID, parms);
        }

        /// <summary>
        ///     Método utilizado por retornar uma consulta paginada do banco de dados.
        /// </summary>
        /// <param name="colunaOrdenacao">Nome da coluna pela qual será ordenada.</param>
        /// <param name="direcaoOrdenacao">Direção da ordenação (ASC ou DESC).</param>
        /// <param name="paginaCorrente">Número da página que será exibida.</param>
        /// <param name="tamanhoPagina">Quantidade de itens em cada página.</param>
        /// <param name="totalRegistros">Quantidade total de registros na tabela.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        public static IDataReader LoadDataReader(string colunaOrdenacao, string direcaoOrdenacao, int paginaCorrente, int tamanhoPagina, out int totalRegistros)
        {
            var inicio = (paginaCorrente - 1) * tamanhoPagina + 1;
            var fim = paginaCorrente * tamanhoPagina;

            var SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Pes.Idf_Pesquisa_Vaga_Disponibilidade, Pes.Idf_Disponibilidade, Pes.Idf_Pesquisa_Vaga FROM TAB_Pesquisa_Vaga_Disponibilidade Pes";
            var SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio + " AND " + fim;

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }

        #endregion

        #region LoadObject

        /// <summary>
        ///     Método utilizado para retornar uma instância de PesquisaVagaDisponibilidade a partir do banco de dados.
        /// </summary>
        /// <param name="idPesquisaVagaDisponibilidade">Chave do registro.</param>
        /// <returns>Instância de PesquisaVagaDisponibilidade.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static PesquisaVagaDisponibilidade LoadObject(int idPesquisaVagaDisponibilidade)
        {
            using (var dr = LoadDataReader(idPesquisaVagaDisponibilidade))
            {
                var objPesquisaVagaDisponibilidade = new PesquisaVagaDisponibilidade();
                if (SetInstance(dr, objPesquisaVagaDisponibilidade))
                {
                    return objPesquisaVagaDisponibilidade;
                }
            }
            throw new RecordNotFoundException(typeof(PesquisaVagaDisponibilidade));
        }

        /// <summary>
        ///     Método utilizado para retornar uma instância de PesquisaVagaDisponibilidade a partir do banco de dados, dentro de
        ///     uma transação.
        /// </summary>
        /// <param name="idPesquisaVagaDisponibilidade">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de PesquisaVagaDisponibilidade.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static PesquisaVagaDisponibilidade LoadObject(int idPesquisaVagaDisponibilidade, SqlTransaction trans)
        {
            using (var dr = LoadDataReader(idPesquisaVagaDisponibilidade, trans))
            {
                var objPesquisaVagaDisponibilidade = new PesquisaVagaDisponibilidade();
                if (SetInstance(dr, objPesquisaVagaDisponibilidade))
                {
                    return objPesquisaVagaDisponibilidade;
                }
            }
            throw new RecordNotFoundException(typeof(PesquisaVagaDisponibilidade));
        }

        #endregion

        #region CompleteObject

        /// <summary>
        ///     Método utilizado para completar uma instância de PesquisaVagaDisponibilidade a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            using (var dr = LoadDataReader(IdPesquisaVagaDisponibilidade))
            {
                return SetInstance(dr, this);
            }
        }

        /// <summary>
        ///     Método utilizado para completar uma instância de PesquisaVagaDisponibilidade a partir do banco de dados, dentro de
        ///     uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (var dr = LoadDataReader(IdPesquisaVagaDisponibilidade, trans))
            {
                return SetInstance(dr, this);
            }
        }

        #endregion

        #region SetInstance

        /// <summary>
        ///     Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as
        ///     colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objPesquisaVagaDisponibilidade">Instância a ser manipulada.</param>
        /// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance(IDataReader dr, PesquisaVagaDisponibilidade objPesquisaVagaDisponibilidade, bool dispose = true)
        {
            try
            {
                if (dr.Read())
                {
                    objPesquisaVagaDisponibilidade.IdPesquisaVagaDisponibilidade = Convert.ToInt32(dr["Idf_Pesquisa_Vaga_Disponibilidade"]);
                    if (dr["Idf_Disponibilidade"] != DBNull.Value)
                    {
                        objPesquisaVagaDisponibilidade._disponibilidade = new Disponibilidade(Convert.ToInt32(dr["Idf_Disponibilidade"]));
                    }
                    if (dr["Idf_Pesquisa_Vaga"] != DBNull.Value)
                    {
                        objPesquisaVagaDisponibilidade._pesquisaVaga = new PesquisaVaga(Convert.ToInt32(dr["Idf_Pesquisa_Vaga"]));
                    }

                    objPesquisaVagaDisponibilidade._persisted = true;
                    objPesquisaVagaDisponibilidade._modified = false;

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
                {
                    dr.Dispose();
                }
            }
        }

        #endregion

        #endregion
    }
}