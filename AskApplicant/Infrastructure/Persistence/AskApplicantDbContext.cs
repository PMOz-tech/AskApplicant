using AskApplicant.Core.Entities;
using MongoDB.Driver;

namespace AskApplicant.Infrastructure.Persistence
{
    public class AskApplicantDbContext
    {
        private readonly IMongoDatabase _database;

        public AskApplicantDbContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<ProgramInfo> ProgramInfos => _database.GetCollection<ProgramInfo>("ProgramInfos");
        public IMongoCollection<ContactInformation> ContactInformations => _database.GetCollection<ContactInformation>("ContactInformations");
        public IMongoCollection<Question> Questions => _database.GetCollection<Question>("Questions");
        public IMongoCollection<MultiChoice> MultiChoices => _database.GetCollection<MultiChoice>("MultiChoices");
        public IMongoCollection<ApplicantAnswer> ApplicantAnswers => _database.GetCollection<ApplicantAnswer>("ApplicantAnswers");

    }
}
