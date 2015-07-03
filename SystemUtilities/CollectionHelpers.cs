using System;
using System.Collections.Specialized;
using System.Linq;

namespace biz.dfch.CS.System.Utilities
{
    public class CollectionHelpers
    {
        public static bool CompareNameValueCollections
            (
            NameValueCollection left
            ,
            NameValueCollection right
            ,
            bool exactOrder
            )
        {
            if(null == left || null == right)
            {
                return false;
            }
            var leftKeys = left.AllKeys;
            var rightKeys = right.AllKeys;
            if (leftKeys.Count() != rightKeys.Count())
            {
                return false;
            }
            if (exactOrder)
            {
                return leftKeys
                        .SequenceEqual(rightKeys)
                            && leftKeys.All(key => left[key] == right[key]);
            }
            else
            {
                return leftKeys
                    .OrderBy(key => key)
                        .SequenceEqual(rightKeys.OrderBy(key => key))
                            && leftKeys.All(key => left[key] == right[key]);
            }
        }
    }
}
