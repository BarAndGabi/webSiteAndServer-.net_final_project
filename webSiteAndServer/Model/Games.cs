using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webSiteAndServer.Model
{
    public class Games
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        [Required]
        public int PlayerId { get; set; }
        public bool? PlayerWon { get; set; }
        public bool? GameFinished { get; set; }

        [Required, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public DateTime StartTime { get; set; }
        public int? TimePlayedSeconds { get; set; }

    }
}
