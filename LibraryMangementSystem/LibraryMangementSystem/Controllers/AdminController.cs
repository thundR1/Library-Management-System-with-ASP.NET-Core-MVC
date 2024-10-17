using LibraryMangementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly LibraryContext _context;

    public AdminController(UserManager<IdentityUser> userManager, LibraryContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<IActionResult> Dashboard(int? page)
    {
        int PageSize = 10;
        int PageNumber = (page ?? 1);
        int totalItems = _userManager.Users.Count();
        var users = _userManager.Users.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList();
        var userRolesViewModel = new List<UserRolesViewModel>();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userRolesViewModel.Add(new UserRolesViewModel
            {
                UserId = user.Id,
                Email = user.Email!,
                Roles = roles
            });
        }
        int totalPages = (int)Math.Ceiling((double)totalItems / PageSize);
        ViewBag.TotalPages = totalPages;
        ViewBag.CurrentPage = PageNumber;
        return View(userRolesViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> PromoteToAdmin(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            var member = _context.Members.FirstOrDefault(m => m.IdentityUserId == user.Id);
            if (member != null)
            {
                _context.Members.Remove(member);
            }
            var librarian = new Librarian
            {
                Email = member!.Email,
                IdentityUserId = user.Id,
                FirstName = member.FirstName,
                LastName = member.LastName,
                Phone = member.Phone
            };
            _context.Librarians.Add(librarian);
            await _userManager.AddToRoleAsync(user, "Librarian");
            await _userManager.RemoveFromRoleAsync(user, "Member");
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Dashboard");
    }

    [HttpPost]
    public async Task<IActionResult> DemoteToMember(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            var librarian = _context.Librarians.FirstOrDefault(l => l.IdentityUserId == user.Id);
            if (librarian != null)
            {
                _context.Librarians.Remove(librarian);
            }
            var member = new Member
            {
                FirstName = librarian!.FirstName,
                LastName = librarian!.LastName,
                Email = librarian!.Email,
                Phone = librarian!.Phone,
                IdentityUserId = user.Id
            };
            _context.Members.Add(member);
            await _userManager.RemoveFromRoleAsync(user, "Librarian");
            await _userManager.AddToRoleAsync(user, "Member");
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Dashboard");
    }

    public async Task<IActionResult> SearchByEmail(string email, int? page)
    {
        int PageSize = 10;
        int PageNumber = (page ?? 1);
        var users = _userManager.Users;
        if (email != null)
        {
            users = users.Where(b => b.Email.Contains(email));
        }
        int totalItems = users.Count();
        var item = users.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList();
        var userRolesViewModel = new List<UserRolesViewModel>();
        foreach (var user in item)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userRolesViewModel.Add(new UserRolesViewModel
            {
                UserId = user.Id,
                Email = user.Email!,
                Roles = roles
            });
        }
        int totalPages = (int)Math.Ceiling((double)totalItems / PageSize);
        ViewBag.TotalPages = totalPages;
        ViewBag.CurrentPage = PageNumber;
        ViewBag.Search = email;
        ViewBag.CurrentAction = "SearchByEmail";
        return View("Dashboard", userRolesViewModel);
    }
}
