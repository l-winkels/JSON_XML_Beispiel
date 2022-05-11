using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Xml.Serialization;
using ValueObjects;

namespace SimpleClient
{
    class Client
    {
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {           
                switch (args[0].Trim().ToLower())
                {
                    case "json":
                        new Client(true).GetAndPrintAddressDataAsync();
                        break;
                    case "xml":
                        new Client(false).GetAndPrintAddressDataAsync();
                        break;
                    default:
                        printUsage();
                        break;
                }
            }
            else
            {
                printUsage();
            }
        }

        /// <summary>
        /// Prints usage informations to the console window.
        /// </summary>
        static void printUsage()
        {
            Console.Out.WriteLine("usage: SimpleClient <json|xml>");
        }

        // ----------------------------------------------------------------------------


        private HttpClient _client;
        private readonly int PORT = 1234;
        private bool _jsonMode;

        /// <summary>
        /// Creates a new Client instance
        /// </summary>
        /// <param name="jsonMode">Set to true to use JSON serialization. Otherwise xml serialization is used</param>
        private Client(bool jsonMode)
        {
            _jsonMode = jsonMode;         
        }

        /// <summary>
        /// Call to function <c>GetSerializedObject()</c> and deserialize the result
        /// </summary>
        public void GetAndPrintAddressDataAsync()
        {
            string serializedObject = GetSerializedObject();

            AddressData addressData;

            if (_jsonMode)
            {                              
                using (StringReader stringReader = new StringReader(serializedObject))
                {
                    JsonSerializer jsonSerializer = JsonSerializer.CreateDefault();
                    addressData = (AddressData) jsonSerializer.Deserialize(stringReader, typeof(AddressData));
                }             
            }
            else
            {
                using (StringReader stringReader = new StringReader(serializedObject))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(AddressData));
                    addressData = (AddressData) xmlSerializer.Deserialize(stringReader);
                }             
            }

            Console.Out.WriteLine("Received AddessData object:");
            Console.Out.WriteLine(addressData.ToString());
        }

        /// <summary>
        /// Performs a http call to http://127.0.0.1 at Port <c>PORT</c> and returns the result
        /// </summary>
        /// <returns>Content of the http response</returns>
        private string GetSerializedObject()
        {
            string serializedObject = null;
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage responseMessage = client.GetAsync("http://127.0.0.1:" + PORT.ToString()+ "/adressdata").Result;
                HttpContent content = responseMessage.Content;

                serializedObject = content.ReadAsStringAsync().Result;
            }

            return serializedObject;
        }
    }
}
