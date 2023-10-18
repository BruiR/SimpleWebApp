namespace SimpleWebApp.Domain.Models.Errors
{
    public class EntityAlreadyExistException : Exception
    {
        //Для вывода ошибок "Такой имейл уже зарег-н"
        public EntityAlreadyExistException(string message) : base(message)
        {
        }

        public EntityAlreadyExistException(string entityName, string propertyName, string propertyValue)
            : base($"{entityName} with {propertyName} = {propertyValue} is already exists")
        {
        }
    }
}
