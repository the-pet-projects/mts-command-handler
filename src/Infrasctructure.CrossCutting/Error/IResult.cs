namespace PetProjects.Mts.CommandHandler.Infrasctructure.CrossCutting.Error
{
    using System.Collections.Generic;

    public interface IResult
    {
        bool Success { get; }

        ICollection<Error> Errors { get; }
    }
}