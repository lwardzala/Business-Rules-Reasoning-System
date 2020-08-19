namespace Reasoning.Core.Contracts
{
    /// <summary>
    /// Equation operator type
    /// </summary>
    public enum OperatorType
    {
        Equal,
        NotEqual,
        GreaterThan,
        LesserThan,
        GreaterOrEqual,
        LesserOrEqual,
        Between,
        NotBetween,
        IsIn,
        NotIn,
        Subset,
        NotSubset
    }

    /// <summary>
    /// An interface for operator method
    /// </summary>
    public interface IOperator
    {
        /// <summary>
        /// Compares two variables
        /// </summary>
        /// <param name="leftTerm">First variable from equation</param>
        /// <param name="rightTerm">Second variable from equation</param>
        /// <returns></returns>
        bool Compare(IValueType leftTerm, IValueType rightTerm);
    }
}
