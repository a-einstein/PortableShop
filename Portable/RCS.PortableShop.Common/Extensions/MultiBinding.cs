using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace RCS.PortableShop.Common.Extensions
{
    // Based on https://gist.github.com/Keboo.
    // Thanks to Kevin.

    // WARNING: This MultiBinding implementation only works when it is directly applied to its target property.
    // It will fail if used inside of a setter (such is the case when used within a trigger or style).

    [ContentProperty(nameof(Bindings))]
    public class MultiBinding : IMarkupExtension<Binding>
    {
        #region Partially emulate Binding
        public string StringFormat { get; set; }

        public IMultiValueConverter Converter { get; set; }

        private object ConverterParameter { get; set; }
        #endregion

        #region IMarkupExtension
        public IList<Binding> Bindings { get; } = new List<Binding>();

        private BindableObject targetObject;
        private readonly IList<BindableProperty> bindableProperties = new List<BindableProperty>();

        public Binding ProvideValue(IServiceProvider serviceProvider)
        {
            // This class is intended for conversion or formatting or both.
            if (string.IsNullOrWhiteSpace(StringFormat) && Converter == null)
                throw new InvalidOperationException($"{nameof(MultiBinding)} requires a {nameof(Converter)} or {nameof(StringFormat)}");

            //Get the object that the markup extension is being applied to
            var provideValueTarget = (IProvideValueTarget)serviceProvider?.GetService(typeof(IProvideValueTarget));
            targetObject = provideValueTarget?.TargetObject as BindableObject;

            if (targetObject == null) return null;

            foreach (var binding in Bindings)
            {
                var bindableProperty = BindableProperty.Create
                (
                    $"Property-{Guid.NewGuid():N}",
                    typeof(object),
                    typeof(MultiBinding),
                    default,
                    propertyChanged: (b, o, n) => SetInternalValue()
                );

                bindableProperties.Add(bindableProperty);
                targetObject.SetBinding(bindableProperty, binding);
            }

            SetInternalValue();

            var result = new Binding
            {
                Path = nameof(InternalValue.Value),
                Converter = new MultiValueConverterWrapper(Converter, StringFormat),
                ConverterParameter = ConverterParameter,
                Source = internalValue
            };

            return result;
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return ProvideValue(serviceProvider);
        }

        #region InternalValue
        private readonly InternalValue internalValue = new InternalValue();

        private void SetInternalValue()
        {
            if (targetObject != null)
                internalValue.Value = bindableProperties.Select(targetObject.GetValue).ToArray();
        }

        private sealed class InternalValue : INotifyPropertyChanged
        {
            private object value;
            public object Value
            {
                get => value;
                set
                {
                    if (!Equals(this.value, value))
                    {
                        this.value = value;
                        OnPropertyChanged();
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        private sealed class MultiValueConverterWrapper : IValueConverter
        {
            private readonly IMultiValueConverter converter;
            private readonly string format;

            public MultiValueConverterWrapper(IMultiValueConverter multiValueConverter, string stringFormat)
            {
                converter = multiValueConverter;
                format = stringFormat;
            }

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var result = value;

                if (converter != null)
                    // Convert.
                    // Note the converter is responsible for its own type checking and may return any object.
                    result = converter.Convert(result as object[], targetType, parameter, culture);

                if (!string.IsNullOrWhiteSpace(format))
                {
                    // Determine type again.
                    var resultArray = result as object[];

                    // Format.
                    // Distinguish formatting on type.
                    result = resultArray != null
                        ? string.Format(format, resultArray)
                        : string.Format(format, result);
                }

                return result;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
        #endregion
    }
}
