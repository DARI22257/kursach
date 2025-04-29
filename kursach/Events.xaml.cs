using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using kursach.VM;

namespace kursach
{
    /// <summary>
    /// Логика взаимодействия для Events.xaml
    /// </summary>
    public partial class Events : Window
    {
        public Events(Model.Client selectedClient)
        {
            InitializeComponent();
            //((EventsMvvm)this.DataContext).SetClose(Close);
            //((EventsMvvm)this.DataContext).SetClient(selectedClient);
        }
        private void GoToNextPage_Click(object sender, RoutedEventArgs e)
        {
            Tasks tasks = new Tasks();
            tasks.Show();
        }
    }
}
