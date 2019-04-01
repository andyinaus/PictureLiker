using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PictureLiker.DAL;
using PictureLiker.Factories;

namespace PictureLiker.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void AddAdminUserIfNotExist(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
                var factory = scope.ServiceProvider.GetService<IEntityFactory>();

                if (unitOfWork.UseRepository.FirstOrDefault(
                        u => u.Role.EqualsIgnoreCase(Authentication.RoleTypes.Administrator)) == null)
                {
                    var adminUser = factory.GetUser()
                        .SetName("Admin")
                        .SetEmail("admin@gmail.com").Result
                        .SetRole(Authentication.RoleTypes.Administrator);

                    unitOfWork.UseRepository.Add(adminUser);
                    unitOfWork.Save();
                }
            }
        }
    }
}