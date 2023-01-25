using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace TCPClient
{
    class Program
    {
        //gelen bağlantı isteği üzerine bağlantının saklanacağı yer.
        static Socket dinleyiciSoket = new Socket
             (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        const int PORT = 52000;

        static void Main(string[] args)
        {
            Console.Title = "TCP Data Transmit - Server";

            // herhangi bir IP adresinden gelebilecek bağlantı isteklerini alabilmesi için;
            TcpListener dinle = new TcpListener(IPAddress.Any, PORT);
            dinle.Start();

            Console.WriteLine("Bağlantı bekleniyor...");

            //IP adresi girildikten sonra bağlantı başlıyor 
            dinleyiciSoket = dinle.AcceptSocket();
            Console.WriteLine("Bağlantı sağlandı. ");

            while (true)
            {
                try
                {
                    //Gelecek mesajı içinde saklayabilmemiz için buffer oluşturarak gelen mesajlar bytelanır. Oluşturulan bufferın (gelenData) boyutu belirlenmeli, uzun mesajlar için daha büyük byte kullanılmalı

                    byte[] gelenData = new byte[256];
                    dinleyiciSoket.Receive(gelenData);  //Receive metodu ile gelen mesaja okuma işlemi yapılır. 

                    //gelen mesajın Türkçe karakter kullanımı UTF8
                    //Split metodu ise gelen mesajın bittiği noktaya kadar alınmasını sağlar.Çünkü çoğu zaman mesaj byteların tamamını doldurmaz ve kalan karakteri boşluk görür. Split ile bu engellenir.
                    string mesaj = (Encoding.UTF8.GetString(gelenData)).Split('\0')[0];

                    Console.WriteLine("Gelen mesaj: " + mesaj);

                    /// Burada Gelen mesajı dinleyip mesajın işlem zamanı sonuna ekleyip konsolda yazma işlemi yapıldı.
                    mesaj += " İşlem Zamanı :  " + DateTime.Now.ToString();
                    byte[] msg = Encoding.UTF8.GetBytes(mesaj);
                    dinleyiciSoket.Send(msg);
                    ///

                    if (mesaj.ToLower().StartsWith("exit"))
                    {

                        Console.WriteLine("Bağlantı kapatılıyor.");
                        dinleyiciSoket.Dispose();
                        break;
                    }
                }
                catch
                {

                    Console.WriteLine("Bağlantı kesildi. Çıkış yapılıyor.");
                    break;
                }
            }

            Console.WriteLine("Kapatmak için ENTER'a basın.");
            Console.Read();
        }
    }
}
