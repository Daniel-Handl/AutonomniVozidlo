using System;

namespace AutonomníVozidlo
{
    public enum Weather {Slunecno = 1, Prsi, Snezi, Vychrice, Apokalypsa}
    class Program
    {
        static void Main(string[] args)
        {
            
            string cesta = "CCCCCCMMMCCCCCMMMCCCCCCTTCCCCCMMMCCCCCCTTCCCCCCCCCCTTCCCCCCC";
            Autonomvz auto = new Autonomvz(100, cesta);
            vypsat vyp = new vypsat();
            vyp.Sub(auto);
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
                    if (Pozice == Cesta.Length - 1) { Stop(); Console.WriteLine("dojeli jsme  8=====D"); }
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
            RC() { }
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
