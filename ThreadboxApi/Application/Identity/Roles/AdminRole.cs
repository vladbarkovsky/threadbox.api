using ThreadboxApi.Application.Identity.Permissions;

namespace ThreadboxApi.Application.Identity.Roles
{
    public abstract class AdminRole : IRole
    {
        public const string Name = "Admin";

        public static HashSet<string> Permissions { get; } = new()
        {
            BoardsPermissions.Manage
        };
    }
}