using System;

namespace ProductAPI.Core.Exceptions
{
    public class ItemDoesntExistException : Exception
    {
        public int Id { get; }

        public ItemDoesntExistException()
        {
        }

        public ItemDoesntExistException(string message)
            : base(message)
        {
        }

        public ItemDoesntExistException(string message, int id)
            : base(message)
        {
            Id = id;
        }
    }
}
