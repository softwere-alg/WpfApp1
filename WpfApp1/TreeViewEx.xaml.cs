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
    public class CheckTreeSource : INotifyPropertyChanged
    {
        private bool _IsExpanded = true;
        private bool? _IsChecked = false;
        private string _Text = "";
        private CheckTreeSource _Parent = null;
        private ObservableCollection<CheckTreeSource> _Children = null;


        public bool IsExpanded
        {
            get { return _IsExpanded; }
            set { _IsExpanded = value; OnPropertyChanged("IsExpanded"); }
        }

        public bool? IsChecked
        {
            get { return _IsChecked; }
            set { _IsChecked = value; OnPropertyChanged("IsChecked"); }
        }

        public string Text
        {
            get { return _Text; }
            set { _Text = value; OnPropertyChanged("Text"); }
        }

        public CheckTreeSource Parent
        {
            get { return _Parent; }
            set { _Parent = value; OnPropertyChanged("Parent"); }
        }

        public ObservableCollection<CheckTreeSource> Children
        {
            get { return _Children; }
            set { _Children = value; OnPropertyChanged("Childen"); }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            if (null == this.PropertyChanged) return;
            this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public void Add(CheckTreeSource child)
        {
            if (null == Children) Children = new ObservableCollection<CheckTreeSource>();
            child.Parent = this;
            Children.Add(child);
        }

        public void UpdateParentStatus()
        {
            if (null != Parent)
            {
                int isCheckedNull = 0;
                int isCheckedOn = 0;
                int isCheckedOff = 0;
                if (null != Parent.Children)
                {
                    foreach (var item in Parent.Children)
                    {
                        if (null == item.IsChecked) isCheckedNull += 1;
                        if (true == item.IsChecked) isCheckedOn += 1;
                        if (false == item.IsChecked) isCheckedOff += 1;
                    }
                }
                if ((0 < isCheckedNull) || (0 < isCheckedOn) || (0 < isCheckedOff))
                {
                    if (0 < isCheckedNull)
                        Parent.IsChecked = null;
                    else if ((0 < isCheckedOn) && (0 < isCheckedOff))
                        Parent.IsChecked = null;
                    else if (0 < isCheckedOn)
                        Parent.IsChecked = true;
                    else
                        Parent.IsChecked = false;
                }
                Parent.UpdateParentStatus();
            }
        }

        public void UpdateChildStatus()
        {
            if (null != IsChecked)
            {
                if (null != Children)
                {
                    foreach (var item in Children)
                    {
                        item.IsChecked = IsChecked;
                        item.UpdateChildStatus();
                    }
                }
            }
        }
    }

    public class TreeViewLineConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
        object parameter, System.Globalization.CultureInfo culture)
        {
            TreeViewItem item = (TreeViewItem)value;
            ItemsControl ic = ItemsControl.ItemsControlFromItemContainer(item);
            return ic.ItemContainerGenerator.IndexFromContainer(item) == ic.Items.Count - 1;
        }

        public object ConvertBack(object value, Type targetType,
        object parameter, System.Globalization.CultureInfo culture)
        {
            return false;
        }
    }

    /// <summary>
    /// TreeViewEx.xaml の相互作用ロジック
    /// </summary>
    public partial class TreeViewEx : UserControl
    {
        public ObservableCollection<CheckTreeSource> TreeRoot
        {
            get; set;
        }

        public TreeViewEx()
        {
            InitializeComponent();

            TreeRoot = new ObservableCollection<CheckTreeSource>();
            var item1 = new CheckTreeSource() { Text = "Item1", IsExpanded = true, IsChecked = false };
            var item11 = new CheckTreeSource() { Text = "Item1-1", IsExpanded = true, IsChecked = false };
            var item12 = new CheckTreeSource() { Text = "Item1-2", IsExpanded = true, IsChecked = false };
            var item2 = new CheckTreeSource() { Text = "Item2", IsExpanded = false, IsChecked = false };
            var item21 = new CheckTreeSource() { Text = "Item2-1", IsExpanded = true, IsChecked = false };
            TreeRoot.Add(item1);
            TreeRoot.Add(item2);
            item1.Add(item11);
            item1.Add(item12);
            item2.Add(item21);

            tv.ItemsSource = TreeRoot;
        }
    }
}
