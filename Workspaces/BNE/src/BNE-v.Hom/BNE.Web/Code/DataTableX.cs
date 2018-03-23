using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Printing;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace BNE.Web.Code
{
    public static class DataTableX
    {
        public static IEnumerable<dynamic> AsDynamicEnumerable(this DataTable table)
        {
            // Validate argument here..

            return table.AsEnumerable().Select(row => new DynamicRow(row));
        }

        private sealed class DynamicRow : DynamicObject
        {
            private readonly Dictionary<string, object> _objectStore = new Dictionary<string, object>();
            private readonly DataRow _row;

            private DynamicRow()
            {

            }
            internal DynamicRow(DataRow row)
            {
                _row = row;
            }

            public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
            {
                if (binder.Name == "ShallowCopy" && args.Length == 0)
                {
                    result = ShallowCopy();
                    return true;
                }
                return base.TryInvokeMember(binder, args, out result);
            }

            // Interprets a member-access as an indexer-access on the 
            // contained DataRow.
            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                bool retVal;
                if (_row != null)
                {
                    retVal = _row.Table.Columns.Contains(binder.Name);
                    if (retVal)
                        result = _row[binder.Name];
                    else
                        result = null;
                }
                else
                    retVal = _objectStore.TryGetValue(binder.Name, out result);

                return retVal;
            }

            public override bool TrySetMember(SetMemberBinder binder, object value)
            {
                bool retVal;
                if (_row != null)
                {
                    retVal = _row.Table.Columns.Contains(binder.Name);
                    if (retVal)
                        _row[binder.Name] = value;
                }
                else
                {
                    _objectStore[binder.Name] = value;
                    retVal = true;
                }

                return retVal;
            }

            public DynamicRow ShallowCopy()
            {
                var cloneObj = new DynamicRow();

                if (_row != null)
                {
                    for (int index = 0; index < _row.ItemArray.Length; index++)
                    {
                        cloneObj._objectStore.Add(_row.Table.Columns[index].ColumnName, _row.ItemArray[index]);
                    }

                    foreach (var customItem in _objectStore)
                    {
                        cloneObj._objectStore.Add(customItem.Key, customItem.Value);
                    }
                }

                return cloneObj;
            }
        }
    }
}