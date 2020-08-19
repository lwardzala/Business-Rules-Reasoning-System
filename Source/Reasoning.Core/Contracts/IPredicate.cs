namespace Reasoning.Core.Contracts
{
    /// <summary>
    /// An interface for rule's predicate
    /// </summary>
    public interface IPredicate
    {
        /// <summary>
        /// First variable in equation
        /// </summary>
        IVariable LeftTerm { get; set; }
        /// <summary>
        /// Second variable in equation
        /// </summary>
        IVariable RightTerm { get; set; }
        /// <summary>
        /// Equation's operator type
        /// </summary>
        OperatorType Operator { get; set; }
        /// <summary>
        /// Result of predicate's evaluation
        /// </summary>
        bool Result { get; set; }
        /// <summary>
        /// Specifies if predicate has been evaluated
        /// </summary>
        bool Evaluated { get; set; }

        /// <summary>
        /// Validates if predicate is ready to be evaluated
        /// </summary>
        /// <returns>True or False</returns>
        bool IsValid();
        /// <summary>
        /// Runs predicate evaluation
        /// </summary>
        void Evaluate();
        /// <summary>
        /// Tries to get empty variable's id
        /// </summary>
        /// <returns>String or null</returns>
        string GetMissingVariableId();
    }
}
