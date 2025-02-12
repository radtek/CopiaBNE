﻿using BNE.BLL;
using BNE.BLL.Custom;
using BNE.Web.Services.Mobile.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using BNE.BLL.Custom.Solr.Buffer;
using Enumeradores = BNE.BLL.Enumeradores;
using BNE.Web.Services.Mobile.Helper;

namespace BNE.Web.Services.Mobile
{
    public class App : IApp
    {

        #region Métodos privados
        private string GetClientIP()
        {
            OperationContext context = OperationContext.Current;
            MessageProperties prop = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint =
                prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            string ip = endpoint.Address;

            return ip;
        }

        private void SalvarFuncaoPretendida(string funcao, List<FuncaoPretendida> listFuncoesPretendidas)
        {
            if (!String.IsNullOrEmpty(funcao))
            {
                foreach (var funcaoPretendida in listFuncoesPretendidas)
                {
                    funcaoPretendida.Persisted(false);
                }
                FuncaoPretendida objFuncaoPretendidaAuxiliar = listFuncoesPretendidas.FirstOrDefault(f => (f.Funcao != null && f.Funcao.DescricaoFuncao == funcao) || f.DescricaoFuncaoPretendida == funcao);

                if (objFuncaoPretendidaAuxiliar == null) //Se for uma nova função
                {
                    var objFuncaoPretendida = new FuncaoPretendida();

                    Funcao objFuncao;
                    FuncaoErroSinonimo objFuncaoErroSinonimo;
                    if (Funcao.CarregarPorDescricao(funcao, out objFuncao))
                    {
                        objFuncaoPretendida.Funcao = objFuncao;
                        objFuncaoPretendida.DescricaoFuncaoPretendida = String.Empty;
                    }
                    else if (FuncaoErroSinonimo.CarregarPorDescricao(funcao, out objFuncaoErroSinonimo))
                    {
                        objFuncaoErroSinonimo.Funcao.CompleteObject();
                        objFuncaoPretendida.Funcao = objFuncaoErroSinonimo.Funcao;
                        objFuncaoPretendida.DescricaoFuncaoPretendida = String.Empty;
                    }
                    else
                    {
                        objFuncaoPretendida.Funcao = null;
                        objFuncaoPretendida.DescricaoFuncaoPretendida = funcao;
                    }
                    objFuncaoPretendida.QuantidadeExperiencia = 0;

                    if(listFuncoesPretendidas.Count > 0)
                        listFuncoesPretendidas.RemoveAt(0);

                    listFuncoesPretendidas.Insert(0, objFuncaoPretendida);
                }
            }
        }


        #endregion

        #region Teste
        public OutStatusDTO Teste()
        {
            return new OutStatusDTO() { Status = 1 };
        }
        #endregion

        #region Candidatar
        public OutStatusDTO Candidatar(InCandidatarDTO candidatar)
        {
            OutStatusDTO dto = new OutStatusDTO();

            try
            {
                var listPerguntas = new List<VagaCandidatoPergunta>();
                if (candidatar.Perguntas != null)
                {
                    foreach (RespostaPerguntaDTO objResposta in candidatar.Perguntas)
                    {
                        VagaCandidatoPergunta objVagaCandidatoPergunta;

                        objVagaCandidatoPergunta = new VagaCandidatoPergunta()
                        {
                            VagaPergunta = new VagaPergunta(objResposta.IdfVagaPergunta),
                            FlagResposta = objResposta.RespostaPergunta,
                        };

                        listPerguntas.Add(objVagaCandidatoPergunta);
                    }
                }

                int? quantidadeCandidaturas;
                Curriculo curriculo = Curriculo.LoadObject(candidatar.Id_Curriculo);
                VagaCandidato.Candidatar(curriculo,
                    new Vaga(candidatar.Id_Vaga),
                    null,
                    listPerguntas,
                    GetClientIP(), false, false, candidatar.Avulsa, Enumeradores.OrigemCandidatura.WebServiceMobile,  out quantidadeCandidaturas);

                BufferAtualizacaoCurriculo.Update(curriculo);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex,
                    "erro candidatura App - IdVaga:" + candidatar.Id_Vaga + " / Id_Curriculo: " +
                    candidatar.Id_Curriculo + " / Status:" + candidatar.Status
                    + " / Avulsa:" + candidatar.Avulsa);

                dto.Status = (int)Status.ERRO;
                dto.erro = ex.Message;
                return dto;
            }

            dto.Status = (int)Status.SUCESSO_SIM;
            return dto;
        }
        #endregion

        #region CandidataTanque
        public OutCandidaturaTanqueDTO CandidatarTanque(InCandidatarDTO candidatar)
        {
            OutCandidaturaTanqueDTO dto = new OutCandidaturaTanqueDTO();

            try
            {
                //Se tiver pergunta não candidata e retorna o link da vaga para a pessoa acessar
                if (VagaPergunta.RecuperarListaPerguntas(candidatar.Id_Vaga, null).Count > 0)
                {
                    Curriculo objCurriculo = Curriculo.LoadObject(candidatar.Id_Curriculo);
                    string linkVaga = Vaga.MontarLinkVagaSMS(candidatar.Id_Vaga, objCurriculo, "?utm_source=candidatarvaga&utm_medium=SMS&utm_campaign=vagacomperguntaSMS");
                    dto.linkVaga = linkVaga;
                    dto.Status = (int)Status.INSUCESSO_NAO;
                }
                else
                {
                    int? quantidadeCandidaturas;
                    var listPerguntas = new List<VagaCandidatoPergunta>();
                    Curriculo curriculo = Curriculo.LoadObject(candidatar.Id_Curriculo);
                    VagaCandidato.Candidatar(curriculo,
                        new Vaga(candidatar.Id_Vaga),
                        null,
                        listPerguntas,
                        GetClientIP(), false, false, candidatar.Avulsa, Enumeradores.OrigemCandidatura.Tanque, out quantidadeCandidaturas);

                    BufferAtualizacaoCurriculo.Update(curriculo);
                    dto.Status = (int)Status.SUCESSO_SIM;
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "CandidatarTanque -> IdVaga:" + candidatar.Id_Vaga + " / Curriculo:" + candidatar.Id_Curriculo + "/ Avulsa:" + candidatar.Avulsa);
                dto.Status = (int)Status.ERRO;
                dto.erro = ex.Message;
                return dto;
            }


            return dto;
        }
        #endregion

        #region [CampanhaTanque]
        /// <summary>
        /// Utilizado para Retornar a campanha de Recrutamento disparada no bne.
        /// Para retornar o telefone de contato da sms de responda da campanha.
        /// </summary>
        /// <param name="vaga"></param>
        /// <returns></returns>
        public OutCampanhaTanqueDTO CampanhaTanque(InCampanhaTanqueDTO vaga)
        {
            OutCampanhaTanqueDTO objCampanha = new OutCampanhaTanqueDTO();
            try
            {
                CampanhaRecrutamento objCampanhaRecrutamento = CampanhaRecrutamento.getCampanhaPorVaga(vaga.IdVaga); 
                objCampanha.idCampanhaRecrutamento = objCampanhaRecrutamento.IdCampanhaRecrutamento;
                //objCampanha.IdmotivoCampanhaFinalizada = objCampanhaRecrutamento.MotivoCampanhaFinalizada.IdMotivoCampanhaFinalizada;
                //objCampanha.quantidadeRetorno = objCampanhaRecrutamento.QuantidadeRetorno;
                objCampanha.tipoRetornoCampanhaRecrutamento = objCampanhaRecrutamento.TipoRetornoCampanhaRecrutamento.IdTipoRetornoCampanhaRecrutamento;
                objCampanha.NumeroDDDTelefoneContato = objCampanhaRecrutamento.NumeroDDDTelefoneContato;
                objCampanha.NUMTelefoneContato = objCampanhaRecrutamento.NUMTelefoneContato;
                objCampanha.Status = (int)Status.SUCESSO_SIM;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Erro ao busca Campanha -> VagaId:" + vaga.IdVaga);
                objCampanha.erro = ex.ToString();
                objCampanha.Status = (int)Status.INSUCESSO_NAO;
            }
           
            return objCampanha;

        }
        #endregion

        #region DadosUsuario
        public OutDadosUsuarioDTO DadosUsuario(int c)
        {
            try
            {
                OutDadosUsuarioDTO dto = new OutDadosUsuarioDTO();

                int id_curriculo = c;

                Curriculo curriculo = Curriculo.LoadObject(id_curriculo);
                curriculo.PessoaFisica.CompleteObject();
                curriculo.PessoaFisica.Endereco.CompleteObject();
                curriculo.PessoaFisica.Endereco.Cidade.CompleteObject();
                curriculo.PessoaFisica.Endereco.Cidade.Estado.CompleteObject();

                // estatisticas do curriculo
                dto.FlagVip = curriculo.FlagVIP;

                // dados do usuario
                dto.Cpf = curriculo.PessoaFisica.CPF.ToString("00000000000");
                dto.Nome = curriculo.PessoaFisica.NomePessoa;
                dto.Nascimento = curriculo.PessoaFisica.DataNascimento.ToString("s");
                dto.Cidade = BLL.Custom.Helper.FormatarCidade(curriculo.PessoaFisica.Endereco.Cidade.NomeCidade,
                    curriculo.PessoaFisica.Endereco.Cidade.Estado.SiglaEstado);
                dto.Celular =
                    (curriculo.PessoaFisica.NumeroDDDCelular ?? String.Empty) +
                    (curriculo.PessoaFisica.NumeroCelular ?? String.Empty);
                dto.Email = curriculo.PessoaFisica.EmailPessoa;
                if (curriculo.ValorPretensaoSalarial.HasValue)
                    dto.PretensaoSalarial = ((decimal)curriculo.ValorPretensaoSalarial).ToString("0.00", CultureInfo.InvariantCulture);
                dto.Sexo = curriculo.PessoaFisica.Sexo.IdSexo;

                FuncaoPretendida funcaoPretendida;
                FuncaoPretendida.CarregarPorCurriculo(id_curriculo, out funcaoPretendida);
                if (funcaoPretendida != null)
                {
                    funcaoPretendida.Funcao.CompleteObject();
                    dto.Funcao = funcaoPretendida.Funcao.DescricaoFuncao;
                }
                using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
                {
                    conn.Open();

                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            ParametroCurriculo objParametroCurriculo = null;
                            dto.Candidaturas = ParametroCurriculo.CarregarParametroPorCurriculo(Enumeradores.Parametro.QuantidadeCandidaturaDegustacao, curriculo, out objParametroCurriculo, trans);
                            if (dto.Candidaturas || objParametroCurriculo != null)
                            {
                                dto.QuantidadeCandidaturas = Convert.ToInt32(objParametroCurriculo.ValorParametro);
                            }
                            else
                            {
                                dto.QuantidadeCandidaturas = 0;
                            }
                            trans.Commit();
                        }
                        catch
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                    conn.Close();
                }
                // Planos a venda para o usuario
                bool stcUniversitario = false;
                bool stcComVIP = false;
                List<Origem> listOrigens;
                if (CurriculoOrigem.CarregarOrigensDoCurriculo(curriculo, out listOrigens))
                {
                    // existe pelo menos uma origem para o curriculo
                    stcUniversitario =
                        listOrigens.Any(origem =>
                        {
                            OrigemFilial origemFilial;

                            try
                            {
                                origemFilial =
                                    OrigemFilial.CarregarPorOrigem(origem.IdOrigem);
                            }
                            catch (EL.RecordNotFoundException)
                            {
                                return false;
                            }

                            return origemFilial == null ? false : origemFilial.Filial.PossuiSTCUniversitario(); // condicao para ser vip universitario
                        });

                    stcComVIP =
                        listOrigens.Any(origem =>
                        {
                            OrigemFilial origemFilial;

                            try
                            {
                                origemFilial =
                                    OrigemFilial.CarregarPorOrigem(origem.IdOrigem);
                            }
                            catch (EL.RecordNotFoundException)
                            {
                                return false;
                            }

                            return origemFilial == null ? false : origemFilial.Filial.PossuiSTCUniversitario(); // condicao para ser parceiro com vip
                        });
                }

                if (stcUniversitario && stcComVIP)
                {
                    // retorna planos mensal e trimestral do VIP Universitario
                    dto.IdPlanoMensal =
                        Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.PlanoVIPUniversitarioMensal));
                    dto.IdPlanoTri =
                        Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.PlanoVIPUniversitarioTrimestral));
                    dto.VlrPlanoMensalSemDesconto = Parametro.RecuperaValorParametro(Enumeradores.Parametro.ValorSemDescontoVIPOperacaoMensal);
                    dto.VlrPlanoTrimestralSemDesconto = Parametro.RecuperaValorParametro(Enumeradores.Parametro.ValorSemDescontoVIPOperacaoTrimestral);
                }
                else
                {
                    // retorna planos de acordo com a categoria da funcao
                    FuncaoCategoria funcaoCategoria = FuncaoCategoria.RecuperarCategoriaPorCurriculo(curriculo);
                    dto.IdPlanoMensal =
                        Plano.RecuperarCodigoPlanoMensalPorFuncaoCategoria(funcaoCategoria);
                    dto.IdPlanoTri =
                        Plano.RecuperarCodigoPlanoTrimestralPorFuncaoCategoria(funcaoCategoria);
                    switch (funcaoCategoria.IdFuncaoCategoria)
                    {
                        case (int)Enumeradores.FuncaoCategoria.Operacao:
                            dto.VlrPlanoMensalSemDesconto = Parametro.RecuperaValorParametro(Enumeradores.Parametro.ValorSemDescontoVIPOperacaoMensal);
                            dto.VlrPlanoTrimestralSemDesconto = Parametro.RecuperaValorParametro(Enumeradores.Parametro.ValorSemDescontoVIPOperacaoTrimestral);
                            break;
                        case (int)Enumeradores.FuncaoCategoria.Apoio:
                            dto.VlrPlanoMensalSemDesconto = Parametro.RecuperaValorParametro(Enumeradores.Parametro.ValorSemDescontoVIPApoioMensal);
                            dto.VlrPlanoTrimestralSemDesconto = Parametro.RecuperaValorParametro(Enumeradores.Parametro.ValorSemDescontoVIPApoioTrimestral);
                            break;
                        case (int)Enumeradores.FuncaoCategoria.Especialista:
                            dto.VlrPlanoMensalSemDesconto = Parametro.RecuperaValorParametro(Enumeradores.Parametro.ValorSemDescontoVIPEspecialistaMensal);
                            dto.VlrPlanoTrimestralSemDesconto = Parametro.RecuperaValorParametro(Enumeradores.Parametro.ValorSemDescontoVIPEspecialistaTrimestral);
                            break;
                        case (int)Enumeradores.FuncaoCategoria.Gestao:
                            dto.VlrPlanoMensalSemDesconto = Parametro.RecuperaValorParametro(Enumeradores.Parametro.ValorSemDescontoVIPGestaoMensal);
                            dto.VlrPlanoTrimestralSemDesconto = Parametro.RecuperaValorParametro(Enumeradores.Parametro.ValorSemDescontoVIPGestaoTrimestral);
                            break;
                    }

                }

                dto.Foto = string.Empty;
                try
                {
                    byte[] byteArray = BLL.PessoaFisicaFoto.RecuperarArquivo(curriculo.PessoaFisica.CPF);
                    if (byteArray != null)
                        dto.Foto = Convert.ToBase64String(CreateThumbnail(byteArray, 640));
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex);
                    dto.Foto = null;
                }

                return dto;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "DadosUsuario -> c:" + c);
                throw;
            }
        }
        public static byte[] CreateThumbnail(byte[] PassedImage, int LargestSide)
        {
            byte[] ReturnedThumbnail;

            using (MemoryStream StartMemoryStream = new MemoryStream(),
                                NewMemoryStream = new MemoryStream())
            {
                // write the string to the stream  
                StartMemoryStream.Write(PassedImage, 0, PassedImage.Length);

                // create the start Bitmap from the MemoryStream that contains the image  
                Bitmap startBitmap = new Bitmap(StartMemoryStream);

                // set thumbnail height and width proportional to the original image.  
                int newHeight;
                int newWidth;
                double HW_ratio;
                if (startBitmap.Height > startBitmap.Width)
                {
                    newHeight = LargestSide;
                    HW_ratio = (double)((double)LargestSide / (double)startBitmap.Height);
                    newWidth = (int)(HW_ratio * (double)startBitmap.Width);
                }
                else
                {
                    newWidth = LargestSide;
                    HW_ratio = (double)((double)LargestSide / (double)startBitmap.Width);
                    newHeight = (int)(HW_ratio * (double)startBitmap.Height);
                }

                // create a new Bitmap with dimensions for the thumbnail.  
                Bitmap newBitmap = new Bitmap(newWidth, newHeight);

                // Copy the image from the START Bitmap into the NEW Bitmap.  
                // This will create a thumnail size of the same image.  
                newBitmap = ResizeImage(startBitmap, newWidth, newHeight);

                // Save this image to the specified stream in the specified format.  
                newBitmap.Save(NewMemoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                // Fill the byte[] for the thumbnail from the new MemoryStream.  
                ReturnedThumbnail = NewMemoryStream.ToArray();
            }

            // return the resized image as a string of bytes.  
            return ReturnedThumbnail;
        }

        // Resize a Bitmap  
        private static Bitmap ResizeImage(Bitmap image, int width, int height)
        {
            Bitmap resizedImage = new Bitmap(width, height);
            using (Graphics gfx = Graphics.FromImage(resizedImage))
            {
                gfx.DrawImage(image, new Rectangle(0, 0, width, height),
                    new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
            }
            return resizedImage;
        }
        #endregion

        #region PesquisaVagas
        public OutPesquisaVagasDTO PesquisaVagas(InPesquisaVagasDTO pesquisaVagas)
        {
            try
            {
                OutPesquisaVagasDTO dto = new OutPesquisaVagasDTO();
                PesquisaVaga objPesquisaVaga = new PesquisaVaga();

                Funcao funcao = null;
                if (!string.IsNullOrWhiteSpace(pesquisaVagas.Funcao))
                    Funcao.CarregarPorDescricao(pesquisaVagas.Funcao, out funcao);

                Cidade cidade = null;
                if (!string.IsNullOrWhiteSpace(pesquisaVagas.Cidade))
                    Cidade.CarregarPorNome(pesquisaVagas.Cidade, out cidade);

                int totalRegistros;
                if (funcao == null && cidade == null && pesquisaVagas.Id_Empresa != 0)
                {
                    objPesquisaVaga = null;
                }
                else
                {
                    objPesquisaVaga.Funcao = funcao;
                    objPesquisaVaga.Cidade = cidade;
                }
                DataTable dt = new DataTable();

                try
                #region [Consultar vaga na API]
                {
                    dt = BLL.PesquisaVaga.BuscaVagaAPIDT(new PesquisaVaga()
                    {
                        Funcao = funcao,
                        Cidade = cidade,
                        Curriculo =  pesquisaVagas.Id_Curriculo.HasValue ? new Curriculo(pesquisaVagas.Id_Curriculo.Value) : null
                    },
                          10,
                          (int)(pesquisaVagas.Pagina ?? 1),
                          null,
                          null,
                          false,
                          OrdenacaoBuscaVaga.Padrao,
                          out totalRegistros
                          );

                }
                #endregion

                catch (Exception ex)
                {
                   EL.GerenciadorException.GravarExcecao(ex, "Consulta vaga na api - pelo mobile");

                   dt = PesquisaVaga.BuscaVagaFullText(
                   objPesquisaVaga,
                   10,
                   (int)(pesquisaVagas.Pagina ?? 1),
                   pesquisaVagas.Id_Curriculo,
                   funcao == null ? null : (int?)funcao.IdFuncao,
                   cidade == null ? null : (int?)cidade.IdCidade,
                   String.Empty,
                   null,
                   null,
                   String.Empty,
                   pesquisaVagas.Id_Empresa,
                   null,
                   OrdenacaoBuscaVaga.Padrao,
                   out totalRegistros);
                }

                foreach (DataRow row in dt.Rows)
                {

                    VagaDTO item = new VagaDTO();

                    item.Atribuicoes = row["Des_Atribuicoes"].ToString();
                    item.Cidade = row["Nme_Cidade"].ToString().Contains("/") ?  row["Nme_Cidade"].ToString() : BLL.Custom.Helper.FormatarCidade(row["Nme_Cidade"].ToString(), row["Sig_Estado"].ToString()) ;
                    item.Funcao = row["Des_Funcao"].ToString().Equals("Estagiário") ? row["Des_Curso"].ToString() : row["Des_Funcao"].ToString();
                    item.Id_Vaga = Convert.ToInt32(row["Idf_Vaga"]);
                    item.QuantidadeDeVagas = Convert.ToInt32(row["Qtd_Vaga"]);
                    if (row["Vlr_Salario_Para"] == DBNull.Value || Convert.ToDecimal(row["Vlr_Salario_Para"]) <= 0)
                        item.SalarioMax = null;
                    else
                        item.SalarioMax = Convert.ToDecimal(row["Vlr_Salario_Para"])
                            .ToString("0.00", CultureInfo.InvariantCulture);
                    if (row["Vlr_Salario_De"] == DBNull.Value || Convert.ToDecimal(row["Vlr_Salario_De"]) <= 0)
                        item.SalarioMin = null;
                    else
                        item.SalarioMin = Convert.ToDecimal(row["Vlr_Salario_De"])
                            .ToString("0.00", CultureInfo.InvariantCulture);
                    item.StatusVaga = Convert.ToInt32(row["Flg_Candidatou"]);
                    item.Url = row["Url_Vaga"].ToString();
                    item.Arquivada = Convert.ToBoolean(row["Flg_Vaga_Arquivada"]);
                    item.PCD = row["Idf_Deficiencia"] != DBNull.Value && Convert.ToInt32(row["Idf_Deficiencia"]) > 0 ? true : false;

                    OutPerguntasVagaDTO temp = PerguntasVaga(item.Id_Vaga);
                    item.Perguntas = temp.Status;

                    #region Tipo de Vinculo
                    if (VagaTipoVinculo.PossuiVinculo(new Vaga(item.Id_Vaga), Enumeradores.TipoVinculo.Estágio))
                    {
                        if (item.Funcao != "Estagiário") item.Funcao = "Estágio para " + item.Funcao;
                    }
                    #endregion

                    dto.ListaDeVagas.Add(item);
                }

                dto.Paginas =
                    10 * (int)(pesquisaVagas.Pagina ?? 1) < totalRegistros;


                return dto;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "PesquisaVagas");
                throw;
            }
        }
        #endregion

        #region DetalhesVaga
        public OutDetalhesVagaDTO DetalhesVaga(InDetalhesVagaDTO detalhes)
        {
            try
            {
                OutDetalhesVagaDTO dto = new OutDetalhesVagaDTO();

                Vaga vaga = Vaga.LoadObject(detalhes.Id_Vaga);
                vaga.Cidade.CompleteObject();
                vaga.Cidade.Estado.CompleteObject();
                vaga.Funcao.CompleteObject();
                vaga.Funcao.AreaBNE.CompleteObject();

                Curriculo curriculo = Curriculo.LoadObject(detalhes.Id_Curriculo);

                VagaVisualizada.SalvarVisualizacaoVaga(vaga, curriculo);

                dto.Atribuicoes = vaga.DescricaoAtribuicoes;
                dto.Beneficios = vaga.DescricaoBeneficio;
                dto.DataPublicacao = vaga.DataCadastro.ToString("s");
                dto.Cidade = BLL.Custom.Helper.FormatarCidade(vaga.Cidade.NomeCidade, vaga.Cidade.Estado.SiglaEstado);
                dto.QuantidadeDeVagas = (int)vaga.QuantidadeVaga;
                dto.Requisitos = vaga.DescricaoRequisito;
                dto.Id_Empresa = vaga.Filial.IdFilial; // é passada a filial e não empresa!
                dto.SalarioMax = vaga.ValorSalarioPara == null ?
                    null :
                    ((decimal)vaga.ValorSalarioPara).ToString("0.00", CultureInfo.InvariantCulture);
                dto.SalarioMin = vaga.ValorSalarioDe == null ?
                    null :
                    ((decimal)vaga.ValorSalarioDe).ToString("0.00", CultureInfo.InvariantCulture);
                dto.Funcao = vaga.Funcao.DescricaoFuncao;

                dto.StatusVaga = VagaCandidato.CurriculoJaCandidatouVaga(curriculo, vaga) ? 1 : 0;

                if (vaga.Deficiencia != null && vaga.Deficiencia.IdDeficiencia > 0) {
                    vaga.Deficiencia.CompleteObject();
                    dto.ListaDeficiencia.Add(new DeficienciaDTO { DescricaoDeficiencia = vaga.Deficiencia.DescricaoDeficiencia });
                }


                #region Tipo de Vinculo
                if (VagaTipoVinculo.PossuiVinculo(new Vaga(vaga.IdVaga), Enumeradores.TipoVinculo.Estágio))
                {

                    var listaCurso = VagaCurso.ListarCursoPorVaga(vaga.IdVaga);
                    if (listaCurso.Count.Equals(0))
                        dto.Funcao = "Estágio para " + dto.Funcao;
                    else if (listaCurso.Count.Equals(1))
                    {
                        dto.Funcao = "Estágio para ";
                        dto.Funcao = string.Concat(dto.Funcao, listaCurso[0].Curso != null ? listaCurso[0].Curso.DescricaoCurso : listaCurso[0].DescricaoCurso);
                    }
                    else
                    {
                        dto.Funcao = "Estágio para ";
                        for (int i = 0; i < listaCurso.Count(); i++)
                        {
                            if (i < listaCurso.Count() - 1)
                            {
                                dto.Funcao = string.Concat(dto.Funcao, listaCurso[i].Curso != null ? listaCurso[i].Curso.DescricaoCurso : listaCurso[i].DescricaoCurso);
                                dto.Funcao = string.Concat(dto.Funcao, ", ");
                            }
                            else
                            {
                                dto.Funcao = dto.Funcao.Remove(dto.Funcao.Length - 2) + " ou ";
                                dto.Funcao = string.Concat(dto.Funcao, listaCurso[i].Curso != null ? listaCurso[i].Curso.DescricaoCurso : listaCurso[i].DescricaoCurso);
                            }

                        }
                    }
                }
                #endregion


                dto.Url = Vaga.MontarUrlVaga(detalhes.Id_Vaga,
                    vaga.Funcao.DescricaoFuncao,
                    vaga.Funcao.AreaBNE.DescricaoAreaBNE,
                    vaga.Cidade.NomeCidade,
                    vaga.Cidade.Estado.SiglaEstado);

                OutPerguntasVagaDTO temp = PerguntasVaga(vaga.IdVaga);
                dto.Perguntas = temp.Status;
                dto.Arquivada = vaga.FlagVagaArquivada;

                return dto;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "DetalhesVaga");
                throw;
            }
        }
        #endregion

        #region PerguntasVaga
        public OutPerguntasVagaDTO PerguntasVaga(int v)
        {
            OutPerguntasVagaDTO dto = new OutPerguntasVagaDTO();
            try
            {
               
                dto.Status = (int)Status.INSUCESSO_NAO;

                int id_vaga = v;

                DataTable dt;
                try
                {
                    dt = VagaPergunta.RecuperarPerguntas(new Vaga(id_vaga));
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex);
                    dto.Status = (int)Status.ERRO;
                    return dto;
                }

                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        dto.Perguntas.Add(new VagaPerguntaDTO()
                        {
                            DescricaoPergunta = row["Des_Vaga_Pergunta"].ToString(),
                            IdfVagaPergunta = Convert.ToInt32(row["Idf_Vaga_Pergunta"])
                        });

                        dto.Status = (int)Status.SUCESSO_SIM;
                    }
                }

                return dto;
            }
            catch(Exception ex)               
            {
                EL.GerenciadorException.GravarExcecao(ex, "PerguntasVaga");
            }
            return dto;

        }
        #endregion

        #region CadastroMiniCurriculo
        public OutCadastroMiniCurriculoDTO CadastroMiniCurriculo(InCadastroMiniCurriculoDTO cadastro)
        {
            OutCadastroMiniCurriculoDTO dto = new OutCadastroMiniCurriculoDTO();

            try
            {
                Curriculo objCurriculo;
                PessoaFisica objPessoaFisica;
                Endereco endereco;
                Cidade cidade;
                PessoaFisicaComplemento complemento;
                UsuarioFilialPerfil perfil;

                #region Validação Salario
                string salarioMin = Parametro.RecuperaValorParametro(Enumeradores.Parametro.SalarioMinimoNacional);
                string salario = cadastro.PretensaoSalarial;
                if (!string.IsNullOrEmpty(salarioMin))
                {
                    if (salario.Contains(".") || salario.Contains(","))
                    {
                        salario = salario.Replace(",", "").Replace(".", "");
                        decimal vlrSalario = Convert.ToDecimal(salario);

                        if (salario.Length == 4 || salario.Length == 5)
                        {
                            salario = (vlrSalario / 10).ToString();
                        }
                        else if (salario.Length >= 6)
                        {
                            salario = (vlrSalario / 100).ToString();
                        }
                    }
                    if (Convert.ToDecimal(salario) < Convert.ToDecimal(string.Format("{0:N}", salarioMin.Replace(",", "."))))
                    {
                        dto.Status = (int)Status.SALARIOMINIMO;
                        dto.Mensagem = $"A pretenção salarial deve ser maior que o salário minímo R$ {salarioMin}.";
                        return dto;
                    }
                }
                else
                {
                    dto.Status = (int)Status.ERRO;
                }
                #endregion

                if (PessoaFisica.CarregarPorCPF(cadastro.Cpf, out objPessoaFisica) &&
                    (objPessoaFisica.FlagInativo == null || objPessoaFisica.FlagInativo == false))
                {
                    DateTime nascimento = DateTime.Parse(cadastro.Nascimento,
                        CultureInfo.GetCultureInfo("pt-br")).Date;

                    if (nascimento.Equals(objPessoaFisica.DataNascimento))
                    {
                        if (Curriculo.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objCurriculo))
                        {
                            cadastro.Id_Curriculo = objCurriculo.IdCurriculo;
                        }
                    }
                    else
                    {
                        dto.Id_Curriculo = 0;
                        dto.Status = (int)Status.INSUCESSO_NAO;
                        return dto;
                    }
                }
                if (cadastro.Id_Curriculo != null)
                {
                    //atualiza cadastro
                    objCurriculo = Curriculo.LoadObject((int)cadastro.Id_Curriculo);
                    objPessoaFisica = objCurriculo.PessoaFisica;
                    objCurriculo.PessoaFisica.CompleteObject();
                    endereco = objPessoaFisica.Endereco;
                    objPessoaFisica.Endereco.CompleteObject();
                    if (!PessoaFisicaComplemento.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out complemento))
                    {
                        complemento = new PessoaFisicaComplemento();
                    }
                    UsuarioFilialPerfil.CarregarUsuarioFilialPerfilCandidatoAtivo(objPessoaFisica, out perfil);
                }
                else
                {
                    //cadastro novo
                    objCurriculo = new Curriculo();
                    objPessoaFisica = new PessoaFisica();
                    endereco = new Endereco();
                    objPessoaFisica.Endereco = endereco;
                    complemento = new PessoaFisicaComplemento();
                    perfil = new UsuarioFilialPerfil()
                    {
                        Perfil = new Perfil((int)Enumeradores.Perfil.AcessoNaoVIP)
                    };
                }

                var listFuncoesPretendidas = FuncaoPretendida.CarregarFuncoesPretendidasPorCurriculo(objCurriculo);

                Funcao funcao;
                Funcao.CarregarPorDescricao(cadastro.Funcao, out funcao);

                SalvarFuncaoPretendida(cadastro.Funcao, listFuncoesPretendidas);

                Cidade.CarregarPorNome(cadastro.Cidade, out cidade);
                objPessoaFisica.Endereco.Cidade = cidade;
                objPessoaFisica.Cidade = cidade;

                objPessoaFisica.NumeroCPF = cadastro.Cpf;
                objPessoaFisica.NomePessoa = cadastro.Nome;
                objPessoaFisica.NomePessoaPesquisa = BoundaryHelper.RemoverAcentos(cadastro.Nome);
                objPessoaFisica.DataNascimento = DateTime.Parse(cadastro.Nascimento);
                objPessoaFisica.Sexo = new Sexo(cadastro.Sexo);
                objPessoaFisica.NumeroDDDCelular = cadastro.Celular.Substring(0, 2);
                objPessoaFisica.NumeroCelular = cadastro.Celular.Substring(2);
                objPessoaFisica.FlagInativo = false;
                objPessoaFisica.DescricaoIP = GetClientIP();
                objPessoaFisica.EmailPessoa = cadastro.Email;

                objCurriculo.ValorPretensaoSalarial =
                    Convert.ToDecimal(cadastro.PretensaoSalarial, CultureInfo.InvariantCulture);
                objCurriculo.DescricaoIP = GetClientIP();
                objCurriculo.SalvarMiniCurriculo(objPessoaFisica, listFuncoesPretendidas, null, null,null, perfil, complemento, null, null, null);

                //grava codigo facebook
                if (!String.IsNullOrEmpty(cadastro.CodUserFacebook))
                {
                    PessoaFisicaRedeSocial pfRedeSocial;
                    if (!PessoaFisicaRedeSocial.CarregarPorPessoaFisicaRedeSocial(objPessoaFisica.IdPessoaFisica,
                        (int)Enumeradores.RedeSocial.FaceBook,
                        out pfRedeSocial))
                    {
                        pfRedeSocial = new PessoaFisicaRedeSocial();
                        pfRedeSocial.PessoaFisica = objPessoaFisica;
                        pfRedeSocial.FlagInativo = false;
                        pfRedeSocial.CodigoIdentificador = objPessoaFisica.EmailPessoa;
                        pfRedeSocial.RedeSocialCS = new RedeSocialCS((int)Enumeradores.RedeSocial.FaceBook);
                    }
                    pfRedeSocial.CodigoInternoRedeSocial = cadastro.CodUserFacebook;
                    pfRedeSocial.Save();
                }

                dto.Id_Curriculo = objCurriculo.IdCurriculo;

                if (cadastro.Id_Curriculo != null)
                    dto.Status = (int)Status.ATUALIZADO;
                else
                    dto.Status = (int)Status.SUCESSO_SIM;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "CadastroMiniCurriculo");
                dto.Status = (int)Status.ERRO;
            }

            return dto;
        }
        #endregion

        #region DetalhesEmpresa
        public OutDetalhesEmpresaDTO DetalhesEmpresa(InDetalhesEmpresaDTO detalhes)
        {
            try
            {
                OutDetalhesEmpresaDTO dto = new OutDetalhesEmpresaDTO();

                Filial filial = Filial.LoadObject(detalhes.Id_Empresa);

                filial.Endereco.CompleteObject();
                filial.Endereco.Cidade.CompleteObject();
                filial.Endereco.Cidade.Estado.CompleteObject();

                if (detalhes.Id_Vaga.HasValue && detalhes.Id_Vaga.Value != -1)
                {
                    Vaga vaga = Vaga.LoadObject(detalhes.Id_Vaga.Value);
                    dto.Nome_Empresa = !vaga.FlagConfidencial ? filial.RazaoSocial : "Confidencial";
                }
                else
                {
                    dto.Nome_Empresa = filial.RazaoSocial;
                }

                dto.Cidade = BLL.Custom.Helper.FormatarCidade(filial.Endereco.Cidade.NomeCidade, filial.Endereco.Cidade.Estado.SiglaEstado);
                dto.NumeroVagasDivulgadas = filial.RecuperarQuantidadeVagasDivuldadas();
                dto.QuantidadeDeFuncionarios = filial.QuantidadeFuncionarios;

                return dto;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "detalhes");
                throw;
            }
        }
        #endregion

        #region Login
        public OutLoginDTO Login(InLoginDTO login)
        {
            OutLoginDTO dto;

            try
            {
                PessoaFisica pf;
                if (PessoaFisica.CarregarPorCPF(login.Cpf, out pf) &&
                    (pf.FlagInativo == null || pf.FlagInativo == false))
                {

                    DateTime nascimento = new DateTime();
                    if (login.Data_Nascimento != null)
                    {
                        nascimento = DateTime.Parse(login.Data_Nascimento,
                        CultureInfo.GetCultureInfo("pt-br")).Date;
                    }
                    
                    if (nascimento.Equals(pf.DataNascimento))
                    {
                        Curriculo curriculo;
                        if (Curriculo.CarregarPorPessoaFisica(pf.IdPessoaFisica, out curriculo))
                        {//Task 43574 - Não logar quem esta com status de exclusão lógica
                            if (curriculo == null || curriculo.SituacaoCurriculo.IdSituacaoCurriculo.Equals((int)Enumeradores.SituacaoCurriculo.ExclusaoLogica))
                            {
                                dto = new OutLoginDTO()
                                {
                                    Id_Curriculo = 0,
                                    Status = (int)Status.INSUCESSO_NAO
                                };
                                return dto;
                            }
                            if (curriculo.FlagInativo || curriculo.SituacaoCurriculo.IdSituacaoCurriculo.Equals((int)Enumeradores.SituacaoCurriculo.Bloqueado) || curriculo.SituacaoCurriculo.IdSituacaoCurriculo.Equals((int)Enumeradores.SituacaoCurriculo.Cancelado))
                            {
                                return new OutLoginDTO()
                                {
                                    Id_Curriculo = 0,
                                    Status = (int)Status.ATUALIZADO /* Retorna quando o curriculo esta bloqueado  */
                                };
                            }

                            UsuarioFilialPerfil perfil;
                            if (UsuarioFilialPerfil.CarregarUsuarioFilialPerfilCandidatoAtivo(pf, out perfil))
                            {
                                dto = new OutLoginDTO()
                                {
                                    Id_Curriculo = curriculo.IdCurriculo,
                                    Status = (int)Status.SUCESSO_SIM
                                };

                                //grava codigo facebook
                                if (!String.IsNullOrEmpty(login.UserCodFacebook))
                                {
                                    PessoaFisicaRedeSocial pfRedeSocial;
                                    if (!PessoaFisicaRedeSocial.CarregarPorPessoaFisicaRedeSocial(pf.IdPessoaFisica,
                                        (int)Enumeradores.RedeSocial.FaceBook,
                                        out pfRedeSocial))
                                    {
                                        pfRedeSocial = new PessoaFisicaRedeSocial();
                                        pfRedeSocial.PessoaFisica = pf;
                                        pfRedeSocial.FlagInativo = false;
                                        pfRedeSocial.CodigoIdentificador = pf.EmailPessoa;
                                        pfRedeSocial.RedeSocialCS = new RedeSocialCS((int)Enumeradores.RedeSocial.FaceBook);
                                    }
                                    pfRedeSocial.CodigoInternoRedeSocial = login.UserCodFacebook;
                                    pfRedeSocial.Save();
                                }

                                return dto;
                            }

                        }
                        else
                        {
                            dto = new OutLoginDTO()
                            {
                                Id_Curriculo = 0,
                                Status = (int)Status.INSUCESSO_NAO
                            };
                            return dto;
                        }
                    }
                    else
                    {
                        dto = new OutLoginDTO()
                        {
                            Id_Curriculo = 0,
                            Status = (int)Status.JAFEITO
                        };
                        return dto;
                    }
                }

                dto = new OutLoginDTO()
                {
                    Id_Curriculo = 0,
                    Status = (int)Status.INSUCESSO_NAO
                };
            }
            catch (Exception ex)
            {
         
        EL.GerenciadorException.GravarExcecao(ex, "Login -> Cpf:" + login.Cpf + " Data_Nascimento:" + login.Data_Nascimento +  " UserCodFacebook:" + login.UserCodFacebook);
                dto = new OutLoginDTO()
                {
                    Id_Curriculo = 0,
                    Status = (int)Status.ERRO
                };
            }

            return dto;
        }
        #endregion

        #region TimeLine
        public OutTimeLineDTO TimeLine(InTimeLineDTO timeline)
        {
            OutTimeLineDTO dto = new OutTimeLineDTO();

            if (timeline.FiltroJaEnviei)
                CarregarTimelineJaEnviei(timeline, dto);

            if (timeline.FiltroMensagens)
                CarregarTimelineMensagem(timeline, dto);

            if (timeline.FiltroQuemMeViu)
                CarregarTimelineQuemMeViu(timeline, dto);

            if (timeline.FiltroVagas)
                CarregarTimelineVaga(timeline, dto);

            DateTime ultimaAtualizacao =
                DateTime.Parse(timeline.DataUltimaAtualizacao);

            int totalRegistros = dto.ListaDeEventos.Count;

            dto.ListaDeEventos =
                dto.ListaDeEventos
                    .Where(e => e.DataHora > ultimaAtualizacao)
                    .OrderByDescending(e => e.DataHora)
                    .Skip(10 * (timeline.Pagina - 1))
                    .Take(10)
                    .ToList();

            if (totalRegistros < 10 * timeline.Pagina && dto.Status == (int)Status.SUCESSO_SIM)
                dto.Status = (int)Status.ATUALIZADO; // Status.ATUALIZADO = 2 --> nao tem mais paginas a retornar

            return dto;
        }

        #region CarregarTimelineVaga
        private void CarregarTimelineVaga(InTimeLineDTO input, OutTimeLineDTO output)
        {
            if (output.Status == (int)Status.ERRO)
                return;

            try
            {
                int totalRegistros;

                Curriculo curriculo = Curriculo.LoadObject(input.Id_Curriculo);

                FuncaoPretendida funcaoPretendida;
                FuncaoPretendida.CarregarPorCurriculo(input.Id_Curriculo, out funcaoPretendida);
                funcaoPretendida.Funcao.CompleteObject();

                PesquisaVaga pesquisaVaga = new PesquisaVaga()
                {
                    Funcao = funcaoPretendida.Funcao,
                    Cidade = curriculo.CidadePretendida ?? curriculo.CidadeEndereco
                };

                DataTable dt = PesquisaVaga.BuscaVagaFullText(
                    pesquisaVaga,
                    Int32.MaxValue / 2,
                    1,
                    input.Id_Curriculo,
                    null,
                    curriculo.CidadePretendida != null ?
                        curriculo.CidadePretendida.IdCidade :
                        curriculo.CidadeEndereco.IdCidade,
                    String.Empty,
                    null,
                    null,
                    String.Empty,
                    null,
                    null,
                    OrdenacaoBuscaVaga.Padrao,
                    out totalRegistros);

                foreach (DataRow row in dt.Rows)
                {
                    TimelineVagaDTO item = new TimelineVagaDTO();

                    item.DataHora = Convert.ToDateTime(row["Dta_Abertura"]);
                    item.Cidade = BLL.Custom.Helper.FormatarCidade(row["Nme_Cidade"].ToString(), row["Sig_Estado"].ToString());
                    item.Funcao = row["Des_Funcao"].ToString();
                    item.Id_Vaga = Convert.ToInt32(row["Idf_Vaga"]);
                    item.QuantidadeDeVagas = Convert.ToInt32(row["Qtd_Vaga"]);
                    if (row["Vlr_Salario_Para"] == DBNull.Value)
                        item.SalarioMax = null;
                    else
                        item.SalarioMax = Convert.ToDecimal(row["Vlr_Salario_Para"])
                            .ToString("0.00", CultureInfo.InvariantCulture);
                    if (row["Vlr_Salario_De"] == DBNull.Value)
                        item.SalarioMin = null;
                    else
                        item.SalarioMin = Convert.ToDecimal(row["Vlr_Salario_De"])
                            .ToString("0.00", CultureInfo.InvariantCulture);
                    item.Descricao = row["Des_Atribuicoes"].ToString();
                    item.Status = Convert.ToInt32(row["Flg_Candidatou"]);
                    item.Url = row["Url_Vaga"].ToString();
                    item.Arquivada = Convert.ToBoolean(row["Flg_Vaga_Arquivada"]);
                    item.PCD = row["Idf_Deficiencia"] != DBNull.Value && Convert.ToInt32(row["Idf_Deficiencia"]) > 0 ? true : false;

                    #region Tipo de Vinculo
                    if (VagaTipoVinculo.PossuiVinculo(new Vaga(item.Id_Vaga), Enumeradores.TipoVinculo.Estágio))
                    {
                        item.Funcao = "Estágio para " + item.Funcao;
                    }
                    #endregion

                    OutPerguntasVagaDTO temp = PerguntasVaga(item.Id_Vaga);
                    item.Perguntas = temp.Status;

                    output.ListaDeEventos.Add(item);
                }

                output.Status = (int)Status.SUCESSO_SIM;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                output.Status = (int)Status.ERRO;
            }
        }
        #endregion

        #region CarregarTimelineQuemMeViu
        private void CarregarTimelineQuemMeViu(InTimeLineDTO input, OutTimeLineDTO output)
        {
            if (output.Status == (int)Status.ERRO)
                return;

            try
            {
                int totalRegistros;

                DataTable dt =
                    CurriculoQuemMeViu.RecuperarQuemMeViu(input.Id_Curriculo, 1, Int32.MaxValue / 2, out totalRegistros);

                foreach (DataRow row in dt.Rows)
                {
                    TimelineQuemMeViuDTO item = new TimelineQuemMeViuDTO();

                    Cidade cidade = Cidade.LoadObject(Convert.ToInt32(row["Idf_Cidade"]));
                    cidade.Estado.CompleteObject();

                    output.ListaDeEventos.Add(new TimelineQuemMeViuDTO()
                    {
                        DataHora = Convert.ToDateTime(row["Dta_Quem_Me_Viu"]),
                        Cidade = BLL.Custom.Helper.FormatarCidade(cidade.NomeCidade, cidade.Estado.SiglaEstado),
                        Id_Empresa = Convert.ToInt32(row["Idf_Filial"]),
                        NomeEmpresa = row["Raz_Social"].ToString()
                    });
                }

                output.Status = (int)Status.SUCESSO_SIM;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                output.Status = (int)Status.ERRO;
            }
        }
        #endregion

        #region CarregarTimelineMensagem
        private void CarregarTimelineMensagem(InTimeLineDTO input, OutTimeLineDTO output)
        {
            if (output.Status == (int)Status.ERRO)
                return;

            try
            {
                int totalRegistros;

                Curriculo curriculo = Curriculo.LoadObject(input.Id_Curriculo);

                UsuarioFilialPerfil perfil;
                UsuarioFilialPerfil.CarregarUsuarioFilialPerfilCandidatoAtivo(curriculo.PessoaFisica, out perfil);

                DataTable dt = MensagemCS.CarregarMensagensRecebidas(perfil.IdUsuarioFilialPerfil, 1, Int32.MaxValue / 2, null, true, out totalRegistros);

                foreach (DataRow row in dt.Rows)
                {
                    if (Convert.ToInt32(row["Idf_Tipo_Mensagem_CS"]) == 1)
                    {
                        output.ListaDeEventos.Add(new TimelineMensagemDTO()
                        {
                            DataHora = Convert.ToDateTime(row["Dta_Cadastro"]),
                            Assunto = row["Des_Email_Assunto"].ToString(),
                            Mensagem = row["Des_Mensagem"].ToString(),
                            Tipo = Convert.ToInt32(row["Idf_Tipo_Mensagem_CS"])
                        });
                    }
                }

                output.Status = (int)Status.SUCESSO_SIM;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                output.Status = (int)Status.ERRO;
            }
        }
        #endregion

        #region CarregarTimelineJaEnviei
        private void CarregarTimelineJaEnviei(InTimeLineDTO input, OutTimeLineDTO output)
        {
            if (output.Status == (int)Status.ERRO)
                return;

            try
            {
                int totalRegistros;

                DataTable dt =
                    VagaCandidato.CarregarVagaCandidatadaPorCurriculo(input.Id_Curriculo, false, 1, Int32.MaxValue / 2, out totalRegistros);

                foreach (DataRow row in dt.Rows)
                {
                    Funcao funcao = Funcao.LoadObject(Convert.ToInt32(row["Idf_Funcao"]));

                    output.ListaDeEventos.Add(new TimelineJaEnvieiDTO()
                    {
                        DataHora = Convert.ToDateTime(row["Dta_Cadastro"]),
                        Id_Empresa = Convert.ToInt32(row["Idf_Filial"]),
                        Funcao = funcao.DescricaoFuncao,
                        Id_Vaga = Convert.ToInt32(row["Idf_Vaga"]),
                        NomeEmpresa = row["Raz_Social"].ToString(),
                        Salario = row["Vlr_Salario"].ToString(),
                        Arquivada = Convert.ToBoolean(row["Flg_Vaga_Arquivada"]),
                        PCD = row["Idf_Deficiencia"] != DBNull.Value && Convert.ToInt32(row["Idf_Deficiencia"]) > 0 ? true : false
                    });
                }

                output.Status = (int)Status.SUCESSO_SIM;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                output.Status = (int)Status.ERRO;
            }
        }
        #endregion

        #endregion

        #region RegistraTokenAPNS
        public OutStatusDTO RegistraTokenAPNS(InTokenAPNSDTO registra)
        {
            try
            {
                MobileToken token = null;
                if (MobileToken.CarregarPorIdCurriculoToken(registra.Id_Curriculo, registra.IMEI, out token))
                {
                    token.CodigoToken = registra.TokenAPNS;
                    token.Save();
                    return new OutStatusDTO() { Status = (int)Status.SUCESSO_SIM };
                }

                token = new MobileToken();

                token.CodigoToken = registra.TokenAPNS;
                token.CodigoDispositivo = registra.IMEI;
                token.TipoSistemaMobile = new TipoSistemaMobile((int)Enumeradores.TipoSistemaMobile.iOS);
                token.Curriculo = Curriculo.LoadObject(registra.Id_Curriculo);
                token.Save();
                return new OutStatusDTO() { Status = (int)Status.SUCESSO_SIM };
            }
            catch (Exception ex)
            {

                EL.GerenciadorException.GravarExcecao(ex,
                    " RemoveTokenAPNS-> Id_Curriculo:" + registra.Id_Curriculo + "TokenAPNS:" + registra.TokenAPNS +
                    " IMEI:" +
                    registra.IMEI);
                return new OutStatusDTO() { Status = (int)Status.ERRO };
            }


        }
        #endregion

        #region RemoveTokenAPNS
        public OutStatusDTO RemoveTokenAPNS(InTokenAPNSDTO remove)
        {
            try
            {
                MobileToken token;
                if (!MobileToken.CarregarPorIdCurriculoToken(remove.Id_Curriculo, remove.IMEI, out token))
                    return new OutStatusDTO() {Status = (int) Status.INSUCESSO_NAO};
                MobileToken.Delete(token.IdMobileToken);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex,
                    " RemoveTokenAPNS-> Id_Curriculo:" + remove.Id_Curriculo + "TokenAPNS:" + remove.TokenAPNS +
                    " IMEI:" +
                    remove.IMEI);
                return new OutStatusDTO() {Status = (int) Status.ERRO};
            }

            return new OutStatusDTO() { Status = (int)Status.SUCESSO_SIM };
        }

        #endregion

        #region RegistraTokenGCM
        public OutStatusDTO RegistraTokenGCM(InTokenGCMDTO registra)
        {
            try
            {
                MobileToken token = null;
                if (MobileToken.CarregarPorIdCurriculoToken(registra.Id_Curriculo, registra.IMEI, out token))
                {
                    token.CodigoToken = registra.TokenGCM;
                    token.Save();
                    return new OutStatusDTO() { Status = (int)Status.SUCESSO_SIM };
                }

                token = new MobileToken();

                token.CodigoToken = registra.TokenGCM;
                token.CodigoDispositivo = registra.IMEI;
                token.TipoSistemaMobile = new TipoSistemaMobile((int)Enumeradores.TipoSistemaMobile.Android);
                token.Curriculo = Curriculo.LoadObject(registra.Id_Curriculo);
                token.Save();
                return new OutStatusDTO() { Status = (int)Status.SUCESSO_SIM };
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex,
                    " RegistraTokenGCM-> Id_Curriculo:" + registra.Id_Curriculo + "TokenGCM:" + registra.TokenGCM +
                    " IMEI:" +
                    registra.IMEI);
                return new OutStatusDTO() { Status = (int)Status.ERRO };
            }
        }
        #endregion

        #region RemoveTokenGCM
        public OutStatusDTO RemoveTokenGCM(InTokenGCMDTO remove)
        {
            try
            {
                MobileToken token;
                if (!MobileToken.CarregarPorIdCurriculoToken(remove.Id_Curriculo, remove.IMEI, out token))
                    return new OutStatusDTO() { Status = (int)Status.INSUCESSO_NAO };
                MobileToken.Delete(token.IdMobileToken);
            }
            catch (Exception ex)
            {

                EL.GerenciadorException.GravarExcecao(ex,
                    "RemoveTokenGCM -> Id_Curriculo:" + remove.Id_Curriculo + "TokenGCM:" + remove.TokenGCM + " IMEI:" +
                    remove.IMEI);
                return new OutStatusDTO() { Status = (int)Status.ERRO };
            }

            return new OutStatusDTO() { Status = (int)Status.SUCESSO_SIM };
        }
        #endregion

        #region LiberarVIP
        public OutStatusDTO LiberarVIP(int c, int p, string pt, string oid)
        {
            int idCurriculo = c;
            int idPlano = p;
            string purchaseToken = pt;
            string orderId = oid;
            try
            {
                //Log de requisições para liberar VIP
                EL.GerenciadorException.GravarExcecao(new Exception(), "Liberar VIP via APP Google Play -> c:" + c + " p:" + p + " oid:" + oid + " pt:" + pt);

                if (string.IsNullOrEmpty(pt))
                {
                    EL.GerenciadorException.GravarExcecao(new Exception(), "Liberar VIP via APP Google Play -> parametro pt null, não realizará liberação");
                    return new OutStatusDTO() { Status = (int)Status.INSUCESSO_NAO, erro = "Não existe um transação válida" };
                }

                if (ValidationTokenGoogleAPI.ValidationToken(purchaseToken, idPlano))
                {
                    Curriculo curriculo =
                    Curriculo.LoadObject(idCurriculo);

                    //FuncaoCategoria funcaoCategoria =
                    //    FuncaoCategoria.RecuperarCategoriaPorCurriculo(curriculo);

                    if (!PlanoAdquirido.ConcederPlanoPFViaMobile(curriculo, new Plano(idPlano), purchaseToken, orderId))
                    {
                        EL.GerenciadorException.GravarExcecao(new Exception(), "Liberar VIP via APP Google Play -> falha ao ConcederPlanoPFViaMobile, não realizará liberação ->  c:" + c + " p:" + p + " oid:" + oid + " pt:" + pt);
                        return new OutStatusDTO() { Status = (int)Status.INSUCESSO_NAO, erro = "Não existe um transação válida" };
                    }
                }
                else
                {
                    EL.GerenciadorException.GravarExcecao(new Exception(), "Liberar VIP via APP Google Play -> falha no ValidationToken, não realizará liberação -> c:" + c + " p:" + p + " oid:" + oid + " pt:" + pt);
                    return new OutStatusDTO() { Status = (int)Status.INSUCESSO_NAO, erro = "Não existe um transação válida" };
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Liberar VIP via APP Google Play -> erro inesperado, não realizará liberação ->  c:" + c + " p:" + p + " oid:" + oid + " pt:" + pt);
                return new OutStatusDTO() { Status = (int)Status.ERRO, erro = "Não existe um transação válida" };
            }

            //realizado com sucesso
            EL.GerenciadorException.GravarExcecao(new Exception(), "Liberar VIP via APP Google Play -> SUCESSO!!!");
            return new OutStatusDTO() { Status = (int)Status.SUCESSO_SIM };
        }
        #endregion

        #region ValidaNotificacao
        public OutStatusDTO ValidaNotificacao(InValidaNotificacaoDTO valida)
        {
            return new OutStatusDTO() { Status = (int)Status.SUCESSO_SIM };
        }
        #endregion

    }
}
