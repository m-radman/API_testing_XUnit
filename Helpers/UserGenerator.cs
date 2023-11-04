﻿using Bogus;
using GoRest_L9.Models;

namespace GoRest_L9.Helpers
{
    public static class UserGenerator
    {
        public static User InstantiateUser()
        {
            Faker faker = new Faker();

            string userName = faker.Name.FullName();
            string userEmail = faker.Internet.ExampleEmail();

            string gender = getGender();
            string status = getStatus();

            User newUser = new(null, userName, userEmail, gender, status);
            return newUser;
        }

        private static string getGender()
        {
            Random rnd = new Random();

            int option = rnd.Next(1, 3);
            string gender;

            if (option == 1)
            {
                return gender = "male";
            }

            return gender = "female";
        }

        private static string getStatus()
        {
            Random rnd = new Random();

            int option = rnd.Next(1, 3);
            string status;

            if (option == 1)
            {
                return status = "active"; ;
            }

            return status = "inactive";
        }
    }
}