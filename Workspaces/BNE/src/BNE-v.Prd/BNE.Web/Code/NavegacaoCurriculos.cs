using System;
using System.Collections.Generic;
using System.Linq;
using BNE.Common.Session;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Code.ViewStateObjects;

namespace BNE.Web.Code
{
    public class NavegacaoCurriculos
    {

        /// <summary>
        /// Utilizado na navegação pelos curriculos, o bool é utilizado para identificar se o retorno já foi salvo na base
        /// </summary>
        public SessionVariable<Dictionary<ResultadoPesquisaCurriculo.TipoPesquisa, Dictionary<int, List<KeyValuePair<int, bool>>>>> PesquisaCurriculoCurriculos = new SessionVariable<Dictionary<ResultadoPesquisaCurriculo.TipoPesquisa, Dictionary<int, List<KeyValuePair<int, bool>>>>>(Chave.Permanente.PesquisaCurriculo.ToString(), () =>
        {
            return Enum.GetValues(typeof(ResultadoPesquisaCurriculo.TipoPesquisa)).Cast<ResultadoPesquisaCurriculo.TipoPesquisa>().ToDictionary(key => key, key => new Dictionary<int, List<KeyValuePair<int, bool>>>());
        });

        public void AdicionarCurriculo(ResultadoPesquisaCurriculo.TipoPesquisa tipoPesquisa, int identificadorTipoPesquisa, int idCurriculo)
        {
            //Se é uma busca que já possui valores salvos
            if (PesquisaCurriculoCurriculos.Value[tipoPesquisa].ContainsKey(identificadorTipoPesquisa))
            {
                //Adiciona o novo currículo
                if (PesquisaCurriculoCurriculos.Value[tipoPesquisa][identificadorTipoPesquisa].All(m => m.Key != idCurriculo))
                    PesquisaCurriculoCurriculos.Value[tipoPesquisa][identificadorTipoPesquisa].Add(new KeyValuePair<int, bool>(idCurriculo, false));
            }
            else
                PesquisaCurriculoCurriculos.Value[tipoPesquisa].Add(identificadorTipoPesquisa, new List<KeyValuePair<int, bool>> { new KeyValuePair<int, bool>(idCurriculo, false) });

        }
        public void AdicionarCurriculos(ResultadoPesquisaCurriculo.TipoPesquisa tipoPesquisa, int identificadorTipoPesquisa, List<int> idsCurriculo)
        {
            //Se é uma busca que já possui valores salvos
            if (PesquisaCurriculoCurriculos.Value[tipoPesquisa].ContainsKey(identificadorTipoPesquisa))
            {
                var novos = idsCurriculo.Where(param => PesquisaCurriculoCurriculos.Value[tipoPesquisa][identificadorTipoPesquisa].All(lista => lista.Key != param)).Select(x => new KeyValuePair<int, bool>(x, false));

                PesquisaCurriculoCurriculos.Value[tipoPesquisa][identificadorTipoPesquisa].AddRange(novos);
            }
            else //Se é um novo, adiciona todos
            {
                var novos = idsCurriculo.Select(x => new KeyValuePair<int, bool>(x, false)).ToList();
                PesquisaCurriculoCurriculos.Value[tipoPesquisa].Add(identificadorTipoPesquisa, novos);
            }
        }

        public int CurriculoAnterior(ResultadoPesquisaCurriculo.TipoPesquisa tipoPesquisa, int identificadorTipoPesquisa, int itemAtual)
        {
            if (PesquisaCurriculoCurriculos.Value[tipoPesquisa].ContainsKey(identificadorTipoPesquisa))
            {
                var dictionary = PesquisaCurriculoCurriculos.Value[tipoPesquisa][identificadorTipoPesquisa].Select(m => m.Key).ToList();
                
                return dictionary.ElementAtOrDefault(dictionary.IndexOf(itemAtual) - 1);
            }
            return 0;
        }

        public int ProximoCurriculo(ResultadoPesquisaCurriculo.TipoPesquisa tipoPesquisa, int identificadorTipoPesquisa, int itemAtual)
        {
            if (PesquisaCurriculoCurriculos.Value[tipoPesquisa].ContainsKey(identificadorTipoPesquisa))
            {
                var dictionary = PesquisaCurriculoCurriculos.Value[tipoPesquisa][identificadorTipoPesquisa].Select(m => m.Key).ToList();

                return dictionary.ElementAtOrDefault(dictionary.IndexOf(itemAtual) + 1);
            }
            return 0;
        }

    }
}