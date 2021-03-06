using BitcoinJPYPrice.Common;
using BitcoinJPYPrice.ExchangeAPI;
using System.Windows;
using System.Windows.Media;

namespace BitcoinJPYPrice
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string _mutexName = "BitcoinJPYPriceApp";
        private static readonly System.Threading.Mutex mutex = new System.Threading.Mutex(false, _mutexName);

        private IntervalTimer _intervalTimer;

        private void RestoreWindowSize()
        {
            Height = Properties.Settings.Default.WindowHeight;
            Width = Properties.Settings.Default.WindowWidth;
        }

        private void RestoreFont()
        {
            string fontName = Properties.Settings.Default.FontName;
            if (!string.IsNullOrEmpty(fontName))
            {
                FontFamily = new FontFamily(fontName);
            }
            else
            {
                fontName = SystemFonts.MessageFontFamily.ToString();
                Properties.Settings.Default.FontName = fontName;
                Properties.Settings.Default.Save();
                FontFamily = new FontFamily(fontName);
            }
        }

        private void BitbankTicker()
        {
            BitbankAPI bitbank = new BitbankAPI();
            TickerFeed feed = new TickerFeed(
                bitbank,
                "bitbank",
                lblExchangeName4,
                lblAsk4,
                lblBid4,
                lblLast4,
                lblSpread4,
                lblAskDiffValue4,
                lblBidDiffValue4,
                lblLastDiffValue4,
                _intervalTimer);
            feed.Run();
        }

        private void BitFlyerTicker()
        {
            BitFlyerAPI bitFlyer = new BitFlyerAPI();
            TickerFeed feed = new TickerFeed(
                bitFlyer,
                "bitFlyer",
                lblExchangeName2,
                lblAsk2,
                lblBid2,
                lblLast2,
                lblSpread2,
                lblAskDiffValue2,
                lblBidDiffValue2,
                lblLastDiffValue2,
                _intervalTimer);
            feed.Run();
        }

        private void CoincheckTicker()
        {
            CoincheckAPI coincheck = new CoincheckAPI();
            TickerFeed feed = new TickerFeed(
                coincheck,
                "Coincheck",
                lblExchangeName3,
                lblAsk3,
                lblBid3,
                lblLast3,
                lblSpread3,
                lblAskDiffValue3,
                lblBidDiffValue3,
                lblLastDiffValue3,
                _intervalTimer);
            feed.Run();
        }

        private void GMOCoinTicker()
        {
            GMOCoinAPI gmocoin = new GMOCoinAPI();
            TickerFeed feed = new TickerFeed(
                gmocoin,
                "GMOコイン",
                lblExchangeName5,
                lblAsk5,
                lblBid5,
                lblLast5,
                lblSpread5,
                lblAskDiffValue5,
                lblBidDiffValue5,
                lblLastDiffValue5,
                _intervalTimer);
            feed.Run();
        }

        private void ZaifTicker()
        {
            ZaifAPI zaif = new ZaifAPI();
            TickerFeed feed = new TickerFeed(
                zaif,
                "Zaif",
                lblExchangeName1,
                lblAsk1,
                lblBid1,
                lblLast1,
                lblSpread1,
                lblAskDiffValue1,
                lblBidDiffValue1,
                lblLastDiffValue1,
                _intervalTimer);
            feed.Run();
        }

        public void RunTickerFeeds()
        {
            int intervalSec = Properties.Settings.Default.IntervalSec * 1000;
            _intervalTimer = new IntervalTimer(intervalSec);

            BitbankTicker();
            BitFlyerTicker();
            CoincheckTicker();
            GMOCoinTicker();
            ZaifTicker();

            _intervalTimer.Start();
        }

        private void WindowClose()
        {
            Properties.Settings.Default.WindowWidth = Width;
            Properties.Settings.Default.WindowHeight = Height;
            Properties.Settings.Default.Save();
        }

        private void Quit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ShowPreferencesWindow(object sender, RoutedEventArgs e)
        {
            PreferencesWindow wnd = new PreferencesWindow();
            bool? result = wnd.ShowDialog();
            if (result.Value == true)
            {
                FontFamily = new FontFamily(Properties.Settings.Default.FontName);
                _intervalTimer.ChangeIntervalTimer(Properties.Settings.Default.IntervalSec * 1000);
            }
        }

        private void ShowAboutWindow(object sender, RoutedEventArgs e)
        {
            AboutWindow wnd = new AboutWindow();
            wnd.ShowDialog();
        }

        public MainWindow()
        {
            InitializeComponent();
            if (!mutex.WaitOne(0, false))
            {
                MessageBox.Show("Bitcoin JPY Price already running.");
                Application.Current.Shutdown();
                return;
            }

            RestoreWindowSize();
            RestoreFont();

            ContentRendered += (s, e) => { RunTickerFeeds(); };
            Closing += (s, e) => { WindowClose(); };
        }
    }
}
