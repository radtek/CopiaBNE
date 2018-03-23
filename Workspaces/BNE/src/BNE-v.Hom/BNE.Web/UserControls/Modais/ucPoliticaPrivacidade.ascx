<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ucPoliticaPrivacidade.ascx.cs" Inherits="BNE.Web.UserControls.Modais.ucPoliticaPrivacidade" %>

<script type="text/javascript">

    function AbrirModalPoliticaPrivacidade() {
        $("#modalPoliticaPrivacidade").show();
         document.getElementById('spModal').style.display = 'block';
    }

    function FecharModalPoliticaPrivacidade() {
        $("#modalPoliticaPrivacidade").hide();
         document.getElementById('spModal').style.display = 'none';
    }
</script>
<span id="spModal" style="display: none;">
       <div class="modal-backdrop fade in" style="opacity: .5 !important;"></div>
</span>
<div id="modalPoliticaPrivacidade" class="modal fade in politica-privacidade" tabindex="-1" role="dialog" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content" style="height: 547px !important;">
            <button type="button" class="close" onclick="FecharModalPoliticaPrivacidade();" aria-label="Close"><span aria-hidden="true">&times;</span></button>
            <img src="/img/logo_bne.png" />
            <div class="conteudo">
                <div class="titulo">
                    <b>POLÍTICA DE PRIVACIDADE E PROTEÇÃO DE DADOS</b>
                </div>
                <p class="paragrafo">
                    A presente política de privacidade e proteção dados rege o tratamento que damos à informação recebida no acesso e uso dos serviços do site, que BNE – Banco Nacional de Empregos oferece gratuitamente aos usuários de Internet interessados em seus serviços e conteúdos.
                </p>
                <p class="paragrafo">
                    Em conformidade com a legislação sobre proteção de dados pessoais, o BNE - Banco Nacional de Empregos deve atender a certos requisitos que garantem que qualquer informação fornecida pelos usuários é tratada com diligência e confidencialidade.
                </p>
                <p class="paragrafo">
                    Em caso de dúvidas relacionadas a nossa política de privacidade entre em contato: atendimento@bne.com.br
                </p>
                <b>1. Dados coletados</b>
                <p class="paragrafo">
                    O BNE coleta dados pessoais dos usuários com o objetivo de facilitar o contato entre empregadores e candidatos para oferecer ferramentas e serviços que apoiam os processos de busca de emprego, incluindo o envio via e-mail de vagas disponíveis de acordo com o perfil do candidato.
                </p>
                <p class="paragrafo">
                    Também são recolhidos dados adicionais dos usuários para efetuar análises estatísticas que permitem melhorar o desempenho dos serviços do site.
                </p>
                <b>Para candidatos:</b>
                <p class="paragrafo">
                    Ao registrar-se no site serão solicitados os dados pessoais através do preenchimento de um formulário. Os dados solicitados são estritamente necessários para a realização do propósito do site e serão armazenados e processados exclusivamente com esse objeto e sempre no âmbito da política estabelecida.
                </p>
                <p class="paragrafo">
                    Os dados pessoais do candidato incluem CPF, nome, data de nascimento, contatos,  formação e empregos. Os campos marcados com asterisco (*) no formulário de inscrição são de preenchimento obrigatório.
                </p>
                <b>Para empresas:</b>
                <p class="paragrafo">
                    O usuário responsável pela empresa precisa registrar-se no portal com informações básicas e pessoais, além do cartão do CNPJ, para realizar o recrutamento e seleção de profissionais.
                </p>
                <p class="paragrafo">
                    Estes dados são necessários para acesso posterior ao portal.
                </p>
                <p class="paragrafo">
                    Além da inclusão da vaga pelo próprio responsável, elas também podem ser captadas na internet, como por exemplo, site da empresa.
                </p>
                <b>2. Armazenamento e Transferência de Dados</b>
                <p class="paragrafo">
                    O uso do site e o registro ou apresentação de dados pessoais para o BNE implica o consentimento do candidato para o tratamento automatizado dos dados pessoais fornecidos, bem como o envio de comunicações por via eletrônica com informações relacionadas com BNE.  Durante o processo de registro, o candidato consente que os dados são transferidos a terceiros.
                </p>
                <p class="paragrafo">
                    Para estes fins significa terceiro "aqueles usuários ou entidades que desejem entrar em contato com o candidato" em relação aos processos de recrutamento. 
                </p>
                <p class="paragrafo">
                    O usuário que registra (privado e profissional) expressa seu conhecimento e, por conseguinte, concorda expressamente, que seus dados e o conteúdo que fornecer será pública e visível no site, para os motores de busca da Internet e que poderão ser fornecidas a outras empresas no âmbito da prestação do serviço.
                </p>
                <p class="paragrafo">
                    O usuário consente expressamente com a transferência de seus dados a empresas associadas ou pertencentes ao grupo BNE para o envio de ofertas próprias e promoções relacionadas aos interesses e perfil do usuário.
                </p>
                <p class="paragrafo">
                    O usuário está ciente e concorda que todos os dados que nos fornecer ou que forem gerados durante o seu relacionamento com o BNE poderão ser compartilhados com parceiros comerciais e com os parceiros destes, sempre para a finalidade de apoiar decisões de concessão de crédito, realização de negócios ou ofertas direcionadas. 
                </p>
                <p class="paragrafo">
                    Os efeitos da presente autorização serão estendidos para as partes envolvidas em uma venda, fusão, transferência de ativos ou reorganização societária.
                </p>
                <b>3. Segurança</b>
                <p class="paragrafo">
                    A segurança de seus dados é muito importante para o BNE, por isso no BNE adotou medidas técnicas e organizacionais de acordo com as regras estabelecidas para a segurança dos dados pessoais e evitar a sua alteração, perda, tratamento ou acesso não autorizado, tendo em conta o estado da tecnologia, a natureza dos dados armazenados e os riscos a que estão expostas.
                </p>
                <b>4 . Configurações de cookies</b>
                <p class="paragrafo">
                    O BNE utiliza cookies no site a fim de melhorar a experiência do usuário, permitindo que seja fornecido um melhor conteúdo, por exemplo, indicações mais precisas de ofertas de emprego. Os cookies que usamos não armazenam quaisquer informações confidenciais. Nosso objetivo é ter um site mais pessoal, informativo e amigável para o usuário e os cookies nos ajudam a obtê-lo.
                </p>
                <p class="paragrafo">
                    O que são Cookies?
                </p>
                <p class="paragrafo">
                    Eles são pequenos arquivos armazenados no computador do usuário que acessar o site. Esses arquivos passam entre o servidor do BNE e o computador do usuário final e permanecem armazenados no computador, mesmo depois de fechar a página, permitindo que o usuário tenha acesso da próxima vez que usar  o site BNE.
                </p>
                <p class="paragrafo">
                    A maioria dos navegadores aceitam cookies automaticamente mas você pode alterar suas configurações da internet para não aceitar cookies ou sujeitá-los a sua autorização.
                </p>
                <b>5. Direito de acesso, alteração, oposição e cancelamento dos dados</b>
                <p class="paragrafo">
                    O usuário tem o direito de ter acesso aos seus dados, a qualquer momento,  para corrigi-los se os detalhes estiverem incorretos, alterar suas configurações de privacidade, se opor ao tratamento dos seus dados e cancelar a inscrição de serviços do BNE.
                </p>
                <p class="paragrafo">
                    Esses direitos podem ser feitos através da própria configuração da página da web.
                </p>
                <b>6. Modificação da política de privacidade e proteção de dados</b>
                <p class="paragrafo">
                    O BNE reserva se ao direito de modificar a presente política para adaptá-la para futuros desenvolvimentos legislativos ou jurisprudenciais.
                </p>
                <p class="paragrafo">
                    Qualquer mudança que não derivam de adaptação às alterações legislativas ou jurisprudenciais será anunciada no site ou, se significativas, comunicada por e-mail para os usuários.
                </p>
                <b>Bne.com.br</b>
                <p class="paragrafo">
                    atendimento@bne.com.br
                </p>
            </div>
        </div>
    </div>
</div>
