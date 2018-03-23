using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BNE.BLL;

namespace Bne.Web.Services.API.Controllers
{
    public class BNEApiController : ApiController
    {
        internal HttpResponseMessage errorRequestPost(HttpStatusCode badRequest, string msgError)
        {
            return Request.CreateErrorResponse(badRequest, msgError);
        }

        internal UsuarioFilialPerfil Login(decimal CPF, DateTime dataNascimento, decimal? cnpj = null)
        {
            PessoaFisica objPessoaFisica;
            if (PessoaFisica.CarregarPorCPF(CPF, out objPessoaFisica))
            {
                //Se a data informada for diferente do cadastro mostra mensagem para ir para o SOSRH.
                if (dataNascimento.ToString("yyyy-MM-dd") == objPessoaFisica.DataNascimento.ToString("yyyy-MM-dd"))
                {
                    //Busca qtos perfis esse usuário tem desbloquados													  
                    UsuarioFilialPerfil objUsuarioFilialPerfil = null;
                    
                    //Carregando usuario para uma filial específica
                    if (cnpj.HasValue)
                    {
                        if (UsuarioFilialPerfil.CarregarUsuarioEmpresaPorPessoaFisica(objPessoaFisica.IdPessoaFisica, cnpj.Value, out objUsuarioFilialPerfil))
                        {
                            int quantidadeEmpresa = UsuarioFilialPerfil.QuantidadeUsuarioEmpresa(objPessoaFisica);
                            
                            if (quantidadeEmpresa != 0) //Se o usuário tiver empresa relacionada.
                            {
                                return objUsuarioFilialPerfil;
                            }
                        }
                    }
                    else if (UsuarioFilialPerfil.CarregarUsuarioEmpresaPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objUsuarioFilialPerfil))
                    {
                        //CNPJ não informado. Carregando usuário para a primeira empresa retornada do banco
                        int quantidadeEmpresa = UsuarioFilialPerfil.QuantidadeUsuarioEmpresa(objPessoaFisica);

                        if (quantidadeEmpresa != 0) //Se o usuário tiver empresa relacionada.
                        {
                            return objUsuarioFilialPerfil;
                        }
                    }
                }
            }

            return null;
        }

        internal UsuarioFilialPerfil Login(HttpRequestMessage request)
        {
            DateTime dataNascimento;
            decimal cpf;
            decimal cnpj;

#if DEBUG
            cpf = 00980926939;
            dataNascimento = new DateTime(1985, 04, 26);
#else
            //Se os dados de login nao estao definidos no cabecalho, retorna null indicando falha na autenticacao
            if (!Request.Headers.Contains("Num_CPF"))
                return null;

            dataNascimento = DateTime.ParseExact(request.Headers.GetValues("Dta_Nascimento").First(), "yyyy-MM-dd", null);
            cpf = Convert.ToDecimal(request.Headers.GetValues("Num_CPF").First());
#endif
            //Se o CNPJ foi informado, converte para decimal e recupera usuario da filial
            if (request.Headers.Contains("Num_CNPJ") && Decimal.TryParse(request.Headers.GetValues("Num_CNPJ").First(), out cnpj))
                    return Login(cpf, dataNascimento, cnpj);

            return Login(cpf, dataNascimento);
        }
    }
}
