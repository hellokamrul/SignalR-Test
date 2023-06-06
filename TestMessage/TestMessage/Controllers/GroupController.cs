using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TestMessage.Data;
using TestMessage.Models;

namespace TestMessage.Controllers
{
    public class GroupController : Controller
    {
        private readonly AppDbContext dbContext;

        public GroupController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IActionResult> Create()
        {

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Groups group)
        {
            //var dt = new Groups
            //{
            //    Id = group.Id,
            //    Name = group.Name,
            //    Users = { 1 ,2 }
            //};
            if (ModelState.IsValid)
            {
                dbContext.Groups.Add(group);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            return View(group);
        }


        [HttpGet]
        public async Task<IActionResult> AddUser(int groupId)
        {
            var group = await dbContext.Groups.FindAsync(groupId);
            if (group == null)
            {
                return NotFound();
            }

            var users = await dbContext.Users.ToListAsync();
            ViewBag.GroupId = groupId;
            ViewBag.Users = users;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(int groupId, int userId)
        {
            var group = await dbContext.Groups.FindAsync(groupId);
            var user = await dbContext.Users.FindAsync(userId);

            if (group == null || user == null)
            {
                return NotFound();
            }

            group.Users.Add(user);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }

}
