using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mozu.Api;
using Autofac;
using Mozu.Api.ToolKit.Config;

namespace Mozu_BED_Training_Exercise_9_1
{
    [TestClass]
    public class MozuDataConnectorTests
    {
        private IApiContext _apiContext;
        private IContainer _container;

        [TestInitialize]
        public void Init()
        {
            _container = new Bootstrapper().Bootstrap().Container;
            var appSetting = _container.Resolve<IAppSetting>();
            var tenantId = int.Parse(appSetting.Settings["TenantId"].ToString());
            var siteId = int.Parse(appSetting.Settings["SiteId"].ToString());

            _apiContext = new ApiContext(tenantId, siteId);
        }

        [TestMethod]
        public void Exercise_9_1_Get_Attribute()
        {
            /* --Create a New Product Attribute Resource
             *     Resources are used to leverage the methods provided by the SDK to talk to the Mozu service
             *     via the Mozu REST API. Every resource takes an ApiContext object as a parameter.
             */
            var productAttributeResource = new Mozu.Api.Resources.Commerce.Catalog.Admin.Attributedefinition.AttributeResource(_apiContext);
            /*
             * --Utilize the Product Attribute Resource to Get All Product Attributes Returned as an AttributeCollection
             *     Product attributes are the properties of the product with differing types based on your needs.
             *     Define your product attribute as Options, Properties, or Extras
             *     —the following properties are accessible from a Product Attribute object:
             *     productAttribute.AdminName -- string
             *     productAttribute.AttributeCode -- string
             *     productAttribute.AttributeDataTypeSequence -- int
             *     productAttribute.AttributeFQN -- string
             *     productAttribute.AttributeMetadata -- List<AttributeMetadataItem>
             *     productAttribute.AttributeSequence -- int
             *     productAttribute.AuditInfo -- AuditInfo
             *     productAttribute.Conent -- Content (This object contains the name, description, and locale code for identifying language and country.)
             *     productAttribute.DataType -- string
             *     
             *     (Types include "List", "Text box", "Text area", "Yes/No", and "Date")
             *     productAttribute.InputType -- string
             *     
             *     (Used to identify the characteristics of an attribute)
             *     productAttribute.IsExtra -- bool
             *     productAttribute.IsOption -- bool
             *     productAttribute.IsProperty -- bool
             *     
             *     productAttribute.LocalizedContent -- List<AttributeLocalizedContent>
             *     productAttribute.MasterCatalogId -- int
             *     productAttribute.Namespace -- string 
             *     productAttribute.SearchSettings -- SearchSettings
             *     
             *     (Used to store properties such as max/min date, max/min numeric value, max/min string length, or a regular expression)
             *     productAttribute.Validation -- Validation
             *     productAttribute.ValueType -- string
             *     
             *     (Used to store predefined values, such as options in a List)
             *     productAttribute.VocabularyValues -- List<AttributeVocabularyValue>
             * 
             * 
             *     See the following sites for more info:
             *     http://developer.mozu.com/content/learn/appdev/product-admin/product-attribute-definition.htm
             *     http://developer.mozu.com/content/api/APIResources/commerce/commerce.catalog/commerce.catalog.admin.attributedefinition.attributes.htm
             */
            var productAttributes = productAttributeResource.GetAttributesAsync(startIndex: 0, pageSize: 200).Result;

            //Add Your Code: 
            //Write Total Count of Attributes

            System.Diagnostics.Debug.WriteLine(string.Format("Product Attributes Total Count: {0}", productAttributes.TotalCount));

            //Add Your Code: 
            //Get an Attribute by Fully Qualified Name

            var individualAttribute = productAttributeResource.GetAttributeAsync("tenant~rating").Result;

            //Note: AttributeFQN (fully qualified name) follows the naming convention tenant~attributeName

            //Add Your Code:
            //Write the Attribute Data Type

            System.Diagnostics.Debug.WriteLine(string.Format("Product Attribute Data Type[{0}]: {1}", individualAttribute.AttributeCode, individualAttribute.DataType));

            //Add Your Code:
            //Write the Attribute Input Type

            System.Diagnostics.Debug.WriteLine(string.Format("Product Attribute Input Type[{0}]: {1}", individualAttribute.AttributeCode, individualAttribute.InputType));

            //Add Your Code:
            //Write the Attribute Characteristics 

            System.Diagnostics.Debug.WriteLine(string.Format("Product Attribute Characteristic [{0}]: An Extra? {1}, An Option? {2}, A Property? {3}", individualAttribute.AttributeCode, individualAttribute.IsExtra, individualAttribute.IsOption, individualAttribute.IsProperty));

            //Or...

            WriteAttributeCharacteristic(individualAttribute);

            //Add Your Code: 
            if(individualAttribute.VocabularyValues != null)
            {
                foreach (var value in individualAttribute.VocabularyValues)
                {
                    //Write vocabulary values
                    System.Diagnostics.Debug.WriteLine(string.Format("Product Attribute Vocabulary Values[{0}]: Value({1}) StringContent({2})", individualAttribute.AttributeCode, value.Value, value.Content.StringValue));
                }
            }

            //Add Your Code: 
            //Get an Attribute filtered by name
            //Note: See this page for more info about filters:
            //http://developer.mozu.com/content/api/APIResources/StandardFeatures/FilteringAndSortingSyntax.htm

            var filteredAttributes = productAttributeResource.GetAttributesAsync(filter: "adminName sw 'a'").Result;

            var singleAttributeFromFiltered = new Mozu.Api.Contracts.ProductAdmin.Attribute();

            if(filteredAttributes.TotalCount > 0)
            {
                singleAttributeFromFiltered = filteredAttributes.Items[0];
            }

            //Add Your Code:
            if (singleAttributeFromFiltered.AttributeCode != null)
            {
                //Write the Attribute Data Type

                System.Diagnostics.Debug.WriteLine(string.Format("Product Attribute Data Type[{0}]: {1}", singleAttributeFromFiltered.AttributeCode, singleAttributeFromFiltered.DataType));

                //Write the Attribute Input Type

                System.Diagnostics.Debug.WriteLine(string.Format("Product Attribute Input Type[{0}]: {1}", singleAttributeFromFiltered.AttributeCode, singleAttributeFromFiltered.InputType));

                //Write the Attribute Characteristics

                System.Diagnostics.Debug.WriteLine(string.Format("Product Attribute Characteristic [{0}]: An Extra? {1}, An Option? {2}, A Property? {3}", singleAttributeFromFiltered.AttributeCode, singleAttributeFromFiltered.IsExtra, singleAttributeFromFiltered.IsOption, singleAttributeFromFiltered.IsProperty));

                //Or...

                WriteAttributeCharacteristic(singleAttributeFromFiltered);

                if (singleAttributeFromFiltered.VocabularyValues != null)
                {
                    foreach (var value in singleAttributeFromFiltered.VocabularyValues)
                    {
                        //Write vocabulary values
                        System.Diagnostics.Debug.WriteLine(string.Format("Product Attribute Vocabulary Values[{0}]: Value({1}) StringContent({2})", singleAttributeFromFiltered.AttributeCode, value.Value, value.Content.StringValue));
                    }
                }
            }
        }

        /// <summary>
        /// Helper method for checking the characteristic of a returned Product Attribute
        /// </summary>
        /// <param name="Attribute"></param>
        private void WriteAttributeCharacteristic(Mozu.Api.Contracts.ProductAdmin.Attribute individualAttribute)
        {
            System.Diagnostics.Debug.Write(string.Format("Product Attribute Characteristic[{0}]: ", individualAttribute.AttributeCode));

            if ((bool)individualAttribute.IsExtra)
            {
                System.Diagnostics.Debug.Write("Is an Extra");
            }
            else if ((bool)individualAttribute.IsOption)
            {
                System.Diagnostics.Debug.Write("Is an Option");
            }
            else if ((bool)individualAttribute.IsProperty)
            {
                System.Diagnostics.Debug.Write("Is a Property");
            }
            else
            {
                System.Diagnostics.Debug.Write("Has no characteristic");
            }
        }
    }
}
