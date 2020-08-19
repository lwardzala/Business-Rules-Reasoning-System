namespace Reasoning.Core.Contracts
{
    /// <summary>
    /// An interface of variable's value
    /// </summary>
    public interface IValueType
    {
        /// <summary>
        /// Checks if term is included in value
        /// </summary>
        /// <param name="leftTerm"></param>
        /// <returns>True or False</returns>
        bool Contains(IValueType leftTerm);
        /// <summary>
        /// Returns variable's value
        /// </summary>
        /// <returns>object</returns>
        object GetValue();
    }
}
