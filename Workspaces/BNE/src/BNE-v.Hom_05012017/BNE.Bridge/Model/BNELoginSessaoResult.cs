namespace BNE.Bridge
{

    public class BNESessaoLoginModelResult : BNESessaoLoginModelBase
    {
        private BNESessaoLoginResultValueInfo _metadataInfo;

        public BNESessaoLoginResultValueInfo DataProcessInfo
        {
            get { return _metadataInfo = _metadataInfo ?? new BNESessaoLoginResultValueInfo(); }
            set { _metadataInfo = value; }
        }

        public BNESessaoLoginModelResult()
        {

        }

        public BNESessaoLoginModelResult(BNESessaoLoginResultType bneSessaoLoginResultType)
        {
            this.Value = bneSessaoLoginResultType;
        }

        public BNESessaoLoginModelResult(BNESessaoLoginResultType bneSessaoLoginResultType, BNESessaoProfileType bneSessaoProfileType)
        {
            this.Profile = bneSessaoProfileType;
            this.Value = bneSessaoLoginResultType;
        }

        internal BNESessaoLoginModelResult WrapDetails(BNESessaoLoginProcessModel processInfo)
        {
            if (this.Profile == BNESessaoProfileType.CANDIDATO)
            {
                object objAuxCandidato;
                if (processInfo.ExtraInfo.TryGetValue("Curriculo", out objAuxCandidato))
                {
                    this.DataProcessInfo.Curriculo = (BLL.Curriculo)objAuxCandidato;
                }

                if (processInfo.ExtraInfo.TryGetValue("ExisteCurriculoNaOrigem", out objAuxCandidato))
                {
                    this.DataProcessInfo.ExisteCurriculoNaOrigem = (bool)objAuxCandidato;
                }

                if (processInfo.ExtraInfo.TryGetValue("UsaSTC", out objAuxCandidato))
                {
                    this.DataProcessInfo.UsaSTC = (bool)objAuxCandidato;
                }
            }

            object objAuxGeneral;
            if (processInfo.ExtraInfo.TryGetValue("QuantidadeEmpresas", out objAuxGeneral))
            {
                this.DataProcessInfo.QuantidadeEmpresas = (int)objAuxGeneral;
            }

            return this;
        }

    }
}
