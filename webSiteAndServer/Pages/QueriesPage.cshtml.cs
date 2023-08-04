using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using webSiteAndServer.Data;
using webSiteAndServer.Model;

namespace webSiteAndServer.Pages
{
    public class QueriesPageModel : PageModel
    {
        private readonly Connect4Context connect4Context;
        public List<User>? Users { get; set; } 
        public DataTable? QueryResult { get; set; }

        public QueriesPageModel(Connect4Context connect4Context)
        {
            this.connect4Context = connect4Context;
           
        }


        public void OnGet()
        {
        }

        public IActionResult OnPost(string queryName)
        {
            switch (queryName)
            {
                case "query1":
                    ViewData["SelectedQuery"] = 1;
                    Query1();
                    break;
                case "query2":
                    Query2();
                      break;
                case "querry3":
                    break;
                case "querry4":
                    break;
                case "querry5":
                    break;
                case "querry6":
                    break;
                case "querry7":
                    break;

                // Add cases for other queries
                default:
                    break;
            }

            return Page();
        }

        private void Query1()
        {
            Users = this.connect4Context.Users.ToList();

            QueryResult = new DataTable();
            QueryResult.Columns.Add("PlayerId", typeof(int));
            QueryResult.Columns.Add("Firstname", typeof(string));
            QueryResult.Columns.Add("PhoneNumber", typeof(string));
            QueryResult.Columns.Add("Country", typeof(string));

            foreach (var user in Users)
            {
                QueryResult.Rows.Add(user.PlayerId, user.FirstName, user.PhoneNumber, user.Country);
            }
            ViewData["QueryResult"] = QueryResult;
        }

        private void Query2()
        {
            var userGames = this.connect4Context.Games
                .GroupBy(g => g.PlayerId)
                .Select(group => new
                {
                    PlayerId = group.Key,
                    LatestStartTime = group.Max(g => g.StartTime)
                })
                .OrderBy(item => item.PlayerId)
                .ToList();

            var users = this.connect4Context.Users.ToList();

            QueryResult = new DataTable();
            QueryResult.Columns.Add("Username", typeof(string));
            QueryResult.Columns.Add("LatestGameStartTime", typeof(DateTime));

            foreach (var userGame in userGames)
            {
                var user = users.FirstOrDefault(u => u.PlayerId == userGame.PlayerId);
                if (user != null)
                {
                    QueryResult.Rows.Add(user.FirstName, userGame.LatestStartTime);
                }
            }

            ViewData["QueryResult"] = QueryResult;
            
        }

        private void Query3()
        {
            var games = this.connect4Context.Games.ToList();

            QueryResult = new DataTable();
            QueryResult.Columns.Add("Id", typeof(string));
            QueryResult.Columns.Add("PlayerId", typeof(int));
            QueryResult.Columns.Add("PlayerWon", typeof(bool));
            QueryResult.Columns.Add("GameFinished", typeof(bool));
            QueryResult.Columns.Add("StartTime", typeof(DateTime));
            QueryResult.Columns.Add("TimePlayedSeconds", typeof(int));

            foreach (var game in games)
            {
                QueryResult.Rows.Add(game.Id, game.PlayerId, game.PlayerWon, game.GameFinished, game.StartTime, game.TimePlayedSeconds);
            }

            ViewData["QueryResult"] = QueryResult;
        }

    }

}
