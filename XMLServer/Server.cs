using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Xml.Serialization;
using ValueObjects;

namespace SimpleServer
{
    class Server
    {
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                AddressData addressData = new AddressData("Max", "Musermann", "Musterweg 1", "Musterort", "Germany", "12345");

                switch (args[0].Trim().ToLower())
                {
                    case "json":
                        new Server(true, addressData).Listen();
                        break;
                    case "xml":
                        new Server(false, addressData).Listen();
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
            Console.Out.WriteLine("usage: SimpleServer <json|xml>");
        }
        // ----------------------------------------------------------------------------


        private HttpListener _listener;
        private readonly int PORT = 1234;
        private bool _jsonMode;
        private AddressData _sampleData;

        /// <summary>
        /// Creates a new server instance
        /// </summary>
        /// <param name="jsonMode">Set to true to use JSON serialization. Otherwise xml serialization is used</param>
        /// <param name="sampleData">A ValueObject to send it to the client</param>
        private Server(bool jsonMode, AddressData sampleData)
        {
            _jsonMode = jsonMode;
            _sampleData = sampleData;
        }

        /// <summary>
        /// Listen on Port <c>PORT</c> for incomming http requests.
        /// Call Method <c>Process</c> with the conte
        /// </summary>
        private void Listen()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://*:" + PORT.ToString() + "/");
            _listener.Start();
            Console.Out.WriteLine("Now listening on port " + PORT.ToString());
            while (true)
            {
                HttpListenerContext context = null;
                try
                {
                    context = _listener.GetContext();
                    HttpListenerRequest request = context.Request;
                    HttpListenerResponse response = context.Response;
                    Console.Out.WriteLine("  Incomeing Request: " + request.Url.ToString());

                    if ("/adressdata".Equals (context.Request.Url.AbsolutePath.Trim().ToLower()))
                    {
                        Console.Out.WriteLine("  AddressData request");
                        response = SerializeAddressData2Response(response);
                    }
                    else
                    {
                        Console.Out.WriteLine("  Unknown api call");
                        response = FillResponse(response, "Unknown api call", MediaTypeNames.Text.Plain, HttpStatusCode.NotImplemented);
                    }

                    response.Close();                   
                }
                catch (Exception ex)
                {
                    if (context != null)
                    {
                        HttpListenerResponse response = GetErrorResponse(context.Response, ex);
                        response.Close();
                    }
                    throw;                        
                }
            }
        }
        /// <summary>
        /// Generates an error response
        /// </summary>
        /// <param name="response">The response object to be filled</param>
        /// <param name="e">The exception that idicates the error</param>
        /// <returns>The filled response object</returns>
        private HttpListenerResponse GetErrorResponse(HttpListenerResponse response, Exception e)
        {
            return FillResponse(response, e.ToString(), MediaTypeNames.Text.Plain, HttpStatusCode.InternalServerError);
        }
        /// <summary>
        /// Serialize the sample adress data object to the given response
        /// </summary>
        /// <param name="response">The response to be filled with serialized address data</param>
        /// <returns>The filled response object</returns>
        private HttpListenerResponse SerializeAddressData2Response(HttpListenerResponse response)
        {
            if (_jsonMode)
            {
                StringBuilder stringBuilder = new StringBuilder();
                using (StringWriter write2Stringbuilder = new StringWriter(stringBuilder))
                {
                    JsonSerializer jsonSerializer = JsonSerializer.CreateDefault();
                    jsonSerializer.Serialize(write2Stringbuilder, _sampleData);
                }

                return FillResponse(response, stringBuilder.ToString(), MediaTypeNames.Application.Json);
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                using (StringWriter write2Stringbuilder = new StringWriter(stringBuilder))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(AddressData));
                    xmlSerializer.Serialize(write2Stringbuilder, _sampleData);
                }

                return FillResponse(response, stringBuilder.ToString(), MediaTypeNames.Application.Xml);
            }
        }

        /// <summary>
        /// Fills a request with content data and sets the required header fields accordingly
        /// </summary>
        /// <param name="response">The response object to be filled</param>
        /// <param name="content">The content for the response</param>
        /// <param name="mimeType">The type of the response content</param>
        /// <param name="statusCode">The http status code for the response (default: OK (200))</param>
        /// <returns>The filled response object</returns>
        private HttpListenerResponse FillResponse(HttpListenerResponse response, string content, string mimeType, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            byte[] data = Encoding.UTF8.GetBytes(content);
            response.ContentType = mimeType;
            response.ContentEncoding = Encoding.UTF8;
            response.ContentLength64 = data.LongLength;
            response.StatusCode = ((int)statusCode);

            // Write out to the response stream
            response.OutputStream.Write(data, 0, data.Length);

            return response;
        }

    }
}
