using RestEL.Interfaces;
using RestEL.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;


namespace RestEL.Modules
{       
    public class Register<T> where T : class
    {
        public Setter Settings { get; set; }
        public string ClientServiceName { get; set; }
        public string EntityName;
        public string LibraryServiceName { get; set; }
        public IDictionary<string, Func<IREST<T>>> Providers = new Dictionary<string, Func<IREST<T>>>();
        public IDictionary<string, Func<IRepository<T>>> Repositories = new Dictionary<string, Func<IRepository<T>>>();

        private static string ProviderName;
        private static Type LibClassName;
        private static List<Requestquota> RequestQuota;
        private static Sourcelist SourceList;
        private static Targetlist TargetList;

        public void LoadProviders<U>() where U : T
        {
            // reg client service assembly
            Assembly asm_client = Assembly.Load( ClientServiceName );

            // loop through client classes
            foreach (Type _type in asm_client.GetTypes())
            {
                string _APIName = "";

                // check if class has registration attribute 
                foreach (RegisterAPIAttribute _version in _type.GetCustomAttributes(typeof(RegisterAPIAttribute), false))
                {
                    _APIName = _version.APIName;
                    if (!string.IsNullOrEmpty(_APIName)) { break; }
                }
                if (string.IsNullOrEmpty(_APIName)) { continue; }

                // match setting value that matches class name, i.e., a provider
                for (var i = 0; i < Settings.AllSettings.services[0].API[0].providers.Count - 1; i++)
                {
                    if (_APIName == Settings.AllSettings.services[0].API[0].providers[i].APIName)
                    {
                        RegisterAPI(asm_client,
                                    _type,
                                    EntityName,
                                    Settings.AllSettings.services[0].API[0].providers[i],
                                    Settings.AllSettings.services[0].API[0].providers[i].endpoint.usageLimits[0].requestQuota);
                        _APIName = "";
                        break;
                    }
                }
            }


            // reg library service assembly
            Assembly asm_library = Assembly.Load( LibraryServiceName  ); 

            bool exit_asm2 = false;
            foreach (Type _type in asm_library.GetTypes())
            {

                if (exit_asm2 == true) { break; }

                string _ProviderName = "";
                // check if class has registration attribute 
                var _attrs = _type.GetCustomAttributes(typeof(RegisterRepositoryAttribute), false);
                if (_attrs.Length > 0)
                {
                    foreach (RegisterRepositoryAttribute _version in _attrs)
                    {
                        _ProviderName = _version.ProviderName;
                        if (!string.IsNullOrEmpty(_ProviderName)) { break; }
                    }
                    if (string.IsNullOrEmpty(_ProviderName)) { continue; }

                    // Source: match setting value that matches class name, i.e., a provider
                    for (var i = 0; i < Settings.AllSettings.repositories.sourceList.Count && exit_asm2 == false; i++)
                    {
                        if (_ProviderName == Settings.AllSettings.repositories.sourceList[i].providerName &&
                            Settings.AppSettings.settings.repositorySelected.Source == Settings.AllSettings.repositories.sourceList[i].name)
                        {
                            LibClassName = _type;
                            ProviderName = _ProviderName;
                            RequestQuota = Settings.InitAPISettings.providers[0].endpoint.usageLimits[0].requestQuota;

                            SourceList = Settings.SourceDB.sourceList[0];

                            break;
                        }
                    }

                    // Target: match setting value that matches class name, i.e., a provider
                    for (var i = 0; i < Settings.AllSettings.repositories.targetList.Count && exit_asm2 == false; i++)
                    {
                        if (_ProviderName == Settings.AllSettings.repositories.targetList[i].providerName &&
                            Settings.AppSettings.settings.repositorySelected.Target == Settings.AllSettings.repositories.targetList[i].name)
                        {
                            TargetList = Settings.TargetDB.targetList[0];

                            break;
                        }
                    }

                    // when ONE repository is registered, stop
                    if (LibClassName != null && ProviderName != null && RequestQuota != null && SourceList != null && TargetList != null) { exit_asm2 = true; }
    
                }
            }

            RegisterRepository(asm_client, asm_library, LibClassName, ProviderName, EntityName, RequestQuota, SourceList, TargetList);
        }

     
        private void RegisterAPI(Assembly asm_client, Type className, string entityName, Providers serviceObject, List<Requestquota> APILimits)
        {
            Type[] _entity = { asm_client.GetType(entityName) };
            Type _providerEntity = className.MakeGenericType(_entity);
            var _instantClass = Activator.CreateInstance(_providerEntity, new object[] { serviceObject, APILimits }) as IREST<T>;

            Providers.Add(serviceObject.APIName, () => _instantClass);
        }

        private void RegisterRepository(Assembly asm_client, Assembly asm_library, Type className, string ProviderName, string entityName, List<Requestquota> APILimits, Sourcelist SourceDB, Targetlist TargetDB) 
        {
            Type[] _entity = { asm_client.GetType(entityName) };
            Type _providerEntity = className.MakeGenericType(_entity);
            var _instantClass = Activator.CreateInstance(_providerEntity, new object[] { APILimits, SourceDB, TargetDB }) as IRepository<T>;

            Repositories.Add(ProviderName, () => _instantClass);
        }

    }
}