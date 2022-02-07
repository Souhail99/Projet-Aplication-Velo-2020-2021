using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace bdd_projet
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Clients
    {
        private int num;
        private string nom;
        private string prenom;
        private string adresse;
        private string mail;
        private int telephone;
        private int type;
        private DateTime date_Souscription;

        private List<string> descriptions = new List<string>();
        private List<int> duree = new List<int>();
        private List<DateTime> lesdates = new List<DateTime>();
        private List<List<int>> liste = new List<List<int>>();

        private string desc;

        public Clients() { }
        public Clients(int num, string nom, string prenom, string adresse, string mail, int telephone)
        {
            this.num = num;
            this.nom = nom;
            this.prenom = prenom;
            this.adresse = adresse;
            this.mail = mail;
            this.telephone = telephone;
        }
        public Clients(int num, string nom, string prenom, string description)
        {
            this.num = num;
            this.nom = nom;
            this.prenom = prenom;
            this.desc = description;
        }
        public Clients(int num, string nom, string prenom, string adresse, string mail, int telephone, int type, DateTime date)
        {
            this.num = num;
            this.nom = nom;
            this.prenom = prenom;
            this.adresse = adresse;
            this.mail = mail;
            this.telephone = telephone;

            this.type = type;
            this.date_Souscription = date;
        }
        public Clients(int num, int type, DateTime date)
        {
            this.num = num;
            this.type = type;
            this.date_Souscription = date;
        }
        private string Descriptions1(int n)
        {
            string tid = "SELECT descriptions FROM Car_Fidelio";

            MySqlCommand cmd = new MySqlCommand(tid, MainWindow.maConnexion);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    descriptions.Add(reader.GetValue(i).ToString());
                }
            }
            reader.Close();
            string result = descriptions[n];
            return result;

        }
        public override string ToString()
        {
            return this.num + "\n" + this.nom + "\n" + this.prenom + "\n" + "Type de carte Fidelio :" + "\n" + Descriptions1(this.type) + "\n" + "Il reste moins de : " + "\n" + (DateTime.Now - this.date_Souscription);
        }


        [JsonProperty]
        public int Num
        {
            get { return this.num; }
            set { this.num = value; }
        }
        [JsonProperty]
        public string Nom
        {
            get { return this.nom; }
            set { this.nom = value; }
        }
        [JsonProperty]
        public string Prenom
        {
            get { return this.prenom; }
            set { this.prenom = value; }
        }

        public string Adresse
        {
            get { return this.adresse; }
            set { this.adresse = value; }
        }
        public string Mail
        {
            get { return this.mail; }
            set { this.mail = value; }
        }
        public int Telephone
        {
            get { return this.telephone; }
            set { this.telephone = value; }
        }

        [JsonProperty]
        public int Type
        {
            get { return this.type; }
            set { this.type = value; }
        }

        [JsonProperty]
        public DateTime Date_Souscription
        {
            get { return this.date_Souscription; }
            set { this.date_Souscription = value; }
        }
        [JsonProperty]
        public string Descriptions
        {
            get
            {

                string result = "";
                string tid = "SELECT descriptions FROM Car_Fidelio";

                MySqlCommand cmd = new MySqlCommand(tid, MainWindow.maConnexion);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        descriptions.Add(reader.GetValue(i).ToString());
                    }
                }
                reader.Close();
                if (this.type >= 1)
                {

                    result = descriptions[this.type - 1];
                }
                else
                {

                    result = "";
                }
                return result;
            }
        }
        [JsonProperty]
        public string EndSouscriptions
        {
            get
            {

                string result = "";
                string tid = "SELECT Car_Fidelio.duree FROM Car_Fidelio";
                int a = 0;
                MySqlCommand cmd = new MySqlCommand(tid, MainWindow.maConnexion);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    duree.Add(Convert.ToInt32(Convert.ToString(reader.GetString(0)[0])));
                }
                reader.Close();
                if (this.type >= 1)
                {
                    a = (this.date_Souscription - DateTime.Now).Days + duree[this.type - 1] * 365;
                    if (a < 0)
                    {
                        result = "Ce client n'est plus abonné à Fidelio depuis " + Convert.ToString(Math.Abs(a)) + " jours.";
                    }
                    else
                    {
                        if (a <= 60)
                        {
                            result = "Bientot fini, à renouveler, il reste : " + Convert.ToString(a) + " jours.";
                        }
                        else
                        {
                            result = "il reste " + Convert.ToString(a) + " jours.";
                        }

                    }
                }
                else
                {
                    result = "";
                }
                return result;
            }
        }

        [JsonProperty]
        public DateTime FinSouscriptions
        {
            get
            {
                DateTime dt = DateTime.Now;
                DateTime dt2 = new DateTime();
                string tid = "SELECT Car_Fidelio.duree FROM Car_Fidelio";
                int a = 0;
                MySqlCommand cmd = new MySqlCommand(tid, MainWindow.maConnexion);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    duree.Add(Convert.ToInt32(Convert.ToString(reader.GetString(0)[0])));
                }
                reader.Close();
                if (this.type >= 1)
                {
                    a = (this.date_Souscription - DateTime.Now).Days + duree[this.type - 1] * 365;
                    dt2 = dt.AddDays(a);
                }
                return dt2;
            }
        }


    }
}
