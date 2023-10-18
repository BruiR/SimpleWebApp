namespace SimpleWebApp.Domain.Models.Errors
{
    public class DuplicationOfEntityRelationships : Exception
    {
        //Для вывода User с id=id уже имееть Role с id=id
        public DuplicationOfEntityRelationships(string message) : base(message)
        {
        }

        public DuplicationOfEntityRelationships(string FirstEntityName, string FirstPropertyName, string FirstPropertyValue,
            string SecondEntityName, string SecondPropertyName, string SecondPropertyValue)
            : base($"{FirstEntityName} with {FirstPropertyName} = {FirstPropertyValue} is already has " +
                  $"{SecondEntityName} with {SecondPropertyName} = {SecondPropertyValue}.")
        {
        }
    }
}
