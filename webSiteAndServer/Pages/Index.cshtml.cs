using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace webSiteAndServer.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
        [HttpPost]
        public IActionResult OnPostLogin(string userName, int userId)
        {
            // Here, you can implement any logic you need.
            // For this example, we simply return -1.
            return new JsonResult(-1);
        }

    }
}