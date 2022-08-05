using System;
using ShoppingCart.Models;
using Microsoft.AspNetCore.Identity;
namespace ShoppingCart.DataAccess.Interfaces
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {

    }
}

