using RestEL.Models;
using System.Collections.Generic;

namespace RestEL.Interfaces
{
    public interface IRepository<T> where T : class 
    {
        // connect 
        string ConnSourceDB { get; }
        string ConnTargetDB { get; }

        // command
        string CommSelect { get; }
        string CommUpdate { get; }

        // filter
        List<Requestquota> ResultLimit { get; }

        // store
        List<T> RecordSet { get; }

        // crud 
        void Get();
        void Save(params object[] parameters);
    }
}
