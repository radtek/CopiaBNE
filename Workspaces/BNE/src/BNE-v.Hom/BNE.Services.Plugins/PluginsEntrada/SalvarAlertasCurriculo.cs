using BNE.Services.AsyncServices.Plugins;
using System;
using System.Collections.Generic;
using BNE.Services.AsyncServices.Plugins.Interface;
using BNE.Services.Base.ProcessosAssincronos;
using System.ComponentModel.Composition;
using BNE.Services.Plugins.PluginResult;
using BNE.BLL;
using System.Data.SqlClient;

namespace BNE.Services.Plugins.PluginsEntrada
{
    [Export(typeof(IInputPlugin))]
    [ExportMetadata("Type", "SalvarAlertasCurriculo")]
    public class SalvarAlertasCurriculo : InputPlugin
    {
        public IPluginResult DoExecute(ParametroExecucaoCollection objParametros, Dictionary<string, byte[]> objAnexos)
        {
            return this.DoExecuteTask(objParametros, objAnexos);
        }

        protected override IPluginResult DoExecuteTask(ParametroExecucaoCollection objParametros, Dictionary<string, byte[]> objAnexos)
        {
            var idCurriculo = objParametros["idCurriculo"].ValorInt;
          
            try
            {
                Curriculo objCurriculo = Curriculo.LoadObject(idCurriculo.Value);
                using (SqlConnection conn = new SqlConnection(BLL.DataAccessLayer.CONN_NOTIFICACAO))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            //salvar na tabela Alerta Curriculos
                            List<FuncaoPretendida> listFuncoesPretendidas = FuncaoPretendida.CarregarFuncoesPretendidasPorCurriculo(objCurriculo);
                            BLL.Notificacao.AlertaCurriculos alertacv = new BLL.Notificacao.AlertaCurriculos();
                            objCurriculo.CidadeEndereco.CompleteObject();
                            alertacv.IdCurriculo = objCurriculo.IdCurriculo;
                            objCurriculo.PessoaFisica.CompleteObject();
                            alertacv.NomePessoa = objCurriculo.PessoaFisica.NomeCompleto;
                            alertacv.EmailPessoa = objCurriculo.PessoaFisica.EmailPessoa;
                            alertacv.FlagVIP = objCurriculo.FlagVIP;
                            alertacv.ValorPretensaoSalarial = objCurriculo.ValorPretensaoSalarial;
                            alertacv.DescricaoFuncao = listFuncoesPretendidas[0].Funcao.DescricaoFuncao;
                            alertacv.NomeCidade = objCurriculo.CidadeEndereco.NomeCidade;
                            alertacv.SiglaEstado = objCurriculo.CidadeEndereco.Estado.SiglaEstado;
                            alertacv.NumeroCPF = objCurriculo.PessoaFisica.CPF;
                            alertacv.DataNascimento = objCurriculo.PessoaFisica.DataNascimento;
                            if (objCurriculo.PessoaFisica.Deficiencia != null)
                                alertacv.IdDeficiencia = objCurriculo.PessoaFisica.Deficiencia.IdDeficiencia;
                            alertacv.Save(trans);

                            //alerta para todos os dias;
                            BLL.Notificacao.AlertaCurriculosAgenda.SalvarAlertaTodosOsDias(objCurriculo.IdCurriculo, trans);
                            //alerta Função
                            BLL.Notificacao.AlertaFuncoes alertaFuncao = new BLL.Notificacao.AlertaFuncoes();
                            alertaFuncao.IdFuncao = listFuncoesPretendidas[0].Funcao.IdFuncao;
                            alertaFuncao.DescricaoFuncao = listFuncoesPretendidas[0].Funcao.DescricaoFuncao;
                            alertaFuncao.AlertaCurriculos = alertacv;
                            alertaFuncao.Save(trans);

                            //alerta Cidade
                            BLL.Notificacao.AlertaCidades alertaCidades = new BLL.Notificacao.AlertaCidades();
                            alertaCidades.AlertaCurriculos = alertacv;
                            alertaCidades.IdCidade = objCurriculo.CidadeEndereco.IdCidade;
                            alertaCidades.NomeCidade = objCurriculo.CidadeEndereco.NomeCidade;
                            alertaCidades.SiglaEstado = objCurriculo.CidadeEndereco.Estado.SiglaEstado;
                            alertaCidades.Save(trans);

                            trans.Commit();

                        }
                        catch (Exception)
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, String.Format("Erro ao salvar alerta de vagas pra o curriculo {0}", idCurriculo.Value));
            }
            return new MensagemPlugin(this, true);
        }
    }
}
