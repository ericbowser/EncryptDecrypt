using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using EncryptDecrypt.Models;
using System.Text.Json;
using System.Security.Cryptography;

namespace EncryptDecrypt
{
    public interface IStoreStuffRepository
    {
        Task SaveKey(string userName, byte[] encryptedPassword, string publicKey, string privateKey);
        Task<KeyObject> GetSavedKey(string userName);
    }

    internal class StoreStuffRespoitory : IStoreStuffRepository
    {
        private readonly IConfiguration _config;


        public StoreStuffRespoitory()
        {

        }

        public async Task Save(string user, byte[] password, string pubKey, string privKey)
        {
             await SaveKey(user, password, pubKey, privKey);
        }

        public async Task<KeyObject> GetSavedKey(string userName)
        {
            var sql = @"SELECT * FROM dbo.KeyObject";
            var config = new ConfigurationManager();
            var conn = config.GetSection("ConnectionStrings");
            var store = conn.GetConnectionString("StoreStuff");

            using (var client = new SqlConnection(store))
            {
                client.Open();
                var keys = await client.QueryAsync<KeyObject>(sql);
                var theKey = keys.FirstOrDefault(x => x.UserName == userName);
                return theKey;
            }
        }

        public async Task SaveKey(string userName, byte[] encryptedPassword, string publicKey, string privateKey)
        {
            try
            {
                var config = new ConfigurationManager();
                var conn = config.GetSection("ConnectionStrings");
                var store = conn.GetConnectionString("StoreStuff");
                using (var client = new SqlConnection(store))
                {
                    client.Open();
                    var parameters = new DynamicParameters();
                    parameters.Add("@UserName", userName, DbType.String, ParameterDirection.Input);
                    parameters.Add("@EncryptedPassword", encryptedPassword, DbType.Binary, ParameterDirection.Input);
                    parameters.Add("@PublicKey", publicKey, DbType.String, ParameterDirection.Input);
                    parameters.Add("@PrivateKey", privateKey, DbType.String, ParameterDirection.Input);
                    parameters.Add("@output_parameter", 0, DbType.Int32, ParameterDirection.Input);

                    await client.ExecuteAsync("dbo.InsertKey", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
