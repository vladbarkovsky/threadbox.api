namespace ThreadboxApi.Application.Identity.Permissions
{
    public class BoardPermissions : IPermissionSet
    {
        private const string Prefix = "Board";

        public const string Manage = $"{Prefix}.Manage";
    }
}