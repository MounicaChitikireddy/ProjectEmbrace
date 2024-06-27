using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using webapi.Models;
using webapi.Services;
using Action = webapi.Models.Action;

namespace webapi.Data
{
    public class webapiContext : IdentityDbContext<User>
    {
        public webapiContext(DbContextOptions<webapiContext> options)
            : base(options)
        {

        }

        public DbSet<webapi.Models.Contact> Contacts { get; set; } = default!;

        public DbSet<webapi.Models.User> User { get; set; } = default!;



        public DbSet<CurrentDevice> CurrentDevices { get; set; } = default!;
        public DbSet<ChangelogBatchDevice> ChangelogBatchDevices { get; set; }

        public DbSet<Campaign> Campaigns { get; set; } = default!;





        public DbSet<DeviceType> DeviceTypes { get; set; }

        public DbSet<ChangelogBatch> ChangelogBatches { get; set; }

        public DbSet<Receipt> Receipts { get; set; }

        public DbSet<ReceiptItem> ReceiptItems { get; set; }

        public DbSet<AggregatedDevice> AggregatedDevices { get; set; }

        public DbSet<UserSettings> UserSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasAlternateKey(x => x.DisplayId);
            ConfigureDefaultUserSettings(modelBuilder);
        }

        protected void ConfigureDefaultUserSettings(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserSettings>().Property(x => x.AddInventoryDefaultGrade).HasDefaultValue("B-FRN");
            modelBuilder.Entity<UserSettings>().Property(x => x.RemoveInventoryDefaultGrade).HasDefaultValue("B-FRN");
            modelBuilder.Entity<UserSettings>().Property(x => x.TableDensity).HasDefaultValue("comfortable");
            modelBuilder.Entity<UserSettings>().Property(x => x.TableFullScreenByDefault).HasDefaultValue(false);



        }

        public async Task IntializeDeviceTypes()
        {
            if (!DeviceTypes.Any())
            {
                DeviceType wheelChair = new DeviceType
                {
                    Category = "Wheelchair",
                    CategoryNormalized = "WHEELCHAIR",
                    Size = "Adult",
                    SizeNormalized = "ADULT",
                    Type = "Normal",
                    TypeNormalized = "NORMAL"
                };

                DeviceType crutches = new DeviceType()
                {
                    Category = "Crutches",
                    CategoryNormalized = "CRUTCHES",
                    Size = "Adult",
                    SizeNormalized = "ADULT",
                    Type = "Normal",
                    TypeNormalized = "NORMAL"
                };

                DeviceTypes.Add(wheelChair);
                DeviceTypes.Add(crutches);
                await SaveChangesAsync();
                
            }

        }

        public async Task InitializeCampaigns()
        {
            if (!Campaigns.Any())
            {
                Campaign c1 = new Campaign()
                {
                    CampaignName = "Navajo Campaign 2024",
                    StartDate = DateTime.Parse("01-01-2024"),
                    EndDate = DateTime.Now
                };

                Campaign c2 = new Campaign()
                {
                    CampaignName = "India Campaign",
                    StartDate = DateTime.Parse("03-07-2024"),
                    EndDate = DateTime.Parse("04-07-2024")
                };

                Campaigns.Add(c1);
                Campaigns.Add(c2);
                await SaveChangesAsync();
            }

        }
        
        
        public async Task InitializeContacts()
        {
            if (!Contacts.Any())
            {
                List<string> fNames = new List<string> { "Jeff", "Max", "Matt", "Peter", "Alex", "Mason" };
                List<string> lNames = new List<string> { "Smith", "Johnson", "Jones", "Hill", "Gomez", "Butler" };

                List<string> organizations = new List<string>
                    { "Utah Health", "John Muir Health", "Kaiser Permanente", "Intermountain Health" };
                
                Random r = new Random();

                foreach (var firstName in fNames)
                {
                    Contact c = new Contact
                    {
                        Email = "email@test.com",
                        FullName = firstName + " " + lNames[r.Next(lNames.Count)],
                        Organization = organizations[r.Next(organizations.Count)],
                        PhoneNum = "555-555-5555",
                        Role = "Equipment Supervisor"
                    };
                    Contacts.Add(c);
                }

                await SaveChangesAsync();
            }
        }

        public async Task InitializeCurrentDevice()
        {
            if (!CurrentDevices.Any())
            {
                var c = from campaign in this.Campaigns
                    where campaign.CampaignId == 1
                    select campaign;
                CurrentDevice device = new CurrentDevice
                {
                    DeviceType = (from d in DeviceTypes where d.CategoryNormalized == "WHEELCHAIR" select d).First(),
                    Grade = "B-FRN",
                    Location = "Salt Lake City Warehouse".ToUpper(),
                    Campaign = c.FirstOrDefault()
                };
                CurrentDevices.Add(device);
                await SaveChangesAsync();
            }
        }

        public async Task InitializeCurrentDevices()
        {
            if (!CurrentDevices.Any())
            {
                CurrentDevices.Add(new CurrentDevice()
                {
                    DeviceType = (from d in DeviceTypes
                        where d.DeviceTypeId == 1
                        select d).First(),
                    Grade = "B-FRN",
                    Location = "SLC"
                });
            }
        }
        
        
        public async Task InitializeUser(ApplicationUserManager UserManager)
        {
            if (!UserManager.Users.Any())
            {

                var user = new User
                {
                    FullName = "Volunteer Employee",
                    Title = "One Time Volunteer",
                    Department = "Volunteer",
                    Email = "Volunteer@email.com",
                    EmailConfirmed = true,
                    StartDate = DateTime.Now,
                    UserName = "Volunteer@email.com",

                    PhoneNumber = "555-555-5555",

                };
                await UserManager.CreateAsync(user, "Password1@");

                var UserSettings = new UserSettings
                {
                    User = user,
                    AvatarColor = "",
                    AvatarText = "",
                };
                this.UserSettings.Add(UserSettings);



                var user2 = new User
                {
                    FullName = "Admin Employee",
                    Title = "Head Admin",
                    Department = "HQ",
                    Email = "Admin@email.com",
                    EmailConfirmed = true,
                    StartDate = DateTime.Now,
                    UserName = "Admin@email.com",
                    PhoneNumber = "555-555-5555"
                };
                await UserManager.CreateAsync(user2, "Password1@");

                var user2Settings = new UserSettings
                {
                    User = user2,
                    AvatarColor = "",
                    AvatarText = "",
                };
                this.UserSettings.Add(user2Settings);

                var user3 = new User
                {
                    FullName = "Reviewer Employee",
                    Title = "Warehouse Manager",
                    Department = "Warehouse",
                    Email = "Reviewer@email.com",
                    EmailConfirmed = true,
                    StartDate = DateTime.Now,
                    UserName = "Reviewer@email.com",
                    PhoneNumber = "555-555-5555"
                };

                await UserManager.CreateAsync(user3, "Password1@");
                var user3Settings = new UserSettings
                {
                    User = user3,
                    AvatarColor = "",
                    AvatarText = "",
                };
                this.UserSettings.Add(user3Settings);


                await this.SaveChangesAsync();
            }


        }

        public async Task InitializeRole(RoleManager<IdentityRole> roleManager, ApplicationUserManager userManager)

        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await roleManager.CreateAsync(new IdentityRole("Reviewer"));
                await roleManager.CreateAsync(new IdentityRole("Employee"));



                await userManager.AddToRoleAsync(await userManager.FindByNameAsync("Volunteer@email.com"), "Employee");

                await userManager.AddToRoleAsync(await userManager.FindByNameAsync("Reviewer@email.com"), "Reviewer");

                await userManager.AddToRoleAsync(await userManager.FindByNameAsync("Admin@email.com"), "Admin");

            }

        }


    }
}

