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
        ORDER BY Idf_Escolaridade DESC";

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

        #endregion

        #region ExisteFormacaoInformada
        /// <summary>
        /// M�todo utilizado para saber se um determinada pessoa fisica possue pelo menos uma formacao relacionada a ele.
        /// </summary>
        /// <param name="idPessoaFisica">Codigo identificador de uma pessoa f�sica</param>
        /// <returns></returns>
        public static bool ExisteFormacaoInformada(int idPessoaFisica)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int));
            parms[0].Value = idPessoaFisica;

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spselectboolformacao, parms)) > 0;
        }
        #endregion

        #region Insert
        /// <summary>
        /// M�todo utilizado para inserir uma inst�ncia de Formacao no banco de dados.
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
        /// M�todo utilizado para inserir uma inst�ncia de Formacao no banco de dados, dentro de uma transa��o.
        /// </summary>
        /// <param name="trans">Transa��o existente no banco de dados.</param>
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
        /// M�todo utilizado para atualizar uma inst�ncia de Formacao no banco de dados.
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
        /// M�todo utilizado para atualizar uma inst�ncia de Formacao no banco de dados, dentro de uma transa��o.
        /// </summary>
        /// <param name="trans">Transa��o existente no banco de dados.</param>
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

        #region ListarFormacaoList
        /// <summary>
        /// M�todo respons�vel por retornar uma List com todas as inst�ncias de Formacao dependendo do parametro cursosComplementares
        /// </summary>
        /// <param name="idPessoaFisica">C�digo identificador de uma pessoa f�sica</param>
        /// <param name="cursosComplementares">Bool, true para listar apenas formacao e false para listar apenas cursos complementares</param>
        /// <returns></returns>
        public static List<Formacao> ListarFormacaoList(int idPessoaFisica, bool cursosComplementares)
        {
            List<Formacao> listFormacao = new List<Formacao>();

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms[0].Value = idPessoaFisica;

            string query = Spselectporpessoafisica;

            if (cursosComplementares)
                query += " AND F.Idf_Escolaridade = 18 -- Cursos Complementares";
            else
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
        /// M�todo respons�vel por retornar uma List com todas as inst�ncias de Formacao
        /// </summary>
        /// <param name="idPessoaFisica">C�digo identificador de uma pessoa f�sica</param>
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
        /// M�todo respons�vel por retornar uma DataTable com todas as inst�ncias de Formacao dependendo do parametro cursosComplementares
        /// </summary>
        /// <param name="idPessoaFisica">C�digo identificador de uma pessoa f�sica</param>
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
        /// Retorna um DataTable com as forma��es da pessoa fisica informada
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
        /// M�todo respons�vel por retornar uma DataTable com todas as inst�ncias de Formacao menos cursos complementares
        /// </summary>
        /// <param name="idPessoaFisica">C�digo identificador de uma pessoa f�sica</param>
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
        /// M�todo respons�vel por carregar um objeto do banco de dados que dizem respeito ao idenficador da
        /// pessoa f�sica e o idenficador da escolaridade.
        /// </summary>
        /// <param name="idPessoaFisica">Identificador da Pessoa F�sica</param>
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
        /// M�todo respons�vel por carregar a maior forma��o de uma Pessoa Fisica
        /// </summary>
        /// <param name="objPessoaFisica">Identificador da Pessoa F�sica</param>
        /// <param name="objFormacao">Objeto de retorno Forma��o</param>
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
        /// M�todo respons�vel por carregar a �ltima forma��o do candidato
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

        #region Inser��o em Massa
        /// <summary>
        /// Crie uma tabela para inserir em massa.
        /// Passe um DataTable nulo para e a classe em populada. 
        /// Ela ir� instanciar um novo DataTable j� com colunas definidas apartir dos par�metros sql definidos na classe.
        /// Os valores setados nas propriedades s�o transformados em uma linha na tabela.
        /// </summary>
        /// <param name="dt"></param>
        public void AddBulkTable(ref DataTable dt)
        {
            DataAccessLayer.AddBulkTable(ref dt, this);
        }

        /// <summary>
        /// Realiza inser��o em massa.
        /// </summary>
        /// <param name="dt">Tabela criada pelo m�todo AddBulkTable</param>
        /// <param name="trans">Transa��o</param>
        public static void SaveBulkTable(DataTable dt, SqlTransaction trans)
        {
            DataAccessLayer.SaveBulkTable(dt, "BNE_Formacao", trans);
        }
        #endregion

    }
}