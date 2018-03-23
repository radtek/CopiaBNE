//-- Data: 05/10/2012 14:44
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.Mensagem.AsyncServices.BLL
{
    public partial class TipoAtividadeSistema // Tabela: plataforma.TAB_Tipo_Atividade_Sistema
    {

        #region RelacaoTipoAtividade
        /// <summary>
        /// Objeto representando a relação de Tipo de atividae
        /// </summary>
        public class RelacaoTipoAtividade
        {
            public String DesTipoAtividade { get; set; }
            public Enumeradores.TipoAtividade TipoAtividade { get; set; }
            public Model.Sistema Sistema { get; set; }
            public Model.Template Template { get; set; }

            public RelacaoTipoAtividade(string desTipoAtividade, int tipoAtividade, Model.Sistema objSistema, Model.Template objTemplate)
            {
                DesTipoAtividade = desTipoAtividade;
                TipoAtividade = (Enumeradores.TipoAtividade)tipoAtividade;
                Sistema = objSistema;
                Template = objTemplate;
            }
        }
        #endregion

        #region Consultas

        #region Splistartipos
        private const String Splistartipos = @"
        SELECT	ta.Idf_Tipo_Atividade,
				ta.Des_Tipo_Atividade,
                sis.IdSistema,
                sis.Nome as NomeSistema,
                tem.IdTemplateEmail,
                tem.Nome as NomeEmail,
                tsms.IdTemplateSMS,
                tsms.Nome as NomeSMS
        FROM    atividade.TAB_Tipo_Atividade_Sistema tas
				INNER JOIN atividade.TAB_Tipo_Atividade ta ON ta.Idf_Tipo_Atividade = tas.Idf_Tipo_Atividade
                INNER JOIN mensagem.Sistema sis ON tas.IdSistema = sis.IdSistema
                LEFT JOIN mensagem.TemplateEmail tem ON tas.IdTemplateEmail = tem.IdTemplateEmail
                LEFT JOIN mensagem.TemplateSMS tsms ON tas.IdTemplateSMS = tsms.IdTemplateSMS
        WHERE   tas.Flg_Inativo = 0 AND ta.Flg_Inativo = 0
        ";
        #endregion

        #region Splistartipo
        private const String Splistartipo = @"
        SELECT	tas.Idf_Tipo_Atividade_Sistema, tas.Des_Tipo_Atividade_Sistema, tas.Flg_Inativo, tas.Idf_Tipo_Atividade
        FROM    atividade.TAB_Tipo_Atividade_Sistema tas
        WHERE   tas.IdSistema = @IdSistema 
                AND ( 
                        (@IdTemplateEmail IS NOT NULL AND tas.IdTemplateEmail = @IdTemplateEmail)
                        OR
                        (@IdTemplateSMS IS NOT NULL AND tas.IdTemplateSMS = @IdTemplateSMS)
                    )
        ";
        #endregion

        #endregion

        #region Métodos

        #region ListarTiposAtividade
        public static List<RelacaoTipoAtividade> ListarTiposAtividade()
        {
            var lst = new List<RelacaoTipoAtividade>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Splistartipos, null))
            {
                while (dr.Read())
                {
                    Model.Template objTemplate;
                    if (dr["IdTemplateEmail"] != DBNull.Value)
                        objTemplate = new Model.TemplateEmail { Id = Convert.ToInt32(dr["IdTemplateEmail"]), Nome = Convert.ToString(dr["NomeEmail"]) };
                    else
                        objTemplate = new Model.TemplateSMS { Id = Convert.ToInt32(dr["IdTemplateSMS"]), Nome = Convert.ToString(dr["NomeSMS"]) };

                    lst.Add(new RelacaoTipoAtividade(
                        Convert.ToString(dr["Des_Tipo_Atividade"]),
                        Convert.ToInt32(dr["Idf_Tipo_Atividade"]),
                        new Model.Sistema { Id = Convert.ToByte(dr["IdSistema"]), Nome = Convert.ToString(dr["NomeSistema"]) },
                        objTemplate));
                }
            }

            return lst;
        }
        #endregion

        #region RecuperarPorSistemaETemplate
        /// <summary>
        /// Recupera o id do tipo atividade relacionado
        /// </summary>
        /// <param name="objSistema"></param>
        /// <param name="objTemplate"></param>
        /// <returns></returns>
        public static TipoAtividadeSistema RecuperarPorSistemaETemplate(Model.Sistema objSistema, Model.Template objTemplate)
        {
            object valueEmail = DBNull.Value;
            if (objTemplate is Model.TemplateEmail)
                valueEmail = objTemplate.Id;

            object valueSMS = DBNull.Value;
            if (objTemplate is Model.TemplateSMS)
                valueSMS = objTemplate.Id;

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@IdSistema", SqlDbType = SqlDbType.TinyInt, Value = objSistema.Id },
                    new SqlParameter { ParameterName = "@IdTemplateEmail", SqlDbType = SqlDbType.Int, Value = valueEmail },
                    new SqlParameter { ParameterName = "@IdTemplateSMS", SqlDbType = SqlDbType.Int, Value = valueSMS }
                };

            var objTipoAtividadeSistema = new TipoAtividadeSistema();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Splistartipo, parms))
            {
                if (!SetInstance(dr, objTipoAtividadeSistema))
                    objTipoAtividadeSistema = null;
            }
            return objTipoAtividadeSistema;
        }
        #endregion

        #endregion

    }
}