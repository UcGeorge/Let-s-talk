namespace Server
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class chat_message
    {
        public string Id { get; set; }
        public string message_text { get; set; }
        public chat_client sender { get; set; }
        public chat_client receiver { get; set; }
        public DateTime date_added { get; set; }
    }
}
