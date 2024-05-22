# (OCP) Open/Closed Principle
Entita by mìla být otevøená pro rozšíøení, ale uzavøená pro úprvy. "Otevøít pro rozšíøení" znamená, že musíme navrhnout náš modul/tøídu tak, aby nové funkce mohly být pøidány pouze tehdy, když jsou vygenerovány nové požadavky. "Uzavøeno pro úpravy" znamená, že jsme již vyvinuli tøídu a prošla jednotkovým testováním. Pak bychom jej nemìli mìnit, dokud nenajdeme chyby. Jak se øíká, tøída by mìla být otevøená pro rozšíøení; mùžeme použít dìdiènost.

## Pøíklad 1
```
public class Rectangle{
   public double Height {get;set;}
   public double Wight {get;set; }
}
```

Naše aplikace potøebuje vypoèítat celkovou plochu kolekce obdélníkù. Protože jsme se již nauèili Princip jednotné odpovìdnosti (SRP), nemusíme do obdélníku vkládat kód výpoètu celkové plochy. Takže zde jsem vytvoøil další tøídu pro výpoèet plochy.

```
public class AreaCalculator {
   public double TotalArea(Rectangle[] arrRectangles)
   {
      double area;
      foreach(var objRectangle in arrRectangles)
      {
         area += objRectangle.Height * objRectangle.Width;
      }
      return area;
   }
}
```

Mùžeme ale naši aplikaci rozšíøit tak, aby dokázala vypoèítat plochu nejen obdélníkù, ale i kruhù? Nyní máme problém s výpoètem plochy, protože zpùsob výpoètu plochy kruhu je jiný. Mùžeme zmìnit metodu TotalArea tak, aby akceptovala pole objektù jako argument.

```
public class Rectangle{
   public double Height {get;set;}
   public double Wight {get;set; }
}

public class Circle{
   public double Radius {get;set;}
}

public class AreaCalculator
{
   public double TotalArea(object[] arrObjects)
   {
      double area = 0;
      Rectangle objRectangle;
      Circle objCircle;
      foreach(var obj in arrObjects)
      {
         if(obj is Rectangle)
         {
            area += obj.Height * obj.Width;
         }
         else
         {
            objCircle = (Circle)obj;
            area += objCircle.Radius * objCircle.Radius * Math.PI;
         }
      }
      return area;
   }
}
```

### Problém
Mùžeme pøidat trojúhelník a vypoèítat jeho plochu pøidáním dalšího bloku "if" v metodì TotalArea AreaCalculator. Ale pokaždé, když zavedeme nový tvar, musíme zmìnit metodu TotalArea. Tøída AreaCalculator tedy není uzavøena pro úpravy.

### Øešení
Obecnì to mùžeme udìlat odkazem na abstrakce pro závislosti, jako jsou rozhraní nebo abstraktní tøídy, spíše než pomocí konkrétních tøíd.

```
public abstract class Shape
{
   public abstract double Area();
}

public class Rectangle: Shape
{
   public double Height {get;set;}
   public double Width {get;set;}
   public override double Area()
   {
      return Height * Width;
   }
}
public class Circle: Shape
{
   public double Radius {get;set;}
   public override double Area()
   {
      return Radius * Radius * Math.PI;
   }
}

public class AreaCalculator
{
   public double TotalArea(Shape[] arrShapes)
   {
      double area=0;
      foreach(var objShape in arrShapes)
      {
         area += objShape.Area();
      }
      return area;
   }
}
```

Nyní náš kód následuje jak SRP, tak OCP. Kdykoli zavedete nový tvar odvozením z abstraktní tøídy „Shape“, nemusíte tøídu „AreaCalculator“ mìnit.

Nebo to mùžeme udìlat pomocí rozhraní. Z:

```
public class Rectangle
{
    public double Width { get; set; }
    public double Height { get; set; }
}

public class AreaCalculator
{
    public double CalculateArea(Rectangle rectangle)
    {
        return rectangle.Width * rectangle.Height;
    }
}

```

Na:

```
public interface IShape
{
    double CalculateArea();
}

public class Rectangle : IShape
{
    // implementation
}

public class Circle : IShape
{
    // implementation
}
```

## Pøíklad 2
Uvažujme scénáø, kdy služba pro export souborù zpoèátku podporuje export dat do souborù CSV.

```
public class FileExporter
{
    public void ExportToCsv(string filePath, DataTable data)
    {
        // Code to export data to a CSV file.
    }
}
```

### Problém
V tomto pøíkladu tøída FileExporter pøímo implementuje funkce pro export dat do souborù CSV. Pokud však pozdìji chceme podporovat export dat do jiných formátù souborù, jako je Excel nebo JSON, úprava tøídy FileExporter by porušila OCP.

### Øešení
Abychom mohli používat OCP, musíme navrhnout naši doménu služby pro export souborù tak, aby byla otevøená pro rozšíøení.

Viz následující pøíklad refaktorovaného kódu.

```
public abstract class FileExporter
{
    public abstract void Export(string filePath, DataTable data);
}

public class CsvFileExporter : FileExporter
{
    public override void Export(string filePath, DataTable data)
    {
        // Code logic to export data to a CSV file.
    }
}

public class ExcelFileExporter : FileExporter
{
    public override void Export(string filePath, DataTable data)
    {
        // Code logic to export data to an Excel file.
    }
}

public class JsonFileExporter : FileExporter
{
    public override void Export(string filePath, DataTable data)
    {
        // Code logic to export data to a JSON file.
    }
}
```

Ve vylepšené implementaci zavádíme abstraktní tøídu FileExporter , která definuje spoleèné chování pro všechny operace exportu souborù. Každý konkrétní exportér souborù ( CsvFileExporter , ExcelFileExporter a JsonFileExporter ) dìdí z tøídy FileExporter a implementuje metodu Export podle konkrétní logiky exportu formátu souboru.

Použití OCP umožòuje pøidávat nové exportéry souborù bez úprav starých, což usnadòuje pøidávání nových funkcí zavedením podtøíd základní tøídy FileExporter.

Tento pøístup zvyšuje flexibilitu kódu, opìtovnou použitelnost a udržovatelnost. Váš kód bez problémù zvládne nové požadavky a zmìny, aniž by zavádìl chyby nebo narušoval stávající funkce.