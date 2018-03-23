//-- Data: 01/08/2016 11:52
//-- Autor: Mailson

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
namespace BNE.BLL
{
    public partial class LogAlteracaoVaga // Tabela: BNE_Log_Alteracao_Vaga
    {

        #region [CompararVagas]

        public static void CompararVagas(Vaga vaga, int? idfUsuarioFilialPerfil, Enumeradores.VagaLog? Processo, SqlTransaction trans = null)
        {
            try
            {
            var objVaga = Vaga.LoadObject(vaga.IdVaga);
            StringBuilder alteracao = new StringBuilder();
            
            if (!(!String.IsNullOrEmpty(objVaga.DescricaoTiposVinculo) ? objVaga.DescricaoTiposVinculo : string.Empty).Equals(!string.IsNullOrEmpty(vaga.DescricaoTiposVinculo) ? vaga.DescricaoTiposVinculo : string.Empty))
                alteracao.Append(String.Format("<br><b>  Tipo Vinculo - De:</b>{0} <b>Para:</b> {1} ", objVaga.DescricaoTiposVinculo, vaga.DescricaoTiposVinculo));
            
                if(objVaga.Funcao != null && vaga.Funcao != null){
                    if(!objVaga.Funcao.IdFuncao.Equals(vaga.Funcao.IdFuncao))
                        alteracao.Append(String.Format("<br><b> Função - De:</b>{0} <b>Para:</b> {1} ", objVaga.Funcao.IdFuncao, vaga.Funcao.IdFuncao));
                    }
                else if(objVaga.Funcao != null && vaga.Funcao == null)
                     alteracao.Append(String.Format("<br><b> Função - De:</b>{0} <b>Para:</b> {1} ", objVaga.Funcao.IdFuncao, "null"));
                else if(objVaga.Funcao == null && vaga.Funcao !=null)
                     alteracao.Append(String.Format("<br><b> Função - De:</b>{0} <b>Para:</b> {1} ", "null", vaga.Funcao.IdFuncao));
            
                if(objVaga.Cidade != null && vaga.Cidade != null){
                    if(!objVaga.Cidade.IdCidade.Equals(vaga.Cidade.IdCidade))
                         alteracao.Append(String.Format("<br><b>  Cidade - De:</b>{0} <b>Para:</b> {1} ", objVaga.Cidade.IdCidade, vaga.Cidade.IdCidade));
                }
                else if(objVaga.Cidade != null && vaga.Cidade == null)
                     alteracao.Append(String.Format("<br><b>  Cidade - De:</b>{0} <b>Para:</b> {1} ", objVaga.Cidade.IdCidade, "null"));
                else if(objVaga.Cidade == null && vaga.Cidade != null)
                     alteracao.Append(String.Format("<br><b>  Cidade - De:</b>{0} <b>Para:</b> {1} ", "null", vaga.Cidade.IdCidade));
            
            if(!objVaga.IdBairro.Equals(vaga.IdBairro))
                alteracao.Append(String.Format("<br><b>  Id Bairro - De:</b>{0} <b>Para:</b> {1} ", objVaga.IdBairro, vaga.IdBairro));

            if(!(!String.IsNullOrEmpty(objVaga.NomeBairro) ? objVaga.NomeBairro : string.Empty).Equals(!String.IsNullOrEmpty(vaga.NomeBairro) ? vaga.NomeBairro : string.Empty))
                alteracao.Append(String.Format("<br><b>  Nome Bairro - De:</b>{0} <b>Para:</b> {1} ", objVaga.NomeBairro, vaga.NomeBairro));

            if(!objVaga.QuantidadeVaga.Equals(vaga.QuantidadeVaga))
                alteracao.Append(String.Format("<br><b>  Número de Vagas - De:</b>{0} <b>Para:</b> {1} ", objVaga.QuantidadeVaga, vaga.QuantidadeVaga));

            if (objVaga.Escolaridade != null && vaga.Escolaridade != null)
            {
                if (!objVaga.Escolaridade.IdEscolaridade.Equals(vaga.Escolaridade.IdEscolaridade))
                    alteracao.Append(String.Format("<br><b>  Escolaridade - De:</b>{0} <b>Para:</b> {1} ", objVaga.Escolaridade.IdEscolaridade, vaga.Escolaridade.IdEscolaridade));
            }
            else if (objVaga.Escolaridade != null && vaga.Escolaridade ==null)
                alteracao.Append(String.Format("<br><b>  Escolaridade - De:</b>{0} <b>Para:</b> {1} ", objVaga.Escolaridade.IdEscolaridade, "null"));
            else if (vaga.Escolaridade != null && objVaga.Escolaridade ==null)
                alteracao.Append(String.Format("<br><b>  Escolaridade - De:</b>{0} <b>Para:</b> {1} ", "null", vaga.Escolaridade.IdEscolaridade));

            if(!objVaga.ValorSalarioDe.Equals(vaga.ValorSalarioDe))
                alteracao.Append(String.Format("<br><b>  Salario de - De:</b>{0} <b>Para:</b> {1} ", objVaga.ValorSalarioPara, vaga.ValorSalarioDe));
            if(!objVaga.ValorSalarioPara.Equals(vaga.ValorSalarioPara))
                alteracao.Append(String.Format("<br><b>  Salario Para - De:</b>{0} <b>Para:</b> {1} ", objVaga.ValorSalarioPara, vaga.ValorSalarioPara));

            if(!(!String.IsNullOrEmpty(objVaga.DescricaoBeneficio) ? objVaga.DescricaoBeneficio : string.Empty).Equals(!String.IsNullOrEmpty(vaga.DescricaoBeneficio) ? vaga.DescricaoBeneficio : string.Empty))
                alteracao.Append(String.Format("<br><b>  Benefícios - De:</b>{0} <b>Para:</b> {1} ", objVaga.DescricaoBeneficio, vaga.DescricaoBeneficio));

            if(!objVaga.NumeroIdadeMinima.Equals(vaga.NumeroIdadeMinima))
                alteracao.Append(String.Format("<br><b>  Idade Minima - De:</b>{0} <b>Para:</b> {1} ", objVaga.NumeroIdadeMaxima, vaga.NumeroIdadeMinima));
            if(!objVaga.NumeroIdadeMaxima.Equals(vaga.NumeroIdadeMaxima))
                alteracao.Append(String.Format("<br><b>  Idade Máxima - De:</b>{0} <b>Para:</b> {1} ", objVaga.NumeroIdadeMaxima, vaga.NumeroIdadeMaxima));

            if (objVaga.Sexo != null && vaga.Sexo != null)
            {
                if (!objVaga.Sexo.IdSexo.Equals(vaga.Sexo.IdSexo))
                    alteracao.Append(String.Format("<br><b>  Sexo - De:</b>{0} <b>Para:</b> {1} ", objVaga.Sexo.IdSexo, vaga.Sexo.IdSexo));
            }
            else if (objVaga.Sexo != null && vaga.Sexo ==null)
                alteracao.Append(String.Format("<br><b>  Sexo - De:</b>{0} <b>Para:</b> {1} ", objVaga.Sexo.IdSexo, "null"));
            else if (vaga.Sexo != null && objVaga.Sexo ==null)
                alteracao.Append(String.Format("<br><b>  Sexo - De:</b>{0} <b>Para:</b> {1} ", "null", vaga.Sexo.IdSexo));

            if(!(!String.IsNullOrEmpty(objVaga.DescricaoRequisito) ? objVaga.DescricaoRequisito : string.Empty).Equals(!String.IsNullOrEmpty(vaga.DescricaoRequisito) ? vaga.DescricaoRequisito : string.Empty))
                alteracao.Append(String.Format("<br><b>  Requisitos - De:</b>{0} <b>Para:</b> {1} ", objVaga.DescricaoRequisito, vaga.DescricaoRequisito));

            if(!(!String.IsNullOrEmpty(objVaga.DescricaoAtribuicoes) ? objVaga.DescricaoAtribuicoes : string.Empty).Equals(!String.IsNullOrEmpty(vaga.DescricaoAtribuicoes) ? vaga.DescricaoAtribuicoes : string.Empty))
                alteracao.Append(String.Format("<br><b>  Atribuições -De:</b>{0} <b>Para:</b> {1} ", objVaga.DescricaoAtribuicoes, vaga.DescricaoAtribuicoes));
            if(!(!String.IsNullOrEmpty(objVaga.DescricaoDisponibilidades) ? objVaga.DescricaoDisponibilidades : string.Empty).Equals(!String.IsNullOrEmpty(vaga.DescricaoDisponibilidades) ? vaga.DescricaoDisponibilidades : string.Empty))
                alteracao.Append(String.Format("<br><b>  Disponibilidade de trabalho - De:</b>{0} <b>Para:</b> {1} ", objVaga.DescricaoDisponibilidades, vaga.DescricaoDisponibilidades));
            if(!objVaga.FlagDeficiencia.Equals(vaga.FlagDeficiencia))
                alteracao.Append(String.Format("<br><b>  Flag Deficiencia - De:</b>{0} <b>Para:</b> {1} ", objVaga.FlagDeficiencia, vaga.FlagDeficiencia));

            if(objVaga.Deficiencia != null && vaga.Deficiencia != null){
                if(!objVaga.Deficiencia.IdDeficiencia.Equals(vaga.Deficiencia.IdDeficiencia))
                alteracao.Append(String.Format("<br><b>  Id Deficiencia - De:</b>{0} <b>Para:</b> {1} ", objVaga.Deficiencia.IdDeficiencia, vaga.Deficiencia.IdDeficiencia));
            }
            else if(objVaga.Deficiencia !=null && vaga.Deficiencia == null)
                alteracao.Append(String.Format("<br><b>  Id Deficiencia - De:</b>{0} <b>Para:</b> {1} ", objVaga.Deficiencia.IdDeficiencia, "null"));
            else if(objVaga.Deficiencia == null && vaga.Deficiencia !=null)
                alteracao.Append(String.Format("<br><b>  Id Deficiencia - De:</b>{0} <b>Para:</b> {1} ", "null", vaga.Deficiencia.IdDeficiencia));
            
            if(!(!String.IsNullOrEmpty(objVaga.NomeEmpresa) ? objVaga.NomeEmpresa : string.Empty).Equals(!String.IsNullOrEmpty(vaga.NomeEmpresa) ? vaga.NomeEmpresa : string.Empty))
                alteracao.Append(String.Format("<br><b>  Nome Empresa - De:</b>{0} <b>Para:</b> {1} ", objVaga.NomeEmpresa, vaga.NomeEmpresa));
            if(!(!String.IsNullOrEmpty(objVaga.NumeroDDD) ? objVaga.NumeroDDD : string.Empty).Equals(!String.IsNullOrEmpty(vaga.NumeroDDD) ? vaga.NumeroDDD : string.Empty))
                alteracao.Append(String.Format("<br><b>  DDD - De: </b>{0} <b>Para: </b> {1} ", objVaga.NumeroDDD, vaga.NumeroDDD));
            if(!(!String.IsNullOrEmpty(objVaga.NumeroTelefone) ? objVaga.NumeroTelefone : string.Empty).Equals(!String.IsNullOrEmpty(vaga.NumeroTelefone) ? vaga.NumeroTelefone : string.Empty))
                alteracao.Append(String.Format("<br><b>  Telefone - De: </b>{0} <b>Para: </b> {1} ", objVaga.NumeroTelefone, vaga.NumeroTelefone));
            if (!objVaga.FlagConfidencial.Equals(vaga.FlagConfidencial))
                alteracao.Append(String.Format("<br><b>  Confidencial - De: </b>{0} <b>Para: </b>  ", objVaga.FlagConfidencial, vaga.FlagConfidencial));
            if (!objVaga.EmailVaga.Equals(vaga.EmailVaga))
                alteracao.Append(String.Format("<br><b>  Email - De:</b>{0} <b>Para:</b> {1} ", objVaga.EmailVaga, vaga.EmailVaga));
            if(!objVaga.FlagReceberCadaCV.Equals(vaga.FlagReceberCadaCV))
                alteracao.Append(String.Format("<br><b>  Receber E-mail de Cada Candidatura - De:</b>{0} <b>Para:</b> {1} ", objVaga.FlagReceberCadaCV, vaga.FlagReceberCadaCV));
            if(!objVaga.FlagReceberTodosCV.Equals(vaga.FlagReceberTodosCV))
                alteracao.Append(String.Format("<br><b>  Receber E-mail Diário Com as Candidatura - De:</b>{0} <b>Para:</b> {1} ", objVaga.FlagReceberTodosCV, vaga.FlagReceberTodosCV));
            if(!objVaga.FlagInativo.Equals(vaga.FlagInativo))
                alteracao.Append(String.Format("<br><b>  Flag Inativo - De:</b>{0} <b>Para:</b> {1} ", objVaga.FlagInativo, vaga.FlagInativo));
            if(!objVaga.FlagVagaArquivada.Equals(vaga.FlagVagaArquivada))
                alteracao.Append(String.Format("<br><b>  Flag Vaga Arquivada - De:</b>{0} <b>Para:</b> {1} ", objVaga.FlagVagaArquivada, vaga.FlagVagaArquivada));
            if(!objVaga.FlagAuditada.Equals(vaga.FlagAuditada))
                alteracao.Append(String.Format("<br><b>  Flag Auditada - De:</b>{0} <b>Para:</b> {1}", objVaga.FlagAuditada, vaga.FlagAuditada));
            if(!objVaga.FlagBNERecomenda.Equals(vaga.FlagBNERecomenda))
                alteracao.Append(String.Format("<br><b>  Flag BNE Recomenda - De:</b>{0} <b>Para:</b> {1} ", objVaga.FlagBNERecomenda, vaga.FlagBNERecomenda));
            if(!objVaga.FlagEmpresaEmAuditoria.Equals(vaga.FlagEmpresaEmAuditoria))
                alteracao.Append(String.Format("<br><b>  Flag Empresa em Auditoria - De:</b>{0} <b>Para:</b> {1} ", objVaga.FlagEmpresaEmAuditoria, vaga.FlagEmpresaEmAuditoria));
            if(!objVaga.FlagLiberada.Equals(vaga.FlagLiberada))
                alteracao.Append(String.Format("<br><b>  Flag Liberada - De:</b>{0} <b>Para:</b> {1} ", objVaga.FlagLiberada, vaga.FlagLiberada));
            if(!objVaga.FlagVagaMassa.Equals(vaga.FlagVagaMassa))
                alteracao.Append(String.Format("<br><b>  Flag Vaga Massa - De:</b>{0} <b>Para:</b> {1} ", objVaga.FlagVagaMassa, vaga.FlagVagaMassa));
            if(!objVaga.FlagVagaRapida.Equals(vaga.FlagVagaRapida))
                alteracao.Append(String.Format("<br><b>  Flag Vaga Rapida - De:</b>{0} <b>Para:</b> {1} ", objVaga.FlagVagaRapida, vaga.FlagVagaRapida));
            

            if(!StringBuilder.Equals(alteracao.ToString(),string.Empty))
                SalvarLog(objVaga, alteracao.ToString(), idfUsuarioFilialPerfil, Processo, trans);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Salvar log de alteração de vaga no CompararVagas");
            }
           
            
        }

        #endregion

        #region [CompararPerguntas]
        public static void CompararPerguntas(VagaPergunta vagaPergunta, int? idfUsuarioFilialPerfil, Enumeradores.VagaLog? Processo, SqlTransaction trans = null)
        {
            try
            {
                StringBuilder alteracao = new StringBuilder();
                VagaPergunta objVagaPergunta = new VagaPergunta(vagaPergunta.IdVagaPergunta);
                if (!objVagaPergunta.TipoResposta.Equals(vagaPergunta.TipoResposta))
                    alteracao.Append(String.Format("<br>Tipo Resposta<b>  - De:</b>{0} <b>Para:</b> {1} ", objVagaPergunta.TipoResposta, vagaPergunta.TipoResposta));
                if (!objVagaPergunta.DescricaoVagaPergunta.Equals(vagaPergunta.DescricaoVagaPergunta))
                    alteracao.Append(String.Format("<br>Descrição Pergunta<b>  - De:</b>{0} <b>Para:</b> {1} ", objVagaPergunta.DescricaoVagaPergunta, vagaPergunta.DescricaoVagaPergunta));
                if (!objVagaPergunta.FlagResposta.Equals(vagaPergunta.FlagResposta))
                    alteracao.Append(String.Format("<br>Flag Resposta<b>  - De:</b>{0} <b>Para:</b> {1} ", objVagaPergunta.FlagResposta, vagaPergunta.FlagResposta));
                if (!objVagaPergunta.Flaginativo.Equals(vagaPergunta.Flaginativo))
                    alteracao.Append(String.Format("<br>Flag Inativo<b>  - De:</b>{0} <b>Para:</b> {1} ", objVagaPergunta.Flaginativo, vagaPergunta.Flaginativo));

                if (!String.IsNullOrEmpty(alteracao.ToString()))
                    SalvarLog(objVagaPergunta.Vaga, alteracao.ToString(), idfUsuarioFilialPerfil, Processo, trans);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "CompararPerguntas");
            }
        }
        #endregion

        #region [SalvarLog]
        private static void SalvarLog(Vaga vaga, string alteracao, int? idfUsuarioFilialPerfil, Enumeradores.VagaLog? Processo, SqlTransaction trans = null)
        {
            LogAlteracaoVaga objLog = new LogAlteracaoVaga();
            objLog.DescricaoAlteracao = alteracao;
            objLog.Vaga = vaga;
            if (idfUsuarioFilialPerfil.HasValue)
                objLog.UsuarioFilialPerfil = new UsuarioFilialPerfil(idfUsuarioFilialPerfil.Value);
            if (Processo.HasValue)
                objLog.NomeServico = Processo.Value.ToString();
            if (trans != null)
                objLog.Save(trans);
            else
                objLog.Save();
        }
        #endregion
    }
}