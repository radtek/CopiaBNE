using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BNE.BLL
{
    public class PesquisaCurriculoAvaliacao
    {

        private int _IdPesquisaCurriculoAvaliacao;
        private PesquisaCurriculo _pesquisaCurriculo;
        private Avaliacao _avaliacao;

        private bool _persisted = false;
        private bool _modified = false;


        private const string SQL_INSERT = "INSERT INTO BNE.BNE_Pesquisa_Curriculo_Avaliacao (Idf_Pesquisa_Curriculo, Idf_Avaliacao) VALUES (@Idf_Pesquisa_Curriculo, @Idf_Avaliacao); SET @Idf_Pesquisa_Curriculo_Avaliacao = SCOPE_IDENTITY();";
        private const string SQL_UPDATE = "UPDATE BNE.BNE_Pesquisa_Curriculo_Avaliacao SET  Idf_Pesquisa_Curriculo = @Idf_Pesquisa_Curriculo, Idf_Avaliacao = @Idf_Avaliacao WHERE Idf_Pesquisa_Curriculo_Avaliacao = @Idf_Pesquisa_Curriculo_Avaliacao;";
        private const string SQL_SELECT = "SELECT TOP 1 * FROM BNE.BNE_Pesquisa_Curriculo_Avaliacao WHERE Idf_Pesquisa_Curriculo_Avaliacao = @Idf_Pesquisa_Curriculo_Avaliacao;";


        public PesquisaCurriculoAvaliacao()
        {
            
        }

        public PesquisaCurriculoAvaliacao(int id) 
        {
            this._IdPesquisaCurriculoAvaliacao = id;
            this.Select();
        }

        public int IdPesquisaCurriculoAvaliacao
        {
            get
            {
                return this._IdPesquisaCurriculoAvaliacao;
            }
        }

        public PesquisaCurriculo PesquisaCurriculo
        {
            get
            {
                return this._pesquisaCurriculo;
            }
            set
            {
                this._pesquisaCurriculo = value;
                this._modified = true;
            }
        }

        public Avaliacao Avaliacao
        {
            get
            {
                return this._avaliacao;
            }
            set
            {
                this._avaliacao = value;
                this._modified = true;
            }
        }


        public void Save(SqlTransaction trans = null)
        {
            if (!this._persisted)
                this.Insert(trans);
            else
                this.Update(trans);
        }


        private List<SqlParameter> GetParameters() 
        {
            List<SqlParameter> parms = new List<SqlParameter>()
            {
                new SqlParameter(){ ParameterName = "@Idf_Pesquisa_Curriculo_Avaliacao", SqlDbType = SqlDbType.Int },
                new SqlParameter(){ ParameterName = "@Idf_Pesquisa_Curriculo", SqlDbType = SqlDbType.Int },
                new SqlParameter(){ ParameterName = "@Idf_Avaliacao", SqlDbType = SqlDbType.Int }
            };
            return parms;
        }


        private void SetParameters(List<SqlParameter> parms)
        {
            parms[0].Value = this._IdPesquisaCurriculoAvaliacao;
            
            if (this._pesquisaCurriculo != null)
                parms[1].Value = this._pesquisaCurriculo.IdPesquisaCurriculo;
            else
                parms[1].Value = DBNull.Value;

            if (this._avaliacao != null)
                parms[2].Value = this._avaliacao.IdAvaliacao;
            else
                parms[2].Value = DBNull.Value;
        }



        private void Insert(SqlTransaction trans = null)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = null;

            if(trans != null)
                cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SQL_INSERT, parms);
            else
                cmd = DataAccessLayer.ExecuteNonQueryCmd(CommandType.Text, SQL_INSERT, parms);

            this._IdPesquisaCurriculoAvaliacao = Convert.ToInt32(cmd.Parameters["@Idf_Pesquisa_Curriculo_Avaliacao"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }

        private void Update(SqlTransaction trans = null)
        {
            if (this._modified)
            {
                List<SqlParameter> parms = GetParameters();
                SetParameters(parms);

                if (trans != null)
                    DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SQL_UPDATE, parms);
                else
                    DataAccessLayer.ExecuteNonQuery(CommandType.Text, SQL_UPDATE, parms);

                this._modified = false;
            }
        }


        private void Select()
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SQL_SELECT, parms))
            {
                if (dr.Read())
                {
                    this._pesquisaCurriculo = new PesquisaCurriculo(dr.GetInt32(dr.GetOrdinal("Idf_Pesquisa_Curriculo")));
                    this._avaliacao = new Avaliacao(dr.GetInt32(dr.GetOrdinal("Idf_Avaliacao")));
                    this._persisted = true;
                }
                else 
                {
                    this._IdPesquisaCurriculoAvaliacao = 0;
                }
            }
            
        }






    }
}
