namespace ThreadboxApi.Application.Identity.Permissions
{
    public class PostsPermissions : IPermissionSet
    {
        private const string Prefix = "Posts";

        public const string Delete = $"{Prefix}.Delete";
    }
}