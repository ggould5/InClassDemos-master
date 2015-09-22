using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using eRestaurantSystem.DAL.Entities;
using System.Data.Entity;
#endregion
namespace eRestaurantSystem.DAL
{
    //This class should be restricted to access by ONLY 
    //the BLL methods.
    //This class should NOT be accessable outside of the
    //component library

    internal class eRestaurantContacts : DbContext
    {
        public eRestaurantContacts()
            : base("name=EatIn")
        {
            //Constructor is used to pass web config string name
        }
        //setup the DbSet Mappings
        //There must be one DbSet for each entity
        public DbSet<SpecialEvent> SpecialEvents { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Table> Tables { get; set; }

        //When overriding OnModelCreate() it is important to remember
        //to call the base method's implementation before you exit the method

        //The ManyTOManyNavigationPropertyConfiguration.Map method lets you
        //configure the tables and columns used for many-to-many relationships.
        //It takes a ManyTOManyNavigationPropertyConfiguration in which 
        //you specify the column names by calling the MapLeftKey, MapRightKey,
        //and ToTable Methods.

        //The "left" key is the one specified in the HasMany method
        //the "right" key is the one specified in the WithMany method

        //we have a many to many relationship between reservation and tables
        //a reservation may need many tables.
        //a table can have over many reservations.

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Reservation>().HasMany(r => r.Tables)
                .WithMany(x => x.Reservations)
                .Map(mapping =>
                {
                    mapping.ToTable("ReservationTables");
                    mapping.MapLeftKey("TableID");
                    mapping.MapRightKey("ReservationsID");
                });
            base.OnModelCreating(modelBuilder);
        }
    }
}
