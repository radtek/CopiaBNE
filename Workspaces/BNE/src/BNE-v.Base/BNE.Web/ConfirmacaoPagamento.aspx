<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true" CodeBehind="ConfirmacaoPagamento.aspx.cs"
    Inherits="BNE.Web.ConfirmacaoPagamento" %>

<%@ Register Src="~/UserControls/Modais/EnvioEmail.ascx" TagPrefix="uc3" TagName="EnvioEmail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">

    <!-- Rastreamento de transações - GA -->
    <script type="text/javascript">
        _gaq.push(['_addTrans',
            '<%=IdPlanoAdquirido%>',           // transaction ID - required
            'BNE',  // affiliation or store name
            '<%=VlrPagamento%>',          // total - required
            '0',           // tax
            '0',              // shipping
            '<%=NmeCidade%>',       // city
            '<%=NmeEstado%>',     // state or province
            '<%=SiglaPais%>'             // country
        ]);

        // add item might be called for every item in the shopping cart
        // where your ecommerce engine loops through each item in the cart and
        // prints out _addItem for each
        _gaq.push(['_addItem',
            '<%=IdPlanoAdquirido%>',           // transaction ID - required
            '<%=IdPlano%>',           // SKU/code - required
            '<%=NomePlano%>',        // product name
            '<%=TipoPlano%>',   // category or variation
            '<%=VlrPagamento%>',          // unit price - required
            '1'               // quantity - required
        ]);

        _gaq.push(['_setCustomVar',
            1,                   // This custom var is set to slot #1.  Required parameter.
            'FormaPagamento',           // The top-level name for your online content categories.  Required parameter.
            '<%=FormaPagamento%>'  // Sets the value of "Section" to "Life & Style" for this particular aricle.  Required parameter.
        ]);

            _gaq.push(['_trackTrans']); //submits transaction to the Analytics servers
    </script>
    <!-- END Rastreamento de transações - GA -->
    <!-- Adwords -->
    <script type="text/javascript">
        /* <![CDATA[ */
        var google_conversion_id = 998802791;
        var google_conversion_language = "en";
        var google_conversion_format = "2";
        var google_conversion_color = "ffffff";
        var google_conversion_label = "ZO7mCNnwqAwQ54qi3AM";
        var google_remarketing_only = false;
        /* ]]> */
    </script>
    <script type="text/javascript" src="//www.googleadservices.com/pagead/conversion.js">
    </script>
    <noscript>
        <div style="display: inline;">
            <img height="1" width="1" style="border-style: none;" alt="" src="//www.googleadservices.com/pagead/conversion/998802791/?label=ZO7mCNnwqAwQ54qi3AM&amp;guid=ON&amp;script=0" />
        </div>
    </noscript>
    <!-- END Adwords -->
    <Employer:DynamicScript runat="server" Src="/js/local/BoletoBancario.js" type="text/javascript" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <asp:Panel ID="pnlBoleto" runat="server" CssClass="pnlBoleto">
        <div>
            <p class="texto_utilizar_servicos">Obrigado por confiar em nossos serviços!</p>
            <p class="texto_utilizar_servicos">Efetue o pagamento do boleto e tenha acesso completo conforme o plano selecionado.</p>
            <p class="texto_utilizar_servicos">A liberação ocorre no 1° dia útil após o pagamento, caso precise de liberação imediata encaminhe o comprovante para financeiro@bne.com.br.</p>
        </div>
        <div class="container_boleto">
            <asp:Image ID="imgBoleto" runat="server" />
        </div>
        <div class="painel_botoes">
            <asp:Button ID="btnImprimir" runat="server" CssClass="botao_padrao" Text="Imprimir" OnClientClick="return ImprimirBoletoBancario()" />
            <a href="" id="hlDownload" runat="server" class="botao_padrao download" download>Salvar boletos</a>
            <%--<asp:HyperLink ID="hlDownload" runat="server" CssClass="botao_padrao download" Text="Salvar boletos" Target="_blank" />--%>
            <asp:Button ID="btnEnviarPorEmail" runat="server" CssClass="botao_padrao" Text="Enviar por E-mail" OnClick="btnEnviarPorEmail_Click" />
            <asp:Button ID="btnVoltar" runat="server" CssClass="botao_padrao" Text="Voltar" OnClick="BtnVoltarClick" />
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlPlanoLiberado" runat="server" CssClass="pnlConfirmacaoCartao">
        <div>
            <div class="icone_confirmacao" style="float: left;">
                <img alt=""
                    src="/img/img_modal_confirmacao_envio_curriculo_icone.png" />
            </div>
            <p class="texto_pagamento_sucesso">
                Pagamento realizado com sucesso!
            </p>
            <p class="texto_utilizar_servicos">
                Seu pagamento foi realizado e a partir de agora você já pode utilizar os serviços exclusivos do BNE. 
                Caso precise de ajuda, utilize o Atendimento Online.
            </p>
        </div>
        <div class="painel_botoes">
            <asp:Button ID="btn_sala_vip" runat="server" CssClass="botao_padrao" Text="Ir para minha sala VIP" OnClick="BtnIrParaSalaVipClick" />
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlAguardandoIntermediador" runat="server" CssClass="pnlAguardandoIntermediador">
        <div>
            <div class="icone_confirmacao" style="float: left;">
                <img alt=""
                    src="/img/img_modal_confirmacao_envio_curriculo_icone.png" />
            </div>
            <p class="texto_pagamento_sucesso">
                Pagamento registrado com sucesso!
            </p>
            <p class="texto_utilizar_servicos">
                Seu pagamento foi registrado no
                <asp:Label runat="server" ID="lblNomeIntermediador"></asp:Label>. Assim que recebermos a confirmação de pagamento, seu plano será Liberado.
                Caso precise de ajuda, utilize o Atendimento Online.
            </p>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlDebitoRecorrente_Aguardando" runat="server" CssClass="pnlAguardandoIntermediador">
        <div>
            <div class="icone_confirmacao" style="float: left;">
                <img alt=""
                    src="/img/img_modal_confirmacao_envio_curriculo_icone.png" />
            </div>
            <p class="texto_pagamento_sucesso">
                Pagamento registrado com sucesso!
            </p>
            <p class="texto_utilizar_servicos">
                Seu pagamento foi registrado. Assim que recebermos a confirmação do débito, seu plano será Liberado. Certifique-se que o saldo seja suficiente para que o pagamento seja finalizado.
                Caso precise de ajuda, utilize o Atendimento Online.
            </p>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlDebitoRecorrente_Liberado" runat="server" CssClass="pnlAguardandoIntermediador">
        <div>
            <div class="icone_confirmacao" style="float: left;">
                <img alt=""
                    src="/img/img_modal_confirmacao_envio_curriculo_icone.png" />
            </div>
            <p class="texto_pagamento_sucesso">
                Pagamento registrado com sucesso!
            </p>
            <p class="texto_utilizar_servicos">
                Seu pagamento foi registrado e seu plano liberado. O débito deve ocorrer em sua conta em até 48 horas. Certifique-se que o saldo seja suficiente para que o pagamento seja finalizado.
                Caso precise de ajuda, utilize o Atendimento Online.
            </p>
        </div>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
    <uc3:EnvioEmail runat="server" ID="ucEnvioEmail" />
</asp:Content>
