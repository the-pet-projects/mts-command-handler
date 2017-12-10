namespace PetProjects.Mts.CommandHandler.Infrasctructure.CrossCutting.Error
{
    using System;
    using System.Collections.Generic;

    public class Error
    {
        public string Message { get; set; }

        public int ErrorCode { get; set; }

        public ICollection<Exception> Exceptions { get; set; }
    }
}