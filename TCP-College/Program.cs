using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCPCollege
{
    static class Program
    {
        private static TcpClient client;
        private static NetworkStream stream;
        
        static void Main(string[] args)
        {
            try
            {
                int port = 52002;
                string ipAddress = "challenge01.root-me.org";
                client = new TcpClient(ipAddress, port);
                Console.WriteLine("Connected to server");

                // Get network stream
                stream = client.GetStream();

                // Receive and print full response for debugging
                byte[] data = new byte[1024];
                int bytes = stream.Read(data, 0, data.Length);
                string response = Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine($"Received full message:\n{response}");

                // Split and print each line for debugging
                string[] lines = response.Split('\n');
                Console.WriteLine($"Number of lines: {lines.Length}");
                for (int i = 0; i < lines.Length; i++)
                {
                    Console.WriteLine($"Line {i}: {lines[i]}");
                }

                // Get the line with the calculation
                string calculationLine = lines[6]; // Try first line instead
                Console.WriteLine($"Calculation line: {calculationLine}");
                
                string[] parts = calculationLine.Split(' ');
                Console.WriteLine($"Parts: {string.Join(", ", parts)}");
                
                // Adjust indices based on actual message format
                int number = int.Parse(parts[5]);
                int multiplier = int.Parse(parts[9]);
                Console.WriteLine($"Numbers: {number} and {multiplier}");

                // Calculate and send with 2 decimal places
                double result = Math.Sqrt(number) * multiplier;
                string answer = result.ToString("F2") + "\n"; // Add newline character
                byte[] messageBytes = Encoding.ASCII.GetBytes(answer);
                stream.Write(messageBytes, 0, messageBytes.Length);
                Console.WriteLine($"Sent answer: {answer}");

                // Read the server's response
                bytes = stream.Read(data, 0, data.Length);
                response = Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine($"Server response: {response}");
                
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
            finally
            {
                stream?.Close();
                client?.Close();
            }
        }
    }
}

