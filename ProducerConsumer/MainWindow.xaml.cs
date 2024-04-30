using System.Threading.Tasks;
using System.Windows;

namespace ProducerConsumer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // cringe but idc
            transportLine1.InjectConsumer(consumer);
            transportLine2.InjectConsumer(consumer);
            transportLine3.InjectConsumer(consumer);

            producer1.InjectTransportLine(transportLine1);
            producer2.InjectTransportLine(transportLine2);
            producer3.InjectTransportLine(transportLine3);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            producer1.StartGeneratingColor();
            producer2.StartGeneratingColor();
            producer3.StartGeneratingColor();
            
        }
    }
}
