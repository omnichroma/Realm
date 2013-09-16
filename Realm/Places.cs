﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realm
{
    public class Place
    {
        public Random rand;
        public int q;
        public Globals globals = new Globals();
        public List<Enemy> enemylist = new List<Enemy>();

        protected virtual string GetDesc()
        {
            return "You are smack-dab in the middle of nowhere.";
        }
        public string Description
        {
            get { return GetDesc(); }
        }

        public virtual Enemy getEnemyList()
        {
            List<Enemy> templist = new List<Enemy>();
            templist.Add(new Goblin());
            templist.Add(new Slime());
            templist.Add(new Bandit());
            int randint = rand.Next(1, templist.Count + 1);

            return templist[randint - 1];
        }
        public virtual char[] getAvailableCommands()
        {
            List<char> templist = new List<char>();
            if (Globals.PlayerPosition.x > 0)
                templist.Add('w');
            if (Globals.PlayerPosition.x < Globals.map.GetUpperBound(0))
                templist.Add('e');
            if (Globals.PlayerPosition.y > 0)
                templist.Add('s');
            if (Globals.PlayerPosition.y < Globals.map.GetUpperBound(1))
                templist.Add('n');
            if (Main.Player.backpack.Count > 0)
                templist.Add('b');
            if (Main.hasmap)
                templist.Add('m');
            return templist.ToArray<char>();
        }

        private bool IsValid(char cmd)
        {
            bool IsFound = false;
            char[] cmdlist = getAvailableCommands();
            foreach (char c in cmdlist)
            {
                IsFound = c == cmd;
                if (IsFound)
                    break;
            }
            return IsFound;
        }

        public virtual bool handleInput(char input)
        {
            switch (input)
            {
                case 'n':
                    if (Globals.PlayerPosition.y < Globals.map.GetUpperBound(1))
                        Globals.PlayerPosition.y += 1;
                    break;
                case 'e':
                    if (Globals.PlayerPosition.x < Globals.map.GetUpperBound(0))
                        Globals.PlayerPosition.x += 1;
                    break;
                case 's':
                    if (Globals.PlayerPosition.y > 0)
                        Globals.PlayerPosition.y -= 1;
                    break;
                case 'w':
                    if (Globals.PlayerPosition.x > 0)
                        Globals.PlayerPosition.x -= 1;
                    break;
                case 'b':
                    if (Main.Player.backpack.Count > 0)
                        Main.BackpackLoop();
                    break;
                case 'm':
                    if (Main.hasmap)
                        Formatting.drawmap();
                    break;
                default:
                    return false;
            }
            return true;
        }
        public Place()
        {
            rand = new Random();
        }
    }

    public class WKingdom : Place
    {
        protected override string GetDesc()
        {
            return "King: 'Come, " + Main.Player.name + ". I plan give you the ultimate gift, eternal respite. You're not sure why he has called you but you don't like it. The Western King approaches and unsheathes his blade emitting a strong aura of  bloodlust. Fight(f) or run(r)?";
        }

        public override Enemy getEnemyList()
        {
            return new Enemy();
        }
        public override char[] getAvailableCommands()
        {
            return new char[] { 'f', 'r' };
        }

        public override bool handleInput(char input)
        {
            switch (input)
            {
                case 'f':
                    Main.BattleLoop(new WesternKing());
                    break;
                case 'r':
                    Formatting.type("You escaped the Western King, but you're pretty damn lost now.");
                    Globals.PlayerPosition.y -= 2;
                    Main.MainLoop();
                    break;
                default:
                    return false;
            }
            return true;
        }
    }

    public class IllusionForest : Place
    {
        protected override string GetDesc()
        {
            return "You find yourself in a forest, that bizarrely appears to wrap itself around you, like a fun house mirror. You are more lost than that time you were on a road trip and your phone died so you had no GPS. Do you want to travel east(e), north(n), backpack(b), or search the area(z)?";
        }
        public override Enemy getEnemyList()
        {
            return new Slime();
        }
        public override char[] getAvailableCommands()
        {
            List<char> templist = new List<char>();
            if (Main.Player.backpack.Count >= 1)
                templist.Add('b');
            if (Main.hasmap)
                templist.Add('m');
            if (Globals.PlayerPosition.x > 0)
                templist.Add('w');
            if (Globals.PlayerPosition.x < Globals.map.GetUpperBound(0))
                templist.Add('e');
            if (Globals.PlayerPosition.y > 0)
                templist.Add('s');
            if (Globals.PlayerPosition.y < Globals.map.GetUpperBound(1))
                templist.Add('n');
            templist.Add('n');
            templist.Add('z');
            return templist.ToArray<char>();
        }

        public override bool handleInput(char input)
        {
            switch (input)
            {
                case 'm':
                    if (Main.hasmap)
                        Formatting.drawmap();
                    break;
                case 'n':
                    Globals.PlayerPosition.y += 1;
                    break;
                case 'e':
                    Globals.PlayerPosition.x += 1;
                    break;
                case 's':
                    Globals.PlayerPosition.y -= 1;
                    break;
                case 'z':
                    if (Main.forrestcounter == 0)
                    {
                        Formatting.type("You decide to look around. You find a trail leading to a clearing. Once in the  clearing, you see a suit of cardboard armor held together with duct tape, a refrigerator box, and a cardboad tube. Pick them up? Your current commands are y, n");
                        Formatting.type("");
                        char tempinput = Console.ReadKey().KeyChar;
                        switch (tempinput)
                        {
                            case 'y':
                                Formatting.type("Amid the -trash- gear on the ground, you find a tile with the letter 'G' on it.");
                                Main.Player.backpack.Add(globals.cardboard_armor);
                                Formatting.type("Obtained 'Cardboard Armor'!");
                                Main.Player.backpack.Add(globals.cardboard_sword);
                                Formatting.type("Obtained 'Cardboard Shield'!");
                                Main.Player.backpack.Add(globals.cardboard_shield);
                                Formatting.type("Obtained 'Cardboard Shield'!");
                                Main.forrestcounter++;
                                break;
                            case 'n':
                                Formatting.type("\r\nLoser.");
                                break;
                        }
                    }
                    else
                    {
                        Formatting.type("You've already been here!");
                    }
                    break;
                case 'b':
                    Main.BackpackLoop();
                    break;
                default:
                    return false;
            }
            return true;
        }
    }

    public class Riverwell : Place
    {
        protected override string GetDesc()
        {
            return "You come across a river town centered around a massive well full of magically enriched electrolyte water. And the good stuff. Do you want to go north(n), south(s), east(e), west(w), backpack(b), or visit the town(v)";
        }
        public override char[] getAvailableCommands()
        {
            List<char> templist = new List<char>();
            if (Main.Player.backpack.Count >= 1)
                templist.Add('b');
            if (Main.hasmap)
                templist.Add('m');
            if (Globals.PlayerPosition.x > 0)
                templist.Add('w');
            if (Globals.PlayerPosition.x < Globals.map.GetUpperBound(0))
                templist.Add('e');
            if (Globals.PlayerPosition.y > 0)
                templist.Add('s');
            if (Globals.PlayerPosition.y < Globals.map.GetUpperBound(1))
                templist.Add('n'); templist.Add('v');
            return templist.ToArray<char>();
        }
        public override Enemy getEnemyList()
        {
            List<Enemy> templist = new List<Enemy>();
            templist.Add(new Goblin());

            Random rand = new Random();
            int randint = rand.Next(0, templist.Count + 1);

            return templist[randint];
        }
        public override bool handleInput(char input)
        {
            switch (input)
            {
                case 'm':
                    if (Main.hasmap)
                        Formatting.drawmap();
                    break;
                case 'n':
                    Globals.PlayerPosition.y += 1;
                    break;
                case 'e':
                    Globals.PlayerPosition.x += 1;
                    break;
                case 'w':
                    Globals.PlayerPosition.x -= 1;
                    break;
                case 's':
                    Globals.PlayerPosition.y -= 1;
                    break;
                case 'v':
                    Formatting.type("There is an inn(i) and an arms dealer(a). Or you can investigate the well(w).");
                    Formatting.type("");
                    char tempinput = Console.ReadKey().KeyChar;
                    switch (tempinput)
                    {
                        case 'i':
                            Formatting.type("Innkeep: \"It will cost you 3 gold. Are you sure?\"(y/n)");
                            char _tempinput = Console.ReadKey().KeyChar;
                            switch (_tempinput)
                            {
                                case 'y':
                                    if (Main.Purchase(3))
                                    {
                                        Formatting.type("Your health has been fully restored, although you suspect you have lice.");
                                        Main.Player.hp = Main.Player.maxhp;
                                    }
                                    break;
                                case 'n':
                                    Formatting.type("You leave the inn.");
                                    break;
                            }
                            break;
                        case 'a':
                            Formatting.type("You visit the arms dealer. He has naught to sell but a wooden staff and a plastic ring. Buy both for 11 gold? (y/n)");
                            char __tempinput = Console.ReadKey().KeyChar;
                            switch (__tempinput)
                            {
                                case 'y':
                                    if (Main.Purchase(11, globals.wood_staff))
                                    {
                                        Formatting.type("Obtained 'Wood Staff'!");
                                        Main.Player.backpack.Add(globals.plastic_ring);
                                        Formatting.type("Obtained 'Plastic Ring'!");
                                        Formatting.type("On the inside of the ring, the letter 'o' is embossed.");
                                        Formatting.type("You buy the staff and the ring. He grins, and you know you've been ripped off.");
                                    }
                                    break;
                                case 'n':
                                    Formatting.type("You leave the shop.");
                                    break;
                            }
                            break;
                        case 'w':
                            Formatting.type("Do you want to look inside the well?(y/n)");
                            char ___tempinput = Console.ReadKey().KeyChar;
                            switch (___tempinput)
                            {
                                case 'y':
                                    Formatting.type("You fall in and drown.");
                                    End.IsDead = true;
                                    End.GameOver();
                                    break;
                                case 'n':
                                    Formatting.type("You leave.");
                                    break;
                            }
                            break;
                    }
                    break;
                case 'b':
                    Main.BackpackLoop();
                    break;
                default:
                    return false;
            }
            return true;
        }
    }

    public class Seaport : Place
    {
        protected override string GetDesc()
        {
            return "You arrive at a seaside port bustling with couriers and merchants. Do you want to go to the arms dealer(a), the library(l), or inn(i)?";
        }
        public override Enemy getEnemyList()
        {
            List<Enemy> templist = new List<Enemy>();
            templist.Add(new Goblin());
            templist.Add(new Slime());
            templist.Add(new Bandit());

            Random rand = new Random();
            int randint = rand.Next(0, templist.Count + 1);

            return templist[randint];
        }
        public override char[] getAvailableCommands()
        {
            List<char> templist = new List<char>();
            if (Main.Player.backpack.Count >= 1)
                templist.Add('b');
            if (Main.hasmap)
                templist.Add('m');
            if (Globals.PlayerPosition.x > 0)
                templist.Add('w');
            if (Globals.PlayerPosition.x < Globals.map.GetUpperBound(0))
                templist.Add('e');
            if (Globals.PlayerPosition.y > 0)
                templist.Add('s');
            if (Globals.PlayerPosition.y < Globals.map.GetUpperBound(1))
                templist.Add('n'); templist.Add('a');
            templist.Add('l');
            templist.Add('i');
            return templist.ToArray<char>();
        }
        public override bool handleInput(char input)
        {
            switch (input)
            {
                case 'm':
                    if (Main.hasmap)
                        Formatting.drawmap();
                    break;
                case 'n':
                    Globals.PlayerPosition.y += 1;
                    break;
                case 'e':
                    Globals.PlayerPosition.x += 1;
                    break;
                case 's':
                    Globals.PlayerPosition.y -= 1;
                    break;
                case 'i':
                    Formatting.type("Innkeep: \"It will cost you 5 gold. Are you sure?\"(y/n)");
                    char _tempinput = Console.ReadKey().KeyChar;
                    switch (_tempinput)
                    {
                        case 'y':
                            if (Main.Purchase(3))
                            {
                                Formatting.type("Your health has been fully restored, although the matress was as hard as stale crackers and your back kinda hurts.");
                                Formatting.type("As you're leaving the inn, you notice the letter 'd' in the coffee grounds from the espresso you had that morning.");
                                Main.Player.hp = Main.Player.maxhp;
                            }
                            break;
                        case 'n':
                            Formatting.type("You leave the inn.");
                            break;
                    }
                    break;
                case 'a':
                    Formatting.type("You visit the arms dealer. He's out of stock except for an iron lance(l, $15) and an iron buckler(b, $10). (n to leave)");
                    char __tempinput = Console.ReadKey().KeyChar;
                    switch (__tempinput)
                    {
                        case 'l':
                            if (Main.Purchase(15, globals.iron_lance))
                            {
                                Formatting.type("It's almost as if he doesn't even see you.");
                                Formatting.type("Obtained 'Iron Lance'!");
                            }
                            break;
                        case 'b':
                            if (Main.Purchase(10, globals.iron_buckler))
                            {
                                Formatting.type("It's almost as if he doesn't even see you.");
                                Formatting.type("Obtained 'Iron Buckler'!");
                            }
                            break;
                        case 'n':
                            Formatting.type("You leave.");
                            break;
                        default:
                            break;
                    }
                    break;
                case 'l':
                    if (Main.libcounter == 0)
                    {
                        Formatting.type("You see a massive building with columns the size of a house. This is obivously the town's main attraction. Nerds are streaming in and out like a river. You try to go inside, but you're stopped at the door. The enterance fee is 3 g. Pay? (y/n)");
                        char ___tempinput = Console.ReadKey().KeyChar;
                        switch (___tempinput)
                        {
                            case 'y':
                                if (Main.Purchase(3))
                                {
                                    Formatting.type("You enter the library. Before you lays a vast emporium of knowledge. You scratch your butt, then look for some comic books or something. 3 books catch your eye. 'The Wizard's Lexicon'(a), 'The Warrior's Code'(b), and 'Codex Pallatinus'(c). Which do you read?");
                                    switch (Console.ReadKey().KeyChar)
                                    {
                                        case 'a':
                                            Formatting.type("You read of the maegi of old. As you flip through, something catches your eye. You see what looks to be ancient writing, but you somehow understand it.");
                                            Main.Player.abilities.AddCommand(new Combat.EnergyOverload("Energy Overload", 'e'));
                                            Formatting.type("Learned 'Energy Overload!'");
                                            break;
                                        case 'b':
                                            Formatting.type("You pore over the pages, and see a diagram of an ancient technique, lost to the ages.");
                                            Main.Player.abilities.AddCommand(new Combat.BladeDash("Blade Dash", 'd'));
                                            Formatting.type("Learned 'Blade Dash'!");
                                            break;
                                        case 'c':
                                            Formatting.type("You squint your eyes to see the tiny text. This tome convinces you of the existence of Lord Luxferre, the Bringer of Light. The book teaches you the importance of protecting others.");
                                            Main.Player.abilities.AddCommand(new Combat.HolySmite("Holy Smite", 'h'));
                                            break;
                                    }
                                }
                                break;
                            case 'n':
                                Formatting.type("You leave.");
                                break;
                        }
                        Main.libcounter++;
                    }
                    else
                        Formatting.type("The library is closed.");
                    break;
                case 'b':
                    Main.BackpackLoop();
                    break;
                default:
                    return false;
            }
            return true;
        }
    }
    public class Valleyburg : Place
    {
        protected override string GetDesc()
        {
            return "You arrive at a small town just east of the Western Kingdom. Do you want to visit the town(v) or head to the inn(i)?";
        }
        public override Enemy getEnemyList()
        {
            List<Enemy> templist = new List<Enemy>();
            templist.Add(new Goblin());
            templist.Add(new Slime());
            templist.Add(new Bandit());

            Random rand = new Random();
            int randint = rand.Next(0, templist.Count + 1);

            return templist[randint];
        }
        public override char[] getAvailableCommands()
        {
            List<char> templist = new List<char>();
            if (Main.Player.backpack.Count >= 1)
                templist.Add('b');
            if (Main.hasmap)
                templist.Add('m');
            if (Globals.PlayerPosition.x > 0)
                templist.Add('w');
            if (Globals.PlayerPosition.x < Globals.map.GetUpperBound(0))
                templist.Add('e');
            if (Globals.PlayerPosition.y > 0)
                templist.Add('s');
            if (Globals.PlayerPosition.y < Globals.map.GetUpperBound(1))
                templist.Add('n'); 
            templist.Add('i');
            templist.Add('v');
            return templist.ToArray<char>();
        }
        public override bool handleInput(char input)
        {
            switch (input)
            {
                case 'm':
                    if (Main.hasmap)
                        Formatting.drawmap();
                    break;
                case 'n':
                    Globals.PlayerPosition.y += 1;
                    break;
                case 'e':
                    Globals.PlayerPosition.x += 1;
                    break;
                case 's':
                    Globals.PlayerPosition.y -= 1;
                    break;
                case 'w':
                    Globals.PlayerPosition.x -= 1;
                    break;
                case 'i':
                    Formatting.type("Innkeep: \"It will cost you 5 gold. Are you sure?\"(y/n)");
                    char _tempinput = Console.ReadKey().KeyChar;
                    switch (_tempinput)
                    {
                        case 'y':
                            if (Main.Purchase(3))
                            {
                                Formatting.type("Your health has been fully restored, although the matress smelled of mildew, and so do your clothes.");
                                Main.Player.hp = Main.Player.maxhp;
                            }
                            break;
                        case 'n':
                            Formatting.type("You leave the inn.");
                            break;
                    }
                    break;
                case 'v':
                    Formatting.type("You visit the town. You may choose to visit with the townsfolk(t), or head to the artificer(a).");
                    char __tempinput = Console.ReadKey().KeyChar;
                    switch (__tempinput)
                    {
                        case 'a':
                            Formatting.type("The artificer has some magically charged rings for sale. Buy one for 20? (y/n)");
                            switch (Console.ReadKey().KeyChar)
                            {
                                case 'y':
                                    if (Main.Purchase(15, globals.iron_band))
                                    {
                                        Formatting.type("He smiles weakly and thanks you.");
                                        Formatting.type("Obtained 'Iron Band'!");
                                    }
                                    break;
                                case 'n':
                                    Formatting.type("You leave.");
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case 't':
                            Formatting.type("You talk to a villager. He muses about the fact that sometimes, reality doens't feel real at all. Puzzled by his comment, you walk away.");
                            Formatting.type("A paper flaps out of his cloack as he walks away. On it is nothing but the letter 'd'.");
                            break;
                        default:
                            break;
                    }
                    break;
                case 'b':
                    Main.BackpackLoop();
                    break;
                default:
                    return false;
            }
            return true;
        }
    }

    public class NMtns : Place
    {
        protected override string GetDesc()
        {
            return "You find yourself at the foot of a mountain. There is a village not far off do you wish to go there?(y to enter)";
        }
        public override Enemy getEnemyList()
        {
            List<Enemy> templist = new List<Enemy>();
            templist.Add(new Goblin());
            templist.Add(new Bandit());

            Random rand = new Random();
            int randint = rand.Next(0, templist.Count + 1);

            return templist[randint];
        }
        public override char[] getAvailableCommands()
        {
            List<char> templist = new List<char>();
            if (Main.Player.backpack.Count >= 1)
                templist.Add('b');
            if (Main.hasmap)
                templist.Add('m');
            if (Globals.PlayerPosition.x > 0)
                templist.Add('w');
            if (Globals.PlayerPosition.x < Globals.map.GetUpperBound(0))
                templist.Add('e');
            if (Globals.PlayerPosition.y > 0)
                templist.Add('s');
            if (Globals.PlayerPosition.y < Globals.map.GetUpperBound(1))
                templist.Add('n'); 
            templist.Add('y');
            return templist.ToArray<char>();
        }
        public override bool handleInput(char input)
        {
            switch (input)
            {
                case 'm':
                    if (Main.hasmap)
                        Formatting.drawmap();
                    break;
                case 'n':
                    Globals.PlayerPosition.y += 1;
                    break;
                case 'e':
                    Globals.PlayerPosition.x += 1;
                    break;
                case 's':
                    Globals.PlayerPosition.y -= 1;
                    break;
                case 'w':
                    Globals.PlayerPosition.x -= 1;
                    break;
                case 'y':
                    Formatting.type("You see a library(l) and a weaponsmith(w). Which do you wish to enter?");
                    switch (Console.ReadKey().KeyChar)
                    {
                        case 'l':
                            if (Main.ramseycounter == 0)
                            {
                                Formatting.type("There are three books. Do you wish to read Climbing Safety (c), Solomon's Answer (s), or Gordon Ramsay: A Biology (g)");
                                switch (Console.ReadKey().KeyChar)
                                {
                                    case 'c':
                                        Formatting.type("Climbing mountains requires absolute safety. Rule #1: Don't Fall(.....this book seems pretty thick. Do you wish to continue reading?(y/n))");
                                        switch (Console.ReadKey().KeyChar)
                                        {
                                            case 'y':
                                                Formatting.type("As you silently become informed about safety, you notice that a segment of the wall is opening itself up to your far right. Do you wish to enter?(y/n)");
                                                switch (Console.ReadKey().KeyChar)
                                                {
                                                    case 'y':
                                                        Formatting.type("You try to enter, but the door requires a password.");
                                                        if (Console.ReadLine() == "Don't Fall")
                                                        {
                                                            Formatting.type(" You open the door to find a dark room with a suspicious figure conducting suspicious rituals. The man looks flustered and says 'Nice day isn't it?'. As you wonder what anyone would be doing in such a dark room, the man edges his way to the entrance and dashes outside. He forgot to take his book with him. Do you wish to take it? (y/n) ");
                                                            switch (Console.ReadKey().KeyChar)
                                                            {
                                                                case 'y':
                                                                    Main.Player.abilities.AddCommand(new Combat.ConsumeSoul("Consume Soul", 'e'));
                                                                    Formatting.type(" Learned 'Consume Soul'!");
                                                                    break;
                                                                case 'n':
                                                                    Formatting.type("You remember that you are an exemplary member of society and that you will by no means touch another's belongings without their consent. You leave the room like the good man you are.");
                                                                    break;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Formatting.type("Password incorrect.");
                                                            break;
                                                        }
                                                        break;
                                                    case 'n':
                                                        Formatting.type("You did not see anything out of the ordinary. You have never seen anything out of the ordinary. As you leave you makes sure to shut the door behind you because you did not see anything out of the ordinary.");
                                                        break;
                                                }
                                                break;
                                            case 'n':
                                                Formatting.type("You believe that you already know enough about safety. Put the book back in it's spot in the bookshelf?(y/n)");
                                                switch (Console.ReadKey().KeyChar)
                                                {
                                                    case 'y':
                                                        Formatting.type("You insert the book back in it's righteous position. You feel good about doing a good deed.");
                                                        break;
                                                    case 'n':
                                                        Main.Player.hp -= 2;
                                                        Formatting.type("You are trash. You are the pondscum of society. Repent and pay with your life. You take 2 damage.");
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                    case 's':
                                        Formatting.type("You don't know how to read this language, however you do find a crumpled up map of the realm in the back of the book.");
                                        Formatting.type("Obtained 'Map'!");
                                        Main.hasmap = true;
                                        break;
                                    case 'g':
                                        Main.Player.intl += 1;
                                        Formatting.type("You are touched by the art of cooking. Being forged in the flame of cooking, your ability to think up vicious insults has improved. Your intelligence has improved a little");
                                        Main.gbooks++;
                                        break;
                                }
                                Main.ramseycounter++;
                                break;
                            }
                            else
                            {
                                Formatting.type("The library is closed, but you find a signed version of Gordon Ramsey's book.");
                                break;
                            }
                        case 'w':
                            Formatting.type("You realize that you don't speak the same language as the shopkeeper. You take all of his peppermint candy and leave.");
                            Formatting.type("Before you toss it on the floor like the scumbad you are, you notice one of the candy wrappers has an apostrphe on the inside.");
                            break;
                    }
                    break;
                case 'b':
                    Main.BackpackLoop();
                    break;
                default:
                    return false;
            }
            return true;
        }
    }

    public class NKingdom : Place
    {
        protected override string GetDesc()
        {
            return "You have arrived at the gate of a castle. There are many people passing through the gate. You see the royal library(l), a smithy's guild(g), the north castle(c), and the marketplace(p). Where do you wish to go?";
        }
        public override Enemy getEnemyList()
        {
            List<Enemy> templist = new List<Enemy>();
            templist.Add(new Goblin());
            templist.Add(new Bandit());

            Random rand = new Random();
            int randint = rand.Next(0, templist.Count + 1);

            return templist[randint];
        }
        public override char[] getAvailableCommands()
        {
            List<char> templist = new List<char>();
            if (Main.Player.backpack.Count >= 1)
                templist.Add('b');
            if (Main.hasmap)
                templist.Add('m');
            if (Globals.PlayerPosition.x > 0)
                templist.Add('w');
            if (Globals.PlayerPosition.x < Globals.map.GetUpperBound(0))
                templist.Add('e');
            if (Globals.PlayerPosition.y > 0)
                templist.Add('s');
            if (Globals.PlayerPosition.y < Globals.map.GetUpperBound(1))
                templist.Add('n');
            templist.Add('l');
            templist.Add('g');
            templist.Add('n');
            templist.Add('m');
            return templist.ToArray<char>();
        }
        public override bool handleInput(char input)
        {
            switch (input)
            {
                case 'm':
                    if (Main.hasmap)
                        Formatting.drawmap();
                    break;
                case 'n':
                    Globals.PlayerPosition.y += 1;
                    break;
                case 'e':
                    Globals.PlayerPosition.x += 1;
                    break;
                case 's':
                    Globals.PlayerPosition.y -= 1;
                    break;
                case 'w':
                    Globals.PlayerPosition.x -= 1;
                    break;
                case 'l':
                    if (Main.nlibcounter == 0)
                    {
                        Formatting.type("In the royal library you find 'Crescent Path'(c), 'Tale of Sariel'(t), 'History of The Realm'(h), and Gordon Ramsey: A Geology(g). Which do you wish to read?");
                        switch (Console.ReadKey().KeyChar)
                        {
                            case 'c':
                                Formatting.type(" The book reads 'In the land of the central sands.......(This book is incredibly long. If you wish to complete this book, it may cost you.");
                                switch (Console.ReadKey().KeyChar)
                                {
                                    case 'y':
                                        Main.Player.hp -= 1;
                                        Main.Player.spd += 3;
                                        Formatting.type("You are exhausted from finishing the book, but you feel like your speed has increased a little.");
                                        break;
                                    case 'n':
                                        Formatting.type("You decide that completing this book is not worth the time. Do you wish to put the book back in it's original spot on the bookshelf?(y/n)");
                                        switch (Console.ReadKey().KeyChar)
                                        {
                                            case 'y':
                                                Formatting.type("You place the book back in it's original position. You daydream about a perfect society where all of mankind would put books back in bookshelves.");
                                                break;
                                            case 'n':

                                                Formatting.type("Are you the trash of society?");
                                                switch (Console.ReadKey().KeyChar)
                                                {
                                                    case 'y':
                                                        Main.Player.hp -= 3;
                                                        Formatting.type("You lose 3 hp. Dirtbag.");
                                                        break;
                                                    case 'n':
                                                        Formatting.type("You place the book back in the bookshelf.");
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                }
                                break;
                            case 't':
                                Formatting.type("You read of the exploits of the ancient hero Sariel, and his philosophies of protecting others.");
                                Formatting.type("Learned 'Dawnstrike'!");
                                Main.Player.abilities.AddCommand(new Combat.Dawnstrike("Dawnstrike", 't'));
                                break;
                            case 'h':
                                Formatting.type("You read 5000 pages on the history of the realm. You're not sure why you did that, but you feel smarter.");
                                Main.Player.intl += 2;
                                break;
                            case 'g':
                                Formatting.type("You learn of the layers of granite and basalt on Planet Ramsey.");
                                Main.Player.def += 1;
                                Main.gbooks++;
                                break;
                        }
                        Main.nlibcounter++;
                    }
                    else
                    {
                        Formatting.type("The library is closed.");
                        break;
                    }
                    break;
                case 'g':
                    Formatting.type("The Smith's Guild only has their cumulative project for sale. IT's a masterpeice, but very expensive. Buy Bloodmail for 50 gold? (y/n)");
                    switch(Console.ReadKey().KeyChar)
                    {
                        case 'y':
                            break;
                        case 'n':
                            break;
                    }
                    break;
                case 'c':
                    break;
                case 'p':
                    break;
                case 'b':
                    Main.BackpackLoop();
                    break;
                default:
                    return false;
            }
            return true;
        }
    }

    public class CentralKingdom : Place
    {
        protected override string GetDesc()
        {
            return "You step through the gates of the city, and witness the largest coalescence of humanity you've seen in you entire life. There are hundereds of great wonders in this behemoth of a city. You may visit the library(l), the weaponsmith(a), the inn(i), the magic shop(q), or the town's great monument(o)";
        }
        public override Enemy getEnemyList()
        {
            List<Enemy> templist = new List<Enemy>();
            templist.Add(new Goblin());
            templist.Add(new Bandit());

            Random rand = new Random();
            int randint = rand.Next(0, templist.Count + 1);

            return templist[randint];
        }
        public override char[] getAvailableCommands()
        {
            List<char> templist = new List<char>();
            if (Main.Player.backpack.Count >= 1)
                templist.Add('b');
            if (Main.hasmap)
                templist.Add('m');
            if (Globals.PlayerPosition.x > 0)
                templist.Add('w');
            if (Globals.PlayerPosition.x < Globals.map.GetUpperBound(0))
                templist.Add('e');
            if (Globals.PlayerPosition.y > 0)
                templist.Add('s');
            if (Globals.PlayerPosition.y < Globals.map.GetUpperBound(1))
                templist.Add('n'); 
            templist.Add('a');
            templist.Add('l');
            templist.Add('i');
            templist.Add('q');
            templist.Add('o');
            return templist.ToArray<char>();
        }
        public override bool handleInput(char input)
        {
            switch (input)
            {
                case 'm':
                    if (Main.hasmap)
                        Formatting.drawmap();
                    break;
                case 'n':
                    Globals.PlayerPosition.y += 1;
                    break;
                case 'e':
                    Globals.PlayerPosition.x += 1;
                    break;
                case 's':
                    Globals.PlayerPosition.y -= 1;
                    break;
                case 'w':
                    Globals.PlayerPosition.x -= 1;
                    break;

                case 'l':
                    if (Main.centrallibcounter == 0)
                    {
                        Formatting.type("This massive building is a monument to human knowledge. You feel dwarfed by its towering presence.");
                        Formatting.type("You arrive at a shelf that draws your attention. There are five books. The first one is a musty tome entitled Alcwyn's Legacy(a). The second one is A Guide to Theivery(g), the third Sacrificial Rite(s). The fourth book is The Void(v). The final book's title Ramsey: A Mathematics(r). Which do you read?");
                        switch (Console.ReadKey().KeyChar)
                        {
                            case 'a':
                                Formatting.type("You become enlightened in the ways of the elder wizard Alcywn.");
                                Formatting.type("Learned 'Curse'!");
                                Main.Player.abilities.AddCommand(new Combat.Curse("Curse", 'c'));
                                
                                break;
                            case 'g':
                                Formatting.type("Now skilled in the art of stealing, you gain 10% more gold.");
                                Main.is_theif = true;
                                break;
                            case 's':
                                Formatting.type("You become skilled in the art of sacrifice.");
                                Formatting.type("Learned 'Sacrifice'!");
                                Main.Player.abilities.AddCommand(new Combat.Sacrifice("Sacrifice", 's'));
                                break;
                            case 'v':
                                Formatting.type("You learn of the Void. You can phase in and out of reality.");
                                Formatting.type("Learned 'Phase'!");
                                Main.Player.abilities.AddCommand(new Combat.Phase("Phase", 'p'));
                                break;
                            case 'r':
                                Formatting.type("You become educated on the mathematics of cooking.");
                                Main.Player.intl += 1;
                                Main.gbooks++;
                                break;
                        }
                        Main.ramseycounter++;
                        break;
                    }
                    else
                    {
                        Formatting.type("The library is closed.");
                        break;
                    }
                case 'a':
                    Formatting.type("You approach a building with a sigil bearing crossed swords. You suspect this is the weaponsmith. You enter, and he has loads of goodies for sale. Buy Iron Rapier(r, 30), Iron Chainmail(c, 30), Iron Buckler (b, 25), or Bloodthirsty Longsword(l, 50)?");
                    switch(Console.ReadKey().KeyChar)
                    {
                        case 'r':
                            Main.Purchase(30, globals.iron_rapier);
                            Formatting.type("Obtained 'Iron Rapier'!");
                            break;
                        case 'c':
                            Main.Purchase(30, globals.iron_mail);
                            Formatting.type("Obtained 'Iron Chainmail!'!");
                            break;
                        case 'b':
                            Main.Purchase(25, globals.iron_buckler);
                            Formatting.type("Obtained 'Iron Buckler'!");
                            break;
                        case 'l':
                            Main.Purchase(50, globals.bt_longsword);
                            Formatting.type("Obatined 'Bloodthirsty Longsword'!");
                            break;
                        default:
                            break;
                    }
                    break;
                case 'q':
                    if (Main.magiccounter == 0)
                    {
                        Formatting.type("You arrive at a shifty magic dealer in a back alley. He offers to sell you a Blood Amulet(b, 50), a new ability(a, 50), or sell you a secret (s, 50).");
                        switch (Console.ReadKey().KeyChar)
                        {
                            case 'b':
                                Main.Purchase(50, globals.blood_amulet);
                                Formatting.type("Obtained 'Blood Amulet'!");
                                break;
                            case 'a':
                                Main.Purchase(50);
                                Formatting.type("He holds out his hand, and reality appears to bend around it. Kind of like the Degauss button on monitors from the 90's.");
                                Main.Player.abilities.AddCommand(new Combat.VorpalBlades("Vorbal Blades", 'v'));
                                Formatting.type("Learned 'Vorpal Blade'!");
                                break;
                            case 's':
                                Main.Purchase(50);
                                Formatting.type("'I have a secret to tell you. Everything you know is wrong.' He holds out his hand, and reality appears to bend around it. Kind of like the Degauss button on monitors from the 90's. 'See this?' he says. 'This is what's known as the Flux. Everything is from the Flux, and controlled by the Flux. Learn to control it, and you control reality.'. he dissapears througha shimmering portal and leaves you there mystified.");
                                break;
                        }
                        Main.magiccounter++;
                        break;
                    }
                    else
                    {
                        Formatting.type("You look for the magic man, but he's gone.");
                        break;
                    }
                case 'b':
                    Main.BackpackLoop();
                    break;
                case 'i':
                    Formatting.type("The sign above the inn reads 'Donaldius Trump'. Stay the night for 15 gold? (y/n)");
                    switch(Console.ReadKey().KeyChar)
                    {
                        case 'y':
                            Formatting.type("You feel refreshed, however the bedsheets smelled like cold blooded capitalism and weasely politicians.");
                            Main.Purchase(15);
                            Main.Player.hp = Main.Player.maxhp;
                            break;
                        case 'n':
                            Formatting.type("You leave the posh hotel.");
                            break;
                        default:
                            break;
                    }
                    break;
                case 'o':
                    Formatting.type("You visit the city's monument, which is a tourist attraction for the entire Realm. People are everywhere. You elbow your way through the crowd to get a better look. The monument is a massive sword, stabbed into the ground as if placed there by a giant god. Blue light is pulsing up it like a giant conduit. You spot a secret door in the blade of the sword. Enter (y/n)");
                    switch(Console.ReadKey().KeyChar)
                    {
                        case 'y':
                            Formatting.type("The door requires a password.");
                            if (Console.ReadLine() == "God's Death")
                                Main.Endgame();
                            else
                                Formatting.type("The door remains shut.");
                            break;
                        case 'n':
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    return false;
            }
            return true;
        }
    }
    public class SKingdom : Place
    {
        protected override string GetDesc()
        {
            return "You step through the gates of the southernmost kingdom in the land. The gates are still scarred from the Invasion. The library is a smoking ruin. You may go to the arms dealer(a), the residential district(r) or the inn(i)";
        }
        public override Enemy getEnemyList()
        {
            List<Enemy> templist = new List<Enemy>();
            templist.Add(new Goblin());
            templist.Add(new Bandit());

            Random rand = new Random();
            int randint = rand.Next(0, templist.Count + 1);

            return templist[randint];
        }
        public override char[] getAvailableCommands()
        {
            List<char> templist = new List<char>();
            if (Main.Player.backpack.Count >= 1)
                templist.Add('b');
            if (Main.hasmap)
                templist.Add('m');
            if (Globals.PlayerPosition.x > 0)
                templist.Add('w');
            if (Globals.PlayerPosition.x < Globals.map.GetUpperBound(0))
                templist.Add('e');
            if (Globals.PlayerPosition.y > 0)
                templist.Add('s');
            if (Globals.PlayerPosition.y < Globals.map.GetUpperBound(1))
                templist.Add('n');
            templist.Add('a');
            templist.Add('l');
            templist.Add('i');
            templist.Add('q');
            templist.Add('o');
            return templist.ToArray<char>();
        }
        public override bool handleInput(char input)
        {
            switch (input)
            {
                case 'm':
                    if (Main.hasmap)
                        Formatting.drawmap();
                    break;
                case 'n':
                    Globals.PlayerPosition.y += 1;
                    break;
                case 'e':
                    Globals.PlayerPosition.x += 1;
                    break;
                case 'w':
                    Globals.PlayerPosition.x -= 1;
                    break;
                case 'a':
                    Formatting.type("The arms dealer has a small stand, but his wares are valuable. You may buy Darksteel Amulet(100, a), Darksteel Kris(80, k), Darksteel Kite Shield(s, 100), or Darksteel Scalemail(c, 110).");
                    switch (Console.ReadKey().KeyChar)
                    {
                        case 'a':
                            Main.Purchase(100, globals.ds_amulet);
                            Formatting.type("Obtained 'Darksteel Amulet'!");
                            break;
                        case 'c':
                            Main.Purchase(110, globals.ds_scale);
                            Formatting.type("Obatined 'Darksteel Scalemail'!");
                            break;
                        case 'k':
                            Main.Purchase(80, globals.ds_kris);
                            Formatting.type("Obtained 'Darksteel Kris!'!");
                            break;
                        case 's':
                            Main.Purchase(100, globals.ds_kite);
                            Formatting.type("Obtained 'Darksteel Kite Shield'!");
                            break;
                        default:
                            break;
                    }
                    break;

                case 'b':
                    Main.BackpackLoop();
                    break;
                case 'i':
                    Formatting.type("The inn is burned, but still in use. Stay the night for 20 gold? (y/n)");
                    switch (Console.ReadKey().KeyChar)
                    {
                        case 'y':
                            Formatting.type("You feel refreshed, although smelling of ash.");
                            Main.Purchase(20);
                            Main.Player.hp = Main.Player.maxhp;
                            break;
                        case 'n':
                            Formatting.type("You leave the inn.");
                            break;
                        default:
                            break;
                    }
                    break;
                case 'r':
                    Formatting.type("You visit the house of the jobless former librarian. He says there is something very strange about that sword monument in Central. He says you can never go anywhere unarmed. He teaches you a new ability.");
                    Formatting.type("Learned 'Incinerate'!");
                    Main.Player.abilities.AddCommand(new Combat.Incinerate("Incinerate", 'i'));
                    Formatting.type("As you're leaving, you notice the letter 'l' burned into the doorknob.");
                    break;
                default:
                    return false;
            }
            return true;
        }
    }

    public class Newport : Place
    {

    }

    public class Nomad : Place
    {

    }

    public class EKingdom : Place
    {

    }

    public class TwinPaths : Place
    {

    }

    public class Ravenkeep : Place
    {

    }
}