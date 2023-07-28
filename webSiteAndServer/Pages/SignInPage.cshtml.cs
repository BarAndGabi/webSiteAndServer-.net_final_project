using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webSiteAndServer.Data;
using webSiteAndServer.Model;

namespace webSiteAndServer.Pages
{
    public class SignInPageModel : PageModel
    {
        private readonly Connect4Context connect4Context;

        [BindProperty]
        public UserViewModel SignUpRequest { get; set; }

        public SignInPageModel(Connect4Context connect4Context)
        {
            this.connect4Context = connect4Context;
        }
        public void OnGet()
        {
        }


        public IActionResult OnPost()
        {
           

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (connect4Context.users.Any(p => p.PlayerId == SignUpRequest.PlayerId))
            {
                ModelState.AddModelError("Player.PlayerId", "Player ID already exists.");
                return Page();
            }
            try
            {
                var user = new User
                {
                    PlayerId = SignUpRequest.PlayerId,
                    FirstName = SignUpRequest.FirstName,
                    PhoneNumber = SignUpRequest.PhoneNumber,
                    Country = SignUpRequest.Country
                };

                connect4Context.users.Add(user);
                connect4Context.SaveChanges();



            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error occurred while saving to the database.");
                return Page();
            }
            return RedirectToPage("Index");
        }
    }
}
