using System.Configuration;
using System.Security.Cryptography;
using System.Windows.Media;
using WpfConfiguratorLib.attributes;
using WpfConfiguratorLib.entities;

namespace WpfConfiguratorTester
{
    public class TestData : ConfigGroup
    {
        public override string DisplayName
        {
            get { return "Test Data"; }
        }

        public override string Description
        {
            get { return "All of the test stuff goes here"; }
        }

        public override SolidColorBrush Brush
        {
            get { return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0F465D")); }
        }

        [ConfigProperty("Upgrade Required", DefaultValue = true, IsPrivate = true)]
        public bool UpgradeRequired { get; set; }

        [ConfigProperty("First Name", Description = "Your first name")]
        public string FirstName { get; set; }

        [ConfigProperty("Last Name",Description = "Your last name")]
        public string LastName { get; set; }

        [ConfigGroupPropertyAttribute("Billing Address", Description = "The address to which your bill will be sent", Color = "#1997CA")]
        public AddressData Address { get; set; }

        [ConfigGroupPropertyAttribute("Shipping Address", Color = "#FF0000")]
        public AddressData Address2 { get; set; }
    }

    public class AddressData : ConfigGroup
    {
        public override string DisplayName
        {
            get { return "Address"; }
        }

        public override string Description
        {
            get { return "A mailing address"; }
        }

        [ConfigProperty("Street Number", DefaultValue = "", Description = "The number of the building")]
        public string StreetNumber { get; set; }

        [ConfigProperty("Street Name", DefaultValue = "", Description = "The name of the street on which the building resides.")]
        public string StreetName { get; set; }

        [ConfigProperty("City", DefaultValue = "")]
        public string City { get; set; }

        [ConfigProperty("State", DefaultValue = "")]
        public string State { get; set; }

        [ConfigProperty("Zip Code", DefaultValue = "")]
        public string ZipCode { get; set; }
    }
}