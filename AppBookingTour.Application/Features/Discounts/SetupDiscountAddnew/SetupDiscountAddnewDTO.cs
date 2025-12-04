using AppBookingTour.Share.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBookingTour.Application.Features.Discounts.SetupDiscountAddnew
{
    public class SetupDiscountAddnewDTO : BaseResponse
    {
        public List<KeyValuePair<int, string>> ListStatus { get; set; }
        public List<KeyValuePair<int, string>> ListServiceType { get; set; }
    }
}
