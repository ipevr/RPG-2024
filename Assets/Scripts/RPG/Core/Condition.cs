using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    [System.Serializable]
    public class Condition
    {
        [SerializeField] private Predicate predicate;
        [SerializeField] private bool not;
        [SerializeField] private string[] parameters;
        
        public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
        {
            foreach (var evaluator in evaluators)
            {
                var result = evaluator.Evaluate(predicate, parameters);
                if (result == null) continue;

                if (result == false) return not;
            }

            return !not;
        }
    }
}
