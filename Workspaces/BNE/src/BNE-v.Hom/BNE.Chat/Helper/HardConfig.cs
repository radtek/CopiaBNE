using System;
using System.Configuration;

namespace BNE.Chat.Helper
{
    public sealed class HardConfig<T>
    {
        #region [ Fields ]
        private readonly string _configNamePath;
        private readonly T _valueDefault;

        private bool _isEval;
        private T _realValue;

        private bool _hasConfig;
        private bool _hasValidValue;
        private bool _accessError;
        #endregion

        #region [ Constructor ]
        public HardConfig(string configNamePath, T orDefault)
        {
            if (string.IsNullOrEmpty(configNamePath))
                throw new NullReferenceException("configNamePath");

            ValidateGenericType();

            this._configNamePath = configNamePath;
            this._valueDefault = orDefault;
        }
        #endregion

        #region [ Properties ]
        public T Value
        {
            get
            {
                if (_isEval)
                    return _realValue;

                Eval();
                return _realValue;
            }
        }

        public bool HasConfig
        {
            get
            {
                if (_isEval)
                    return _hasConfig;

                Eval();
                return _hasConfig;
            }
        }

        public bool HasValue
        {
            get
            {
                if (_isEval)
                    return _hasValidValue;

                Eval();
                return _hasValidValue;
            }
        }

        public bool AccessError
        {
            get
            {
                if (_isEval)
                    return _accessError;

                Eval();
                return _accessError;
            }
        }
        #endregion

        #region [ Public ]
        public void Reevaluate()
        {
            _hasConfig = false;
            _hasValidValue = false;
            _accessError = false;
            _isEval = false;
        }
        #endregion

        #region [ Private ]
        private void ValidateGenericType()
        {
            var type = typeof(T);

            if (type.IsValueType)
            {
                if (!type.IsEnum && !type.IsPrimitive)
                {
                    throw new NotSupportedException(typeof(T).Name + " type is not allowed.");
                }
            }
            else
            {
                if (type != typeof(string))
                    throw new NotSupportedException(typeof(T).Name + " type is not allowed.");
            }
        }

        private void Eval()
        {
            object config;
            try
            {
                config = ConfigurationManager.AppSettings.Get(_configNamePath);
            }
            catch
            {
                _accessError = true;
                return;
            }
            finally
            {
                _isEval = true;
            }

            if (config == null)
            {
                _realValue = _valueDefault;
                return;
            }

            T value;
            try
            {
                value = (T)Convert.ChangeType(config, typeof(T));
            }
            catch
            {
                _hasConfig = true;
                _realValue = _valueDefault;
                return;
            }

            _hasConfig = true;
            _hasValidValue = true;
            _realValue = value;
        }
        #endregion

    }
}
