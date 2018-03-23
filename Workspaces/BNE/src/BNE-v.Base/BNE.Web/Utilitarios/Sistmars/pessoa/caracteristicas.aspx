<%@ Page Language="c#"
    CodeBehind="caracteristicas.aspx.cs"
    AutoEventWireup="false"
    Inherits="SistMars.pessoa.frmCaracteristicas" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>.: SistMars
        :.</title>
    <meta name="GENERATOR"
        content="Microsoft Visual Studio 7.0">
    <meta name="CODE_LANGUAGE"
        content="C#">
    <meta name="vs_defaultClientScript"
        content="JavaScript">
    <meta name="vs_targetSchema"
        content="http://schemas.microsoft.com/intellisense/ie5">
    <link rel="stylesheet"
        href="../include/SistMars.css"
        type="text/css">
</head>
<body ms_positioning="GridLayout"
    background="../images/fundos.jpg">
    <form id="frmCaracteristicas"
    method="post"
    action="resultado.aspx">
    <input type="hidden"
        name="hiddenSelecao">
    <table width="650"
        align="center"
        border="0"
        class="texto10">
        <tr>
            <td colspan="2"
                valign="middle">
                <img src="../images/logosistopo.gif"
                    border="0"><br>
                <br>
            </td>
        </tr>
        <tr>
            <td>
                <img src="../images/avaliacao.gif">
            </td>
            <td align="right">
                <img src="../images/direitos_autorais.gif"
                    border="0">
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <br>
                <img src="../images/delineamento.gif">
            </td>
        </tr>
        <tr>
            <td colspan="2"
                class="justificar">
                &nbsp;&nbsp;&nbsp;&nbsp;Na
                tabela abaixo,
                assinale as
                características
                que mais se
                aproximem
                do seu jeito
                de ser. Para
                isso, defina
                seu comportamento
                mais freqüente
                e pense sempre
                em você no
                momento atual.
                Não marque
                as qualidades
                que gostaria
                de ter e sim
                aquelas que
                você realmente
                possui.<br>
                &nbsp;&nbsp;&nbsp;&nbsp;A
                partir de
                suas escolhas,
                nosso sistema
                traça graficos
                que identificam
                respostas
                manipuladas,
                portanto seja
                autêntico.
                Não se preocupe
                em assinalar
                um traço de
                personalidade
                que considere
                negativo pois
                ele pode ter
                aspecto positivo
                dentro do
                conjunto.<br>
                &nbsp;&nbsp;&nbsp;&nbsp;Marque,
                obrigatoriamente,
                duas características
                em cada coluna:
            </td>
        </tr>
    </table>
    <br>
    <br>
    <script language="javascript">
        var corOriginal;

        //funcao chamada quando clica em uma caracteristica..
        //alem de mudar a imagem de checkzinho na tela,
        //guarda no hidden todos os valores selecionados até o momento
        function select(img) {
            var total = 0;
            //verifica quantos itens tem selecionados na coluna
            for (i = 0; i < 8; i++)
                total = total + somar(img.substring(0, 1) + (i + 1));

            //se ja tiver 2 itens selecionados, nao deixa selecionar mais e dá um alerta
            if (total >= 2 && document.all(img).src.indexOf("nao") > 1)
                alert('Selecione apenas 2 características por coluna\nCaso deseje trocar uma opção, primeiro desmarque outra');
            else {
                if (document.all(img).src.indexOf("sim") > 1) {
                    //desmarcando
                    document.all(img).src = "../images/img_nao.gif";
                    removerItemLista(img);
                }
                else {
                    //selecionando
                    document.all(img).src = "../images/img_sim.gif";
                    adicionarItemLista(img);
                }
            }
        }

        //adiciona um item ao hidden que guarda todas as posicoes dos itens selecionados
        function adicionarItemLista(item) {
            if (frmCaracteristicas.hiddenSelecao.value == "")
                frmCaracteristicas.hiddenSelecao.value = item
            else
                frmCaracteristicas.hiddenSelecao.value += "," + item
        }

        //remove um item do hidden que guarda todas as posicoes dos itens selecionados
        function removerItemLista(item) {
            var posicao = frmCaracteristicas.hiddenSelecao.value.indexOf(item)

            if (posicao == 0) //se for o primeiro item na lista, só tira duas posicoes:  "A1"
                frmCaracteristicas.hiddenSelecao.value =
							frmCaracteristicas.hiddenSelecao.value.substring(3);
            else //se for outro item, tira a virgula mais as duas posicoes:  ",A2"
                frmCaracteristicas.hiddenSelecao.value =
							frmCaracteristicas.hiddenSelecao.value.substring(0, posicao - 1) +
							frmCaracteristicas.hiddenSelecao.value.substring(posicao + 2);
        }

        //funcao utilizada para obter o total de itens selecionados por coluna
        function somar(img) {
            if (document.all(img).src.indexOf("sim") > 1)
                return 1;
            else
                return 0;
        }

        //muda cor da td
        function over(td) {
            corOriginal = td.style.backgroundColor;
            td.style.backgroundColor = "#ffffcc";
        }

        //volta cor da td
        function out(td) {
            td.style.backgroundColor = corOriginal;
        }

        //verifica se selecionou a quantidade certa de itens e dá o submit
        function continuar() {
            if (frmCaracteristicas.hiddenSelecao.value.length == 47)
                frmCaracteristicas.submit();
            else
                alert('Por favor, selecione duas características em cada coluna antes de continuar');
        }
    </script>
    <table align="center"
        width="758"
        border="0"
        bordercolor="#2a186f"
        class="texto7"
        cellpadding="1"
        cellspacing="0">
        <tr valign="middle"
            class="titulo1">
            <td>
                A
            </td>
            <td rowspan="9"
                class="tdSeparador">
                <img src=""
                    border="0"
                    width="3"
                    height="1">
            </td>
            <td>
                B
            </td>
            <td rowspan="9"
                class="tdSeparador">
                <img src=""
                    border="0"
                    width="3"
                    height="1">
            </td>
            <td>
                C
            </td>
            <td rowspan="9"
                class="tdSeparador">
                <img src=""
                    border="0"
                    width="3"
                    height="1">
            </td>
            <td>
                D
            </td>
            <td rowspan="9"
                class="tdSeparador">
                <img src=""
                    border="0"
                    width="3"
                    height="1">
            </td>
            <td>
                E
            </td>
            <td rowspan="9"
                class="tdSeparador">
                <img src=""
                    border="0"
                    width="3"
                    height="1">
            </td>
            <td>
                F
            </td>
            <td rowspan="9"
                class="tdSeparador">
                <img src=""
                    border="0"
                    width="3"
                    height="1">
            </td>
            <td>
                G
            </td>
            <td rowspan="9"
                class="tdSeparador">
                <img src=""
                    border="0"
                    width="3"
                    height="1">
            </td>
            <td>
                H
            </td>
        </tr>
        <tr bgcolor="#ffffff">
            <td onclick="select('A1');"
                bgcolor="#eefaff"
                style="cursor: hand"
                width="12%"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="A1"
                    value="A1">Intuitivo
            </td>
            <td onclick="select('B1');"
                style="cursor: hand"
                width="12%"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="B1"
                    value="B1">Empreendedor
            </td>
            <td onclick="select('C1');"
                bgcolor="#eefaff"
                style="cursor: hand"
                width="12%"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="C1"
                    value="C1">Explosivo
            </td>
            <td onclick="select('D1');"
                style="cursor: hand"
                width="12%"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="D1"
                    value="D1">Curioso
            </td>
            <td onclick="select('E1');"
                bgcolor="#eefaff"
                style="cursor: hand"
                width="12%"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="E1"
                    value="E1">Dinâmico
            </td>
            <td onclick="select('F1');"
                style="cursor: hand"
                width="12%"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="F1"
                    value="F1">Íntegro
            </td>
            <td onclick="select('G1');"
                bgcolor="#eefaff"
                style="cursor: hand"
                width="12%"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="G1"
                    value="G1">Respeitador
            </td>
            <td onclick="select('H1');"
                style="cursor: hand"
                width="12%"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="H1"
                    value="H1">Idealista
            </td>
        </tr>
        <tr bgcolor="#ffffff">
            <td onclick="select('A2');"
                bgcolor="#eefaff"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="A2"
                    value="A2">Persuasivo
            </td>
            <td onclick="select('B2');"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="B2"
                    value="B2">Presente
            </td>
            <td onclick="select('C2');"
                bgcolor="#eefaff"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="C2"
                    value="C2">Vencedor
            </td>
            <td onclick="select('D2');"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="D2"
                    value="D2">Versátil
            </td>
            <td onclick="select('E2');"
                bgcolor="#eefaff"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="E2"
                    value="E2">Convincente
            </td>
            <td onclick="select('F2');"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="F2"
                    value="F2">Resolvido
            </td>
            <td onclick="select('G2');"
                bgcolor="#eefaff"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="G2"
                    value="G2">Sereno
            </td>
            <td onclick="select('H2');"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="H2"
                    value="H2">Feliz
            </td>
        </tr>
        <tr bgcolor="#ffffff">
            <td onclick="select('A3');"
                bgcolor="#eefaff"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="A3"
                    value="A3">Firme
            </td>
            <td onclick="select('B3');"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="B3"
                    value="B3">Moderado
            </td>
            <td onclick="select('C3');"
                bgcolor="#eefaff"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="C3"
                    value="C3">Julgador
            </td>
            <td onclick="select('D3');"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="D3"
                    value="D3">Agradável
            </td>
            <td onclick="select('E3');"
                bgcolor="#eefaff"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="E3"
                    value="E3">Líder
            </td>
            <td onclick="select('F3');"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="F3"
                    value="F3">Planejador
            </td>
            <td onclick="select('G3');"
                bgcolor="#eefaff"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="G3"
                    value="G3">Inconveniente
            </td>
            <td onclick="select('H3');"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="H3"
                    value="H3">Impulsivo
            </td>
        </tr>
        <tr bgcolor="#ffffff">
            <td onclick="select('A4');"
                bgcolor="#eefaff"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="A4"
                    value="A4">Teimoso
            </td>
            <td onclick="select('B4');"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="B4"
                    value="B4">Guerreiro
            </td>
            <td onclick="select('C4');"
                bgcolor="#eefaff"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="C4"
                    value="C4">Vibrante
            </td>
            <td onclick="select('D4');"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="D4"
                    value="D4">Imprevisível
            </td>
            <td onclick="select('E4');"
                bgcolor="#eefaff"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="E4"
                    value="E4">Leal
            </td>
            <td onclick="select('F4');"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="F4"
                    value="F4">Egoísta
            </td>
            <td onclick="select('G4');"
                bgcolor="#eefaff"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="G4"
                    value="G4">Unificador
            </td>
            <td onclick="select('H4');"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="H4"
                    value="H4">Animado
            </td>
        </tr>
        <tr bgcolor="#ffffff">
            <td onclick="select('A5');"
                bgcolor="#eefaff"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="A5"
                    value="A5">Sério
            </td>
            <td onclick="select('B5');"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="B5"
                    value="B5">Legislador
            </td>
            <td onclick="select('C5');"
                bgcolor="#eefaff"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="C5"
                    value="C5">Controlador
            </td>
            <td onclick="select('D5');"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="D5"
                    value="D5">Protetor
            </td>
            <td onclick="select('E5');"
                bgcolor="#eefaff"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="E5"
                    value="E5">Aventureiro
            </td>
            <td onclick="select('F5');"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="F5"
                    value="F5">Profissional
            </td>
            <td onclick="select('G5');"
                bgcolor="#eefaff"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="G5"
                    value="G5">Solitário
            </td>
            <td onclick="select('H5');"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="H5"
                    value="H5">Comprometido
            </td>
        </tr>
        <tr bgcolor="#ffffff">
            <td onclick="select('A6');"
                bgcolor="#eefaff"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="A6"
                    value="A6">Resistente
            </td>
            <td onclick="select('B6');"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="B6"
                    value="B6">Motivado
            </td>
            <td onclick="select('C6');"
                bgcolor="#eefaff"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="C6"
                    value="C6">Emotivo
            </td>
            <td onclick="select('D6');"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="D6"
                    value="D6">Defensor
            </td>
            <td onclick="select('E6');"
                bgcolor="#eefaff"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="E6"
                    value="E6">Distraído
            </td>
            <td onclick="select('F6');"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="F6"
                    value="F6">Criativo
            </td>
            <td onclick="select('G6');"
                bgcolor="#eefaff"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="G6"
                    value="G6">Sensível
            </td>
            <td onclick="select('H6');"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="H6"
                    value="H6">Sonhador
            </td>
        </tr>
        <tr bgcolor="#ffffff">
            <td onclick="select('A7');"
                bgcolor="#eefaff"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="A7"
                    value="A7">Econômico
            </td>
            <td onclick="select('B7');"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="B7"
                    value="B7">Observador
            </td>
            <td onclick="select('C7');"
                bgcolor="#eefaff"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="C7"
                    value="C7">Prudente
            </td>
            <td onclick="select('D7');"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="D7"
                    value="D7">Verdadeiro
            </td>
            <td onclick="select('E7');"
                bgcolor="#eefaff"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="E7"
                    value="E7">Arrebatador
            </td>
            <td onclick="select('F7');"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="F7"
                    value="F7">Prático
            </td>
            <td onclick="select('G7');"
                bgcolor="#eefaff"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="G7"
                    value="G7">Paciente
            </td>
            <td onclick="select('H7');"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="H7"
                    value="H7">Incorrigível
            </td>
        </tr>
        <tr bgcolor="#ffffff">
            <td onclick="select('A8');"
                bgcolor="#eefaff"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="A8"
                    value="A8">Autoritário
            </td>
            <td onclick="select('B8');"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="B8"
                    value="B8">Ambicioso
            </td>
            <td onclick="select('C8');"
                bgcolor="#eefaff"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="C8"
                    value="C8">Indomável
            </td>
            <td onclick="select('D8');"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="D8"
                    value="D8">Exigente
            </td>
            <td onclick="select('E8');"
                bgcolor="#eefaff"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="E8"
                    value="E8">Envolvido
            </td>
            <td onclick="select('F8');"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="F8"
                    value="F8">Sofisticado
            </td>
            <td onclick="select('G8');"
                bgcolor="#eefaff"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="G8"
                    value="G8">Superior
            </td>
            <td onclick="select('H8');"
                style="cursor: hand"
                onmouseover="over(this);"
                onmouseout="out(this);">
                <img src="../images/img_nao.gif"
                    name="img"
                    id="H8"
                    value="H8">Deprimido
            </td>
        </tr>
    </table>
    <table width="758"
        border="0"
        align="center">
        <tr>
            <td height="5">
            </td>
        </tr>
        <tr>
            <td align="right">
                <img src="../images/continuar.gif"
                    style="cursor: hand"
                    onclick="continuar();">
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
