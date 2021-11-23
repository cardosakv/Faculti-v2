using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faculti.Helpers
{
    internal class SMS
    {
        private static readonly Dictionary<string, string> _carrierEmails = new()
        {
            ["Globe"] = "@pmms.globe.com.ph",
            ["Smart"] = "@smart.mms.ph",
            ["Sun"] = "@mms.suncellular.com.ph"
        };

        public static string? GetCarrierEmail(string phoneNumber)
        {
            var prefix = phoneNumber.Substring(0, 4);
            string carrierEmail = null;

            switch (prefix)
            {
                case "0817":
                case "0905":
                case "0906":
                case "0915":
                case "0916":
                case "0917":
                case "0926":
                case "0927":
                case "0935":
                case "0936":
                case "0937":
                case "0945":
                case "0953":
                case "0954":
                case "0955":
                case "0956":
                case "0965":
                case "0966":
                case "0967":
                case "0975":
                case "0977":
                case "0978":
                case "0979":
                case "0994":
                case "0995":
                case "0996":
                case "0997":
                    carrierEmail = _carrierEmails["Globe"];
                    break;

                case "0908":
                case "0918":
                case "0919":
                case "0920":
                case "0921":
                case "0928":
                case "0929":
                case "0939":
                case "0947":
                case "0949":
                case "0951":
                case "0961":
                case "0998":
                case "0999":
                    carrierEmail = _carrierEmails["Smart"];
                    break;

                case "0922":
                case "0923":
                case "0924":
                case "0925":
                case "0931":
                case "0932":
                case "0934":
                case "0940":
                case "0941":
                case "0942":
                case "0943":
                case "0973":
                case "0974":
                    carrierEmail = _carrierEmails["Sun"];
                    break;
            }

            if (string.IsNullOrEmpty(carrierEmail))
                return null;

            return $"63{phoneNumber.Substring(1, 10)}{carrierEmail}";
        }
    }
}
