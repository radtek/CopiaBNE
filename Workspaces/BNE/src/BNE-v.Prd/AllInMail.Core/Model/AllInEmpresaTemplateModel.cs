using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllInMail
{
    public class AllInEmpresaTemplateModel
    {
        public int E_PFId { get; set; }
        public string E_PFNome { get; set; }
        public string E_PFSexo { get; set; }
        public string E_PFCPF { get; set; }
       
        public DateTime E_PFDtNasc { get; set; }
        public bool E_PFMaster { get; set; }
        public bool E_PFInativo { get; set; }

        public string E_PFEmail { get; set; }

        public string E_PFFuncaoEx { get; set; }
        public string E_PFDDDCel { get; set; }
        public string E_PFCel { get; set; }
        public string E_PFDDDtel { get; set; }
        public string E_PFTel { get; set; }

        public DateTime? E_PFDtUltAcc { get; set; }
        public DateTime? E_PFDtUltPesqCv { get; set; }

        public int E_FilialId { get; set; }
        public string E_RazaoS { get; set; }
        public string E_NomeFan { get; set; }

        public string E_Situacao { get; set; }

        public bool E_Inativo { get; set; }

        public int? E_CepNum { get; set; }
        public string E_CepTex
        {
            get
            {
                if (!E_CepNum.HasValue)
                    return string.Empty;
                var cepText = E_CepNum.Value.ToString();
                if (cepText.Length <= 5)
                    return string.Empty;
                return new string(cepText.Take(5).Concat(new[] { '-' }).Concat(cepText.Skip(5)).ToArray());
            }
        }

        public string E_Cidade { get; set; }
        public string E_UF { get; set; }
        public string E_Bairro { get; set; }

        public string E_Site { get; set; }
        public bool? E_FCursos { get; set; }
        public bool? E_FAceitaEstag { get; set; }

        public bool? E_FPubVaga { get; set; }

        public bool E_FPlano { get; set; }
        public DateTime? E_DtInPlano { get; set; }
        public DateTime? E_DtFinPlano { get; set; }

        public DateTime E_DtCad { get; set; }
        public DateTime E_DtMod { get; set; }

        public DateTime? E_VGDt { get; set; }
        public string E_VGFunc { get; set; }
        public string E_VGCont { get; set; }
        public string E_VGCid { get; set; }
        public string E_VGUF { get; set; }
        public string E_VGEsc { get; set; }
        public string E_VGSexo { get; set; }
        public string E_VGAtrib { get; set; }

        public string E_CodCnae { get; set; }

        public DateTime? E_DtUltAcc { get; set; }
        public DateTime? E_DtUltPesqCv { get; set; }

        public string E_VendResp { get; set; }
        public DateTime E_VendDtContr { get; set; }
        public string E_VendSit { get; set; }

        public int? E_QtdCurVisTo { get; set; }
        public int? E_QtdCurVisSem { get; set; }

        public int? E_QtdCurRecA { get; set; }
        public int? E_QtdCurRecB { get; set; }

        public int? E_QtdCurStc { get; set; }

        public string E_AuxTextA { get; set; }
        public string E_AuxTextB { get; set; }

        public DateTime? E_AuxData { get; set; }
        public int? E_AuxNumber { get; set; }
    }
}
