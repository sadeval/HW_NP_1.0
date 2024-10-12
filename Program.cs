using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TimeServer
{
    class Program
    {
        static void Main()
        {
            const int port = 13000;
            TcpListener server = null;

            try
            {
                server = new TcpListener(IPAddress.Any, port);
                server.Start();
                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Клиент подключился.");

                    // Обработка запросов клиента
                    NetworkStream stream = client.GetStream();
                    byte[] buffer = new byte[256];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string clientRequest = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    string serverResponse = "";
                    if (clientRequest.Equals("date", StringComparison.OrdinalIgnoreCase))
                    {
                        serverResponse = DateTime.Now.ToShortDateString();
                    }
                    else if (clientRequest.Equals("time", StringComparison.OrdinalIgnoreCase))
                    {
                        serverResponse = DateTime.Now.ToLongTimeString();
                    }
                    else
                    {
                        serverResponse = "Неверный запрос.";
                    }

                    // Отправка ответа клиенту
                    byte[] responseBytes = Encoding.UTF8.GetBytes(serverResponse);
                    stream.Write(responseBytes, 0, responseBytes.Length);

                    // Закрытие соединения
                    client.Close();
                    Console.WriteLine("Соединение с клиентом закрыто.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка: " + e.Message);
            }
            finally
            {
                server?.Stop();
            }
        }
    }
}
