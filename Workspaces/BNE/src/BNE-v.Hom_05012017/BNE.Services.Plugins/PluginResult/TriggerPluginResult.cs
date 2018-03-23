using BNE.Services.AsyncServices.Plugins.Interface;
using BNE.Services.Plugins.PluginsEntrada;

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
        public T ResultValue
        {
            get { return (T) ObjectValue; }
        }

        public TriggerPluginResultEx(GatilhosEmail triggerWrapperPlugin, bool finishTask, AllInResultType typeResult, T tResult)
            : base(triggerWrapperPlugin, finishTask, typeResult, tResult)
        {
        }
    }

    public class TriggerPluginResult : IPluginResult
    {
        private readonly AllInResultType _resType;
        private readonly GatilhosEmail _triggerWrapperPlugin;

        public object ObjectValue { get; private set; }

        public AllInResultType ResultType
        {
            get { return _resType; }
        }

        #region [ Construtor ]
        public TriggerPluginResult(GatilhosEmail triggerWrapperPlugin, bool finishTask)
        {
            _triggerWrapperPlugin = triggerWrapperPlugin;
            InputPluginName = triggerWrapperPlugin.MetadataName;
            FinishTask = finishTask;
        }

        public TriggerPluginResult(GatilhosEmail triggerWrapperPlugin, bool finishTask, AllInResultType res, object resultValue)
        {
            _triggerWrapperPlugin = triggerWrapperPlugin;
            FinishTask = finishTask;
            _resType = res;
            ObjectValue = resultValue;
        }
        #endregion

        #region [ Properties ]
        public string InputPluginName { get; protected set; }

        public bool FinishTask { get; protected set; }

        public bool FinishWithPonteAzul
        {
            get { return false; }
        }
        #endregion
    }
}