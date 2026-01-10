namespace RPG.Core
{
    public interface IPredicateEvaluator
    {
        bool? Evaluate(Predicate predicate, string[] parameters);
    }
}