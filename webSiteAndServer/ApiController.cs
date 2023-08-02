
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
                int colSize = matrix.GetLength(1);
                 int nextCol = this.GetNextCol(matrix);

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
                    return Ok(nextCol);
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

        private int GetNextCol(int[,] matrix)
        {
            // Check if there is a winning move for the server
            int nextCol = FindWinningMove(matrix, 2);
            if (nextCol != -1)
            {
                return nextCol;
            }

            // Check if opponent (player 1) is one turn away from winning
            int opponentWinningMove = FindWinningMove(matrix, 1);
            if (opponentWinningMove != -1)
            {
                return opponentWinningMove; // Block opponent's winning move
            }

            // No immediate winning move for either player, use the minimax algorithm to find the best move
            return FindBestMove(matrix);
        }

        public int FindBestMove(int[,] board)
        {
            int depth = 6; // Desired depth for the search tree (adjust as needed)
            int bestMove = -1;
            int maxValue = int.MinValue;

            for (int col = 0; col < 7; col++)
            {
                if (IsValidMove(board, col))
                {
                    int[,] tempBoard = CopyBoard(board);
                    MakeMove(tempBoard, col, 2); // Make a hypothetical move for the server

                    int moveValue = Minimax(tempBoard, depth - 1, false, int.MinValue, int.MaxValue);

                    if (moveValue > maxValue)
                    {
                        maxValue = moveValue;
                        bestMove = col;
                    }
                }
            }

            return bestMove;
        }
        // Helper function to find the opponent's (player 1) winning move
        private int FindWinningMove(int[,] board, int player)
        {
            for (int col = 0; col < 7; col++)
            {
                if (IsValidMove(board, col))
                {
                    int[,] tempBoard = CopyBoard(board);
                    MakeMove(tempBoard, col, player);
                    if (IsTerminalNode(tempBoard))
                    {
                        return col;
                    }
                }
            }
            return -1; // No winning move found
        }

        private int Minimax(int[,] board, int depth, bool maximizingPlayer, int alpha, int beta)
        {
            if (depth == 0 || IsTerminalNode(board))
            {
                return Evaluate(board);
            }

            if (maximizingPlayer)
            {
                int value = int.MinValue;
                for (int col = 0; col < 7; col++)
                {
                    if (IsValidMove(board, col))
                    {
                        int[,] tempBoard = CopyBoard(board);
                        MakeMove(tempBoard, col, 2);
                        value = Math.Max(value, Minimax(tempBoard, depth - 1, false, alpha, beta));
                        alpha = Math.Max(alpha, value);
                        if (beta <= alpha)
                        {
                            break; // Beta cutoff
                        }
                    }
                }
                return value;
            }
            else
            {
                int value = int.MaxValue;
                for (int col = 0; col < 7; col++)
                {
                    if (IsValidMove(board, col))
                    {
                        int[,] tempBoard = CopyBoard(board);
                        MakeMove(tempBoard, col, 1); // Assume player (1) makes optimal moves
                        value = Math.Min(value, Minimax(tempBoard, depth - 1, true, alpha, beta));
                        beta = Math.Min(beta, value);
                        if (beta <= alpha)
                        {
                            break; // Alpha cutoff
                        }
                    }
                }
                return value;
            }
        }


        private int[,] CopyBoard(int[,] board)
        {
            int[,] copy = new int[6, 7];
            Array.Copy(board, copy, board.Length);
            return copy;
        }

        // Check if a move is valid in the given column
        private bool IsValidMove(int[,] board, int column)
        {
            // Check if the column is within bounds
            if (column < 0 || column >= 7)
            {
                return false;
            }

            // Check if the top row in the column is empty (valid move)
            return board[0, column] == 0;
        }

        // Make a move on the board
        private void MakeMove(int[,] board, int column, int player)
        {
            for (int row = 5; row >= 0; row--)
            {
                if (board[row, column] == 0)
                {
                    board[row, column] = player;
                    break;
                }
            }
        }

        // Check if the current board state is a terminal node (win, loss, or draw)
        private bool IsTerminalNode(int[,] board)
        {
            // Check for a win in rows
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    int player = board[row, col];
                    if (player != 0 && board[row, col + 1] == player && board[row, col + 2] == player && board[row, col + 3] == player)
                    {
                        return true; // Win in a row
                    }
                }
            }

            // Check for a win in columns
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    int player = board[row, col];
                    if (player != 0 && board[row + 1, col] == player && board[row + 2, col] == player && board[row + 3, col] == player)
                    {
                        return true; // Win in a column
                    }
                }
            }

            // Check for a win in diagonals (top-left to bottom-right)
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    int player = board[row, col];
                    if (player != 0 && board[row + 1, col + 1] == player && board[row + 2, col + 2] == player && board[row + 3, col + 3] == player)
                    {
                        return true; // Win in a diagonal (top-left to bottom-right)
                    }
                }
            }

            // Check for a win in diagonals (top-right to bottom-left)
            for (int row = 0; row < 3; row++)
            {
                for (int col = 3; col < 7; col++)
                {
                    int player = board[row, col];
                    if (player != 0 && board[row + 1, col - 1] == player && board[row + 2, col - 2] == player && board[row + 3, col - 3] == player)
                    {
                        return true; // Win in a diagonal (top-right to bottom-left)
                    }
                }
            }

            // Check for a draw (all cells filled)
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    if (board[row, col] == 0)
                    {
                        return false; // Empty cell found, game still in progress
                    }
                }
            }

            return true; // All cells filled, game is a draw
        }

        // Evaluate the board state and assign a value based on heuristics
        private int Evaluate(int[,] board)
        {
            int player1Score = 0;
            int player2Score = 0;

            // Evaluate rows
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    int player = board[row, col];
                    if (player == 1)
                    {
                        player1Score++;
                    }
                    else if (player == 2)
                    {
                        player2Score++;
                    }
                }
            }

            // Evaluate columns
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    int player = board[row, col];
                    if (player == 1)
                    {
                        player1Score++;
                    }
                    else if (player == 2)
                    {
                        player2Score++;
                    }
                }
            }

            // Evaluate diagonals (top-left to bottom-right)
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    int player = board[row, col];
                    if (player == 1)
                    {
                        player1Score++;
                    }
                    else if (player == 2)
                    {
                        player2Score++;
                    }
                }
            }

            // Evaluate diagonals (top-right to bottom-left)
            for (int row = 0; row < 3; row++)
            {
                for (int col = 3; col < 7; col++)
                {
                    int player = board[row, col];
                    if (player == 1)
                    {
                        player1Score++;
                    }
                    else if (player == 2)
                    {
                        player2Score++;
                    }
                }
            }

            // Calculate the final evaluation score (player1Score - player2Score)
            return player1Score - player2Score;
        }


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
