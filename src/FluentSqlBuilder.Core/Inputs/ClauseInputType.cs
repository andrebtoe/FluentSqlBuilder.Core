namespace FluentSqlBuilder.Core.Inputs
{
    public enum ClauseInputType
    {
        ByOperation = 1,
        ByOperationComparison = 2,
        ByLike = 3,
        ByIsNull = 4,
        ByIsNotNull = 5,
        ByIn = 6,
        ByNotIn = 7
    }
}