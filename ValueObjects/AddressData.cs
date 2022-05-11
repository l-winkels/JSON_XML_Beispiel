using System;

namespace ValueObjects
{
    public class AddressData
    {
        public String FirstName { get; private set; }
        public String LastName { get; private set; }
        public String Street { get; private set; }
        public String City { get; private set; }        
        public String Country { get; private set; }
        public String ZipCode { get; private set; }

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
