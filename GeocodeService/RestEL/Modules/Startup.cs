using RestEL.Models;

namespace RestEL.Modules
{
    public static class Startup
    {
        public static void Initialize<T>(string ClientServiceName) where T : class
        {
            Register<T> reg = new Register<T>()
            {
                    ClientServiceName = ClientServiceName,
                    EntityName = typeof(T).FullName,
                    LibraryServiceName = Constants.LIB_SERVICE_NAME,
                    Settings = new Setter(
                            Setter.GetSettings<ServiceAPI>(Constants.SERVICEAPI_FILE),
                            Setter.GetSettings<Package>(Constants.PACKAGE_FILE)
                            ),
            };

            reg.LoadProviders<T>();

            ProcessRecords<T> pr = new ProcessRecords<T>(reg.Settings, reg.Providers, reg.Repositories);
        }
    }
}
