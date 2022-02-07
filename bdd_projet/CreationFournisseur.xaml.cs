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
//using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using System.Data;
using Control = System.Windows.Controls.Control;
using TextBox = System.Windows.Controls.TextBox;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace bdd_projet
{
    /// <summary>
    /// Logique d'interaction pour CreationFournisseur.xaml
    /// </summary>
    public partial class CreationFournisseur : Page
    {
        private List<int> listeNum = new List<int> { };
        private int value = 0;
        MySqlCommand cmd;
        private string connexionString2 = "SERVER=localhost;PORT=3306;" +
                                          "DATABASE=VELOMAX;" +
                                          "UID=root;PASSWORD=NEONALPHAUOMO3";

        #region bool de focus
        private bool numDel = false;
        private bool nomDel = false;
        private bool nom_contactdel = false;
        private bool adressedel = false;
        private bool labeldel = false;
        private bool unable = false;
        #endregion

        public CreationFournisseur(List<int> listeNum, int value)
        {
            InitializeComponent();
            this.value = value;
            if (value == 1)
            {
                num.Visibility = Visibility.Visible;
                num.Text = "N° de l'entreprise à modifier";
                nom.IsEnabled = false;
                nom_contact.IsEnabled = false;
                adresse.IsEnabled = false;
                label.IsEnabled = false;
                Submit.IsEnabled = false;
            }
            this.listeNum = listeNum;
            unable = true;
            num.Focus();
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

        private void nom_contact_GotFocus(object sender, RoutedEventArgs e)
        {
            if (nom_contactdel == false)
            {
                nom_contact.Text = "";
                nom_contact.TextAlignment = TextAlignment.Left;
                nom_contact.BorderBrush = Brushes.Black;
                nom_contactdel = true;
                nom_contact.Foreground = Brushes.Black;
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

        private void label_GotFocus(object sender, RoutedEventArgs e)
        {
            if (labeldel == false)
            {
                label.Text = "";
                label.TextAlignment = TextAlignment.Left;
                label.BorderBrush = Brushes.Black;
                labeldel = true;
                label.Foreground = Brushes.Black;
            }
        }


        private void num_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(num.Text))
            {
                if (value == 0)
                {
                    num.Text = "Numéro du Siret";
                }
                if (value == 1)
                {
                    num.Text = "Numéro du Siret (pour modification)";
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
                nom.Text = "Nom de l'entreprise";
                nom.Foreground = Brushes.DarkGray;
                nom.TextAlignment = TextAlignment.Left;
                nomDel = false;
                nom.BorderBrush = Brushes.DarkGray;
            }
        }

        private void nom_contact_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nom_contact.Text))
            {

                nom_contact.Text = "Nom du contact";
                nom_contact.Foreground = Brushes.DarkGray;
                nom_contact.TextAlignment = TextAlignment.Left;
                nom_contactdel = false;
                nom_contact.BorderBrush = Brushes.DarkGray;
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

        private void label_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(label.Text))
            {
                label.Text = "Label";
                label.Foreground = Brushes.DarkGray;
                label.TextAlignment = TextAlignment.Left;
                labeldel = false;
                label.BorderBrush = Brushes.DarkGray;
            }
        }
        #endregion




        DispatcherTimer timer = new DispatcherTimer();

        #region Gestiona
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
            MySqlConnection maConnexion2 = new MySqlConnection(connexionString2);
            //if (string.IsNullOrWhiteSpace(num.Text) == true || int.TryParse(num.Text, out val) == false)
            //{
            //    canSubmit = false;
            //    num.Text = "Argument invalide";
            //    num.TextAlignment = TextAlignment.Center;
            //    num.BorderBrush = Brushes.Red;
            //    num.Foreground = Brushes.Red;
            //    numDel = false;
            //}

            if (string.IsNullOrWhiteSpace(nom.Text) == true || int.TryParse(nom.Text, out int i) == true
                || nom.Text == "Nom de l'entreprise" || nom.Text == "Argument invalide")
            {
                canSubmit = false;
                nom.Text = "Argument invalide";
                nom.TextAlignment = TextAlignment.Center;
                nom.BorderBrush = Brushes.Red;
                nom.Foreground = Brushes.Red;
                nomDel = false;
            }
            if (string.IsNullOrWhiteSpace(nom_contact.Text) == true || int.TryParse(nom_contact.Text, out int j) == true
                || nom_contact.Text == "Nom du contact" || nom_contact.Text == "Argument invalide")
            {
                canSubmit = false;
                nom_contact.Text = "Argument invalide";
                nom_contact.TextAlignment = TextAlignment.Center;
                nom_contact.BorderBrush = Brushes.Red;
                nom_contact.Foreground = Brushes.Red;
                nom_contactdel = false;
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
            if (string.IsNullOrWhiteSpace(label.Text) == true || int.TryParse(label.Text, out d) == false || Convert.ToInt32(label.Text) > 4 || Convert.ToInt32(label.Text) < 1)
            {
                canSubmit = false;
                label.Text = "Argument invalide";
                label.TextAlignment = TextAlignment.Center;
                label.BorderBrush = Brushes.Red;
                label.Foreground = Brushes.Red;
                labeldel = false;
            }

            if (canSubmit)
            {
                string insertTable = "";
                bool b = true;
                bool c = false;
                int no = 0;
                numDel = true;
                while (b)
                {
                    Random alea = new Random();
                    no = alea.Next(999999999);//peut pas mettre 13 chiffres
                    for (int ao = 0; ao <= Client.IDs.Count - 1; ao++)
                    {
                        if (no == Client.IDs[ao])
                        {
                            c = true;
                        }
                    }
                    if (c == false)
                    {
                        b = false;
                    }

                }

                if (value == 0)
                {
                    insertTable = "insert into Fournisseur values (@no,@nom, @nom_contact, @adresse, @label)";
                    try
                    {
                        cmd = new MySqlCommand(insertTable, MainWindow.maConnexion);
                        cmd.Parameters.AddWithValue("@no", no);
                        cmd.Parameters.AddWithValue("@nom", nom.Text);
                        cmd.Parameters.AddWithValue("@nom_contact", nom_contact.Text);
                        cmd.Parameters.AddWithValue("@adresse", adresse.Text);
                        cmd.Parameters.AddWithValue("@label", Convert.ToInt32(label.Text));
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
                    insertTable = "UPDATE Fournisseur SET nom_entreprise=@nom, nom_contact=@nom_contact,adresse=@adresse, label=@label WHERE Siret=@no";

                    try
                    {
                        cmd = new MySqlCommand(insertTable, MainWindow.maConnexion);
                        cmd.Parameters.AddWithValue("@no", no);
                        cmd.Parameters.AddWithValue("@nom", nom.Text);
                        cmd.Parameters.AddWithValue("@nom_contact", nom_contact.Text);
                        cmd.Parameters.AddWithValue("@adresse", adresse.Text);
                        cmd.Parameters.AddWithValue("@label", Convert.ToInt32(label.Text));
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
                    MainWindow.Accueil.NavigationService.Navigate(new Fournisseur());
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

        private void KeyEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Submit_Click(sender, e);
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(num), null);
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(nom), null);
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(nom_contact), null);
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(adresse), null);
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(label), null);
                Keyboard.ClearFocus();
            }
        }

        private void num_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (value == 1)
            {
                int val = 0;
                if (int.TryParse(num.Text, out val) == true && listeNum.Contains(val))
                {
                    MySqlCommand command = MainWindow.maConnexion.CreateCommand();
                    string request = "SELECT * FROM Fournisseur WHERE Siret=@num";
                    command.CommandText = request;
                    command.Parameters.Add("@num", MySqlDbType.Int32).Value = val;
                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        nom.Text = reader.GetString(1);
                        nomDel = true;
                        Reaccessible(nom);
                        nom_contact.Text = reader.GetString(2);
                        nom_contactdel = true;
                        Reaccessible(nom_contact);
                        adresse.Text = reader.GetString(3);
                        adressedel = true;
                        Reaccessible(adresse);
                        label.Text = reader.GetString(4);
                        labeldel = true;
                        Reaccessible(label);
                    }
                    reader.Close();
                    command.Dispose();
                    Submit.IsEnabled = true;
                }
                else if (unable)
                {
                    Unaccessible(nom);
                    nom.Text = "Nom de l'entreprise";
                    Unaccessible(nom_contact);
                    nom_contact.Text = "Nom du contact";
                    Unaccessible(adresse);
                    adresse.Text = "Adresse";
                    Unaccessible(nom_contact);
                    label.Text = "Label";
                    Unaccessible(label);
                    Submit.IsEnabled = false;
                }
            }
        }



    }
}
