using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Security;
using System.Windows.Documents;
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

        [ConfigProperty("First Name", Description = "Your first name", DefaultValue = "Fred")]
        public string FirstName { get; set; }

        [ConfigProperty("Last Name", Description = "Your last name")]
        public string LastName { get; set; }

        [ConfigProperty("Password", Description = "A big secret", DefaultValue = default(SecureString))]
        public SecureString Password { get; set; }

        [ConfigProperty("Account active", Description = "If the account is currently active or not")]
        public bool IsAccountActive { get; set; }

        [ConfigProperty("Account Type", Description = "The type of the account")]
        public AccountType AccountType { get; set; }

        [ConfigGroupProperty("Billing Address", Description = "The address to which your bill will be sent", Color = "#1997CA")]
        public AddressData Address { get; set; }

        [ConfigGroupProperty("Shipping Address", Color = "#FF0000")]
        public AddressData Address2 { get; set; }

        [ConfigProperty("Household Member Count", Description = "The number of people in the household", DefaultValue = 2)]
        public int HouseholdMembers { get; set; }

        [ConfigProperty("Monthly Rent", Description = "The dollar amount of the monthly rent payment", DefaultValue = 123)]
        public long Rent { get; set; }

        [ConfigProperty("Monthly Utilities", Description = "The dollar amount of the monthly utilities", DefaultValue = 98.76)]
        public double Utilities { get; set; }

        [ConfigProperty("Monthly Insurance", Description = "The dollar amount of the monthly property insurance payment", DefaultValue = 321.12)]
        public float Insurance { get; set; }

        [ConfigListProperty("Alternate Addresses", Description = "Other places where we can hunt you down")]
        public ObservableCollection<AddressData> Addresses { get; set; }

        [ConfigListProperty("Authorized Names", Description = "Names of people that are authorized on the account")]
        public ObservableCollection<Observable<string>> AuthorizedNames { get; set; }

        [ConfigListProperty("Numbers", Description = "A collection of numbers")]
        public ObservableCollection<Observable<int>> Numbers { get; set; }
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

        [ConfigProperty("House Color", Description = "The color of the house")]
        public HouseColors HouseColor { get; set; }

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

    public enum HouseColors
    {
        [Description("None")]
        NoPaint,
        [Description("Bright Red")]
        Red,
        [Description("Cerulean Blue")]
        Blue,
        [Description("Grass Green")]
        Green,
        [Description("Daisy Yellow")]
        Yellow
    }

    public enum AccountType
    {
        [Description("Standard")]
        Normal,
        Premier,
        Executive
    }
}