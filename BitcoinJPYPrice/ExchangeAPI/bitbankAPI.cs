using BitcoinJPYPrice.Base;
using BitcoinJPYPrice.ExchangeAPI.JSON;
using BitcoinJPYPrice.Net;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;


namespace BitcoinJPYPrice.ExchangeAPI
{
    public class BitbankAPI : BaseTickerAPI
    {
        private Http HttpAccess { get; set; }
        private BitbankTicker DeserializedResponse { get; set; }

        private float _ask;
        private float _bid;
        private float _last;
        private int _statusCode;

        public BitbankAPI()
        {
            if (HttpAccess == null) { HttpAccess = new Http(); }
        }

        public override void GetTicker()
        {
            string endpoint = "https://public.bitbank.cc";
            string pair = "btc_jpy";
            string api = "ticker";

            HttpAccess.Url = $"{endpoint}/{pair}/{api}";
            (int statusCode, string response) = HttpAccess.HttpContent().Result;

            if (HttpStatusCode.OK == (HttpStatusCode)statusCode)
            {
                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(BitbankTicker));
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(response), false))
                {
                    DeserializedResponse = js.ReadObject(ms) as BitbankTicker;
                }
                try
                {
                    _ = float.TryParse(DeserializedResponse.data.sell, out _ask);
                    _ = float.TryParse(DeserializedResponse.data.buy, out _bid);
                    _ = float.TryParse(DeserializedResponse.data.last, out _last);
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
