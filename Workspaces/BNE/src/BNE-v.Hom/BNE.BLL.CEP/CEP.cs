/*  Autor: Gustavo Marty Sroka
 *  Data: 13/04/2016
 *  Classe copiada do web service de CEP da webfopag, com pequenos ajustes no código
 */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using BNE.ExtensionsMethods;

namespace BNE.CEP
{
	public class Cep
	{
		#region Consultas
		private const string SQL_CAMPOS = "SELECT Num_Cep, Tip_Logradouro, Des_Endereco, Des_Complemento, Des_Bairro, Des_UF, Des_Cidade ";
        private const string SQL_COUNT = "SELECT isnull(count(*),0) ";
        private const string SQL_FROM = "FROM cep.CEP_VW_CEP ";
		#endregion

		#region Atributos
		private int _id;
		private string _logradouro;
		private string _numero;
		private string _tipoLogradouro;
		private string _bairro;
		private string _bairroAbreviado;
		private string _cidade;
		private string _estado;
		private string _cep;

		private bool _encontrou;
		#endregion

		#region Propriedades

		#region ID
		public int ID
		{
			get
			{
				return this._id;
			}
			set
			{
				this._id = value;
			}
		}
		#endregion

		#region Logradouro
		public string Logradouro
		{
			get
			{
				return this._logradouro;
			}
			set
			{
				this._logradouro = value;
			}
		}
		#endregion

		#region Numero
		public string Numero
		{
			get
			{
				return this._numero;
			}
			set
			{
				this._numero = value;
			}
		}
		#endregion

		#region TipoLogradouro
		public string TipoLogradouro
		{
			get
			{
				return this._tipoLogradouro;
			}
			set
			{
				this._tipoLogradouro = value;
			}
		}
		#endregion

		#region Bairro
		public string Bairro
		{
			get
			{
				return this._bairro;
			}
			set
			{
				this._bairro = value;
			}
		}
		#endregion

		#region BairroAbreviado
		public string BairroAbreviado
		{
			get
			{
				return this._bairroAbreviado;
			}
			set
			{
				this._bairroAbreviado = value;
			}
		}
		#endregion

		#region Cidade
		public string Cidade
		{
			get
			{
				return this._cidade;
			}
			set
			{
				this._cidade = value;
			}
		}
		#endregion

		#region Estado
		public string Estado
		{
			get
			{
				return this._estado;
			}
			set
			{
				this._estado = value;
			}
		}
		#endregion

		#region Cep
		public string sCep
		{
			get
			{
				return this._cep;
			}
			set
			{
				this._cep = value;
			}
		}
		#endregion

		#region Encontrou
		public bool Encontrou
		{
			get
			{
				return this._encontrou;
			}
			set
			{
				this._encontrou = value;
			}
		}
        #endregion

        #region Complemento
        public string Complemento
        {
            get; set;
        }
        #endregion

        #endregion

        #region Métodos

        #region CompletaCEP
        public static void CompletarCEP(ref Cep cep)
        {
            using (var dr = CompletaCEP(cep))
            {
                if (dr.Read())
                {
                    cep.sCep = dr["Num_Cep"].ToString();
                    cep.Logradouro = dr["Des_Endereco"].ToString();
                    cep.TipoLogradouro = dr["Tip_Logradouro"].ToString();
                    cep.Bairro = dr["Des_Bairro"].ToString();
                    cep.Cidade = dr["Des_Cidade"].ToString();
                    cep.Estado = dr["Des_UF"].ToString();
                    cep.Complemento = dr["Des_Complemento"] as string;
                    cep.Encontrou = true;
                }
            }
        }
        private static IDataReader CompletaCEP(Cep cep)
		{
			return CompletaCEPDR(cep.sCep, cep.Logradouro, cep.TipoLogradouro, cep.Bairro, cep.Estado, cep.Cidade);
		}
		#endregion

		#region ConsultaCEP
		public static IDataReader ConsultaCEP(Cep cep)
		{
			return ConsultaCEPDR(cep.sCep, cep.Logradouro, cep.TipoLogradouro, cep.Bairro, cep.Estado, cep.Cidade);
		}

        public static int ContaConsulta(Cep cep)
        {
            StringBuilder query = new StringBuilder();
            List<SqlParameter> parms;

            query.Append(SQL_COUNT);
            query.Append(SQL_FROM);
            query.Append(getWhere(cep.sCep, cep.Logradouro, cep.TipoLogradouro, cep.Bairro, cep.Estado, cep.Cidade, out parms));

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, query.ToString(), parms));
        }
		#endregion

		#region getWhere
		private static string getWhere(string cep, string logradouro, string tipoLogradouro, string bairro, string estado, string cidade, out List<SqlParameter> parms)
		{
			parms = new List<SqlParameter>();

			StringBuilder query = new StringBuilder();
			List<string> clausulas = new List<string>();
			clausulas.Add(" WHERE 1 = 1");

			if (cep != null && !String.IsNullOrEmpty(cep.Trim()))
			{
				parms.Add(new SqlParameter("@Num_Cep", SqlDbType.VarChar, 8) { Value = cep.Trim() });
				clausulas.Add(" Num_Cep = @Num_Cep");
			}

			if (logradouro != null && !String.IsNullOrEmpty(logradouro.Trim()))
			{
				parms.Add(new SqlParameter("@Nme_Logradouro", SqlDbType.VarChar, 100) { Value = logradouro.Trim() });
				clausulas.Add(" Des_Endereco like '%' + @Nme_Logradouro + '%' COLLATE Latin1_General_CI_AI");
			}

			if (tipoLogradouro != null && !String.IsNullOrEmpty(tipoLogradouro.Trim()))
			{
				parms.Add(new SqlParameter("@Tip_Logradouro", SqlDbType.VarChar, 36) { Value = tipoLogradouro.Trim() });
				clausulas.Add(" Tip_Logradouro = @Tip_Logradouro");
			}

			if (bairro != null && !String.IsNullOrEmpty(bairro.Trim()))
			{
				parms.Add(new SqlParameter("@Nme_Bairro", SqlDbType.VarChar, 72) { Value = bairro.Trim() });
				clausulas.Add(" Des_Bairro like '%' + @Nme_Bairro + '%' COLLATE Latin1_General_CI_AI");
			}

			if (cidade != null && !String.IsNullOrEmpty(cidade.Trim()))
			{
				parms.Add(new SqlParameter("@Nme_Cidade", SqlDbType.VarChar, 72) { Value = cidade.Trim() });
				clausulas.Add(" Des_Cidade like '%' + @Nme_Cidade + '%' COLLATE Latin1_General_CI_AI");
			}

			if (estado != null && !String.IsNullOrEmpty(estado.Trim()))
			{
				parms.Add(new SqlParameter("@Sig_Estado", SqlDbType.Char, 2) { Value = estado.Trim() });
				clausulas.Add(" Des_Uf = @Sig_Estado");
			}

			if (parms.Count == 0)
				parms = null;

			return string.Join("\n\t AND ", clausulas.ToArray());
		}

		private static string getWhereValorExato(string cep, string logradouro, string tipoLogradouro, string bairro, string estado, string cidade, out List<SqlParameter> parms)
		{
			parms = new List<SqlParameter>();

			StringBuilder query = new StringBuilder();
			List<string> clausulas = new List<string>();
			clausulas.Add(" WHERE 1 = 1");

			if (cep != null && !String.IsNullOrEmpty(cep.Trim()))
			{
				parms.Add(new SqlParameter("@Num_Cep", SqlDbType.VarChar, 8) { Value = cep.Trim() });
				clausulas.Add(" Num_Cep = @Num_Cep");
			}

			if (logradouro != null && !String.IsNullOrEmpty(logradouro.Trim()))
			{
				parms.Add(new SqlParameter("@Nme_Logradouro", SqlDbType.VarChar, 100) { Value = logradouro.Trim() });
				clausulas.Add(" Des_Endereco = @Nme_Logradouro COLLATE Latin1_General_CI_AI");
			}

			if (tipoLogradouro != null && !String.IsNullOrEmpty(tipoLogradouro.Trim()))
			{
				parms.Add(new SqlParameter("@Tip_Logradouro", SqlDbType.VarChar, 36) { Value = tipoLogradouro.Trim() });
				clausulas.Add(" Tip_Logradouro = @Tip_Logradouro");
			}

			if (bairro != null && !String.IsNullOrEmpty(bairro.Trim()))
			{
				parms.Add(new SqlParameter("@Nme_Bairro", SqlDbType.VarChar, 72) { Value = bairro.Trim() });
				clausulas.Add(" Des_Bairro = @Nme_Bairro COLLATE Latin1_General_CI_AI");
			}

			if (cidade != null && !String.IsNullOrEmpty(cidade.Trim()))
			{
				parms.Add(new SqlParameter("@Nme_Cidade", SqlDbType.VarChar, 72) { Value = cidade.Trim() });
				clausulas.Add(" Des_Cidade = @Nme_Cidade COLLATE Latin1_General_CI_AI");
			}

			if (estado != null && !String.IsNullOrEmpty(estado.Trim()))
			{
				parms.Add(new SqlParameter("@Sig_Estado", SqlDbType.Char, 2) { Value = estado.Trim() });
				clausulas.Add(" Des_Uf = @Sig_Estado");
			}

			if (parms.Count == 0)
				parms = null;

			return string.Join("\n\t AND ", clausulas.ToArray());
		}
		#endregion

		#region CompletaCEPDR
		internal static IDataReader CompletaCEPDR(string cep, string logradouro, string tipoLogradouro, string bairro, string estado, string cidade)
		{
			StringBuilder query = new StringBuilder();
			List<SqlParameter> parms;

			query.Append(SQL_CAMPOS);
			query.Append(SQL_FROM);
			query.Append(getWhereValorExato(cep, logradouro, tipoLogradouro, bairro, estado, cidade, out parms));

			return DataAccessLayer.ExecuteReader(CommandType.Text, query.ToString(), parms);
		}
		#endregion

		#region ConsultaCEPDR
		internal static IDataReader ConsultaCEPDR(string cep, string logradouro, string tipoLogradouro, string bairro, string estado, string cidade)
		{
			StringBuilder query = new StringBuilder();
			List<SqlParameter> parms;

			query.Append(SQL_CAMPOS);
			query.Append(SQL_FROM);
			query.Append(getWhere(cep, logradouro, tipoLogradouro, bairro, estado, cidade, out parms));

			return DataAccessLayer.ExecuteReader(CommandType.Text, query.ToString(), parms);
		}
		#endregion

		#region ConsultaCEPDS
        public static IList<Cep> ConsultaCEPDS(string cep, string logradouro, string tipoLogradouro, string bairro, string estado, string cidade, string ordenacao)
		{
			StringBuilder query = new StringBuilder();
			List<SqlParameter> parms;

			query.Append("SELECT TOP 15 Num_Cep, Tip_Logradouro, Des_Endereco, Des_Complemento, Des_Bairro, Des_UF, Des_Cidade ");
			query.Append(SQL_FROM);

			query.Append(getWhere(cep, logradouro, tipoLogradouro, bairro, estado, cidade, out parms));

			if (!string.IsNullOrEmpty(ordenacao))
			{
				query.Append("ORDER BY " + ordenacao);
			}

			return CarregaListaCep(DataAccessLayer.ExecuteReaderDs(CommandType.Text, query.ToString(), parms).Tables[0]);
		}
		#endregion

		#region ConsultaCEPPaginacao
        public static IList<Cep> ConsultaCEPPaginacao(string cep, string logradouro, string tipoLogradouro, string bairro, string estado, string cidade, CEP.Colunas nomeColuna, CEP.DirecaoOrdenacao direcaoOrdenacao, int paginaCorrente, int tamanhoPagina, out int totalRegistros)
		{
			totalRegistros = 0;

			int FirstRec;
			int LastRec;
			StringBuilder iSelect = new StringBuilder();
			string iSelectCount;
			string iSelectPag;
			List<SqlParameter> parms;

			FirstRec = (paginaCorrente - 1) * tamanhoPagina;
			LastRec = (paginaCorrente * tamanhoPagina + 1);

            iSelect.Append("SELECT ROW_NUMBER() OVER (ORDER BY " + nomeColuna.GetAttribute<ColumnAttribute>().Name + " " + direcaoOrdenacao + " ) AS RowID, ");
			iSelect.Append("Num_Cep, ");
			iSelect.Append("Tip_Logradouro, ");
			iSelect.Append("Des_Endereco, ");
			iSelect.Append("Des_Complemento,  ");
			iSelect.Append("Des_Bairro, ");
			iSelect.Append("Des_UF, ");
			iSelect.Append("Des_Cidade ");
			iSelect.Append("FROM cep.CEP_VW_CEP ");

			string sWhere = getWhere(cep, logradouro, tipoLogradouro, bairro, estado, cidade, out parms);
			if (!String.IsNullOrEmpty(sWhere))
				iSelect.Append(sWhere);

			iSelectCount = " Select Count(*) From ( " + @iSelect + " ) As TblTempCount";

			SqlDataReader drCount = DataAccessLayer.ExecuteReader(CommandType.Text, iSelectCount, parms);
			if (drCount.Read())
				totalRegistros = Convert.ToInt32(drCount[0]);

			iSelectPag = " Select *	From ( " + @iSelect + " ) As TblTempPag	Where RowID > " + FirstRec.ToString() + " And RowID < " + LastRec.ToString();

			sWhere = getWhere(cep, logradouro, tipoLogradouro, bairro, estado, cidade, out parms);  //apenas para recarregar os parametros
			DataTable dt = DataAccessLayer.ExecuteReaderDs(CommandType.Text, iSelectPag, parms).Tables[0];

			drCount.Close();

			return CarregaListaCep(dt);
		}
		#endregion

        #region CarregaListaCep
        private static IList<Cep> CarregaListaCep(DataTable tb)
        {
            var ls = new List<Cep>(tb.Rows.Count);
            foreach (DataRow linha in tb.Rows)
            {
                ls.Add(new Cep
                {
                    sCep = linha["Num_Cep"] as string,
                    TipoLogradouro = linha["Tip_Logradouro"] as string,
                    Logradouro = linha["Des_Endereco"] as string,
                    Bairro = linha["Des_Bairro"] as string,
                    Cidade = linha["Des_Cidade"] as string,
                    Estado = linha["Des_UF"] as string,
                    Complemento = linha["Des_Complemento"] as string,
                    Encontrou = true,
                });
            }
            return ls;
        }
        #endregion

        #region ListarTipoLogradouro
        public static IList<Cep> ListarTipoLogradouro(string cepsParaListar)
		{
			if (string.IsNullOrEmpty(cepsParaListar))
				return new List<Cep>();

			List<SqlParameter> parms;
			parms = new List<SqlParameter>(0);

			string query = string.Format(SQL_CAMPOS + " FROM cep.CEP_VW_CEP WHERE Num_Cep IN ({0})", cepsParaListar);

			return CarregaListaCep(DataAccessLayer.ExecuteReaderDs(CommandType.Text, query, parms).Tables[0]);
		}
		#endregion

		#endregion


	}
}
