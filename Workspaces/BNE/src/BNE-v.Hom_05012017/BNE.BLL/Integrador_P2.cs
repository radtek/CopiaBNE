using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class Integrador
    {
        #region Atributos

        private Dictionary<Enumeradores.Parametro, String> _parametros;

        #endregion

        #region Propriedades

        #region TipoIntegrador
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public Enumeradores.TipoIntegrador? TipoIntegrador
        {
            get
            {
                if (this._tipoIntegrador == null)
                {
                    return null;
                }
                return (Enumeradores.TipoIntegrador)this._tipoIntegrador.IdTipoIntegrador;
            }
            set
            {
                if (this._tipoIntegrador == null)
                {
                    this._tipoIntegrador = new TipoIntegrador((int)value);
                }
                else
                {
                    this._tipoIntegrador.IdTipoIntegrador = (int)value;
                }
                this._modified = true;
            }
        }
        #endregion

        public Dictionary<Enumeradores.Parametro, String> Parametros
        {
            get
            {
                if (_idIntegrador <= 0)
                {
                    return null;
                }
                if (_parametros == null)
                {
                    CarregarParametros();
                }
                return _parametros;
            }
        }

        #endregion

        #region Consultas

        private const string Splistarativas = @"
        SELECT  *
        FROM    TAB_Integrador I WITH(NOLOCK)
        WHERE   I.Flg_Inativo = 0";

        private const string Spinativarvagas = @"
        declare @sql nvarchar(Max)
        Set @sql=   'UPDATE  BNE_Vaga
                    SET     Flg_Inativo = 1 /* Inativa */
                    WHERE   Idf_Vaga IN (SELECT Idf_Vaga FROM BNE_Vaga_Integracao vi
						                    WHERE vi.Idf_Integrador = '+@Id_Integrador+'
						                    AND vi.Idf_Vaga NOT IN ('+@Ids_vagas_importadas+'));
                    UPDATE  BNE_Vaga_Integracao
                    SET     Flg_Inativo = 1 /* Inativa */
                    WHERE   Idf_Vaga IN (   SELECT Idf_Vaga FROM BNE_Vaga_Integracao vi
						                    WHERE vi.Idf_Integrador = '+@Id_Integrador+'
						                    AND vi.Idf_Vaga NOT IN ('+@Ids_vagas_importadas+'));'
        exec sp_executesql @sql";

        private const string SP_SELECT_PARAMETROS = @"SELECT *
                                                        FROM TAB_Parametro_Integrador WITH(NOLOCK)
                                                        WHERE Idf_Integrador = @Idf_Integrador";

        private const string SP_SELECT_INTEGRADOR_SINE = @"SELECT *
                                                        FROM TAB_Integrador WITH(NOLOCK)
                                                        WHERE Idf_Tipo_Integrador = 3";

        #endregion

        #region Métodos

        /// <summary>
        /// Obtém a lista de parâmetros de integração da instância do Integrador.
        /// </summary>
        private void CarregarParametros()
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Integrador", SqlDbType.Int, 4));

            parms[0].Value = _idIntegrador;

            _parametros = new Dictionary<Enumeradores.Parametro, string>();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_SELECT_PARAMETROS, parms))
            {
                while (dr.Read())
                {
                    _parametros[(Enumeradores.Parametro)Convert.ToInt32(dr["Idf_Parametro"])] = dr["Vlr_Parametro"].ToString();
                }
            }
        }

        /// <summary>
        /// Obtém o valor do parâmetro da instância do integrador.
        /// </summary>
        /// <param name="parametro">Parametro a ser recuperado</param>
        /// <returns>Retorna null se o parâmetro não está definido</returns>
        public String GetValorParametro(Enumeradores.Parametro parametro)
        {
            String retorno = null;
            Parametros.TryGetValue(parametro, out retorno);
            return retorno;
        }

        #region ListarIntegradoresAtivos
        /// <summary>
        /// Retorna todas as origens ativas
        /// </summary>
        /// <returns>DataReader</returns>
        public static IDataReader ListarIntegradoresAtivos()
        {
            return DataAccessLayer.ExecuteReader(CommandType.Text, Splistarativas, null);
        }
        #endregion

        #region RecuperarIntegradoresAtivos
        /// <summary>
        /// Retorna todas as origens ativas
        /// </summary>
        /// <returns>IEnumerable</returns>
        public static IEnumerable<Integrador> RecuperarIntegradoresAtivos()
        {
            var lista = new List<Integrador>();
            using (IDataReader dr = ListarIntegradoresAtivos())
            {
                while (dr.Read())
                {
                    var objIntegrador = new Integrador();

                    if (SetInstanceNotDispose(dr, objIntegrador))
                    {
                        lista.Add(objIntegrador);
                        if (dr["Idf_Tipo_Integrador"] != DBNull.Value)
                            objIntegrador._tipoIntegrador = new TipoIntegrador(Convert.ToInt32(dr["Idf_Tipo_Integrador"]));
                        if (dr["Idf_Usuario_Filial_Perfil"] != DBNull.Value)
                            objIntegrador._usuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));
                    }
                }
            }
            return lista;
        }
        #endregion

        #region SetInstanceNotDispose
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objOrigemImportacao">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstanceNotDispose(IDataReader dr, Integrador objIntegrador)
        {
            objIntegrador._idIntegrador = Convert.ToInt32(dr["Idf_Integrador"]);
            objIntegrador._filial = Filial.LoadObject(Convert.ToInt32(dr["Idf_Filial"]));
            objIntegrador._filial.Endereco = Endereco.LoadObject(objIntegrador._filial.Endereco.IdEndereco);
            objIntegrador._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
            objIntegrador._tipoIntegrador = new TipoIntegrador(Convert.ToInt32(dr["Idf_Tipo_Integrador"]));
            objIntegrador._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));

            objIntegrador._persisted = true;
            objIntegrador._modified = false;

            return true;
        }
        #endregion

        #region InativarVagas
        /// <summary>
        /// Inativa todas as vagas do integrador que não estejam presentes da lista VagasAtivas
        /// </summary>
        /// <param name="vagasAtivas">Lista de Vagas Ativas do Sistema. As vagas presentes nessa lista NÃO serão excluídas</param>
        /// <param name="objIntegrador">Integrador das vagas a serem excluídas</param>
        public void InativarVagas(List<Vaga> vagasAtivas, Integrador objIntegrador)
        {
            String listaIdsVagas = "";
            foreach (Vaga vaga in vagasAtivas)
            {
                if (listaIdsVagas != ""){
                    listaIdsVagas += ",";
                }
                listaIdsVagas += vaga.IdVaga.ToString();
            }

            //Se nenhuma vaga foi passada como ativa, desativa todas as vagas do integrador, passando 0 como id de vaga ativa.
            if (String.IsNullOrEmpty(listaIdsVagas))
            {
                listaIdsVagas = "0";
            }

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Ids_vagas_importadas", SqlDbType = SqlDbType.NVarChar, Value = listaIdsVagas },
                    new SqlParameter { ParameterName = "@Id_Integrador", SqlDbType = SqlDbType.NVarChar, Size = 50, Value = objIntegrador.IdIntegrador.ToString() }
                };

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, Spinativarvagas, parms);
        }
        #endregion

        #region CarregaIntegradorSINE
        public static Integrador CarregaIntegradorSINE()
        {
            using (IDataReader dr = LoadDataReaderIntegradorSINE())
            {
                Integrador objIntegrador = new Integrador();
                if (SetInstance(dr, objIntegrador))
                    return objIntegrador;
            }
            return null;
        }

        private static IDataReader LoadDataReaderIntegradorSINE()
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            return DataAccessLayer.ExecuteReader(CommandType.Text, SP_SELECT_INTEGRADOR_SINE, parms);
        }
        #endregion CarregaIntegradorSINE



        #endregion

    }
}
