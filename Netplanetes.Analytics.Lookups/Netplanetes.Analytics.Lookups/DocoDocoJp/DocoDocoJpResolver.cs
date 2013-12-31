using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Sitecore.Analytics.Lookups;
using Sitecore.Diagnostics;
using C = Netplanetes.Analytics.Lookups.DocoDocoJp.DocoocoJpConstants;

namespace Netplanetes.Analytics.Lookups.DocoDocoJp
{
    /// <summary>
    /// Ip to Geolocation Resolver for DocodocoJP
    /// Please look at following url for api details.
    /// http://www.docodoco.jp/document/index.html
    /// </summary>
    public class DocoDocoJpResolver : HttpWebLookupResolverBase
    {
        protected override global::Sitecore.Analytics.Lookups.WhoIsInformation ParseResponseStream(System.IO.Stream s)
        {
            XDocument dom = XDocument.Load(s, LoadOptions.None);

            if (!CheckSuccess(dom))
            {
                Log.Debug("Lookup Ip to GeoLocation Failed", this);
                DumpResponse(dom);
                return WhoIsInformation.Unknown;
            }
            return CreateWhoIsInformation(dom);
        }

        protected virtual bool CheckSuccess(XDocument dom)
        {
            XElement e = dom.Element(C.Status);

            return e == null;
        }

        private WhoIsInformation CreateWhoIsInformation(XDocument dom)
        {
            WhoIsInformation who = new WhoIsInformation();
            foreach (var e in dom.Root.Elements())
            {
                string key = e.Name.LocalName;
                string value = e.Value;

                FillWhoIsInformation(key, value, who);
            }

            if (who.IsEmpty) return WhoIsInformation.Unknown;

            return who;
        }

        protected virtual void FillWhoIsInformation(string key, string value, WhoIsInformation who)
        {
            Assert.ArgumentNotNull(key, "key");
            Assert.ArgumentNotNull(value, "value");
            Assert.ArgumentNotNull(who, "who");

            if (string.IsNullOrWhiteSpace(value)) return;

            key = key.ToLowerInvariant();

            switch (key)
            {
                case C.Ip:
                    break;
                case C.ContinentCode:
                    break;
                case C.CountryCode:
                    who.Country = value;
                    break;
                case C.CountryAName:
                    break;
                case C.CountryJName:
                    break;
                case C.PrefCode:
                    who.AreaCode = value;
                    break;
                case C.RegionCode:
                    break;
                case C.PrefAName:
                    who.Region = value;
                    break;
                case C.PrefJName:
                    break;
                case C.PrefLatitude:
                    if (string.IsNullOrWhiteSpace(who.Latitude))
                    {
                        who.Latitude = value;
                    }
                    break;
                case C.PrefLongtude:
                    if (string.IsNullOrWhiteSpace(who.Longitude))
                    {
                        who.Longitude = value;
                    }
                    break;
                case C.PrefCF:
                    break;
                case C.CityCode:
                    who.MetroCode = value;
                    break;
                case C.CityAName:
                    who.City = value;
                    break;
                case C.CityJName:
                    break;
                case C.CityLatitude:
                    who.Latitude = value;
                    break;
                case C.CityLongtude:
                    who.Longitude = value;
                    break;
                case C.CityCF:
                    break;
                case C.BCFlag:
                    break;
                case C.OrgCode:
                    break;
                case C.OrgOfficeCode:
                    break;
                case C.OrgIndependentCode:
                    break;
                case C.OrgName:
                    who.BusinessName = value;
                    break;
                case C.OrgPrefCode:
                    break;
                case C.OrgCityCode:
                    break;
                case C.OrgZipCode:
                    who.PostalCode = value;
                    break;
                case C.OrgAliress:
                    break;
                case C.OrgTel:
                    break;
                case C.OrgFax:
                    break;
                case C.OrgIpoType:
                    break;
                case C.OrgDate:
                    break;
                case C.OrgCapitalCode:
                    break;
                case C.OrgEmployeesCode:
                    break;
                case C.OrgGrossCode:
                    break;
                case C.OrgPresclassent:
                    break;
                case C.OrgIndustrialCategoryL:
                    break;
                case C.OrgIndustrialCategoryM:
                    break;
                case C.OrgIndustrialCategoryS:
                    break;
                case C.OrgIndustrialCategoryT:
                    break;
                case C.OrgDomainName:
                    break;
                case C.OrgDomainType:
                    break;
                case C.LineCode:
                    break;
                case C.LineJName:
                    who.Isp = value;
                    break;
                case C.LineCF:
                    break;
                case C.TimeZone:
                    break;
                case C.TelCode:
                    break;
                case C.StockTickerNumber:
                    break;
                case C.DomainName:
                    who.Dns = value;
                    break;
                case C.DomainType:
                    break;
                default:
                    Log.Info("Unknown Key " + key, this);
                    break;
            }
        }

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

            if (string.IsNullOrEmpty(this.ApplicationId))
            {
                Log.Info("ApplicationId is not configured", this);
                return false;
            }
            if (string.IsNullOrEmpty(this.ApplicationSecret))
            {
                Log.Info("ApplicationSecret is not configurad", this);
                return false;
            }
            return true;

        }

        protected void DumpResponse(XDocument dom)
        {
            Log.Debug(dom.ToString(SaveOptions.None));
        }


        protected override string FormatRequestUrl(string ip)
        {
            return string.Format(this.UrlTemplate, this.ApplicationId, this.ApplicationSecret, ip);
        }

    }
}
