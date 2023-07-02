using System;
using Azure.Identity;
using Azure.ResourceManager.ContainerInstance.Models;
using Azure.ResourceManager.KeyVault;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Azure.ResourceManager.ContainerInstance;
using Azure.ResourceManager.KeyVault.Models;
using Azure;
using System.Threading.Tasks;
using Module.AppFunctions.Models;
using System.Linq;
using Bygdrift.Warehouse;
using Microsoft.Extensions.Logging;
using Azure.Core;

namespace Module.AppFunctions
{
    public class TimerTrigger
    {
        public AppBase<Settings> App { get; }

        public TimerTrigger(ILogger<TimerTrigger> logger) => App = new AppBase<Settings>(logger);

        [FunctionName("TimeTrigger")]
        public async Task Run([TimerTrigger("0 */5 * * * *"
#if DEBUG
            ,RunOnStartup = true
#endif
            )] TimerInfo myTimer)
        {
            App.Log.LogWarning($"Nr 3: {DateTime.Now}");

#if DEBUG
            var tenant = App.Config["Tenant"];
            var clientSecret = App.Config["ClientSecret"];
            var clientId = App.Config["ClientId"];
            TokenCredential credential = new ClientSecretCredential(tenant, clientId, clientSecret); //https://github.com/Azure/azure-sdk-for-net/issues/29967
#else
            TokenCredential credential = new DefaultAzureCredential();
#endif
            var armClient = new ArmClient(credential);
            App.Log.LogWarning($"101");

            SubscriptionResource subscription = await armClient.GetDefaultSubscriptionAsync();  //https://learn.microsoft.com/en-us/dotnet/azure/sdk/resource-management?tabs=PowerShell#management-sdk-cheat-sheet
            App.Log.LogInformation($"224" + subscription.Id.SubscriptionId);

            ResourceGroupResource rg = subscription.GetResourceGroup(App.Settings.ResourceGroup);
            App.Log.LogWarning($"105");

            App.Log.LogWarning($"106");
            var keyVaultName = App.KeyVault.SecretClient.VaultUri.Host.Split('.')?[0];
            App.Log.LogWarning($"107");
            var keyVault = await rg.GetKeyVaultAsync(keyVaultName);
            App.Log.LogWarning($"108");

            var containers = new List<Container>();
            if (App.Settings.Container1Name is not null && App.Settings.Container1Image is not null)
                containers.Add(new Container(App, App.Settings.Container1Name, App.Settings.Container1Image, 1, 1, App.Settings.Container1Variables));
            if (App.Settings.Container2Name is not null && App.Settings.Container2Image is not null)
                containers.Add(new Container(App, App.Settings.Container2Name, App.Settings.Container2Image, 1, 1, App.Settings.Container2Variables));
            if (App.Settings.Container3Name is not null && App.Settings.Container3Image is not null)
                containers.Add(new Container(App, App.Settings.Container3Name, App.Settings.Container3Image, 1, 1, App.Settings.Container3Variables));
            App.Log.LogWarning($"109");

            var containername = App.ModuleName + keyVaultName.Replace("keyvault", "");  //Tilføjer guid fra keyvault. Det vil tage lang tid at gennemskue hvordan jeg selv bygger den.
            App.Log.LogWarning($"110");
            var operatingSystem = App.Settings.ContainerOperatingSystem.Equals("Linux", StringComparison.OrdinalIgnoreCase) ? ContainerInstanceOperatingSystemType.Linux : ContainerInstanceOperatingSystemType.Windows;
            App.Log.LogWarning($"110.5 {operatingSystem}");
            var containerGroup = new ContainerGroup(containername, App.ModuleName, operatingSystem, containers);
            App.Log.LogWarning($"111");
            await CreateACI(rg, containerGroup, keyVault);
            App.Log.LogWarning($"112");
        }

        private static async Task CreateACI(ResourceGroupResource rg, ContainerGroup containerGroup, KeyVaultResource keyVault)
        {
            var containerGroupCollection = rg.GetContainerGroups();
            if (!containerGroupCollection.Any(o => o.Data.Name == containerGroup.ContainerName))  //It will fail completing because Keyvault isn't set yet. If they have the Bygdrift Warehouse component installed, the container will fail fast at first run.
            {
                var res = await containerGroupCollection.CreateOrUpdateAsync(WaitUntil.Completed, containerGroup.ContainerName, containerGroup.GetContainerGroupData());
                var principalId = res.Value.Data.Identity.PrincipalId.ToString() ?? "";
                await UpdateKeyVaultPolicies(keyVault, principalId);
            }
            else
            {
                var res = await containerGroupCollection.CreateOrUpdateAsync(WaitUntil.Completed, containerGroup.ContainerName, containerGroup.GetContainerGroupData());
                await res.Value.StartAsync(WaitUntil.Completed);
            }
        }

        private static async Task UpdateKeyVaultPolicies(KeyVaultResource keyVault, string principalId)
        {
            var identityPermissions = new IdentityAccessPermissions();
            identityPermissions.Secrets.Add(new IdentityAccessSecretPermission("get"));
            identityPermissions.Secrets.Add(new IdentityAccessSecretPermission("list"));
            Guid tenantId = keyVault.Data.Properties.TenantId;
            var keyVaultPolicy = new KeyVaultAccessPolicy(tenantId, principalId, identityPermissions);
            var keyVaultPolicies = new List<KeyVaultAccessPolicy> { keyVaultPolicy };
            var keyVaultPolicyProperties = new KeyVaultAccessPolicyProperties(keyVaultPolicies);
            var keyVaultParameters = new KeyVaultAccessPolicyParameters(keyVaultPolicyProperties);
            await keyVault.UpdateAccessPolicyAsync(AccessPolicyUpdateKind.Add, keyVaultParameters);
        }
    }
}
