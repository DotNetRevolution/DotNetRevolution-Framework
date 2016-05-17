using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    public class EntityCollection<T> : Collection<T>
    {
        public EntityCollection()
            : base()
        {
        }

        public EntityCollection(IList<T> list)
            : base(list)
        {
            Contract.Requires(list != null);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "item")]
        [Obsolete("Unboxing this collection is only allowed in the declarating class.", true)]
        public new void Add(T item) { }

        [Obsolete("Unboxing this collection is only allowed in the declarating class.", true)]
        public new void Clear() { }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "index")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "item")]
        [Obsolete("Unboxing this collection is only allowed in the declarating class.", true)]
        public new void Insert(int index, T item) { }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "item")]
        [Obsolete("Unboxing this collection is only allowed in the declarating class.", true)]
        public new void Remove(T item) { }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "index")]
        [Obsolete("Unboxing this collection is only allowed in the declarating class.", true)]
        public new void RemoveAt(int index) { }       
    }
}
