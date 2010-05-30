﻿using System;
namespace Dropthings.Data.Repository
{
    public interface IRoleRepository : IDisposable
    {
        System.Collections.Generic.List<Dropthings.Data.aspnet_Role> GetAllRole();
        Dropthings.Data.aspnet_Role GetRoleByRoleName(string roleName);
    }
}