using LanHouse.Entities.BNE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business
{
    public class Candidatura
    {

        #region AtualizaNumeroCandidaturasUsuario
        /// <summary>
        /// Atualiza a quantidade de candidaturas disponível para o usuário
        /// </summary>
        /// <param name="idCurriculo"></param>
        /// <returns></returns>
        public static bool AtualizaNumeroCandidaturasUsuario(int idCurriculo)
        {
            using (var entity = new LanEntities())
            {
                try 
                { 
                    var parametro_CV = (
                        from parametro_cv in entity.TAB_Parametro_Curriculo
                        where parametro_cv.Idf_Curriculo == idCurriculo
                        && parametro_cv.Idf_Parametro == (int)Business.Enumeradores.Parametro.QuantidadeCandidaturaDegustacao
                        select parametro_cv).FirstOrDefault();
            
                    int quantidadeAtual = Convert.ToInt32(parametro_CV.Vlr_Parametro);
                    quantidadeAtual = quantidadeAtual - 1;

                    parametro_CV.Vlr_Parametro = quantidadeAtual.ToString();

                    entity.SaveChanges();

                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        #endregion

        #region QuantidadeCandidaturasDisponiveisUsuario
        /// <summary>
        /// Retorna a quantidade de candidaturas disponível para o usuário
        /// </summary>
        /// <param name="idCurriculo"></param>
        /// <returns></returns>
        public static int QuantidadeCandidaturasDisponiveisUsuario(int idCurriculo)
        {
            using (var entity = new LanEntities())
            {
                var query = (
                        from parametro_cv in entity.TAB_Parametro_Curriculo
                        where parametro_cv.Idf_Curriculo == idCurriculo
                        && parametro_cv.Idf_Parametro == (int)Business.Enumeradores.Parametro.QuantidadeCandidaturaDegustacao
                        select parametro_cv.Vlr_Parametro).FirstOrDefault();

                    return Convert.ToInt32(query);
            }
            
        }
        #endregion

        #region VerificaCandidaturaVaga
        /// <summary>
        /// Verifica se o candidato já se candidatou a uma vaga específica
        /// </summary>
        public static bool VerificaCandidaturaVaga(int idCurriculo, int idVaga)
        {
            using (var entity = new LanEntities())
            {
                var query = (
                    from vaga_candidato in entity.BNE_Vaga_Candidato
                    where vaga_candidato.Idf_Curriculo == idCurriculo
                        && vaga_candidato.Idf_Vaga == idVaga
                    select vaga_candidato).Count();

                return query > 0;
            }
        }
        #endregion

        #region InserirCandidatura
        public static bool InserirCandidatura(int idCurriculo, int idVaga)
        {
            using (var entity = new LanEntities())
            {
                try
                { 
                    var vaga = entity.BNE_Vaga.FirstOrDefault(x => x.Idf_Vaga == idVaga);

                    if (vaga != null)
                    {
                        entity.BNE_Vaga_Candidato.Add(new BNE_Vaga_Candidato
                        {
                            Idf_Vaga = idVaga,
                            Idf_Curriculo = idCurriculo,
                            Dta_Cadastro = DateTime.Now,
                            Idf_Status_Curriculo_Vaga = vaga.Flg_Receber_Todos_CV.HasValue && vaga.Flg_Receber_Todos_CV.Value ? (int)Enumeradores.StatusCurriculoVaga.AguardoEnvio : (int)Enumeradores.StatusCurriculoVaga.SemEnvio, //Aguardando Envio - para que o BNE processe a candidatura
                            Flg_Inativo = false,
                            Flg_Auto_Candidatura = false
                        });
                        entity.SaveChanges();

                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
        #endregion
    }
}
