using Azure.Core;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using webSiteAndServer.Data;
using webSiteAndServer.Model;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

public class UserManagementModel : PageModel
{
    private readonly Connect4Context connect4context;

    public UserManagementModel(Connect4Context context)
    {
        this.connect4context = context;
    }

    [BindProperty]
    [Required(ErrorMessage = "New Value is required.")]
    public string NewValue { get; set; }

    public List<User> Users { get; set; }

    public void OnGet()
    {
        Users = this.connect4context.users.ToList();
    }

    public bool ValidateNewValue(string columnName)
    {
        if (columnName == "PlayerId")
        {
            bool playerIdExists = connect4context.users.Any(u => u.PlayerId == int.Parse(NewValue));
            if (playerIdExists)
            {
                return false; // PlayerId already exists
            }
        }
        else if (columnName == "PhoneNumber")
        {
            if (NewValue.Length < 10)
            {
                return false;
            }
        }

        // Add more validation checks for other columns if needed

        return true; // Validation passed
    }

    public IActionResult OnPostEdit(int userId, string column)
    {
        var user = this.connect4context.users.FirstOrDefault(u => u.PlayerId == userId);

        if (user != null)
        {
            if (!ModelState.IsValid)
            {
                Users = this.connect4context.users.ToList();
                return Page();
            }

            if (!ValidateNewValue(column))
            {
                ModelState.AddModelError("NewValue", "Invalid new value.");
                Users = this.connect4context.users.ToList();
                return Page();
            }

            if (column == "PlayerId")
            {
                bool playerIdExists = connect4context.users.Any(u => u.PlayerId == int.Parse(NewValue));
                if (playerIdExists)
                {
                    ModelState.AddModelError("NewValue", "Player ID already exists.");
                    Users = this.connect4context.users.ToList();
                    return Page();
                }
            }
            else if (column == "PhoneNumber")
            {
                if (NewValue.Length < 10)
                {
                    ModelState.AddModelError("NewValue", "Phone Number must have at least 10 digits.");
                    Users = this.connect4context.users.ToList();
                    return Page();
                }
            }

            // Proceed with updating the database
            if (column == "PlayerId")
            {
                user.PlayerId = int.Parse(NewValue);
            }
            // else if other columns...

            connect4context.SaveChanges();
        }

        return RedirectToPage();
    }

    public IActionResult OnPostDelete(int userId)
    {
        var user = this.connect4context.users.FirstOrDefault(u => u.PlayerId == userId);

        if (user != null)
        {
            this.connect4context.users.Remove(user);
            this.connect4context.SaveChanges();
        }

        return RedirectToPage();
    }
}
