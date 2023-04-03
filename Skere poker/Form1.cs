using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skere_poker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //om random getallen te genereren
        Random random = new Random();
        //lijst met alle afbeeldingen
        List<Bitmap> images = new List<Bitmap> { Properties.Resources.dobbel1, Properties.Resources.dobbel2, Properties.Resources.dobbel3, Properties.Resources.dobbel4, Properties.Resources.dobbel5, Properties.Resources.dobbel6 };
        //1 lijst per hand
        List<int> nummers1 = new List<int>();
        List<int> nummers2 = new List<int>();
        private void controleer()
        {
            //vraagt de scores op en update de labels
            Tuple<int, string, int> score1 = Score(nummers1);
            Tuple<int, string, int> score2 = Score(nummers2);
            label2.Text = $"You had a {score1.Item2}\nYour score is: {score1.Item1}";
            label1.Text = $"Your opponent had a {score2.Item2}\nTheir score is: {score2.Item1}";
            int tocheck1, tocheck2;
            //controlleert of gelijk spel is, indien dit zo is zal het ervoor zorgen dat het de gelijkspel score controleerd in de plaats van de totaalscore
            if (score1.Item1 == score2.Item1)
            {
                label2.Text += $"\nTie score: {score1.Item3}";
                label1.Text += $"\nTie score: {score2.Item3}";
                tocheck1 = score1.Item3;
                tocheck2 = score2.Item3;
            }
            else
            {
                tocheck1 = score1.Item1;
                tocheck2 = score2.Item1;
            } //contoleert wie winnaar is door de te checken scores te vergelijken en toont messagebox met of je gewonnen bent of niet
            if (tocheck1 > tocheck2)
            {
                MessageBox.Show("You won!", "EZ");
            }
            else if (tocheck1 < tocheck2)
            {
                MessageBox.Show("You lost", "NOOB");
            }
            else
            {
                MessageBox.Show("You Tied", "Better luck next time!");
            }
        }
        private Tuple<int, string, int> Score(List<int> nummer1)
        {
            //roept alle functies die ik gemaakt heb om te zoeken welke score je hebt op, de eerste returnwaarde is de score, de tweede wat je precies hebt en de derde is de score die je wilt controleren indien er gelijkspel is
            if (straightflush(nummer1))
            {
                return Tuple.Create(7, "Straight Flush", nummer1.Sum());
            }
            else if (fiveofakind(nummer1).Item1)
            {
                return Tuple.Create(6, "Five of a kind", fiveofakind(nummer1).Item2);
            }
            else if (fourofakind(nummer1).Item1)
            {
                return Tuple.Create(5, "Four of a kind", fourofakind(nummer1).Item2);
            }
            else if (fullhouse(nummer1))
            {
                return Tuple.Create(4, "Fullhouse", nummer1.Sum());
            }
            else if (threeofakind(nummer1).Item1)
            {
                return Tuple.Create(3, "Three of a kind", threeofakind(nummer1).Item2);
            }
            else if (twopair(nummer1).Item1)
            {
                return Tuple.Create(2, "Two Pairs", twopair(nummer1).Item2 + twopair(nummer1).Item3);
            }
            else if (pair(nummer1).Item1)
            {
                return Tuple.Create(1, "Pair", pair(nummer1).Item2);
            }
            return Tuple.Create(0, "You Suck!", 0);

        }
        private bool straightflush(List<int> nummer1)
        {  //sorteert eerst de lijst en gaat dan na of elke variabele in de lijst vanaf de tweede gelijk is aan de vorige +1
            nummer1.Sort();
            for (int i = 1; i < nummer1.Count(); i++)
            {
                if (nummer1[i] != nummer1[i - 1] + 1)
                    return false;
            }
            return true;
        }
        private Tuple<int, List<int>> ofakind(int amount, List<int> nummer1)
        { //kijkt voor elke waarde van 1 tot 6 of het getal (van 1 tot 6) "amount" keer in de lijst zit, het returned het aantal keer dit het geval is met daarnaast een lijst van alle getallen waarvoor dit het geval is
            int totalamm = 0;
            List<int> kinds = new List<int>();
            for (int i = 1; i <= 6; i++)
            {
                if (nummer1.Count(getal => getal == i) == amount)
                {
                    totalamm++;
                    kinds.Add(i);
                }
            }
            return Tuple.Create(totalamm, kinds);
        }
        private Tuple<bool, int> fourofakind(List<int> nummer1)
        { //roept de functie ofakind() op en als deze functie returned dat er 1 keer 4 dezelfde getallen zijn dan returned het true met het getal dat 4 keer voorkomt (het is geen lijst meer omdat er maar 1 mogelijk is in dit geval)
            Tuple<int, List<int>> tocheck = ofakind(4, nummer1);
            if (tocheck.Item1 == 1)
                return Tuple.Create(true, tocheck.Item2[0]);
            return Tuple.Create(false, 0);
        }
        private Tuple<bool, int> fiveofakind(List<int> nummer1)
        {//hetzelfde als fourofakind() maar dan met 5
            Tuple<int, List<int>> tocheck = ofakind(5, nummer1);
            if (tocheck.Item1 == 1)
                return Tuple.Create(true, tocheck.Item2[0]);
            return Tuple.Create(false, 0);
        }//roept de functie three of a kind en pair op en returned true als beide waar zijn
        private bool fullhouse(List<int> nummer1)
        {
            if (threeofakind(nummer1).Item1 && pair(nummer1).Item1)
                return true;
            return false;
        }
        private Tuple<bool, int> threeofakind(List<int> nummer1)
        {//zelfde als fourofakind en fiveofakind
            Tuple<int, List<int>> tocheck = ofakind(3, nummer1);
            if (tocheck.Item1 == 1)
                return Tuple.Create(true, tocheck.Item2[0]);
            return Tuple.Create(false, 0);
        }
        private Tuple<bool, int, int> twopair(List<int> nummer1)
        {//controlleerd met ofakind hoeveel keer 2 dezelfde getallen voorkomen en indien dit 2 keer is dan returned het true met de waarden van de 2 unieke dobbelstenen uit de paren
            Tuple<int, List<int>> tocheck = ofakind(2, nummer1);
            if (ofakind(2, nummer1).Item1 == 2)
                return Tuple.Create(true, tocheck.Item2[0], tocheck.Item2[1]);
            return Tuple.Create(false, 0, 0);
        }
        private Tuple<bool, int> pair(List<int> nummer1)
        { //zelfde als fourofakind threeofakind en fiveofakind
            Tuple<int, List<int>> tocheck = ofakind(2, nummer1);
            if (tocheck.Item1 == 1)
                return Tuple.Create(true, tocheck.Item2[0]);
            return Tuple.Create(false, 0);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //cleared de twee handen zodat je niet met de handen van de vorige game speelt
            nummers1.Clear();
            nummers2.Clear();
            //itereert over alle pictureboxen in de form
            foreach (PictureBox pb in this.Controls.OfType<PictureBox>())
            {
                //maakt random getal tussen 1 en 6
                int randomgetal = random.Next(1, 7);
                //stelt de pictureboxen in met de juist image, de -1 is omdat images[] een lijst is en lijsten beginnen te tellen vanaf 0 en aangezien er 6 afbeeldingen in de lijst zitten is dobbelsteen 1, de 0de foto in de lijst
                pb.Image = images[randomgetal - 1];
                //als de eerste hand als 5 dobbelstenen heeft dan zal hij aan de tweede hand toevoegen
                if (nummers1.Count == 5)
                {
                    nummers2.Add(randomgetal);
                }
                else
                {
                    nummers1.Add(randomgetal);
                }
            }
            controleer();
        }
    }
}