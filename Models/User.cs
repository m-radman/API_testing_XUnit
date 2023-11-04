using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoRest_L9.Models
{
    public class User
    {
        public string? id { get; set; }
        public string? name { get; set; }
        public string? email { get; set; }
        public string? gender { get; set; }
        public string? status { get; set; }

        public User(string? id, string? name, string? email, string? gender, string? status)
        {
            this.id = id;
            this.name = name;
            this.email = email;
            this.gender = gender;
            this.status = status;
        }
    }
}
