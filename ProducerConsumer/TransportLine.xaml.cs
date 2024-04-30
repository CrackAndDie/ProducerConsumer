using ProducerConsumer.Models;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProducerConsumer
{
    public partial class TransportLine : UserControl
    {
        public SentColor CurrentColor
        {
            get { return (SentColor)GetValue(CurrentColorProperty); }
            set { SetValue(CurrentColorProperty, value); }
        }

        public static readonly DependencyProperty CurrentColorProperty = DependencyProperty.Register(
                  nameof(CurrentColor),
                  typeof(SentColor),
                  typeof(TransportLine),
                  new PropertyMetadata(null)
              );

        private Consumer _consumer;
        private SentColor _colorToSend;

        private const int TIME_OF_COLOR_TRANSPORT = 4; // in secs

        public TransportLine()
        {
            InitializeComponent();

            DependencyPropertyDescriptor
                .FromProperty(TransportLine.CurrentColorProperty, typeof(TransportLine))
                .AddValueChanged(this, (s, e) => 
                {
                    Debug.WriteLine("ASdasdasd");
                });
        }

        // just ioc cause idc
        public void InjectConsumer(Consumer consumer)
        {
            _consumer = consumer;
        }

        async public void SendColor(SentColor color)
        {
            _colorToSend = color;
            await StartTransport();
            OnColorShouldBeSent();
            await RollBack();
        }

        async private Task StartTransport()
        {
            transportingCircle.Background = new SolidColorBrush(_colorToSend.Color);
            double circleWidth = transportingCircle.ActualWidth;
            double controlWidth = this.ActualWidth - circleWidth;
            double delta = controlWidth / (TIME_OF_COLOR_TRANSPORT * 1000);
            await Task.Run(() =>
            {
                Thickness currMargin = new Thickness();
                while (currMargin.Left < controlWidth)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        currMargin = new Thickness(currMargin.Left + delta, 0, 0, 0);
                        transportingCircle.Margin = currMargin;
                    });
                    Thread.Sleep(1);
                }
            });
        }

        private void OnColorShouldBeSent()
        {
            _consumer.AddColor(_colorToSend);
        }

        async private Task RollBack()
        {
            transportingCircle.Background = Brushes.Transparent;
            double controlWidth = this.ActualWidth;
            double delta = controlWidth / 1000;
            Thickness startMargin = transportingCircle.Margin;
            await Task.Run(() =>
            {
                Thickness currMargin = startMargin;
                while (currMargin.Left > 0)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        currMargin = new Thickness(currMargin.Left - delta, 0, 0, 0);
                        transportingCircle.Margin = currMargin;
                    });
                    Thread.Sleep(1);
                }
            });
        }
    }
}
