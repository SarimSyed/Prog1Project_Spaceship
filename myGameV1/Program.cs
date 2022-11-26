using System;

namespace myGameV1
{
    class Program
    {
        //Constants
        const int CONSOLEX = 20, CONSOLEY = 36;
        const char SHIP = 'A';
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            //Console window size
            Console.WindowHeight = CONSOLEY+2;
            Console.WindowWidth = CONSOLEX+1; //size picked from trial and error
            Console.SetBufferSize(CONSOLEX+1,CONSOLEY+2); //This actually makes the window smaller but its causing an issue where If i make it less that 2 sizes bigger than my window constant my arrays have issues.
            //locations
            int charX = 5, charY = CONSOLEY - 1;
            int oldCharX = 6, oldCharY = 10;
            int lives = 3;
            double shipCharge=0;
            bool hit;
            //obstacles
            int[] cometX = new int[CONSOLEX]; 
            int[] cometY = new int[CONSOLEX];
            int[] oldComet = new int[CONSOLEX];

            //FillArrays(ref wall1X, ref wall1Y);
            Aliens(ref cometX, ref cometY);

            ConsoleKey k = ConsoleKey.NoName;
            Console.WriteLine("\n\n\n\n\n\n\n\n   Press any key " +
                "\n       start");
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("Use Arrow keys \nto move the ship\n\n\nYour ship has been \ndamaged after a long battle\nand can only take\n3 hits.\nYou must now\nretreat through an\nasteroid field.\n\nSurvive until the\njumpdrive reaches\nmax charge\nand the ship\ncan escape");
            Console.ReadKey();
            Console.Clear();

            // while esc key is not pressed the game will continue
            while (k != ConsoleKey.Escape)
            {
                //System.Threading.Thread.Sleep(150);//add wait for input to avoid lag
                //was keyboard hit?
                if (Console.KeyAvailable)
                {
                    // save old cursor location for it to be erased
                    oldCharX = charX;
                    oldCharY = charY;
                    // gets kets key to be able to exit and also sets location
                    GetKey(ref k, ref charX, ref charY);
                }
                //fill and draw the walls

                System.Threading.Thread.Sleep(50);
                //Draw the ship
                Draw(charX, charY, oldCharY, oldCharX);
                //hit collision check
                if (CollisionCheck(cometX, cometY, charX, charY))
                {
                    Console.SetCursorPosition(charX, charY);
                    Console.Write("*");
                    lives--;
                }
                //Show lives
                Health(lives);
                if (lives == 0)
                {
                    break;
                }

                //Draw Comets and move them

                CometFall(cometX, ref cometY, ref oldComet);
                System.Threading.Thread.Sleep(250);
                CometDraw(cometX, ref cometY, ref oldComet);
                shipCharge = shipCharge + 1;
                DriveCharge(shipCharge);
                if (shipCharge == 100)
                {
                    break;
                }

            }

            if (shipCharge >= 100)
            {
                Console.Clear();
                Console.SetCursorPosition(1, 14);
                Console.Write("Jump successful\nYou have\nescaped!");
                Console.SetCursorPosition(charX, charY);
                Console.Write(SHIP);

            }
            else
            {
                Console.SetCursorPosition(charX, charY);
                Console.Write("X");
                Console.SetCursorPosition(1, 14);
                Console.Write("Your ship was\ndestroyed\n\nYour side\nhas suffered\na great loss");
            }
        }
        static void DriveCharge(double shipCharge)
        {
            Console.SetCursorPosition(5, 1);
            if (shipCharge < 25)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (shipCharge < 50)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else if (shipCharge < 75)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            Console.Write("Drive charge:%" + shipCharge);
            Console.ForegroundColor = ConsoleColor.White;
        }
        //comets
        static void Aliens(ref int[] alienX, ref int[] alienY)
        {
            Random r = new Random();
            
            for (int i = 0; i < alienX.Length; i++)
            {
                alienX[i] = r.Next(1, CONSOLEX  );
                alienY[i] = r.Next(1, CONSOLEY  );
            }

        }

        static void CometDraw(int[] cometX, ref int[] cometY, ref int[] oldCometY)
        {// to be able to erase
           

            for (int i = 0; i < cometX.Length; i++)//maybe change this variable or constant later on
            {
               // Console.SetCursorPosition(cometX[i], (cometY[i]));
               // Console.Write(" ");

                Console.SetCursorPosition(cometX[i], oldCometY[i]);
                Console.Write(" ");
                Console.SetCursorPosition(cometX[i], cometY[i]);
                Console.Write("O");
            }

        }
        static void Health(int lives)
        {
            Console.SetCursorPosition(1, 1);
            Console.ForegroundColor = ConsoleColor.Green;

            if (lives == 3)
            {
                Console.Write("AAA");
            }
            else if(lives == 2)
            {
                Console.Write("AA ");
            }
            else
            {
                Console.Write("A  ");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
        static void CometFall(int[] cometX, ref int[] cometY, ref int[] oldY)
        {
            for (int i = 0; i < cometX.Length ; i++)
            {
                oldY[i] = cometY[i];
            }

            for (int i = 0; i < cometX.Length; i++)
            {
                cometY[i]++;
            }
            for (int i = 0; i < cometY.Length; i++)
            {
                //sends comet all the way up
                if (37 < cometY[i])
                {
                    cometY[i] = 3;
                }
            }
        }
        static bool CollisionCheck(int[] cometX, int[] cometY, int charX, int charY)
        {
            bool hit = false;
            for (int i = 0; i < cometY.Length; i++)
            {
                if (cometX[i] == charX && cometY[i] == charY)
                {
                    hit = true;
                    break;
                }              
            }
            return hit;
        }
        //Get key and movement
        static void GetKey(ref ConsoleKey k, ref int charX, ref int charY)
        {
            k = Console.ReadKey(true).Key; //keystroke will be put into k
            switch (k)
            {
                case ConsoleKey.LeftArrow:
                    charX--;
                    charY++;
                    break;
                case ConsoleKey.UpArrow:
                    charY--;
                    break;
                case ConsoleKey.RightArrow:
                    charX++;
                    charY++;//Prevents the ship from going diagonal
                    break;
                case ConsoleKey.DownArrow:
                    charY++;
                    charY++;
                    break;
                default:
                    break;
            }
            //Make sure the location of x and y stays within console
            //dindt use else because i want it to check all everything
            if (charX < 0)
            {
                charX++;
            }
            if (charX > (CONSOLEX))
            {
                charX-- ;
            }
            if (charY <0)
            {
                charY++;
            }
            if (charY > CONSOLEX)
            {
                charY-- ;
            }
        }
        static void Draw(int charX, int charY, int oldCharY, int oldCharX)
        {
            //Erase before printing only if new spot isn't the same as last (prevents blinking)
            if (oldCharX != charX || oldCharY != charY)
            {
                Console.SetCursorPosition(oldCharX, oldCharY);
                Console.Write(" ");
            }
            //Print ship
            Console.SetCursorPosition(charX, charY);
            Console.Write(SHIP);
        }
    }
}
