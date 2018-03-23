namespace AllInMail
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }

        public virtual DbSet<BNE_Acao_Publicacao> BNE_Acao_Publicacao { get; set; }
        public virtual DbSet<BNE_Adicional_Plano> BNE_Adicional_Plano { get; set; }
        public virtual DbSet<BNE_Adicional_Plano_Situacao> BNE_Adicional_Plano_Situacao { get; set; }
        public virtual DbSet<BNE_Agradecimento> BNE_Agradecimento { get; set; }
        public virtual DbSet<BNE_Amplitude_Salarial> BNE_Amplitude_Salarial { get; set; }
        public virtual DbSet<BNE_Arquivo> BNE_Arquivo { get; set; }
        public virtual DbSet<BNE_Auditor_Publicador> BNE_Auditor_Publicador { get; set; }
        public virtual DbSet<BNE_Cadastro_Parceiro> BNE_Cadastro_Parceiro { get; set; }
        public virtual DbSet<BNE_Campanha> BNE_Campanha { get; set; }
        public virtual DbSet<BNE_Campanha_Curriculo> BNE_Campanha_Curriculo { get; set; }
        public virtual DbSet<BNE_Campo_Publicacao> BNE_Campo_Publicacao { get; set; }
        public virtual DbSet<BNE_Carta_Email> BNE_Carta_Email { get; set; }
        public virtual DbSet<BNE_Carta_SMS> BNE_Carta_SMS { get; set; }
        public virtual DbSet<BNE_Celular> BNE_Celular { get; set; }
        public virtual DbSet<BNE_Celular_Selecionador> BNE_Celular_Selecionador { get; set; }
        public virtual DbSet<BNE_Cidade_Propaganda> BNE_Cidade_Propaganda { get; set; }
        public virtual DbSet<BNE_Codigo_Desconto> BNE_Codigo_Desconto { get; set; }
        public virtual DbSet<BNE_Codigo_Desconto_Plano> BNE_Codigo_Desconto_Plano { get; set; }
        public virtual DbSet<BNE_Consultor_R1> BNE_Consultor_R1 { get; set; }
        public virtual DbSet<BNE_Conta_Twitter> BNE_Conta_Twitter { get; set; }
        public virtual DbSet<BNE_Conteudo_HTML> BNE_Conteudo_HTML { get; set; }
        public virtual DbSet<BNE_Conversas_Ativas> BNE_Conversas_Ativas { get; set; }
        public virtual DbSet<BNE_Curriculo> BNE_Curriculo { get; set; }
        public virtual DbSet<BNE_Curriculo_Auditoria> BNE_Curriculo_Auditoria { get; set; }
        public virtual DbSet<BNE_Curriculo_Busca> BNE_Curriculo_Busca { get; set; }
        public virtual DbSet<BNE_Curriculo_Classificacao> BNE_Curriculo_Classificacao { get; set; }
        public virtual DbSet<BNE_Curriculo_Correcao> BNE_Curriculo_Correcao { get; set; }
        public virtual DbSet<BNE_Curriculo_Disponibilidade> BNE_Curriculo_Disponibilidade { get; set; }
        public virtual DbSet<BNE_Curriculo_Disponibilidade_Cidade> BNE_Curriculo_Disponibilidade_Cidade { get; set; }
        public virtual DbSet<BNE_Curriculo_Entrevista> BNE_Curriculo_Entrevista { get; set; }
        public virtual DbSet<BNE_Curriculo_Fila> BNE_Curriculo_Fila { get; set; }
        public virtual DbSet<BNE_Curriculo_Fulltext> BNE_Curriculo_Fulltext { get; set; }
        public virtual DbSet<BNE_Curriculo_Idioma> BNE_Curriculo_Idioma { get; set; }
        public virtual DbSet<BNE_Curriculo_Observacao> BNE_Curriculo_Observacao { get; set; }
        public virtual DbSet<BNE_Curriculo_Origem> BNE_Curriculo_Origem { get; set; }
        public virtual DbSet<BNE_Curriculo_Permissao> BNE_Curriculo_Permissao { get; set; }
        public virtual DbSet<BNE_Curriculo_Publicacao> BNE_Curriculo_Publicacao { get; set; }
        public virtual DbSet<BNE_Curriculo_Quem_Me_Viu> BNE_Curriculo_Quem_Me_Viu { get; set; }
        public virtual DbSet<BNE_Curriculo_Visualizacao> BNE_Curriculo_Visualizacao { get; set; }
        public virtual DbSet<BNE_Curriculo_Visualizacao_Historico> BNE_Curriculo_Visualizacao_Historico { get; set; }
        public virtual DbSet<BNE_Curso_Funcao_Tecla> BNE_Curso_Funcao_Tecla { get; set; }
        public virtual DbSet<BNE_Curso_Modalidade_Tecla> BNE_Curso_Modalidade_Tecla { get; set; }
        public virtual DbSet<BNE_Curso_Parceiro_Tecla> BNE_Curso_Parceiro_Tecla { get; set; }
        public virtual DbSet<BNE_Curso_Tecla> BNE_Curso_Tecla { get; set; }
        public virtual DbSet<BNE_Email_Destinatario> BNE_Email_Destinatario { get; set; }
        public virtual DbSet<BNE_Email_Destinatario_Cidade> BNE_Email_Destinatario_Cidade { get; set; }
        public virtual DbSet<BNE_Empresa_Home> BNE_Empresa_Home { get; set; }
        public virtual DbSet<BNE_Estatistica> BNE_Estatistica { get; set; }
        public virtual DbSet<BNE_Experiencia_Profissional> BNE_Experiencia_Profissional { get; set; }
        public virtual DbSet<BNE_Fale_Presidente> BNE_Fale_Presidente { get; set; }
        public virtual DbSet<BNE_Financeiro> BNE_Financeiro { get; set; }
        public virtual DbSet<BNE_Formacao> BNE_Formacao { get; set; }
        public virtual DbSet<BNE_Funcao_Erro_Sinonimo> BNE_Funcao_Erro_Sinonimo { get; set; }
        public virtual DbSet<BNE_Funcao_Pretendida> BNE_Funcao_Pretendida { get; set; }
        public virtual DbSet<BNE_Grupo_Cidade> BNE_Grupo_Cidade { get; set; }
        public virtual DbSet<BNE_Historico_Publicacao_Curriculo> BNE_Historico_Publicacao_Curriculo { get; set; }
        public virtual DbSet<BNE_Historico_Publicacao_Vaga> BNE_Historico_Publicacao_Vaga { get; set; }
        public virtual DbSet<BNE_Indicacao_Filial> BNE_Indicacao_Filial { get; set; }
        public virtual DbSet<BNE_Inscricao_Curso> BNE_Inscricao_Curso { get; set; }
        public virtual DbSet<BNE_Intencao_Filial> BNE_Intencao_Filial { get; set; }
        public virtual DbSet<BNE_Linha_Arquivo> BNE_Linha_Arquivo { get; set; }
        public virtual DbSet<BNE_Lista_Cidade> BNE_Lista_Cidade { get; set; }
        public virtual DbSet<BNE_Mensagem_CS> BNE_Mensagem_CS { get; set; }
        public virtual DbSet<BNE_Mensagem_Sistema> BNE_Mensagem_Sistema { get; set; }
        public virtual DbSet<BNE_Mini_Curriculo> BNE_Mini_Curriculo { get; set; }
        public virtual DbSet<BNE_Mobile_Token> BNE_Mobile_Token { get; set; }
        public virtual DbSet<BNE_Noticia> BNE_Noticia { get; set; }
        public virtual DbSet<BNE_Noticia_Visualizacao> BNE_Noticia_Visualizacao { get; set; }
        public virtual DbSet<BNE_Operadora> BNE_Operadora { get; set; }
        public virtual DbSet<BNE_Pagamento> BNE_Pagamento { get; set; }
        public virtual DbSet<BNE_Pagamento_Situacao> BNE_Pagamento_Situacao { get; set; }
        public virtual DbSet<BNE_Palavra_Chave> BNE_Palavra_Chave { get; set; }
        public virtual DbSet<BNE_Palavra_Proibida> BNE_Palavra_Proibida { get; set; }
        public virtual DbSet<BNE_Palavra_Publicacao> BNE_Palavra_Publicacao { get; set; }
        public virtual DbSet<BNE_Parametro_Busca_CV> BNE_Parametro_Busca_CV { get; set; }
        public virtual DbSet<BNE_Parceiro> BNE_Parceiro { get; set; }
        public virtual DbSet<BNE_Parceiro_Tecla> BNE_Parceiro_Tecla { get; set; }
        public virtual DbSet<BNE_Pesquisa_Vaga_log> BNE_Pesquisa_Vaga_log { get; set; }
        public virtual DbSet<BNE_Plano> BNE_Plano { get; set; }
        public virtual DbSet<BNE_Plano_Adquirido> BNE_Plano_Adquirido { get; set; }
        public virtual DbSet<BNE_Plano_Adquirido_Detalhes> BNE_Plano_Adquirido_Detalhes { get; set; }
        public virtual DbSet<BNE_Plano_Forma_Pagamento> BNE_Plano_Forma_Pagamento { get; set; }
        public virtual DbSet<BNE_Plano_Parcela> BNE_Plano_Parcela { get; set; }
        public virtual DbSet<BNE_Plano_Parcela_Situacao> BNE_Plano_Parcela_Situacao { get; set; }
        public virtual DbSet<BNE_Plano_Quantidade> BNE_Plano_Quantidade { get; set; }
        public virtual DbSet<BNE_Plano_Situacao> BNE_Plano_Situacao { get; set; }
        public virtual DbSet<BNE_Plano_Tipo> BNE_Plano_Tipo { get; set; }
        public virtual DbSet<BNE_Propaganda> BNE_Propaganda { get; set; }
        public virtual DbSet<BNE_Propaganda_Estado> BNE_Propaganda_Estado { get; set; }
        public virtual DbSet<BNE_Publicador> BNE_Publicador { get; set; }
        public virtual DbSet<BNE_Rastreador> BNE_Rastreador { get; set; }
        public virtual DbSet<BNE_Rastreador_Curriculo> BNE_Rastreador_Curriculo { get; set; }
        public virtual DbSet<BNE_Rastreador_Disponibilidade> BNE_Rastreador_Disponibilidade { get; set; }
        public virtual DbSet<BNE_Rastreador_Idioma> BNE_Rastreador_Idioma { get; set; }
        public virtual DbSet<BNE_Rastreador_Resultado_Vaga> BNE_Rastreador_Resultado_Vaga { get; set; }
        public virtual DbSet<BNE_Rastreador_Vaga> BNE_Rastreador_Vaga { get; set; }
        public virtual DbSet<BNE_Rede_Social_Conta> BNE_Rede_Social_Conta { get; set; }
        public virtual DbSet<BNE_Rede_Social_CS> BNE_Rede_Social_CS { get; set; }
        public virtual DbSet<BNE_Regra_Campo_Publicacao> BNE_Regra_Campo_Publicacao { get; set; }
        public virtual DbSet<BNE_Regra_Publicacao> BNE_Regra_Publicacao { get; set; }
        public virtual DbSet<BNE_Resposta_Automatica> BNE_Resposta_Automatica { get; set; }
        public virtual DbSet<BNE_Revidor> BNE_Revidor { get; set; }
        public virtual DbSet<BNE_Rota> BNE_Rota { get; set; }
        public virtual DbSet<BNE_Simulacao_R1> BNE_Simulacao_R1 { get; set; }
        public virtual DbSet<BNE_Situacao_Curriculo> BNE_Situacao_Curriculo { get; set; }
        public virtual DbSet<BNE_Situacao_Formacao> BNE_Situacao_Formacao { get; set; }
        public virtual DbSet<BNE_Solicitacao_R1> BNE_Solicitacao_R1 { get; set; }
        public virtual DbSet<BNE_Status_Curriculo_Vaga> BNE_Status_Curriculo_Vaga { get; set; }
        public virtual DbSet<BNE_Status_Mensagem_CS> BNE_Status_Mensagem_CS { get; set; }
        public virtual DbSet<BNE_Template> BNE_Template { get; set; }
        public virtual DbSet<BNE_Tipo_Adicional> BNE_Tipo_Adicional { get; set; }
        public virtual DbSet<BNE_Tipo_Codigo_Desconto> BNE_Tipo_Codigo_Desconto { get; set; }
        public virtual DbSet<BNE_Tipo_Curriculo> BNE_Tipo_Curriculo { get; set; }
        public virtual DbSet<BNE_Tipo_Mensagem_CS> BNE_Tipo_Mensagem_CS { get; set; }
        public virtual DbSet<BNE_Tipo_Origem> BNE_Tipo_Origem { get; set; }
        public virtual DbSet<BNE_Tipo_Pagamento> BNE_Tipo_Pagamento { get; set; }
        public virtual DbSet<BNE_Tipo_Perfil> BNE_Tipo_Perfil { get; set; }
        public virtual DbSet<BNE_Tipo_Publicacao> BNE_Tipo_Publicacao { get; set; }
        public virtual DbSet<BNE_Tipo_Sistema_Mobile> BNE_Tipo_Sistema_Mobile { get; set; }
        public virtual DbSet<BNE_Tipo_Vinculo> BNE_Tipo_Vinculo { get; set; }
        public virtual DbSet<BNE_Transacao> BNE_Transacao { get; set; }
        public virtual DbSet<BNE_Transacao_Resposta> BNE_Transacao_Resposta { get; set; }
        public virtual DbSet<BNE_Transacao_Retorno> BNE_Transacao_Retorno { get; set; }
        public virtual DbSet<BNE_Usuario> BNE_Usuario { get; set; }
        public virtual DbSet<BNE_Usuario_Filial> BNE_Usuario_Filial { get; set; }
        public virtual DbSet<BNE_Vaga> BNE_Vaga { get; set; }
        public virtual DbSet<BNE_Vaga_Candidato> BNE_Vaga_Candidato { get; set; }
        public virtual DbSet<BNE_Vaga_Candidato_Pergunta> BNE_Vaga_Candidato_Pergunta { get; set; }
        public virtual DbSet<BNE_Vaga_Disponibilidade> BNE_Vaga_Disponibilidade { get; set; }
        public virtual DbSet<BNE_Vaga_Divulgacao> BNE_Vaga_Divulgacao { get; set; }
        public virtual DbSet<BNE_Vaga_Fulltext> BNE_Vaga_Fulltext { get; set; }
        public virtual DbSet<BNE_Vaga_Home> BNE_Vaga_Home { get; set; }
        public virtual DbSet<BNE_Vaga_Integracao> BNE_Vaga_Integracao { get; set; }
        public virtual DbSet<BNE_Vaga_Palavra_Chave> BNE_Vaga_Palavra_Chave { get; set; }
        public virtual DbSet<BNE_Vaga_Pergunta> BNE_Vaga_Pergunta { get; set; }
        public virtual DbSet<BNE_Vaga_Rede_Social> BNE_Vaga_Rede_Social { get; set; }
        public virtual DbSet<BNE_Vaga_Tipo_Vinculo> BNE_Vaga_Tipo_Vinculo { get; set; }
        public virtual DbSet<GLO_Chave_Cielo> GLO_Chave_Cielo { get; set; }
        public virtual DbSet<GLO_Cobranca_Boleto> GLO_Cobranca_Boleto { get; set; }
        public virtual DbSet<GLO_Cobranca_Boleto_Arquivo_Remessa> GLO_Cobranca_Boleto_Arquivo_Remessa { get; set; }
        public virtual DbSet<GLO_Cobranca_Boleto_Arquivo_Retorno> GLO_Cobranca_Boleto_Arquivo_Retorno { get; set; }
        public virtual DbSet<GLO_Cobranca_Boleto_Lista_Remessa> GLO_Cobranca_Boleto_Lista_Remessa { get; set; }
        public virtual DbSet<GLO_Cobranca_Boleto_Lista_Retorno> GLO_Cobranca_Boleto_Lista_Retorno { get; set; }
        public virtual DbSet<GLO_Cobranca_Boleto_LOG> GLO_Cobranca_Boleto_LOG { get; set; }
        public virtual DbSet<GLO_Cobranca_Cartao> GLO_Cobranca_Cartao { get; set; }
        public virtual DbSet<GLO_Cobranca_Cartao_LOG> GLO_Cobranca_Cartao_LOG { get; set; }
        public virtual DbSet<GLO_Convenio_Bancario> GLO_Convenio_Bancario { get; set; }
        public virtual DbSet<GLO_Licenca_Cobre_Bem> GLO_Licenca_Cobre_Bem { get; set; }
        public virtual DbSet<GLO_Mensagem_Operadora_Cartao> GLO_Mensagem_Operadora_Cartao { get; set; }
        public virtual DbSet<GLO_Mensagem_Retorno_Boleto> GLO_Mensagem_Retorno_Boleto { get; set; }
        public virtual DbSet<GLO_Mensagem_Retorno_Cartao> GLO_Mensagem_Retorno_Cartao { get; set; }
        public virtual DbSet<GLO_Status_Transacao> GLO_Status_Transacao { get; set; }
        public virtual DbSet<GLO_Transacao> GLO_Transacao { get; set; }
        public virtual DbSet<LOG_Exclusao_CPF> LOG_Exclusao_CPF { get; set; }
        public virtual DbSet<TAB_Atividade> TAB_Atividade { get; set; }
        public virtual DbSet<TAB_Avaliacao> TAB_Avaliacao { get; set; }
        public virtual DbSet<TAB_Campo_Integrador> TAB_Campo_Integrador { get; set; }
        public virtual DbSet<TAB_Cidade_Capital> TAB_Cidade_Capital { get; set; }
        public virtual DbSet<TAB_Contato> TAB_Contato { get; set; }
        public virtual DbSet<TAB_Curso> TAB_Curso { get; set; }
        public virtual DbSet<TAB_Curso_Fonte> TAB_Curso_Fonte { get; set; }
        public virtual DbSet<Tab_Disponibilidade> Tab_Disponibilidade { get; set; }
        public virtual DbSet<TAB_Empresa> TAB_Empresa { get; set; }
        public virtual DbSet<TAB_Endereco> TAB_Endereco { get; set; }
        public virtual DbSet<TAB_Envio_Email_Sal_BR> TAB_Envio_Email_Sal_BR { get; set; }
        public virtual DbSet<TAB_Feriado> TAB_Feriado { get; set; }
        public virtual DbSet<TAB_Feriado_Modelo> TAB_Feriado_Modelo { get; set; }
        public virtual DbSet<TAB_Filial> TAB_Filial { get; set; }
        public virtual DbSet<TAB_Filial_BNE> TAB_Filial_BNE { get; set; }
        public virtual DbSet<TAB_Filial_Observacao> TAB_Filial_Observacao { get; set; }
        public virtual DbSet<TAB_Fonte> TAB_Fonte { get; set; }
        public virtual DbSet<TAB_Funcao_Mini_Agrupadora> TAB_Funcao_Mini_Agrupadora { get; set; }
        public virtual DbSet<TAB_Grupo_Economico> TAB_Grupo_Economico { get; set; }
        public virtual DbSet<TAB_Idioma> TAB_Idioma { get; set; }
        public virtual DbSet<TAB_Integracao_SINE_2> TAB_Integracao_SINE_2 { get; set; }
        public virtual DbSet<TAB_Integrador> TAB_Integrador { get; set; }
        public virtual DbSet<TAB_Integrador_Curriculo> TAB_Integrador_Curriculo { get; set; }
        public virtual DbSet<TAB_Mensagem_CS> TAB_Mensagem_CS { get; set; }
        public virtual DbSet<TAB_Mensagem_Mailing_Bruno> TAB_Mensagem_Mailing_Bruno { get; set; }
        public virtual DbSet<TAB_Mini_Agrupadora> TAB_Mini_Agrupadora { get; set; }
        public virtual DbSet<TAB_Nivel_Curso> TAB_Nivel_Curso { get; set; }
        public virtual DbSet<TAB_Nivel_Idioma> TAB_Nivel_Idioma { get; set; }
        public virtual DbSet<TAB_Origem> TAB_Origem { get; set; }
        public virtual DbSet<TAB_Origem_Filial> TAB_Origem_Filial { get; set; }
        public virtual DbSet<TAB_Origem_Filial_Funcao> TAB_Origem_Filial_Funcao { get; set; }
        public virtual DbSet<TAB_Parametro_Curriculo> TAB_Parametro_Curriculo { get; set; }
        public virtual DbSet<TAB_Parametro_Filial> TAB_Parametro_Filial { get; set; }
        public virtual DbSet<TAB_Parametro_Integrador> TAB_Parametro_Integrador { get; set; }
        public virtual DbSet<TAB_Perfil> TAB_Perfil { get; set; }
        public virtual DbSet<TAB_Perfil_Usuario> TAB_Perfil_Usuario { get; set; }
        public virtual DbSet<TAB_Pesquisa_Curriculo> TAB_Pesquisa_Curriculo { get; set; }
        public virtual DbSet<TAB_Pesquisa_Curriculo_Disponibilidade> TAB_Pesquisa_Curriculo_Disponibilidade { get; set; }
        public virtual DbSet<TAB_Pesquisa_Curriculo_Idioma> TAB_Pesquisa_Curriculo_Idioma { get; set; }
        public virtual DbSet<TAB_Pesquisa_Salarial> TAB_Pesquisa_Salarial { get; set; }
        public virtual DbSet<TAB_Pesquisa_Vaga> TAB_Pesquisa_Vaga { get; set; }
        public virtual DbSet<TAB_Pesquisa_Vaga_Disponibilidade> TAB_Pesquisa_Vaga_Disponibilidade { get; set; }
        public virtual DbSet<TAB_Pesquisa_Vaga_Tipo_Vinculo> TAB_Pesquisa_Vaga_Tipo_Vinculo { get; set; }
        public virtual DbSet<TAB_Pessoa_Fisica> TAB_Pessoa_Fisica { get; set; }
        public virtual DbSet<TAB_Pessoa_Fisica_Complemento> TAB_Pessoa_Fisica_Complemento { get; set; }
        public virtual DbSet<TAB_Pessoa_Fisica_Foto> TAB_Pessoa_Fisica_Foto { get; set; }
        public virtual DbSet<TAB_Pessoa_Fisica_Idioma> TAB_Pessoa_Fisica_Idioma { get; set; }
        public virtual DbSet<TAB_Pessoa_Fisica_Rede_Social> TAB_Pessoa_Fisica_Rede_Social { get; set; }
        public virtual DbSet<TAB_Pessoa_Fisica_Temp> TAB_Pessoa_Fisica_Temp { get; set; }
        public virtual DbSet<TAB_Pessoa_Fisica_Veiculo> TAB_Pessoa_Fisica_Veiculo { get; set; }
        public virtual DbSet<TAB_Regiao_Metropolitana> TAB_Regiao_Metropolitana { get; set; }
        public virtual DbSet<TAB_Regiao_Metropolitana_Cidade> TAB_Regiao_Metropolitana_Cidade { get; set; }
        public virtual DbSet<TAB_Regra_Substituicao_Integracao> TAB_Regra_Substituicao_Integracao { get; set; }
        public virtual DbSet<TAB_Requisicao_Integrador_Curriculo> TAB_Requisicao_Integrador_Curriculo { get; set; }
        public virtual DbSet<TAB_Situacao_Filial> TAB_Situacao_Filial { get; set; }
        public virtual DbSet<TAB_Status_Codigo_Desconto> TAB_Status_Codigo_Desconto { get; set; }
        public virtual DbSet<TAB_Status_Transacao> TAB_Status_Transacao { get; set; }
        public virtual DbSet<TAB_Tipo_Arquivo> TAB_Tipo_Arquivo { get; set; }
        public virtual DbSet<TAB_Tipo_Feriado> TAB_Tipo_Feriado { get; set; }
        public virtual DbSet<TAB_Tipo_Integrador> TAB_Tipo_Integrador { get; set; }
        public virtual DbSet<TAB_Tipo_Parceiro> TAB_Tipo_Parceiro { get; set; }
        public virtual DbSet<TAB_Transacao_Mensagem_Erro> TAB_Transacao_Mensagem_Erro { get; set; }
        public virtual DbSet<TAB_Usuario_Filial_Perfil> TAB_Usuario_Filial_Perfil { get; set; }
        public virtual DbSet<TAB_Usuario_Funcao> TAB_Usuario_Funcao { get; set; }
        public virtual DbSet<BNE_Integracao> BNE_Integracao { get; set; }
        public virtual DbSet<BNE_Integracao_Admissao> BNE_Integracao_Admissao { get; set; }
        public virtual DbSet<BNE_Integracao_Situacao> BNE_Integracao_Situacao { get; set; }
        public virtual DbSet<BNE_Tipo_Contrato> BNE_Tipo_Contrato { get; set; }
        public virtual DbSet<TAB_Area_BNE> TAB_Area_BNE { get; set; }
        public virtual DbSet<TAB_Banco> TAB_Banco { get; set; }
        public virtual DbSet<TAB_Categoria_Habilitacao> TAB_Categoria_Habilitacao { get; set; }
        public virtual DbSet<TAB_Categoria_Permissao> TAB_Categoria_Permissao { get; set; }
        public virtual DbSet<TAB_Centro_Servico> TAB_Centro_Servico { get; set; }
        public virtual DbSet<TAB_Cidade> TAB_Cidade { get; set; }
        public virtual DbSet<TAB_Classe_Salarial> TAB_Classe_Salarial { get; set; }
        public virtual DbSet<TAB_CNAE_Classe> TAB_CNAE_Classe { get; set; }
        public virtual DbSet<TAB_CNAE_Divisao> TAB_CNAE_Divisao { get; set; }
        public virtual DbSet<TAB_CNAE_Grupo> TAB_CNAE_Grupo { get; set; }
        public virtual DbSet<TAB_CNAE_Secao> TAB_CNAE_Secao { get; set; }
        public virtual DbSet<TAB_CNAE_Sub_Classe> TAB_CNAE_Sub_Classe { get; set; }
        public virtual DbSet<TAB_Deficiencia> TAB_Deficiencia { get; set; }
        public virtual DbSet<TAB_DePara_Email> TAB_DePara_Email { get; set; }
        public virtual DbSet<TAB_Email_Situacao> TAB_Email_Situacao { get; set; }
        public virtual DbSet<TAB_Endereco_Pessoa_Juridica> TAB_Endereco_Pessoa_Juridica { get; set; }
        public virtual DbSet<TAB_Escolaridade> TAB_Escolaridade { get; set; }
        public virtual DbSet<TAB_Estado> TAB_Estado { get; set; }
        public virtual DbSet<TAB_Estado_Civil> TAB_Estado_Civil { get; set; }
        public virtual DbSet<TAB_Flag> TAB_Flag { get; set; }
        public virtual DbSet<TAB_FPAS> TAB_FPAS { get; set; }
        public virtual DbSet<TAB_FPAS_Aliquota> TAB_FPAS_Aliquota { get; set; }
        public virtual DbSet<TAB_Funcao> TAB_Funcao { get; set; }
        public virtual DbSet<TAB_Funcao_Categoria> TAB_Funcao_Categoria { get; set; }
        public virtual DbSet<TAB_GPS> TAB_GPS { get; set; }
        public virtual DbSet<TAB_Grau_Escolaridade> TAB_Grau_Escolaridade { get; set; }
        public virtual DbSet<TAB_Inscricao_Estadual> TAB_Inscricao_Estadual { get; set; }
        public virtual DbSet<TAB_Mensagem> TAB_Mensagem { get; set; }
        public virtual DbSet<TAB_Motivo_Rescisao> TAB_Motivo_Rescisao { get; set; }
        public virtual DbSet<TAB_Nacionalidade> TAB_Nacionalidade { get; set; }
        public virtual DbSet<TAB_Natureza_Juridica> TAB_Natureza_Juridica { get; set; }
        public virtual DbSet<TAB_Operadora_Cartao> TAB_Operadora_Cartao { get; set; }
        public virtual DbSet<TAB_Operadora_Celular> TAB_Operadora_Celular { get; set; }
        public virtual DbSet<TAB_Parametro> TAB_Parametro { get; set; }
        public virtual DbSet<TAB_Parcela_Cartao> TAB_Parcela_Cartao { get; set; }
        public virtual DbSet<TAB_Permissao> TAB_Permissao { get; set; }
        public virtual DbSet<TAB_Pessoa_Juridica> TAB_Pessoa_Juridica { get; set; }
        public virtual DbSet<TAB_Pessoa_Juridica_Logo> TAB_Pessoa_Juridica_Logo { get; set; }
        public virtual DbSet<TAB_Plugin> TAB_Plugin { get; set; }
        public virtual DbSet<TAB_Plugins_Compatibilidade> TAB_Plugins_Compatibilidade { get; set; }
        public virtual DbSet<TAB_Porte_Empresa> TAB_Porte_Empresa { get; set; }
        public virtual DbSet<TAB_Raca> TAB_Raca { get; set; }
        public virtual DbSet<TAB_RAT> TAB_RAT { get; set; }
        public virtual DbSet<TAB_Rede_Social> TAB_Rede_Social { get; set; }
        public virtual DbSet<TAB_Regiao> TAB_Regiao { get; set; }
        public virtual DbSet<TAB_Sexo> TAB_Sexo { get; set; }
        public virtual DbSet<TAB_Simples> TAB_Simples { get; set; }
        public virtual DbSet<TAB_Sistema> TAB_Sistema { get; set; }
        public virtual DbSet<TAB_Status_Atividade> TAB_Status_Atividade { get; set; }
        public virtual DbSet<TAB_Status_Mensagem> TAB_Status_Mensagem { get; set; }
        public virtual DbSet<TAB_Tipo_Atividade> TAB_Tipo_Atividade { get; set; }
        public virtual DbSet<TAB_Tipo_Contato> TAB_Tipo_Contato { get; set; }
        public virtual DbSet<TAB_Tipo_Endereco> TAB_Tipo_Endereco { get; set; }
        public virtual DbSet<TAB_Tipo_Logo> TAB_Tipo_Logo { get; set; }
        public virtual DbSet<TAB_Tipo_Mensagem> TAB_Tipo_Mensagem { get; set; }
        public virtual DbSet<TAB_Tipo_Plugin> TAB_Tipo_Plugin { get; set; }
        public virtual DbSet<TAB_Tipo_Saida> TAB_Tipo_Saida { get; set; }
        public virtual DbSet<TAB_Tipo_Sanguineo> TAB_Tipo_Sanguineo { get; set; }
        public virtual DbSet<TAB_Tipo_Transacao> TAB_Tipo_Transacao { get; set; }
        public virtual DbSet<TAB_Tipo_Veiculo> TAB_Tipo_Veiculo { get; set; }
        public virtual DbSet<TAB_Tipo_Vinculo_Integracao> TAB_Tipo_Vinculo_Integracao { get; set; }
        public virtual DbSet<BNE_Gerente_Filial_New> BNE_Gerente_Filial_New { get; set; }
        public virtual DbSet<BNE_Propaganda_Email> BNE_Propaganda_Email { get; set; }
        public virtual DbSet<BNE_Teste_Cores> BNE_Teste_Cores { get; set; }
        public virtual DbSet<log_queue> log_queue { get; set; }
        public virtual DbSet<TAB_Campanha_CV_Desatualizado> TAB_Campanha_CV_Desatualizado { get; set; }
        public virtual DbSet<TAB_Filial_Logo> TAB_Filial_Logo { get; set; }
        public virtual DbSet<TAB_Mensagem_Mailing> TAB_Mensagem_Mailing { get; set; }
        public virtual DbSet<TAB_Repositorio_CV_XML> TAB_Repositorio_CV_XML { get; set; }
        public virtual DbSet<TAB_Repositorio_XML> TAB_Repositorio_XML { get; set; }
        public virtual DbSet<TAB_Substituicao_Integracao> TAB_Substituicao_Integracao { get; set; }
        public virtual DbSet<TAB_random> TAB_random { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BNE_Acao_Publicacao>()
                .Property(e => e.Des_Acao_Publicacao)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Acao_Publicacao>()
                .HasMany(e => e.BNE_Regra_Publicacao)
                .WithRequired(e => e.BNE_Acao_Publicacao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Adicional_Plano_Situacao>()
                .Property(e => e.Des_Adicional_Plano_Situacao)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Agradecimento>()
                .Property(e => e.Nme_Pessoa)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Agradecimento>()
                .Property(e => e.Eml_Pessoa)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Agradecimento>()
                .Property(e => e.Des_Mensagem)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Amplitude_Salarial>()
                .Property(e => e.Vlr_Mediana)
                .HasPrecision(10, 2);

            modelBuilder.Entity<BNE_Amplitude_Salarial>()
                .Property(e => e.Vlr_Amplitude_Inferior)
                .HasPrecision(10, 2);

            modelBuilder.Entity<BNE_Amplitude_Salarial>()
                .Property(e => e.Vlr_Amplitude_Superior)
                .HasPrecision(10, 2);

            modelBuilder.Entity<BNE_Amplitude_Salarial>()
                .Property(e => e.Vlr_Amplitude_Inferior_Alterada)
                .HasPrecision(10, 2);

            modelBuilder.Entity<BNE_Amplitude_Salarial>()
                .Property(e => e.Vlr_Amplitude_Superior_Alterada)
                .HasPrecision(10, 2);

            modelBuilder.Entity<BNE_Arquivo>()
                .Property(e => e.Nme_Arquivo)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Arquivo>()
                .Property(e => e.Dsc_Conteudo_Arquivo)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Arquivo>()
                .HasMany(e => e.BNE_Linha_Arquivo)
                .WithRequired(e => e.BNE_Arquivo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Cadastro_Parceiro>()
                .Property(e => e.Des_Login)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Cadastro_Parceiro>()
                .Property(e => e.Des_Senha)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Campanha>()
                .Property(e => e.Nme_Campanha)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Campanha>()
                .Property(e => e.Des_Mensagem)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Campanha>()
                .HasMany(e => e.BNE_Campanha_Curriculo)
                .WithRequired(e => e.BNE_Campanha)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Campanha_Curriculo>()
                .Property(e => e.Nme_Pessoa)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Campanha_Curriculo>()
                .Property(e => e.Num_DDD_Celular)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Campanha_Curriculo>()
                .Property(e => e.Num_Celular)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Campo_Publicacao>()
                .Property(e => e.Des_Campo_Publicacao)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Campo_Publicacao>()
                .HasMany(e => e.BNE_Regra_Campo_Publicacao)
                .WithRequired(e => e.BNE_Campo_Publicacao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Carta_Email>()
                .Property(e => e.Nme_Carta_Email)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Carta_Email>()
                .Property(e => e.Vlr_Carta_Email)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Carta_Email>()
                .Property(e => e.Des_Assunto)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Carta_SMS>()
                .Property(e => e.Nme_Carta_SMS)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Carta_SMS>()
                .Property(e => e.Vlr_Carta_SMS)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Celular>()
                .Property(e => e.Cod_Imei_Celular)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Celular>()
                .Property(e => e.Cod_Token_Celular)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Celular>()
                .HasMany(e => e.BNE_Celular_Selecionador)
                .WithRequired(e => e.BNE_Celular)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Celular_Selecionador>()
                .HasMany(e => e.BNE_Campanha)
                .WithRequired(e => e.BNE_Celular_Selecionador)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Cidade_Propaganda>()
                .Property(e => e.Eml_Remetente)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Cidade_Propaganda>()
                .Property(e => e.Nme_Remetente)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Cidade_Propaganda>()
                .Property(e => e.Des_Cargo_Remetente)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Cidade_Propaganda>()
                .Property(e => e.Num_Fone_Remetente)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Cidade_Propaganda>()
                .Property(e => e.Num_Fone_Geral_Remetente)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Codigo_Desconto>()
                .Property(e => e.Des_Codigo_Desconto)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Codigo_Desconto>()
                .Property(e => e.Des_Identificacao_Codigo)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Consultor_R1>()
                .Property(e => e.Nme_Consultor)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Consultor_R1>()
                .Property(e => e.Des_Email)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Consultor_R1>()
                .HasMany(e => e.BNE_Simulacao_R1)
                .WithRequired(e => e.BNE_Consultor_R1)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Consultor_R1>()
                .HasMany(e => e.BNE_Solicitacao_R1)
                .WithRequired(e => e.BNE_Consultor_R1)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Consultor_R1>()
                .HasMany(e => e.TAB_Cidade)
                .WithMany(e => e.BNE_Consultor_R1)
                .Map(m => m.ToTable("BNE_Consultor_Cidade_R1", "BNE").MapLeftKey("Idf_Consultor_R1").MapRightKey("Idf_Cidade"));

            modelBuilder.Entity<BNE_Conta_Twitter>()
                .Property(e => e.Des_Consumer_Key)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Conta_Twitter>()
                .Property(e => e.Des_Consumer_Secret)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Conta_Twitter>()
                .Property(e => e.Des_Access_Token)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Conta_Twitter>()
                .Property(e => e.Des_Access_Token_Secret)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Conta_Twitter>()
                .Property(e => e.Des_Login)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Conta_Twitter>()
                .Property(e => e.Des_Senha)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Conteudo_HTML>()
                .Property(e => e.Nme_Conteudo)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Conteudo_HTML>()
                .Property(e => e.Vlr_Conteudo)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Curriculo>()
                .Property(e => e.Vlr_Pretensao_Salarial)
                .HasPrecision(10, 2);

            modelBuilder.Entity<BNE_Curriculo>()
                .Property(e => e.Des_Mini_Curriculo)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Curriculo>()
                .Property(e => e.Obs_Curriculo)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Curriculo>()
                .Property(e => e.Des_Analise)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Curriculo>()
                .Property(e => e.Des_Sugestao_Carreira)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Curriculo>()
                .Property(e => e.Des_Cursos_Oferecidos)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Curriculo>()
                .Property(e => e.Vlr_Ultimo_Salario)
                .HasPrecision(10, 0);

            modelBuilder.Entity<BNE_Curriculo>()
                .Property(e => e.Des_IP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Curriculo>()
                .HasMany(e => e.BNE_Cadastro_Parceiro)
                .WithRequired(e => e.BNE_Curriculo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Curriculo>()
                .HasMany(e => e.BNE_Campanha_Curriculo)
                .WithRequired(e => e.BNE_Curriculo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Curriculo>()
                .HasMany(e => e.BNE_Conversas_Ativas)
                .WithRequired(e => e.BNE_Curriculo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Curriculo>()
                .HasMany(e => e.BNE_Vaga_Candidato)
                .WithRequired(e => e.BNE_Curriculo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Curriculo>()
                .HasMany(e => e.BNE_Curriculo_Classificacao)
                .WithRequired(e => e.BNE_Curriculo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Curriculo>()
                .HasMany(e => e.BNE_Curriculo_Disponibilidade_Cidade)
                .WithRequired(e => e.BNE_Curriculo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Curriculo>()
                .HasMany(e => e.BNE_Curriculo_Disponibilidade)
                .WithRequired(e => e.BNE_Curriculo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Curriculo>()
                .HasMany(e => e.BNE_Curriculo_Idioma)
                .WithRequired(e => e.BNE_Curriculo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Curriculo>()
                .HasMany(e => e.BNE_Curriculo_Correcao)
                .WithRequired(e => e.BNE_Curriculo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Curriculo>()
                .HasMany(e => e.BNE_Curriculo_Entrevista)
                .WithRequired(e => e.BNE_Curriculo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Curriculo>()
                .HasOptional(e => e.BNE_Curriculo_Fulltext)
                .WithRequired(e => e.BNE_Curriculo);

            modelBuilder.Entity<BNE_Curriculo>()
                .HasMany(e => e.BNE_Curriculo_Origem)
                .WithRequired(e => e.BNE_Curriculo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Curriculo>()
                .HasMany(e => e.BNE_Curriculo_Permissao)
                .WithRequired(e => e.BNE_Curriculo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Curriculo>()
                .HasMany(e => e.BNE_Curriculo_Publicacao)
                .WithRequired(e => e.BNE_Curriculo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Curriculo>()
                .HasMany(e => e.BNE_Intencao_Filial)
                .WithRequired(e => e.BNE_Curriculo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Curriculo>()
                .HasMany(e => e.BNE_Curriculo_Visualizacao)
                .WithRequired(e => e.BNE_Curriculo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Curriculo>()
                .HasMany(e => e.BNE_Curriculo_Visualizacao_Historico)
                .WithRequired(e => e.BNE_Curriculo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Curriculo>()
                .HasMany(e => e.BNE_Funcao_Pretendida)
                .WithRequired(e => e.BNE_Curriculo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Curriculo>()
                .HasMany(e => e.BNE_Vaga_Divulgacao)
                .WithRequired(e => e.BNE_Curriculo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Curriculo>()
                .HasMany(e => e.BNE_Rastreador_Curriculo)
                .WithRequired(e => e.BNE_Curriculo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Curriculo>()
                .HasMany(e => e.BNE_Inscricao_Curso)
                .WithRequired(e => e.BNE_Curriculo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Curriculo>()
                .HasMany(e => e.BNE_Historico_Publicacao_Curriculo)
                .WithRequired(e => e.BNE_Curriculo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Curriculo>()
                .HasMany(e => e.BNE_Curriculo_Quem_Me_Viu)
                .WithRequired(e => e.BNE_Curriculo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Curriculo>()
                .HasMany(e => e.BNE_Mobile_Token)
                .WithRequired(e => e.BNE_Curriculo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Curriculo>()
                .HasMany(e => e.BNE_Curriculo_Observacao)
                .WithRequired(e => e.BNE_Curriculo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Curriculo>()
                .HasMany(e => e.TAB_Parametro_Curriculo)
                .WithRequired(e => e.BNE_Curriculo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Curriculo>()
                .HasMany(e => e.TAB_Requisicao_Integrador_Curriculo)
                .WithRequired(e => e.BNE_Curriculo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Curriculo_Busca>()
                .Property(e => e.Des_Busca)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Curriculo_Classificacao>()
                .Property(e => e.Des_Observacao)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Curriculo_Correcao>()
                .Property(e => e.Des_Correcao)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Curriculo_Fulltext>()
                .Property(e => e.Des_MetaBusca)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Curriculo_Fulltext>()
                .Property(e => e.Des_Experiencia_Profissional)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Curriculo_Fulltext>()
                .Property(e => e.Des_Curso)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Curriculo_Fulltext>()
                .Property(e => e.Nme_Fonte)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Curriculo_Fulltext>()
                .Property(e => e.Raz_Social)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Curriculo_Fulltext>()
                .Property(e => e.Des_Metabusca_Rapida)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Curriculo_Observacao>()
                .Property(e => e.Des_Observacao)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Curriculo_Origem>()
                .Property(e => e.Des_IP)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Curriculo_Permissao>()
                .Property(e => e.Des_IP)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Curriculo_Visualizacao>()
                .Property(e => e.Des_IP)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Curriculo_Visualizacao_Historico>()
                .Property(e => e.Des_IP)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Curso_Modalidade_Tecla>()
                .Property(e => e.Des_Curso_Modalidade_Tecla)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Curso_Modalidade_Tecla>()
                .HasMany(e => e.BNE_Curso_Parceiro_Tecla)
                .WithRequired(e => e.BNE_Curso_Modalidade_Tecla)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Curso_Parceiro_Tecla>()
                .Property(e => e.Des_URL_Curso_Tecla)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Curso_Parceiro_Tecla>()
                .Property(e => e.Des_Curso)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Curso_Parceiro_Tecla>()
                .Property(e => e.Des_Conteudo)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Curso_Parceiro_Tecla>()
                .Property(e => e.Des_Caminho_Imagem_Banner)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Curso_Parceiro_Tecla>()
                .Property(e => e.Des_Caminho_Imagem_Miniatura)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Curso_Parceiro_Tecla>()
                .Property(e => e.Des_Titulo_Curso)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Curso_Parceiro_Tecla>()
                .Property(e => e.Des_Publico_Alvo)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Curso_Parceiro_Tecla>()
                .Property(e => e.Des_Instrutor_Curso)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Curso_Parceiro_Tecla>()
                .Property(e => e.Des_Assinatura_Instrutor_Curso)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Curso_Parceiro_Tecla>()
                .Property(e => e.Vlr_Curso_Sem_Desconto)
                .HasPrecision(10, 2);

            modelBuilder.Entity<BNE_Curso_Parceiro_Tecla>()
                .Property(e => e.Vlr_Curso)
                .HasPrecision(10, 2);

            modelBuilder.Entity<BNE_Curso_Parceiro_Tecla>()
                .Property(e => e.Vlr_Curso_Parcela)
                .HasPrecision(10, 2);

            modelBuilder.Entity<BNE_Curso_Parceiro_Tecla>()
                .HasMany(e => e.BNE_Inscricao_Curso)
                .WithRequired(e => e.BNE_Curso_Parceiro_Tecla)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Curso_Tecla>()
                .Property(e => e.Des_Curso_Tecla)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Curso_Tecla>()
                .HasMany(e => e.BNE_Curso_Funcao_Tecla)
                .WithRequired(e => e.BNE_Curso_Tecla)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Curso_Tecla>()
                .HasMany(e => e.BNE_Curso_Parceiro_Tecla)
                .WithRequired(e => e.BNE_Curso_Tecla)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Email_Destinatario>()
                .Property(e => e.Des_Email)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Email_Destinatario>()
                .Property(e => e.Nme_Pessoa)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Email_Destinatario>()
                .Property(e => e.Num_DDD_Telefone)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Email_Destinatario>()
                .Property(e => e.Num_Telefone)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Email_Destinatario>()
                .HasMany(e => e.BNE_Email_Destinatario_Cidade)
                .WithRequired(e => e.BNE_Email_Destinatario)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Empresa_Home>()
                .Property(e => e.Des_Nome_URL)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Empresa_Home>()
                .Property(e => e.Des_Caminho_Imagem)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Experiencia_Profissional>()
                .Property(e => e.Raz_Social)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Experiencia_Profissional>()
                .Property(e => e.Des_Funcao_Exercida)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Experiencia_Profissional>()
                .Property(e => e.Des_Atividade)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Fale_Presidente>()
                .Property(e => e.Nme_Usuario)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Fale_Presidente>()
                .Property(e => e.Num_CPF)
                .HasPrecision(11, 0);

            modelBuilder.Entity<BNE_Fale_Presidente>()
                .Property(e => e.Eml_Pessoa)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Fale_Presidente>()
                .Property(e => e.Des_Assunto)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Fale_Presidente>()
                .Property(e => e.Des_Mensagem)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Financeiro>()
                .Property(e => e.Des_Caminho)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Formacao>()
                .Property(e => e.Des_Endereco)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Formacao>()
                .Property(e => e.Des_Curso)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Formacao>()
                .Property(e => e.Des_Fonte)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Funcao_Erro_Sinonimo>()
                .Property(e => e.Des_Funcao_Erro_Sinonimo)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Funcao_Pretendida>()
                .Property(e => e.Des_Funcao_Pretendida)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Grupo_Cidade>()
                .HasMany(e => e.BNE_Email_Destinatario_Cidade)
                .WithRequired(e => e.BNE_Grupo_Cidade)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Grupo_Cidade>()
                .HasMany(e => e.BNE_Lista_Cidade)
                .WithRequired(e => e.BNE_Grupo_Cidade)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Historico_Publicacao_Curriculo>()
                .Property(e => e.Des_Historico)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Historico_Publicacao_Vaga>()
                .Property(e => e.Des_Historico)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Indicacao_Filial>()
                .Property(e => e.Nme_Empresa)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Mensagem_CS>()
                .Property(e => e.Des_Mensagem)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Mensagem_CS>()
                .Property(e => e.Des_Email_Destinatario)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Mensagem_CS>()
                .Property(e => e.Des_Email_Assunto)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Mensagem_CS>()
                .Property(e => e.Des_Email_Remetente)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Mensagem_CS>()
                .Property(e => e.Num_DDD_Celular)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Mensagem_CS>()
                .Property(e => e.Num_Celular)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Mensagem_CS>()
                .Property(e => e.Nme_Anexo)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Mensagem_CS>()
                .Property(e => e.Des_Obs)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Mensagem_Sistema>()
                .Property(e => e.Des_Mensagem_Sistema)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Mini_Curriculo>()
                .Property(e => e.Des_Mini_Curriculo)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Mobile_Token>()
                .Property(e => e.Cod_Token)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Mobile_Token>()
                .Property(e => e.Cod_Dispositivo)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Noticia>()
                .Property(e => e.Des_Noticia)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Noticia>()
                .Property(e => e.Nme_Titulo_Noticia)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Noticia>()
                .Property(e => e.Nme_Link_Noticia)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Noticia>()
                .HasMany(e => e.BNE_Noticia_Visualizacao)
                .WithRequired(e => e.BNE_Noticia)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Operadora>()
                .Property(e => e.Des_Operadora)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Pagamento>()
                .Property(e => e.Des_Identificador)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Pagamento>()
                .Property(e => e.Des_Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Pagamento>()
                .Property(e => e.Vlr_Pagamento)
                .HasPrecision(10, 2);

            modelBuilder.Entity<BNE_Pagamento>()
                .Property(e => e.Cod_Guid)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Pagamento>()
                .Property(e => e.Num_Nota_Fiscal)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Pagamento_Situacao>()
                .Property(e => e.Des_Pagamento_Situacao)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Pagamento_Situacao>()
                .HasMany(e => e.BNE_Pagamento)
                .WithRequired(e => e.BNE_Pagamento_Situacao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Palavra_Chave>()
                .Property(e => e.Des_Palavra_Chave)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Palavra_Chave>()
                .HasMany(e => e.BNE_Vaga_Palavra_Chave)
                .WithRequired(e => e.BNE_Palavra_Chave)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Palavra_Proibida>()
                .Property(e => e.Des_Palavra_Proibida)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Palavra_Publicacao>()
                .Property(e => e.Des_Palavra_Publicacao)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Palavra_Publicacao>()
                .Property(e => e.Des_Palavra_Substituicao)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Parametro_Busca_CV>()
                .Property(e => e.Nme_SP_Busca)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Parceiro>()
                .Property(e => e.Des_Parceiro)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Parceiro>()
                .Property(e => e.Num_CPF)
                .HasPrecision(11, 0);

            modelBuilder.Entity<BNE_Parceiro>()
                .Property(e => e.Num_CNPJ)
                .HasPrecision(14, 0);

            modelBuilder.Entity<BNE_Parceiro>()
                .Property(e => e.Des_Email)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Parceiro_Tecla>()
                .Property(e => e.Nme_Parceiro_Tecla)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Parceiro_Tecla>()
                .Property(e => e.Des_URL_Cadastro)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Parceiro_Tecla>()
                .Property(e => e.Des_URL_Autenticacao)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Parceiro_Tecla>()
                .HasMany(e => e.BNE_Cadastro_Parceiro)
                .WithRequired(e => e.BNE_Parceiro_Tecla)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Parceiro_Tecla>()
                .HasMany(e => e.BNE_Curso_Parceiro_Tecla)
                .WithRequired(e => e.BNE_Parceiro_Tecla)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Pesquisa_Vaga_log>()
                .Property(e => e.Des_Metabusca_Rapida)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Pesquisa_Vaga_log>()
                .Property(e => e.Sig_Estado)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Pesquisa_Vaga_log>()
                .Property(e => e.Vlr_Salario_Min)
                .HasPrecision(10, 2);

            modelBuilder.Entity<BNE_Pesquisa_Vaga_log>()
                .Property(e => e.Vlr_Salario_Max)
                .HasPrecision(10, 2);

            modelBuilder.Entity<BNE_Pesquisa_Vaga_log>()
                .Property(e => e.Idfs_Disponibilidade)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Pesquisa_Vaga_log>()
                .Property(e => e.Idfs_Tipo_Vinculo)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Pesquisa_Vaga_log>()
                .Property(e => e.Raz_Social)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Pesquisa_Vaga_log>()
                .Property(e => e.Cod_Vaga)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Plano>()
                .Property(e => e.Des_Plano)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Plano>()
                .Property(e => e.Vlr_Base)
                .HasPrecision(10, 2);

            modelBuilder.Entity<BNE_Plano>()
                .Property(e => e.Vlr_Base_Minimo)
                .HasPrecision(10, 2);

            modelBuilder.Entity<BNE_Plano>()
                .HasMany(e => e.BNE_Codigo_Desconto_Plano)
                .WithRequired(e => e.BNE_Plano)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Plano>()
                .HasMany(e => e.BNE_Plano_Adquirido)
                .WithRequired(e => e.BNE_Plano)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Plano_Adquirido>()
                .Property(e => e.Vlr_Base)
                .HasPrecision(9, 2);

            modelBuilder.Entity<BNE_Plano_Adquirido>()
                .HasMany(e => e.BNE_Adicional_Plano)
                .WithRequired(e => e.BNE_Plano_Adquirido)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Plano_Adquirido>()
                .HasMany(e => e.BNE_Plano_Parcela)
                .WithRequired(e => e.BNE_Plano_Adquirido)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Plano_Adquirido>()
                .HasMany(e => e.BNE_Plano_Adquirido_Detalhes)
                .WithRequired(e => e.BNE_Plano_Adquirido)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Plano_Adquirido_Detalhes>()
                .Property(e => e.Nme_Res_Plano_Adquirido)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Plano_Adquirido_Detalhes>()
                .Property(e => e.Num_Res_DDD_Telefone)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Plano_Adquirido_Detalhes>()
                .Property(e => e.Num_Res_Telefone)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Plano_Adquirido_Detalhes>()
                .Property(e => e.Eml_Envio_Boleto)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Plano_Adquirido_Detalhes>()
                .Property(e => e.Des_Observacao)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Plano_Forma_Pagamento>()
                .Property(e => e.Des_Plano_Forma_Pagamento)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Plano_Forma_Pagamento>()
                .Property(e => e.Txt_Formula)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Plano_Forma_Pagamento>()
                .HasMany(e => e.BNE_Plano)
                .WithRequired(e => e.BNE_Plano_Forma_Pagamento)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Plano_Parcela>()
                .Property(e => e.Vlr_Parcela)
                .HasPrecision(10, 2);

            modelBuilder.Entity<BNE_Plano_Parcela_Situacao>()
                .Property(e => e.Des_Status_Pagamento)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Plano_Parcela_Situacao>()
                .HasMany(e => e.BNE_Plano_Parcela)
                .WithRequired(e => e.BNE_Plano_Parcela_Situacao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Plano_Situacao>()
                .Property(e => e.Des_Plano_Situacao)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Plano_Situacao>()
                .HasMany(e => e.BNE_Plano_Adquirido)
                .WithRequired(e => e.BNE_Plano_Situacao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Plano_Tipo>()
                .Property(e => e.Des_Plano_Tipo)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Plano_Tipo>()
                .HasMany(e => e.BNE_Plano)
                .WithRequired(e => e.BNE_Plano_Tipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Propaganda>()
                .Property(e => e.Nme_Propaganda)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Propaganda>()
                .Property(e => e.Des_Email_Propaganda)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Propaganda>()
                .Property(e => e.Des_SMS_Propaganda)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Propaganda>()
                .Property(e => e.Des_Titulo_Propaganda)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Propaganda>()
                .Property(e => e.Eml_Remetente)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Rastreador>()
                .Property(e => e.Des_Palavra_Chave)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Rastreador>()
                .Property(e => e.Vlr_Salario_Inicio)
                .HasPrecision(19, 4);

            modelBuilder.Entity<BNE_Rastreador>()
                .Property(e => e.Vlr_Salario_Fim)
                .HasPrecision(19, 4);

            modelBuilder.Entity<BNE_Rastreador>()
                .Property(e => e.Des_Bairro)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Rastreador>()
                .Property(e => e.Raz_Social)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Rastreador>()
                .Property(e => e.Des_CEP_Fim)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Rastreador>()
                .Property(e => e.Des_CEP_Inicio)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Rastreador>()
                .HasMany(e => e.BNE_Rastreador_Disponibilidade)
                .WithRequired(e => e.BNE_Rastreador)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Rastreador>()
                .HasMany(e => e.BNE_Rastreador_Idioma)
                .WithRequired(e => e.BNE_Rastreador)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Rastreador>()
                .HasMany(e => e.BNE_Rastreador_Curriculo)
                .WithRequired(e => e.BNE_Rastreador)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Rastreador_Vaga>()
                .Property(e => e.Des_Palavra_Chave)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Rede_Social_Conta>()
                .Property(e => e.Des_Login)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Rede_Social_Conta>()
                .Property(e => e.Des_Senha)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Rede_Social_Conta>()
                .Property(e => e.Des_Comunidade)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Rede_Social_CS>()
                .Property(e => e.Des_Rede_Social)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Rede_Social_CS>()
                .HasMany(e => e.BNE_Rede_Social_Conta)
                .WithRequired(e => e.BNE_Rede_Social_CS)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Rede_Social_CS>()
                .HasMany(e => e.BNE_Vaga_Rede_Social)
                .WithRequired(e => e.BNE_Rede_Social_CS)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Rede_Social_CS>()
                .HasMany(e => e.TAB_Pessoa_Fisica_Rede_Social)
                .WithRequired(e => e.BNE_Rede_Social_CS)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Regra_Publicacao>()
                .HasMany(e => e.BNE_Regra_Campo_Publicacao)
                .WithRequired(e => e.BNE_Regra_Publicacao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Resposta_Automatica>()
                .Property(e => e.Des_Resposta_Automatica)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Resposta_Automatica>()
                .Property(e => e.Nme_Resposta_Automatica)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Revidor>()
                .HasMany(e => e.BNE_Curriculo_Fila)
                .WithOptional(e => e.BNE_Revidor)
                .HasForeignKey(e => e.Idf_Revidor_Order);

            modelBuilder.Entity<BNE_Rota>()
                .Property(e => e.Nme_Rota)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Rota>()
                .Property(e => e.Des_URL)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Rota>()
                .Property(e => e.Des_Caminho_Fisico)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Simulacao_R1>()
                .Property(e => e.Nme_Pessoa)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Simulacao_R1>()
                .Property(e => e.Num_DDD_Telefone)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Simulacao_R1>()
                .Property(e => e.Num_Telefone)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Simulacao_R1>()
                .Property(e => e.Eml_Pessoa)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Simulacao_R1>()
                .Property(e => e.Vlr_Salario_Max)
                .HasPrecision(10, 2);

            modelBuilder.Entity<BNE_Simulacao_R1>()
                .Property(e => e.Vlr_Salario_Min)
                .HasPrecision(10, 2);

            modelBuilder.Entity<BNE_Simulacao_R1>()
                .Property(e => e.Vlr_Taxa_Abertura)
                .HasPrecision(10, 2);

            modelBuilder.Entity<BNE_Simulacao_R1>()
                .Property(e => e.Vlr_Servico_Prestado)
                .HasPrecision(10, 2);

            modelBuilder.Entity<BNE_Simulacao_R1>()
                .Property(e => e.Vlr_Bonus_Solicitacao_Online)
                .HasPrecision(10, 2);

            modelBuilder.Entity<BNE_Simulacao_R1>()
                .Property(e => e.Vlr_Total)
                .HasPrecision(10, 2);

            modelBuilder.Entity<BNE_Simulacao_R1>()
                .Property(e => e.Vlr_Margem_Percentual_Servico)
                .HasPrecision(10, 2);

            modelBuilder.Entity<BNE_Situacao_Curriculo>()
                .Property(e => e.Des_Situacao_Curriculo)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Situacao_Curriculo>()
                .HasMany(e => e.BNE_Curriculo)
                .WithRequired(e => e.BNE_Situacao_Curriculo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Situacao_Formacao>()
                .Property(e => e.Des_Situacao_Formacao)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Solicitacao_R1>()
                .Property(e => e.Nme_Solicitante)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Solicitacao_R1>()
                .Property(e => e.Num_DDD_Solicitante)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Solicitacao_R1>()
                .Property(e => e.Num_Telefone_Solicitante)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Solicitacao_R1>()
                .Property(e => e.Eml_Solicitante)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Solicitacao_R1>()
                .Property(e => e.Des_Atividade)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Solicitacao_R1>()
                .Property(e => e.Des_Requisito_Obrigatorio)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Solicitacao_R1>()
                .Property(e => e.Des_Requisito_Desejavel)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Solicitacao_R1>()
                .Property(e => e.Des_Conhecimento_Informatica)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Solicitacao_R1>()
                .Property(e => e.Vlr_Salario_De)
                .HasPrecision(19, 4);

            modelBuilder.Entity<BNE_Solicitacao_R1>()
                .Property(e => e.Vlr_Salario_Ate)
                .HasPrecision(19, 4);

            modelBuilder.Entity<BNE_Solicitacao_R1>()
                .Property(e => e.Des_Beneficio)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Solicitacao_R1>()
                .Property(e => e.Des_Informacao_Adicional)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Status_Curriculo_Vaga>()
                .Property(e => e.Des_Status_Curriculo_Vaga)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Status_Mensagem_CS>()
                .Property(e => e.Des_Status_Mensagem)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Status_Mensagem_CS>()
                .HasMany(e => e.BNE_Mensagem_CS)
                .WithRequired(e => e.BNE_Status_Mensagem_CS)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Template>()
                .Property(e => e.Nme_Template)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Tipo_Adicional>()
                .Property(e => e.Des_Tipo_Adicional)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Tipo_Adicional>()
                .HasMany(e => e.BNE_Adicional_Plano)
                .WithRequired(e => e.BNE_Tipo_Adicional)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Tipo_Codigo_Desconto>()
                .Property(e => e.Des_Tipo_Codigo_Desconto)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Tipo_Codigo_Desconto>()
                .HasMany(e => e.BNE_Codigo_Desconto_Plano)
                .WithRequired(e => e.BNE_Tipo_Codigo_Desconto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Tipo_Curriculo>()
                .Property(e => e.Des_Tipo_Curriculo)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Tipo_Curriculo>()
                .HasMany(e => e.BNE_Curriculo)
                .WithRequired(e => e.BNE_Tipo_Curriculo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Tipo_Mensagem_CS>()
                .Property(e => e.Des_Tipo_Mensagem)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Tipo_Mensagem_CS>()
                .HasMany(e => e.BNE_Mensagem_CS)
                .WithRequired(e => e.BNE_Tipo_Mensagem_CS)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Tipo_Origem>()
                .Property(e => e.Des_Tipo_Origem)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Tipo_Origem>()
                .HasMany(e => e.TAB_Origem)
                .WithRequired(e => e.BNE_Tipo_Origem)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Tipo_Pagamento>()
                .Property(e => e.Des_Tipo_Pagamaneto)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Tipo_Perfil>()
                .Property(e => e.Des_Tipo_Perfil)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Tipo_Perfil>()
                .HasMany(e => e.TAB_Perfil)
                .WithRequired(e => e.BNE_Tipo_Perfil)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Tipo_Publicacao>()
                .Property(e => e.Des_Tipo_Publicacao)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Tipo_Publicacao>()
                .HasMany(e => e.BNE_Campo_Publicacao)
                .WithRequired(e => e.BNE_Tipo_Publicacao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Tipo_Sistema_Mobile>()
                .Property(e => e.Des_Tipo_Sistema_Mobile)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Tipo_Sistema_Mobile>()
                .HasMany(e => e.BNE_Mobile_Token)
                .WithRequired(e => e.BNE_Tipo_Sistema_Mobile)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Tipo_Vinculo>()
                .Property(e => e.Des_Tipo_Vinculo)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Tipo_Vinculo>()
                .HasMany(e => e.BNE_Vaga_Tipo_Vinculo)
                .WithRequired(e => e.BNE_Tipo_Vinculo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Transacao>()
                .Property(e => e.Vlr_Documento)
                .HasPrecision(10, 2);

            modelBuilder.Entity<BNE_Transacao>()
                .Property(e => e.Num_Cartao_Credito)
                .HasPrecision(19, 0);

            modelBuilder.Entity<BNE_Transacao>()
                .Property(e => e.Des_IP_Comprador)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Transacao>()
                .Property(e => e.Des_Agencia_Debito)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Transacao>()
                .Property(e => e.Des_Conta_Corrente_Debito)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Transacao>()
                .Property(e => e.Nme_Titular_Conta_Corrente_Debito)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Transacao>()
                .Property(e => e.Num_CPF_Titular_Conta_Corrente_Debito)
                .HasPrecision(11, 0);

            modelBuilder.Entity<BNE_Transacao>()
                .Property(e => e.Des_Transacao)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Transacao>()
                .Property(e => e.Des_Mensagem_Captura)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Transacao>()
                .Property(e => e.Num_CNPJ_Titular_Conta_Corrente_Debito)
                .HasPrecision(14, 0);

            modelBuilder.Entity<BNE_Transacao>()
                .HasMany(e => e.BNE_Linha_Arquivo)
                .WithRequired(e => e.BNE_Transacao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Transacao>()
                .HasMany(e => e.BNE_Transacao_Resposta)
                .WithRequired(e => e.BNE_Transacao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Transacao>()
                .HasMany(e => e.BNE_Transacao_Retorno)
                .WithRequired(e => e.BNE_Transacao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Transacao_Resposta>()
                .Property(e => e.Des_Resultado_Solicitacao_Aprovacao)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Transacao_Resposta>()
                .Property(e => e.Des_Codigo_Autorizacao)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Transacao_Resposta>()
                .Property(e => e.Des_Transacao)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Transacao_Resposta>()
                .Property(e => e.Des_Cartao_Mascarado)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Transacao_Resposta>()
                .Property(e => e.Num_Sequencial_Unico)
                .HasPrecision(6, 0);

            modelBuilder.Entity<BNE_Transacao_Resposta>()
                .Property(e => e.Des_Comprovante_Administradora)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Transacao_Resposta>()
                .Property(e => e.Des_Nacionalidade_Emissor)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Transacao_Retorno>()
                .Property(e => e.Des_Autorizacao)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Transacao_Retorno>()
                .Property(e => e.Des_Motivo_Nao_Finalizada)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Transacao_Retorno>()
                .Property(e => e.Des_Nao_Finalizada)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Usuario>()
                .Property(e => e.Sen_Usuario)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Usuario>()
                .Property(e => e.Des_Session_ID)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Usuario>()
                .HasMany(e => e.TAB_Atividade)
                .WithOptional(e => e.BNE_Usuario)
                .HasForeignKey(e => e.Idf_Usuario_Gerador);

            modelBuilder.Entity<BNE_Usuario_Filial>()
                .Property(e => e.Des_Funcao)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Usuario_Filial>()
                .Property(e => e.Num_Ramal)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Usuario_Filial>()
                .Property(e => e.Num_DDD_Comercial)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Usuario_Filial>()
                .Property(e => e.Num_Comercial)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Usuario_Filial>()
                .Property(e => e.Eml_Comercial)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Vaga>()
                .Property(e => e.Cod_Vaga)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Vaga>()
                .Property(e => e.Vlr_Salario_De)
                .HasPrecision(19, 4);

            modelBuilder.Entity<BNE_Vaga>()
                .Property(e => e.Eml_Vaga)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Vaga>()
                .Property(e => e.Des_Requisito)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Vaga>()
                .Property(e => e.Nme_Empresa)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Vaga>()
                .Property(e => e.Des_Beneficio)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Vaga>()
                .Property(e => e.Des_Atribuicoes)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Vaga>()
                .Property(e => e.Num_DDD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Vaga>()
                .Property(e => e.Num_Telefone)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Vaga>()
                .Property(e => e.Des_Funcao)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Vaga>()
                .Property(e => e.Vlr_Salario_Para)
                .HasPrecision(19, 4);

            modelBuilder.Entity<BNE_Vaga>()
                .HasMany(e => e.BNE_Historico_Publicacao_Vaga)
                .WithRequired(e => e.BNE_Vaga)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Vaga>()
                .HasMany(e => e.BNE_Rastreador_Resultado_Vaga)
                .WithRequired(e => e.BNE_Vaga)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Vaga>()
                .HasMany(e => e.BNE_Vaga_Integracao)
                .WithRequired(e => e.BNE_Vaga)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Vaga>()
                .HasOptional(e => e.BNE_Vaga_Fulltext)
                .WithRequired(e => e.BNE_Vaga);

            modelBuilder.Entity<BNE_Vaga>()
                .HasMany(e => e.BNE_Vaga_Divulgacao)
                .WithRequired(e => e.BNE_Vaga)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Vaga>()
                .HasMany(e => e.BNE_Vaga_Home)
                .WithRequired(e => e.BNE_Vaga)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Vaga>()
                .HasMany(e => e.BNE_Vaga_Candidato)
                .WithRequired(e => e.BNE_Vaga)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Vaga>()
                .HasMany(e => e.BNE_Vaga_Disponibilidade)
                .WithRequired(e => e.BNE_Vaga)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Vaga>()
                .HasMany(e => e.BNE_Vaga_Palavra_Chave)
                .WithRequired(e => e.BNE_Vaga)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Vaga>()
                .HasMany(e => e.BNE_Vaga_Pergunta)
                .WithRequired(e => e.BNE_Vaga)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Vaga>()
                .HasMany(e => e.BNE_Vaga_Rede_Social)
                .WithRequired(e => e.BNE_Vaga)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Vaga>()
                .HasMany(e => e.BNE_Vaga_Tipo_Vinculo)
                .WithRequired(e => e.BNE_Vaga)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Vaga_Candidato>()
                .HasMany(e => e.BNE_Vaga_Candidato_Pergunta)
                .WithRequired(e => e.BNE_Vaga_Candidato)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Vaga_Fulltext>()
                .Property(e => e.Des_Metabusca_Rapida)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Vaga_Home>()
                .Property(e => e.Des_Funcao_Vaga_Home)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Vaga_Home>()
                .Property(e => e.Des_Vaga_Home)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Vaga_Home>()
                .Property(e => e.Cod_Vaga)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Vaga_Integracao>()
                .Property(e => e.Cod_Vaga_Integrador)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Vaga_Pergunta>()
                .Property(e => e.Des_Vaga_Pergunta)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Vaga_Pergunta>()
                .HasMany(e => e.BNE_Vaga_Candidato_Pergunta)
                .WithRequired(e => e.BNE_Vaga_Pergunta)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Vaga_Rede_Social>()
                .Property(e => e.Des_Vaga_Rede_Social)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Chave_Cielo>()
                .Property(e => e.Cod_Chave_Cielo)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Chave_Cielo>()
                .Property(e => e.Url_web_Service)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Chave_Cielo>()
                .Property(e => e.Cod_Filiacao)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Cobranca_Boleto>()
                .Property(e => e.Num_CNPJ_Cedente)
                .HasPrecision(14, 0);

            modelBuilder.Entity<GLO_Cobranca_Boleto>()
                .Property(e => e.Num_CPF_Cedente)
                .HasPrecision(11, 0);

            modelBuilder.Entity<GLO_Cobranca_Boleto>()
                .Property(e => e.Num_Agencia_Bancaria)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Cobranca_Boleto>()
                .Property(e => e.Num_Conta)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Cobranca_Boleto>()
                .Property(e => e.Num_DV_Conta)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Cobranca_Boleto>()
                .Property(e => e.Raz_Social_Cedente)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Cobranca_Boleto>()
                .Property(e => e.Nme_Pessoa_Cedente)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Cobranca_Boleto>()
                .Property(e => e.Vlr_Boleto)
                .HasPrecision(10, 2);

            modelBuilder.Entity<GLO_Cobranca_Boleto>()
                .Property(e => e.Num_Nosso_Numero)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Cobranca_Boleto>()
                .Property(e => e.Num_CPF_Sacado)
                .HasPrecision(11, 0);

            modelBuilder.Entity<GLO_Cobranca_Boleto>()
                .Property(e => e.Num_CNPJ_Sacado)
                .HasPrecision(14, 0);

            modelBuilder.Entity<GLO_Cobranca_Boleto>()
                .Property(e => e.Nme_Pessoa_Sacado)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Cobranca_Boleto>()
                .Property(e => e.Raz_Social_Sacado)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Cobranca_Boleto>()
                .Property(e => e.End_Email_Sacado)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Cobranca_Boleto>()
                .Property(e => e.Des_Logradouro_Sacado)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Cobranca_Boleto>()
                .Property(e => e.Num_Endereço_Sacado)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Cobranca_Boleto>()
                .Property(e => e.Des_Complemento_Sacado)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Cobranca_Boleto>()
                .Property(e => e.Num_Cep_Sacado)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Cobranca_Boleto>()
                .Property(e => e.Des_Bairro_Sacado)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Cobranca_Boleto>()
                .Property(e => e.Des_Instrucao_Caixa)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Cobranca_Boleto>()
                .Property(e => e.Cod_Barras)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Cobranca_Boleto>()
                .Property(e => e.Arq_Boleto)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Cobranca_Boleto>()
                .HasMany(e => e.BNE_Linha_Arquivo)
                .WithRequired(e => e.GLO_Cobranca_Boleto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GLO_Cobranca_Boleto_Arquivo_Remessa>()
                .Property(e => e.Arq_Remessa)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Cobranca_Boleto_Arquivo_Remessa>()
                .Property(e => e.Nme_Arquivo)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Cobranca_Boleto_Arquivo_Retorno>()
                .Property(e => e.Arq_Retorno)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Cobranca_Boleto_Arquivo_Retorno>()
                .Property(e => e.Nme_Arquivo)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Cobranca_Boleto_LOG>()
                .Property(e => e.Des_Transacao)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Cobranca_Cartao>()
                .Property(e => e.Cod_Filiacao)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Cobranca_Cartao>()
                .Property(e => e.Cod_Dta_Juliana)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Cobranca_Cartao>()
                .Property(e => e.Cod_TID)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Cobranca_Cartao>()
                .Property(e => e.Vlr_Recebimento)
                .HasPrecision(10, 2);

            modelBuilder.Entity<GLO_Cobranca_Cartao_LOG>()
                .Property(e => e.Des_Cobranca)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Convenio_Bancario>()
                .Property(e => e.Num_CNPJ)
                .HasPrecision(14, 0);

            modelBuilder.Entity<GLO_Convenio_Bancario>()
                .Property(e => e.Num_Convenio)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Convenio_Bancario>()
                .Property(e => e.Num_Sequencia)
                .HasPrecision(13, 0);

            modelBuilder.Entity<GLO_Licenca_Cobre_Bem>()
                .Property(e => e.Num_CNPJ)
                .HasPrecision(14, 0);

            modelBuilder.Entity<GLO_Licenca_Cobre_Bem>()
                .Property(e => e.Arq_Licenca)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Mensagem_Operadora_Cartao>()
                .Property(e => e.Cod_Mensagem_Cartao)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Mensagem_Operadora_Cartao>()
                .Property(e => e.Des_Mensagem_Cartao)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Mensagem_Operadora_Cartao>()
                .Property(e => e.Des_Global_Mensagem)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Mensagem_Retorno_Boleto>()
                .Property(e => e.Des_Status)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Mensagem_Retorno_Boleto>()
                .Property(e => e.Cod_Status)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Mensagem_Retorno_Boleto>()
                .HasMany(e => e.GLO_Cobranca_Boleto_Lista_Remessa)
                .WithOptional(e => e.GLO_Mensagem_Retorno_Boleto)
                .HasForeignKey(e => e.Idf_Status_Cobranca_Boleto);

            modelBuilder.Entity<GLO_Mensagem_Retorno_Boleto>()
                .HasMany(e => e.GLO_Cobranca_Boleto_Lista_Retorno)
                .WithOptional(e => e.GLO_Mensagem_Retorno_Boleto)
                .HasForeignKey(e => e.Idf_Status_Cobranca_Boleto);

            modelBuilder.Entity<GLO_Mensagem_Retorno_Cartao>()
                .Property(e => e.Cod_Mensagem)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Mensagem_Retorno_Cartao>()
                .Property(e => e.Des_Mensagem)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Mensagem_Retorno_Cartao>()
                .Property(e => e.Des_Mensagem_Global)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Status_Transacao>()
                .Property(e => e.Des_Status_Transacao)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Transacao>()
                .Property(e => e.Cod_Guid)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Transacao>()
                .Property(e => e.Url_Retorno)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Transacao>()
                .Property(e => e.Des_Identificacao)
                .IsUnicode(false);

            modelBuilder.Entity<GLO_Transacao>()
                .HasMany(e => e.GLO_Cobranca_Cartao)
                .WithRequired(e => e.GLO_Transacao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LOG_Exclusao_CPF>()
                .Property(e => e.Num_CPF)
                .HasPrecision(11, 0);

            modelBuilder.Entity<LOG_Exclusao_CPF>()
                .Property(e => e.Des_Usuario_Exclusao)
                .IsUnicode(false);

            modelBuilder.Entity<LOG_Exclusao_CPF>()
                .Property(e => e.Nme_Maquina)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Atividade>()
                .Property(e => e.Des_Caminho_Arquivo_Upload)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Atividade>()
                .Property(e => e.Des_Caminho_Arquivo_Gerado)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Atividade>()
                .Property(e => e.Des_Erro)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Avaliacao>()
                .Property(e => e.Des_Avaliacao)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Avaliacao>()
                .HasMany(e => e.BNE_Curriculo_Classificacao)
                .WithRequired(e => e.TAB_Avaliacao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Campo_Integrador>()
                .Property(e => e.Des_Campo_Integrador)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Campo_Integrador>()
                .HasMany(e => e.TAB_Regra_Substituicao_Integracao)
                .WithOptional(e => e.TAB_Campo_Integrador)
                .HasForeignKey(e => e.Idf_Campo_Integrador);

            modelBuilder.Entity<TAB_Campo_Integrador>()
                .HasMany(e => e.TAB_Regra_Substituicao_Integracao1)
                .WithOptional(e => e.TAB_Campo_Integrador1)
                .HasForeignKey(e => e.Idf_Campo_Integrador);

            modelBuilder.Entity<TAB_Contato>()
                .Property(e => e.Nme_Contato)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Contato>()
                .Property(e => e.Tip_Contato)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Contato>()
                .Property(e => e.Num_DDD_Celular)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Contato>()
                .Property(e => e.Num_Celular)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Contato>()
                .Property(e => e.Num_DDD_Telefone)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Contato>()
                .Property(e => e.Num_Telefone)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Contato>()
                .Property(e => e.Num_Ramal_Telefone)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Contato>()
                .Property(e => e.Num_DDD_Fax)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Contato>()
                .Property(e => e.Num_Fax)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Contato>()
                .Property(e => e.Eml_Contato)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Curso>()
                .Property(e => e.Des_Curso)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Curso>()
                .Property(e => e.Cod_Curso)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Curso>()
                .HasMany(e => e.BNE_Rastreador)
                .WithOptional(e => e.TAB_Curso)
                .HasForeignKey(e => e.Idf_Curso_Outros_Cursos);

            modelBuilder.Entity<TAB_Curso>()
                .HasMany(e => e.BNE_Rastreador1)
                .WithOptional(e => e.TAB_Curso1)
                .HasForeignKey(e => e.Idf_Curso);

            modelBuilder.Entity<TAB_Curso>()
                .HasMany(e => e.TAB_Curso_Fonte)
                .WithRequired(e => e.TAB_Curso)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Curso>()
                .HasMany(e => e.TAB_Pesquisa_Curriculo)
                .WithOptional(e => e.TAB_Curso)
                .HasForeignKey(e => e.Idf_Curso_Tecnico_Graduacao);

            modelBuilder.Entity<TAB_Curso>()
                .HasMany(e => e.TAB_Pesquisa_Curriculo1)
                .WithOptional(e => e.TAB_Curso1)
                .HasForeignKey(e => e.Idf_Curso_Outros_Cursos);

            modelBuilder.Entity<TAB_Curso>()
                .HasMany(e => e.TAB_Grau_Escolaridade)
                .WithMany(e => e.TAB_Curso)
                .Map(m => m.ToTable("TAB_Curso_Grau_Escolaridade", "BNE").MapLeftKey("Idf_Curso").MapRightKey("Idf_Grau_Escolaridade"));

            modelBuilder.Entity<TAB_Curso_Fonte>()
                .Property(e => e.Des_Curso)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Curso_Fonte>()
                .Property(e => e.Des_Pagamento)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Curso_Fonte>()
                .Property(e => e.Des_Contato)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Curso_Fonte>()
                .Property(e => e.Des_Obs)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Curso_Fonte>()
                .Property(e => e.Des_Duracao)
                .IsUnicode(false);

            modelBuilder.Entity<Tab_Disponibilidade>()
                .Property(e => e.Des_Disponibilidade)
                .IsUnicode(false);

            modelBuilder.Entity<Tab_Disponibilidade>()
                .HasMany(e => e.BNE_Curriculo_Disponibilidade)
                .WithRequired(e => e.Tab_Disponibilidade)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tab_Disponibilidade>()
                .HasMany(e => e.BNE_Rastreador_Disponibilidade)
                .WithRequired(e => e.Tab_Disponibilidade)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tab_Disponibilidade>()
                .HasMany(e => e.BNE_Vaga_Disponibilidade)
                .WithRequired(e => e.Tab_Disponibilidade)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tab_Disponibilidade>()
                .HasMany(e => e.TAB_Pesquisa_Curriculo_Disponibilidade)
                .WithRequired(e => e.Tab_Disponibilidade)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Empresa>()
                .Property(e => e.Num_CNPJ)
                .HasPrecision(14, 0);

            modelBuilder.Entity<TAB_Empresa>()
                .Property(e => e.Raz_Social)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Endereco>()
                .Property(e => e.Num_CEP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Endereco>()
                .Property(e => e.Des_Logradouro)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Endereco>()
                .Property(e => e.Num_Endereco)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Endereco>()
                .Property(e => e.Des_Complemento)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Endereco>()
                .Property(e => e.Des_Bairro)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Endereco>()
                .HasMany(e => e.TAB_Filial)
                .WithRequired(e => e.TAB_Endereco)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Envio_Email_Sal_BR>()
                .Property(e => e.Des_Email)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Feriado>()
                .Property(e => e.Des_Motivo_Feriado)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Feriado_Modelo>()
                .Property(e => e.Des_Motivo_Feriado)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Filial>()
                .Property(e => e.Num_CNPJ)
                .HasPrecision(14, 0);

            modelBuilder.Entity<TAB_Filial>()
                .Property(e => e.Raz_Social)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Filial>()
                .Property(e => e.Nme_Fantasia)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Filial>()
                .Property(e => e.End_Site)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Filial>()
                .Property(e => e.Num_DDD_Comercial)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Filial>()
                .Property(e => e.Num_Comercial)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Filial>()
                .Property(e => e.Des_IP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Filial>()
                .Property(e => e.Des_Pagina_Facebook)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Filial>()
                .Property(e => e.Num_Comercial2)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Filial>()
                .HasMany(e => e.BNE_Curriculo_Classificacao)
                .WithRequired(e => e.TAB_Filial)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Filial>()
                .HasMany(e => e.BNE_Curriculo_Permissao)
                .WithRequired(e => e.TAB_Filial)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Filial>()
                .HasMany(e => e.BNE_Curriculo_Quem_Me_Viu)
                .WithRequired(e => e.TAB_Filial)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Filial>()
                .HasMany(e => e.BNE_Curriculo_Visualizacao)
                .WithRequired(e => e.TAB_Filial)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Filial>()
                .HasMany(e => e.BNE_Curriculo_Visualizacao_Historico)
                .WithRequired(e => e.TAB_Filial)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Filial>()
                .HasOptional(e => e.BNE_Empresa_Home)
                .WithRequired(e => e.TAB_Filial);

            modelBuilder.Entity<TAB_Filial>()
                .HasMany(e => e.BNE_Intencao_Filial)
                .WithRequired(e => e.TAB_Filial)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Filial>()
                .HasMany(e => e.BNE_Usuario)
                .WithOptional(e => e.TAB_Filial)
                .HasForeignKey(e => e.Idf_Ultima_Filial_Logada);

            modelBuilder.Entity<TAB_Filial>()
                .HasMany(e => e.TAB_Integrador)
                .WithRequired(e => e.TAB_Filial)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Filial>()
                .HasMany(e => e.TAB_Filial_Observacao)
                .WithRequired(e => e.TAB_Filial)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Filial>()
                .HasMany(e => e.TAB_Origem_Filial)
                .WithRequired(e => e.TAB_Filial)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Filial>()
                .HasMany(e => e.TAB_Filial_BNE)
                .WithRequired(e => e.TAB_Filial)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Filial>()
                .HasMany(e => e.TAB_Integrador_Curriculo)
                .WithRequired(e => e.TAB_Filial)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Filial>()
                .HasMany(e => e.TAB_Parametro_Filial)
                .WithRequired(e => e.TAB_Filial)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Filial>()
                .HasMany(e => e.TAB_Filial_Logo)
                .WithRequired(e => e.TAB_Filial)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Filial_BNE>()
                .Property(e => e.Des_IP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Filial_BNE>()
                .Property(e => e.Sen_BNE_Velho)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Filial_Observacao>()
                .Property(e => e.Des_Observacao)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Filial_Observacao>()
                .Property(e => e.Des_Proxima_Acao)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Fonte>()
                .Property(e => e.Sig_Fonte)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Fonte>()
                .Property(e => e.Nme_Fonte)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Fonte>()
                .Property(e => e.Num_cnpj)
                .HasPrecision(14, 0);

            modelBuilder.Entity<TAB_Fonte>()
                .HasMany(e => e.BNE_Rastreador)
                .WithOptional(e => e.TAB_Fonte)
                .HasForeignKey(e => e.Idf_Fonte_Outros_Cursos);

            modelBuilder.Entity<TAB_Fonte>()
                .HasMany(e => e.BNE_Rastreador1)
                .WithOptional(e => e.TAB_Fonte1)
                .HasForeignKey(e => e.Idf_Fonte);

            modelBuilder.Entity<TAB_Fonte>()
                .HasMany(e => e.TAB_Curso_Fonte)
                .WithRequired(e => e.TAB_Fonte)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Fonte>()
                .HasMany(e => e.TAB_Pesquisa_Curriculo)
                .WithOptional(e => e.TAB_Fonte)
                .HasForeignKey(e => e.Idf_Fonte_Tecnico_Graduacao);

            modelBuilder.Entity<TAB_Fonte>()
                .HasMany(e => e.TAB_Pesquisa_Curriculo1)
                .WithOptional(e => e.TAB_Fonte1)
                .HasForeignKey(e => e.Idf_Fonte_Outros_Cursos);

            modelBuilder.Entity<TAB_Grupo_Economico>()
                .Property(e => e.Nme_Grupo_Economico)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Grupo_Economico>()
                .HasMany(e => e.TAB_Empresa)
                .WithRequired(e => e.TAB_Grupo_Economico)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Idioma>()
                .Property(e => e.Des_Idioma)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Idioma>()
                .HasMany(e => e.BNE_Curriculo_Idioma)
                .WithRequired(e => e.TAB_Idioma)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Idioma>()
                .HasMany(e => e.BNE_Rastreador_Idioma)
                .WithRequired(e => e.TAB_Idioma)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Idioma>()
                .HasMany(e => e.TAB_Pesquisa_Curriculo_Idioma)
                .WithRequired(e => e.TAB_Idioma)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Idioma>()
                .HasMany(e => e.TAB_Pessoa_Fisica_Idioma)
                .WithRequired(e => e.TAB_Idioma)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Integracao_SINE_2>()
                .Property(e => e.Num_CPF)
                .HasPrecision(11, 0);

            modelBuilder.Entity<TAB_Integracao_SINE_2>()
                .Property(e => e.Des_Observacao)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Integrador>()
                .HasMany(e => e.BNE_Vaga_Integracao)
                .WithRequired(e => e.TAB_Integrador)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Integrador>()
                .HasMany(e => e.TAB_Regra_Substituicao_Integracao)
                .WithOptional(e => e.TAB_Integrador)
                .HasForeignKey(e => e.Idf_Integrador);

            modelBuilder.Entity<TAB_Integrador>()
                .HasMany(e => e.TAB_Parametro_Integrador)
                .WithRequired(e => e.TAB_Integrador)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Integrador>()
                .HasMany(e => e.TAB_Regra_Substituicao_Integracao1)
                .WithOptional(e => e.TAB_Integrador1)
                .HasForeignKey(e => e.Idf_Integrador);

            modelBuilder.Entity<TAB_Integrador_Curriculo>()
                .Property(e => e.Sen_Integrador_Curriculo)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Integrador_Curriculo>()
                .HasMany(e => e.TAB_Requisicao_Integrador_Curriculo)
                .WithRequired(e => e.TAB_Integrador_Curriculo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Mensagem_CS>()
                .Property(e => e.Des_Mensagem)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Mensagem_CS>()
                .Property(e => e.Des_Email_Remetente)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Mensagem_CS>()
                .Property(e => e.Des_Email_Destino)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Mensagem_CS>()
                .Property(e => e.Des_Assunto)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Mensagem_CS>()
                .Property(e => e.Nme_Anexo)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Mensagem_CS>()
                .Property(e => e.Num_DDD_Celular)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Mensagem_CS>()
                .Property(e => e.Num_Celular)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Mensagem_Mailing_Bruno>()
                .Property(e => e.Des_Mensagem)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Mensagem_Mailing_Bruno>()
                .Property(e => e.Des_Email_Remetente)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Mensagem_Mailing_Bruno>()
                .Property(e => e.Des_Email_Destino)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Mensagem_Mailing_Bruno>()
                .Property(e => e.Des_Assunto)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Mensagem_Mailing_Bruno>()
                .Property(e => e.Nme_Anexo)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Mensagem_Mailing_Bruno>()
                .Property(e => e.Num_DDD_Celular)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Mensagem_Mailing_Bruno>()
                .Property(e => e.Num_Celular)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Mini_Agrupadora>()
                .Property(e => e.Nme_Mini_Agrupadora)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Nivel_Curso>()
                .Property(e => e.Des_Nivel_Curso)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Nivel_Curso>()
                .HasMany(e => e.TAB_Curso)
                .WithRequired(e => e.TAB_Nivel_Curso)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Nivel_Idioma>()
                .Property(e => e.Des_Nivel_Idioma)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Nivel_Idioma>()
                .HasMany(e => e.TAB_Pessoa_Fisica_Idioma)
                .WithRequired(e => e.TAB_Nivel_Idioma)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Origem>()
                .Property(e => e.Des_Origem)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Origem>()
                .Property(e => e.Des_URL)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Origem>()
                .HasMany(e => e.BNE_Curriculo_Origem)
                .WithRequired(e => e.TAB_Origem)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Origem>()
                .HasMany(e => e.BNE_Vaga)
                .WithRequired(e => e.TAB_Origem)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Origem>()
                .HasMany(e => e.TAB_Origem_Filial)
                .WithRequired(e => e.TAB_Origem)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Origem_Filial>()
                .Property(e => e.Des_IP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Origem_Filial>()
                .Property(e => e.Des_Diretorio)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Origem_Filial>()
                .Property(e => e.Des_Mensagem_Candidato)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Origem_Filial>()
                .Property(e => e.Des_Pagina_Inicial)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Origem_Filial>()
                .HasMany(e => e.TAB_Origem_Filial_Funcao)
                .WithRequired(e => e.TAB_Origem_Filial)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Origem_Filial_Funcao>()
                .Property(e => e.Des_Funcao)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Parametro_Curriculo>()
                .Property(e => e.Vlr_Parametro)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Parametro_Filial>()
                .Property(e => e.Vlr_Parametro)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Parametro_Integrador>()
                .Property(e => e.Vlr_Parametro)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Perfil>()
                .Property(e => e.Des_Perfil)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Perfil>()
                .HasMany(e => e.TAB_Perfil_Usuario)
                .WithRequired(e => e.TAB_Perfil)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Perfil>()
                .HasMany(e => e.TAB_Permissao)
                .WithMany(e => e.TAB_Perfil)
                .Map(m => m.ToTable("TAB_Perfil_Permissao", "BNE").MapLeftKey("Idf_Perfil").MapRightKey("Idf_Permissao"));

            modelBuilder.Entity<TAB_Pesquisa_Curriculo>()
                .Property(e => e.Des_IP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pesquisa_Curriculo>()
                .Property(e => e.Des_Palavra_Chave)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pesquisa_Curriculo>()
                .Property(e => e.Sig_Estado)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pesquisa_Curriculo>()
                .Property(e => e.Num_Salario_Min)
                .HasPrecision(19, 2);

            modelBuilder.Entity<TAB_Pesquisa_Curriculo>()
                .Property(e => e.Num_Salario_Max)
                .HasPrecision(19, 2);

            modelBuilder.Entity<TAB_Pesquisa_Curriculo>()
                .Property(e => e.Des_Cod_CPF_Nome)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pesquisa_Curriculo>()
                .Property(e => e.Des_Bairro)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pesquisa_Curriculo>()
                .Property(e => e.Des_Logradouro)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pesquisa_Curriculo>()
                .Property(e => e.Num_CEP_Min)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pesquisa_Curriculo>()
                .Property(e => e.Num_CEP_Max)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pesquisa_Curriculo>()
                .Property(e => e.Des_Experiencia)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pesquisa_Curriculo>()
                .Property(e => e.Raz_Social)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pesquisa_Curriculo>()
                .Property(e => e.Num_DDD_Telefone)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pesquisa_Curriculo>()
                .Property(e => e.Num_Telefone)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pesquisa_Curriculo>()
                .Property(e => e.Eml_Pessoa)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pesquisa_Curriculo>()
                .Property(e => e.Des_Curso_Tecnico_Graduacao)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pesquisa_Curriculo>()
                .Property(e => e.Des_Curso_Outros_Cursos)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pesquisa_Curriculo>()
                .Property(e => e.Idf_Escolaridade_WebStagio)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pesquisa_Curriculo>()
                .HasMany(e => e.TAB_Pesquisa_Curriculo_Disponibilidade)
                .WithRequired(e => e.TAB_Pesquisa_Curriculo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Pesquisa_Curriculo>()
                .HasMany(e => e.TAB_Pesquisa_Curriculo_Idioma)
                .WithRequired(e => e.TAB_Pesquisa_Curriculo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Pesquisa_Salarial>()
                .Property(e => e.Vlr_Media)
                .HasPrecision(19, 4);

            modelBuilder.Entity<TAB_Pesquisa_Salarial>()
                .Property(e => e.Vlr_Maximo)
                .HasPrecision(19, 4);

            modelBuilder.Entity<TAB_Pesquisa_Salarial>()
                .Property(e => e.Vlr_Minimo)
                .HasPrecision(19, 4);

            modelBuilder.Entity<TAB_Pesquisa_Salarial>()
                .Property(e => e.Vlr_Junior)
                .HasPrecision(19, 4);

            modelBuilder.Entity<TAB_Pesquisa_Salarial>()
                .Property(e => e.Vlr_Treinee)
                .HasPrecision(19, 4);

            modelBuilder.Entity<TAB_Pesquisa_Salarial>()
                .Property(e => e.Vlr_Senior)
                .HasPrecision(19, 4);

            modelBuilder.Entity<TAB_Pesquisa_Salarial>()
                .Property(e => e.Vlr_Master)
                .HasPrecision(19, 4);

            modelBuilder.Entity<TAB_Pesquisa_Salarial>()
                .Property(e => e.Vlr_Pleno)
                .HasPrecision(19, 4);

            modelBuilder.Entity<TAB_Pesquisa_Vaga>()
                .Property(e => e.Des_IP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pesquisa_Vaga>()
                .Property(e => e.Des_Palavra_Chave)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pesquisa_Vaga>()
                .Property(e => e.Sig_Estado)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pesquisa_Vaga>()
                .Property(e => e.Num_Salario_Min)
                .HasPrecision(19, 2);

            modelBuilder.Entity<TAB_Pesquisa_Vaga>()
                .Property(e => e.Num_Salario_Max)
                .HasPrecision(19, 2);

            modelBuilder.Entity<TAB_Pesquisa_Vaga>()
                .Property(e => e.Des_Cod_Vaga)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pesquisa_Vaga>()
                .Property(e => e.Raz_Social)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Num_CPF)
                .HasPrecision(11, 0);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Nme_Pessoa)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Ape_Pessoa)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Nme_Mae)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Nme_Pai)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Num_RG)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Nme_Orgao_Emissor)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Sig_UF_Emissao_RG)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Num_PIS)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Num_CTPS)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Des_Serie_CTPS)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Sig_UF_CTPS)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Num_DDD_Telefone)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Num_Telefone)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Num_DDD_Celular)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Num_Celular)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Eml_Pessoa)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Nme_Pessoa_Pesquisa)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Sig_Estado)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .Property(e => e.Des_IP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .HasMany(e => e.BNE_Curriculo)
                .WithRequired(e => e.TAB_Pessoa_Fisica)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .HasMany(e => e.BNE_Experiencia_Profissional)
                .WithRequired(e => e.TAB_Pessoa_Fisica)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .HasMany(e => e.BNE_Formacao)
                .WithRequired(e => e.TAB_Pessoa_Fisica)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .HasMany(e => e.BNE_Usuario)
                .WithRequired(e => e.TAB_Pessoa_Fisica)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .HasMany(e => e.TAB_Pessoa_Fisica_Idioma)
                .WithRequired(e => e.TAB_Pessoa_Fisica)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .HasOptional(e => e.TAB_Pessoa_Fisica_Complemento)
                .WithRequired(e => e.TAB_Pessoa_Fisica);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .HasMany(e => e.TAB_Pessoa_Fisica_Foto)
                .WithRequired(e => e.TAB_Pessoa_Fisica)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .HasMany(e => e.TAB_Pessoa_Fisica_Rede_Social)
                .WithRequired(e => e.TAB_Pessoa_Fisica)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .HasMany(e => e.TAB_Pessoa_Fisica_Veiculo)
                .WithRequired(e => e.TAB_Pessoa_Fisica)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica>()
                .HasMany(e => e.TAB_Usuario_Filial_Perfil)
                .WithRequired(e => e.TAB_Pessoa_Fisica)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica_Complemento>()
                .Property(e => e.Num_Habilitacao)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica_Complemento>()
                .Property(e => e.Num_Titulo_Eleitoral)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica_Complemento>()
                .Property(e => e.Num_Zona_Eleitoral)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica_Complemento>()
                .Property(e => e.Num_Secao_Eleitoral)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica_Complemento>()
                .Property(e => e.Num_Registro_Conselho)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica_Complemento>()
                .Property(e => e.Des_Pais_Visto)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica_Complemento>()
                .Property(e => e.Num_Visto)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica_Complemento>()
                .Property(e => e.Des_Placa_Veiculo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica_Complemento>()
                .Property(e => e.Num_Renavam)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica_Complemento>()
                .Property(e => e.Num_Doc_Reservista)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica_Complemento>()
                .Property(e => e.Num_CEI)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica_Complemento>()
                .Property(e => e.Num_Altura)
                .HasPrecision(3, 2);

            modelBuilder.Entity<TAB_Pessoa_Fisica_Complemento>()
                .Property(e => e.Num_Peso)
                .HasPrecision(5, 2);

            modelBuilder.Entity<TAB_Pessoa_Fisica_Complemento>()
                .Property(e => e.Des_Conhecimento)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica_Complemento>()
                .Property(e => e.Des_Complemento_Deficiencia)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica_Complemento>()
                .Property(e => e.Nme_Anexo)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica_Complemento>()
                .HasMany(e => e.TAB_Contato)
                .WithRequired(e => e.TAB_Pessoa_Fisica_Complemento)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica_Rede_Social>()
                .Property(e => e.Cod_Identificador)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica_Rede_Social>()
                .Property(e => e.Cod_Interno_Rede_Social)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica_Temp>()
                .Property(e => e.Num_CPF)
                .HasPrecision(11, 0);

            modelBuilder.Entity<TAB_Pessoa_Fisica_Temp>()
                .Property(e => e.Num_DDD_Celular)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica_Temp>()
                .Property(e => e.Num_Celular)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica_Veiculo>()
                .Property(e => e.Des_Modelo)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica_Veiculo>()
                .Property(e => e.Des_placa_Veiculo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Fisica_Veiculo>()
                .Property(e => e.Num_Renavam)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Regiao_Metropolitana>()
                .Property(e => e.Nme_Regiao_Metropolitana)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Regiao_Metropolitana>()
                .Property(e => e.Nme_Regiao_Metropolitana_Pesquisa)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Regiao_Metropolitana>()
                .Property(e => e.Sig_UF)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Regiao_Metropolitana>()
                .Property(e => e.CID_Regiao_Metropolitana)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Regiao_Metropolitana>()
                .HasMany(e => e.TAB_Regiao_Metropolitana_Cidade)
                .WithRequired(e => e.TAB_Regiao_Metropolitana)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Requisicao_Integrador_Curriculo>()
                .Property(e => e.Nme_Cliente)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Situacao_Filial>()
                .Property(e => e.Des_Situacao_Filial)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Situacao_Filial>()
                .HasMany(e => e.TAB_Filial)
                .WithRequired(e => e.TAB_Situacao_Filial)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Situacao_Filial>()
                .HasMany(e => e.TAB_Filial_BNE)
                .WithRequired(e => e.TAB_Situacao_Filial)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Status_Codigo_Desconto>()
                .Property(e => e.Des_Status_Codigo_Desconto)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Status_Codigo_Desconto>()
                .HasMany(e => e.BNE_Codigo_Desconto)
                .WithRequired(e => e.TAB_Status_Codigo_Desconto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Status_Transacao>()
                .Property(e => e.Dsc_Status_Transacao)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Status_Transacao>()
                .HasMany(e => e.BNE_Transacao)
                .WithRequired(e => e.TAB_Status_Transacao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Tipo_Arquivo>()
                .Property(e => e.Dsc_Tipo_Arquivo)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Tipo_Arquivo>()
                .HasMany(e => e.BNE_Arquivo)
                .WithRequired(e => e.TAB_Tipo_Arquivo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Tipo_Feriado>()
                .Property(e => e.Des_Tipo_Feriado)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Tipo_Feriado>()
                .HasMany(e => e.TAB_Feriado)
                .WithRequired(e => e.TAB_Tipo_Feriado)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Tipo_Feriado>()
                .HasMany(e => e.TAB_Feriado_Modelo)
                .WithRequired(e => e.TAB_Tipo_Feriado)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Tipo_Integrador>()
                .Property(e => e.Des_Tipo_Integrador)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Tipo_Parceiro>()
                .Property(e => e.Des_Tipo_Parceiro)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Tipo_Parceiro>()
                .HasMany(e => e.BNE_Parceiro)
                .WithRequired(e => e.TAB_Tipo_Parceiro)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Transacao_Mensagem_Erro>()
                .Property(e => e.Des_Codigo_Erro)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Transacao_Mensagem_Erro>()
                .Property(e => e.Des_Descricao_Erro)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Transacao_Mensagem_Erro>()
                .Property(e => e.Des_Mensagem_Amigavel)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Usuario_Filial_Perfil>()
                .Property(e => e.Des_IP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Usuario_Filial_Perfil>()
                .Property(e => e.Sen_Usuario_Filial_Perfil)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Usuario_Filial_Perfil>()
                .HasMany(e => e.BNE_Auditor_Publicador)
                .WithRequired(e => e.TAB_Usuario_Filial_Perfil)
                .HasForeignKey(e => e.Idf_Publicador)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Usuario_Filial_Perfil>()
                .HasMany(e => e.BNE_Auditor_Publicador1)
                .WithRequired(e => e.TAB_Usuario_Filial_Perfil1)
                .HasForeignKey(e => e.Idf_Usuario_Filial_Perfil)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Usuario_Filial_Perfil>()
                .HasMany(e => e.BNE_Celular_Selecionador)
                .WithRequired(e => e.TAB_Usuario_Filial_Perfil)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Usuario_Filial_Perfil>()
                .HasMany(e => e.BNE_Conversas_Ativas)
                .WithRequired(e => e.TAB_Usuario_Filial_Perfil)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Usuario_Filial_Perfil>()
                .HasMany(e => e.BNE_Curriculo_Auditoria)
                .WithRequired(e => e.TAB_Usuario_Filial_Perfil)
                .HasForeignKey(e => e.Idf_Usuario_Filial_Perfil)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Usuario_Filial_Perfil>()
                .HasMany(e => e.BNE_Curriculo_Auditoria1)
                .WithOptional(e => e.TAB_Usuario_Filial_Perfil1)
                .HasForeignKey(e => e.Idf_Publicador_Perfil);

            modelBuilder.Entity<TAB_Usuario_Filial_Perfil>()
                .HasMany(e => e.BNE_Curriculo_Correcao)
                .WithRequired(e => e.TAB_Usuario_Filial_Perfil)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Usuario_Filial_Perfil>()
                .HasMany(e => e.BNE_Curriculo_Publicacao)
                .WithRequired(e => e.TAB_Usuario_Filial_Perfil)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Usuario_Filial_Perfil>()
                .HasMany(e => e.BNE_Email_Destinatario)
                .WithRequired(e => e.TAB_Usuario_Filial_Perfil)
                .HasForeignKey(e => e.Idf_Usuario_Gerador)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Usuario_Filial_Perfil>()
                .HasMany(e => e.BNE_Email_Destinatario_Cidade)
                .WithRequired(e => e.TAB_Usuario_Filial_Perfil)
                .HasForeignKey(e => e.Idf_Usuario_Gerador)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Usuario_Filial_Perfil>()
                .HasMany(e => e.BNE_Financeiro)
                .WithRequired(e => e.TAB_Usuario_Filial_Perfil)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Usuario_Filial_Perfil>()
                .HasMany(e => e.BNE_Mensagem_CS)
                .WithOptional(e => e.TAB_Usuario_Filial_Perfil)
                .HasForeignKey(e => e.Idf_Usuario_Filial_Perfil);

            modelBuilder.Entity<TAB_Usuario_Filial_Perfil>()
                .HasMany(e => e.BNE_Mensagem_CS1)
                .WithOptional(e => e.TAB_Usuario_Filial_Perfil1)
                .HasForeignKey(e => e.Idf_Usuario_Filial_Des);

            modelBuilder.Entity<TAB_Usuario_Filial_Perfil>()
                .HasMany(e => e.BNE_Noticia_Visualizacao)
                .WithRequired(e => e.TAB_Usuario_Filial_Perfil)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Usuario_Filial_Perfil>()
                .HasMany(e => e.BNE_Pagamento)
                .WithOptional(e => e.TAB_Usuario_Filial_Perfil)
                .HasForeignKey(e => e.IDF_Usuario_Gerador);

            modelBuilder.Entity<TAB_Usuario_Filial_Perfil>()
                .HasMany(e => e.BNE_Pagamento1)
                .WithRequired(e => e.TAB_Usuario_Filial_Perfil1)
                .HasForeignKey(e => e.Idf_Usuario_Filial_Perfil)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Usuario_Filial_Perfil>()
                .HasMany(e => e.BNE_Plano_Adquirido)
                .WithRequired(e => e.TAB_Usuario_Filial_Perfil)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Usuario_Filial_Perfil>()
                .HasMany(e => e.BNE_Publicador)
                .WithRequired(e => e.TAB_Usuario_Filial_Perfil)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Usuario_Filial_Perfil>()
                .HasMany(e => e.BNE_Revidor)
                .WithRequired(e => e.TAB_Usuario_Filial_Perfil)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Usuario_Filial_Perfil>()
                .HasMany(e => e.BNE_Usuario_Filial)
                .WithRequired(e => e.TAB_Usuario_Filial_Perfil)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Usuario_Filial_Perfil>()
                .HasMany(e => e.BNE_Vaga)
                .WithRequired(e => e.TAB_Usuario_Filial_Perfil)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Usuario_Filial_Perfil>()
                .HasMany(e => e.TAB_Perfil_Usuario)
                .WithRequired(e => e.TAB_Usuario_Filial_Perfil)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Usuario_Filial_Perfil>()
                .HasMany(e => e.TAB_Usuario_Funcao)
                .WithRequired(e => e.TAB_Usuario_Filial_Perfil)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Integracao>()
                .Property(e => e.Num_CPF)
                .HasPrecision(11, 0);

            modelBuilder.Entity<BNE_Integracao>()
                .Property(e => e.Nme_Pessoa)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Integracao>()
                .Property(e => e.Ape_Pessoa)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Integracao>()
                .Property(e => e.Nme_Mae)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Integracao>()
                .Property(e => e.Nme_Pai)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Integracao>()
                .Property(e => e.Num_RG)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Integracao>()
                .Property(e => e.Nme_Orgao_Emissor)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Integracao>()
                .Property(e => e.Sig_UF_Emissao_RG)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Integracao>()
                .Property(e => e.Des_Logradouro)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Integracao>()
                .Property(e => e.Des_Complemento)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Integracao>()
                .Property(e => e.Num_Endereco)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Integracao>()
                .Property(e => e.Num_CEP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Integracao>()
                .Property(e => e.Des_Bairro)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Integracao>()
                .Property(e => e.Num_DDD_Telefone)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Integracao>()
                .Property(e => e.Num_Telefone)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Integracao>()
                .Property(e => e.Num_DDD_Celular)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Integracao>()
                .Property(e => e.Num_Celular)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Integracao>()
                .Property(e => e.Eml_Pessoa)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Integracao>()
                .Property(e => e.Raz_Social)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Integracao>()
                .Property(e => e.Vlr_Salario)
                .HasPrecision(10, 2);

            modelBuilder.Entity<BNE_Integracao>()
                .Property(e => e.Num_Habilitacao)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Integracao_Admissao>()
                .Property(e => e.Num_CPF)
                .HasPrecision(11, 0);

            modelBuilder.Entity<BNE_Integracao_Situacao>()
                .Property(e => e.Des_Integracao_Situacao)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Integracao_Situacao>()
                .HasMany(e => e.BNE_Integracao)
                .WithRequired(e => e.BNE_Integracao_Situacao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Integracao_Situacao>()
                .HasMany(e => e.BNE_Integracao_Admissao)
                .WithRequired(e => e.BNE_Integracao_Situacao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Tipo_Contrato>()
                .Property(e => e.Des_Tipo_Contrato)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Area_BNE>()
                .Property(e => e.Des_Area_BNE)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Area_BNE>()
                .Property(e => e.Des_Area_BNE_Pesquisa)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Banco>()
                .Property(e => e.Nme_Banco)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Banco>()
                .Property(e => e.Ape_Banco)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Categoria_Habilitacao>()
                .Property(e => e.Des_Categoria_Habilitacao)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Categoria_Permissao>()
                .Property(e => e.Des_Categoria_Permissao)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Categoria_Permissao>()
                .HasMany(e => e.TAB_Permissao)
                .WithRequired(e => e.TAB_Categoria_Permissao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Centro_Servico>()
                .Property(e => e.Des_Centro_Servico)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Centro_Servico>()
                .Property(e => e.Log_DB)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Centro_Servico>()
                .Property(e => e.Sen_DB)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Centro_Servico>()
                .Property(e => e.Des_Schema)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Centro_Servico>()
                .HasMany(e => e.TAB_Mensagem)
                .WithRequired(e => e.TAB_Centro_Servico)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Cidade>()
                .Property(e => e.Nme_Cidade)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Cidade>()
                .Property(e => e.Sig_Estado)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Cidade>()
                .Property(e => e.Cod_Rais)
                .HasPrecision(7, 0);

            modelBuilder.Entity<TAB_Cidade>()
                .HasMany(e => e.BNE_Agradecimento)
                .WithRequired(e => e.TAB_Cidade)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Cidade>()
                .HasMany(e => e.BNE_Curriculo)
                .WithOptional(e => e.TAB_Cidade)
                .HasForeignKey(e => e.Idf_Cidade_Pretendida);

            modelBuilder.Entity<TAB_Cidade>()
                .HasMany(e => e.BNE_Curriculo1)
                .WithOptional(e => e.TAB_Cidade1)
                .HasForeignKey(e => e.Idf_Cidade_Endereco);

            modelBuilder.Entity<TAB_Cidade>()
                .HasMany(e => e.BNE_Curriculo_Disponibilidade_Cidade)
                .WithRequired(e => e.TAB_Cidade)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Cidade>()
                .HasMany(e => e.BNE_Indicacao_Filial)
                .WithRequired(e => e.TAB_Cidade)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Cidade>()
                .HasMany(e => e.BNE_Lista_Cidade)
                .WithRequired(e => e.TAB_Cidade)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Cidade>()
                .HasMany(e => e.BNE_Simulacao_R1)
                .WithRequired(e => e.TAB_Cidade)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Cidade>()
                .HasMany(e => e.BNE_Solicitacao_R1)
                .WithRequired(e => e.TAB_Cidade)
                .HasForeignKey(e => e.Idf_Cidade)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Cidade>()
                .HasMany(e => e.BNE_Solicitacao_R11)
                .WithRequired(e => e.TAB_Cidade1)
                .HasForeignKey(e => e.Idf_Cidade_Solicitante)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Cidade>()
                .HasMany(e => e.BNE_Vaga)
                .WithRequired(e => e.TAB_Cidade)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Cidade>()
                .HasMany(e => e.GLO_Cobranca_Boleto)
                .WithOptional(e => e.TAB_Cidade)
                .HasForeignKey(e => e.Idf_Cidade_Sacado);

            modelBuilder.Entity<TAB_Cidade>()
                .HasOptional(e => e.TAB_Cidade_Capital)
                .WithRequired(e => e.TAB_Cidade);

            modelBuilder.Entity<TAB_Cidade>()
                .HasMany(e => e.TAB_Endereco)
                .WithRequired(e => e.TAB_Cidade)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Cidade>()
                .HasMany(e => e.TAB_Regiao_Metropolitana)
                .WithRequired(e => e.TAB_Cidade)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Cidade>()
                .HasMany(e => e.TAB_Regiao_Metropolitana_Cidade)
                .WithRequired(e => e.TAB_Cidade)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Cidade>()
                .HasMany(e => e.BNE_Integracao)
                .WithRequired(e => e.TAB_Cidade)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Classe_Salarial>()
                .Property(e => e.Vlr_Salario_Medio)
                .HasPrecision(19, 4);

            modelBuilder.Entity<TAB_Classe_Salarial>()
                .Property(e => e.Vlr_Piso)
                .HasPrecision(19, 4);

            modelBuilder.Entity<TAB_Classe_Salarial>()
                .Property(e => e.Vlr_Teto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<TAB_CNAE_Classe>()
                .Property(e => e.Cod_CNAE_Classe)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_CNAE_Classe>()
                .Property(e => e.Des_CNAE_Classe)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_CNAE_Classe>()
                .HasMany(e => e.TAB_CNAE_Sub_Classe)
                .WithRequired(e => e.TAB_CNAE_Classe)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_CNAE_Divisao>()
                .Property(e => e.Cod_CNAE_Divisao)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_CNAE_Divisao>()
                .Property(e => e.Des_CNAE_Divisao)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_CNAE_Divisao>()
                .HasMany(e => e.TAB_CNAE_Grupo)
                .WithRequired(e => e.TAB_CNAE_Divisao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_CNAE_Grupo>()
                .Property(e => e.Cod_CNAE_Grupo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_CNAE_Grupo>()
                .Property(e => e.Des_CNAE_Grupo)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_CNAE_Grupo>()
                .HasMany(e => e.TAB_CNAE_Classe)
                .WithRequired(e => e.TAB_CNAE_Grupo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_CNAE_Secao>()
                .Property(e => e.Cod_CNAE_Secao)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_CNAE_Secao>()
                .Property(e => e.Des_CNAE_Secao)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_CNAE_Secao>()
                .HasMany(e => e.TAB_CNAE_Divisao)
                .WithRequired(e => e.TAB_CNAE_Secao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_CNAE_Sub_Classe>()
                .Property(e => e.Cod_CNAE_Sub_Classe)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_CNAE_Sub_Classe>()
                .Property(e => e.Des_CNAE_Sub_Classe)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_CNAE_Sub_Classe>()
                .HasMany(e => e.TAB_Filial)
                .WithOptional(e => e.TAB_CNAE_Sub_Classe)
                .HasForeignKey(e => e.Idf_CNAE_Principal);

            modelBuilder.Entity<TAB_CNAE_Sub_Classe>()
                .HasMany(e => e.TAB_Pessoa_Juridica)
                .WithRequired(e => e.TAB_CNAE_Sub_Classe)
                .HasForeignKey(e => e.Idf_CNAE_Principal)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_CNAE_Sub_Classe>()
                .HasMany(e => e.TAB_Pessoa_Juridica1)
                .WithOptional(e => e.TAB_CNAE_Sub_Classe1)
                .HasForeignKey(e => e.Idf_CNAE_Secundario);

            modelBuilder.Entity<TAB_Deficiencia>()
                .Property(e => e.Des_Deficiencia)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_DePara_Email>()
                .Property(e => e.Des_Email_Erro)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_DePara_Email>()
                .Property(e => e.Des_Email_Correto)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Email_Situacao>()
                .Property(e => e.Des_Email_Situacao)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Email_Situacao>()
                .HasMany(e => e.BNE_Usuario_Filial)
                .WithOptional(e => e.TAB_Email_Situacao)
                .HasForeignKey(e => e.Idf_Email_Situacao_Bloqueio);

            modelBuilder.Entity<TAB_Email_Situacao>()
                .HasMany(e => e.BNE_Usuario_Filial1)
                .WithOptional(e => e.TAB_Email_Situacao1)
                .HasForeignKey(e => e.Idf_Email_Situacao_Confirmacao);

            modelBuilder.Entity<TAB_Email_Situacao>()
                .HasMany(e => e.BNE_Usuario_Filial2)
                .WithOptional(e => e.TAB_Email_Situacao2)
                .HasForeignKey(e => e.Idf_Email_Situacao_Validacao);

            modelBuilder.Entity<TAB_Email_Situacao>()
                .HasMany(e => e.TAB_Pessoa_Fisica)
                .WithOptional(e => e.TAB_Email_Situacao)
                .HasForeignKey(e => e.Idf_Email_Situacao_Bloqueio);

            modelBuilder.Entity<TAB_Email_Situacao>()
                .HasMany(e => e.TAB_Pessoa_Fisica1)
                .WithOptional(e => e.TAB_Email_Situacao1)
                .HasForeignKey(e => e.Idf_Email_Situacao_Confirmacao);

            modelBuilder.Entity<TAB_Email_Situacao>()
                .HasMany(e => e.TAB_Pessoa_Fisica2)
                .WithOptional(e => e.TAB_Email_Situacao2)
                .HasForeignKey(e => e.Idf_Email_Situacao_Validacao);

            modelBuilder.Entity<TAB_Endereco_Pessoa_Juridica>()
                .Property(e => e.Num_CEP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Endereco_Pessoa_Juridica>()
                .Property(e => e.Des_Logradouro)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Endereco_Pessoa_Juridica>()
                .Property(e => e.Num_Endereco)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Endereco_Pessoa_Juridica>()
                .Property(e => e.Des_Complemento)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Endereco_Pessoa_Juridica>()
                .Property(e => e.Des_Bairro)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Escolaridade>()
                .Property(e => e.Des_Geral)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Escolaridade>()
                .Property(e => e.Des_BNE)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Escolaridade>()
                .Property(e => e.Des_Rais)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Escolaridade>()
                .Property(e => e.Cod_Rais)
                .HasPrecision(7, 0);

            modelBuilder.Entity<TAB_Escolaridade>()
                .Property(e => e.Des_Abreviada)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Escolaridade>()
                .HasMany(e => e.BNE_Formacao)
                .WithRequired(e => e.TAB_Escolaridade)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Escolaridade>()
                .HasMany(e => e.BNE_Solicitacao_R1)
                .WithRequired(e => e.TAB_Escolaridade)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Estado>()
                .Property(e => e.Sig_Estado)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Estado>()
                .Property(e => e.Nme_Estado)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Estado>()
                .HasMany(e => e.BNE_Propaganda_Estado)
                .WithRequired(e => e.TAB_Estado)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Estado>()
                .HasMany(e => e.TAB_Inscricao_Estadual)
                .WithRequired(e => e.TAB_Estado)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Estado_Civil>()
                .Property(e => e.Des_Estado_Civil)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Flag>()
                .Property(e => e.Des_Flag)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_FPAS>()
                .Property(e => e.Des_FPAS)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_FPAS_Aliquota>()
                .Property(e => e.Nme_FPAS_Aliquota)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Funcao>()
                .Property(e => e.Des_Funcao)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Funcao>()
                .Property(e => e.Des_Job)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Funcao>()
                .Property(e => e.Des_Experiencia)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Funcao>()
                .Property(e => e.Vlr_Piso_Profissional)
                .HasPrecision(19, 4);

            modelBuilder.Entity<TAB_Funcao>()
                .Property(e => e.Des_Cursos)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Funcao>()
                .Property(e => e.Des_Competencias)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Funcao>()
                .Property(e => e.Des_Funcao_Pesquisa)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Funcao>()
                .Property(e => e.Cod_Funcao_Folha)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Funcao>()
                .Property(e => e.Cod_CBO)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Funcao>()
                .Property(e => e.Des_Local_Trabalho)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Funcao>()
                .Property(e => e.Des_EPI)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Funcao>()
                .Property(e => e.Des_PPRA)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Funcao>()
                .Property(e => e.Des_PCMSO)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Funcao>()
                .Property(e => e.Des_Equipamentos)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Funcao>()
                .HasMany(e => e.BNE_Amplitude_Salarial)
                .WithRequired(e => e.TAB_Funcao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Funcao>()
                .HasMany(e => e.BNE_Curso_Funcao_Tecla)
                .WithRequired(e => e.TAB_Funcao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Funcao>()
                .HasMany(e => e.BNE_Simulacao_R1)
                .WithRequired(e => e.TAB_Funcao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Funcao>()
                .HasMany(e => e.BNE_Solicitacao_R1)
                .WithRequired(e => e.TAB_Funcao)
                .HasForeignKey(e => e.Idf_Funcao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Funcao>()
                .HasMany(e => e.BNE_Solicitacao_R11)
                .WithRequired(e => e.TAB_Funcao1)
                .HasForeignKey(e => e.Idf_Funcao_Solicitante)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Funcao>()
                .HasMany(e => e.TAB_Funcao_Mini_Agrupadora)
                .WithRequired(e => e.TAB_Funcao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Funcao>()
                .HasMany(e => e.TAB_Origem_Filial_Funcao)
                .WithRequired(e => e.TAB_Funcao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Funcao>()
                .HasMany(e => e.TAB_Pesquisa_Salarial)
                .WithRequired(e => e.TAB_Funcao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Funcao>()
                .HasMany(e => e.TAB_Usuario_Funcao)
                .WithRequired(e => e.TAB_Funcao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Funcao>()
                .HasMany(e => e.BNE_Integracao)
                .WithRequired(e => e.TAB_Funcao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Funcao>()
                .HasMany(e => e.TAB_Funcao1)
                .WithOptional(e => e.TAB_Funcao2)
                .HasForeignKey(e => e.Idf_Funcao_Agrupadora);

            modelBuilder.Entity<TAB_Funcao_Categoria>()
                .Property(e => e.Des_Funcao_Categoria)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Funcao_Categoria>()
                .Property(e => e.Cod_Funcao_Categoria)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Funcao_Categoria>()
                .HasMany(e => e.TAB_Funcao)
                .WithRequired(e => e.TAB_Funcao_Categoria)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_GPS>()
                .Property(e => e.Des_GPS)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Grau_Escolaridade>()
                .Property(e => e.Des_Grau_Escolaridade)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Grau_Escolaridade>()
                .HasMany(e => e.TAB_Nivel_Curso)
                .WithRequired(e => e.TAB_Grau_Escolaridade)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Grau_Escolaridade>()
                .HasMany(e => e.TAB_Escolaridade)
                .WithRequired(e => e.TAB_Grau_Escolaridade)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Inscricao_Estadual>()
                .Property(e => e.Num_Inscricao)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Mensagem>()
                .Property(e => e.Des_Mensagem)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Mensagem>()
                .Property(e => e.Des_Email_Destinatario)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Mensagem>()
                .Property(e => e.Des_Email_Assunto)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Mensagem>()
                .Property(e => e.Arq_Anexo)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Mensagem>()
                .Property(e => e.Des_Email_Remetente)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Mensagem>()
                .Property(e => e.Num_DDD_Celular)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Mensagem>()
                .Property(e => e.Num_Celular)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Mensagem>()
                .Property(e => e.Nme_Anexo)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Mensagem>()
                .Property(e => e.Des_Obs)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Motivo_Rescisao>()
                .Property(e => e.Des_Motivo_Rescisao)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Motivo_Rescisao>()
                .Property(e => e.Sig_Causa_Afastamento)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Motivo_Rescisao>()
                .Property(e => e.Sig_Codigo_Afastamento)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Motivo_Rescisao>()
                .HasMany(e => e.BNE_Integracao)
                .WithRequired(e => e.TAB_Motivo_Rescisao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Nacionalidade>()
                .Property(e => e.Des_Nacionalidade)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Natureza_Juridica>()
                .Property(e => e.Cod_Natureza_Juridica)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Natureza_Juridica>()
                .Property(e => e.Des_Natureza_Juridica)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Natureza_Juridica>()
                .HasOptional(e => e.TAB_Pessoa_Juridica)
                .WithRequired(e => e.TAB_Natureza_Juridica);

            modelBuilder.Entity<TAB_Operadora_Cartao>()
                .Property(e => e.Des_Operadora)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Operadora_Cartao>()
                .Property(e => e.COD_OPERADORA)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Operadora_Celular>()
                .Property(e => e.Nme_Operadora_Celular)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Operadora_Celular>()
                .Property(e => e.Des_URL_Logo)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Parametro>()
                .Property(e => e.Nme_Parametro)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Parametro>()
                .Property(e => e.Vlr_Parametro)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Parametro>()
                .HasMany(e => e.TAB_Parametro_Curriculo)
                .WithRequired(e => e.TAB_Parametro)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Parametro>()
                .HasMany(e => e.TAB_Parametro_Integrador)
                .WithRequired(e => e.TAB_Parametro)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Parcela_Cartao>()
                .Property(e => e.Des_Parcela)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Permissao>()
                .Property(e => e.Des_Permissao)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Permissao>()
                .Property(e => e.Cod_Permissao)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Juridica>()
                .Property(e => e.Num_CNPJ)
                .HasPrecision(14, 0);

            modelBuilder.Entity<TAB_Pessoa_Juridica>()
                .Property(e => e.Num_CEI)
                .HasPrecision(12, 0);

            modelBuilder.Entity<TAB_Pessoa_Juridica>()
                .Property(e => e.Raz_Social)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Juridica>()
                .Property(e => e.Nme_Fantasia)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Juridica>()
                .Property(e => e.Ape_Empresa)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Juridica>()
                .Property(e => e.End_Site)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Pessoa_Juridica>()
                .Property(e => e.Num_FAP)
                .HasPrecision(6, 4);

            modelBuilder.Entity<TAB_Pessoa_Juridica>()
                .Property(e => e.Num_PAT)
                .HasPrecision(12, 0);

            modelBuilder.Entity<TAB_Pessoa_Juridica>()
                .HasMany(e => e.TAB_Endereco_Pessoa_Juridica)
                .WithRequired(e => e.TAB_Pessoa_Juridica)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Pessoa_Juridica>()
                .HasMany(e => e.TAB_Inscricao_Estadual)
                .WithRequired(e => e.TAB_Pessoa_Juridica)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Pessoa_Juridica>()
                .HasMany(e => e.TAB_Pessoa_Juridica_Logo)
                .WithRequired(e => e.TAB_Pessoa_Juridica)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Plugin>()
                .Property(e => e.Des_Plugin)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Plugin>()
                .Property(e => e.Des_Plugin_Metadata)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Plugin>()
                .HasMany(e => e.TAB_Plugins_Compatibilidade)
                .WithRequired(e => e.TAB_Plugin)
                .HasForeignKey(e => e.Idf_Plugin_Entrada)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Plugin>()
                .HasMany(e => e.TAB_Plugins_Compatibilidade1)
                .WithRequired(e => e.TAB_Plugin1)
                .HasForeignKey(e => e.Idf_Plugin_Saida)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Plugins_Compatibilidade>()
                .HasMany(e => e.TAB_Atividade)
                .WithRequired(e => e.TAB_Plugins_Compatibilidade)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Porte_Empresa>()
                .Property(e => e.Des_Porte_Empresa)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Raca>()
                .Property(e => e.Des_Raca)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Raca>()
                .Property(e => e.Des_Raca_BNE)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_RAT>()
                .Property(e => e.Des_RAT)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Rede_Social>()
                .Property(e => e.Des_Rede_Social)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Regiao>()
                .Property(e => e.Nme_Regiao)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Sexo>()
                .Property(e => e.Des_Sexo)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Sexo>()
                .Property(e => e.Sig_Sexo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Sexo>()
                .HasMany(e => e.BNE_Integracao)
                .WithRequired(e => e.TAB_Sexo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Simples>()
                .Property(e => e.Des_Simples)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Sistema>()
                .Property(e => e.Nme_Sistema)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Sistema>()
                .HasMany(e => e.TAB_Mensagem)
                .WithRequired(e => e.TAB_Sistema)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Status_Atividade>()
                .Property(e => e.Des_Status_Atividade)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Status_Atividade>()
                .HasMany(e => e.TAB_Atividade)
                .WithRequired(e => e.TAB_Status_Atividade)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Status_Mensagem>()
                .Property(e => e.Des_Status_Mensagem)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Tipo_Atividade>()
                .Property(e => e.Des_Tipo_Atividade)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Tipo_Atividade>()
                .HasMany(e => e.TAB_Plugin)
                .WithRequired(e => e.TAB_Tipo_Atividade)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Tipo_Contato>()
                .Property(e => e.Des_Tipo_Contato)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Tipo_Endereco>()
                .Property(e => e.Des_Tipo_Endereco)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Tipo_Endereco>()
                .HasMany(e => e.TAB_Endereco_Pessoa_Juridica)
                .WithRequired(e => e.TAB_Tipo_Endereco)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Tipo_Logo>()
                .Property(e => e.Des_Tipo_Logo)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Tipo_Logo>()
                .HasMany(e => e.TAB_Pessoa_Juridica_Logo)
                .WithRequired(e => e.TAB_Tipo_Logo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Tipo_Mensagem>()
                .Property(e => e.Des_Tipo_Mensagem)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Tipo_Plugin>()
                .Property(e => e.Des_Tipo_Plugin)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Tipo_Plugin>()
                .HasMany(e => e.TAB_Plugin)
                .WithRequired(e => e.TAB_Tipo_Plugin)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Tipo_Saida>()
                .Property(e => e.Des_Tipo_Saida)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Tipo_Sanguineo>()
                .Property(e => e.Des_Tipo_Sanguineo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Tipo_Transacao>()
                .Property(e => e.Des_Transacao)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Tipo_Veiculo>()
                .Property(e => e.Des_Tipo_Veiculo)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Tipo_Veiculo>()
                .HasMany(e => e.TAB_Pessoa_Fisica_Veiculo)
                .WithRequired(e => e.TAB_Tipo_Veiculo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAB_Tipo_Vinculo_Integracao>()
                .Property(e => e.Des_Tipo_Vinculo)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Tipo_Vinculo_Integracao>()
                .HasMany(e => e.BNE_Integracao)
                .WithRequired(e => e.TAB_Tipo_Vinculo_Integracao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BNE_Gerente_Filial_New>()
                .Property(e => e.Nme_Gerente_Filial)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Gerente_Filial_New>()
                .Property(e => e.Num_CPF)
                .HasPrecision(11, 0);

            modelBuilder.Entity<BNE_Gerente_Filial_New>()
                .Property(e => e.Eml_Pessoa)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Gerente_Filial_New>()
                .Property(e => e.Ape_Filial)
                .IsUnicode(false);

            modelBuilder.Entity<BNE_Propaganda_Email>()
                .Property(e => e.Num_Cpf)
                .HasPrecision(11, 0);

            modelBuilder.Entity<log_queue>()
                .Property(e => e.Des_ERROR_MESSAGE)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Campanha_CV_Desatualizado>()
                .Property(e => e.Num_CPF)
                .HasPrecision(11, 0);

            modelBuilder.Entity<TAB_Mensagem_Mailing>()
                .Property(e => e.Des_Mensagem)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Mensagem_Mailing>()
                .Property(e => e.Des_Email_Remetente)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Mensagem_Mailing>()
                .Property(e => e.Des_Email_Destino)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Mensagem_Mailing>()
                .Property(e => e.Des_Assunto)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Mensagem_Mailing>()
                .Property(e => e.Nme_Anexo)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Mensagem_Mailing>()
                .Property(e => e.Num_DDD_Celular)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Mensagem_Mailing>()
                .Property(e => e.Num_Celular)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Substituicao_Integracao>()
                .Property(e => e.Des_Antiga)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_Substituicao_Integracao>()
                .Property(e => e.Des_Nova)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_random>()
                .Property(e => e.Nme_Localidade)
                .IsUnicode(false);

            modelBuilder.Entity<TAB_random>()
                .Property(e => e.Sig_Estado)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
