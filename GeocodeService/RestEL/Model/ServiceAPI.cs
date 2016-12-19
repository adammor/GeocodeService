using Newtonsoft.Json;
using RestEL.Modules;
using System;
using System.Collections.Generic;

namespace RestEL.Models
{
public class ServiceAPI
{   
    public List<Generalsetting> generalSettings { get; set; }    
    public List<Service> services { get; set; }
    public Repository repositories { get; set; }
}
     
public class Service
{
public List<API> API { get; set; }
}

public class Generalsetting
{
    public List<Roundrobin> roundRobin { get; set; }
    public List<Retry> retry { get; set; }
}

public class Roundrobin
{
    public Roundrobin() { direction = "Fixed"; }
    public string direction { get; set; }
}

public class Retry
{
    public int numOfRetries { get; set; }
    public int secondsDelay { get; set; }
}

public class API
{
    public string type { get; set; }
    public string description { get; set; }
    public List<Providers> providers { get; set; }
}

public class Providers
{
    public string APIName { get; set; }
    public Guid GUID { get; set; }

    public bool discontinued { get; set; }

    [JsonProperty("comments", NullValueHandling = NullValueHandling.Ignore)]
    public string comments { get; set; }

    [JsonProperty("dependencies", NullValueHandling = NullValueHandling.Ignore)]
    public List<string> dependencies { get; set; }

    public Endpoint endpoint { get; set; }
}

public class Endpoint
{
    [JsonProperty("protocol", NullValueHandling = NullValueHandling.Ignore)]
    public string protocol { get; set; }

    [JsonProperty("baseURL", NullValueHandling = NullValueHandling.Ignore)]
    public string baseURL { get; set; }

    [JsonProperty("responseFormat", NullValueHandling = NullValueHandling.Ignore)]
    public string responseFormat { get; set; }

    [JsonProperty("key", NullValueHandling = NullValueHandling.Ignore)]
    public string key { get; set; }

    [JsonProperty("keylessTestAllowed", NullValueHandling = NullValueHandling.Ignore)]
    public bool keylessTestAllowed { get; set; }

    [JsonProperty("parameter", NullValueHandling = NullValueHandling.Ignore)]
    public string parameter { get; set; }

    [JsonProperty("usageLimits", NullValueHandling = NullValueHandling.Ignore)]
    public List<Usagelimit> usageLimits { get; set; }
}

public class Usagelimit
{
    public string planType { get; set; }
    public List<Requestquota> requestQuota { get; set; }
}

public class Requestquota
{
    [JsonProperty("second", NullValueHandling = NullValueHandling.Ignore)]
    public int second { get; set; }

    [JsonProperty("minute", NullValueHandling = NullValueHandling.Ignore)]
    public int minute { get; set; }

    [JsonProperty("hour", NullValueHandling = NullValueHandling.Ignore)]
    public int hour { get; set; }

    [JsonProperty("day", NullValueHandling = NullValueHandling.Ignore)]
    public int day { get; set; }

    [JsonProperty("year", NullValueHandling = NullValueHandling.Ignore)]
    public int year { get; set; }


    public Requestquota()
    {
        second = -1;
        minute = -1;
        hour = -1;
        day = -1;
        year = -1;
    }

}

public class Repository
{
    public List<Sourcelist> sourceList { get; set; }
    public List<Targetlist> targetList { get; set; }
}

public class Sourcelist 
{
    public string name { get; set; }
    public string connectionString { get; set; }
    public string providerName { get; set; }
    public string getQuery { get; set; }
}

public class Targetlist 
{
    public string name { get; set; }
    public string connectionString { get; set; }
    public string providerName { get; set; }
    public string updateQuery { get; set; }
}

}


