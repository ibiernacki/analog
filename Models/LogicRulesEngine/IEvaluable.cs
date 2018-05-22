namespace Models.LogicRulesEngine
{
    public interface IEvaluable<in TContext>
    {
        bool Evaluate(TContext context);
    }
}