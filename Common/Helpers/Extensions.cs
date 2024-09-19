using Highgeek.McWebApp.Common.Services.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Highgeek.McWebApp.Common.Helpers
{
    public static class Extensions
    {
        public static bool IsNullOrEmpty<T>( this IList<T> List ) {
            return ( List == null || List.Count < 1 );
        }

        public static bool IsNullOrEmpty<TKey, TValue>( this IDictionary<TKey, TValue> Dictionary ) {
            return ( Dictionary == null || Dictionary.Count < 1 );
        }

        
        /// <summary>
        /// Determines whether the collection is null or contains no elements.
        /// </summary>
        /// <typeparam name="T">The IEnumerable type.</typeparam>
        /// <param name="enumerable">The enumerable, which may be null or empty.</param>
        /// <returns>
        ///     <c>true</c> if the IEnumerable is null or empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                return true;
            }
            /* If this is a list, use the Count property. 
            * The Count property is O(1) while IEnumerable.Count() is O(N). */
            var collection = enumerable as ICollection<T>;
            if (collection != null)
            {
                return collection.Count < 1;
            }
            return !enumerable.Any();
        }

        /// <summary>
        /// Determines whether the collection is null or contains no elements.
        /// </summary>
        /// <typeparam name="T">The IEnumerable type.</typeparam>
        /// <param name="collection">The collection, which may be null or empty.</param>
        /// <returns>
        ///     <c>true</c> if the IEnumerable is null or empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
        {
            if (collection == null)
            {
                return true;
            }
            return collection.Count < 1;
        }


        public static async void WriteExceptionToRedis(this Exception exception)
        {
            DateTime dateTime = DateTime.UtcNow;

            string date = dateTime.ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFF").Replace(":", "-");
            await RedisService.SetInRedis("exceptions:mcwebapp:" + Environment.GetEnvironmentVariable("HIGHGEEK_APPNAME") + ":" + date, exception.ToJson());
        }
    }
}