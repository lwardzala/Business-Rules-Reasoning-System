using System;
using System.Linq;
using System.Collections.Generic;

using Reasoning.Core.Contracts;

namespace Reasoning.Core.Services
{
    public class ReasoningService : IReasoningService
    {
        public IReasoningProcess StartReasoning(IReasoningProcess reasoningProcess)
        {
            var result = ClearReasoning(reasoningProcess);

            return ContinueReasoning(result);
        }

        public IReasoningProcess ContinueReasoning(IReasoningProcess reasoningProcess)
        {
            switch (reasoningProcess.ReasoningMethod)
            {
                case ReasoningMethod.Deduction:
                    return Deduction(reasoningProcess);
                case ReasoningMethod.HypothesisTesting:
                    return HypothesisTesting(reasoningProcess);
                default:
                    return null;
            }
        }

        public IReasoningProcess SetValues(IReasoningProcess reasoningProcess, IList<IVariable> variables)
        {
            reasoningProcess.KnowledgeBase.RuleSet.ToList().ForEach(rule =>
            {
                rule.Predicates.ToList().ForEach(predicate =>
                {
                    variables.ToList().ForEach(variable =>
                    {
                        if (predicate.LeftTerm.Id == variable.Id)
                        {
                            predicate.LeftTerm.Value = variable.Value;
                        }
                    });
                });
            });

            return reasoningProcess;
        }

        public IReasoningProcess ResetReasoning(IReasoningProcess reasoningProcess)
        {
            reasoningProcess.State = ReasoningState.INITIALIZED;
            reasoningProcess.ReasonedItems = new List<IVariable>();
            reasoningProcess.EvaluationMessage = EvaluationMessage.NONE;
            reasoningProcess.KnowledgeBase.RuleSet.ToList().ForEach(rule =>
            {
                rule.Evaluated = false;
                rule.Result = false;
                rule.Predicates.ToList().ForEach(predicate =>
                {
                    predicate.Evaluated = false;
                    predicate.Result = false;
                });
            });

            return reasoningProcess;
        }

        public IReasoningProcess ClearReasoning(IReasoningProcess reasoningProcess)
        {
            var result = ResetReasoning(reasoningProcess);

            var variables = AnalyzeVariablesFrequency(reasoningProcess);

            reasoningProcess.KnowledgeBase.RuleSet.ToList().ForEach(rule =>
            {
                rule.Predicates.ToList().ForEach(predicate =>
                {
                    predicate.LeftTerm.Frequency = variables.First(variable => variable.Id == predicate.LeftTerm.Id).Frequency;
                    predicate.LeftTerm.Value = null;
                });
            });

            return result;
        }

        public IEnumerable<string> GetAllMissingVariableIds(IReasoningProcess reasoningProcess)
        {
            return GetAllMissingVariables(reasoningProcess).Select(variable => variable.Id);
        }

        public IEnumerable<IVariable> GetAllMissingVariables(IReasoningProcess reasoningProcess)
        {
            var result = new List<IVariable>();

            reasoningProcess.KnowledgeBase.RuleSet.ToList().ForEach(rule =>
            {
                rule.Predicates.ToList().ForEach(predicate =>
                {
                    if (predicate.LeftTerm.IsEmpty() && result.All(variable => variable.Id != predicate.LeftTerm.Id))
                        result.Add(predicate.LeftTerm);
                });
            });

            result.Sort();

            return result;
        }

        private IEnumerable<IVariable> AnalyzeVariablesFrequency(IReasoningProcess reasoningProcess)
        {
            var result = new List<IVariable>();

            reasoningProcess.KnowledgeBase.RuleSet.ToList().ForEach(rule =>
            {
                rule.Predicates.ToList().ForEach(predicate =>
                {
                    if (!result.Contains(predicate.LeftTerm))
                    {
                        predicate.LeftTerm.Frequency = 0;
                        result.Add(predicate.LeftTerm);
                    }
                    var index = result.FindIndex(item => item.Id == predicate.LeftTerm.Id);
                    if (index != -1) result[index].Frequency++;
                });
            });

            return result;
        }

        private IReasoningProcess Deduction(IReasoningProcess reasoningProcess)
        {
            try
            {
                foreach (var rule in reasoningProcess.KnowledgeBase.RuleSet)
                {
                    if (!rule.Evaluated)
                    {
                        rule.Evaluate();
                    }

                    if (rule.Evaluated && rule.Result)
                    {
                        reasoningProcess.ReasonedItems.Add(rule.Conclusion);
                    }
                }
            }
            catch (Exception)
            {
                reasoningProcess.EvaluationMessage = EvaluationMessage.ERROR;
                reasoningProcess.State = ReasoningState.FINISHED;

                return reasoningProcess;
            }

            var finished = reasoningProcess.KnowledgeBase.RuleSet.All(_ => _.Evaluated);

            reasoningProcess.State = finished ? ReasoningState.FINISHED : ReasoningState.STOPPED;
            reasoningProcess.EvaluationMessage = reasoningProcess.ReasonedItems.Count > 0 
                ? EvaluationMessage.PASSED
                : finished ? EvaluationMessage.FAILED : EvaluationMessage.MISSING_VALUES;

            return reasoningProcess;
        }

        private IReasoningProcess HypothesisTesting(IReasoningProcess reasoningProcess)
        {
            var rules = reasoningProcess.KnowledgeBase.RuleSet.Where(rule =>
                rule.Conclusion.Id == reasoningProcess.Hypothesis.Id
                && rule.Conclusion.Value.Equals(reasoningProcess.Hypothesis.Value)).ToList();

            foreach (var rule in rules)
            {
                if (!rule.Evaluated)
                {
                    rule.Evaluate();
                }

                if (rule.Evaluated
                    && rule.Result
                    && rule.Conclusion.Id == reasoningProcess.Hypothesis.Id
                    && rule.Conclusion.Value.Equals(reasoningProcess.Hypothesis.Value))
                {
                    reasoningProcess.ReasonedItems = new List<IVariable> { reasoningProcess.Hypothesis };
                }
            }

            var finished = rules.All(_ => _.Evaluated);

            reasoningProcess.State = finished ? ReasoningState.FINISHED : ReasoningState.STOPPED;
            reasoningProcess.EvaluationMessage = reasoningProcess.ReasonedItems.Count > 0
                ? EvaluationMessage.PASSED
                : finished ? EvaluationMessage.FAILED : EvaluationMessage.MISSING_VALUES;

            return reasoningProcess;
        }
    }
}
