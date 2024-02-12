using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    /// <summary>
    /// 任意の型に対するObservableクラス
    /// </summary>
    /// <typeparam name="T">Observeする型</typeparam>
    public class ObservableObject<T> : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void RaisePropertyChanging([CallerMemberName] string propertyName = null)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        private T _value = default;
        public T Value
        {
            get { return _value; }
            set { if (!_value.Equals(value)) { RaisePropertyChanging(); _value = value; RaisePropertyChanged(); } }
        }

        public ObservableObject(T value)
        {
            _value = value;
        }
    }

    /// <summary>
    /// 項目が追加または削除されたとき、またはリスト全体が更新されたときに独自処理を行うObservableCollection
    /// </summary>
    /// <typeparam name="T">コレクションする型</typeparam>
    public class ObservableCollectionEx<T> : ObservableCollection<T>
    {
        public delegate void AddItemDelegate(T item);
        public delegate void RemoveItemDelegate(T item);

        public AddItemDelegate AddItemProcess;
        public RemoveItemDelegate RemoveItemProcess;

        protected override void ClearItems()
        {
            foreach (T item in this)
            {
                RemoveItemProcess?.Invoke(item);
            }

            base.ClearItems();
        }

        protected override void InsertItem(int index, T item)
        {
            AddItemProcess?.Invoke(item);

            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            RemoveItemProcess?.Invoke(this[index]);

            base.RemoveItem(index);
        }

        protected override void SetItem(int index, T item)
        {
            RemoveItemProcess?.Invoke(this[index]);
            AddItemProcess?.Invoke(item);

            base.SetItem(index, item);
        }
    }
}
