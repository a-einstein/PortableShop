﻿using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace RCS.PortableShop.Common.Controls
{
    public partial class ClearableEntry : CustomContentView
    {
        public ClearableEntry()
        {
            InitializeComponent();

            ClearCommand = new Command(Clear);
        }

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(ClearableEntry));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set
            {
                SetValue(TextProperty, value);
                RaisePropertyChanged(nameof(Text));
            }
        }

        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(ClearableEntry));

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }


        public static readonly BindableProperty ClearCommandProperty =
            BindableProperty.Create(nameof(ClearCommand), typeof(ICommand), typeof(ClearableEntry));

        public ICommand ClearCommand
        {
            get { return (ICommand)GetValue(ClearCommandProperty); }
            set
            {
                SetValue(ClearCommandProperty, value);
                RaisePropertyChanged(nameof(ClearCommand));
            }
        }

        private void Clear()
        {
            Text = string.Empty;
        }

        public IList<Behavior> EntryBehaviors
        {
            get { return entry.Behaviors; }
            set
            {
                entry.Behaviors.Clear();

                foreach (var behavior in value)
                {
                    entry.Behaviors.Add(behavior);
                }
            }
        }
    }
}
