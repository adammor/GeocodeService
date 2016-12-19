using System;
using RestEL.Modules;
using RestEL.Models;
using RestEL.Interfaces;
using System.Collections.Generic;
using JSONHelpers;
using RESTHelpers;

namespace GeocodeService.Modules
{
    [RegisterAPI(APIName = "Google Maps Geocoding API")]
    public class GoogleMaps<T> : IREST<T> where T : StreetAddress
    {
        public string restProtocol { get; set; }
        public string restLink { get; set; }
        public string restParameter { get; set; }
        public string restKey { get; set; }
        public string responseFormat { get; set; }
        public Guid restUID { get; set; }
        public Requestquota restAPILimits { get; set; }

        public GoogleMaps(Providers serviceObject, List<Requestquota> APILimits)
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
                _requestUri.Path = responseFormat;
                var query = !string.IsNullOrEmpty(_strAddress) ? string.Concat("address=", _strAddress) : "";
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
                        if (!json.IsPropertyInJSON(_jsonText, "PropertyName", "partial_match")
                            || obj.results[0].geometry.location_type == "ROOFTOP"
                            || obj.results[0].geometry.location_type == "RANGE_INTERPOLATED" //center of street block
                            )
                        {
                            //update model
                            payload.Latitude = obj.results[0].geometry.location.lat;
                            payload.Longitude = obj.results[0].geometry.location.lng;
                            payload.LocationType = obj.results[0].geometry.location_type;
                            payload.ServiceAPIEndPointUID = restUID;
                        } else
                        {
                            //TODO: add bad match code
                            payload.LocationType = "no_good_match";  //TODO: a retry operation should be added (but lower priority than changed RecordSet)
                        }                            
                    }
                    else //if not OK
                    { 
                        //payload.LocationType = obj.status;                          
                    } 
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
                    //}
                }
            }



            return payload;
        }


        public string StringifyPayload(T address) 
        {
            //replace any nulls on less essential fields before concatenating to the returned string
            address.PostalCode = address.PostalCode == null ? "" : address.PostalCode.ToString().TrimEnd() + ",";
            address.Country = address.Country == null ? "" : address.Country.ToString();

            string _strAddress = string.Concat(
                            address.Address.ToString(), ",", 
                            address.City.ToString(), ",",
                            address.Region.ToString(), " ",
                            address.PostalCode.ToString(),
                            address.Country.ToString()
                            );

            return _strAddress;
        }


        public string CallProvider(string requestUri)
        {
            return RESTHelper.CallProvider(requestUri);
        }
    }
}