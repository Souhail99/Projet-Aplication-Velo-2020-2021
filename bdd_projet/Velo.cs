using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace bdd_projet
{
    [Serializable]
    [XmlRoot(ElementName = "Velo")]
    public class VeloClass
    {
        private int numero_produit;
        private string nom;
        private string grandeur;
        private int prix;
        private string type;
        private DateTime date_introduction;
        private DateTime date_discontinuité;

        public VeloClass() { }
        public VeloClass(int num, string nom, string grandeur, int prix, string type, DateTime datein, DateTime datedi)
        {
            this.numero_produit = num;
            this.nom = nom;
            this.grandeur = grandeur;
            this.prix = prix;
            this.type = type;
            this.date_introduction = datein;
            this.date_discontinuité = datedi;
        }
        public VeloClass(int num, string nom, string grandeur, int prix, string type, DateTime datein)
        {
            this.numero_produit = num;
            this.nom = nom;
            this.grandeur = grandeur;
            this.prix = prix;
            this.type = type;
            this.date_introduction = datein;
        }
        public override string ToString()
        {
            return this.numero_produit + "\n" + this.nom + "\n" + this.grandeur + "\n" + this.prix + "\n" + this.type + "\n" + this.date_introduction + "\n" + this.date_discontinuité;
        }
        public int Numero_produit
        {
            get { return this.numero_produit; }
            set { this.numero_produit = value; }
        }
        public string Nom
        {
            get { return this.nom; }
            set { this.nom = value; }
        }
        public string Grandeur
        {
            get { return this.grandeur; }
            set { this.grandeur = value; }
        }
        public int Prix
        {
            get { return this.prix; }
            set { this.prix = value; }
        }
        public string Type
        {
            get { return this.type; }
            set { this.type = value; }
        }
        public DateTime Date_introduction
        {
            get { return this.date_introduction; }
            set { this.date_introduction = value; }
        }
        public DateTime Date_discontinuité
        {
            get { return this.date_discontinuité; }
            set { this.date_discontinuité = value; }
        }
    }
}
