using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.BLL;
using BNE.BLL.Enumeradores;
using BNE.BLL.Notificacao;
using BNE.EL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Funcao = BNE.BLL.Funcao;
using TipoMensagem = BNE.Web.Code.Enumeradores.TipoMensagem;

namespace BNE.Web
{
    public partial class AlertaVagasFormulario : BasePage
    {
        #region Propriedades

        #region Listas
        /// <summary>
        ///     Listas que são usadas para carregar as Cidades e Funcoes que possuem alertas para um curriculo
        /// </summary>
        private DataTable Cidades;

        private DataTable Funcoes;
        #endregion

        /// <summary>
        ///     Objetos que são utilizados durante a execução
        /// </summary>
        private PessoaFisica objPessoaFisica;

        private AlertaCurriculos alertaCurriculo;
        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Inicializar();

            ucConfirmacao.eventVoltar += ucConfirmacao_Voltar;
        }
        #endregion

        #region btnSalvar_Click
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            //Carrega o objeto de Curriculo que tem alerta
            try
            {
                Curriculo objCurriculo;

                Curriculo.CarregarPorPessoaFisica(IdPessoaFisicaLogada.Value, out objCurriculo);

                alertaCurriculo = AlertaCurriculos.LoadObject(objCurriculo.IdCurriculo);

                //Carrega datatables
                Cidades = (Cidades == null) ? Session["dtCidades"] as DataTable : Cidades;
                Funcoes = (Funcoes == null) ? Session["dtFuncoes"] as DataTable : Funcoes;

                //Lista de Cidades Selecionadas ou Excluidas
                var listaAlertaCidades = new List<AlertaCidades>();

                //Lista de Funções Selecionadas ou Excluídas
                var listaAlertaFuncoes = new List<AlertaFuncoes>();

                //Objetos utilizados para manipulação de Dados
                AlertaCidades cidade;
                AlertaFuncoes funcao;


                //Verifica se a Lista de Cidades está preenchida
                //Caso Sim verifica se é cidade nova ou cidade existente
                if (Cidades.Rows.Count > 0)
                {
                    foreach (DataRow row in Cidades.Rows)
                    {
                        var cidadeEstado = row["NomeCidade"].ToString().Split(Convert.ToChar("/"));

                        if (Convert.ToBoolean(row["Novo"]))
                        {
                            cidade = new AlertaCidades();
                            cidade.FlagInativo = (Convert.ToBoolean(row["Ativa"])) ? false : true;

                            if (cidade.FlagInativo)
                            {
                                cidade = null;
                            }
                            else
                            {
                                cidade.AlertaCurriculos = alertaCurriculo;
                                cidade.IdCidade = Convert.ToInt32(row["IdCidade"]);
                                cidade.NomeCidade = cidadeEstado[0];
                                cidade.SiglaEstado = cidadeEstado[1];
                            }
                        }
                        else
                        {
                            try
                            {
                                cidade = AlertaCidades.LoadObject(alertaCurriculo.IdCurriculo, Convert.ToInt32(row["IdCidade"]));
                                cidade.FlagInativo = (Convert.ToBoolean(row["Ativa"])) ? false : true;
                                if (cidade.FlagInativo)
                                {
                                    AlertaCidades.Delete(cidade.AlertaCurriculos.IdCurriculo, cidade.IdCidade);
                                    cidade = null;
                                }
                            }
                            catch
                            {
                                continue;
                            }
                        }
                        if (cidade != null)
                        {
                            listaAlertaCidades.Add(cidade);
                            cidade = null;
                        }
                    }

                    AlertaCidades.SalvarListaAlertaCidadesCurriculo(listaAlertaCidades);
                }

                //Verifica se a Lista de Funcções está preenchida
                //Caso Sim verifica se é função nova ou existente
                if (Funcoes.Rows.Count > 0)
                {
                    foreach (DataRow row in Funcoes.Rows)
                    {
                        if (Convert.ToBoolean(row["Novo"]))
                        {
                            funcao = new AlertaFuncoes();
                            funcao.FlagInativo = (Convert.ToBoolean(row["Ativa"])) ? false : true;

                            if (funcao.FlagInativo)
                            {
                                funcao = null;
                            }
                            else
                            {
                                funcao.AlertaCurriculos = alertaCurriculo;
                                funcao.FlagSimilar = (Convert.ToBoolean(row["Similar"])) ? false : true;
                                funcao.IdFuncao = Convert.ToInt32(row["IdFuncao"]);
                                funcao.DescricaoFuncao = row["DescFuncao"].ToString();
                            }
                        }
                        else
                        {
                            try
                            {
                                funcao = AlertaFuncoes.LoadObject(Convert.ToInt32(row["IdFuncao"]), alertaCurriculo.IdCurriculo);
                                funcao.FlagInativo = (Convert.ToBoolean(row["Ativa"])) ? false : true;
                                funcao.FlagSimilar = (Convert.ToBoolean(row["Similar"])) ? false : true;
                                if (funcao.FlagInativo)
                                {
                                    AlertaFuncoes.Delete(funcao.IdFuncao, funcao.AlertaCurriculos.IdCurriculo);
                                    funcao = null;
                                }
                            }
                            catch (RecordNotFoundException)
                            {
                                continue;
                            }
                        }
                        if (funcao != null)
                        {
                            listaAlertaFuncoes.Add(funcao);
                            funcao = null;
                        }
                    }


                    if(listaAlertaFuncoes.Count > 0)
                    {
                        using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(BLL.DataAccessLayer.CONN_NOTIFICACAO))
                        {
                            conn.Open();
                            using (System.Data.SqlClient.SqlTransaction trans = conn.BeginTransaction())
                            {
                                try
                                {
                                    AlertaFuncoes.SalvarListaAlertaFuncoesCurriculo(listaAlertaFuncoes,trans);
                                    trans.Commit();
                                }
                                catch (Exception ex)
                                {
                                    GerenciadorException.GravarExcecao(ex, string.Format("Erro ao cadastrar alertas por funcao -> idCurriculo {0}", objCurriculo.IdCurriculo));
                                }
                            }
                        }
                    }
                }

                //salvar dias da semana para receber o jornal
                var diasDaSemana = new Dictionary<int, bool>();

                diasDaSemana.Add(int.Parse(domingo.Value), domingo.Checked);
                diasDaSemana.Add(int.Parse(segunda.Value), segunda.Checked);
                diasDaSemana.Add(int.Parse(terca.Value), terca.Checked);
                diasDaSemana.Add(int.Parse(quarta.Value), quarta.Checked);
                diasDaSemana.Add(int.Parse(quinta.Value), quinta.Checked);
                diasDaSemana.Add(int.Parse(sexta.Value), sexta.Checked);
                diasDaSemana.Add(int.Parse(sabado.Value), sabado.Checked);

                AlertaCurriculosAgenda.SalvarDiasDaSemana(objCurriculo.IdCurriculo, diasDaSemana);
                ucConfirmacao.PreencherCampos("Alerta", " Seu Alerta foi cadastrado com sucesso!", false,true);
                ucConfirmacao.MostrarModal();
            }
            catch (RecordNotFoundException)
            {
                ExibirMensagem("Houve um erro na sua solicitação de Cadastro de Alertas!", TipoMensagem.Erro, false);
            }
        }
        #endregion

        #region OnTextChanged
        protected void OnTextChanged(object sender, EventArgs e)
        {
            if (IdPessoaFisicaLogada.HasValue)
            {
                //Carrega o CV
                Curriculo objCurriculo;
                Curriculo.CarregarPorPessoaFisica(IdPessoaFisicaLogada.Value, out objCurriculo);
                try
                {
                    alertaCurriculo = AlertaCurriculos.LoadObject(objCurriculo.IdCurriculo);

                    //Recupera os DataTables
                    Cidades = (Cidades == null) ? Session["dtCidades"] as DataTable : Cidades;
                    Funcoes = (Funcoes == null) ? Session["dtFuncoes"] as DataTable : Funcoes;

                    //Ao sair de um campo de Texto, verifica qual campo é
                    //E adiciona um novo elemento
                    if ((sender as TextBox).ID == txtCidade.ID)
                    {
                        Cidade city;
                        if (Cidade.CarregarPorNome(txtCidade.Text, out city))
                            AdicionarCidades(city);
                        if (Cidades.Rows.Count > 0)
                        {
                            rptCidades.DataSource = Cidades;
                            rptCidades.DataBind();
                        }
                    }
                    else
                    {
                        Funcao funcao;

                        if (string.IsNullOrEmpty(FuncoesSel.Value) || FuncoesSel.Value.Equals("null"))
                        {
                            funcao = Funcao.CarregarPorDescricao(((sender) as TextBox).Text);
                            if (funcao == null)
                                return;
                        }
                        else
                        {
                            try
                            {
                                funcao = Funcao.LoadObject(Convert.ToInt32(FuncoesSel.Value));
                            }
                            catch (RecordNotFoundException)
                            {
                                funcao = new Funcao(Convert.ToInt32(FuncoesSel.Value));
                            }
                        }
                        if (funcao != null)
                        {
                            AdicionarFuncoes(funcao, false);

                            List<Funcao> listFuncoes;

                            listFuncoes = FuncaoPretendida.CarregarFuncoesSimilaresPorFuncao(funcao.IdFuncao);

                            if (listFuncoes.Count > 0)
                            {
                                foreach (var item in listFuncoes)
                                {
                                    AdicionarFuncoes(item, true);
                                }
                            }

                            if (Funcoes.Rows.Count > 0)
                            {
                                rptFuncoes.DataSource = Funcoes;
                                rptFuncoes.DataBind();
                            }
                        }
                        else
                        {
                            throw new RecordNotFoundException(typeof (Funcao));
                        }
                    }
                }
                catch (RecordNotFoundException rex)
                {
                    ExibirMensagem("Não foi possível gerar Alerta! " + rex.Message, TipoMensagem.Erro, false);
                }
                finally
                {
                    (sender as TextBox).Text = "";
                }
            }
            else
            {
                Redirect(Page.GetRouteUrl(RouteCollection.LoginComercialCandidato.ToString(), null));
            }
        }
        #endregion

        #region repeater_ItemCreated
        protected void repeater_ItemCreated(object source, RepeaterItemEventArgs e)
        {
            var scriptMan = ScriptManager.GetCurrent(this);
            LinkButton btn = null;
            btn = e.Item.FindControl("lnkDeletarCidade") as LinkButton;
            if (btn == null)
                btn = e.Item.FindControl("lnkDeletarFuncao") as LinkButton;

            if (btn != null)
            {
                scriptMan.RegisterAsyncPostBackControl(btn);
            }
        }
        #endregion

        #region repeater_ItemCommand
        protected void repeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            //Inativa um elemento dado um comando
            DataTable table;
            var nmRow = "";

            if (e.CommandName == "deletarCidade")
            {
                table = Session["dtCidades"] as DataTable;
                nmRow = "IdCidade";
            }
            else
            {
                table = Session["dtFuncoes"] as DataTable;
                nmRow = "IdFuncao";
            }

            foreach (DataRow row in table.Rows)
            {
                if (row[nmRow].ToString() == e.CommandArgument.ToString())
                {
                    row["class"] += " liEsconder";
                    row["Ativa"] = false;
                }
            }

            if (e.CommandName == "deletarCidade")
            {
                rptCidades.DataSource = null;
                rptCidades.DataSource = table;
                rptCidades.DataBind();
                upTxtCidade.Update();
            }
            else
            {
                rptFuncoes.DataSource = null;
                rptFuncoes.DataSource = table;
                rptFuncoes.DataBind();
                upTxtFuncao.Update();
            }
        }
        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        /// <summary>
        ///     Ao Iniciar, o Page_Load
        ///     Se o usuário está Logado
        ///     Ajusta o título da tela, Inicializa a barra de pesquisa, ajusta o Topo do Usuário
        ///     Recupera por Default a cidade do Usuário e suas Funções Pretendidas
        ///     Senão
        ///     Redireciona para a tela de Login
        /// </summary>
        /// <remarks>Luan Fernandes</remarks>
        private void Inicializar()
        {
            #region Inicia Propriedades
            ///Cria a Lista que irá armazenar e popular as cidades na tela
            Cidades = new DataTable();
            Cidades.Columns.Add(new DataColumn("IdCidade", Type.GetType("System.Int32")));
            Cidades.Columns.Add(new DataColumn("NomeCidade", Type.GetType("System.String")));
            Cidades.Columns.Add(new DataColumn("class", Type.GetType("System.String")));
            Cidades.Columns.Add(new DataColumn("Ativa", Type.GetType("System.Boolean")));
            Cidades.Columns.Add(new DataColumn("Novo", Type.GetType("System.Boolean")));

            ///Cria a Lista que irá armazenar e popular as funções na tela
            Funcoes = new DataTable();
            Funcoes.Columns.Add(new DataColumn("IdFuncao", Type.GetType("System.Int32")));
            Funcoes.Columns.Add(new DataColumn("DescFuncao", Type.GetType("System.String")));
            Funcoes.Columns.Add(new DataColumn("class", Type.GetType("System.String")));
            Funcoes.Columns.Add(new DataColumn("Ativa", Type.GetType("System.Boolean")));
            Funcoes.Columns.Add(new DataColumn("Novo", Type.GetType("System.Boolean")));
            Funcoes.Columns.Add(new DataColumn("Similar", Type.GetType("System.Boolean")));
            #endregion

            if (IdUsuarioFilialPerfilLogadoCandidato.HasValue)
            {
                AjustarTituloTela("Alerta de Vagas");
                InicializarBarraBusca(TipoBuscaMaster.Vaga, false, "Alerta de Vagas");
                objPessoaFisica = PessoaFisica.LoadObject(IdPessoaFisicaLogada.Value);

                if (objPessoaFisica != null)
                {
                    CarregarAlertas(objPessoaFisica);
                }
                else
                {
                    ExibirMensagem("Você não está cadastrado(a)!", TipoMensagem.Aviso, false);
                }
            }
            else
                Redirect(GetRouteUrl(RouteCollection.LoginComercialCandidato.ToString(), null));
        }
        #endregion

        #region CarregarAlertas
        /// <summary>
        ///     Carrega os Alertas existentes para uma pessoa física com currículo ativo
        /// </summary>
        /// <param name="objPessoaFisica">Pessoa Física Logada</param>
        private void CarregarAlertas(PessoaFisica objPessoaFisica)
        {
            //Objeto que carregará de acordo com a pessoa Física
            Curriculo objCurriculo;

            if (Curriculo.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objCurriculo))
            {
                if (objCurriculo.FlagInativo)
                    ExibirMensagem("Seu Currículo está inativo, não é possível ativar alertas!", TipoMensagem.Aviso, false);
                else
                {
                    #region CarregaCvAlerta

                    //Carrega o objeto de Curriculo que tem alerta
                    try
                    {
                        alertaCurriculo = AlertaCurriculos.LoadObject(objCurriculo.IdCurriculo);
                    }
                    catch (RecordNotFoundException)
                    {
                        try
                        {
                            alertaCurriculo = new AlertaCurriculos();
                            alertaCurriculo.IdCurriculo = objCurriculo.IdCurriculo;
                            alertaCurriculo.EmailPessoa = objPessoaFisica.EmailPessoa;
                            alertaCurriculo.FlagVIP = objCurriculo.FlagVIP;
                            alertaCurriculo.NomePessoa = objPessoaFisica.NomePessoa;
                            alertaCurriculo.Save();

                            Curriculo.OnAlterarCurriculo(objCurriculo);
                        }
                        catch (Exception ex)
                        {
                            ExibirMensagem("Ocorreu um erro na solicitação!", TipoMensagem.Erro);
                            EL.GerenciadorException.GravarExcecao(ex, $"Criar alerta de vagas(tela de alerta) cv {objCurriculo.IdCurriculo}");
                        }
                    }
                    #endregion

                    #region VerificaAlertasExistentes

                    //Chama as cidades e funções em alerta para o currículo
                    AlertasExistentes(objCurriculo.IdCurriculo);
                    #endregion

                    #region Adiciona Cidades e Funções para Novos CV's ou CV's Sem Alertas

                    #region Cidades
                    if (Cidades.Rows.Count <= 0)
                    {
                        #region AdicionaCidades
                        var listaCidadesDefault = new List<Cidade>();

                        if (objPessoaFisica.Cidade.CompleteObject())
                        {
                            if (!listaCidadesDefault.Contains(objPessoaFisica.Cidade))
                                listaCidadesDefault.Add(objPessoaFisica.Cidade);
                        }

                        if (objCurriculo.CidadeEndereco != null)
                        {
                            if (objCurriculo.CidadeEndereco.CompleteObject())
                                if (!listaCidadesDefault.Contains(objCurriculo.CidadeEndereco))
                                    listaCidadesDefault.Add(objCurriculo.CidadeEndereco);
                        }

                        if (objCurriculo.CidadePretendida != null)
                        {
                            if (objCurriculo.CidadePretendida.CompleteObject())
                                if (!listaCidadesDefault.Contains(objCurriculo.CidadePretendida))
                                    listaCidadesDefault.Add(objCurriculo.CidadePretendida);
                        }

                        var dt = CurriculoDisponibilidadeCidade.ListarCidadesPorCurriculo(objCurriculo.IdCurriculo);

                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                Cidade city;
                                if (Cidade.CarregarPorNome(row["Nme_Cidade"].ToString(), out city))
                                    if (city != null)
                                        if (!listaCidadesDefault.Contains(city))
                                            listaCidadesDefault.Add(city);
                            }
                        }

                        foreach (var cidade in listaCidadesDefault)
                        {
                            AdicionarCidades(cidade);
                        }

                        if (Cidades.Rows.Count > 0)
                        {
                            rptCidades.DataSource = Cidades;
                            rptCidades.DataBind();
                        }
                        #endregion
                    }
                    else
                    {
                        rptCidades.DataSource = Cidades;
                        rptCidades.DataBind();
                    }
                    #endregion

                    #region Funcoes
                    if (Funcoes.Rows.Count <= 0)
                    {
                        #region AdicionaFuncao_e_Similares
                        var listFuncoesPretendidas = FuncaoPretendida.CarregarFuncoesPretendidasPorCurriculo(objCurriculo);
                        var listFuncoes = new List<Funcao>();
                        foreach (var funcaoPretendida in listFuncoesPretendidas)
                        {
                            if (funcaoPretendida.Funcao != null)
                            {
                                if (funcaoPretendida.Funcao.CompleteObject())
                                    AdicionarFuncoes(funcaoPretendida.Funcao);

                                listFuncoes = FuncaoPretendida.CarregarFuncoesSimilaresPorFuncao(funcaoPretendida.Funcao.IdFuncao);

                                if (listFuncoes.Count > 0)
                                {
                                    foreach (var funcao in listFuncoes)
                                    {
                                        AdicionarFuncoes(funcao, true);
                                    }
                                }
                            }
                        }

                        if (Funcoes.Rows.Count > 0)
                        {
                            rptFuncoes.DataSource = Funcoes;
                            rptFuncoes.DataBind();
                        }
                        #endregion
                    }
                    else
                    {
                        rptFuncoes.DataSource = Funcoes;
                        rptFuncoes.DataBind();
                    }
                    #endregion

                    #region AgendaSemanal
                    var dtAgenda = AlertaCurriculosAgenda.ListarDiasDaSemana(objCurriculo.IdCurriculo);

                    foreach (var item in dtAgenda.AsEnumerable())
                    {
                        switch (short.Parse(item[0].ToString()))
                        {
                            case 1:
                                domingo.Checked = true;
                                break;
                            case 2:
                                segunda.Checked = true;
                                break;
                            case 3:
                                terca.Checked = true;
                                break;
                            case 4:
                                quarta.Checked = true;
                                break;
                            case 5:
                                quinta.Checked = true;
                                break;
                            case 6:
                                sexta.Checked = true;
                                break;
                            case 7:
                                sabado.Checked = true;
                                break;
                        }
                    }
                    #endregion

                    #endregion
                }
                Session.Add("dtCidades", Cidades);
                Session.Add("dtFuncoes", Funcoes);
            }
            else
            {
                ExibirMensagem("Seu Perfil não utiliza esta funcionalidade ou Seu currículo não está cadastrado!", TipoMensagem.Aviso, false);
            }
        }
        #endregion

        #region AdicionarCidades
        /// <summary>
        ///     Adiciona uma Cidade Nova ou Ativa uma existente
        /// </summary>
        /// <param name="cidade">A Cidade selecionada</param>
        private void AdicionarCidades(Cidade cidade)
        {
            var jaExiste = false;

            AlertaCidades alertaCidade;

            foreach (DataRow item in Cidades.Rows)
            {
                if (item["IdCidade"].ToString().Equals(cidade.IdCidade.ToString()))
                {
                    jaExiste = true;
                    try
                    {
                        alertaCidade = AlertaCidades.LoadObject(alertaCurriculo.IdCurriculo, cidade.IdCidade);
                        alertaCidade.FlagInativo = false;
                        alertaCidade.Save();
                    }
                    catch (RecordNotFoundException)
                    {
                    }
                    finally
                    {
                        if (item["class"].ToString().Contains("liEsconder"))
                        {
                            item["class"] = item["class"].ToString().Replace(" liEsconder", "");
                            item["Ativa"] = true;
                        }
                        item["class"] = item["class"].ToString().Replace("liActive", "liInactive");
                        item["Ativa"] = true;
                    }
                }
                else
                {
                    item["class"] = item["class"].ToString().Replace("liInactive", "liActive");
                }
            }
            if (!jaExiste)
            {
                var drCidade = Cidades.NewRow();
                drCidade["IdCidade"] = cidade.IdCidade;
                drCidade["NomeCidade"] = cidade.NomeCidade + "/" + cidade.Estado.SiglaEstado;
                drCidade["class"] = "liActive";
                drCidade["Ativa"] = true;
                drCidade["Novo"] = true;

                Cidades.Rows.Add(drCidade);
            }
        }
        #endregion

        #region AdicionarFuncoes
        /// <summary>
        ///     Adiciona uma nova função ou ativa uma existente
        /// </summary>
        /// <param name="funcao">A Função selecionada</param>
        /// <param name="isSimilar">Se é uma função similar a outra já selecionada</param>
        private void AdicionarFuncoes(Funcao funcao, bool isSimilar = false, bool similaresPretendidas = false)
        {
            var jaExiste = false;
            AlertaFuncoes alertaFuncao;

            foreach (DataRow item in Funcoes.Rows)
            {
                if (item["IdFuncao"].ToString() == funcao.IdFuncao.ToString())
                {
                    jaExiste = true;
                    try
                    {
                        alertaFuncao = AlertaFuncoes.LoadObject(funcao.IdFuncao, alertaCurriculo.IdCurriculo);
                        alertaFuncao.FlagInativo = false;
                        alertaFuncao.Save();
                    }
                    catch (RecordNotFoundException)
                    {
                    }
                    finally
                    {
                        if (item["class"].ToString().Contains("liEsconder"))
                        {
                            if (isSimilar)
                                item["class"] = "liActive liSimilar";
                            else
                                item["class"] = "liActive";

                            item["Ativa"] = true;
                        }
                        else
                        {
                            if (isSimilar)
                                item["class"] = " liSimilar";
                        }
                    }
                }
                else
                {
                    item["class"] = item["class"].ToString().Replace("liInactive", "liActive");
                }
            }
            if (!jaExiste | similaresPretendidas)
            {
                var drFuncao = Funcoes.NewRow();
                drFuncao["IdFuncao"] = funcao.IdFuncao;
                drFuncao["DescFuncao"] = funcao.DescricaoFuncao;
                if (similaresPretendidas | isSimilar)
                    drFuncao["class"] = "liActive liSimilar";
                else
                    drFuncao["class"] = "liActive";
                drFuncao["Ativa"] = true;
                drFuncao["Novo"] = true;
                drFuncao["Similar"] = isSimilar;

                Funcoes.Rows.Add(drFuncao);
            }
        }
        #endregion

        #region AlertasExistentes
        /// <summary>
        ///     Carrega os alertas existentes de determinado currículo
        /// </summary>
        /// <param name="IdCurriculo">o Id do currículo a ser carregados os alertas</param>
        private void AlertasExistentes(int IdCurriculo)
        {
            var dtCidades = AlertaCidades.ListarCidadesAlertaCurriculo(IdCurriculo);
            var dtFuncoes = AlertaFuncoes.ListarFuncoesAlertaCurriculo(IdCurriculo);

            foreach (DataRow row in dtCidades.Rows)
            {
                #region AdicionaCidades
                var drCidade = Cidades.NewRow();
                drCidade["IdCidade"] = row["Idf_Cidade"];
                drCidade["NomeCidade"] = row["Nme_Cidade"] + "/" + row["Sig_Estado"];
                drCidade["Novo"] = false;
                if (!Convert.ToBoolean(row["Flg_Inativo"]))
                {
                    drCidade["class"] = "liActive";
                    drCidade["Ativa"] = true;
                }
                else
                {
                    drCidade["class"] = "liActive liEsconder";
                    drCidade["Ativa"] = false;
                }

                Cidades.Rows.Add(drCidade);
                #endregion
            }

            foreach (DataRow row in dtFuncoes.Rows)
            {
                #region AdicionaFuncoes
                var drFuncao = Funcoes.NewRow();
                drFuncao["IdFuncao"] = row["Idf_Funcao"];
                drFuncao["DescFuncao"] = row["Des_Funcao"];
                drFuncao["Novo"] = false;
                if (!Convert.ToBoolean(row["Flg_Inativo"]))
                {
                    drFuncao["class"] = "liActive";
                    drFuncao["Ativa"] = true;
                }
                else
                {
                    drFuncao["class"] = "liActive liEsconder";
                    drFuncao["Ativa"] = false;
                }
                drFuncao["Similar"] = Convert.ToBoolean(row["Flg_Similar"]);
                Funcoes.Rows.Add(drFuncao);
                #endregion
            }
        }
        #endregion

        #region [Voltar]
        void ucConfirmacao_Voltar()
        {
            Redirect(GetRouteUrl(RouteCollection.SalaVIP.ToString(), null));
        }
        #endregion

        #endregion
    }
}