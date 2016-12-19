using RestEL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using JSONHelpers;
using FileScan;


namespace RestEL.Modules
{
    public class Setter
    {
        public List<Generalsetting> InitGeneralSettings { get; private set; }
        public API InitAPISettings { get; private set; }
        public Repository SourceDB { get; private set; }
        public Repository TargetDB { get; private set; }
        public ServiceAPI AllSettings { get; private set; }
        public Package AppSettings { get; private set; }     

        public Setter(ServiceAPI _allSettings, Package _appSettings)
        {

            AllSettings = _allSettings;
            AppSettings = _appSettings;

            InitGeneralSettings = GetGeneralSettings(_appSettings.settings.generalSettingsSelected.runOptions[0].roundRobin);

            InitAPISettings = GetAPISettings(_appSettings.settings.APISelected.type,
                                            _appSettings.settings.APISelected.APIName,
                                            _appSettings.settings.APISelected.planType);

            SourceDB = GetDBSettings("Source", _appSettings.settings.repositorySelected.Source);
            TargetDB = GetDBSettings("Target", _appSettings.settings.repositorySelected.Target);
        }

        public List<Generalsetting> GetGeneralSettings(string direction)
        {
            var Generalsettings = (from _general in AllSettings.generalSettings
                                   select new Generalsetting
                                   {
                                       roundRobin = (from _rr in _general.roundRobin
                                                     where _rr.direction == direction
                                                     select new Roundrobin
                                                     {
                                                         direction = _rr.direction
                                                     }).ToList(),

                                       retry = (from _r in _general.retry
                                                select new Retry
                                                {
                                                    numOfRetries = _r.numOfRetries,
                                                    secondsDelay = _r.secondsDelay
                                                }).ToList(),
                                   }).ToList();

            return Generalsettings;
        }

        //select the service 
        public API GetAPISettings(string serviceType, string APIName, string planType)
        {
            //select service     
            var APIS =
                            (from _service in AllSettings.services[0].API
                             where _service.type == serviceType
                             select new API
                             {
                                 type = _service.type,
                                 description = _service.description,
                                 providers = (from _sl in _service.providers
                                              where _sl.APIName == APIName
                                              select new Providers
                                              {
                                                  APIName = _sl.APIName,
                                                  GUID = _sl.GUID,
                                                  dependencies = (from _d in _sl.dependencies select _d).ToList(),
                                                  endpoint = (from _ul in _sl.endpoint.usageLimits
                                                              where _ul.planType == planType
                                                              select new Endpoint
                                                              {
                                                                  protocol = _sl.endpoint.protocol,
                                                                  baseURL = _sl.endpoint.baseURL,
                                                                  responseFormat = _sl.endpoint.responseFormat,
                                                                  key = _sl.endpoint.key,
                                                                  keylessTestAllowed = _sl.endpoint.keylessTestAllowed,
                                                                  parameter = _sl.endpoint.parameter,
                                                                  usageLimits = (from _pt in _sl.endpoint.usageLimits
                                                                                 where _pt.planType == planType
                                                                                 select new Usagelimit
                                                                                 {
                                                                                     planType = _ul.planType,
                                                                                     requestQuota = (from _rq in _pt.requestQuota
                                                                                                     select new Requestquota
                                                                                                     {
                                                                                                         second = _rq.second,
                                                                                                         minute = _rq.minute,
                                                                                                         hour = _rq.hour,
                                                                                                         day = _rq.day,
                                                                                                         year = _rq.year
                                                                                                     }).ToList()
                                                                                 }).ToList()
                                                              }).SingleOrDefault()
                                              }).ToList()
                             }).ToList();


            return APIS.Single();
        }


        public Repository GetDBSettings(string repositoryType, string name)
        {             
            var repo = new List<Repository>();
            repo.Add(AllSettings.repositories);

            var Repository = ( from _r in repo
                select new Repository 
                {
                     sourceList = (from _l in _r.sourceList
                                  where _l.name == name
                                     select new Sourcelist
                                         {
                                             getQuery = _l.getQuery,
                                             name = _l.name,
                                             connectionString = _l.connectionString,
                                             providerName = _l.providerName,
                                         }).ToList(),

                     targetList = (from _l in _r.targetList
                                  where _l.name == name
                                         select new Targetlist
                                         {
                                             updateQuery = _l.updateQuery,
                                             name = _l.name,
                                             connectionString = _l.connectionString,
                                             providerName = _l.providerName,
                                         }).ToList()
                });


            return Repository.Single();
              
        }

        public static T GetSettings<T>(string fileLocation)
        {
            T _result = default(T);

            string stringified="";
            var json = new JSONProcessor();

            try
            {
                stringified = FileReader.ReadFileIntoString(fileLocation);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            try
            {

                _result = json.ConvertJsonToType<T>(stringified);
            }
            catch (Exception ex)
            {
                Console.WriteLine(json.ErrorMessages[0], " [FilePath: " + fileLocation + "]");
                Console.WriteLine(ex);
            }

            return _result;
        }
    }
}
