using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.PessoaFisica.Domain
{
    class CadastrarPreCurriculo
    {
        public bool CadastrarMiniCV(Command.PessoaFisica pessoaFisica, out bool candidatura)
        {
            var objPessoaFisica = CarregarPessoaFisicaJaExiste(pessoaFisica.CPF, pessoaFisica.DataNascimento);

            var baseData = DateTime.Now;
            var tipoCurriculo = _tipoCurriculo.GetById(1);
            var situacaoCurriculo = _situacaoCurriculo.GetById(1);

            //TODO: Charan => pegar origem conforme o STC ou parceiro BNE
            var origem = _origem.GetById(1); // Origem 1 é o BNE.

            #region PessoaFisica
            if (objPessoaFisica == null)
            {
                objPessoaFisica = new Model.PessoaFisica
                {
                    DataCadastro = baseData
                };
            }

            var objSexo = _sexo.GetByChar(pessoaFisica.Sexo);


            objPessoaFisica.Nome = pessoaFisica.Nome;
            objPessoaFisica.CPF = Convert.ToDecimal(Utils.LimparMascaraCPFCNPJCEP(pessoaFisica.CPF.ToString()));
            objPessoaFisica.DataNascimento = pessoaFisica.DataNascimento;
            objPessoaFisica.Sexo = objSexo;
            objPessoaFisica.DeficienciaGlobal = pessoaFisica.IdDeficiencia != null ? _deficiencia.GetById(pessoaFisica.IdDeficiencia.Value) : null;
            objPessoaFisica.DataAlteracao = baseData;
            objPessoaFisica.EscolaridadeGlobal = pessoaFisica.IdEscolaridade > 0 ? _escolaridade.GetById(pessoaFisica.IdEscolaridade) : null;

            #region Cidade
            var objCidade = _cidade.GetByNomeUF(pessoaFisica.Cidade);
            objPessoaFisica.Cidade = objCidade;
            #endregion

            var objEmailPessoaFisica = new Model.Email();

            if (pessoaFisica.Email != null)
            {
                objEmailPessoaFisica.Endereco = pessoaFisica.Email;
                objEmailPessoaFisica.PessoaFisica = objPessoaFisica;
                objEmailPessoaFisica.DataCadastro = baseData;

                _pessoaFisicaRepository.Add(objPessoaFisica);
                _email.SalvarEmail(objEmailPessoaFisica);
            }

            #region Telefone
            var dddCelular = byte.Parse(pessoaFisica.Celular.Replace("(", "").Replace(")", "").Replace("-", "").Substring(0, 2));
            var celular = decimal.Parse(pessoaFisica.Celular.Replace("(", "").Replace(")", "").Replace("-", "").Substring(3));

            _telefoneCelular.SalvarTelefone(objPessoaFisica, dddCelular, celular);
            #endregion

            #endregion

            #region Curriculo
            var objCurriculo = new Model.Curriculo
            {
                PessoaFisica = objPessoaFisica,
                FlgVIP = false,
                Ativo = false,
                FlgDisponivelViagem = false,
                TipoCurriculo = tipoCurriculo,
                SituacaoCurriculo = situacaoCurriculo,
                PretensaoSalarial = pessoaFisica.PretensaoSalarial.Value,
                DataCadastro = baseData
            };

            var cvSalvo = _curriculo.SalvarMiniCurriculo(objCurriculo);
            #endregion

            #region Funcao Pretendida
            short tempoExperiencia = 0;

            if (pessoaFisica.TempoExperienciaAnos != null && pessoaFisica.TempoExperienciaMeses != null)
                tempoExperiencia = short.Parse(((pessoaFisica.TempoExperienciaAnos * 12) + pessoaFisica.TempoExperienciaMeses).ToString());

            //TODO Charan => verificar as funções, por ex.: Desenvolvedor PHP na retorna nessa consulta
            var objFuncao = _funcao.GetByNome(pessoaFisica.Funcao);

            var objFuncaoPretendida = new Model.FuncaoPretendida
            {
                DataCadastro = baseData,
                IdFuncao = objFuncao.Id,
                Curriculo = objCurriculo,
                Descricao = pessoaFisica.Funcao,
                TempoExperiencia = tempoExperiencia
            };

            _funcaoPretendida.Salvar(objFuncaoPretendida);
            #endregion

            #region CurriculoOrigem
            var objCurriculoOrigem = new Model.CurriculoOrigem
            {
                DataCadastro = baseData,
                Curriculo = objCurriculo,
                OrigemGlobal = origem
            };

            _curriculoOrigem.Salvar(objCurriculoOrigem);
            #endregion

            #region CurriculoParametro
            var objCurriculoParametro = new Model.CurriculoParametro
            {
                Curriculo = objCurriculo,
                DataCadastro = baseData,
                Ativo = true,
                Valor = Model.Enumeradores.Parametro.QuantidadeCandidaturaDegustacao.ToString(),
                Parametro = _parametro.GetById(Model.Enumeradores.Parametro.QuantidadeCandidaturaDegustacao)
            };

            _curriculoParametro.Salvar(objCurriculoParametro);
            #endregion

            //Chamar o mapeamento
            var commandObject = MapearNovoToAntigo(objCurriculo, baseData, objCurriculoOrigem, objFuncaoPretendida, objPessoaFisica, objCurriculoParametro, objEmailPessoaFisica.Endereco, 0, dddCelular, celular);
            new Mapper.ToOld.PessoaFisica().Map(commandObject, out candidatura, true);

            _unitOfWork.Commit();

            #region Enviar Carta de Confirmação de e-mail.
            if (pessoaFisica.Email != null)
            {
                EnviarCartaConfirmacaoDeEmail(objPessoaFisica.Nome, objPessoaFisica.CPF, objPessoaFisica.DataNascimento, objCurriculo.Id, objPessoaFisica.Id, objEmailPessoaFisica.Endereco);
            }
            #endregion

            return true;
        }
    }
}
