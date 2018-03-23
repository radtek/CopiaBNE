using System;
using System.Collections.Generic;
using System.Web.UI;
using BNE.BLL;
using BNE.Web.Code;

namespace BNE.Web.UserControls.Modais
{
    public partial class ucAtendimentoVip : BaseUserControl
    {

        public void MostrarModal(int idCurriculo, int idUsuarioFilialPerfil)
        {
            //BNE_Plano_Motivo_Cancelamento
            litDescontoConcedido.Text = litMotivoCancelamento.Text = lblDataCancelamento.Text = litQuemCancelou.Text = string.Empty;

            percentDesconto.Value = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.LimiteDescontoVipPorcentagem);
            UIHelper.CarregarCheckBoxList(cblMotivoCancelar, BLL.MotivoCancelamento.ListarVIP());
            hdfCurriculo.Value = idCurriculo.ToString();

            BLL.PlanoAdquirido objPlanoAdquirido;


            if (BLL.PlanoAdquirido.CarregarPlanoAdquiridoPorSituacao(new Curriculo(idCurriculo),
               (int)BLL.Enumeradores.PlanoSituacao.Liberado, out objPlanoAdquirido))
            {
                string quemCancelou;
                litMotivoCancelamento.Text =  PlanoMotivoCancelamento.ConsultaMotivoCancelamentoUltimoPlano(objPlanoAdquirido.IdPlanoAdquirido, out quemCancelou); // CurriculoCorrecao.ConsultaMotivoExcluxaoCurriculo(idCurriculo);

                objPlanoAdquirido.Plano.CompleteObject();

                lblValorPlano.Text = objPlanoAdquirido.Plano.ValorBase.ToString();
                lblTitulo.Text = objPlanoAdquirido.Plano.DescricaoPlano;

                if (!objPlanoAdquirido.FlagRecorrente && objPlanoAdquirido.DataCancelamento.HasValue)
                    lnkCancelarPlano.Enabled = lnkDesconto.Enabled = false;
                else
                    lnkCancelarPlano.Enabled = lnkDesconto.Enabled = true;

                hdfPlanoAd.Value = objPlanoAdquirido.IdPlanoAdquirido.ToString();
                if (objPlanoAdquirido.DataCancelamento.HasValue)
                {
                    lblDataCancelamento.Text = objPlanoAdquirido.DataCancelamento.Value.ToString("dd/MM/yyyy HH:mm");
                    litQuemCancelou.Text = quemCancelou;
                    pnlVipCancelado.Visible = true;
                    lnkDesconto.Enabled = lnkCancelarPlano.Enabled = false;
                }

                //Verificar se ja tem desconto.
                #region [Verificar se tem Desconto]
                BLL.PlanoDesconto objPlanoDesconto;
                string UsuarioInterno;
                if (PlanoDesconto.RecuperaDesconto(objPlanoAdquirido.IdPlanoAdquirido, out objPlanoDesconto, out UsuarioInterno))
                {
                    //só pode editar o desconto
               
                    litDescontoConcedido.Text = $"Desconto de {objPlanoDesconto.ValorDesconto} concedido por {UsuarioInterno} em {objPlanoDesconto.DataCadastro.ToString("dd/MM/yyyy hh:mm")}";
                    pnlDesconto.Visible = true;
                }
                #endregion

                UIHelper.CarregarRadGrid(gvPagamentos, Pagamento.HistoricoPagamentoRecorrenciaPF(objPlanoAdquirido.IdPlanoAdquirido));
            }


            mpeAtendimentoVip.Show();

            ScriptManager.RegisterStartupScript(upAtendimentoVip, upAtendimentoVip.GetType(), "aplicar mascara", "ChangeDes();", true);

            upAtendimentoVip.Update();
 


        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lnkCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                var objCurriculo = Curriculo.LoadObject(Convert.ToInt32(hdfCurriculo.Value));
                BLL.PlanoAdquirido objPlanoAdquirido;

                if (objCurriculo.VIP() && BLL.PlanoAdquirido.CarregarPlanoAdquiridoPorSituacao(objCurriculo, (int)BLL.Enumeradores.PlanoSituacao.Liberado, out objPlanoAdquirido))
                {
                    List<PlanoMotivoCancelamento> motivo = new List<PlanoMotivoCancelamento>();
                    for (int i = 0; i < cblMotivoCancelar.Items.Count; i++)
                    {
                        if (cblMotivoCancelar.Items[i].Selected)
                        {
                            PlanoMotivoCancelamento objPlanoMotivoBloqueio = new PlanoMotivoCancelamento();
                            objPlanoMotivoBloqueio.IdPlanoAdquirido = objPlanoAdquirido.IdPlanoAdquirido;
                            objPlanoMotivoBloqueio.IdMotivoCancelamento = Convert.ToInt32(cblMotivoCancelar.Items[i].Value);
                            objPlanoMotivoBloqueio.DescricaoDetalheMotivoCancelamento = Convert.ToInt32(cblMotivoCancelar.Items[i].Value).Equals((int)BLL.Enumeradores.MotivoCancelamento.OutrosVIP) ? txtOutro.Text : null;
                            objPlanoMotivoBloqueio.FlgInativo = false;// ativar quando for cancelado pelo adm
                            motivo.Add(objPlanoMotivoBloqueio);
                        }
                    }

                    PlanoAdquirido.CancelarAssinaturaPlanoRecorrente(objPlanoAdquirido.IdPlanoAdquirido, base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value, motivo);

                    objCurriculo.PessoaFisica.CompleteObject();
                    objPlanoAdquirido.CompleteObject();
                    lblDataCancelamento.Text = objPlanoAdquirido.DataCancelamento.Value.ToString("dd/MM/yyyy HH:mm");
                    CurriculoObservacao.SalvarCRM("Plano vip cancelado", objCurriculo, "Cancelamento de plano pelo administrador");
                }

               
                pnlVipCancelado.Visible = true;

                upAtendimentoVip.Update();

            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Erro ao cancelar o plano recorrente pela pela de adm");
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "aplicar mascara", "ChangeDes();", true);


        }

        protected void btiFechar_Click(object sender, ImageClickEventArgs e)
        {
            mpeAtendimentoVip.Hide();
        }

        protected void linkDesconto_Click(object sender, EventArgs e)
        {
            try
            {
                //conceder desconto na proxima parcela.
                var objPlanoDesconto = new PlanoDesconto();
                string usuarioInterno;
                PlanoDesconto.RecuperaDesconto(Convert.ToInt32(hdfPlanoAd.Value), out objPlanoDesconto, out usuarioInterno);

                var objUsuarioFilialPerfil = new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value);
                objPlanoDesconto.PlanoAdquirido = new PlanoAdquirido(Convert.ToInt32(hdfPlanoAd.Value));
                objPlanoDesconto.UsuarioFilialPerfil = objUsuarioFilialPerfil;
                objPlanoDesconto.DataAtualizacao = DateTime.Now;
                var valorDesconto = Convert.ToDecimal(lblValorPlano.Text) - Convert.ToDecimal(hdfValorComDesconto.Value);
                objPlanoDesconto.ValorDesconto = valorDesconto;

                objPlanoDesconto.Save();
                
                litDescontoConcedido.Text = $"Desconto de {objPlanoDesconto.ValorDesconto} concedido por {new PessoaFisica(base.IdPessoaFisicaLogada.Value).RecuperarNomePessoa()} em {(objPlanoDesconto.DataAtualizacao != null ? objPlanoDesconto.DataAtualizacao.Value.ToString("dd/MM/yyyy hh:mm") : objPlanoDesconto.DataCadastro.ToString("dd/MM/yyyy hh:mm"))}";
                pnlDesconto.Visible = true;
                upAtendimentoVip.Update();
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Erro ao dar desconto no plano VIP");
            }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "aplicar mascara", "ChangeDes();", true);

        }

    }
}