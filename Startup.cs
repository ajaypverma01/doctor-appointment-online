using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Owin;
using Owin;
using System;

[assembly: OwinStartupAttribute(typeof(DoctorPatient.Startup))]
namespace DoctorPatient
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
            ConfigureAuth(app);
        }

       
    }
}
