using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace StatisticalAnalysis.WpfClient.Controls
{
    public class IndexedItemsControl : ItemsControl
    {
        public static int GetIndex(DependencyObject obj)
        {
            return (int)obj.GetValue(IndexProperty);
        }
        public static void SetIndex(DependencyObject obj, int value)
        {
            obj.SetValue(IndexProperty, value);
        }
        /// <summary>
        /// Keeps track of the index of a ListBoxItem
        /// </summary>
        public static readonly DependencyProperty IndexProperty =
            DependencyProperty.RegisterAttached("Index", typeof(int), typeof(IndexedItemsControl), new UIPropertyMetadata(0));

        public static bool GetIsLast(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsLastProperty);
        }
        public static void SetIsLast(DependencyObject obj, bool value)
        {
            obj.SetValue(IsLastProperty, value);
        }
        /// <summary>
        /// Informs if a ListBoxItem is the last in the collection.
        /// </summary>
        public static readonly DependencyProperty IsLastProperty =
            DependencyProperty.RegisterAttached("IsLast", typeof(bool), typeof(IndexedItemsControl), new UIPropertyMetadata(false));

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            this.ReindexItems();
            
            base.OnItemsChanged(e);
        }

        void ItemsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.ReindexItems();
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            // We set the index and other related properties when generating a ItemContainer
            var index = this.Items.IndexOf(item);
            SetIsLast(element, index == this.Items.Count - 1);
            SetIndex(element, index);

            base.PrepareContainerForItemOverride(element, item);
        }

        private void ReindexItems()
        {
            // If the collection is modified, it may be necessary to reindex all ListBoxItems.
            foreach (var item in this.Items)
            {
                var itemContainer = this.ItemContainerGenerator.ContainerFromItem(item);
                if (itemContainer == null) continue;

                int index = this.Items.IndexOf(item);
                SetIsLast(itemContainer, index == this.Items.Count - 1);
                SetIndex(itemContainer, index);
            }
        }
    }
}
