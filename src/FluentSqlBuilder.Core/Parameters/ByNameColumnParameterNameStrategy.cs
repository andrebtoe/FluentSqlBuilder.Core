using SqlBuilderFluent.Parameters.Interfaces;

namespace SqlBuilderFluent.Parameters
{
    public class ByNameColumnParameterNameStrategy : IParameterNameStrategy
    {
        public string GetName(string nameField)
        {
            return nameField;
        }
    }
}