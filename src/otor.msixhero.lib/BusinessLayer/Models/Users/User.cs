﻿using System;

namespace otor.msixhero.lib.BusinessLayer.Models.Users
{
    [Serializable]
    public class User
    {
        public User(string name)
        {
            Name = name;
        }

        public User()
        {
        }

        public string Name { get; set; }
    }
}