using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cars.Models
{
    public class ViewCarOrderModel
    {
        public int ID { get; set; }
        public string Model { get; set; }
        public string ImagePath { get; set; }
        public string Des { get; set; }
        public decimal DailyPrice { get; set; }
        public Nullable<int> VehicleID { get; set; }
        public Nullable<int> UserID { get; set; }

        public string CarModelName { get; set; }
        public virtual ICollection<Tb_Order> Tb_Order { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public string Contact { get; set; }


    }
}