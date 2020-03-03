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

        public new static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(IList), typeof(BindablePicker), propertyChanged: OnItemsSourceChanged);

        public new IList ItemsSource
        {
            get => (IList)GetValue(ItemsSourceProperty);
            private set => SetValue(ItemsSourceProperty, value);
        }

        public new static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(BindablePicker), propertyChanged: OnSelectedItemChanged);

        public new object SelectedItem
        {
            get => (object)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var bindablePicker = bindable as BindablePicker;
            bindablePicker?.Items.Clear();

            if (newValue != null)
            {
                //now it works like "subscribe once" but you can improve
                foreach (var item in newValue as IList)
                {
                    bindablePicker?.Items.Add(item.ToString());
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
