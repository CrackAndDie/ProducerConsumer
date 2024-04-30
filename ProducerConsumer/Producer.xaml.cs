using ProducerConsumer.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProducerConsumer
{
    public partial class Producer : Border
    {
        private TransportLine _transportLine;
        private readonly Random _random;
        private const int TIME_BETWEEN_GENERATIONS = 100; // in ms

        public Producer()
        {
            InitializeComponent();
            _random = new Random(GetHashCode());
        }

        // just ioc cause idc
        public void InjectTransportLine(TransportLine transportLine)
        {
            _transportLine = transportLine;
        }

        async public void StartGeneratingColor()
        {
            int timeOfGenerating = _random.Next(1, 5); // from 1 to 5 secs
            Color randomedColor = new Color();
            await Task.Run(() =>
            {
                int elapsedTime = 0; // in ms
                while (elapsedTime < timeOfGenerating * 1000)
                {
                    // probably redunant
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        randomedColor = GetRandomColor();
                        this.Background = new SolidColorBrush(randomedColor);
                    });
                    elapsedTime += TIME_BETWEEN_GENERATIONS;
                    Thread.Sleep(TIME_BETWEEN_GENERATIONS);
                }
            });
            // todo generating
            OnColorShouldBeSent(new SentColor() { Color = randomedColor });
        }

        private void OnColorShouldBeSent(SentColor color)
        {
            _transportLine.SendColor(color);
        }

        private Color GetRandomColor()
        {
            return Color.FromRgb((byte)_random.Next(1, 255),
                              (byte)_random.Next(1, 255), (byte)_random.Next(1, 255));
        }
    }
}
