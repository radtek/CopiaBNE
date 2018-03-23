using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SilverlightPrototype1Screens
{
	public partial class Debito_em_Conta : UserControl
	{
		public Debito_em_Conta()
		{
			try{
				// Required to initialize variables
				InitializeComponent();
				
				cbFormaPagamento.SelectedIndex = Pagamento.fp;
				
				if(cbFormaPagamento.SelectedIndex == 0){
					gridCartao.Visibility = System.Windows.Visibility.Collapsed;
					gridDebito.Visibility = System.Windows.Visibility.Visible;
				}else{
					gridCartao.Visibility = System.Windows.Visibility.Visible;
					gridDebito.Visibility = System.Windows.Visibility.Collapsed;
				}
			}
			catch(Exception ex){
				MessageBox.Show(ex.Message);
			}
		}

		private void cbFormaPagamento_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			try{
				if(cbFormaPagamento.SelectedIndex == 0){
					gridCartao.Visibility = System.Windows.Visibility.Collapsed;
					gridDebito.Visibility = System.Windows.Visibility.Visible;
				}else{
					gridCartao.Visibility = System.Windows.Visibility.Visible;
					gridDebito.Visibility = System.Windows.Visibility.Collapsed;
				}
			}catch(Exception ex){
				
			}
		}
	}
}