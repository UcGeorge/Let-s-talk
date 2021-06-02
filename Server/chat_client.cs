namespace Server
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class chat_client
    {
        public string Id { get; set; }
        public DateTime? last_seen { get; set; }
        public DateTime date_added { get; set; }
    }
}
