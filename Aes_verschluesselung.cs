using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;


namespace HelloWorldApp
{

    class Aes_verschluesselung : Db
    {
        public static string shift(string ergeb)
        {
            //binär erwartet

            StringBuilder zwischenAblage = new StringBuilder();
            string zwischenZeichen_links = "";
            string zwischenZeichen_rechts = "";
            string fuerXor = "";

            //standard zur rechnung mit xor
            //hex 1b = bin 00011011
            string xor_1b = "00011011";

            //shift 1 nach links ist die multiplikation mit 2, xor muss nur bei größeren byte gemacht werden ( unten das ==1)
            //erste holen bevor es weg ist
            zwischenZeichen_links = ergeb.Substring(0, 1);
            //hole die ersten 7, bei shift wird hinten eine 0 dran gesetzt
            zwischenZeichen_rechts = ergeb.Substring(1, 7);
            //jetzt hinten dran setzen
            zwischenAblage.Append(zwischenZeichen_rechts);

            // es wird nach links geshifted, demnach geht vorderer bit verloren und 0 wird rechts angefügt
            zwischenAblage.Append("0");

            fuerXor = zwischenAblage.ToString();
            zwischenAblage.Remove(0, 8);

            if (zwischenZeichen_links == "1")
            {
                fuerXor = xor(fuerXor, xor_1b);
                //Console.WriteLine("---- es wurde ein xor mit 1b gemacht");
                //Console.WriteLine("----       Byte: " + ergeb + "");
                //Console.WriteLine("---- nach shift: " + fuerXor + "");
            }

            return fuerXor;
        }

            public static string xor_multiMit_2(string ergeb, int mal3_GleichNull)
        {
            string fuerXor = "";
            string ergeb2 = "";


            fuerXor = shift(ergeb);
            ergeb2 = fuerXor;

            //xor mit 1b wenn größer als 2 hoch 7 -- also ganz linker bit == 1 ist
            //für multiplikation mit 3 wird xor 1b nicht genommen, also mal3_GleichNull==0
            if (mal3_GleichNull == 0)
            {
                ergeb2 = xor(fuerXor, ergeb);
                //Console.WriteLine("---- es wurde ein xor gemacht für multi 3");
                //Console.WriteLine("----       Byte: "+ ergeb + "");
                //Console.WriteLine("---- nach shift: " + fuerXor + "");
                //Console.WriteLine("----   nach xor: " + ergeb2 + "");
            }
            
            return ergeb2;
        }
        public static string sollBinaer(string sollBinaer)
        {

            string test = Convert.ToString(Convert.ToInt64(sollBinaer, 16), 2);

            int bytesLänge = test.Length;
            int diffBits = 8 - bytesLänge;
            StringBuilder Tescht = new StringBuilder();

            // fehlende nullen
            for (int k = 0; k < diffBits; k++)
            {
                Tescht.Append("0");
            }

            Tescht.Append(test);
            test = Tescht.ToString();
            Tescht.Remove(0, 8);

            /*
            string hexString = "44";
            hextest = "";

            string hs = hexString.Substring(0, 2);
            hextest = Convert.ToChar(Convert.ToUInt32(hs, 16)).ToString();

            Console.WriteLine(" hexString: " + hexString + " hs: " + hs + " hextest: " + hextest + " binär " + Aes.sollBinaer(hexString) + "");

            byte doh = byte.Parse(hexString);
            string test = Convert.ToString(doh, 2);

            //----

            bytes = Encoding.ASCII.GetBytes(sollBinaer);
            byteLänge = Convert.ToString(bytes[0], 2).Length;
            diffBit = 8 - byteLänge;

            */

            return test;
        }


        public static string xor(string xor_vergl1, string xor_vergl2)
        {
            //  binär erwartet

            //neue matrix auf diese das xor von beiden gespeichert wird
            string xor = "";
            StringBuilder xor_zwischen = new StringBuilder();

            
            for (int springer2 = 0; springer2 < 8; springer2++)
            {
                if (xor_vergl1.Substring(springer2, 1) == xor_vergl2.Substring(springer2, 1))
                {
                    xor_zwischen.Append("0");
                    //Console.WriteLine(" Zeile: " + i + " Spalte: " + j + " springer: " + springer2 + " ");
                    //Console.WriteLine(" matrix_bin: " + matrix_bin[j, i].Substring(springer2, 1) + " hash_matrix_bin: " + hash_matrix_bin[j, i].Substring(springer2, 1) + " ");
                    //Console.WriteLine("" + xor_eing.Substring(springer2, 1) + " == " + hash_matrix_bin[j, i].Substring(springer2, 1) + " --> " + xor_zwischen + "");
                }
                else
                {
                    xor_zwischen.Append("1");
                    //Console.WriteLine("" + xor_eing.Substring(springer2, 1) + " != " + hash_matrix_bin[j, i].Substring(springer2, 1) + " --> " + xor_zwischen + "");
                }
            }
            xor = xor_zwischen.ToString();
            xor_zwischen.Remove(0, 8);

            //Console.WriteLine(" xor  " + xor + "");


            return xor.ToString();
        }
        public static string sha256(string passwort)
        {
            // hash, zerstückeln-- passwort wird zu einer einheitlichen länge, geht nicht wieder zurück
            
            SHA256 sha256Hash = SHA256.Create();
            var hash = new StringBuilder();
            //ComputeHash berechnet den Hash-- input zu byte array ,256 bits, 32 zeichenlänge
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(passwort));
            foreach (byte theByte in bytes)
            {
                //x2-- 2-stellige ausgabe
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

        internal static string verschluesselung(string Text, string hash)
        {
            //https:/ /nvlpubs.nist.gov/nistpubs/fips/nist.fips.197.pdf
            //http:/ /codeplanet.eu/tutorials/cpp/51-advanced-encryption-standard.html
            //http:/ /www.gm.fh-koeln.de/~hk/lehre/ala/ws0506/Praktikum/Projekt/C_gelb/Dokumentation_AES.pdf
            //https:/ /itsecblog.de/rijndael-aes-sichere-block-und-schluesselgroessen/
            //https:/ /www.cryptool.org/de/cto/aes-step-by-step
            //256--> 4 Zeilen, 8 Spalten, 8 byte
            //00 00 00 00 00 00 00 00
            //00 00 00 00 00 00 00 00
            //00 00 00 00 00 00 00 00
            //00 00 00 00 00 00 00 00

            string[,] matrix = new string[100, 100];
            string[,] matrix_bin = new string[100, 100];
            string zwischen = "";
            Console.WriteLine("hash; " + hash + "");
            /*
            F00 F01 F02
            F10 F11 F12
            F20 F21 F22
            */
            // i ist spalte++
            int i = 0;
            // j ist zeile++
            int j = 0;

            int springer = 0;

            int freieBytes = Text.Length;
            int mod = freieBytes % 8;
            int diff = 32 - freieBytes;

            //Console.WriteLine("freieBytes:  " + freieBytes + "");
            //Console.WriteLine("mod:  " + mod + "");
            //Console.WriteLine("diff:  " + diff + "");

            // Text ist nicht immer 32 zeichen lang
            // mehr --> es muss ein neuer Durchgang mit neuen Block gemacht werden
            // HELLO........WORLD
            //  1. Block   H  O  .  .     2. Block   L  .  .  .
            //             E  .  .  W                D  .  .  .
            //             L  .  .  O                .  .  .  .
            //             L  .  .  R                .  .  .  .
            //
            // weniger --> am Ende des Blocks muss gefüllt werden --> padding 
            // benutze prinzip  mit ISO/IEC 7816-4 -- je nach anzahl an leere bytes wird benannt mit hex
            // 1. freier= 00 , 2 freie 08 00 , 3 freie 08 00 00 ........

            //matrix[zeile,spalte]
            byte[] bytes;
            int byteLänge = 0;
            int diffBit = 0;
            string[,] text_matrix = new string[100, 100];
            //string zwischenZuByte = "";
            StringBuilder zwischenZuByte = new StringBuilder();
            int m=0;

            for (i = 0; i < 8; i++)
            {
                for (j = 0; j < 4; j++)//ist hinten leer, grße holen
                {
                    if (springer < freieBytes)
                    {
                        zwischen = Text.Substring(springer, 1);
                        springer++;
                    }
                    else if (springer == freieBytes)
                    {   //zum padding, hex 00 ist ascii null
                        //zwischen = string.Format("0{0}",diff);
                        zwischen = string.Format("a");
                        //Console.WriteLine("teil matrix: " + zwischen + " wurde hinzugefügt");
                        springer++;
                        //Test für hex 0f
                        m++;
                    }
                    else
                    {
                        zwischen = string.Format("0");
                        //Console.WriteLine("teil matrix: " + zwischen + " wurde hinzugefügt");

                        springer++;
                    }
                    //hex speichern
                    //join ("wenn was dazwischen stehen soll", inhalte) - inhalt wird zusammen gepackt
                    matrix[j, i] = string.Join("", zwischen.Select(c => String.Format("{0:X2}", Convert.ToInt32(c))));

                    if (m != 0)
                    { 
                        matrix[j, i] = "0f";
                        m = 0;
                    }
                    
                    matrix_bin[j, i] = sollBinaer(matrix[j, i]);

                    //Console.WriteLine("Zeile: " + i + " Spalte: " + j + " springer: " + springer + " ");
                    //Console.WriteLine("teil matrix: " + zwischen + " = " + matrix[j, i] + " = " + matrix_bin[j, i] + "");

                    text_matrix[j, i] = zwischen;
                }
            }


            // AES ist in 4 Runden, welche wiederholt werden

            //--Substitution:
            //In diesem Schritt wird jedes Byte mit einer Substitutionsbox(S - Box) verschlüsselt.Die S-Box gibt dabei eine Regel an,
            //wie ein Byte eines Blockes durch einen anderen Wert zu ersetzen ist. Dadurch werden die Bytes vermischt.

            //--Shift Row:
            //Nach der Substitution werden nun die Zeilen in der Tabelle um eine bestimmte Anzahl an Spalten nach links verschoben.
            //Die überlaufenden Zellen werden rechts an die jeweilige Zeile wieder angehängt.

            //--Mix Column:
            //In diesem Schritt geht es nun darum, die Spalten zu vermischen. Hierfür wird jede Spalte mit einer bestimmten Matrix multipliziert.
            //Die Bytes werden dabei nicht als Zahlen gesehen, sondern als Polynome. Dies bedeutet, dass die Multiplikation über einem Galois Feld(2 ^ 8) durchgeführt wird.

            //--Key Addition:
            //Hier wird nun jeder Block mit dem aktuellen Rundenschlüssel XOR verknüpft.Diese XOR Verknüpfung läuft dabei bitweise ab.




            //-------Substitution

            //jetzt wird die S-Box gebraucht
            //ich habe sie erstmal schon gegeben genommen
            // mit ihr werden jetzt die hex werte der text-matrix ausgetausscht, indem sie neu zugewiesen werden, zbsp matri= 6c --> 50

            //{{Zeile1},{Zeile2}}
            //{{spalte1,spalte2,spalte3},{spalte1,spalte2,spalte3}}
            //{ { Zeile1_spalte1,Zeile1_spalte2,Zeile1_spalte3},
            //  { Zeile2_spalte1,Zeile2_spalte2,Zeile2_spalte3} }
            

            /*https:/ /cryptography.fandom.com/wiki/Rijndael_S-box
             
               | 0  1  2  3  4  5  6  7  8  9  a  b  c  d  e  f
            ---|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
            00 |63 7c 77 7b f2 6b 6f c5 30 01 67 2b fe d7 ab 76 
            10 |ca 82 c9 7d fa 59 47 f0 ad d4 a2 af 9c a4 72 c0 
            20 |b7 fd 93 26 36 3f f7 cc 34 a5 e5 f1 71 d8 31 15 
            30 |04 c7 23 c3 18 96 05 9a 07 12 80 e2 eb 27 b2 75 
            40 |09 83 2c 1a 1b 6e 5a a0 52 3b d6 b3 29 e3 2f 84 
            50 |53 d1 00 ed 20 fc b1 5b 6a cb be 39 4a 4c 58 cf 
            60 |d0 ef aa fb 43 4d 33 85 45 f9 02 7f 50 3c 9f a8 
            70 |51 a3 40 8f 92 9d 38 f5 bc b6 da 21 10 ff f3 d2 
            80 |cd 0c 13 ec 5f 97 44 17 c4 a7 7e 3d 64 5d 19 73 
            90 |60 81 4f dc 22 2a 90 88 46 ee b8 14 de 5e 0b db 
            a0 |e0 32 3a 0a 49 06 24 5c c2 d3 ac 62 91 95 e4 79 
            b0 |e7 c8 37 6d 8d d5 4e a9 6c 56 f4 ea 65 7a ae 08 
            c0 |ba 78 25 2e 1c a6 b4 c6 e8 dd 74 1f 4b bd 8b 8a 
            d0 |70 3e b5 66 48 03 f6 0e 61 35 57 b9 86 c1 1d 9e 
            e0 |e1 f8 98 11 69 d9 8e 94 9b 1e 87 e9 ce 55 28 df 
            f0 |8c a1 89 0d bf e6 42 68 41 99 2d 0f b0 54 bb 16
        
            */


            // hier kommt die s-Box
            string Rijndael_ForwardS_box = "637c777bf26b6fc53001672bfed7ab76ca82c97dfa5947f0add4a2af9ca472c0b7fd9326363ff7cc34a5e5f171d8311504c723c31896059a071280e2eb27b27509832c1a1b6e5aa0523bd6b329e32f8453d100ed20fcb15b6acbbe394a4c58cfd0efaafb434d338545f9027f503c9fa851a3408f929d38f5bcb6da2110fff3d2cd0c13ec5f974417c4a77e3d645d197360814fdc222a908846eeb814de5e0bdbe0323a0a4906245cc2d3ac629195e479e7c8376d8dd54ea96c56f4ea657aae08ba78252e1ca6b4c6e8dd741f4bbd8b8a703eb5664803f60e613557b986c11d9ee1f8981169d98e949b1e87e9ce5528df8ca1890dbfe6426841992d0fb054bb16";

            string[,] S_Box = new string[100, 100];
            springer = 0;

            for (i = 0; i < 16; i++)
            {
                for (j = 0; j < 16; j++)
                {
                    S_Box[i, j] = Rijndael_ForwardS_box.Substring(springer, 2);
                    springer += 2;
                }
            }
            /*
            for (i = 0; i < 16; i++)
            {
                Console.WriteLine("" + S_Box[i, 0] + "  " + S_Box[i, 1] + "  " + S_Box[i, 2] + "  " + S_Box[i, 3] + "  " + S_Box[i, 4] + "  " + S_Box[i, 5] + "  " + S_Box[i, 6] + "  " + S_Box[i, 7] + "  " + S_Box[i, 8] + "  " + S_Box[i, 9] + "  " + S_Box[i, 10] + "  " + S_Box[i, 11] + "  " + S_Box[i, 12] + "  " + S_Box[i, 13] + "  " + S_Box[i, 14] + "  " + S_Box[i, 15] + "");
            }
            */



            // hier fangen die runden an, 256 -- 14 runden
            int aes_runde = 0;
            string[,] KeyAddition = new string[100, 100];


            for (aes_runde = 0; aes_runde < 14; aes_runde++)
            {

                if (aes_runde > 0)
                {
                    Console.WriteLine("--------------------------------------------------------------------");
                    Console.WriteLine("                  Hier fängt eine neue verschlüssel Runde an");
                    Console.WriteLine("--------------------------------------------------------------------");

                    for (i = 0; i < 8; i++)
                    {
                        for (j = 0; j < 4; j++)
                        {

                            matrix[j, i] = KeyAddition[j, i];
                            //Console.WriteLine(" matrix: " + matrix[j, i] + " KeyAddition: " + KeyAddition[j, i] + "");
                        }
                    }
                }



                //-------Substitution

                //zur zuordnung in die Tabelle, dabei werden die beiden stellen zu koordinaten
                string hex_vergleich = "0123456789abcdef";

                string[,] matrix_Substitution = new string[100, 100];
                int r = 0;

                int zeile = 0;
                int spalte = 0;

                for (i = 0; i < 8; i++)
                {
                    for (j = 0; j < 4; j++)
                    {
                        for (int k = 0; k < 16; k++)
                        {
                            //Console.WriteLine("Vergleich von:" + matrix[j, i] + " hex_vergleich: " + hex_vergleich + "");
                            //Console.WriteLine(" Zeile: " + i + " Spalte: " + j + " k: " + k + "");

                            if (matrix[j, i].Substring(0, 1).Equals(hex_vergleich.Substring(k, 1), StringComparison.OrdinalIgnoreCase))
                            {
                                //Console.WriteLine("Vergleich matrix teil 1: " + matrix[j, i].Substring(0, 1) + " hex_vergleich: " + hex_vergleich.Substring(k, 1) + " ");
                                zeile = k;
                            }

                            if (matrix[j, i].Substring(1, 1).Equals(hex_vergleich.Substring(k, 1), StringComparison.OrdinalIgnoreCase))
                            {
                                //Console.WriteLine("Vergleich matrix teil 2: " + matrix[j, i].Substring(1, 1) + " hex_vergleich: " + hex_vergleich.Substring(k, 1) + " ");
                                spalte = k;
                            }
                        }
                        matrix_Substitution[j, i] = S_Box[zeile, spalte];
                        //Console.WriteLine(" Zeile: " + zeile + " Spalte: " + spalte + " matrix_Substitution: " + matrix_Substitution[j, i] + "");
                    }
                }


                //-------Shift Row

                string[,] hash_matrix = new string[100, 100];
                string[,] hash_matrix_bin = new string[100, 100];

                string[,] matrix_ShiftRow = new string[100, 100];
                string[,] matrix_ShiftRow_hex = new string[100, 100];
                string[,] matrix_ShiftRow_bin = new string[100, 100];

                string zwischenZeichen = "";
                string zwischenSpeicher = "";
                string zwischenZeichen_links = "";

                StringBuilder zwischenAblage = new StringBuilder();
                StringBuilder zwischenSpeicherung = new StringBuilder();


                for (j = 0; j < 4; j++)
                {
                    //holt sich zeilenweise
                    for (i = 0; i < 8; i++)
                    {
                        zwischenAblage.Append(matrix_Substitution[j, i]);
                        //Console.WriteLine(" Zeile: " + i + " Spalte: " + j + "");
                        //Console.WriteLine("ShiftRow: " + matrix_ShiftRow[j, i] + "");
                    }

                    zwischenSpeicher = zwischenAblage.ToString();
                    zwischenAblage.Remove(0, 16);

                    if (j == 0)
                    {
                        zwischenSpeicherung.Append(zwischenSpeicher);
                        //Console.WriteLine("ShiftRow: " + zwischenSpeicherung + "");
                    }
                    else if (j == 1)
                    {
                        zwischenZeichen_links = zwischenSpeicher.Substring(0, 2);

                        zwischenSpeicherung.Append(zwischenSpeicher.Substring(2, 14));
                        zwischenSpeicherung.Append(zwischenZeichen_links);
                    }
                    else if (j == 2)
                    {
                        zwischenZeichen_links = zwischenSpeicher.Substring(0, 4);

                        zwischenSpeicherung.Append(zwischenSpeicher.Substring(4, 12));
                        zwischenSpeicherung.Append(zwischenZeichen_links);
                    }
                    else if (j == 3)
                    {
                        zwischenZeichen_links = zwischenSpeicher.Substring(0, 6);

                        zwischenSpeicherung.Append(zwischenSpeicher.Substring(6, 10));
                        zwischenSpeicherung.Append(zwischenZeichen_links);
                    }
                    else
                        Console.WriteLine(" --fehler--");

                    zwischenZeichen = zwischenSpeicherung.ToString();

                    for (i = 0; i < 8; i++)
                    {
                        matrix_ShiftRow[j, i] = zwischenZeichen.Substring(i * 2, 2);
                        Console.WriteLine(matrix_ShiftRow[j, i]);

                        matrix_ShiftRow_bin[j, i] = sollBinaer(matrix_ShiftRow[j, i]);

                        //Console.WriteLine("matrix_ShiftRow[j, i] " + matrix_ShiftRow[j, i] + " matrix_ShiftRow_bin[j, i]: " + matrix_ShiftRow_bin[j, i] + "");
                    }
                    zwischenSpeicherung.Remove(0, 16);
                }

                /*

                hatte ich falsch, ist für bitweises shiften

                for (i = 0; i < 8; i++)
                {
                    for (j = 0; j < 4; j++)
                    {
                        //Console.WriteLine(matrix_Substitution[j, i]);

                        bytes = Encoding.ASCII.GetBytes(matrix_Substitution[j, i]);
                        zwischenSpeicher = Convert.ToString(bytes[0], 2);
                        //Console.WriteLine(zwischenSpeicher);


                        byteLänge = Convert.ToString(bytes[0], 2).Length;
                        diffBit = 8 - byteLänge;

                        // fehlende nullen
                        for (int k = 0; k < diffBit; k++)
                        {
                            hash_matrix_bin[i, j + k] = string.Join("0", hash_matrix_bin[j, i]);
                            zwischenAblage.Append("0");
                        }

                        byteLänge += diffBit;
                        zwischenAblage.Append(Convert.ToString(bytes[0], 2));

                        zwischenSpeicher = zwischenAblage.ToString();

                        zwischenAblage.Remove(0, 8);


                        //erste holen bevor es weg ist
                        zwischenZeichen_links = zwischenSpeicher.Substring(0, i);
                        //hole die ersten 7, bei shift wird hinten eine 0 dran gesetzt
                        zwischenZeichen_rechts = zwischenSpeicher.Substring(i, 8-i);
                        //jetzt hinten dran setzen
                        zwischenAblage.Append(zwischenZeichen_rechts);
                        zwischenAblage.Append(zwischenZeichen_links);

                        matrix_ShiftRow[j, i] = zwischenAblage.ToString();
                        zwischenAblage.Remove(0, 8);


                        Console.WriteLine(" Zeile: " + i + " Spalte: " + j + " nach links um: "+i+"");
                        Console.WriteLine(" anfang: " + zwischenSpeicher + " ShiftRow: " + matrix_ShiftRow[j, i] + "");


                        //für hex

                        string hex_lang = Convert.ToInt32(matrix_ShiftRow[j, i], 2).ToString("X");

                        //wenn hex einstellig ist
                        if (hex_lang.Length < 2)
                        {
                            matrix_ShiftRow_hex[j, i] = string.Format("0{0}", hex_lang);
                        }
                        else
                            matrix_ShiftRow_hex[j, i] = hex_lang;
                    }
                }
                */



                //--------Mix Column
                //matrix ist noch binär
                //  https:/ /crypto.stackexchange.com/questions/2402/how-to-solve-mixcolumns
                //https:/ /it352.files.wordpress.com/2012/02/20120111_mix_columns.pdf

                // XOR:  00=0 01=1 10=1 11=0
                //string xor_vergl1 = "11110000";
                //string xor_vergl2 = "10001110";

                //string ergeb = xor(xor_vergl1, xor_vergl2);
                //sollBinaer(matrix_ShiftRow[j, i]);

                //Console.WriteLine(" mmmmmmmmmm: " + ergeb + "");


                string ergeb = "";
                string ergeb_1 = "";
                string ergeb_2 = "";
                string ergeb_3 = "";
                string ergeb_4 = "";
                string ergeg_lang = "";

                string[,] MixColumn = new string[100, 100];
                string[,] MixColumn_bin = new string[100, 100];

                /*
                int[,] Galois_Feld =  { { 2, 3, 1, 1 },
                                        { 1, 2, 3, 1 },
                                        { 1, 1, 2, 3 },
                                        { 3, 1, 1, 2 } };
                */

                //testwerte für 1. spalte, für invers wieder anders herum

                //matrix_ShiftRow_bin[0, 0] = "11011011"; //db--> 8e
                //matrix_ShiftRow_bin[1, 0] = "00010011"; //13--> 4d
                //matrix_ShiftRow_bin[2, 0] = "01010011"; //53--> a1
                //matrix_ShiftRow_bin[3, 0] = "01000101"; //45--> bc

                //matrix_ShiftRow_bin[0, 0] = "11110010"; //f2--> 9f
                //matrix_ShiftRow_bin[1, 0] = "00001010"; //0a--> dc
                //matrix_ShiftRow_bin[2, 0] = "00100010"; //22--> 58
                //matrix_ShiftRow_bin[3, 0] = "01011100"; //5c--> 9d

                //matrix_ShiftRow_bin[0, 0] = "11010100"; //d4--> d5
                //matrix_ShiftRow_bin[1, 0] = "11010100"; //d4--> d5
                //matrix_ShiftRow_bin[2, 0] = "11010100"; //d4--> d7
                //matrix_ShiftRow_bin[3, 0] = "11010101"; //d5--> d6


                for (i = 0; i < 8; i++)
                {
                    for (j = 0; j < 4; j++)
                    {
                        //Console.WriteLine("Anfang : " + matrix_ShiftRow_bin[j, i] + " hex: " + matrix_ShiftRow[j, i] + "");

                        //2 3 1 1
                        if (j == 0)
                        {
                            //Console.WriteLine("--1");
                            ergeb_1 = xor_multiMit_2(matrix_ShiftRow_bin[0, i], 1);
                            ergeb_2 = xor_multiMit_2(matrix_ShiftRow_bin[1, i], 0);
                            ergeb_3 = matrix_ShiftRow_bin[2, i];
                            ergeb_4 = matrix_ShiftRow_bin[3, i];

                            //Console.WriteLine("ergeb_1: " + ergeb_1 + "");
                            //Console.WriteLine("ergeb_2: " + ergeb_2 + "");
                            //Console.WriteLine("ergeb_3: " + ergeb_3 + "");
                            //Console.WriteLine("ergeb_4: " + ergeb_4 + "");

                            /* ist fehlerhaft

                            ergeb = Aes.xor(ergeb_1, ergeb_2);
                            ergeb = Aes.xor(ergeb, ergeb_3);
                            ergeb = Aes.xor(ergeb, ergeb_4);
                            */
                            ergeb = AesEntschluesselung.byte_xor_reihe(ergeb_1, ergeb_2, ergeb_3, ergeb_4);

                        }
                        //1 2 3 1
                        else if (j == 1)
                        {
                            //Console.WriteLine("--2");
                            ergeb_1 = matrix_ShiftRow_bin[0, i];
                            ergeb_2 = xor_multiMit_2(matrix_ShiftRow_bin[1, i], 1);
                            ergeb_3 = xor_multiMit_2(matrix_ShiftRow_bin[2, i], 0);
                            ergeb_4 = matrix_ShiftRow_bin[3, i];

                            ergeb = AesEntschluesselung.byte_xor_reihe(ergeb_1, ergeb_2, ergeb_3, ergeb_4);
                        }
                        //1 1 2 3
                        else if (j == 2)
                        {
                            //Console.WriteLine("--3");
                            ergeb_1 = matrix_ShiftRow_bin[0, i];
                            ergeb_2 = matrix_ShiftRow_bin[1, i];
                            ergeb_3 = xor_multiMit_2(matrix_ShiftRow_bin[2, i], 1);
                            ergeb_4 = xor_multiMit_2(matrix_ShiftRow_bin[3, i], 0);

                            ergeb = AesEntschluesselung.byte_xor_reihe(ergeb_1, ergeb_2, ergeb_3, ergeb_4);
                        }
                        //3 1 1 2
                        else if (j == 3)
                        {
                            //Console.WriteLine("--4");
                            ergeb_1 = xor_multiMit_2(matrix_ShiftRow_bin[0, i], 0);
                            ergeb_2 = matrix_ShiftRow_bin[1, i];
                            ergeb_3 = matrix_ShiftRow_bin[2, i];
                            ergeb_4 = xor_multiMit_2(matrix_ShiftRow_bin[3, i], 1);

                            ergeb = AesEntschluesselung.byte_xor_reihe(ergeb_1, ergeb_2, ergeb_3, ergeb_4);
                        }
                        else
                            Console.WriteLine("--fehler--");

                        //MixColumn[j, i] = string.Join("", ergeb.Select(c => String.Format("{0:X2}", Convert.ToInt32(c))));

                        ergeg_lang = Convert.ToInt32(ergeb, 2).ToString("X");

                        //wenn hex einstellig ist
                        if (ergeg_lang.Length < 2)
                        {
                            MixColumn[j, i] = string.Format("0{0}", ergeg_lang);
                        }
                        else
                            MixColumn[j, i] = ergeg_lang;

                        MixColumn_bin[j, i] = ergeb;

                        //Console.WriteLine("Spalte: " + i + " Zeile: " + j + "  " + matrix_ShiftRow[j, i] + " wurde zu " + ergeb + " somit zu hex: " + MixColumn[j, i] + "");
                    }
                }


                //----------Key Addition


                int springer2 = 0;

                // der hash wird zu byte
                for (i = 0; i < 8; i++)
                {
                    for (j = 0; j < 4; j++)
                    {
                        hash_matrix[j, i] = hash.Substring(springer2, 2);
                        springer2 += 2;

                        hash_matrix_bin[j, i] = sollBinaer(hash_matrix[j, i]);

                        //Console.WriteLine("hash_matrix: " + hash_matrix[j, i] + " mhash_matrix_bin: " + hash_matrix_bin[j, i] + "");
                    }
                }

                //jetzt wird xor zwischen mixcolumn und dem hash gemacht

                string[,] KeyAddition_bin = new string[100, 100];

                for (i = 0; i < 8; i++)
                {
                    for (j = 0; j < 4; j++)
                    {
                        KeyAddition_bin[j, i] = xor(MixColumn_bin[j, i], hash_matrix_bin[j, i]);

                        //Console.WriteLine("matrix_ShiftRow: " + MixColumn_bin[j, i] + " hash_matrix_bin: " + hash_matrix_bin[j, i] + " KeyAddition:  " + KeyAddition_bin[j, i]+"");

                        ergeg_lang = Convert.ToInt32(KeyAddition_bin[j, i], 2).ToString("X");

                        //wenn hex einstellig ist
                        if (ergeg_lang.Length < 2)
                        {
                            KeyAddition[j, i] = string.Format("0{0}", ergeg_lang);
                        }
                        else
                            KeyAddition[j, i] = ergeg_lang;
                    }
                }

                // ausgabe der matrizen, verbesserbar

                Console.WriteLine("text_matrix");

                for (i = 0; i < 4; i++)
                {
                    Console.WriteLine("" + text_matrix[i, 0] + "  " + text_matrix[i, 1] + "  " + text_matrix[i, 2] + "  " + text_matrix[i, 3] + "  " + text_matrix[i, 4] + "  " + text_matrix[i, 5] + "  " + text_matrix[i, 6] + "  " + text_matrix[i, 7] + "");
                }

                Console.WriteLine("hex matrix");

                for (i = 0; i < 4; i++)
                {
                    Console.WriteLine("" + matrix[i, 0] + "  " + matrix[i, 1] + "  " + matrix[i, 2] + "  " + matrix[i, 3] + "  " + matrix[i, 4] + "  " + matrix[i, 5] + "  " + matrix[i, 6] + "  " + matrix[i, 7] + "");
                }

                Console.WriteLine("matrix_Substitution");

                for (i = 0; i < 4; i++)
                {
                    Console.WriteLine("" + matrix_Substitution[i, 0] + "  " + matrix_Substitution[i, 1] + "  " + matrix_Substitution[i, 2] + "  " + matrix_Substitution[i, 3] + "  " + matrix_Substitution[i, 4] + "  " + matrix_Substitution[i, 5] + "  " + matrix_Substitution[i, 6] + "  " + matrix_Substitution[i, 7] + "");
                }

                Console.WriteLine("matrix_ShiftRow");

                for (i = 0; i < 4; i++)
                {
                    Console.WriteLine("" + matrix_ShiftRow[i, 0] + "  " + matrix_ShiftRow[i, 1] + "  " + matrix_ShiftRow[i, 2] + "  " + matrix_ShiftRow[i, 3] + "  " + matrix_ShiftRow[i, 4] + "  " + matrix_ShiftRow[i, 5] + "  " + matrix_ShiftRow[i, 6] + "  " + matrix_ShiftRow[i, 7] + "");
                }

                Console.WriteLine("MixColumn");

                for (i = 0; i < 4; i++)
                {
                    Console.WriteLine("" + MixColumn[i, 0] + "  " + MixColumn[i, 1] + "  " + MixColumn[i, 2] + "  " + MixColumn[i, 3] + "  " + MixColumn[i, 4] + "  " + MixColumn[i, 5] + "  " + MixColumn[i, 6] + "  " + MixColumn[i, 7] + "");
                }

                Console.WriteLine("KeyAddition");

                for (i = 0; i < 4; i++)
                {
                    Console.WriteLine("" + KeyAddition[i, 0] + "  " + KeyAddition[i, 1] + "  " + KeyAddition[i, 2] + "  " + KeyAddition[i, 3] + "  " + KeyAddition[i, 4] + "  " + KeyAddition[i, 5] + "  " + KeyAddition[i, 6] + "  " + KeyAddition[i, 7] + "");
                }

            }



            StringBuilder verschl_ergeb = new StringBuilder();

            for (i = 0; i < 8; i++)
            {
                for (j = 0; j < 4; j++)
                {
                    verschl_ergeb.Append(KeyAddition[j, i]);
                }
            }
            string verschl_ergebnis = verschl_ergeb.ToString();

            //text wird base64 codiert
            verschl_ergebnis = Base.codierung(verschl_ergebnis);


            return verschl_ergebnis.ToString();
        }
    }
}