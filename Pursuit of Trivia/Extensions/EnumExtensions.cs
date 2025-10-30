using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pursuit_of_Trivia.Extensions
{
    public static class EnumExtensions
    {
        public static string GetName(this Enum value)
        {
            // Get the field info for this enum value using reflection
            var field = value.GetType().GetField(value.ToString());

            // Try to get the Description attribute if it exists
            var attribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault() as DescriptionAttribute;

            // If we found a Description attribute, use its value
            // Otherwise, fall back to smart splitting of the enum name
            return attribute?.Description ?? value.ToString().SplitPascalCase();
        }

        private static string SplitPascalCase(this string value)
        {
            // Add a space before any uppercase letter that follows a character
            return string.Concat(value.Select((x, i) => i > 0 && char.IsUpper(x) ? " " + x : x.ToString()));
        }
    }
}
