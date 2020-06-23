using System.Collections.Generic;

namespace TransferService.Service.Authorization
{
    public class Permissions
    {
        public enum Permission
        {
            DeleteUser,
            ApproveUser,
        }

        public static List<Permission> AdminPermissions { get; } = new List<Permission>() { Permission.DeleteUser, Permission.ApproveUser };
    }
}
