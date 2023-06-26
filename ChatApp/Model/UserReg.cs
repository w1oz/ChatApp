using MongoDB.Bson.Serialization.Attributes;

namespace ChatApp.Model
{
    public class UserReg
    {

        public string FirstName { get; set; }

       
        public string LastName { get; set; }

     
        public string UserName { get; set; }

   
        public string Password { get; set; }
    }
}
