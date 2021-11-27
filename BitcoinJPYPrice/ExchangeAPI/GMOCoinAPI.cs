using BitcoinJPYPrice.Base;
using BitcoinJPYPrice.ExchangeAPI.JSON;
using BitcoinJPYPrice.Net;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;

namespace BitcoinJPYPrice.ExchangeAPI
{
    public class GMOCoinAPI : BaseTickerAPI
    {
        private Http HttpAccess { get; set; }
        private GMOCoinTicker DeserializedResponse { get; set; }

        private float _ask;
        private float _bid;
        private float _last;
        private int _statusCode;

        public GMOCoinAPI()
        {
            if (HttpAccess == null) { HttpAccess = new Http(); }
        }

        public override void GetTicker()
        {
            string endpoint = "https://api.coin.z.com/public";
            string api = "v1/ticker?symbol=BTC";

            HttpAccess.Url = $"{endpoint}/{api}";
            (int statusCode, string response) = HttpAccess.HttpContent().Result;

            if (HttpStatusCode.OK == (HttpStatusCode)statusCode)
            {
                DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings
                {
                    DateTimeFormat = new System.Runtime.Serialization.DateTimeFormat("yyyy-MM-dd'T'HH:mm:ss.FFF'Z'")
                };

                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(GMOCoinTicker), settings);
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(response), false))
                {
                    DeserializedResponse = js.ReadObject(ms) as GMOCoinTicker;
                }
                _ = float.TryParse(DeserializedResponse.data[0].ask, out _ask);
                _ = float.TryParse(DeserializedResponse.data[0].bid, out _bid);
                _ = float.TryParse(DeserializedResponse.data[0].last, out _last);
                _statusCode = statusCode;
            }
        }

        public override float GetAsk() { return _ask; }

        public override float GetBid() { return _bid; }

        public override float GetLast() { return _last; }

        public override int GetStatusCode() { return _statusCode; }
    }
}
