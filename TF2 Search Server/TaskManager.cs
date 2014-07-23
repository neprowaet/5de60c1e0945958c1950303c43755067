using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;

namespace TF2_Search_Server
{
    class TaskManager
    {

        public TaskManager()
        {
            if (File.Exists("current.xml"))
            {
                CreateBase("sada");
            }
            else
            {
                XDocument xmlData = XDocument.Load("tasksqueue.xml");

                IEnumerable<XElement> searchHandler =
                    from el in xmlData.Root.Elements()
                    select el;

                CreateBase(searchHandler.Count().ToString());

            }
        }
        void CreateBase(string type)
        {
            switch (type)
            {
                default:

                    break;
            }

            XDocument xmlData = XDocument.Load("current.xml");
            xmlData.Root.Add(new XElement("profiles"));
            //76561198129271308
            for (ulong i = 76561197960287930; i < 76561198129271308; i++)
            {
                Console.Clear();
                Console.WriteLine((i - 76561197960287930).ToString());
                xmlData.Root.Element("profiles").Add(new XElement("profile",
                    new XElement("steamid", i.ToString()),
                    new XElement("d2", new XElement("hours"), new XElement("items")),
                    new XElement("cs", new XElement("hours"), new XElement("items")),
                    new XElement("tf", new XElement("hours"), new XElement("items"))
                    ));
            }
            xmlData.Save("current.xml");
        }
    }
}
