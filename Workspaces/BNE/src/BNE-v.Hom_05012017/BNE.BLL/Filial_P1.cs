//-- Data: 24/06/2016 10:29
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;
using Microsoft.SqlServer.Types;

namespace BNE.BLL
{
    public partial class Filial // Tabela: TAB_Filial
    {
        #region Atributos
        private int _idFilial;
        private bool _flagMatriz;
        private decimal? _numeroCNPJ;
        private string _razaoSocial;
        private string _nomeFantasia;
        private CNAESubClasse _cNAEPrincipal;
        private NaturezaJuridica _naturezaJuridica;
        private Endereco _endereco;
        private string _enderecoSite;
        private string _numeroDDDComercial;
        private string _numeroComercial;
        private bool _flagInativo;
        private DateTime _dataCadastro;
        private DateTime _dataAlteracao;
        private int? _quantidadeUsuarioAdicional;
        private int _quantidadeFuncionarios;
        private string _descricaoIP;
        private bool _flagOfereceCursos;
        private SituacaoFilial _situacaoFilial;
        private string _descricaoPaginaFacebook;
        private string _numeroComercial2;
        private SqlGeography _descricaoLocalizacao;
        private TipoParceiro _tipoParceiro;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdFilial
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdFilial
        {
            get { return this._idFilial; }
        }
        #endregion

        #region FlagMatriz
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagMatriz
        {
            get { return this._flagMatriz; }
            set
            {
                this._flagMatriz = value;
                this._modified = true;
            }
        }
        #endregion

        #region NumeroCNPJ
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public decimal? NumeroCNPJ
        {
            get { return this._numeroCNPJ; }
            set
            {
                this._numeroCNPJ = value;
                this._modified = true;
            }
        }
        #endregion

        #region RazaoSocial
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo obrigatório.
        /// </summary>
        public string RazaoSocial
        {
            get { return this._razaoSocial; }
            set
            {
                this._razaoSocial = value;
                this._modified = true;
            }
        }
        #endregion

        #region CNAEPrincipal
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public CNAESubClasse CNAEPrincipal
        {
            get { return this._cNAEPrincipal; }
            set
            {
                this._cNAEPrincipal = value;
                this._modified = true;
            }
        }
        #endregion

        #region NaturezaJuridica
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public NaturezaJuridica NaturezaJuridica
        {
            get { return this._naturezaJuridica; }
            set
            {
                this._naturezaJuridica = value;
                this._modified = true;
            }
        }
        #endregion

        #region Endereco
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public Endereco Endereco
        {
            get { return this._endereco; }
            set
            {
                this._endereco = value;
                this._modified = true;
            }
        }
        #endregion

        #region EnderecoSite
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo opcional.
        /// </summary>
        public string EnderecoSite
        {
            get { return this._enderecoSite; }
            set
            {
                this._enderecoSite = value;
                this._modified = true;
            }
        }
        #endregion

        #region NumeroDDDComercial
        /// <summary>
        /// Tamanho do campo: 2.
        /// Campo obrigatório.
        /// </summary>
        public string NumeroDDDComercial
        {
            get { return this._numeroDDDComercial; }
            set
            {
                this._numeroDDDComercial = value;
                this._modified = true;
            }
        }
        #endregion

        #region NumeroComercial
        /// <summary>
        /// Tamanho do campo: 10.
        /// Campo obrigatório.
        /// </summary>
        public string NumeroComercial
        {
            get { return this._numeroComercial; }
            set
            {
                this._numeroComercial = value;
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
            get { return this._flagInativo; }
            set
            {
                this._flagInativo = value;
                this._modified = true;
            }
        }
        #endregion

        #region QuantidadeUsuarioAdicional
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public int? QuantidadeUsuarioAdicional
        {
            get { return this._quantidadeUsuarioAdicional; }
            set
            {
                this._quantidadeUsuarioAdicional = value;
                this._modified = true;
            }
        }
        #endregion

        #region QuantidadeFuncionarios
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public int QuantidadeFuncionarios
        {
            get { return this._quantidadeFuncionarios; }
            set
            {
                this._quantidadeFuncionarios = value;
                this._modified = true;
            }
        }
        #endregion

        #region DescricaoIP
        /// <summary>
        /// Tamanho do campo: 15.
        /// Campo obrigatório.
        /// </summary>
        public string DescricaoIP
        {
            get { return this._descricaoIP; }
            set
            {
                this._descricaoIP = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagOfereceCursos
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagOfereceCursos
        {
            get { return this._flagOfereceCursos; }
            set
            {
                this._flagOfereceCursos = value;
                this._modified = true;
            }
        }
        #endregion

        #region DescricaoPaginaFacebook
        /// <summary>
        /// Tamanho do campo: 200.
        /// Campo opcional.
        /// </summary>
        public string DescricaoPaginaFacebook
        {
            get { return this._descricaoPaginaFacebook; }
            set
            {
                this._descricaoPaginaFacebook = value;
                this._modified = true;
            }
        }
        #endregion

        #region NumeroComercial2
        /// <summary>
        /// Tamanho do campo: 10.
        /// Campo opcional.
        /// </summary>
        public string NumeroComercial2
        {
            get { return this._numeroComercial2; }
            set
            {
                this._numeroComercial2 = value;
                this._modified = true;
            }
        }
        #endregion

        #region DescricaoLocalizacao
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public SqlGeography DescricaoLocalizacao
        {
            get { return this._descricaoLocalizacao; }
            set
            {
                this._descricaoLocalizacao = value;
                this._modified = true;
            }
        }
        #endregion

        #region TipoParceiro
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public TipoParceiro TipoParceiro
        {
            get { return this._tipoParceiro; }
            set
            {
                this._tipoParceiro = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Construtores
        public Filial()
        {
        }

        public Filial(int idFilial)
        {
            this._idFilial = idFilial;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO TAB_Filial (Flg_Matriz, Num_CNPJ, Raz_Social, Nme_Fantasia, Idf_CNAE_Principal, Idf_Natureza_Juridica, Idf_Endereco, End_Site, Num_DDD_Comercial, Num_Comercial, Flg_Inativo, Dta_Cadastro, Dta_Alteracao, Qtd_Usuario_Adicional, Qtd_Funcionarios, Des_IP, Flg_Oferece_Cursos, Idf_Situacao_Filial, Des_Pagina_Facebook, Num_Comercial2, Des_Localizacao, Idf_Tipo_Parceiro) VALUES (@Flg_Matriz, @Num_CNPJ, @Raz_Social, @Nme_Fantasia, @Idf_CNAE_Principal, @Idf_Natureza_Juridica, @Idf_Endereco, @End_Site, @Num_DDD_Comercial, @Num_Comercial, @Flg_Inativo, @Dta_Cadastro, @Dta_Alteracao, @Qtd_Usuario_Adicional, @Qtd_Funcionarios, @Des_IP, @Flg_Oferece_Cursos, @Idf_Situacao_Filial, @Des_Pagina_Facebook, @Num_Comercial2, @Des_Localizacao, @Idf_Tipo_Parceiro);SET @Idf_Filial = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE TAB_Filial SET Flg_Matriz = @Flg_Matriz, Num_CNPJ = @Num_CNPJ, Raz_Social = @Raz_Social, Nme_Fantasia = @Nme_Fantasia, Idf_CNAE_Principal = @Idf_CNAE_Principal, Idf_Natureza_Juridica = @Idf_Natureza_Juridica, Idf_Endereco = @Idf_Endereco, End_Site = @End_Site, Num_DDD_Comercial = @Num_DDD_Comercial, Num_Comercial = @Num_Comercial, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro, Dta_Alteracao = @Dta_Alteracao, Qtd_Usuario_Adicional = @Qtd_Usuario_Adicional, Qtd_Funcionarios = @Qtd_Funcionarios, Des_IP = @Des_IP, Flg_Oferece_Cursos = @Flg_Oferece_Cursos, Idf_Situacao_Filial = @Idf_Situacao_Filial, Des_Pagina_Facebook = @Des_Pagina_Facebook, Num_Comercial2 = @Num_Comercial2, Des_Localizacao = @Des_Localizacao, Idf_Tipo_Parceiro = @Idf_Tipo_Parceiro WHERE Idf_Filial = @Idf_Filial";
        private const string SPDELETE = "DELETE FROM TAB_Filial WHERE Idf_Filial = @Idf_Filial";
        private const string SPSELECTID = "SELECT * FROM TAB_Filial WITH(NOLOCK) WHERE Idf_Filial = @Idf_Filial";
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
            parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Flg_Matriz", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Num_CNPJ", SqlDbType.Decimal, 9));
            parms.Add(new SqlParameter("@Raz_Social", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Nme_Fantasia", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Idf_CNAE_Principal", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Natureza_Juridica", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Endereco", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@End_Site", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Num_DDD_Comercial", SqlDbType.Char, 2));
            parms.Add(new SqlParameter("@Num_Comercial", SqlDbType.Char, 10));
            parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Dta_Alteracao", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Qtd_Usuario_Adicional", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Qtd_Funcionarios", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Des_IP", SqlDbType.Char, 15));
            parms.Add(new SqlParameter("@Flg_Oferece_Cursos", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Idf_Situacao_Filial", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Des_Pagina_Facebook", SqlDbType.VarChar, 200));
            parms.Add(new SqlParameter("@Num_Comercial2", SqlDbType.Char, 10));
            parms.Add(new SqlParameter { ParameterName = "@Des_Localizacao", UdtTypeName = "Geography" });
            parms.Add(new SqlParameter("@Idf_Tipo_Parceiro", SqlDbType.Int, 4));
            return (parms);
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
            parms[0].Value = this._idFilial;
            parms[1].Value = this._flagMatriz;

            if (this._numeroCNPJ.HasValue)
                parms[2].Value = this._numeroCNPJ;
            else
                parms[2].Value = DBNull.Value;

            parms[3].Value = this._razaoSocial;
            parms[4].Value = this._nomeFantasia;

            if (this._cNAEPrincipal != null)
                parms[5].Value = this._cNAEPrincipal.IdCNAESubClasse;
            else
                parms[5].Value = DBNull.Value;


            if (this._naturezaJuridica != null)
                parms[6].Value = this._naturezaJuridica.IdNaturezaJuridica;
            else
                parms[6].Value = DBNull.Value;


            if (this._endereco != null)
                parms[7].Value = this._endereco.IdEndereco;
            else
                parms[7].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._enderecoSite))
                parms[8].Value = this._enderecoSite;
            else
                parms[8].Value = DBNull.Value;

            parms[9].Value = this._numeroDDDComercial;
            parms[10].Value = this._numeroComercial;
            parms[11].Value = this._flagInativo;

            if (this._quantidadeUsuarioAdicional.HasValue)
                parms[14].Value = this._quantidadeUsuarioAdicional;
            else
                parms[14].Value = DBNull.Value;

            parms[15].Value = this._quantidadeFuncionarios;
            parms[16].Value = this._descricaoIP;
            parms[17].Value = this._flagOfereceCursos;
            parms[18].Value = this._situacaoFilial.IdSituacaoFilial;

            if (!String.IsNullOrEmpty(this._descricaoPaginaFacebook))
                parms[19].Value = this._descricaoPaginaFacebook;
            else
                parms[19].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._numeroComercial2))
                parms[20].Value = this._numeroComercial2;
            else
                parms[20].Value = DBNull.Value;


            if (this._descricaoLocalizacao != null)
                parms[21].Value = this._descricaoLocalizacao;
            else
                parms[21].Value = SqlGeography.Null;


            if (this._tipoParceiro != null)
                parms[22].Value = this._tipoParceiro.IdTipoParceiro;
            else
                parms[22].Value = DBNull.Value;


            if (!this._persisted)
            {
                parms[0].Direction = ParameterDirection.Output;
                this._dataCadastro = DateTime.Now;
            }
            else
            {
                parms[0].Direction = ParameterDirection.Input;
            }
            parms[12].Value = this._dataCadastro;
            this._dataAlteracao = DateTime.Now;
            parms[13].Value = this._dataAlteracao;
        }
        #endregion

        #region Insert
        /// <summary>
        /// Método utilizado para inserir uma instância de Filial no banco de dados.
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
                        this._idFilial = Convert.ToInt32(cmd.Parameters["@Idf_Filial"].Value);
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
        /// Método utilizado para inserir uma instância de Filial no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idFilial = Convert.ToInt32(cmd.Parameters["@Idf_Filial"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de Filial no banco de dados.
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
        /// Método utilizado para atualizar uma instância de Filial no banco de dados, dentro de uma transação.
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
        /// Método utilizado para salvar uma instância de Filial no banco de dados.
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
        /// Método utilizado para salvar uma instância de Filial no banco de dados, dentro de uma transação.
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
        /// Método utilizado para excluir uma instância de Filial no banco de dados.
        /// </summary>
        /// <param name="idFilial">Chave do registro.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idFilial)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));

            parms[0].Value = idFilial;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }

        /// <summary>
        /// Método utilizado para excluir uma instância de Filial no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idFilial">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idFilial, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));

            parms[0].Value = idFilial;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }

        /// <summary>
        /// Método utilizado para excluir uma lista de Filial no banco de dados.
        /// </summary>
        /// <param name="idFilial">Lista de chaves.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(List<int> idFilial)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from TAB_Filial where Idf_Filial in (";

            for (int i = 0; i < idFilial.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idFilial[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idFilial">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idFilial)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));

            parms[0].Value = idFilial;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }

        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idFilial">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idFilial, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));

            parms[0].Value = idFilial;

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
            int inicio = ((paginaCorrente - 1)*tamanhoPagina) + 1;
            int fim = paginaCorrente*tamanhoPagina;

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Fil.Idf_Filial, Fil.Flg_Matriz, Fil.Num_CNPJ, Fil.Raz_Social, Fil.Nme_Fantasia, Fil.Idf_CNAE_Principal, Fil.Idf_Natureza_Juridica, Fil.Idf_Endereco, Fil.End_Site, Fil.Num_DDD_Comercial, Fil.Num_Comercial, Fil.Flg_Inativo, Fil.Dta_Cadastro, Fil.Dta_Alteracao, Fil.Qtd_Usuario_Adicional, Fil.Qtd_Funcionarios, Fil.Des_IP, Fil.Flg_Oferece_Cursos, Fil.Idf_Situacao_Filial, Fil.Des_Pagina_Facebook, Fil.Num_Comercial2, Fil.Des_Localizacao, Fil.Idf_Tipo_Parceiro FROM TAB_Filial Fil";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de Filial a partir do banco de dados.
        /// </summary>
        /// <param name="idFilial">Chave do registro.</param>
        /// <returns>Instância de Filial.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static Filial LoadObject(int idFilial)
        {
            using (IDataReader dr = LoadDataReader(idFilial))
            {
                Filial objFilial = new Filial();
                if (SetInstance(dr, objFilial))
                    return objFilial;
            }
            throw (new RecordNotFoundException(typeof (Filial)));
        }

        /// <summary>
        /// Método utilizado para retornar uma instância de Filial a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idFilial">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de Filial.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static Filial LoadObject(int idFilial, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idFilial, trans))
            {
                Filial objFilial = new Filial();
                if (SetInstance(dr, objFilial))
                    return objFilial;
            }
            throw (new RecordNotFoundException(typeof (Filial)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de Filial a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idFilial))
            {
                return SetInstance(dr, this);
            }
        }

        /// <summary>
        /// Método utilizado para completar uma instância de Filial a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idFilial, trans))
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
        /// <param name="objFilial">Instância a ser manipulada.</param>
        /// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance(IDataReader dr, Filial objFilial, bool dispose = true)
        {
            try
            {
                if (dr.Read())
                {
                    objFilial._idFilial = Convert.ToInt32(dr["Idf_Filial"]);
                    objFilial._flagMatriz = Convert.ToBoolean(dr["Flg_Matriz"]);
                    if (dr["Num_CNPJ"] != DBNull.Value)
                        objFilial._numeroCNPJ = Convert.ToDecimal(dr["Num_CNPJ"]);
                    objFilial._razaoSocial = Convert.ToString(dr["Raz_Social"]);
                    objFilial._nomeFantasia = Convert.ToString(dr["Nme_Fantasia"]);
                    if (dr["Idf_CNAE_Principal"] != DBNull.Value)
                        objFilial._cNAEPrincipal = new CNAESubClasse(Convert.ToInt32(dr["Idf_CNAE_Principal"]));
                    if (dr["Idf_Natureza_Juridica"] != DBNull.Value)
                        objFilial._naturezaJuridica = new NaturezaJuridica(Convert.ToInt32(dr["Idf_Natureza_Juridica"]));
                    if (dr["Idf_Endereco"] != DBNull.Value)
                        objFilial._endereco = new Endereco(Convert.ToInt32(dr["Idf_Endereco"]));
                    if (dr["End_Site"] != DBNull.Value)
                        objFilial._enderecoSite = Convert.ToString(dr["End_Site"]);
                    objFilial._numeroDDDComercial = Convert.ToString(dr["Num_DDD_Comercial"]);
                    objFilial._numeroComercial = Convert.ToString(dr["Num_Comercial"]);
                    objFilial._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                    objFilial._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                    objFilial._dataAlteracao = Convert.ToDateTime(dr["Dta_Alteracao"]);
                    if (dr["Qtd_Usuario_Adicional"] != DBNull.Value)
                        objFilial._quantidadeUsuarioAdicional = Convert.ToInt32(dr["Qtd_Usuario_Adicional"]);
                    objFilial._quantidadeFuncionarios = Convert.ToInt32(dr["Qtd_Funcionarios"]);
                    objFilial._descricaoIP = Convert.ToString(dr["Des_IP"]);
                    objFilial._flagOfereceCursos = Convert.ToBoolean(dr["Flg_Oferece_Cursos"]);
                    objFilial._situacaoFilial = new SituacaoFilial(Convert.ToInt32(dr["Idf_Situacao_Filial"]));
                    if (dr["Des_Pagina_Facebook"] != DBNull.Value)
                        objFilial._descricaoPaginaFacebook = Convert.ToString(dr["Des_Pagina_Facebook"]);
                    if (dr["Num_Comercial2"] != DBNull.Value)
                        objFilial._numeroComercial2 = Convert.ToString(dr["Num_Comercial2"]);
                    if (dr["Des_Localizacao"] != DBNull.Value)
                        objFilial._descricaoLocalizacao = (SqlGeography) (dr["Des_Localizacao"]);
                    if (dr["Idf_Tipo_Parceiro"] != DBNull.Value)
                        objFilial._tipoParceiro = new TipoParceiro(Convert.ToInt32(dr["Idf_Tipo_Parceiro"]));

                    objFilial._persisted = true;
                    objFilial._modified = false;

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