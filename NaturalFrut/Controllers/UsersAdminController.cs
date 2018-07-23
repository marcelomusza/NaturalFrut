using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using NaturalFrut.Models;

namespace NaturalFrut.Controllers
{

    public class UsersAdminController : Controller
    {

        public UserManager<ApplicationUser> UserManager { get; private set; }
        public RoleManager<IdentityRole> RoleManager { get; private set; }
        public ApplicationDbContext context { get; private set; }


        public UsersAdminController()
        {
            context = new ApplicationDbContext();
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
        }

        public UsersAdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        
        //
        // GET: /Users/
        public ActionResult Index()
        {
           
            var usuarios = UserManager.Users.ToList();
            var userVM = new List<UserViewModel>();

            foreach(var user in usuarios)
            {
                userVM.Add(

                    new UserViewModel
                    {
                        Id = user.Id,
                        Username = user.UserName,
                        Roles = UserManager.GetRoles(user.Id)
                    }
                    
                );
            }


            return View(userVM);

        }
        
        public ActionResult Detalles(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var user = UserManager.FindById(id);

            var userVM = new UserViewModel
            {
                Id = user.Id,
                Username = user.UserName,
                Roles = UserManager.GetRoles(user.Id)
            };


            return View(userVM);
        }

       
        public ActionResult Editar(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            ViewBag.RoleId = new SelectList(RoleManager.Roles, "Id", "Name");

            var user = UserManager.FindById(id);
            if (user == null)
                return HttpNotFound();

            return View(user);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Guardar([Bind(Include = "UserName,Id")] ApplicationUser formuser, string id, string RoleId)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.RoleId = new SelectList(RoleManager.Roles, "Id", "Name");

            var user = UserManager.FindById(id);
            user.UserName = formuser.UserName;

            if (ModelState.IsValid)
            {
                
                UserManager.Update(user);
               
                var rolesForUser = UserManager.GetRoles(id);
                if (rolesForUser.Count() > 0)
                {
                    foreach (var item in rolesForUser)
                    {
                        var result = UserManager.RemoveFromRole(id, item);
                    }
                }

                if (!String.IsNullOrEmpty(RoleId))
                {
                    //Find Role
                    var role = RoleManager.FindById(RoleId);
                    //Add user to new role
                    var result = UserManager.AddToRole(id, role.Name);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", result.Errors.First().ToString());
                        ViewBag.RoleId = new SelectList(RoleManager.Roles, "Id", "Name");
                        return View();
                    }
                }
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.RoleId = new SelectList(RoleManager.Roles, "Id", "Name");
                return View();
            }
        }

       
    }
}
