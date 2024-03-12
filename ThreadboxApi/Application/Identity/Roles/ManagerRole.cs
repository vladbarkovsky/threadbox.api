using ThreadboxApi.Application.Identity.Permissions;

namespace ThreadboxApi.Application.Identity.Roles
{
    public class ManagerRole : IRole
    {
        public const string Name = "Manager";

        public static HashSet<string> Permissions { get; } = new()
        {
            ThreadsPermissions.Delete,
            PostsPermissions.Delete
        };
    }
}