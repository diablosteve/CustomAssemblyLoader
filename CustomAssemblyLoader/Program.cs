using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ICustomDataHandler;

namespace CustomAssemblyLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                List<string> dataPath = new List<string>();
                int refreshTime = 10; //In minute
                List<string> customNamespaceClass = new List<string>();

                List<Type> types = new List<Type>();
                string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                List<string> dllNames = new List<string>();

                foreach (string dll in Directory.GetFiles(path, "*.dll"))
                {
                    dllNames.Add(dll);
                }

                foreach (string dll in dllNames)
                {
                    try
                    {
                        Assembly dllFile = Assembly.LoadFile(dll);
                        /**
                        *  Load the DLL and collect the inner classes
                        *  
                        *  // Specific Class call
                        *  // Type dllType = dllFile.GetType("KelerDataHandler.KelerDataHandler");
                        */
                        foreach (var type in dllFile.ExportedTypes)
                        {
                            var typeInterfaces = type.GetInterfaces();
                            if (typeInterfaces.Length > 0)
                            {
                                /**
                                *  If the class inherit from an interface, check if it exactly the IShadowData interface
                                */
                                foreach (var typeInterface in typeInterfaces)
                                {
                                    if (typeInterface.Name == "IShadowData")
                                    {
                                        types.Add(type);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogWriter(ex.ToString());
                    }
                }

                /**
                 *  Invoke Run method from collected specific classes
                 */
                foreach (Type myType in types)
                {
                    MethodInfo myMethod = myType.GetMethod("Run");
                    object obj = Activator.CreateInstance(myType);
                    var item = myMethod.Invoke(obj, null);
                }
            }
            catch (Exception ex)
            {
                Logger.LogWriter(ex.ToString());
            }
        }
    }
}
