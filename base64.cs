// C# program to print Hello World!
using System;
using System.Data.SqlClient;
using System.Text;

// namespace declaration
namespace HelloWorldApp
{

    class Base : Db
    {

        internal static string codierung(string value)
        {
            //charArray hat den text
            char[] charArray = value.ToCharArray();

            Console.WriteLine("Text in ascii......   "+ value + "");

            //wie lang ist der eingegebene Text
            int zeichenlaenge = 0;
            string binaerString = "";
            //string bilden
            StringBuilder sb = new StringBuilder();

            //für die originale länge, um zugefügte enden wieder zu entfenen
            int zeichenlaenge_ursprung = 0;
           
            char zwischen;
            int AsciCode = 0;
            string AsciStr = "";

            byte[] bytes;

            for (int i = 0; i < charArray.Length; i++)
            {
                zwischen = charArray[i];
                AsciCode = (int)zwischen;

                //AsciStr = zwischen.ToString();
                //AsciStr = Convert.ToString(charArray[i]);

                //in bits wandeln

                // so geht es nicht, ist utf8, nichts anderes
                bytes = Encoding.ASCII.GetBytes(charArray);

                //so könnte es sein, geht aber nicht
                //UTF8Encoding utf8 = new UTF8Encoding();
                //bytes = utf8.GetBytes(charArray);

                //byte in string, sonst nicht ansehbar, zum weiter machen
                string d = Convert.ToString(bytes[i], 2);

                //binaerString = string.Format(d);

                //-------string der kette -muss- länge 8 haben--------
                //beim umfomen werden nullen vorne ignoriert, hier für länge wieder angefügt

                int laenge_string = d.Length;

                for (int j = 0; (8 - laenge_string) != 0; j++)
                {
                    if (laenge_string != 8)
                    {
                        sb.Append("0");
                        laenge_string++;
                        //Console.WriteLine("String: " + laenge_string + "");
                    }
                }
                Console.WriteLine("" + charArray[i] + "...." + AsciCode + "...." + d + "  ");
                sb.Append(d);

                zeichenlaenge++;
            }

            zeichenlaenge_ursprung = zeichenlaenge;

            Console.WriteLine("");
            Console.WriteLine("Zeichenlänge: " + zeichenlaenge + "");
            Console.WriteLine("Text in ascii: " + sb + "");


            //für die einteilung in 6-bit abschnitte muss die Zeichenlänge durch 3 teilbar sein, --Zeichenlänge*8 sorgt für volle abschnitte
            //wenn unzureichend wird jeweils 00000000 (8 nullen) hinten drangesetzt und bei codierung als = interpretiert

            if (zeichenlaenge % 3 != 0)
            {
                Console.WriteLine("Zeichenlänge musste angepasst werden");

                for (int i = 0; zeichenlaenge % 3 != 0; i++)
                {
                    zeichenlaenge++;
                    sb.Append("00000000");
                }

                Console.WriteLine("Zeichenlänge: " + zeichenlaenge + "");
                Console.WriteLine("Text in ascii: " + sb + "");
            }
            else
            {
                Console.WriteLine("Zeichenlänge musste nicht angepasst werden");
            }
            Console.WriteLine("");
            Console.WriteLine("Zeichenkette wird geteilt und zugewiesen");

            //kettenlänge -- Zeichenlänge*8
            //gliederung dann mit -- (Zeichenlänge*8)/6

            int lauf = (zeichenlaenge * 8) / 6;
            Console.WriteLine("Lauf gesamt: " + lauf + "");
            //um nicht zu überspeichern
            int lauf2 = lauf;

            //zum string teilen, Substring einstellen länge
            int startIndex = 0;
            int length = 6;


            string tr = sb.ToString();          //gesamt string übergeben von funktion
            string[] test = new string[100];    //darauf kommt kette gegliedert

            StringBuilder codiertStr = new StringBuilder();
            int nummerKonv = 0;
            string konv_zu = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

            /*  kompakt in for ist fehlerhaft, später dran basteln, jetzt unten getrennt da
             
            for (int i = 0; i < lauf; i++)
            {
                //---Kette teilen
                String substring = tr.Substring(startIndex, length);
                startIndex += 6;

                //---zuweisen, binär zu zeichen

                nummerKonv = Convert.ToInt32(substring, 2);

                //test[i] = Convert.ToString(Convert.ToChar(Convert.ToInt32(tr.Substring(startIndex, length), 2)));
                //test[i] = Convert.ToChar(nummerKonv);

                if (zeichenlaenge_ursprung != zeichenlaenge)
                {
                    //endung mit = ersetzen, wenn hinzugefügt
                    Console.WriteLine("zeichenlaenge_ursprung:" + zeichenlaenge_ursprung + "  zeichenlaenge: " + zeichenlaenge + "");
                    zeichenlaenge_ursprung++;
                    Console.WriteLine("Kette: " + lauf2 + "");
                    //test[lauf2] = "=";
                    lauf2--;
                    test[lauf2] = "=";
                    Console.WriteLine("mmmmmKette: " + test[zeichenlaenge_ursprung] + "zeichenlaenge_ursprung" + zeichenlaenge_ursprung + "");
                    i--;

                }

                else if (lauf2 >= 2)
                {
                    test[i] = konv_zu.Substring(nummerKonv, 1);
                    lauf2--;
                }

                codiertStr.Append(test[i]);

                //Console.WriteLine("lauf: " + lauf2 + "");
                Console.WriteLine("Kette:" + substring + "  ergibt die Zahl: " + nummerKonv + "  konvertiert zu " + test[i] + "");

            }
            */

                for (int i = 0; i < lauf; i++)
            {
                //---Kette teilen, in 6er glieder
                String substring = tr.Substring(startIndex, length);
                startIndex += 6;

                //---zuweisen, binär zu zeichen
                nummerKonv = Convert.ToInt32(substring, 2);

                test[i] = konv_zu.Substring(nummerKonv, 1);

                //codiertStr.Append(test[i]);

                Console.WriteLine("Kettenglied: " + substring + "  ergibt die Zahl: " + nummerKonv + "  konvertiert zu " + test[i] + "");
            }

            //endung mit = ersetzen, wenn hinzugefügt

            if (zeichenlaenge_ursprung != zeichenlaenge)
            {
                Console.WriteLine("");
                Console.WriteLine("Zeichenlaenge Ursprung: " + zeichenlaenge_ursprung + "  neue Zeichenlaenge: " + zeichenlaenge + "");
                Console.WriteLine("Somit muss hinten zur ergänzung das A mit = ersetzt werden");

                for (int i = 0; zeichenlaenge_ursprung != zeichenlaenge; i++)
                {
                    zeichenlaenge_ursprung++;
                    lauf2--;
                    test[lauf2] = "=";
                    Console.WriteLine("Zeichen: " + test[lauf2] + " überspielt Stelle: " + zeichenlaenge_ursprung + "");

                }
            }
            //alles wird in den String gepackt, außerhalb, weil es sonst zu lücken oder überschhreibung kommt (kann)
            for (int i = 0; i < lauf; i++)
            {
                codiertStr.Append(test[i]);
            }
            Console.WriteLine("");
            Console.WriteLine("Somit ergibt der Text codiert: " + codiertStr + "");
            Console.WriteLine("");
            
            string codiert = codiertStr.ToString();

            return codiert;
        }

        internal static string entcodierung(string codiert)
        {
            string konv_zu = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
            string codiert_vergleich = codiert;

            int[] codiert_dezi = new int[100];

            int lauf_decod = 0;

            //NullReferenceException zum zwischen darstellen
            StringBuilder codiert_dezi_zwischen = new StringBuilder();

            Console.WriteLine("Erhaltene Kette wieder zu zahlen");
            Console.WriteLine("Erhaltene Kette wieder zu zahlen"+ konv_zu.Length + "");

            int Null_stelle = 0;

            for (int i = 0; i < codiert_vergleich.Length; i++)
            {
                for(int j = 0; j < konv_zu.Length; j++)
                {
                    //Console.WriteLine("codiert: " + codiert.Substring(i, 1) + " konv_zu: " + konv_zu.Substring(j, 1) + "");
                    //Console.WriteLine("j: " + j + "");
                    if(codiert.Substring(i, 1)==null)
                    {
                        break;
                    }
                    //zeichen werden wieder zugeordnet, position ergibt zahl für bits
                    if (codiert.Substring(i, 1) == konv_zu.Substring(j, 1))
                    {
                    codiert_dezi[i] = j;
                    codiert_dezi_zwischen.Append(codiert_dezi[i]);
                    codiert_dezi_zwischen.Append(" ");

                    Console.WriteLine(""+ codiert.Substring(i, 1)+" ergibt als Zahl: " + Convert.ToString(j) + "");
                    //Console.WriteLine(" j: " + j + " i: " + i + "");
                    //Console.WriteLine("dezi: " + codiert_dezi[i] + "");
                    break;

                    }

                }
                //Console.WriteLine("i: " + i + "");

                //allein = ist im weiteren nicht nutzlich, es wird nur ein = hinten übernommen
                //loop macht nebenbei lenght-überschreitungsprobleme
                if (codiert.Substring(i, 1) == "=")
                {
                    lauf_decod = i;
                    Null_stelle++;
                    break;
                }
                lauf_decod = i;
            }
            Console.WriteLine("");
            Console.WriteLine("Folgend in Zahlen: " + codiert_dezi_zwischen + "");
            //Console.WriteLine("dezi länge: " + lauf_decod + "");
            Console.WriteLine("");
            Console.WriteLine("Jetzt werden die Zahlen binär und wenn zu kurz vorne eine 0 angefügt");

            StringBuilder codiertStr_bin = new StringBuilder();

           
            string d;
            //Anzahl bit-Zeichen
            int laenge_bin =0;
            string binary = "";
            int laenge_string = 0;

            for (int i = 0; i <= lauf_decod; i++)
            {

                binary = Convert.ToString(codiert_dezi[i], 2);

                //-------string der kette -muss- länge 8 haben--------

                laenge_string = binary.Length;
                laenge_bin += (laenge_string-1);

                Console.WriteLine("");
                Console.WriteLine("" + codiert_dezi[i] + "...." + binary + "");
                Console.WriteLine("binäre länge.... " + laenge_string + "");
                if (laenge_string != 6)
                {
                    int k = 6 - laenge_string;

                    //rest hinten abhacken, sollte verbessert werden
                    if (k < 0)
                    { 
                        break; 
                    }

                    Console.WriteLine("0 anfügen ...." + k + "");
                    
                    for (int j = 0; k != 0; j++)
                    {
                        codiertStr_bin.Append("0");
                        Console.WriteLine("0 angefügt: " + k + "");
                        k--;
                        laenge_bin++;
                    }
                }

                codiertStr_bin.Append(binary);
                laenge_bin++;

            }

            Console.WriteLine("Die Zeichenkette mit Anzahl Zeichen: " + laenge_bin + "");
            Console.WriteLine("Zeichenkette: " + codiertStr_bin + "");
            //zum test mit "Tür @Rahmen",  codiertStr_bin sollte normal 2 nullen mehr haben
            //Console.WriteLine("Zeichenkette: 0101010000111111011100100010000001000000010100100110000101101000011011010110010101101110");


            //laenge_bin -2 ---2 nicht benötigte nullen weggeschnitten
            //es sollen wieder 8er glieder werden
            int durchlauf = laenge_bin / 8 - Null_stelle;

            Console.WriteLine("");
            Console.WriteLine("Kette wird wieder in 8er glieder geteilt und zugewiesen");
            //Console.WriteLine("----"+durchlauf+" = "+laenge_bin+" /8 - "+Null_stelle+"");
            Console.WriteLine("Lauf: " + durchlauf + " Nullstelle: "+ Null_stelle + " Länge: "+ laenge_bin + "");

            //zum string teilen, Substring einstellen länge
            int startIndex = 0;
            int length = 8;


            string tr = codiertStr_bin.ToString();          //gesamt string übergeben von funktion
            string[] test = new string[100];    //darauf kommt ketten gegliedert

            StringBuilder decodiertStr = new StringBuilder();
            string charKonv = "";

            byte[] bytes;
            Char[] chars;

            for (int i = 0; i < durchlauf; i++)
            {
                //---Kette teilen, in 8er glieder
                test[i] = tr.Substring(startIndex, length);
                startIndex += 8;

                //---zuweisen, binär zu glieder

                //bytes = Encoding.ASCII.GetBytes(test[i]);
                //charKonv = Encoding.ASCII.GetString(bytes);

                charKonv = Convert.ToString(test[i]);

                int zu = Convert.ToInt32(test[i], 2);
                char nummerKonv = Convert.ToChar(zu);

                decodiertStr.Append(nummerKonv);

                //-----------

                //bytes = Encoding.UTF8.GetBytes(charKonv);

                //UTF8Encoding utf8 = new UTF8Encoding();
                //bytes = utf8.GetBytes(charKonv);

                //string nummerKonv_test = Encoding.UTF8.GetString(bytes);

                //string result = (new UTF8Encoding(false)).GetString(bytes);

                //Console.WriteLine("---Test---:" + result + "");

                Console.WriteLine("Lauf:" + i + "  Kettenglied: " + charKonv + " Zahl: " + zu + " konvertiert zu: " + nummerKonv + "");
            }

            Console.WriteLine("");
            Console.WriteLine("Dekodiert:  " + decodiertStr + "");
            string decodiert = decodiertStr.ToString();

            return decodiert;
        }
    }
}