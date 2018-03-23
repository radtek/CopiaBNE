using BNE.Solr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.BLL.Custom
{
    public class BuscaVaga
    {
        public static string MontaQuerySolr(PesquisaVaga objPesquisaVaga, int tamanhoPagina, int paginaAtual, int? idCurriculo, int? idFuncao, int? idCidade, string palavraChave, int? idOrigem, bool? empresaOfereceCursos, string sigEstado, int? idFilial, int? idFuncaoArea, OrdenacaoBuscaVaga ordenacao, out int totalRegistros, bool mostrarConfidencial = true)
        {
            totalRegistros = 10;
            return "http://10.1.0.45:8983/solr/VagaBNE/select?q=*%3A*&wt=json&indent=true";
        }
    }
}
