using ChatApp.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
namespace ChatApp.Services
{
    public class UserServices
    {
        private readonly IMongoCollection<User> _UserColleciton;
        public UserServices(IOptions<DbModel> ChatAppSetting)
        {
            var mongoClient = new MongoClient(
                ChatAppSetting.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                ChatAppSetting.Value.DatabaseName);

            _UserColleciton = mongoDatabase.GetCollection<User>(
                ChatAppSetting.Value.UsersCollectionName);
        }
        public async Task<List<User>> GetAsync() =>
        await _UserColleciton.Find(_ => true).ToListAsync();

        public async Task<User?> GetAsync(string id) =>
            await _UserColleciton.Find(x => x.Id == id).FirstOrDefaultAsync();
        public async Task<User?> GetAccountAsync(string username) =>
            await _UserColleciton.Find(x => x.UserName == username).FirstOrDefaultAsync();
        public async Task CreateAsync(User newUser) =>
            await _UserColleciton.InsertOneAsync(newUser);

        public async Task UpdateAsync(string id, User updatedUser) =>
            await _UserColleciton.ReplaceOneAsync(x => x.Id == id, updatedUser);
        public async Task UpdateToken(string id, string ref_token)
        {
            var user = await _UserColleciton.Find(x => x.Id == id).FirstOrDefaultAsync();
            user.RefreshToken = ref_token;
            await _UserColleciton.ReplaceOneAsync(x => x.Id == id, user);
        }

        public async Task RemoveAsync(string id) =>
            await _UserColleciton.DeleteOneAsync(x => x.Id == id);
    }
}
