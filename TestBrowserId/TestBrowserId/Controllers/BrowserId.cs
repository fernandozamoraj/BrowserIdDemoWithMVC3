using System.Collections.Specialized;
using System.Net;
using System.Web.Mvc;
using Newtonsoft.Json;


namespace TestBrowserId.Controllers
{
    public class BrowserId
    {
        public string Audience { get; set; }
        public string Assertion { get; set; }
        public string Email { get; set; }
        public string Validity { get; set; }
        public string Issuer { get; set; }

        private string PostRequest(string url, NameValueCollection data)
        {
            WebClient wc = new WebClient();
            byte[] b = wc.UploadValues(url, data);
            string s = System.Text.Encoding.UTF8.GetString(b);
            return s;
        }

        public BrowserId(string audience, string assertion)
        {
            this.Audience = audience;
            this.Assertion = assertion;
        }

        /*
         Send the assertion to the browserid.org server (this must be over HTTPS)
          The response is read to determine if the assertion is authentic
         */
        public bool VerifyAssertion()
        {
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("assertion", this.Assertion);
            parameters.Add("audience", this.Audience);
            string s = PostRequest("https://browserid.org/verify", parameters);

            JsonData obj = JsonConvert.DeserializeObject<JsonData>(s);
            if (obj.status == "okay")
            {
                this.Email = obj.email;
                this.Validity = obj.validity;
                this.Issuer = obj.issuer;
                return true;
            }
            else
            {
                return false;
            }

        }
    }

    public class JsonData
    {
        public string email { get; set; }
        public string status { get; set; }
        public string validity { get; set; }
        public string issuer { get; set; }
    }

    
}
