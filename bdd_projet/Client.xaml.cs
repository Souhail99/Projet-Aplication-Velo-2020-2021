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
using System.Windows.Threading;
using MySql.Data.MySqlClient;
using System.Windows.Media.Animation;
using MessageBox = System.Windows.MessageBox;
using System.Data;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace bdd_projet
{
    /// <summary>
    /// Logique d'interaction pour Client.xaml
    /// </summary>
    public partial class Client : Page
    {
        static DispatcherTimer timer = new DispatcherTimer();
        public static List<int> IDs = new List<int>();
        /*private MySqlConnection maConnexion = new MySqlConnection("SERVER=localhost;PORT=3306;" +
                                        "DATABASE=VELOMAX;" +
                                        "UID=root;PASSWORD=NEONALPHAUOMO3");*/

        private List<string> nomCompagnie = new List<string>();
        private List<int> nom = new List<int>();
        private List<int> prenom = new List<int>();
        private List<int> adresse = new List<int>();
        private List<int> mail = new List<int>();
        private List<int> tel = new List<int>();
        private List<int> nom_contact = new List<int>();
        private List<int> ahesion = new List<int>();
        private List<int> Nbachat = new List<int>();


        List<int> listeNum = new List<int> { };
        private bool numDel = false;
        private bool firstTime = true;

        private int value = 0;

        public Client()
        {
            InitializeComponent();

            //maConnexion.Open();
            TableaudesID();
        }
        private void TableaudesID()
        {
            string tid = "SELECT No_client FROM Clients";
            MySqlCommand cmd = new MySqlCommand(tid, MainWindow.maConnexion);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                //IDs.Add(Convert.ToInt32(reader["No_client"]));
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    IDs.Add(Convert.ToInt32(reader.GetValue(i).ToString()));
                }

            }
            reader.Close();

        }

        private void Click()
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation(1, 0, new TimeSpan(0, 0, 0, 0, 300));
            doubleAnimation.EasingFunction = new ExponentialEase();

            ThicknessAnimation marginAn = new ThicknessAnimation(new Thickness(0, 0, 0, 0), new Thickness(0, 0, 0, 100), new TimeSpan(0, 0, 0, 0, 500));
            marginAn.EasingFunction = new ExponentialEase();

            MainWindow.Accueil.BeginAnimation(Control.OpacityProperty, doubleAnimation);
            MainWindow.Accueil.BeginAnimation(Control.MarginProperty, marginAn);
        }

        private void Creation_Click(object sender, RoutedEventArgs e)
        {
            Click();
            timer.Tick += new EventHandler(delegate (Object o, EventArgs a)
            {
                timer.Stop();
                MainWindow.Accueil.NavigationService.Navigate(new CreationClient(listeNum, 0));
            });
            timer.Interval = timer.Interval = TimeSpan.FromSeconds(0.35);
            timer.Start();
        }

        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            Click();
            timer.Tick += new EventHandler(delegate (Object o, EventArgs a)
            {
                timer.Stop();
                MainWindow.Accueil.NavigationService.Navigate(new CreationClient(listeNum, 1));
            });
            timer.Interval = timer.Interval = TimeSpan.FromSeconds(0.35);
            timer.Start();

        }

        private void Suppr_Click(object sender, RoutedEventArgs e)
        {
            num.IsEnabled = true;
            num.Visibility = Visibility.Visible;
            del.Visibility = Visibility.Visible;
            del.IsEnabled = true;

            Submission.Opacity = 1;

            ThicknessAnimation marginAn = new ThicknessAnimation(new Thickness(-220, 0, 0, 0), new Thickness(0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 500));
            marginAn.EasingFunction = new QuadraticEase();

            DoubleAnimation doubleAnimation = new DoubleAnimation(0, 1, new TimeSpan(0, 0, 0, 0, 500));
            BackEase be = new BackEase(); be.EasingMode = EasingMode.EaseOut;
            doubleAnimation.EasingFunction = be;

            Submission.BeginAnimation(System.Windows.Controls.Control.MarginProperty, marginAn);
            Submission.BeginAnimation(System.Windows.Controls.Control.OpacityProperty, doubleAnimation);
        }

        private void num_GotFocus(object sender, RoutedEventArgs e)
        {
            if (numDel == false)
            {
                num.Text = "";
                num.FontSize = 12;
                num.TextAlignment = TextAlignment.Left;
                num.BorderBrush = Brushes.Black;
                numDel = true;
                num.Foreground = Brushes.Black;
            }
        }
        private void num_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

            if (e.Key == Key.Return)
            {
                del_Click(sender, e);
            }
        }

        private void num_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(num.Text))
            {
                num.Text = "N° du client";
                num.Foreground = Brushes.DarkGray;
                num.TextAlignment = TextAlignment.Center;
                numDel = false;
            }
        }

        private void del_Click(object sender, RoutedEventArgs e)
        {
            bool canSubmit = true;
            int val = 0;
            if (string.IsNullOrWhiteSpace(num.Text) == true || int.TryParse(num.Text, out val) == false)
            {
                canSubmit = false;
                num.Text = "Invalid argument";
                num.TextAlignment = TextAlignment.Center;
                num.BorderBrush = Brushes.Red;
                num.Foreground = Brushes.Red;
                numDel = false;
            }
            if (canSubmit)
            {
                string delTable = "DELETE FROM Clients WHERE No_client = @num";

                MySqlCommand command = MainWindow.maConnexion.CreateCommand();
                command.CommandText = delTable;
                command.Parameters.Add("@num", MySqlDbType.Int32).Value = val;

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException er )
                {
                    MessageBox.Show(er.ToString());
                    return;
                }

                command.Dispose();
                if (IDs.Contains(val) == false)
                {
                    num.Text = "Le n° doit exister";
                    num.FontSize = 10;
                    num.TextAlignment = TextAlignment.Center;
                    num.BorderBrush = Brushes.Red;
                    num.Foreground = Brushes.Red;
                    FocusManager.SetFocusedElement(FocusManager.GetFocusScope(num), null);
                    Keyboard.ClearFocus();
                    numDel = false;
                }
                if (numDel == true)
                {
                    MainWindow.Accueil.NavigationService.Navigate(new Client());
                }
            }
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            string query = "Select * from clients";
            MySqlCommand command = MainWindow.maConnexion.CreateCommand();
            command.CommandText = query;

            MySqlDataReader reader = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            dataGrid1.ItemsSource = dt.DefaultView;
            reader.Close();

            DoubleAnimation doubleAnimation = new DoubleAnimation(0, 1, new TimeSpan(0, 0, 0, 1, 500));
            doubleAnimation.EasingFunction = new ExponentialEase();

            ThicknessAnimation marginAn = new ThicknessAnimation(new Thickness(0, 20, 0, 0), new Thickness(0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 700));
            marginAn.EasingFunction = new ExponentialEase();

            MainWindow.Accueil.BeginAnimation(System.Windows.Controls.Control.OpacityProperty, doubleAnimation);
            MainWindow.Accueil.BeginAnimation(System.Windows.Controls.Control.MarginProperty, marginAn);
            firstTime = false;


        }


        #region RadioButtons
        private void prenom_Checked(object sender, RoutedEventArgs e)
        {
            if (value == 1)
            {
                string query = "Select * from Clients WHERE No_client LIKE '1%' ORDER BY prenom";
                MySqlCommand command = MainWindow.maConnexion.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                dataGrid1.ItemsSource = dt.DefaultView;
                reader.Close();
            }
            if (value == 0)
            {
                value = 2;
            }
            if (value == 2)
            {
                string query = "Select * from Clients ORDER BY prenom";
                MySqlCommand command = MainWindow.maConnexion.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                dataGrid1.ItemsSource = dt.DefaultView;
                reader.Close();
            }


        }

        private void nom_checked(object sender, RoutedEventArgs e)
        {

            if (value == 1)
            {
                string query = "Select * from Clients WHERE No_client LIKE '1%' ORDER BY nom";
                MySqlCommand command = MainWindow.maConnexion.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                dataGrid1.ItemsSource = dt.DefaultView;
                reader.Close();
            }
            if (value == 0)
            {
                string query = "Select * from Clients WHERE No_client LIKE '2%' ORDER BY nom_contact";
                MySqlCommand command = MainWindow.maConnexion.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                dataGrid1.ItemsSource = dt.DefaultView;
                reader.Close();
            }
            if (value == 2)
            {
                string query = "Select * from Clients ORDER BY nom";
                MySqlCommand command = MainWindow.maConnexion.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                dataGrid1.ItemsSource = dt.DefaultView;
                reader.Close();
            }

        }

        private void num_checked(object sender, RoutedEventArgs e)
        {
            if (!firstTime)
            {
                if (value == 0)
                {
                    string query = "Select * from Clients WHERE No_client LIKE '2%' ORDER BY No_client";
                    MySqlCommand command = MainWindow.maConnexion.CreateCommand();
                    command.CommandText = query;
                    MySqlDataReader reader = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    dataGrid1.ItemsSource = dt.DefaultView;
                    reader.Close();
                }
                if (value == 1)
                {
                    string query = "Select * from Clients WHERE No_client LIKE '1%' ORDER BY No_client";
                    MySqlCommand command = MainWindow.maConnexion.CreateCommand();
                    command.CommandText = query;
                    MySqlDataReader reader = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    dataGrid1.ItemsSource = dt.DefaultView;
                    reader.Close();
                }
                if (value == 2)
                {
                    string query = "Select * from Clients  No_client";
                    MySqlCommand command = MainWindow.maConnexion.CreateCommand();
                    command.CommandText = query;
                    MySqlDataReader reader = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    dataGrid1.ItemsSource = dt.DefaultView;
                    reader.Close();
                }
            }
        }

        private void nomcompagnie_Checked(object sender, RoutedEventArgs e)
        {
            if (value == 1)
            {
                value = 2;
            }
            if (value == 0)
            {
                string query = "Select * from Clients WHERE No_client LIKE '2%' ORDER BY nom_compagnie";
                MySqlCommand command = MainWindow.maConnexion.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                dataGrid1.ItemsSource = dt.DefaultView;
                reader.Close();
            }
            if (value == 2)
            {
                string query = "Select * from Clients ORDER BY nom_compagnie";
                MySqlCommand command = MainWindow.maConnexion.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                dataGrid1.ItemsSource = dt.DefaultView;
                reader.Close();
            }
        }


        private void entreprise_checked(object sender, RoutedEventArgs e)
        {
            value = 0;
        }

        private void particulier_Checked(object sender, RoutedEventArgs e)
        {
            value = 1;
        }

        private void lesdeux_checked(object sender, RoutedEventArgs e)
        {
            value = 2;
        }

        #endregion

        private void dataGrid1_CopyingRowClipboardContent(object sender, DataGridRowClipboardEventArgs e)
        {

        }


    }
}

