// C# program to print Hello World!
using System;
using System.Data.SqlClient;
using System.Text;

namespace Verwaltung_grafisch
{

    public class Datenbank
    {
        public void DB_schreiben(int IDNumber, SqlConnection connection)
        {
            Console.WriteLine("es wird geschrieben....");

            Console.WriteLine("....nummer ist " + IDNumber + "");
            string sql = "Insert into Kundenverwaltung_kurz (Kundennummer, NameOderFirma, Telefon, Mail) values (" + IDNumber + ", '" + "Dieter" + "', '" + "0159524" + "', '" + "tdl2@web" + "')";
            SqlCommand command = new SqlCommand(sql, connection);

            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.InsertCommand = new SqlCommand(sql, connection); // Insert (erstellen/einfügen) Command erstellen


            adapter.InsertCommand.ExecuteNonQuery(); // Command ausführen
            command.Dispose();

            Console.WriteLine("....schreiben fertig");
        }
        //-----
        public void DB_lesen(string table, SqlConnection connection)
        {
            Console.WriteLine("" + table + " wird gelesen....");

            //string sql = "Select Kundennummer, NameOderFirma, Telefon, Mail from Kundenverwaltung_kurz";
            string sql = "Select * FROM " + table + "";
            SqlCommand command = new SqlCommand(sql, connection);


            SqlDataReader reader = command.ExecuteReader(); //command.ExecuteReader();


            if (table == "Kundenverwaltung_kurz")
            {
                while (reader.Read())
                {
                    string rowResult = string.Format("Kundennummer: {0}, Name oder Firma: {1}, Telefon: {2}, Mail: {3}",
                                    reader.GetValue(0), reader.GetValue(1), reader.GetValue(2), reader.GetValue(3));
                    Console.WriteLine(rowResult);
                }
            }
            else if (table == "Kundenverwaltung_genau")
            {
                while (reader.Read())
                {
                    string rowResult = string.Format("Kundennummer: {0}, Ansprechpartner: {1},Vertrag: {2},Treuepunkte: {3},Eintritt: {4},gekaufte Produkte: {5},letzterKaufOderKontakt: {6}",
                                    reader.GetValue(0), reader.GetValue(1), reader.GetValue(2), reader.GetValue(3), reader.GetValue(4), reader.GetValue(5), reader.GetValue(6), reader.GetValue(7));
                    Console.WriteLine(rowResult);
                }
            }
            //Bestellungen
            else
            {
                while (reader.Read())
                {
                    string rowResult = string.Format("Kundennummer: {0}, Bestellnummer: {1},Produktnummer: {2},Anzahl: {3},Preis: {4}",
                                    reader.GetValue(0), reader.GetValue(1), reader.GetValue(2), reader.GetValue(3), reader.GetValue(4));
                    Console.WriteLine(rowResult);
                }
            }

            reader.Close();
            command.Dispose();

            Console.WriteLine("...." + table + " lesen fertig");

        }
        //-------
        public void DB_loeschen(string table, string columnName, int IDNumber, string bestellnummer, int eine_oder_alles, SqlConnection connection)
        {
            Console.WriteLine("...." + table + " wird gelöscht....");

            //muss umgekehrt gelöscht werden-- sonst FK-fehler

            SqlCommand command1 = new SqlCommand("DELETE FROM Kundenverwaltung_kurz WHERE " + columnName + " = " + IDNumber + "", connection);
            SqlCommand command2 = new SqlCommand("DELETE FROM Kundenverwaltung_genau WHERE " + columnName + " = " + IDNumber + "", connection);
            SqlCommand command3 = new SqlCommand("DELETE FROM Bestellungen WHERE " + columnName + " = " + IDNumber + "", connection);


            if (table == "Kundenverwaltung_kurz")
            {
                Console.WriteLine("DELETE FROM Bestellungen WHERE " + columnName + " = " + IDNumber + "");
                command3.ExecuteNonQuery();
                command3.Dispose();
                Console.WriteLine("DELETE FROM Kundenverwaltung_genau WHERE " + columnName + " = " + IDNumber + "");
                command2.ExecuteNonQuery();
                command2.Dispose();
                Console.WriteLine("DELETE FROM Kundenverwaltung_kurz WHERE " + columnName + " = " + IDNumber + "");
                command1.ExecuteNonQuery();
                command1.Dispose();
            }
            else if (table == "Kundenverwaltung_genau")
            {
                Console.WriteLine("DELETE FROM Bestellungen WHERE " + columnName + " = " + IDNumber + "");
                command3.ExecuteNonQuery();
                command3.Dispose();
                Console.WriteLine("DELETE FROM Kundenverwaltung_genau WHERE " + columnName + " = " + IDNumber + "");
                command2.ExecuteNonQuery();
                command2.Dispose();
            }
            else
            {
                if (eine_oder_alles == 0)
                {
                    int bestellnummer_int = int.Parse(bestellnummer);
                    Console.WriteLine("DELETE FROM Bestellungen WHERE " + columnName + " = '%" + bestellnummer_int + "%'");

                }
                else
                {
                    columnName = "Kundennummer";
                    Console.WriteLine("DELETE FROM Bestellungen WHERE " + columnName + " = " + IDNumber + "");

                }
                 

            }

            command3.ExecuteNonQuery();
            command3.Dispose();

            /*
            SqlCommand command = new SqlCommand("DELETE FROM " + table + " WHERE " + columnName + " = " + IDNumber + "", connection);
            command.ExecuteNonQuery();
            command.Dispose();
            */

            Console.WriteLine("" + table + "....löschen fertig");
        }
        //--------
        public void DB_update(string table, string columnName, string columnName1_1, string columnName1_2, string columnName1_3, int IDNumber, string update, SqlConnection connection)
        {
            Console.WriteLine("es wird aktualisiert....");
            Console.WriteLine("UPDATE " + table + " SET  " + columnName1_1 + " = '" + update + "'," + columnName1_2 + " = '" + update + "'," + columnName1_3 + " = '" + update + "' WHERE " + columnName + " = " + IDNumber + "");

            SqlCommand command = new SqlCommand("UPDATE " + table + " SET  " + columnName1_1 + " = '" + update + "'," + columnName1_2 + " = '" + update + "'," + columnName1_3 + " = '" + update + "' WHERE " + columnName + " = " + IDNumber + " ", connection);
            /* SqlCommand command = new SqlCommand("UPDATE " + table + " SET  @colu11 = @up, @colu12 = @up, @colu13 = @up WHERE @colu = " + IDNumber + " ", connection);

             command.Parameters.AddWithValue("@table", table);
             command.Parameters.AddWithValue("@up", update);
             command.Parameters.AddWithValue("@colu11", columnName1_1);
             command.Parameters.AddWithValue("@colu12", columnName1_2);
             command.Parameters.AddWithValue("@colu13", columnName1_3);
             command.Parameters.AddWithValue("@colu", columnName);
            */

            command.ExecuteNonQuery();
            command.Dispose();

            Console.WriteLine("....aktualisieren fertig");

        }
        //-------
        public void DB_Bestellung(int IDNumber, string bestellnummer_eintrag, string produktnummer_eintrag, int anzahlProdukt_eintrag, int preis_eintrag, SqlConnection connection)
        {
            Console.WriteLine("es wird bestellt....");
            Console.WriteLine("INSERT into Bestellungen(Kundennummer, Bestellnummer, Produktnummer, Anzahl, Preis)  VALUES (" + IDNumber + ", " + bestellnummer_eintrag + ", " + produktnummer_eintrag + ", " + anzahlProdukt_eintrag + ", " + preis_eintrag + ")");


            SqlCommand command = new SqlCommand("INSERT into Bestellungen(Kundennummer, Bestellnummer, Produktnummer, Anzahl, Preis)  VALUES (" + IDNumber + ", '" + bestellnummer_eintrag + "', " + produktnummer_eintrag + ", " + anzahlProdukt_eintrag + ", " + preis_eintrag + ")", connection);

            command.Parameters.AddWithValue("@bestell", bestellnummer_eintrag);
            command.Parameters.AddWithValue("@proNum_ein", produktnummer_eintrag);
            command.Parameters.AddWithValue("@anzahl", anzahlProdukt_eintrag);


            command.ExecuteNonQuery();
            command.Dispose();

            Console.WriteLine("....bestellen fertig");
        }

        public string DB_search(string table, string sucheInSpalte, string sucheNach, SqlConnection connection)
        {
            Console.WriteLine("es wird gesucht....");
            Console.WriteLine("SELECT * FROM " + table + " WHERE " + sucheInSpalte + " LIKE " + sucheNach + "");

            string rowResult = "";

            //SqlCommand command = new SqlCommand("SELECT * FROM " + table + " WHERE @suSpalt LIKE @suNach", connection);

            SqlCommand command = new SqlCommand("SELECT * FROM " + table + " WHERE "+sucheInSpalte+" LIKE '%" + sucheNach + "%'", connection);

            command.Parameters.AddWithValue("@suSpalt", sucheInSpalte);
            command.Parameters.AddWithValue("@suNach", sucheNach);

            SqlDataReader reader = command.ExecuteReader();

            string test = "";

            if (table == "Kundenverwaltung_kurz")
            {
                while (reader.Read())
                {
                    /*
                     rowResult = string.Format("Kundennummer: {0}, Name oder Firma: {1}, Telefon: {2}, Mail: {3}",
                                    reader.GetValue(0), reader.GetValue(1), reader.GetValue(2), reader.GetValue(3));
                     */
                    rowResult = string.Format("{0}@{1}@{2}@{3}",
                                    reader.GetValue(0), reader.GetValue(1), reader.GetValue(2), reader.GetValue(3));
                    Console.WriteLine(rowResult);
                    Console.WriteLine("....suchung 1");
                }
            }
            else if (table == "Kundenverwaltung_genau")
            {
                while (reader.Read())
                {
                    //Format("Kundennummer: {0}, Ansprechpartner: {1},Vertrag: {2},Treuepunkte: {3},Eintritt: {4},gekaufte Produkte: {5},letzterKaufOderKontakt: {6}",
                    rowResult = string.Format("{0}@{1}@{2}@{3}@{4}@{5}@{6}",
                                    reader.GetValue(0), reader.GetValue(1), reader.GetValue(2), reader.GetValue(3), reader.GetValue(4), reader.GetValue(5), reader.GetValue(6));
                    Console.WriteLine(rowResult);
                    Console.WriteLine("....suchung 2");
                }
            }
            //Bestellungen


            else
            {
                while (reader.Read())
                {
                    //Format("Kundennummer: {0}, Bestellnummer: {1},Produktnummer: {2},Anzahl: {3},Preis: {4}",

                    if (test == "1")
                        {
                            rowResult += string.Format("@{0}@{1}@{2}@{3}@{4}",
                                                                reader.GetValue(0), reader.GetValue(1), reader.GetValue(2), reader.GetValue(3), reader.GetValue(4));
                            Console.WriteLine(rowResult);
                            Console.WriteLine("....suchung 3");
                        }
                    else
                    {
                        rowResult += string.Format("{0}@{1}@{2}@{3}@{4}",
                                                                reader.GetValue(0), reader.GetValue(1), reader.GetValue(2), reader.GetValue(3), reader.GetValue(4));
                        Console.WriteLine(rowResult);
                        Console.WriteLine("....suchung 3");

                        test = "1";
                    }

                }
            }

            reader.Close();
            command.Dispose();

            Console.WriteLine("....suchung fertig");

            return rowResult;
        }




    }
}