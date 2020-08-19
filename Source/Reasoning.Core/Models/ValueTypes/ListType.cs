using System;
using System.Collections;
using System.Reflection;

using Reasoning.Core.Contracts;

namespace Reasoning.Core.Models.ValueTypes
{
    public class ListType : object, IValueType
    {
        public ArrayList Values { get; set; }

        public ListType(object list)
        {
            Values = new ArrayList();
            
            try
            {
                var listType = list.GetType();
                var getEnumerator = listType.GetMethod("GetEnumerator", Type.EmptyTypes);
                var enumerator = (IEnumerator)getEnumerator?.Invoke(list, new object[] { });

                while (enumerator.MoveNext())
                {
                    Values.Add(new BaseType(enumerator.Current));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Couldn't convert object to a list", ex);
            }
        }

        public static bool operator ==(ListType leftTerm, ListType rightTerm)
        {
            if (leftTerm.Values.Count != rightTerm.Values.Count) return false;

            var rightTermCopy = (ArrayList)rightTerm.Values.Clone();
            foreach (var value in leftTerm.Values)
            {
                if (rightTermCopy.Contains(value))
                {
                    rightTermCopy.Remove(value);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public static bool operator !=(ListType leftTerm, ListType rightTerm)
        {
            return !(leftTerm == rightTerm);
        }

        public override bool Equals(object obj)
        {
            if (obj is ListType type)
            {
                return this == type;
            }

            return false;
        }

        public bool Between(IValueType leftTerm)
        {
            if (Values.Count != 2) throw new Exception("Invalid object. Proper range value has to be provided");
            if (!(leftTerm is BaseType)) return false;

            if (!((BaseType) leftTerm).IsNumber() || !((BaseType) Values[0]).IsNumber() ||
                !((BaseType) Values[1]).IsNumber()) return false;

            return (BaseType)Values[0] <= ((BaseType)leftTerm) && (BaseType)Values[1] >= ((BaseType)leftTerm);
        }

        public bool Contains(IValueType leftTerm)
        {
            switch (leftTerm)
            {
                case BaseType _:
                    return Values.Contains(leftTerm);
                case ListType type:
                {
                    var copy = (ArrayList)Values.Clone();
                    foreach (var value in type.Values)
                    {
                        if (copy.Contains(value))
                        {
                            copy.Remove(value);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    return true;
                }

                default:
                    return false;
            }
        }

        public object GetValue()
        {
            return Values;
        }

        public override string ToString()
        {
            var temp = "{";
            foreach (BaseType value in Values)
            {
                temp += value + ";";
            }
            return temp + "}";
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
