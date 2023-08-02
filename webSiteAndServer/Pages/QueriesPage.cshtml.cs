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
            if (queryName == "query1")
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

            return Page();
        }
    }
}
