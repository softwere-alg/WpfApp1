using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace WpfApp1
{
    //public class ListViewItem
    //{
    //    public ObservableObject<bool> Checked { get; }
    //    public ObservableObject<string> Item1 { get; }
    //    public ObservableObject<string> Item2 { get; }

    //    public ListViewItem(bool initChecked, string item1, string item2)
    //    {
    //        Checked = new ObservableObject<bool>(initChecked);
    //        Item1 = new ObservableObject<string>(item1);
    //        Item2 = new ObservableObject<string>(item2);
    //    }
    //}

    /// <summary>
    /// ListViewEx.xaml の相互作用ロジック
    /// </summary>
    public partial class ListViewEx : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public class ListViewItem
        {
            public ObservableObject<bool> Checked { get; }
            public ReadOnlyCollection<ObservableObject<string>> SubItems { get; }

            private ListViewItem(bool initChecked, List<ObservableObject<string>> list)
            {
                Checked = new ObservableObject<bool>(initChecked);
                SubItems = new ReadOnlyCollection<ObservableObject<string>>(list);
            }
        }

        public class ListViewHeader
        {
            public string Title { get; }
            public double Width { get; }

            public ListViewHeader(string title, double width)
            {
                Title = title;
                Width = width;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        private ObservableCollectionEx<ListViewItem> _itemsSource;
        public ObservableCollectionEx<ListViewItem> ItemsSource
        {
            get
            {
                return _itemsSource;
            }

            set
            {
                _itemsSource = value;

                lv.ItemsSource = _itemsSource;
                _itemsSource.AddItemProcess += BindItem;
                _itemsSource.RemoveItemProcess += DebindItem;
                BindItemsSource();
            }
        }

        private ReadOnlyCollection<ListViewHeader> _headers;
        public ReadOnlyCollection<ListViewHeader> Headers
        {
            get
            {
                return _headers;
            }

            //set
            //{
            //    _headers = value;

            //    GridView gridView = new GridView();

            //    for (int i = 0; i < _headers.Count; i++)
            //    {
            //        ListViewHeader header = _headers[i];

            //        if (i == 0)
            //        {
            //            gridView.Columns.Add(new GridViewColumn()
            //            {
            //                Header = header.Title,
            //                Width = header.Width,
            //                CellTemplate = FindResource("checkbox") as DataTemplate
            //            });
            //        }
            //        else
            //        {
            //            gridView.Columns.Add(new GridViewColumn()
            //            {
            //                Header = header.Title,
            //                Width = header.Width,
            //                DisplayMemberBinding = new Binding($"SubItems[{i}].Value")
            //            });
            //        }
            //    }
            //    lv.View = gridView;
            //}
        }

        private bool _checkbox = false;
        public bool CheckBox
        {
            get { return _checkbox; }
        }

        public ListViewEx(bool checkbox, IList<ListViewHeader> headers)
        {
            InitializeComponent();

            _checkbox = checkbox;
            _headers = new ReadOnlyCollection<ListViewHeader>(headers);

            GridView gridView = new GridView();

            for (int i = 0; i < _headers.Count; i++)
            {
                ListViewHeader header = _headers[i];

                if (i == 0 && _checkbox)
                {
                    gridView.Columns.Add(new GridViewColumn()
                    {
                        Header = header.Title,
                        Width = header.Width,
                        CellTemplate = FindResource("checkbox") as DataTemplate
                    });
                }
                else
                {
                    gridView.Columns.Add(new GridViewColumn()
                    {
                        Header = header.Title,
                        Width = header.Width,
                        DisplayMemberBinding = new Binding($"SubItems[{i}].Value")
                    });
                }
            }
            lv.View = gridView;
        }

        public ListViewItem CreateListViewItems(bool initChecked, List<string> list)
        {
            List<ObservableObject<string>> oList = new List<ObservableObject<string>>();
            int i = 0;
            for (; i < Math.Min(list.Count, _headers.Count); i++)
            {
                oList.Add(new ObservableObject<string>(list[i]));
            }
            for (; i < _headers.Count; i++)
            {
                oList.Add(new ObservableObject<string>(""));
            }

            System.Reflection.BindingFlags flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic;
            ListViewItem listViewItem = (ListViewItem)Activator.CreateInstance(typeof(ListViewItem), flags, null, new object[] { initChecked, oList }, null);
            return listViewItem;
        }

        private void BindItemsSource()
        {
            foreach (ListViewItem item in _itemsSource)
            {
                BindItem(item);
            }
        }

        public void BindItem(ListViewItem item)
        {
            item.Checked.PropertyChanged += Selected_PropertyChanged;
            item.Checked.PropertyChanging += Selected_PropertyChanging;
        }

        public void DebindItem(ListViewItem item)
        {
            item.Checked.PropertyChanged -= Selected_PropertyChanged;
            item.Checked.PropertyChanging -= Selected_PropertyChanging;
        }

        private void Selected_PropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            PropertyChanging?.Invoke(sender, e);
        }

        private void Selected_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(sender, e);
        }
    }
}
