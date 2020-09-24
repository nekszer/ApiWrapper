using System;

namespace ApiWrapper
{
    public class ApiParameter : Attribute
    {
        public bool Ignore { get; set; }
    }
}
