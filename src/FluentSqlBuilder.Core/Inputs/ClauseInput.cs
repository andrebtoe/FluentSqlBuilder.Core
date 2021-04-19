using SqlBuilderFluent.Exceptions;

namespace FluentSqlBuilder.Core.Inputs
{
    public class ClauseInput
    {
        public ClauseInput(string column, string operation, string parameterFormated, ClauseInputType type)
        {
            Column = column;
            Operation = operation;
            ParameterFormated = parameterFormated;
            Type = type;
        }

        public ClauseInput(string column, string parameterFormated, ClauseInputType type)
        {
            Column = column;
            ParameterFormated = parameterFormated;
            Type = type;
        }

        public ClauseInput(string column, ClauseInputType type)
        {
            Column = column;
            Type = type;
        }

        public string Column { get; private set; }
        public string Operation { get; private set; }
        public string ParameterFormated { get; private set; }
        public ClauseInputType Type { get; private set; }

        public override string ToString()
        {
            string clause;

            switch (Type)
            {
                case ClauseInputType.ByOperation:
                    clause = $"{Column} {Operation} {ParameterFormated}";
                    break;
                case ClauseInputType.ByOperationComparison:
                    clause = $"{Column} {Operation} {ParameterFormated}";
                    break;
                case ClauseInputType.ByLike:
                    clause = $"{Column} LIKE {ParameterFormated}";
                    break;
                case ClauseInputType.ByIsNull:
                    clause = $"{Column} IS NULL";
                    break;
                case ClauseInputType.ByIsNotNull:
                    clause = $"{Column} IS NOT NULL";
                    break;
                case ClauseInputType.ByIn:
                    clause = $"{Column} IN ({ParameterFormated})";
                    break;
                case ClauseInputType.ByNotIn:
                    clause = $"{Column} NOT IN ({ParameterFormated})";
                    break;
                default:
                    throw new SqlBuilderException("'Type' invalid");
            }

            return clause;
        }
    }
}