using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public class Indicacao
    {

        private const string SP_SO_INDICADOS = @"SELECT CONCAT('(',idc.Num_DDD_Celular,')', idc.Num_Celular) AS NumInidcado FROM BNE.BNE_Indicacao ico WITH (NOLOCK) 
                                                INNER JOIN BNE.BNE_Indicado idc WITH (NOLOCK) ON ico.Idf_Indicacao = idc.Idf_Indicacao
                                                WHERE ico.Idf_Curriculo = @Idf_Curriculo AND CONCAT(idc.Num_DDD_Celular, idc.Num_Celular) IN ({0})";

        private const string SP_INSERT_ICO = @"INSERT INTO BNE.BNE_Indicacao (Idf_Curriculo, Dta_Cadastro) VALUES  (@Idf_Curriculo, GETDATE()); SELECT CAST(SCOPE_IDENTITY() AS INT) AS Idf_Indicacao;";


        private const string SP_INSERT_ICD = @" INSERT INTO BNE.BNE_Indicado ( Idf_Indicacao, Nme_Indicado ,Num_DDD_Celular , Num_Celular, Eml_Indicado ) " +
                                              " VALUES  ( @Idf_Indicacao , @Nme_Indicado ,@Num_DDD_Celular , @Num_Celular, @Eml_Indicado );";

        private const string SP_INSERT_INDICADO = @" INSERT INTO BNE.BNE_Indicado ( Idf_Indicacao, Nme_Indicado, Eml_Indicado ) VALUES  ( @Idf_Indicacao , @Nme_Indicado, @Eml_Indicado );";

        private const string SP_TEM_PRM = @"SELECT * FROM BNE.TAB_Parametro_Curriculo WHERE Idf_Parametro = @Idf_Parametro AND  Idf_Curriculo = @Idf_Curriculo;";

        private const string SP_UPDTE_PRM = @"UPDATE BNE.TAB_Parametro_Curriculo SET Vlr_Parametro = @Vlr_Parametro WHERE Idf_Parametro = @Idf_Parametro AND  Idf_Curriculo = @Idf_Curriculo;";

        private const string SP_CREAT_PRM = @" INSERT INTO BNE.TAB_Parametro_Curriculo(Idf_Parametro, Idf_Curriculo, Dta_Cadastro, Vlr_Parametro, Flg_Inativo) " +
                                             " VALUES  (@Idf_Parametro , @Idf_Curriculo ,GETDATE(),  @Vlr_Parametro,  0);";


        private int _id;
        public int Id { get { return _id; } }

        private Curriculo _curriculo;
        public  Curriculo Curriculo { get { return _curriculo;  } }

        private PessoaFisica _pessoaFisica;
        public  PessoaFisica PessoaFisica { get { return _pessoaFisica;  } }

        public DateTime DataCadastro { get; set; }

        private List<Indicado> _indicados;
        public  List<Indicado> Indicados { get { return _indicados; } }
        
        public Indicacao(Curriculo curriculo, PessoaFisica pessoaFisica) 
        {
            this._pessoaFisica = pessoaFisica;
            this._curriculo = curriculo;
            this._indicados = new List<Indicado>();
        }


        /// <summary>Recebe uma lista de telefones (DDD + Num) sem espaços e chars especiais
        /// e filtra a lista removendo os que não foram indicados.
        /// <para>Lista de telefones (DDD + Num) sem espaços e chars especiais entre aspas simples. Ex.: '4195744478'</para>
        /// </summary> 
        public List<string> FiltrarSomenteIndicados(List<string> telefones)
        {
            List<string> indicados = new List<string>();
            string query = string.Format(SP_SO_INDICADOS, string.Join(",", telefones.ToArray()));

            List<SqlParameter> prms = new List<SqlParameter>(){ new SqlParameter("Idf_Curriculo", SqlDbType.Int, 4){ Value = this._curriculo.IdCurriculo } };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, query, prms)) 
            {
                while (dr.Read()) { indicados.Add(dr.GetString(0)); }
            }

            return indicados;
        }

        public void AdicionarIndicado(string _nome, string _numddd, string _numcelular, string _email) 
        {
            Indicado ind = new Indicado();
            ind.Indicacao = this;
            ind.Nome = _nome;
            ind.NumDDD = _numddd;
            ind.NumeroCelular = _numcelular;
            ind.Email = _email;

            if (!_indicados.Contains(ind)) _indicados.Add(ind);
        }


        /// <summary>Realiza o prcesso de indicação salvado os indicados na tabela e enviado as mensagens SMS.</summary> 
        public bool Indicar()
        {
            bool result = false;
            try
            {
                List<SqlParameter> prms = new List<SqlParameter>() { new SqlParameter("Idf_Curriculo", SqlDbType.Int, 4) { Value = this._curriculo.IdCurriculo } };
                using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
		        {
			        conn.Open();
			        using (SqlTransaction trans = conn.BeginTransaction())
			        {
				        try
				        {
                            IDataReader dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, SP_INSERT_ICO, prms);
                            if (dr.Read())
                            { 
                                this._id = dr.GetInt32(0);
                                dr.Close();

                                try 
                                {
                                    foreach (var idc in _indicados)
                                    {
                                        List<SqlParameter> prms2 = new List<SqlParameter>() {
                                            new SqlParameter("Idf_Indicacao", SqlDbType.Int, 4) { Value = this._id },
                                            new SqlParameter("Nme_Indicado", SqlDbType.VarChar, 50) { Value = idc.Nome },
                                            //new SqlParameter("Num_DDD_Celular", SqlDbType.VarChar, 2) { Value = idc.NumDDD },
                                            //new SqlParameter("Num_Celular", SqlDbType.VarChar, 10) { Value = idc.NumeroCelular },
                                            new SqlParameter("Eml_Indicado", SqlDbType.VarChar, 100) { Value = idc.Email }

                                        };
                                        DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SP_INSERT_INDICADO, prms2); 
                                    }

                                    List<SqlParameter> prmsT1 = new List<SqlParameter>() 
                                    {
                                        new SqlParameter("Idf_Parametro", SqlDbType.Int, 4) { Value = (int)Enumeradores.Parametro.QuantidadeCandidaturaDegustacao },
                                        new SqlParameter("Idf_Curriculo", SqlDbType.Int, 4) { Value = this._curriculo.IdCurriculo  }
                                    };


                                    List<SqlParameter> prmsT2 = new List<SqlParameter>() 
                                    {
                                        new SqlParameter("Idf_Parametro", SqlDbType.Int, 4) { Value = (int)Enumeradores.Parametro.QuantidadeCandidaturaDegustacao },
                                        new SqlParameter("Idf_Curriculo", SqlDbType.Int, 4) { Value = this._curriculo.IdCurriculo  },
                                        new SqlParameter("Vlr_Parametro", SqlDbType.Int, 4) { Value = Parametro.RecuperaValorParametro(Enumeradores.Parametro.CandidaturasGratisPorIndicarAmigos) },
                                    };


                                    var dr2 = DataAccessLayer.ExecuteReader(trans, CommandType.Text, SP_TEM_PRM, prmsT1);
                                    bool FazAtualizacao = dr2.Read();
                                    dr2.Close();
                                    if (FazAtualizacao) 
                                        DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SP_UPDTE_PRM, prmsT2);
                                    else
                                        DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SP_CREAT_PRM, prmsT2);


                                    EnviarEmail();
                                    
                                    trans.Commit();
                                    result = true;
                                }
                                catch (Exception ex) 
                                {
                                    EL.GerenciadorException.GravarExcecao(ex);
                                    trans.Rollback();
                                }
                            }
                            else
                                trans.Rollback();
				        }
				        catch(Exception ex)
				        {
                            EL.GerenciadorException.GravarExcecao(ex);
					        trans.Rollback();
				        }
			        }
		        }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }

            return result;
        }

        #region EnviarEmail
        /// <summary>
        /// Método que envia a carta para o indicado.
        /// </summary>
        private void EnviarEmail()
        {
            //Recuperar Carta
            var carta = CartaEmail.RecuperarCarta(Enumeradores.CartaEmail.indicaoAmigoBH);
            string UrlDestino = Parametro.RecuperaValorParametro(Enumeradores.Parametro.UrlIndiqueAmigoBH);
            string assunto = string.Empty;
            string conteudo = string.Empty;

            this._curriculo.PessoaFisica = this._pessoaFisica;

            //Enviar E-mail
            foreach (var indicado in this._indicados)
	        {
                string quemIndicou = this._curriculo.PessoaFisica.RecuperarNomePessoa();
                conteudo = carta.Conteudo.Replace("{nomeIndicado}", indicado.Nome);
                conteudo = conteudo.Replace("{quemIndicou}", quemIndicou);
                conteudo = conteudo.Replace("{emailIndicado}", indicado.Email);
                //conteudo = conteudo.Replace("{urlDestino}", UrlDestino);

                assunto = carta.Assunto.Replace("{nomeIndicado}", indicado.Nome);
                assunto = assunto.Replace("{quemIndicou}", quemIndicou);

                MensagemCS.SalvarEmail(_curriculo, null, null, null, assunto, conteudo,Enumeradores.CartaEmail.indicaoAmigoBH, "atendimento@bne.com.br", indicado.Email, null, null, null);
            }
        }
        #endregion

        private void EnviarSMS()
        {
            using (BLL.BNETanqueService.AppClient objWsTanque = new BLL.BNETanqueService.AppClient()) 
            {
                objWsTanque.Open();
                List<BNE.BLL.BNETanqueService.Mensagem> listaSMS = new List<BNE.BLL.BNETanqueService.Mensagem>();
                foreach (var idc in this._indicados)
                {
                    var mensagem = new BNE.BLL.BNETanqueService.Mensagem();
                    mensagem.ci = idc.NumDDD.Trim() + idc.NumeroCelular.Trim();
                    mensagem.np = idc.Nome;
                    mensagem.nc = Convert.ToDecimal(idc.NumDDD.Trim() + idc.NumeroCelular.Trim());
                    mensagem.dm = string.Format(Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.TemplateIndiqueSMS),
                                                 idc.Nome.Split(' ')[0], (this._pessoaFisica.Sexo.IdSexo == 1)? "o":"a", this._pessoaFisica.NomePessoa.Split(' ')[0]);
                    listaSMS.Add(mensagem);
                }
                var receberMensagem = new BNE.BLL.BNETanqueService.InReceberMensagem { l = listaSMS.ToArray(), cu = "IndicacaoBne" };
                try
                {
                    
                    var retorno = objWsTanque.ReceberMensagem(receberMensagem);
                    if (retorno.l == null)
                        throw new Exception("Problema SMS");
                    else if (retorno.l.Count() == 0)
                        throw new Exception("Problema SMS");
                }
                catch (Exception ex) { throw ex; }
            }
        }
    }
}
