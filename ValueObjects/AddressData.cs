using System;

namespace ValueObjects
{
    public class AddressData
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Street { get; set; }
        public String City { get; set; }        
        public String Country { get; set; }
        public String ZipCode { get; set; }

        /// <summary>
        /// Default initialization without values is not allowed.
        /// </summary>
        private AddressData() { }

        public AddressData(string firstname, string lastname, string street, string city, string country, string zipcode)
        {
            FirstName = firstname;
            LastName = lastname;
            Street = street;
            City = city;            
            Country = country;
            ZipCode = zipcode;
        }

        /// <summary>
        /// String representation of AddressData
        /// </summary>
        /// <returns>String with the address data</returns>
        public override string ToString()
        {
            return FirstName + " " + LastName + Environment.NewLine
                    + Street + Environment.NewLine
                    + ZipCode + " " + City + Environment.NewLine                    
                    + Country + Environment.NewLine;
        }
    }
}
