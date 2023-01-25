using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace TCPServer
{
    class Program
    {
        // TCP bağlantısının açık kalması için
        static Socket soket = new Socket
             (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        //mesajlarımızın gideceği port.
        const int PORT = 52000;

        static void Main(string[] args)
        {
            
            string IP = "";

            Console.Title = "TCP Data Transmit - Client";

            //127.0.0.1
            Console.Write("Bağlanılacak bilgisayarın IP numarası: ");
            IP = Console.ReadLine();

            Console.WriteLine(IP + " adresindeki bilgisayara bağlanılıyor.Lütfen Bekleyiniz.");


            try
            {
                soket.Connect(new IPEndPoint(IPAddress.Parse(IP), PORT));
                Console.WriteLine("Başarıyla bağlanıldı!");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n (X) -> Bağlanmaya çalışırken hata oluştu: " + ex.Message);
            }


            while (true && soket.Connected)
            {
                Console.Write("Mesaj: ");

                //Göndermek istediğimiz mesajı yazıyoruz.
                string gonder = Console.ReadLine();

                //ağ üzerinden göndermek istediğimiz mesajları bytelara dönüştürdük.
                soket.Send(Encoding.UTF8.GetBytes(gonder));
                /// 
                byte[] bytes = new byte[1024];
                int bytesRec = soket.Receive(bytes);
                Console.WriteLine("Serverdan Gelen Mesaj = {0}",
                    Encoding.UTF8.GetString(bytes, 0, bytesRec));
                ///
            }

            Console.WriteLine("Programı sonlandırmak için ENTER tuşuna basın...");
            Console.Read();
        }
    }
}

