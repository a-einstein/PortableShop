using System;
using System.Collections;
using Xamarin.Forms;

namespace RCS.PortableShop.Common.Controls
{
    // Thanks to https://gist.github.com/Sankra/286cfbdd5dfd379a9155
    public class BindablePicker : Picker
    {
        public BindablePicker()
        {
            SelectedIndexChanged += OnSelectedIndexChanged;
        }

        public static BindableProperty ItemsSourceProperty =
            BindableProperty.Create<BindablePicker, IList>(o => o.ItemsSource, default(IList), propertyChanged: OnItemsSourceChanged);

        public static BindableProperty SelectedItemProperty =
            BindableProperty.Create<BindablePicker, object>(o => o.SelectedItem, default(object), propertyChanged: OnSelectedItemChanged);

        public IList ItemsSource
        {
            get { return (IList)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        private static void OnItemsSourceChanged(BindableObject bindable, IEnumerable oldValue, IEnumerable newValue)
        {
            var bindablePicker = bindable as BindablePicker;
            bindablePicker.Items.Clear();

            if (newValue != null)
            {
                //now it works like "subscribe once" but you can improve
                foreach (var item in newValue)
                {
                    bindablePicker.Items.Add(item.ToString());
                }
            }
        }

        private void OnSelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            if (SelectedIndex < 0 || SelectedIndex > Items.Count - 1)
            {
                SelectedItem = null;
            }
            else
            {
                // Correction thanks to https://forums.xamarin.com/discussion/70172/bindable-picker-control-not-updating-bound-property
                SelectedItem = ItemsSource[SelectedIndex];
            }
        }

        private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var bindablePicker = bindable as BindablePicker;

            if (newValue != null)
            {
                bindablePicker.SelectedIndex = bindablePicker.Items.IndexOf(newValue.ToString());
            }
        }
    }
}
