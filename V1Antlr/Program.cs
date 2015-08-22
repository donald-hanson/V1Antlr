using System;
using System.Collections.Generic;
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
                .Select("ID", "Title", "BodyHtml", "Variants.@Count", "Variants")
                .Where("Variants;ID>'0'")
                .Order("Title", "-BodyHtml", "+ID")
                .Skip(1)
                .Take(10)
                .ToQuery();

            var products = new Product[]
                           {
                               new Product
                               {
                                   ID = 1,
                                   Title = "Product #1",
                                   BodyHtml = "Product #1 Body",
                               },
                               new Product
                               {
                                   ID = 2,
                                   Title = "Product #2",
                                   BodyHtml = "Product #2 Body",
                               },
                               new Product
                               {
                                   ID = 3,
                                   Title = "Product #3",
                                   BodyHtml = "Product #3 Body",
                                   Variants = new List<ProductVariant>
                                              {
                                                  new ProductVariant
                                                  {
                                                      ID = 30,
                                                      Price = 30.30m,
                                                      Quantity = 30,
                                                      Title = "Variant 30"
                                                  },
                                                  new ProductVariant
                                                  {
                                                      ID = 31,
                                                      Price = 31.31m,
                                                      Quantity = 31,
                                                      Title = "Variant 31"
                                                  },
                                                  new ProductVariant
                                                  {
                                                      ID = 32,
                                                      Price = 32.32m,
                                                      Quantity = 32,
                                                      Title = "Variant 32"
                                                  },
                                              }
                               },
                               new Product
                               {
                                   ID = 4,
                                   Title = "Product #4",
                                   BodyHtml = "Product #4 Body",
                                   Variants = new List<ProductVariant>
                                              {
                                                  new ProductVariant
                                                  {
                                                      ID = 40,
                                                      Price = 40.40m,
                                                      Quantity = 40,
                                                      Title = "Variant 40"
                                                  },
                                                  new ProductVariant
                                                  {
                                                      ID = 41,
                                                      Price = 41.41m,
                                                      Quantity = 41,
                                                      Title = "Variant 41"
                                                  },
                                                  new ProductVariant
                                                  {
                                                      ID = 42,
                                                      Price = 42.42m,
                                                      Quantity = 42,
                                                      Title = "Variant 42"
                                                  },
                                                  new ProductVariant
                                                  {
                                                      ID = 43,
                                                      Price = 43.43m,
                                                      Quantity = 43,
                                                      Title = "Variant 43"
                                                  },
                                              }
                               },
                           };

            var sortQuery = QueryBuilder.For("Product", metaModel).Select("ID").Order("-ID").ToQuery();

            var sorted = products.AsQueryable().ApplyQuery(sortQuery);

            var selected = products.AsQueryable().ApplyQuery(query);

            foreach (var asset in selected.Assets)
            {
                Console.WriteLine("==========");
                foreach (var attributeDefinition in query.Selection)
                {
                    var value = asset.GetAttributeValue<object>(attributeDefinition);
                    Console.WriteLine("\t{0}={1}", attributeDefinition.Name, value);
                }
            }


            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
