using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BNE.Compress.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConsoleManager.Show();
                BNE.Compress.Console.Program.Main(new[] { TxtInput.Text, TxtOutput.Text });
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

            System.Console.WriteLine("Completed! Press Enter to Close");

            try
            {
                System.Console.ReadKey();
            }
            catch
            {

            }

            try
            {
                ConsoleManager.Hide();
            }
            catch
            {

            }

        }

        private void BtnInputFile_Click(object sender, RoutedEventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                var res = dlg.ShowDialog();

                if (res == System.Windows.Forms.DialogResult.OK || res == System.Windows.Forms.DialogResult.Yes)
                {
                    this.TxtInput.Text = dlg.FileName;

                    if (string.IsNullOrWhiteSpace(this.TxtOutput.Text))
                    {
                        this.TxtOutput.Text = dlg.FileName;
                    }
                }
            }
        }

        private void BtnInputFolder_Click(object sender, RoutedEventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {

                var res = dlg.ShowDialog();

                if (res == System.Windows.Forms.DialogResult.OK || res == System.Windows.Forms.DialogResult.Yes)
                {
                    this.TxtInput.Text = dlg.SelectedPath;

                    if (string.IsNullOrWhiteSpace(this.TxtOutput.Text))
                    {
                        this.TxtOutput.Text = dlg.SelectedPath;
                    }
                }
            }

        }

        private void BtnOutputFile_Click(object sender, RoutedEventArgs e)
        {
            using (var dlg = new SaveFileDialog())
            {
                if (!string.IsNullOrEmpty(TxtInput.Text))
                {
                    dlg.FileName = TxtInput.Text.Split('\\').LastOrDefault() ?? string.Empty;
                }

                var res = dlg.ShowDialog();

                if (res == System.Windows.Forms.DialogResult.OK || res == System.Windows.Forms.DialogResult.Yes)
                {
                    this.TxtOutput.Text = dlg.FileName;
                }
            }
        }

        private void BtnOutputFolder_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new FolderBrowserDialog();

            var res = dlg.ShowDialog();

            if (res == System.Windows.Forms.DialogResult.OK || res == System.Windows.Forms.DialogResult.Yes)
            {
                this.TxtOutput.Text = dlg.SelectedPath;
            }
        }
    }
}
