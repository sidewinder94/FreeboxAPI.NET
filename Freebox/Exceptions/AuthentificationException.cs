using System;
using System.Collections.Generic;
using System.Text;

namespace Freebox.Exceptions
{
    public class AuthentificationException : Exception
    {
        public AuthentificationException(string message) : base(message)
        {
        }

        public AuthentificationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public AuthentificationException()
        {
        }
    }
}
