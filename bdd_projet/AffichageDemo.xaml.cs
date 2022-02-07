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
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using System.Xml.Serialization;
using System.IO;
using Newtonsoft.Json;



namespace bdd_projet
{
    /// <summary>
    /// Logique d'interaction pour AffichageDemo.xaml
    /// </summary>
    public partial class AffichageDemo : Page
    {
        private int value = 0;
        static string JsonString;
        private List<Clients> clients = new List<Clients> { };

        private MySqlCommand cmd;

        public AffichageDemo(int value, bool next)
        {
            InitializeComponent();

            before.IsEnabled = true;
            after.IsEnabled = true;

            ThicknessAnimation db = new ThicknessAnimation();
            ThicknessAnimation dbradio = new ThicknessAnimation();
            if (next)
            {
                db = new ThicknessAnimation(new Thickness(300, 0, 0, 0), new Thickness(0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 400));
            }
            else
            {
                db = new ThicknessAnimation(new Thickness(0, 0, 300, 0), new Thickness(0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 400));
                dbradio = new ThicknessAnimation(new Thickness(0, 130, 500, 0), new Thickness(40, 130, 0, 0), new TimeSpan(0, 0, 0, 0, 400));

                radio.BeginAnimation(Control.MarginProperty, dbradio);
            }
            db.EasingFunction = new ExponentialEase();

            DoubleAnimation doubleAnimation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(2));
            doubleAnimation.EasingFunction = new ExponentialEase();

            Anim.BeginAnimation(Control.MarginProperty, db);
            Anim.BeginAnimation(Control.OpacityProperty, doubleAnimation);

            this.value = value;
            Process();
        }
        public void gamma()
        {

            string query = "Select clients.No_client,clients.nom,clients.prenom,clients.adresse,clients.mail,clients.telephone,fidelio.No_client,fidelio.No_programme,fidelio.date_inscription from clients,fidelio where clients.No_client=fidelio.No_client";
            MySqlCommand command = MainWindow.maConnexion.CreateCommand();
            command.CommandText = query;

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                clients.Add(new Clients(int.Parse(reader.GetString(0)), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), int.Parse(reader.GetString(5)), int.Parse(reader.GetString(7)), DateTime.Parse(reader.GetString(8))));

            }
            reader.Close();

        }
        public void Process()
        {
            if (value == 0)
            {
                string query = "SELECT COUNT(DISTINCT No_client) FROM Clients;";
                MySqlCommand command = MainWindow.maConnexion.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                int nb = reader.Read() ? reader.GetInt32(0) : -1;
                infos.Visibility = Visibility.Visible;

                infos.Text = "Nombre de client dans la base de données : " + nb;
                command.Dispose();

                before.Visibility = Visibility.Hidden;
                dataGrid1.Visibility = Visibility.Hidden;
            }
            if (value == 1)
            {
                Title.Text = "Clients et cumul de leurs\n commandes en euros";
                string query = "SELECT c.nom, c.prenom as 'prénom', prixTotal as 'prix total (en €)' " +
                                "from clients as c, " +
                                "commande, " +
                                    "(SELECT sum(prix) as prixTotal, " +
                                    "No_commande as numCommande " +
                                    "FROM( " +
                                        "SELECT p.prix, i.No_commande " +
                                        "FROM pieces as p, itemcommande as i " +
                                        "where p.numpiece = i.Ref_item " +
                                        "UNION ALL " +
                                        "SELECT v.prix, i.No_commande " +
                                        "FROM velo as v, itemcommande as i " +
                                        "where v.numProduit = i.Ref_item and i.Ref_item REGEXP '^-?[0-9]+$' " +
                                    ") as sub " +
                            "GROUP BY numCommande) as p " +
                        "WHERE numCommande = commande.No_commande and commande.No_client = c.No_client " +
                        "ORDER BY numCommande";

                MySqlCommand command = MainWindow.maConnexion.CreateCommand();
                command.CommandText = query;
                MySqlDataReader reader = command.ExecuteReader();
                dataGrid1.Visibility = Visibility.Visible;
                DataTable dt = new DataTable();
                dt.Load(reader);
                dataGrid1.ItemsSource = dt.DefaultView;

                before.Visibility = Visibility.Visible;
                infos.Visibility = Visibility.Hidden;
            }
            if (value == 2)
            {
                Title.Text = "Liste des pièces avec \nun faible stock";

                string query = "SELECT * from PIECES where quantite < 3";
                MySqlCommand command = MainWindow.maConnexion.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                dataGrid1.Visibility = Visibility.Visible;
                DataTable dt = new DataTable();
                dt.Load(reader);
                dataGrid1.ItemsSource = dt.DefaultView;
            }
            if (value == 3)
            {
                infos.Visibility = Visibility.Hidden;
                Open.Visibility = Visibility.Collapsed;

                Title.Text = "Nombre de pièces fournies \npar fournisseur";

                string query = "SELECT numsiret as 'n° de siret', sum(quantite) as 'quantité' " +
                                "from fournisseur as f, " +
                                "( " +
                                    "SELECT f.siret as numsiret, p.quantite as quantite " +
                                    "from pieces as p, " +
                                    "fournisseur as f " +
                                    "where p.Siret = f.siret " +
                                ") " +
                                "as o " +
                                "WHERE numsiret = f.siret " +
                                "GROUP BY numsiret";

                MySqlCommand command = MainWindow.maConnexion.CreateCommand();
                command.CommandText = query;
                MySqlDataReader reader = command.ExecuteReader();
                dataGrid1.Visibility = Visibility.Visible;
                DataTable dt = new DataTable();
                dt.Load(reader);
                dataGrid1.ItemsSource = dt.DefaultView;
            }
            if (value == 4)
            {
                Title.Text = "Export en xml de la table vélos";
                dataGrid1.Visibility = Visibility.Hidden;
                List<VeloClass> vélos = new List<VeloClass> { };

                string query = "Select * from Velo";
                MySqlCommand command = MainWindow.maConnexion.CreateCommand();
                command.CommandText = query;
                Open.Visibility = Visibility.Visible;
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (!Convert.IsDBNull(reader["dateDiscont"]))
                    {
                        vélos.Add(new VeloClass(int.Parse(reader.GetString(0)), reader.GetString(1), reader.GetString(2), int.Parse(reader.GetString(3)), reader.GetString(4), DateTime.Parse(reader.GetString(5)), DateTime.Parse(reader.GetString(6))));
                    }
                    else
                    {
                        vélos.Add(new VeloClass(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), int.Parse(reader.GetString(3)), reader.GetString(4), DateTime.Parse(reader.GetString(5))));
                    }
                }
                reader.Close();

                XmlSerializer xs = new XmlSerializer(typeof(List<VeloClass>));
                StreamWriter wr = new StreamWriter("velo.xml");
                xs.Serialize(wr, vélos);
                tooltip.Content = "Ouvrir le XAML";
                wr.Close();

                infos.Visibility = Visibility.Visible;

                infos.Text = "Export en XML de la classe Velo";
            }
            if (value == 5)
            {
                Title.Text = "Export en json des souscriptions";
                after.Visibility = Visibility.Hidden;
                dataGrid1.Visibility = Visibility.Hidden;
                Open.Visibility = Visibility.Visible;
                List<Clients> clients = new List<Clients> { };

                string query = "Select clients.No_client,clients.nom,clients.prenom,clients.adresse,clients.mail,clients.telephone,fidelio.No_client,fidelio.No_programme,fidelio.date_inscription from clients,fidelio where clients.No_client=fidelio.No_client";
                MySqlCommand command = MainWindow.maConnexion.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    clients.Add(new Clients(int.Parse(reader.GetString(0)), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), int.Parse(reader.GetString(5)), int.Parse(reader.GetString(7)), DateTime.Parse(reader.GetString(8))));
                    //clients.Add(new Clients(int.Parse(reader.GetString(6)), int.Parse(reader.GetString(7)),DateTime.Parse(reader.GetString(8))));
                }
                reader.Close();
                tooltip.Content = "Ouvrir le JSON";
                JsonString = JsonConvert.SerializeObject(clients, Formatting.Indented);
                using (var streamwriter = new StreamWriter("FideliteExpiration.json"))
                {
                    using (var jsonWriter = new JsonTextWriter(streamwriter))
                    {
                        jsonWriter.Formatting = Formatting.Indented;
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(jsonWriter, JsonConvert.DeserializeObject(JsonString));
                    }
                }

                infos.Visibility = Visibility.Visible;

                infos.Text = "Export en JSON des relances à effectuer";
            }
            if (value == 6)
            {
                infos.Visibility = Visibility.Hidden;
                Open.Visibility = Visibility.Collapsed;
                before.Visibility = Visibility.Hidden;

                Title.Text = "Nombre de commandes \npar item";

                string query = "SELECT p.numPiece as 'numProduit', max(if(p.Siret != sir, " +
                    "p.nbcommande + nb, p.nbcommande)) as 'nombre commandes' FROM pieces as p, " +
                    "(SELECT numPiece as num, nbcommande as nb, Siret as sir from pieces) as a WHERE p.numPiece = num GROUP BY numPiece;";

                MySqlCommand command = MainWindow.maConnexion.CreateCommand();
                command.CommandText = query;
                MySqlDataReader reader = command.ExecuteReader();
                dataGrid1.Visibility = Visibility.Visible;
                DataTable dt = new DataTable();
                dt.Load(reader);
                reader.Close();
                command.Dispose();

                query = "SELECT numProduit, nbcommande as 'nombre commandes' from velo";
                command.CommandText = query;
                MySqlDataReader reader2 = command.ExecuteReader();
                DataTable dt2 = new DataTable();
                dt2.Load(reader2);
                dt.Merge(dt2, true, MissingSchemaAction.Ignore);
                dataGrid1.ItemsSource = dt.DefaultView;
                reader2.Close();


            }
            if (value == 7)
            {
                infos.Visibility = Visibility.Hidden;
                Title.Text = "La liste des membres pour chaque \nprogramme d’adhésion";

                radio.Visibility = Visibility.Visible;

            }
            if (value == 8)
            {
                Title.Text = "Les dates d’expiration des adhésions ";
                //after.Visibility = Visibility.Hidden;
                gamma();
                string drop = "DROP TABLE IF EXISTS velomax.feat";
                string create = " CREATE TABLE velomax.feat(Num int, Nom varchar(80), Prenom varchar(80), Type int, Date_Souscription DATE, Descriptions varchar(80), EndSouscriptions varchar(100), FinSouscriptions DATE)";
                try
                {
                    MySqlCommand command = new MySqlCommand(drop, MainWindow.maConnexion);
                    command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                try
                {
                    MySqlCommand command = new MySqlCommand(create, MainWindow.maConnexion);
                    command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                for (int i = 0; i <= clients.Count - 1; i++)
                {
                    string insertTable = "insert into velomax.feat(Num, Nom, Prenom, Type, Date_Souscription, Descriptions, EndSouscriptions, FinSouscriptions) values (@Num, @Nom, @Prenom, @Type, @Date_Souscription, @Descriptions, @EndSouscriptions, @FinSouscriptions)";
                    try
                    {
                        cmd = new MySqlCommand(insertTable, MainWindow.maConnexion);
                        cmd.Parameters.AddWithValue("@Num", clients[i].Num);
                        cmd.Parameters.AddWithValue("@Nom", clients[i].Nom);
                        cmd.Parameters.AddWithValue("@Prenom", clients[i].Prenom);
                        cmd.Parameters.AddWithValue("@Type", clients[i].Type);
                        cmd.Parameters.AddWithValue("@Date_Souscription", clients[i].Date_Souscription);
                        cmd.Parameters.AddWithValue("@Descriptions", clients[i].Descriptions);
                        cmd.Parameters.AddWithValue("@EndSouscriptions", clients[i].EndSouscriptions);
                        cmd.Parameters.AddWithValue("@FinSouscriptions", clients[i].FinSouscriptions);
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

                string query2 = "SELECT * From velomax.feat";

                MySqlCommand command2 = MainWindow.maConnexion.CreateCommand();
                command2.CommandText = query2;
                MySqlDataReader reader2 = command2.ExecuteReader();
                dataGrid1.Visibility = Visibility.Visible;
                DataTable dt = new DataTable();
                dt.Load(reader2);
                reader2.Close();
                dataGrid1.ItemsSource = dt.DefaultView;
            }
            if (value == 9)
            {
                infos.Visibility = Visibility.Hidden;
                Open.Visibility = Visibility.Collapsed;
                after.Visibility = Visibility.Hidden;
                Title.Text = "Moyenne des prix et quantités";

                string query = "SELECT avg(prixTotal) as 'moyenne prix commandes' from clients as c, commande , (SELECT sum(prix) as prixTotal, No_commande as numCommande FROM(SELECT p.prix, i.No_commande FROM pieces as p, itemcommande as i where p.numpiece = i.Ref_item UNION ALL SELECT v.prix, i.No_commande FROM velo as v, itemcommande as i where v.numProduit = i.Ref_item and i.Ref_item  ) as sub GROUP BY numCommande) as p WHERE numCommande = commande.No_commande and commande.No_client = c.No_client";


                MySqlCommand command = MainWindow.maConnexion.CreateCommand();
                command.CommandText = query;
                MySqlDataReader reader = command.ExecuteReader();
                dataGrid1.Visibility = Visibility.Visible;
                DataTable dt = new DataTable();
                dt.Load(reader);
                reader.Close();

                query = "SELECT AVG(quantite_item) as 'moyenne nombre de pieces' from commande natural join itemcommande";
                command.CommandText = query;
                reader = command.ExecuteReader();
                DataTable dt2 = new DataTable();
                dt2.Load(reader);
                dt.Merge(dt2);
                dataGrid1.ItemsSource = dt.DefaultView;
                reader.Close();
            }
        }
        DispatcherTimer timer = new DispatcherTimer();
        private void after_Click(object sender, RoutedEventArgs e)
        {
            ThicknessAnimation db = new ThicknessAnimation(new Thickness(0, 0, 0, 0), new Thickness(0, 0, 400, 0), new TimeSpan(0, 0, 0, 0, 600));
            db.EasingFunction = new ExponentialEase();

            DoubleAnimation doubleAnimation = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.25));
            doubleAnimation.EasingFunction = new ExponentialEase();

            Anim.BeginAnimation(Control.MarginProperty, db);
            Anim.BeginAnimation(Control.OpacityProperty, doubleAnimation);

            db = new ThicknessAnimation(new Thickness(40, 130, 0, 0), new Thickness(0, 130, 500, 0), new TimeSpan(0, 0, 0, 0, 100));

            radio.BeginAnimation(Control.MarginProperty, db);

            timer.Tick += new EventHandler(delegate (Object o, EventArgs a)
            {
                timer.Stop();
                MainWindow.Accueil.NavigationService.Navigate(new AffichageDemo(value + 1, true));
            });
            timer.Interval = TimeSpan.FromSeconds(0.3);
            timer.Start();
            before.IsEnabled = false;
            after.IsEnabled = false;
        }

        private void before_Click(object sender, RoutedEventArgs e)
        {
            ThicknessAnimation db = new ThicknessAnimation(new Thickness(0, 0, 0, 0), new Thickness(400, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 600));
            db.EasingFunction = new ExponentialEase();

            DoubleAnimation doubleAnimation = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.25));
            doubleAnimation.EasingFunction = new ExponentialEase();

            Anim.BeginAnimation(Control.MarginProperty, db);
            Anim.BeginAnimation(Control.OpacityProperty, doubleAnimation);

            timer.Tick += new EventHandler(delegate (Object o, EventArgs a)
            {
                timer.Stop();
                MainWindow.Accueil.NavigationService.Navigate(new AffichageDemo(value - 1, false));
            });
            timer.Interval = TimeSpan.FromSeconds(0.3);
            timer.Start();
            before.IsEnabled = false;
            after.IsEnabled = false;
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            if (value == 4) { System.Diagnostics.Process.Start(@"velo.xml"); }
            if (value == 5) { System.Diagnostics.Process.Start(@"FideliteExpiration.json"); }
        }
        #region RadioButtons
        private void Fidelio_checked(object sender, RoutedEventArgs e)
        {
            string query = "Select c.No_client,c.nom,c.prenom from clients c where c.No_client in (Select c.No_client from clients c where c.No_client in (Select c.No_client from clients c join fidelio f on c.No_client = f.No_client join Car_Fidelio cf on cf.No_programme = f.No_programme where cf.descriptions = 'Fidelio' and DATEDIFF(NOW(), f.date_inscription)<=365 group by cf.No_programme))";

            MySqlCommand command = MainWindow.maConnexion.CreateCommand();
            command.CommandText = query;

            MySqlDataReader reader = command.ExecuteReader();
            dataGrid1.Visibility = Visibility.Visible;
            DataTable dt = new DataTable();
            dt.Load(reader);
            dataGrid1.ItemsSource = dt.DefaultView;
            reader.Close();
            command.Dispose();
        }

        private void FidelioOr_Checked(object sender, RoutedEventArgs e)
        {

            string query = "Select c.No_client,c.nom,c.prenom from clients c where c.No_client in (Select c.No_client from clients c where c.No_client in (Select c.No_client from clients c join fidelio f on c.No_client = f.No_client join Car_Fidelio cf on cf.No_programme = f.No_programme where cf.descriptions = 'Fidelio Or' and DATEDIFF(NOW(), f.date_inscription)<=730 group by cf.No_programme))";
            MySqlCommand command = MainWindow.maConnexion.CreateCommand();
            command.CommandText = query;

            MySqlDataReader reader = command.ExecuteReader();
            dataGrid1.Visibility = Visibility.Visible;
            DataTable dt = new DataTable();
            dt.Load(reader);
            dataGrid1.ItemsSource = dt.DefaultView;
            reader.Close();
            command.Dispose();
        }

        private void FidelioPlatine_checked(object sender, RoutedEventArgs e)
        {
            string query = "Select c.No_client,c.nom,c.prenom from clients c where c.No_client in (Select c.No_client from clients c where c.No_client in (Select c.No_client from clients c join fidelio f on c.No_client = f.No_client join Car_Fidelio cf on cf.No_programme = f.No_programme where cf.descriptions = 'Fidelio Platine' and DATEDIFF(NOW(), f.date_inscription)<=730 group by cf.No_programme))";
            MySqlCommand command = MainWindow.maConnexion.CreateCommand();
            command.CommandText = query;
            MySqlDataReader reader = command.ExecuteReader();

            dataGrid1.Visibility = Visibility.Visible;
            DataTable dt = new DataTable();
            dt.Load(reader);
            dataGrid1.ItemsSource = dt.DefaultView;
            reader.Close();
            command.Dispose();
        }

        private void FidelioMax_checked(object sender, RoutedEventArgs e)
        {

            string query = "Select c.No_client,c.nom,c.prenom from clients c where c.No_client in (Select c.No_client from clients c where c.No_client in (Select c.No_client from clients c join fidelio f on c.No_client = f.No_client join Car_Fidelio cf on cf.No_programme = f.No_programme where cf.descriptions = 'Fidelio Max' and DATEDIFF(NOW(), f.date_inscription)<=1095 group by cf.No_programme))";
            MySqlCommand command = MainWindow.maConnexion.CreateCommand();
            command.CommandText = query;
            MySqlDataReader reader = command.ExecuteReader();
            dataGrid1.Visibility = Visibility.Visible;
            DataTable dt = new DataTable();
            dt.Load(reader);
            dataGrid1.ItemsSource = dt.DefaultView;
            reader.Close();
            command.Dispose();
        }

        #endregion
    }
}
