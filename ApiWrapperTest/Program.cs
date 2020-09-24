using ApiWrapper;
using ApiWrapper.Abstractions;
using RestSharp;
using System;
using System.Collections.Generic;

namespace ApiWrapperTest
{
    public class Program 
    {
        static void Main(string[] args)
        {
            Task();
            Console.ReadKey();
        }

        private static async void Task()
        {
            var client = new HttpClient("http://localhost:1337/");
            var response = await client
                .SetMethod(Method.PUT)
                .SetParams(new List<Parameter>
                {
                    new Parameter("emailAddress", "+525532182681@gmail.com", ParameterType.GetOrPost),
                    new Parameter("password", "+525532182681", ParameterType.GetOrPost),
                    new Parameter("rememberMe", "1", ParameterType.GetOrPost)
                })
                .ExecuteAsync<string>("api/v1/entrance/login");
            var cookies = client.GetResponseCookies();

            foreach (var cookie in cookies)
            {
                Console.WriteLine(cookie.Key + " " + cookie.Value);
            }

            Console.WriteLine(response);

            var me = await client.SetMethod(Method.GET)
                .SetCookies(cookies)
                .ExecuteAsync<string>("api/v1/account/me");

            Console.WriteLine(me);
        }
    }

    public class ConsoleLogger : ILogger
    {
        public void Write(string log)
        {
            Console.Write(log);
        }
    }

    public class Usuario
    {
        [ApiParameter(Ignore = true)]
        public string nombre { get; set; }
    }
}