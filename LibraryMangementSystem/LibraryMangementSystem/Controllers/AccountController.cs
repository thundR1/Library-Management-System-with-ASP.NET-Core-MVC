using LibraryMangementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LibraryMangementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly LibraryContext _context;
        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, LibraryContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)    // , string returnUrl
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email!, model.Password!, false, false);

                if (result.Succeeded)
                {
                    //if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    //{
                    //    return LocalRedirect(returnUrl);
                    //}
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid login attempt.");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password!);

                if (result.Succeeded)
                {
                    var member = new Member { Email= model.Email!, IdentityUserId = user.Id };
                    _context.Members.Add(member);
                    await _context.SaveChangesAsync();
                    
                    await _userManager.AddToRoleAsync(user, "Member");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> UploadProfilePicture(ProfilePictureViewModel model)
        {
            if (model.ProfilePicture != null)
            {
                var allowedTypes = new[] { "image/jpeg", "image/png" };

                if (!allowedTypes.Contains(model.ProfilePicture.ContentType))
                {
                    TempData["ProfilePictureError"] = "Only JPG and PNG images are allowed.";
                    return RedirectToAction("Index", "MemberProfile");
                }
                using (var memoryStream = new MemoryStream())
                {
                    await model.ProfilePicture.CopyToAsync(memoryStream);

                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var member = await _context.Members.FirstOrDefaultAsync(usr => usr.IdentityUserId == userId);
                    if (member != null)
                    {
                        member.ProfilePicture = memoryStream.ToArray();
                        await _context.SaveChangesAsync();
                    }
                }
                return RedirectToAction("Index", "MemberProfile");
            }
            TempData["NoPicError"] = "No image provided.";
            return RedirectToAction("Index", "MemberProfile");
        }
    }
}
