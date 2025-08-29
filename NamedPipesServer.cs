using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;

namespace Zebra_Scanner_Scale_OPOS
{
    class NamedPipesServer
    {
        private static NamedPipeServerStream pipeServer;
        internal static StreamWriter pipeWriter;
        private static readonly object pipeLock = new object();
        private static bool isServerRunning = false;

        internal static void StartNamedPipeServer()
        {
            if (isServerRunning)
            {
                Console.WriteLine("Server is already running.");
                return;
            }

            // Start the server in a background task
            Task.Run(() => ListenForConnections());
        }

        private static void ListenForConnections()
        {
            isServerRunning = true;
            while (true)
            {
                // Create a new pipe server instance for each connection
                NamedPipeServerStream currentPipeServer = null;
                try
                {
                    currentPipeServer = new NamedPipeServerStream("ZebraScannerScalePipe", PipeDirection.Out, NamedPipeServerStream.MaxAllowedServerInstances, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
                    Console.WriteLine("Waiting for pipe client...");

                    currentPipeServer.WaitForConnection();
                    Console.WriteLine("Pipe client connected.");

                    // Safely update the shared pipe objects under a lock
                    lock (pipeLock)
                    {
                        // Dispose of previous connections if they exist
                        pipeWriter?.Dispose();
                        pipeServer?.Dispose();

                        pipeServer = currentPipeServer;
                        pipeWriter = new StreamWriter(pipeServer, Encoding.UTF8) { AutoFlush = true };
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"An IO exception occurred: {ex.Message}");
                }
                catch (ObjectDisposedException)
                {
                    Console.WriteLine("Server pipe was disposed unexpectedly.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An unexpected error occurred in the server loop: {ex.Message}");
                }
                finally
                {
                    // Ensure the temporary pipe server is disposed if an error occurred before the lock was taken
                    if (currentPipeServer != null && currentPipeServer != pipeServer)
                    {
                        currentPipeServer.Dispose();
                    }
                }
            }
        }

        internal static void SendDataToClient(string data)
        {
            // Use a lock to ensure thread-safe access to the pipe objects
            lock (pipeLock)
            {
                try
                {
                    if (pipeWriter != null && pipeServer != null && pipeServer.IsConnected)
                    {
                        pipeWriter.WriteLine(data);
                        Console.WriteLine($"Sent: {data}");
                    }
                    else
                    {
                        Console.WriteLine("Pipe is not connected. Message not sent.");
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Error writing to pipe: {ex.Message}. Client has likely disconnected.");
                }
                catch (ObjectDisposedException)
                {
                    Console.WriteLine("Pipe writer is disposed. Client has disconnected.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An unexpected error occurred while writing: {ex.Message}");
                }
            }
        }
    }
}