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
using System.Data;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace bdd_projet
{
    //logs de l'adresse mail : maxlegrand404@gmail.com
    //mot de passe : LFLtKtjib7o&4E&D (utilisez copié/collé)

    public partial class Pieces : Page
    {
        private bool numDel = false;
        private bool siretDel = false;
        private bool firstTime = true;

        string tampnum = "";
        string tampsiret = "";

        public Pieces()
        {
            InitializeComponent();
            Missing.FontSize = 18;
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
                MainWindow.Accueil.NavigationService.Navigate(new CreationPiece(0));
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
                MainWindow.Accueil.NavigationService.Navigate(new CreationPiece(1));
            });
            timer.Interval = TimeSpan.FromSeconds(0.35);
            timer.Start();
        }
        private void Suppr_Click(object sender, RoutedEventArgs e)
        {
            num.IsEnabled = true;
            siret.IsEnabled = true;
            num.Visibility = Visibility.Visible;
            del.Visibility = Visibility.Visible;
            siret.Visibility = Visibility.Visible;
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

            if (string.IsNullOrWhiteSpace(num.Text) == true || num.Text == "N° du produit" || num.Text == "Argument invalide" || num.Text == "Le n° doit exister")
            {
                canSubmit = false;
                tampnum = num.Text;
                num.Text = "Argument invalide";
                num.TextAlignment = TextAlignment.Center;
                num.BorderBrush = Brushes.Red;
                num.Foreground = Brushes.Red;
                numDel = false;
            }
            if (string.IsNullOrWhiteSpace(siret.Text) == true || int.TryParse(String.Concat(siret.Text.Where(c => !Char.IsWhiteSpace(c))), out val) == false
                || siret.Text == "Argument invalide" || siret.Text == "Le n° doit exister")
            {
                canSubmit = false;
                tampsiret = siret.Text;
                siret.Text = "Argument invalide";
                siret.TextAlignment = TextAlignment.Center;
                siret.BorderBrush = Brushes.Red;
                siret.Foreground = Brushes.Red;
                siretDel = false;
            }
            if (canSubmit)
            {
                string delTable = "DELETE FROM pieces WHERE numPiece = @num AND Siret = @siret";

                MySqlCommand command = MainWindow.maConnexion.CreateCommand();
                command.CommandText = delTable;
                command.Parameters.Add("@num", MySqlDbType.VarChar).Value = String.Concat(num.Text.ToLower().Where(c => !Char.IsWhiteSpace(c)));
                command.Parameters.Add("@siret", MySqlDbType.VarChar).Value = String.Concat(siret.Text.ToLower().Where(c => !Char.IsWhiteSpace(c))); ;

                delTable = "SELECT * FROM pieces WHERE numPiece = @num AND Siret = @siret";
                MySqlCommand com = MainWindow.maConnexion.CreateCommand();
                com.CommandText = delTable;
                com.Parameters.Add("@num", MySqlDbType.VarChar).Value = String.Concat(num.Text.ToLower().Where(c => !Char.IsWhiteSpace(c)));
                com.Parameters.Add("@siret", MySqlDbType.VarChar).Value = String.Concat(siret.Text.ToLower().Where(c => !Char.IsWhiteSpace(c))); ;
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

                if (ans == "False")
                {
                    tampnum = num.Text;
                    num.Text = "Le n° doit exister";
                    num.TextAlignment = TextAlignment.Center;
                    num.BorderBrush = Brushes.Red;
                    num.Foreground = Brushes.Red;
                    FocusManager.SetFocusedElement(FocusManager.GetFocusScope(num), null);
                    numDel = false;

                    tampsiret = siret.Text;
                    siret.Text = "Le n° doit exister";
                    siret.TextAlignment = TextAlignment.Center;
                    siret.BorderBrush = Brushes.Red;
                    siret.Foreground = Brushes.Red;
                    FocusManager.SetFocusedElement(FocusManager.GetFocusScope(siret), null);
                    siretDel = false;

                    Keyboard.ClearFocus();
                }

                command.Dispose();

                if (ans == "True")
                {
                    command.CommandText = "SELECT * FROM PIECES WHERE numPiece = @num";
                    MySqlDataReader nb = command.ExecuteReader();
                    ans = nb.Read().ToString();
                    nb.Close();
                    if (ans == "False")
                    {
                        delTable = "DELETE V FROM VELO V " +
                                          "JOIN ASSEMBLAGE AS A ON a.numVelo = v.numProduit WHERE @num " +
                                          "IN(a.cadre, a.guidon, a.frein, a.selle, a.derailleurAvant, a.derailleurArriere, a.roueAvant, a.roueArriere, a.reflecteurs, a.pedalier, a.ordinateur, a.panier); ";

                        command.CommandText = delTable;
                        command.ExecuteNonQuery();
                        command.Dispose();
                    }
                    MainWindow.Accueil.NavigationService.Navigate(new Pieces());
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

        private void Pieces_Loaded(object sender, RoutedEventArgs e)
        {
            string query = "Select * from pieces ORDER BY Siret";
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

            bool nul = false;

            query = "SELECT numPiece, SUM(quantite) from pieces as p " +
                    "GROUP BY numPiece " +
                    "HAVING SUM(quantite) = 0;";
            command = MainWindow.maConnexion.CreateCommand();
            command.CommandText = query;

            reader = command.ExecuteReader();
            string str = "";
            int count = 0;
            while (reader.Read())
            {
                str += reader.GetString(0) + ", ";
                count++;
                if (count % 8 == 0) 
                { 
                    str += "\n";
                    Missing.FontSize = 14;
                }
                nul = true;
            }
            str = str.Length > 0 ? str.Substring(0, str.Length - 2) : str;
            if (nul) { str += count == 1 ? " n'est plus disponible" : " ne sont plus disponibles"; Careful.Visibility = Visibility.Visible; }
            reader.Close();

            Missing.Text = str;
        }
        DispatcherTimer timer = new DispatcherTimer();
        private void siret_GotFocus(object sender, RoutedEventArgs e)
        {
            if (siretDel == false)
            {
                siret.Text = tampsiret;
                siret.FontSize = 12;
                siret.TextAlignment = TextAlignment.Left;
                siret.BorderBrush = Brushes.Black;
                siretDel = true;
                siret.Foreground = Brushes.Black;
            }
        }

        private void siret_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(siret.Text))
            {
                siret.Text = "Siret";
                siret.Foreground = Brushes.DarkGray;
                siret.TextAlignment = TextAlignment.Center;
                siretDel = false;
            }
        }

        private void siret_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                del_Click(sender, e);
            }
        }
        private void dataGrid1_CopyingRowClipboardContent(object sender, DataGridRowClipboardEventArgs e)
        {

        }
    }
}
