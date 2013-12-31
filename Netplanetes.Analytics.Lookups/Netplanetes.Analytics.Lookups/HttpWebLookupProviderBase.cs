using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Analytics.Lookups;
using Sitecore.Configuration;
using Sitecore.Diagnostics;

namespace Netplanetes.Analytics.Lookups
{
    /// <summary>
    /// Lookup Provider Base class
    /// </summary>
    public abstract class HttpWebLookupProviderBase : LookupProviderBase
    {
        #region properties
        private string _urlTemplate = string.Empty;
        /// <summary>
        /// request url template.
        /// for example
        /// http://ip-api.com/xml/{0} for ip-api.com
        /// http://api.docodoco.jp/v4/search?key1={0}&key2={1}&ipadr={2} for docodocojp.co.jp
        /// </summary>
        protected string UrlTemplate
        {
            get
            {
                if (string.IsNullOrEmpty(_urlTemplate))
                {
                    _urlTemplate = Settings.GetSetting("Netplanetes.Lookup.UrlTemplate", GetDefaultUrlTemplate());
                    if (string.IsNullOrEmpty(_urlTemplate))
                    {
                        throw new global::Sitecore.Exceptions.ProviderConfigurationException("Please configure Netplanetes.Lookup.UrlTemplate setting");
                    }
                }
                return _urlTemplate;
            }
        }
        private int _timeout = -1;
        /// <summary>
        /// timeout (milliseconds)
        /// default:30000 (30 sec)
        /// </summary>
        protected int Timeout
        {
            get
            {
                if (_timeout < 0)
                {
                    _timeout = int.Parse(Settings.GetSetting("Netplanetes.Lookup.Timeout", "30000"));
                }
                return _timeout;
            }
        }

        private string _applicationId = null;
        /// <summary>
        /// ApplicationId
        /// </summary>
        protected string ApplicationId
        {
            get
            {
                if (_applicationId == null)
                {
                    _applicationId = Settings.GetSetting("Netplanetes.Lookup.ApplicationId");
                }
                return _applicationId;
            }
        }

        private string _applicationSecret = null;
        /// <summary>
        /// ApplicationSecret
        /// </summary>
        protected string ApplicationSecret
        {
            get
            {
                if (_applicationSecret == null)
                {
                    _applicationSecret = Settings.GetSetting("Netplanetes.Lookup.ApplicationSecret");
                }
                return _applicationSecret;
            }
        }
        private IWhoIsInfomationResolver _resolver = null;
        /// <summary>
        /// IIpResolver
        /// </summary>
        protected IWhoIsInfomationResolver Resolver
        {
            get
            {
                if (_resolver == null)
                {
                    // double-lock
                    lock (_syncObject)
                    {
                        if (_resolver == null)
                        {
                            _resolver = CreateResolver();
                            if (!_resolver.ValidateConfig())
                            {
                                throw new global::Sitecore.Exceptions.ConfigurationException("LookupResolver is not configured correctly");
                            }
                        }
                    }
                }
                return _resolver;
            }
        }
        /// <summary>
        /// synchronization object
        /// </summary>
        protected object _syncObject = new object();
        #endregion

        #region methods
        /// <summary>
        /// Create Resolver 
        /// </summary>
        /// <returns></returns>
        protected abstract IWhoIsInfomationResolver CreateResolver();

        /// <summary>
        /// Resolve WhoIsInformation from ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public override WhoIsInformation GetInformationByIp(string ip)
        {
            Assert.ArgumentNotNull(ip, "ip");
            try
            {
                return this.Resolver.ResolveWhoIsInfomation(ip);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, this);
                Log.Error(ex.StackTrace, this);

                throw;
            }

        }

        protected abstract string GetDefaultUrlTemplate();
        #endregion

    }
}
