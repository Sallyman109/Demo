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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace TripCalculatorFrontEndWpfApp
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

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WebServiceClient client = new WebServiceClient();
                float Cost;
                if (!float.TryParse(CostTextBox.Text, out Cost))
                {
                    MessageBox.Show("Cost has to be numeric");
                    return;
                }
                bool postWasSuccessful = await client.PostCost(UserTextBox.Text, Cost);
                if (postWasSuccessful)
                {
                    UserTextBox.Text = ""; CostTextBox.Text = "";
                    MessageBox.Show("The POST was successful");
                }
                else
                {
                    MessageBox.Show("The POST was not successful");
                }
            }
            catch (Exception) { MessageBox.Show("Error Occurred"); }
        }

        private async void ReceiveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WebServiceClient client = new WebServiceClient();
                List<MoneyTransferred> ListOfMoneyTransferred = await client.GetWhoOwesWhoAsync();
                StringBuilder TextBoxString = new StringBuilder();
                foreach (MoneyTransferred moneyTransferred in ListOfMoneyTransferred)
                {
                    TextBoxString.AppendLine("" + moneyTransferred);
                }
                Window newWindow = new Window();
                newWindow.Height = 300;
                newWindow.Width = 350;
                StackPanel stackPanel = new StackPanel { Orientation = Orientation.Vertical };
                stackPanel.Children.Add(new TextBlock { Text = TextBoxString.ToString() });
                newWindow.Content = stackPanel;
                newWindow.Show();
            }
            catch (Exception) { MessageBox.Show("Error Occurred"); }
}
    }
}
