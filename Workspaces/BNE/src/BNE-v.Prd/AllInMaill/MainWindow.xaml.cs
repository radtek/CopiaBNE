using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit.PropertyGrid;
using System.Reactive;
using System.Reactive.Linq;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Metadata.Edm;
using AllInMail.Base.Vm;
using AllInMail.Vm;
using AllInMail.Helper;
using System.IO;
using System.Threading;
using AllInMail.Core;

namespace AllInMail
{


    public partial class MainWindow : Window
    {
        private readonly MainViewModel _mainVm;
        private CancellationTokenSource _token;
        public MainWindow()
        {
            InitializeComponent();

            Observable.FromEvent<RoutedEventHandler, RoutedEventArgs>(t =>
                                    new RoutedEventHandler((s, e) => t(e)),
                                    a => this.Loaded += a,
                                    b => this.Loaded -= b)
                                        .Take(1)
                                        .Subscribe(MainWindow_Loaded);

            this._mainVm = new MainViewModel();
            this._mainVm.ActionTitle = "PROCESS";
            this._mainVm.CanInvokeAction = true;
            this.DataContext = _mainVm;
        }

        async void MainWindow_Loaded(RoutedEventArgs e)
        {
            //var et = new BNE_Entities();
            //et.Configuration.LazyLoadingEnabled = true;
            //et.Configuration.ProxyCreationEnabled = true;

            //var first = et.BNE_Curriculo.First();

            //if (first.Dta_Cadastro > new DateTime())
            //{
            //    DbModelBuilder d = new DbModelBuilder(DbModelBuilderVersion.Latest);

            //    d.Entity<BNE_Curriculo>().Ignore(a => a.Dta_Cadastro);

            //    var res = d.Build(et.Database.Connection);

            //    var toUse = res.Compile();
            //    var model = toUse.CreateObjectContext<ObjectContext>(et.Database.Connection);

            //    if (first.Dta_Cadastro == new DateTime())
            //    {

            //    }
            //    var query = et.BNE_Curriculo.Where(a => a.Idf_Curriculo > 0);

            //    query.Include(a => a.BNE_Conversas_Ativas);
            //}

            //await Task.Delay(100);
            //MessageBox.Show("Teste");
        }

        private void File_Click(object sender, RoutedEventArgs e)
        {
            SelectFile();
        }

        private bool SelectFile()
        {
            var dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Data"; // Default file name
            dlg.DefaultExt = ".csv"; // Default file extension
            dlg.Filter = "CSV documents (.csv)|*.csv"; // Filter files by extension

            var res = dlg.ShowDialog();

            if (!res.HasValue)
                return false;

            _mainVm.OutputFile = dlg.FileName;
            return true;
        }

        private async void Process_Click(object sender, RoutedEventArgs e)
        {
            if ("PROCESS".EqualsEx(_mainVm.ActionTitle))
            {
                if (await PrepareProcess())
                {
                    await StartProcess();
                }
                return;
            }

            if (_token == null)
                return;

            if (MessageBox.Show("Tem certeza que deseja interromper o processo", "Aviso", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;

            _mainVm.CanInvokeAction = false;
            _token.Cancel();
        }

        private async Task<bool> PrepareProcess()
        {
            if (_token != null)
                _token.Dispose();

            _token = new CancellationTokenSource();

            if (((MetricSettings)_mainVm.StartSettings).ContinueInFinalOfTheFile)
            {
                if (!File.Exists(_mainVm.OutputFile))
                {
                    MessageBox.Show("Arquivo inexistente!", "Aviso", MessageBoxButton.OK);
                    return false;
                }
            }
            else
            {
                if (File.Exists(_mainVm.OutputFile))
                {
                    var res = MessageBox.Show("Arquivo já existe, deseja utilizá-lo?", "Questão", MessageBoxButton.YesNo);

                    if (res == MessageBoxResult.Cancel)
                        return false;

                    if (res == MessageBoxResult.No)
                    {
                        if (!SelectFile())
                            return false;

                        return await PrepareProcess();
                    }
                }
            }

            return true;
        }

        private async Task StartProcess()
        {
            _mainVm.ActionTitle = "STOP";
            bool cancel = false;
            _mainVm.EnableEditStartedSettings = false;
            try
            {
                var folder = new string(_mainVm.OutputFile.Reverse().SkipWhile(a => a != '\\').Reverse().ToArray());

                var dir = new DirectoryInfo(folder);
                if (!dir.Exists)
                {
                    MessageBox.Show("Pasta Inexistente", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var context = SynchronizationContext.Current;
                _mainVm.SetNotificationContext(context);

                var progress = new ProgressViewModel();
                progress.SetNotificationContext(context);
                _mainVm.Progress = progress;

                string res = await Process();
                MessageBox.Show(res);
            }
            catch (TaskCanceledException ex)
            {
                cancel = true;
            }
            catch (OperationCanceledException ex)
            {
                cancel = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                _mainVm.EnableEditStartedSettings = true;
                _mainVm.CanInvokeAction = true;
                if (cancel)
                {
                    MessageBox.Show("Processo cancelado com sucesso.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                _mainVm.ActionTitle = "PROCESS";
            }
        }

        private async Task<string> Process()
        {
            IExporterDataBasic exporter;
            if (_mainVm.ProcessType == ExportationType.Curriculo)
                exporter = new ExporterDataBase(_mainVm);
            else
                exporter = new ExporterEmpresaDefault(_mainVm);

            Lazy<Stream> mainStreamLazy = new Lazy<Stream>(() =>
            {
                Stream stream;
                if (((MetricSettings)_mainVm.StartSettings).ContinueInFinalOfTheFile)
                {
                    stream = new FileStream(_mainVm.OutputFile, FileMode.Append, FileAccess.Write, FileShare.None);
                    stream.Seek(0, System.IO.SeekOrigin.End);
                    return stream;
                }
                else
                {
                    if (File.Exists(_mainVm.OutputFile))
                        File.Delete(_mainVm.OutputFile);

                    stream = new FileStream(_mainVm.OutputFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                }
                return stream;
            });

            var t1 = exporter.Process(mainStreamLazy, _token.Token);
            string res;
            try
            {
                res = await t1;
            }
            finally
            {
                if (mainStreamLazy.IsValueCreated)
                {
                    mainStreamLazy.Value.Close();
                    mainStreamLazy.Value.Dispose();
                }
            }
            return res;
        }



    }
}
