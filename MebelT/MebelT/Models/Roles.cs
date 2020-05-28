/*using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
namespace MebelT.Models
{
    public class AppDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
 
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
 
            // создаем две роли
            var role1 = new IdentityRole { Name = "admin" };
            var role2 = new IdentityRole { Name = "manager" };
 
            // добавляем роли в бд
            roleManager.Create(role1);
            roleManager.Create(role2);
 
            // создаем пользователей
            var admin = new ApplicationUser { Email = "admin@mail.ru", UserName = "admin@mail.ru" };
            var manager = new ApplicationUser { Email = "manager@mail.ru", UserName = "manager@mail.ru" };
            string passwordA = "adM_1n";
            string passwordM = "mAnag_3r!";
            var resultA = userManager.Create(admin, passwordA);
            var resultM = userManager.Create(manager, passwordM);
 
            // если создание пользователя прошло успешно
            if(resultA.Succeeded && resultM.Succeeded)
            {
                // добавляем для пользователя роль
                userManager.AddToRole(admin.Id, role1.Name);
                userManager.AddToRole(manager.Id, role2.Name);
            }
 
            base.Seed(context);
        }
    }
}*/