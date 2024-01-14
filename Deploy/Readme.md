# Brug ARM-skabeloner til at opsætte og vedligeholde dette modul

## Forudsætninger

Hvis du ikke har et Azure-abonnement, kan du oprette et gratis [konto](https://azure.microsoft.com/free/?ref=microsoft.com&utm_source=microsoft.com&utm_medium=docs&utm_campaign=visualstudio), før du begynder.

Som det første skal du installere [Warehouse-miljøet](https://github.com/Bygdrift/Warehouse/tree/master/Deploy). Det opretter grundmiljøet i Azure med en Datalake, database, keyvault, Apllication Insight. Derefter kan du installere forskellige moduler som alle anvender denne grundopsætning.

## Videoer

<!-- 2022-01-28: [Opsæt eksempel-modulet](https://www.youtube.com/watch?v=itwd2XdHIkM): -->

2022-01-28: [Opdater et allerede installeret modul, når en ny opdatering er blevet pushet til GitHub](https://www.youtube.com/watch?v=XywfV_n-320):

## Konfigurer Warehouse-miljøet med portalen:
Opsæt nu dette modul:
[![Deploy til Azure](https://raw.githubusercontent.com/Bygdrift/Warehouse/master/Docs/Images/deploytoazureButton.svg)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FBygdrift%2FWarehouse.Modules.DaluxFMApi%2Fmaster%2FDeploy%2FWarehouse.Modules.DaluxFMApi_ARM.json)
[![Visualize](https://raw.githubusercontent.com/Bygdrift/Warehouse/master/Docs/Images/visualizebutton.svg)](http://armviz.io/#/?load=https%3A%2F%2Fraw.githubusercontent.com%2FBygdrift%2FWarehouse.Modules.DaluxFMApi%2Fmaster%2FDeploy%2FWarehouse.Modules.DaluxFMApi_ARM.json)

Installationen vil opsætte en Windows-hostingplan og en functionapp, der indeholder softwaren fra dette GitHub-lager.

Hvis du skal ændre nogle indstillinger, kan du køre opsætningen igen. Du kan også ændre indstillinger i Azure. Du må dog ikke ændre modulets navn, da det er et id som forplanter sig ud i opsætningen. Indstillinger er gemt i: Functionapp > configuration. Og: Keyvault > Secrets.

## Konfigurer miljøet med Azure CLI

Du kan også køre ARM fra PowerShell i stedet for at køre det via portalen.

Kør enten PowerShell fra computeren ved at installere [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli), eller brug [Azure Cloud Shell](https:/ /shell.azure.com/bash) fra Azure-portalen. Følgende instruktion vil fokusere på kørsel fra en computer.

Download denne [ARM.parameters.json](https://raw.githubusercontent.com/Bygdrift/Warehouse.Modules.DaluxFMApi/master/Deploy/Warehouse.Modules.DaluxFMApi_ARM.parameters.json) til en mappe og udfyld omhyggeligt hver variabel.

Download [ARM.json](https://raw.githubusercontent.com/Bygdrift/Warehouse.Modules.DaluxFMApi/master/Deploy/Warehouse.Modules.DaluxFMApi_ARM.json) til den samme mappe.

Log ind på azure: `az login`.

Og derefter: `az deployment group create -g <resourceGroupName> --template-file ./Warehouse.Modules.DaluxFMApi_ARM.json --parameters ./Warehouse.Modules.DaluxFMApi_ARM.parameters.json`

Erstat `<resourceGroupName>` med det faktiske navn.

Det er en fordel med CLI, at man kan samle alle parametre i én json og opbygge et powershell script som opretter hele miljøet. Hvis noget fejler så kan man afinstallere, ændre på kode og køre det igen.

Man kan fjerne alt man har installeret med: `az group delete -g <resourceGroupName>`. Vær opmærksom på at den grundlæggende Warehouse-opsætning også slettes.