//-- Data: 04/04/2013 15:24
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;

namespace BNE.BLL
{
    public partial class Integracao // Tabela: plataforma.BNE_Integracao
    {

        #region Sprecuperarintegracoes
        private const string Sprecuperarintegracoes = @"
        SELECT * FROM plataforma.BNE_Integracao WHERE Dta_Integracao IS NULL AND ( Idf_Integracao_Situacao = 1 OR Idf_Integracao_Situacao = 3 )
        ";
        #endregion

        #region RecuperarIntegracoes
        /// <summary>
        /// Recupera as integracoes não integradas
        /// </summary>
        /// <returns>Uma datatable com os emails não enviados</returns>
        public static List<Integracao> RecuperarIntegracoes()
        {
            var lista = new List<Integracao>();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Sprecuperarintegracoes, null))
            {
                while (dr.Read())
                {
                    var objIntegracao = new Integracao();

                    if (SetInstance_NonDispose(dr, objIntegracao))
                        lista.Add(objIntegracao);
                }
            }
            return lista;
        }
        #endregion

        #region SetInstance_NonDispose
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objIntegracao">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance_NonDispose(IDataReader dr, Integracao objIntegracao)
        {
            objIntegracao._idIntegracao = Convert.ToInt32(dr["Idf_Integracao"]);
            if (dr["Num_CPF"] != DBNull.Value)
                objIntegracao._numeroCPF = Convert.ToDecimal(dr["Num_CPF"]);
            objIntegracao._nomePessoa = Convert.ToString(dr["Nme_Pessoa"]);
            if (dr["Ape_Pessoa"] != DBNull.Value)
                objIntegracao._apelidoPessoa = Convert.ToString(dr["Ape_Pessoa"]);
            objIntegracao._sexo = new Sexo(Convert.ToInt32(dr["Idf_Sexo"]));
            objIntegracao._dataNascimento = Convert.ToDateTime(dr["Dta_Nascimento"]);
            if (dr["Nme_Mae"] != DBNull.Value)
                objIntegracao._nomeMae = Convert.ToString(dr["Nme_Mae"]);
            if (dr["Nme_Pai"] != DBNull.Value)
                objIntegracao._nomePai = Convert.ToString(dr["Nme_Pai"]);
            if (dr["Num_RG"] != DBNull.Value)
                objIntegracao._numeroRG = Convert.ToString(dr["Num_RG"]);
            if (dr["Dta_Expedicao_RG"] != DBNull.Value)
                objIntegracao._dataExpedicaoRG = Convert.ToDateTime(dr["Dta_Expedicao_RG"]);
            if (dr["Nme_Orgao_Emissor"] != DBNull.Value)
                objIntegracao._nomeOrgaoEmissor = Convert.ToString(dr["Nme_Orgao_Emissor"]);
            if (dr["Sig_UF_Emissao_RG"] != DBNull.Value)
                objIntegracao._siglaUFEmissaoRG = Convert.ToString(dr["Sig_UF_Emissao_RG"]);
            if (dr["Idf_Raca"] != DBNull.Value)
                objIntegracao._raca = new Raca(Convert.ToInt32(dr["Idf_Raca"]));
            if (dr["Idf_Deficiencia"] != DBNull.Value)
                objIntegracao._deficiencia = new Deficiencia(Convert.ToInt32(dr["Idf_Deficiencia"]));
            if (dr["Des_Logradouro"] != DBNull.Value)
                objIntegracao._descricaoLogradouro = Convert.ToString(dr["Des_Logradouro"]);
            if (dr["Des_Complemento"] != DBNull.Value)
                objIntegracao._descricaoComplemento = Convert.ToString(dr["Des_Complemento"]);
            if (dr["Num_Endereco"] != DBNull.Value)
                objIntegracao._numeroEndereco = Convert.ToString(dr["Num_Endereco"]);
            if (dr["Num_CEP"] != DBNull.Value)
                objIntegracao._numeroCEP = Convert.ToString(dr["Num_CEP"]);
            if (dr["Des_Bairro"] != DBNull.Value)
                objIntegracao._descricaoBairro = Convert.ToString(dr["Des_Bairro"]);
            objIntegracao._cidade = new Cidade(Convert.ToInt32(dr["Idf_Cidade"]));
            if (dr["Num_DDD_Telefone"] != DBNull.Value)
                objIntegracao._numeroDDDTelefone = Convert.ToString(dr["Num_DDD_Telefone"]);
            if (dr["Num_Telefone"] != DBNull.Value)
                objIntegracao._numeroTelefone = Convert.ToString(dr["Num_Telefone"]);
            if (dr["Num_DDD_Celular"] != DBNull.Value)
                objIntegracao._numeroDDDCelular = Convert.ToString(dr["Num_DDD_Celular"]);
            if (dr["Num_Celular"] != DBNull.Value)
                objIntegracao._numeroCelular = Convert.ToString(dr["Num_Celular"]);
            if (dr["Eml_Pessoa"] != DBNull.Value)
                objIntegracao._emailPessoa = Convert.ToString(dr["Eml_Pessoa"]);
            if (dr["Idf_Escolaridade"] != DBNull.Value)
                objIntegracao._escolaridade = new Escolaridade(Convert.ToInt32(dr["Idf_Escolaridade"]));
            if (dr["Idf_Estado_Civil"] != DBNull.Value)
                objIntegracao._estadoCivil = new EstadoCivil(Convert.ToInt32(dr["Idf_Estado_Civil"]));
            if (dr["Raz_Social"] != DBNull.Value)
                objIntegracao._razaoSocial = Convert.ToString(dr["Raz_Social"]);
            objIntegracao._funcao = new Funcao(Convert.ToInt32(dr["Idf_funcao"]));
            if (dr["Dta_Admissao"] != DBNull.Value)
                objIntegracao._dataAdmissao = Convert.ToDateTime(dr["Dta_Admissao"]);
            if (dr["Dta_Saida_Prevista"] != DBNull.Value)
                objIntegracao._dataSaidaPrevista = Convert.ToDateTime(dr["Dta_Saida_Prevista"]);
            objIntegracao._valorSalario = Convert.ToDecimal(dr["Vlr_Salario"]);
            if (dr["Num_Habilitacao"] != DBNull.Value)
                objIntegracao._numeroHabilitacao = Convert.ToString(dr["Num_Habilitacao"]);
            if (dr["Idf_Categoria_Habilitacao"] != DBNull.Value)
                objIntegracao._categoriaHabilitacao = new CategoriaHabilitacao(Convert.ToInt32(dr["Idf_Categoria_Habilitacao"]));
            if (dr["Flg_filhos"] != DBNull.Value)
                objIntegracao._flagfilhos = Convert.ToBoolean(dr["Flg_filhos"]);
            if (dr["Idf_Tipo_Veiculo"] != DBNull.Value)
                objIntegracao._tipoVeiculo = new TipoVeiculo(Convert.ToInt32(dr["Idf_Tipo_Veiculo"]));
            if (dr["Ano_Veiculo"] != DBNull.Value)
                objIntegracao._anoVeiculo = Convert.ToInt32(dr["Ano_Veiculo"]);
            objIntegracao._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
            if (dr["Dta_Integracao"] != DBNull.Value)
                objIntegracao._dataIntegracao = Convert.ToDateTime(dr["Dta_Integracao"]);
            objIntegracao._integracaoSituacao = new IntegracaoSituacao(Convert.ToInt32(dr["Idf_Integracao_Situacao"]));
            objIntegracao._tipoVinculoIntegracao = new TipoVinculoIntegracao(Convert.ToInt32(dr["Idf_Tipo_Vinculo_Integracao"]));
            objIntegracao._motivoRescisao = new MotivoRescisao(Convert.ToInt32(dr["Idf_Motivo_Rescisao"]));

            objIntegracao._persisted = true;
            objIntegracao._modified = false;

            return true;
        }

        #endregion

    }
}