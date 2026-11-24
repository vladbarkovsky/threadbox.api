namespace ThreadboxApi.Application.Identity.Permissions
{
    public class PostPermissions : IPermissionSet
    {
        private const string Prefix = "Post";

        public const string Delete = $"{Prefix}.Delete";
    }
}