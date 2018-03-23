using LanHouse.Entities.BNE;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business
{
    public class PessoaFisicaFormacao
    {
        #region ListarFormacao
        /// <summary>
        /// Listar as formações da pessoa
        /// </summary>
        /// <returns></returns>
        public static List<DTO.Formacao> ListarFormacao(int idPessoaFisica)
        {
            using (var entity = new LanEntities())
            {
                var query = (
                    from formacao in entity.BNE_Formacao
                    join cursoTemp in entity.TAB_Curso on formacao.Idf_Curso equals cursoTemp.Idf_Curso into cur from curso in cur.DefaultIfEmpty()
                    join fonteTemp in entity.TAB_Fonte on formacao.Idf_Fonte equals fonteTemp.Idf_Fonte into font from fonte in font.DefaultIfEmpty()
                    join esc in entity.TAB_Escolaridade on formacao.Idf_Escolaridade equals esc.Idf_Escolaridade
                    join cidadeTemp in entity.TAB_Cidade on formacao.Idf_Cidade equals cidadeTemp.Idf_Cidade into ef from cidade in ef.DefaultIfEmpty()
                    where formacao.Idf_Pessoa_Fisica == idPessoaFisica && formacao.Flg_Inativo == false && formacao.Idf_Escolaridade < 18
                    select new DTO.Formacao
                    {
                        anoConclusao = formacao.Ano_Conclusao,
                        idFormacao = formacao.Idf_Formacao,
                        idEscolaridade = formacao.Idf_Escolaridade,
                        nivel = esc.Des_BNE,
                        idCurso = formacao.Idf_Curso,
                        curso = curso == null ? "" : curso.Des_Curso,
                        grau = esc.Idf_Grau_Escolaridade,
                        idCidade = formacao.Idf_Cidade,
                        cidadeFormacao = cidade == null ? "" : cidade.Nme_Cidade + "/" + cidade.Sig_Estado,
                        cargaHoraria = formacao.Qtd_Carga_Horaria,
                        idSituacaoFormacao = formacao.Idf_Situacao_Formacao,
                        descricaoCurso = formacao.Des_Curso,
                        instituicao = formacao.Idf_Fonte != null ? fonte.Nme_Fonte : formacao.Des_Fonte
                    }).ToList();

                return query;
            }
        }
        #endregion

        #region ListarCursos
        /// <summary>
        /// Listar os cursos da pessoa
        /// </summary>
        /// <returns></returns>
        public static List<DTO.Formacao> ListarCursos(int idPessoaFisica)
        {
            using (var entity = new LanEntities())
            {
                var query = (
                    from formacao in entity.BNE_Formacao
                    join esc in entity.TAB_Escolaridade on formacao.Idf_Escolaridade equals esc.Idf_Escolaridade
                    join cidadeTemp in entity.TAB_Cidade on formacao.Idf_Cidade equals cidadeTemp.Idf_Cidade into ef
                    from cidade in ef.DefaultIfEmpty()
                    where formacao.Idf_Pessoa_Fisica == idPessoaFisica && formacao.Idf_Escolaridade == 18 && formacao.Flg_Inativo == false
                    select new DTO.Formacao
                    {
                        anoConclusao = formacao.Ano_Conclusao,
                        idFormacao = formacao.Idf_Formacao,
                        idEscolaridade = formacao.Idf_Escolaridade,
                        nivel = esc.Des_BNE,
                        curso = formacao.Des_Curso,
                        grau = esc.Idf_Grau_Escolaridade,
                        idCidade = formacao.Idf_Cidade,
                        cidadeFormacao = cidade == null ? "" : cidade.Nme_Cidade + "/" + cidade.Sig_Estado,
                        cargaHoraria = formacao.Qtd_Carga_Horaria,
                        idSituacaoFormacao = formacao.Idf_Situacao_Formacao,
                        descricaoCurso = formacao.Des_Curso,
                        instituicao = formacao.Des_Fonte
                    }).ToList();

                return query;
            }
        }
        #endregion
    }
}