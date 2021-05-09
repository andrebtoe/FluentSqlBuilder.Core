using SqlBuilderFluent.Exceptions;

namespace FluentSqlBuilder.Core.Inputs
{
    public class ClauseInput
    {
        public ClauseInput(string column, string operation, string parameterFormatedOrLiteralValue, ClauseInputType type)
        {
            Column = column;
            Operation = operation;
            ParameterFormatedOrLiteralValue = parameterFormatedOrLiteralValue;
            Type = type;
        }

        public ClauseInput(string column, string parameterFormated, ClauseInputType type)
        {
            Column = column;
            ParameterFormatedOrLiteralValue = parameterFormated;
            Type = type;
        }

        public ClauseInput(string column, ClauseInputType type)
        {
            Column = column;
            Type = type;
        }

        public string Column { get; private set; }
        public string Operation { get; private set; }
        public string ParameterFormatedOrLiteralValue { get; private set; }
        public ClauseInputType Type { get; private set; }

        public override string ToString()
        {
            string clause;

            switch (Type)
            {
                case ClauseInputType.ByOperation:
                    clause = $"{Column} {Operation} {ParameterFormatedOrLiteralValue}";
                    break;
                case ClauseInputType.ByOperationComparison:
                    clause = $"{Column} {Operation} {ParameterFormatedOrLiteralValue}";
                    break;
                case ClauseInputType.ByLike:
                    clause = $"{Column} LIKE {ParameterFormatedOrLiteralValue}";
                    break;
                case ClauseInputType.ByIsNull:
                    clause = $"{Column} IS NULL";
                    break;
                case ClauseInputType.ByIsNotNull:
                    clause = $"{Column} IS NOT NULL";
                    break;
                case ClauseInputType.ByIn:
                    clause = $"{Column} IN ({ParameterFormatedOrLiteralValue})";
                    break;
                case ClauseInputType.ByNotIn:
                    clause = $"{Column} NOT IN ({ParameterFormatedOrLiteralValue})";
                    break;
                default:
                    throw new SqlBuilderException("'Type' invalid");
            }

            return clause;
        }
    }
}