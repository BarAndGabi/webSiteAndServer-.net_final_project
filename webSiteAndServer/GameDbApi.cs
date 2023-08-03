using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace webSiteAndServer
{
    [Route("api/GameDbApi")]
    [ApiController]
    public class GameDbApi : ControllerBase
    {
        private readonly Data.Connect4Context db;
    

        // POST: api/GameDbApi/writeStartGame
        [HttpPost("writeStartGame")]
        public IActionResult Post([FromBody] StartGameRequestModel gameRequest)
        {
            var game = new Model.Game
            {
                Id = gameRequest.GameId,
                PlayerId = gameRequest.PlayerId,
                StartTime = gameRequest.StartTime,
                GameFinished = false
            };
            try
            {
                db.Games.Add(game);

                db.SaveChanges();
            }
            catch
            {
                return BadRequest(false);
            }

            return Ok(true);
        }

        // POST: api/GameDbApi/writeEndGame
        [HttpPost("writeEndGame")]
        public IActionResult WriteEndGame([FromBody] EndGameRequestModel endGameDTO)
        {
            try
            {
                // Find the game in the database based on the provided gameId
                var game = db.Games.Find(endGameDTO.GameId);

                if (game == null)
                {
                    return NotFound(); // Game with the provided ID not found
                }

                // Update the game properties
                game.PlayerWon = endGameDTO.PlayerWon;
                game.TimePlayedSeconds = endGameDTO.TimeLengthSeconds;
                game.GameFinished = true; // Mark the game as finished

                db.SaveChanges();
            }
            catch (Exception)
            {
                return BadRequest(false);
            }

            return Ok(true);
        }
    }

  
    // Create a model class to represent the start game request data.
    public class StartGameRequestModel
    {
        public int PlayerId { get; set; }
        public string GameId { get; set; }
        //StartTime
        public DateTime StartTime { get; set; }

    }
    // Create a model class to represent the end game request data.
    public class EndGameRequestModel
    {
        public string GameId { get; set; }
        public bool? PlayerWon { get; set; }
        public int? TimeLengthSeconds { get; set; }
    }
}

