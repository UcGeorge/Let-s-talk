using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Server
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=LTDBModel")
        {
        }

        public virtual DbSet<chat_client> chat_client { get; set; }
        public virtual DbSet<chat_file> chat_file { get; set; }
        public virtual DbSet<chat_message> chat_message { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
