using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservasiAPI.Repository.Models
{
    [Table("reviews")]
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("bookingid")]
        public int BookingId { get; set; }

        [ForeignKey("BookingId")]
        public Booking? Booking { get; set; }

        [Required]
        [Column("roomid")]
        public int RoomId { get; set; }

        [ForeignKey("RoomId")]
        public Room? Room { get; set; }

        [Required]
        [Column("userid")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Required]
        [Column("rating")]
        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        [Column("title")]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Column("comment")]
        [StringLength(1000)]
        public string Comment { get; set; } = string.Empty;

        [Required]
        [Column("status")]
        public string Status { get; set; } = "pending";

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("moderatedat")]
        public DateTime? ModeratedAt { get; set; }

        [Column("moderatedby")]
        public int? ModeratedById { get; set; }

        [ForeignKey("ModeratedById")]
        public User? ModeratedBy { get; set; }

        [Column("fullname")]
        public string Fullname { get; set; } = string.Empty;

        [Column("roomtype")]
        public string RoomType { get; set; } = string.Empty;
    }
}