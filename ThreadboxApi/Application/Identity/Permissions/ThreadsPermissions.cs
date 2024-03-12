namespace ThreadboxApi.Application.Identity.Permissions
{
    public class ThreadsPermissions : IPermissionSet
    {
        private const string Prefix = "Permissions";

        public const string Delete = $"{Prefix}.Delete";
    }
}