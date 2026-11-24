namespace ThreadboxApi.Application.Identity.Permissions
{
    public class SectionPermissions : IPermissionSet
    {
        private const string Prefix = "Section";

        public const string Manage = $"{Prefix}.Manage";
    }
}