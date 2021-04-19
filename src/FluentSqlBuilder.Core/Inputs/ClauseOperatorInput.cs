using SqlBuilderFluent.Types;
using System.Text;

namespace FluentSqlBuilder.Core.Inputs
{
    public class ClauseOperatorInput
    {
        public ClauseOperatorInput(ClauseInputOperator @operator, SqlBuilderFormatting formatting)
        {
            Operator = @operator;
            Formatting = formatting;
        }

        public ClauseInputOperator Operator { get; private set; }
        public SqlBuilderFormatting Formatting { get; private set; }

        public override string ToString()
        {
            var operatorNormalized = @Operator.ToString().ToUpper();

            var operatorToAdd = $"{operatorNormalized} ";
            var operatorToClause = new StringBuilder();

            if (Formatting == SqlBuilderFormatting.Indented)
                operatorToClause.AppendLine();
            else if (Formatting == SqlBuilderFormatting.None)
                operatorToClause.Append(" ");

            operatorToClause.Append(operatorToAdd);

            return operatorToClause.ToString();
        }
    }
}