using System;
using System.Linq;
using System.Text;
using System.IO.Pipes;
using System.IO;


namespace Zebra_MP7000_OPOS
{
    class NamedPipesServer
    {
        internal static NamedPipeServerStream pipeServer;
        internal static StreamWriter pipeWriter;
        private static bool isServerRunning = false;

        internal static void StartNamedPipeServer()
        {
            if (isServerRunning)
            {
                Console.WriteLine("Server is already running.");
                return;
            }

            try
            {
                pipeServer = new NamedPipeServerStream("ZebraScannerScalePipe", PipeDirection.Out, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
                Console.WriteLine("Waiting for pipe client...");

                // This will block until a client connects.
                // You might want to run this in a separate thread/Task.
                pipeServer.WaitForConnection();

                pipeWriter = new StreamWriter(pipeServer, Encoding.UTF8) { AutoFlush = true };
                isServerRunning = true;
                Console.WriteLine("Pipe client connected.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during server startup: {ex.Message}");
                // Clean up resources on failure
                pipeWriter?.Dispose();
                pipeServer?.Dispose();
                isServerRunning = false;
            }
        }

        // Add a method to handle client disconnection and cleanup
        internal static void DisconnectAndRestart()
        {
            Console.WriteLine("Client disconnected. Cleaning up and restarting server.");
            pipeWriter?.Dispose();
            pipeServer?.Dispose();
            isServerRunning = false;
            StartNamedPipeServer(); // Restart the server
        }
    }
}
