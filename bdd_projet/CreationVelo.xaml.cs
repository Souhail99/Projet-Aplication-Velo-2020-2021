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
using System.Windows.Media.Animation;
using System.Timers;
using System.Windows.Threading;

namespace bdd_projet
{
    /// <summary>
    /// Logique d'interaction pour CreationVelo.xaml
    /// </summary>
    public partial class CreationVelo : Page
    {
        #region bool de focus
        private bool numDel = false;
        private bool nomDel = false;
        private bool grandeurDel = false;
        private bool prixDel = false;
        private bool typeDel = false;
        private bool introDel = false;
        private bool discDel = false;
        private bool unable = false;
        #endregion

        private int value = 0;

        string tampnum = "";

        public CreationVelo(int value) //value = 0 si creation, 1 si modification
        {
            InitializeComponent();
            this.value = value;
            if (value == 1)
            {
                num.Text = "N° du produit à modifier";
                nom.IsEnabled = false;
                grandeur.IsEnabled = false;
                prix.IsEnabled = false;
                type.IsEnabled = false;
                dateIntro.IsEnabled = false;
                dateDisc.IsEnabled = false;
                Submit.IsEnabled = false;
            }
            unable = true;
            num.Focus();
        }

        #region GestionFocus
        private void num_GotFocus(object sender, RoutedEventArgs e)
        {
            if (numDel == false)
            {
                num.Text = tampnum;
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
                nom.BorderBrush = Brushes.Black;
                nomDel = true;
                nom.Foreground = Brushes.Black;
            }
        }

        private void grandeur_GotFocus(object sender, RoutedEventArgs e)
        {
            if (grandeurDel == false)
            {
                grandeur.Text = "";
                grandeur.BorderBrush = Brushes.Black;
                grandeurDel = true;
                grandeur.Foreground = Brushes.Black;
            }
        }

        private void prix_GotFocus(object sender, RoutedEventArgs e)
        {
            if (prixDel == false)
            {
                prix.Text = "";
                prix.BorderBrush = Brushes.Black;
                prixDel = true;
                prix.Foreground = Brushes.Black;
            }
        }

        private void type_GotFocus(object sender, RoutedEventArgs e)
        {
            if (typeDel == false)
            {
                type.Text = "";
                type.BorderBrush = Brushes.Black;
                typeDel = true;
                type.Foreground = Brushes.Black;
            }
        }

        private void dateIntro_GotFocus(object sender, RoutedEventArgs e)
        {
            if (introDel == false)
            {
                dateIntro.Text = "";
                dateIntro.BorderBrush = Brushes.Black;
                introDel = true;
                dateIntro.Foreground = Brushes.Black;
            }
        }

        private void dateDisc_GotFocus(object sender, RoutedEventArgs e)
        {
            if (discDel == false)
            {
                dateDisc.Text = "";
                dateDisc.BorderBrush = Brushes.Black;
                discDel = true;
                dateDisc.Foreground = Brushes.Black;
            }
        }

        private void num_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(num.Text))
            {
                if (value == 0)
                {
                    num.Text = "N° du produit";
                }
                if (value == 1)
                {
                    num.Text = "N° du produit à modifier";
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
                nom.Text = "Nom du produit";
                nom.Foreground = Brushes.DarkGray;
                nom.TextAlignment = TextAlignment.Left;
                nomDel = false;
                nom.BorderBrush = Brushes.DarkGray;
            }
        }

        private void grandeur_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(grandeur.Text))
            {
                grandeur.Text = "Grandeur";
                grandeur.Foreground = Brushes.DarkGray;
                grandeur.TextAlignment = TextAlignment.Left;
                grandeurDel = false;
                grandeur.BorderBrush = Brushes.DarkGray;
            }
        }

        private void prix_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(prix.Text))
            {
                prix.Text = "Prix";
                prix.Foreground = Brushes.DarkGray;
                prix.TextAlignment = TextAlignment.Left;
                prixDel = false;
                prix.BorderBrush = Brushes.DarkGray;
            }
        }

        private void type_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(type.Text))
            {
                type.Text = "Type";
                type.Foreground = Brushes.DarkGray;
                type.TextAlignment = TextAlignment.Left;
                typeDel = false;
                type.BorderBrush = Brushes.DarkGray;
            }
        }

        private void dateIntro_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(dateIntro.Text))
            {
                dateIntro.Text = "Date d'introduction";
                dateIntro.Foreground = Brushes.DarkGray;
                dateIntro.TextAlignment = TextAlignment.Left;
                introDel = false;
                dateIntro.BorderBrush = Brushes.DarkGray;
            }
        }
        private void dateDisc_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(dateDisc.Text))
            {
                dateDisc.BorderBrush = Brushes.DarkGray;
            }
        }
        #endregion

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            bool canSubmit = true;
            int val = 0;
            int cost = 0;
            DateTime dtIn = new DateTime();
            DateTime dtDc = new DateTime();

            bool valDc = false;

            if (string.IsNullOrWhiteSpace(num.Text) == true || int.TryParse(num.Text, out val) == false)
            {
                canSubmit = false;
                num.BorderBrush = Brushes.Red;
                numDel = false;
            }
            if (string.IsNullOrWhiteSpace(nom.Text) == true || int.TryParse(nom.Text, out int i) == true
                || nom.Text == "Nom du produit")
            {
                canSubmit = false;
                nom.BorderBrush = Brushes.Red;
                nomDel = false;
            }
            if (grandeur.Text.ToLower() != "adultes" && grandeur.Text.ToLower() != "jeunes" && grandeur.Text.ToLower() != "hommes"
               && grandeur.Text.ToLower() != "dames" && grandeur.Text.ToLower() != "filles" && grandeur.Text.ToLower() != "garçons")
            {
                canSubmit = false;
                grandeur.BorderBrush = Brushes.Red;
                grandeurDel = false;
            }
            if (int.TryParse(prix.Text, out cost) == false || cost < 0)
            {
                canSubmit = false;
                prix.BorderBrush = Brushes.Red;
                prixDel = false;
            }
            if (type.Text.ToUpper() != "VTT" && type.Text.ToLower() != "vélo de course" && type.Text.ToLower() != "classique" && type.Text.ToUpper() != "BMX")
            {
                canSubmit = false;
                type.BorderBrush = Brushes.Red;
                typeDel = false;
            }
            if (DateTime.TryParse(dateIntro.Text, out dtIn) == false)
            {
                canSubmit = false;
                dateIntro.BorderBrush = Brushes.Red;
                introDel = false;
            }
            if (dateDisc.Text != "Date discontinuité" && string.IsNullOrWhiteSpace(dateDisc.Text) == false && DateTime.TryParse(dateDisc.Text, out dtDc) == false)
            {
                canSubmit = false;
                dateDisc.BorderBrush = Brushes.Red;
                discDel = false;
            }
            if (dateDisc.Text != "Date discontinuité" && string.IsNullOrWhiteSpace(dateDisc.Text) == false && dateDisc.Text != "Argument invalide")
            {
                valDc = true;
            }
            if (canSubmit)
            {
                string insertTable = "";
                if (value == 0)
                {
                    insertTable = "insert into velo values (@num, @nom, @grandeur, @cost, @type, @dtIn, @dtDc)";
                }
                if (value == 1)
                {
                    insertTable = "update velo set nom=@nom, grandeur=@grandeur, prix=@cost, type=@type" +
                       ", dateIntro=@dtIn, dateDiscont=@dtDc WHERE numProduit=@num";
                }
                MySqlCommand command = MainWindow.maConnexion.CreateCommand();
                command.CommandText = insertTable;
                command.Parameters.Add("@num", MySqlDbType.Int32).Value = val;
                command.Parameters.Add("@nom", MySqlDbType.VarChar).Value = GoodMaj(nom.Text);
                command.Parameters.Add("@grandeur", MySqlDbType.VarChar).Value = GoodMaj(grandeur.Text);
                command.Parameters.Add("@cost", MySqlDbType.Int32).Value = cost;
                command.Parameters.Add("@type", MySqlDbType.VarChar).Value = GoodMaj(type.Text);
                command.Parameters.Add("@dtIn", MySqlDbType.DateTime).Value = dtIn;
                if (valDc)
                {
                    command.Parameters.Add("@dtDc", MySqlDbType.DateTime).Value = dtDc;
                }
                else
                {
                    command.Parameters.Add("@dtDc", MySqlDbType.DateTime).Value = null;
                }

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException er)
                {
                    if (er.Code == 0)
                    {
                        tampnum = num.Text;
                        num.Text = "Le n° doit être unique";
                        num.TextAlignment = TextAlignment.Center;
                        num.BorderBrush = Brushes.Red;
                        num.Foreground = Brushes.Red;
                        numDel = false;
                    }
                    return;
                }

                command.Dispose();
                if (numDel == true)
                {
                    MainWindow.Accueil.NavigationService.Navigate(new Velo());
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

        DispatcherTimer timer = new DispatcherTimer();

        private string GoodMaj(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return null;
            }
            if (str.Length > 1 && str.ToLower() != "vtt" && str.ToLower() != "bmx")
            {
                return char.ToUpper(str[0]) + str.Substring(1);
            }
            return str.ToUpper();
        }
        private void Unaccessible(TextBox tb)
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
        private void KeyEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Submit_Click(sender, e);
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(nom), null);
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(grandeur), null);
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(prix), null);
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(type), null);
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(dateIntro), null);
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(dateDisc), null);
                Keyboard.ClearFocus();
            }
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation(0, 1, new TimeSpan(0, 0, 0, 2, 0));
            doubleAnimation.EasingFunction = new ExponentialEase();

            ThicknessAnimation marginAn = new ThicknessAnimation(new Thickness(0, 20, 0, 0), new Thickness(0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 400));
            marginAn.EasingFunction = new ExponentialEase();

            MainWindow.Accueil.BeginAnimation(Control.OpacityProperty, doubleAnimation);
            MainWindow.Accueil.BeginAnimation(Control.MarginProperty, marginAn);
        }
        private void num_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (value == 1)
            {
                int val = 0;
                if (int.TryParse(num.Text, out val) == true)
                {
                    MySqlCommand command = MainWindow.maConnexion.CreateCommand();
                    string request = "SELECT * FROM velo WHERE numProduit=@num";
                    command.CommandText = request;
                    command.Parameters.Add("@num", MySqlDbType.Int32).Value = val;
                    MySqlDataReader reader = command.ExecuteReader();
                    string ans = reader.Read().ToString();

                    if(ans=="False")
                    {
                        Unaccessible(nom);
                        nom.Text = "Nom du produit";
                        Unaccessible(grandeur);
                        grandeur.Text = "Grandeur";
                        Unaccessible(prix);
                        prix.Text = "Prix";
                        Unaccessible(type);
                        type.Text = "Type";
                        Unaccessible(dateIntro);
                        dateIntro.Text = "Date d'introduction";
                        Unaccessible(dateDisc);
                        dateDisc.Text = "Date discontinuité";
                        Submit.IsEnabled = false;
                    }

                    else
                    {
                        reader.Close();
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            nom.Text = reader.GetString(1);
                            nomDel = true;
                            Reaccessible(nom);
                            grandeur.Text = reader.GetString(2);
                            grandeurDel = true;
                            Reaccessible(grandeur);
                            prix.Text = reader.GetString(3);
                            prixDel = true;
                            Reaccessible(prix);
                            type.Text = reader.GetString(4);
                            typeDel = true;
                            Reaccessible(type);
                            dateIntro.Text = Convert.ToDateTime(reader.GetString(5)).ToString("yyyy-MM-dd");
                            introDel = true;
                            Reaccessible(dateIntro);
                            if (reader.IsDBNull(6))
                            {
                                dateDisc.Text = "";
                            }
                            else
                            {
                                dateDisc.Text = Convert.ToDateTime(reader.GetString(6)).ToString("yyyy-MM-dd");
                            }
                            discDel = true;
                            Reaccessible(dateDisc);
                            Submit.IsEnabled = true;
                        }
                    }

                    command.Dispose();
                }
                else if (unable)
                {
                    Unaccessible(nom);
                    nom.Text = "Nom du produit";
                    Unaccessible(grandeur);
                    grandeur.Text = "Grandeur";
                    Unaccessible(prix);
                    prix.Text = "Prix";
                    Unaccessible(type);
                    type.Text = "Type";
                    Unaccessible(dateIntro);
                    dateIntro.Text = "Date d'introduction";
                    Unaccessible(dateDisc);
                    dateDisc.Text = "Date discontinuité";
                    Submit.IsEnabled = false;
                }
            }
        }
    }
}
