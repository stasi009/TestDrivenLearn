using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigEditor
{
    static class Helpers
    {
        public static string RegexEncode(this char item)
        {
            return "\\u" + Convert.ToUInt16(item).ToString("x").PadLeft(4, '0');
        }

        public static Dictionary<string, string> ParseKeyValuePairs(this string value, char parameterDelimeter = ';', char keyValueDelimeter = '=', char startValueDelimeter = '{', char endValueDelimeter = '}', bool ignoreDuplicateKeys = true)
        {
            if (value == (string)null)
                throw new ArgumentNullException("value");

            if (parameterDelimeter == keyValueDelimeter ||
                parameterDelimeter == startValueDelimeter ||
                parameterDelimeter == endValueDelimeter ||
                keyValueDelimeter == startValueDelimeter ||
                keyValueDelimeter == endValueDelimeter ||
                startValueDelimeter == endValueDelimeter)
                throw new ArgumentException("All delimeters must be unique");

            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
            StringBuilder escapedValue = new StringBuilder();
            string escapedParameterDelimeter = parameterDelimeter.RegexEncode();
            string escapedKeyValueDelimeter = keyValueDelimeter.RegexEncode();
            string escapedStartValueDelimeter = startValueDelimeter.RegexEncode();
            string escapedEndValueDelimeter = endValueDelimeter.RegexEncode();
            string[] elements;
            string key, unescapedValue;
            bool valueEscaped = false;
            int delimeterDepth = 0;
            char character;

            // Escape any parameter or key/value delimeters within tagged value sequences
            //      For example, the following string:
            //          "normalKVP=-1; nestedKVP={p1=true; p2=false}")
            //      would be encoded as:
            //          "normalKVP=-1; nestedKVP=p1\\u003dtrue\\u003b p2\\u003dfalse")
            for (int x = 0; x < value.Length; x++)
            {
                character = value[x];

                if (character == startValueDelimeter)
                {
                    if (!valueEscaped)
                    {
                        valueEscaped = true;
                        continue;   // Don't add tag start delimeter to final value
                    }
                    else
                    {
                        // Handle nested delimeters
                        delimeterDepth++;
                    }
                }

                if (character == endValueDelimeter)
                {
                    if (valueEscaped)
                    {
                        if (delimeterDepth > 0)
                        {
                            // Handle nested delimeters
                            delimeterDepth--;
                        }
                        else
                        {
                            valueEscaped = false;
                            continue;   // Don't add tag stop delimeter to final value
                        }
                    }
                    else
                    {
                        throw new FormatException(string.Format("Failed to parse key/value pairs: invalid delimeter mismatch. Encountered end value delimeter \'{0}\' before start value delimeter \'{1}\'.", endValueDelimeter, startValueDelimeter));
                    }
                }

                if (valueEscaped)
                {
                    // Escape any delimeter characters inside nested key/value pair
                    if (character == parameterDelimeter)
                        escapedValue.Append(escapedParameterDelimeter);
                    else if (character == keyValueDelimeter)
                        escapedValue.Append(escapedKeyValueDelimeter);
                    else if (character == startValueDelimeter)
                        escapedValue.Append(escapedStartValueDelimeter);
                    else if (character == endValueDelimeter)
                        escapedValue.Append(escapedEndValueDelimeter);
                    else
                        escapedValue.Append(character);
                }
                else
                {
                    escapedValue.Append(character);
                }
            }

            if (delimeterDepth != 0 || valueEscaped)
            {
                // If value is still escaped, tagged expression was not terminated
                if (valueEscaped)
                    delimeterDepth = 1;

                throw new FormatException(string.Format("Failed to parse key/value pairs: invalid delimeter mismatch. Encountered more {0} than {1}.", delimeterDepth > 0 ? "start value delimeters \'" + startValueDelimeter + "\'" : "end value delimeters \'" + endValueDelimeter + "\'", delimeterDepth < 0 ? "start value delimeters \'" + startValueDelimeter + "\'" : "end value delimeters \'" + endValueDelimeter + "\'"));
            }

            // Parse key/value pairs from escaped value
            foreach (string parameter in escapedValue.ToString().Split(parameterDelimeter))
            {
                // Parse out parameter's key/value elements
                elements = parameter.Split(keyValueDelimeter);

                if (elements.Length == 2)
                {
                    // Get key expression
                    key = elements[0].ToString().Trim();

                    // Get unescaped value expression
                    unescapedValue = elements[1].ToString().Trim().
                        Replace(escapedParameterDelimeter, parameterDelimeter.ToString()).
                        Replace(escapedKeyValueDelimeter, keyValueDelimeter.ToString()).
                        Replace(escapedStartValueDelimeter, startValueDelimeter.ToString()).
                        Replace(escapedEndValueDelimeter, endValueDelimeter.ToString());

                    // Add key/value pair to dictionary
                    if (ignoreDuplicateKeys)
                    {
                        // Add or replace key elements with unescaped value
                        keyValuePairs[key] = unescapedValue;
                    }
                    else
                    {
                        // Add key elements with unescaped value throwing an exception for encountered duplicate keys
                        if (keyValuePairs.ContainsKey(key))
                            throw new ArgumentException(string.Format("Failed to parse key/value pairs: duplicate key encountered. Key \"{0}\" is not unique within the string: \"{1}\"", key, value));

                        keyValuePairs.Add(key, unescapedValue);
                    }
                }
            }

            return keyValuePairs;
        }
    }
}
