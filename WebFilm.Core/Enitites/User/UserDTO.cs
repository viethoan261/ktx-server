﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebFilm.Core.Enitites.User
{
    public class UserDTO
    {
       
        public string username { get; set; }

        public string password { get; set; }

        public string fullname { get; set; }

        public string role { get; set; }
        public string email { get; set; }

        public string phone { get; set; }
    }
}
