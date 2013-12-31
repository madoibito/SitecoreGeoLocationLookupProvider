using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Analytics.Lookups;

namespace Netplanetes.Analytics.Lookups
{
    /// <summary>
    /// Base
    /// </summary>
    public abstract class HttpWebLookupResolverBase : IWhoIsInfomationResolver
    {
        #region properties
        /// <summary>
        /// URL Template
        /// </summary>
        public string UrlTemplate { get; set; }
        /// <summary>
        /// Timeout
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// Application Id
        /// </summary>
        public string ApplicationId { get; set; }
        /// <summary>
        /// Application Secret Key
        /// </summary>
        public string ApplicationSecret { get; set; }
        #endregion


        /// <summary>
        /// Resolve WhoIsInfomation from ip string.
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public WhoIsInformation ResolveWhoIsInfomation(string ip)
        {
            HttpWebRequest request = CreateWebRequest(ip);

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                using (Stream s = response.GetResponseStream())
                {
                    return ParseResponseStream(s);
                }
            }
        }
        /// <summary>
        /// Parse response to create WhoIsInformation object.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        protected abstract WhoIsInformation ParseResponseStream(Stream s);

        /// <summary>
        /// Create HttpWebRequest
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        protected virtual HttpWebRequest CreateWebRequest(string ip)
        {
            HttpWebRequest request = WebRequest.Create(this.FormatRequestUrl(ip)) as HttpWebRequest;
            request.Timeout = this.Timeout;

            return request;
        }
        /// <summary>
        /// Format url string for query ip-api.
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        protected virtual string FormatRequestUrl(string ip)
        {
            return string.Format(UrlTemplate, ip);
        }

        /// <summary>
        /// Validate Configuration
        /// Always return true.
        /// </summary>
        /// <returns></returns>
        public virtual bool ValidateConfig()
        {
            return true;
        }

    }
}
