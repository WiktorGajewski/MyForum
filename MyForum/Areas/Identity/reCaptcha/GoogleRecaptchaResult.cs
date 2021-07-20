using Newtonsoft.Json;
using System.Collections.Generic;

namespace MyForum.Areas.Identity.reCaptcha
{
    public class GoogleRecaptchaResult
    {
        private string m_Success;

        [JsonProperty("success")]
        public string Success
        {
            get { return m_Success; }
            set { m_Success = value; }
        }

        private List<string> m_ErrorCodes;

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes
        {
            get { return m_ErrorCodes; }
            set { m_ErrorCodes = value; }
        }

        public string Validate(string gRecaptchaResponse, string secret)
        {
            var client = new System.Net.WebClient();

            var GoogleReply = 
                client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, gRecaptchaResponse));

            var captchaResponse = JsonConvert.DeserializeObject<GoogleRecaptchaResult>(GoogleReply);

            return captchaResponse.Success.ToLower();
        }
    }
}
