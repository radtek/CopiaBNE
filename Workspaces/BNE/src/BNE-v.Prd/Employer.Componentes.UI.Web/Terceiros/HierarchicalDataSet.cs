using System;
using System.Data;
using System.Web.UI;
using System.Collections;
using System.Web.UI.WebControls;

/*************************************************************************/
/*   Written By Ralph Varjabedian                                        */
/*   You may use this code freely and copy this code provided that       */
/*   You do not remove this copyright notice.                            */
/*   April 2008                                                          */
/*   http://www.varjabedian.net/archive/2008/04/22/binding-asp.net-treeview-to-a-dataset-or-an-objectdatasource.aspx */
/*   Revision 2.2                                                        */
/*     Taking into account string column names                           */
/*     Adding DataView support to enable Linq queries to be binded using */
/*     AsDataView() extension method    
/*************************************************************************/

namespace Varjabedian.HierarchicalDataSet
{
    /// <summary>
    /// A class that translates a DataSet into IHierarchicalDataSource that can be used to bind Hierarchical data to a TreeView
    /// </summary>
    #pragma warning disable 1591
    public class HierarchicalDataSet : IHierarchicalDataSource
    {
        /// <summary>
        /// The DataView reference that the class is pointing to
        /// </summary>
        DataView dataView;

        /// <summary>
        /// The name of the primary key column
        /// </summary>
        string idColumnName;

        /// <summary>
        /// The name of the parent foreign key column
        /// </summary>
        string parentIdColumnName;

        /// <summary>
        /// evaluated once the constructor is called, true if the primary keys are strings
        /// </summary>
        bool columnIsString;

        /// <summary>
        /// The constructor of the class
        /// </summary>
        /// <param name="dataSet">The dataset that contains the data, the first table's view will be taken</param>
        /// <param name="idColumnName">The Primary key column name</param>
        /// <param name="parentIdColumnName">The Parent Primary key column name that identifies the Parent-Child relationship</param>
        public HierarchicalDataSet(DataSet dataSet, string idColumnName, string parentIdColumnName)
        {
            this.dataView = dataSet.Tables[0].DefaultView;
            this.idColumnName = idColumnName;
            this.parentIdColumnName = parentIdColumnName;

            if (dataSet.Tables[0].Columns[idColumnName].DataType != dataSet.Tables[0].Columns[parentIdColumnName].DataType)
                throw new Exception("The two column names passed should be of the same type");

            columnIsString = dataSet.Tables[0].Columns[idColumnName].DataType == typeof(string);
        }

        /// <summary>
        /// The constructor of the class
        /// </summary>
        /// <param name="dataView">The dataView that contains the data</param>
        /// <param name="idColumnName">The Primary key column name</param>
        /// <param name="parentIdColumnName">The Parent Primary key column name that identifies the Parent-Child relationship</param>
        public HierarchicalDataSet(DataView dataView, string idColumnName, string parentIdColumnName)
        {
            this.dataView = dataView;
            this.idColumnName = idColumnName;
            this.parentIdColumnName = parentIdColumnName;

            if (dataView.Table.Columns[idColumnName].DataType != dataView.Table.Columns[parentIdColumnName].DataType)
                throw new Exception("The two column names passed should be of the same type");

            columnIsString = dataView.Table.Columns[idColumnName].DataType == typeof(string);
        }

        public event EventHandler DataSourceChanged; // never used here

        public HierarchicalDataSourceView GetHierarchicalView(string viewPath)
        {
            return new DataSourceView(this, viewPath);
        }

        #region supporting methods
        /// <summary>
        /// Gets the parent row, given a row
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        DataRowView GetParentRow(DataRowView row)
        {
            dataView.RowFilter = GetFilter(idColumnName, row[parentIdColumnName].ToString());
            DataRowView parentRow = dataView[0];
            dataView.RowFilter = "";
            return parentRow;
        }

        /// <summary>
        /// Prepares the filter based on the column names and type
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetFilter(string columnName, string value)
        {
            if (columnIsString)
                return String.Format("{0} = '{1}'", columnName, value.Replace("'", "''"));
            else
                return String.Format("{0} = {1}", columnName, value);
        }

        string GetChildrenViewPath(string viewPath, DataRowView row)
        {
            return viewPath + "\\" + row[idColumnName].ToString();
        }

        bool HasChildren(DataRowView row)
        {
            dataView.RowFilter = GetFilter(parentIdColumnName, row[idColumnName].ToString());
            bool hasChildren = dataView.Count > 0;
            dataView.RowFilter = "";
            return hasChildren;
        }

        string GetParentViewPath(string viewPath)
        {
            return viewPath.Substring(0, viewPath.LastIndexOf("\\"));
        }
        #endregion

        #region private classes that implement further interfaces
        class DataSourceView : HierarchicalDataSourceView
        {
            HierarchicalDataSet hDataSet;
            string viewPath;

            public DataSourceView(HierarchicalDataSet hDataSet, string viewPath)
            {
                this.hDataSet = hDataSet;
                this.viewPath = viewPath;
            }

            public override IHierarchicalEnumerable Select()
            {
                return new HierarchicalEnumerable(hDataSet, viewPath);
            }
        }

        class HierarchicalEnumerable : IHierarchicalEnumerable
        {
            HierarchicalDataSet hDataSet;
            string viewPath;

            public HierarchicalEnumerable(HierarchicalDataSet hDataSet, string viewPath)
            {
                this.hDataSet = hDataSet;
                this.viewPath = viewPath;
            }

            public IHierarchyData GetHierarchyData(object enumeratedItem)
            {
                DataRowView row = (DataRowView)enumeratedItem;
                return new HierarchyData(hDataSet, viewPath, row);
            }

            public IEnumerator GetEnumerator()
            {
                if (viewPath == "")
                    hDataSet.dataView.RowFilter = String.Format("{0} is null", hDataSet.parentIdColumnName);
                else
                {
                    string lastID = viewPath.Substring(viewPath.LastIndexOf("\\") + 1);
                    hDataSet.dataView.RowFilter = hDataSet.GetFilter(hDataSet.parentIdColumnName, lastID);
                }

                IEnumerator i = hDataSet.dataView.ToTable().DefaultView.GetEnumerator();
                hDataSet.dataView.RowFilter = "";

                return i;
            }
        }

        class HierarchyData : IHierarchyData
        {
            HierarchicalDataSet hDataSet;
            DataRowView row;
            string viewPath;

            public HierarchyData(HierarchicalDataSet hDataSet, string viewPath, DataRowView row)
            {
                this.hDataSet = hDataSet;
                this.viewPath = viewPath;
                this.row = row;
            }

            public IHierarchicalEnumerable GetChildren()
            {
                return new HierarchicalEnumerable(hDataSet, hDataSet.GetChildrenViewPath(viewPath, row));
            }

            public IHierarchyData GetParent()
            {
                return new HierarchyData(hDataSet, hDataSet.GetParentViewPath(viewPath), hDataSet.GetParentRow(row));
            }

            public bool HasChildren
            {
                get
                {
                    return hDataSet.HasChildren(row);
                }
            }

            public object Item
            {
                get
                {
                    return row;
                }
            }

            public string Path
            {
                get
                {
                    return viewPath;
                }
            }

            public string Type
            {
                get
                {
                    return typeof(DataRowView).ToString();
                }
            }
        }
        #endregion
    }

    public static class HierarchicalDataSet_TreeView_Extensions
    {
        public static void SetDataSourceFromDataSet(this TreeView treeView, DataSet dataSet, string idColumnName, string parentIdColumnName)
        {
            treeView.DataSource = new HierarchicalDataSet(dataSet, idColumnName, parentIdColumnName);
        }
    }
}