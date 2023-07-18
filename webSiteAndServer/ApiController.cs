
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;



namespace webSiteAndServer.Controllers
{
    [Route("api/b")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        [HttpPost("GetServerTurn")]
        public ActionResult<int> GetServerTurn([FromBody] string  matrixJSON)
        {
            int[,] matrix = this.JsonToMatrix(matrixJSON);


            try
            {
                int counter = 0;
                int colSize = matrix.GetLength(1);

                //Check if any column has empty cells
                List<int> columnsWithEmptyCells = new List<int>();
                for (int col = 0; col < colSize; col++)
                {
                    if (HasSpaceInCol(matrix, col))
                    {
                        columnsWithEmptyCells.Add(col);
                    }
                }

                if (columnsWithEmptyCells.Count > 0)
                {
                    // Select a random column from the list of columns with empty cells
                    int randomIndex = GetRandomIndex(columnsWithEmptyCells.Count);
                    int selectedColumn = columnsWithEmptyCells[randomIndex];
                    return Ok(selectedColumn);
                }
                else
                {
                    return NotFound("No column with empty cells found.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or return an appropriate error response
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }
        //function to that gets the json and create matrix Board based on it : 
        //FILL HERE - "jsonToMatrix" function

        private int GetRandomIndex(int maxExclusive)
        {
            Random rnd = new Random();
            int index = rnd.Next(0, maxExclusive);
            return index;
        }


        private bool HasSpaceInCol(int[,] matrix, int col)
        {
            int rowSize = matrix.GetLength(0);
            for (int i = 0; i < rowSize; i++)
            {
                if (matrix[i, col] == 0)
                {
                    return true;
                }
            }
            return false;
        }
        private int[,] JsonToMatrix(string json)
        {
            
                // Deserialize the JSON string to a jagged array
                int[][] boardArray = JsonSerializer.Deserialize<int[][]>(json);

                // Get the dimensions of the board
                int numRows = boardArray.Length;
                int numCols = numRows > 0 ? boardArray[0].Length : 0;

                // Create a matrix with the same dimensions as the board
                int[,] matrix = new int[numRows, numCols];

                // Copy the values from the jagged array to the matrix
                for (int i = 0; i < numRows; i++)
                {
                    for (int j = 0; j < numCols; j++)
                    {
                        matrix[i, j] = boardArray[i][j];
                    }
                }

                return matrix;
            
        
        }
    }
}
