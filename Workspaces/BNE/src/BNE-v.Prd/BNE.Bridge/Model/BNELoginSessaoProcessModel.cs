using BNE.BLL.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BNE.Bridge
{
    internal class BNESessaoLoginProcessModel
    {
        private Dictionary<string, object> _processInformation;
        public Dictionary<string, object> ExtraInfo
        {
            get { return _processInformation = _processInformation ?? new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase); }
            set { _processInformation = value; }
        }

        private Action _onCompleteAction;
        public Action OnCompleteAction
        {
            get { return _onCompleteAction; }
            set { _onCompleteAction = value; }
        }

        public BNESessaoLoginResultType Result { get; set; }

        public BNESessaoProfileType Profile { get; set; }

        public void Concat(Action action)
        {
            _onCompleteAction += action;
        }

        public void Override(Action action)
        {
            _onCompleteAction = action;
        }

        internal void Remove(Action action)
        {
            _onCompleteAction -= action;
        }
    }
}
