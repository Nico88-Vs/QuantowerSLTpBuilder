Quantower SL/TP Builder
QuantowerSLTpBuilder è una libreria progettata per estendere la piattaforma Quantower permettendo una gestione flessibile delle condizioni di Stop Loss (SL) e Take Profit (TP). La libreria consente agli sviluppatori di implementare logiche personalizzate per la gestione degli ordini SL/TP utilizzando delegati e strategie dinamiche.

Caratteristiche
Delegati per SL e TP personalizzati: Gestione di logiche SL/TP completamente personalizzate tramite l'uso di delegati.
Integrazione diretta con Quantower: L'integrazione si basa sulla piattaforma Quantower, una piattaforma avanzata per il trading multi-asset.
Gestione centralizzata degli ordini: Creazione, gestione e aggiornamento degli ordini con SL e TP.
Supporto per più ordini simultanei: La libreria gestisce più ordini di SL e TP in modo flessibile e scalabile.
Logging e diagnostica: Integrazione con il sistema di logging di Quantower per tracciare gli errori e le operazioni sugli ordini.
Installazione
Clonare il repository
Per iniziare, clona il repository GitHub del progetto:

bash
Copia codice
git clone https://github.com/Nico88-Vs/QuantowerSLTpBuilder.git
cd QuantowerSLTpBuilder
Compilare il progetto
Assicurati di avere il .NET SDK installato (versione 5.0 o successiva). Compila il progetto utilizzando:

bash
Copia codice
dotnet build --configuration Release
Integrazione con Quantower
Per integrare questa libreria con Quantower:

Compila la libreria come indicato sopra.
Posiziona il file .dll generato nella cartella dei componenti aggiuntivi (custom add-ons) di Quantower.
Utilizzo
Definire le Condizioni di SL e TP
Definisci le tue strategie personalizzate per lo Stop Loss e il Take Profit utilizzando delegati personalizzati.

csharp
Copia codice
double CustomSlStrategy(object obj)
{
    // Logica personalizzata per lo Stop Loss
    return 100.0; // Esempio: 100 pips
}

double CustomTpStrategy(object obj)
{
    // Logica personalizzata per il Take Profit
    return 200.0; // Esempio: 200 pips
}
Configurare SlTpCondictionHolder
Configura il contenitore delle condizioni SL e TP utilizzando i delegati appena definiti.

csharp
Copia codice
var slDelegate = new SlTpCondictionHolder<double>.DefineSl[] { CustomSlStrategy };
var tpDelegate = new SlTpCondictionHolder<double>.DefineTp[] { CustomTpStrategy };
var slObjects = new double[] { 100.0 };
var tpObjects = new double[] { 200.0 };

var slTpCondictionHolder = new SlTpCondictionHolder<double>(slObjects, tpObjects, slDelegate, tpDelegate);
Creare e Gestire Ordini
Usa TpSlComputator per piazzare gli ordini con le condizioni di SL e TP definite.

csharp
Copia codice
var computator = new TpSlComputator<double>(slTpCondictionHolder);
computator.PlaceOrder(trade);
Per aggiornare gli ordini SL/TP:

csharp
Copia codice
computator.UpdateOrder(orderList, isTp: true);
Componenti Principali
SlTpCondictionHolder<T>
Questo struct memorizza i delegati e gli oggetti associati per le strategie di SL/TP.

Proprietà:

SlDelegate[]: Array di delegati per SL.
TpDelegate[]: Array di delegati per TP.
SlDelegateObj[]: Oggetti associati ai delegati SL.
TpDelegateObj[]: Oggetti associati ai delegati TP.
Costruttore:

Inizializza il contenitore con gli array di oggetti e delegati per SL e TP.
TpSlComputator<T>
Gestisce la logica per piazzare e aggiornare gli ordini con SL e TP.

Costruttore:

Richiede un'istanza di SlTpCondictionHolder<T> per inizializzare la logica di gestione degli ordini.
Metodi:

PlaceOrder(Trade trade): Piazza un ordine con SL e TP.
UpdateOrder(List<Order> orders, bool isTp): Aggiorna gli ordini esistenti.
SlTpItems
Questo contenitore rappresenta gli ordini principali, associando gli ordini di Stop Loss e Take Profit.

Proprietà:

EntryOrder: L'ordine principale di ingresso.
SlItems: Lista degli ordini SL.
TpItems: Lista degli ordini TP.
Metodi:

AddSl(Order order): Aggiunge un ordine SL.
AddTp(Order order): Aggiunge un ordine TP.
Logging
La libreria integra un sistema di logging per monitorare e gestire errori e messaggi diagnostici. Gli errori relativi al piazzamento o all'aggiornamento degli ordini vengono gestiti attraverso il sistema di logging di Quantower.

csharp
Copia codice
Core.Instance.Loggers.Log("Error placing SL", LoggingLevel.Error);
Assicurati di monitorare i log per risolvere eventuali problemi.

Contribuire
Se desideri contribuire al progetto, sentiti libero di fare una pull request o di aprire una issue nel repository GitHub.

Fork del progetto
Crea una branch per la tua feature (git checkout -b feature/AmazingFeature)
Fai commit delle modifiche (git commit -m 'Aggiunta di una feature incredibile')
Fai push alla branch (git push origin feature/AmazingFeature)
Apri una Pull Request
Licenza
Questo progetto è distribuito sotto la licenza MIT - consulta il file LICENSE per maggiori dettagli.


