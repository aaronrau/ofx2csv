using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Globalization;

using ofx2csv.Model;

namespace ofx2csv
{
    class Program
    {
        static void Main(string[] args)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;

            if (args.Length == 0)
            {
                Console.WriteLine("Please enter name and path of OFX file. Example ofx2csv c:\\myaccount.ofx");

                return;

            }
            FileInfo d = new FileInfo(args[0]);
            OfxDocument document = new OfxDocument(new FileStream(args[0], FileMode.Open));
       
            using (System.IO.StreamWriter fs = new System.IO.StreamWriter(d.DirectoryName+"\\"+d.Name.Replace(".ofx",".csv")))
            {
                fs.WriteLine("Date,Description,Original Description,Amount,Transaction");
                foreach (Transaction trans in document.Transactions)
                {
                    Console.WriteLine(trans.Name + trans.TransType + trans.TransAmount);
          
                    string format = "yyyyMMddHHmmss";
                    string formattedDate = "";
                    try
                    {
                        DateTime result = DateTime.ParseExact(trans.DatePosted, format, provider);
                        Console.WriteLine("{0} converts to {1}.", trans.DatePosted, result.ToString());
                        formattedDate = result.ToString();
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("{0} is not in the correct format.", trans.DatePosted);
                    }


                    fs.WriteLine(formattedDate + "," + trans.Name + "," + trans.Memo + "," + trans.TransAmount + "," + trans.TransType);
                }
            }

            

            int i = 0;
        }
    }
}
