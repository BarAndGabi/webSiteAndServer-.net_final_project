using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webSiteAndServer.Data;
using webSiteAndServer.Model;

namespace webSiteAndServer.Pages
{
    public class GameManagementModel : PageModel
    {
        private readonly Connect4Context connect4Context;

        public GameManagementModel(Connect4Context context)
        {
            connect4Context = context;
        }

        [BindProperty]
        public List<Game> Games { get; set; }

        public void OnGet()
        {
            Games = connect4Context.Games.ToList();
        }

        public IActionResult OnPost(string action, string gameId)
        {
            var gameToDelete = connect4Context.Games.FirstOrDefault(g => g.Id == gameId);

            if (gameToDelete != null)
            {
                if (action == "delete")
                {
                    connect4Context.Games.Remove(gameToDelete);
                    connect4Context.SaveChanges();
                }
            }

            return RedirectToPage();
        }
    }
}
