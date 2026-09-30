using System.Reflection;
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
            return roleType.GetProperty("Permissions").GetValue(obj: null) as HashSet<string>;
        }

        public static HashSet<string> GetRolePermissions(string roleName)
        {
            Type roleType = GetRoleTypes().Where(bool (Type type) => GetRoleName(type) == roleName).Single();
            return GetRolePermissions(roleType);
        }

        public static IEnumerable<Type> GetRoleTypes()
        {
            return Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(bool (Type type) => type.IsAssignableTo(typeof(IRole)) && type.IsClass);
        }
    }
}
