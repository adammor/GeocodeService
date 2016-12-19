using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Directionals
{
    public class RoundRobin<T> : Collection<T>
    {
        public const string FORWARD = "Forward";
        public const string RANDOM = "Random";
        public const string FIXED = "Fixed";

        private int _currentIdx = 0;
        public string MethodName;

        public RoundRobin(string MethodName)
        {
            this.MethodName = MethodName;
        }

        public RoundRobin()
        {

        }
        
        public T Current
        {
            get
            {
                return this[_currentIdx];
            }
        }

        public void Forward()
        {
            if (Count - 1 > _currentIdx) { _currentIdx++; } else { _currentIdx = 0; }
        }


        public void Random()
        {
            Random r = new Random();
            _currentIdx = (r.Next(1, Count + 1)) - 1;
        }

        public void Fixed()
        {
            //position does not move
        }

        public void Move()
        {
            var methods = new List<string>();
            methods.Add(FORWARD);
            methods.Add(RANDOM);
            methods.Add(FIXED);

            foreach (var item in methods)
            {
                var method = this.GetType().GetMethod(item);
                if (MethodName == item)
                {
                    method.Invoke(this, null);
                    break;
                }
            }
        }
    }
   
}