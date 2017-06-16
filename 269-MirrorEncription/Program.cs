using System;
using System.IO;
using System.Threading.Tasks;

namespace DailyProgrammer
{
    class MirrorEncryption_269
    {
        public static void Main(string[] args)
        {
            String filePath = "example.txt";
            if (args.Length < 1) {
                Console.WriteLine("No file was given, using demo version.\n\n");
            } else {
                filePath = args[0];
            }
            Encrypter encrypter = new Encrypter(filePath);
            encrypter.PrintGrid();
            String result = encrypter.Decrypt();
            Console.WriteLine("Result: {0}", result);
        }
    }

    internal class Encrypter
    {
        const int GRID_SIZE = 15;
        const int INNER_GRID_SIZE = GRID_SIZE - 2;

        public char[,] Grid
        {
            get;
        }

        public String InputWord
        {
            get;
            private set;
        }

        public Encrypter(String filePath)
        {
            Grid = new char[GRID_SIZE,GRID_SIZE];
            InitGrid();
            ParseFile(filePath);
        }

        public String Decrypt()
        {
            Console.WriteLine("Input word: {0}", InputWord);
            Console.WriteLine("Decrypting/Encrypting word...");
            char[] result = String.Copy(InputWord).ToCharArray();

            Parallel.For(0, result.Length, (i) => {
                Particle p = new Particle(InputWord[i], i);
                bool finished = false;
                char gridValue;
                while (!finished)
                {
                    p.pos[0] += p.velocity[0];
                    p.pos[1] += p.velocity[1];

                    gridValue = Grid[p.pos[1], p.pos[0]];
                    if (gridValue == '\\')
                    {
                        if (p.velocity[0] == 0)
                        {
                            p.velocity[0] = p.velocity[1];
                            p.velocity[1] = 0;
                        }
                        else
                        {
                            p.velocity[1] = p.velocity[0];
                            p.velocity[0] = 0;
                        }
                    }
                    else if (gridValue == '/')
                    {
                        if (p.velocity[0] == 0)
                        {
                            p.velocity[0] = -p.velocity[1];
                            p.velocity[1] = 0;
                        }
                        else
                        {
                            p.velocity[1] = -p.velocity[0];
                            p.velocity[0] = 0;
                        }
                    }
                    else if (gridValue == ' ')
                    {
                        continue;
                    }
                    else
                    {
                        finished = true;
                        result[i] = gridValue;
                    }
                }
            });

            return new string(result);
        }

        public void PrintGrid()
        {
            Console.WriteLine("Grid:\n");
            String separator = "";
            for (int i = 0; i < GRID_SIZE; i++)
            {
                separator += "----";
            }

            for (int i = 0; i < GRID_SIZE; i++)
            {
                for (int j = 0; j < GRID_SIZE; j++)
                {
                    Console.Write("{0,2}", Grid[i, j]);
                    if (j != GRID_SIZE - 1) {
                       Console.Write(" |");
                    }
                }

                if (i != GRID_SIZE - 1)
                {
                    Console.WriteLine("\n" + separator);
                }
                else
                {
                    Console.WriteLine("\n\n");
                }
            }
        }

        public void ChangeGrid(String gridPath) {
            ParseFile(gridPath);
        }

        private void InitGrid()
        {

			int n = GRID_SIZE - 1;

            // put spaces in corners (just formatting purposes)
            Grid[0, 0] = Grid[0, n] = Grid[n, 0] = Grid[n, n] = ' ';

			for (int i = 1; i < n; i++)
            {
                Grid[i, 0] = (char)('A' + i - 1);
                Grid[0, i] = (char)('a' + i - 1);
                Grid[n, i] = (char)('N' + i - 1);
                Grid[i, n] = (char)('n' + i - 1);
            }
        }

        private void ParseFile(String filePath)
        {
            using (TextReader reader = File.OpenText(filePath))
            {
                for (int i = 1; i <= INNER_GRID_SIZE; i++)
                {
                    String line = reader.ReadLine();
                    if (line == null || line.Length < INNER_GRID_SIZE)
                    {
                        throw new FormatException("The file has an invalid format.");
                    }
                    for (int j = 1; j <= INNER_GRID_SIZE; j++)
                    {
                        Grid[i, j] = line[j - 1];
                    }
                }

                InputWord = reader.ReadLine();
            }
        }

        protected struct Particle {
            public int[] pos;
            public int[] velocity;
            public int wordIndex;
            public char result;

            public Particle(char inputChar, int wordIndex) {
                pos = new int[2];
                velocity = new int[2];
                result = '\0';
                this.wordIndex = wordIndex;

                Init(inputChar);
            }

            private void Init(char inputChar) {
                if (inputChar >= 'A' && inputChar <= 'Z')
                {
                    pos[0] = ((inputChar - 'A') / INNER_GRID_SIZE) * (inputChar -'N' + 1);
                    pos[1] = Math.Min(inputChar - 'A' + 1, GRID_SIZE - 1);
                } else if (inputChar >= 'a' && inputChar <= 'z')
                {
                    pos[0] = Math.Min(inputChar - 'a' + 1, GRID_SIZE - 1);
                    pos[1] = ((inputChar - 'a') / INNER_GRID_SIZE) * (inputChar - 'n' + 1);
                } else 
                {
                    throw new FormatException(String.Format("Invalid character: {0}", inputChar));
                }

                if (pos[0] == 0) 
                {
                    velocity[0] = 1;
                    velocity[1] = 0;
                } else if (pos[0] == GRID_SIZE - 1) 
                {
                    velocity[0] = -1;
                    velocity[1] = 0;
                } else if (pos[1] == 0) 
                {
                    velocity[0] = 0;
                    velocity[1] = 1;
                } else 
                {
                    velocity[0] = 0;
                    velocity[1] = -1;
                }
            }
        }
    }
}
