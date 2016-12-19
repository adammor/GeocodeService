using RestEL.Models;
using System;

namespace RestEL.Interfaces
{    public interface IREST<T> where T : class
    {
        string restLink { get; set; }
        string restParameter { get; set; }
        string restKey { get; set; }
        Guid restUID { get; set; }
        Requestquota restAPILimits { get; set; }

        string StringifyPayload(T payload);

        T GetDataAsync(T payload);

        string CallProvider(string requestUri);
    }
}
