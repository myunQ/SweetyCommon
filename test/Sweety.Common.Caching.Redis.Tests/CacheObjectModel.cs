using System;
using System.Collections.Generic;

namespace Sweety.Common.Caching.Redis.Tests
{
    public class CacheObjectModel
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public IList<CacheObjectModel> Children { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (Object.ReferenceEquals(this, obj)) return true;
            if (!(obj is CacheObjectModel model)) return false;

            bool result = ID == model.ID
                && Name == model.Name;

            if (result)
            {
                if (Children != null && model.Children != null)
                {
                    if (Children.Count == model.Children.Count)
                    {
                        for (int i = 0; i<Children.Count; i++)
                        {
                            if(!Children[i].Equals(model.Children[i]))
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else if(!(Children == null && model.Children == null))
                {
                    return false;
                }
            }

            return result;
        }
    }
}
