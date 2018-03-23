using System;
using System.Text;
using System.Data.SqlClient;

namespace BNE.BLL.Custom
{
    public static class ModeloEmail
    {
        
        #region CorpoVisualizacaoCadastroEmpresa
        public static string CorpoVisualizacaoCadastroEmpresa(Filial objFilial, bool montarCabecalho, SqlTransaction trans = null)
        {
            const string verde = "http://www.bne.com.br/img/btn_green.png";
            const string vermelho = "http://www.bne.com.br/img/btn_red.png";

            var sb = new StringBuilder();

            #region Abre a TABLE
            sb.Append("<table>");
            #endregion

            #region Cabecalho

            if (montarCabecalho)
            {
                sb.Append(@"<tr><TD line-height='200%\' background-color='yellow' >Parabéns! Sua empresa foi incluída com sucesso em nosso banco de dados. <BR/>
                                Aguarde a liberação de seu acesso para pesquisar currículos e enviar convites para entrevista! <BR/>
                                Abaixo os dados que foram incluídos em nosso cadastro: <BR/>
                        </td></tr>");
            }
            #endregion

            #region Usuario Master
            UsuarioFilialPerfil objUsuarioFilialPerfil;
            if (UsuarioFilialPerfil.CarregarPorPerfilFilial(BLL.Enumeradores.Perfil.AcessoEmpresaMaster.GetHashCode(), objFilial.IdFilial, out objUsuarioFilialPerfil, trans != null ? trans : null))
            {
                if (trans != null)
                    objUsuarioFilialPerfil.PessoaFisica.CompleteObject(trans);
                else
                    objUsuarioFilialPerfil.PessoaFisica.CompleteObject();

                //TODO - Rever complete object para recuperar poucos campos
                sb.Append("<tr><td>Usuário Master da Empresa</td></tr>");
                sb.Append(String.Format("<tr><td>CPF: {0}</td></tr>", objUsuarioFilialPerfil.PessoaFisica.NumeroCPF));
                sb.Append(String.Format("<tr><td>Data Nascimento: {0}</td></tr>", objUsuarioFilialPerfil.PessoaFisica.DataNascimento.ToShortDateString()));
                sb.Append(String.Format("<tr><td>Nome Usuário: {0}</td></tr>", objUsuarioFilialPerfil.PessoaFisica.NomeCompleto));
                if (objUsuarioFilialPerfil.PessoaFisica.Sexo != null)
                {
                    if (trans != null)
                        objUsuarioFilialPerfil.PessoaFisica.Sexo.CompleteObject(trans);
                    else
                        objUsuarioFilialPerfil.PessoaFisica.Sexo.CompleteObject();

                    sb.Append(String.Format("<tr><td>Gênero: {0}</td></tr>", objUsuarioFilialPerfil.PessoaFisica.Sexo.DescricaoSexo));
                }

                UsuarioFilial objUsuarioFilial;
                if (UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(objUsuarioFilialPerfil.IdUsuarioFilialPerfil, out objUsuarioFilial, trans != null ? trans : null))
                {
                    if (objUsuarioFilial.Funcao != null)
                    {
                        if (trans != null)
                            objUsuarioFilial.Funcao.CompleteObject(trans);
                        else
                            objUsuarioFilial.Funcao.CompleteObject();

                        sb.Append(String.Format("<tr><td>Função Exercida: {0}</td></tr>", objUsuarioFilial.Funcao.DescricaoFuncao));
                    }
                    else
                        sb.Append(String.Format("<tr><td>Função Exercida: {0}</td></tr>", objUsuarioFilial.DescricaoFuncao));

                    bool emailInvalido = false;
                    if (!String.IsNullOrEmpty(objUsuarioFilial.EmailComercial))
                    {
                        var email = objUsuarioFilial.EmailComercial;
                        var domainwithat = email.Substring(email.IndexOf('@'));
                        emailInvalido = DeParaEmail.ExistsDomainWithAt(domainwithat);
                    }

                    sb.Append(String.Format("<tr><td>Email: {0} &nbsp; <img src=\"{1}\"/></td></tr>", objUsuarioFilial.EmailComercial, emailInvalido ? vermelho : verde));
                    sb.Append(String.Format("<tr><td>Fone Comercial: ({0}) {1}</td></tr>", objUsuarioFilial.NumeroDDDComercial, objUsuarioFilial.NumeroComercial));
                }
                sb.Append(String.Format("<tr><td>Celular: ({0}) {1}</td></tr>", objUsuarioFilialPerfil.PessoaFisica.NumeroDDDCelular, objUsuarioFilialPerfil.PessoaFisica.NumeroCelular));
            }
            #endregion

            #region Dados Gerais
            sb.Append("<tr><td>Dados Gerais da Empresa</td></tr>");
            sb.Append(String.Format("<tr><td>CNPJ: {0}</td></tr>", objFilial.NumeroCNPJ.Value.ToString()));
            sb.Append(String.Format("<tr><td>Site: {0}</td></tr>", objFilial.EnderecoSite));
            sb.Append(String.Format("<tr><td>Número de Funcionários: {0}</td></tr>", objFilial.QuantidadeFuncionarios.ToString()));
            sb.Append(String.Format("<tr><td>Comercial: ({0}) {1}</td></tr>", objFilial.NumeroDDDComercial, objFilial.NumeroComercial));
            #endregion

            int totalRegistros;
            Filial.ListarFiliaisDadosRepetidos(objFilial.IdFilial, 0, 0, out totalRegistros);

            #region Cartão CNPJ
            sb.AppendFormat("<tr><td>Cartão do CNPJ&nbsp; <img src=\"{0}\"/></td></tr>", totalRegistros == 0 ? verde : vermelho);
            sb.Append(String.Format("<tr><td>Razão Social: {0}</td></tr>", objFilial.RazaoSocial));
            sb.Append(String.Format("<tr><td>Nome Fantasia ou Apelido: {0}</td></tr>", objFilial.NomeFantasia));
            CNAESubClasse objCNAESubClasse;

            if (trans != null)
                objFilial.CNAEPrincipal.CompleteObject(trans);
            else
                objFilial.CNAEPrincipal.CompleteObject();

            if (CNAESubClasse.CarregarPorCodigo(objFilial.CNAEPrincipal.CodigoCNAESubClasse, out objCNAESubClasse, trans != null ? trans : null))
            {
                sb.Append(String.Format("<tr><td>CNAE: {0} - {1}</td></tr>", objFilial.CNAEPrincipal.CodigoCNAESubClasse, objFilial.CNAEPrincipal.DescricaoCNAESubClasse));
            }

            if (trans != null)
                objFilial.CNAEPrincipal.CompleteObject(trans);
            else
                objFilial.CNAEPrincipal.CompleteObject();

            objFilial.CNAEPrincipal.CompleteObject();

            NaturezaJuridica objNaturezaJuridica;
            objFilial.NaturezaJuridica.CompleteObject();
            if (NaturezaJuridica.CarregarPorCodigo(objFilial.NaturezaJuridica.CodigoNaturezaJuridica, out objNaturezaJuridica, trans != null ? trans : null))
            {
                sb.Append(String.Format("<tr><td>Natureza Jurídica: {0} - {1}</td></tr>", objFilial.NaturezaJuridica.CodigoNaturezaJuridica, objFilial.NaturezaJuridica.DescricaoNaturezaJuridica));
            }

            if (objFilial.Endereco != null)
            {
                if (trans != null)
                    objFilial.Endereco.CompleteObject(trans);
                else
                    objFilial.Endereco.CompleteObject();


                sb.Append(String.Format("<tr><td>CEP: {0}</td></tr>", objFilial.Endereco.NumeroCEP));
                sb.Append(String.Format("<tr><td>Endereço: {0}, {1}, {2} - {3}</td></tr>", objFilial.Endereco.DescricaoLogradouro, objFilial.Endereco.NumeroEndereco.ToString(), objFilial.Endereco.DescricaoComplemento, objFilial.Endereco.DescricaoBairro));

                if (objFilial.Endereco.Cidade != null)
                {
                    if (trans != null)
                        objFilial.Endereco.Cidade.CompleteObject(trans);
                    else
                        objFilial.Endereco.Cidade.CompleteObject();

                    sb.Append(String.Format("<tr><td>CEP: {0} - {1}</td></tr>", objFilial.Endereco.Cidade.NomeCidade, objFilial.Endereco.Cidade.Estado.SiglaEstado));
                }
            }

            #endregion

            #region Fecha a TABLE
            sb.Append("</table>");
            #endregion

            return sb.ToString();
        }
        #endregion

    }
}
