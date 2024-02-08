using AspNetServiceLib.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AspNetServiceLib.ServiceInterface.Implementation
{
    public class ServiceMessageComparison
    {
        private static readonly Regex dateTimeRegex = new Regex(@"\d{1,4}[-/]\d{1,4}[-/]\d{1,4}([T ]\d{1,2}:\d{1,2}:\d{1,2}(\.\d+)?)?");

        /// <summary>
        /// Determines whether two service messages are identical with respect to the workflow's replay approach.
        /// </summary>
        public static bool AreEqual(ServiceMessage message1, ServiceMessage message2)
        {
            if (message1 == null && message2 == null)
            {
                return true;
            }
            if (message1 == null || message2 == null)
            {
                return false;
            }
            if (message1.MessageData.SequenceEqual(message2.MessageData))
            {
                return true;
            }

            var messageString1 = dateTimeRegex.Replace(message1.MessageText, "");
            var messageString2 = dateTimeRegex.Replace(message2.MessageText, "");
            return messageString1 == messageString2;
        }
    }
}
