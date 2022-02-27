using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntegraControlsXL.Common
{
    /// <summary>
    /// Provides conversion of enumerations decorated with this type converter to show <see cref="DescriptionAttribute"/> decorated values.
    /// </summary>
    /// <remarks><i>If a value has no <see cref="DescriptionAttribute"/> the name of the value is used.</i></remarks>
    public class DescriptionConverter : EnumConverter
    {
        /// <summary>
        /// Creates a new description converter instance for the specified type.
        /// </summary>
        /// <param name="type">The type to associate with the converter.</param>
        public DescriptionConverter(Type type) : base(type) { }

        /// <summary>
        /// Converts the <paramref name="value"/> to the specified <paramref name="destinationType"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                if (value != null)
                {
                    FieldInfo? fieldInfo = value.GetType().GetField(value.ToString());

                    if (fieldInfo != null)
                    {
                        var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

                        return ((attributes.Length > 0) && (!string.IsNullOrEmpty(attributes[0].Description))) ? attributes[0].Description : value.ToString();
                    }
                }

                return string.Empty;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
