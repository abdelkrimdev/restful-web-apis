using System;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

using SupaTrupa.WebAPI.Settings;
using SupaTrupa.WebAPI.Shared.Attributes;

namespace SupaTrupa.WebAPI.Shared.MongoDb
{
    /// <summary>
    /// Mongo Service used to create and get collections.
    /// </summary>
    public static class MongoService
    {
        /// <summary>
        /// Retrieves the default connection string from the appsettings.json file.
        /// </summary>
        /// <param name="settings">The MongoDb settings object to use to construct the connection string.</param>
        /// <returns>Returns the default connection string from the appsettings.json file.</returns>
        public static string GetConnectionString(IOptions<MongoSettings> settings)
        {
			if (settings == null)
				throw new ArgumentNullException(nameof(settings));
            
            var mongo = settings.Value;

            return $"mongodb://{mongo.User}:{mongo.Pass}@{mongo.Host}:{mongo.Port}/{mongo.Data}";
        }

        /// <summary>
        /// Creates and returns a MongoCollection from the specified type and connection string.
        /// </summary>
        /// <typeparam name="T">The type to get the collection of.</typeparam>
        /// <param name="connectionString">The connection string to use to get the collection from.</param>
        /// <returns>Returns a MongoCollection from the specified type and connection string.</returns>
        public static IMongoCollection<T> GetCollectionFromConnectionString<T>(string connectionString)
        {
			if (string.IsNullOrEmpty(connectionString))
				throw new ArgumentNullException(nameof(connectionString));
            
            return GetCollectionFromConnectionString<T>(connectionString, GetCollectionName<T>());
        }

        /// <summary>
        /// Creates and returns a MongoCollection from the specified type and connection string.
        /// </summary>
        /// <typeparam name="T">The type to get the collection of.</typeparam>
        /// <param name="connectionString">The connection string to use to get the collection from.</param>
        /// <param name="collectionName">The name of the collection to use.</param>
        /// <returns>Returns a MongoCollection from the specified type and connection string.</returns>
        public static IMongoCollection<T> GetCollectionFromConnectionString<T>(string connectionString, string collectionName)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            if (string.IsNullOrEmpty(collectionName))
                throw new ArgumentNullException(nameof(collectionName));
            
            return GetDatabaseFromUrl(MongoUrl.Create(connectionString)).GetCollection<T>(collectionName);
        }

        /// <summary>
        /// Creates and returns a MongoCollection from the specified type and url.
        /// </summary>
        /// <typeparam name="T">The type to get the collection of.</typeparam>
        /// <param name="url">The url to use to get the collection from.</param>
        /// <returns>Returns a MongoCollection from the specified type and url.</returns>
        public static IMongoCollection<T> GetCollectionFromUrl<T>(MongoUrl url)
        {
            if (url == null)
                throw new ArgumentNullException(nameof(url));

            return GetCollectionFromUrl<T>(url, GetCollectionName<T>());
        }

        /// <summary>
        /// Creates and returns a MongoCollection from the specified type and url.
        /// </summary>
        /// <typeparam name="T">The type to get the collection of.</typeparam>
        /// <param name="url">The url to use to get the collection from.</param>
        /// <param name="collectionName">The name of the collection to use.</param>
        /// <returns>Returns a MongoCollection from the specified type and url.</returns>
        public static IMongoCollection<T> GetCollectionFromUrl<T>(MongoUrl url, string collectionName)
        {
			if (url == null)
				throw new ArgumentNullException(nameof(url));

            if (string.IsNullOrEmpty(collectionName))
				throw new ArgumentNullException(nameof(collectionName));
            
            return GetDatabaseFromUrl(url).GetCollection<T>(collectionName);
        }

        /// <summary>
        /// Creates and returns a MongoDatabase from the specified url.
        /// </summary>
        /// <param name="url">The url to use to get the database from.</param>
        /// <returns>Returns a MongoDatabase from the specified url.</returns>
        public static IMongoDatabase GetDatabaseFromUrl(MongoUrl url)
        {
			if (url == null)
                throw new ArgumentNullException(nameof(url));
            
            return GetClientFromUrl(url).GetDatabase(url.DatabaseName);
        }

        /// <summary>
        /// Creates and returns a MongoClient from the specified url.
        /// </summary>
        /// <param name="url">The url to use to get the client from.</param>
        /// <returns>Returns a MongoClient from the specified url.</returns>
        public static IMongoClient GetClientFromUrl(MongoUrl url)
        {
            if (url == null)
                throw new ArgumentNullException(nameof(url));

			return new MongoClient(url);
        }

        /// <summary>
        /// Determines the collection name for T and assures it is not empty
        /// </summary>
        /// <typeparam name="T">The type to determine the collection name for.</typeparam>
        /// <typeparam name="TKey">The type used for the entity's Id.</typeparam>
        /// <returns>Returns the collection name for T.</returns>
        static string GetCollectionName<T>()
        {
            string collectionName;
            if (!typeof(T).BaseType.Equals(typeof(object)))
            {
                collectionName = GetCollectionNameFromType<T>();
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
        static string GetCollectionNameFromType<T>()
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
                if (typeof(MongoEntity).IsAssignableFrom(typeof(T)))
                {
                    // No attribute found, get name of the basetype
                    collectionName = typeof(T).BaseType.Name;
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
        static string GetCollectioNameFromInterface<T>()
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
