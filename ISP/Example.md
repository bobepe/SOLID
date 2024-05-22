# (ISP) Interface Segregation Principle
1.1. Princip segregace rozhraní uvádí, že tøída by nemìla být nucena implementovat rozhraní, která nepoužívá. Tento princip podporuje vytváøení malých rozhraní specifických pro klienta.

1.2. Princip segregace rozhraní uvádí, že "klienti by nemìli být nuceni implementovat rozhraní, která nepoužívají. Místo jednoho tlustého rozhraní je upøednostòováno mnoho malých rozhraní založených na skupinách metod, z nichž každé obsluhuje jeden submodul."

Mùžeme to definovat i jinak. Rozhraní by mìlo více souviset s kódem, který jej používá, než s kódem, který jej implementuje. Metody na rozhraní jsou tedy definovány tím, které metody klientský kód potøebuje, spíše než které metody tøída implementuje. Klienti by tedy nemìli být nuceni záviset na rozhraních, která nepoužívají.

Nemìli byste být nuceni implementovat rozhraní, když váš objekt tento úèel nesdílí. Èím vìtší je rozhraní, tím pravdìpodobnìji obsahuje metody, které nemohou používat všichni implementátoøi. To je podstata principu segregace rozhraní.

## Pøíklad 1

```
public interface IWorker
{
    void Work();
    void Eat();
}

public class Manager : IWorker
{
    // implementation
}

public class Robot : IWorker
{
    // implementation
}
```
Tøída Robotje nucena implementovat Eatmetodu, èímž porušuje ISP.

```
public interface IWorkable
{
    void Work();
}

public interface IEatable
{
    void Eat();
}

public class Manager : IWorkable, IEatable
{
    // implementation
}

public class Robot : IWorkable
{
    // implementation
}
```
Rozdìlením IWorkerrozhraní na menší rozhraní ( IWorkablea IEatable) mohou tøídy implementovat pouze to, co potøebují, v souladu s ISP.

Podle LSP by žádný klient nemìl být nucen používat rozhraní, které je pro nìj irelevantní. Jinými slovy, klienti by nemìli být nuceni záviset na metodách, které nepoužívají.

## Pøíklad 2
Pøedpokládejme, že potøebujeme vybudovat systém pro IT firmu, který obsahuje role jako TeamLead a Programmer, kde TeamLead rozdìluje velké úkoly na menší úkoly a pøidìluje je svým programátorùm nebo na nich mùže pøímo pracovat.

Na základì specifikací musíme vytvoøit rozhraní a tøídu TeamLead k jeho implementaci. 
```
public Interface ILead
{
   void CreateSubTask();
   void AssginTask();
   void WorkOnTask();
}

public class TeamLead : ILead
{
   public void AssignTask()
   {
      //Code to assign a task.
   }
   public void CreateSubTask()
   {
      //Code to create a sub task
   }
   public void WorkOnTask()
   {
      //Code to implement perform assigned task.
   }
}
```

Pozdìji se však do systému zavede další role, napøíklad manažer, který pøidìluje úkoly TeamLead a nebude na nich pracovat.

```
public class Manager: ILead
{
   public void AssignTask()
   {
      //Code to assign a task.
   }
   public void CreateSubTask()
   {
      //Code to create a sub task.
   }
   public void WorkOnTask()
   {
      throw new Exception("Manager can't work on Task");
   }
}
```
Protože manažer nemùže pracovat na úkolu a zároveò nikdo nemùže pøidìlovat úkoly manažerovi, tato WorkOnTask() by nemìla být ve tøídì Manager. Ale implementujeme tuto tøídu z rozhraní ILead; musíme poskytnout konkrétní metodu. Zde nutíme tøídu Manager implementovat metodu WorkOnTask() bez úèelu. To je špatnì. Design porušuje ISP.

Máme 3 role. Manažer (rozdìluje a pøidìluje úkoly), TeamLead (pøidìlovat, rozdìlovat, pracovat), Programátor (pracovat)
```
public interface IProgrammer
{
   void WorkOnTask();
}
```

Rozhraní, které poskytuje smlouvy pro správu úkolù:
```
public interface ILead
{
   void AssignTask();
   void CreateSubTask();
}
```

Implementace
```
public class Programmer: IProgrammer
{
   public void WorkOnTask()
   {
      //code to implement to work on the Task.
   }
}

public class Manager: ILead
{
   public void AssignTask()
   {
      //Code to assign a Task
   }
   public void CreateSubTask()
   {
   //Code to create a sub taks from a task.
   }
}

public class TeamLead: IProgrammer, ILead
{
   public void AssignTask()
   {
      //Code to assign a Task
   }
   public void CreateSubTask()
   {
      //Code to create a sub task from a task.
   }
   public void WorkOnTask()
   {
      //code to implement to work on the Task.
   }
}
```

## Pøíklad 3

```
public interface IOrder
{
    void PlaceOrder();
    void CancelOrder();
    void UpdateOrder();
    void CalculateTotal();
    void GenerateInvoice();
    void SendConfirmationEmail();
    void PrintLabel();
}

public class OnlineOrder : IOrder
{
    // Implementation of all methods.
}

public class InStoreOrder : IOrder
{
    // Implementation of all methods.
}
```

V pøedchozím pøíkladu máme rozhraní IOrder , které obsahuje metody pro zadání objednávky, zrušení objednávky, aktualizaci objednávky, výpoèet souètu, vygenerování faktury, odeslání potvrzovacího e-mailu a tisk štítku.

Ne všechny tøídy klientù implementující toto rozhraní však vyžadují nebo používají všechny tyto metody. To porušuje ISP, protože klienti jsou nuceni záviset na metodách, které nepotøebují.

Sledováním ISP mùžeme refaktorovat kód rozdìlením rozhraní na menší, více zamìøená rozhraní.

```
public interface IOrder
{
    void PlaceOrder();
    void CancelOrder();
    void UpdateOrder();
}

public interface IOrderProcessing
{
    void CalculateTotal();
}

public interface IInvoiceGenerator
{
    void GenerateInvoice();
}

public interface IEmailSender
{
    void SendConfirmationEmail();
}

public interface ILabelPrinter
{
    void PrintLabel();
}

// Implement only the necessary interfaces in client classes.
public class OnlineOrder : IOrder, IOrderProcessing, IInvoiceGenerator, IEmailSender
{
    // Implementation of required methods.
}

public class InStoreOrder : IOrder, IOrderProcessing, ILabelPrinter
{
    // Implementation of required methods.
}
```

Oddìlením rozhraní nyní máme menší, více zamìøená rozhraní, která si klienti mohou zvolit implementovat na základì svých specifických potøeb. Tento pøístup odstraòuje zbyteèné závislosti a umožòuje lepší rozšiøitelnost a udržovatelnost. Klienti mohou implementovat pouze rozhraní, která požadují, výsledkem je èistší kód, který je snazší pochopit, testovat a upravovat.