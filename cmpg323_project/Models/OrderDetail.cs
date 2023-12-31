﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace cmpg323_project.Models
{
    public partial class OrderDetail
    {
        public short OrderDetailsId { get; set; }
        public short OrderId { get; set; }
        public short ProductId { get; set; }
        public int Quantity { get; set; }
        public int? Discount { get; set; }

        [JsonIgnore]
        public virtual Order Order { get; set; } = null!;

        [JsonIgnore]
        public virtual Product Product { get; set; } = null!;
    }
}
