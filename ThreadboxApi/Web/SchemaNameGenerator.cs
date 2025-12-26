using NJsonSchema.Generation;
using System.Text.RegularExpressions;

namespace ThreadboxApi.Web
{
    public class SchemaNameGenerator : DefaultSchemaNameGenerator, ISchemaNameGenerator
    {
        public override string Generate(Type type)
        {
            return GetFriendlyName(type).Replace(".", string.Empty);
        }

        private string GetFriendlyName(Type type)
        {
            try
            {
                if (type.IsArray)
                {
                    return GetFriendlyName(type.GetElementType()) +
                        '[' +
                        new string(',', type.GetArrayRank() - 1) +
                        ']';
                }

                if (type.IsPointer)
                {
                    return GetFriendlyName(type.GetElementType()) + '*';
                }

                if (type.IsGenericType)
                {
                    if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        return GetFriendlyName(Nullable.GetUnderlyingType(type)) + '?';
                    }

                    var typeName = GetNestedName(type);

                    typeName = Regex.Replace(
                        typeName,
                        @"`(\d+)",
                        x => '<' + new string(',', int.Parse(x.Groups[1].Value) - 1) + '>');

                    if (!type.IsGenericTypeDefinition)
                    {
                        var genericArguments = new Queue<Type>(type.GenericTypeArguments);

                        typeName = Regex.Replace(
                            typeName,
                            @"(?<=<,*)(?=,*>)",
                            _ => GetFriendlyName(genericArguments.Dequeue()));
                    }

                    return typeName;
                }

                if (type.IsPrimitive)
                {
                    return type switch
                    {
                        Type x when x == typeof(char) => "char",
                        Type x when x == typeof(byte) => "byte",
                        Type x when x == typeof(sbyte) => "sbyte",
                        Type x when x == typeof(short) => "short",
                        Type x when x == typeof(ushort) => "ushort",
                        Type x when x == typeof(int) => "int",
                        Type x when x == typeof(uint) => "uint",
                        Type x when x == typeof(long) => "long",
                        Type x when x == typeof(ulong) => "ulong",
                        _ => throw new NotImplementedException(),
                    };
                }

                return type switch
                {
                    Type x when x == typeof(string) => "string",
                    Type x when x == typeof(void) => "void",
                    _ => GetNestedName(type)
                };
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

        private string GetNestedName(Type type)
        {
            string typeName = type.Name;

            Type declaringType = type.DeclaringType;

            while (declaringType != null)
            {
                typeName = declaringType.Name + '.' + typeName;
                declaringType = declaringType.DeclaringType;
            }

            return typeName;
        }
    }
}