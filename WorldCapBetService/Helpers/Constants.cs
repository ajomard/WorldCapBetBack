namespace WorldCapBetService.Helpers
{
    public static class Constants
    {
        public static class Strings
        {
            public static class JwtClaimIdentifiers
            {
                public const string Rol = "rol", Id = "id", UserName = "username", FirstName = "firstName", LastName = "lastName";
            }

            public static class JwtClaims
            {
                public const string UserAccess = "User";
                public const string AdminAccess = "Admin";
            }
        }
    }
}
