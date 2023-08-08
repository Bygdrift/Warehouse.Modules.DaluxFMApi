# Warehouse modul: DaluxFMApi

Med dette modul kan du hente data fra [DaluxFM](https://www.dalux.com/da/fm-overview/) til dit eget datavarehus på Azure.

Modulet bruger DaluxFM API. Der er et andet modul [her](https://github.com/hillerod/Warehouse.Modules.DaluxFM), der bruger DaluxFM SOAP-webservice men Dalux er ved at udfase det.

Modulet er bygget med [Bygdrift Warehouse](https://github.com/Bygdrift/Warehouse), der gør det muligt at vedhæfte flere moduler i det samme Azure miljø som indsamler data fra alle slags tjenester på en billig måde i Datalake og  MS SQL database.

## Opbygning

Udover Bygdrift Warehouse basisopsætning, så installeres der også en `Azure Function` Ved installationen bestemmer du hvor tit denne function skal køre. Den sikrer at den nyeste [Docker Container](https://ghcr.io/bygdrift/warehouse.modules.daluxfmapi/container) er installeret som en `Azure Container Instance` og derefter eksekverer den programmet fra containeren.
Dette setup sikrer billig afvikling på Azure.

# Installation i Azure

Alle moduler kan installeres og faciliteres med ARM-skabeloner (Azure Resource Management): [Brug ARM-skabeloner til at konfigurere og vedligeholde dette modul](https://github.com/Bygdrift/Warehouse.Modules.DaluxFMAPI/tree/master/Deploy) .

# Test af modulet

Hvis du er udvikler og vil teste modulets kode, skal du downloade projektet og åbne det med Visual Studio eller Visual Studio Code.
Udfyld indstillingerne i `ModuleTests/appsettings.json`.
Debug filen: `ModuleTests/ImporterTest.cs`, ved at åbne filen, højreklik på `TestRunModule()` og vælg 'Debug test(s)'.

## Kontakt

For information eller konsulenttid, kan du skrive til bygdrift@gmail.com. Dalux laver løbende tilføjelser og forbedringer af deres API og dem hjælper jeg gerne med at opdatere i dette modul.

# License

[MIT License](https://github.com/Bygdrift/Warehouse.Modules.DaluxFMApi/blob/master/License.md)
