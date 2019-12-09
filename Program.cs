using System;
using System.Threading;

namespace AutonomníVozidlo
{
    public enum Weather {Slunecno, Prsi, Snezi, Vychrice, Apokalypsa}
    class Program
    {
        static void Main(string[] args)
        {
            Meteo met = new Meteo();
            string cesta = "CCCCCCMMMCCCCCMMMCCCCCCTTCCCCCMMMCCCCCCTTCCCCCCCCCCTTCCCCCCC";
            Autonomvz auto = new Autonomvz(100, cesta);
            vypsat vyp = new vypsat();
            vyp.Sub(auto);
            RC rc = new RC();
            rc.Sub(auto);
            auto.Drive();
            
            
        }

        public class Autonomvz
        {
            public Autonomvz(decimal cestovnirychlost, string cesta)
            {
                Cesta = cesta;
                CestovniRychlost = cestovnirychlost;
                ID = Guid.NewGuid();
                
            }
            public delegate void Change(Autonomvz a, Road r);
            public event Change change;
            bool drive;
            Road road = new Road();
            public void Drive()
            {
                drive = true;
                string aktdruhces = "";
                string mindruhces = "";
                Pozice = 0;
                while (drive)
                {
                    if (Pozice == Cesta.Length - 1) {Console.WriteLine("dojebali jsme  8=====D"); Stop();  }
                    System.Threading.Thread.Sleep((int)((5m/CestovniRychlost)*1000m));

                    aktdruhces = Cesta.Substring(Pozice, 1);
                    
                    road.TypeOfRoad = aktdruhces;

                    if (aktdruhces != mindruhces)
                    {
                        if (mindruhces != "")
                        {
                            change(this, road);
                            
                        }
                    }
                    mindruhces = aktdruhces;
                    Pozice++;
                }
            }
            public void Stop()
            {
                drive = false;
                change(this,road);
                //Thread.ResetAbort();
                
            }
            int Pozice { get; set; }
            public decimal CestovniRychlost { get; set; }
            public decimal AktualniRychlost { get; set; }

            public string Cesta { get; set; }
            public bool Svetla { get; set; }
            public Guid ID { get; set; }
        }
        public class Road : EventArgs
        {
            public string TypeOfRoad { get;set;}
        }


        public class RC
        {
            public RC() { }
            public void Sub(Autonomvz a)
            {
                a.change += StateAdapt;
            }
            public void StateAdapt(Autonomvz a, Road r)
            {
                switch (r.TypeOfRoad)
                {
                    case "C": a.Svetla = false; a.AktualniRychlost = a.CestovniRychlost; break;
                    case "T": a.Svetla = true; a.AktualniRychlost =0.8m* a.CestovniRychlost; break;
                    case "M": a.Svetla = false; a.AktualniRychlost = 0.5m * a.CestovniRychlost; break;

                }
                
            }
        }

        public class Meteo
        {
          public static Random ran = new Random();
          public Thread pocasi = new Thread(new ThreadStart(Metoda));
           public Meteo() { pocasi.Start(); }


            public static void Metoda()
            {
                while (true)
                {
                    System.Threading.Thread.Sleep(ran.Next(3000, 5001));
                    weather = (Weather)ran.Next(Enum.GetValues(typeof(Weather)).Length+1);
                    Console.WriteLine(weather);
                }
            }

            
            public static Weather weather {get;set;}
        }
       



        class vypsat
        {
          public  vypsat() { }
            public void Sub(Autonomvz a)
            {
                a.change += Vypsat;
            }
            public void Vypsat(Autonomvz a, Road r)
            { Console.WriteLine(r.TypeOfRoad+" "+a.ID); }

        }
    }
}
