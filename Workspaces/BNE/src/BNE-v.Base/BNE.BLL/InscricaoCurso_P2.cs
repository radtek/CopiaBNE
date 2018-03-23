//-- Data: 16/04/2013 16:57
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Specialized;
using System.Data.SqlClient;
using BNE.BLL.Custom;

namespace BNE.BLL
{
    public partial class InscricaoCurso // Tabela: BNE_Inscricao_Curso
    {

        #region Matricular
        public static bool Matricular(Curriculo objCurriculo, CursoParceiroTecla objCursoParceiro, out string login, out string senha)
        {
            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        objCursoParceiro.ParceiroTecla.CompleteObject(trans);

                        CadastroParceiro objCadastroParceiro;
                        if (CadastroParceiro.PossuiCadastroParceiro(objCurriculo, objCursoParceiro.ParceiroTecla, out objCadastroParceiro, trans))
                        {
                            login = objCadastroParceiro.DescricaoLogin;
                            senha = objCadastroParceiro.DescricaoSenha;
                        }
                        else
                        {
                            CadastrarCandidato(Curriculo.CarregarCurriculoDTO(objCurriculo.IdCurriculo), objCursoParceiro.ParceiroTecla, out login, out senha);
                            objCadastroParceiro = new CadastroParceiro
                                {
                                    Curriculo = objCurriculo,
                                    ParceiroTecla = objCursoParceiro.ParceiroTecla,
                                    DescricaoLogin = login,
                                    DescricaoSenha = senha
                                };

                            objCadastroParceiro.Save(trans);
                        }

                        var objInscricao = new InscricaoCurso
                            {
                                Curriculo = objCurriculo,
                                CursoParceiroTecla = objCursoParceiro,
                                DataInscricao = DateTime.Now
                            };

                        objInscricao.Save(trans);

                        trans.Commit();

                        return true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        EL.GerenciadorException.GravarExcecao(ex);
                        throw;
                    }
                }
            }
        }

        #endregion

        #region CadastrarCandidato
        private static bool CadastrarCandidato(DTO.Curriculo objCurriculo, ParceiroTecla objParceiro, out string login, out string senha)
        {
            senha = Guid.NewGuid().ToString().Substring(0, 8);
            login = objCurriculo.Email;

            try
            {
                string url = objParceiro.DescricaoURLCadastro;
                const string contentType = "application/x-www-form-urlencoded";

                var parametrosRequisicao = new NameValueCollection
                    {
                        {"aluno_nome", objCurriculo.NomeCompleto},
                        {"aluno_cpf", Helper.FormatarCPF(objCurriculo.NumeroCPF)},
                        {"aluno_data_nasc", objCurriculo.DataNascimento.ToShortDateString()},
                        {"aluno_email", objCurriculo.Email},
                        {"aluno_cep", objCurriculo.NumeroCEP},
                        {"aluno_endereco", objCurriculo.Logradouro},
                        {"aluno_endereco_num", objCurriculo.NumeroEndereco},
                        {"aluno_bairro", objCurriculo.Bairro},
                        {"aluno_cidade", objCurriculo.NomeCidade},
                        {"aluno_uf", objCurriculo.SiglaEstado},
                        {"aluno_tel", Helper.FormatarTelefone(objCurriculo.NumeroDDDCelular, objCurriculo.NumeroCelular)},
                        {"aluno_senha", senha},
                        {"aluno_conf_senha", senha},
                    };

                Custom.Helper.EfetuarRequisicao(url, parametrosRequisicao, contentType);

                return true;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
            return false;
        }
        #endregion

    }
}