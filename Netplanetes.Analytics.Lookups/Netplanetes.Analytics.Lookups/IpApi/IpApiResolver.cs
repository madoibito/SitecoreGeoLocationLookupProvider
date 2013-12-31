using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Sitecore.Analytics.Lookups;
using Sitecore.Diagnostics;
using C = Netplanetes.Analytics.Lookups.IpApi.IpApiConstants;
using System.IO;

namespace Netplanetes.Analytics.Lookups.IpApi
{
    /// <summary>
    /// resolver implementation uses ip-api.com web service.
    /// </summary>
    public class IpApiResolver : HttpWebLookupResolverBase
    {
        #region constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        public IpApiResolver() { }
        #endregion

        #region methods

        /// <summary>
        /// Parse response to create WhoIsInformation object.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        protected override WhoIsInformation ParseResponseStream(Stream s)
        {
            using (XmlReader reader = XmlReader.Create(s))
            {
                // check success
                if (!CheckSuccess(reader))
                {
                    Log.Debug("Lookup Ip to GeoLocation Failed", this);
                    DumpResponse(reader);
                    return WhoIsInformation.Unknown;
                }
                // create WhoIsInformation
                return CreateWhoIsInformation(reader);
            }
        }

        /// <summary>
        /// Check Response
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected virtual bool CheckSuccess(XmlReader reader)
        {
            // read ahead to status node.
            if (reader.ReadToFollowing(C.Status))
            {
                reader.Read();
                return C.Success.Equals(reader.Value, StringComparison.InvariantCultureIgnoreCase);
            }
            throw new FormatException("status element is not found");
        }
        /// <summary>
        /// Create WhoIsInformation object
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private WhoIsInformation CreateWhoIsInformation(XmlReader reader)
        {
            WhoIsInformation who = new WhoIsInformation();
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    string key = reader.Name;
                    // move to content
                    reader.Read();
                    string value = reader.Value;

                    FillWhoIsInformation(key, value, who);

                }
            }
            if (who.IsEmpty) return WhoIsInformation.Unknown;

            return who;
        }
        /// <summary>
        /// Fill WhoIsInfomation Property
        /// 
        /// Please refer following url about key and value 
        /// http://ip-api.com/docs/api:returned_values
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="who"></param>
        protected virtual void FillWhoIsInformation(string key, string value, WhoIsInformation who)
        {
            Assert.ArgumentNotNull(key, "key");
            Assert.ArgumentNotNull(value, "value");
            Assert.ArgumentNotNull(who, "who");

            if (string.IsNullOrWhiteSpace(value)) return;
            key = key.ToLowerInvariant();

            switch (key)
            {
                case C.Country:
                    // not used
                    break;
                case C.CountryCode:
                    who.Country = value;
                    break;
                case C.Region:
                    who.AreaCode = value;
                    break;
                case C.RegionName:
                    who.Region = value;
                    break;
                case C.City:
                    who.City = value;
                    break;
                case C.Zip:
                    who.PostalCode = value;
                    break;
                case C.Lat:
                    who.Latitude = value;
                    break;
                case C.Lon:
                    who.Longitude = value;
                    break;
                case C.TimeZone:
                    // not used
                    break;
                case C.Isp:
                    who.Isp = value;
                    break;
                case C.Org:
                    who.BusinessName = value;
                    break;
                case C.AS:
                    // not used
                    break;
                case C.Reverse:
                    who.Dns = value;
                    break;
                case C.Query:
                    // not used
                    break;
                case C.Status:
                case C.Message:
                    // not used here
                    break;
                default:
                    Log.Info("Unknown Key " + key, this);
                    break;
            }
        }

        /// <summary>
        /// Validate Resolver configuration
        /// Check if UrlTemplate and Timeout are configured.
        /// </summary>
        /// <returns></returns>
        public override bool ValidateConfig()
        {
            if (string.IsNullOrEmpty(this.UrlTemplate))
            {
                Log.Info("UrlTemplate is not configured", this);
                return false;
            }

            if (this.Timeout < 0)
            {
                Log.Info("Timeout is not configured", this);
                return false;
            }

            return true;
        }


        /// <summary>
        /// Dump Output Response to log.
        /// </summary>
        /// <param name="reader"></param>
        protected void DumpResponse(XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    string key = reader.Name;
                    // move to content
                    reader.Read();
                    string value = reader.Value;

                    Log.Debug(string.Format("{0} - {1}", key, value), this);
                }
            }
        }
        #endregion

    }
}
