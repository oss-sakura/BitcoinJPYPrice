using System;
using System.Runtime.Serialization;

namespace BitcoinJPYPrice.ExchangeAPI.JSON
{
    // bitFlyer ticker api document.
    // https://lightning.bitflyer.com/docs?lang=ja#ticker

    [DataContract]
    public class BitFlyerTicker
    {
        [DataMember(Name = "product_code")]
        public string product_code { get; set; }
        [DataMember(Name = "state")]
        public string state { get; set; }
        [DataMember(Name = "timestamp")]
        public DateTime timestamp { get; set; }
        [DataMember(Name = "tick_id")]
        public int tick_id { get; set; }
        [DataMember(Name = "best_bid")]
        public float best_bid { get; set; }
        [DataMember(Name = "best_ask")]
        public float best_ask { get; set; }
        [DataMember(Name = "best_bid_size")]
        public float best_bid_size { get; set; }
        [DataMember(Name = "best_ask_size")]
        public float best_ask_size { get; set; }
        [DataMember(Name = "total_bid_depth")]
        public float total_bid_depth { get; set; }
        [DataMember(Name = "total_ask_depth")]
        public float total_ask_depth { get; set; }
        [DataMember(Name = "market_bid_size")]
        public float market_bid_size { get; set; }
        [DataMember(Name = "market_ask_size")]
        public float market_ask_size { get; set; }
        [DataMember(Name = "ltp")]
        public float ltp { get; set; }
        [DataMember(Name = "volume")]
        public float volume { get; set; }
        [DataMember(Name = "volume_by_product")]
        public float volume_by_product { get; set; }
    }
}
