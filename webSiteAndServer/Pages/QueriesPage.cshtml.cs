using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using webSiteAndServer.Data;
using webSiteAndServer.Model;

namespace webSiteAndServer.Pages
{
    public class QueriesPageModel : PageModel
    {
        private readonly Connect4Context connect4Context;
        public List<User>? Users { get; set; }
        public List<SelectListItem> UsersCombo { get; set; }
        public List<SelectListItem> CountryCombo { get; set; }

        public DataTable? QueryResult { get; set; }
        public int SelectedUserId { get; set; }
        public string SelectedCountry { get; set; }

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
                    this.Query1();
                    break;
                case "query2":
                    ViewData["SelectedQuery"] = 2;
                    this.Query2();
                      break;
                case "query3":
                    ViewData["SelectedQuery"] = 3;
                    this.Query3();
                    break;
                case "query4":
                    return Page();
                case "query5":
                    ViewData["SelectedQuery"] = 5;
                     this.Query5(); 
                    
                    break;
                case "query6":
                    ViewData["SelectedQuery"] = 6;
                    this.Query6();
                    break;
                case "query7":
                    ViewData["SelectedQuery"] =7;
                    this.Query7();
                    break;
                case "query8":
                    ViewData["SelectedQuery"] = 8;
                    this.Query8();
                    break;  

                // Add cases for other queries
                default:
                    break;
            }

                return Page();
        }

        

        private void Query7()
        {
            var playerGamesCount = connect4Context.Games
                .GroupBy(game => game.PlayerId)
                .Select(group => new
                {
                    PlayerId = group.Key,
                    GamesPlayed = group.Count()
                })
                .ToList();

            var playersPlayed3OrMore = playerGamesCount
                .Where(player => player.GamesPlayed >= 3)
                .ToList();

            var playersPlayed2 = playerGamesCount
                .Where(player => player.GamesPlayed == 2)
                .ToList();

            var playersPlayed1 = playerGamesCount
                .Where(player => player.GamesPlayed == 1)
                .ToList();

            var playersDidntPlayYet = playerGamesCount
                .Where(player => player.GamesPlayed == 0)
                .ToList();

            var queryResultTable = new DataTable();
            queryResultTable.TableName = "Players with 3 or More Games Played";
            queryResultTable.Columns.Add("PlayerId", typeof(int));
            queryResultTable.Columns.Add("GamesPlayed", typeof(int));

            foreach (var playerGame in playersPlayed3OrMore)
            {
                queryResultTable.Rows.Add(playerGame.PlayerId, playerGame.GamesPlayed);
            }

            var playersPlayed2Table = new DataTable();
            playersPlayed2Table.TableName = "Players with 2 Games Played";
            playersPlayed2Table.Columns.Add("PlayerId", typeof(int));
            playersPlayed2Table.Columns.Add("GamesPlayed", typeof(int));
            foreach (var playerGame in playersPlayed2)
            {
                playersPlayed2Table.Rows.Add(playerGame.PlayerId, playerGame.GamesPlayed);
            }

            var playersPlayed1Table = new DataTable();
            playersPlayed1Table.TableName = "Players with 1 Game Played";
            playersPlayed1Table.Columns.Add("PlayerId", typeof(int));
            playersPlayed1Table.Columns.Add("GamesPlayed", typeof(int));
            foreach (var playerGame in playersPlayed1)
            {
                playersPlayed1Table.Rows.Add(playerGame.PlayerId, playerGame.GamesPlayed);
            }

            var playersDidntPlayYetTable = new DataTable();
            playersDidntPlayYetTable.TableName = "Players Who Didn't Play Yet";
            playersDidntPlayYetTable.Columns.Add("PlayerId", typeof(int));
            playersDidntPlayYetTable.Columns.Add("GamesPlayed", typeof(int));
            foreach (var playerGame in playersDidntPlayYet)
            {
                playersDidntPlayYetTable.Rows.Add(playerGame.PlayerId, playerGame.GamesPlayed);
            }

            ViewData["QueryResult"] = queryResultTable;
            ViewData["PlayersPlayed2"] = playersPlayed2Table;
            ViewData["PlayersPlayed1"] = playersPlayed1Table;
            ViewData["PlayersDidntPlayYet"] = playersDidntPlayYetTable;
        }



        private void Query6()
        {
                var playerGamesCount = connect4Context.Games
                    .GroupBy(game => game.PlayerId)
                    .Select(group => new
                    {
                        PlayerId = group.Key,
                        GamesPlayed = group.Count()
                    })
                    .ToList();

                QueryResult = new DataTable();
                QueryResult.Columns.Add("PlayerId", typeof(int));
                QueryResult.Columns.Add("GamesPlayed", typeof(int));

                foreach (var playerGame in playerGamesCount)
                {
                    QueryResult.Rows.Add(playerGame.PlayerId, playerGame.GamesPlayed);
                }

                ViewData["QueryResult"] = QueryResult;
            
        }


        private void Query4()
        {
            throw new NotImplementedException();
        }

        private void Query1()
        {
            Users = this.connect4Context.users.ToList();

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

            var users = this.connect4Context.users.ToList();

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
                else
                {
                    // If the user doesn't exist in the "users" list, add the user with null start time.
                    QueryResult.Rows.Add("Unknown User", null);
                }
            }

            // Now, find users who haven't played any games and add them to the DataTable.
            var playedUserIds = userGames.Select(ug => ug.PlayerId).ToList();
            var unplayedUsers = users.Where(u => !playedUserIds.Contains(u.PlayerId));
            foreach (var unplayedUser in unplayedUsers)
            {
                QueryResult.Rows.Add(unplayedUser.FirstName,null);
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
        private void Query5()
        {
    // Populate UsersCombo directly in the model
        UsersCombo = connect4Context.users
        .Select(user => new SelectListItem
        {
            Value = user.PlayerId.ToString(),
            Text = user.FirstName
        })
        .Distinct()
        .ToList();

            ViewData["UserCombo"] = UsersCombo;


            if (int.TryParse(Request.Form["selectedUserId"], out int selectedUserId))
            {
                var userGames = connect4Context.Games
                    .Where(game => game.PlayerId == selectedUserId)
                    .ToList();

                QueryResult = new DataTable();
                QueryResult.Columns.Add("GameId", typeof(string));
                QueryResult.Columns.Add("PlayerId", typeof(int));
                QueryResult.Columns.Add("PlayerWon", typeof(bool));
                QueryResult.Columns.Add("GameFinished", typeof(bool));
                QueryResult.Columns.Add("StartTime", typeof(DateTime));
                QueryResult.Columns.Add("TimePlayedSeconds", typeof(int));

                foreach (var game in userGames)
                {
                    QueryResult.Rows.Add(game.Id, game.PlayerId, game.PlayerWon, game.GameFinished, game.StartTime, game.TimePlayedSeconds);
                }

                ViewData["QueryResult"] = QueryResult;
            }


        }
        private void Query8()
        {
            //combo box of countries from db
            this.CountryCombo = connect4Context.users
                .Select(user => new SelectListItem
                {
                    Value = user.Country,
                    Text = user.Country
                }
                )
                .Distinct()
                .ToList();
            
            ViewData["countryCombo"] = this.CountryCombo;
            //table of users with selected Country from db
            if (Request.Form.ContainsKey("SelectedCountry"))
                this.SubmitCountries();

        }
        private void SubmitCountries()
        {
            var selectedCountry = Request.Form["selectedCountry"];
            var users = connect4Context.users
                .Where(user => user.Country == selectedCountry)
                .ToList();

            QueryResult = new DataTable();
            QueryResult.Columns.Add("PlayerId", typeof(int));
            QueryResult.Columns.Add("Firstname", typeof(string));
            QueryResult.Columns.Add("PhoneNumber", typeof(string));

            foreach (var user in users)
            {
                QueryResult.Rows.Add(user.PlayerId, user.FirstName, user.PhoneNumber);
            }

            ViewData["QueryResult"] = QueryResult;
            
         
        }



    }

}
