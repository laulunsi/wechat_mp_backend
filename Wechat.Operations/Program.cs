using System;
using System.IO;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace Wechat.Operations
{
    internal class Program
    {
        public static IConfiguration Configuration { get; set; }

        private static void Main(string[] args)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile("appsettings.Development.json", true, true)
                .Build();

            Console.WriteLine("Wechat Operation System Started.");

            while (true)
                RunOnce();
        }

        private static void RunOnce()
        {
            Console.WriteLine("-------------------------------");
            Console.WriteLine("1. 获取access_token");
            Console.WriteLine("2. 查询自定义菜单");
            Console.WriteLine("-------------------------------");

            Console.Write("select:");
            var idx = Console.ReadLine();

            switch (idx)
            {
                case "1":
                    GetAccessToken();
                    break;
            }

            Console.WriteLine("run completed");
        }

        private static void GetAccessToken()
        {
            const string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";

            var appId = Configuration.GetSection("Weixin:ServiceAccount:Sft:AppId").Value;
            var appSecret = Configuration.GetSection("Weixin:ServiceAccount:Sft:AppSecret").Value;

            using (var client = new WebClient())
            {
                var response = client.DownloadString(string.Format(url, appId, appSecret));
            }
        }
    }
}