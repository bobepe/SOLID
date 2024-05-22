# (DIP) Dependency Inversion Principle
Princip Inversion Inversion navrhuje, �e moduly na vysok� �rovni by nem�ly z�viset na modulech n�zk� �rovn�, ale oba by m�ly z�viset na abstrakc�ch. Nav�c by abstrakce nem�ly z�viset na detailech; podrobnosti by m�ly z�viset na abstrakc�ch.


## P��klad 1
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
T��da Switchp��mo z�vis� na konkr�tn� LightBulbt��d� a poru�uje DIP.

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
Zaveden�m rozhran� ( ISwitchable) Switchnyn� t��da z�vis� na abstrakci a dodr�uje Princip inverze z�vislosti.

Podle DIP nepi�te ��dn� pevn� propojen� k�d, proto�e to je no�n� m�ra, kterou je t�eba udr�ovat, kdy� se aplikace zv�t�uje a zv�t�uje. Pokud t��da z�vis� na jin� t��d�, pak mus�me zm�nit jednu t��du, pokud se v t�to z�visl� t��d� n�co zm�n�. V�dy bychom se m�li sna�it ps�t voln� spojen� t��dy.

## P��klad 2

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
V p�edchoz�m p��kladu se UserController t�sn� propojuje s konkr�tn� t��dou Database a vytv��� p��mou z�vislost. Pokud se rozhodneme zm�nit implementaci datab�ze nebo zav�st nov� mechanismus �lo�i�t�, budeme muset upravit t��du UserController , kter� poru�uje princip Open-Closed.

Abychom tento probl�m vy�e�ili a dodr�eli DIP, mus�me p�evr�tit z�vislosti zaveden�m abstrakce, na kter� z�vis� moduly vysok� i n�zk� �rovn�. Obvykle je tato abstrakce definov�na pomoc� rozhran� nebo abstraktn� t��dy.
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

V t�to aktualizovan� verzi p�edstavujeme rozhran� IDataStorage , kter� definuje smlouvu o operac�ch ukl�d�n� dat. T��da Database implementuje toto rozhran� a poskytuje konkr�tn� implementaci. V d�sledku toho se t��da UserController nyn� spol�h� na rozhran� IDataStorage sp�e ne� na konkr�tn� t��du Database , co� vede k tomu, �e je odd�lena od konkr�tn�ch mechanism� �lo�i�t�.

Tato inverze z�vislost� usnad�uje roz�i�itelnost a �dr�bu. M��eme zav�st nov� implementace �lo�i�t�, jako je souborov� syst�m nebo cloudov� �lo�i�t�, jednoduch�m vytvo�en�m nov�ch t��d, kter� implementuj� rozhran� IDataStorage , ani� bychom museli upravovat UserController nebo jak�koli jin� moduly na vysok� �rovni.

## P��klad 3
Princip inverze z�vislosti (DIP) uv�d�, �e moduly/t��dy na vysok� �rovni by nem�ly z�viset na modulech/t��d�ch ni��� �rovn�. Za prv�, oboj� by m�lo z�viset na abstrakc�ch. Za druh�, abstrakce by se nem�ly spol�hat na detaily. Kone�n�, detaily by m�ly z�viset na abstrakc�ch.

Moduly/t��dy na vysok� �rovni implementuj� obchodn� pravidla nebo logiku v syst�mu (aplikaci). N�zko�rov�ov� moduly/t��dy se zab�vaj� podrobn�j��mi operacemi; jin�mi slovy, mohou zapisovat informace do datab�z� nebo p�ed�vat zpr�vy opera�n�mu syst�mu nebo slu�b�m.

Vysoko�rov�ov� modul/t��da, kter� z�vis� na n�zko�rov�ov�ch modulech/t��d�ch nebo na n�jak� jin� t��d� a v� hodn� o ostatn�ch t��d�ch, se kter�mi interaguje, se ��k�, �e je �zce propojen. Kdy� t��da v� explicitn� o n�vrhu a implementaci jin� t��dy, zvy�uje to riziko, �e zm�ny jedn� t��dy poru�� druhou. Mus�me tedy udr�ovat tyto moduly/t��dy na vysok� a n�zk� �rovni co nejv�ce voln� propojen�. Abychom toho dos�hli, mus�me je oba u�init z�visl�mi na abstrakc�ch m�sto toho, abychom se navz�jem znali. Za�n�me p��kladem.

P�edpokl�dejme, �e pot�ebujeme pracovat na modulu pro protokolov�n� chyb, kter� zaznamen�v� trasov�n� z�sobn�ku v�jimek do souboru. Jednoduch�, �e? N�sleduj�c� t��dy poskytuj� funkce pro protokolov�n� trasov�n� z�sobn�ku do souboru.

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
T��da klienta exportuje data z mnoha soubor� do datab�ze.

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
Vypad� dob�e. Klientovi jsme odeslali na�i ��dost. N� klient v�ak chce toto trasov�n� z�sobn�ku ulo�it do datab�ze, pokud dojde k v�jimce IO. Hmm... OK, ��dn� probl�m. I to um�me implementovat. Zde mus�me p�idat jednu dal�� t��du, kter� poskytuje funkci pro protokolov�n� trasov�n� z�sobn�ku do datab�ze a dal�� metodu v ExceptionLogger pro interakci s na�� novou t��dou pro protokolov�n� trasov�n� z�sobn�ku.

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
Zat�m to vypad� dob�e. Ale kdykoli chce klient zav�st nov� logger, mus�me ExceptionLogger zm�nit p�id�n�m nov� metody. P�edpokl�dejme, �e v tom budeme po n�jak� dob� pokra�ovat. V takov�m p��pad� uvid�me tlustou t��du ExceptionLogger s velkou sadou postup�, kter� poskytuj� funkcionalitu pro p�ihl�en� zpr�vy do r�zn�ch c�l�. Pro� k tomuto probl�mu doch�z�? Proto�e ExceptionLogger p��mo kontaktuje n�zko�rov�ov� t��dy FileLogger a DbLogger, aby zaprotokolovaly v�jimku. Pot�ebujeme zm�nit design, aby tato t��da ExceptionLogger mohla b�t voln� spojena s t�mito t��dami. Abychom to ud�lali, mus�me mezi n� zav�st abstrakci, aby ExcetpionLogger mohl kontaktovat abstrakci a zaprotokolovat v�jimku m�sto p��m� z�vislosti na t��d�ch n�zk� �rovn�.

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
Nyn� se p�esuneme k iniciaci n�zko�rov�ov� t��dy z t��dy ExcetpionLogger do t��dy DataExporter, aby byl ExceptionLogger voln� spojen� s n�zko�rov�ov�mi t��dami FileLogger a EventLogger. A t�m poskytujeme t��d� DataExporter mo�nost rozhodnout, jak� druh Loggeru by m�l b�t vol�n na z�klad� v�jimky, kter� nastane.

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
�sp�n� jsme odstranili z�vislost na t��d�ch n�zk� �rovn�. Tento ExceptionLogger nez�vis� na t��d�ch FileLogger a EventLogger pro protokolov�n� trasov�n� z�sobn�ku. Pro novou funkci protokolov�n� ji� nepot�ebujeme m�nit k�d ExceptionLogger. Mus�me vytvo�it novou t��du protokolov�n�, kter� implementuje rozhran� ILogger a p�id� dal�� blok catch do metody ExportDataFromFile t��dy DataExporter.

```
public class EventLogger: ILogger
{
   public void LogMessage(string aMessage)
   {
      //Code to write a message in system's event viewer.
   }
}
```
A mus�me p�idat podm�nku do t��dy DataExporter jako v n�sleduj�c�m:

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

Vypad� dob�e. Ale zavedli jsme z�vislost zde v bloc�ch catch t��dy DataExporter. N�kdo tedy mus� b�t zodpov�dn� za poskytnut� nezbytn�ch objekt� do ExceptionLogger, aby byla pr�ce hotov�.

Dovolte mi to vysv�tlit na p��kladu z re�ln�ho sv�ta. P�edpokl�dejme, �e chceme m�t d�ev�nou �idli se specifick�mi rozm�ry a druhem d�eva, z n�ho� bude tato �idle vyrobena. Rozhodov�n� o opat�en�ch a d�ev� pak nem��eme nechat na truhl��i. Zde je jeho �kolem vyrobit �idli na z�klad� na�ich po�adavk� pomoc� jeho n�stroj� a my mu poskytneme specifikace, aby vyrobil dobrou �idli.

Jak� je tedy p��nos, kter� m�me z n�vrhu? Ano, rozhodn� jsme z toho t�ili. Kdykoli pot�ebujeme zav�st novou funkci protokolov�n�, mus�me upravit t��dy DataExporter a ExceptionLogger. Ale v aktualizovan�m n�vrhu pot�ebujeme p�idat pouze dal�� blok catch pro novou funkci protokolov�n� v�jimek. Pot�ebujeme pouze spr�vn� porozum�t syst�mu, po�adavk�m a prost�ed� a naj�t oblasti, kde by se m�lo DIP dodr�ovat. Spojka nen� ve sv� podstat� zlo. Pokud nem�te n�jak� mno�stv� vazby, v� software za v�s nic neud�l�.