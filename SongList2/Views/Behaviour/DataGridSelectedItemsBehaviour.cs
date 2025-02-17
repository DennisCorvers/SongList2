using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace SongList2.Views.Behaviour
{
    public static class DataGridSelectedItemsBehaviour
    {
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.RegisterAttached(
                "SelectedItems",
                typeof(IList),
                typeof(DataGridSelectedItemsBehaviour),
                new FrameworkPropertyMetadata(null, OnSelectedItemsChanged));

        public static void SetSelectedItems(DependencyObject element, IList value)
        {
            element.SetValue(SelectedItemsProperty, value);
        }

        public static IList GetSelectedItems(DependencyObject element)
        {
            return (IList)element.GetValue(SelectedItemsProperty);
        }

        private static bool _isUpdatingFromCode = false; // Prevent recursion

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DataGrid dataGrid)
            {
                if (e.OldValue is INotifyCollectionChanged oldCollection)
                {
                    oldCollection.CollectionChanged -= OnBoundCollectionChanged;
                }

                if (e.NewValue is INotifyCollectionChanged newCollection)
                {
                    newCollection.CollectionChanged += OnBoundCollectionChanged;
                }

                dataGrid.SelectionChanged += (s, args) => UpdateBoundCollection(dataGrid);
            }
        }

        private static void OnBoundCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // This event is triggered when the bound collection changes.
            // We don't modify DataGrid.SelectedItems here to avoid infinite recursion.
        }

        private static void UpdateBoundCollection(DataGrid dataGrid)
        {
            if (_isUpdatingFromCode) return; // Prevent infinite loops

            _isUpdatingFromCode = true;

            var boundSelectedItems = GetSelectedItems(dataGrid);
            if (boundSelectedItems == null) return;

            boundSelectedItems.Clear();
            foreach (var item in dataGrid.SelectedItems)
            {
                boundSelectedItems.Add(item);
            }

            _isUpdatingFromCode = false;
        }
    }
}
