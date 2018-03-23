using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AllInMail.Core
{
    public class AllInEmpresaDefConverter : AllInDefConverterStd<AllInEmpresaTemplateModel>
    {
        private string _delimiter = ";";
        private readonly string _replaceDelimiter = "|";
        private readonly string _declaration = "e_pfId;e_pfNome;e_pfSexo;e_pfCpf;e_pfDtNasc;e_pfMaster;e_pfInativo;e_pfEmail;e_pfFuncaoEx;e_pfDDDCel;e_pfCel;e_pfDDDTel;e_pfTel;e_pfDtUltAcc;e_pfDtUltPesqCv;e_filialId;e_razaoS;e_nomeFan;e_situacao;e_Inativo;e_cepNum;e_cepTex;e_cidade;e_uf;e_bairro;e_site;e_fCursos;e_fAceitaEstag;e_fPubVaga;e_fPlano;e_dtInPlano;e_dtFinPlano;e_dtCad;e_dtMod;e_vgDt;e_vgFunc;e_vgCont;e_vgCid;e_vgUF;e_vgEsc;e_vgSexo;e_vgAtrib;e_codCnae;e_dtUltAcc;e_DtUltPesqCv;e_vendResp;e_vendDtContr;e_vendSit;e_qtdCurVisTo;e_qtdCurVisSem;e_qtdCurRecA;e_qtdCurRecB;e_qtdCurStc;e_auxTextA;e_auxTextB;e_auxData;e_auxNumber";

        public override string Delimiter
        {
            get { return _delimiter; }
            set { _delimiter = value; }
        }

        public override string GetDeclaration()
        {
            return _declaration;
        }

        protected override IEnumerable<IDefFilter> InitTreatmentFilters()
        {
            yield break;
        }

        protected override IEnumerable<DefPreParser> PreParsers()
        {
            yield return AllInParser.BoolFormat;
            yield return AllInParser.BoolNullableFormat;
            yield return AllInParser.DatePtBrCustomFormat;
            yield return AllInParser.DateNullablePtBrCustomFormat;
            yield return AllInParser.IntNullableFormat;
            yield return AllInParser.StringNullFormat;
        }

        protected override IEnumerable<DefFilterInit<object, string>> IntermediateFilters()
        {
            yield break;
        }

        protected override IEnumerable<DefPosParser> PosParsers()
        {
            yield return new DefPosParser(a => a.Length > 250 ? new string(a.Take(247).Concat("...").ToArray()) : a);
            yield return new DefPosParser(a => Regex.Replace(a, @"\r\n?|\n", " "));
            yield return new DefPosParser(a => a.Trim());
            yield return new DefPosParser(a => a.Replace(_delimiter, _replaceDelimiter));
        }

        protected override IEnumerable<DefPosFilter> PosFilters()
        {
            yield return new DefPosFilterFor<AllInEmpresaTemplateModel>(a => a.E_UF,
                                                     a => a.Length > 2 ? new string(a.Take(2).Select(b => char.ToUpper(b)).ToArray()) : a.ToUpper());

            yield return new DefPosFilterFor<AllInEmpresaTemplateModel>(a => a.E_VGUF,
                                                  a => a.Length > 2 ? new string(a.Take(2).Select(b => char.ToUpper(b)).ToArray()) : a.ToUpper());

            yield return new DefPosFilterFor<AllInEmpresaTemplateModel>(a => a.E_CepNum,
                                                             a => new string(a.Where(char.IsNumber).ToArray()));

            yield return new DefPosFilterFor<AllInEmpresaTemplateModel>(a => a.E_CepNum,
                                                                a => a.Length > 8 ? new string(a.Take(8).ToArray()) : a);

            yield return new DefPosFilterFor<AllInEmpresaTemplateModel>(a => a.E_CepTex,
                                                                a => a.Length > 10 ? new string(a.Take(10).ToArray()) : a);

            yield return new DefPosFilterFor<AllInEmpresaTemplateModel>(a => a.E_PFSexo)
                                                    .UseWith(a => new string(a.Take(1).ToArray()).ToLower())
                                                    .UseTooWith(a => a != "m" && a != "f" ? "" : a);


        }
    }
}
