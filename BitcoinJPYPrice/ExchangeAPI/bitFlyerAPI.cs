using BitcoinJPYPrice.Base;
using BitcoinJPYPrice.ExchangeAPI.JSON;
using BitcoinJPYPrice.Net;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;

namespace BitcoinJPYPrice.ExchangeAPI
{
    public class BitFlyerAPI : BaseTickerAPI
    {
        private Http HttpAccess { get; set; }
        private BitFlyerTicker DeserializedResponse { get; set; }

        private float _ask;
        private float _bid;
        private float _last;
        private int _statusCode;

        public BitFlyerAPI()
        {
            if (HttpAccess == null) { HttpAccess = new Http(); }
        }

        public override void GetTicker()
        {
            string endpoint = "https://api.bitflyer.com/v1";
            string api = "ticker";

            HttpAccess.Url = $"{endpoint}/{api}";
            (int statusCode, string response) = HttpAccess.HttpContent().Result;

            if (HttpStatusCode.OK == (HttpStatusCode)statusCode)
            {
                DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings
                {
                    DateTimeFormat = new System.Runtime.Serialization.DateTimeFormat("yyyy-MM-dd'T'HH:mm:ss.FFF")
                };

                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(BitFlyerTicker), settings);
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(response), false))
                {
                    DeserializedResponse = js.ReadObject(ms) as BitFlyerTicker;
                }
                try
                {
                    _ask = DeserializedResponse.best_ask;
                    _bid = DeserializedResponse.best_bid;
                    _last = DeserializedResponse.ltp;
                    _statusCode = statusCode;
                }
                catch
                {
                    _statusCode = -1;
                }
            }
        }

        public override float GetAsk() { return _ask; }

        public override float GetBid() { return _bid; }

        public override float GetLast() { return _last; }

        public override int GetStatusCode() { return _statusCode; }
    }
}
