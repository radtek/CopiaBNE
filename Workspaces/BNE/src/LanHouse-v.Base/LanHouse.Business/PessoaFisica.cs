using LanHouse.Entities.BNE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business
{
    public class PessoaFisica
    {
        #region ValidarCPFeDataNascimento

        /// <summary>
        /// Validar os dados do Candidato
        /// Retorna um inteiro indicando:
        /// 0- CPF não existe na base
        /// 1- CPF existe, mas digitou a data de nascimento errada
        /// 2- CPF e Data de nascimento corretos
        /// </summary>
        /// <param name="cpf"></param>
        /// <param name="dataNascimento"></param>
        /// <param name="objPessoaFisica"></param>
        /// <returns></returns>
        public static int ValidarCPFeDataNascimento(decimal cpf, DateTime dataNascimento, out TAB_Pessoa_Fisica objPessoaFisica)
        {
            int retorno = 0;
            using (var entity = new LanEntities())
            {
                objPessoaFisica = entity.TAB_Pessoa_Fisica.FirstOrDefault(p => p.Num_CPF == cpf);

                if (objPessoaFisica != null)
                {
                    objPessoaFisica = entity.TAB_Pessoa_Fisica.FirstOrDefault(p => p.Num_CPF == cpf && p.Dta_Nascimento == dataNascimento);
                    if (objPessoaFisica != null)
                    {
                        entity.Entry(objPessoaFisica).Reference(c => c.TAB_Cidade).Load();
                        retorno = 2;
                    }
                    else
                    {
                        retorno = 1;
                    }
                }
                
                return retorno;
            }
        }

        #endregion

        #region CarregarPorCPF
        public static bool CarregarPorCPF(decimal cpf, out TAB_Pessoa_Fisica objPessoaFisica)
        {
            using (var entity = new LanEntities())
            {
                objPessoaFisica = entity.TAB_Pessoa_Fisica.FirstOrDefault(p => p.Num_CPF == cpf);
                return objPessoaFisica != null;
            }
        }
        #endregion

        #region CarregarPorCV
        public static bool CarregarPorCV(int idCurriculo, out TAB_Pessoa_Fisica objPessoaFisica)
        {
            using (var entity = new LanEntities())
            {
                objPessoaFisica = (from pessoa in entity.TAB_Pessoa_Fisica
                                   join curriculo in entity.BNE_Curriculo on pessoa.Idf_Pessoa_Fisica equals curriculo.Idf_Pessoa_Fisica
                                   where curriculo.Idf_Curriculo == idCurriculo
                                   select pessoa).FirstOrDefault();

                return objPessoaFisica != null;
            }
        }
        public static bool CarregarPorCV(int idCurriculo, out TAB_Pessoa_Fisica objPessoaFisica, LanEntities context)
        {
            objPessoaFisica = (from pessoa in context.TAB_Pessoa_Fisica
                               join curriculo in context.BNE_Curriculo on pessoa.Idf_Pessoa_Fisica equals curriculo.Idf_Pessoa_Fisica
                                where curriculo.Idf_Curriculo == idCurriculo
                                select pessoa).FirstOrDefault();

            return objPessoaFisica != null;
        }
        #endregion

        #region SalvarImagemCandidato

        public static bool SalvarImagemCandidato(int idPessoaFisica, byte[] imagemCandidato, LanEntities context)
        {
            bool retorno = false;

            //flagar inativo para as antigas
            var imagens = (from img in context.TAB_Pessoa_Fisica_Foto
                           where img.Idf_Pessoa_Fisica == idPessoaFisica && img.Flg_Inativo == false
                           select img).ToList();

            foreach (var img in imagens)
            {
                img.Flg_Inativo = true;
            }

            //adicionar nova imagem
            TAB_Pessoa_Fisica_Foto foto = new TAB_Pessoa_Fisica_Foto();

            foto.Idf_Pessoa_Fisica = idPessoaFisica;
            foto.Img_Pessoa = imagemCandidato;
            foto.Dta_Cadastro = DateTime.Now;
            foto.Dta_Alteracao = DateTime.Now;
            foto.Flg_Inativo = false;

            context.TAB_Pessoa_Fisica_Foto.Add(foto);

            context.SaveChanges();

            retorno = true;

            return retorno;
        }

        #endregion


        #region CarregarImagemCandidato
        /// <summary>
        /// Carregar a imagem(foto) do candidato
        /// </summary>
        /// <param name="idPessoaFisica"></param>
        /// <returns></returns>
        public static byte[] CarregarImagemCandidato(int idPessoaFisica)
        {
            using (var entity = new LanEntities())
            {

                var imagem = entity.TAB_Pessoa_Fisica_Foto.FirstOrDefault(c => c.Idf_Pessoa_Fisica == idPessoaFisica && !c.Flg_Inativo);

                if (imagem != null && imagem.Img_Pessoa != null)
                    return imagem.Img_Pessoa;

                return null;
            }
        }
        #endregion 

    }
}
