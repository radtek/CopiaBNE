using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using Usuario = BNE.Mapper.Models.PessoaJuridica.Usuario;

namespace BNE.Mapper.ToOld
{
    public class PessoaJuridica
    {

        public bool Map(Models.PessoaJuridica.PessoaJuridica commonObject)
        {
            using (SqlConnection conn = new SqlConnection(BLL.DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        bool novo = false;
                        BLL.Filial objFilial;
                        if (!BLL.Filial.CarregarPorCnpj(commonObject.NumeroCNPJ, out objFilial))
                        {
                            objFilial = new BLL.Filial { NumeroCNPJ = commonObject.NumeroCNPJ, SituacaoFilial = new BLL.SituacaoFilial((int)BLL.Enumeradores.SituacaoFilial.AguardandoPublicacao) };
                            novo = true;
                        }

                        objFilial.DataCadastro = commonObject.DataCadastro;
                        objFilial.DataAlteracao = commonObject.DataAlteracao;
                        objFilial.RazaoSocial = commonObject.RazaoSocial;
                        objFilial.NomeFantasia = !string.IsNullOrEmpty(commonObject.NomeFantasia) ? commonObject.NomeFantasia : commonObject.RazaoSocial;
                        objFilial.EnderecoSite = commonObject.Site;
                        objFilial.QuantidadeFuncionarios = commonObject.QuantidadeFuncionario;

                        BLL.CNAESubClasse objCNAESubClasse;
                        if (BLL.CNAESubClasse.CarregarPorCodigo(commonObject.CNAE, out objCNAESubClasse))
                            objFilial.CNAEPrincipal = objCNAESubClasse;

                        BLL.NaturezaJuridica objNaturezaJuridica;
                        if (BLL.NaturezaJuridica.CarregarPorCodigo(commonObject.NaturezaJuridica, out objNaturezaJuridica))
                            objFilial.NaturezaJuridica = objNaturezaJuridica;

                        if (objFilial.Endereco != null)
                            objFilial.Endereco.CompleteObject(trans);
                        else
                            objFilial.Endereco = new BLL.Endereco { DataCadastro = commonObject.DataCadastro };

                        objFilial.Endereco.NumeroCEP = commonObject.CEP.ToString();
                        objFilial.Endereco.DescricaoLogradouro = commonObject.Logradouro;
                        objFilial.Endereco.NumeroEndereco = commonObject.Numero;
                        objFilial.Endereco.DescricaoComplemento = commonObject.Complemento;
                        objFilial.Endereco.DescricaoBairro = commonObject.Bairro;
                        objFilial.Endereco.DataAlteracao = commonObject.DataAlteracao;

                        BLL.Cidade objCidade;
                        if (BLL.Cidade.CarregarPorNome(commonObject.Cidade, out objCidade))
                            objFilial.Endereco.Cidade = objCidade;

                        objFilial.NumeroDDDComercial = commonObject.NumeroDDDComercial;
                        objFilial.NumeroComercial = commonObject.NumeroComercial.ToString(CultureInfo.InvariantCulture);
                        objFilial.DescricaoIP = commonObject.IP;

                        objFilial.Endereco.SalvarMigracao(trans);
                        objFilial.SalvarMigracao(trans);

                        MapUsuarios(commonObject.Usuarios, objFilial, novo, trans);

                        if (novo)
                        {
                            var objParametroSaldoCriacaoCampanha = new BLL.ParametroFilial
                            {
                                IdParametro = (int)BLL.Enumeradores.Parametro.CampanhaRecrutamentoQuantidadeSaldoEnvioCampanha,
                                IdFilial = objFilial.IdFilial,
                                ValorParametro = BLL.Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.CampanhaRecrutamentoQuantidadeSaldoEnvioCampanha),
                                FlagInativo = false
                            };
                            objParametroSaldoCriacaoCampanha.Save(trans);

                            var objParametroAutorizoBNEPublicarVagas = new BLL.ParametroFilial
                            {
                                IdParametro = (int)BLL.Enumeradores.Parametro.AutorizoBNEPublicarVagas,
                                IdFilial = objFilial.IdFilial,
                                ValorParametro = BLL.Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.AutorizoBNEPublicarVagas),
                                FlagInativo = false
                            };
                            objParametroAutorizoBNEPublicarVagas.Save(trans);
                        }

                        trans.Commit();

                        return true;
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

        }

        public bool MapUsuarios(Models.PessoaJuridica.PessoaJuridica commonObject)
        {
            using (SqlConnection conn = new SqlConnection(BLL.DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        BLL.Filial objFilial;
                        if (!BLL.Filial.CarregarPorCnpj(commonObject.NumeroCNPJ, out objFilial))
                            objFilial = new BLL.Filial { NumeroCNPJ = commonObject.NumeroCNPJ, SituacaoFilial = new BLL.SituacaoFilial((int)BLL.Enumeradores.SituacaoFilial.AguardandoPublicacao) };

                        MapUsuarios(commonObject.Usuarios, objFilial, false, trans);

                        trans.Commit();

                        return true;
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        private void MapUsuarios(List<Usuario> usuarios, BLL.Filial objFilial, bool novoCadastroPJ, SqlTransaction trans)
        {
            if (usuarios != null)
            {
                foreach (var usuario in usuarios)
                {
                    BLL.PessoaFisica objPessoaFisica;
                    BLL.UsuarioFilialPerfil objUsuarioFilialPerfil;
                    BLL.UsuarioFilial objUsuarioFilial;
                    BLL.Usuario objUsuario;

                    //Usuario objUsuario;
                    if (BLL.PessoaFisica.CarregarPorCPF(usuario.NumeroCPF, out objPessoaFisica))
                    {
                        if (BLL.UsuarioFilialPerfil.CarregarPorPessoaFisicaFilial(objPessoaFisica.IdPessoaFisica, objFilial.IdFilial, out objUsuarioFilialPerfil))
                        {
                            if (!BLL.UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(objUsuarioFilialPerfil.IdUsuarioFilialPerfil, out objUsuarioFilial))
                                objUsuarioFilial = Criar(objUsuarioFilialPerfil, usuario.DataCadastro);
                        }
                        else
                        {
                            objUsuarioFilialPerfil = Criar(objFilial, objPessoaFisica, usuario.DataCadastro, usuario.IP);
                            objUsuarioFilial = Criar(objUsuarioFilialPerfil, usuario.DataCadastro);
                        }

                        if (!BLL.Usuario.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objUsuario, trans))
                            objUsuario = Criar(objPessoaFisica, usuario.DataCadastro);

                    }
                    else
                    {
                        objPessoaFisica = new BLL.PessoaFisica
                        {
                            CPF = usuario.NumeroCPF,
                            DescricaoIP = usuario.IP,
                            DataCadastro = usuario.DataCadastro
                        };

                        objUsuarioFilialPerfil = Criar(objFilial, objPessoaFisica, usuario.DataCadastro, usuario.IP);
                        objUsuarioFilial = Criar(objUsuarioFilialPerfil, usuario.DataCadastro);
                        objUsuario = Criar(objPessoaFisica, usuario.DataCadastro);
                    }

                    objPessoaFisica.DataNascimento = usuario.DataNascimento;
                    objPessoaFisica.NomePessoa = usuario.Nome;
                    objPessoaFisica.NomePessoaPesquisa = usuario.Nome;

                    if (!string.IsNullOrWhiteSpace(usuario.Email))
                        objPessoaFisica.EmailPessoa = usuario.Email;

                    objPessoaFisica.NumeroDDDCelular = usuario.NumeroDDDCelular;
                    objPessoaFisica.NumeroCelular = usuario.NumeroCelular.ToString(CultureInfo.InvariantCulture);

                    if (!string.IsNullOrWhiteSpace(usuario.Sexo))
                        objPessoaFisica.Sexo = new BLL.Sexo(usuario.Sexo.ToLower().Equals("m") ? (int)BLL.Enumeradores.Sexo.Masculino : (int)BLL.Enumeradores.Sexo.Feminino);

                    objUsuarioFilialPerfil.Perfil = usuario.UsuarioMaster ? new BLL.Perfil((int)BLL.Enumeradores.Perfil.AcessoEmpresaMaster) : new BLL.Perfil((int)BLL.Enumeradores.Perfil.AcessoEmpresa);
                    objUsuarioFilialPerfil.SenhaUsuarioFilialPerfil = objPessoaFisica.DataNascimento.ToString("ddMMyyyy");
                    objUsuarioFilialPerfil.FlagInativo = usuario.UsuarioInativo;

                    objUsuarioFilial.EmailComercial = usuario.EmailComercial;

                    BLL.Funcao objFuncao;
                    if (BLL.Funcao.CarregarPorDescricao(usuario.Funcao, out objFuncao, trans))
                    {
                        objUsuarioFilial.Funcao = objFuncao;
                        objUsuarioFilial.DescricaoFuncao = usuario.Funcao;
                    }
                    else if (usuario.IdFuncaoVelho.HasValue)
                    {
                        objUsuarioFilial.Funcao = new BLL.Funcao(usuario.IdFuncaoVelho.Value);
                        objUsuarioFilial.DescricaoFuncao = usuario.Funcao;
                    }
                    else
                    {
                        objUsuarioFilial.Funcao = null;
                        objUsuarioFilial.DescricaoFuncao = usuario.Funcao;
                    }
                    objUsuarioFilial.NumeroDDDComercial = usuario.NumeroDDDComercial;
                    objUsuarioFilial.NumeroComercial = usuario.NumeroComercial.ToString(CultureInfo.InvariantCulture);
                    objUsuarioFilial.NumeroRamal = usuario.NumeroRamal > Decimal.Zero ? usuario.NumeroRamal.ToString(CultureInfo.InvariantCulture) : null;

                    objPessoaFisica.DataAlteracao = usuario.DataAlteracao;
                    objUsuarioFilialPerfil.DataAlteracao = usuario.DataAlteracao;
                    objUsuario.DataAlteracao = usuario.DataCadastro;

                    //Setado apenas uma vez quando for a inserção de um novo usuário, o que cadastrou, para uma nova empresa
                    if (novoCadastroPJ)
                    {
                        objUsuarioFilialPerfil.FlagUsuarioResponsavel = true;
                        novoCadastroPJ = false;
                    }

                    //TODO cuidar quando tiver edição
                    objPessoaFisica.FlagInativo = false;

                    objPessoaFisica.SalvarMigracao(trans);
                    objUsuario.SalvarMigracao(trans);
                    objUsuarioFilialPerfil.SalvarMigracao(trans);
                    objUsuarioFilial.SalvarMigracao(trans);
                }
            }
        }

        private BLL.Usuario Criar(BLL.PessoaFisica objPessoaFisica, DateTime dataCadastro)
        {
            return new BLL.Usuario
            {
                PessoaFisica = objPessoaFisica,
                SenhaUsuario = "00000000",
                DataCadastro = dataCadastro
            };
        }

        private BLL.UsuarioFilial Criar(BLL.UsuarioFilialPerfil objUsuarioFilialPerfil, DateTime dataCadastro)
        {
            return new BLL.UsuarioFilial
            {
                UsuarioFilialPerfil = objUsuarioFilialPerfil,
                DataCadastro = dataCadastro
            };
        }

        private BLL.UsuarioFilialPerfil Criar(BLL.Filial objFilial, BLL.PessoaFisica objPessoaFisica, DateTime dataCadastro, string IP)
        {
            return new BLL.UsuarioFilialPerfil
            {
                PessoaFisica = objPessoaFisica,
                Filial = objFilial,
                DescricaoIP = IP,
                DataCadastro = dataCadastro
            };
        }

    }
}
