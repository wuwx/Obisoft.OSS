using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Obisoft.OSS.Services
{
    public class HTTPService : object
    {
        public CookieContainer cc = new CookieContainer();
        public async Task<string> Post(string Url, string postDataStr, string Decode = "utf-8")
        {
            var request = WebRequest.CreateHttp(Url);
            if (cc.Count == 0)
            {
                request.CookieContainer = new CookieContainer();
                cc = request.CookieContainer;
            }
            else
            {
                request.CookieContainer = cc;
            }
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            var myRequestStream = await request.GetRequestStreamAsync();
            var myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("GB2312"));
            await myStreamWriter.WriteAsync(postDataStr);
            myStreamWriter.Dispose();
            var response = await request.GetResponseAsync();
            var myResponseStream = response.GetResponseStream();
            var myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding(Decode));
            string retString = await myStreamReader.ReadToEndAsync();
            myStreamReader.Dispose();
            myResponseStream.Dispose();
            return retString;
        }

        public async Task<string> PostFile(string Url, string filepath, string Decode = "utf-8")
        {
            var file = new FileInfo(filepath);
            var fileStream = new FileStream(filepath, mode: FileMode.Open);
            var request = new HttpClient();
            var form = new MultipartFormDataContent();
            form.Add(new StreamContent(fileStream), "file", file.FullName);
            var response = await request.PostAsync(Url, form);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> Get(string Url, string Coding = "utf-8")
        {
            var request = WebRequest.CreateHttp(Url);
            if (cc.Count == 0)
            {
                request.CookieContainer = new CookieContainer();
                cc = request.CookieContainer;
            }
            else
            {
                request.CookieContainer = cc;
            }
            request.Method = "GET";
            request.ContentType = "text/html;charset=" + Coding;
            var response = await request.GetResponseAsync();
            var myResponseStream = response.GetResponseStream();
            var myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding(Coding));
            string retString = await myStreamReader.ReadToEndAsync();
            myStreamReader.Dispose();
            myResponseStream.Dispose();
            return retString;
        }

    }
}