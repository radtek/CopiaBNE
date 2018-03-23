using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using BNE.BLL;
using BNE.Common;
using IMG = System.Drawing;
using Bne.Web.Services.API.DTO.CadastroCV;
using System.Linq;
using System.Data.SqlClient;

namespace Bne.Web.Services.API.Business
{
    public class Curriculos
    {
        private static Regex reNotNumeric = new Regex("[^0-9]");

        #region CarregarFoto
        /// <summary>
        /// Metodo responsável por carregar a foto 
        /// </summary>
        private static string CarregarFoto(int IdCurriculo, bool mostrarDadosCompletos)
        {
            if (mostrarDadosCompletos)
            {
                byte[] byteArray = BNE.BLL.PessoaFisicaFoto.RecuperarFotoPorCurriculoId(IdCurriculo);
                if (byteArray != null)
                {
                    Stream streamIn = new MemoryStream(byteArray);
                    IMG.Image img = IMG.Image.FromStream(streamIn);

                    //Proporção de imagem
                    decimal width = img.Width;
                    decimal heigth = img.Height;

                    while (width > 200 || heigth > 200)
                    {
                        width = Math.Truncate(width * Convert.ToDecimal(0.9));
                        heigth = Math.Truncate(heigth * Convert.ToDecimal(0.9));
                    }

                    IMG.Image thumbNail = img.GetThumbnailImage((int)width, (int)heigth, null, new IntPtr());
                    Stream streamOut = new MemoryStream();
                    thumbNail.Save(streamOut, ImageFormat.Jpeg);

                    return Convert.ToBase64String(((MemoryStream)streamOut).ToArray());
                }
                else
                    return null;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region VerDadosCadastro
        public static CadastroCurriculo VerDadosCadastro(
            int idCurriculo,
            Filial objFilial,
            UsuarioFilialPerfil objUsuarioFilialPerfil,
            bool flgDadosPessoais = false)
        {
            if (objFilial != null && objUsuarioFilialPerfil != null)
            {
                CurriculoVisualizacaoHistorico.SalvarHistoricoVisualizacao(objFilial, objUsuarioFilialPerfil, new BNE.BLL.Curriculo(idCurriculo), flgDadosPessoais, Helper.RecuperarIP());
            }

            int idPessoaFisica = PessoaFisica.RecuperarIdPorCurriculo(new Curriculo(idCurriculo));

            CadastroCurriculo cadastro = CarregarCV(new PessoaFisica(idPessoaFisica));
            

            if (!flgDadosPessoais)
            {
                var miniCurriculoIncompleto = new CadastroMiniCurriculoIncompleto();
                Helper.CopyProperties(cadastro.MiniCurriculo, miniCurriculoIncompleto);
                miniCurriculoIncompleto.DataNascimento = null;
                miniCurriculoIncompleto.DDDCelular = null;
                miniCurriculoIncompleto.NumeroCelular = null;
                miniCurriculoIncompleto.Email = null;
                miniCurriculoIncompleto.Nome = Helper.AbreviarNome(cadastro.MiniCurriculo.Nome);
                miniCurriculoIncompleto.Sexo = null;
                cadastro.MiniCurriculo = miniCurriculoIncompleto;

                cadastro.DadosPessoais = null;

                cadastro.DadosComplementares.Deficiencia = null;
                cadastro.DadosComplementares.CategoriaCNH = null;

            }

            return cadastro;
        }
        #endregion

        #region VerDadosCompleto
        public static DTO.ResultadoPesquisaCurriculoCompleto VerDadosCompleto(
            int idCurriculo,
            Filial objFilial,
            UsuarioFilialPerfil objUsuarioFilialPerfil,
            bool flgDadosPessoais = false)
        {
            if (objFilial != null && objUsuarioFilialPerfil != null)
            {
                CurriculoVisualizacaoHistorico.SalvarHistoricoVisualizacao(objFilial, objUsuarioFilialPerfil, new BNE.BLL.Curriculo(idCurriculo), flgDadosPessoais, Helper.RecuperarIP());
            }

            DTO.ResultadoPesquisaCurriculoCompleto rdcc = new DTO.ResultadoPesquisaCurriculoCompleto();

            //Tenta carregar o cv do solr, se não busca do sql
            BNE.BLL.DTO.Curriculo objCurriculo = BNE.BLL.Curriculo.CarregarCurriculoSolr(idCurriculo, true) ?? BNE.BLL.Curriculo.CarregarCurriculoDTO(idCurriculo, BNE.BLL.Curriculo.DadosCurriculo.Tudo);

            #region Dados Pessoais

            rdcc.vip = objCurriculo.VIP;
            rdcc.idCurriculo = objCurriculo.IdCurriculo;
            rdcc.dtaAtualizacao = objCurriculo.DataAtualizacaoCurriculo.ToString("yyyy-MM-dd");
            rdcc.cidade = objCurriculo.NomeCidade;
            rdcc.estado = objCurriculo.SiglaEstado;
            rdcc.bairroEndereco = objCurriculo.Bairro;
            rdcc.cpf = objCurriculo.NumeroCPF;
            rdcc.nomeCompleto = Helper.AbreviarNome(objCurriculo.NomeCompleto);
            rdcc.aceitaEstagio = objCurriculo.AceitaEstagio;
            //Carregar imagem
            rdcc.foto = CarregarFoto(objCurriculo.IdCurriculo, true);

            if (flgDadosPessoais)
            {
                rdcc.dtaNascimento = objCurriculo.DataNascimento.ToString("yyyy-MM-dd");
                rdcc.dddCelular = objCurriculo.NumeroDDDCelular;
                rdcc.numCelular = objCurriculo.NumeroCelular;
                rdcc.dddTelefone = objCurriculo.NumeroDDDTelefone;
                rdcc.numTelefone = objCurriculo.NumeroTelefone;
                rdcc.email = objCurriculo.Email;
                rdcc.nomeCompleto = objCurriculo.NomeCompleto;
                rdcc.idade = objCurriculo.Idade;
                //rdcc.orgaoExpeditor = objCurriculo.rg + "/" + objCurriculo.PessoaFisica.SiglaUFEmissaoRG;
                //rdcc.numeroRG = objCurriculo.PessoaFisica.NumeroRG;
                if (objCurriculo.Sexo != null)
                    rdcc.sexo = objCurriculo.Sexo[0];

                rdcc.carteira = objCurriculo.CategoriaHabilitacao;

                rdcc.deficiente = objCurriculo.Deficiencia;

                rdcc.estadoCivil = objCurriculo.EstadoCivil;

                #region Endereco,CEP,Cidade
                rdcc.cepEndereco = objCurriculo.NumeroCEP;
                rdcc.logradouroEndereco = objCurriculo.Logradouro;
                rdcc.numeroEndereco = objCurriculo.NumeroEndereco;
                rdcc.complementoEndereco = objCurriculo.Complemento;
                #endregion
            }

            #endregion

            #region Funções Pretendidas
            foreach (var funcao in objCurriculo.FuncoesPretendidas)
            {
                if (!String.IsNullOrEmpty(rdcc.funcoes))
                    rdcc.funcoes += ";";
                rdcc.funcoes += funcao.NomeFuncaoPretendida;
            }
            rdcc.pretensao = objCurriculo.ValorPretensaoSalarial;
            #endregion

            #region Escolaridade
            rdcc.escolaridade = objCurriculo.UltimaFormacaoCompleta;
            rdcc.formacoes = objCurriculo.Formacoes;
            #endregion

            rdcc.listExperienciaProfissional = objCurriculo.Experiencias;
            foreach (var exp in rdcc.listExperienciaProfissional)
            {
                exp.DataAdmissao = DateTime.Parse(exp.DataAdmissao.ToString("yyyy-MM-dd"));
                if (exp.DataDemissao.HasValue)
                    exp.DataDemissao = DateTime.Parse(exp.DataDemissao.Value.ToString("yyyy-MM-dd"));
            }

            rdcc.idiomas = objCurriculo.Idiomas;

            #region Observacoes
            rdcc.caracteristicasPessoais = string.Format("{0} - {1}cm - {2}kg", objCurriculo.Raca, objCurriculo.Altura, objCurriculo.Peso);
            rdcc.outrosConhecimento = objCurriculo.OutrosConhecimentos;
            rdcc.FlagViagem = objCurriculo.DisponibilidadeViajar;
            rdcc.observacoes = objCurriculo.Observacao;
            #endregion

            return rdcc;
        }
        #endregion

        /// <summary>
        /// Carrega os dados do cadastro do Currículo
        /// </summary>
        /// <param name="objPessoaFisica"></param>
        /// <returns></returns>
        public static CadastroCurriculo CarregarCV(PessoaFisica objPessoaFisica)
        {
            Curriculo objCurriculo;
            UsuarioFilialPerfil objUsuarioFilialPerfil;
            PessoaFisicaComplemento objPessoaFisicaComplemento;
            Contato objContatoCelular = null;
            Contato objContatoTelefone = null;
            List<BNE.BLL.FuncaoPretendida> lstFuncaoPretendida = null;
            List<ExperienciaProfissional> lstExperienciaProfissional = null;
            List<Formacao> lstCursosFormacao = null;
            List<Formacao> lstCursosComplementares = null;
            List<PessoafisicaIdioma> lstPessoaFisicaIdioma = null;
            List<CurriculoDisponibilidade> lstCurriculoDisponibilidade = null;
            List<PessoaFisicaVeiculo> lstPessoaFisicaVeiculo = null;
            List<CurriculoDisponibilidadeCidade> lstCurriculoDisponibilidadeCidade = null;
            List<ParametroCurriculo> lstParametrosCurriculo = new List<ParametroCurriculo>();

            CarregarDados(objPessoaFisica,
                out objCurriculo,
                out objUsuarioFilialPerfil,
                out objPessoaFisicaComplemento,
                out objContatoCelular,
                out objContatoTelefone,
                out lstFuncaoPretendida,
                out lstExperienciaProfissional,
                out lstCursosFormacao,
                out lstCursosComplementares,
                out lstPessoaFisicaIdioma,
                out lstCurriculoDisponibilidade,
                out lstPessoaFisicaVeiculo,
                out lstCurriculoDisponibilidadeCidade,
                out lstParametrosCurriculo);

            var objCadastroCurriculo = new CadastroCurriculo();

            #region Carregando MiniCurriculo
            var flgAceitaEstagio = lstParametrosCurriculo.FirstOrDefault(p => p.Parametro.IdParametro == (int)BNE.BLL.Enumeradores.Parametro.CurriculoAceitaEstagio);
            objCadastroCurriculo.MiniCurriculo = new CadastroMiniCurriculo()
            {
                AceitoEstagio = flgAceitaEstagio == null ? false : Convert.ToBoolean(flgAceitaEstagio.ValorParametro),
                Cidade = objPessoaFisica.Cidade?.ToString(),
                Cpf = objPessoaFisica.CPF,
                DataNascimento = objPessoaFisica.DataNascimento,
                DDDCelular = objPessoaFisica.NumeroDDDCelular == null ? (short)0 : Convert.ToInt16(reNotNumeric.Replace(objPessoaFisica.NumeroDDDCelular, string.Empty)),
                Email = objPessoaFisica.EmailPessoa,
                Escolaridade = objPessoaFisica.Escolaridade?.DescricaoBNE,
                Nome = objPessoaFisica.NomeCompleto,
                NumeroCelular = objPessoaFisica.NumeroCelular == null ? 0 : Convert.ToDecimal(reNotNumeric.Replace(objPessoaFisica.NumeroCelular, string.Empty)),
                PretensaoSalarial = objCurriculo.ValorPretensaoSalarial.HasValue ?
                                    objCurriculo.ValorPretensaoSalarial.Value : 0,
                Sexo = (DTO.Enum.Sexo)objPessoaFisica.Sexo.IdSexo
            };

            var lstFuncaoPretendidaDto = new List<DTO.CadastroCV.FuncaoPretendida>();
            foreach (var objFuncaoPretendida in lstFuncaoPretendida)
            {
                objFuncaoPretendida.Funcao?.CompleteObject();
                lstFuncaoPretendidaDto.Add(new DTO.CadastroCV.FuncaoPretendida()
                {
                    Funcao = string.IsNullOrEmpty(objFuncaoPretendida.DescricaoFuncaoPretendida) ?
                                objFuncaoPretendida.Funcao.DescricaoFuncao : objFuncaoPretendida.DescricaoFuncaoPretendida,
                    MesesDeExperiencia = objFuncaoPretendida.QuantidadeExperiencia.HasValue ?
                                            objFuncaoPretendida.QuantidadeExperiencia.Value : (short)0
                });
            }
            objCadastroCurriculo.MiniCurriculo.FuncoesPretendidas = lstFuncaoPretendidaDto.ToArray();
            #endregion Carregando MiniCurriculo

            #region CarregandoDadoa Complementares
            objCadastroCurriculo.DadosComplementares = new DadosComplementares()
            {
                Altura = objPessoaFisicaComplemento.NumeroAltura,
                CategoriaCNH = (DTO.Enum.CategoriaCNH?)objPessoaFisicaComplemento.CategoriaHabilitacao?.IdCategoriaHabilitacao,
                ComplementoDeficiencia = objPessoaFisicaComplemento.DescricaoComplementoDeficiencia,
                Deficiencia = objPessoaFisica.Deficiencia?.DescricaoDeficiencia,
                DisponibilidadeOutrasCidades = lstCurriculoDisponibilidadeCidade.Select(c => c.Cidade.ToString()).ToArray(),
                Disponibilidades = lstCurriculoDisponibilidade
                .Select(cd => (DTO.Enum.Disponibilidade)cd.Disponibilidade.IdDisponibilidade).ToArray(),
                NumeroCnh = string.IsNullOrEmpty(objPessoaFisicaComplemento.NumeroHabilitacao) ? null :
                            (decimal?)Convert.ToDecimal(reNotNumeric.Replace(objPessoaFisicaComplemento.NumeroHabilitacao, string.Empty)),
                Observacoes = objCurriculo.ObservacaoCurriculo,
                OutrosConhecimentos = objPessoaFisicaComplemento.DescricaoConhecimento,
                Peso = objPessoaFisicaComplemento.NumeroPeso,
                PossuiFilhos = objPessoaFisicaComplemento.FlagFilhos,
                Raca = (DTO.Enum.Raca?)objPessoaFisica.Raca?.IdRaca,
            };

            var lstVeiculos = new List<VeiculoCurriculo>();
            foreach (var objVeiculo in lstPessoaFisicaVeiculo)
            {
                lstVeiculos.Add(new VeiculoCurriculo()
                {
                    Ano = objVeiculo.AnoVeiculo,
                    DescricaoModelo = objVeiculo.DescricaoModelo,
                    TipoVeiculo = objVeiculo.TipoVeiculo.DescricaoTipoVeiculo
                });
            }
            objCadastroCurriculo.DadosComplementares.Veiculos = lstVeiculos.ToArray();

            #endregion CarregandoDadoa Complementares

            #region Dados Pessoais
            objCadastroCurriculo.DadosPessoais = new DadosPessoais()
            {
                DDDCelularRecado = string.IsNullOrWhiteSpace(objContatoCelular?.NumeroDDDCelular) ?
                                    (short)0 : Convert.ToInt16(reNotNumeric.Replace(objContatoCelular.NumeroDDDCelular, string.Empty)),
                DDDTelefoneFixo = string.IsNullOrWhiteSpace(objPessoaFisica.NumeroDDDTelefone) ?
                                    (short)0 : Convert.ToInt16(reNotNumeric.Replace(objPessoaFisica.NumeroDDDTelefone, string.Empty)),
                DDDTelefoneFixoRecado = string.IsNullOrWhiteSpace(objContatoTelefone?.NumeroDDDTelefone) ?
                                    (short)0 : Convert.ToInt16(reNotNumeric.Replace(objContatoTelefone.NumeroDDDTelefone, string.Empty)),
                Endereco = new EnderecoCurriculo()
                {
                    Bairro = objPessoaFisica.Endereco.DescricaoBairro,
                    Cep = string.IsNullOrWhiteSpace(objPessoaFisica.Endereco.NumeroCEP) ? 
                            0 : Convert.ToDecimal(reNotNumeric.Replace(objPessoaFisica.Endereco.NumeroCEP, string.Empty)),
                    Cidade = objPessoaFisica.Endereco.Cidade.ToString(),
                    Complemento = objPessoaFisica.Endereco.DescricaoComplemento,
                    Logradouro = objPessoaFisica.Endereco.DescricaoLogradouro,
                    Numero = objPessoaFisica.Endereco.NumeroEndereco
                },
                EstadoCivil = objPessoaFisica.EstadoCivil?.DescricaoEstadoCivil,
                NomeContatoCelular = objContatoCelular?.NomeContato,
                NomeContatoTelefoneFixo = objContatoTelefone?.NomeContato,
                NumeroCelularRecado = string.IsNullOrWhiteSpace(objContatoCelular?.NumeroCelular) ?
                                    0 : Convert.ToDecimal(reNotNumeric.Replace(objContatoCelular.NumeroCelular, string.Empty)),
                NumeroRg = string.IsNullOrWhiteSpace(objPessoaFisica.NumeroRG) ?
                                    0 : Convert.ToDecimal(reNotNumeric.Replace(objPessoaFisica.NumeroRG, string.Empty)),
                NumeroTelefoneFixo = string.IsNullOrWhiteSpace(objPessoaFisica.NumeroTelefone) ?
                                    0 : Convert.ToDecimal(reNotNumeric.Replace(objPessoaFisica.NumeroTelefone, string.Empty)),
                NumeroTelefoneFixoRecado = string.IsNullOrWhiteSpace(objContatoTelefone?.NumeroTelefone) ?
                                    0 : Convert.ToDecimal(reNotNumeric.Replace(objContatoTelefone.NumeroTelefone, string.Empty)),
                OrgaoEmissorRg = objPessoaFisica.NomeOrgaoEmissor
            };
            #endregion Dados Pessoais

            #region Experiencias
            var lstExperienciasDto = new List<CadastroExperienciaProfissional>();
            foreach (var objExperiencia in lstExperienciaProfissional)
            {
                lstExperienciasDto.Add(new CadastroExperienciaProfissional()
                {
                    Area = objExperiencia.AreaBNE.DescricaoAreaBNE,
                    Atribuicoes = objExperiencia.DescricaoAtividade,
                    DataAdmissao = objExperiencia.DataAdmissao,
                    DataDemissao = objExperiencia.DataDemissao,
                    Funcao = string.IsNullOrEmpty(objExperiencia.DescricaoFuncaoExercida) ?
                                objExperiencia.Funcao.DescricaoFuncao : objExperiencia.DescricaoFuncaoExercida,
                    NomeEmpresa = objExperiencia.RazaoSocial,
                    Salario = objExperiencia.VlrSalario.HasValue ? objExperiencia.VlrSalario.Value : 0
                });
            }
            objCadastroCurriculo.Experiencias = lstExperienciasDto.ToArray();
            #endregion Experiencias

            objCadastroCurriculo.Formacao = new FormacaoCurriculo();
            #region Cursos Formacao
            var lstCursosFormacaoDto = new List<DTO.CadastroCV.Curso>();
            foreach (var objFormacao in lstCursosFormacao)
            {
                objFormacao.Cidade?.CompleteObject();
                objFormacao.Cidade?.Estado.CompleteObject();
                objFormacao.Fonte?.CompleteObject();
                objFormacao.Escolaridade?.CompleteObject();
                objFormacao.Curso?.CompleteObject();

                lstCursosFormacaoDto.Add(new DTO.CadastroCV.Curso()
                {
                    AnoDeConclusao = objFormacao.AnoConclusao,
                    Cidade = objFormacao.Cidade?.ToString(),
                    Instituicao = string.IsNullOrEmpty(objFormacao.DescricaoFonte) ?
                                    objFormacao.Fonte?.NomeFonte : objFormacao.DescricaoFonte,
                    NivelFormacao = objFormacao.Escolaridade?.DescricaoBNE,
                    NomeCurso = string.IsNullOrEmpty(objFormacao.DescricaoCurso) ?
                                    objFormacao.Curso?.DescricaoCurso : objFormacao.DescricaoCurso,
                    Periodo = objFormacao.NumeroPeriodo,
                    Situacao = (DTO.Enum.SituacaoCurso?)objFormacao.SituacaoFormacao?.IdSituacaoFormacao
                });
            }
            objCadastroCurriculo.Formacao.CursosFormacao = lstCursosFormacaoDto.ToArray();
            #endregion Cursos Formacao

            #region Cursos Complementares
            var lstCursosComplementaresDto = new List<DTO.CadastroCV.CursoComplementar>();
            foreach (var objFormacao in lstCursosComplementares)
            {
                objFormacao.Cidade?.CompleteObject();
                objFormacao.Cidade?.Estado.CompleteObject();
                objFormacao.Fonte?.CompleteObject();
                objFormacao.Curso?.CompleteObject();

                lstCursosComplementaresDto.Add(new DTO.CadastroCV.CursoComplementar()
                {
                    AnoConclusao = objFormacao.AnoConclusao,
                    Cidade = objFormacao.Cidade?.ToString(),
                    Instituicao = string.IsNullOrEmpty(objFormacao.DescricaoFonte) ?
                                    objFormacao.Fonte?.NomeFonte : objFormacao.DescricaoFonte,
                    NomeCurso = string.IsNullOrEmpty(objFormacao.DescricaoCurso) ?
                                    objFormacao.Curso?.DescricaoCurso : objFormacao.DescricaoCurso,
                    CargaHoraria = objFormacao.QuantidadeCargaHoraria
                });
            }
            objCadastroCurriculo.Formacao.CursosComplementares = lstCursosComplementaresDto.ToArray();
            #endregion Cursos Complementares

            #region Idioma
            var lstIdiomaDto = new List<CadastroIdioma>();
            foreach (var objIdioma in lstPessoaFisicaIdioma)
            {
                objIdioma.Idioma?.CompleteObject();
                objIdioma.NivelIdioma?.CompleteObject();

                lstIdiomaDto.Add(new CadastroIdioma()
                {
                    DescricaoIdioma = objIdioma.Idioma?.DescricaoIdioma,
                    NivelIdioma = (DTO.Enum.NivelIdioma)objIdioma.NivelIdioma?.IdNivelIdioma
                });
            }
            objCadastroCurriculo.Formacao.Idiomas = lstIdiomaDto.ToArray();
            #endregion Idioma


            return objCadastroCurriculo;
        }

        /// <summary>
        /// Salva um novo currículo
        /// </summary>
        /// <param name="objPessoaFisica">Pessoa física do CV. 
        /// Em caso de novo currículo, informar null. </param>
        /// <param name="dtoCurriculo">Dados do currículo</param>
        /// <param name="idCurriculo">Id do curriculo salvo.</param>
        /// <param name="idOrigemBNE">Origem com a qual o currículo deve estar vinculado.</param>
        /// <param name="message">Mensagem de erro em caso de falha na inserção</param>
        /// <returns>True se salvou conrretamente</returns>
        public static bool Salvar(PessoaFisica objPessoaFisica,
            CadastroCurriculo dtoCurriculo,
            int? idOrigemBNE,
            out int? idCurriculo,
            out string message)
        {
            Curriculo objCurriculo;
            UsuarioFilialPerfil objUsuarioFilialPerfil;
            PessoaFisicaComplemento objPessoaFisicaComplemento;
            Contato objContatoCelular = null;
            Contato objContatoTelefone = null;
            List<ExperienciaProfissional> lstExperienciaProfissional = null;
            List<Formacao> lstCursosFormacao = null;
            List<Formacao> lstCursosComplementares = null;
            List<PessoafisicaIdioma> lstPessoaFisicaIdioma = null;
            List<CurriculoDisponibilidade> lstCurriculoDisponibilidade = null;
            List<PessoaFisicaVeiculo> lstPessoaFisicaVeiculo = null;
            List<CurriculoDisponibilidadeCidade> lstCurriculoDisponibilidadeCidade = null;
            List<ParametroCurriculo> lstParametrosCurriculo = new List<ParametroCurriculo>();

            CarregarDados(objPessoaFisica,
                dtoCurriculo,
                out objCurriculo,
                out objUsuarioFilialPerfil,
                out objPessoaFisicaComplemento);

            // Se é uma inserção de curriculo, o MiniCurriculo deve ser indicado
            if (objPessoaFisica == null && dtoCurriculo.MiniCurriculo == null)
            {
                message = "Para inserir um novo currículo, os dados do minicurriculo devem ser informados";
                idCurriculo = null;
                return false;
            }

            // Criando pessoa física para novo cv
            if (objPessoaFisica == null)
            {
                objPessoaFisica = new PessoaFisica();
                objPessoaFisica.Endereco = new Endereco();
                objPessoaFisicaComplemento = new PessoaFisicaComplemento();
                objUsuarioFilialPerfil = new UsuarioFilialPerfil
                {
                    Perfil = new Perfil((int)BNE.BLL.Enumeradores.Perfil.AcessoNaoVIP)
                };

                objPessoaFisica.NumeroCPF = dtoCurriculo.MiniCurriculo.Cpf.ToString("0");
                objPessoaFisica.DataNascimento = dtoCurriculo.MiniCurriculo.DataNascimento;
            }

            if (dtoCurriculo.MiniCurriculo != null &&
                !MapearMiniCurriculo(
                    dtoCurriculo.MiniCurriculo,
                    idOrigemBNE,
                    objPessoaFisica,
                    objCurriculo,
                    objUsuarioFilialPerfil,
                    objPessoaFisicaComplemento,
                    lstParametrosCurriculo,
                    out message))
            {
                idCurriculo = null;
                return false;
            }

            if (dtoCurriculo.DadosPessoais != null &&
                !MapearDadosPessoais(
                    dtoCurriculo.DadosPessoais,
                    objPessoaFisica,
                    objCurriculo,
                    out objContatoCelular,
                    out objContatoTelefone,
                    out message))
            {
                idCurriculo = null;
                return false;
            }
            if (dtoCurriculo.Experiencias != null &&
                dtoCurriculo.Experiencias.Length > 0 &&
                !MapearExperiencias(
                    dtoCurriculo.Experiencias,
                    out lstExperienciaProfissional,
                    out message))
            {
                idCurriculo = null;
                return false;
            }

            var apagarCursosFormacao = false;
            var apagarCursosComplementares = false;
            var apagarIdiomas = false;
            if (dtoCurriculo.Formacao != null)
            {
                if (dtoCurriculo.Formacao.CursosFormacao != null &&
                    dtoCurriculo.Formacao.CursosFormacao.Length > 0)
                {
                    if (!MapearFormacoes(
                        dtoCurriculo.Formacao.CursosFormacao,
                        objPessoaFisica,
                        objCurriculo,
                        out lstCursosFormacao,
                        out message))
                    {
                        idCurriculo = null;
                        return false;
                    }

                    apagarCursosFormacao = true;
                }

                if (dtoCurriculo.Formacao.CursosComplementares != null &&
                    dtoCurriculo.Formacao.CursosComplementares.Length > 0)
                {
                    MapearCursosComplementares(
                        dtoCurriculo.Formacao.CursosComplementares,
                        objPessoaFisica,
                        out lstCursosComplementares);
                    apagarCursosComplementares = true;
                }

                if (dtoCurriculo.Formacao.Idiomas != null &&
                    dtoCurriculo.Formacao.Idiomas.Length > 0)
                {
                    if (!MapearIdiomas(
                        dtoCurriculo.Formacao.Idiomas,
                        objPessoaFisica,
                        out lstPessoaFisicaIdioma,
                        out message))
                    {
                        idCurriculo = null;
                        return false;
                    }
                    apagarIdiomas = true;
                }
            }

            if (dtoCurriculo.DadosComplementares != null)
            {
                if (!MapearDadosComplementares(
                    dtoCurriculo.DadosComplementares,
                    objPessoaFisica,
                    objCurriculo,
                    objPessoaFisicaComplemento,
                    out lstCurriculoDisponibilidade,
                    out lstPessoaFisicaVeiculo,
                    out lstCurriculoDisponibilidadeCidade,
                    out message))
                {
                    idCurriculo = null;
                    return false;
                }
            }

            Origem objOrigem = null;
            if (idOrigemBNE.HasValue)
                objOrigem = new Origem(idOrigemBNE.Value);

            //Salvar
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    bool novoCadastro;
                    try
                    {
                        objCurriculo.SalvarMiniCurriculo(
                            trans,
                            out novoCadastro,
                            objPessoaFisica,
                            MapearFuncoesPretendidas(dtoCurriculo.MiniCurriculo.FuncoesPretendidas),
                            objOrigem,
                            null,
                            null,
                            objUsuarioFilialPerfil,
                            objPessoaFisicaComplemento,
                            null,
                            null,
                            null,
                            objOrigemSecundaria: new Origem((int)BNE.BLL.Enumeradores.Origem.BNE));

                        ExperienciaProfissional.ExcluirExperienciasDePessoa(
                            objPessoaFisica.IdPessoaFisica,
                            trans);
                        objCurriculo.SalvarDadosPessoais(
                            trans,
                            objPessoaFisica,
                            objPessoaFisicaComplemento,
                            objContatoTelefone,
                            objContatoCelular,
                            null,
                            lstExperienciaProfissional?.ElementAtOrDefault(0),
                            lstExperienciaProfissional?.ElementAtOrDefault(1),
                            lstExperienciaProfissional?.ElementAtOrDefault(2),
                            lstExperienciaProfissional?.ElementAtOrDefault(3),
                            lstExperienciaProfissional?.ElementAtOrDefault(4),
                            lstExperienciaProfissional?.ElementAtOrDefault(5),
                            lstExperienciaProfissional?.ElementAtOrDefault(6),
                            lstExperienciaProfissional?.ElementAtOrDefault(7),
                            lstExperienciaProfissional?.ElementAtOrDefault(8),
                            lstExperienciaProfissional?.ElementAtOrDefault(9));

                        foreach (var paramCurriculo in lstParametrosCurriculo)
                            paramCurriculo.Save(trans);

                        if (apagarCursosFormacao)
                            Formacao.DeleteFormacoes(objPessoaFisica.IdPessoaFisica, trans);

                        if (lstCursosFormacao != null)
                            foreach (var formacao in lstCursosFormacao)
                                formacao.Save(trans);

                        if (apagarCursosComplementares)
                            Formacao.DeleteOutrosCursos(objPessoaFisica.IdPessoaFisica, trans);

                        if (lstCursosComplementares != null)
                            foreach (var formacao in lstCursosComplementares)
                                formacao.Save(trans);

                        if (apagarIdiomas)
                            PessoafisicaIdioma.DeletePorPessoaFisica(objPessoaFisica.IdPessoaFisica, trans);

                        if (lstPessoaFisicaIdioma != null)
                            foreach (var idioma in lstPessoaFisicaIdioma)
                                idioma.Save(trans);

                        if (lstCurriculoDisponibilidade != null)
                            CurriculoDisponibilidade.AtualizaCurriculo(objCurriculo.IdCurriculo, lstCurriculoDisponibilidade, trans);

                        if (lstPessoaFisicaVeiculo != null)
                        {
                            PessoaFisicaVeiculo.DeletarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, trans);
                            foreach (var veiculo in lstPessoaFisicaVeiculo)
                                veiculo.Save(trans);
                        }

                        if (lstCurriculoDisponibilidadeCidade != null)
                            CurriculoDisponibilidadeCidade.AtualizaListaCurriculo(objCurriculo.IdCurriculo,
                                lstCurriculoDisponibilidadeCidade, trans);

                        objPessoaFisica.Save(trans);

                        trans.Commit();
                        //Task 45613
                        if(novoCadastro)
                            objCurriculo.AdicionarAtividadeAssincronoSalvarAlertasCurriculo(objPessoaFisica.EmailPessoa, objCurriculo.IdCurriculo);
                       
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            message = string.Empty;
            idCurriculo = objCurriculo.IdCurriculo;
            return true;
        }

        #region Carregar Dados

        /// <summary>
        /// Centraliza o carregamento e criação dos dados para evitar multiplos selects no banco de dados
        /// </summary>
        /// <param name="lstFuncaoPretendida"></param>
        /// <param name="objPessoaFisica"></param>
        /// <param name="objCurriculo"></param>
        /// <param name="objUsuarioFilialPerfil"></param>
        /// <param name="objPessoaFisicaComplemento"></param>
        /// <param name="objContatoCelular"></param>
        /// <param name="objContatoTelefone"></param>
        /// <param name="lstExperienciaProfissional"></param>
        /// <param name="lstCursosFormacao"></param>
        /// <param name="lstCursosComplementares"></param>
        /// <param name="lstPessoaFisicaIdioma"></param>
        /// <param name="lstCurriculoDisponibilidade"></param>
        /// <param name="lstPessoaFisicaVeiculo"></param>
        /// <param name="lstCurriculoDisponibilidadeCidade"></param>
        /// <param name="lstParametrosCurriculo"></param>
        public static void CarregarDados(PessoaFisica objPessoaFisica,
            out Curriculo objCurriculo,
            out UsuarioFilialPerfil objUsuarioFilialPerfil,
            out PessoaFisicaComplemento objPessoaFisicaComplemento,
            out Contato objContatoCelular,
            out Contato objContatoTelefone,
            out List<BNE.BLL.FuncaoPretendida> lstFuncaoPretendida,
            out List<ExperienciaProfissional> lstExperienciaProfissional,
            out List<Formacao> lstCursosFormacao,
            out List<Formacao> lstCursosComplementares,
            out List<PessoafisicaIdioma> lstPessoaFisicaIdioma,
            out List<CurriculoDisponibilidade> lstCurriculoDisponibilidade,
            out List<PessoaFisicaVeiculo> lstPessoaFisicaVeiculo,
            out List<CurriculoDisponibilidadeCidade> lstCurriculoDisponibilidadeCidade,
            out List<ParametroCurriculo> lstParametrosCurriculo)
        {
            if (objPessoaFisica == null)
            {
                throw new ArgumentException("Não foi possível localizar o currículo");
            }

            #region Carregando objetos para o salvamento do CV
            // Carregando informações para currículo já existente
            if (!Curriculo.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objCurriculo))
            {
                throw new ArgumentException("Não foi possível localizar o currículo para a pessoa");
            }
            objPessoaFisica.CompleteObject();
            objPessoaFisica.Cidade?.CompleteObject();
            objPessoaFisica.Escolaridade?.CompleteObject();
            objPessoaFisica.EstadoCivil?.CompleteObject();
            objPessoaFisica.Deficiencia?.CompleteObject();

            var objEnderecoPF = new Endereco();
            Endereco.CarregarPorPessoaFisica(objPessoaFisica, out objEnderecoPF);
            objPessoaFisica.Endereco = objEnderecoPF;

            if (!PessoaFisicaComplemento.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica,
                    out objPessoaFisicaComplemento))
                objPessoaFisicaComplemento = new PessoaFisicaComplemento();

            if (!UsuarioFilialPerfil.CarregarUsuarioFilialPerfilCandidatoAtivoEInativo(objPessoaFisica,
                    out objUsuarioFilialPerfil))
            {
                objUsuarioFilialPerfil = new UsuarioFilialPerfil
                {
                    Perfil = new Perfil((int)BNE.BLL.Enumeradores.Perfil.AcessoNaoVIP)
                };
            }

            Contato.CarregarPorPessoaFisicaTipoContato(objPessoaFisica.IdPessoaFisica,
                (int)BNE.BLL.Enumeradores.TipoContato.RecadoCelular, out objContatoCelular, null);
            Contato.CarregarPorPessoaFisicaTipoContato(objPessoaFisica.IdPessoaFisica,
                (int)BNE.BLL.Enumeradores.TipoContato.RecadoFixo, out objContatoTelefone, null);

            lstFuncaoPretendida = BNE.BLL.FuncaoPretendida.CarregarFuncoesPretendidasPorCurriculo(objCurriculo);
            lstExperienciaProfissional = ExperienciaProfissional.CarregarExperienciaPorPessoaFisica(objPessoaFisica.IdPessoaFisica);
            lstCursosFormacao = Formacao.ListarFormacaoList(objPessoaFisica.IdPessoaFisica, false);
            lstCursosComplementares = Formacao.ListarFormacaoList(objPessoaFisica.IdPessoaFisica, true);
            lstCurriculoDisponibilidade = CurriculoDisponibilidade.Listar(objCurriculo.IdCurriculo);
            lstPessoaFisicaVeiculo = PessoaFisicaVeiculo.ListarPessoaFisicaVeiculo(objPessoaFisica.IdPessoaFisica);
            lstCurriculoDisponibilidadeCidade = CurriculoDisponibilidadeCidade.Listar(objCurriculo.IdCurriculo);
            lstParametrosCurriculo = ParametroCurriculo.CarregarParametroPorCurriculo(objCurriculo.IdCurriculo, null);
            lstPessoaFisicaIdioma = PessoafisicaIdioma.Listar(objPessoaFisica.IdPessoaFisica);
            #endregion Carregando objetos para o salvamento do CV
        }

        /// <summary>
        /// Centraliza o carregamento e criação dos dados para evitar multiplos selects no banco de dados
        /// </summary>
        /// <param name="objPessoaFisica"></param>
        /// <param name="dtoCurriculo"></param>
        /// <param name="objCurriculo"></param>
        /// <param name="objUsuarioFilialPerfil"></param>
        /// <param name="objPessoaFisicaComplemento"></param>
        public static void CarregarDados(PessoaFisica objPessoaFisica,
            CadastroCurriculo dtoCurriculo,
            out Curriculo objCurriculo,
            out UsuarioFilialPerfil objUsuarioFilialPerfil,
            out PessoaFisicaComplemento objPessoaFisicaComplemento)
        {
            #region Carregando objetos para o salvamento do CV
            if (objPessoaFisica != null)
            {
                // Carregando informações para currículo já existente
                if (!Curriculo.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objCurriculo))
                {
                    objCurriculo = new Curriculo();
                    objCurriculo.PessoaFisica = objPessoaFisica;
                }

                if (objPessoaFisica.Endereco != null)
                    objPessoaFisica.Endereco.CompleteObject();
                else
                    objPessoaFisica.Endereco = new Endereco();

                if (!PessoaFisicaComplemento.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica,
                        out objPessoaFisicaComplemento))
                    objPessoaFisicaComplemento = new PessoaFisicaComplemento();

                if (!UsuarioFilialPerfil.CarregarUsuarioFilialPerfilCandidatoAtivoEInativo(objPessoaFisica,
                        out objUsuarioFilialPerfil))
                {
                    objUsuarioFilialPerfil = new UsuarioFilialPerfil
                    {
                        Perfil = new Perfil((int)BNE.BLL.Enumeradores.Perfil.AcessoNaoVIP)
                    };
                }
            }
            else
            {
                // Criando objetos para novo curriculo
                objCurriculo = new Curriculo();
                objPessoaFisica = new PessoaFisica();
                objPessoaFisica.Endereco = new Endereco();
                objPessoaFisicaComplemento = new PessoaFisicaComplemento();
                objUsuarioFilialPerfil = new UsuarioFilialPerfil
                {
                    Perfil = new Perfil((int)BNE.BLL.Enumeradores.Perfil.AcessoNaoVIP)
                };

                objPessoaFisica.NumeroCPF = dtoCurriculo.MiniCurriculo.Cpf.ToString("0");
                objPessoaFisica.DataNascimento = dtoCurriculo.MiniCurriculo.DataNascimento;
            }
            #endregion Carregando objetos para o salvamento do CV
        }

        #endregion Carregar Dados

        #region Mapear Dados Pessoais

        /// <summary>
        /// Salvar Dados pessoais
        /// </summary>
        /// <param name="dtoDadosPessoais"></param>
        /// <param name="objPessoaFisica"></param>
        /// <param name="objCurriculo"></param>
        /// <param name="objContatoCelular"></param>
        /// <param name="objContatoTelefone"></param>
        /// <param name="mensagemErro"></param>
        /// <returns></returns>
        public static bool MapearDadosPessoais(
            DadosPessoais dtoDadosPessoais,
            PessoaFisica objPessoaFisica,
            Curriculo objCurriculo,
            out Contato objContatoCelular,
            out Contato objContatoTelefone,
            out string mensagemErro)
        {
            objPessoaFisica.NumeroRG = dtoDadosPessoais.NumeroRg.ToString("0");
            objPessoaFisica.NomeOrgaoEmissor = dtoDadosPessoais.OrgaoEmissorRg;
            objPessoaFisica.NumeroDDDTelefone = dtoDadosPessoais.DDDTelefoneFixo.ToString("0");
            objPessoaFisica.NumeroTelefone = dtoDadosPessoais.NumeroTelefoneFixo.ToString("0");

            var objEstadoCivil = EstadoCivil.LoadObject(dtoDadosPessoais.EstadoCivil);
            if (objEstadoCivil == null)
            {
                objContatoCelular = objContatoTelefone = null;
                mensagemErro = "Descrição do estado civil não é válida";
                return false;
            }
            objPessoaFisica.EstadoCivil = objEstadoCivil;

            #region Endereco
            //Se houve alteração do CEP
            if ((!string.IsNullOrEmpty(objPessoaFisica.Endereco.NumeroCEP) &&
                !objPessoaFisica.Endereco.NumeroCEP.Equals(dtoDadosPessoais.Endereco.Cep)))
                objCurriculo.DescricaoLocalizacao = null;

            //Endereco
            objPessoaFisica.Endereco.NumeroCEP = dtoDadosPessoais.Endereco.Cep.ToString("0");
            objPessoaFisica.Endereco.DescricaoLogradouro = dtoDadosPessoais.Endereco.Logradouro;
            objPessoaFisica.Endereco.NumeroEndereco = dtoDadosPessoais.Endereco.Numero;
            objPessoaFisica.Endereco.DescricaoComplemento = dtoDadosPessoais.Endereco.Complemento;
            objPessoaFisica.Endereco.DescricaoBairro = dtoDadosPessoais.Endereco.Bairro;

            Cidade objCidade;
            if (Cidade.CarregarPorNome(dtoDadosPessoais.Endereco.Cidade, out objCidade))
                objPessoaFisica.Endereco.Cidade = objCidade;

            objPessoaFisica.Cidade = objCidade;
            #endregion Endereco

            #region Telefone Fixo para recado
            if (Contato.CarregarPorPessoaFisicaTipoContato(objPessoaFisica.IdPessoaFisica,
                (int)BNE.BLL.Enumeradores.TipoContato.RecadoFixo, out objContatoTelefone, null))
            {
                if (dtoDadosPessoais.NumeroTelefoneFixo > 0
                    && dtoDadosPessoais.DDDTelefoneFixo > 0)
                {
                    objContatoTelefone.NumeroDDDTelefone = dtoDadosPessoais.DDDTelefoneFixoRecado.ToString("0");
                    objContatoTelefone.NumeroTelefone = dtoDadosPessoais.NumeroTelefoneFixoRecado.ToString("0");
                    objContatoTelefone.NomeContato = dtoDadosPessoais.NomeContatoTelefoneFixo;
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
                    NumeroDDDTelefone = dtoDadosPessoais.DDDTelefoneFixo.ToString("0"),
                    NumeroTelefone = dtoDadosPessoais.NumeroTelefoneFixoRecado.ToString("0"),
                    NomeContato = dtoDadosPessoais.NomeContatoTelefoneFixo,
                    TipoContato_ = new TipoContato((int)BNE.BLL.Enumeradores.TipoContato.RecadoFixo)
                };
            }
            #endregion Telefone Fixo para recado

            #region Celular para recado
            if (Contato.CarregarPorPessoaFisicaTipoContato(objPessoaFisica.IdPessoaFisica,
                (int)BNE.BLL.Enumeradores.TipoContato.RecadoCelular, out objContatoCelular, null))
            {
                if (dtoDadosPessoais.DDDCelularRecado > 0 &&
                    dtoDadosPessoais.NumeroCelularRecado > 0)
                {
                    objContatoCelular.NumeroDDDCelular = dtoDadosPessoais.DDDCelularRecado.ToString("0");
                    objContatoCelular.NumeroCelular = dtoDadosPessoais.NumeroCelularRecado.ToString("0");
                    objContatoCelular.NomeContato = dtoDadosPessoais.NomeContatoCelular;
                    objContatoCelular.TipoContato_ = new TipoContato((int)BNE.BLL.Enumeradores.TipoContato.RecadoCelular);
                }
                else
                {
                    objContatoCelular.NumeroDDDCelular = string.Empty;
                    objContatoCelular.NumeroCelular = string.Empty;
                    objContatoCelular.NomeContato = string.Empty;
                    objContatoCelular.TipoContato_ = new TipoContato((int)BNE.BLL.Enumeradores.TipoContato.RecadoCelular);
                }
            }
            else
            {
                objContatoCelular = new Contato
                {
                    NumeroDDDCelular = dtoDadosPessoais.DDDCelularRecado.ToString("0"),
                    NumeroCelular = dtoDadosPessoais.NumeroCelularRecado.ToString("0"),
                    NomeContato = dtoDadosPessoais.NomeContatoCelular,
                    TipoContato_ = new TipoContato((int)BNE.BLL.Enumeradores.TipoContato.RecadoCelular)
                };
            }
            #endregion Celular para recado

            mensagemErro = string.Empty;
            return true;
        }
        #endregion Mapear Dados Pessoais

        #region Mapear Experiencias
        /// <summary>
        /// Realiza o mapeamento da lista de dtos de experiências para uma lista de experiencias da BLL.
        /// </summary>
        /// <param name="lstDtoExperiencias"></param>
        /// <param name="lstExperienciasBll"></param>
        /// <param name="mensagemErro"></param>
        /// <returns></returns>
        public static bool MapearExperiencias(
            CadastroExperienciaProfissional[] lstDtoExperiencias,
            out List<ExperienciaProfissional> lstExperienciasBll,
            out string mensagemErro)
        {
            lstExperienciasBll = new List<ExperienciaProfissional>();

            foreach (var dtoExperiencia in lstDtoExperiencias)
            {
                if (string.IsNullOrWhiteSpace(dtoExperiencia.NomeEmpresa) ||
                   string.IsNullOrWhiteSpace(dtoExperiencia.Funcao) ||
                   string.IsNullOrWhiteSpace(dtoExperiencia.Atribuicoes))
                {
                    mensagemErro = "O nome da empresa, a funcao e as atribuições devem ser indicadas nas experiências";
                    return false;
                }

                var objExperienciaProfissional = new ExperienciaProfissional();

                objExperienciaProfissional.RazaoSocial = dtoExperiencia.NomeEmpresa;
                objExperienciaProfissional.DataAdmissao = dtoExperiencia.DataAdmissao;
                objExperienciaProfissional.DataDemissao = dtoExperiencia.DataDemissao;
                objExperienciaProfissional.DescricaoAtividade = dtoExperiencia.Atribuicoes;
                objExperienciaProfissional.VlrSalario = dtoExperiencia.Salario;

                AreaBNE areaBne;
                if (!AreaBNE.CarregarPorDescricao(dtoExperiencia.Area, out areaBne))
                {
                    mensagemErro = "Descrição de área inválida";
                    return false;
                }
                objExperienciaProfissional.AreaBNE = areaBne;

                Funcao objFuncao;
                if (Funcao.CarregarPorDescricao(dtoExperiencia.Funcao, out objFuncao))
                {
                    objExperienciaProfissional.Funcao = objFuncao;
                    objExperienciaProfissional.DescricaoFuncaoExercida = string.Empty;
                }
                else
                {
                    objExperienciaProfissional.Funcao = null;
                    objExperienciaProfissional.DescricaoFuncaoExercida = dtoExperiencia.Funcao;
                }

                lstExperienciasBll.Add(objExperienciaProfissional);
            }

            mensagemErro = string.Empty;
            return true;
        }
        #endregion Mapear Experiencias

        #region Mapear Formacoes
        /// <summary>
        /// Realiza o mapeamento de uma lista de dtos de formacoes para uma lista de Formacoes da BLL
        /// </summary>
        /// <param name="dtoCursos"></param>
        /// <param name="objPessoaFisica"></param>
        /// <param name="objCurriculo"></param>
        /// <param name="lstFormacaoBll"></param>
        /// <param name="mensagemErro"></param>
        /// <returns></returns>
        public static bool MapearFormacoes(
            DTO.CadastroCV.Curso[] dtoCursos,
            PessoaFisica objPessoaFisica,
            Curriculo objCurriculo,
            out List<Formacao> lstFormacaoBll,
            out string mensagemErro)
        {
            lstFormacaoBll = new List<Formacao>();

            foreach (var curso in dtoCursos)
            {
                var objFormacao = new Formacao();

                objFormacao.PessoaFisica = objPessoaFisica;

                Escolaridade objEscolaridade;
                if (!Escolaridade.CarregarPorNome(curso.NivelFormacao, out objEscolaridade))
                {
                    mensagemErro = $"Descrição de nivel formação inválida para o curso {curso.NivelFormacao}";
                    return false;
                }
                objFormacao.Escolaridade = objEscolaridade;

                #region instituicao
                if (!string.IsNullOrEmpty(curso.Instituicao))
                {
                    string[] instituicaoSigla = curso.Instituicao.Split('-');
                    string nomeInstituicao;
                    if (instituicaoSigla.Length.Equals(1))
                        nomeInstituicao = instituicaoSigla[0].Trim();
                    else
                        nomeInstituicao = instituicaoSigla[1].Trim();

                    if (!string.IsNullOrEmpty(nomeInstituicao))
                    {
                        Fonte objFonte;
                        if (Fonte.CarregarPorNome(nomeInstituicao, out objFonte))
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
                }else
                    objFormacao.Fonte = null;
                #endregion instituicao

                #region Curso
                //Quando não existe curso na formação define-a como null
                int[] semCurso = { 4, 5, 6, 7 };
                if (semCurso.Contains(objEscolaridade.IdEscolaridade))
                {
                    objFormacao.Curso = null;
                    objFormacao.DescricaoCurso = String.Empty;
                }
                else
                {
                    if (!String.IsNullOrEmpty(curso.NomeCurso))
                    {
                        BNE.BLL.Curso objCurso;
                        if (BNE.BLL.Curso.CarregarPorNome(curso.NomeCurso, out objCurso))
                        {
                            objFormacao.Curso = objCurso;
                            objFormacao.DescricaoCurso = String.Empty;
                        }
                        else
                        {
                            objFormacao.Curso = null;
                            objFormacao.DescricaoCurso = curso.NomeCurso.Trim();
                        }
                    }
                    else
                        objFormacao.Curso = null;
                }
                #endregion Curso

                int[] incompletas = { 4, 6, 8, 10, 11 };

                #region Formacoes Incompletas
                //Lista de formações incompletas.

                //Caso a formação seja incompleta o campo de ano de conclusão deve receber o valor NULO
                //caso contrário busca o valor informado no campo.
                if (incompletas.Contains(objEscolaridade.IdEscolaridade) ||
                    curso.AnoDeConclusao <= 0)
                    objFormacao.AnoConclusao = null;
                else
                    objFormacao.AnoConclusao = curso.AnoDeConclusao;
                #endregion Formacoes Incompletas

                #region Formacoes Completas
                //Se a formação é completa a situação e período deve receber valores nulos.
                int[] especializacoes = { 14, 15, 16, 17 };
                if (!incompletas.Contains(objEscolaridade.IdEscolaridade))
                {
                    objFormacao.SituacaoFormacao = null;
                    objFormacao.NumeroPeriodo = null;

                    //mudar parametro currículo Flag_Estagio para false já que a escolaridade não é completa
                    ParametroCurriculo objAceitaEstagio;
                    if (ParametroCurriculo.CarregarParametroPorCurriculo(
                            BNE.BLL.Enumeradores.Parametro.CurriculoAceitaEstagio,
                            objCurriculo, out objAceitaEstagio, null))
                    {
                        if (objAceitaEstagio.ValorParametro.ToLower() == "true")
                        {
                            objAceitaEstagio.ValorParametro = "false";
                            objAceitaEstagio.Save();
                        }
                    }
                }
                else
                {
                    if (curso.Situacao.HasValue)
                        objFormacao.SituacaoFormacao = new SituacaoFormacao(Convert.ToInt16(curso.Situacao.Value));
                    else
                        objFormacao.SituacaoFormacao = null;

                    if (curso.Periodo.HasValue)
                        objFormacao.NumeroPeriodo = curso.Periodo;
                    else
                        objFormacao.NumeroPeriodo = null;
                }
                #endregion Formacoes Completas

                #region Cidade
                Cidade objCidade;
                if (Cidade.CarregarPorNome(curso.Cidade, out objCidade))
                    objFormacao.Cidade = objCidade;
                else
                    objFormacao.Cidade = null;
                #endregion Cidade

                lstFormacaoBll.Add(objFormacao);
            }

            mensagemErro = string.Empty;
            return true;
        }
        #endregion Mapear Formacoes

        #region Mapear Cursos Complementares
        /// <summary>
        /// Realiza o mapeamento de uma lista de dtos de Cursos Complementares para uma lista de Formacoes da BLL
        /// </summary>
        /// <param name="dtoCursos"></param>
        /// <param name="objPessoaFisica"></param>
        /// <param name="lstFormacaoBll"></param>
        /// <returns></returns>
        public static bool MapearCursosComplementares(
            DTO.CadastroCV.CursoComplementar[] dtoCursos,
            PessoaFisica objPessoaFisica,
            out List<Formacao> lstFormacaoBll)
        {
            lstFormacaoBll = new List<Formacao>();

            foreach (var dtoCurso in dtoCursos)
            {
                var objFormacao = new Formacao();

                objFormacao.PessoaFisica = objPessoaFisica;
                objFormacao.Escolaridade = new Escolaridade((int)BNE.BLL.Enumeradores.Escolaridade.OutrosCursos);

                #region Instituicao
                string[] instituicaoSigla = dtoCurso.Instituicao.Split('-');
                string nomeInstituicao;
                if (instituicaoSigla.Length.Equals(1))
                    nomeInstituicao = instituicaoSigla[0].Trim();
                else
                    nomeInstituicao = instituicaoSigla[1].Trim();

                Fonte objFonte;
                if (Fonte.CarregarPorNome(nomeInstituicao, out objFonte))
                {
                    objFormacao.Fonte = objFonte;
                    objFormacao.DescricaoFonte = String.Empty;
                }
                else
                {
                    objFormacao.Fonte = null;
                    objFormacao.DescricaoFonte = nomeInstituicao;
                }
                #endregion Instituicao

                BNE.BLL.Curso objCurso;
                if (BNE.BLL.Curso.CarregarPorNome(dtoCurso.NomeCurso, out objCurso))
                    objFormacao.Curso = objCurso;
                else
                    objFormacao.Curso = null;
                objFormacao.DescricaoCurso = objFormacao.Curso == null ? dtoCurso.NomeCurso : null;

                Cidade objCidade;
                if (Cidade.CarregarPorNome(dtoCurso.Cidade, out objCidade))
                    objFormacao.Cidade = objCidade;
                else
                    objFormacao.Cidade = null;

                objFormacao.QuantidadeCargaHoraria = (dtoCurso.CargaHoraria.HasValue &&
                                                        dtoCurso.CargaHoraria.Value > 0) ?
                                                        dtoCurso.CargaHoraria : null;

                objFormacao.AnoConclusao = (dtoCurso.AnoConclusao.HasValue &&
                                                dtoCurso.AnoConclusao.Value > 0) ?
                                                dtoCurso.AnoConclusao : null;
                objFormacao.FlagInativo = false;

                lstFormacaoBll.Add(objFormacao);
            }

            return true;
        }
        #endregion Mapear Cursos Complementares

        #region Mapear Idiomas
        /// <summary>
        /// Realiza o mapeamento de uma lista de dtos de Idiomas para uma lista de PessoaFisicaIdioma da BLL
        /// </summary>
        /// <param name="dtoIdiomas"></param>
        /// <param name="objPessoaFisica"></param>
        /// <param name="lstPessoaFisicaIdiomaBll"></param>
        /// <param name="mensagemErro"></param>
        /// <returns></returns>
        public static bool MapearIdiomas(
            DTO.CadastroCV.CadastroIdioma[] dtoIdiomas,
            PessoaFisica objPessoaFisica,
            out List<PessoafisicaIdioma> lstPessoaFisicaIdiomaBll,
            out string mensagemErro)
        {
            lstPessoaFisicaIdiomaBll = new List<PessoafisicaIdioma>();

            foreach (var dtoIdioma in dtoIdiomas)
            {
                PessoafisicaIdioma objPessoaFisicaIdioma = new PessoafisicaIdioma();
                objPessoaFisicaIdioma.PessoaFisica = objPessoaFisica;

                Idioma objIdioma;
                if (!Idioma.CarregarPorNome(dtoIdioma.DescricaoIdioma, out objIdioma))
                {
                    mensagemErro = $"Descrição de idioma inválida: {dtoIdioma.DescricaoIdioma}";
                    return false;
                }
                objPessoaFisicaIdioma.Idioma = objIdioma;

                objPessoaFisicaIdioma.NivelIdioma = new NivelIdioma(Convert.ToInt32(dtoIdioma.NivelIdioma));

                objPessoaFisicaIdioma.FlagInativo = false;

                lstPessoaFisicaIdiomaBll.Add(objPessoaFisicaIdioma);
            }

            mensagemErro = string.Empty;
            return true;
        }
        #endregion Mapear Idiomas

        #region Mapear Dados Complementares
        /// <summary>
        /// Realiza o mapeamento dos dados complementares para objetos da BLL
        /// </summary>
        /// <param name="dtoDadosComplementares"></param>
        /// <param name="objPessoaFisica"></param>
        /// <param name="objCurriculo"></param>
        /// <param name="objPessoaFisicaComplemento"></param>
        /// <param name="lstCurriculoDisponibilidadeBll"></param>
        /// <param name="lstPessoaFisicaVeiculoBll"></param>
        /// <param name="lstCurriculoDisponibilidadeCidadeBll"></param>
        /// <param name="mensagemErro"></param>
        /// <returns></returns>
        public static bool MapearDadosComplementares(
            DTO.CadastroCV.DadosComplementares dtoDadosComplementares,
            PessoaFisica objPessoaFisica,
            Curriculo objCurriculo,
            PessoaFisicaComplemento objPessoaFisicaComplemento,
            out List<CurriculoDisponibilidade> lstCurriculoDisponibilidadeBll,
            out List<PessoaFisicaVeiculo> lstPessoaFisicaVeiculoBll,
            out List<CurriculoDisponibilidadeCidade> lstCurriculoDisponibilidadeCidadeBll,
            out string mensagemErro)
        {
            lstCurriculoDisponibilidadeBll = null;
            lstPessoaFisicaVeiculoBll = null;
            lstCurriculoDisponibilidadeCidadeBll = null;

            objPessoaFisicaComplemento.FlagFilhos = dtoDadosComplementares.PossuiFilhos;

            if (dtoDadosComplementares.CategoriaCNH.HasValue)
                objPessoaFisicaComplemento.CategoriaHabilitacao = new CategoriaHabilitacao(Convert.ToInt32(dtoDadosComplementares.CategoriaCNH.Value));

            if (dtoDadosComplementares.NumeroCnh.HasValue)
                objPessoaFisicaComplemento.NumeroHabilitacao = dtoDadosComplementares.NumeroCnh.Value.ToString("0");
            if (dtoDadosComplementares.Altura.HasValue)
                objPessoaFisicaComplemento.NumeroAltura = dtoDadosComplementares.Altura.Value;
            if (dtoDadosComplementares.Peso.HasValue)
                objPessoaFisicaComplemento.NumeroPeso = dtoDadosComplementares.Peso.Value;

            if (dtoDadosComplementares.Disponibilidades != null)
            {
                lstCurriculoDisponibilidadeBll = new List<CurriculoDisponibilidade>();
                foreach (var disponibilidade in dtoDadosComplementares.Disponibilidades)
                {
                    lstCurriculoDisponibilidadeBll.Add(new CurriculoDisponibilidade
                    {
                        Curriculo = objCurriculo,
                        Disponibilidade = new Disponibilidade(Convert.ToInt32(disponibilidade))
                    });
                }

                if (dtoDadosComplementares.Disponibilidades.Contains(DTO.Enum.Disponibilidade.Viagem))
                    objPessoaFisicaComplemento.FlagViagem = true;

            }

            if (dtoDadosComplementares.Observacoes != null)
                objCurriculo.ObservacaoCurriculo = dtoDadosComplementares.Observacoes;
            if (dtoDadosComplementares.OutrosConhecimentos != null)
                objPessoaFisicaComplemento.DescricaoConhecimento = dtoDadosComplementares.OutrosConhecimentos;

            //Raça
            if (dtoDadosComplementares.Raca.HasValue)
                objPessoaFisica.Raca = new Raca(Convert.ToInt32(dtoDadosComplementares.Raca));

            //Deficiência
            if (dtoDadosComplementares.Deficiencia != null)
            {
                Deficiencia objDeficiencia = Deficiencia.CarregarPorDescricao(dtoDadosComplementares.Deficiencia);
                if (objDeficiencia == null)
                {
                    mensagemErro = "Descrição de deficiencia errada";
                    return false;
                }
                objPessoaFisica.Deficiencia = objDeficiencia;
            }

            if (dtoDadosComplementares.ComplementoDeficiencia != null)
                objPessoaFisicaComplemento.DescricaoComplementoDeficiencia = dtoDadosComplementares.ComplementoDeficiencia;

            if (dtoDadosComplementares.Veiculos != null)
            {
                lstPessoaFisicaVeiculoBll = new List<PessoaFisicaVeiculo>();
                foreach (var veiculo in dtoDadosComplementares.Veiculos)
                {
                    TipoVeiculo objTipoVeiculo = TipoVeiculo.CarregarPorDescricao(veiculo.TipoVeiculo);
                    if (objTipoVeiculo == null)
                    {
                        mensagemErro = $"Tipo de veiculo informado inválido ({veiculo.TipoVeiculo})";
                        return true;
                    }

                    lstPessoaFisicaVeiculoBll.Add(new PessoaFisicaVeiculo
                    {
                        PessoaFisica = objPessoaFisica,
                        AnoVeiculo = veiculo.Ano,
                        DescricaoModelo = veiculo.DescricaoModelo,
                        TipoVeiculo = objTipoVeiculo
                    });
                }
            }

            if (dtoDadosComplementares.DisponibilidadeOutrasCidades != null)
            {
                lstCurriculoDisponibilidadeCidadeBll = new List<CurriculoDisponibilidadeCidade>();
                foreach (var cidade in dtoDadosComplementares.DisponibilidadeOutrasCidades)
                {
                    Cidade objCidade;
                    if (!Cidade.CarregarPorNome(cidade, out objCidade))
                    {
                        mensagemErro = $"Nome de cidade indicado na disponibilidade inválido {cidade}";
                        return false;
                    }

                    lstCurriculoDisponibilidadeCidadeBll.Add(new CurriculoDisponibilidadeCidade
                    {
                        Cidade = objCidade,
                        Curriculo = objCurriculo,
                        FlagInativo = false
                    });
                }
            }

            mensagemErro = string.Empty;
            return true;
        }
        #endregion Mapear Dados Complementares

        #region [ MiniCurriculo ]

        /// <summary>
        /// Salva o minicurriculo
        /// </summary>
        /// <param name="miniCurriculo"></param>
        /// <param name="idOrigemBNE"></param>
        /// <param name="mensagemErro"></param>
        /// <returns></returns>
        public static bool MapearMiniCurriculo(
            CadastroMiniCurriculo miniCurriculo,
            int? idOrigemBNE,
            PessoaFisica objPessoaFisica,
            Curriculo objCurriculo,
            UsuarioFilialPerfil objUsuarioFilialPerfil,
            PessoaFisicaComplemento objPessoaFisicaComplemento,
            List<ParametroCurriculo> lstParametrosCurriculo,
            out string mensagemErro)
        {
            // Validando informações do minicurriculo
            if (!ValidarMiniCurriculo(objCurriculo.IdCurriculo, miniCurriculo, out mensagemErro))
                return false;

            //Pessoa Física
            objPessoaFisica.NomePessoa = Helper.AjustarString(miniCurriculo.Nome);
            objPessoaFisica.NomePessoaPesquisa = Helper.RemoverAcentos(miniCurriculo.Nome);
            objPessoaFisica.Sexo = new Sexo(Convert.ToInt32(miniCurriculo.Sexo));
            objPessoaFisica.NumeroDDDCelular = miniCurriculo.DDDCelular.ToString("0");
            objPessoaFisica.NumeroCelular = miniCurriculo.NumeroCelular.ToString("0");
            objPessoaFisica.FlagInativo = false;
            objPessoaFisica.DescricaoIP = objCurriculo.DescricaoIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            objPessoaFisica.EmailPessoa = miniCurriculo.Email;
            objCurriculo.ValorPretensaoSalarial = miniCurriculo.PretensaoSalarial;

            // Escolaridade
            Escolaridade objEscolaridade;
            if (!Escolaridade.CarregarPorNome(miniCurriculo.Escolaridade, out objEscolaridade))
            {
                mensagemErro = "A descrição de escolaridade informada não é válida";
                return false;
            }
            objPessoaFisica.Escolaridade = objEscolaridade;

            // Cidade
            Cidade objCidade;
            if (Cidade.CarregarPorNome(miniCurriculo.Cidade, out objCidade))
                objPessoaFisica.Cidade = objPessoaFisica.Endereco.Cidade = objCidade;

            // Flag Aceita estágios
            if (objEscolaridade.FlagEscolaridadeCompleta)
                miniCurriculo.AceitoEstagio = false;

            ParametroCurriculo aceitaEstagParamCurriculo = null;
            if (objCurriculo.IdCurriculo > 0 &&
                ParametroCurriculo.CarregarParametroPorCurriculo(BNE.BLL.Enumeradores.Parametro.CurriculoAceitaEstagio,
                objCurriculo,
                out aceitaEstagParamCurriculo,
                null))
            {
                aceitaEstagParamCurriculo.ValorParametro = miniCurriculo.AceitoEstagio.ToString();
            }
            else
            {
                aceitaEstagParamCurriculo = new ParametroCurriculo
                {
                    Curriculo = objCurriculo,
                    Parametro = new Parametro((int)BNE.BLL.Enumeradores.Parametro.CurriculoAceitaEstagio),
                    ValorParametro = miniCurriculo.AceitoEstagio.ToString()
                };
            }
            lstParametrosCurriculo.Add(aceitaEstagParamCurriculo);

            return true;
        }

        /// <summary>
        /// Valida as informações do MiniCurriculo
        /// </summary>
        /// <param name="idCurriculo"></param>
        /// <param name="miniCurriculo"></param>
        /// <param name="mensagemErro"></param>
        /// <returns></returns>
        public static bool ValidarMiniCurriculo(int idCurriculo,
            CadastroMiniCurriculo miniCurriculo,
            out string mensagemErro)
        {
            // Verificando cpf e data de nascimento informados
            PessoaFisica objPessoaFisica;
            if (PessoaFisica.CarregarPorCPF(miniCurriculo.Cpf, out objPessoaFisica) &&
                !objPessoaFisica.DataNascimento.Equals(miniCurriculo.DataNascimento))
            {
                mensagemErro = "CPF já cadastrado mas Data de nascimento não confere";
                return false;
            }

            // Verificando se o idCurriculo informado corresponde ao CPF
            if (idCurriculo > 0)
            {
                int idCurriculoBanco;
                if (Curriculo.CarregarIdPorCpf(miniCurriculo.Cpf, out idCurriculoBanco) &&
                    idCurriculo != idCurriculoBanco)
                {
                    mensagemErro = "CPF informado não corresponde ao curriculo informado";
                    return false;
                }
            }

            if (miniCurriculo.PretensaoSalarial <= 0)
            {
                mensagemErro = string.Format("Por favor, preencha a pretensão salarial");
                return false;
            }

            mensagemErro = string.Empty;
            return true;
        }

        /// <summary>
        /// Realiza o mepamento de funções pretendidas do DTO para objetos da BLL.
        /// </summary>
        /// <param name="funcoesPretendidas"></param>
        /// <returns></returns>
        private static List<BNE.BLL.FuncaoPretendida> MapearFuncoesPretendidas(DTO.CadastroCV.FuncaoPretendida[] funcoesPretendidas)
        {
            var retorno = new List<BNE.BLL.FuncaoPretendida>();

            foreach (var funcaoPretendida in funcoesPretendidas)
            {
                if (!String.IsNullOrEmpty(funcaoPretendida.Funcao))
                {
                    var objFuncaoPretendida = new BNE.BLL.FuncaoPretendida();
                    objFuncaoPretendida.QuantidadeExperiencia = funcaoPretendida.MesesDeExperiencia <= 0 ?
                                                                0 :
                                                                (short?)funcaoPretendida.MesesDeExperiencia;

                    Funcao objFuncao;
                    FuncaoErroSinonimo objFuncaoErroSinonimo;
                    if (Funcao.CarregarPorDescricao(funcaoPretendida.Funcao, out objFuncao))
                    {
                        objFuncaoPretendida.Funcao = objFuncao;
                        objFuncaoPretendida.DescricaoFuncaoPretendida = String.Empty;
                    }
                    else if (FuncaoErroSinonimo.CarregarPorDescricao(funcaoPretendida.Funcao, out objFuncaoErroSinonimo))
                    {
                        objFuncaoPretendida.Funcao = objFuncaoErroSinonimo.Funcao;
                        objFuncaoPretendida.DescricaoFuncaoPretendida = String.Empty;
                    }
                    else
                    {
                        objFuncaoPretendida.Funcao = null;
                        objFuncaoPretendida.DescricaoFuncaoPretendida = funcaoPretendida.Funcao;
                    }

                    retorno.Add(objFuncaoPretendida);
                }
            }

            return retorno;
        }

        #endregion [ MiniCurriculo ]

    }
}