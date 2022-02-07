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
using System.Text.RegularExpressions;

namespace bdd_projet
{
    /// <summary>
    /// Logique d'interaction pour CreationCommande.xaml
    /// </summary>
    public partial class CreationCommande : Page
    {

        #region bools de focus
        bool numCoDel = false;
        bool numPDel = false;
        bool qteDel = false;
        bool numClDel = false;
        bool dateCDel = false;
        bool addLDel = false;
        bool dateLDel = false;
        bool unable = false;
        #endregion

        private int value = 0;
        private bool firstTime = true;
        private List<string> items = new List<string> { };

        public CreationCommande(int value)
        {
            InitializeComponent();
            this.value = value;
            if (value == 1)
            {
                numP.IsEnabled = true;
                qte.IsEnabled = true;
                numCl.IsEnabled = false;
                dateC.IsEnabled = false;
                addL.IsEnabled = false;
                dateL.IsEnabled = false;
                Submit.IsEnabled = false;
            }
            unable = true;
            numCo.Focus();
        }

        #region GestionFocus
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
        private void numP_GotFocus(object sender, RoutedEventArgs e)
        {
            if (numPDel == false)
            {
                numP.Text = "";
                numP.TextAlignment = TextAlignment.Left;
                numP.BorderBrush = Brushes.Black;
                numPDel = true;
                numP.Foreground = Brushes.Black;
            }
        }
        private void qte_GotFocus(object sender, RoutedEventArgs e)
        {
            if (qteDel == false)
            {
                qte.Text = "";
                qte.TextAlignment = TextAlignment.Left;
                qte.BorderBrush = Brushes.Black;
                qteDel = true;
                qte.Foreground = Brushes.Black;
            }
        }
        private void numCl_GotFocus(object sender, RoutedEventArgs e)
        {
            if (numClDel == false)
            {
                numCl.Text = "";
                numCl.TextAlignment = TextAlignment.Left;
                numCl.BorderBrush = Brushes.Black;
                numClDel = true;
                numCl.Foreground = Brushes.Black;
            }
        }
        private void dateC_GotFocus(object sender, RoutedEventArgs e)
        {
            if (dateCDel == false)
            {
                dateC.Text = "";
                dateC.TextAlignment = TextAlignment.Left;
                dateC.BorderBrush = Brushes.Black;
                dateCDel = true;
                dateC.Foreground = Brushes.Black;
            }
        }
        private void addL_GotFocus(object sender, RoutedEventArgs e)
        {
            if (addLDel == false)
            {
                addL.Text = "";
                addL.TextAlignment = TextAlignment.Left;
                addL.BorderBrush = Brushes.Black;
                addLDel = true;
                addL.Foreground = Brushes.Black;
            }
        }
        private void dateL_GotFocus(object sender, RoutedEventArgs e)
        {
            if (dateLDel == false)
            {
                dateL.Text = "";
                dateL.FontSize = 12;
                dateL.TextAlignment = TextAlignment.Left;
                dateL.BorderBrush = Brushes.Black;
                dateLDel = true;
                dateL.Foreground = Brushes.Black;
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
        private void numP_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(numP.Text))
            {
                numP.Text = "N° de produit";
                numP.Foreground = Brushes.DarkGray;
                numP.TextAlignment = TextAlignment.Left;
                numPDel = false;
                numP.BorderBrush = Brushes.DarkGray;
            }
        }
        private void qte_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(qte.Text))
            {
                qte.Text = "Quantité";
                qte.Foreground = Brushes.DarkGray;
                qte.TextAlignment = TextAlignment.Left;
                qteDel = false;
                qte.BorderBrush = Brushes.DarkGray;
            }
        }
        private void numCl_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(numCl.Text))
            {
                numCl.Text = "N° de client";
                numCl.Foreground = Brushes.DarkGray;
                numCl.TextAlignment = TextAlignment.Left;
                numClDel = false;
                numCl.BorderBrush = Brushes.DarkGray;
            }
        }
        private void dateC_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(dateC.Text))
            {
                dateC.Text = "Date de commande";
                dateC.Foreground = Brushes.DarkGray;
                dateC.TextAlignment = TextAlignment.Left;
                dateCDel = false;
                dateC.BorderBrush = Brushes.DarkGray;
            }
        }
        private void addL_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(addL.Text))
            {
                addL.Text = "Adresse de livraison";
                addL.Foreground = Brushes.DarkGray;
                addL.TextAlignment = TextAlignment.Left;
                addLDel = false;
                addL.BorderBrush = Brushes.DarkGray;
            }
        }
        private void dateL_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(dateL.Text))
            {
                dateL.Text = "Date de livraison";
                dateL.Foreground = Brushes.DarkGray;
                dateL.TextAlignment = TextAlignment.Left;
                dateLDel = false;
                dateL.BorderBrush = Brushes.DarkGray;
            }
        }
        #endregion

        public int NumberOccurence(List<string> items, string item)
        {
            int count = 0;
            for (int i = 0; i < items.Count; i += 2)
            {
                if (items[i] == item) { count++; }
            }
            return count;
        }
        public bool CheckIfEnough(string numPiece, int nb)
        {
            if (numPiece.ToLower() != "null")
            {
                MySqlCommand command = MainWindow.maConnexion.CreateCommand();
                command.CommandText = "SELECT * FROM PIECES WHERE numPiece  = @piece AND quantite >= @number";
                command.Parameters.Add("@piece", MySqlDbType.VarChar).Value = numPiece.ToUpper();
                command.Parameters.Add("@number", MySqlDbType.Int32).Value = nb;

                MySqlDataReader reader = command.ExecuteReader();
                string ans = reader.Read().ToString();
                reader.Close();
                command.Dispose();
                //MessageBox.Show(numPiece + "     " + ans);
                if (ans == "True") { return true; }
                return false;
            }

            return true;
        }
        private void Submit_Click(object send, RoutedEventArgs e)
        {
            bool canSubmit = true;
            bool endDate = false;
            DateTime dtCo = new DateTime();
            DateTime dtL = new DateTime();
            int num = 0;


            if (string.IsNullOrWhiteSpace(numCo.Text) == true || numCo.Text == "N° de commande" || numCo.Text == "Invalid argument")
            {
                canSubmit = false;
                numCo.Text = "Invalid argument";
                numCo.TextAlignment = TextAlignment.Center;
                numCo.BorderBrush = Brushes.Red;
                numCo.Foreground = Brushes.Red;
                numCoDel = false;
            }
            if (string.IsNullOrWhiteSpace(numCl.Text) == true || int.TryParse(numCl.Text, out num) == false)
            {
                canSubmit = false;
                numCl.Text = "Invalid argument";
                numCl.TextAlignment = TextAlignment.Center;
                numCl.BorderBrush = Brushes.Red;
                numCl.Foreground = Brushes.Red;
                numClDel = false;
            }
            if (DateTime.TryParse(dateC.Text, out dtCo) == false)
            {
                canSubmit = false;
                dateC.Text = "Invalid argument";
                dateC.TextAlignment = TextAlignment.Center;
                dateC.BorderBrush = Brushes.Red;
                dateC.Foreground = Brushes.Red;
                dateCDel = false;
            }
            if (string.IsNullOrWhiteSpace(addL.Text) || addL.Text == "Adresse de livraison" || addL.Text == "Invalid argument")
            {
                canSubmit = false;
                addL.Text = "Invalid argument";
                addL.TextAlignment = TextAlignment.Center;
                addL.BorderBrush = Brushes.Red;
                addL.Foreground = Brushes.Red;
                addLDel = false;
            }
            if (!string.IsNullOrWhiteSpace(dateL.Text) && dateL.Text != "Date de livraison" && DateTime.TryParse(dateL.Text, out dtL) == false && dateL.Text != "Impossible de commander à cette date")
            {
                canSubmit = false;
                dateL.Text = "Invalid argument";
                dateL.TextAlignment = TextAlignment.Center;
                dateL.BorderBrush = Brushes.Red;
                dateL.Foreground = Brushes.Red;
                dateLDel = false;
            }
            if (!string.IsNullOrWhiteSpace(dateL.Text) && dateL.Text != "Date de livraison" && dateL.Text != "Impossible de commander à cette date")
            {
                endDate = true;
            }
            if (canSubmit)
            {
                //MessageBox.Show("test num commande");
                MySqlCommand command = MainWindow.maConnexion.CreateCommand();
                command.CommandText = "SELECT * FROM COMMANDE WHERE No_commande = @command";
                command.Parameters.Add("@command", MySqlDbType.VarChar).Value = numCo.Text.ToUpper();

                MySqlDataReader reader = command.ExecuteReader();
                string ans = reader.Read().ToString();
                reader.Close();
                command.Dispose();
                if (ans == "True" & value == 0) //Si le numéro de commande est déjà attribué
                {
                    numCo.Text = "Numéro de commande non unique";
                    numCo.Foreground = Brushes.Red;
                    numCo.BorderBrush = Brushes.Red;
                    numCoDel = false;
                }
                else
                {

                    //MessageBox.Show("test existence pieces");
                    bool exists = true;
                    bool moreThanOnce = false;
                    Regex rx = new Regex("^-?[0-9]+$");
                    for (int i = 0; i < items.Count; i += 2)
                    {
                        if (rx.Match(items[i]).Success)
                        {
                            MySqlCommand com = MainWindow.maConnexion.CreateCommand();
                            com.CommandText = "SELECT cadre, guidon, frein, selle, derailleurAvant, derailleurArriere, roueAvant," +
                                " roueArriere, reflecteurs, pedalier, ordinateur, panier FROM ASSEMBLAGE WHERE numVelo  = @num";
                            com.Parameters.Add("@num", MySqlDbType.VarChar).Value = items[i];

                            reader = com.ExecuteReader();
                            List<string> components = new List<string> { };
                            while (reader.Read())
                            {
                                for (int j = 0; j < 12; j++)
                                {
                                    if (!reader.IsDBNull(j) && j > 0)
                                    {
                                        components.Add(reader.GetString(j));
                                    }
                                }
                            }
                            reader.Close();
                            foreach (string s in components)
                            {
                                if (!CheckIfEnough(s, int.Parse(items[i + 1]))) { exists = false; }
                            }
                        }
                        else
                        {
                            MySqlCommand com = MainWindow.maConnexion.CreateCommand();
                            com.CommandText = "SELECT * FROM PIECES WHERE pieces.numPiece = @num AND pieces.quantite>=@quant";
                            com.Parameters.Add("@num", MySqlDbType.VarChar).Value = items[i];
                            com.Parameters.Add("@quant", MySqlDbType.Int32).Value = int.Parse(items[i + 1]);

                            reader = com.ExecuteReader();
                            ans = reader.Read().ToString();
                            if (ans == "False") { exists = false; }
                            reader.Close();
                            com.Dispose();

                            if (NumberOccurence(items, items[i]) > 1) { moreThanOnce = true; }
                            //MessageBox.Show(NumberOccurence(items, items[i]).ToString());
                        }
                    }
                    if (!exists) //Si aucune pièce ne correspond (existe pas, pas assez)
                    {
                        Liste.Items.Clear();
                        items.Clear();
                        numP.Text = "Pièce manquante ou en trop faible quantité";
                        numP.Foreground = Brushes.Red;
                        numP.BorderBrush = Brushes.Red;
                        numPDel = false;
                    }
                    else if (moreThanOnce)
                    {
                        numP.Text = "Pièce commandée en double";
                        numP.Foreground = Brushes.Red;
                        numP.BorderBrush = Brushes.Red;
                        numPDel = false;
                    }
                    else
                    {
                        //MessageBox.Show("test num client");
                        MySqlCommand com = MainWindow.maConnexion.CreateCommand();
                        com.CommandText = "SELECT * FROM CLIENTS WHERE No_client = @num";
                        com.Parameters.Add("@num", MySqlDbType.Int32).Value = int.Parse(numCl.Text);

                        reader = com.ExecuteReader();
                        ans = reader.Read().ToString();
                        reader.Close();
                        com.Dispose();
                        if (ans == "False" & value == 0) //Si aucun client avec ce numéro n'est enregistré
                        {
                            numCl.Text = "Ce client n'existe pas";
                            numCl.Foreground = Brushes.Red;
                            numCl.BorderBrush = Brushes.Red;
                            numClDel = false;
                        }
                        else
                        {
                            numCl.IsEnabled = false;
                            //MessageBox.Show("test date");
                            if (endDate && (DateTime.Parse(dateL.Text) - DateTime.Parse(dateC.Text)).TotalDays < 5)
                            {
                                dateL.Text = "Impossible de commander à cette date";
                                dateL.TextAlignment = TextAlignment.Center;
                                dateL.BorderBrush = Brushes.Red;
                                dateL.Foreground = Brushes.Red;
                                dateLDel = false;
                            }
                            else
                            {

                                //MessageBox.Show("insertion dans commande");

                                string insertTable = "";
                                if (value == 0)
                                {
                                    insertTable = "insert into commande values (@numCo, @numCl, @dtCo, @addL, @dtL)";
                                }
                                if (value == 1)
                                {

                                    insertTable = "update commande set adresse_livraison=@addL,date_livraison=@dtL where No_commande=@numCo  ";
                                }
                                command = MainWindow.maConnexion.CreateCommand();
                                command.CommandText = insertTable;
                                command.Parameters.Add("@numCo", MySqlDbType.VarChar).Value = numCo.Text.ToUpper();
                                command.Parameters.Add("@numP", MySqlDbType.VarChar).Value = numCo.Text.ToUpper();
                                command.Parameters.Add("@qte", MySqlDbType.Int32).Value = numCo.Text.ToUpper();
                                command.Parameters.Add("@numCl", MySqlDbType.Int32).Value = num;
                                command.Parameters.Add("@dtCo", MySqlDbType.DateTime).Value = dtCo;
                                command.Parameters.Add("@addL", MySqlDbType.VarChar).Value = GoodMaj(addL.Text);
                                command.Parameters.Add("@dtL", MySqlDbType.DateTime).Value = dtL;

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
                                string delete = "DELETE from itemcommande where No_commande=@numC ";
                                com = MainWindow.maConnexion.CreateCommand();
                                com.CommandText = delete;
                                com.Parameters.Add("@numC", MySqlDbType.VarChar).Value = numCo.Text.ToUpper();
                                try
                                {
                                    com.ExecuteNonQuery();
                                }
                                catch (MySqlException er)
                                {
                                    MessageBox.Show(er.ToString());
                                    return;
                                }
                                //MessageBox.Show("insertion dans itemcommande");
                                for (int i = 0; i < items.Count; i += 2)
                                {
                                    string insertItems = "insert into itemcommande values (@numC, @numP, @qte)";
                                    com.Dispose();
                                    com.CommandText = insertItems;
                                    com.Parameters.Add("@numP", MySqlDbType.VarChar).Value = items[i];
                                    com.Parameters.Add("@qte", MySqlDbType.Int32).Value = int.Parse(items[i + 1]);
                                    try
                                    {
                                        com.ExecuteNonQuery();
                                    }
                                    catch (MySqlException er)
                                    {
                                        MessageBox.Show(er.ToString());
                                        return;
                                    }
                                    com.Dispose();
                                }
                                MainWindow.Accueil.NavigationService.Navigate(new Commande());
                            }

                        }
                    }
                }

            }
            if (canSubmit == false)
            {
                ThicknessAnimation marginAn = new ThicknessAnimation(new Thickness(-10, 0, 0, 0), new Thickness(10, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 400));
                marginAn.EasingFunction = new BounceEase();

                MainWindow.Accueil.BeginAnimation(Control.MarginProperty, marginAn);
                Loading();
            }
        }

        private void Ajout_Click(object send, RoutedEventArgs e)
        {
            bool canSubmit = true;

            int numQ = 0;

            if (string.IsNullOrWhiteSpace(numCo.Text) == true || numCo.Text == "N° de commande" || numCo.Text == "Invalid argument")
            {
                canSubmit = false;
                numCo.Text = "Invalid argument";
                numCo.TextAlignment = TextAlignment.Center;
                numCo.BorderBrush = Brushes.Red;
                numCo.Foreground = Brushes.Red;
                numCoDel = false;
            }
            if (string.IsNullOrWhiteSpace(numP.Text) == true || numP.Text == "N° item" || numP.Text == "Invalid argument")
            {
                canSubmit = false;
                numP.Text = "Invalid argument";
                numP.TextAlignment = TextAlignment.Center;
                numP.BorderBrush = Brushes.Red;
                numP.Foreground = Brushes.Red;
                numPDel = false;
            }
            if (string.IsNullOrWhiteSpace(qte.Text) == true || int.TryParse(qte.Text, out numQ) == false)
            {
                canSubmit = false;
                qte.Text = "Invalid argument";
                qte.TextAlignment = TextAlignment.Center;
                qte.BorderBrush = Brushes.Red;
                qte.Foreground = Brushes.Red;
                qteDel = false;
            }
            if (canSubmit)
            {
                if (numCoDel == true)
                {
                    Liste.Items.Add(qte.Text + " * " + numP.Text.ToUpper() + "\n");
                    items.Add(numP.Text.ToUpper());
                    items.Add(numQ.ToString());
                }

            }
            if (canSubmit == false)
            {
                ThicknessAnimation marginAn = new ThicknessAnimation(new Thickness(-10, 0, 0, 0), new Thickness(10, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 400));
                marginAn.EasingFunction = new BounceEase();

                MainWindow.Accueil.BeginAnimation(Control.MarginProperty, marginAn);
                Loading();
            }
        }

        DispatcherTimer timer = new DispatcherTimer();
        void Loading()
        {
            timer.Tick += new EventHandler(delegate (Object o, EventArgs a)
            {
                timer.Stop();
                ThicknessAnimation marginAn = new ThicknessAnimation(new Thickness(10, 0, 0, 0), new Thickness(0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 1));
                marginAn.EasingFunction = new QuadraticEase();

                MainWindow.Accueil.BeginAnimation(Control.MarginProperty, marginAn);
            });
            timer.Interval = TimeSpan.FromSeconds(0.29);
            timer.Start();
        }
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
            if (e.Key == Key.Return)
            {
                Submit_Click(sender, e);
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(numCl), null);
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(dateC), null);
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(addL), null);
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(dateL), null);
                Keyboard.ClearFocus();
                Ajout_Click(sender, e);
                //FocusManager.SetFocusedElement(FocusManager.GetFocusScope(numCo), null);
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(numP), null);
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(qte), null);
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

            if (value == 0)
            {

                Reaccessible(dateC);
                dateC.Text = DateTime.Now.ToString("yyyy-MM-dd");
                dateCDel = true;

                Reaccessible(dateL);
                dateL.Text = DateTime.Now.AddDays(5).ToString("yyyy-MM-dd");
                dateLDel = true;

            }
        }
        private void numCo_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool found = false;
            if (value == 1)
            {

                MySqlCommand command = MainWindow.maConnexion.CreateCommand();
                string request = "SELECT * FROM commande WHERE No_commande=@numCo";
                command.CommandText = request;
                command.Parameters.Add("@numCo", MySqlDbType.VarChar).Value = numCo.Text;

                //int numQ = 0;
                MySqlCommand command1 = MainWindow.maConnexion.CreateCommand();
                string request1 = "SELECT * FROM itemcommande WHERE No_commande=@numCo";
                command1.CommandText = request1;
                command1.Parameters.Add("@numCo", MySqlDbType.VarChar).Value = numCo.Text;
                MySqlDataReader reader1 = command1.ExecuteReader();
                while (reader1.Read())
                {
                    items.Add(reader1.GetString(1));
                    items.Add(reader1.GetString(2));
                }
                reader1.Close();
                command1.Dispose();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {

                    numCl.Text = reader.GetString(1);
                    numClDel = true;
                    //Reaccessible(numCl);
                    dateC.Text = Convert.ToDateTime(reader.GetString(2)).ToString("yyyy-MM-dd");
                    dateCDel = true;
                    //Reaccessible(dateC);
                    addL.Text = reader.GetString(3);
                    addLDel = true;
                    Reaccessible(addL);
                    dateL.Text = Convert.ToDateTime(reader.GetString(4)).ToString("yyyy-MM-dd");
                    dateLDel = true;
                    Reaccessible(dateL);
                    found = true;
                    Reaccessible(qte);
                    Reaccessible(numP);


                    for (int i = 1; i < items.Count; i += 2)
                    {
                        Liste.Items.Add(items[i] + " * " + items[i - 1] + "\n");
                    }

                }

                command.Dispose();

                Submit.IsEnabled = true;

                //items.Clear();
            }
            if (!found && !firstTime && value == 1)
            {
                Unaccessible(numCl);
                numCl.Text = "No_client";
                Unaccessible(dateC);
                dateC.Text = "Date de commande";
                Unaccessible(addL);
                addL.Text = "Adresse de livraison";
                Unaccessible(dateL);
                dateL.Text = "Date de livraison";
                Submit.IsEnabled = false;
            }


        }
        private void del_Click(object sender, RoutedEventArgs e)
        {
            if (Liste.Items.Count > 0)
            {
                string nrv = Liste.Items.GetItemAt(Liste.SelectedIndex).ToString().Replace("\n", "");
                string[] tab = nrv.Split(' ');
                int index = items.IndexOf(tab[2]);

                items.RemoveAt(index);
                items.RemoveAt(index);
                Liste.Items.RemoveAt(Liste.SelectedIndex);
            }
        }
    }
}
