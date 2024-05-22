# (LSP) Liskov Substitution Principle
Objekty základní tøídy musí být zamìnitelné s objekty kterékoli z jejích odvozených tøíd, aniž by to ovlivnilo pøesnost programu.
Kteréhokoliv potomka by mìlo být možné použít kdekoliv, kde je použit rodiè.

## Pøíklad 1
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

### Problém
V tomto pøíkladu máme tøídu Vehicle , která pøedstavuje generické vozidlo. Má abstraktní metody StartEngine() a StopEngine() pro spouštìní a zastavování motoru. Máme také tøídu Car , která dìdí z Vehicle a poskytuje nezbytnou implementaci pro metody související s motorem.

Když však pøedstavíme nový typ vozidla, jako je ElectricCar , které nemá motor, narazíme na porušení LSP. V tomto pøípadì by pokus o volání metod StartEngine() nebo StopEngine() na objektu ElectricCar vedl k výjimkám, protože elektrická auta nemají motory.

```
Vehicle car = new Car();
car.StartEngine(); // Outputs "Starting the car engine."
Vehicle electricCar = new ElectricCar();
electricCar.StartEngine(); // Throws InvalidOperationException
```

### Øešení
Abychom toto porušení vyøešili, musíme zajistit správnou zámìnu objektù. Jedním z pøístupù je zavedení rozhraní nazvaného IEnginePowered , které pøedstavuje vozidla s motory.

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

V tomto opraveném návrhu tøída Car implementuje rozhraní IEnginePowered spolu s tøídou Vehicle . Tøída vozidla bude zahrnovat spoleèné vlastnosti a chování vozidla pro oba. Tento návrh poskytuje nezbytnou implementaci pro metody související s motorem. Tøída ElectricCar také neimplementuje rozhraní IEnginePowered , protože nemá motor.

```
IEnginePowered car = new Car();
car.StartEngine(); // Outputs "Starting the car engine."
Vehicle electricCar = new ElectricCar();
electricCar.StartEngine(); // No exception; no action is performed.
```

Poté upravíme design tak, aby Vehicle zùstalo základní tøídou pro všechna vozidla, IEnginePowered deklaruje metody související s motorem a Car implementuje Vehicle i IEnginePowered , zatímco ElectricCar pouze rozšiøuje Vehicle . Tímto zpùsobem instance ElectricCar nemusí implementovat metody související s motorem, èímž se zabrání výjimkám.

Pomocí LSP jsme zajistili, že program zùstane pøesný a konzistentní pøi nahrazování objektù odvozených tøíd za objekty jejich základní tøídy.

## Pøíklad 2
Pøedpokládejme, že potøebujeme vytvoøit aplikaci pro správu dat pomocí skupiny textových souborù SQL. Zde musíme napsat funkcionalitu pro naètení a uložení textu skupiny SQL souborù v adresáøi aplikace. Potøebujeme tedy tøídu, která spravuje zatížení a uchovává text skupiny souborù SQL spolu s tøídou SqlFile.

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

Po nìjaké dobì nám však naši vedoucí mohou øíci, že mùžeme mít ve složce aplikace nìkolik souborù pouze pro ètení, takže musíme omezit tok, kdykoli se je pokusí uložit.
Musíme upravit "SqlFileManager" pøidáním jedné podmínky do smyèky, abychom se vyhnuli výjimce. Mùžeme to udìlat vytvoøením tøídy "ReadOnlySqlFile", která zdìdí tøídu "SqlFile", a musíme zmìnit metodu SaveTextIntoFiles() zavedením podmínky, která zabrání volání metody SaveText() na instancích ReadOnlySqlFile. 

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

Zde jsme zmìnili metodu SaveTextIntoFiles() ve tøídì SqlFileManager, abychom urèili, zda je instance ReadOnlySqlFile, abychom se vyhnuli výjimce. Tuto tøídu ReadOnlySqlFile nemùžeme použít jako náhradu za jejího rodièe, aniž bychom zmìnili kód SqlFileManager. Mùžeme tedy øíci, že tento design nenásleduje LSP. Udìlejme tento design podle LSP. Zde pøedstavíme rozhraní, aby byla tøída SqlFileManager nezávislá na ostatních blocích.

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

Nyní implementujeme IReadableSqlFile prostøednictvím tøídy ReadOnlySqlFile, která ète pouze text ze souborù pouze pro ètení. IWritableSqlFile i IReadableSqlFile implementujeme ve tøídì SqlFile, pomocí které mùžeme èíst a zapisovat soubory.

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

Nyní je návrh tøídy SqlFileManager podobný.

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

Zde metoda GetTextFromFiles() získá pouze seznam instancí tøíd, které implementují rozhraní IReadOnlySqlFile. To znamená instance tøídy SqlFile a ReadOnlySqlFile. A metoda SaveTextIntoFiles() získává pouze seznam instancí tøídy, která implementuje rozhraní IWritableSqlFiles, v tomto pøípadì instance SqlFile. Nyní tedy mùžeme øíci, že náš design následuje LSP. A problém jsme vyøešili pomocí principu segregace rozhraní (ISP), identifikace abstrakce a metody oddìlení odpovìdnosti