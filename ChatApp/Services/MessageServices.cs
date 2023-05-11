using ChatApp.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ChatApp.Services
{
    public class MessageServices
    {
        private readonly IMongoCollection<MessageModel> _UserColleciton;
        public MessageServices(IOptions<DbModel> ChatAppSetting)
        {
            var mongoClient = new MongoClient(
                ChatAppSetting.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                ChatAppSetting.Value.DatabaseName);

            _UserColleciton = mongoDatabase.GetCollection<MessageModel>(
                ChatAppSetting.Value.MessageCollectionName);
        }
        public async Task<List<MessageModel>> GetAsync() =>
        await _UserColleciton.Find(_ => true).ToListAsync();

        public async Task<MessageModel?> GetAsync(string id) =>
            await _UserColleciton.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(MessageModel newUser) =>
            await _UserColleciton.InsertOneAsync(newUser);

        public async Task UpdateAsync(string id, MessageModel updatedUser) =>
            await _UserColleciton.ReplaceOneAsync(x => x.Id == id, updatedUser);

        public async Task RemoveAsync(string id) =>
            await _UserColleciton.DeleteOneAsync(x => x.Id == id);
    }
}
