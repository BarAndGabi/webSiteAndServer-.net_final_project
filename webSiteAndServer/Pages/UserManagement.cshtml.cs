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
    public string ErrorMessage { get; set; }

    public void OnGet()
    {
        Users = connect4Context.users.ToList();
    }

    public IActionResult OnPost()
    {
        int userId = int.Parse(Request.Form["userId"]);
        string action = Request.Form["action"];

        var user = this.connect4Context.users.FirstOrDefault(u => u.PlayerId == userId);
        string newValue = Request.Form["newValue"];
        if (user != null)
        {
            if (action == "edit")
            {
                ColumnBeingEdited = Request.Form["column"];
                UserIdBeingEdited = userId;

                if (ColumnBeingEdited == "Country")
                {


                    if (!string.IsNullOrEmpty(newValue) && !int.TryParse(newValue, out _))
                    {
                        // Capitalize the first letter of the country name
                        newValue = char.ToUpper(newValue[0]) + newValue.Substring(1);
                        user.Country = newValue;
                        this.connect4Context.SaveChanges();

                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Cant Enter a number";
                        
                    }

                }

                // Handle editing other column
                if (ColumnBeingEdited == "Id")
                {
                    // Validate if the new ID is available
                    int newId = int.Parse(newValue);
                    var existingUser = connect4Context.users.FirstOrDefault(u => u.PlayerId == newId);
                    if (existingUser != null)
                    {
                        TempData["ErrorMessage"] = "Player ID already taken.";

                    }
                    else
                    {
                        user.PlayerId = newId;
                        this.connect4Context.SaveChanges();
                    }
                    
                    
                }
                if (ColumnBeingEdited == "FirstName")
                {
                    if (!string.IsNullOrEmpty(newValue) && !int.TryParse(newValue, out _))
                    {
                        user.FirstName = newValue;
                        this.connect4Context.SaveChanges();
                    }
                    
                }
                if (ColumnBeingEdited == "PhoneNumber")
                {
                    // Remove any non-numeric characters from the input
                    newValue = new string(newValue.Where(char.IsDigit).ToArray());

                    if (newValue.Length == 10)
                    {
                        user.PhoneNumber = newValue;
                        this.connect4Context.SaveChanges();
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Phone number must be exactly 10 digits.";

                    }
                }

               
                return RedirectToPage("UserManagement");
        }
        else if (action == "delete")
        {
            this.connect4Context.users.Remove(user);
            this.connect4Context.SaveChanges();
        }
        }

        return RedirectToPage("UserManagement");
    }
}
