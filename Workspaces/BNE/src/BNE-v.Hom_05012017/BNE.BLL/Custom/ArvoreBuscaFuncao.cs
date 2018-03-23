using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BNE.BLL.Custom
{
    public class ArvoreBuscaFuncao
    {

        #region Contrutores
        private ArvoreBuscaFuncao()
        {
            this._raiz = new NoBuscaDescricao<Funcao>(null, null);
            List<Funcao> lstFuncoes = Funcao.ListarFuncoes();

            foreach (Funcao objFuncao in lstFuncoes)
            {
                AdicionarFuncao(objFuncao);    
            }
        }
        #endregion
        
        #region Atributos
        
        private static ArvoreBuscaFuncao _instance;
        private NoBuscaDescricao<Funcao> _raiz;
        
        #endregion
        
        #region Propriedades

        public NoBuscaDescricao<Funcao> Raiz { get { return _raiz; } }
        
        #endregion

        #region Metodos

        /// <summary>
        /// Obtém a instancia de uma árvore de busca de funções.
        /// </summary>
        /// <returns>Objeto da classe ArvoreBuscaFuncao</returns>
        public static ArvoreBuscaFuncao GetInstance(){
            if (ArvoreBuscaFuncao._instance == null)
	        {
		        ArvoreBuscaFuncao._instance = new ArvoreBuscaFuncao();
	        }
            
            return ArvoreBuscaFuncao._instance;
        }

        public void AdicionarFuncao(Funcao objFuncao)
        {
            //Limpando Funcao
            //Retirando preposições
            String regexPreposicoes = @"(?<=^|[ -!#$%&'()*+,./:;<=>?@[\\\]_`{|}~])((D[AEO])|(N[AO])|(AO)|(PARA)|(COM)|([AEIOU])|(EM))(?!$|\w)";

            String descricaoFuncao = Helper.RemoverAcentos(objFuncao.DescricaoFuncao).ToUpper();
            descricaoFuncao = Regex.Replace(descricaoFuncao, regexPreposicoes, "", RegexOptions.IgnoreCase);

            //Retirando espaços repetidos e os espaços do começo e fim
            descricaoFuncao = Regex.Replace(descricaoFuncao, " {1,}", " ", RegexOptions.IgnoreCase).Trim();

            //Adicionando Descricao da Funcao na Árvore
            this._raiz.Adicionar(descricaoFuncao, objFuncao);
        }

        public Funcao Buscar(String descricao)
        {
            descricao = Helper.RemoverAcentos(descricao);
            NoBuscaDescricao<Funcao> retorno = this.Raiz.BuscarMaisEspecifico(descricao);
            if (retorno != null)
            {
                return retorno.ObjetoVinculado;
            }
            return null;
        }

        #endregion
       
    }
}
