namespace Petrof.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;
    public class SessionHelper
    {
        private string? _sessionId { get; set; }
        public SessionHelper(string sessionId)
        {
            _sessionId = sessionId;
        }

        public string GetId()
        {
            return _sessionId;
        }
    }
}