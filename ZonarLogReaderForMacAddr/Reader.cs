using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ZonarLogReaderForMacAddr
{
    public class fileReading
    {
        public void ReadFiles()
        {
            List<Model> data = new List<Model>();
            DirectoryInfo di = new DirectoryInfo(@"C:\Users\uie88548\Documents\Zonar_SWL_Reports");
            FileInfo[] files = di.GetFiles("*.log");//00002122D0
            foreach (FileInfo file in files)
            {
                string contents = File.ReadAllText($@"C:\Users\uie88548\Documents\Zonar_SWL_Reports\{file.Name}");
                string serialnumber = Path.GetFileNameWithoutExtension(file.Name).Remove(0, 16);
                string localaddress = innerLoop(contents, "config btLocalAddress=", 22, 39);
                string macaddress = innerLoop(contents, "WIFI-MACADDR string ", 20, 32);
                string macaddress1;
                if (Path.GetFileNameWithoutExtension(file.Name).Equals("Report_Test_Run_33000857") || Path.GetFileNameWithoutExtension(file.Name).Equals("Report_Test_Run_33000929") || Path.GetFileNameWithoutExtension(file.Name).Equals("Report_Test_Run_33000952") || Path.GetFileNameWithoutExtension(file.Name).Equals("Report_Test_Run_33000958"))
                {
                    macaddress1 = innerLoop(contents, "AP1", 4, 16);
                }
                else if(Path.GetFileNameWithoutExtension(file.Name).Equals("Report_Test_Run_33000957"))
                {
                    macaddress1 = innerLoop(contents, "WIFI-MACADDR-A(192.168.1.5)P1", 30, 42); 
                }
                else
                {
                    macaddress1 = innerLoop(contents, "WIFI-MACADDR-AP1 string", 24, 36);
                }
                string macaddress2;
                if (Path.GetFileNameWithoutExtension(file.Name).Equals("Report_Test_Run_33000821"))
                {
                    macaddress2 = innerLoop(contents, "WIFI-MACADDR-A(192.168.1.1)P2", 30, 42);
                }
                else if(Path.GetFileNameWithoutExtension(file.Name).Equals("Report_Test_Run_33000852"))
                {
                    macaddress2 = innerLoop(contents, "WIFI-MACADDR-AP2(192.168.1.6)", 30, 42);
                }
                else
                {
                    macaddress2 = innerLoop(contents, "MACADDR-AP2 string ", 19, 31);
                }
                if (macaddress2 == "Number: 3300")
                {
                    macaddress2 = innerLoop(contents, "AP2", 4, 16);
                }
                
                data.Add(new Model() { fileName = file.Name, serialNumber = serialnumber, localAddress = localaddress,wifiMacAddress = macaddress, wifiMacAddress1 = macaddress1, wifiMacAddress2 = macaddress2 });
            }
            CreateCSV(data, @"C:\Users\uie88548\Documents\Zonar_SWL_Reports\ZonarMacs.csv");
        }

        public string innerLoop(string contents, string find, int character1, int character2)
        {
            string seged = "";
            for (int i = contents.LastIndexOf(find) + character1; i < contents.LastIndexOf(find) + character2; i++)
            {
                seged += contents[i];
            }
            return seged;
        }

        //public void WriteCSV<Model>(IEnumerable<Model> items, string path)
        //{
        //    Type itemType = typeof(Model);
        //    var props = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
        //                        .OrderBy(p => p.Name);

        //    using (var writer = new StreamWriter(path))
        //    {
        //        writer.WriteLine(string.Join(", ", props.Select(p => p.Name)));

        //        foreach (var item in items)
        //        {
        //            writer.WriteLine(string.Join(", ", props.Select(p => p.GetValue(item, null))));
        //        }
        //    }
        //}
        public static void CreateCSV(List<Model> list, string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                CreateHeader(list, sw);
                CreateRows(list, sw);
            }
        }

        private static void CreateHeader(List<Model> list, StreamWriter sw)
        {
            MemberInfo[] members = typeof(Model).GetMembers();
            for (int i = 5; i < members.Length; i++)
            {
                Console.WriteLine(members[i].Name);
                sw.Write(members[i].Name + ";");
            }
            sw.Write(sw.NewLine);
        }

        private static void CreateRows(List<Model> list, StreamWriter sw)
        {
            foreach (var item in list)
            {
                MemberInfo[] properties = typeof(Model).GetMembers();
                string value = "";
                for (int i = 5; i < properties.Length; i++)
                {
                    var prop = properties[i];

                    switch (prop.Name)
                    {
                        case "fileName":
                            {
                                value = item.fileName;
                                break;
                            }
                        case "serialNumber":
                            {
                                value = item.serialNumber;
                                break;
                            }
                        case "localAddress":
                            {
                                value = item.localAddress;
                                break;
                            }
                        case "wifiMacAddress":
                            {
                                value = item.wifiMacAddress;
                                break;
                            }
                        case "wifiMacAddress1":
                            {
                                value = item.wifiMacAddress1;
                                break;
                            }
                        case "wifiMacAddress2":
                            {
                                value = item.wifiMacAddress2;
                                break;
                            }
                    }
                    sw.Write(value + ";");
                }
                sw.Write(sw.NewLine);
            }
        }
    }
}