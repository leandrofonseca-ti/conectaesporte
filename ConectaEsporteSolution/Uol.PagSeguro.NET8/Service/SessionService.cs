using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using System.Xml;
using Uol.PagSeguro.NET8.Domain;
using Uol.PagSeguro.NET8.Domain.Direct;
using Uol.PagSeguro.NET8.Exception;
using Uol.PagSeguro.NET8.Log;
using Uol.PagSeguro.NET8.Resources;
using Uol.PagSeguro.NET8.Util;
using Uol.PagSeguro.NET8.XmlParse;

namespace Uol.PagSeguro.NET8.Service
{
    public class SessionService
    {

        /// <summary>
        /// Request a direct payment session
        /// </summary>
        /// <param name="credentials">PagSeguro credentials</param>
        /// <returns><c cref="T:Uol.PagSeguro.NET8.CancelRequestResponse">Result</c></returns>
        public static Session CreateSession(Credentials credentials)
        {

            PagSeguroTrace.Info(String.Format(CultureInfo.InvariantCulture, "SessionService.Register() - begin"));
            try
            {
                using (HttpWebResponse response = HttpURLConnectionUtil.GetHttpPostConnection(
                    PagSeguroConfiguration.SessionUri.AbsoluteUri, BuildSessionURL(credentials)))
                {

                    using (XmlReader reader = XmlReader.Create(response.GetResponseStream()))
                    {

                        Session result = new Session();
                        SessionSerializer.Read(reader, result);
                        PagSeguroTrace.Info(String.Format(CultureInfo.InvariantCulture, "SessionService.Register({0}) - end", result.ToString()));
                        return result;
                    }
                }
            }
            catch (WebException exception)
            {
                PagSeguroServiceException pse = HttpURLConnectionUtil.CreatePagSeguroServiceException((HttpWebResponse)exception.Response);
                PagSeguroTrace.Error(String.Format(CultureInfo.InvariantCulture, "SessionService.Register() - error {0}", pse));
                throw pse;
            }
        }

        private static String BuildSessionURL(Credentials credentials)
        {
            QueryStringBuilder builder = new QueryStringBuilder();
            builder.EncodeCredentialsAsQueryString(credentials);
            return builder.ToString();
        }

    }
}
