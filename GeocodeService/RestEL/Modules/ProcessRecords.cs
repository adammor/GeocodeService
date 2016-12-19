using RestEL.Interfaces;
using RestEL.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using Directionals;

namespace RestEL.Modules
{
    public class ProcessRecords<T> where T : class
    {
        public ServiceAPI AllSettings {get; private set;}
        public Generalsetting GeneralSettings { get; private set; }

        private IDictionary<string, Func<IREST<T>>> providers;
        private Func<IRepository<T>> source; 
        private Func<IRepository<T>> target; 
        private string provider;

        public ProcessRecords(Setter settings, IDictionary<string, Func<IREST<T>>> providers, IDictionary<string, Func<IRepository<T>>> repositories)
        {
            this.AllSettings = settings.AllSettings;
            this.providers = providers;

            //get default settings
            this.GeneralSettings = settings.InitGeneralSettings[0];
            provider = settings.InitAPISettings.providers[0].APIName.ToString();
            repositories.TryGetValue(settings.SourceDB.sourceList[0].providerName.ToString(), out source);
            repositories.TryGetValue(settings.TargetDB.targetList[0].providerName.ToString(), out target);

            Execute();
        }

        private void Execute()
        {

            IREST<T> _rest;
            Func<IREST<T>> _command;

            T _response = null;
            int _retries;
            int _secDelay;

            //get all registered providers
            RoundRobin<string> rr = new RoundRobin<string>(GeneralSettings.roundRobin[0].direction);
            if (GeneralSettings.roundRobin[0].direction != RoundRobin<T>.FIXED)
            {
                foreach (var p in providers)
                {
                    rr.Add(p.Key);
                }
            }
                 
            //retrieve and update geocode, one rec at a time            
            foreach (T rec in source().RecordSet)
            {
                _rest = null;
                _command = null;
                _response = null;

                while (_response == null)
                {
                    providers.TryGetValue(provider, out _command);
                    _rest = _command();  //hydrate instantiation
                    _retries = GeneralSettings.retry[0].numOfRetries;
                    _secDelay = GeneralSettings.retry[0].secondsDelay * Constants.MILLISECONDS; //convert to miliseconds 
                    while (_response == null && _retries != 0)  //TODO: actually, _response won't be null, but an error bubbled up from Get method
                    {
                        _response = _rest.GetDataAsync(rec); //retrieve rec's from 3rd party
                        //_response = null; //test  
                        if (_response != null)
                        {
                            break;
                        }
                        else
                        {
                            _retries--;
                            Thread.Sleep(_secDelay);
                        }
                    }

                    // _retries = 0; //test
                    if (_response == null && _retries == 0 && rr.MethodName != RoundRobin<T>.FIXED)
                    {
                        rr.Move();
                        provider = rr.Current;
                    }

                }

                target().Save(_response);  //update local db with newly retrieved values     
            }
             
        }
             

    }




}
