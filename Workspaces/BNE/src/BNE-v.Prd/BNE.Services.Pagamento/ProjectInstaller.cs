﻿using System.ComponentModel;
using System.Configuration.Install;


namespace BNE.Services.Pagamento
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
    }
}
