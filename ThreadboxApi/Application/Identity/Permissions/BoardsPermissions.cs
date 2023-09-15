namespace ThreadboxApi.Application.Identity.Permissions
{
    public class BoardsPermissions : IPermissionSet
    {
        private const string Prefix = "Boards";

        public const string Read = $"{Prefix}.Read";
        public const string Create = $"{Prefix}.Create";
    }
}