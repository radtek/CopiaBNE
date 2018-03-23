using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;

namespace Bne.Web.Services.API.DTO
{
    [ApiExplorerSettings(IgnoreApi=false)]
    public class PesquisaCurriculo : DTO.Request
    {
        #region Pagina
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public int Pagina
        {
            get;
            set;
        }
        #endregion

        #region Funcao
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public String Funcao
        {
            get;
            set;
        }
        #endregion

        #region Cidade
        /// <summary>
        /// Campo opcional. Enviar no formato nome-da-cidade/UF
        /// </summary>
        public String Cidade
        {
            get;
            set;
        }
        #endregion

        #region PalavraChave
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public string PalavraChave
        {
            get;
            set;
        }
        #endregion

        #region Estado
        /// <summary>
        /// Campo opcional. Enviar abreviatura: UF
        /// </summary>
        public String Estado
        {
            get;
            set;
        }
        #endregion

        #region Escolaridade
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public String Escolaridade
        {
            get;
            set;
        }
        #endregion

        #region Sexo
        /// <summary>
        /// Campo opcional. Enviar "Masculino", "Feminino" ou vazio para ambos
        /// </summary>
        public String Sexo
        {
            get;
            set;
        }
        #endregion

        #region IdadeMinima
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public int? IdadeMinima
        {
            get;
            set;
        }
        #endregion

        #region IdadeMaxima
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public int? IdadeMaxima
        {
            get;
            set;
        }
        #endregion

        #region SalarioMinimo
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public decimal? SalarioMinimo
        {
            get;
            set;
        }
        #endregion

        #region SalarioMaximo
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public decimal? SalarioMaximo
        {
            get;
            set;
        }
        #endregion

        #region QuantidadeExperiencia
        /// <summary>
        /// Campo opcional. Experiência em meses
        /// </summary>
        public Int64? QuantidadeExperiencia
        {
            get;
            set;
        }
        #endregion

        #region Idioma
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public String[] Idioma
        {
            get;
            set;
        }
        #endregion

        #region CodCPFNome
        /// <summary>
        /// Campo opcional. Possível enviar nome, código do currículo no BNE ou CPF
        /// </summary>
        public string CodCPFNome
        {
            get;
            set;
        }
        #endregion

        #region EstadoCivil
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public String EstadoCivil
        {
            get;
            set;
        }
        #endregion

        #region Bairro
        /// <summary>
        /// Tamanho do campo: 50.
        /// Campo opcional.
        /// </summary>
        public string Bairro
        {
            get;
            set;
        }
        #endregion

        #region Logradouro
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo opcional.
        /// </summary>
        public string Logradouro
        {
            get;
            set;
        }
        #endregion

        #region CEPMinimo
        /// <summary>
        /// Tamanho do campo: 8.
        /// Campo opcional.
        /// </summary>
        public string CEPMinimo
        {
            get;
            set;
        }
        #endregion

        #region CEPMaximo
        /// <summary>
        /// Tamanho do campo: 8.
        /// Campo opcional.
        /// </summary>
        public string CEPMaximo
        {
            get;
            set;
        }
        #endregion

        #region CursoTecnicoGraduacao
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public String CursoTecnicoGraduacao
        {
            get;
            set;
        }
        #endregion

        #region InstituicaoTecnicoGraduacao
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public String InstituicaoTecnicoGraduacao
        {
            get;
            set;
        }
        #endregion

        #region CursoOutrosCursos
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public String CursoOutrosCursos
        {
            get;
            set;
        }
        #endregion

        #region InstituicaoOutrosCursos
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public String InstituicaoOutrosCursos
        {
            get;
            set;
        }
        #endregion

        #region EmpresaQueJaTrabalhou
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo opcional.
        /// </summary>
        public string EmpresaQueJaTrabalhou
        {
            get;
            set;
        }
        #endregion

        #region AreaEmpresaQueJaTrabalhou
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public String AreaEmpresaQueJaTrabalhou
        {
            get;
            set;
        }
        #endregion

        #region CategoriaHabilitacao
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public String CategoriaHabilitacao
        {
            get;
            set;
        }
        #endregion

        #region DDDTelefone
        /// <summary>
        /// Tamanho do campo: 2.
        /// Campo opcional.
        /// </summary>
        public string DDDTelefone
        {
            get;
            set;
        }
        #endregion

        #region NumeroTelefone
        /// <summary>
        /// Tamanho do campo: 8.
        /// Campo opcional.
        /// </summary>
        public string NumeroTelefone
        {
            get;
            set;
        }
        #endregion

        #region Email
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo opcional.
        /// </summary>
        public string Email
        {
            get;
            set;
        }
        #endregion

        #region Deficiencia
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public String Deficiencia
        {
            get;
            set;
        }
        #endregion

        #region TipoVeiculo
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public String TipoVeiculo
        {
            get;
            set;
        }
        #endregion

        #region Raca
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public String Raca
        {
            get;
            set;
        }
        #endregion

        #region PossuiFilhos
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public bool? PossuiFilhos
        {
            get;
            set;
        }
        #endregion

        #region QueroContratarEstagiarios
        /// <summary>
        /// Tamanho do campo: 50.
        /// Campo opcional.
        /// </summary>
        public bool QueroContratarEstagiarios
        {
            get;
            set;
        }
        #endregion
    }
}