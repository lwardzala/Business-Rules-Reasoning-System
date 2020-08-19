using System;

namespace Reasoning.Core.Contracts
{
    /// <summary>
    /// An interface of variable
    /// </summary>
    public interface IVariable : IComparable<IVariable>
    {
        /// <summary>
        /// Variable's id
        /// </summary>
        string Id { get; set; }
        /// <summary>
        /// Variable's name. If null, inherits from Id
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Raw variable's value
        /// </summary>
        object Value { get; set; }
        /// <summary>
        /// Specifies how many rules includes the variable
        /// </summary>
        int Frequency { get; set; }

        /// <summary>
        /// Checks if left term value is empty
        /// </summary>
        /// <returns>True or False</returns>
        bool IsEmpty();
        /// <summary>
        /// Casts variable's value to an instance ready for comparison
        /// </summary>
        /// <returns>BaseType or ValueType</returns>
        IValueType GetValue();
    }
}
