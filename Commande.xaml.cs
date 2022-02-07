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
    /// Logique d'interaction pour Commande.xaml
    /// </summary>
    public partial class Commande : Page
    {
        private bool numDel = false;
        public List<string> listeNum = new List<string> { };
        private bool firstTime = true;
        public Commande()
        {
            InitializeComponent();

            firstTime = false;
        }
        #region Click and Focus
        private void Click(int val)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation(1, 0, new TimeSpan(0, 0, 0, 0, 300));
            doubleAnimation.EasingFunction = new ExponentialEase();

            ThicknessAnimation marginAn = new ThicknessAnimation(new Thickness(0, 0, 0, 0), new Thickness(0, 0, 0, 100), new TimeSpan(0, 0, 0, 0, 500));
            marginAn.EasingFunction = new ExponentialEase();

            MainWindow.Accueil.BeginAnimation(Control.OpacityProperty, doubleAnimation);
            MainWindow.Accueil.BeginAnimation(Control.MarginProperty, marginAn);

            Loading(val);
        }
        private void Creation_Click(object sender, RoutedEventArgs e)
        {
            Click(0);
        }
        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            Click(1);
        }
        private void Suppr_Click(object sender, RoutedEventArgs e)
        {
            num.IsEnabled = true;
            num.Visibility = Visibility.Visible;
            del.Visibility = Visibility.Visible;
            del.IsEnabled = true;

            Submission.Opacity = 1;

            ThicknessAnimation marginAn = new ThicknessAnimation(new Thickness(0, -20, 0, 0), new Thickness(0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 300));
            marginAn.EasingFunction = new QuadraticEase();
            DoubleAnimation doubleAnimation = new DoubleAnimation(0, 1, new TimeSpan(0, 0, 0, 0, 500));
            BackEase be = new BackEase(); be.EasingMode = EasingMode.EaseOut;
            doubleAnimation.EasingFunction = be;

            Submission.BeginAnimation(Control.MarginProperty, marginAn);
            Submission.BeginAnimation(Control.OpacityProperty, doubleAnimation);
            Panel.SetZIndex(Submission, 2);
        }
        private void Detail_Click(object sender, RoutedEventArgs e)
        {
            Click(2);
        }
        private void del_Click(object sender, RoutedEventArgs e)
        {
            bool canSubmit = true;

            if (string.IsNullOrWhiteSpace(num.Text))
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

                string request = "SELECT * FROM COMMANDE WHERE No_commande=@num";

                MySqlCommand check = MainWindow.maConnexion.CreateCommand();
                check.CommandText = request;
                check.Parameters.Add("num", MySqlDbType.VarChar).Value = num.Text.ToLower();
                MySqlDataReader reader = check.ExecuteReader();
                string ans = reader.Read().ToString();
                reader.Close();
                check.Dispose();

                string delTable = "DELETE FROM commande WHERE No_commande = @num";

                MySqlCommand command = MainWindow.maConnexion.CreateCommand();
                command.CommandText = delTable;
                command.Parameters.Add("@num", MySqlDbType.String).Value = num.Text.ToUpper();

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException er)
                {
                    MessageBox.Show(er.ToString());
                    return;
                }

                command.Dispose();

                if (ans == "False")
                {
                    num.Text = "Le n° doit exister";
                    num.TextAlignment = TextAlignment.Center;
                    num.BorderBrush = Brushes.Red;
                    num.Foreground = Brushes.Red;
                    FocusManager.SetFocusedElement(FocusManager.GetFocusScope(num), null);
                    Keyboard.ClearFocus();
                    numDel = false;
                }
                reader.Close();
                if (numDel == true)
                {
                    MainWindow.Accueil.NavigationService.Navigate(new Commande());
                }
            }
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
        private void num_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(num.Text))
            {
                num.Text = "N° de la commande";
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

        private void Commande_Loaded(object sender, RoutedEventArgs e)
        {
            string query = "Select * from commande";
            MySqlCommand command = MainWindow.maConnexion.CreateCommand();
            command.CommandText = query;

            MySqlDataReader reader = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            dataGrid1.ItemsSource = dt.DefaultView;

            command.Dispose();
            num_checked(sender, e);
            DoubleAnimation doubleAnimation = new DoubleAnimation(0, 1, new TimeSpan(0, 0, 0, 2, 0));
            doubleAnimation.EasingFunction = new ExponentialEase();

            ThicknessAnimation marginAn = new ThicknessAnimation(new Thickness(0, 20, 0, 0), new Thickness(0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 400));
            marginAn.EasingFunction = new ExponentialEase();

            MainWindow.Accueil.BeginAnimation(Control.OpacityProperty, doubleAnimation);
            MainWindow.Accueil.BeginAnimation(Control.MarginProperty, marginAn);
            num_checked(sender, e);
        }
        DispatcherTimer timer = new DispatcherTimer();
        void Loading(int val) //0 pour CreationCommande, 1 pour ModifCommande, 2 itemComande
        {
            if (val == 2)
            {
                timer.Tick += new EventHandler(delegate (Object o, EventArgs a)
                {
                    timer.Stop();
                    MainWindow.Accueil.NavigationService.Navigate(new ItemCommande());

                });
                timer.Interval = TimeSpan.FromSeconds(0.35);
                timer.Start();
            }
            else if (val == 0)
            {
                timer.Tick += new EventHandler(delegate (Object o, EventArgs a)
                {
                    timer.Stop();
                    MainWindow.Accueil.NavigationService.Navigate(new CreationCommande(val));
                });
                timer.Interval = TimeSpan.FromSeconds(0.35);
                timer.Start();
            }
            else
            {
                timer.Tick += new EventHandler(delegate (Object o, EventArgs a)
                {
                    timer.Stop();
                    MainWindow.Accueil.NavigationService.Navigate(new CreationCommande(val));
                });
                timer.Interval = TimeSpan.FromSeconds(0.35);
                timer.Start();
            }

        }
        int val;
        private void dateL_checked(object sender, RoutedEventArgs e)
        {
            string query = "";
            if (val == 0)
            {
                query = "Select c.No_commande,cl.nom,prenom,Ref_item,v.nom,v.prix,date_commande,date_livraison,c.adresse_livraison from commande c,clients cl,itemcommande i,velo v where c.No_client=cl.No_client and  i.No_commande=c.No_commande and v.numProduit=i.Ref_item order by date_livraison";
            }
            else
            {
                query = "Select c.No_commande,prenom,Ref_item,p.descr,p.prix,date_commande,date_livraison,c.adresse_livraison from commande c,clients cl,itemcommande i,pieces p where c.No_client=cl.No_client and  i.No_commande=c.No_commande and p.numPiece=i.Ref_item order by date_livraison";
            }

            MySqlCommand command = MainWindow.maConnexion.CreateCommand();
            command.CommandText = query;

            MySqlDataReader reader = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            dataGrid1.ItemsSource = dt.DefaultView;
        }

        private void nom_checked(object sender, RoutedEventArgs e)
        {
            string query = "";
            if (val == 0)
            {
                query = "Select c.No_commande,cl.nom,prenom,Ref_item,v.nom,v.prix,date_commande,date_livraison,c.adresse_livraison from commande c,clients cl,itemcommande i,velo v where c.No_client=cl.No_client and  i.No_commande=c.No_commande and v.numProduit=i.Ref_item order by cl.nom";
            }
            else
            {
                query = "Select c.No_commande,cl.nom,prenom,Ref_item,p.descr,p.prix,date_commande,date_livraison,c.adresse_livraison from commande c,clients cl,itemcommande i,pieces p where c.No_client=cl.No_client and  i.No_commande=c.No_commande and p.numPiece=i.Ref_item order by cl.nom";
            }
            MySqlCommand command = MainWindow.maConnexion.CreateCommand();
            command.CommandText = query;

            MySqlDataReader reader = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            dataGrid1.ItemsSource = dt.DefaultView;
        }

        private void prix_checked(object sender, RoutedEventArgs e)
        {
            string query = "";
            if (val == 0)
            {
                query = "Select c.No_commande,cl.nom,prenom,Ref_item,v.nom,v.prix,date_commande,date_livraison,c.adresse_livraison from commande c,clients cl,itemcommande i,velo v where c.No_client=cl.No_client and  i.No_commande=c.No_commande and v.numProduit=i.Ref_item order by prix";
            }
            else
            {
                query = "Select c.No_commande,prenom,Ref_item,p.descr,p.prix,date_commande,date_livraison,c.adresse_livraison from commande c,clients cl,itemcommande i,pieces p where c.No_client=cl.No_client and  i.No_commande=c.No_commande and p.numPiece=i.Ref_item order by prix";
            }
            MySqlCommand command = MainWindow.maConnexion.CreateCommand();
            command.CommandText = query;

            MySqlDataReader reader = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            dataGrid1.ItemsSource = dt.DefaultView;
        }

        private void vel_checked(object sender, RoutedEventArgs e)
        {
            val = 0;
        }

        private void piece_checked(object sender, RoutedEventArgs e)
        {
            val = 1;
        }

        private void num_checked(object sender, RoutedEventArgs e)
        {
            if (!firstTime)
            {
                string query = "";
                if (val == 0)
                {
                    query = "Select c.No_commande,cl.nom,prenom,Ref_item,v.nom,v.prix,date_commande,date_livraison,c.adresse_livraison from commande c,clients cl,itemcommande i,velo v where c.No_client=cl.No_client and  i.No_commande=c.No_commande and v.numProduit=i.Ref_item order by No_commande";
                }
                else
                {
                    query = "Select c.No_commande,prenom,Ref_item,p.descr,p.prix,date_commande,date_livraison,c.adresse_livraison from commande c,clients cl,itemcommande i,pieces p where c.No_client=cl.No_client and  i.No_commande=c.No_commande and p.numPiece=i.Ref_item order by No_commande";
                }
                MySqlCommand command = MainWindow.maConnexion.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                dataGrid1.ItemsSource = dt.DefaultView;
            }
        }

        private void dataGrid1_CopyingRowClipboardContent(object sender, DataGridRowClipboardEventArgs e)
        {

        }
    }
}
