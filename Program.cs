using MarsInvestigationProject.Models;
using MarsInvestigationProject.Models.Enums;

internal class Program
{
    private static void Main(string[] args)
    {
        #region createPlateau

        Plateau? plateau = createPlateau();
        while (plateau is null)
        {
            plateau = createPlateau();
        };
        log($"{plateau.maxX}x{plateau.maxY} plateau has been discovered", LogLevel.Info);

        #endregion

        #region moveOperations

        bool keepResearching;
        do
        {
            Position? position = getPosition(plateau);
            while (position is null)
            {
                position = getPosition(plateau);
            };
            log($"Rover has been landed on {position.x},{position.y},{position.r}.", LogLevel.Info);

            var instructions = getInstructions();
            while (string.IsNullOrWhiteSpace(instructions))
            {
                instructions = getInstructions();
            };
            log($"Roger that! Rover will follow your instructions...", LogLevel.Info);


            if (moveRover(position, instructions))
                log($"Rover's final position: {position.x} {position.y} {position.r}", LogLevel.Info);
            else
                log($"The connection with the rover has been lost. Last contacted location: {position.x},{position.y},{position.r}", LogLevel.Warning);

            keepResearching = askForMoreRovers();
        } while (keepResearching);

        #endregion

        #region customMethods

        bool askForMoreRovers()
        {
            log($"Press 'Y' to research with another rover, press any key to abort mission.", LogLevel.Order);
            ConsoleKeyInfo info = Console.ReadKey();
            Console.WriteLine();
            if (info.Key == ConsoleKey.Y)
                return true;
            else
                return false;
        }

        static Plateau? createPlateau()
        {
            log("Enter upper-right coordinates of the plateau(X Y):", LogLevel.Order);
            string[] plateauCoords = ((Console.ReadLine() ?? string.Empty)).Split(' ');
            if (plateauCoords.Length != 2)
            {
                log("Invalid Input, the format should be \"X Y\" as the number.", LogLevel.Warning);
                return null;
            }
            try
            {
                return new Plateau(int.Parse(plateauCoords[0]), int.Parse(plateauCoords[1]));
            }
            catch
            {
                log("Invalid Input, the format should be \"X Y\" as the number.", LogLevel.Warning);
                return null;
            }
        }

        static Position? getPosition(Plateau plateau)
        {
            log("Enter rover position and direction(X Y R):", LogLevel.Order);
            string[] pos = ((Console.ReadLine() ?? string.Empty)).Split(' ');
            if (pos.Length != 3)
            {
                log("Invalid Input, the format should be \"X Y R\" as the order of number,number,letter(N,W,S,E)", LogLevel.Warning);
                return null;
            }
            try
            {
                int x = int.Parse(pos[0]);
                int y = int.Parse(pos[1]);
                if (x <= plateau.maxX && y <= plateau.maxY) return new Position(x, y, Enum.Parse<Route>(pos[2], true));

                log("The rover landed off the plateau. Try again with a new rover", LogLevel.Warning);
                return null;
            }
            catch
            {
                log("Invalid Input, the format should be \"X Y R\" as the order of number,number,letter(N,W,S,E)", LogLevel.Warning);
                return null;
            }
        }

        static string getInstructions()
        {
            log("Enter instructions, it can be combination of L,R,M without whitespaces and commas:", LogLevel.Order);
            string instructions = (Console.ReadLine() ?? string.Empty).ToUpper();
            char[] allowed = { 'L', 'R', 'M' };
            if (instructions.Any(c => !allowed.Contains(c)))
            {
                log("Invalid Input, It must contain only the letters L,R,M", LogLevel.Warning);
                return string.Empty;
            }
            return instructions;
        }

        bool moveRover(Position position, string instructions)
        {
            bool status = true;
            foreach (char instruction in instructions)
            {
                if (instruction == 'M')
                {
                    (position.x, position.y, status) = move(position.x, position.y, position.r, status);
                    if (!status)
                    {
                        return status;
                    }
                }
                else
                {
                    position.r = rotate(position.r, instruction);
                }
            }
            return status;
        }

        Route rotate(Route route, char instruction)
        {
            if (instruction == 'L')
                return (Route)(((int)route + 3) % 4);
            else
                return (Route)(((int)route + 1) % 4);
        }

        (int, int, bool) move(int x, int y, Route route, bool status)
        {
            switch (route)
            {
                case Route.N:
                    if (y < plateau.maxY)
                        y++;
                    else
                        status = false;
                    break;
                case Route.E:
                    if (x < plateau.maxX)
                        x++;
                    else
                        status = false;
                    break;
                case Route.S:
                    if (y > 0)
                        y--;
                    else
                        status = false;
                    break;
                case Route.W:
                    if (x > 0)
                        x--;
                    else
                        status = false;
                    break;
                default:
                    throw new ArgumentException("Invalid direction");
            }
            return (x, y, status);
        }

        static void log(string m, LogLevel l)
        {
            switch (l)
            {
                case LogLevel.Order:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case LogLevel.Info:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
            Console.WriteLine(m);
            Console.ForegroundColor = ConsoleColor.White;
        } 

        #endregion
    }

}