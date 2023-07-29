using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using webSiteAndServer.Data;
using webSiteAndServer.Model;

namespace webSiteAndServer.Pages
{
    public class SignInPageModel : PageModel
    {
        private readonly Connect4Context connect4Context;

        [BindProperty]
        [Required(ErrorMessage="FirstName is required")]
        public string FirstName { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "ID is required")]
        public int PlayerId { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "PhoneNumber is required")]
        public string Phone { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }

        public string succesMessage = "";
        public string errorMessage = "";
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

            if (connect4Context.users.Any(p => p.PlayerId == PlayerId))
            {
                ModelState.AddModelError("Player.PlayerId", "Player ID already exists.");
                return Page();
            }
            try
            {
                var user = new User
                {
                    PlayerId = PlayerId,
                    FirstName = FirstName,
                    PhoneNumber = Phone,
                    Country = Country
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
