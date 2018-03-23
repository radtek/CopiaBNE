//-- Data: 02/03/2010 09:20
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
    public partial class Formacao // Tabela: BNE_Formacao
    {

        #region Consultas
        private const string Spselectboolformacao = "SELECT COUNT(Idf_Formacao) FROM BNE_Formacao WITH(NOLOCK) WHERE Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica AND Flg_Inativo = 0";

        private const string Spselectporpessoafisica = @"
        SELECT  F.Idf_Formacao ,
                E.Des_BNE ,
                ISNULL(C.Des_Curso , F.Des_Curso) AS Des_Curso ,
                ISNULL(FT.Nme_Fonte, F.Des_Fonte) AS Nme_Fonte ,
                FT.Sig_Fonte ,
                SF.Des_Situacao_Formacao,
                CI.Nme_Cidade,
                CI.Sig_Estado,
                F.*
        FROM    BNE_Formacao F
                LEFT JOIN plataforma.TAB_Escolaridade E ON F.Idf_Escolaridade = E.Idf_Escolaridade
                LEFT JOIN TAB_Curso C ON F.Idf_Curso = C.Idf_Curso
                LEFT JOIN TAB_Fonte FT ON F.Idf_Fonte = FT.Idf_Fonte
                LEFT JOIN BNE_Situacao_Formacao SF ON F.Idf_Situacao_Formacao = SF.Idf_Situacao_Formacao
                LEFT JOIN plataforma.TAB_Cidade CI ON F.Idf_Cidade = CI.Idf_Cidade
        WHERE   Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica 
                AND F.Flg_Inativo = 0 
                AND E.Flg_BNE = 1
        ";
        
        private const string Spselecttoppessoafisicaescolaridade = "SELECT * FROM BNE_Formacao WITH(NOLOCK) WHERE Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica  AND Idf_Escolaridade = @Idf_Escolaridade AND Flg_Inativo = 0";
        
        private const string Spselectmaiorformacaoporpessoafisica = @"
        SELECT  TOP 1 F.* 
        FROM    BNE_Formacao F WITH(NOLOCK)
        WHERE   F.Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica AND 
                F.Flg_Inativo = 0 AND 
                F.Idf_Escolaridade <> 18 /* Diferente de Aperfeicamento */
        ORDER BY Idf_Escolaridade DESC, F.Dta_Cadastro DESC";

        private const string SPSELECTULTIMAFORMACAOCANDIDATO = @"
        SELECT  TOP 1 
                Idf_Formacao,
                Idf_Escolaridade,
                Des_Fonte,
                Des_Curso 
        FROM    BNE_Formacao 
		WHERE   Idf_Pessoa_Fisica=@Idf_Pessoa_Fisica AND
		        Flg_Inativo = 0 AND
		        Idf_Escolaridade in (6,8,10,11) AND
		        (Ano_Conclusao < DATEPART(YEAR,GETDATE()) or Ano_Conclusao IS NULL)
                ORDER BY Dta_Alteracao DESC";

        private const string SP_DELETE_FORMACOES = @"
        DELETE FROM BNE.BNE_Formacao
        WHERE Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica
        AND Idf_Escolaridade <> 18;
        ";

        private const string SP_DELETE_OUTROS_CURSOS = @"
        DELETE FROM BNE.BNE_Formacao
        WHERE Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica
        AND Idf_Escolaridade = 18;
        ";
        #endregion

        #region [spInativarFormacaoPorPessoaFisica]
        private const string spInativarFormacaoPorPessoaFisica = @"update Bne_Formacao set flg_inativo = 1 where Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica ";
        #endregion

        #region ExisteFormacaoInformada
        /// <summary>
        /// Método utilizado para saber se um determinada pessoa fisica possue pelo menos uma formacao relacionada a ele.
        /// </summary>
        /// <param name="idPessoaFisica">Codigo identificador de uma pessoa física</param>
        /// <param name="trans">Transacao</param>
        /// <returns></returns>
        public static bool ExisteFormacaoInformada(int idPessoaFisica, SqlTransaction trans = null)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int));
            parms[0].Value = idPessoaFisica;

            if (trans != null)
            {
                return Convert.ToInt32(DataAccessLayer.ExecuteScalar(trans, CommandType.Text, Spselectboolformacao, parms)) > 0;
            }

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spselectboolformacao, parms)) > 0;
        }
        #endregion

        #region Insert
        /// <summary>
        /// Método utilizado para inserir uma instância de Formacao no banco de dados.
        /// </summary>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Insert()
        {
            List<SqlParameter> parms = GetParameters();

            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        //Salvando caso seja uma nova fonte.
                        if (this.Fonte != null)
                            this.Fonte.Save(trans);

                        //Salvando curso caso seja uma nova fonte.
                        if (this.Curso != null)
                            this.Curso.Save(trans);

                        if (this.Fonte != null && this.Curso != null)
                        {
                            CursoFonte objCursoFonte;
                            if (!CursoFonte.LoadObject(this.Fonte.IdFonte, this.Curso.IdCurso, trans, out objCursoFonte))
                            {
                                objCursoFonte = new CursoFonte();
                                objCursoFonte.Fonte = this.Fonte;
                                objCursoFonte.DataAtualizacao = DateTime.Now;
                                objCursoFonte.Curso = this.Curso;
                                objCursoFonte.Save(trans);
                            }
                        }

                        SetParameters(parms);

                        SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
                        this._idFormacao = Convert.ToInt32(cmd.Parameters["@Idf_Formacao"].Value);
                        cmd.Parameters.Clear();
                        this._persisted = true;
                        this._modified = false;

                        //Atualizando a PF com a maior escolaridade
                        Formacao objFormacao;
                        if (Formacao.CarregarMaiorFormacaoPorPessoaFisica(this._pessoaFisica, out objFormacao, trans))
                        {
                            this.PessoaFisica.CompleteObject(trans);
                            this.PessoaFisica.Escolaridade = objFormacao.Escolaridade;
                            this.PessoaFisica.Save(trans);
                        }

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
        /// Método utilizado para inserir uma instância de Formacao no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            //Salvando caso seja uma nova fonte.
            if (this.Fonte != null)
                this.Fonte.Save(trans);

            //Salvando curso caso seja uma nova fonte.
            if (this.Curso != null)
                this.Curso.Save(trans);

            if (this.Fonte != null && this.Curso != null)
            {
                CursoFonte objCursoFonte;
                if (!CursoFonte.LoadObject(this.Fonte.IdFonte, this.Curso.IdCurso, trans, out objCursoFonte))
                {

                    objCursoFonte = new CursoFonte();
                    objCursoFonte.Fonte = this.Fonte;
                    objCursoFonte.Curso = this.Curso;
                    objCursoFonte.DataAtualizacao = DateTime.Now;
                    objCursoFonte.Save(trans);
                }
            }
           
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idFormacao = Convert.ToInt32(cmd.Parameters["@Idf_Formacao"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;

            //Atualizando a PF com a maior escolaridade
            Formacao objFormacao;
            if (Formacao.CarregarMaiorFormacaoPorPessoaFisica(this._pessoaFisica, out objFormacao, trans))
            {
                this.PessoaFisica.CompleteObject(trans);
                this.PessoaFisica.Escolaridade = objFormacao.Escolaridade;
                this.PessoaFisica.Save(trans);
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de Formacao no banco de dados.
        /// </summary>
        /// <remarks>Gieyson Stlemak</remarks>
        private void Update()
        {
            if (this._modified)
            {
                List<SqlParameter> parms = GetParameters();
                using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            //Salvando caso seja uma nova fonte.
                            if (this.Fonte != null)
                                this.Fonte.Save(trans);

                            //Salvando curso caso seja uma nova fonte.
                            if (this.Curso != null)
                                this.Curso.Save(trans);

                            if (this.Fonte != null && this.Curso != null)
                            {
                                CursoFonte objCursoFonte;
                                if (!CursoFonte.LoadObject(this.Fonte.IdFonte, this.Curso.IdCurso, trans, out objCursoFonte))
                                {
                                    objCursoFonte = new CursoFonte();
                                    objCursoFonte.Fonte = this.Fonte;
                                    objCursoFonte.Curso = this.Curso;
                                    objCursoFonte.DataAtualizacao = DateTime.Now;
                                    objCursoFonte.Save(trans);
                                }
                            }
                            
                            SetParameters(parms);
                            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPUPDATE, parms);
                            this._modified = false;

                            //Atualizando a PF com a maior escolaridade
                            Formacao objFormacao;
                            if (Formacao.CarregarMaiorFormacaoPorPessoaFisica(this._pessoaFisica, out objFormacao, trans))
                            {
                                this.PessoaFisica.CompleteObject(trans);
                                this.PessoaFisica.Escolaridade = objFormacao.Escolaridade;
                                this.PessoaFisica.Save(trans);
                            }
                            
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
        }
        /// <summary>
        /// Método utilizado para atualizar uma instância de Formacao no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stlemak</remarks>
        private void Update(SqlTransaction trans)
        {
            if (this._modified)
            {
                List<SqlParameter> parms = GetParameters();
                //Salvando caso seja uma nova fonte.
                if (this.Fonte != null)
                    this.Fonte.Save(trans);

                //Salvando curso caso seja uma nova fonte.
                if (this.Curso != null)
                    this.Curso.Save(trans);

                if (this.Fonte != null && this.Curso != null)
                {
                    CursoFonte objCursoFonte;
                    if (!CursoFonte.LoadObject(this.Fonte.IdFonte, this.Curso.IdCurso, trans, out objCursoFonte))
                    {
                        objCursoFonte = new CursoFonte();
                        objCursoFonte.Fonte = this.Fonte;
                        objCursoFonte.Curso = this.Curso;
                        objCursoFonte.Save(trans);
                    }
                }

                SetParameters(parms);
                DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPUPDATE, parms);
                this._modified = false;

                //Atualizando a PF com a maior escolaridade
                Formacao objFormacao;
                if (Formacao.CarregarMaiorFormacaoPorPessoaFisica(this._pessoaFisica, out objFormacao, trans))
                {
                    this.PessoaFisica.CompleteObject(trans);
                    this.PessoaFisica.Escolaridade = objFormacao.Escolaridade;
                    this.PessoaFisica.Save(trans);
                }
            }
        }
        #endregion

        #region DeleteOutrosCursos
        /// <summary>
        /// Apaga todas as formações da pessoa fisica que sejam de aperfeicoamento/complementares
        /// </summary>
        /// <param name="idPessoaFisica"></param>
        public static void DeleteOutrosCursos(int idPessoaFisica)
        {
            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    DeleteOutrosCursos(idPessoaFisica, trans);
                }
            }
        }

        /// <summary>
        /// Apaga todas as formações da pessoa fisica que sejam de aperfeicoamento/complementares
        /// </summary>
        /// <param name="idPessoaFisica"></param>
        public static void DeleteOutrosCursos(int idPessoaFisica, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms[0].Value = idPessoaFisica;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SP_DELETE_OUTROS_CURSOS, parms);
        }
        #endregion DeleteFormacaoNaoEspecializacao

        #region DeleteFormacoes
        /// <summary>
        /// Apaga todas as formações da pessoa fisica que NAO sejam de aperfeicoamento/complementares
        /// </summary>
        /// <param name="idPessoaFisica"></param>
        public static void DeleteFormacoes(int idPessoaFisica)
        {
            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    DeleteFormacoes(idPessoaFisica, trans);
                }
            }
        }

        /// <summary>
        /// Apaga todas as formações da pessoa fisica que NAO sejam de aperfeicoamento/complementares
        /// </summary>
        /// <param name="idPessoaFisica"></param>
        public static void DeleteFormacoes(int idPessoaFisica, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms[0].Value = idPessoaFisica;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SP_DELETE_FORMACOES, parms);
        }
        #endregion DeleteFormacoes


        #region ListarFormacaoList
        /// <summary>
        /// Método responsável por retornar uma List com todas as instâncias de Formacao dependendo do parametro cursosComplementares
        /// </summary>
        /// <param name="idPessoaFisica">Código identificador de uma pessoa física</param>
        /// <param name="cursosComplementares">Bool, true para listar apenas cursos complementares e false para listar apenas formacoes</param>
        /// <returns></returns>
        public static List<Formacao> ListarFormacaoList(int idPessoaFisica, bool cursosComplementares, bool todos = false)
        {
            List<Formacao> listFormacao = new List<Formacao>();

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms[0].Value = idPessoaFisica;

            string query = Spselectporpessoafisica;

            if (cursosComplementares && !todos)
                query += " AND F.Idf_Escolaridade = 18 -- Cursos Complementares";
            else if(!todos)
                query += " AND F.Idf_Escolaridade <> 18 -- Outros Cursos";

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, query, parms))
            {
                while (dr.Read())
                    listFormacao.Add(Formacao.LoadObject(Convert.ToInt32(dr["Idf_Formacao"])));

                if (!dr.IsClosed)
                    dr.Close();
            }

            return listFormacao;
        }
        #endregion

        #region ListarFormacao
        /// <summary>
        /// Método responsável por retornar uma List com todas as instâncias de Formacao
        /// </summary>
        /// <param name="idPessoaFisica">Código identificador de uma pessoa física</param>
        /// <param name="cursosComplementares">Bool, true para listar apenas formacao e false para listar apenas cursos complementares</param>
        /// <returns></returns>
        public static IDataReader ListarFormacao(int idPessoaFisica)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms[0].Value = idPessoaFisica;

            string query = Spselectporpessoafisica;

            return DataAccessLayer.ExecuteReader(CommandType.Text, query, parms);
        }
        /// <summary>
        /// Método responsável por retornar uma DataTable com todas as instâncias de Formacao dependendo do parametro cursosComplementares
        /// </summary>
        /// <param name="idPessoaFisica">Código identificador de uma pessoa física</param>
        /// <param name="cursosComplementares">Bool, true para listar apenas formacao e false para listar apenas cursos complementares</param>
        /// <returns></returns>
        public static DataTable ListarFormacao(int idPessoaFisica, bool graducao, bool especializacao, bool cursosComplementares)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms[0].Value = idPessoaFisica;

            string query = Spselectporpessoafisica;

            if (graducao)
                query += " AND (F.Idf_Escolaridade >= 4 AND F.Idf_Escolaridade <= 13)";
            else if (especializacao)
                query += " AND (F.Idf_Escolaridade >= 14 AND F.Idf_Escolaridade <= 17)";
            else if (cursosComplementares)
                query += " AND F.Idf_Escolaridade = 18 -- Cursos Complementares";

            query += "ORDER BY Ano_Conclusao DESC";

            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, query, parms).Tables[0];
        }
        #endregion

        #region ListarFormacaoDT
        /// <summary>
        /// Retorna um DataTable com as formações da pessoa fisica informada
        /// </summary>
        /// <param name="idPessoaFisica"></param>
        /// <returns></returns>
        public static DataTable ListarFormacaoDT(int idPessoaFisica)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms[0].Value = idPessoaFisica;

            string query = Spselectporpessoafisica;

            query += @" ORDER BY CASE WHEN F.Idf_Escolaridade = 18 THEN 1 ELSE 0 END, F.Idf_Escolaridade DESC, F.Ano_Conclusao DESC";

            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, query, parms).Tables[0];
        }
        #endregion

        #region ListarFormacaoMenosCursosComplementares
        /// <summary>
        /// Método responsável por retornar uma DataTable com todas as instâncias de Formacao menos cursos complementares
        /// </summary>
        /// <param name="idPessoaFisica">Código identificador de uma pessoa física</param>
        /// <param name="cursosComplementares">Bool, true para listar apenas formacao e false para listar apenas cursos complementares</param>
        /// <returns></returns>
        public static DataTable ListarFormacaoMenosCursosComplementares(int idPessoaFisica)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms[0].Value = idPessoaFisica;

            string query = Spselectporpessoafisica;

            query += " AND F.Idf_Escolaridade <> 18 -- Cursos Complementares";

            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, query, parms).Tables[0];
        }
        #endregion

        #region CarregarPorPessoaFisicaEscolaridade
        /// <summary>
        /// Método responsável por carregar um objeto do banco de dados que dizem respeito ao idenficador da
        /// pessoa física e o idenficador da escolaridade.
        /// </summary>
        /// <param name="idPessoaFisica">Identificador da Pessoa Física</param>
        /// <param name="idEscolaridade">Identificador da Escolaridade</param>
        /// <returns>objFormacao</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorPessoaFisicaEscolaridade(int idPessoaFisica, int idEscolaridade, out Formacao objFormacao)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Escolaridade", SqlDbType.Int, 4));
            parms[0].Value = idPessoaFisica;
            parms[1].Value = idEscolaridade;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselecttoppessoafisicaescolaridade, parms))
            {
                objFormacao = new Formacao();
                if (SetInstance(dr, objFormacao))
                    return true;
            }
            objFormacao = null;
            return false;
        }
        public static bool CarregarPorPessoaFisicaEscolaridade(SqlTransaction trans, int idPessoaFisica, int idEscolaridade, out Formacao objFormacao)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Escolaridade", SqlDbType.Int, 4));
            parms[0].Value = idPessoaFisica;
            parms[1].Value = idEscolaridade;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselecttoppessoafisicaescolaridade, parms))
            {
                objFormacao = new Formacao();
                if (SetInstance(dr, objFormacao))
                    return true;
            }
            objFormacao = null;
            return false;
        }
        #endregion

        #region CarregarMaiorFormacaoPorPessoaFisica
        /// <summary>
        /// Método responsável por carregar a maior formação de uma Pessoa Fisica
        /// </summary>
        /// <param name="objPessoaFisica">Identificador da Pessoa Física</param>
        /// <param name="objFormacao">Objeto de retorno Formação</param>
        /// <returns>Booleano</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarMaiorFormacaoPorPessoaFisica(PessoaFisica objPessoaFisica, out Formacao objFormacao, SqlTransaction trans)
        {
            var parms = new List<SqlParameter> {new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4)};
            parms[0].Value = objPessoaFisica.IdPessoaFisica;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectmaiorformacaoporpessoafisica, parms))
            {
                objFormacao = new Formacao();
                if (SetInstance(dr, objFormacao))
                    return true;
            }
            objFormacao = null;
            return false;
        }
        public static bool CarregarMaiorFormacaoPorPessoaFisica(PessoaFisica objPessoaFisica, out Formacao objFormacao)
        {
            var parms = new List<SqlParameter> { new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4) };
            parms[0].Value = objPessoaFisica.IdPessoaFisica;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectmaiorformacaoporpessoafisica, parms))
            {
                objFormacao = new Formacao();
                if (SetInstance(dr, objFormacao))
                    return true;
            }
            objFormacao = null;
            return false;
        }
        #endregion

        #region RetornarUltimaFormacaoCandidato
        /// <summary>
        /// Método responsável por carregar a última formação do candidato
        /// </summary>
        /// <param name="idPessoaFisica"></param>
        /// <returns></returns>
        public static IDataReader RetornarUltimaFormacaoCandidato(int idPessoaFisica)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));

            parms[0].Value = idPessoaFisica;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTULTIMAFORMACAOCANDIDATO, parms);
        }
        #endregion

        /// <summary>
        /// Salvar Formacao
        /// </summary>
        /// <param name="objFormacao"></param>
        #region SalvarFormacao
        public void SalvarFormacao(Formacao objFormacao)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        objFormacao.Save();

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

        #endregion

        #region Inserção em Massa
        /// <summary>
        /// Crie uma tabela para inserir em massa.
        /// Passe um DataTable nulo para e a classe em populada. 
        /// Ela irá instanciar um novo DataTable já com colunas definidas apartir dos parâmetros sql definidos na classe.
        /// Os valores setados nas propriedades são transformados em uma linha na tabela.
        /// </summary>
        /// <param name="dt"></param>
        public void AddBulkTable(ref DataTable dt)
        {
            DataAccessLayer.AddBulkTable(ref dt, this);
        }

        /// <summary>
        /// Realiza inserção em massa.
        /// </summary>
        /// <param name="dt">Tabela criada pelo método AddBulkTable</param>
        /// <param name="trans">Transação</param>
        public static void SaveBulkTable(DataTable dt, SqlTransaction trans)
        {
            DataAccessLayer.SaveBulkTable(dt, "BNE_Formacao", trans);
        }
        #endregion

        #region InativarFormacaoPorPessoaFisica
        public static void InativarFormacaoPorPessoaFisica(int Idf_Pessoa_Fisica, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Pessoa_Fisica", SqlDbType = SqlDbType.Int, Size = 4, Value = Idf_Pessoa_Fisica}
                };

            if (trans != null)
                DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, spInativarFormacaoPorPessoaFisica, parms);
            else
                DataAccessLayer.ExecuteNonQuery(CommandType.Text, spInativarFormacaoPorPessoaFisica, parms);
        }
        #endregion

        #region [ Migration Mapping ]
        /// <summary>
        /// Médodos e atributos auxiliares à migração de dados para o novo
        /// domínio.
        /// </summary>
        public int MigrationId
        {
            set
            {
                this._idFormacao = value;
            }
            get { return this._idFormacao; }
        }
        #endregion
    }
}