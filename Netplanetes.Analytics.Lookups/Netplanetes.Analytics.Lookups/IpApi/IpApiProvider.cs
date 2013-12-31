using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netplanetes.Analytics.Lookups.IpApi
{
    /// <summary>
    /// This class is implementation of LookupProvider uses "http://ip-api.com"
    /// Please lookup at ip-api for detail specification.
    /// 
    /// </summary>
    /// <remarks>
    /// ip-api has following usage limits. <br/>
    /// 
    /// ip-api will automatically ban any IP addresses doing over 240 requests per minute. Contact them to get your IP whitelisted or unbanned.
    /// You are free to use ip-api.com for non-commercial use.
    /// </remarks>
    public class IpApiProvider : HttpWebLookupProviderBase
    {
        #region methods
        /// <summary>
        /// Create Resolver
        /// </summary>
        /// <returns></returns>
        protected override IWhoIsInfomationResolver CreateResolver()
        {
            IpApiResolver resolver = new IpApiResolver();
            resolver.UrlTemplate = this.UrlTemplate;
            resolver.Timeout = this.Timeout;

            return resolver;
        }

        protected override string GetDefaultUrlTemplate()
        {
            return "http://ip-api.com/xml/{0}";
        }
        #endregion

    }
}
