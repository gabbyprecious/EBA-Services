using System;
using System.Collections.Generic;
using System.Globalization;
using FinquestTest.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

using static System.Net.Mime.MediaTypeNames;

using BC = BCrypt.Net.BCrypt;



namespace FinquestTest.Services;


public class UsersService
{
    private readonly IMongoCollection<User> _usersCollection;

    public UsersService(
        IOptions<UserStoreDatabaseSettings> userStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            userStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            userStoreDatabaseSettings.Value.DatabaseName);

        _usersCollection = mongoDatabase.GetCollection<User>(
            userStoreDatabaseSettings.Value.UsersCollectionName);
    }

    public async Task<List<User>> GetAsync() =>
        await _usersCollection.Find(_ => true).ToListAsync();

    public async Task<List<User>> ListAsync(string firstName = "", string lastName = "", string atleastAConnection = "", string order ="", string orderBy="")
    {
        // Building Filter
        var builder = Builders<User>.Filter;
        var filter = builder.Empty;

        if (!string.IsNullOrWhiteSpace(firstName))
        {
            var firstNameFilter = builder.Eq(x => x.FirstName, firstName);
            filter &= firstNameFilter;
        }

        if (!string.IsNullOrWhiteSpace(lastName))
        {
            var lastNameFilter = builder.Eq(x => x.LastName, lastName);
            filter &= lastNameFilter;
        }

        if(!string.IsNullOrWhiteSpace(atleastAConnection))
        {
            if (atleastAConnection == "yes")
            {
                var connectionFilter = builder.Ne(x => x.LastConnectionDate, null);
                filter &= connectionFilter;
            } else
            {
                var connectionFilter = builder.Eq(x => x.LastConnectionDate, null);
                filter &= connectionFilter;
            }
        }

        //Building Sorter
        SortDefinition<User> sortData;

        if (string.IsNullOrWhiteSpace(orderBy))
        {
            orderBy = "asc";
        }


        if (string.IsNullOrWhiteSpace(order))
        {
            order = "LastName";
        }

            
        if (orderBy == "desc")
        {
            if (order == "CreationDate")
            {
                sortData = Builders<User>.Sort.Descending("Audit.CreationDate");
            }
            else
            {
                sortData = Builders<User>.Sort.Descending(order);
            }
            return await _usersCollection.Find(filter).Sort(sortData).ToListAsync();

        } else
        {
            if (order == "CreationDate")
            {
                sortData = Builders<User>.Sort.Ascending("Audit.CreationDate");
            }
            else
            {
                sortData = Builders<User>.Sort.Ascending(order);
            }
            return await _usersCollection.Find(filter).Sort(sortData).ToListAsync();
        }
    }

    public async Task<User?> GetAsync(string id) =>
        await _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<User?> GetByUsernameAsync(string username) =>
        await _usersCollection.Find(x => x.Username == username).FirstOrDefaultAsync();

    public async Task<User> CreateAsync(User newUser)
    {
        newUser.Password = BC.HashPassword(newUser.Password);
        Audit audit = new Audit(newUser.Username);
        newUser.Audit = audit;
        await _usersCollection.InsertOneAsync(newUser);
        return newUser;
    }

    public async Task UpdateAsync(User user, UpdateModel updatedUser)
    {
        if (!string.IsNullOrEmpty(updatedUser.Password))
            user.Password = BC.HashPassword(updatedUser.Password);

        if (!string.IsNullOrEmpty(updatedUser.FirstName))
            user.FirstName = updatedUser.FirstName;

        if (!string.IsNullOrEmpty(updatedUser.LastName))
            user.LastName = updatedUser.LastName;

        if (!string.IsNullOrEmpty(updatedUser.Username))
            user.Username = updatedUser.Username;

        if (updatedUser.BirthDate != null)
            user.BirthDate = updatedUser.BirthDate;

        user.Audit.UpdatedDate = DateTime.Now;
        user.Audit.UpdatedUsername = updatedUser.Username;

        var filter = Builders<User>.Filter.Eq(x => x.Id, user.Id);
        await _usersCollection.ReplaceOneAsync(filter, user);
    }
        

    public async Task RemoveAsync(string id) =>
        await _usersCollection.DeleteOneAsync(x => x.Id == id);

    public async Task<User?> LoginAsync(string username, string password)
    {
        User user = await _usersCollection.Find(x => x.Username.Equals(username)).FirstOrDefaultAsync();

        if(user == null)
        {
            return null;
        }
        if (BC.Verify(password, user.Password))
        {
            user.LastConnectionDate = DateTime.Now;
            await _usersCollection.ReplaceOneAsync(x => x.Id == user.Id, user);
            return user;
        } else
        {
            return null;
        }
    }
      
}


