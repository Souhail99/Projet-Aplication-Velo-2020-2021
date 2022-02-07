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
using MessageBox = System.Windows.MessageBox;
using System.Data;
using Control = System.Windows.Controls.Control;
using TextBox = System.Windows.Controls.TextBox;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace bdd_projet
{
    /// <summary>
    /// Logique d'interaction pour CreationClient.xaml
    /// </summary>
    public partial class CreationClient : Page
    {
        private List<int> listeNum = new List<int> { };
        private int value = 0;
        MySqlCommand cmd;

        public static int valeur = 0;

        #region bool de focus
        private bool numDel = false;
        private bool numDel2 = false;
        private bool nomDel = false;
        private bool prenomdel = false;
        private bool nom_compagniedel = false;
        private bool adressedel = false;
        private bool maildel = false;
        private bool telephonedel = false;
        private bool adressedel2 = false;
        private bool maildel2 = false;
        private bool telephonedel2 = false;
        private bool nom_contactdel = false;
        private bool adhesiondel = false;
        private bool unable = false;
        #endregion

        public CreationClient(List<int> listeNum, int value)
        {
            InitializeComponent();
            //form.Visibility = Visibility.Visible;
            //form2.Visibility = Visibility.Collapsed;
            this.value = value;
            if (value == 1)
            {
                num.Visibility = Visibility.Visible;
                num2.Visibility = Visibility.Visible;
                if (valeur == 0)
                {
                    form.Visibility = Visibility.Visible;
                    form2.Visibility = Visibility.Collapsed;
                    num.Visibility = Visibility.Visible;
                    num.Text = "N° du client à modifier";
                    nom.IsEnabled = false;
                    prenom.IsEnabled = false;
                    adresse.IsEnabled = false;
                    mail.IsEnabled = false;
                    telephone.IsEnabled = false;
                    adhesion.IsEnabled = false;
                    Submit.IsEnabled = false;
                }
                if (valeur == 1)
                {

                    form.Visibility = Visibility.Collapsed;
                    form2.Visibility = Visibility.Visible;
                    num.Text = "N° du client à modifier";
                    nomdelacompagnie.IsEnabled = false;
                    adresse2.IsEnabled = false;
                    mail2.IsEnabled = false;
                    telephone2.IsEnabled = false;
                    nomducontact.IsEnabled = false;
                    Submit.IsEnabled = false;
                }

            }
            this.listeNum = listeNum;
            unable = true;
            num.Focus();
            num2.Focus();
        }

        //Pour Modif ou création des clients particuliers
        #region GestionFocus
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

        private void nom_GotFocus(object sender, RoutedEventArgs e)
        {
            if (nomDel == false)
            {
                nom.Text = "";
                nom.TextAlignment = TextAlignment.Left;
                nom.BorderBrush = Brushes.Black;
                nomDel = true;
                nom.Foreground = Brushes.Black;
            }
        }

        private void prenom_GotFocus(object sender, RoutedEventArgs e)
        {
            if (prenomdel == false)
            {
                prenom.Text = "";
                prenom.TextAlignment = TextAlignment.Left;
                prenom.BorderBrush = Brushes.Black;
                prenomdel = true;
                prenom.Foreground = Brushes.Black;
            }
        }

        private void adresse_GotFocus(object sender, RoutedEventArgs e)
        {
            if (adressedel == false)
            {
                adresse.Text = "";
                adresse.TextAlignment = TextAlignment.Left;
                adresse.BorderBrush = Brushes.Black;
                adressedel = true;
                adresse.Foreground = Brushes.Black;
            }
        }

        private void mail_GotFocus(object sender, RoutedEventArgs e)
        {
            if (maildel == false)
            {
                mail.Text = "";
                mail.TextAlignment = TextAlignment.Left;
                mail.BorderBrush = Brushes.Black;
                maildel = true;
                mail.Foreground = Brushes.Black;
            }
        }

        private void telephone_GotFocus(object sender, RoutedEventArgs e)
        {
            if (telephonedel == false)
            {
                telephone.Text = "";
                telephone.TextAlignment = TextAlignment.Left;
                telephone.BorderBrush = Brushes.Black;
                telephonedel = true;
                telephone.Foreground = Brushes.Black;
            }
        }

        private void adhesion_GotFocus(object sender, RoutedEventArgs e)
        {
            if (adhesiondel == false)
            {
                adhesion.Text = "";
                adhesion.TextAlignment = TextAlignment.Left;
                adhesion.BorderBrush = Brushes.Black;
                adhesiondel = true;
                adhesion.Foreground = Brushes.Black;
            }
        }

        private void num_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(num.Text))
            {
                if (value == 0)
                {
                    num.Text = "Numéro du client";
                }
                if (value == 1)
                {
                    num.Text = "Numéro du client (pour modification)";
                }
                num.Foreground = Brushes.DarkGray;
                num.TextAlignment = TextAlignment.Left;
                numDel = false;
                num.BorderBrush = Brushes.DarkGray;
            }
        }

        private void nom_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nom.Text))
            {
                nom.Text = "Nom du client";
                nom.Foreground = Brushes.DarkGray;
                nom.TextAlignment = TextAlignment.Left;
                nomDel = false;
                nom.BorderBrush = Brushes.DarkGray;
            }
        }

        private void prenom_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(prenom.Text))
            {

                prenom.Text = "Prenom du client";
                prenom.Foreground = Brushes.DarkGray;
                prenom.TextAlignment = TextAlignment.Left;
                prenomdel = false;
                prenom.BorderBrush = Brushes.DarkGray;
            }
        }

        private void adresse_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(adresse.Text))
            {
                adresse.Text = "Adresse";
                adresse.Foreground = Brushes.DarkGray;
                adresse.TextAlignment = TextAlignment.Left;
                adressedel = false;
                adresse.BorderBrush = Brushes.DarkGray;
            }
        }

        private void mail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(mail.Text))
            {
                mail.Text = "Mail";
                mail.Foreground = Brushes.DarkGray;
                mail.TextAlignment = TextAlignment.Left;
                maildel = false;
                mail.BorderBrush = Brushes.DarkGray;
            }
        }

        private void telephone_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(telephone.Text))
            {
                telephone.Text = "telephone";
                telephone.Foreground = Brushes.DarkGray;
                telephone.TextAlignment = TextAlignment.Left;
                telephonedel = false;
                telephone.BorderBrush = Brushes.DarkGray;
            }
        }
        private void adhesion_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(adhesion.Text))
            {
                adhesion.Text = "Ce Client a-t-il souscrit à Fidelio";
                adhesion.Foreground = Brushes.DarkGray;
                adhesion.TextAlignment = TextAlignment.Left;
                adhesiondel = false;
                adhesion.BorderBrush = Brushes.DarkGray;
            }
        }
        #endregion


        //Pour Modif ou création des clients entreprise
        #region GestionFocus
        private void num2_GotFocus(object sender, RoutedEventArgs e)
        {
            if (numDel2 == false)
            {
                num2.Text = "";
                num2.FontSize = 12;
                num2.TextAlignment = TextAlignment.Left;
                num2.BorderBrush = Brushes.Black;
                numDel2 = true;
                num2.Foreground = Brushes.Black;
            }
        }


        private void Nom_de_la_compagnie_GotFocus(object sender, RoutedEventArgs e)
        {
            if (nom_compagniedel == false)
            {
                nomdelacompagnie.Text = "";
                nomdelacompagnie.TextAlignment = TextAlignment.Left;
                nomdelacompagnie.BorderBrush = Brushes.Black;
                nom_compagniedel = true;
                nomdelacompagnie.Foreground = Brushes.Black;
            }
        }


        private void adresse2_GotFocus(object sender, RoutedEventArgs e)
        {
            if (adressedel2 == false)
            {
                adresse2.Text = "";
                adresse2.TextAlignment = TextAlignment.Left;
                adresse2.BorderBrush = Brushes.Black;
                adressedel2 = true;
                adresse2.Foreground = Brushes.Black;
            }
        }

        private void mail2_GotFocus(object sender, RoutedEventArgs e)
        {
            if (maildel2 == false)
            {
                mail2.Text = "";
                mail2.TextAlignment = TextAlignment.Left;
                mail2.BorderBrush = Brushes.Black;
                maildel2 = true;
                mail2.Foreground = Brushes.Black;
            }
        }

        private void telephone2_GotFocus(object sender, RoutedEventArgs e)
        {
            if (telephonedel2 == false)
            {
                telephone2.Text = "";
                telephone2.TextAlignment = TextAlignment.Left;
                telephone2.BorderBrush = Brushes.Black;
                telephonedel2 = true;
                telephone2.Foreground = Brushes.Black;
            }
        }

        private void nomducontact_GotFocus(object sender, RoutedEventArgs e)
        {
            if (nom_contactdel == false)
            {
                nomducontact.Text = "";
                nomducontact.TextAlignment = TextAlignment.Left;
                nomducontact.BorderBrush = Brushes.Black;
                nom_contactdel = true;
                nomducontact.Foreground = Brushes.Black;
            }
        }


        private void num2_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(num2.Text))
            {
                if (value == 0)
                {
                    num.Text = "Numéro du client";
                }
                if (value == 1)
                {
                    num.Text = "Numéro du client (pour modification)";
                }
                num2.Foreground = Brushes.DarkGray;
                num2.TextAlignment = TextAlignment.Left;
                numDel2 = false;
                num2.BorderBrush = Brushes.DarkGray;
            }
        }

        private void Nom_de_la_compagnie_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nomdelacompagnie.Text))
            {

                nomdelacompagnie.Text = "Prenom du client";
                nomdelacompagnie.Foreground = Brushes.DarkGray;
                nomdelacompagnie.TextAlignment = TextAlignment.Left;
                nom_compagniedel = false;
                nomdelacompagnie.BorderBrush = Brushes.DarkGray;
            }
        }


        private void adresse2_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(adresse2.Text))
            {
                adresse2.Text = "Adresse";
                adresse2.Foreground = Brushes.DarkGray;
                adresse2.TextAlignment = TextAlignment.Left;
                adressedel2 = false;
                adresse2.BorderBrush = Brushes.DarkGray;
            }
        }

        private void mail2_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(mail2.Text))
            {
                mail2.Text = "Mail";
                mail2.Foreground = Brushes.DarkGray;
                mail2.TextAlignment = TextAlignment.Left;
                maildel2 = false;
                mail2.BorderBrush = Brushes.DarkGray;
            }
        }

        private void telephone2_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(telephone2.Text))
            {
                telephone2.Text = "telephone";
                telephone2.Foreground = Brushes.DarkGray;
                telephone2.TextAlignment = TextAlignment.Left;
                telephonedel2 = false;
                telephone2.BorderBrush = Brushes.DarkGray;
            }
        }


        private void nomducontact_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nomducontact.Text))
            {

                nomducontact.Text = "Nom du contact";
                nomducontact.Foreground = Brushes.DarkGray;
                nomducontact.TextAlignment = TextAlignment.Left;
                nom_contactdel = false;
                nomducontact.BorderBrush = Brushes.DarkGray;
            }
        }

        #endregion


        #region Méthodes et attributs utiles
        public static int Premierchiffre2(int d)
        {
            double nbr = Convert.ToDouble(d);
            // Cela retournera le nombre total de chiffres - 1
            double l = Math.Log10(nbr);
            int pre = Convert.ToInt32(nbr / Math.Pow(10, l));
            return pre;
        }
        public static char Premierchiffre(int d)
        {
            string a = Convert.ToString(d);
            char result = a[0];
            return result;
        }

        DispatcherTimer timer = new DispatcherTimer();

        private string GoodMaj(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return null;
            }
            else
            {
                return str.ToUpper();
            }
        }
        private void Unaccessible(System.Windows.Controls.TextBox tb)
        {
            tb.IsEnabled = false;
            tb.Foreground = Brushes.DarkGray;
            tb.TextAlignment = TextAlignment.Left;
            tb.BorderBrush = Brushes.DarkGray;
        }
        private void Reaccessible(TextBox tb)
        {
            tb.IsEnabled = true;
            tb.FontSize = 12;
            tb.TextAlignment = TextAlignment.Left;
            tb.BorderBrush = Brushes.Black;
            tb.Foreground = Brushes.Black;
        }

        #endregion

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            bool canSubmit = true;
            int val = 0;
            int d = 0;
            //MySqlConnection maConnexion2 = new MySqlConnection(connexionString2);


            if (valeur == 0)
            {
                if (value == 1)
                {
                    num.Visibility = Visibility.Visible;
                    if (string.IsNullOrWhiteSpace(num.Text) == true || int.TryParse(num.Text, out val) == false)
                    {
                        canSubmit = false;
                        num.Text = "Argument invalide";
                        num.TextAlignment = TextAlignment.Center;
                        num.BorderBrush = Brushes.Red;
                        num.Foreground = Brushes.Red;
                        numDel = false;
                    }
                }
                if (string.IsNullOrWhiteSpace(nom.Text) == true || int.TryParse(nom.Text, out int i) == true
                    || nom.Text == "Nom du client" || nom.Text == "Argument invalide")
                {
                    canSubmit = false;
                    nom.Text = "Argument invalide";
                    nom.TextAlignment = TextAlignment.Center;
                    nom.BorderBrush = Brushes.Red;
                    nom.Foreground = Brushes.Red;
                    nomDel = false;
                }
                if (string.IsNullOrWhiteSpace(prenom.Text) == true || int.TryParse(prenom.Text, out int j) == true
                    || prenom.Text == "Prenom du client" || prenom.Text == "Argument invalide")
                {
                    canSubmit = false;
                    prenom.Text = "Argument invalide";
                    prenom.TextAlignment = TextAlignment.Center;
                    prenom.BorderBrush = Brushes.Red;
                    prenom.Foreground = Brushes.Red;
                    prenomdel = false;
                }
                if (string.IsNullOrWhiteSpace(adresse.Text) == true || int.TryParse(adresse.Text, out int w) == true
                    || adresse.Text == "Adresse" || adresse.Text == "Argument invalide")
                {
                    canSubmit = false;
                    adresse.Text = "Argument invalide";
                    adresse.TextAlignment = TextAlignment.Center;
                    adresse.BorderBrush = Brushes.Red;
                    adresse.Foreground = Brushes.Red;
                    adressedel = false;
                }
                if (string.IsNullOrWhiteSpace(mail.Text) == true || int.TryParse(mail.Text, out int u) == true
                    || mail.Text == "Mail" || mail.Text == "Argument invalide")
                {
                    canSubmit = false;
                    mail.Text = "Argument invalide";
                    mail.TextAlignment = TextAlignment.Center;
                    mail.BorderBrush = Brushes.Red;
                    mail.Foreground = Brushes.Red;
                    maildel = false;
                }
                if (int.TryParse(telephone.Text, out d) == false)
                {
                    canSubmit = false;
                    telephone.Text = "Argument invalide";
                    telephone.TextAlignment = TextAlignment.Center;
                    telephone.BorderBrush = Brushes.Red;
                    telephone.Foreground = Brushes.Red;
                    telephonedel = false;
                }
                if (string.IsNullOrWhiteSpace(adhesion.Text) == true || int.TryParse(adhesion.Text, out int v) == true
                    || adhesion.Text == "Ce Client a-t-il souscrit à Fidelio" || adhesion.Text == "Argument invalide")
                {
                    canSubmit = false;
                    adhesion.Text = "Argument invalide";
                    adhesion.TextAlignment = TextAlignment.Center;
                    adhesion.BorderBrush = Brushes.Red;
                    adhesion.Foreground = Brushes.Red;
                    adhesiondel = false;
                }
                if (canSubmit)
                {
                    string insertTable = "";
                    bool b = true;
                    bool c = false;
                    bool lon = false;
                    int no = 0;
                    numDel = true;
                    while (b)
                    {
                        Random alea = new Random();
                        no = alea.Next(100000);
                        char rand = Premierchiffre(no);
                        int n = Convert.ToInt32(Math.Log10(no) + 1);
                        if (n <= 5 && rand == '1')
                        {
                            lon = true;
                        }
                        for (int ao = 0; ao <= Client.IDs.Count - 1; ao++)
                        {
                            if (no == Client.IDs[ao])
                            {
                                c = true;
                            }
                        }
                        if (lon == true && c == false)
                        {
                            b = false;
                        }

                    }
                    if (value == 0)
                    {
                        insertTable = "insert into Clients(No_client, nom, prenom, adresse, mail,telephone,adhesion) values (@no,@nom, @prenom, @adresse, @mail, @telephone, @adhesion)";
                        bool alpha = false;
                        if (adhesion.Text.ToUpper() == "O")
                        {
                            alpha = true;
                        }

                        try
                        {
                            cmd = new MySqlCommand(insertTable, MainWindow.maConnexion);
                            cmd.Parameters.AddWithValue("@no", no);
                            cmd.Parameters.AddWithValue("@nom", nom.Text);
                            cmd.Parameters.AddWithValue("@prenom", GoodMaj(prenom.Text));
                            cmd.Parameters.AddWithValue("@adresse", GoodMaj(adresse.Text));
                            cmd.Parameters.AddWithValue("@mail", mail.Text);
                            cmd.Parameters.AddWithValue("@telephone", telephone.Text);
                            cmd.Parameters.AddWithValue("@adhesion", Convert.ToBoolean(alpha));
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                        catch (MySqlException er)
                        {
                            MessageBox.Show(er.ToString());
                            return;
                        }
                        cmd.Dispose();
                    }
                    if (value == 1)
                    {
                        no = Convert.ToInt32(num.Text);
                        insertTable = "update Clients set nom=@nom, prenom=@prenom, adresse=@adresse, mail=@mail" +
                           ", telephone=@telephone, adhesion=@adhesion WHERE No_client=@no";

                        bool alpha = false;
                        if (adhesion.Text.ToUpper() == "O")
                        {
                            alpha = true;
                        }

                        try
                        {
                            cmd = new MySqlCommand(insertTable, MainWindow.maConnexion);
                            cmd.Parameters.AddWithValue("@no", no);
                            cmd.Parameters.AddWithValue("@nom", nom.Text);
                            cmd.Parameters.AddWithValue("@prenom", GoodMaj(prenom.Text));
                            cmd.Parameters.AddWithValue("@adresse", GoodMaj(adresse.Text));
                            cmd.Parameters.AddWithValue("@mail", mail.Text);
                            cmd.Parameters.AddWithValue("@telephone", telephone.Text);
                            cmd.Parameters.AddWithValue("@adhesion", Convert.ToBoolean(alpha));
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                        catch (MySqlException er)
                        {
                            MessageBox.Show(er.ToString());
                            return;
                        }
                        cmd.Dispose();
                    }

                    if (numDel == true)
                    {
                        MainWindow.Accueil.NavigationService.Navigate(new Client());
                    }
                }
                if (canSubmit == false)
                {
                    ThicknessAnimation marginAn = new ThicknessAnimation(new Thickness(-10, 0, 0, 0), new Thickness(10, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 400));
                    marginAn.EasingFunction = new BounceEase();

                    MainWindow.Accueil.BeginAnimation(Control.MarginProperty, marginAn);
                    timer.Tick += new EventHandler(delegate (Object o, EventArgs a)
                    {
                        timer.Stop();
                        ThicknessAnimation margin = new ThicknessAnimation(new Thickness(10, 0, 0, 0), new Thickness(0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 1));
                        margin.EasingFunction = new QuadraticEase();

                        MainWindow.Accueil.BeginAnimation(Control.MarginProperty, margin);
                    });
                    timer.Interval = TimeSpan.FromSeconds(0.29);
                    timer.Start();
                }
            }
            if (valeur == 1)
            {
                if (value == 1)
                {
                    num2.Visibility = Visibility.Visible;
                    if (string.IsNullOrWhiteSpace(num2.Text) == true || int.TryParse(num2.Text, out val) == false)
                    {
                        canSubmit = false;
                        num2.Text = "Argument invalide";
                        num2.TextAlignment = TextAlignment.Center;
                        num2.BorderBrush = Brushes.Red;
                        num2.Foreground = Brushes.Red;
                        numDel2 = false;
                    }
                }
                if (string.IsNullOrWhiteSpace(nomdelacompagnie.Text) == true || int.TryParse(nomdelacompagnie.Text, out int i) == true
                    || nomdelacompagnie.Text == "Nom du client" || nomdelacompagnie.Text == "Argument invalide")
                {
                    canSubmit = false;
                    nomdelacompagnie.Text = "Argument invalide";
                    nomdelacompagnie.TextAlignment = TextAlignment.Center;
                    nomdelacompagnie.BorderBrush = Brushes.Red;
                    nomdelacompagnie.Foreground = Brushes.Red;
                    nom_compagniedel = false;
                }

                if (string.IsNullOrWhiteSpace(adresse2.Text) == true || int.TryParse(adresse2.Text, out int w) == true
                    || adresse2.Text == "Adresse" || adresse2.Text == "Argument invalide")
                {
                    canSubmit = false;
                    adresse2.Text = "Argument invalide";
                    adresse2.TextAlignment = TextAlignment.Center;
                    adresse2.BorderBrush = Brushes.Red;
                    adresse2.Foreground = Brushes.Red;
                    adressedel2 = false;
                }
                if (string.IsNullOrWhiteSpace(mail2.Text) == true || int.TryParse(mail2.Text, out int u) == true
                    || mail2.Text == "Mail" || mail2.Text == "Argument invalide")
                {
                    canSubmit = false;
                    mail2.Text = "Argument invalide";
                    mail2.TextAlignment = TextAlignment.Center;
                    mail2.BorderBrush = Brushes.Red;
                    mail2.Foreground = Brushes.Red;
                    maildel2 = false;
                }
                if (int.TryParse(telephone2.Text, out d) == false)
                {
                    canSubmit = false;
                    telephone2.Text = "Argument invalide";
                    telephone2.TextAlignment = TextAlignment.Center;
                    telephone2.BorderBrush = Brushes.Red;
                    telephone2.Foreground = Brushes.Red;
                    telephonedel2 = false;
                }
                if (string.IsNullOrWhiteSpace(nomducontact.Text) == true || int.TryParse(nomducontact.Text, out int v) == true
                    || nomducontact.Text == "Nom du contact" || nomducontact.Text == "Argument invalide")
                {
                    canSubmit = false;
                    nomducontact.Text = "Argument invalide";
                    nomducontact.TextAlignment = TextAlignment.Center;
                    nomducontact.BorderBrush = Brushes.Red;
                    nomducontact.Foreground = Brushes.Red;
                    nom_contactdel = false;
                }
                if (canSubmit)
                {
                    string insertTable = "";
                    bool b = true;
                    bool c = false;
                    bool lon = false;
                    int no = 0;
                    numDel2 = true;
                    while (b)
                    {
                        Random alea = new Random();
                        no = alea.Next(100000);
                        char rand = Premierchiffre(no);
                        int n = Convert.ToInt32(Math.Log10(no) + 1);
                        if (n == 5 && rand == '2')
                        {
                            lon = true;
                        }
                        for (int ao = 0; ao <= Client.IDs.Count - 1; ao++)
                        {
                            if (no == Client.IDs[ao])
                            {
                                c = true;
                            }
                        }
                        if (lon == true && c == false)
                        {
                            b = false;
                        }

                    }
                    if (value == 0)
                    {
                        insertTable = "insert into Clients(No_client,nom_compagnie,adresse, mail,telephone, nom_contact) values (@no,@nomdelacompagnie2, @adresse2, @mail2, @telephone2, @nomducontact)";

                        try
                        {
                            cmd = new MySqlCommand(insertTable, MainWindow.maConnexion);
                            cmd.Parameters.AddWithValue("@no", no);
                            cmd.Parameters.AddWithValue("@nomdelacompagnie2", nomdelacompagnie.Text);
                            cmd.Parameters.AddWithValue("@adresse2", GoodMaj(adresse2.Text));
                            cmd.Parameters.AddWithValue("@mail2", mail2.Text);
                            cmd.Parameters.AddWithValue("@telephone2", Convert.ToInt32(telephone2.Text));
                            cmd.Parameters.AddWithValue("@nomducontact", nomducontact.Text);
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                        catch (MySqlException er)
                        {
                            MessageBox.Show(er.ToString());
                            return;
                        }
                        cmd.Dispose();
                    }
                    if (value == 1)
                    {
                        no = Convert.ToInt32(num2.Text);
                        insertTable = "update Clients set nom_compagnie=@nomdelacompagnie, adresse=@adresse2, mail=@mail2" +
                           ", telephone=@telephone2, nom_contact=@nomducontact WHERE No_client=@no";




                        //MessageBox.Show(Convert.ToString(Convert.ToInt32(num.Text)));
                        //maConnexion2.Open();
                        try
                        {
                            cmd = new MySqlCommand(insertTable, MainWindow.maConnexion);
                            cmd.Parameters.AddWithValue("@no", no);
                            cmd.Parameters.AddWithValue("@nomdelacompagnie", nomdelacompagnie.Text);
                            cmd.Parameters.AddWithValue("@adresse2", GoodMaj(adresse2.Text));
                            cmd.Parameters.AddWithValue("@mail2", mail2.Text);
                            cmd.Parameters.AddWithValue("@telephone2", telephone2.Text);
                            cmd.Parameters.AddWithValue("@nomducontact", nomducontact.Text);
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                        catch (MySqlException er)
                        {
                            MessageBox.Show(er.ToString());
                            return;
                        }
                        cmd.Dispose();
                    }

                    if (numDel2 == true)
                    {
                        MainWindow.Accueil.NavigationService.Navigate(new Client());
                    }
                }
                if (canSubmit == false)
                {
                    ThicknessAnimation marginAn = new ThicknessAnimation(new Thickness(-10, 0, 0, 0), new Thickness(10, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 400));
                    marginAn.EasingFunction = new BounceEase();

                    MainWindow.Accueil.BeginAnimation(Control.MarginProperty, marginAn);
                    timer.Tick += new EventHandler(delegate (Object o, EventArgs a)
                    {
                        timer.Stop();
                        ThicknessAnimation margin = new ThicknessAnimation(new Thickness(10, 0, 0, 0), new Thickness(0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 1));
                        margin.EasingFunction = new QuadraticEase();

                        MainWindow.Accueil.BeginAnimation(Control.MarginProperty, margin);
                    });
                    timer.Interval = TimeSpan.FromSeconds(0.29);
                    timer.Start();
                }
            }

        }

        private void KeyEnter(object sender, KeyEventArgs e)
        {
            if (valeur == 0)
            {
                if (e.Key == Key.Return)
                {
                    Submit_Click(sender, e);
                    FocusManager.SetFocusedElement(FocusManager.GetFocusScope(num), null);
                    FocusManager.SetFocusedElement(FocusManager.GetFocusScope(nom), null);
                    FocusManager.SetFocusedElement(FocusManager.GetFocusScope(prenom), null);
                    FocusManager.SetFocusedElement(FocusManager.GetFocusScope(adresse), null);
                    FocusManager.SetFocusedElement(FocusManager.GetFocusScope(mail), null);
                    FocusManager.SetFocusedElement(FocusManager.GetFocusScope(telephone), null);
                    FocusManager.SetFocusedElement(FocusManager.GetFocusScope(adhesion), null);
                    Keyboard.ClearFocus();
                }
            }
            if (valeur == 1)
            {
                if (e.Key == Key.Return)
                {
                    Submit_Click(sender, e);
                    FocusManager.SetFocusedElement(FocusManager.GetFocusScope(num2), null);
                    FocusManager.SetFocusedElement(FocusManager.GetFocusScope(nomdelacompagnie), null);
                    FocusManager.SetFocusedElement(FocusManager.GetFocusScope(adresse2), null);
                    FocusManager.SetFocusedElement(FocusManager.GetFocusScope(mail2), null);
                    FocusManager.SetFocusedElement(FocusManager.GetFocusScope(telephone2), null);
                    FocusManager.SetFocusedElement(FocusManager.GetFocusScope(nomducontact), null);
                    Keyboard.ClearFocus();
                }
            }

        }
        private void KeyEnter2(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Return)
            {
                Submit_Click(sender, e);
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(num2), null);
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(nomdelacompagnie), null);
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(adresse2), null);
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(mail2), null);
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(telephone2), null);
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(nomducontact), null);
                Keyboard.ClearFocus();
            }


        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            /*ThicknessAnimation marginAn = new ThicknessAnimation(new Thickness(10, 0, 0, 0), new Thickness(0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 1));
            CubicEase ce = new CubicEase(); ce.EasingMode = EasingMode.EaseInOut;
            marginAn.EasingFunction = ce;

            MainWindow.Accueil.BeginAnimation(Control.MarginProperty, marginAn);

            ThicknessAnimation db = new ThicknessAnimation(new Thickness(0, 0, 750, 0), new Thickness(0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 800));
            db.EasingFunction = new ExponentialEase();

            DoubleAnimation doubleAnimation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(4));
            doubleAnimation.EasingFunction = new ExponentialEase();

            form.BeginAnimation(Control.MarginProperty, db);
            MainWindow.Accueil.BeginAnimation(Control.OpacityProperty, doubleAnimation);*/

            DoubleAnimation doubleAnimation = new DoubleAnimation(0, 1, new TimeSpan(0, 0, 0, 2, 0));
            doubleAnimation.EasingFunction = new ExponentialEase();

            ThicknessAnimation marginAn = new ThicknessAnimation(new Thickness(0, 20, 0, 0), new Thickness(0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 400));
            marginAn.EasingFunction = new ExponentialEase();

            MainWindow.Accueil.BeginAnimation(Control.OpacityProperty, doubleAnimation);
            MainWindow.Accueil.BeginAnimation(Control.MarginProperty, marginAn);
        }



        private void num_TextChanged(object sender, TextChangedEventArgs e)
        {
            char b = '0';
            if (string.IsNullOrEmpty(num.Text) == false)
            {
                string testalpha = num.Text;
                b = testalpha[0];
            }
            if (b == '1')
            {
                int val = 0;
                if (int.TryParse(num.Text, out val) == true && Client.IDs.Contains(val))
                {
                    MySqlCommand a = MainWindow.maConnexion.CreateCommand();
                    string request = "SELECT nom,prenom,adresse,mail,telephone,adhesion FROM Clients WHERE No_client=@num";
                    a.CommandText = request;
                    a.Parameters.Add("@num", MySqlDbType.Int32).Value = val;
                    MySqlDataReader reader = a.ExecuteReader();
                    while (reader.Read())
                    {
                        try
                        {
                            nom.Text = reader.GetString(0);
                            nomDel = true;
                            Reaccessible(nom);
                            prenom.Text = reader.GetString(1);
                            prenomdel = true;
                            Reaccessible(prenom);
                            adresse.Text = reader.GetString(2);
                            adressedel = true;
                            Reaccessible(adresse);
                            mail.Text = reader.GetString(3);
                            maildel = true;
                            Reaccessible(mail);
                            telephone.Text = reader.GetString(4);
                            telephonedel = true;
                            Reaccessible(telephone);
                            adhesion.Text = reader.GetString(5);
                            adhesiondel = true;
                            Reaccessible(adhesion);
                        }
                        catch (MySqlException er)
                        {
                            MessageBox.Show(er.ToString());
                            return;
                        }
                    }
                    reader.Close();
                    a.Dispose();
                    Submit.IsEnabled = true;
                }
                else if (unable)
                {
                    Unaccessible(nom);
                    nom.Text = "Nom du client";
                    Unaccessible(prenom);
                    prenom.Text = "Prenom du client";
                    Unaccessible(adresse);
                    adresse.Text = "Adresse";
                    Unaccessible(mail);
                    mail.Text = "Mail";
                    Unaccessible(telephone);
                    telephone.Text = "telephone";
                    Unaccessible(adhesion);
                    adhesion.Text = "Ce Client  a-t-il souscrit à Fidelio";
                    Submit.IsEnabled = false;
                }

            }





        }




        private void num2_TextChanged(object sender, TextChangedEventArgs e)
        {
            char b = '0';
            if (string.IsNullOrEmpty(num2.Text) == false)
            {
                string testalpha = num2.Text;
                b = testalpha[0];
            }

            if (b == '2')
            {
                int val = 0;
                if (int.TryParse(num2.Text, out val) == true && Client.IDs.Contains(val))
                {
                    MySqlCommand a = MainWindow.maConnexion.CreateCommand();
                    string request = "SELECT nom_compagnie,adresse,mail,telephone,nom_contact FROM Clients WHERE No_client=@num2";
                    a.CommandText = request;
                    a.Parameters.Add("@num2", MySqlDbType.Int32).Value = val;
                    MySqlDataReader reader = a.ExecuteReader();
                    while (reader.Read())
                    {
                        try
                        {
                            nomdelacompagnie.Text = reader.GetString(0);
                            nom_compagniedel = true;
                            Reaccessible(nomdelacompagnie);
                            adresse2.Text = reader.GetString(1);
                            adressedel2 = true;
                            Reaccessible(adresse2);
                            mail2.Text = reader.GetString(2);
                            maildel2 = true;
                            Reaccessible(mail2);
                            telephone2.Text = reader.GetString(3);
                            telephonedel2 = true;
                            Reaccessible(telephone2);
                            nomducontact.Text = reader.GetString(4);
                            nom_contactdel = true;
                            Reaccessible(nomducontact);
                        }
                        catch (MySqlException er)
                        {
                            MessageBox.Show(er.ToString());
                            return;
                        }
                    }
                    reader.Close();
                    a.Dispose();
                    Submit.IsEnabled = true;
                }
                else if (unable)
                {
                    Unaccessible(nomdelacompagnie);
                    nomdelacompagnie.Text = "Nom de l'entreprise";
                    Unaccessible(adresse);
                    adresse.Text = "Adresse";
                    Unaccessible(mail);
                    mail.Text = "Mail";
                    Unaccessible(telephone);
                    telephone.Text = "telephone";
                    Unaccessible(nomducontact);
                    nomducontact.Text = "Nom du contact";
                    Submit.IsEnabled = false;
                }


            }

            /*else
           {
               num2.Text = "Argument invalide";
               num2.TextAlignment = TextAlignment.Center;
               num2.BorderBrush = Brushes.Red;
               num2.Foreground = Brushes.Red;
           }*/


        }


        private void Creation_Client_Click(object sender, RoutedEventArgs e)
        {
            valeur = 0;
            form2.Visibility = Visibility.Collapsed;
            form.Visibility = Visibility.Visible;
            if (value == 1)
            {
                num.Text = "N° du client à modifier";
                nom.IsEnabled = false;
                prenom.IsEnabled = false;
                adresse.IsEnabled = false;
                mail.IsEnabled = false;
                telephone.IsEnabled = false;
                adhesion.IsEnabled = false;
                Submit.IsEnabled = false;
            }
        }

        private void Creation_Entreprise_Click(object sender, RoutedEventArgs e)
        {
            valeur = 1;
            form.Visibility = Visibility.Collapsed;
            form2.Visibility = Visibility.Visible;
            if (value == 1)
            {
                //num2.Visibility = Visibility.Visible;
                num2.Text = "N° du client à modifier";
                //num2.IsEnabled = true;
                nomdelacompagnie.IsEnabled = false;
                adresse2.IsEnabled = false;
                mail2.IsEnabled = false;
                telephone2.IsEnabled = false;
                nomducontact.IsEnabled = false;
                Submit.IsEnabled = false;
            }
        }


    }
}
