# (LSP) Liskov Substitution Principle
Objekty z�kladn� t��dy mus� b�t zam�niteln� s objekty kter�koli z jej�ch odvozen�ch t��d, ani� by to ovlivnilo p�esnost programu.
Kter�hokoliv potomka by m�lo b�t mo�n� pou��t kdekoliv, kde je pou�it rodi�.

## P��klad 1
```
public abstract class Vehicle
{
    public abstract void StartEngine();
    public abstract void StopEngine();
}
public class Car : Vehicle
{
    public override void StartEngine()
    {
        Console.WriteLine("Starting the car engine.");
        // Code to start the car engine
    }
    public override void StopEngine()
    {
        Console.WriteLine("Stopping the car engine.");
        // Code to stop the car engine
    }
}
public class ElectricCar : Vehicle
{
    public override void StartEngine()
    {
        throw new InvalidOperationException("Electric cars do not have engines.");
    }
    public override void StopEngine()
    {
        throw new InvalidOperationException("Electric cars do not have engines.");
    }
}
```

### Probl�m
V tomto p��kladu m�me t��du Vehicle , kter� p�edstavuje generick� vozidlo. M� abstraktn� metody StartEngine() a StopEngine() pro spou�t�n� a zastavov�n� motoru. M�me tak� t��du Car , kter� d�d� z Vehicle a poskytuje nezbytnou implementaci pro metody souvisej�c� s motorem.

Kdy� v�ak p�edstav�me nov� typ vozidla, jako je ElectricCar , kter� nem� motor, naraz�me na poru�en� LSP. V tomto p��pad� by pokus o vol�n� metod StartEngine() nebo StopEngine() na objektu ElectricCar vedl k v�jimk�m, proto�e elektrick� auta nemaj� motory.

```
Vehicle car = new Car();
car.StartEngine(); // Outputs "Starting the car engine."
Vehicle electricCar = new ElectricCar();
electricCar.StartEngine(); // Throws InvalidOperationException
```

### �e�en�
Abychom toto poru�en� vy�e�ili, mus�me zajistit spr�vnou z�m�nu objekt�. Jedn�m z p��stup� je zaveden� rozhran� nazvan�ho IEnginePowered , kter� p�edstavuje vozidla s motory.

```
public abstract class Vehicle
{
    // Common vehicle behavior and properties.
}
public interface IEnginePowered
{
    void StartEngine();
    void StopEngine();
}
public class Car : Vehicle, IEnginePowered
{
    public void StartEngine()
    {
        Console.WriteLine("Starting the car engine.");
        // Code to start the car engine.
    }
    public void StopEngine()
    {
        Console.WriteLine("Stopping the car engine.");
        // Code to stop the car engine.
    }
}
public class ElectricCar : Vehicle
{
    // Specific behavior for electric cars.
}
```

V tomto opraven�m n�vrhu t��da Car implementuje rozhran� IEnginePowered spolu s t��dou Vehicle . T��da vozidla bude zahrnovat spole�n� vlastnosti a chov�n� vozidla pro oba. Tento n�vrh poskytuje nezbytnou implementaci pro metody souvisej�c� s motorem. T��da ElectricCar tak� neimplementuje rozhran� IEnginePowered , proto�e nem� motor.

```
IEnginePowered car = new Car();
car.StartEngine(); // Outputs "Starting the car engine."
Vehicle electricCar = new ElectricCar();
electricCar.StartEngine(); // No exception; no action is performed.
```

Pot� uprav�me design tak, aby Vehicle z�stalo z�kladn� t��dou pro v�echna vozidla, IEnginePowered deklaruje metody souvisej�c� s motorem a Car implementuje Vehicle i IEnginePowered , zat�mco ElectricCar pouze roz�i�uje Vehicle . T�mto zp�sobem instance ElectricCar nemus� implementovat metody souvisej�c� s motorem, ��m� se zabr�n� v�jimk�m.

Pomoc� LSP jsme zajistili, �e program z�stane p�esn� a konzistentn� p�i nahrazov�n� objekt� odvozen�ch t��d za objekty jejich z�kladn� t��dy.

## P��klad 2
P�edpokl�dejme, �e pot�ebujeme vytvo�it aplikaci pro spr�vu dat pomoc� skupiny textov�ch soubor� SQL. Zde mus�me napsat funkcionalitu pro na�ten� a ulo�en� textu skupiny SQL soubor� v adres��i aplikace. Pot�ebujeme tedy t��du, kter� spravuje zat�en� a uchov�v� text skupiny soubor� SQL spolu s t��dou SqlFile.

```
public class SqlFile
{
   public string FilePath {get;set;}
   public string FileText {get;set;}
   public string LoadText()
   {
      /* Code to read text from sql file */
   }
   public string SaveText()
   {
      /* Code to save text into sql file */
   }
}

public class SqlFileManager
{
   public List<SqlFile> lstSqlFiles {get;set}

   public string GetTextFromFiles()
   {
      StringBuilder objStrBuilder = new StringBuilder();
      foreach(var objFile in lstSqlFiles)
      {
         objStrBuilder.Append(objFile.LoadText());
      }
      return objStrBuilder.ToString();
   }
   public void SaveTextIntoFiles()
   {
      foreach(var objFile in lstSqlFiles)
      {
         objFile.SaveText();
      }
   }
}
```

Po n�jak� dob� n�m v�ak na�i vedouc� mohou ��ci, �e m��eme m�t ve slo�ce aplikace n�kolik soubor� pouze pro �ten�, tak�e mus�me omezit tok, kdykoli se je pokus� ulo�it.
Mus�me upravit "SqlFileManager" p�id�n�m jedn� podm�nky do smy�ky, abychom se vyhnuli v�jimce. M��eme to ud�lat vytvo�en�m t��dy "ReadOnlySqlFile", kter� zd�d� t��du "SqlFile", a mus�me zm�nit metodu SaveTextIntoFiles() zaveden�m podm�nky, kter� zabr�n� vol�n� metody SaveText() na instanc�ch ReadOnlySqlFile. 

```
public class SqlFileManager
{
   public List<SqlFile? lstSqlFiles {get;set}

   public string GetTextFromFiles()
   {
      StringBuilder objStrBuilder = new StringBuilder();
      foreach(var objFile in lstSqlFiles)
      {
         objStrBuilder.Append(objFile.LoadText());
      }
      return objStrBuilder.ToString();
   }

   public void SaveTextIntoFiles()
   {
      foreach(var objFile in lstSqlFiles)
      {
         //Check whether the current file object is read-only or not.If yes, skip calling it's
         // SaveText() method to skip the exception.

         if(! objFile is ReadOnlySqlFile)
         objFile.SaveText();
      }
   }
}
```

Zde jsme zm�nili metodu SaveTextIntoFiles() ve t��d� SqlFileManager, abychom ur�ili, zda je instance ReadOnlySqlFile, abychom se vyhnuli v�jimce. Tuto t��du ReadOnlySqlFile nem��eme pou��t jako n�hradu za jej�ho rodi�e, ani� bychom zm�nili k�d SqlFileManager. M��eme tedy ��ci, �e tento design nen�sleduje LSP. Ud�lejme tento design podle LSP. Zde p�edstav�me rozhran�, aby byla t��da SqlFileManager nez�visl� na ostatn�ch bloc�ch.

```
public interface IReadableSqlFile
{
   string LoadText();
}
public interface IWritableSqlFile
{
   void SaveText();
}
```

Nyn� implementujeme IReadableSqlFile prost�ednictv�m t��dy ReadOnlySqlFile, kter� �te pouze text ze soubor� pouze pro �ten�. IWritableSqlFile i IReadableSqlFile implementujeme ve t��d� SqlFile, pomoc� kter� m��eme ��st a zapisovat soubory.

```
public class SqlFile: IWritableSqlFile,IReadableSqlFile
{
   public string FilePath {get;set;}
   public string FileText {get;set;}
   public string LoadText()
   {
      /* Code to read text from sql file */
   }
   public void SaveText()
   {
      /* Code to save text into sql file */
   }
}
```

Nyn� je n�vrh t��dy SqlFileManager podobn�.

```
public class SqlFileManager
{
   public string GetTextFromFiles(List<IReadableSqlFile> aLstReadableFiles)
   {
      StringBuilder objStrBuilder = new StringBuilder();
      foreach(var objFile in aLstReadableFiles)
      {
         objStrBuilder.Append(objFile.LoadText());
      }
      return objStrBuilder.ToString();
   }

   public void SaveTextIntoFiles(List<IWritableSqlFile> aLstWritableFiles)
   {
      foreach(var objFile in aLstWritableFiles)
      {
          objFile.SaveText();
      }
   }
}
```

Zde metoda GetTextFromFiles() z�sk� pouze seznam instanc� t��d, kter� implementuj� rozhran� IReadOnlySqlFile. To znamen� instance t��dy SqlFile a ReadOnlySqlFile. A metoda SaveTextIntoFiles() z�sk�v� pouze seznam instanc� t��dy, kter� implementuje rozhran� IWritableSqlFiles, v tomto p��pad� instance SqlFile. Nyn� tedy m��eme ��ci, �e n� design n�sleduje LSP. A probl�m jsme vy�e�ili pomoc� principu segregace rozhran� (ISP), identifikace abstrakce a metody odd�len� odpov�dnosti