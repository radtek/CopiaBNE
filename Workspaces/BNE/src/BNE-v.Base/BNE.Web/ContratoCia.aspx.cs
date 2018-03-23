using System;
using System.Collections.Specialized;
using System.Globalization;

namespace BNE.Web
{
    public partial class ContratoCia : System.Web.UI.Page
    {

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            Inicializar();
        }
        #endregion

        #endregion

        #region Metodos

        #region Inicializar
        public void Inicializar()
        {
            decimal valorPlano = Decimal.Zero;

            NameValueCollection nvc = Request.Form;

            if (string.IsNullOrWhiteSpace(nvc["ValorPlano"]))
                lblVlrPlano.Text = "0,00";

            if (string.IsNullOrWhiteSpace(nvc["NumeroParcelas"]))
            {
                lblNumParcelas.Text = "0";
                valorPlano = 0;
            }
            else
            {
                valorPlano = Convert.ToDecimal(nvc["ValorPlano"].ToString()) * Convert.ToDecimal(nvc["NumeroParcelas"].ToString());
            }

            vlrParcelaExtenso.Text = NumeroExtenso(Convert.ToDecimal(nvc["ValorPlano"]), true);

            lblVlrExtenso.Text = NumeroExtenso(Convert.ToDecimal(valorPlano), true);

            if (!string.IsNullOrWhiteSpace(nvc["RazaoSocial"]))
            {
                lblRazaoSocial.Text = nvc["RazaoSocial"];
                lblEmpresaAss.Text = nvc["RazaoSocial"];
            }
            else
                lblRazaoSocial.Text = "____";

            if (!string.IsNullOrWhiteSpace(nvc["NumeroCNPJ"]))
                lblCNPJ.Text = nvc["NumeroCNPJ"];
            else
                lblCNPJ.Text = "____";

            if (nvc["CEP"] != null)
                lblCEP.Text = nvc["CEP"];
            else
                lblCEP.Text = "____";

            if (!string.IsNullOrWhiteSpace(nvc["NomeCidade"]))
                lblCidade.Text = nvc["NomeCidade"];
            else
                lblCidade.Text = "____";

            if (!string.IsNullOrWhiteSpace(nvc["NumeroCPF"]))
                lblCPF.Text = nvc["NumeroCPF"];
            else
                lblCPF.Text = "____";

            if (nvc["NumeroRua"] != null)
                lblNumeroRua.Text = nvc["NumeroRua"];
            else
                lblNumeroRua.Text = "____";

            if (!string.IsNullOrWhiteSpace(nvc["NumeroParcelas"]))
                lblNumParcelas.Text = nvc["NumeroParcelas"];
            else
                lblNumParcelas.Text = "____";

            if (!string.IsNullOrWhiteSpace(nvc["NumeroParcelas"]))
                lblNumParcelasExtenso.Text = NumeroExtenso(Convert.ToDecimal(nvc["NumeroParcelas"]), false);
            else
                lblNumParcelasExtenso.Text = "____";

            if (!string.IsNullOrWhiteSpace(nvc["NomePessoa"]))
                lblResponsavel.Text = nvc["NomePessoa"];
            else
                lblResponsavel.Text = "____";

            if (!string.IsNullOrWhiteSpace(nvc["NumeroRG"]))
                lblRG.Text = nvc["NumeroRG"];
            else
                lblRG.Text = "____";

            if (!string.IsNullOrWhiteSpace(nvc["DescricaoRua"]))
                lblRua.Text = nvc["DescricaoRua"];
            else
                lblRua.Text = "____";

            if (!string.IsNullOrWhiteSpace(nvc["Estado"]))
                lblUF.Text = nvc["Estado"];
            else
                lblUF.Text = "____";

            if (!string.IsNullOrWhiteSpace(valorPlano.ToString()))
                lblVlrPlano.Text = valorPlano.ToString("0,0.00", CultureInfo.CurrentCulture);
            else
                lblVlrPlano.Text = "____";

            if (!string.IsNullOrWhiteSpace(nvc["TempoPlano"]))
                lblVlrTempoPlano.Text = nvc["TempoPlano"].ToString();
            else
                lblVlrTempoPlano.Text = "____";

            if (!string.IsNullOrWhiteSpace(nvc["TempoPlano"]))
                lblVlrTempoPlanoExtenso.Text = NumeroExtenso(Convert.ToDecimal(nvc["TempoPlano"]), false);
            else
                lblVlrTempoPlanoExtenso.Text = "____";

            if (!string.IsNullOrWhiteSpace(nvc["ValorPlano"]))
                vlrParcela.Text = nvc["ValorPlano"].ToString();
            else
                vlrParcela.Text = "____";

            if (!string.IsNullOrWhiteSpace(nvc["QuantidadeSms"]))
                lblQuantidadeSms.Text = nvc["QuantidadeSms"].ToString();
            else
                lblQuantidadeSms.Text = "____";

            if (!string.IsNullOrWhiteSpace(nvc["QuantidadeUsuarios"]))
                lblUsuario.Text = string.Format("{0} ({1})", nvc["QuantidadeUsuarios"], NumeroExtenso(Convert.ToDecimal(nvc["QuantidadeUsuarios"]), false));
            else
                lblUsuario.Text = "____";

        }
        #endregion

        #region NumeroExtenso

        private static string NumeroExtenso(decimal valor, bool monetario)
        {
            if (valor <= 0 | valor >= 1000000000000000)
                return "Valor não suportado pelo sistema.";
            else
            {
                string strValor = valor.ToString("000000000000000.00");
                string valor_por_extenso = string.Empty;

                for (int i = 0; i <= 15; i += 3)
                {
                    valor_por_extenso += escreva_parte(Convert.ToDecimal(strValor.Substring(i, 3)));
                    if (i == 0 & valor_por_extenso != string.Empty)
                    {
                        if (Convert.ToInt32(strValor.Substring(0, 3)) == 1)
                            valor_por_extenso += " Trilhão" + ((Convert.ToDecimal(strValor.Substring(3, 12)) > 0) ? " e " : string.Empty);
                        else if (Convert.ToInt32(strValor.Substring(0, 3)) > 1)
                            valor_por_extenso += " Trilhões" + ((Convert.ToDecimal(strValor.Substring(3, 12)) > 0) ? " e " : string.Empty);
                    }
                    else if (i == 3 & valor_por_extenso != string.Empty)
                    {
                        if (Convert.ToInt32(strValor.Substring(3, 3)) == 1)
                            valor_por_extenso += " Bilhão" + ((Convert.ToDecimal(strValor.Substring(6, 9)) > 0) ? " e " : string.Empty);
                        else if (Convert.ToInt32(strValor.Substring(3, 3)) > 1)
                            valor_por_extenso += " Bilhões" + ((Convert.ToDecimal(strValor.Substring(6, 9)) > 0) ? " e " : string.Empty);
                    }
                    else if (i == 6 & valor_por_extenso != string.Empty)
                    {
                        if (Convert.ToInt32(strValor.Substring(6, 3)) == 1)
                            valor_por_extenso += " Milhão" + ((Convert.ToDecimal(strValor.Substring(9, 6)) > 0) ? " e " : string.Empty);
                        else if (Convert.ToInt32(strValor.Substring(6, 3)) > 1)
                            valor_por_extenso += " Milhões" + ((Convert.ToDecimal(strValor.Substring(9, 6)) > 0) ? " e " : string.Empty);
                    }
                    else if (i == 9 & valor_por_extenso != string.Empty)
                        if (Convert.ToInt32(strValor.Substring(9, 3)) > 0)
                            valor_por_extenso += " Mil" + ((Convert.ToDecimal(strValor.Substring(12, 3)) > 0) ? " e " : string.Empty);

                    if (i == 12)
                    {
                        if (valor_por_extenso.Length > 8)
                            if (valor_por_extenso.Substring(valor_por_extenso.Length - 6, 6) == "Bilhão" | valor_por_extenso.Substring(valor_por_extenso.Length - 6, 6) == "MILHÃO")
                                valor_por_extenso += " de";
                            else
                                if (valor_por_extenso.Substring(valor_por_extenso.Length - 7, 7) == "Bilhões" | valor_por_extenso.Substring(valor_por_extenso.Length - 7, 7) == "MILHÕES" | valor_por_extenso.Substring(valor_por_extenso.Length - 8, 7) == "TRILHÕES")
                                    valor_por_extenso += " de";
                                else
                                    if (valor_por_extenso.Substring(valor_por_extenso.Length - 8, 8) == "Trilhões")
                                        valor_por_extenso += " de";

                        if (monetario)
                        {
                            if (Convert.ToInt64(strValor.Substring(0, 15)) == 1)
                                valor_por_extenso += " Real";
                            else if (Convert.ToInt64(strValor.Substring(0, 15)) > 1)
                                valor_por_extenso += " Reais";
                        }
                        if (Convert.ToInt32(strValor.Substring(16, 2)) > 0 && valor_por_extenso != string.Empty)
                            valor_por_extenso += " e ";
                    }

                    if (monetario)
                    {
                        if (i == 15)
                            if (Convert.ToInt32(strValor.Substring(16, 2)) == 1)
                                valor_por_extenso += " Centavo";
                            else if (Convert.ToInt32(strValor.Substring(16, 2)) > 1)
                                valor_por_extenso += " Centavos";
                    }
                }
                return valor_por_extenso;
            }
        }

        static string escreva_parte(decimal valor)
        {
            if (valor <= 0)
                return string.Empty;
            else
            {
                string montagem = string.Empty;
                if (valor > 0 & valor < 1)
                {
                    valor *= 100;
                }
                string strValor = valor.ToString("000");
                int a = Convert.ToInt32(strValor.Substring(0, 1));
                int b = Convert.ToInt32(strValor.Substring(1, 1));
                int c = Convert.ToInt32(strValor.Substring(2, 1));

                if (a == 1) montagem += (b + c == 0) ? "Cem" : "Cento";
                else if (a == 2) montagem += "Duzentos";
                else if (a == 3) montagem += "Trezentos";
                else if (a == 4) montagem += "Quatrocentos";
                else if (a == 5) montagem += "Quinhentos";
                else if (a == 6) montagem += "Seiscentos";
                else if (a == 7) montagem += "Setecentos";
                else if (a == 8) montagem += "Oitocentos";
                else if (a == 9) montagem += "Novecentos";

                if (b == 1)
                {
                    if (c == 0) montagem += ((a > 0) ? " e " : string.Empty) + "Dez";
                    else if (c == 1) montagem += ((a > 0) ? " e " : string.Empty) + "Onze";
                    else if (c == 2) montagem += ((a > 0) ? " e " : string.Empty) + "Doze";
                    else if (c == 3) montagem += ((a > 0) ? " e " : string.Empty) + "Treze";
                    else if (c == 4) montagem += ((a > 0) ? " e " : string.Empty) + "Quatorze";
                    else if (c == 5) montagem += ((a > 0) ? " e " : string.Empty) + "Quinze";
                    else if (c == 6) montagem += ((a > 0) ? " e " : string.Empty) + "Dezesseis";
                    else if (c == 7) montagem += ((a > 0) ? " e " : string.Empty) + "Dezessete";
                    else if (c == 8) montagem += ((a > 0) ? " e " : string.Empty) + "Dezoito";
                    else if (c == 9) montagem += ((a > 0) ? " e " : string.Empty) + "Dezenove";
                }
                else if (b == 2) montagem += ((a > 0) ? " e " : string.Empty) + "Vinte";
                else if (b == 3) montagem += ((a > 0) ? " e " : string.Empty) + "Trinta";
                else if (b == 4) montagem += ((a > 0) ? " e " : string.Empty) + "Quarenta";
                else if (b == 5) montagem += ((a > 0) ? " e " : string.Empty) + "Cinquenta";
                else if (b == 6) montagem += ((a > 0) ? " e " : string.Empty) + "Sessenta";
                else if (b == 7) montagem += ((a > 0) ? " e " : string.Empty) + "Setenta";
                else if (b == 8) montagem += ((a > 0) ? " e " : string.Empty) + "Oitenta";
                else if (b == 9) montagem += ((a > 0) ? " e " : string.Empty) + "Noventa";

                if (strValor.Substring(1, 1) != "1" & c != 0 & montagem != string.Empty) montagem += " E ";

                if (strValor.Substring(1, 1) != "1")
                    if (c == 1) montagem += "Um";
                    else if (c == 2) montagem += "Dois";
                    else if (c == 3) montagem += "Três";
                    else if (c == 4) montagem += "Quatro";
                    else if (c == 5) montagem += "Cinco";
                    else if (c == 6) montagem += "Seis";
                    else if (c == 7) montagem += "Sete";
                    else if (c == 8) montagem += "Oito";
                    else if (c == 9) montagem += "Nove";

                return montagem;
            }
        }

        #endregion


    }
        #endregion
}