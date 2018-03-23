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
	public partial class PAgamento_com_Desconto : UserControl
	{
		public PAgamento_com_Desconto()
		{
			// Required to initialize variables
			InitializeComponent();
		}

		private void hlDebito_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			Pagamento.fp = 0;
		}

		private void hlCredito_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			Pagamento.fp = 1;
		}
	}
}