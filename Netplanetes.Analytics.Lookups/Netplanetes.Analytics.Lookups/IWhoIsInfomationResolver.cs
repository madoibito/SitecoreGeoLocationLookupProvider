using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Analytics.Lookups;

namespace Netplanetes.Analytics.Lookups
{
    public interface IWhoIsInfomationResolver
    {
        /// <summary>
        /// URL Template
        /// </summary>
        string UrlTemplate { get; set; }
        /// <summary>
        /// Timeout
        /// </summary>
        int Timeout { get; set; }

        /// <summary>
        /// Application Id
        /// </summary>
        string ApplicationId { get; set; }
        /// <summary>
        /// Application Secret Key
        /// </summary>
        string ApplicationSecret { get; set; }
        /// <summary>
        /// Resolve IP to Geo location
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        WhoIsInformation ResolveWhoIsInfomation(string ip);

        /// <summary>
        /// Validate Configuration
        /// </summary>
        /// <returns></returns>
        bool ValidateConfig();

    }
}
