using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BNE.BLL;
using System.IO;
namespace Bne.Web.Services.API.DTO
{
    public class ResultadoPesquisaCurriculoCompleto : Curriculo
    {
        //Resumo Mini Curriculo
        public string foto;
        public bool vip;
        public decimal cpf;
        public string dtaAtualizacao;
        public string dtaNascimento;
        public string dddCelular;
        public string numCelular;
        public string dddTelefone;
        public string numTelefone;
        public string email;
        public string nomeCompleto;
        public char sexo;
        public string estadoCivil;
        public int idade;
        public string funcoes;
        public decimal? pretensao;
        public string escolaridade;
        public string observacoes;
        public bool aceitaEstagio;


        //Endereço
        public string cepEndereco;
        public string logradouroEndereco;
        public string numeroEndereco;
        public string complementoEndereco;
        public string bairroEndereco;
        public string cidade;
        public string estado;

        //Dados Pessoais e Profissionais
        public string carteira;
        public int idCurriculo;
        public string numeroRG;
        public string orgaoExpeditor;
        public string deficiente;

        public string caracteristicasPessoais;
        public string outrosConhecimento;
        public bool? FlagViagem;

        public List<BNE.BLL.DTO.ExperienciaProfissional> listExperienciaProfissional;

        public List<BNE.BLL.DTO.Idioma> idiomas;
        
        public List<BNE.BLL.DTO.Formacao> formacoes;

        public ResultadoPesquisaCurriculoCompleto()
        {
            
        }

    }
}
