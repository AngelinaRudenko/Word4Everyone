﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Word4Everyone.Model
{
    public class User //: IdentityUser<int>
    {
        [Key]
        public /*new*/ int Id { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        [DataType(DataType.EmailAddress)]
        public /*override*/ string Email { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public ConfirmationStatus Status { get; set; }
    }

    public enum ConfirmationStatus
    {
        Submitted,
        Approved,
        Rejected
    }
}
