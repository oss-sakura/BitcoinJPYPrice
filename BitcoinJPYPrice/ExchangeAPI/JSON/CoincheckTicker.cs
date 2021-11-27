using System.Runtime.Serialization;

namespace BitcoinJPYPrice.ExchangeAPI.JSON
{
    // Coincheck ticker api document.
    // https://coincheck.com/ja/documents/exchange/api#ticker

    [DataContract]
    public class CoincheckTicker
    {
        [DataMember(Name = "last")]
        public float last { get; set; }
        [DataMember(Name = "bid")]
        public float bid { get; set; }
        [DataMember(Name = "ask")]
        public float ask { get; set; }
        [DataMember(Name = "high")]
        public float high { get; set; }
        [DataMember(Name = "low")]
        public float low { get; set; }
        [DataMember(Name = "volume")]
        public float volume { get; set; }
        [DataMember(Name = "timestamp")]
        public int timestamp { get; set; }
    }
}
