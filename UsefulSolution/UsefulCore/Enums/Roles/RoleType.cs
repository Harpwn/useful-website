namespace UsefulCore.Enums.Roles
{
    public enum RoleType
    {
        Standard = 0,
        Administrator = 1,
        SuperAdministrator = 2
    }

    public static class RoleTypeExtensions
    {
        public static string RoleTypeIconClass(this RoleType type) =>
            type switch
            {
                RoleType.SuperAdministrator => "is-primary",
                RoleType.Administrator => "is-success",
                _ => string.Empty,
            };
        
    }
}
