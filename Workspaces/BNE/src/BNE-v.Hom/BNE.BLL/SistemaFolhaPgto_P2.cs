//-- Data: 24/08/2017 12:33
//-- Autor: Mailson

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Caching;

namespace BNE.BLL
{
    public partial class SistemaFolhaPgto // Tabela: BNE_Sistema_Folha_Pgto
    {
        public class PesquisaFolha
        {
            public List<SistemaFolhaPgto> lista { get; set; }
            public bool MostraModal { get; set; }
        }


        #region [Consultas]

        #region [splistaSistemas]

        private const string splistaSistemas = @"select idf_sistema_folha_pgto, des_sistema_folha_pgto
                             from BNE.BNE_Sistema_Folha_Pgto with(nolock)
                             where flg_inativo = 0";
        #endregion

        #endregion

        #region [listaSistemas]
        public static List<SistemaFolhaPgto> listaSistemas()
        {
            string cachekey = String.Format("SistemaFolhaPgto");
            if (MemoryCache.Default[cachekey] != null)
                return (List<SistemaFolhaPgto>)MemoryCache.Default[cachekey];


            List<SistemaFolhaPgto> lista = new List<SistemaFolhaPgto>();

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, splistaSistemas, null))
            {
                while (dr.Read())
                {
                    SistemaFolhaPgto objp = new SistemaFolhaPgto()
                    {
                        _idSistemaFolhaPgto = Convert.ToInt16(dr["idf_sistema_folha_pgto"]),
                        _descricaoSistemaFolhaPgto = dr["des_sistema_folha_pgto"].ToString()
                    };
                    lista.Add(objp);
                }
            }

            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now.AddMinutes(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.MinutosLimpezaCache)));
            MemoryCache.Default.Add(cachekey, lista, policy);

            return lista;

        }
        #endregion

        public static PesquisaFolha FolhaPesquisa(int idFilial)
        {

            PesquisaFolha objPesquisa = new PesquisaFolha();
            objPesquisa.MostraModal = !SistemaFolhaPgtoPesquisa.JaRespondeu(idFilial);
                if (objPesquisa.MostraModal)
                {
                    var lista = SistemaFolhaPgto.listaSistemas();
                    objPesquisa.lista = lista;
                }

            return objPesquisa;
        }
    }
}