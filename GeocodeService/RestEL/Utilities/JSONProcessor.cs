using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using NJsonSchema;
using System;
using System.Collections.Generic;
using System.IO;


namespace JSONHelpers
{
    public class JSONProcessor 
    {
        public bool IsJSONValid { get; private set; }
        public List<string> ErrorMessages { get; private set; }
        
        public JSONProcessor()
        {
            this.IsJSONValid = false;
        }

        public void PrepJSONSafely(string jsonString)
        {
            try
            {
                JToken.Parse(jsonString);;
                IsJSONValid = true;
            }
            catch (JsonException ex)
            {
                IsJSONValid = false;
                ErrorMessages.Add(ex.Message);  //gives line and position in json that failed to meet validation
            }
            catch (Exception ex)
            {
                IsJSONValid = false;
                Console.WriteLine(ex);
            }
        }

        public bool IsPropertyInJSON(string jsonString, string propertyName, string propertyValue)
        {
            if (IsJSONValid == false) { PrepJSONSafely(jsonString); }  //if not already checked;    

            var _hasProperty = false;

            try
            {
                using (var _reader = new JsonTextReader(new StringReader(jsonString)))
                {
                    while (_reader.Read())
                    {
                        if (_reader.TokenType.ToString() == propertyName
                            && _reader.Value.ToString() == propertyValue)
                        {
                            _hasProperty = true;
                        }
                    }
                }
            }
            catch (JsonReaderException ex)
            {
                ErrorMessages.Add(ex.Message);  
            }
            
            return _hasProperty;
        }

        public T ConvertJsonToType<T>(string jsonString)
        {
            if (IsJSONValid == false) { PrepJSONSafely(jsonString); }  //if not already checked;      

            JObject JsonObject = JObject.Parse(jsonString);

            JSchema schema = JSONProcessor.GenerateSchemaForClass<T>();  //temporarily generate JsonObject schema from class   
            IList<string> _jsonSchemaInvalidationMessage = null;      
            var HasValidSchema = JsonObject.IsValid(schema, out _jsonSchemaInvalidationMessage);

            T _result = default(T);
            if (HasValidSchema) //check if user-defined JsonObject valid against corresponding schema 
            {
                return JsonConvert.DeserializeObject<T>(JsonObject.ToString());  //convert JObject to String to Type
            }
            else
            {
                foreach (var msg in _jsonSchemaInvalidationMessage)
                {
                    ErrorMessages.Add(msg);  //gives line and position in json that failed to meet validation
                    _result = default(T);

                    throw new JSchemaException();

                }
            }
               
            return _result;
        }


        public static JSchema GenerateSchemaForClass<T>()
        {
            //Name: NJsonSchema for .NET (works with JSON.NET)
            //Owner: Rico Suter 
            //Purpose: Generates json schemas from .NET classes; unlike JSON.NET, can generate an unlimited number.
            JsonSchema4 schema = null;
            try
            {
                schema = JsonSchema4.FromType<T>(); //generate json schema from .NET class        
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            //Name: Json.NET
            //Owner: James Newton-King
            //Purpose: format json schema into json 
            //History: replaces JsonSchema, which is deprecated (may need to bring back if JSchema doesn't allow unlimited use)
            JSchema schemaData = null;
            try
            {
                schemaData = JSchema.Parse(schema.ToJson()); 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return schemaData;
        }
    }
}