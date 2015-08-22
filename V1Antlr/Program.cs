using System;
using System.Linq;
using log4net.Config;
using Magnum.TypeScanning;
using V1Antlr.Data;
using V1Antlr.Meta;
using V1Antlr.Sample;

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

                "Order.LineItems.Properties[Name='Color'].Value",
                "Order.LineItems[+Properties;Quantity>'0'].Properties[Value!=''].Value"

                //"Product.Url", // Calculated?
            };

            foreach(var testToken in testTokens)
            {
                var attr = metaModel.GetAttributeDefinition(testToken);
                Console.WriteLine(attr.Token);
            }

            Query query = QueryBuilder.For("Product", metaModel)
                .Select("ID","Title","BodyHtml","Variants")
                .Where("ID>'1'")
                .Order("Title","-BodyHtml", "+ID")
                .Skip(1)
                .Take(1)
                .ToQuery();

            var products = new Product[]
                           {
                               new Product { ID = 1, Title = "Product #1", BodyHtml = "Product #1 Body" },
                               new Product { ID = 2, Title = "Product #2", BodyHtml = "Product #2 Body" },
                               new Product { ID = 3, Title = "Product #3", BodyHtml = "Product #3 Body" },
                           };

            var selected = products.AsQueryable().ApplyQuery(query);

            var sortQuery = QueryBuilder.For("Product", metaModel).Select("ID").Order("-ID").ToQuery();

            var sorted = products.AsQueryable().ApplyQuery(sortQuery);

            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
