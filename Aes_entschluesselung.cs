// C# program to print Hello World!
using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

// namespace declaration
namespace HelloWorldApp
{

    class AesEntschluesselung : Db
    {

        public static string byte_multi_neu(string x, string multipli)
        {
            //string a = "09";
            //string b = "0d";

            //https:/ /crypto.stackexchange.com/questions/2569/how-does-one-implement-the-inverse-of-aes-mixcolumns

            string ergeb = "";

            // "09"  -->  9, 00001001
            // "0b"  --> 11, 00001011
            // "0d"  --> 13, 00001101
            // "0e"  --> 14, 00001110

            x = Convert.ToInt32(x, 16).ToString();
            x = byteNullen(Convert.ToString(byte.Parse(x), 2));

            //x×9=(((x×2)×2)×2)+x
            if (multipli== "09")
            {
                //Console.WriteLine("--mult1");
                ergeb = Aes_verschluesselung.xor(Aes_verschluesselung.shift(Aes_verschluesselung.shift(Aes_verschluesselung.shift(x))),x);
            }

            //x×11=((((x×2)×2)+x)×2)+x
            if (multipli == "0b")
            {
                //Console.WriteLine("--mult2");
                ergeb = Aes_verschluesselung.xor(Aes_verschluesselung.shift(Aes_verschluesselung.xor(Aes_verschluesselung.shift(Aes_verschluesselung.shift(x)), x)), x);
            }

            //x×13=((((x×2)+x)×2)×2)+x
            if (multipli == "0d")
            {
                //Console.WriteLine("--mult3");
                ergeb = Aes_verschluesselung.xor(Aes_verschluesselung.shift(Aes_verschluesselung.shift(Aes_verschluesselung.xor(Aes_verschluesselung.shift(x),x))), x);
            }

            //x×14=((((x×2)+x)×2)+x)×2
            if (multipli == "0e")
            {
                //Console.WriteLine("--mult4");
                ergeb = Aes_verschluesselung.shift(Aes_verschluesselung.xor(Aes_verschluesselung.shift(Aes_verschluesselung.xor(Aes_verschluesselung.shift(x), x)), x));
            }
            //Console.WriteLine("ergeb:  " + ergeb + "");

            return ergeb;
        }

        public static string byteNullen(string einByte)
        {
            int bytesLänge = einByte.Length;
            int diffBits = 8 - bytesLänge;
            StringBuilder Tescht = new StringBuilder();

            // fehlende nullen
            for (int k = 0; k < diffBits; k++)
            {
                Tescht.Append("0");
            }

            Tescht.Append(einByte);
            einByte = Tescht.ToString();
            Tescht.Remove(0, 8);

            return einByte;
        }

        public static string byte_xor_reihe(string a, string b, string c, string d)
        {
            //https:/ /www.rapidtables.com/convert/number/ascii-hex-bin-dec-converter.html

            /*test
            a = "1011 0011";
            b = "1101 1010";
            c = "0101 1101";
            d = "0011 0000";

            a = "b3";
            b = "da";
            c = "5d";
            d = "30";
            */


            // falls werte in hex, noch machen
            /*
            a = Convert.ToInt32(a, 16).ToString();
            b = Convert.ToInt32(b, 16).ToString();
            c = Convert.ToInt32(c, 16).ToString();
            d = Convert.ToInt32(d, 16).ToString();
            */

            // werte kommen in binär string

            // das muss noch verbessert werden, geht gerade

            Console.WriteLine("------------------------------ergeb_1: " + a + "");
            Console.WriteLine("ergeb_1: " + b + "");
            Console.WriteLine("ergeb_1: " + c + "");
            Console.WriteLine("ergeb_1: " + d + "");

            a =String.Format("{0:X2}", Convert.ToUInt64(a, 2));
            b =String.Format("{0:X2}", Convert.ToUInt64(b, 2));
            c =String.Format("{0:X2}", Convert.ToUInt64(c, 2));
            d =String.Format("{0:X2}", Convert.ToUInt64(d, 2));

            Console.WriteLine("------------------------------ergeb_2: " + a + "");
            Console.WriteLine("ergeb_1: " + b + "");
            Console.WriteLine("ergeb_1: " + c + "");
            Console.WriteLine("ergeb_1: " + d + "");


            a = Convert.ToInt32(a, 16).ToString();
            b = Convert.ToInt32(b, 16).ToString();
            c = Convert.ToInt32(c, 16).ToString();
            d = Convert.ToInt32(d, 16).ToString();


            Console.WriteLine("------------------------------ergeb_3: " + a + "");
            Console.WriteLine("ergeb_1: " + b + "");
            Console.WriteLine("ergeb_1: " + c + "");
            Console.WriteLine("ergeb_1: " + d + "");



            byte doh1 = byte.Parse(a);
            byte doh2 = byte.Parse(b);
            byte doh3 = byte.Parse(c);
            byte doh4 = byte.Parse(d);


            Console.WriteLine("ergeb_1: " + a + "");

            byte bitergeb = (byte)(doh1 ^ doh2 ^ doh3 ^ doh4);
            string multa = Convert.ToString(bitergeb, 2);

            multa= byteNullen(multa);

            Console.WriteLine("ergeb: " + multa + "");

            return multa;
        }


        internal static void entschluesselung(string Text_verschluesslt, string passwort)
        {
            //bei der entschlüsselung wird alles anders herum gemacht, also
            // Key Addition
            // Mix Column
            // Shift Row
            // Substitution
            // dabei wird invers verfahren, so ändert sich die S-Box (matrix ist inverse) und xor ist inverse auf xor
            // a xor b = c
            // a xor c = b
            // b xor c = a

            //text kommt base64 codiert
            Text_verschluesslt = Base.entcodierung(Text_verschluesslt);


            string[,] matrix = new string[100, 100];
            string[,] hash_matrix = new string[100, 100];

            string[,] matrix_bin = new string[100, 100];
            string[,] hash_matrix_bin = new string[100, 100];

            int springer = 0;
            string hash = Aes_verschluesselung.sha256(passwort);


            Console.WriteLine("              hash: " + hash + "");
            Console.WriteLine("Text_verschluesslt: " + Text_verschluesslt + "");
            Console.WriteLine("");

            // den verschlüsselten text und den hash vom passwort holen
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    matrix[j, i] = Text_verschluesslt.Substring(springer, 2);
                    hash_matrix[j, i] = hash.Substring(springer, 2);


                    matrix_bin[j, i] = Aes_verschluesselung.sollBinaer(matrix[j, i]);
                    hash_matrix_bin[j, i] = Aes_verschluesselung.sollBinaer(hash_matrix[j, i]);


                    //Console.WriteLine("Zeile: " + i + " Spalte: " + j + " springer: " + springer + " matrix_bin: " + matrix[j, i] + " hash_matrix: " + hash_matrix[j, i] + "");
                    //Console.WriteLine("                       matrix_bin: " + matrix_bin[j, i] + " hash_bin: " + hash_matrix_bin[j, i] + "");
                    springer += 2;
                }
            }

            string[,] matrix_Substitution = new string[100, 100];
            int aes_runde = 0;

            // hier fangen die runden an

            for (aes_runde = 0; aes_runde < 14; aes_runde++)
            {

                if (aes_runde > 0)
                {
                    Console.WriteLine("--------------------------------------------------------------------");
                    Console.WriteLine("                  Hier fängt eine neue entschlüssel Runde an");
                    Console.WriteLine("--------------------------------------------------------------------");
                    for (int i = 0; i < 8; i++)
                    {

                        for (int j = 0; j < 4; j++)
                        {

                            matrix[j, i] = matrix_Substitution[j, i];
                            matrix_bin[j, i] = Aes_verschluesselung.sollBinaer(matrix[j, i]);
                            //Console.WriteLine(" matrix: " + matrix[j, i] + " matrix_Substitution: " + matrix_Substitution[j, i] + "");


                        }
                    }
                }





                //------------Key Addition

                string[,] KeyAddition = new string[100, 100];


                string[,] KeyAddition_bin = new string[100, 100];
                string ergeg_lang = "";

                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {

                        KeyAddition_bin[j, i] = Aes_verschluesselung.xor(matrix_bin[j, i], hash_matrix_bin[j, i]);

                        //Console.WriteLine("matrix_bin: " + matrix_bin[j, i] + " hash_matrix_bin: " + hash_matrix_bin[j, i] + "KeyAddition_bin: " + KeyAddition_bin[j, i] + "");

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



                //---------Mix Column
                //https:/ /nvlpubs.nist.gov/nistpubs/fips/nist.fips.197.pdf
                // hierfür muss mit einer neuen gegebenen matrix multipliziert und xor gemacht werden


                string ergeb = "";
                string ergeb_1;
                string ergeb_2 = "";
                string ergeb_3 = "";
                string ergeb_4 = "";
                
                //galois feld invers
                /*
                0e 0b 0d 09  
                09 0e 0b 0d
                0d 09 0e 0b
                0b 0d 09 0e
                */

                string multi_0e = "0e";
                string multi_0b = "0b";
                string multi_0d = "0d";
                string multi_09 = "09";

                string[,] MixColumn = new string[100, 100];
                string[,] MixColumn_bin = new string[100, 100];
                StringBuilder zwischenAblage = new StringBuilder();



                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        //Console.WriteLine("Anfang : " + KeyAddition[j, i] + "");

                        //e b d 9
                        if (j == 0)
                        {
                            //Console.WriteLine("--1");

                            ergeb_1 = byte_multi_neu(KeyAddition[0, i], multi_0e);
                            ergeb_2 = byte_multi_neu(KeyAddition[1, i], multi_0b);
                            ergeb_3 = byte_multi_neu(KeyAddition[2, i], multi_0d);
                            ergeb_4 = byte_multi_neu(KeyAddition[3, i], multi_09);
                            /*
                            Console.WriteLine("ergeb_1: " + ergeb_1 + "");
                            Console.WriteLine("ergeb_2: " + ergeb_2 + "");
                            Console.WriteLine("ergeb_3: " + ergeb_3 + "");
                            Console.WriteLine("ergeb_4: " + ergeb_4 + "");
                            */

                            /* ist fehlerhaft

                            ergeb = Aes.xor(ergeb_1, ergeb_2);
                            ergeb = Aes.xor(ergeb, ergeb_3);
                            ergeb = Aes.xor(ergeb, ergeb_4);
                            */
                            ergeb = byte_xor_reihe(ergeb_1, ergeb_2, ergeb_3, ergeb_4);

                            //Console.WriteLine("KeyAddition: "+ KeyAddition[j, i] + " Ergebnis: " + ergeb + "");

                        }
                        //9 e b d
                        else if (j == 1)
                        {
                            //Console.WriteLine("--2");

                            ergeb_1 = byte_multi_neu(KeyAddition[0, i], multi_09);
                            ergeb_2 = byte_multi_neu(KeyAddition[1, i], multi_0e);
                            ergeb_3 = byte_multi_neu(KeyAddition[2, i], multi_0b);
                            ergeb_4 = byte_multi_neu(KeyAddition[3, i], multi_0d);

                            ergeb = byte_xor_reihe(ergeb_1, ergeb_2, ergeb_3, ergeb_4);
                        }
                        //d 9 e b
                        else if (j == 2)
                        {
                            //Console.WriteLine("--3");

                            ergeb_1 = byte_multi_neu(KeyAddition[0, i], multi_0d);
                            ergeb_2 = byte_multi_neu(KeyAddition[1, i], multi_09);
                            ergeb_3 = byte_multi_neu(KeyAddition[2, i], multi_0e);
                            ergeb_4 = byte_multi_neu(KeyAddition[3, i], multi_0b);

                            ergeb = byte_xor_reihe(ergeb_1, ergeb_2, ergeb_3, ergeb_4);

                        }
                        //b d 9 e
                        else if (j == 3)
                        {
                            //Console.WriteLine("--4");

                            ergeb_1 = byte_multi_neu(KeyAddition[0, i], multi_0b);
                            ergeb_2 = byte_multi_neu(KeyAddition[1, i], multi_0d);
                            ergeb_3 = byte_multi_neu(KeyAddition[2, i], multi_09);
                            ergeb_4 = byte_multi_neu(KeyAddition[3, i], multi_0e);

                            ergeb = byte_xor_reihe(ergeb_1, ergeb_2, ergeb_3, ergeb_4);
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


                        //da noch klasse für machen

                        int byteLänge = ergeb.Length;
                        int diffBit = 8 - byteLänge;

                        // fehlende nullen
                        for (int k = 0; k < diffBit; k++)
                        {
                            //Console.WriteLine("teil matrix: " + matrix_bin[j, i] + "");
                            //Console.WriteLine("diffBit: " + diffBit + " j: "+j+"");
                            zwischenAblage.Append("0");
                        }

                        byteLänge += diffBit;
                        zwischenAblage.Append(ergeb);

                        MixColumn_bin[j, i] = zwischenAblage.ToString();
                        zwischenAblage.Remove(0, 8);

                        //Console.WriteLine("Spalte: " + i + " Zeile: " + j + "  KeyAddition: " + KeyAddition[j, i] + " wurde zu " + MixColumn_bin[j, i] + " somit zu hex: " + MixColumn[j, i] + "");
                    }
                }



                //---------Shift Row

                //gleiche nur nach rechts shiften


                string[,] matrix_ShiftRow = new string[100, 100];
                string[,] matrix_ShiftRow_hex = new string[100, 100];
                string[,] matrix_ShiftRow_bin = new string[100, 100];
                StringBuilder zwischenSpeicherung = new StringBuilder();

                string zwischenZeichen = "";
                string zwischenSpeicher = "";
                string zwischenZeichen_links = "";


                for (int j = 0; j < 4; j++)
                {
                    //holt sich zeilenweise
                    for (int i = 0; i < 8; i++)
                    {
                        zwischenAblage.Append(MixColumn[j, i]);

                        //Console.WriteLine(" Zeile: " + i + " Spalte: " + j + "");
                        //Console.WriteLine("ShiftRow: " + matrix_ShiftRow[j, i] + "");
                    }

                    zwischenSpeicher = zwischenAblage.ToString();
                    zwischenAblage.Remove(0, 16);

                    if (j == 0)
                    {
                        zwischenSpeicherung.Append(zwischenSpeicher);
                    }
                    else if (j == 1)
                    {
                        zwischenZeichen_links = zwischenSpeicher.Substring(14, 2);
                        zwischenSpeicherung.Append(zwischenZeichen_links);
                        zwischenSpeicherung.Append(zwischenSpeicher.Substring(0, 14));
                    }
                    else if (j == 2)
                    {
                        zwischenZeichen_links = zwischenSpeicher.Substring(12, 4);
                        zwischenSpeicherung.Append(zwischenZeichen_links);
                        zwischenSpeicherung.Append(zwischenSpeicher.Substring(0, 12));
                    }
                    else if (j == 3)
                    {
                        zwischenZeichen_links = zwischenSpeicher.Substring(10, 6);
                        zwischenSpeicherung.Append(zwischenZeichen_links);
                        zwischenSpeicherung.Append(zwischenSpeicher.Substring(0, 10));
                    }
                    else
                        Console.WriteLine(" --fehler--");

                    //Console.WriteLine("ShiftRow: " + zwischenSpeicherung + "");

                    zwischenZeichen = zwischenSpeicherung.ToString();


                    for (int i = 0; i < 8; i++)
                    {
                        matrix_ShiftRow[j, i] = zwischenZeichen.Substring(i * 2, 2);

                        matrix_ShiftRow_bin[j, i] = Aes_verschluesselung.sollBinaer(matrix_ShiftRow[j, i]);

                        //Console.WriteLine("matrix_ShiftRow[j, i] " + matrix_ShiftRow[j, i] + " matrix_ShiftRow_bin[j, i]: " + matrix_ShiftRow_bin[j, i] + "");
                    }
                    zwischenSpeicherung.Remove(0, 16);
                }



                //----------Substitution

                //jetzt wird mit der inversen S-Box das gleiche gemacht

                /*
                   | 0  1  2  3  4  5  6  7  8  9  a b  c d  e f
                ---| --| --| --| --| --| --| --| --| --| --| --| --| --| --| --| --|
                00 | 52 09 6a d5 30 36 a5 38 bf 40 a3 9e 81 f3 d7 fb
                10 | 7c e3 39 82 9b 2f ff 87 34 8e 43 44 c4 de e9 cb
                20 | 54 7b 94 32 a6 c2 23 3d ee 4c 95 0b 42 fa c3 4e
                30 | 08 2e a1 66 28 d9 24 b2 76 5b a2 49 6d 8b d1 25
                40 | 72 f8 f6 64 86 68 98 16 d4 a4 5c cc 5d 65 b6 92
                50 | 6c 70 48 50 fd ed b9 da 5e 15 46 57 a7 8d 9d 84
                60 | 90 d8 ab 00 8c bc d3 0a f7 e4 58 05 b8 b3 45 06
                70 | d0 2c 1e 8f ca 3f 0f 02 c1 af bd 03 01 13 8a 6b
                80 | 3a 91 11 41 4f 67 dc ea 97 f2 cf ce f0 b4 e6 73
                90 | 96 ac 74 22 e7 ad 35 85 e2 f9 37 e8 1c 75 df 6e
                a0 | 47 f1 1a 71 1d 29 c5 89 6f b7 62 0e aa 18 be 1b
                b0 | fc 56 3e 4b c6 d2 79 20 9a db c0 fe 78 cd 5a f4
                c0 | 1f dd a8 33 88 07 c7 31 b1 12 10 59 27 80 ec 5f
                d0 | 60 51 7f a9 19 b5 4a 0d 2d e5 7a 9f 93 c9 9c ef
                e0 | a0 e0 3b 4d ae 2a f5 b0 c8 eb bb 3c 83 53 99 61
                f0 | 17 2b 04 7e ba 77 d6 26 e1 69 14 63 55 21 0c 7d

                */


                // hier ist die s-Box

                //https:/ /cryptography.fandom.com/wiki/Rijndael_S-box
                string Rijndael_ForwardS_box = "52096ad53036a538bf40a39e81f3d7fb7ce339829b2fff87348e4344c4dee9cb547b9432a6c2233dee4c950b42fac34e082ea16628d924b2765ba2496d8bd12572f8f66486689816d4a45ccc5d65b6926c704850fdedb9da5e154657a78d9d8490d8ab008cbcd30af7e45805b8b34506d02c1e8fca3f0f02c1afbd0301138a6b3a9111414f67dcea97f2cfcef0b4e67396ac7422e7ad3585e2f937e81c75df6e47f11a711d29c5896fb7620eaa18be1bfc563e4bc6d279209adbc0fe78cd5af41fdda8338807c731b11210592780ec5f60517fa919b54a0d2de57a9f93c99cefa0e03b4dae2af5b0c8ebbb3c83539961172b047eba77d626e169146355210c7d";

                string hex_vergleich = "0123456789abcdef";

                string[,] S_Box = new string[100, 100];
                springer = 0;

                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        S_Box[i, j] = Rijndael_ForwardS_box.Substring(springer, 2);
                        //Console.WriteLine(" Zeile: " + i + " Spalte: " + j + " springer: " + springer + " S_Box: " + S_Box[i, j] + "");
                        springer += 2;
                    }
                }
                /*
                for (int i = 0; i < 16; i++)
                {
                    Console.WriteLine("" + S_Box[i, 0] + "  " + S_Box[i, 1] + "  " + S_Box[i, 2] + "  " + S_Box[i, 3] + "  " + S_Box[i, 4] + "  " + S_Box[i, 5] + "  " + S_Box[i, 6] + "  " + S_Box[i, 7] + "  " + S_Box[i, 8] + "  " + S_Box[i, 9] + "  " + S_Box[i, 10] + "  " + S_Box[i, 11] + "  " + S_Box[i, 12] + "  " + S_Box[i, 13] + "  " + S_Box[i, 14] + "  " + S_Box[i, 15] + "");

                }
                */


                int r = 0;

                int zeile = 0;
                int spalte = 0;

                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        for (int k = 0; k < 16; k++)
                        {
                            /*
                            Console.WriteLine("anfang: " + matrix_ShiftRow[j, i] + "");
                            Console.WriteLine("if  " + matrix_ShiftRow[j, i].Substring(0, 1) + "  ==  " + hex_vergleich.Substring(k, 1) + "");
                            Console.WriteLine("if  " + matrix_ShiftRow[j, i].Substring(1, 1) + "  ==  " + hex_vergleich.Substring(k, 1) + "");
                            */

                            // alt if (matrix_ShiftRow[j, i].Substring(0, 1) == hex_vergleich.Substring(k, 1))
                            // großschreibung ignorieren
                            if (matrix_ShiftRow[j, i].Substring(0, 1).Equals(hex_vergleich.Substring(k, 1), StringComparison.OrdinalIgnoreCase))
                            {
                                zeile = k;
                            }
                            if (matrix_ShiftRow[j, i].Substring(1, 1).Equals(hex_vergleich.Substring(k, 1), StringComparison.OrdinalIgnoreCase))
                            {
                                spalte = k;
                            }
                            //Console.WriteLine(" Zeile: " + i + " Spalte: " + j + " k: " + k + "");
                        }
                        matrix_Substitution[j, i] = S_Box[zeile, spalte];
                        //Console.WriteLine(" Zeile: " + zeile + " Spalte: " + spalte + " matrix_Substitution: " + matrix_Substitution[j, i] + "");
                    }
                }

                Console.WriteLine("hex matrix");

                for (int i = 0; i < 4; i++)
                {
                    Console.WriteLine("" + matrix[i, 0] + "  " + matrix[i, 1] + "  " + matrix[i, 2] + "  " + matrix[i, 3] + "  " + matrix[i, 4] + "  " + matrix[i, 5] + "  " + matrix[i, 6] + "  " + matrix[i, 7] + "");
                }

                Console.WriteLine("invers KeyAddition");

                for (int i = 0; i < 4; i++)
                {
                    Console.WriteLine("" + KeyAddition[i, 0] + "  " + KeyAddition[i, 1] + "  " + KeyAddition[i, 2] + "  " + KeyAddition[i, 3] + "  " + KeyAddition[i, 4] + "  " + KeyAddition[i, 5] + "  " + KeyAddition[i, 6] + "  " + KeyAddition[i, 7] + "");
                }

                Console.WriteLine("invers MixColumn");

                for (int i = 0; i < 4; i++)
                {
                    Console.WriteLine("" + MixColumn[i, 0] + "  " + MixColumn[i, 1] + "  " + MixColumn[i, 2] + "  " + MixColumn[i, 3] + "  " + MixColumn[i, 4] + "  " + MixColumn[i, 5] + "  " + MixColumn[i, 6] + "  " + MixColumn[i, 7] + "");
                }

                Console.WriteLine("invers matrix_ShiftRow");

                for (int i = 0; i < 4; i++)
                {
                    Console.WriteLine("" + matrix_ShiftRow[i, 0] + "  " + matrix_ShiftRow[i, 1] + "  " + matrix_ShiftRow[i, 2] + "  " + matrix_ShiftRow[i, 3] + "  " + matrix_ShiftRow[i, 4] + "  " + matrix_ShiftRow[i, 5] + "  " + matrix_ShiftRow[i, 6] + "  " + matrix_ShiftRow[i, 7] + "");
                }

                Console.WriteLine("invers matrix_Substitution");

                for (int i = 0; i < 4; i++)
                {
                    Console.WriteLine("" + matrix_Substitution[i, 0] + "  " + matrix_Substitution[i, 1] + "  " + matrix_Substitution[i, 2] + "  " + matrix_Substitution[i, 3] + "  " + matrix_Substitution[i, 4] + "  " + matrix_Substitution[i, 5] + "  " + matrix_Substitution[i, 6] + "  " + matrix_Substitution[i, 7] + "");
                }

            }
            //rundenende


            string hextest = "";

            string[,] TextMatrix = new string[10,10];

            StringBuilder Text = new StringBuilder();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    //um das padding weg zu haben
                    if (matrix_Substitution[j, i] == "0f")
                        goto End;

                    TextMatrix[j, i]= Convert.ToChar(Convert.ToUInt32(matrix_Substitution[j, i], 16)).ToString();
                    hextest += TextMatrix[j, i];

                    //Console.WriteLine("matrix_Substitution: " + matrix_Substitution[j, i] + " hextest: " + hextest + "");
                }
            }

        End:
            ;


            Console.WriteLine("");
            Console.WriteLine("Der Text lautet: " + hextest + "");

        }
    }
}
