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

                string typeName = Regex.Replace(
                    input: GetNestedName(type),
                    pattern: @"`(\d+)",
                    evaluator: string (Match match) => '<' + new string(',', int.Parse(match.Groups[1].Value) - 1) + '>');

                if (!type.IsGenericTypeDefinition)
                {
                    var genericArguments = new Queue<Type>(type.GenericTypeArguments);

                    typeName = Regex.Replace(
                        input: typeName,
                        pattern: @"(?<=<,*)(?=,*>)",
                        evaluator: string (Match match) => GetFriendlyName(genericArguments.Dequeue()));
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
