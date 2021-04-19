using SqlBuilderFluent.Parameters.Interfaces;

namespace SqlBuilderFluent.Parameters
{
    public class ByIndexParameterNameStrategy : IParameterNameStrategy
    {
        private const string parameterPrefox = "Param";
        private int _paramIndex = 0;

        public string GetName(string nameField)
        {
            _paramIndex += 1;

            var parameterName = $"{parameterPrefox}{_paramIndex}";

            return parameterName;
        }
    }
}