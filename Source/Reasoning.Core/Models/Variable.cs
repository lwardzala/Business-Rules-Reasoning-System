using System;
using System.Collections;
using System.Text.Json.Serialization;
using Json.Abstraction.Converters;

using Reasoning.Core.Contracts;
using Reasoning.Core.Models.ValueTypes;

namespace Reasoning.Core.Models
{
    public class Variable : IVariable
    {
        public string Id { get; set; }
        public string Name { get; set; }
        [JsonConverter(typeof(ObjectConverter))]
        public object Value { get; set; }
        public int Frequency { get; set; }

        public Variable()
        {
            Frequency = 0;
        }

        public Variable(string id)
        {
            Id = id;
            Name = id;
            Frequency = 0;
        }

        public Variable(string id, object value)
        {
            Id = id;
            Name = id;
            Value = value;
            Frequency = 0;
        }

        public Variable(string id, string name, object value)
        {
            Id = id;
            Name = name;
            Value = value;
            Frequency = 0;
        }

        public IValueType GetValue()
        {
            try
            {
                switch (Value)
                {
                    case string _:
                    case int _:
                    case double _:
                    case bool _:
                    case long _:
                        return new BaseType(Value);
                    case IEnumerable _:
                        return new ListType(Value);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't cast value of {Name}. Unknown value type", ex);
            }

            return null;
        }

        public bool IsEmpty() => Value == null;

        public virtual string Display()
        {
            return $"{Name} = {Value.ToString()}";
        }

        public int CompareTo(IVariable obj)
        {
            if (obj.Frequency < Frequency) return 1;
            if (obj.Frequency > Frequency) return -1;

            return 0;
        }
    }
}
