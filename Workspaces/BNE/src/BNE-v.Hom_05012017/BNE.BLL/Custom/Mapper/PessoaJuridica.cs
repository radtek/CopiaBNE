using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BNE.BLL.Custom.Mapper
{
    public class PessoaJuridica
    {

        private readonly Regex _apenasNumeros = new Regex(@"^\d+$");

        public Boolean Salvar(Filial objFilial, List<UsuarioFilialPerfil> listaUsuarios, SqlTransaction trans)
        {
            var pathToDLL = ConfigurationManager.AppSettings["PathToMapperDLL"];

            var assemblyMapper = Assembly.LoadFrom(pathToDLL + "/BNE.Mapper.dll");
            var assemblyMapperToNew = Assembly.LoadFrom(pathToDLL + "/BNE.Mapper.ToNew.dll");

            var objPessoaJuridica = Activator.CreateInstance(assemblyMapperToNew.GetType("BNE.Mapper.ToNew.PessoaJuridica"));
            var objPessoaJuridicaModel = Activator.CreateInstance(assemblyMapper.GetType("BNE.Mapper.Models.PessoaJuridica.PessoaJuridica"));
            var constructorInfo = typeof(List<>).MakeGenericType(assemblyMapper.GetType("BNE.Mapper.Models.PessoaJuridica.Usuario")).GetConstructor(Type.EmptyTypes);
            if (constructorInfo != null)
            {
                var objListaUsuario = (IList)constructorInfo.Invoke(null);

                foreach (UsuarioFilialPerfil objUsuarioFilialPerfil in listaUsuarios)
                {
                    objUsuarioFilialPerfil.PessoaFisica.CompleteObject(trans);
                    objUsuarioFilialPerfil.PessoaFisica.Sexo.CompleteObject(trans);

                    var objUsuarioPessoaJuridicaModelAdicional = Activator.CreateInstance(assemblyMapper.GetType("BNE.Mapper.Models.PessoaJuridica.Usuario"));
                    SetValue(objUsuarioPessoaJuridicaModelAdicional, "NumeroCPF", objUsuarioFilialPerfil.PessoaFisica.CPF);
                    SetValue(objUsuarioPessoaJuridicaModelAdicional, "Nome", objUsuarioFilialPerfil.PessoaFisica.NomeCompleto);
                    SetValue(objUsuarioPessoaJuridicaModelAdicional, "DataNascimento", objUsuarioFilialPerfil.PessoaFisica.DataNascimento);
                    SetValue(objUsuarioPessoaJuridicaModelAdicional, "Email", objUsuarioFilialPerfil.PessoaFisica.EmailPessoa ?? string.Empty);

                    if (!string.IsNullOrWhiteSpace(objUsuarioFilialPerfil.PessoaFisica.NumeroDDDCelular) && _apenasNumeros.IsMatch(objUsuarioFilialPerfil.PessoaFisica.NumeroDDDCelular.Trim()) &&
                    !string.IsNullOrWhiteSpace(objUsuarioFilialPerfil.PessoaFisica.NumeroCelular) && _apenasNumeros.IsMatch(objUsuarioFilialPerfil.PessoaFisica.NumeroCelular.Trim()))
                    {
                        SetValue(objUsuarioPessoaJuridicaModelAdicional, "NumeroDDDCelular", objUsuarioFilialPerfil.PessoaFisica.NumeroDDDCelular);
                        SetValue(objUsuarioPessoaJuridicaModelAdicional, "NumeroCelular", objUsuarioFilialPerfil.PessoaFisica.NumeroCelular);
                    }

                    SetValue(objUsuarioPessoaJuridicaModelAdicional, "DataCadastro", objUsuarioFilialPerfil.DataCadastro);
                    SetValue(objUsuarioPessoaJuridicaModelAdicional, "DataAlteracao", objUsuarioFilialPerfil.DataAlteracao);
                    SetValue(objUsuarioPessoaJuridicaModelAdicional, "Sexo", objUsuarioFilialPerfil.PessoaFisica.Sexo.SiglaSexo);
                    SetValue(objUsuarioPessoaJuridicaModelAdicional, "UsuarioMaster", objUsuarioFilialPerfil.Perfil.IdPerfil == (int)Enumeradores.Perfil.AcessoEmpresaMaster);
                    SetValue(objUsuarioPessoaJuridicaModelAdicional, "UsuarioInativo", objUsuarioFilialPerfil.FlagInativo);
                    SetValue(objUsuarioPessoaJuridicaModelAdicional, "IP", !string.IsNullOrWhiteSpace(objUsuarioFilialPerfil.DescricaoIP) ? objUsuarioFilialPerfil.DescricaoIP : objFilial.DescricaoIP);

                    UsuarioFilial objUsuarioFilial;
                    if (UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(objUsuarioFilialPerfil.IdUsuarioFilialPerfil, out objUsuarioFilial))
                    {
                        if (objUsuarioFilial.Funcao != null)
                        {
                            objUsuarioFilial.Funcao.CompleteObject();
                            SetValue(objUsuarioPessoaJuridicaModelAdicional, "Funcao", string.Empty);
                            SetValue(objUsuarioPessoaJuridicaModelAdicional, "IdFuncaoVelho", objUsuarioFilial.Funcao.IdFuncao);
                        }
                        else
                            SetValue(objUsuarioPessoaJuridicaModelAdicional, "Funcao", objUsuarioFilial.DescricaoFuncao);

                        SetValue(objUsuarioPessoaJuridicaModelAdicional, "EmailComercial", objUsuarioFilial.EmailComercial);
                        SetValue(objUsuarioPessoaJuridicaModelAdicional, "NumeroDDDComercial", objUsuarioFilial.NumeroDDDComercial);
                        SetValue(objUsuarioPessoaJuridicaModelAdicional, "NumeroComercial", objUsuarioFilial.NumeroComercial);

                        if (!string.IsNullOrWhiteSpace(objUsuarioFilial.NumeroRamal))
                            SetValue(objUsuarioPessoaJuridicaModelAdicional, "NumeroRamal", objUsuarioFilial.NumeroRamal);
                    }

                    objListaUsuario.Add(objUsuarioPessoaJuridicaModelAdicional);
                }

                #region Dados Pessoa Juridica
                SetValue(objPessoaJuridicaModel, "NumeroCNPJ", objFilial.NumeroCNPJ);
                SetValue(objPessoaJuridicaModel, "RazaoSocial", objFilial.RazaoSocial);
                SetValue(objPessoaJuridicaModel, "NomeFantasia", objFilial.NomeFantasia);
                SetValue(objPessoaJuridicaModel, "QuantidadeFuncionario", objFilial.QuantidadeFuncionarios);
                SetValue(objPessoaJuridicaModel, "Site", objFilial.EnderecoSite);
                if (objFilial.CNAEPrincipal != null)
                    SetValue(objPessoaJuridicaModel, "CNAE", objFilial.CNAEPrincipal.CodigoCNAESubClasse);
                if (objFilial.NaturezaJuridica != null)
                    SetValue(objPessoaJuridicaModel, "NaturezaJuridica", objFilial.NaturezaJuridica.CodigoNaturezaJuridica);
                if (!string.IsNullOrWhiteSpace(objFilial.Endereco.NumeroCEP) && _apenasNumeros.IsMatch(objFilial.Endereco.NumeroCEP))
                    SetValue(objPessoaJuridicaModel, "CEP", objFilial.Endereco.NumeroCEP);
                SetValue(objPessoaJuridicaModel, "Bairro", string.IsNullOrWhiteSpace(objFilial.Endereco.DescricaoBairro) ? null : objFilial.Endereco.DescricaoBairro);
                SetValue(objPessoaJuridicaModel, "Complemento", objFilial.Endereco.DescricaoComplemento);
                SetValue(objPessoaJuridicaModel, "Logradouro", objFilial.Endereco.DescricaoLogradouro ?? string.Empty);
                SetValue(objPessoaJuridicaModel, "Numero", objFilial.Endereco.NumeroEndereco);
                SetValue(objPessoaJuridicaModel, "IP", objFilial.DescricaoIP);

                if (!string.IsNullOrWhiteSpace(objFilial.NumeroComercial) && _apenasNumeros.IsMatch(objFilial.NumeroComercial.Trim()) &&
                    !string.IsNullOrWhiteSpace(objFilial.NumeroDDDComercial) && _apenasNumeros.IsMatch(objFilial.NumeroDDDComercial.Trim()))
                {
                    SetValue(objPessoaJuridicaModel, "NumeroDDDComercial", objFilial.NumeroDDDComercial);
                    SetValue(objPessoaJuridicaModel, "NumeroComercial", objFilial.NumeroComercial);
                }

                SetValue(objPessoaJuridicaModel, "DataCadastro", objFilial.DataCadastro);
                SetValue(objPessoaJuridicaModel, "DataAbertura", new DateTime(1900, 1, 1));
                SetValue(objPessoaJuridicaModel, "DataAlteracao", objFilial.DataAlteracao);
                SetValue(objPessoaJuridicaModel, "Cidade", Helper.FormatarCidade(objFilial.Endereco.Cidade.NomeCidade, objFilial.Endereco.Cidade.Estado.SiglaEstado));
                SetValue(objPessoaJuridicaModel, "Usuarios", objListaUsuario);
                #endregion
            }

            var method = objPessoaJuridica.GetType().GetMethod("Map");

            var retorno = Convert.ToBoolean(method.Invoke(objPessoaJuridica, new[] { objPessoaJuridicaModel }));
            return retorno;
        }

        private void SetValue(object instance, string property, object value)
        {
            PropertyInfo propertyInfo = instance.GetType().GetProperty(property);

            Type t = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;

            object safeValue = (value == null) ? null : Convert.ChangeType(value, t);

            propertyInfo.SetValue(instance, safeValue, null);
        }

    }

}
