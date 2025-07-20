using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ReservasiAPI.Repository.Models;

[Table("users")]
public partial class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("fullname")]
    [StringLength(100)]
    public string? Fullname { get; set; }

    [Column("email")]
    [StringLength(100)]
    public string? Email { get; set; }

    [Column("password")]
    [StringLength(100)]
    public string? Password { get; set; }

    [Column("role")]
    [StringLength(100)]
    public string? Role { get; set; }

    [Column("createtime", TypeName = "datetime")]
    public DateTime? Createtime { get; set; }
}
