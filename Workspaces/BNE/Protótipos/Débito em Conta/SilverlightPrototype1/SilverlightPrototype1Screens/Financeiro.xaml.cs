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
	public partial class Financeiro : UserControl
	{
		public Financeiro()
		{
			// Required to initialize variables
			InitializeComponent();
			
			gridDadosEdicao.Visibility = System.Windows.Visibility.Collapsed;
			btnSalvar.Visibility = System.Windows.Visibility.Collapsed;
		}

		private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			gridDadosEdicao.Visibility = System.Windows.Visibility.Visible;
			btnSalvar.Visibility = System.Windows.Visibility.Visible;
			gridDadosFixos.Visibility = System.Windows.Visibility.Collapsed;
			btnEditar.Visibility = System.Windows.Visibility.Collapsed;
		}

		private void btnSalvar_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			gridDadosEdicao.Visibility = System.Windows.Visibility.Collapsed;
			btnSalvar.Visibility = System.Windows.Visibility.Collapsed;
			gridDadosFixos.Visibility = System.Windows.Visibility.Visible;
			btnEditar.Visibility = System.Windows.Visibility.Visible;
		}
	}
}