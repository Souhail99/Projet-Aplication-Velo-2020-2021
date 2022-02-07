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
using System.Windows.Threading;

namespace bdd_projet
{
    /// <summary>
    /// Logique d'interaction pour CreationPiece.xaml
    /// </summary>
    public partial class CreationPiece : Page
    {
        #region bools de focus
        bool numDel = false;
        bool nomDel = false;
        bool introDel = false;
        bool discDel = false;
        bool descDel = false;
        bool numfournDel = false;
        bool approDel = false;
        bool prixDel = false;
        bool unable = false;
        bool quantiteDel = false;
        #endregion

        string tampfourn = "";
        string tampnum = "";
        string tampnom = "";

        int value = 0;

        public CreationPiece(int value) //value = 0 si creation, 1 si modification
        {
            InitializeComponent();
            this.value = value;
            if (value == 1)
            {
                num.Text = "N° du produit à modifier";

                numfourn.IsEnabled = false;
                dateIntro.IsEnabled = false;
                dateDisc.IsEnabled = false;
                desc.IsEnabled = false;
                appro.IsEnabled = false;
                prix.IsEnabled = false;
                quantite.IsEnabled = false;
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
                num.Foreground = Brushes.Black;
                num.TextAlignment = TextAlignment.Left;
                num.BorderBrush = Brushes.Black;
                numDel = true;
            }
        }
        private void nom_GotFocus(object sender, RoutedEventArgs e)
        {
            if (nomDel == false)
            {
                nom.Text = tampnom;
                nom.Foreground = Brushes.Black;
                nom.TextAlignment = TextAlignment.Left;
                nom.BorderBrush = Brushes.Black;
                nomDel = true;
            }
        }
        private void dateIntro_GotFocus(object sender, RoutedEventArgs e)
        {
            if (introDel == false)
            {
                dateIntro.Text = "";
                dateIntro.Foreground = Brushes.Black;
                dateIntro.BorderBrush = Brushes.Black;
                introDel = true;
            }
        }
        private void dateDisc_GotFocus(object sender, RoutedEventArgs e)
        {
            if (discDel == false)
            {
                dateDisc.Text = "";
                dateDisc.Foreground = Brushes.Black;
                dateDisc.BorderBrush = Brushes.Black;
                discDel = true;
            }
        }
        private void desc_GotFocus(object sender, RoutedEventArgs e)
        {
            if (descDel == false)
            {
                desc.Text = "";
                desc.Foreground = Brushes.Black;
                desc.BorderBrush = Brushes.Black;
                descDel = true;
            }
        }
        private void numfourn_GotFocus(object sender, RoutedEventArgs e)
        {
            if (numfournDel == false)
            {
                numfourn.Text = tampfourn;
                numfourn.Foreground = Brushes.Black;
                numfourn.TextAlignment = TextAlignment.Left;
                numfourn.BorderBrush = Brushes.Black;
                numfournDel = true;
            }
        }
        private void appro_GotFocus(object sender, RoutedEventArgs e)
        {
            if (approDel == false)
            {
                appro.Text = "";
                appro.Foreground = Brushes.Black;
                appro.BorderBrush = Brushes.Black;
                approDel = true;
            }
        }
        private void prix_GotFocus(object sender, RoutedEventArgs e)
        {
            if (prixDel == false)
            {
                prix.Text = "";
                prix.Foreground = Brushes.Black;
                prix.BorderBrush = Brushes.Black;
                prixDel = true;
            }
        }
        private void quantite_GotFocus(object sender, RoutedEventArgs e)
        {
            if (quantiteDel == false)
            {
                quantite.Text = "";
                quantite.Foreground = Brushes.Black;
                quantite.BorderBrush = Brushes.Black;
                quantiteDel = true;
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
        private void desc_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(desc.Text))
            {
                desc.Text = "Description";
                desc.Foreground = Brushes.DarkGray;
                desc.TextAlignment = TextAlignment.Left;
                descDel = false;
                desc.BorderBrush = Brushes.DarkGray;
            }
        }
        private void nom_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nom.Text))
            {
                nom.Text = "Siret du fournisseur";
                nom.Foreground = Brushes.DarkGray;
                nom.TextAlignment = TextAlignment.Left;
                nomDel = false;
                nom.BorderBrush = Brushes.DarkGray;
            }
        }
        private void numfourn_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(numfourn.Text))
            {
                numfourn.Text = "N° produit fournisseur";
                numfourn.Foreground = Brushes.DarkGray;
                numfourn.TextAlignment = TextAlignment.Left;
                numfournDel = false;
                numfourn.BorderBrush = Brushes.DarkGray;
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
        private void appro_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(appro.Text))
            {
                appro.Text = "Délai d'approvisionnement";
                appro.Foreground = Brushes.DarkGray;
                appro.TextAlignment = TextAlignment.Left;
                approDel = false;
                appro.BorderBrush = Brushes.DarkGray;
            }
        }
        private void dateDisc_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(dateDisc.Text))
            {
                dateDisc.BorderBrush = Brushes.DarkGray;
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
        private void quantite_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(quantite.Text))
            {
                quantite.Text = "Quantité";
                quantite.Foreground = Brushes.DarkGray;
                quantite.TextAlignment = TextAlignment.Left;
                quantiteDel = false;
                quantite.BorderBrush = Brushes.DarkGray;
            }
        }
        #endregion
        private void Submit_Click(object send, RoutedEventArgs e)
        {
            bool canSubmit = true;
            int delay = 0;
            int cost = 0;
            int number = 0;
            DateTime dtIn = new DateTime();
            DateTime dtDc = new DateTime();

            bool valDc = false;

            if (string.IsNullOrWhiteSpace(num.Text) == true || num.Text == "N° du produit")
            {
                canSubmit = false;
                num.BorderBrush = Brushes.Red;
                numDel = false;
            }
            if (string.IsNullOrWhiteSpace(nom.Text) == true || int.TryParse(String.Concat(nom.Text.Where(c => !Char.IsWhiteSpace(c))), out int siret) == false || nom.Text == "Siret du fournisseur"
                || nom.Text == "Le n° de siret doit exister")
            {
                canSubmit = false;
                nom.BorderBrush = Brushes.Red;
                nomDel = false;
            }
            if (string.IsNullOrWhiteSpace(desc.Text) || desc.Text == "Description")
            {
                canSubmit = false;
                desc.BorderBrush = Brushes.Red;
                descDel = false;
            }
            if (string.IsNullOrWhiteSpace(numfourn.Text) == true || numfourn.Text == "N° produit fournisseur"
                || numfourn.Text == "Cette pièce de ce fournisseur est déjà proposée"
                || numfourn.Text == "Ce fournisseur ne possède pas cette pièce")
            {
                canSubmit = false;
                numfourn.BorderBrush = Brushes.Red;
                numfournDel = false;
            }
            if (string.IsNullOrWhiteSpace(prix.Text) == true || int.TryParse(String.Concat(prix.Text.Where(c => !Char.IsWhiteSpace(c))), out cost) == false)
            {
                canSubmit = false;
                prix.BorderBrush = Brushes.Red;
                prixDel = false;
            }
            if (string.IsNullOrWhiteSpace(appro.Text) == true || !int.TryParse(String.Concat(appro.Text.Where(c => !Char.IsWhiteSpace(c))), out delay))
            {
                canSubmit = false;
                appro.BorderBrush = Brushes.Red;
                approDel = false;
            }
            if (DateTime.TryParse(String.Concat(dateIntro.Text.Where(c => !Char.IsWhiteSpace(c))), out dtIn) == false)
            {
                canSubmit = false;
                dateIntro.BorderBrush = Brushes.Red;
                introDel = false;
            }
            if (dateDisc.Text != "Date discontinuité" && string.IsNullOrWhiteSpace(dateDisc.Text) == false
                && DateTime.TryParse(String.Concat(dateDisc.Text.Where(c => !Char.IsWhiteSpace(c))), out dtDc) == false)
            {
                canSubmit = false;
                dateDisc.BorderBrush = Brushes.Red;
                discDel = false;
            }
            if (dateDisc.Text != "Date discontinuité" && string.IsNullOrWhiteSpace(dateDisc.Text) == false)
            {
                valDc = true;
            }
            if (string.IsNullOrWhiteSpace(quantite.Text) || !int.TryParse(String.Concat(quantite.Text.Where(c => !Char.IsWhiteSpace(c))), out number))
            {
                canSubmit = false;
                quantite.BorderBrush = Brushes.Red;
                quantiteDel = false;
            }
            if (canSubmit)
            {
                MySqlCommand fourExists = MainWindow.maConnexion.CreateCommand();
                fourExists.CommandText = "select * from fournisseur where Siret = @siret";
                fourExists.Parameters.Add("@siret", MySqlDbType.VarChar).Value = String.Concat(nom.Text.Where(c => !Char.IsWhiteSpace(c)));
                MySqlDataReader reader = fourExists.ExecuteReader();
                string ans = reader.Read().ToString();
                reader.Close();

                if (ans == "True")
                {

                    MySqlCommand com = MainWindow.maConnexion.CreateCommand();
                    com.CommandText = "select * from PieceFournisseur where Siret = @siret AND numProduit = @numProd";
                    com.Parameters.Add("@siret", MySqlDbType.VarChar).Value = String.Concat(nom.Text.Where(c => !Char.IsWhiteSpace(c)));
                    com.Parameters.Add("@numProd", MySqlDbType.VarChar).Value = GoodMaj(String.Concat(numfourn.Text.Where(c => !Char.IsWhiteSpace(c))));
                    reader = com.ExecuteReader();
                    ans = reader.Read().ToString();
                    com.Dispose();

                    if (ans == "True")
                    {
                        MySqlCommand coupleProduitSiretExists = MainWindow.maConnexion.CreateCommand();
                        coupleProduitSiretExists.CommandText = "SELECT num_Produit_fournisseur, Siret FROM pieces WHERE num_Produit_fournisseur=@num AND Siret=@siret";
                        coupleProduitSiretExists.Parameters.Add("@siret", MySqlDbType.VarChar).Value = String.Concat(nom.Text.Where(c => !Char.IsWhiteSpace(c)));
                        coupleProduitSiretExists.Parameters.Add("@num", MySqlDbType.VarChar).Value = GoodMaj(String.Concat(numfourn.Text.Where(c => !Char.IsWhiteSpace(c))));
                        reader = coupleProduitSiretExists.ExecuteReader();
                        ans = reader.Read().ToString();
                        coupleProduitSiretExists.Dispose();

                        if (ans == "False" || value == 1)
                        {
                            string insertTable = "";
                            if (value == 0)
                            {
                                insertTable = "insert into pieces values (@num, @description, @nom, @numfourn, @prix, @dtIn, @dtDc, @delai, @number)";
                            }
                            if (value == 1)
                            {
                                insertTable = "update pieces set descr=@description, num_Produit_fournisseur=@numfourn, prix=@prix" +
                                    ", dateIntro=@dtIn, dateDiscont=@dtDc, delaiAppr=@delai, quantite=@number WHERE numPiece=@num AND Siret=@nom";
                            }

                            MySqlCommand command = MainWindow.maConnexion.CreateCommand();
                            command.CommandText = insertTable;
                            command.Parameters.Add("@num", MySqlDbType.VarChar).Value = String.Concat(num.Text.ToUpper().Where(c => !Char.IsWhiteSpace(c)));
                            command.Parameters.Add("@description", MySqlDbType.VarChar).Value = GoodMaj(desc.Text);
                            command.Parameters.Add("@nom", MySqlDbType.VarChar).Value = String.Concat(nom.Text.Where(c => !Char.IsWhiteSpace(c)));
                            command.Parameters.Add("@numfourn", MySqlDbType.VarChar).Value = GoodMaj(String.Concat(numfourn.Text.Where(c => !Char.IsWhiteSpace(c))));
                            command.Parameters.Add("@prix", MySqlDbType.Int32).Value = cost;
                            command.Parameters.Add("@dtIn", MySqlDbType.DateTime).Value = dtIn;

                            if (valDc)
                            {
                                command.Parameters.Add("@dtDc", MySqlDbType.DateTime).Value = dtDc;
                            }
                            else
                            {
                                command.Parameters.Add("@dtDc", MySqlDbType.DateTime).Value = null;
                            }
                            command.Parameters.Add("@number", MySqlDbType.Int32).Value = number;
                            command.Parameters.Add("@delai", MySqlDbType.Int32).Value = delay;
                            try
                            {
                                command.ExecuteNonQuery();
                            }
                            catch (MySqlException er)
                            {
                                MessageBox.Show(er.ToString());
                                if (er.Code == 0)
                                {
                                    tampnum = num.Text;
                                    num.Text = "L'association n° produit / siret doit être unique";
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
                                MainWindow.Accueil.NavigationService.Navigate(new Pieces());
                            }
                        }
                        else
                        {
                            tampfourn = numfourn.Text;
                            numfourn.Text = "Cette pièce de ce fournisseur est déjà proposée";
                            numfourn.TextAlignment = TextAlignment.Center;
                            numfourn.BorderBrush = Brushes.Red;
                            numfourn.Foreground = Brushes.Red;
                            numfournDel = false;
                        }
                    }
                    else
                    {
                        tampfourn = numfourn.Text;
                        numfourn.Text = "Ce fournisseur ne possède pas cette pièce";
                        numfourn.TextAlignment = TextAlignment.Center;
                        numfourn.BorderBrush = Brushes.Red;
                        numfourn.Foreground = Brushes.Red;
                        numfournDel = false;
                    }
                }
                else
                {
                    tampnom = nom.Text;
                    nom.Text = "Ce fournisseur n'existe pas";
                    nom.TextAlignment = TextAlignment.Center;
                    nom.BorderBrush = Brushes.Red;
                    nom.Foreground = Brushes.Red;
                    nomDel = false;
                }
            }
            if (!canSubmit)
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
            if (e.Key == Key.Return && Submit.IsEnabled)
            {
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(num), null);
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(desc), null);
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(nom), null);
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(numfourn), null);
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(prix), null);
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(dateIntro), null);
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(dateDisc), null);
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(appro), null);
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(quantite), null);
                Keyboard.ClearFocus();
                Submit_Click(sender, e);
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
                if (int.TryParse(nom.Text, out int z) && !string.IsNullOrEmpty(nom.Text))
                {
                    MySqlCommand command = MainWindow.maConnexion.CreateCommand();
                    string request = "SELECT * FROM pieces WHERE numPiece=@num AND Siret=@siret";
                    command.CommandText = request;
                    command.Parameters.Add("@num", MySqlDbType.VarChar).Value = num.Text;
                    command.Parameters.Add("@siret", MySqlDbType.Int32).Value = int.Parse(nom.Text);
                    MySqlDataReader reader = command.ExecuteReader();
                    string ans = reader.Read().ToString();

                    if (ans == "False")
                    {
                        Unaccessible(desc);
                        desc.Text = "Description";
                        Unaccessible(numfourn);
                        numfourn.Text = "n° produit fournisseur";
                        Unaccessible(prix);
                        prix.Text = "Prix";
                        Unaccessible(dateIntro);
                        dateIntro.Text = "Date d'introduction";
                        Unaccessible(dateDisc);
                        dateDisc.Text = "Date discontinuité";
                        Unaccessible(appro);
                        appro.Text = "Délai d'approvisionnement";
                        Unaccessible(quantite);
                        quantite.Text = "Quantité";
                        Submit.IsEnabled = false;
                    }
                    else
                    {
                        reader.Close();
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            desc.Text = reader.GetString(1);
                            descDel = true;
                            Reaccessible(desc);
                            numfourn.Text = reader.GetString(3);
                            numfournDel = true;
                            Reaccessible(numfourn);
                            prix.Text = reader.GetString(4);
                            prixDel = true;
                            Reaccessible(prix);
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
                            appro.Text = reader.GetString(7);
                            approDel = true;
                            Reaccessible(appro);
                            quantite.Text = reader.GetString(8);
                            quantiteDel = true;
                            Reaccessible(quantite);
                            Submit.IsEnabled = true;
                        }
                    }

                    command.Dispose();
                }
                else if (unable)
                {
                    Unaccessible(desc);
                    desc.Text = "Description";
                    Unaccessible(numfourn);
                    numfourn.Text = "n° produit fournisseur";
                    Unaccessible(prix);
                    prix.Text = "Prix";
                    Unaccessible(dateIntro);
                    dateIntro.Text = "Date d'introduction";
                    Unaccessible(dateDisc);
                    dateDisc.Text = "Date discontinuité";
                    Unaccessible(appro);
                    appro.Text = "Délai d'approvisionnement";
                    Unaccessible(quantite);
                    quantite.Text = "Quantité";
                    Submit.IsEnabled = false;
                }
            }
        }
    }
}

