namespace ThreadboxApi.Application.Identity.Permissions
{
    public class ThreadPermissions : IPermissionSet
    {
        private const string Prefix = "Thread";

        public const string Delete = $"{Prefix}.Delete";
    }
}