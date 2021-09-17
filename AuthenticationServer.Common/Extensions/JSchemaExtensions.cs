using AuthenticationServer.Common.Constants.Token;
using AuthenticationServer.Common.Models.ContractModels;
using Newtonsoft.Json.Schema;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationServer.Common.Extentions
{
    public static class JSchemaExtensions
    {
       /* private const string V = "string";

        public static JSchema ToJSchema(this List<ClaimConfig> tokenItems)
        {
            StringBuilder schemaString = new ();

            schemaString.Append(@"{
""type"": ""object"",
""properties"":
{
");

            foreach (ClaimConfig item in tokenItems)
            {
                
            }

            schemaString.Append("}");

            schemaString.Append(@"""required"" : [");
            for (int i = 0; i < tokenItems.Count; i++)
            {
                schemaString.Append($@"""{tokenItems[i].JwtName}""");

                if (i != (tokenItems.Count - 1))
                    schemaString.Append(",");
            }

            schemaString.Append(@"]
},
""additionalProperties"": false
}");

            return JSchema.Parse(schemaString.ToString());
        }


        private static void schemaBuilder(StringBuilder schemaString, ClaimConfig item)
        {
            if (item.Type != ClaimType.array)
                schemaString.Append($@"""{item.JwtName}"": {{""type"": {(item.Type == ClaimType.text ? V : item.Type)}, ""description"": ""{item.Description}""}}");
            else
            {
                schemaString.Append($@"""{item.JwtName}"": {{""type"": {item.Type}, ""enum"": [{{");

                for(int i = 0; i < item.ClaimConfigurations.Count; i++)
                {
                    schemaBuilder(schemaString, item.ClaimConfigurations[i]);
                    schemaString.Append("}");

                    if (i != (item.ClaimConfigurations.Count - 1))
                        schemaString.Append(",");
                }    
            }
        }*/
    }
}
