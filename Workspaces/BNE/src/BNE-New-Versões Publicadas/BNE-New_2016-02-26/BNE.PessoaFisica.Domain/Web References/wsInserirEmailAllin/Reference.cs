﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace BNE.PessoaFisica.Domain.wsInserirEmailAllin {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.81.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="wsInserirEmailBaseBinding", Namespace="http://painel02.allinmail.com.br/wsAllin/inserir_email_base.php")]
    public partial class wsInserirEmailBaseService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback inserirEmailBaseOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event inserirEmailBaseCompletedEventHandler inserirEmailBaseCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("http://painel02.allinmail.com.br/wsAllin/inserir_email_base.php#inserirEmailBase", RequestNamespace="http://painel02.allinmail.com.br/wsAllin/inserir_email_base.php", ResponseNamespace="http://painel02.allinmail.com.br/wsAllin/inserir_email_base.php")]
        [return: System.Xml.Serialization.SoapElementAttribute("return")]
        public string inserirEmailBase(string ticket, object[] dados) {
            object[] results = this.Invoke("inserirEmailBase", new object[] {
                        ticket,
                        dados});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void inserirEmailBaseAsync(string ticket, object[] dados) {
            this.inserirEmailBaseAsync(ticket, dados, null);
        }
        
        /// <remarks/>
        public void inserirEmailBaseAsync(string ticket, object[] dados, object userState) {
            if ((this.inserirEmailBaseOperationCompleted == null)) {
                this.inserirEmailBaseOperationCompleted = new System.Threading.SendOrPostCallback(this.OninserirEmailBaseOperationCompleted);
            }
            this.InvokeAsync("inserirEmailBase", new object[] {
                        ticket,
                        dados}, this.inserirEmailBaseOperationCompleted, userState);
        }
        
        private void OninserirEmailBaseOperationCompleted(object arg) {
            if ((this.inserirEmailBaseCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.inserirEmailBaseCompleted(this, new inserirEmailBaseCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.81.0")]
    public delegate void inserirEmailBaseCompletedEventHandler(object sender, inserirEmailBaseCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.81.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class inserirEmailBaseCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal inserirEmailBaseCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591