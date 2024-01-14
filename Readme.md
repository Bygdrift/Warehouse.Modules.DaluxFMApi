# Warehouse modul: DaluxFMApi
Med dette modul kan du hente data fra [DaluxFM](https://www.dalux.com/da/dalux-fm/drift-og-vedligehold/) til dit eget datavarehus på Azure.
Modulet bruger DaluxFM API. Der er et andet modul [her](https://github.com/hillerod/Warehouse.Modules.DaluxFM), der bruger DaluxFM SOAP-webservice men den teknik er Dalux ved at fase ud.
Modulet er bygget med [Bygdrift Warehouse](https://github.com/Bygdrift/Warehouse), der gør det muligt at vedhæfte flere moduler i det samme Azure miljø som indsamler data fra alle slags tjenester på en billig måde i Datalake og  MS SQL database.
Her kan du læse Dalux egen dokumentation af deres [API](https://app.swaggerhub.com/apis-docs/Dalux/DaluxFM-API).

# Funktion
Modulet er som standard opsat til, en gang i døgnet at kontakte Dalux og trække al data der er tilgængeligt via deres API, og gemme det over i en MS SQL database i Azure.
Herfra er det enkellt at tilgå data fra fx Excel, Power BI og andre fagsystemer.
Hver gang der laves tilretninger i dette modul, så er det enkelt at få opdateringen ind i Azure.

# Installation i Azure
Alle moduler kan installeres og faciliteres med ARM-skabeloner (Azure Resource Management): [Brug ARM-skabeloner til at konfigurere og vedligeholde dette modul](https://github.com/Bygdrift/Warehouse.Modules.DaluxFMAPI/tree/master/Deploy) .

# Test af modulet
Hvis du er udvikler og vil teste modulets kode, skal du downloade projektet og åbne det med Visual Studio eller Visual Studio Code.
Udfyld indstillingerne i `ModuleTests/appsettings.json`.
Debug filen: `ModuleTests/AppFunctions/TimerTriggerTest.cs`, ved at åbne filen, højreklik på `CallOrchestrator()` og vælg 'Debug test(s)'.
Modulet er skrevet som en Azure Durable Function med en chaning process fordi processen med at hente al data fra Dalux, tager længere tid end 10 minutter, som er den maksimale tidsgrænse i normale Azure Functions. Det er grunden til at koden fremstår mere kompleks end hvis den var skrevet som en normal Azure Function.

## Kontakt
For information eller konsulenttid, kan du kontakte mig via [Bygdrift](https://bygdrift.dk). 
Dalux laver løbende forbedringer af deres API og derfor må dette modul holdes opdateret. 
Jeg har kun bygget dette modul til at køre på på en specifik Dalux-løsning, så når den køres på andre Dalux-løsninger kan der findes utilsigtede fejl som jeg må kende før de kan kan fjernes og dette modul optimeres.

# Licens
[MIT License](https://github.com/Bygdrift/Warehouse.Modules.DaluxFMApi/blob/master/License.md)
