using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBookingTour.Domain.IRepositories
{
    public interface IDiscountRepository : IRepository<Discount>
    {
    }
}
