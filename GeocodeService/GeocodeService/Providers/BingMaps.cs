using System;
using RestEL.Modules;
using RestEL.Models;
using RestEL.Interfaces;
using System.Collections.Generic;
using JSONHelpers;
using RESTHelpers;

namespace GeocodeService.Modules
{
    [RegisterAPI(APIName = "Bing Maps API")]
    public class BingMaps<T> : IREST<T> where T : StreetAddress
    {
        public string restProtocol { get; set; }
        public string restLink { get; set; }
        public string restParameter { get; set; }
        public string restKey { get; set; }
        public string responseFormat { get; set; }
        public Guid restUID { get; set; }
        public Requestquota restAPILimits { get; set; }


        public BingMaps(Providers serviceObject, List<Requestquota> APILimits)
        {
            restProtocol = serviceObject.endpoint.protocol;
            restLink = serviceObject.endpoint.baseURL;
            responseFormat = serviceObject.endpoint.responseFormat;
            restParameter = serviceObject.endpoint.parameter;
            restKey = serviceObject.endpoint.key;
            restUID = serviceObject.GUID;        //add endpoint db value to model
            restAPILimits = APILimits[0];
        }
        

        T IREST<T>.GetDataAsync(T payload)
        {
            //must meet minimum parts of an rec
            if (
                    !string.IsNullOrEmpty(payload.Address)
                && !string.IsNullOrEmpty(payload.City)
                && !string.IsNullOrEmpty(payload.Region)
            )
            {
                string _strAddress = StringifyPayload(payload);
                
                var _requestUri = new UriBuilder();
                _requestUri.Scheme = restProtocol;
                _requestUri.Host = restLink;
                _requestUri.Path = _strAddress.Replace(" ", "+");
                var query = !string.IsNullOrEmpty(_strAddress) ? string.Concat("o=", responseFormat) : "";
                query = !string.IsNullOrEmpty(restKey) ? query + string.Concat("&key=", restKey) : "";
                _requestUri.Query = query;
                
                var _jsonText = CallProvider(_requestUri.ToString());

                var json = new JSONProcessor();
                json.PrepJSONSafely(_jsonText);

                if (json.IsJSONValid)
                {
                    dynamic obj = json.ConvertJsonToType<dynamic>(_jsonText);

                    if (obj.status == "OK")
                    {
                        if (obj["resourceSets"][0]["resources"][0]["entityType"].ToString() == "Address"
                             || obj["resourceSets"][0]["resources"][0]["confidence"].ToString() == "High" 
                             || obj["resourceSets"][0]["resources"][0]["confidence"].ToString() == "Medium"
                            )
                        {
                            //update model
                            payload.Latitude = Single.Parse(obj["resourceSets"][0]["resources"][0]["geocodePoints"][0]["coordinates"][0].ToString());
                            payload.Longitude = Single.Parse(obj["resourceSets"][0]["resources"][0]["geocodePoints"][0]["coordinates"][1].ToString());
                            payload.LocationType = obj["resourceSets"][0]["resources"][0]["entityType"].ToString();
                            payload.ServiceAPIEndPointUID = restUID;
                        } else
                        {
                            //TODO: add bad match code
                           // payload.LocationType = "no_good_match";  //TODO: a retry operation should be added (but lower priority than changed RecordSet)
                        }                            
                    //}
                    //else //if not OK
                    //{ 
                    //    //payload.LocationType = obj.status;                          
                    //} 
                    //else if (obj.status == "ZERO_RESULTS")
                    //{ //non-existant rec                            
                    //}
                    //else if (obj.status == "OVER_QUERY_LIMIT")
                    //{ //
                    //}
                    //else if (obj.status == "REQUEST_DENIED")
                    //{ //                        
                    //}
                    //else if (obj.status == "INVALID_REQUEST")
                    //{ //
                    //}
                    //else if (obj.status == "UNKNOWN_ERROR")
                    //{ //
                    }
                }
            }

            return payload;
        }


        public string StringifyPayload(T address) 
        {
            //replace any nulls on less essential fields before concatenating to the returned string
            address.PostalCode = address.PostalCode == null ? "" : address.PostalCode.ToString().TrimEnd();
            address.Country = address.Country == null ? "" : address.Country.ToString();

            string _strAddress =  string.Concat(
                                address.Address.ToString() , ", ",
                                address.City.ToString(), ", ",
                                address.Region.ToString(), ", ",
                                address.Country.ToString(), ", ",
                                address.PostalCode.ToString()
                                );

            return _strAddress;
        }

        public string CallProvider(string requestUri)
        {
           return RESTHelper.CallProvider(requestUri);
        }


    }
}
