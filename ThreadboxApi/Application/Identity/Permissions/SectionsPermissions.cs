namespace ThreadboxApi.Application.Identity.Permissions
{
    public class SectionsPermissions : IPermissionSet
    {
        private const string Prefix = "Sections";

        public const string Manage = $"{Prefix}.Manage";
    }
}