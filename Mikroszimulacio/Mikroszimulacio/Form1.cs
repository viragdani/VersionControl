using Mikroszimulacio.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mikroszimulacio
{
    public partial class Form1 : Form
    {
        List<Person> Population = new List<Person>();
        List<BirthProbability> BirthProbabilities = new List<BirthProbability>();
        List<DeathProbability> DeathProbabilities = new List<DeathProbability>();
        Random rnd = new Random(1999);
        List<Person> Malepop = new List<Person>();
        List<Person> Femalepop = new List<Person>();
        int publicyear;
        public Form1()
        {
            InitializeComponent();
            Population = GetPopulation(textBox1.Text);
            BirthProbabilities = GetBirthProb(@"E:\Mikroszimulacio\születés.csv");
            DeathProbabilities = GetDeathProb(@"E:\Mikroszimulacio\halál.csv");
            
        }
        public List<Person> GetPopulation(string csvpath)
        {
            List<Person> population = new List<Person>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    population.Add(new Person()
                    {
                        BirthYear = int.Parse(line[0]),
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[1]),
                        NbrOfChildren = int.Parse(line[2])
                    });
                }
            }
            return population;
        }
        public List<BirthProbability> GetBirthProb(string csvpath)
        {
            List<BirthProbability> birthProbabilities = new List<BirthProbability>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    birthProbabilities.Add(new BirthProbability()
                    {
                        Age = int.Parse(line[0]),
                        NbrOfChildren = int.Parse(line[1]),
                        BProbability = double.Parse(line[2])
                    });
                }
            }
            return birthProbabilities;
        }
        public List<DeathProbability>  GetDeathProb(string csvpath)
        {
            List<DeathProbability> deathProbabilities = new List<DeathProbability>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    deathProbabilities.Add(new DeathProbability()
                    {
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[0]),
                        Age = int.Parse(line[1]),
                        DProbability = double.Parse(line[2])
                    });
                }
            }
            return deathProbabilities;
        }
        private void SimStep(int year, Person person)
        {
            //Ha halott akkor kihagyjuk, ugrunk a ciklus következő lépésére
            if (!person.IsAlive) return;

            if (person.Gender == Gender.Male)
            {
                Malepop.Add(person);
            }
            else
            {
                Femalepop.Add(person);
            }
            // Letároljuk az életkort, hogy ne kelljen mindenhol újraszámolni
            int age = (int)(year - person.BirthYear);

            // Halál kezelése
            // Halálozási valószínűség kikeresése
            double pDeath = (from x in DeathProbabilities
                             where x.Gender == person.Gender && x.Age == age
                             select x.DProbability).FirstOrDefault();
            // Meghal a személy?
            if (rnd.NextDouble() <= pDeath)
            {
                person.IsAlive = false;
                if (person.Gender==Gender.Male)
                {
                    Malepop.Remove(person);
                }
                else
                {
                    Femalepop.Remove(person);
                }
            }

            
            //Születés kezelése - csak az élő nők szülnek
            if (person.IsAlive && person.Gender == Gender.Female)
            {
                //Szülési valószínűség kikeresése
                double pBirth = (from x in BirthProbabilities
                                 where x.Age == age
                                 select x.BProbability).FirstOrDefault();
                //Születik gyermek?
                if (rnd.NextDouble() <= pBirth)
                {
                    Person újszülött = new Person();
                    újszülött.BirthYear = year;
                    újszülött.NbrOfChildren = 0;
                    újszülött.Gender = (Gender)(rnd.Next(1, 3));
                    Population.Add(újszülött);
                }
            }
        }
        private void Simulation()
        {
            Malepop.Clear();
            Femalepop.Clear();
            richTextBox1.Clear();
            for (int year = 2005; year < numericUpDown1.Value; year++)
            {
                publicyear = year;
                for (int i = 0; i < Population.Count; i++)
                {
                    SimStep(year, Population[i]);
                    DisplayResults();
                }

                int NbrOfMales = (from x in Population
                                  where x.Gender == Gender.Male && x.IsAlive == true
                                  select x).Count();
                int NbrOfFemales = (from x in Population
                                    where x.Gender == Gender.Female && x.IsAlive == true
                                    select x).Count();
                Console.WriteLine(
                string.Format("Év:{0} Fiúk:{1} Lányok:{2}", year, NbrOfMales, NbrOfFemales));
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            Simulation();
            
        }
        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            if (ofd.ShowDialog()==DialogResult.OK)
            {
                textBox1.Text = ofd.FileName;
            }
        }
        private void DisplayResults()
        {

                richTextBox1.Text = richTextBox1+("Szimulációs év: " + publicyear.ToString() + "/n" + "/t" + "Fiúk: " + Malepop.Count.ToString() + "/n" + "/t" + "Lányok: " + Femalepop.Count.ToString()+"/n"+"/n");
        }
    }
}
