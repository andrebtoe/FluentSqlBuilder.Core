namespace SqlBuilderFluent.Parameters.Interfaces
{
    public interface IParameterNameStrategy
    {
        string GetName(string nameField);
    }
}