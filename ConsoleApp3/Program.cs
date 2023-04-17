using Autofac;
using System.ComponentModel;
using static System.Formats.Asn1.AsnWriter;

//Infantry.Cavalry.Archers
public interface IContextI
{
    public void health(int dam);
    public int damage();

    public string Print();
    public void SaveFile();
}

public interface IContextC
{
    public void health(int dam);
    public int damage();

    public string Print();
    public void SaveFile();
}


public interface IContextA
{
    public void health(int dam);
    public int damage();

    public string Print();
    public void SaveFile();
}

public class Infantry: IContextI
{
    public int _health { get; set; } = 150;
   

    public int damage()
    {
        Random random = new Random();
        return random.Next(20, 50);
    }

    public void health(int dam)
    {
       this._health -= dam;
    }

    public string Print()
    {
        if (_health <= 0)
        {
            return "Infantry - died out";
        }
        return "Infantry - " + _health;
    }

    public void SaveFile()
    {
        string str = "";
        using (StreamReader read = new StreamReader("temp.txt"))
        {
             str = read.ReadToEnd();
        }
        using(StreamWriter writer = new StreamWriter("temp.txt"))
        {
            writer.WriteLine(str);
            writer.WriteLine(" ___ ");
            writer.WriteLine("Infantry - " + _health);
        }
    }
}
public class Cavalry: IContextC
{
    public int _health { get; set; } = 100;
    public int damage()
    {
        Random random = new Random();
        return random.Next(30, 40);
    }

    public void health(int dam)
    {
        this._health -= dam;
    }

    public string Print()
    {
        if (_health <= 0)
        {
            return "Cavalry - died out";
        }
        return "Cavalry - " + _health;
    }

    public void SaveFile()
    {
        string str = "";
        using (StreamReader read = new StreamReader("temp.txt"))
        {
            str = read.ReadToEnd();
        }
        using (StreamWriter writer = new StreamWriter("temp.txt"))
        {
            writer.WriteLine(str);
            writer.WriteLine(" ___ ");
            writer.WriteLine("Cavalry - " + _health);
        }
    }
}
public class Archers : IContextA
{
    public int _health { get; set; } = 50;
    
    public int damage()
    {
        Random random = new Random();
        if (random.Next(2) == 1)
            return random.Next(30, 110);
        else
            return 0;// Промах
    }

    public void health(int dam)
    {
        if (this._health <= 0)
        {
            _health = 0;
        }
        this._health -= dam;

        
    }

    public string Print()
    {
       if(_health <= 0)
        {
            return "Archers - died out";
        }
        return "Archers - " + _health;
    }

    public void SaveFile()
    {
        string str = "";
        using (StreamReader read = new StreamReader("temp.txt"))
        {
            str = read.ReadToEnd();
        }
        using (StreamWriter writer = new StreamWriter("temp.txt"))
        {
            writer.WriteLine(str);
            writer.WriteLine(" ___ ");
            writer.WriteLine("Archers - " + health);
        }
    }
}
public class Program
{
    private static Autofac.IContainer Container { get; set; }



    static void Main(string[] args)
    {

        ContainerBuilder builder = new ContainerBuilder();
        builder.RegisterType<Infantry>().As<IContextI>();
        builder.RegisterType<Cavalry>().As<IContextC>();
        builder.RegisterType<Archers>().As<IContextA>();
        
        Container = builder.Build();

        IContextI fightInf;
        IContextC fightCav;
        IContextA fightArc;
        using (var scope = Container.BeginLifetimeScope())
        {

            fightInf = scope.Resolve<IContextI>();
            fightCav = scope.Resolve<IContextC>();
            fightArc = scope.Resolve<IContextA>();
        }

        while (true)
        {
            Console.WriteLine("0 - Exit");
            Console.WriteLine("1 - Fight");
            Console.WriteLine("2 - Print");
            Console.WriteLine("3 - Save File");
            Console.WriteLine("4 - Restart");
            Console.Write("Enter__  ");
            int x = Convert.ToInt32(Console.ReadLine());
            Console.Clear();
            

            if (x == 0)
            {
                break;
            }
            else if(x == 1)
            {
              
                int infdam = fightInf.damage();
                int cavdam = fightCav.damage();
                int arcdam = fightArc.damage();

                fightInf.health(cavdam+arcdam);
                fightCav.health(infdam+arcdam);
                fightArc.health(infdam+cavdam);


                Console.WriteLine(fightInf.Print());
                Console.WriteLine(fightCav.Print());
                Console.WriteLine(fightArc.Print());

            }
            else if(x == 2)
            {
                Console.WriteLine(fightInf.Print());
                Console.WriteLine(fightCav.Print());
                Console.WriteLine(fightArc.Print());
            }
            else if(x == 3)
            {
                fightInf.SaveFile();
                fightCav.SaveFile();
                fightArc.SaveFile();
            }
            else if(x == 4)
            {
                builder = new ContainerBuilder();
                builder.RegisterType<Infantry>().As<IContextI>();
                builder.RegisterType<Cavalry>().As<IContextC>();
                builder.RegisterType<Archers>().As<IContextA>();
                Container = builder.Build();
                using (var scope = Container.BeginLifetimeScope())
                {
                    fightInf = scope.Resolve<IContextI>();
                    fightCav = scope.Resolve<IContextC>();
                    fightArc = scope.Resolve<IContextA>();
                }
            }
            else
            {
                Console.WriteLine("ERROR!");
                
            }

        }
        
    }
}