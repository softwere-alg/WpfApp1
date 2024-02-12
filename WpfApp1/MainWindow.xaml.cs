using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private ListViewEx lv;

        public MainWindow()
        {
            InitializeComponent();

            lv = new ListViewEx(true, new List<ListViewEx.ListViewHeader>() {
                new ListViewEx.ListViewHeader("", 50),
                new ListViewEx.ListViewHeader("Fruits", 100),
                new ListViewEx.ListViewHeader("Color", 100)
            });
            sp.Children.Add(lv);

            ObservableCollectionEx<ListViewEx.ListViewItem> Items = new ObservableCollectionEx<ListViewEx.ListViewItem>();
            Items.Add(lv.CreateListViewItems(true, new List<string>() { "", "Apple", "Red" }));
            Items.Add(lv.CreateListViewItems(false, new List<string>() { "", "Banana", "Yellow" }));
            Items.Add(lv.CreateListViewItems(true, new List<string>() { "", "Orange", "Orange" }));
            lv.ItemsSource = Items;

            lv.PropertyChanged += ViewControl_PropertyChanged;
            lv.PropertyChanging += ViewControl_PropertyChanging;
        }

        private void ViewControl_PropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            Console.WriteLine("Changing");
        }

        private void ViewControl_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Console.WriteLine("Changed");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            lv.ItemsSource[0].Checked.Value = !lv.ItemsSource[0].Checked.Value;
            lv.ItemsSource[0].SubItems[1].Value = "Strawberry";
        }

        //private List<ListViewHeader> Headers = new List<ListViewHeader>();

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            //Headers.Add(new ListViewHeader("A", 100.0));
            //lv.Headers = new ReadOnlyCollection<ListViewHeader>(Headers);

            lv.ItemsSource.Add(lv.CreateListViewItems(false, new List<string>() { "", "Kiwi", "Green" }));
        }
    }
}
