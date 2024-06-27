using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using QuestPDF.Infrastructure;
using Swashbuckle.AspNetCore.Filters;
using webapi.Data;
using webapi.Interfaces;
using webapi.Models;
using webapi.Services;

QuestPDF.Settings.License = LicenseType.Community;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<webapiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("webapiContext") ?? throw new InvalidOperationException("Connection string 'webapiContext' not found.")));

// Add services to the container.
builder.Services.AddFluentEmail(builder.Configuration);
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IEmailSender, EmailController>();
builder.Services.AddTransient<IReceiptService, ReceiptService>();

//Uncomment to enable AggregateService
//builder.Services.AddHostedService<AggregateService>();

builder.Services.AddControllers();

System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddIdentityApiEndpoints<User>(options =>
{
    options.Password.RequiredLength = 6;
    options.User.RequireUniqueEmail = true;
    options.Password.RequireNonAlphanumeric = false;
    options.SignIn.RequireConfirmedEmail = true;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<webapiContext>()
    .AddUserManager<ApplicationUserManager>();

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureApplicationCookie(options => { options.Cookie.SameSite = SameSiteMode.None; });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using (var scope = app.Services.CreateScope())
    {
        var DB = scope.ServiceProvider.GetRequiredService<webapiContext>();
        var UM =  scope.ServiceProvider.GetRequiredService<ApplicationUserManager>();
        var RM = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();


        DB.Database.Migrate();
        await DB.InitializeUser(UM);
        await DB.IntializeDeviceTypes();
        await DB.InitializeCampaigns();
        await DB.InitializeCurrentDevices();
        await DB.InitializeRole(RM, UM);
        await DB.InitializeCurrentDevice();
        await DB.InitializeContacts();
    }

}

app.MapIdentityApi<User>();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapPost("/logout", async (SignInManager<User> signInManager) =>
{
    await signInManager.SignOutAsync().ConfigureAwait(false);
}).RequireAuthorization();

app.Run();

