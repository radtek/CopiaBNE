//-- Data: 14/05/2013 12:54
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class CursoParceiroTecla // Tabela: BNE_Curso_Parceiro_Tecla
	{
		#region Atributos
		private int _idCursoParceiroTecla;
		private CursoTecla _cursoTecla;
		private ParceiroTecla _parceiroTecla;
		private DateTime _dataInclusao;
		private bool _flagInativo;
		private string _descricaoURLCursoTecla;
		private string _descricaoCurso;
		private string _descricaoConteudo;
		private string _descricaoCaminhoImagemBanner;
		private string _descricaoCaminhoImagemMiniatura;
		private string _descricaoTituloCurso;
		private string _descricaoPublicoAlvo;
		private string _descricaoInstrutorCurso;
		private string _descricaoAssinaturaInstrutorCurso;
		private bool _flagCertificado;
		private int _quantidadeCargaHoraria;
		private decimal? _valorCursoSemDesconto;
		private decimal? _valorCurso;
		private int? _quantidadeParcela;
		private decimal? _valorCursoParcela;
		private CursoModalidadeTecla _cursoModalidadeTecla;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdCursoParceiroTecla
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdCursoParceiroTecla
		{
			get
			{
				return this._idCursoParceiroTecla;
			}
		}
		#endregion 

		#region CursoTecla
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public CursoTecla CursoTecla
		{
			get
			{
				return this._cursoTecla;
			}
			set
			{
				this._cursoTecla = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ParceiroTecla
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public ParceiroTecla ParceiroTecla
		{
			get
			{
				return this._parceiroTecla;
			}
			set
			{
				this._parceiroTecla = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DataInclusao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public DateTime DataInclusao
		{
			get
			{
				return this._dataInclusao;
			}
			set
			{
				this._dataInclusao = value;
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

		#region DescricaoURLCursoTecla
		/// <summary>
		/// Tamanho do campo: 1000.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoURLCursoTecla
		{
			get
			{
				return this._descricaoURLCursoTecla;
			}
			set
			{
				this._descricaoURLCursoTecla = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoCurso
		/// <summary>
		/// Tamanho do campo: 3000.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoCurso
		{
			get
			{
				return this._descricaoCurso;
			}
			set
			{
				this._descricaoCurso = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoConteudo
		/// <summary>
		/// Tamanho do campo: 3000.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoConteudo
		{
			get
			{
				return this._descricaoConteudo;
			}
			set
			{
				this._descricaoConteudo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoCaminhoImagemBanner
		/// <summary>
		/// Tamanho do campo: 500.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoCaminhoImagemBanner
		{
			get
			{
				return this._descricaoCaminhoImagemBanner;
			}
			set
			{
				this._descricaoCaminhoImagemBanner = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoCaminhoImagemMiniatura
		/// <summary>
		/// Tamanho do campo: 500.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoCaminhoImagemMiniatura
		{
			get
			{
				return this._descricaoCaminhoImagemMiniatura;
			}
			set
			{
				this._descricaoCaminhoImagemMiniatura = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoTituloCurso
		/// <summary>
		/// Tamanho do campo: 150.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoTituloCurso
		{
			get
			{
				return this._descricaoTituloCurso;
			}
			set
			{
				this._descricaoTituloCurso = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoPublicoAlvo
		/// <summary>
		/// Tamanho do campo: 250.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoPublicoAlvo
		{
			get
			{
				return this._descricaoPublicoAlvo;
			}
			set
			{
				this._descricaoPublicoAlvo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoInstrutorCurso
		/// <summary>
		/// Tamanho do campo: 150.
		/// Campo opcional.
		/// </summary>
		public string DescricaoInstrutorCurso
		{
			get
			{
				return this._descricaoInstrutorCurso;
			}
			set
			{
				this._descricaoInstrutorCurso = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoAssinaturaInstrutorCurso
		/// <summary>
		/// Tamanho do campo: 200.
		/// Campo opcional.
		/// </summary>
		public string DescricaoAssinaturaInstrutorCurso
		{
			get
			{
				return this._descricaoAssinaturaInstrutorCurso;
			}
			set
			{
				this._descricaoAssinaturaInstrutorCurso = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagCertificado
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagCertificado
		{
			get
			{
				return this._flagCertificado;
			}
			set
			{
				this._flagCertificado = value;
				this._modified = true;
			}
		}
		#endregion 

		#region QuantidadeCargaHoraria
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int QuantidadeCargaHoraria
		{
			get
			{
				return this._quantidadeCargaHoraria;
			}
			set
			{
				this._quantidadeCargaHoraria = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorCursoSemDesconto
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public decimal? ValorCursoSemDesconto
		{
			get
			{
				return this._valorCursoSemDesconto;
			}
			set
			{
				this._valorCursoSemDesconto = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorCurso
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public decimal? ValorCurso
		{
			get
			{
				return this._valorCurso;
			}
			set
			{
				this._valorCurso = value;
				this._modified = true;
			}
		}
		#endregion 

		#region QuantidadeParcela
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public int? QuantidadeParcela
		{
			get
			{
				return this._quantidadeParcela;
			}
			set
			{
				this._quantidadeParcela = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorCursoParcela
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public decimal? ValorCursoParcela
		{
			get
			{
				return this._valorCursoParcela;
			}
			set
			{
				this._valorCursoParcela = value;
				this._modified = true;
			}
		}
		#endregion 

		#region CursoModalidadeTecla
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public CursoModalidadeTecla CursoModalidadeTecla
		{
			get
			{
				return this._cursoModalidadeTecla;
			}
			set
			{
				this._cursoModalidadeTecla = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public CursoParceiroTecla()
		{
		}
		public CursoParceiroTecla(int idCursoParceiroTecla)
		{
			this._idCursoParceiroTecla = idCursoParceiroTecla;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Curso_Parceiro_Tecla (Idf_Curso_Tecla, Idf_Parceiro_Tecla, Dta_Inclusao, Flg_Inativo, Des_URL_Curso_Tecla, Des_Curso, Des_Conteudo, Des_Caminho_Imagem_Banner, Des_Caminho_Imagem_Miniatura, Des_Titulo_Curso, Des_Publico_Alvo, Des_Instrutor_Curso, Des_Assinatura_Instrutor_Curso, Flg_Certificado, Qtd_Carga_Horaria, Vlr_Curso_Sem_Desconto, Vlr_Curso, Qtd_Parcela, Vlr_Curso_Parcela, Idf_Curso_Modalidade_Tecla) VALUES (@Idf_Curso_Tecla, @Idf_Parceiro_Tecla, @Dta_Inclusao, @Flg_Inativo, @Des_URL_Curso_Tecla, @Des_Curso, @Des_Conteudo, @Des_Caminho_Imagem_Banner, @Des_Caminho_Imagem_Miniatura, @Des_Titulo_Curso, @Des_Publico_Alvo, @Des_Instrutor_Curso, @Des_Assinatura_Instrutor_Curso, @Flg_Certificado, @Qtd_Carga_Horaria, @Vlr_Curso_Sem_Desconto, @Vlr_Curso, @Qtd_Parcela, @Vlr_Curso_Parcela, @Idf_Curso_Modalidade_Tecla);SET @Idf_Curso_Parceiro_Tecla = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Curso_Parceiro_Tecla SET Idf_Curso_Tecla = @Idf_Curso_Tecla, Idf_Parceiro_Tecla = @Idf_Parceiro_Tecla, Dta_Inclusao = @Dta_Inclusao, Flg_Inativo = @Flg_Inativo, Des_URL_Curso_Tecla = @Des_URL_Curso_Tecla, Des_Curso = @Des_Curso, Des_Conteudo = @Des_Conteudo, Des_Caminho_Imagem_Banner = @Des_Caminho_Imagem_Banner, Des_Caminho_Imagem_Miniatura = @Des_Caminho_Imagem_Miniatura, Des_Titulo_Curso = @Des_Titulo_Curso, Des_Publico_Alvo = @Des_Publico_Alvo, Des_Instrutor_Curso = @Des_Instrutor_Curso, Des_Assinatura_Instrutor_Curso = @Des_Assinatura_Instrutor_Curso, Flg_Certificado = @Flg_Certificado, Qtd_Carga_Horaria = @Qtd_Carga_Horaria, Vlr_Curso_Sem_Desconto = @Vlr_Curso_Sem_Desconto, Vlr_Curso = @Vlr_Curso, Qtd_Parcela = @Qtd_Parcela, Vlr_Curso_Parcela = @Vlr_Curso_Parcela, Idf_Curso_Modalidade_Tecla = @Idf_Curso_Modalidade_Tecla WHERE Idf_Curso_Parceiro_Tecla = @Idf_Curso_Parceiro_Tecla";
		private const string SPDELETE = "DELETE FROM BNE_Curso_Parceiro_Tecla WHERE Idf_Curso_Parceiro_Tecla = @Idf_Curso_Parceiro_Tecla";
		private const string SPSELECTID = "SELECT * FROM BNE_Curso_Parceiro_Tecla WITH(NOLOCK) WHERE Idf_Curso_Parceiro_Tecla = @Idf_Curso_Parceiro_Tecla";
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
			parms.Add(new SqlParameter("@Idf_Curso_Parceiro_Tecla", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curso_Tecla", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Parceiro_Tecla", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Inclusao", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Des_URL_Curso_Tecla", SqlDbType.VarChar, 1000));
			parms.Add(new SqlParameter("@Des_Curso", SqlDbType.VarChar, 3000));
			parms.Add(new SqlParameter("@Des_Conteudo", SqlDbType.VarChar, 3000));
			parms.Add(new SqlParameter("@Des_Caminho_Imagem_Banner", SqlDbType.VarChar, 500));
			parms.Add(new SqlParameter("@Des_Caminho_Imagem_Miniatura", SqlDbType.VarChar, 500));
			parms.Add(new SqlParameter("@Des_Titulo_Curso", SqlDbType.VarChar, 150));
			parms.Add(new SqlParameter("@Des_Publico_Alvo", SqlDbType.VarChar, 250));
			parms.Add(new SqlParameter("@Des_Instrutor_Curso", SqlDbType.VarChar, 150));
			parms.Add(new SqlParameter("@Des_Assinatura_Instrutor_Curso", SqlDbType.VarChar, 200));
			parms.Add(new SqlParameter("@Flg_Certificado", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Qtd_Carga_Horaria", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Vlr_Curso_Sem_Desconto", SqlDbType.Decimal, 9));
			parms.Add(new SqlParameter("@Vlr_Curso", SqlDbType.Decimal, 9));
			parms.Add(new SqlParameter("@Qtd_Parcela", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Vlr_Curso_Parcela", SqlDbType.Decimal, 9));
			parms.Add(new SqlParameter("@Idf_Curso_Modalidade_Tecla", SqlDbType.Int, 4));
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
			parms[0].Value = this._idCursoParceiroTecla;
			parms[1].Value = this._cursoTecla.IdCursoTecla;
			parms[2].Value = this._parceiroTecla.IdParceiroTecla;
			parms[3].Value = this._dataInclusao;
			parms[4].Value = this._flagInativo;
			parms[5].Value = this._descricaoURLCursoTecla;
			parms[6].Value = this._descricaoCurso;
			parms[7].Value = this._descricaoConteudo;
			parms[8].Value = this._descricaoCaminhoImagemBanner;
			parms[9].Value = this._descricaoCaminhoImagemMiniatura;
			parms[10].Value = this._descricaoTituloCurso;
			parms[11].Value = this._descricaoPublicoAlvo;

			if (!String.IsNullOrEmpty(this._descricaoInstrutorCurso))
				parms[12].Value = this._descricaoInstrutorCurso;
			else
				parms[12].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoAssinaturaInstrutorCurso))
				parms[13].Value = this._descricaoAssinaturaInstrutorCurso;
			else
				parms[13].Value = DBNull.Value;

			parms[14].Value = this._flagCertificado;
			parms[15].Value = this._quantidadeCargaHoraria;

			if (this._valorCursoSemDesconto.HasValue)
				parms[16].Value = this._valorCursoSemDesconto;
			else
				parms[16].Value = DBNull.Value;


			if (this._valorCurso.HasValue)
				parms[17].Value = this._valorCurso;
			else
				parms[17].Value = DBNull.Value;


			if (this._quantidadeParcela.HasValue)
				parms[18].Value = this._quantidadeParcela;
			else
				parms[18].Value = DBNull.Value;


			if (this._valorCursoParcela.HasValue)
				parms[19].Value = this._valorCursoParcela;
			else
				parms[19].Value = DBNull.Value;

			parms[20].Value = this._cursoModalidadeTecla.IdCursoModalidadeTecla;

			if (!this._persisted)
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
		/// Método utilizado para inserir uma instância de CursoParceiroTecla no banco de dados.
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
						this._idCursoParceiroTecla = Convert.ToInt32(cmd.Parameters["@Idf_Curso_Parceiro_Tecla"].Value);
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
		/// Método utilizado para inserir uma instância de CursoParceiroTecla no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idCursoParceiroTecla = Convert.ToInt32(cmd.Parameters["@Idf_Curso_Parceiro_Tecla"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de CursoParceiroTecla no banco de dados.
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
		/// Método utilizado para atualizar uma instância de CursoParceiroTecla no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de CursoParceiroTecla no banco de dados.
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
		/// Método utilizado para salvar uma instância de CursoParceiroTecla no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de CursoParceiroTecla no banco de dados.
		/// </summary>
		/// <param name="idCursoParceiroTecla">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCursoParceiroTecla)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curso_Parceiro_Tecla", SqlDbType.Int, 4));

			parms[0].Value = idCursoParceiroTecla;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de CursoParceiroTecla no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCursoParceiroTecla">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCursoParceiroTecla, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curso_Parceiro_Tecla", SqlDbType.Int, 4));

			parms[0].Value = idCursoParceiroTecla;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de CursoParceiroTecla no banco de dados.
		/// </summary>
		/// <param name="idCursoParceiroTecla">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idCursoParceiroTecla)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Curso_Parceiro_Tecla where Idf_Curso_Parceiro_Tecla in (";

			for (int i = 0; i < idCursoParceiroTecla.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idCursoParceiroTecla[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCursoParceiroTecla">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCursoParceiroTecla)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curso_Parceiro_Tecla", SqlDbType.Int, 4));

			parms[0].Value = idCursoParceiroTecla;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCursoParceiroTecla">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCursoParceiroTecla, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curso_Parceiro_Tecla", SqlDbType.Int, 4));

			parms[0].Value = idCursoParceiroTecla;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cur.Idf_Curso_Parceiro_Tecla, Cur.Idf_Curso_Tecla, Cur.Idf_Parceiro_Tecla, Cur.Dta_Inclusao, Cur.Flg_Inativo, Cur.Des_URL_Curso_Tecla, Cur.Des_Curso, Cur.Des_Conteudo, Cur.Des_Caminho_Imagem_Banner, Cur.Des_Caminho_Imagem_Miniatura, Cur.Des_Titulo_Curso, Cur.Des_Publico_Alvo, Cur.Des_Instrutor_Curso, Cur.Des_Assinatura_Instrutor_Curso, Cur.Flg_Certificado, Cur.Qtd_Carga_Horaria, Cur.Vlr_Curso_Sem_Desconto, Cur.Vlr_Curso, Cur.Qtd_Parcela, Cur.Vlr_Curso_Parcela, Cur.Idf_Curso_Modalidade_Tecla FROM BNE_Curso_Parceiro_Tecla Cur";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de CursoParceiroTecla a partir do banco de dados.
		/// </summary>
		/// <param name="idCursoParceiroTecla">Chave do registro.</param>
		/// <returns>Instância de CursoParceiroTecla.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CursoParceiroTecla LoadObject(int idCursoParceiroTecla)
		{
			using (IDataReader dr = LoadDataReader(idCursoParceiroTecla))
			{
				CursoParceiroTecla objCursoParceiroTecla = new CursoParceiroTecla();
				if (SetInstance(dr, objCursoParceiroTecla))
					return objCursoParceiroTecla;
			}
			throw (new RecordNotFoundException(typeof(CursoParceiroTecla)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de CursoParceiroTecla a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCursoParceiroTecla">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de CursoParceiroTecla.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CursoParceiroTecla LoadObject(int idCursoParceiroTecla, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCursoParceiroTecla, trans))
			{
				CursoParceiroTecla objCursoParceiroTecla = new CursoParceiroTecla();
				if (SetInstance(dr, objCursoParceiroTecla))
					return objCursoParceiroTecla;
			}
			throw (new RecordNotFoundException(typeof(CursoParceiroTecla)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de CursoParceiroTecla a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idCursoParceiroTecla))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de CursoParceiroTecla a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idCursoParceiroTecla, trans))
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
		/// <param name="objCursoParceiroTecla">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, CursoParceiroTecla objCursoParceiroTecla)
		{
			try
			{
				if (dr.Read())
				{
					objCursoParceiroTecla._idCursoParceiroTecla = Convert.ToInt32(dr["Idf_Curso_Parceiro_Tecla"]);
					objCursoParceiroTecla._cursoTecla = new CursoTecla(Convert.ToInt32(dr["Idf_Curso_Tecla"]));
					objCursoParceiroTecla._parceiroTecla = new ParceiroTecla(Convert.ToInt32(dr["Idf_Parceiro_Tecla"]));
					objCursoParceiroTecla._dataInclusao = Convert.ToDateTime(dr["Dta_Inclusao"]);
					objCursoParceiroTecla._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objCursoParceiroTecla._descricaoURLCursoTecla = Convert.ToString(dr["Des_URL_Curso_Tecla"]);
					objCursoParceiroTecla._descricaoCurso = Convert.ToString(dr["Des_Curso"]);
					objCursoParceiroTecla._descricaoConteudo = Convert.ToString(dr["Des_Conteudo"]);
					objCursoParceiroTecla._descricaoCaminhoImagemBanner = Convert.ToString(dr["Des_Caminho_Imagem_Banner"]);
					objCursoParceiroTecla._descricaoCaminhoImagemMiniatura = Convert.ToString(dr["Des_Caminho_Imagem_Miniatura"]);
					objCursoParceiroTecla._descricaoTituloCurso = Convert.ToString(dr["Des_Titulo_Curso"]);
					objCursoParceiroTecla._descricaoPublicoAlvo = Convert.ToString(dr["Des_Publico_Alvo"]);
					if (dr["Des_Instrutor_Curso"] != DBNull.Value)
						objCursoParceiroTecla._descricaoInstrutorCurso = Convert.ToString(dr["Des_Instrutor_Curso"]);
					if (dr["Des_Assinatura_Instrutor_Curso"] != DBNull.Value)
						objCursoParceiroTecla._descricaoAssinaturaInstrutorCurso = Convert.ToString(dr["Des_Assinatura_Instrutor_Curso"]);
					objCursoParceiroTecla._flagCertificado = Convert.ToBoolean(dr["Flg_Certificado"]);
					objCursoParceiroTecla._quantidadeCargaHoraria = Convert.ToInt32(dr["Qtd_Carga_Horaria"]);
					if (dr["Vlr_Curso_Sem_Desconto"] != DBNull.Value)
						objCursoParceiroTecla._valorCursoSemDesconto = Convert.ToDecimal(dr["Vlr_Curso_Sem_Desconto"]);
					if (dr["Vlr_Curso"] != DBNull.Value)
						objCursoParceiroTecla._valorCurso = Convert.ToDecimal(dr["Vlr_Curso"]);
					if (dr["Qtd_Parcela"] != DBNull.Value)
						objCursoParceiroTecla._quantidadeParcela = Convert.ToInt32(dr["Qtd_Parcela"]);
					if (dr["Vlr_Curso_Parcela"] != DBNull.Value)
						objCursoParceiroTecla._valorCursoParcela = Convert.ToDecimal(dr["Vlr_Curso_Parcela"]);
					objCursoParceiroTecla._cursoModalidadeTecla = new CursoModalidadeTecla(Convert.ToInt32(dr["Idf_Curso_Modalidade_Tecla"]));

					objCursoParceiroTecla._persisted = true;
					objCursoParceiroTecla._modified = false;

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