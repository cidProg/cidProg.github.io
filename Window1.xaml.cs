using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
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
using System.Windows.Shapes;

namespace Verwaltung_grafisch
{
    /// <summary>
    /// Interaktionslogik für Window1.xaml
    /// </summary>
    /// 


    public partial class Window1 : Window
    {
        

        public Window1()
        {
            InitializeComponent();


            //preis anzeige
            txtNum.Text = _numValue.ToString();

            



        }

        //delegate void D(SqlConnection connection, Datenbank datenbank);

        private void Button_suchen_Click(object sender, RoutedEventArgs e )
        {

            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Kundenverwaltung; Trusted_Connection=True";    //User ID=Praktikant; Password=HelloWorld
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();


            var datenbank = new Datenbank();
            //string table = "Kundenverwaltung_kurz";
            string sucheInSpalte = "Kundennummer";
            string sucheNach = txt_suche_kunde.Text;
            string rowResult = "";
            string[] rowResult_split;

            //DB_search(string table, string sucheInSpalte, string sucheNach, SqlConnection connection)
            rowResult = datenbank.DB_search("Kundenverwaltung_kurz", sucheInSpalte, sucheNach, connection);

            rowResult_split = rowResult.Split('@');



            lbl_ausgabe_kunde_kurz1.Content = "Kundendaten";
            lbl_ausgabe_kunde_kurz2.Content = "Name:     " +rowResult_split[1];
            lbl_ausgabe_kunde_kurz3.Content = "Telefon:   " +rowResult_split[2];
            lbl_ausgabe_kunde_kurz4.Content = "Mail:        " +rowResult_split[3];


            rowResult = datenbank.DB_search("Kundenverwaltung_genau", sucheInSpalte, sucheNach, connection);
            rowResult_split = rowResult.Split('@');


            string Kundennummer1 = rowResult_split[0];
            string Ansprechpartner1 = rowResult_split[1];
            string vertrag1 = rowResult_split[2];
            string Treuepunkte1 = rowResult_split[3];
            string Eintritt1 = rowResult_split[4];
            string letzterKontakt1 = rowResult_split[6];

            if (vertrag1 == "False")
                vertrag1 = "Nein";
            else
                vertrag1 = "Ja";

            List<Kunde> items1 = new List<Kunde>();
            items1.Add(new Kunde()
            {
                Kundennummer = Kundennummer1,
                Ansprechpartner = Ansprechpartner1,
                Vertrag = vertrag1,
                Treuepunkte = Treuepunkte1,
                Eintritt = Eintritt1,
                letzterKontakt = letzterKontakt1
            });
            kunde_lang.ItemsSource = items1;





            rowResult = datenbank.DB_search("Bestellungen", sucheInSpalte, sucheNach, connection);
            rowResult_split = rowResult.Split('@');






            string bestell_bestellnummer="";
            string bestell_produktnummer="";
            string bestell_anzahl="";
            string bestell_preis="";

            List<User> items = new List<User>();
            int j = 0;

            rowResult_split = rowResult.Split('@');

            
            for(int i = 0; i < rowResult.Split('@').Length; )
            {
                
                bestell_bestellnummer = rowResult_split[1+j];
                bestell_produktnummer = rowResult_split[2 + j];
                bestell_anzahl = rowResult_split[3 + j];
                bestell_preis = rowResult_split[4 + j];
                i += rowResult_split[j].Length+1;
                j += 5;
                items.Add(new User() { Bestellnummer = bestell_bestellnummer, Produktnummer = bestell_produktnummer, Anzahl = bestell_anzahl, Preis = bestell_preis });
               
            }

            lvUsers.ItemsSource = items;



            connection.Close();
        }


        

        private void Button_bestellung_neu_Click(object sender, RoutedEventArgs e)
        {

            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Kundenverwaltung; Trusted_Connection=True";    //User ID=Praktikant; Password=HelloWorld
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var datenbank = new Datenbank();

            //DB_Bestellung(int IDNumber, string bestellnummer_eintrag, string produktnummer_eintrag, int anzahlProdukt_eintrag, int preis_eintrag, SqlConnection connection)

            int IDNumber= int.Parse(txt_suche_kunde.Text);
            //string bestellnummer_eintrag = "BF"+ txt_suche_kunde.Text+ DateTime.Now.ToString("YYMMDD");
            string bestellnummer_eintrag = txt_suche_kunde.Text+ txt_bestell_produktnummer.Text+"00"+ _numValue.ToString();
            string produktnummer_eintrag = txt_bestell_produktnummer.Text;
            int anzahlProdukt_eintrag = _numValue;
            int preis_eintrag = 0;






            preis_eintrag = preis_NachAnzahl;

            datenbank.DB_Bestellung(IDNumber, bestellnummer_eintrag, produktnummer_eintrag, anzahlProdukt_eintrag, preis_eintrag, connection);


            //Button_suchen_Click();
            //https:/ /newbedev.com/how-i-can-refresh-listview-in-wpf#:~:text=How%20I%20Can%20Refresh%20ListView%20in%20WPF%20If,approach%3A%20ICollectionView%20view%20%3D%20CollectionViewSource.GetDefaultView%20%28ItemsSource%29%3B%20view.Refresh%20%28%29%3B
            //wpf refresh listview



            connection.Close();
        }

   private void Button_bestellung_loeschen_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Kundenverwaltung; Trusted_Connection=True";    //User ID=Praktikant; Password=HelloWorld
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();



            var datenbank = new Datenbank();

            //DB_loeschen(string table, string columnName, int IDNumber, SqlConnection connection)


            string table = "Bestellungen";
            //Produktnummer
            int IDNumber = int.Parse(txt_suche_kunde.Text);
            string columnName = "Bestellnummer";
            string bestellnummer = txt_bestell_bestellnummer.Text;
            int eine_oder_alles = 0;

            datenbank.DB_loeschen(table, columnName, IDNumber, bestellnummer, eine_oder_alles, connection);















            connection.Close();

        }












        private void lvUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

     

















        private int preis_NachAnzahl = 0;
        int produkt_preis=2;

        public int Preis 
        {

            get { return preis_NachAnzahl; }
            set
            {
                preis_NachAnzahl = _numValue*produkt_preis;
            }
        }




        // für button anzanzahl bestell

        public int _numValue = 0;

        public int NumValue
        {
            get { return _numValue; }
            set
            {
                _numValue = value;
                txtNum.Text = value.ToString();
            }
        }


        private void cmdUp_Click(object sender, RoutedEventArgs e)
        {
            NumValue++;
        }

        private void cmdDown_Click(object sender, RoutedEventArgs e)
        {
            NumValue--;
        }

        private void txtNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtNum == null)
            {
                return;
            }

            if (!int.TryParse(txtNum.Text, out _numValue))
                txtNum.Text = _numValue.ToString();
        }

        private void txt_suche_kundennummer_TextChanged(object sender, TextChangedEventArgs e)
        {

        }





        //-----------------------------


    }
    public class User
    {
        public string Bestellnummer { get; set; }
        public string Produktnummer { get; set; }
        public string Anzahl { get; set; }
        public string Preis { get; set; }
    }
    public class Kunde
    {
        public string Kundennummer { get; set; }
        public string Ansprechpartner { get; set; }
        public string Vertrag { get; set; }
        public string Treuepunkte { get; set; }
        public string Eintritt { get; set; }
        public string letzterKontakt { get; set; }
    }

    /*
     
    <Grid Grid.Column="1">
                    <Rectangle x:Name="viereck_preis" Fill="White"  HorizontalAlignment="Left" Height="22" Width="85" Margin="-110,82,0,105" Opacity="0.2"></Rectangle>
                    <Rectangle  Fill="#e63a00" HorizontalAlignment="Left" Height="1" Width="122" Margin="-147,104,0,104"/>
                    <TextBlock x:Name="txt_preis" Text="{Binding Preis}" FontSize="15" VerticalAlignment="Top"  HorizontalAlignment="Left" Margin="-147,82,0,0" Foreground="Black"/>
                </Grid>



    public class CountModel : INotifyPropertyChanged //using System.ComponentModel;
    {
        private int preis_NachAnzahl = 0;
        int produkt_preis = 2;

        public int Counter
        {
            get { return preis_NachAnzahl; }
            set
            {
                preis_NachAnzahl = Window1.NumValue * produkt_preis;
                OnPropertyChanged(nameof(preis_NachAnzahl));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {

        }
    }
    */
}