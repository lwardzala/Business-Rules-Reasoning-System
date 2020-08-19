using System;

using Reasoning.Core.Contracts;

namespace Reasoning.Core.Models.ValueTypes
{
    public class BaseType : object, IValueType
    {
        public object Value { get; set; }

        public BaseType(object value)
        {
            Value = value;
        }

        public static bool operator ==(BaseType leftTerm, BaseType rightTerm)
        {
            if (leftTerm.IsNumberOrBoolean() && rightTerm.IsNumberOrBoolean())
            {
                return Convert.ToDouble(leftTerm.Value) == Convert.ToDouble(rightTerm.Value);
            }
            
            if (leftTerm.IsString() && rightTerm.IsString())
            {
                return (string)leftTerm.Value == (string)rightTerm.Value;
            }

            return false;
        }

        public static bool operator !=(BaseType leftTerm, BaseType rightTerm)
        {
            return !(leftTerm == rightTerm);
        }

        public static bool operator >(BaseType leftTerm, BaseType rightTerm)
        {
            if (leftTerm.IsNumber() && rightTerm.IsNumber())
            {
                return Convert.ToDouble(leftTerm.Value) > Convert.ToDouble(rightTerm.Value);
            }

            return false;
        }

        public static bool operator <(BaseType leftTerm, BaseType rightTerm)
        {
            if (leftTerm.IsNumber() && rightTerm.IsNumber())
            {
                return Convert.ToDouble(leftTerm.Value) < Convert.ToDouble(rightTerm.Value);
            }

            return false;
        }

        public static bool operator >=(BaseType leftTerm, BaseType rightTerm)
        {
            if (leftTerm.IsNumber() && rightTerm.IsNumber())
            {
                return !(rightTerm > leftTerm);
            }

            return false;
        }

        public static bool operator <=(BaseType leftTerm, BaseType rightTerm)
        {
            if (leftTerm.IsNumber() && rightTerm.IsNumber())
            {
                return !(rightTerm < leftTerm);
            }

            return false;
        }

        public bool Contains(IValueType leftTerm)
        {
            if (!(leftTerm is BaseType)) return false;

            if (((BaseType)leftTerm).IsNumberOrBoolean())
            {
                return this == (BaseType)leftTerm;
            }
            else if (Value is string)
            {
                return ((string)Value).Contains(leftTerm.ToString());

            }

            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj is BaseType)
            {
                return this == (BaseType)obj;
            }

            return false;
        }

        public bool IsNumber()
        {
            if (Value is int || Value is double || Value is long) return true;
            return false;
        }

        public bool IsBoolean()
        {
            if (Value is bool) return true;
            return false;
        }

        public bool IsString()
        {
            if (Value is string) return true;
            return false;
        }

        public bool IsNumberOrBoolean()
        {
            if (IsNumber() || IsBoolean()) return true;
            return false;
        }

        public object GetValue()
        {
            return Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
