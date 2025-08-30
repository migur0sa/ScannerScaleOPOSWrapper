using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Scanner_Scale_OPOS_Wrapper.Constants;

namespace Scanner_Scale_OPOS_Wrapper
{
    internal class Logger
    {
        //This is a thread safety object
        private static readonly object _lockObject = new object();
        public static int debug;

        public static void Log(string message, MessageType messageType)
        {
            lock (_lockObject)
            {
                string dateTime = DateTime.Now.ToString();
                string path = @".\\log.txt";
                switch (messageType)
                {
                    case MessageType.normal:
                        File.AppendAllLines(path, new[] { $"{dateTime} - {message}" });
                        if (debug == 1)
                            Console.WriteLine(message);
                        break;

                    case MessageType.scale_error:
                        File.AppendAllLines(
                            path,
                            new[] { $"{dateTime} - SCALE ERROR - {message}" }
                        );
                        if (debug == 1)
                            Console.WriteLine(message);
                        break;

                    case MessageType.scanner_error:
                        File.AppendAllLines(
                            path,
                            new[] { $"{dateTime} - SCANNER ERROR - {message}" }
                        );
                        if (debug == 1)
                            Console.WriteLine(message);
                        break;

                    case MessageType.ini:
                        File.AppendAllLines(
                            path,
                            new[] { $"{dateTime} - INI READ ERROR - {message}" }
                        );
                        if (debug == 1)
                            Console.WriteLine(message);
                        break;
                    case MessageType.namedPipes_error:
                        File.AppendAllLines(
                            path,
                            new[] { $"{dateTime} - NAMED PIPES ERROR - {message}" }
                        );
                        if (debug == 1)
                            Console.WriteLine(message);
                        break;

                    case MessageType.consoleOnly:
                        if (debug == 1)
                            Console.WriteLine(message);
                        break;

                    default:
                        File.AppendAllLines(
                            path,
                            new[] { $"{dateTime} - MISC MSG/ERROR - {message}" }
                        );
                        if (debug == 1)
                            Console.WriteLine(message);
                        break;
                }
            }
        }
    }
}
