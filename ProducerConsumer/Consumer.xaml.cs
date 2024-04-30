using ProducerConsumer.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProducerConsumer
{
    public partial class Consumer : Border
    {
        private const int MAX_COLORS = 3;

        private readonly List<SentColor> _colors = new List<SentColor>();

        public Consumer()
        {
            InitializeComponent();
        }

        public void AddColor(SentColor color)
        {
            _colors.Add(color);
            if (_colors.Count == MAX_COLORS)
            {
                ApplyColors();
                _colors.Clear();
            }
        }

        private void ApplyColors()
        {
            Debug.Assert(_colors.Count == MAX_COLORS);

            Color newColor = new Color() 
            {
                A = 255,
                R = (byte)((_colors[0].Color.R + _colors[1].Color.R + _colors[2].Color.R) / 3),
                G = (byte)((_colors[0].Color.G + _colors[1].Color.G + _colors[2].Color.G) / 3),
                B = (byte)((_colors[0].Color.B + _colors[1].Color.B + _colors[2].Color.B) / 3),
            };

            this.Background = new SolidColorBrush(newColor);
        }
    }
}
