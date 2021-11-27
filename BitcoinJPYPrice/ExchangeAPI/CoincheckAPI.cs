using BitcoinJPYPrice.Base;
using BitcoinJPYPrice.ExchangeAPI.JSON;
using BitcoinJPYPrice.Net;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;

namespace BitcoinJPYPrice.ExchangeAPI
{
    public class CoincheckAPI : BaseTickerAPI
    {
        private Http HttpAccess { get; set; }
        private CoincheckTicker DeserializedResponse { get; set; }

        private float _ask;
        private float _bid;
        private float _last;
        private int _statusCode;

        public CoincheckAPI()
        {
            if (HttpAccess == null) { HttpAccess = new Http(); }
        }

        public override void GetTicker()
        {
            string endpoint = "https://coincheck.com/api";
            string api = "ticker";

            HttpAccess.Url = $"{endpoint}/{api}";
            (int statusCode, string response) = HttpAccess.HttpContent().Result;

            if (HttpStatusCode.OK == (HttpStatusCode)statusCode)
            {
                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(CoincheckTicker));
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(response), false))
                {
                    DeserializedResponse = js.ReadObject(ms) as CoincheckTicker;
                }
                _ask = DeserializedResponse.ask;
                _bid = DeserializedResponse.bid;
                _last = DeserializedResponse.last;
                _statusCode = statusCode;
            }
        }

        public override float GetAsk() { return _ask; }

        public override float GetBid() { return _bid; }

        public override float GetLast() { return _last; }

        public override int GetStatusCode() { return _statusCode; }
    }
}
