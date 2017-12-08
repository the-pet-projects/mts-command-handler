namespace PetProjects.Mts.CommandHandler.Infrasctructure.CrossCutting.Error
{
    using System.Collections.Generic;
    using System.Linq;

    public class CommandResult<TData> : IResult<TData>
    {
        public CommandResult(TData data)
        {
            this.Data = data;
        }

        public TData Data { get; }

        public bool Success => this.Errors.Any();

        public ICollection<Error> Errors { get; } = new List<Error>();

        public void Add(Error error)
        {
            this.Errors.Add(error);
        }

        public void Remove(Error error)
        {
            this.Errors.Remove(error);
        }
    }
}