using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;

namespace BNE.Chat.Helper
{
    public sealed class KeyWrapper<TKey, TObject> : IStructuralEquatable, IStructuralComparable, IComparable
    {
        #region [ Attributos ]
        private readonly TKey _key;
        #endregion

        #region [ Constructor ]
        public KeyWrapper(TKey key)
        {
            this._key = key;
        }

        public KeyWrapper(TKey key, TObject obj)
        {
            this._key = key;
            this.Item = obj;
        }
        #endregion

        #region [ Properties ]
        public TKey Key
        {
            get { return _key; }
        }

        public TObject Item { get; set; }
        #endregion

        #region [ Public Methods ]
        public override int GetHashCode()
        {
            return _key.GetHashCode();
        }
    
        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as KeyWrapper<TKey, TObject>;
            if (obj != null)
            {
                return this._key.Equals(obj._key);
            }

            return false;
        }
        #endregion

        #region [ Explicit ] 
        int IComparable.CompareTo(object obj)
        {
            return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);
        }

        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            if (other == null)
                return 1;

            var tuple = other as KeyWrapper<TKey, TObject>;
            if (tuple == null)
            {
                throw new ArgumentException(string.Format("ArgumentException_IncorrectType : {0} of {1}.",
                   this.GetType(), "other"));
            }

            int num = comparer.Compare(this._key, tuple._key);
            return num;
        }

        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            if (other == null)
                return false;

            var tuple = other as KeyWrapper<TKey, TObject>;

            if (tuple == null || !comparer.Equals(this._key, tuple._key))
                return false;

            return true;
        }

        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            return comparer.GetHashCode(this._key);
        }
        #endregion
    }
}
