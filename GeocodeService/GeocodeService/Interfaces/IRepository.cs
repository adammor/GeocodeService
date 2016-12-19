using RestEL.Models;
using System.Collections.Generic;

namespace RestEL.Interfaces
{
    public interface IRepository<T> where T : class 
    {
        //connection
        string connSourceDB { get; }
        string connTargetDB { get; }
        string providerNameSourceDB { get; }
        string providerNameTargetDB { get; }

        //initialization
        void Initialize();
        string AddGetQuerySourceDB { get; set; }
        string AddUpdateQueryTargetDB { get; set; }
        string EndPointService { get; set; }

        //crud 
        List<T> All { get; set; }
        List<T> Get(List<Requestquota> recordByLimit);
        void Save(params object[] para);
    }
}
