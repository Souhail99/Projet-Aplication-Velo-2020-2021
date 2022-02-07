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
    /// <summary>
    /// Logique d'interaction pour ImprimePiece.xaml
    /// </summary>
    public partial class ImprimePiece : Page
    {
        private bool firstTime = true;
        bool numCoDel = false;
        public ImprimePiece()
        {
            InitializeComponent();
        }
        private void numCo_GotFocus(object sender, RoutedEventArgs e)
        {
            if (numCoDel == false)
            {
                numCo.Text = "";
                numCo.FontSize = 12;
                numCo.TextAlignment = TextAlignment.Left;
                numCo.BorderBrush = Brushes.Black;
                numCoDel = true;
                numCo.Foreground = Brushes.Black;
            }
        }
        private void numCo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(numCo.Text))
            {
                numCo.Text = "N° de commande";
                numCo.Foreground = Brushes.DarkGray;
                numCo.TextAlignment = TextAlignment.Left;
                numCoDel = false;
                numCo.BorderBrush = Brushes.DarkGray;
            }
        }
        DispatcherTimer timer = new DispatcherTimer();
        private void Verif_Click(object sender, RoutedEventArgs e)
        {
            timer.Tick += new EventHandler(delegate (Object o, EventArgs a)
            {
                timer.Stop();
                MainWindow.Accueil.NavigationService.Navigate(new Pieces());
            });
            timer.Interval = TimeSpan.FromSeconds(0.35);
            timer.Start();
        }
        private void Ok_Click(object send, RoutedEventArgs e)
        {
            bool canSubmit = true;

            //int numQ = 0;

            if (string.IsNullOrWhiteSpace(numCo.Text) == true || numCo.Text == "N° de commande" || numCo.Text == "Invalid argument")
            {
                canSubmit = false;
                numCo.Text = "Invalid argument";
                numCo.TextAlignment = TextAlignment.Center;
                numCo.BorderBrush = Brushes.Red;
                numCo.Foreground = Brushes.Red;
                numCoDel = false;
            }

            if (canSubmit)
            {
                string query = "select v.nom, v.grandeur,cadre,guidon,frein,selle,derailleurAvant,derailleurArriere,roueAvant,roueArriere,reflecteurs,pedalier,ordinateur,panier from assemblage a, itemCommande i, velo v where i.Ref_item=a.numVelo and v.numProduit=i.Ref_item and No_commande=@numC";
                MySqlCommand command = MainWindow.maConnexion.CreateCommand();
                command.CommandText = query;
                command.Parameters.Add("@numC", MySqlDbType.VarChar).Value = numCo.Text.ToUpper();
                MySqlDataReader reader = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                dataGrid1.ItemsSource = dt.DefaultView;

                //command.Parameters.Add("@numC", MySqlDbType.VarChar).Value = numCo.Text.ToUpper();

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException er)
                {
                    if (er.Code == 0)
                    {
                        numCo.Text = "Le n° doit être unique";
                        numCo.FontSize = 10;
                        numCo.TextAlignment = TextAlignment.Center;
                        numCo.BorderBrush = Brushes.Red;
                        numCo.Foreground = Brushes.Red;
                        numCoDel = false;
                    }
                    return;
                }

            }
        }
        private void ImprimePiece_Loaded(object sender, RoutedEventArgs e)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation(0, 1, new TimeSpan(0, 0, 0, 0, 300));
            doubleAnimation.EasingFunction = new ExponentialEase();

            ThicknessAnimation marginAn = new ThicknessAnimation(new Thickness(0, 20, 0, 0), new Thickness(0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 500));
            marginAn.EasingFunction = new ExponentialEase();

            MainWindow.Accueil.BeginAnimation(Control.OpacityProperty, doubleAnimation);
            MainWindow.Accueil.BeginAnimation(Control.MarginProperty, marginAn);

            firstTime = false;
        }
    }
}
