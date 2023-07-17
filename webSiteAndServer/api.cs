using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
namespace webSiteAndServer
{
	[Route("/[controller]")]
	[ApiController]
	public class api : ControllerBase
	{
		

		[Route("GetServerTurn")]
		[HttpPost]
		public int GetServerTurn([FromBody] int[,] matrix)
		{
			// Check for free slots in each column
			int[] freeSlots = new int[7];
			int freeSlotCount = 0;
			for (int col = 0; col < 7; col++)
			{
				for (int row = 0; row < 6; row++)
				{
					if (matrix[row, col] == 0)
					{
						freeSlots[freeSlotCount] = col;
						freeSlotCount++;
						break; // Only consider the first free slot in each column
					}
				}
			}

			if (freeSlotCount > 0)
			{
				// Randomly select a column with free slots
				Random random = new Random();
				int randomCol = freeSlots[random.Next(0, freeSlotCount)];
				return randomCol;
			}
			else
			{
				// No free slots available
				return 1;
			}
		}

	}
}
