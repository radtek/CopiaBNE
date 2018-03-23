using BNE.Services.AsyncServices.Plugins;
using BNE.Services.AsyncServices.Plugins.Interface;
using BNE.Services.Plugins.PluginsEntrada;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace BNE.Services.Plugins.PluginResult
{
    public enum AllInResultType
    {
        None,
        Transaction,
        LifeCycle
    }

    public class TriggerPluginResultEx<T> : TriggerPluginResult
    {
        public TriggerPluginResultEx(GatilhosEmail triggerWrapperPlugin, bool finishTask, AllInResultType typeResult, T tResult)
            : base(triggerWrapperPlugin, finishTask, typeResult, tResult)
        {
        }

        public T ResultValue
        {
            get
            {
                return (T)ObjectValue;
            }
        }
    }

    public class TriggerPluginResult : IPluginResult
    {
        private readonly GatilhosEmail triggerWrapperPlugin;
        private readonly AllInResultType _resType;
        private object _resValue;

        public object ObjectValue
        {
            get { return _resValue; }
        }

        public AllInResultType ResultType
        {
            get { return _resType; }
        }


        #region [ Construtor ]
        public TriggerPluginResult(GatilhosEmail triggerWrapperPlugin, bool finishTask)
        {
            this.triggerWrapperPlugin = triggerWrapperPlugin;
            this.InputPluginName = triggerWrapperPlugin.MetadataName;
            this.FinishTask = finishTask;
        }

        public TriggerPluginResult(GatilhosEmail triggerWrapperPlugin, bool finishTask, AllInResultType res, object resultValue)
        {
            this.triggerWrapperPlugin = triggerWrapperPlugin;
            this.FinishTask = finishTask;
            this._resType = res;
            this._resValue = resultValue;
        }
        #endregion

        #region [ Properties ]

        public string InputPluginName
        {
            get;
            protected set;
        }

        public bool FinishTask
        {
            get;
            protected set;
        }

        public bool FinishWithPonteAzul
        {
            get { return false; }
        }
        #endregion
    }
}
