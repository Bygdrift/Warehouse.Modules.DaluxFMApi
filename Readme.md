# Warehouse module: DaluxFMApi

With this module, you can fetch data from [DaluxFM](https://www.dalux.com/da/fm-overview/) into your own data warehouse on Azure.

The module is build with [Bygdrift Warehouse](https://github.com/Bygdrift/Warehouse), that makes it possible to attach multiple modules within the same azure environment, that can collect data from all kinds of services, in a cheap data lake and database.
The data data lake, is structured as a Common Data Model (CDM), which enables an easy integration to Microsoft Power BI, through Power BI Dataflows. And the Microsoft SQL database, makes it even easier to fetch data to Excel, Power BI and a lot of other systems.

The module uses DaluxFM API. There is another module [here](https://github.com/hillerod/Warehouse.Modules.DaluxFM), that uses DaluxFM SOAP web service.

# Testing module

If you want to test the module, then download the project and open it with Visual Studio or Visual Studio Code.
Fill out the settings in `ModuleTests/appsettings.json`.
Debug the file: `ModuleTests/ImporterTest.cs`, by opening the file, right-click `TestRunModule()` and select 'Debug test(s)'.

# Install on Azure

## Video

This video describes the steps of installing, updating and deleting these modules:
<div align="left">
      <a href="https://www.youtube.com/watch?v=xRm5fj2ZCZo">
         <img src="https://img.youtube.com/vi/xRm5fj2ZCZo/0.jpg">
      </a>
</div>

## Prerequisites

If you don't have an Azure subscription, create a free [account](https://azure.microsoft.com/free/?ref=microsoft.com&utm_source=microsoft.com&utm_medium=docs&utm_campaign=visualstudio) before you begin.

If you want to get data from Azure to Power BI, Azure and Power BI has to be in the same tenant (it is Power BI that has this limitation that comes up when you [later on](#Establish-access-from-Power-BI-to-the-storage-account), wants to ad a dataflow).

Azure CLI: The command-line examples in this article use the [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/) and are formatted for PowerShell. You can install the [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli) locally, or use the [Azure Cloud Shell](https://shell.azure.com/bash).

## Setup:

Download the [PowerShell script](https://github.com/Bygdrift/Warehouse.Modules.Example/blob/master/LoadModuleIntoAzurePsScript.ps1) to setup the environment and module on Azure.

Open it in a notebook and carefully walk through each variable in the top, to assure they are named correct.

Notice, that if you already have setup the environment with a group, data lake and a database, then this module, easily can use the same.
The data lake, database and module that gets installed, are preselected to be as cheap as possible.

Run the script by right click on the file. On the context menu, select `Run with PowerShell`.

## Start the module

The module will be called each time the schedule expression are satisfied such as each day at 1 AM UTC. But if you want to call it now, there is a function in the powers hell script. you can also do it with [Postman](https://docs.microsoft.com/en-us/azure/azure-functions/functions-manually-run-non-http) or through Visual Studio Code that has a nice extension where you can trigger the function [Azure Functions for Visual Studio Code](https://github.com/microsoft/vscode-azurefunctions). I have not found an easier way to do it.

## Remove this module from the environment

```powershell
az functionapp delete -n $moduleName -g $group
```

## Remove the whole environment

The whole setup can easily be removed again by deleting the resource group:

```powershell
az group delete -g $group
```

# Establish access from Power BI to the storage account

From Power BI, it is easy to access data from the Azure database, but if you want to access data from the Azure Data Lake through a dataflow, the storage account must be in the same tenant as PowerBI!

The user that sets up the data flow, must have access to the storage account:
- In Azure web portal, go into the storage account
- Select 'Access Control (IAM)'
- Select Add, and then 'Add role assignment'
- For a role, select 'Storage Blob Data Reader', and add the user

Now the user can log into [Power BI](https://app.powerbi.com/) and:
- Select or create a workspace
- Select 'New' and then 'Dataflow'
- Select 'Attach a Common Data Model folder (preview)'
- Give it a name and attach the path to the model.json in `https://<$storageAccount>.dfs.core.windows.net/<DataLakeContainer>/<ModuleName>/model.json`

# License

[MIT License](https://github.com/Bygdrift/Warehouse.Modules.DaluxFMApi/blob/master/License.md)
