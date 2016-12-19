using RestEL.Interfaces;
using RestEL.Models;
using RestEL.Modules;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestEL.Modules
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class RegisterEntityAttribute : Attribute
    {
        public string EntityName { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class RegisterAPIAttribute : Attribute
    {
        public string APIName { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class RegisterRepositoryAttribute : Attribute
    {
        public string ClassName { get; set; }
        public string ProviderName { get; set; }
    }
}
