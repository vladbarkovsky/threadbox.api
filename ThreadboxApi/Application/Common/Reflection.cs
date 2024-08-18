using System.Reflection;
using ThreadboxApi.Application.Identity.Permissions;
using ThreadboxApi.Application.Identity.Roles;

namespace ThreadboxApi.Application.Common
{
    public class Reflection
    {
        public static string GetRoleName(Type roleType)
        {
            return roleType.GetField("Name").GetRawConstantValue().ToString();
        }

        public static HashSet<string> GetRolePermissions(Type roleType)
        {
            return roleType.GetProperty("Permissions").GetValue(null) as HashSet<string>;
        }

        public static HashSet<string> GetRolePermissions(string roleName)
        {
            var roleType = GetRoleTypes().Where(x => GetRoleName(x) == roleName).Single();
            return GetRolePermissions(roleType);
        }

        public static IEnumerable<Type> GetRoleTypes()
        {
            return Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.IsAssignableTo(typeof(IRole)) && x.IsClass);
        }

        /// <summary>
        /// Creates TypeScript file with permission constants.
        /// See api-permissions.ts in client repository.
        /// </summary>
        public static void GenerateTypeScriptPermissions()
        {
            using var writer = new StreamWriter($@"..\..\threadbox.front\api-permissions.ts");

            writer.WriteLine(
                "/* tslint:disable */" + "\r\n" +
                "/* eslint-disable */" + "\r\n");

            var permissionSetTypes = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.IsAssignableTo(typeof(IPermissionSet)) && x.IsClass);

            foreach (var type in permissionSetTypes)
            {
                writer.WriteLine($"export class {type.Name} {{");
                var constants = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

                foreach (var constant in constants)
                {
                    var constantName = constant.Name[..1].ToLower() + constant.Name[1..];
                    writer.WriteLine($"  static readonly {constantName}: string = '{constant.GetRawConstantValue()}';");
                }

                writer.WriteLine("}\r\n");
            }
        }
    }
}