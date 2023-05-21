using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BobrovaKsiazka
{
    public class River
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public double Length { get; set; }
        public double FlowRate { get; set; }
        public bool ProvidesWaterSupply { get; set; }
        public bool UsedForIrrigation { get; set; }
        public bool SupportsNavigation { get; set; }
        public bool GeneratesHydroelectricPower { get; set; }

        public River(string name, string location, double length, double flowRate, bool providesWaterSupply, bool usedForIrrigation, bool supportsNavigation, bool generatesHydroelectricPower)
        {
            Name = name;
            Location = location;
            Length = length;
            FlowRate = flowRate;
            ProvidesWaterSupply = providesWaterSupply;
            UsedForIrrigation = usedForIrrigation;
            SupportsNavigation = supportsNavigation;
            GeneratesHydroelectricPower = generatesHydroelectricPower;
        }

        public override string ToString()
        {
            return $"Name: {Name}, Location: {Location}, Length: {Length}, Flow Rate: {FlowRate}, Provides Water Supply: {ProvidesWaterSupply}, Used for Irrigation: {UsedForIrrigation}, Supports Navigation: {SupportsNavigation}, Generates Hydroelectric Power: {GeneratesHydroelectricPower}";
        }
    }
}
