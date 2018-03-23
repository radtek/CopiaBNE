using System;
using System.Collections.Generic;
using System.Data.Entity;
using AutoMapper;
using BNE.PessoaFisica.Domain.Aggregates;
using BNE.PessoaFisica.Domain.Command;
using BNE.PessoaFisica.Domain.Model;
using BNE.PessoaFisica.Domain.Repositories;
using CrossCutting.Infrastructure.Repository;
using ExperienciaProfissional = BNE.Mapper.Models.PessoaFisica.ExperienciaProfissional;
using Formacao = BNE.Mapper.Models.PessoaFisica.Formacao;
using FuncaoPretendida = BNE.Mapper.Models.PessoaFisica.FuncaoPretendida;

namespace BNE.PessoaFisica.Data.Repositories
{
    public class CurriculoRepository : BaseRepository<Curriculo, DbContext>, ICurriculoRepository
    {
        private readonly IMapper _mapper;

        public CurriculoRepository(IMapper mapper, DbContext dataContext) : base(dataContext)
        {
            _mapper = mapper;
        }

        public InformacaoCurriculo GetInformacaoCurriculo(decimal cpf, int idVaga)
        {
            InformacaoCurriculo objInformacaoCurriculo = null;
            var dt = new Mapper.ToOld.PessoaFisica().CarregarInformacoesCurriculo(cpf, idVaga);

            if (dt != null && dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];
                objInformacaoCurriculo = new InformacaoCurriculo
                {
                    IdCurriculo = Convert.ToInt32(row[0]),
                    CurriculoVIP = Convert.ToBoolean(row[1]),
                    JaEnvioCvParaVaga = row[2].ToString() != "",
                    IndicadoBNE = Convert.ToBoolean(row[10]),
                    EmpresaBloqueada = row[3].ToString() != "",
                    EstaNaRegiaoBH = row[4].ToString() != "",
                    TemExperienciaProfissional = row[5].ToString() != "",
                    TemFormacao = row[6].ToString() != "",
                    SaldoCandidatura = row[7].ToString() != "" ? Convert.ToInt32(row[7]) : 0,
                    DisseQueNaoTemExperiencia = row[8].ToString() != "",
                    DataNaoTemExperiencia = row[9] != DBNull.Value ? Convert.ToDateTime(row[9]) : (DateTime?) null
                };
                Mapper.ToOld.PessoaFisica.SalvarVisualizacaoVaga(idVaga, objInformacaoCurriculo.IdCurriculo);
            }

            return objInformacaoCurriculo;
        }

        /// <summary>
        ///     Recuperar o identificador do currículo do bne velho
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public int GetIdCurriculo(decimal cpf)
        {
            return new Mapper.ToOld.PessoaFisica().RecuperarIdCurriculoPorCPF(cpf);
        }

        public bool SalvarCandidatura(decimal cpf, int idVaga, bool indicacaoAmigo, List<Tuple<int, bool?, string>> list)
        {
            return new Mapper.ToOld.PessoaFisica().SalvarCandidatura(cpf, idVaga, false, list);
        }

        public void MapFormacao(SalvarFormacaoCommand command)
        {
            var commandObject = MapearFormacao(command);
            new Mapper.ToOld.PessoaFisica().MapFormacao(commandObject, command.IdVaga, command.Candidatar);
        }

        public void MapExperienciaProfissional(SalvarExperienciaProfissionalCommand command, bool salvarExperiencia)
        {
            var commandObject = MapearExperienciaProfissional(command);
            new Mapper.ToOld.PessoaFisica().MapExperienciaProfissional(commandObject, command.IdVaga, salvarExperiencia, command.Candidatar);
        }

        public void MapCurriculoParametro(SalvarExperienciaProfissionalCommand command)
        {
            var commandObject = MapearExperienciaProfissional(command);
            new Mapper.ToOld.PessoaFisica().MapCurriculoParametro(commandObject, command.IdVaga, command.Candidatar);
        }

        public void Map(SalvarCurriculoCommand command, out bool candidatura)
        {
            var commandObject = _mapper.Map<SalvarCurriculoCommand, Mapper.Models.PessoaFisica.PessoaFisica>(command);

            commandObject.Curriculo = _mapper.Map<SalvarCurriculoCommand, Mapper.Models.PessoaFisica.Curriculo>(command);
            commandObject.Curriculo.FuncaoPretendida = new FuncaoPretendida
            {
                TempoExperiencia = command.TempoExperiencia,
                Descricao = command.DescricaoFuncao,
                idFuncao = command.IdFuncao
            };

            commandObject.IdSexo = (byte) (command.Sexo != null ? (command.Sexo == "M" ? 1 : 2) : 0);

            new Mapper.ToOld.PessoaFisica().Map(commandObject, out candidatura, true);
        }

        /// <summary>
        ///     Identificaro do Currículo no Banco do BNE Velho
        /// </summary>
        /// <param name="idCurriculo"></param>
        /// <returns></returns>
        public bool VIP(int idCurriculo)
        {
            return new Mapper.ToOld.PessoaFisica().CurriculoVIP(idCurriculo);
        }

        /// <summary>
        ///     Identificaro do Currículo no Banco do BNE Velho
        /// </summary>
        /// <param name="idCurriculo"></param>
        /// <returns></returns>
        public int SaldoCandidatura(int idCurriculo)
        {
            return new Mapper.ToOld.PessoaFisica().RecuperarSaldoCandidatura(idCurriculo);
        }

        #region MapearFormacao

        private Formacao MapearFormacao(SalvarFormacaoCommand command)
        {
            /*var command = new Domain.Command.Formacao
            {
                DataCadastro = objFormacao.DataCadastro,
                DataAlteracao = objFormacao.DataAlteracao,
                NomeCurso = objFormacao.NomeCurso,
                NomeInstituicao = objFormacao.NomeInstituicao,
                AnoConclusao = objFormacao.AnoConclusao,
                Ativo = objFormacao.Ativo,
                IdCidade = objFormacao.Cidade == null ? 0 : objFormacao.Cidade.Id,
                IdInstituicaoEnsino = objFormacao.InstituicaoEnsino == null ? 0 : objFormacao.InstituicaoEnsino.Id,
                IdCurso = objFormacao.Curso == null ? 0 : objFormacao.Curso.Id,
                IdEscolaridade = objFormacao.EscolaridadeGlobal.Id,
                IdPessoa = objFormacao.PessoaFisica.Id,
                Cpf = objFormacao.PessoaFisica.CPF
            };*/

            var commandObject = _mapper.Map<SalvarFormacaoCommand, Formacao>(command);

            return commandObject;
        }

        #endregion

        #region MapearExperienciaProfissional

        private ExperienciaProfissional MapearExperienciaProfissional(SalvarExperienciaProfissionalCommand command)
        {
            //var commandObject1 = _mapper.Map<Models.ExperienciaProfissional, Domain.Command.ExperienciaProfissional>(objExperiencia);
            //var commandObjectretorno = _mapper.Map<Command.ExperienciaProfissional, Mapper.Models.PessoaFisica.ExperienciaProfissional>(commandObject1);

            /*var command = new Command.ExperienciaProfissional
            {
                DataEntrada = objExperiencia.DataEntrada != null ? objExperiencia.DataEntrada.Value : DateTime.Now,
                DataSaida = objExperiencia.DataSaida,
                DataCadastro = objExperiencia.DataCadastro,
                AtividadesExercidas = objExperiencia.AtividadesExercidas,
                FlgImportado = false,
                UltimoSalario = objExperiencia.UltimoSalario,
                NomeEmpresa = objExperiencia.NomeEmpresa,
                FuncaoExercida = objExperiencia.FuncaoExercida,
                Ativo = objExperiencia.Ativo,
                Cpf = objExperiencia.PessoaFisica.CPF,
                //IdPessoa = objExperiencia.PessoaFisica.Id,
                IdRamoEmpresa = objExperiencia.RamoAtividadeGlobal != null ? objExperiencia.RamoAtividadeGlobal.Id : 0
            };*/

            var commandObject = _mapper.Map<SalvarExperienciaProfissionalCommand, ExperienciaProfissional>(command);

            return commandObject;
        }

        #endregion

        #region MapearNovoToAntigo

        //private AutoMapper.Mapper.Models.PessoaFisica.PessoaFisica MapearNovoToAntigo(Model.Curriculo objCurriculo, DateTime baseData,
        //    Model.CurriculoOrigem objCurriculoOrigem,
        //    Model.FuncaoPretendida objFuncaoPretendida,
        //    Model.PessoaFisica objPessoaFisica,
        //    Model.CurriculoParametro objCurriculoParametro,
        //    string email, int idVaga, byte dddCelular, decimal celular)
        //{
        //    try
        //    {
        //        var commandCV = new Command.Curriculo
        //        {
        //            FlgVIP = objCurriculo.FlgVIP,
        //            FlgInativo = objCurriculo.Ativo,
        //            FlgDisponivelViagem = objCurriculo.FlgDisponivelViagem,
        //            PretensaoSalarial = objCurriculo.PretensaoSalarial,
        //            Observacao = objCurriculo.Observacao,
        //            Conhecimento = objCurriculo.Conhecimento,
        //            IdTipoCurriculo = objCurriculo.TipoCurriculo.Id,
        //            IdSituacaoCurriculo = objCurriculo.SituacaoCurriculo.Id,
        //            DataCadastro = baseData,
        //            DataAtualizacao = objCurriculo.DataAtualizacao,
        //            DataModificacao = objCurriculo.DataModificacao,
        //            IdOrigem = objCurriculoOrigem.OrigemGlobal.Id
        //        };

        //        var cmdFuncaoPretendida = new Command.FuncaoPretendida
        //        {
        //            TempoExperiencia = objFuncaoPretendida.TempoExperiencia,
        //            Descricao = objFuncaoPretendida.Descricao,
        //            idFuncao = objFuncaoPretendida.IdFuncao
        //        };

        //        var command = new CriarAtualizarPessoaFisica
        //        {
        //            CPF = objPessoaFisica.CPF,
        //            Nome = objPessoaFisica.Nome,
        //            Email = email,
        //            DataNascimento = objPessoaFisica.DataNascimento,
        //            DataCadastro = baseData,
        //            DDDCelular = dddCelular,
        //            Celular = celular,
        //            IdVaga = idVaga,
        //            IdSexo = objPessoaFisica.Sexo != null ? objPessoaFisica.Sexo.Sigla == "M" ? 1 : 2 : 0,
        //            IdCidade = objPessoaFisica.Cidade.Id,
        //            IdEscolaridade = objPessoaFisica.EscolaridadeGlobal != null ? objPessoaFisica.EscolaridadeGlobal.Id : 0,
        //            IdDeficiencia = objPessoaFisica.DeficienciaGlobal != null ? objPessoaFisica.DeficienciaGlobal.Id : 0
        //        };

        //        var cmdCurriculoParamtro = new Command.CurriculoParametro
        //        {
        //            idParametro = objCurriculoParametro.Parametro.Id,
        //            Valor = objCurriculoParametro.Valor
        //        };

        //        //inserir objeto Curriculo na PessoaFisica
        //        command.Curriculo = commandCV;
        //        command.Curriculo.FuncaoPretendida = cmdFuncaoPretendida;
        //        command.Curriculo.CurriculoParametro = cmdCurriculoParamtro;


        //        //Consumir o Map
        //        var commandObject = AutoMapper.Mapper.Map<SalvarCurriculoCommand, AutoMapper.Mapper.Models.PessoaFisica.PessoaFisica>(command);

        //        return commandObject;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(ex, "Erro no mapeamento NEWBNE to OLD");
        //        throw;
        //    }
        //}

        #endregion
    }
}