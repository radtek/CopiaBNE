using BNE.BLL;
using BNE.Web.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BNE.Web.UserControls.Modais
{
    public partial class ModalDeficiencia : BaseUserControl
    {
        #region Variavel

        #region CheckAll
        private const string checkAll = @"function checkAll() {
                                check($('#cphConteudo_ucDeficiencia_cblFisica').find('input'));
                                check($('#cphConteudo_ucDeficiencia_cblAuditiva').find('input'));
                                check($('#cphConteudo_ucDeficiencia_cblVisual').find('input'));
                                check($('#cphConteudo_ucDeficiencia_cblMental').find('input'));
                            }

                            function check(cbl) {
                                for (var i = 0; i < cbl.length; i++) {
                                    if ($('#cphConteudo_ucDeficiencia_cbAll')[0].checked)
                                        cbl[i].checked = true;
                                    else
                                        cbl[i].checked = false;
                                }
                            }";
        #endregion
      
        #endregion

        #region Eventos

            #region PageLoad
            protected void Page_Load(object sender, EventArgs e)
            {
                  if (!Page.IsPostBack)
                      PreencherCheckBox();

                  ScriptManager.RegisterStartupScript(upConteudo, GetType(), "Select", checkAll, true);

            }
            #endregion

            #region DeletarAllDeficienciaUsuario
            public void DeletarAllDeficienciaUsuario(int idPessoaFisica)
            {
                //se ja tiver deficiencia e na edição optou por não possuir deficiencia apaga todas.
                if(!String.IsNullOrEmpty(hdDeficiencias.Value))
                    PessoaFisicaDeficiencia.DeleteAll(idPessoaFisica);
            }
                
            #endregion

            #region DeletarAllDeficienciaVaga
            public void DeletarAllDeficienciaVaga(int idVaga)
            {
              if (!String.IsNullOrEmpty(hdDeficiencias.Value))
                VagaDeficiencia.DeleteAll(idVaga);
            }
            #endregion

            #region SalvarPesquisaVaga
            public void SalvarPesquisaVaga(int idPesquisa)
            {
                try
                {
                    string[] salvo = hdDeficiencias.Value.Split(',');
                    List<string> sub = new List<string>();
                    for (int i = 0; i < salvo.Count(); i++)
                    {
                        sub.Add(salvo[i]);
                    }
                    List<int> lstDeletar = new List<int>();
                    DeficienciaDetalhe oDefDetalhe;

                    #region cblFisica
                    for (int i = 0; i < cblFisica.Items.Count; i++)
                    {
                        if (cblFisica.Items[i].Selected && !sub.Contains(cblFisica.Items[i].Value))
                        {
                            BLL.PesquisaVagaDeficiencia oPesquisaDef = new BLL.PesquisaVagaDeficiencia();
                            oPesquisaDef.PesquisaVaga = new BLL.PesquisaVaga(idPesquisa);
                            oDefDetalhe = DeficienciaDetalhe.LoadObject(Convert.ToInt32(cblFisica.Items[i].Value));
                            oPesquisaDef.DeficienciaDetalhe = oDefDetalhe;
                            oPesquisaDef.Deficiencia = oDefDetalhe.Deficiencia;
                            oPesquisaDef.Save();
                        }
                        else if (!cblFisica.Items[i].Selected && sub.Contains(cblFisica.Items[i].Value))
                            lstDeletar.Add(Convert.ToInt32(cblFisica.Items[i].Value));
                    }
                    #endregion

                    #region cblAuditiva
                    for (int i = 0; i < cblAuditiva.Items.Count; i++)
                    {

                        if (cblAuditiva.Items[i].Selected && !sub.Contains(cblAuditiva.Items[i].Value))
                        {
                            BLL.PesquisaVagaDeficiencia oPesquisaDef = new BLL.PesquisaVagaDeficiencia();
                            oPesquisaDef.PesquisaVaga = new BLL.PesquisaVaga(idPesquisa);
                            oDefDetalhe = DeficienciaDetalhe.LoadObject(Convert.ToInt32(cblAuditiva.Items[i].Value));
                            oPesquisaDef.DeficienciaDetalhe = oDefDetalhe;
                            oPesquisaDef.Deficiencia = oDefDetalhe.Deficiencia;
                            oPesquisaDef.Save();
                        }
                        else if (!cblAuditiva.Items[i].Selected && sub.Contains(cblAuditiva.Items[i].Value))
                            lstDeletar.Add(Convert.ToInt32(cblAuditiva.Items[i].Value));
                    }
                    #endregion

                    #region cblVisual
                    for (int i = 0; i < cblVisual.Items.Count; i++)
                    {

                        if (cblVisual.Items[i].Selected && !sub.Contains(cblVisual.Items[i].Value))
                        {
                            BLL.PesquisaVagaDeficiencia oPesquisaDef = new BLL.PesquisaVagaDeficiencia();
                            oPesquisaDef.PesquisaVaga = new BLL.PesquisaVaga(idPesquisa);
                            oDefDetalhe = DeficienciaDetalhe.LoadObject(Convert.ToInt32(cblVisual.Items[i].Value));
                            oPesquisaDef.DeficienciaDetalhe = oDefDetalhe;
                            oPesquisaDef.Deficiencia = oDefDetalhe.Deficiencia;
                            oPesquisaDef.Save();
                        }
                        else if (!cblVisual.Items[i].Selected && sub.Contains(cblVisual.Items[i].Value))
                            lstDeletar.Add(Convert.ToInt32(cblVisual.Items[i].Value));
                    }
                    #endregion

                    #region cblMental
                    for (int i = 0; i < cblMental.Items.Count; i++)
                    {

                        if (cblMental.Items[i].Selected && !sub.Contains(cblMental.Items[i].Value))
                        {
                            BLL.PesquisaVagaDeficiencia oPesquisaDef = new BLL.PesquisaVagaDeficiencia();
                            oPesquisaDef.PesquisaVaga = new BLL.PesquisaVaga(idPesquisa);
                            oDefDetalhe = DeficienciaDetalhe.LoadObject(Convert.ToInt32(cblMental.Items[i].Value));
                            oPesquisaDef.DeficienciaDetalhe = oDefDetalhe;
                            oPesquisaDef.Deficiencia = oDefDetalhe.Deficiencia;
                            oPesquisaDef.Save();
                        }
                        else if (!cblMental.Items[i].Selected && sub.Contains(cblMental.Items[i].Value))
                            lstDeletar.Add(Convert.ToInt32(cblMental.Items[i].Value));
                    }
                    #endregion

                    //if (lstDeletar.Count > 0)
                    //    PessoaFisicaDeficiencia.Delete(lstDeletar, base.IdPessoaFisicaLogada.Value);
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex);
                }
                finally
                {
                    mpeDeficiencia.Hide();
                }
            }
            #endregion

            #region SalvarUsuario
            public void SalvarUsuario(int idPessoaFisica)
                {
                    try
                    {
                        string[] salvo = hdDeficiencias.Value.Split(',');
                        List<string> sub = new List<string>();
                        for(int i = 0; i < salvo.Count(); i++){
                            sub.Add(salvo[i]);
                        }
                        List<int> lstDeletar = new List<int>();
                        DeficienciaDetalhe oDefDetalhe;

                        #region cblFisica
                            for (int i = 0; i < cblFisica.Items.Count; i++)
                            {

                                if (cblFisica.Items[i].Selected && !sub.Contains(cblFisica.Items[i].Value))
                                {
                                    PessoaFisicaDeficiencia oPessoaDef = new PessoaFisicaDeficiencia();
                                    oPessoaDef.PessoaFisica = new PessoaFisica(idPessoaFisica);
                                    oDefDetalhe = DeficienciaDetalhe.LoadObject(Convert.ToInt32(cblFisica.Items[i].Value));
                                    oPessoaDef.DeficienciaDetalhe = oDefDetalhe;
                                    oPessoaDef.Deficiencia = oDefDetalhe.Deficiencia;
                                    oPessoaDef.Save();
                                }
                                else if (!cblFisica.Items[i].Selected && sub.Contains(cblFisica.Items[i].Value))
                                    lstDeletar.Add(Convert.ToInt32(cblFisica.Items[i].Value));
                            }
                        #endregion

                        #region cblAuditiva
                            for (int i = 0; i < cblAuditiva.Items.Count; i++)
                        {

                            if (cblAuditiva.Items[i].Selected && !sub.Contains(cblAuditiva.Items[i].Value))
                            {
                                PessoaFisicaDeficiencia oPessoaDef = new PessoaFisicaDeficiencia();
                                oPessoaDef.PessoaFisica = new PessoaFisica(idPessoaFisica);
                                oDefDetalhe = DeficienciaDetalhe.LoadObject(Convert.ToInt32(cblAuditiva.Items[i].Value));
                                oPessoaDef.DeficienciaDetalhe = oDefDetalhe;
                                oPessoaDef.Deficiencia = oDefDetalhe.Deficiencia;
                                oPessoaDef.Save();
                            }
                            else if (!cblAuditiva.Items[i].Selected && sub.Contains(cblAuditiva.Items[i].Value))
                                lstDeletar.Add(Convert.ToInt32(cblAuditiva.Items[i].Value));
                        }
                        #endregion

                        #region cblVisual
                            for (int i = 0; i < cblVisual.Items.Count; i++)
                        {

                            if (cblVisual.Items[i].Selected && !sub.Contains(cblVisual.Items[i].Value))
                            {
                                PessoaFisicaDeficiencia oPessoaDef = new PessoaFisicaDeficiencia();
                                oPessoaDef.PessoaFisica = new PessoaFisica(idPessoaFisica);
                                oDefDetalhe = DeficienciaDetalhe.LoadObject(Convert.ToInt32(cblVisual.Items[i].Value));
                                oPessoaDef.DeficienciaDetalhe = oDefDetalhe;
                                oPessoaDef.Deficiencia = oDefDetalhe.Deficiencia;
                                oPessoaDef.Save();
                            }
                            else if (!cblVisual.Items[i].Selected && sub.Contains(cblVisual.Items[i].Value))
                                lstDeletar.Add(Convert.ToInt32(cblVisual.Items[i].Value));
                        }
                        #endregion

                        #region cblMental
                            for (int i = 0; i < cblMental.Items.Count; i++)
                            {

                                if (cblMental.Items[i].Selected && !sub.Contains(cblMental.Items[i].Value))
                                {
                                    PessoaFisicaDeficiencia oPessoaDef = new PessoaFisicaDeficiencia();
                                    oPessoaDef.PessoaFisica = new PessoaFisica(idPessoaFisica);
                                    oDefDetalhe = DeficienciaDetalhe.LoadObject(Convert.ToInt32(cblMental.Items[i].Value));
                                    oPessoaDef.DeficienciaDetalhe = oDefDetalhe;
                                    oPessoaDef.Deficiencia = oDefDetalhe.Deficiencia;
                                    oPessoaDef.Save();
                                }
                                else if (!cblMental.Items[i].Selected && sub.Contains(cblMental.Items[i].Value))
                                    lstDeletar.Add(Convert.ToInt32(cblMental.Items[i].Value));
                            }
                            #endregion

                        if (lstDeletar.Count > 0)
                            PessoaFisicaDeficiencia.Delete(lstDeletar, base.IdPessoaFisicaLogada.Value);
                    }
                    catch (Exception ex)
                    {
                   
                    }
                    finally{
                        mpeDeficiencia.Hide();
                    }
                }
            #endregion

            #region SalvarVaga
            public void SalvarVaga(int idVaga)
            {
                try
                {
                    string[] salvo = hdDeficiencias.Value.Split(',');
                    List<string> sub = new List<string>();
                    for (int i = 0; i < salvo.Count(); i++)
                    {
                        sub.Add(salvo[i]);
                    }
                    List<int> lstDeletar = new List<int>();
                    DeficienciaDetalhe oDefDetalhe;

                    #region cblFisica
                    for (int i = 0; i < cblFisica.Items.Count; i++)
                    {

                        if (cblFisica.Items[i].Selected && !sub.Contains(cblFisica.Items[i].Value))
                        {
                            VagaDeficiencia oVagaDef = new VagaDeficiencia();
                            oVagaDef.Vaga = new Vaga(idVaga);
                            oDefDetalhe = DeficienciaDetalhe.LoadObject(Convert.ToInt32(cblFisica.Items[i].Value));
                            oVagaDef.DeficienciaDetalhe = oDefDetalhe;
                            oVagaDef.Deficiencia = oDefDetalhe.Deficiencia;
                            oVagaDef.Save();
                        }
                        else if (!cblFisica.Items[i].Selected && sub.Contains(cblFisica.Items[i].Value))
                            lstDeletar.Add(Convert.ToInt32(cblFisica.Items[i].Value));
                    }
                    #endregion

                    #region cblAuditiva
                    for (int i = 0; i < cblAuditiva.Items.Count; i++)
                    {

                        if (cblAuditiva.Items[i].Selected && !sub.Contains(cblAuditiva.Items[i].Value))
                        {
                            VagaDeficiencia oVagaDef = new VagaDeficiencia();
                            oVagaDef.Vaga = new Vaga(idVaga);
                            oDefDetalhe = DeficienciaDetalhe.LoadObject(Convert.ToInt32(cblAuditiva.Items[i].Value));
                            oVagaDef.DeficienciaDetalhe = oDefDetalhe;
                            oVagaDef.Deficiencia = oDefDetalhe.Deficiencia;
                            oVagaDef.Save();
                        }
                        else if (!cblAuditiva.Items[i].Selected && sub.Contains(cblAuditiva.Items[i].Value))
                            lstDeletar.Add(Convert.ToInt32(cblAuditiva.Items[i].Value));
                    }
                    #endregion

                    #region cblVisual
                    for (int i = 0; i < cblVisual.Items.Count; i++)
                    {

                        if (cblVisual.Items[i].Selected && !sub.Contains(cblVisual.Items[i].Value))
                        {
                            VagaDeficiencia oVagaDef = new VagaDeficiencia();
                            oVagaDef.Vaga = new Vaga(idVaga);
                            oDefDetalhe = DeficienciaDetalhe.LoadObject(Convert.ToInt32(cblVisual.Items[i].Value));
                            oVagaDef.DeficienciaDetalhe = oDefDetalhe;
                            oVagaDef.Deficiencia = oDefDetalhe.Deficiencia;
                            oVagaDef.Save();
                        }
                        else if (!cblVisual.Items[i].Selected && sub.Contains(cblVisual.Items[i].Value))
                            lstDeletar.Add(Convert.ToInt32(cblVisual.Items[i].Value));
                    }
                    #endregion

                    #region cblMental
                    for (int i = 0; i < cblMental.Items.Count; i++)
                    {

                        if (cblMental.Items[i].Selected && !sub.Contains(cblMental.Items[i].Value))
                        {
                            VagaDeficiencia oVagaDef = new VagaDeficiencia();
                            oVagaDef.Vaga = new Vaga(idVaga);
                            oDefDetalhe = DeficienciaDetalhe.LoadObject(Convert.ToInt32(cblMental.Items[i].Value));
                            oVagaDef.DeficienciaDetalhe = oDefDetalhe;
                            oVagaDef.Deficiencia = oDefDetalhe.Deficiencia;
                            oVagaDef.Save();
                        }
                        else if (!cblMental.Items[i].Selected && sub.Contains(cblMental.Items[i].Value))
                            lstDeletar.Add(Convert.ToInt32(cblMental.Items[i].Value));
                    }
                    #endregion

                    if (lstDeletar.Count > 0)
                        VagaDeficiencia.Delete(lstDeletar, idVaga);
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex);
                }
                finally
                {
                    mpeDeficiencia.Hide();
                }
            }
            #endregion

            #region btiFechar_Click
                protected void btiFechar_Click(object sender, ImageClickEventArgs e)
                {
                    mpeDeficiencia.Hide();
                }
            #endregion

        #endregion

        #region Metodos
         
           #region PreencherCheckBox
                protected void PreencherCheckBox()
                {
                    UIHelper.CarregarCheckBoxList(cblFisica, DeficienciaDetalhe.listaDeficiencia((int)BNE.BLL.Enumeradores.Deficiencia.Fisica));
                    UIHelper.CarregarCheckBoxList(cblAuditiva, DeficienciaDetalhe.listaDeficiencia((int)BNE.BLL.Enumeradores.Deficiencia.Auditiva));
                    UIHelper.CarregarCheckBoxList(cblVisual, DeficienciaDetalhe.listaDeficiencia((int)BNE.BLL.Enumeradores.Deficiencia.Visual));
                    UIHelper.CarregarCheckBoxList(cblMental, DeficienciaDetalhe.listaDeficiencia((int)BNE.BLL.Enumeradores.Deficiencia.Mental));
                   upConteudo.Update();
                
                }
            #endregion

           #region CarregaUsuario

                private void CarregaUsuario()
                {
                    PreencherCampos(PessoaFisicaDeficiencia.listaDeficiencia(Convert.ToInt32(base.IdPessoaFisicaLogada.Value)));
                }
                #endregion

           #region CarregaVaga(int? idVaga)

                private void CarregaVaga(int? idVaga)
                {
                    PreencherCampos(VagaDeficiencia.listaDeficienciaVaga(idVaga));
                }
                #endregion
    
           #region PreencherCampos

                private void PreencherCampos(dynamic ListDef)
                {
                    try
                    {
                        foreach (var id in ListDef)
                        {
                            hdDeficiencias.Value += id.DeficienciaDetalhe.IdfDeficienciaDetalhe + ",";

                            var selecionado = cblFisica.Items.FindByValue(id.DeficienciaDetalhe.IdfDeficienciaDetalhe.ToString());
                            if (selecionado != null)
                                selecionado.Selected = true;

                            selecionado = cblAuditiva.Items.FindByValue(id.DeficienciaDetalhe.IdfDeficienciaDetalhe.ToString());
                            if (selecionado != null)
                                selecionado.Selected = true;

                            selecionado = cblVisual.Items.FindByValue(id.DeficienciaDetalhe.IdfDeficienciaDetalhe.ToString());
                            if (selecionado != null)
                                selecionado.Selected = true;

                            selecionado = cblMental.Items.FindByValue(id.DeficienciaDetalhe.IdfDeficienciaDetalhe.ToString());
                            if (selecionado != null)
                                selecionado.Selected = true;
                        }
                    }
                    catch (Exception ex)
                    {

                        EL.GerenciadorException.GravarExcecao(ex, "Modal Deficiencia");
                    }
                   
                }
           #endregion

           #region MostrarModal
                public void MostrarModalUsuario()
                {
                    mpeDeficiencia.Show();
                  //  if (!Page.IsPostBack)    
                        CarregaUsuario();
                }

                public void MostrarModalVaga(int? idVaga)
                {
                  
                    mpeDeficiencia.Show();
                    if (idVaga != null) 
                        CarregaVaga(idVaga);
                }
            #endregion

        #endregion
        
    }
}