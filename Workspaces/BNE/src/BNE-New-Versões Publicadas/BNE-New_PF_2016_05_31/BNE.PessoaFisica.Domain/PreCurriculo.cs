using BNE.PessoaFisica.Data.Repositories;
using System;
using System.Collections.Generic;
using BNE.Data.Infrastructure;
using BNE.ExceptionLog.Interface;

namespace BNE.PessoaFisica.Domain
{
    public class PreCurriculo : IPreCurriculo
    {
        private readonly BNE.PessoaFisica.Data.Repositories.IPreCurriculoRepository _preCurriculoRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public readonly PessoaFisica _pessoaFisica;
        public readonly Parametro _parametro;
        private readonly Global.Domain.Sexo _sexo;

        public PreCurriculo(IPreCurriculoRepository preCurriculoRepository, IUnitOfWork unitOfWork, PessoaFisica pessoaFisica, Parametro parametro,Global.Domain.Sexo sexo, ILogger logger)
        {
            _preCurriculoRepository = preCurriculoRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _sexo = sexo;
            _pessoaFisica = pessoaFisica;
            _parametro = parametro;
        }


        /// <summary>
        /// Retornar por Id
        /// </summary>
        /// <param name="idPreCurriculo"></param>
        /// <returns></returns>
        public Model.PreCurriculo Get(long idPreCurriculo)
        {
           return _preCurriculoRepository.GetById(idPreCurriculo);
        }

        /// <summary>
        /// Retornar por Nome
        /// </summary>
        /// <param name="nome"></param>
        /// <returns></returns>
        public Model.PreCurriculo Get(string nome)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retornar por nome e email
        /// </summary>
        /// <param name="nome"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public Model.PreCurriculo Get(string nome, string email)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retornar por nome e data nascimento
        /// </summary>
        /// <param name="nome"></param>
        /// <param name="dataNascimento"></param>
        /// <returns></returns>
        public Model.PreCurriculo Get(string nome, DateTime dataNascimento)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retornar todos
        /// </summary>
        /// <returns></returns>
        public ICollection<Model.PreCurriculo> GetAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Carrgar a Vaga pelo Id
        /// </summary>
        /// <param name="idVaga"></param>
        /// <returns></returns>
        public Command.Vaga CarregarVaga(string idVaga)
        {
            Command.Vaga vaga = null;
            string UrlSolr = _parametro.RecuperarValor(Model.Enumeradores.Parametro.UrlVagasSolr);
            vaga = Custom.SOLR.CarregarVagaSolr.CarregarVaga(idVaga, UrlSolr);
            return vaga;
        }

        public List<Model.Pergunta> CarregarPergunta(string idVaga)
        {
            //List<Command.Pergunta>
            List<Mapper.Models.Vaga.Pergunta> lista = new Mapper.ToOld.PessoaFisica().GetPergunta(Convert.ToInt32(idVaga));
            List<Model.Pergunta> listaPergunta = new List<Model.Pergunta>();
            foreach (var pergunta in lista)
            {
                listaPergunta.Add(new Model.Pergunta
                {                
                    descricaoVagaPergunta = pergunta.descricaoVagaPergunta,
                    idVagaPergunta = pergunta.idVagaPergunta,
                    tipoResposta = pergunta.tipoResposta
                    });
            }
            return listaPergunta;

        }
       

        /// <summary>
        /// Criar um pré-Currículo
        /// </summary>
        /// <param name="preCurriculo"></param>
        /// <returns></returns>
        public Model.PreCurriculo Post(Command.PreCurriculo preCurriculo)
        {
            try
            {
                string dddCelular = preCurriculo.Celular.Replace("(","").Replace(")","").Replace("-","").Substring(0,2);
                string celular = preCurriculo.Celular.Replace("(","").Replace(")","").Replace("-","").Substring(3);
                Int16 tempoExperiencia = 0;

                if (preCurriculo.TempoExperienciaAnos != null && preCurriculo.TempoExperienciaMeses != null)
                    tempoExperiencia = Int16.Parse(((preCurriculo.TempoExperienciaAnos * 12) + preCurriculo.TempoExperienciaMeses).ToString());

                var objSexo = _sexo.GetByChar(preCurriculo.Sexo);
                var objPreCurriculo = new Model.PreCurriculo
                {
                    Nome = preCurriculo.Nome,
                    Email = preCurriculo.Email,
                    DDDCelular = dddCelular,
                    Celular = celular,
                    TempoExperiencia = tempoExperiencia,
                    PretensaoSalarial = preCurriculo.PretensaoSalarial,
                    IdCidade = preCurriculo.IdCidade,
                    IdFuncao = preCurriculo.IdFuncao,
                    IdVaga = preCurriculo.IdVaga,
                    DataCadastro = DateTime.Now,
                    DescricaoFuncao = preCurriculo.DescricaoFuncao,
                    Sexo = objSexo
                };

                _preCurriculoRepository.Add(objPreCurriculo);

                _unitOfWork.Commit();

                return objPreCurriculo;
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// Gerar o Mini Currículo a partir do Pré-Cadastro
        /// </summary>
        /// <param name="preCurriculo"></param>
        /// <returns></returns>
        public bool GerarMiniCurriculo(Command.PreCurriculo preCurriculo, out bool candidatura)
        {
            try
            {
                var resultado =  _pessoaFisica.PostMiniCurriculo(preCurriculo, out candidatura);
                return resultado;
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

       
        /// <summary>
        /// Gerar a candidatura para um candidato ja cadastrado
        /// </summary>
        /// <param name="preCurriculo"></param>
        /// <returns></returns>
        public Model.PessoaFisica CandidatoExiste(decimal cpf, DateTime dataNascimento)
        {
            try
            {
                var resultado = _pessoaFisica.RetornarFisicaJaExiste(cpf, dataNascimento);
                return resultado;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// Gerar a candidatura para um candidato ja cadastrado
        /// </summary>
        /// <param name="preCurriculo"></param>
        /// <returns></returns>
        public Tuple<string,bool> CandidatarCurriculo(Command.PreCurriculo preCurriculo)
        {
            try
            {
                var resultado = _pessoaFisica.Candidatar(preCurriculo);
                return resultado;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        #region SetarCurriculonoPreCurriculo
        /// <summary>
        /// Setar o Id do Currículo no Pré Curriculo após cadastrar o mini CV.
        /// </summary>
        /// <param name="preCurriculo"></param>
        public void SetarCurriculonoPreCurriculo(Model.PreCurriculo preCurriculo)
        {
            _preCurriculoRepository.Update(preCurriculo);
            _unitOfWork.Commit();
        }
        #endregion

        #region GerarHashAcessoLoginAutomatico
        public string GerarHashAcessoLoginAutomatico(decimal cpf, DateTime dataNascimento, string urlDestino)
        {
            return _pessoaFisica.GerarHashAcessoLoginAutomatico(cpf, dataNascimento, urlDestino);
        }
        #endregion

    }

    public interface IPreCurriculo
    {
        ICollection<Model.PreCurriculo> GetAll();
        Model.PreCurriculo Get(Int64 idPreCurriculo);
        Model.PreCurriculo Get(string nome, string email);
        Model.PreCurriculo Get(string nome, DateTime dataNascimento);
        Model.PreCurriculo Post(Command.PreCurriculo preCurriculo);
    }
}