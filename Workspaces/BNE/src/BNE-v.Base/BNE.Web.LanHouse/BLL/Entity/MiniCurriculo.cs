using System;
using System.Collections.Generic;
using System.Linq;
using BNE.BLL;
using BNE.BLL.Integracoes.Facebook;
using BNE.Web.LanHouse.EntityFramework;

namespace BNE.Web.LanHouse.BLL.Entity
{
    public class MiniCurriculo
    {

        #region Membros públicos
        // tela2
        public string DDD { get; private set; }
        public string NumCelular { get; private set; }
        public string NomeCompleto { get; private set; }
        public Enumeradores.Sexo Sexo { get; private set; }
        // tela3
        public decimal Cpf { get; private set; }
        public DateTime DataNasc { get; private set; }
        public string Email { get; private set; }
        public string Cargo { get; private set; }
        public int? IdFuncao { get; private set; }
        public decimal? Salario { get; private set; }

        public int IdadePessoa { get; private set; }
        public byte[] Foto { get; private set; }
        #endregion Membros públicos

        #region Construtores
        public MiniCurriculo(BNE_Curriculo objCurriculo)
        {
            this.DDD = objCurriculo.TAB_Pessoa_Fisica.Num_DDD_Celular;
            this.NumCelular = Code.Helper.FormatarTelefone(objCurriculo.TAB_Pessoa_Fisica.Num_Celular);
            this.NomeCompleto = objCurriculo.TAB_Pessoa_Fisica.Nme_Pessoa;
            this.Sexo = (Enumeradores.Sexo)objCurriculo.TAB_Pessoa_Fisica.Idf_Sexo;

            this.Cpf = objCurriculo.TAB_Pessoa_Fisica.Num_CPF;
            this.DataNasc = objCurriculo.TAB_Pessoa_Fisica.Dta_Nascimento;
            this.Email = objCurriculo.TAB_Pessoa_Fisica.Eml_Pessoa;

            this.IdadePessoa = BNE.BLL.PessoaFisica.RetornarIdade(objCurriculo.TAB_Pessoa_Fisica.Dta_Nascimento);

            BNE_Funcao_Pretendida objFuncaoPretendida;
            objFuncaoPretendida =
                objCurriculo
                    .BNE_Funcao_Pretendida
                    .FirstOrDefault(fp => fp.Idf_Funcao != null || !String.IsNullOrEmpty(fp.Des_Funcao_Pretendida));

            if (objFuncaoPretendida == null)
                throw new InvalidOperationException(String.Format("O currículo {0} não possui nenhuma função pretendida!", objCurriculo.Idf_Curriculo));

            if (objFuncaoPretendida.Idf_Funcao.HasValue)
            {
                TAB_Funcao objFuncao;
                if (BLL.Funcao.CarregarPorId(objFuncaoPretendida.Idf_Funcao.Value, out objFuncao))
                {
                    this.Cargo = objFuncao.Des_Funcao;
                }
                else
                    throw new InvalidOperationException(String.Format("Não foi encontrada a função {0} no banco de dados", objFuncaoPretendida.Idf_Funcao.Value));
            }
            else
            {
                this.Cargo = objFuncaoPretendida.Des_Funcao_Pretendida;
            }

            this.Salario = objCurriculo.Vlr_Pretensao_Salarial;

            TAB_Pessoa_Fisica_Foto objPessoaFisicaFoto;
            objPessoaFisicaFoto =
                objCurriculo
                    .TAB_Pessoa_Fisica
                    .TAB_Pessoa_Fisica_Foto
                    .FirstOrDefault();

            if (objPessoaFisicaFoto != null)
                this.Foto = objPessoaFisicaFoto.Img_Pessoa;
        }
        #endregion Construtores

        #region Métodos públicos

        public static bool Salvar(ref int idPessoaFisica, string ipAddress, int idCidade, int? idOrigemFilial, int idOrigemLan, string cartaBoasVindas, SegundaTela tela2, TerceiraTela tela3, ProfileFacebook.DadosFacebook dadosFacebook, out string mensagemErro)
        {
            mensagemErro = string.Empty;

            BNE.BLL.PessoaFisica objPessoaFisica;

            if (idPessoaFisica != 0)
                objPessoaFisica = BNE.BLL.PessoaFisica.LoadObject(idPessoaFisica);
            else
            {
                if (!BNE.BLL.PessoaFisica.CarregarPorCPF(tela3.Cpf, out objPessoaFisica))
                    objPessoaFisica = new BNE.BLL.PessoaFisica();
            }

            BNE.BLL.Curriculo objCurriculo;
            BNE.BLL.UsuarioFilialPerfil objUsuarioFilialPerfil;
            BNE.BLL.PessoaFisicaComplemento objPessoaFisicaComplemento;
            BNE.BLL.PessoaFisicaFoto objPessoaFisicaFoto;

            //Se a pessoa informada estiver com um CPF no cadastro diferente do informado, invalida o salvamento
            int idNoCadastro;
            if (idPessoaFisica != 0 && BNE.BLL.PessoaFisica.ExistePessoaFisica(tela3.Cpf, out idNoCadastro) && idPessoaFisica != idNoCadastro)
                return false;

            if (!BNE.BLL.PessoaFisicaFoto.CarregarFoto(objPessoaFisica.IdPessoaFisica, out objPessoaFisicaFoto))
                objPessoaFisicaFoto = new BNE.BLL.PessoaFisicaFoto();

            if (objPessoaFisica.Endereco != null)
                objPessoaFisica.Endereco.CompleteObject();
            else
                objPessoaFisica.Endereco = new BNE.BLL.Endereco();

            if (!BNE.BLL.PessoaFisicaComplemento.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objPessoaFisicaComplemento))
                objPessoaFisicaComplemento = new BNE.BLL.PessoaFisicaComplemento();

            if (!BNE.BLL.Curriculo.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objCurriculo))
                objCurriculo = new BNE.BLL.Curriculo();

            if (!BNE.BLL.UsuarioFilialPerfil.CarregarUsuarioFilialPerfilCandidatoAtivoEInativo(objPessoaFisica, out objUsuarioFilialPerfil))
            {
                objUsuarioFilialPerfil = new BNE.BLL.UsuarioFilialPerfil
                {
                    Perfil = new BNE.BLL.Perfil((int)BNE.BLL.Enumeradores.Perfil.AcessoNaoVIP)
                };
            }

            var listFuncoesPretendidas = new List<BNE.BLL.FuncaoPretendida>();
            ObterFuncoesPretendidas(tela3, listFuncoesPretendidas);

            //Pessoa Física
            objPessoaFisica.NumeroCPF = tela3.Cpf.ToString();
            objPessoaFisica.DataNascimento = tela2.DataNasc;
            objPessoaFisica.NomePessoa = tela2.NomeCompleto;
            objPessoaFisica.NomePessoaPesquisa = BNE.BLL.Custom.Helper.RemoverAcentos(tela2.NomeCompleto);
            objPessoaFisica.Sexo = new BNE.BLL.Sexo(tela2.Sexo);
            objPessoaFisica.NumeroDDDCelular = tela2.DDD;
            objPessoaFisica.NumeroCelular = tela2.NumCelular;
            objPessoaFisica.FlagInativo = false;
            objPessoaFisica.DescricaoIP = objCurriculo.DescricaoIP = ipAddress;
            objPessoaFisica.EmailPessoa = tela3.Email;

            //Endereco
            BNE.BLL.Cidade objCidade = BNE.BLL.Cidade.LoadObject(idCidade);
            objPessoaFisica.Endereco.Cidade = objCidade;
            objPessoaFisica.Cidade = objCidade;

            objCurriculo.ValorPretensaoSalarial = tela3.Salario;

            var objOrigem = idOrigemFilial.HasValue ? BNE.BLL.Origem.LoadObject((int)idOrigemFilial) : null;

            #region Validação Celular
            var celularValidado = false;
            if (!objPessoaFisica.FlagCelularConfirmado && !string.IsNullOrWhiteSpace(tela2.CodigoValidacaoCelular))
            {
                celularValidado = BNE.BLL.PessoaFisica.ValidacaoCelularUtilizarCodigo(objPessoaFisica.NumeroDDDCelular, objPessoaFisica.NumeroCelular, tela2.CodigoValidacaoCelular);

                if (!celularValidado)
                {
                    mensagemErro = "O código de validação informado é inválido!";
                    return false;
                }
            }
            #endregion

            //Salvar
            objCurriculo.SalvarMiniCurriculo(objPessoaFisica,
                listFuncoesPretendidas,
                objOrigem,
                cartaBoasVindas,
                objUsuarioFilialPerfil,
                objPessoaFisicaComplemento,
                null,
                objPessoaFisicaFoto,
                dadosFacebook,
                celularValidado,
                new Origem(idOrigemLan));

            // retorna id da pessoa fisica
            idPessoaFisica = objPessoaFisica.IdPessoaFisica;

            return true;
        }

        public static bool Salvar(ref int idPessoaFisica, string ipAddress, int idCidade, SextaTela tela6)
        {
            BNE.BLL.PessoaFisicaComplemento objPessoaFisicaComplemento;
            BNE.BLL.Curriculo objCurriculo;

            BNE.BLL.PessoaFisica objPessoaFisica = BNE.BLL.PessoaFisica.LoadObject(idPessoaFisica);

            if (objPessoaFisica.Endereco != null)
                objPessoaFisica.Endereco.CompleteObject();
            else
                objPessoaFisica.Endereco = new BNE.BLL.Endereco();

            if (!BNE.BLL.Curriculo.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objCurriculo))
                objCurriculo = new BNE.BLL.Curriculo();

            if (!BNE.BLL.PessoaFisicaComplemento.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objPessoaFisicaComplemento))
                objPessoaFisicaComplemento = new BNE.BLL.PessoaFisicaComplemento();

            Contato objContatoTelefone;
            if (Contato.CarregarPorPessoaFisicaTipoContato(objPessoaFisica.IdPessoaFisica, (int)BNE.BLL.Enumeradores.TipoContato.RecadoFixo, out objContatoTelefone, null))
            {
                if (!String.IsNullOrEmpty(tela6.TelefoneRecadoFone) && !String.IsNullOrEmpty(tela6.TelefoneRecadoDDD))
                {
                    objContatoTelefone.NumeroDDDTelefone = tela6.TelefoneRecadoDDD;
                    objContatoTelefone.NumeroTelefone = tela6.TelefoneRecadoFone;
                    objContatoTelefone.NomeContato = tela6.FalarCom;
                    objContatoTelefone.TipoContato_ = new TipoContato((int)BNE.BLL.Enumeradores.TipoContato.RecadoFixo);
                }
                else
                {
                    objContatoTelefone.NumeroDDDTelefone = string.Empty;
                    objContatoTelefone.NumeroTelefone = string.Empty;
                    objContatoTelefone.NomeContato = string.Empty;
                    objContatoTelefone.TipoContato_ = new TipoContato((int)BNE.BLL.Enumeradores.TipoContato.RecadoFixo);
                }
            }
            else
            {
                objContatoTelefone = new Contato
                {
                    NumeroDDDTelefone = tela6.TelefoneRecadoDDD,
                    NumeroTelefone = tela6.TelefoneRecadoFone,
                    NomeContato = tela6.FalarCom,
                    TipoContato_ = new TipoContato((int)BNE.BLL.Enumeradores.TipoContato.RecadoFixo),
                    PessoaFisicaComplemento = objPessoaFisicaComplemento
                };
            }

            objPessoaFisica.Endereco.NumeroCEP = tela6.Cep;
            objPessoaFisica.EstadoCivil = new EstadoCivil(tela6.IdEstadoCivil);

            objCurriculo.SalvarDadosPessoais(objPessoaFisica, objPessoaFisicaComplemento, objContatoTelefone, null, null, null, null, null, null, null, null, null, null, null,null,null);

            return true;
        }

        public static bool Salvar(ref int idPessoaFisica, string recuperarIP, QuintaTela tela5)
        {
            BNE.BLL.Curriculo objCurriculo;
            BNE.BLL.PessoaFisica objPessoaFisica = BNE.BLL.PessoaFisica.LoadObject(idPessoaFisica);

            if (objPessoaFisica.Endereco != null)
                objPessoaFisica.Endereco.CompleteObject();
            else
                objPessoaFisica.Endereco = new BNE.BLL.Endereco();

            if (!BNE.BLL.Curriculo.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objCurriculo))
                objCurriculo = new BNE.BLL.Curriculo();

            Formacao objFormacao = new Formacao();

            objFormacao.PessoaFisica = objPessoaFisica;
            objFormacao.Escolaridade = new BNE.BLL.Escolaridade(tela5.Escolaridade);

            string nomeInstituicao = string.Empty;
            if (tela5.InstituicaoEnsino != null)
            {
                string[] instituicaoSigla = tela5.InstituicaoEnsino.Split('-');
                if (instituicaoSigla.Length.Equals(1))
                    nomeInstituicao = instituicaoSigla[0].Trim();
                else
                    nomeInstituicao = instituicaoSigla[1].Trim();
            }

            if (!String.IsNullOrEmpty(nomeInstituicao))
            {
                BNE.BLL.Fonte objFonte;
                if (BNE.BLL.Fonte.CarregarPorNome(nomeInstituicao, out objFonte))
                {
                    objFormacao.Fonte = objFonte;
                    objFormacao.DescricaoFonte = String.Empty;
                }
                else
                {
                    objFormacao.Fonte = null;
                    objFormacao.DescricaoFonte = nomeInstituicao;
                }
            }
            else
                objFormacao.Fonte = null;

            if (!String.IsNullOrEmpty(tela5.NomeCurso))
            {
                Curso objCurso;
                if (Curso.CarregarPorNome(tela5.NomeCurso, out objCurso))
                {
                    objFormacao.Curso = objCurso;
                    objFormacao.DescricaoCurso = String.Empty;
                }
                else
                {
                    objFormacao.Curso = null;
                    objFormacao.DescricaoCurso = tela5.NomeCurso;
                }
            }
            else
                objFormacao.Curso = null;

            if (tela5.Situacao.HasValue)
                objFormacao.SituacaoFormacao = new SituacaoFormacao((short)tela5.Situacao.Value);
            else
                objFormacao.SituacaoFormacao = null;

            if (tela5.Periodo.HasValue)
                objFormacao.NumeroPeriodo = (short)tela5.Periodo.Value;
            else
                objFormacao.NumeroPeriodo = null;

            if (tela5.AnoConclusao.HasValue)
                objFormacao.AnoConclusao = (short)tela5.AnoConclusao;
            else
                objFormacao.AnoConclusao = null;

            BNE.BLL.Cidade objCidade;
            if (BNE.BLL.Cidade.CarregarPorNome(tela5.Cidade, out objCidade))
                objFormacao.Cidade = objCidade;
            else
                objFormacao.Cidade = null;

            objFormacao.Save();

            BNE.BLL.ExperienciaProfissional objExperienciaProfissional1 = null;
            if (tela5.ExperienciaProfissional.Count > 0 && tela5.ExperienciaProfissional.ElementAt(0) != null)
            {
                var ep = tela5.ExperienciaProfissional.ElementAt(0);
                objExperienciaProfissional1 = SalvarExperienciasProfissionais(ep.NomeEmpresa, ep.AreaBNE, ep.DataAdmissao, ep.DataDemissao, ep.DescricaoFuncao, ep.Atribuicoes, null);
                objCurriculo.ValorUltimoSalario = ep.UltimoSalario;
            }

            BNE.BLL.ExperienciaProfissional objExperienciaProfissional2 = null;
            if (tela5.ExperienciaProfissional.Count > 1 && tela5.ExperienciaProfissional.ElementAt(1) != null)
            {
                var ep = tela5.ExperienciaProfissional.ElementAt(0);
                objExperienciaProfissional2 = SalvarExperienciasProfissionais(ep.NomeEmpresa, ep.AreaBNE, ep.DataAdmissao, ep.DataDemissao, ep.DescricaoFuncao, ep.Atribuicoes, null);
            }

            BNE.BLL.ExperienciaProfissional objExperienciaProfissional3 = null;
            if (tela5.ExperienciaProfissional.Count > 2 && tela5.ExperienciaProfissional.ElementAt(2) != null)
            {
                var ep = tela5.ExperienciaProfissional.ElementAt(0);
                objExperienciaProfissional3 = SalvarExperienciasProfissionais(ep.NomeEmpresa, ep.AreaBNE, ep.DataAdmissao, ep.DataDemissao, ep.DescricaoFuncao, ep.Atribuicoes, null);
            }

            //BNE.BLL.ExperienciaProfissional objExperienciaProfissional4 = null;
            //if (tela5.ExperienciaProfissional.Count > 3 && tela5.ExperienciaProfissional.ElementAt(3) != null)
            //{
            //    var ep = tela5.ExperienciaProfissional.ElementAt(0);
            //    objExperienciaProfissional4 = SalvarExperienciasProfissionais(ep.NomeEmpresa, ep.AreaBNE, ep.DataAdmissao, ep.DataDemissao, ep.DescricaoFuncao, ep.Atribuicoes, null);
            //}

            //BNE.BLL.ExperienciaProfissional objExperienciaProfissional5 = null;
            //if (tela5.ExperienciaProfissional.Count > 4 && tela5.ExperienciaProfissional.ElementAt(4) != null)
            //{
            //    var ep = tela5.ExperienciaProfissional.ElementAt(0);
            //    objExperienciaProfissional5 = SalvarExperienciasProfissionais(ep.NomeEmpresa, ep.AreaBNE, ep.DataAdmissao, ep.DataDemissao, ep.DescricaoFuncao, ep.Atribuicoes, null);
            //}

            //TODO: Ajustar
            objCurriculo.SalvarDadosPessoais(objPessoaFisica, null, null, null, null, objExperienciaProfissional1, objExperienciaProfissional2, objExperienciaProfissional3, null,null,null,null,null,null,null,null);
            objCurriculo.SalvarFormacaoCursos();

            return true;
        }

        #endregion Métodos públicos

        #region Métodos privados

        private static void ObterFuncoesPretendidas(TerceiraTela tela3, List<BNE.BLL.FuncaoPretendida> listFuncoesPretendidas)
        {
            BNE.BLL.FuncaoPretendida objFuncaoPretendida = new BNE.BLL.FuncaoPretendida();
            BNE.BLL.Funcao objFuncao = null;

            if (tela3.Cargo.HasValue)
                objFuncao = BNE.BLL.Funcao.LoadObject(tela3.Cargo.Value);

            objFuncaoPretendida.Funcao = objFuncao;
            objFuncaoPretendida.DescricaoFuncaoPretendida = tela3.DescricaoCargo;
            objFuncaoPretendida.QuantidadeExperiencia = 0;

            listFuncoesPretendidas.Add(objFuncaoPretendida);
            listFuncoesPretendidas.Add(null);
            listFuncoesPretendidas.Add(null);
        }

        #region SalvarExperienciasProfissionais
        private static BNE.BLL.ExperienciaProfissional SalvarExperienciasProfissionais(string txtEmpresa, int ddlAtividadeEmpresa, DateTime? txtDataAdmissao, DateTime? txtDataDemissao, string txtFuncaoExercida, string txtAtividadeExercida, int? idExperienciaProfessional)
        {
            BNE.BLL.ExperienciaProfissional objExperienciaProfissional;
            if (!String.IsNullOrEmpty(txtEmpresa))
            {
                if (idExperienciaProfessional.HasValue)
                    objExperienciaProfissional = BNE.BLL.ExperienciaProfissional.LoadObject(idExperienciaProfessional.Value);
                else
                    objExperienciaProfissional = new BNE.BLL.ExperienciaProfissional();

                objExperienciaProfissional.RazaoSocial = txtEmpresa;
                objExperienciaProfissional.AreaBNE = new AreaBNE(Convert.ToInt32(ddlAtividadeEmpresa));
                objExperienciaProfissional.DataAdmissao = txtDataAdmissao.Value;
                objExperienciaProfissional.DataDemissao = txtDataDemissao;
                BNE.BLL.Funcao objFuncao;
                if (BNE.BLL.Funcao.CarregarPorDescricao(txtFuncaoExercida, out objFuncao))
                {
                    objExperienciaProfissional.Funcao = objFuncao;
                    objExperienciaProfissional.DescricaoFuncaoExercida = String.Empty;
                }
                else
                {
                    objExperienciaProfissional.Funcao = null;
                    objExperienciaProfissional.DescricaoFuncaoExercida = txtFuncaoExercida;
                }

                objExperienciaProfissional.DescricaoAtividade = txtAtividadeExercida;
            }
            else if (idExperienciaProfessional.HasValue)
            {
                objExperienciaProfissional = BNE.BLL.ExperienciaProfissional.LoadObject(idExperienciaProfessional.Value);
                objExperienciaProfissional.FlagInativo = true;
            }
            else
                objExperienciaProfissional = null;

            return objExperienciaProfissional;
        }
        #endregion

        #endregion Métodos privados

    }
}