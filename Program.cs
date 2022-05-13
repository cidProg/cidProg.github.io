// C# program to print Hello World!
using System;
using System.Data.SqlClient;
using System.Text;

// namespace declaration
namespace HelloWorldApp
{

    class Db
    {
     

 // Main 
        static void Main(string[] args, Datenbank datenbank)
        {
            /*
            // base64
            string codierText = "Tür @Rahmenäö";

            string codiert = Base.codierung(codierText);

            string decodiertStr = Base.entcodierung(codiert);
            */





            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Kundenverwaltung; Trusted_Connection=True";    //User ID=Praktikant; Password=HelloWorld


            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            // Testdurchlauf

            string table = "Kundenverwaltung_kurz";         //auf andere noch anpassen
            int IDNumber = 6795;                           //test_PK

            string columnName = "Kundennummer";
            string columnName1_1 = "NameOderFirma";         //primär nicht überspielbar
            string columnName1_2 = "Telefon";
            string columnName1_3 = "Mail";
            string update = "p88811mm";                      

            string sucheInSpalte = "NameOderFirma";
            string sucheNach = "ete";               

            string bestellnummer_eintrag = "BF595125";  
            string produktnummer_eintrag = "125";
            int anzahlProdukt_eintrag = 25;
            int preis_eintrag = 1230;

            //Kundenverwaltung_kurz
            //Kundenverwaltung_genau
            //Bestellungen


            datenbank.DB_lesen(table, connection);
            Console.WriteLine("-----------------------------------");
            datenbank.DB_schreiben(IDNumber, connection);

            Console.WriteLine("-----------------------------------");
            datenbank.DB_lesen(table, connection);

            Console.WriteLine("-----------------------------------");
            datenbank.DB_update(table, columnName, columnName1_1, columnName1_2, columnName1_3, IDNumber, update, connection);

            Console.WriteLine("-----------------------------------");
            datenbank.DB_lesen(table, connection);

            Console.WriteLine("-----------------------------------");
            datenbank.DB_search(table, sucheInSpalte, sucheNach, connection);

            Console.WriteLine("-----------------------------------");
            int IDNumber_bestell = 1138;
            datenbank.DB_Bestellung(IDNumber, bestellnummer_eintrag, produktnummer_eintrag, anzahlProdukt_eintrag, preis_eintrag, connection);

            Console.WriteLine("-----------------------------------");
            datenbank.DB_lesen("Bestellungen", connection);

            Console.WriteLine("-----------------------------------");
            datenbank.DB_loeschen(table, columnName, IDNumber, connection);

            Console.WriteLine("-----------------------------------");
            datenbank.DB_lesen(table, connection);

            Console.WriteLine("-----------------------------------");
            datenbank.DB_lesen("Bestellungen", connection);   //erstellte zeilte sollte gelöscht sein

            connection.Close();
            //----------

            

            /*
            //erstmal maximal 32 zeichen
            string Text = "Dass ist 1 Text=0258";
            string passwort = "PassWort=0345-ivf";

            
            string hash= Aes_verschluesselung.sha256(passwort);



            string Text_verschluesslt = Aes_verschluesselung.verschluesselung(Text, hash);


            AesEntschluesselung.entschluesselung(Text_verschluesslt, passwort);
            */


            int  datetime = int.Parse(DateTime.Now.ToString("HH"));

            Console.WriteLine("datetime: " + datetime + "");



            Console.WriteLine("");
            Console.WriteLine("Bye World!");
        }
    }
}