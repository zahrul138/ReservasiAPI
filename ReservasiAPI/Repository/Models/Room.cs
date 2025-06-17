using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservasiAPI.Repository.Models
{
    [Table("rooms")]
    public class Room
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("title")]
        public string Title { get; set; } = null!;

        [Column("shortdescription")]
        public string? ShortDescription { get; set; }

        [Column("fulldescription")]
        public string? FullDescription { get; set; }

        [Required]
        [Column("price")]
        public decimal Price { get; set; }

        [Column("size")]
        public string? Size { get; set; }

        [Column("occupancy")]
        public string? Occupancy { get; set; }

        [Column("bed")]
        public string? Bed { get; set; }

        [Column("roomview")]
        public string? RoomView { get; set; }

        [Column("image1")]
        public string? Image1 { get; set; }

        [Column("image2")]
        public string? Image2 { get; set; }

        [Column("image3")]
        public string? Image3 { get; set; }

        [Column("features")]
        public string? Features { get; set; }

        [Column("amenities")]
        public string? Amenities { get; set; }

        [Column("policies")]
        public string? Policies { get; set; }

        [Required]
        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("createdat")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
