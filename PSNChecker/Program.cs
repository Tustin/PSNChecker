using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;
namespace PSNChecker
{
    class Program
    {
        //Class for the POST request
        class PSNRequest
        {
            public string onlineId;
            public bool reserveIfAvailable = false;
        }
        static void Main(string[] args)
        {
            Console.Title = "PSN Username Checker ~ Tustin";
            if (args.Length == 0)
            {
                Console.WriteLine("HOLD UP! Invalid parameters passed.");
                Console.WriteLine("How to use this:");
                Console.WriteLine("PSNChecker.exe path_to_wordlist.txt (optional)path_to_output.txt");
                Console.WriteLine("The optional output path will output all available PSNs to a text file.");
                Console.ReadKey();
                return;
            }
            int im_good = 0, dat_boi_o_shit = 0;
            List<string> valid = new List<string>();
            using (StreamReader r = new StreamReader(args[0]))
            {
                string line;

                while ((line = r.ReadLine()) != null)
                {
                    var output = Check(line);
                    if (output == null)
                    {
                        Console.WriteLine("{0} -> VALID", line);
                        im_good++;
                        valid.Add(line);
                    }
                    else
                    {
                        dynamic resp = JsonConvert.DeserializeObject(output);
                        string msg = resp[0].validationErrors[0].message;
                        Console.WriteLine("{0} -> {1}", line, msg.Split('.')[0]);
                    }
                    dat_boi_o_shit++;
                }
            }
            Console.WriteLine("Finished validating names. {0}/{1} names were valid.", im_good, dat_boi_o_shit);
            if (args.Length == 2)
            {
                File.WriteAllLines(args[1], valid.ToArray());
                Console.WriteLine("Output valid PSN names to {0}", args[1]);
            }
        }
        //Checks the validity of the PSN name. 
        static string Check(string PSN)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://accounts.api.playstation.com/api/v1/accounts/onlineIds");
            req.ContentType = "application/json";
            req.Method = WebRequestMethods.Http.Post;

            PSNRequest o = new PSNRequest();
            o.onlineId = PSN;
            string json = JsonConvert.SerializeObject(o);

            using (var streamWriter = new StreamWriter(req.GetRequestStream()))
            {
                streamWriter.Write(json);
            }
            //This code is so terrible but apparently C# likes to throw an exception on "bad" HTTP codes (like 400). 
            //Fuck you Microsoft.
            try
            {
                var resp = (HttpWebResponse)req.GetResponse();
                //We'll just make this return null because if it hits here then the response was good (aka the name is available).
                //Otherwise it just outputs an empty JSON object.
                return null;
            }
            catch (WebException e)
            {
                //Try the same song and dance as above but instead using the exception this time so we can at least get the response body.
                using (WebResponse resp = e.Response)
                {
                    using (var reader = new StreamReader(resp.GetResponseStream()))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }
    }
}
