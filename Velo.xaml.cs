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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows.Threading;

namespace bdd_projet
{
    /// <summary>
    /// Logique d'interaction pour Velo.xaml
    /// </summary>
    public partial class Velo : Page
    {
        private bool numDel = false;
        private bool firstTime = true;

        string tampnum = "";

        public Velo()
        {
            InitializeComponent();
        }

        #region Click and Focus
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
                MainWindow.Accueil.NavigationService.Navigate(new CreationVelo(0));
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
                MainWindow.Accueil.NavigationService.Navigate(new CreationVelo(1));
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

            Submission.BeginAnimation(Control.MarginProperty, marginAn);
            Submission.BeginAnimation(Control.OpacityProperty, doubleAnimation);
        }
        private void del_Click(object sender, RoutedEventArgs e)
        {
            bool canSubmit = true;
            int val = 0;

            if (string.IsNullOrWhiteSpace(num.Text) == true || int.TryParse(num.Text, out val) == false)
            {
                canSubmit = false;
                tampnum = num.Text;
                num.Text = "Argument invalide";
                num.TextAlignment = TextAlignment.Center;
                num.BorderBrush = Brushes.Red;
                num.Foreground = Brushes.Red;
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(num), null);
                Keyboard.ClearFocus();
                numDel = false;
            }
            if (canSubmit)
            {
                string delTable = "DELETE FROM Velo WHERE numProduit = @num";

                MySqlCommand command = MainWindow.maConnexion.CreateCommand();
                command.CommandText = delTable;
                command.Parameters.Add("@num", MySqlDbType.Int32).Value = val;

                delTable = "SELECT * FROM velo WHERE numProduit = @num";
                MySqlCommand com = MainWindow.maConnexion.CreateCommand();
                com.CommandText = delTable;
                com.Parameters.Add("@num", MySqlDbType.Int32).Value = val;
                MySqlDataReader reader = com.ExecuteReader();
                string ans = reader.Read().ToString();
                reader.Close();

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException)
                {
                    return;
                }

                command.Dispose();
                if (ans == "False")
                {
                    tampnum = num.Text;
                    num.Text = "Le n° doit exister";
                    num.FontSize = 12;
                    num.TextAlignment = TextAlignment.Center;
                    num.BorderBrush = Brushes.Red;
                    num.Foreground = Brushes.Red;
                    FocusManager.SetFocusedElement(FocusManager.GetFocusScope(num), null);
                    Keyboard.ClearFocus();
                    numDel = false;
                }
                if (ans == "True")
                {
                    MainWindow.Accueil.NavigationService.Navigate(new Velo());
                }
            }
        }

        private void num_GotFocus(object sender, RoutedEventArgs e)
        {
            if (numDel == false)
            {
                num.Text = tampnum;
                num.FontSize = 12;
                num.TextAlignment = TextAlignment.Left;
                num.BorderBrush = Brushes.Black;
                numDel = true;
                num.Foreground = Brushes.Black;
            }
        }

        private void num_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(num.Text))
            {
                num.Text = "N° du produit";
                num.Foreground = Brushes.DarkGray;
                num.TextAlignment = TextAlignment.Center;
                numDel = false;
            }
        }

        private void num_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                del_Click(sender, e);
            }
        }
        #endregion

        DispatcherTimer timer = new DispatcherTimer();
        private void Velo_Loaded(object sender, RoutedEventArgs e)
        {
            string query = "Select * from Velo";
            MySqlCommand command = MainWindow.maConnexion.CreateCommand();
            command.CommandText = query;

            MySqlDataReader reader = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            dataGrid1.ItemsSource = dt.DefaultView;

            DoubleAnimation doubleAnimation = new DoubleAnimation(0, 1, new TimeSpan(0, 0, 0, 2, 0));
            doubleAnimation.EasingFunction = new ExponentialEase();

            ThicknessAnimation marginAn = new ThicknessAnimation(new Thickness(0, 20, 0, 0), new Thickness(0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 400));
            marginAn.EasingFunction = new ExponentialEase();

            MainWindow.Accueil.BeginAnimation(Control.OpacityProperty, doubleAnimation);
            MainWindow.Accueil.BeginAnimation(Control.MarginProperty, marginAn);

            firstTime = false;
        }

        #region RadioButtons
        private void categorie_checked(object sender, RoutedEventArgs e)
        {
            string query = "Select * from Velo ORDER BY type";
            MySqlCommand command = MainWindow.maConnexion.CreateCommand();
            command.CommandText = query;

            MySqlDataReader reader = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            dataGrid1.ItemsSource = dt.DefaultView;
        }

        private void nom_checked(object sender, RoutedEventArgs e)
        {
            string query = "Select * from Velo ORDER BY nom";
            MySqlCommand command = MainWindow.maConnexion.CreateCommand();
            command.CommandText = query;

            MySqlDataReader reader = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            dataGrid1.ItemsSource = dt.DefaultView;
        }

        private void num_checked(object sender, RoutedEventArgs e)
        {
            if (!firstTime)
            {
                string query = "Select * from Velo ORDER BY numProduit";
                MySqlCommand command = MainWindow.maConnexion.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                dataGrid1.ItemsSource = dt.DefaultView;
            }
        }
        #endregion

        private void dataGrid1_CopyingRowClipboardContent(object sender, DataGridRowClipboardEventArgs e)
        {

        }
    }
}
