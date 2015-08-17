using System;
using System.Linq;
using log4net.Config;
using Magnum.TypeScanning;
using V1Antlr.Meta;

namespace V1Antlr
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            var types = TypeScanner.Scan(x => { x.AssembliesFromApplicationBaseDirectory(); }).Where(x=>x.Namespace == "V1Antlr.Sample").ToArray();

            var metaModel = new MetaModel();

            metaModel.RegisterTypes(types);

            var attr = metaModel.GetAttributeDefinition("Product.Variants.Price.@Min");

            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
