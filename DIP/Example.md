# (DIP) Dependency Inversion Principle
Princip Inversion Inversion navrhuje, že moduly na vysoké úrovni by nemìly záviset na modulech nízké úrovnì, ale oba by mìly záviset na abstrakcích. Navíc by abstrakce nemìly záviset na detailech; podrobnosti by mìly záviset na abstrakcích.


## Pøíklad 1
```
public class LightBulb
{
    public void TurnOn() { /* implementation */ }
    public void TurnOff() { /* implementation */ }
}

public class Switch
{
    private LightBulb bulb;

    public Switch(LightBulb bulb)
    {
        this.bulb = bulb;
    }

    public void Toggle()
    {
        if (bulb.IsOn)
            bulb.TurnOff();
        else
            bulb.TurnOn();
    }
}
```
Tøída Switchpøímo závisí na konkrétní LightBulbtøídì a porušuje DIP.

```
public interface ISwitchable
{
    void TurnOn();
    void TurnOff();
}

public class LightBulb : ISwitchable
{
    // implementation
}

public class Switch
{
    private ISwitchable device;

    public Switch(ISwitchable device)
    {
        this.device = device;
    }

    public void Toggle()
    {
        if (device.IsOn)
            device.TurnOff();
        else
            device.TurnOn();
    }
}
```
Zavedením rozhraní ( ISwitchable) Switchnyní tøída závisí na abstrakci a dodržuje Princip inverze závislosti.

Podle DIP nepište žádný pevnì propojený kód, protože to je noèní mùra, kterou je tøeba udržovat, když se aplikace zvìtšuje a zvìtšuje. Pokud tøída závisí na jiné tøídì, pak musíme zmìnit jednu tøídu, pokud se v této závislé tøídì nìco zmìní. Vždy bychom se mìli snažit psát volnì spojené tøídy.

## Pøíklad 2

```
public class UserController
{
    private Database database;
    public UserController()
    {
        this.database = new Database();
    }
    // ...
}
```
V pøedchozím pøíkladu se UserController tìsnì propojuje s konkrétní tøídou Database a vytváøí pøímou závislost. Pokud se rozhodneme zmìnit implementaci databáze nebo zavést nový mechanismus úložištì, budeme muset upravit tøídu UserController , která porušuje princip Open-Closed.

Abychom tento problém vyøešili a dodrželi DIP, musíme pøevrátit závislosti zavedením abstrakce, na které závisí moduly vysoké i nízké úrovnì. Obvykle je tato abstrakce definována pomocí rozhraní nebo abstraktní tøídy.
```
public interface IDataStorage
{
    // Define the contract for data storage operations.
    void SaveData(string data);
    string RetrieveData();
}

public class Database : IDataStorage
{
    public void SaveData(string data)
    {
        // Implementation for saving data to a database.
    }
    public string RetrieveData()
    {
        // Implementation for retrieving data from a database.
    }
}

public class UserController
{
    private IDataStorage dataStorage;
    public UserController(IDataStorage dataStorage)
    {
        this.dataStorage = dataStorage;
    }
    // ...
}
```

V této aktualizované verzi pøedstavujeme rozhraní IDataStorage , které definuje smlouvu o operacích ukládání dat. Tøída Database implementuje toto rozhraní a poskytuje konkrétní implementaci. V dùsledku toho se tøída UserController nyní spoléhá na rozhraní IDataStorage spíše než na konkrétní tøídu Database , což vede k tomu, že je oddìlena od konkrétních mechanismù úložištì.

Tato inverze závislostí usnadòuje rozšiøitelnost a údržbu. Mùžeme zavést nové implementace úložištì, jako je souborový systém nebo cloudové úložištì, jednoduchým vytvoøením nových tøíd, které implementují rozhraní IDataStorage , aniž bychom museli upravovat UserController nebo jakékoli jiné moduly na vysoké úrovni.

## Pøíklad 3
Princip inverze závislosti (DIP) uvádí, že moduly/tøídy na vysoké úrovni by nemìly záviset na modulech/tøídách nižší úrovnì. Za prvé, obojí by mìlo záviset na abstrakcích. Za druhé, abstrakce by se nemìly spoléhat na detaily. Koneènì, detaily by mìly záviset na abstrakcích.

Moduly/tøídy na vysoké úrovni implementují obchodní pravidla nebo logiku v systému (aplikaci). Nízkoúrovòové moduly/tøídy se zabývají podrobnìjšími operacemi; jinými slovy, mohou zapisovat informace do databází nebo pøedávat zprávy operaènímu systému nebo službám.

Vysokoúrovòový modul/tøída, který závisí na nízkoúrovòových modulech/tøídách nebo na nìjaké jiné tøídì a ví hodnì o ostatních tøídách, se kterými interaguje, se øíká, že je úzce propojen. Když tøída ví explicitnì o návrhu a implementaci jiné tøídy, zvyšuje to riziko, že zmìny jedné tøídy poruší druhou. Musíme tedy udržovat tyto moduly/tøídy na vysoké a nízké úrovni co nejvíce volnì propojené. Abychom toho dosáhli, musíme je oba uèinit závislými na abstrakcích místo toho, abychom se navzájem znali. Zaènìme pøíkladem.

Pøedpokládejme, že potøebujeme pracovat na modulu pro protokolování chyb, který zaznamenává trasování zásobníku výjimek do souboru. Jednoduché, že? Následující tøídy poskytují funkce pro protokolování trasování zásobníku do souboru.

```
public class FileLogger
{
   public void LogMessage(string aStackTrace)
   {
      //code to log stack trace into a file.
   }
}

public class ExceptionLogger
{
   public void LogIntoFile(Exception aException)
   {
      FileLogger objFileLogger = new FileLogger();
      objFileLogger.LogMessage(GetUserReadableMessage(aException));
   }

   private GetUserReadableMessage(Exception ex)
   {
      string strMessage = string. Empty;
      //code to convert Exception's stack trace and message to user readable format.
      ....
      ....
      return strMessage;
   }
}
```
Tøída klienta exportuje data z mnoha souborù do databáze.

```
public class DataExporter
{
   public void ExportDataFromFile()
   {

      try {
         //code to export data from files to the database.
      }

      catch(Exception ex)
      {
         new ExceptionLogger().LogIntoFile(ex);
      }
   }
}
```
Vypadá dobøe. Klientovi jsme odeslali naši žádost. Náš klient však chce toto trasování zásobníku uložit do databáze, pokud dojde k výjimce IO. Hmm... OK, žádný problém. I to umíme implementovat. Zde musíme pøidat jednu další tøídu, která poskytuje funkci pro protokolování trasování zásobníku do databáze a další metodu v ExceptionLogger pro interakci s naší novou tøídou pro protokolování trasování zásobníku.

```
public class DbLogger
{
   public void LogMessage(string aMessage)
   {
      //Code to write message in the database.
   }
}

public class FileLogger
{
   public void LogMessage(string aStackTrace)
   {
      //code to log stack trace into a file.
   }
}

public class ExceptionLogger
{
   public void LogIntoFile(Exception aException)
   {
      FileLogger objFileLogger = new FileLogger();
      objFileLogger.LogMessage(GetUserReadableMessage(aException));
   }
   public void LogIntoDataBase(Exception aException)
   {
      DbLogger objDbLogger = new DbLogger();
      objDbLogger.LogMessage(GetUserReadableMessage(aException));
   }
   private string GetUserReadableMessage(Exception ex)
   {
      string strMessage = string.Empty;
      //code to convert Exception's stack trace and message to user readable format.
      ....
      ....
      return strMessage;
   }
}

public class DataExporter
{
   public void ExportDataFromFile()
   {
      try {
         //code to export data from files to database.
      }
      catch(IOException ex)
      {
         new ExceptionLogger().LogIntoDataBase(ex);
      }
      catch(Exception ex)
      {
         new ExceptionLogger().LogIntoFile(ex);
      }
   }
}
```
Zatím to vypadá dobøe. Ale kdykoli chce klient zavést nový logger, musíme ExceptionLogger zmìnit pøidáním nové metody. Pøedpokládejme, že v tom budeme po nìjaké dobì pokraèovat. V takovém pøípadì uvidíme tlustou tøídu ExceptionLogger s velkou sadou postupù, které poskytují funkcionalitu pro pøihlášení zprávy do rùzných cílù. Proè k tomuto problému dochází? Protože ExceptionLogger pøímo kontaktuje nízkoúrovòové tøídy FileLogger a DbLogger, aby zaprotokolovaly výjimku. Potøebujeme zmìnit design, aby tato tøída ExceptionLogger mohla být volnì spojena s tìmito tøídami. Abychom to udìlali, musíme mezi nì zavést abstrakci, aby ExcetpionLogger mohl kontaktovat abstrakci a zaprotokolovat výjimku místo pøímé závislosti na tøídách nízké úrovnì.

```
public interface ILogger
{
   void LogMessage(string aString);
}

public class DbLogger: ILogger
{
   public void LogMessage(string aMessage)
   {
      //Code to write message in database.
   }
}
public class FileLogger: ILogger
{
   public void LogMessage(string aStackTrace)
   {
      //code to log stack trace into a file.
   }
}
```
Nyní se pøesuneme k iniciaci nízkoúrovòové tøídy z tøídy ExcetpionLogger do tøídy DataExporter, aby byl ExceptionLogger volnì spojený s nízkoúrovòovými tøídami FileLogger a EventLogger. A tím poskytujeme tøídì DataExporter možnost rozhodnout, jaký druh Loggeru by mìl být volán na základì výjimky, která nastane.

```
public class ExceptionLogger
{
   private ILogger _logger;
   public ExceptionLogger(ILogger aLogger)
   {
      this._logger = aLogger;
   }
   public void LogException(Exception aException)
   {
      string strMessage = GetUserReadableMessage(aException);
      this._logger.LogMessage(strMessage);
   }
   private string GetUserReadableMessage(Exception aException)
   {
      string strMessage = string.Empty;
      //code to convert Exception's stack trace and message to user readable format.
      ....
      ....
      return strMessage;
   }
}
public class DataExporter
{
   public void ExportDataFromFile()
   {
      ExceptionLogger _exceptionLogger;
      try {
         //code to export data from files to database.
      }
      catch(IOException ex)
      {
         _exceptionLogger = new ExceptionLogger(new DbLogger());
         _exceptionLogger.LogException(ex);
      }
      catch(Exception ex)
      {
         _exceptionLogger = new ExceptionLogger(new FileLogger());
         _exceptionLogger.LogException(ex);
      }
   }
}
```
Úspìšnì jsme odstranili závislost na tøídách nízké úrovnì. Tento ExceptionLogger nezávisí na tøídách FileLogger a EventLogger pro protokolování trasování zásobníku. Pro novou funkci protokolování již nepotøebujeme mìnit kód ExceptionLogger. Musíme vytvoøit novou tøídu protokolování, která implementuje rozhraní ILogger a pøidá další blok catch do metody ExportDataFromFile tøídy DataExporter.

```
public class EventLogger: ILogger
{
   public void LogMessage(string aMessage)
   {
      //Code to write a message in system's event viewer.
   }
}
```
A musíme pøidat podmínku do tøídy DataExporter jako v následujícím:

```
public class DataExporter
{
   public void ExportDataFromFile()
   {
      ExceptionLogger _exceptionLogger;
      try {
         //code to export data from files to database.
      }
      catch(IOException ex)
      {
         _exceptionLogger = new ExceptionLogger(new DbLogger());
         _exceptionLogger.LogException(ex);
      }
      catch(SqlException ex)
      {
         _exceptionLogger = new ExceptionLogger(new EventLogger());
         _exceptionLogger.LogException(ex);
      }
      catch(Exception ex)
      {
         _exceptionLogger = new ExceptionLogger(new FileLogger());
         _exceptionLogger.LogException(ex);
      }
   }
}
```

Vypadá dobøe. Ale zavedli jsme závislost zde v blocích catch tøídy DataExporter. Nìkdo tedy musí být zodpovìdný za poskytnutí nezbytných objektù do ExceptionLogger, aby byla práce hotová.

Dovolte mi to vysvìtlit na pøíkladu z reálného svìta. Pøedpokládejme, že chceme mít døevìnou židli se specifickými rozmìry a druhem døeva, z nìhož bude tato židle vyrobena. Rozhodování o opatøeních a døevì pak nemùžeme nechat na truhláøi. Zde je jeho úkolem vyrobit židli na základì našich požadavkù pomocí jeho nástrojù a my mu poskytneme specifikace, aby vyrobil dobrou židli.

Jaký je tedy pøínos, který máme z návrhu? Ano, rozhodnì jsme z toho tìžili. Kdykoli potøebujeme zavést novou funkci protokolování, musíme upravit tøídy DataExporter a ExceptionLogger. Ale v aktualizovaném návrhu potøebujeme pøidat pouze další blok catch pro novou funkci protokolování výjimek. Potøebujeme pouze správnì porozumìt systému, požadavkùm a prostøedí a najít oblasti, kde by se mìlo DIP dodržovat. Spojka není ve své podstatì zlo. Pokud nemáte nìjaké množství vazby, váš software za vás nic neudìlá.