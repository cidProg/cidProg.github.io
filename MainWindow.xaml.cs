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
using System.Threading;
using System.Drawing.Drawing2D;
using OpenXmlPowerTools;

namespace Verwaltung_grafisch
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


        }
        /*
         Kunde suchen
         Produkt-Bestellungen suchen
         neuen kunden hinzufügen
         Einstellungen
         
         */

        private void Button_logIn_Click(object sender, RoutedEventArgs e)
        {
            string benutzername = "";  //kev
            string passwort = "";     //kev2
            string anrede_H = "Herr";
            string anrede_F = "Frau";
            string anrede = "";
            string name = "";
            string gruß = "";


            if (benutzername == txt_benutzername.Text && passwort == txt_passwort.Password.ToString())
            {

                //StackPanel.Children.Remove(txt_passwort);

                //auf der rechten Seite alles neu
                StackPanel.Children.Clear();

                

                if(benutzername=="kev")
                {
                    anrede = anrede_H;
                    name = "Warren";
                }
                else if (benutzername == "")
                {
                    anrede = anrede_F;
                    name = "Kallen";
                }

                //zeit holen für die begrüßung
                int datetime = int.Parse(DateTime.Now.ToString("HH"));
                
                if (datetime < 11)
                    gruß = "Morgen";
                else if (datetime < 18)
                    gruß = "Tag";
                else
                    gruß = "Abend";

                /*
                lbl_name.FontSize = 25;

                lbl_gruß.Content = "Guten " + gruß + ",";
                lbl_name.Content = anrede + " " + name ;

                */
                System.Windows.Controls.Label lbl1 = new Label();

                //FontSize="25" FontFamily="Roboto" HorizontalAlignment="Center" Margin="20" Padding="0,20,0,0"

                lbl1.Content = "Was wollen Sie tun?";
                Grid.SetRow(lbl1, 1);
                lbl1.FontFamily = new FontFamily("Roboto");
                lbl1.FontSize = 20;
                lbl1.HorizontalAlignment = HorizontalAlignment.Center;
                lbl1.Margin = new Thickness(20, 20, 20, 20);
                StackPanel.Children.Add(lbl1);


                // butto 1 kunde suchen

                System.Windows.Controls.Button Button_suche_kunde = new Button();

                Grid.SetRow(Button_suche_kunde, 1);
                Button_suche_kunde.Content = "Kunde suchen";
                Button_suche_kunde.Name = "Button_suche_kunde";
                Button_suche_kunde.Click += Button_suche_kunde_Click;
                Button_suche_kunde.BorderThickness = new Thickness(0, 0, 0, 0);
                Button_suche_kunde.FontSize = 15;
                Button_suche_kunde.FontFamily = new FontFamily("Roboto");
                //button.Background = new SolidColorBrush(Color.FromArgb(Avalue, rValue, gValue, bValue));
                Button_suche_kunde.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fe6d30"));


                Border a_Border = new Border();
                a_Border.CornerRadius = new CornerRadius(10, 10, 10, 10);
                a_Border.BorderThickness = new Thickness(5);
                a_Border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fe6d30"));
                a_Border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fe6d30"));
                a_Border.Width = 170;
                a_Border.Height = 30;
                a_Border.Margin = new Thickness(0, 15, 0, 0);
                a_Border.Child = Button_suche_kunde;
                StackPanel.Children.Add(a_Border);


                // butto 1 kunde zufügen

                System.Windows.Controls.Button Button_hinzufuegen_kunde = new Button();

                Grid.SetRow(Button_hinzufuegen_kunde, 1);
                Button_hinzufuegen_kunde.Content = "Kunde hinzufügen";
                Button_hinzufuegen_kunde.Name = "Button_hinzufuegen_kunde";
                Button_hinzufuegen_kunde.Click += Button_hinzufuegen_kunde_Click;
                Button_hinzufuegen_kunde.BorderThickness = new Thickness(0, 0, 0, 0);
                Button_hinzufuegen_kunde.FontSize = 15;
                Button_hinzufuegen_kunde.FontFamily = new FontFamily("Roboto");
                Button_hinzufuegen_kunde.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fe6d30"));

                Border c_Border = new Border();
                c_Border.CornerRadius = new CornerRadius(10, 10, 10, 10);
                c_Border.BorderThickness = new Thickness(5);
                c_Border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fe6d30"));
                c_Border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fe6d30"));
                c_Border.Width = 170;
                c_Border.Height = 30;
                c_Border.Margin = new Thickness(0, 15, 0, 0);
                c_Border.Child = Button_hinzufuegen_kunde;
                StackPanel.Children.Add(c_Border);

                // butto 1 kunde suchen

                System.Windows.Controls.Button Button_suche_Bestellungen = new Button();

                Grid.SetRow(Button_suche_Bestellungen, 1);
                Button_suche_Bestellungen.Content = "Bestellungen anzeigen";
                Button_suche_Bestellungen.Name = "Button_suche_Bestellungen";
                Button_suche_Bestellungen.Click += Button_suche_Bestellungen_Click;
                Button_suche_Bestellungen.BorderThickness = new Thickness(0, 0, 0, 0);
                Button_suche_Bestellungen.FontSize = 15;
                Button_suche_Bestellungen.FontFamily = new FontFamily("Roboto");
                Button_suche_Bestellungen.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fe6d30"));

                Border d_Border = new Border();
                d_Border.CornerRadius = new CornerRadius(10, 10, 10, 10);
                d_Border.BorderThickness = new Thickness(5);
                d_Border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fe6d30"));
                d_Border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fe6d30"));
                d_Border.Width = 170;
                d_Border.Height = 30;
                d_Border.Margin = new Thickness(0, 15, 0, 0);
                d_Border.Child = Button_suche_Bestellungen;
                StackPanel.Children.Add(d_Border);

                // butto 1 kunde suchen

                System.Windows.Controls.Button Button_einstellungen = new Button();

                Grid.SetRow(Button_einstellungen, 1);
                Button_einstellungen.Content = "Einstellungen";
                Button_einstellungen.Name = "Button_einstellungen";
                Button_einstellungen.Click += Button_einstellungen_Click;
                Button_einstellungen.BorderThickness = new Thickness(0, 0, 0, 0);
                Button_einstellungen.FontSize = 15;
                Button_einstellungen.FontFamily = new FontFamily("Roboto");
                Button_einstellungen.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fe6d30"));

                Border e_Border = new Border();
                e_Border.CornerRadius = new CornerRadius(10, 10, 10, 10);
                e_Border.BorderThickness = new Thickness(5);
                e_Border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fe6d30"));
                e_Border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fe6d30"));
                e_Border.Width = 170;
                e_Border.Height = 30;
                e_Border.Margin = new Thickness(0, 15, 0, 0);
                e_Border.Child = Button_einstellungen;
                StackPanel.Children.Add(e_Border);

                // so im code machbar, beispiel

                Button pswB = new Button();
                //StackPanel.Children.Add(pswB);

                pswB.Content = "test";
                pswB.Name = "txtB_benutzername";
                pswB.HorizontalAlignment = HorizontalAlignment.Center;
                pswB.Height = 22;
                pswB.Width = 2444;
                pswB.Background = Brushes.Blue;
                pswB.BorderThickness = new Thickness(0, 0, 0, 0);
                Grid.SetRow(pswB, 1);


                TextBlock da_textBlock = new TextBlock(new Run("A bit of text content..."));

                da_textBlock.Background = Brushes.AntiqueWhite;
                da_textBlock.Foreground = Brushes.Navy;

                da_textBlock.FontFamily = new FontFamily("Century Gothic");
                da_textBlock.FontSize = 12;
                da_textBlock.FontStretch = FontStretches.UltraExpanded;
                da_textBlock.FontStyle = FontStyles.Italic;
                da_textBlock.FontWeight = FontWeights.UltraBold;

                da_textBlock.LineHeight = Double.NaN;
                //padding left,top,right,bottom
                da_textBlock.Padding = new Thickness(5, 10, 5, 10);
                da_textBlock.Margin = new Thickness(0, 10, 0, 0);
                da_textBlock.TextAlignment = TextAlignment.Center;
                da_textBlock.TextWrapping = TextWrapping.Wrap;

                da_textBlock.Typography.NumeralStyle = FontNumeralStyle.OldStyle;
                da_textBlock.Typography.SlashedZero = true;

                //button click
                /*
                Button tescht = new Button();
                tescht.Click += Button_Click; //Hooking up to event
                myGrid.Children.Add(tescht); //Adding to grid or other parent

                private void Button_Click(object sender, RoutedEventArgs e) //Event which will be triggerd on click of ya button
                {
                    throw new NotImplementedException();
                }
                */

                // umrandung
                Button tescht = new Button();
                tescht.BorderThickness = new Thickness(0, 0, 0, 0);
                tescht.Content = "text hhhh";
                //tescht.Background.Opacity = 1;
                //.....weiteres einstellen
                Border b = new Border();
                b.CornerRadius = new CornerRadius(10,10, 10, 10);
                b.BorderThickness = new Thickness(2);
                b.BorderBrush = Brushes.Blue;
                b.Background = Brushes.Blue;
                b.Width = 100;
                b.Height = 22;
                b.Child = tescht;//adding this rectangle will show you how the corner is overflowing
                //StackPanel.Children.Add(b);
                //StackPanel.Children.Add(b);




                /*
                textBox1.Background = Brushes.White;
                textBox1.Background = new SolidColorBrush(Colors.White);
                textBox1.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0, 0));
                textBox1.Background = System.Windows.SystemColors.MenuHighlightBrush;
                */
            }
            else
            {
                if (benutzername != txt_benutzername.Text)
                    lbl_benutzername_fehler.Content = "##### Der Benutzername ist falsch! #####";
                else
                    lbl_benutzername_fehler.Content = "";
                if (passwort != txt_passwort.Password.ToString())
                    lbl_passwort_fehler.Content = "##### Das Passwort ist falsch! #####";
                else
                    lbl_passwort_fehler.Content = "";
            }





        }


        private void Button_suche_kunde_Click(object sender, RoutedEventArgs e) //Event which will be triggerd on click of ya button
        { 
            //leer
            //var window = new Window();
            //window.Show();

            //nicht anders rum, sonst
            Window1 subWindow = new Window1();
            subWindow.Show();
            Application.Current.Windows[0].Close();

            
        }
        private void Button_hinzufuegen_kunde_Click(object sender, RoutedEventArgs e) //Event which will be triggerd on click of ya button
        {
        }
        private void Button_suche_Bestellungen_Click(object sender, RoutedEventArgs e) //Event which will be triggerd on click of ya button
        {
        }
        private void Button_einstellungen_Click(object sender, RoutedEventArgs e) //Event which will be triggerd on click of ya button
        {
        }

    }
}
