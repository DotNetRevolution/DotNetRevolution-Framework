using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace DotNetRevolution.Core.Base
{
    public abstract class ValueObject<T> : IEquatable<T>
        where T : ValueObject<T>
    {
        public override bool Equals(object obj)
        {
            return obj != null && Equals(obj as T);
        }

        public override int GetHashCode()
        {
            const int startValue = 17;
            const int multiplier = 59;

            var fields = GetFields();
            
            return fields.Select(field => field.GetValue(this))
                .Where(value => value != null)
                .Aggregate(startValue, (current, value) => current*multiplier + value.GetHashCode());
        }

        public virtual bool Equals(T other)
        {
            if (other == null)
            {
                return false;
            }
            
            var t = GetType();
            var otherType = other.GetType();
            
            if (t != otherType)
            {
                return false;
            }
            
            var fields = t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            Contract.Assume(Contract.ForAll(fields, x => x != null));

            foreach (var field in fields)
            {
                Contract.Assume(field != null);

                var value1 = field.GetValue(other);
                var value2 = field.GetValue(this);
                
                if (value1 == null)
                {
                    if (value2 != null)
                    {
                        return false;
                    }
                }
                else if (value1.Equals(value2) == false)
                {
                    return false;
                }
            }

            return true;
        }

        private IEnumerable<FieldInfo> GetFields()
        {
            var t = GetType();

            var fields = new List<FieldInfo>();
            
            while (t != null && t != typeof(object))
            {
                fields.AddRange(t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public));
                
                t = t.BaseType;
            }

            return fields;
        }

        public static bool operator ==(ValueObject<T> valueObject1, ValueObject<T> valueObject2)
        { 
            return ((object)valueObject1 != null) && valueObject1.Equals(valueObject2);
        }

        public static bool operator !=(ValueObject<T> valueObject1, ValueObject<T> valueObject2)
        {
            return !(valueObject1 == valueObject2);
        }
    }
}
