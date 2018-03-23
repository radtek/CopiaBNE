using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BNE.BLL.Custom
{
    /// <summary>
    /// Classe que implementa uma árvore B para a busca de descricoes. Cada nó da arvore armazena um objeto vinculado, que é o objeto correspondente à descrição encontrada.
    /// </summary>
    /// <typeparam name="T">Tipo do objeto vinculado a ser armazenado.</typeparam>
    public class NoBuscaDescricao<T>
    {
        #region Contrutor
        public NoBuscaDescricao(String Descricao, NoBuscaDescricao<T> NoPai)
        {
            this._descricao = Descricao;
            if (Descricao != null)
            {
                String pattern = Parametro.RecuperaValorParametro(Enumeradores.Parametro.RegexIndexacaoVaga);
                pattern = pattern.Replace("{palavra}", Regex.Replace(Descricao, @"[ao]$", "([AO]|)", RegexOptions.IgnoreCase));
                this._regex = new Regex(pattern, RegexOptions.IgnoreCase);
            }
            this.Nos = new List<NoBuscaDescricao<T>>();

            if (NoPai != null)
            {
                this._profundidade = NoPai.Profundidade + 1;
                NoPai.Nos.Add(this);
            }
            else
            {
                this._profundidade = 0;
            }
        }
        #endregion

        #region Atributos
        private Regex _regex;
        private String _descricao;
        private T _objetoVinculado;
        private Int32 _profundidade;
        #endregion

        #region Propriedades
        /// <summary>
        /// Expressão regular utilizada para buscar a descrição do nó na descrição buscada na árvore.
        /// </summary>
        public Regex Regex { get { return this._regex; } }
        /// <summary>
        /// Descrição do nó
        /// </summary>
        public String Descricao { get { return this._descricao; } }
        /// <summary>
        /// Nós filhos.
        /// </summary>
        public List<NoBuscaDescricao<T>> Nos { get; set; }
        /// <summary>
        /// Objeto vinculado ao nó
        /// </summary>
        public T ObjetoVinculado { get { return this._objetoVinculado; } }
        /// <summary>
        /// Profuncidade do nó. Tem a utilidade de filtrar objeto mais específico. Quanto maior a profundidade, mais nós do ramo casaram com a sequencia indicada.
        /// </summary>
        public Int32 Profundidade { get { return this._profundidade; } }
        #endregion

        #region Métodos

        /// <summary>
        /// Adicionar um objeto vinculado à arvore de pesquisa.
        /// </summary>
        /// <param name="descricao">Descrição a ser utilizada para indexação do objeto.</param>
        /// <param name="objetoVinculado">Objeto Vinculado à descrição informada.</param>
        public void Adicionar(String descricao, T objetoVinculado)
        {
            this.Adicionar(Helper.RemoverAcentos(descricao).Split(' ').ToList(), objetoVinculado);
        }

        /// <summary>
        /// Método recurssivo utilizado para a inserção do objeto na função pública Adicionar
        /// </summary>
        /// <param name="descricao">Lista de String que representa uma descrição.</param>
        /// <param name="objetoVinculado">bjeto Vinculado à descrição informada.</param>
        private void Adicionar(List<String> descricao, T objetoVinculado)
        {
            NoBuscaDescricao<T> no;
            if (Nos.Exists(n => n.Regex.IsMatch(descricao[0])))
            {
                no = Nos.First(n => n.Regex.IsMatch(descricao[0]));
            }
            else
            {
                no = new NoBuscaDescricao<T>(descricao[0], this);
            }

            if (descricao.Count == 1)
            {
                no._objetoVinculado = objetoVinculado;
                return;
            }

            no.Adicionar(descricao.GetRange(1, descricao.Count - 1), objetoVinculado);
        }

        /// <summary>
        /// Método que busca todos os nós que casam com a descricao informada.
        /// </summary>
        /// <param name="descricao">Descricao a ser buscada no ramo</param>
        /// <returns>Lista com os nós que casaram com a descricao informada</returns>
        public List<NoBuscaDescricao<T>> Buscar(String descricao)
        {
            if (String.IsNullOrEmpty(descricao))
            {
                return null;
            }

            List<NoBuscaDescricao<T>> resultado = new List<NoBuscaDescricao<T>>();

            if (this.Regex != null && this.Regex.IsMatch(descricao) && this.ObjetoVinculado != null)
            {
                resultado.Add(this);
            }

            if (Nos.Exists(n => n.Regex.IsMatch(descricao)))
            {
                foreach (NoBuscaDescricao<T> no in Nos.Where(n => n.Regex.IsMatch(descricao)))
                {
                    List<NoBuscaDescricao<T>> resultadoFilho = no.Buscar(descricao);
                    if (resultadoFilho != null)
                    {
                        resultado.AddRange(resultadoFilho.Where(d => d.ObjetoVinculado != null));
                    }
                }
            }
            return resultado;
        }

        /// <summary>
        /// Busca o objeto vinculado mais específico do ramo
        /// </summary>
        /// <param name="descricao">Descricao a ser procurada</param>
        /// <returns>Objeto NoBuscaDescricao correspondente ao nó mais específico.</returns>
        public NoBuscaDescricao<T> BuscarMaisEspecifico(String descricao)
        {
            descricao = Helper.RemoverAcentos(descricao);
            List<NoBuscaDescricao<T>> resultado = this.Buscar(descricao);
            if (resultado != null && resultado.Count > 0)
            {
                return resultado.OrderByDescending(n => n.Profundidade).First();
            }

            return null;
        }
        #endregion
    }
}
