# OrderAgregatorAPI_MVP
MVP agregátoru objednávek podle ukázkového zadání od jechtom-cbdata

Pár slov k implemetaci:
- Projekt jsem založil jako ASP.net Core WebAPI projekt se swaggerem (přes něj se API dobře zkouší).
Měl jsem několik dotazů, které jsem poslal, ale stihl jsem naimplementovat "první verzi" před jejich odpovězením.
Pokud to bude požadováno, tak případně mohu docomittnout úpravy podle odpovědí.
V rámci "ukázkového zadání" a "ukázkové implementace" si ale myslím, že to nutné nebude.

realizovaná vylepšení:
- productId jako int - pokud to je vždy int-ové číslo, tak je lepší to držet jako int už z důvodu optimalizace a konzistence dat. V mém řešení jsem vyšel z předpokladu, že to bude int. Když tak můžu upravit na cokoliv jiného, pokud o to bude zájem a commitnout do GITu.
- Nastavitelnost četnosti posílání/vypisování agregovaných počtu kusů v appsettings.json (setting AggregatorFlushConfiguration.FlushIntervalSeconds)
- Trochu logování.

možná vylepšení:
- Místo .net 8 je možné použít .net 9, kde jsou údajně performance a security vylepšení. Já jsem vyšel z předpokladu, že se používá LTS verze, což je .net 8. Přechod na .net 9 by byl ale měl být celkem jednoduchý. Když tak mohu dodělat a commitnout do GITu, pokud o to bude zájem.
- Mohla by se přidat nějaká podoba autentizace/autorizace.
- Mohl by být někde uložen (v DB / appsettings.json / Azure Key Vault) seznam povolených IDček produktů a brát v úvahu pouze tyto povolené.
- Pokud by ve vstupních datech bylo něco jako orderId nebo requestId, tak by se daly ignorovat nadbytečná duplicitní volání API.
- Nastavitelnost "resetování/neresetování" agregovaných počtu kusů po vypsání do konzole / poslání někam dál.
- Dal by se nastavit Rate Limiting pro omezení maximálního využití API.
- API metoda pro POST /api/Orders by mohla být jako async. Momentálně to ale nemá smysl, když se využívá in memmory úložiště v podobě Dictionary. Smysl by to mohlo mít v případě volání nějakého jiného API, nebo ukládání třeba do MSSQL DB.
