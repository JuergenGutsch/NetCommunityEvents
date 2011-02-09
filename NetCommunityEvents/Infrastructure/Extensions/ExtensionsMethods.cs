using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace NetCommunityEvents.Infrastructure.Extensions
{ /// <summary>
    /// Contains methods that extend the namespace System.
    /// </summary>
    public static class ExtensionsMethods
    {
        /// <summary>
        /// Casts the source string to specified type. If the cast is not possible or fails, the
        /// specified default value is used.
        /// </summary>
        /// <typeparam name="T">The target type.</typeparam>
        /// <param name="value">The source string.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>An instance of the specified type.</returns>
        public static T ToOrDefault<T>(this string value, T defaultValue)
        {
            // Get the default value as return value.
            T returnValue = defaultValue;

            // If there is no source string, break.
            if (value == null)
            {
                return returnValue;
            }

            // Check whether string can be casted to the target type. If not, break.
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            if (!converter.CanConvertFrom(typeof(string)))
            {
                return returnValue;
            }

            // Try to cast to the target type. If this fails, ignore the exception.
            try
            {
                returnValue = (T)converter.ConvertFrom(value);
            }
            catch (Exception)
            {
            }

            // Return to the caller.
            return returnValue;
        }

        /// <summary>
        /// Casts the source string to specified type. If the cast is not possible or fails, the
        /// target type's default value is used.
        /// </summary>
        /// <typeparam name="T">The target type.</typeparam>
        /// <param name="value">The source string.</param>
        /// <returns>An instance of the specified type.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Type parameter can not be derived as it is only used for the return type.")]
        public static T ToOrDefault<T>(this string value)
        {
            // Get the typed value or use the target type's default value.
            return value.ToOrDefault(default(T));
        }

        /// <summary>
        /// Decides whether the specified string is null or empty.
        /// </summary>
        /// <param name="source">The string.</param>
        /// <returns><c>true</c> if the string is null or empty; <c>false</c> otherwise.</returns>
        public static bool IsNullOrEmpty(this string source)
        {
            return String.IsNullOrEmpty(source);
        }

        /// <summary>
        /// Regular expression, which is used to validate an E-Mail address.
        /// </summary>
        public const string MatchEmailPattern = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

        /// <summary>
        /// Checks whether the given string is a valid email address.
        /// </summary>
        /// <param name="source">string that contains an email address.</param>
        /// <returns>True, wenn string is not null and contains a valid email address;
        /// otherwise false.</returns>
        public static bool IsValidEmail(this string source)
        {
            if (!String.IsNullOrEmpty(source))
            {
                return Regex.IsMatch(source, MatchEmailPattern);
            }
            return false;

        }
    }
}