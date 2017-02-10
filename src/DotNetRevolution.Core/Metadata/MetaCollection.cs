using DotNetRevolution.Core.Extension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;

namespace DotNetRevolution.Core.Metadata
{
    public class MetaCollection : Collection<Meta>
    {
        public MetaCollection()
        {
        }

        public MetaCollection(IReadOnlyCollection<Meta> collection) 
        {
            Contract.Requires(collection != null);
            Contract.Requires(Contract.ForAll(collection, o => o != null));

            collection.ForEach(Add);
        }

        public void AddOrReplace(Meta item)
        {
            var currentMeta = this.FirstOrDefault(x => x.Key == item.Key);

            if (currentMeta != null)
            {
                Remove(currentMeta);
            }

            Add(item);
        }

        public void AddIfMissing(Meta item)
        {
            if (this.All(x => x.Key != item.Key))
            {
                Add(item);
            }
        }
                
        protected override void InsertItem(int index, Meta item)
        {
            // don't allow null items
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            // don't allow duplicate keys
            if (this.Any(x => x.Key == item.Key))
            {
                throw new ArgumentException("An item with the same key already exists", nameof(item));
            }

            base.InsertItem(index, item);
        }

        protected override void SetItem(int index, Meta item)
        {
            // don't allow null items
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var indexItem = this[index];
            Contract.Assume(indexItem != null);

            // check if replacing same key
            if (indexItem.Key == item.Key)
            {
                // replacing
                base.SetItem(index, item);
            }
            else
            {
                // find first item with new key
                var keyItem = this.FirstOrDefault(x => x.Key == item.Key);

                // check if any item with the new key exists
                if (keyItem == null)
                {
                    // no item with key exists, call base
                    base.SetItem(index, item);
                }
                else
                {
                    // trying to add a duplicate key
                    throw new ArgumentException("An item with the same key already exists", nameof(item));
                }
            }            
        }
    }
}
