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
        [Range(1, 1000, ErrorMessage = "The ID must be between 1-1000")]
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
            this.FirstName = "";
            this.PlayerId = -1;
            this.Country = "";
            this.Phone="";
            this.AddHardcodedUsersToDB();
            this.AddHardcodedGamesToDB();


        }

        private void AddHardcodedUsersToDB()
        {
            // Check if there are any users in the database
            if (connect4Context.users.Any())
            {
                return; // If there are users, do not add hardcoded users again
            }

            // Create three hardcoded user records
            var user1 = new User
            {
                PlayerId = 1,
                FirstName = "a",
                PhoneNumber = "1234567890",
                Country = "CountryA"
            };

            var user2 = new User
            {
                PlayerId = 2,
                FirstName = "b",
                PhoneNumber = "9876543210",
                Country = "CountryB"
            };

            var user3 = new User
            {
                PlayerId = 3,
                FirstName = "c",
                PhoneNumber = "5555555555",
                Country = "CountryC"
            };

            // Add the users to the database using the Connect4Context
            connect4Context.users.AddRange(user1, user2, user3);
            connect4Context.SaveChanges();
        }
        //make 10 random games for each player 
        public void AddHardcodedGamesToDB()
        {
            //check if there is less then 30 games in the db
            if (connect4Context.Games.Count() < 30)
            {
                //create 30 games
                for (int i = 1; i <= 3; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        Random random = new Random();
                        //random game 
                        var randomGame = new Model.Game
                        {
                            GameFinished = this.randomBool(),
                            Id = this.randomGameID(),
                            PlayerId = i,
                            StartTime = DateTime.Now,
                            TimePlayedSeconds = random.Next(1, 1000),
                            PlayerWon = this.randomBool()
                        };
                        connect4Context.Games.Add(randomGame);
                        connect4Context.SaveChanges();
                    }
                }
            }
        }

        

        private string randomGameID()
        {
           //returns random string in length of 10
           Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 10)
                             .Select(s => s[random.Next(s.Length)]).ToArray());
            
        }

        private bool? randomBool()
        {
            //returns random true or false
            Random random = new Random();
            return random.Next(0, 2) == 1;

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
                ModelState.AddModelError("", "Error occurred while saving to the database.\n"+ex.Message);
                return Page();
            }
            ModelState.Clear();
            return RedirectToPage("Index");
        }
    }
}
