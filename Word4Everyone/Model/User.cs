using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Word4Everyone.Model
{
    public class User : IdentityUser// <int>
    {
         public virtual ICollection<Document> Documents { get; set; }
    }
}
