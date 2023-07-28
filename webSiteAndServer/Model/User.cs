using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace webSiteAndServer.Model
{
	public class User
	{
		[Key]
		public int PlayerId { get; set; }

		[Required]
		public string? FirstName { get; set; }


		public TimeSpan TimeStamp { get; set; } = TimeSpan.Zero;

        [Required]
        public string? PhoneNumber { get; set; }

		[Required]
		public string? Country { get; set; }




    }
}

