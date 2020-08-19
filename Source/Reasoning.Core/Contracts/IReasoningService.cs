using System.Collections.Generic;

namespace Reasoning.Core.Contracts
{
    /// <summary>
    /// A contract for Reasoning Service
    /// </summary>
    public interface IReasoningService
    {
        /// <summary>
        /// Starts reasoning with reseting all evaluation states and left term values
        /// </summary>
        /// <param name="reasoningProcess">Existing reasoning process</param>
        /// <returns>Evaluated reasoning process</returns>
        IReasoningProcess StartReasoning(IReasoningProcess reasoningProcess);
        /// <summary>
        /// Continues reasoning with existing state
        /// </summary>
        /// <param name="reasoningProcess">Existing reasoning process</param>
        /// <returns>Evaluated reasoning process</returns>
        IReasoningProcess ContinueReasoning(IReasoningProcess reasoningProcess);
        /// <summary>
        /// Tries to set variables to every rule
        /// </summary>
        /// <param name="reasoningProcess">Existing reasoning process</param>
        /// <param name="variables">Variable list for injection</param>
        /// <returns>Modified reasoning process</returns>
        IReasoningProcess SetValues(IReasoningProcess reasoningProcess, IList<IVariable> variables);
        /// <summary>
        /// Resets reasoning evaluation but preserving left term values
        /// </summary>
        /// <param name="reasoningProcess">Existing reasoning process</param>
        /// <returns>Modified reasoning process</returns>
        IReasoningProcess ResetReasoning(IReasoningProcess reasoningProcess);
        /// <summary>
        /// Resets reasoning evaluation and variables
        /// </summary>
        /// <param name="reasoningProcess">Existing reasoning process</param>
        /// <returns>Modified reasoning process</returns>
        IReasoningProcess ClearReasoning(IReasoningProcess reasoningProcess);
        /// <summary>
        /// Tries to get all missing variable ids in reasoning process
        /// </summary>
        /// <param name="reasoningProcess">Existing reasoning process</param>
        /// <returns>Enumerated variable ids</returns>
        IEnumerable<string> GetAllMissingVariableIds(IReasoningProcess reasoningProcess);
    }
}
