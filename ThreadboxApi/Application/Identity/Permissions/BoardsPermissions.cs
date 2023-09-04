namespace ThreadboxApi.Application.Identity.Permissions
{
    public class BoardsPermissions : IPermissionProfile
    {
        private const string Prefix = "Boards";

        public const string Read = $"{Prefix}.Read";
    }
}