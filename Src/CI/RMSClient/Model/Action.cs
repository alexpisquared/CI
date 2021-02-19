using System;
using System.Collections.Generic;

#nullable disable

namespace RMSClient.Model
{
    public partial class Action
    {
        public int TypeId { get; set; }
        public int SubTypeId { get; set; }
        public int ActionId { get; set; }
        public string Name { get; set; }
    }
}
