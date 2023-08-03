using System.Text.RegularExpressions;

namespace ThreadboxApi.Web.ApiSpecification
{
    public static class SchemaNameGenerationExtensions
    {
        private static string GetNestedName(this Type type)
        {
            string typeName = type.Name;

            if (type.IsNested)
            {
                Type declaringType = type.DeclaringType!;

                while (true)
                {
                    typeName = declaringType.Name + "." + typeName;

                    if (!declaringType.IsNested)
                        break;

                    declaringType = declaringType.DeclaringType!;
                }
            }

            return typeName;
        }

        public static string RegexReplace(this string text, string pattern, MatchEvaluator evaluator, RegexOptions options = RegexOptions.None)
        {
            return Regex.Replace(text, pattern, evaluator, options);
        }

        public static string ToFriendlyString(this Type type, bool includeNamespace = false)
        {
            try
            {
                if (type.IsArray)
                    return type.GetElementType()!.ToFriendlyString(includeNamespace) + "[" + new string(',', type.GetArrayRank() - 1) + "]";
                else if (type.IsPointer)
                {
                    return type.GetElementType()!.ToFriendlyString(includeNamespace) + "*";
                }
                else if (type.IsGenericType)
                {
                    if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
                        return Nullable.GetUnderlyingType(type)!.ToFriendlyString(includeNamespace) + "?";

                    string typeName = type.GetNestedName();

                    typeName = typeName.RegexReplace(@"`(\d+)", x => '<' + new string(',', int.Parse(x.Groups[1].Value) - 1) + '>');

                    if (!type.IsGenericTypeDefinition)
                    {
                        var genericArguments = new Queue<Type>(type.GenericTypeArguments);

                        typeName = typeName.RegexReplace(
                            @"(?<=<,*)(?=,*>)",
                            _ => genericArguments.Dequeue().ToFriendlyString(includeNamespace));
                    }

                    if (includeNamespace && !string.IsNullOrEmpty(type.Namespace))
                    {
                        typeName = type.Namespace + "." + typeName;
                    }

                    return typeName;
                }
                else
                {
                    if (type.IsPrimitive)
                    {
                        if (type == typeof(char))
                            return "char";
                        if (type == typeof(byte))
                            return "byte";
                        if (type == typeof(sbyte))
                            return "sbyte";
                        if (type == typeof(short))
                            return "short";
                        if (type == typeof(ushort))
                            return "ushort";
                        if (type == typeof(int))
                            return "int";
                        if (type == typeof(uint))
                            return "uint";
                        if (type == typeof(long))
                            return "long";
                        if (type == typeof(ulong))
                            return "ulong";
                    }

                    if (type == typeof(string))
                        return "string";

                    if (type == typeof(void))
                        return "void";

                    return type.GetNestedName();
                }
            }
            catch
            {
#if DEBUG
                throw;
#else
                return type.Name;
#endif
            }
        }
    }
}