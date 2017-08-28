using System;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

using SupaTrupa.WebAPI.AppSettings;
using SupaTrupa.WebAPI.Shared.Attributes;
using SupaTrupa.WebAPI.Shared.Contracts;

namespace SupaTrupa.WebAPI.Shared.MongoDb
{
    /// <summary>
    /// Internal miscellaneous utility functions.
    /// </summary>
    public class MongoService
    {
        /// <summary>
        /// Retrieves the default connection string from the appsettings.json file.
        /// </summary>
        /// <param name="settings">The MongoDb settings object to use to construct the connection string.</param>
        /// <returns>Returns the default connection string from the appsettings.json file.</returns>
        public static string GetConnectionString(IOptions<MongoDbSettings> settings)
        {
            var mongo = settings.Value;

            return $"mongodb://{mongo.User}:{mongo.Pass}@{mongo.Host}:{mongo.Port}/{mongo.Data}";
        }

        /// <summary>
        /// Creates and returns a MongoCollection from the specified type and connection string.
        /// </summary>
        /// <typeparam name="T">The type to get the collection of.</typeparam>
        /// <typeparam name="TKey">The type used for the entity's Id.</typeparam>
        /// <param name="connectionString">The connection string to use to get the collection from.</param>
        /// <returns>Returns a MongoCollection from the specified type and connection string.</returns>
        public static IMongoCollection<T> GetCollectionFromConnectionString<T, TKey>(string connectionString)
            where T : IEntity<TKey>
        {
            return MongoService.GetCollectionFromConnectionString<T, TKey>(connectionString, GetCollectionName<T, TKey>());
        }

        /// <summary>
        /// Creates and returns a MongoCollection from the specified type and connection string.
        /// </summary>
        /// <typeparam name="T">The type to get the collection of.</typeparam>
        /// <typeparam name="TKey">The type used for the entity's Id.</typeparam>
        /// <param name="connectionString">The connection string to use to get the collection from.</param>
        /// <param name="collectionName">The name of the collection to use.</param>
        /// <returns>Returns a MongoCollection from the specified type and connection string.</returns>
        public static IMongoCollection<T> GetCollectionFromConnectionString<T, TKey>(string connectionString, string collectionName)
            where T : IEntity<TKey>
        {
            return MongoService
                .GetDatabaseFromUrl(new MongoUrl(connectionString))
                .GetCollection<T>(collectionName);
        }

        /// <summary>
        /// Creates and returns a MongoCollection from the specified type and url.
        /// </summary>
        /// <typeparam name="T">The type to get the collection of.</typeparam>
        /// <typeparam name="TKey">The type used for the entity's Id.</typeparam>
        /// <param name="url">The url to use to get the collection from.</param>
        /// <returns>Returns a MongoCollection from the specified type and url.</returns>
        public static IMongoCollection<T> GetCollectionFromUrl<T, TKey>(MongoUrl url)
            where T : IEntity<TKey>
        {
            return MongoService.GetCollectionFromUrl<T, TKey>(url, GetCollectionName<T, TKey>());
        }

        /// <summary>
        /// Creates and returns a MongoCollection from the specified type and url.
        /// </summary>
        /// <typeparam name="T">The type to get the collection of.</typeparam>
        /// <typeparam name="TKey">The type used for the entity's Id.</typeparam>
        /// <param name="url">The url to use to get the collection from.</param>
        /// <param name="collectionName">The name of the collection to use.</param>
        /// <returns>Returns a MongoCollection from the specified type and url.</returns>
        public static IMongoCollection<T> GetCollectionFromUrl<T, TKey>(MongoUrl url, string collectionName)
            where T : IEntity<TKey>
        {
            return MongoService
                .GetDatabaseFromUrl(url)
                .GetCollection<T>(collectionName);
        }

        /// <summary>
        /// Creates and returns a MongoDatabase from the specified url.
        /// </summary>
        /// <param name="url">The url to use to get the database from.</param>
        /// <returns>Returns a MongoDatabase from the specified url.</returns>
        private static IMongoDatabase GetDatabaseFromUrl(MongoUrl url) => new MongoClient(url).GetDatabase(url.DatabaseName);

        /// <summary>
        /// Determines the collection name for T and assures it is not empty
        /// </summary>
        /// <typeparam name="T">The type to determine the collection name for.</typeparam>
        /// <typeparam name="TKey">The type used for the entity's Id.</typeparam>
        /// <returns>Returns the collection name for T.</returns>
        private static string GetCollectionName<T, TKey>() where T : IEntity<TKey>
        {
            string collectionName;
            if (!typeof(T).BaseType.Equals(typeof(object)))
            {
                collectionName = GetCollectionNameFromType<T, TKey>();
            }
            else
            {
                collectionName = GetCollectioNameFromInterface<T>();
            }

            if (string.IsNullOrEmpty(collectionName))
            {
                throw new ArgumentException("Collection name cannot be empty for this entity");
            }
            return collectionName;
        }

        /// <summary>
        /// Determines the collection name from the specified type.
        /// </summary>
        /// <typeparam name="T">The type to get the collection name from.</typeparam>
        /// <typeparam name="TKey">The type used for the entity's Id.</typeparam>
        /// <returns>Returns the collection name from the specified type.</returns>
        private static string GetCollectionNameFromType<T, TKey>()
        {
            string collectionName;

            // Check to see if the object (inherited from Entity) has a CollectionName attribute
            var att = Attribute.GetCustomAttribute(typeof(T), typeof(CollectionNameAttribute));
            if (att != null)
            {
                // It does! Return the value specified by the CollectionName attribute
                collectionName = ((CollectionNameAttribute)att).Name;
            }
            else
            {
                if (typeof(IEntity<TKey>).IsAssignableFrom(typeof(T)))
                {
                    // No attribute found, get name of the basetype
                    collectionName = typeof(T).GetType().BaseType.Name;
                }
                else
                {
                    collectionName = typeof(T).Name;
                }
            }

            return collectionName;
        }

        /// <summary>
        /// Determines the collection name from the specified type.
        /// </summary>
        /// <typeparam name="T">The type to get the collection name from.</typeparam>
        /// <returns>Returns the collection name from the specified type.</returns>
        private static string GetCollectioNameFromInterface<T>()
        {
            string collectionName;

            // Check to see if the object (inherited from Entity) has a CollectionName attribute
            var att = Attribute.GetCustomAttribute(typeof(T), typeof(CollectionNameAttribute));
            if (att != null)
            {
                // It does! Return the value specified by the CollectionName attribute
                collectionName = ((CollectionNameAttribute)att).Name;
            }
            else
            {
                collectionName = typeof(T).Name;
            }

            return collectionName;
        }
    }
}
