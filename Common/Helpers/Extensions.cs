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
    }
}