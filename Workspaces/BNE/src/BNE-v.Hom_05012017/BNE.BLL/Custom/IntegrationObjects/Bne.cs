using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Net;

namespace BNE.BLL.Custom.IntegrationObjects
{
    [XmlRoot("bne")]
    [XmlInclude(typeof(VagaBne))]
    public class Bne
    {
        public Bne()
        {
            Geracao = DateTime.Now;
        }

        public Bne(String codigoVagaIntegrador, int idCurriculo)
        {
            Candidatura = new Candidatura();
            Candidatura._codigovaga = codigoVagaIntegrador.ToString();
            Candidatura.Curriculo = new Curriculo();
            Candidatura.Curriculo._codigo = idCurriculo.ToString();
        }

        private List<VagaBne> _vagas;

        [XmlAttribute("geracao", DataType = "dateTime")]
        public DateTime Geracao { get; set; }

        [XmlArray("vagas")]
        [XmlArrayItem("vaga")]
        public List<VagaBne> Vagas
        {
            get
            {
                if (_vagas == null)
                {
                    _vagas = new List<VagaBne>();
                }
                return _vagas;
            }
            set { _vagas = value; }
        }

        [XmlElement("candidatura")]
        public Candidatura Candidatura;

        [XmlElement("retorno")]
        public Retorno Retorno;

        public Bne Enviar(String url)
        {
            try
            {
                String serializedXML;
                this.Serialize(out serializedXML);
                byte[] buffer = Encoding.UTF8.GetBytes(serializedXML);

                var webRequest = WebRequest.Create(url);
                webRequest.Method = "POST";
                webRequest.ContentLength = buffer.Length;
                webRequest.ContentType = "application/xml";

                var postData = webRequest.GetRequestStream();
                postData.Write(buffer, 0, buffer.Length);
                postData.Close();

                var webResponse = webRequest.GetResponse();

                Bne retorno;
                using (var sr = new StreamReader(webResponse.GetResponseStream()))
                {
                    retorno = Bne.Deserialize(sr);
                }

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Serialize(Stream s)
        {
            Type[] vagasTypes = { typeof(Bne), typeof(VagaBne) };
            XmlSerializer serializer = new XmlSerializer(typeof(Bne), vagasTypes);
            serializer.Serialize(s, this);
        }

        public void Serialize(out String s)
        {
            MemoryStream ms = new MemoryStream();

            this.Serialize(ms);

            ms.Position = 0;

            StreamReader sr = new StreamReader(ms);
            s = sr.ReadToEnd();
            s = Helper.LimparCaracteresInvalidosEmXML(s);
        }

        public static Bne Deserialize(StreamReader s)
        {
            Type[] vagasTypes = { typeof(Bne), typeof(VagaBne) };
            XmlSerializer serializer = new XmlSerializer(typeof(Bne), vagasTypes);
            return (Bne)serializer.Deserialize(s);
        }
    }

    [XmlType("retorno")]
    public class Retorno
    {
        [XmlElement("sucesso", DataType = "boolean")]
        public Boolean Sucesso { get; set; }

        [XmlIgnore]
        public string _mensagemErro { get; set; }
        [XmlElement("mensagemErro")]
        public System.Xml.XmlCDataSection MensagemErro
        {
            get
            {
                return new System.Xml.XmlDocument().CreateCDataSection(_mensagemErro);
            }
            set
            {
                _mensagemErro = value.Value;
            }
        }
    }

    [XmlType("vaga")]
    public class VagaBne
    {
        [XmlIgnore]
        public string _codigo { get; set; }
        [XmlElement("codigo")]
        public System.Xml.XmlCDataSection Codigo
        {
            get
            {
                return new System.Xml.XmlDocument().CreateCDataSection(_codigo);
            }
            set
            {
                _codigo = value.Value;
            }
        }

        [XmlIgnore]
        public string _funcao { get; set; }
        [XmlElement("funcao")]
        public System.Xml.XmlCDataSection Funcao
        {
            get
            {
                return new System.Xml.XmlDocument().CreateCDataSection(_funcao);
            }
            set
            {
                _funcao = value.Value;
            }
        }

        [XmlElement("cidade")]
        public Cidade Cidade { get; set; }

        [XmlIgnore]
        public string _empresa { get; set; }
        [XmlElement("empresa")]
        public System.Xml.XmlCDataSection Empresa
        {
            get
            {
                return new System.Xml.XmlDocument().CreateCDataSection(_empresa);
            }
            set
            {
                _empresa = value.Value;
            }
        }

        [XmlElement("confidencial", DataType = "boolean")]
        public Boolean Confidencial { get; set; }

        [XmlElement("numeroVagas", DataType = "int")]
        public int NumeroVagas { get; set; }

        [XmlElement("salarioDe", DataType = "decimal")]
        public decimal SalarioDe { get; set; }

        [XmlElement("salarioAte", DataType = "decimal")]
        public decimal SalarioAte { get; set; }

        [XmlElement("idadeDe", DataType = "int")]
        public int IdadeDe { get; set; }

        [XmlElement("idadeAte", DataType = "int")]
        public int IdadeAte { get; set; }

        [XmlIgnore]
        public string _sexo { get; set; }
        [XmlElement("sexo")]
        public System.Xml.XmlCDataSection Sexo
        {
            get
            {
                return new System.Xml.XmlDocument().CreateCDataSection(_sexo);
            }
            set
            {
                _sexo = value.Value;
            }
        }

        [XmlIgnore]
        public string _beneficios { get; set; }
        [XmlElement("beneficios")]
        public System.Xml.XmlCDataSection Beneficios
        {
            get
            {
                return new System.Xml.XmlDocument().CreateCDataSection(_beneficios);
            }
            set
            {
                _beneficios = value.Value;
            }
        }

        [XmlIgnore]
        public string _escolaridade { get; set; }
        [XmlElement("escolaridade")]
        public System.Xml.XmlCDataSection Escolaridade
        {
            get
            {
                return new System.Xml.XmlDocument().CreateCDataSection(_escolaridade);
            }
            set
            {
                _escolaridade = value.Value;
            }
        }

        [XmlIgnore]
        public string _disponibilidade { get; set; }
        [XmlElement("disponibilidade")]
        public System.Xml.XmlCDataSection Disponibilidade
        {
            get
            {
                return new System.Xml.XmlDocument().CreateCDataSection(_disponibilidade);
            }
            set
            {
                _disponibilidade = value.Value;
            }
        }

        [XmlIgnore]
        public string _contrato { get; set; }
        [XmlElement("contrato")]
        public System.Xml.XmlCDataSection Contrato
        {
            get
            {
                return new System.Xml.XmlDocument().CreateCDataSection(_contrato);
            }
            set
            {
                _contrato = value.Value;
            }
        }

        [XmlIgnore]
        public string _deficiencia { get; set; }
        [XmlElement("deficiencia")]
        public System.Xml.XmlCDataSection Deficiencia
        {
            get
            {
                return new System.Xml.XmlDocument().CreateCDataSection(_deficiencia);
            }
            set
            {
                _deficiencia = value.Value;
            }
        }

        [XmlIgnore]
        public string _atribuicoes { get; set; }
        [XmlElement("atribuicoes")]
        public System.Xml.XmlCDataSection Atribuicoes
        {
            get
            {
                return new System.Xml.XmlDocument().CreateCDataSection(_atribuicoes);
            }
            set
            {
                _atribuicoes = value.Value;
            }
        }

        [XmlIgnore]
        public string _requisitos { get; set; }
        [XmlElement("requisitos")]
        public System.Xml.XmlCDataSection Requisitos
        {
            get
            {
                return new System.Xml.XmlDocument().CreateCDataSection(_requisitos);
            }
            set
            {
                _requisitos = value.Value;
            }
        }

        [XmlElement("dataCadastro", DataType = "dateTime")]
        public DateTime DataCadastro { get; set; }

        [XmlElement("dataAlteracao", DataType = "dateTime")]
        public DateTime DataAlteracao { get; set; }

        [XmlElement("inativa", DataType = "boolean")]
        public Boolean Inativa { get; set; }

        [XmlIgnore]
        public string _emailRetorno { get; set; }
        [XmlElement("emailRetorno")]
        public System.Xml.XmlCDataSection EmailRetorno
        {
            get
            {
                return new System.Xml.XmlDocument().CreateCDataSection(_emailRetorno);
            }
            set
            {
                _emailRetorno = value.Value;
            }
        }

        [XmlElement("telefoneRetorno")]
        public Telefone TelefoneRetorno { get; set; }
    }

    [XmlType("candidatura")]
    public class Candidatura
    {
        [XmlIgnore]
        public string _codigovaga { get; set; }
        [XmlElement("codigovaga")]
        public System.Xml.XmlCDataSection CodigoVaga
        {
            get
            {
                return new System.Xml.XmlDocument().CreateCDataSection(_codigovaga);
            }
            set
            {
                _codigovaga = value.Value;
            }
        }

        [XmlElement("curriculo")]
        public Curriculo Curriculo { get; set; }
    }

    [XmlType("curriculo")]
    public class Curriculo
    {
        [XmlIgnore]
        public string _codigo { get; set; }
        [XmlElement("codigo")]
        public System.Xml.XmlCDataSection Codigo
        {
            get
            {
                return new System.Xml.XmlDocument().CreateCDataSection(_codigo);
            }
            set
            {
                _codigo = value.Value;
            }
        }

        [XmlElement("dataAtualizacao", DataType = "dateTime")]
        public DateTime DataAtualizacao { get; set; }

        [XmlIgnore]
        public string _nome { get; set; }
        [XmlElement("nome")]
        public System.Xml.XmlCDataSection Nome
        {
            get
            {
                return new System.Xml.XmlDocument().CreateCDataSection(_nome);
            }
            set
            {
                _nome = value.Value;
            }
        }

        [XmlElement("dataNascimento", DataType = "dateTime")]
        public DateTime DataNascimento { get; set; }

        [XmlIgnore]
        public string _cpf { get; set; }
        [XmlElement("cpf")]
        public System.Xml.XmlCDataSection Cpf
        {
            get
            {
                return new System.Xml.XmlDocument().CreateCDataSection(_cpf);
            }
            set
            {
                _cpf = value.Value;
            }
        }

        [XmlIgnore]
        public string _sexo { get; set; }
        [XmlElement("sexo")]
        public System.Xml.XmlCDataSection Sexo
        {
            get
            {
                return new System.Xml.XmlDocument().CreateCDataSection(_sexo);
            }
            set
            {
                _sexo = value.Value;
            }
        }

        [XmlElement("cidade")]
        public Cidade Cidade { get; set; }

        [XmlElement("celular")]
        public Telefone Celular { get; set; }

        [XmlIgnore]
        public string _email { get; set; }
        [XmlElement("email")]
        public System.Xml.XmlCDataSection Email
        {
            get
            {
                return new System.Xml.XmlDocument().CreateCDataSection(_email);
            }
            set
            {
                _email = value.Value;
            }
        }

        [XmlElement("pretensao", DataType = "decimal")]
        public Decimal Pretensao { get; set; }

        [XmlIgnore]
        public string _funcaoPretendida { get; set; }
        [XmlElement("funcaoPretendida")]
        public System.Xml.XmlCDataSection FuncaoPretendida
        {
            get
            {
                return new System.Xml.XmlDocument().CreateCDataSection(_funcaoPretendida);
            }
            set
            {
                _funcaoPretendida = value.Value;
            }
        }

        [XmlArray("experiencias")]
        [XmlArrayItem("experiencia")]
        public List<Experiencia> Experiencias;

        [XmlArray("formacao")]
        [XmlArrayItem("curso")]
        public List<Curso> Cursos;

        public void CompletarCurriculo(BLL.Curriculo objCurriculo)
        {
            PessoaFisica objPessoaFisica = PessoaFisica.LoadObject(objCurriculo.PessoaFisica.IdPessoaFisica);

            this.Celular = new Telefone();
            this.Celular.DDD = Convert.ToInt32(objPessoaFisica.NumeroDDDCelular);
            this.Celular._numero = objPessoaFisica.NumeroCelular;

            this.Cidade = new Cidade();
            objPessoaFisica.Cidade.CompleteObject();
            this.Cidade._nome = objPessoaFisica.Cidade.NomeCidade;
            objPessoaFisica.Cidade.Estado.CompleteObject();
            this.Cidade._estado = objPessoaFisica.Cidade.Estado.SiglaEstado;

            this._cpf = objPessoaFisica.CPF.ToString();
            this.DataAtualizacao = objCurriculo.DataAtualizacao;
            this.DataNascimento = objPessoaFisica.DataNascimento;
            this._email = objPessoaFisica.EmailPessoa;
            this._nome = objPessoaFisica.NomeCompleto;
            if (objCurriculo.ValorPretensaoSalarial.HasValue)
            {
                this.Pretensao = objCurriculo.ValorPretensaoSalarial.Value;
            }
            this._sexo = objPessoaFisica.Sexo.IdSexo == 1 ? "M" : "F";

            List<FuncaoPretendida> funcoesPretendidas = BLL.FuncaoPretendida.CarregarFuncoesPretendidasPorCurriculo(objCurriculo);
            if (funcoesPretendidas.Count > 0)
            {
                FuncaoPretendida objFuncaoPretendida = funcoesPretendidas.OrderBy(f => f.DataCadastro).First();
                objFuncaoPretendida.Funcao.CompleteObject();
                this._funcaoPretendida = objFuncaoPretendida.Funcao.DescricaoFuncao;
            }
            
            //Listando cursos
            this.Cursos = new List<Curso>();
            foreach (Formacao formacao in Formacao.ListarFormacaoList(objPessoaFisica.IdPessoaFisica, false))
            {
                Curso c = new Curso();
                c.AnoConclusao = Convert.ToInt32(formacao.AnoConclusao);
                c.Cidade = new Cidade();
                if (formacao.Cidade != null)
                {
                    formacao.Cidade.CompleteObject();
                    formacao.Cidade.Estado.CompleteObject();
                    c.Cidade._nome = formacao.Cidade.NomeCidade;
                    c.Cidade._estado = formacao.Cidade.Estado.SiglaEstado;
                }
                c._instituicao = formacao.DescricaoFonte;

                if (formacao.Escolaridade != null)
                {
                    formacao.Escolaridade.CompleteObject();
                    c._nivel = formacao.Escolaridade.DescricaoBNE;
                }

                c._nomeCurso = formacao.DescricaoCurso;
                c.Periodo = Convert.ToInt32(formacao.NumeroPeriodo);
                if (formacao.SituacaoFormacao != null)
                {
                    formacao.SituacaoFormacao.CompleteObject();
                    c._situacao = formacao.SituacaoFormacao.DescricaoSituacaoFormacao;
                }
                this.Cursos.Add(c);
            }

            //Listando Experiencias
            //Experiencia
            List<int> listExp = objPessoaFisica.RecuperarExperienciaProfissional(null);
            if (listExp.Count > 0)
            {
                this.Experiencias = new List<Experiencia>();
            }
            if (listExp.Count >= 1)
            {
                ExperienciaProfissional objExperienciaProfissional = ExperienciaProfissional.LoadObject(listExp[0]);
                this.Experiencias.Add(new Experiencia());
                this.Experiencias[0]._atribuicoes = objExperienciaProfissional.DescricaoAtividade;
                this.Experiencias[0].DataAdmissao = objExperienciaProfissional.DataAdmissao;
                if (objExperienciaProfissional.DataDemissao.HasValue)
                {
                    this.Experiencias[0].DataDemissao = objExperienciaProfissional.DataDemissao.Value;
                }
                this.Experiencias[0].Empresa = new Empresa();
                this.Experiencias[0].Empresa._nome = objExperienciaProfissional.RazaoSocial;
                if (objExperienciaProfissional.AreaBNE != null)
                {
                    objExperienciaProfissional.AreaBNE.CompleteObject();
                    this.Experiencias[0].Empresa._ramo = objExperienciaProfissional.AreaBNE.DescricaoAreaBNE;
                }
                this.Experiencias[0]._funcaoExercida = objExperienciaProfissional.DescricaoFuncaoExercida;
                if (objCurriculo.ValorUltimoSalario.HasValue)
                {
                    this.Experiencias[0].Salario = objCurriculo.ValorUltimoSalario.Value;
                }
            }

            if (listExp.Count >= 2)
            {
                //preenche campos segunda experiencia
                ExperienciaProfissional objExperienciaProfissional = ExperienciaProfissional.LoadObject(listExp[1]);
                this.Experiencias.Add(new Experiencia());
                this.Experiencias[1]._atribuicoes = objExperienciaProfissional.DescricaoAtividade;
                this.Experiencias[1].DataAdmissao = objExperienciaProfissional.DataAdmissao;
                if (objExperienciaProfissional.DataDemissao.HasValue)
                {
                    this.Experiencias[1].DataDemissao = objExperienciaProfissional.DataDemissao.Value;
                }
                this.Experiencias[1].Empresa = new Empresa();
                this.Experiencias[1].Empresa._nome = objExperienciaProfissional.RazaoSocial;
                if (objExperienciaProfissional.AreaBNE != null)
                {
                    objExperienciaProfissional.AreaBNE.CompleteObject();
                    this.Experiencias[1].Empresa._ramo = objExperienciaProfissional.AreaBNE.DescricaoAreaBNE;
                }
                this.Experiencias[1]._funcaoExercida = objExperienciaProfissional.DescricaoFuncaoExercida;
            }

            if (listExp.Count >= 3)
            {
                //preenche campos terceira experiencia
                ExperienciaProfissional objExperienciaProfissional = ExperienciaProfissional.LoadObject(listExp[2]);
                this.Experiencias.Add(new Experiencia());
                this.Experiencias[2]._atribuicoes = objExperienciaProfissional.DescricaoAtividade;
                this.Experiencias[2].DataAdmissao = objExperienciaProfissional.DataAdmissao;
                if (objExperienciaProfissional.DataDemissao.HasValue)
                {
                    this.Experiencias[2].DataDemissao = objExperienciaProfissional.DataDemissao.Value;
                }
                this.Experiencias[2].Empresa = new Empresa();
                this.Experiencias[2].Empresa._nome = objExperienciaProfissional.RazaoSocial;
                if (objExperienciaProfissional.AreaBNE != null)
                {
                    objExperienciaProfissional.AreaBNE.CompleteObject();
                    this.Experiencias[2].Empresa._ramo = objExperienciaProfissional.AreaBNE.DescricaoAreaBNE;
                }
                this.Experiencias[2]._funcaoExercida = objExperienciaProfissional.DescricaoFuncaoExercida;
            }
        }
    }

    [XmlType("cidade")]
    public class Cidade
    {
        [XmlIgnore]
        public string _nome { get; set; }
        [XmlElement("nome")]
        public System.Xml.XmlCDataSection Nome
        {
            get
            {
                return new System.Xml.XmlDocument().CreateCDataSection(_nome);
            }
            set
            {
                _nome = value.Value;
            }
        }

        [XmlIgnore]
        public string _estado { get; set; }
        [XmlElement("estado")]
        public System.Xml.XmlCDataSection Estado
        {
            get
            {
                return new System.Xml.XmlDocument().CreateCDataSection(_estado);
            }
            set
            {
                _estado = value.Value;
            }
        }
    }

    [XmlType("telefone")]
    public class Telefone
    {
        [XmlElement("ddd", DataType = "int")]
        public int DDD { get; set; }

        [XmlIgnore]
        public string _numero { get; set; }
        [XmlElement("numero")]
        public System.Xml.XmlCDataSection Numero
        {
            get
            {
                return new System.Xml.XmlDocument().CreateCDataSection(_numero);
            }
            set
            {
                _numero = value.Value;
            }
        }
    }

    [XmlType("experiencia")]
    public class Experiencia
    {
        [XmlElement("empresa")]
        public Empresa Empresa { get; set; }

        [XmlElement("dataAdmissao", DataType = "dateTime")]
        public DateTime DataAdmissao { get; set; }

        [XmlElement("dataDemissao", DataType = "dateTime")]
        public DateTime DataDemissao { get; set; }

        [XmlIgnore]
        public string _funcaoExercida { get; set; }
        [XmlElement("funcaoExercida")]
        public System.Xml.XmlCDataSection FuncaoExercida
        {
            get
            {
                return new System.Xml.XmlDocument().CreateCDataSection(_funcaoExercida);
            }
            set
            {
                _funcaoExercida = value.Value;
            }
        }

        [XmlIgnore]
        public string _atribuicoes { get; set; }
        [XmlElement("atribuicoes")]
        public System.Xml.XmlCDataSection Atribuicoes
        {
            get
            {
                return new System.Xml.XmlDocument().CreateCDataSection(_atribuicoes);
            }
            set
            {
                _atribuicoes = value.Value;
            }
        }

        [XmlElement("salario", DataType = "decimal")]
        public Decimal Salario { get; set; }
    }

    [XmlType("curso")]
    public class Curso
    {
        [XmlIgnore]
        public string _nivel { get; set; }
        [XmlElement("nivel")]
        public System.Xml.XmlCDataSection Nivel
        {
            get
            {
                return new System.Xml.XmlDocument().CreateCDataSection(_nivel);
            }
            set
            {
                _nivel = value.Value;
            }
        }

        [XmlIgnore]
        public string _situacao { get; set; }
        [XmlElement("situacao")]
        public System.Xml.XmlCDataSection Situacao
        {
            get
            {
                return new System.Xml.XmlDocument().CreateCDataSection(_situacao);
            }
            set
            {
                _situacao = value.Value;
            }
        }

        [XmlElement("anoConclusao", DataType = "int")]
        public int AnoConclusao { get; set; }

        [XmlIgnore]
        public string _instituicao { get; set; }
        [XmlElement("instituicao")]
        public System.Xml.XmlCDataSection Instituicao
        {
            get
            {
                return new System.Xml.XmlDocument().CreateCDataSection(_instituicao);
            }
            set
            {
                _instituicao = value.Value;
            }
        }

        [XmlIgnore]
        public string _nomeCurso { get; set; }
        [XmlElement("nomeCurso")]
        public System.Xml.XmlCDataSection NomeCurso
        {
            get
            {
                return new System.Xml.XmlDocument().CreateCDataSection(_nomeCurso);
            }
            set
            {
                _nomeCurso = value.Value;
            }
        }

        [XmlElement("periodo", DataType = "int")]
        public int Periodo { get; set; }

        [XmlElement("cidade")]
        public Cidade Cidade { get; set; }
    }

    [XmlType("empresa")]
    public class Empresa
    {
        [XmlIgnore]
        public string _nome { get; set; }
        [XmlElement("nome")]
        public System.Xml.XmlCDataSection Nome
        {
            get
            {
                return new System.Xml.XmlDocument().CreateCDataSection(_nome);
            }
            set
            {
                _nome = value.Value;
            }
        }

        [XmlIgnore]
        public string _ramo { get; set; }
        [XmlElement("ramo")]
        public System.Xml.XmlCDataSection Ramo
        {
            get
            {
                return new System.Xml.XmlDocument().CreateCDataSection(_ramo);
            }
            set
            {
                _ramo = value.Value;
            }
        }
    }
}
