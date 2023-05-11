namespace ChatApp.Model
{
    public class DbModel
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string UsersCollectionName { get; set; } = null!;

        public string MessageCollectionName { get; set; } = null!;
    }
}
