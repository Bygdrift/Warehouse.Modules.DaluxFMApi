# Warehouse modul: DaluxFMApi

Med dette modul kan du hente data fra [DaluxFM](https://www.dalux.com/da/fm-overview/) til dit eget datavarehus på Azure.

Modulet bruger DaluxFM API. Der er et andet modul [her](https://github.com/hillerod/Warehouse.Modules.DaluxFM), der bruger DaluxFM SOAP-webservice.

Modulet er bygget med [Bygdrift Warehouse](https://github.com/Bygdrift/Warehouse), der gør det muligt at vedhæfte flere moduler i det samme Azure miljø som indsamler data fra alle slags tjenester på en billig måde i Datalake og  MS SQL database.

# Installation i Azure

Alle moduler kan installeres og faciliteres med ARM-skabeloner (Azure Resource Management): [Brug ARM-skabeloner til at konfigurere og vedligeholde dette modul](https://github.com/Bygdrift/Warehouse.Modules.DaluxFMAPI/tree/master/Deploy) .

# Test af modulet

Hvis du er udvikler og vil teste modulets kode, skal du downloade projektet og åbne det med Visual Studio eller Visual Studio Code.
Udfyld indstillingerne i `ModuleTests/appsettings.json`.
Debug filen: `ModuleTests/ImporterTest.cs`, ved at åbne filen, højreklik på `TestRunModule()` og vælg 'Debug test(s)'.

## Kontakt

For information eller konsulenttid, kan du kontakte mig via [hjemmesiden for Bygdrift](https://bygdrift.dk). Dalux laver løbende tilføjelser og forbedringer af deres API og derfor må dette modul til tider opdateres. Jeg har kun bygget denne function til at køre på på en specifik Dalux-løsning, så når den køres på en anden Dalux-løsning, kan der være små fejl  som jeg kan fjerne.

# Licens

[MIT License](https://github.com/Bygdrift/Warehouse.Modules.DaluxFMApi/blob/master/License.md)
