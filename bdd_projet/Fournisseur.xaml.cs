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
using MySql.Data.MySqlClient;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using MessageBox = System.Windows.MessageBox;
using System.Data;
using Control = System.Windows.Controls.Control;
using TextBox = System.Windows.Controls.TextBox;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace bdd_projet
{
    /// <summary>
    /// Logique d'interaction pour Fournisseur.xaml
    /// </summary>
    public partial class Fournisseur : Page
    {
        static DispatcherTimer timer = new DispatcherTimer();
        private List<int> listeNum = new List<int> { };
        public static List<int> lesSiret = new List<int> { };


        private bool numDel = false;
        private bool firstTime = true;
        public Fournisseur()
        {
            InitializeComponent();
            TableaudesSiret();
        }
        private void TableaudesSiret()
        {
            string tid = "SELECT Siret FROM Fournisseur";
            MySqlCommand cmd = new MySqlCommand(tid, MainWindow.maConnexion);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                //IDs.Add(Convert.ToInt32(reader["No_client"]));
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    lesSiret.Add(Convert.ToInt32(reader.GetValue(i).ToString()));
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
                MainWindow.Accueil.NavigationService.Navigate(new CreationFournisseur(listeNum, 0));
            });
            timer.Interval = TimeSpan.FromSeconds(0.35);
            timer.Start();
        }

        private void Modify_Click(object sender, RoutedEventArgs e)
        {

            Click();
            timer.Tick += new EventHandler(delegate (Object o, EventArgs a)
            {
                timer.Stop();
                MainWindow.Accueil.NavigationService.Navigate(new CreationFournisseur(listeNum, 1));
            });
            timer.Interval = TimeSpan.FromSeconds(0.35);
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
                num.Text = "N° du Siret";
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
                string delTable = "DELETE FROM Fournisseur WHERE Siret = @num";

                MySqlCommand command = MainWindow.maConnexion.CreateCommand();
                command.CommandText = delTable;
                command.Parameters.Add("@num", MySqlDbType.Int32).Value = val;

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException er)
                {
                    return;
                }
                command.Dispose();

                if (listeNum.Contains(val) == false)
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
                    MainWindow.Accueil.NavigationService.Navigate(new Fournisseur());
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            string query = "Select * from fournisseur";
            MySqlCommand command = MainWindow.maConnexion.CreateCommand();
            command.CommandText = query;

            MySqlDataReader reader = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            dataGrid1.ItemsSource = dt.DefaultView;
            reader.Close();
            command.Dispose();

            MySqlCommand com = MainWindow.maConnexion.CreateCommand();
            com.CommandText = "SELECT DISTINCT siret FROM fournisseur";
            reader = com.ExecuteReader();
            while (reader.Read())
            {
                listeNum.Add(Int32.Parse(reader.GetString(0)));
            }
            com.Dispose();
            reader.Close();

            DoubleAnimation doubleAnimation = new DoubleAnimation(0, 1, new TimeSpan(0, 0, 0, 2, 0));
            doubleAnimation.EasingFunction = new ExponentialEase();

            ThicknessAnimation marginAn = new ThicknessAnimation(new Thickness(0, 20, 0, 0), new Thickness(0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 400));
            marginAn.EasingFunction = new ExponentialEase();

            MainWindow.Accueil.BeginAnimation(Control.OpacityProperty, doubleAnimation);
            MainWindow.Accueil.BeginAnimation(Control.MarginProperty, marginAn);

            firstTime = false;
        }

        #region RadioButtons
        private void Siret_checked(object sender, RoutedEventArgs e)
        {
            if (!firstTime)
            {
                string query = "Select * from Fournisseur ORDER BY Siret";
                MySqlCommand command = MainWindow.maConnexion.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                dataGrid1.ItemsSource = dt.DefaultView;
                reader.Close();
            }
        }

        private void Nom_Checked(object sender, RoutedEventArgs e)
        {


            string query = "Select * from Fournisseur ORDER BY nom_Entreprise";
            MySqlCommand command = MainWindow.maConnexion.CreateCommand();
            command.CommandText = query;

            MySqlDataReader reader = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            dataGrid1.ItemsSource = dt.DefaultView;
            reader.Close();



        }

        private void Label_checked(object sender, RoutedEventArgs e)
        {

            string query = "Select * from Fournisseur ORDER BY Label";
            MySqlCommand command = MainWindow.maConnexion.CreateCommand();
            command.CommandText = query;
            MySqlDataReader reader = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            dataGrid1.ItemsSource = dt.DefaultView;
            reader.Close();

        }

        #endregion

        private void dataGrid1_CopyingRowClipboardContent(object sender, DataGridRowClipboardEventArgs e)
        {

        }

    }
}
