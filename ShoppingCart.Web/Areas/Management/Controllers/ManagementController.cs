 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingCart.Web.Areas.Management.Controllers
{
    [Area("Management")]
    [Authorize(policy:"view-management")]
    abstract public class ManagementController : Controller
    {

    }
}

