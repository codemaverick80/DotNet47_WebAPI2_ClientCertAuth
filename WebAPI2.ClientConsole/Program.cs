using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;

namespace WebAPI2.ClientConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            GetApiDataAndSendClientCertToApi();

           // GetCertificateFromFile();
        }



        public static void GetApiDataAndSendClientCertToApi()
        {
           X509Certificate2 clientCert = GetClientCertificateFromStore();

            //X509Certificate2 clientCert = GetCertificateFromFile();

            WebRequestHandler requestHandler = new WebRequestHandler();

            requestHandler.ClientCertificates.Add(clientCert);

            HttpClient client = new HttpClient(requestHandler)
            {
                BaseAddress = new Uri("https://geetsangeet.com/")
            };
            client.DefaultRequestHeaders.Add("API-SenderMessageId", Guid.NewGuid().ToString());
            client.DefaultRequestHeaders.Add("API-SenderApplicationId", "ConsoleApp");
            client.DefaultRequestHeaders.Add("API-SenderHostName", "localhost");
            client.DefaultRequestHeaders.Add("API-CreationTimeStamp", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffK"));
            client.DefaultRequestHeaders.Add("API-TransactionId", "Client TransactionId");


            HttpResponseMessage response = client.GetAsync("api/values").Result;
            string responseContent = response.Content.ReadAsStringAsync().Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine(responseContent);
            Console.ReadKey();

        }
        public static void GetApiData()
        {

            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri("https://geetsangeet.com/")

            };

            HttpResponseMessage response = client.GetAsync("api/values").Result;
            string responseContent = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(responseContent);
            Console.ReadKey();

        }


        private static X509Certificate2 GetClientCertificateFromStore()
        {
            X509Store userCaStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            try
            {
                userCaStore.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection certificatesInStore = userCaStore.Certificates;
                X509Certificate2Collection findResult = certificatesInStore.Find(X509FindType.FindBySubjectName, "OU=TESTAPP", true);
                X509Certificate2 clientCertificate = null;
                if (findResult.Count == 1)
                {
                    clientCertificate = findResult[0];
                }
                else
                {
                    throw new Exception("Unable to locate the correct client certificate.");
                }
                return clientCertificate;
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                userCaStore.Close();
            }

        }

        private static X509Certificate2 GetCertificateFromFile()
        {
            string pfxFilePath = @"C:\Client.pfx";
            string pfxPassword = "XXXXX";

            X509Certificate2 x509 = new X509Certificate2();
            byte[] rawData = ReadFile(pfxFilePath);
            x509.Import(rawData,pfxPassword, X509KeyStorageFlags.DefaultKeySet);
            //Console.WriteLine("{0}Subject: {1}{0}", Environment.NewLine, x509.Subject);
            //Console.WriteLine("{0}Issuer: {1}{0}", Environment.NewLine, x509.Issuer);
            //Console.WriteLine("{0}Version: {1}{0}", Environment.NewLine, x509.Version);
            //Console.WriteLine("{0}Valid Date: {1}{0}", Environment.NewLine, x509.NotBefore);
            //Console.WriteLine("{0}Expiry Date: {1}{0}", Environment.NewLine, x509.NotAfter);
            //Console.WriteLine("{0}Thumbprint: {1}{0}", Environment.NewLine, x509.Thumbprint);
            //Console.WriteLine("{0}Serial Number: {1}{0}", Environment.NewLine, x509.SerialNumber);
            //Console.WriteLine("{0}Friendly Name: {1}{0}", Environment.NewLine, x509.PublicKey.Oid.FriendlyName);
            //Console.WriteLine("{0}Public Key Format: {1}{0}", Environment.NewLine, x509.PublicKey.EncodedKeyValue.Format(true));
            //Console.WriteLine("{0}Raw Data Length: {1}{0}", Environment.NewLine, x509.RawData.Length);
            //Console.WriteLine("{0}Certificate to string: {1}{0}", Environment.NewLine, x509.ToString(true));
            //Console.WriteLine("{0}Certificate to XML String: {1}{0}", Environment.NewLine, x509.PublicKey.Key.ToXmlString(false));

            return x509;
        }
        internal static byte[] ReadFile(string fileName)
        {
            FileStream f = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            int size = (int)f.Length;
            byte[] data = new byte[size];
            size = f.Read(data, 0, size);
            f.Close();
            return data;
        }



        public static void GetAllCertificatesFromRoot()
        {
            X509Store userCaStore = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
            try
            {
                userCaStore.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection certificate2Collection = userCaStore.Certificates;
                foreach (var cert in certificate2Collection)
                {
                    Console.WriteLine("-----------------------------");
                    Console.WriteLine(cert.GetExpirationDateString());
                    Console.WriteLine(cert.Issuer);
                    Console.WriteLine(cert.HasPrivateKey);
                    Console.WriteLine(cert.SubjectName.Name);
                    Console.WriteLine("-----------------------------");
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                userCaStore.Close();
            }
        }

        public static void GetAllCertificatesFromCurrentUserStore()
        {
            X509Store userCaStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            try
            {
                userCaStore.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection certificate2Collection = userCaStore.Certificates;
                foreach (var cert in certificate2Collection)
                {
                    Console.WriteLine("-----------------------------");
                    Console.WriteLine(cert.GetExpirationDateString());
                    Console.WriteLine(cert.Issuer);
                    Console.WriteLine(cert.HasPrivateKey);
                    Console.WriteLine(cert.SubjectName.Name);
                    Console.WriteLine("-----------------------------");
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                userCaStore.Close();
            }
        }

    }
}
