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

            var testTokens = new[] {
                "Product.ID",
                "Product.Title",
                "Product.BodyHtml",
                "Product.Variants.Price.@Min",
                "OrderLineItem.Order.Name",
                "OrderLineItem.Order.CreatedAt",
                "OrderLineItem.Quantity",
                "OrderLineItem.Title",
                "OrderLineItem.VariantTitle",
                "OrderLineItem.Order.CreatedAt",
                //"Product.Visible",
                "OrderLineItem.Product",
                "OrderLineItem.Order.ID",

                "Product.Images[(Position='1'|Position='2');(Position>'0')].Source",
                "OrderLineItem.Properties[Name='Add a Monogram'].Value",
                "OrderLineItem.Properties[Name='Custom Text'].Value",
                "OrderLineItem.Properties[Name='Font'].Value",
                "OrderLineItem.Properties[Name='Thread Color'].Value",
                "OrderLineItem.Properties[Name='Color'].Value",

                "Product.Url", // Calculated?
            };

            foreach(var testToken in testTokens)
            {
                var attr = metaModel.GetAttributeDefinition(testToken);
                Console.WriteLine(attr.Token);
            }

            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
