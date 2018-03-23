using LanHouse.Entities.BNE;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business
{
    public class Empresa
    {
        #region RecuperarEmpresas
        /// <summary>
        /// Efetuar a busca de empresas próximas à LanHouse
        /// </summary>
        /// <param name="start"></param>
        /// <param name="rows"></param>
        /// <param name="LanLat"></param>
        /// <param name="LanLong"></param>
        /// <returns></returns>
        public static IList<DTO.Empresa> RecuperarEmpresas(int start, int rows, double LanLat, double LanLong)
        {
            var searchPoint = CreatePoint(LanLat, LanLong);

            using(var entity = new LanEntities())
            {
                entity.Configuration.LazyLoadingEnabled = false;

                var query = (
                    from e in entity.TAB_Filial
                    join c in entity.LAN_Companhia on e.Idf_Filial equals c.TAB_Filial.Idf_Filial into fc
                    from f in fc.DefaultIfEmpty()
                    let distance = e.Des_Localizacao.Distance(searchPoint) //em metros
                    where distance < 100000 && !e.Flg_Inativo
                    select new DTO.Empresa
                    {
                        id = e.Idf_Filial,
                        nomeFantasia = e.Nme_Fantasia,
                        cnpj = e.Num_CNPJ,
                        apresentacao = f.Des_Carta_Apresentacao,
                        distance = distance,
                        logoEmpresa = (from l in entity.TAB_Filial_Logo where l.Idf_Filial == e.Idf_Filial select l.Img_Logo).FirstOrDefault(),
                        vagas = (from v in entity.BNE_Vaga
                     where v.Idf_Filial == e.Idf_Filial
                     orderby v.Dta_Abertura descending
                     select new DTO.Vaga
                     {
                         id = v.Idf_Vaga,
                         funcao = v.Des_Funcao == null || v.Des_Funcao == "" ? "" : v.Des_Funcao,
                         codigo = v.Cod_Vaga
                     }).Take(3).ToList()
                    });
                return query.OrderBy(e => e.distance).Skip(start).Take(rows).ToList<DTO.Empresa>();
            }
        }
        #endregion

        #region CarregarVagasEmpresa
        /// <summary>
        /// Carregar as vagas da empresa
        /// </summary>
        /// <param name="idf_Filial"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static IList<DTO.Vaga> CarregarVagasEmpresa(int idf_Filial, LanEntities entity)
        {
            IList<DTO.Vaga> vagas = null;

            vagas = (from v in entity.BNE_Vaga
                     where v.Idf_Filial == idf_Filial && v.Flg_Inativo == false
                     orderby v.Dta_Abertura descending
                     select new DTO.Vaga
                     {
                         id = v.Idf_Vaga,
                         funcao = v.Des_Funcao,
                         codigo = v.Cod_Vaga
                     }).Take(3).ToList();

            //se não tem vagas abertas vai pegar as 3 últimas inativas
            if (vagas.Count == 0)
            {
                vagas = (from v in entity.BNE_Vaga
                         where v.Idf_Filial == idf_Filial && v.Flg_Inativo == true
                         orderby v.Dta_Abertura descending
                         select new DTO.Vaga
                         {
                             id = v.Idf_Vaga,
                             funcao = v.Des_Funcao,
                             codigo = v.Cod_Vaga
                         }).Take(3).ToList();
            }

            return vagas;
        }
        #endregion

        #region CreatePoint
        /// <summary>
        /// Criar o tipo de dados DbGeography para fazer a busca por localização
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public static DbGeography CreatePoint(double latitude, double longitude)
        {
            var text = string.Format(CultureInfo.InvariantCulture.NumberFormat,
                                     "POINT({0} {1})", longitude, latitude);
            // 4326 is most common coordinate system used by GPS/Maps
            return DbGeography.PointFromText(text, 4326);
        }
        #endregion
    }
}