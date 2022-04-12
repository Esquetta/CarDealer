using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealer.Entities
{
    public class User:IEntity
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
    }
}
