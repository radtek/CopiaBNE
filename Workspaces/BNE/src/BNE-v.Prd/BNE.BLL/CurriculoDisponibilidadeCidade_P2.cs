//-- Data: 24/07/2012 09:46
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BNE.BLL
{
    public partial class CurriculoDisponibilidadeCidade // Tabela: BNE_Curriculo_Disponibilidade_Cidade
    {

        #region Querys

        private const string SpNomeCidadePorCurriculo = @"
        SELECT  CI.Nme_Cidade + '/' + CI.Sig_Estado as 'Nme_Cidade', Idf_Curriculo_Disponibilidade_Cidade, CDC.Flg_Inativo 
        FROM    BNE_Curriculo_Disponibilidade_Cidade AS CDC WITH(NOLOCK)
                INNER JOIN plataforma.TAB_Cidade AS CI WITH(NOLOCK) ON CDC.Idf_Cidade = CI.Idf_Cidade 
        WHERE   Idf_Curriculo = @Idf_Curriculo 
                AND CDC.Flg_Inativo = 0";

        private const string SpPorCurriculo = @"
        SELECT  *
        FROM    BNE_Curriculo_Disponibilidade_Cidade AS CDC WITH(NOLOCK)
                INNER JOIN plataforma.TAB_Cidade AS CI WITH(NOLOCK) ON CDC.Idf_Cidade = CI.Idf_Cidade 
        WHERE   Idf_Curriculo = @Idf_Curriculo 
                AND CDC.Flg_Inativo = 0";

        private const string Spselectporcurriculocidade = @"
        SELECT  *
        FROM    BNE_Curriculo_Disponibilidade_Cidade AS CDC WITH(NOLOCK)
        WHERE   Idf_Curriculo = @Idf_Curriculo 
                AND Idf_Cidade = @Idf_Cidade";

        private const string SP_ATUALIZA_LISTA_PESSOA_FISICA = @"
        DECLARE @Idf_Cidade INT
        DECLARE @handle INT
        EXEC sp_xml_preparedocument @handle OUTPUT, @Xml

        -- Deletando disponibilidades
        DELETE FROM BNE.BNE_Curriculo_Disponibilidade_Cidade 
        WHERE Idf_Curriculo = @Idf_Curriculo
        AND Idf_Cidade NOT IN (SELECT * FROM OPENXML (@handle, '/ArrayOfInt/int') WITH (id INT '.'))

        -- Incluindo disponibilidades
        DECLARE incluir_cursor CURSOR  
            FOR SELECT id FROM OPENXML (@handle, '/ArrayOfInt/int') WITH (id INT '.')
			        WHERE NOT EXISTS(SELECT 1 FROM BNE.BNE_Curriculo_Disponibilidade_Cidade
								        WHERE Idf_Curriculo = @Idf_Curriculo AND
								        Idf_Cidade = id)

        OPEN incluir_cursor  
        FETCH NEXT FROM incluir_cursor 
	        INTO @Idf_Cidade

        WHILE @@FETCH_STATUS = 0  
        BEGIN 
	        INSERT INTO BNE.BNE_Curriculo_Disponibilidade_Cidade
	                ( Idf_Curriculo ,
	                  Idf_Cidade ,
	                  Flg_Inativo ,
	                  Dta_Cadastro ,
	                  Dta_Alteracao
	                )
	        VALUES  ( @Idf_Curriculo , -- Idf_Curriculo - int
	                  @Idf_Cidade , -- Idf_Cidade - int
	                  0 , -- Flg_Inativo - bit
	                  '2016-10-24 01:17:41' , -- Dta_Cadastro - datetime
	                  '2016-10-24 01:17:41'  -- Dta_Alteracao - datetime
	                )

	        FETCH NEXT FROM incluir_cursor 
	        INTO @Idf_Cidade
        END

        CLOSE incluir_cursor;  
        DEALLOCATE incluir_cursor;  

        EXEC sp_xml_removedocument @handle";


        #region [spDeleteDisponibilidadePorCurriculo]
        private const string spDeleteDisponibilidadePorCurriculo = "delete BNE_Curriculo_Disponibilidade_Cidade where Idf_Curriculo = @Idf_Curriculo";
        #endregion

        #endregion

        #region Métodos

        #region ListarCidades

        /// <summary>
        /// Método que Lista as Cidades Disponiveis por Curriculo
        /// </summary>
        /// <param name="idCurriculo">Idf do Curriculo Selecionado</param>
        /// <returns>Retornar um DataTable contendo uma coluna com as cidades do curriculo informado</returns>
        public static DataTable ListarCidadesPorCurriculo(int idCurriculo)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4)
                };
            parms[0].Value = idCurriculo;

            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, SpNomeCidadePorCurriculo, parms).Tables[0];
        }

        /// <summary>
        /// Lista as Cidades pelo Id do Curriculo
        /// </summary>
        /// <param name="idCurriculo">Idf dp Curriculo Selecionado</param>
        /// <returns>Retornar uma lista de Cidades com o nome da Cidade</returns>
        public static List<String> ListarCidade(int idCurriculo)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4)
                };
            parms[0].Value = idCurriculo;

            var listString = new List<String>();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpNomeCidadePorCurriculo, parms))
            {
                while (dr.Read())
                    listString.Add(dr["Nme_Cidade"].ToString());

                if (!dr.IsClosed)
                    dr.Close();
            }

            return listString;
        }

        /// <summary>
        /// Lista as Cidades pelo Id do Curriculo
        /// </summary>
        /// <param name="idCurriculo">Idf dp Curriculo Selecionado</param>
        /// <returns>Retornar uma lista de Cidades com o nome da Cidade</returns>
        public static List<CurriculoDisponibilidadeCidade> Listar(int idCurriculo)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4)
                };
            parms[0].Value = idCurriculo;

            var lstRetorno = new List<CurriculoDisponibilidadeCidade>();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpPorCurriculo, parms))
            {
                var objCDC = new CurriculoDisponibilidadeCidade();
                while (dr.Read() &&
                    SetInstance_NotDispose(dr, objCDC) &&
                    Cidade.SetInstance_NotDispose(dr, objCDC.Cidade))
                {
                    lstRetorno.Add(objCDC);
                    objCDC = new CurriculoDisponibilidadeCidade();
                }

                if (!dr.IsClosed)
                    dr.Close();
            }

            return lstRetorno;
        }

        #endregion

        #region CarregarPorCurriculoCidade
        /// <summary>
        /// Método responsável por carregar uma instancia de Curriculo Disponbilidade Cidade dado determinado Curriculo e uma Cidade
        /// </summary>
        /// <param name="objCurriculo">Curriculo em questão</param>
        /// <param name="objCidade">Instância de Cidade </param>
        /// <param name="objCurriculoDisponibilidadeCidade">Curriculo Disponbilidade Cidade</param>
        /// <returns>True caso exista</returns>
        public static bool CarregarPorCurriculoCidade(Curriculo objCurriculo, Cidade objCidade, out CurriculoDisponibilidadeCidade objCurriculoDisponibilidadeCidade)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objCurriculo.IdCurriculo} ,
                    new SqlParameter { ParameterName = "@Idf_Cidade", SqlDbType = SqlDbType.Int, Size = 4, Value = objCidade.IdCidade}
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectporcurriculocidade, parms))
            {
                objCurriculoDisponibilidadeCidade = new CurriculoDisponibilidadeCidade();
                if (SetInstance(dr, objCurriculoDisponibilidadeCidade))
                    return true;
                dr.Close();
            }
            objCurriculoDisponibilidadeCidade = null;
            return false;
        }
        #endregion

        #region Save
        /// <summary>
        /// Método sobrescrito utilizado para salvar uma instância de CurriculoDisponibilidadeCidade no banco de dados.
        /// </summary>
        /// <remarks>Luan Fernandes</remarks>
        public void Save()
        {
            if (!this._persisted)
                this.Insert();
            else
                this.Update();

            //AlertaCurriculos.OnAlterarCurriculo(this.Curriculo);
        }
        /// <summary>
        /// Método sobrescrito utilizado para salvar uma instância de CurriculoDisponibilidadeCidade no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Luan Fernandes</remarks>
        public void Save(SqlTransaction trans)
        {
            if (!this._persisted)
                this.Insert(trans);
            else
                this.Update(trans);

            //AlertaCurriculos.OnAlterarCurriculo(this.Curriculo);
        }
        #endregion

        #region AtualizaListaCurriculo

        /// <summary>
        /// Atualiza as disponibilidades por curriculo
        /// </summary>
        /// <param name="idCurriculo"></param>
        /// <param name="lstDisponibilidades"></param>
        /// <param name="trans"></param>
        public static void AtualizaListaCurriculo(int idCurriculo,
            List<CurriculoDisponibilidadeCidade> lstDisponibilidadesCidades,
            SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Xml", SqlDbType.Xml));
            parms[0].Value = idCurriculo;

            XmlSerializer xs = new XmlSerializer(typeof(List<int>));
            MemoryStream ms = new MemoryStream();
            xs.Serialize(ms, lstDisponibilidadesCidades.Select(dc => dc.Cidade.IdCidade).ToList());

            string resultXML = UTF8Encoding.UTF8.GetString(ms.ToArray());

            parms[1].Value = resultXML;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SP_ATUALIZA_LISTA_PESSOA_FISICA, parms);
        }

        #endregion

        #region SetInstance
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objCurriculoDisponibilidadeCidade">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance_NotDispose(IDataReader dr, CurriculoDisponibilidadeCidade objCurriculoDisponibilidadeCidade)
        {
            try
            {
                objCurriculoDisponibilidadeCidade._idCurriculoDisponibilidadeCidade = Convert.ToInt32(dr["Idf_Curriculo_Disponibilidade_Cidade"]);
                objCurriculoDisponibilidadeCidade._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
                objCurriculoDisponibilidadeCidade._cidade = new Cidade(Convert.ToInt32(dr["Idf_Cidade"]));
                objCurriculoDisponibilidadeCidade._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                objCurriculoDisponibilidadeCidade._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                objCurriculoDisponibilidadeCidade._dataAlteracao = Convert.ToDateTime(dr["Dta_Alteracao"]);

                objCurriculoDisponibilidadeCidade._persisted = true;
                objCurriculoDisponibilidadeCidade._modified = false;

                return true;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [DeleteDisponibilidadePorCurriculo]
        /// <summary>
        /// Deleta todas as disponibilidade cidade do curriculo
        /// </summary>
        /// <param name="idCurriculo"></param>
        public static void DeleteDisponibilidadePorCurriculo(int idCurriculo, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4)
                };
            parms[0].Value = idCurriculo;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, spDeleteDisponibilidadePorCurriculo, parms);
        }
        #endregion

        #endregion

    }
}