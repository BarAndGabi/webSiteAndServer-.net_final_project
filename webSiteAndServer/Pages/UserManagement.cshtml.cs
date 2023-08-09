using Azure.Core;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using webSiteAndServer.Data;
using webSiteAndServer.Model;
using Microsoft.EntityFrameworkCore;

public class UserManagementModel : PageModel
{
    private readonly Connect4Context connect4Context;

    public UserManagementModel(Connect4Context context)
    {
        this.connect4Context = context;
    }
    [BindProperty]
    public string ColumnBeingEdited { get; set; }

    [BindProperty]
    public int UserIdBeingEdited { get; set; }

    public List<User> Users { get; set; }

    public void OnGet()
    {
        Users = connect4Context.users.ToList();
    }

    public IActionResult OnPost()
    {
        int userId = int.Parse(Request.Form["userId"]);
        string action = Request.Form["action"];

        var user = this.connect4Context.users.FirstOrDefault(u => u.PlayerId == userId);

        if (user != null)
        {
            if (action == "edit")
            {
                ColumnBeingEdited = Request.Form["column"];
                UserIdBeingEdited = userId;

                if (ColumnBeingEdited == "Country")
                {
                    // Handle editing country
                    // No need to perform any action here
                }
                else if (ColumnBeingEdited == "OtherColumn")
                {
                    // Handle editing other column
                    string newValue = Request.Form["newValue"];

                    if (ColumnBeingEdited == "PlayerId")
                    {
                        // Validate if the new ID is available
                        int newId = int.Parse(newValue);
                        var existingUser = this.connect4Context.users.FirstOrDefault(u => u.PlayerId == newId);
                        if (existingUser != null)
                        {
                            // ID already taken, handle the error
                            // You might want to show an error message or redirect back to the page
                            return Page();
                        }
                        user.PlayerId = newId;
                    }
                    else if (ColumnBeingEdited == "FirstName")
                    {
                        user.FirstName = newValue;
                    }
                    else if (ColumnBeingEdited == "PhoneNumber")
                    {
                        user.PhoneNumber = newValue;
                    }
                }

                this.connect4Context.SaveChanges();
            }
            else if (action == "editCountry")
            {
                string newCountryValue = Request.Form["newCountryValue"];
                user.Country = newCountryValue;
                this.connect4Context.SaveChanges();
            }
            else if (action == "delete")
            {
                this.connect4Context.users.Remove(user);
                this.connect4Context.SaveChanges();
            }
        }

        return RedirectToPage();
    }
}
