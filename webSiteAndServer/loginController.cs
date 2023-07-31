using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace webSiteAndServer
{
    [Route("api/loginController")]
    [ApiController]
    public class loginController : ControllerBase
    {
        private readonly Data.Connect4Context db;
        public loginController(Data.Connect4Context db)
        {
            this.db = db;
        }
        // POST: api/login
        [HttpPost("login")]
        public IActionResult Post([FromBody] LoginRequestModel model)
        {
            // You can handle the received data here.
            // For this example, we'll just return -1 as a response.

            // If you want to use the player ID and player name received in the request, you can access them like this:
            int playerId = model.PlayerId;
            string playerName;
            int id;
            if (model.PlayerName != null)
            {
                playerName = model.PlayerName;
                id = this.checkValidLogin(playerId, playerName);

            }
            else
                return BadRequest(-1);
            // For this example, we'll just return -1 as the response.
            return Ok(id);
        }

        private int checkValidLogin(int id,string name)
        {
            //check if user exist in the db 
            var user = db.users.FirstOrDefault(u => u.PlayerId == id && u.FirstName == name);
            if (user != null)
            {
                return user.PlayerId; // Return the user's ID if found
            }
            else
            {
                return -1; // Return -1 to indicate user not found
            }


        }
    }

    // Create a model class to represent the login request data.
    public class LoginRequestModel
    {
        public int PlayerId { get; set; }
        public string? PlayerName { get; set; }
    }
}
