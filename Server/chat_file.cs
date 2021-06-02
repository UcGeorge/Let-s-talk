namespace Server
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class chat_file
    {
        public string Id { get; set; }
        public double size { get; set; }
        public string location { get; set; }
        public chat_client sender { get; set; }
        public chat_client receiver { get; set; }
        public DateTime dateadded { get; set; }
    }
}
