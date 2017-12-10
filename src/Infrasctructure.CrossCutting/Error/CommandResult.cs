namespace PetProjects.Mts.CommandHandler.Infrasctructure.CrossCutting.Error
{
    using System.Collections.Generic;

    public class CommandResult<TData> : IResult<TData>
    {
        public CommandResult(TData data)
        {
            this.Data = data;
            this.Success = true;
        }

        public TData Data { get; }

        public bool Success { get; set; }

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