using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace kursach
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void LoadHolidays()
        {
            List<string> holidays = new List<string>
            {
                "1 января — Новый год",
                "7 января — Рождество Христово",
                "23 февраля — День защитника Отечества",
                "8 марта — Международный женский день",
                "1 мая — Праздник Весны и Труда",
                "9 мая — День Победы"
            };

            HolidaysListBox.ItemsSource = holidays;
        }

        private void NavigateButton_Click(object sender, RoutedEventArgs e)
        {
             Clients clients = new Clients();
            clients.Show();
        }
    }
}