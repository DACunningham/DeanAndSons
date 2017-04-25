using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Device.Location;

namespace DeanAndSons.Models
{
    public abstract class Contact
    {
        [Key]
        public int ContactID { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 1)]
        [Display(Name = "Property Number/Name")]
        public string PropertyNo { get; set; }

        [Required]
        [StringLength(75, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 3)]
        public string Street { get; set; }

        [Required]
        [StringLength(75, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 3)]
        public string Town { get; set; }

        [Required]
        [StringLength(9, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 3)]
        [Display(Name = "Post Code")]
        public string PostCode { get; set; }

        public double Lat { get; set; }
        public double Long { get; set; }

        [Display(Name = "Telephone Number")]
        public int? TelephoneNo { get; set; }

        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required]
        public DateTime Created { get; set; } = DateTime.Now;

        [Required]
        public DateTime LastModified { get; set; } = DateTime.Now;

        [Required]
        public bool Deleted { get; set; } = false;

        public Contact()
        {

        }

        public Contact(string propertyNo, string street, string town, string postCode, int? telephoneNo, string email)
        {
            PropertyNo = propertyNo;
            Street = street;
            Town = town;
            PostCode = postCode.Trim();
            TelephoneNo = telephoneNo;
            Email = email;

            getLatLong();
        }

        private void getLatLong()
        {
            //string geoCodedAddr = BuildGeoCodeAddressStr();
            string fullUri = "https://maps.googleapis.com/maps/api/geocode/json?address=" + PostCode + "&key=AIzaSyD5D--EeZyEenp7k8zWpvJuRI77NF-4FMM";
            string JsonResponse = "";
            double resultLat = 10.1;
            double resultLong = 10.2;

            try
            {
                using (WebClient wc = new WebClient())
                {
                    using (StreamReader sr = new StreamReader(wc.OpenRead(fullUri)))
                    {
                        JsonResponse = sr.ReadToEnd();
                    }
                }

                GoogleAddressJ gaJson = JsonConvert.DeserializeObject<GoogleAddressJ>(JsonResponse);

                //Check a valid address was received by Google and assign data from Google's response to appropriate properties of EventAddress instance.
                if (gaJson.status == "OK")
                {
                    if (Double.TryParse(gaJson.results[0].geometry.location.lat, out resultLat))
                    {
                        Lat = resultLat;
                    }
                    if (Double.TryParse(gaJson.results[0].geometry.location.lng, out resultLong))
                    {
                        Long = resultLong;
                    }
                    //FormattedAddr = gaJson.results[0].formatted_address;
                }
                else if (gaJson.status == "ZERO_RESULTS")
                {
                    throw new WebException();
                }
            }
            catch (WebException ex)
            {
                Lat = 51.375814;
                Long = -2.359904;
            }

            
        }

        public static GeoCoordinate GetLocationLatLng(string location)
        {
            GeoCoordinate LatLong;

            switch (location)
            {
                case "Bath":
                    LatLong = new GeoCoordinate(51.375814, -2.359904);
                    break;
                case "Chippenham":
                    LatLong = new GeoCoordinate(51.461552, -2.119497);
                    break;
                case "Corsham":
                    LatLong = new GeoCoordinate(51.431494, -2.189678);
                    break;
                case "Devizes":
                    LatLong = new GeoCoordinate(51.349174, -1.994857);
                    break;
                case "Melksham":
                    LatLong = new GeoCoordinate(51.371077, -2.137699);
                    break;
                case "Swindon":
                    LatLong = new GeoCoordinate(51.556332, -1.779642);
                    break;
                case "Trowbridge":
                    LatLong = new GeoCoordinate(51.319872, -2.208825);
                    break;
                default:
                    LatLong = new GeoCoordinate(51.375814, -2.359904);
                    break;
            }

            return LatLong;
        }
    }

    public class GoogleAddressJ
    {
        public string status { get; set; }
        public results[] results { get; set; }

    }

    public class results
    {
        public string formatted_address { get; set; }
        public geometry geometry { get; set; }
        public string[] types { get; set; }
        public address_component[] address_components { get; set; }
    }

    public class geometry
    {
        public string location_type { get; set; }
        public location location { get; set; }
    }

    public class location
    {
        public string lat { get; set; }
        public string lng { get; set; }
    }

    public class address_component
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public string[] types { get; set; }
    }
}
