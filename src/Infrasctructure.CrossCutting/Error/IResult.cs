namespace PetProjects.Mts.CommandHandler.Infrasctructure.CrossCutting.Error
{
    using System.Collections.Generic;

    public interface IResult<out TData>
    {
        TData Data { get; }

        bool Success { get; }

        ICollection<Error> Errors { get; }
    }
}