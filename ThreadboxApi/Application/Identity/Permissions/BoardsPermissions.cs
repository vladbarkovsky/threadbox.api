namespace ThreadboxApi.Application.Identity.Permissions
{
    public class BoardsPermissions : IPermissionSet
    {
        private const string Prefix = "Boards";

        public const string Manage = $"{Prefix}.Manage";
    }
}