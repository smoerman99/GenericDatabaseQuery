using EntityFramework.Models;
using GenericDatabaseQuery.DAL;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace GenericDatabaseQuery
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
       
            //To make this thing work you first need to make an instance of the Crud class and fill in the asked parameters.                         
            Crud Connection = new Crud("your server.com", "your database", "your username", "your password");

            //Then you can what you want, with data you can do 4 things, create, read, update and delete, in short terms (CRUD)

            //-Create: C-//
            Contact AContact = new Contact();
            AContact.Name = "Steven";
            AContact.Email = "s.something@gmail.com";

            Connection.Create(AContact);
            //---------//

            //-Read: R-//
            List<Tag> ListOfAllTags = Connection.SelectAll<Tag>();

            Tag ATag = new Tag();
            ATag.Name = "you search value";

            List<Tag> ListOfSpecificTags = Connection.SelectWhere(ATag);
            //---------//

            //-Update: U-//
            //The id needs to be set in the update object otherwise the update cant work because the id will be 0.
            Tag UpdateTag = new Tag();
            UpdateTag.Id = 6;            
            UpdateTag.Name = "Dependency Injection";

            Connection.Update<Tag>(UpdateTag);
            //---------//

            //-Delete: D-//
            Tag DeleteTag = new Tag();
            DeleteTag.Id = 4;
            DeleteTag.Name = "Sander Moerman";

            Connection.Delete(ATag);
            //---------//
        }
    }
}
