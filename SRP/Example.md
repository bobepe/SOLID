# (SRP) Single Responsibility Principle 
Každá tøída nebo podobná struktura ve vašem kódu by mìla mít pouze jednu úlohu. Vše v této tøídì by mìlo souviset s jediným úèelem.

Resp. nedìlejme pøíliš složité tøídy. Spíš udìlat víc jednoduchých tøíd než jednu složitou

## Pøíklad 1

``` c#
public class UserCreator
{
    public void CreateUser(string username, string email, string password)
    {
        // Validation logic
        if (!ValidateEmail(email))
        {
            throw new ArgumentException("Invalid email format.");
        }
        // Business rules
        // Database persistence
        SaveUserToDatabase(username, email, password);
    }
    private bool ValidateEmail(string email)
    {
        // Validation logic
    }
    private void SaveUserToDatabase(string username, string email, string password)
    {
        // Database persistence logic
    }
}
```

### Problém
V tomto kódu tøída UserCreator porušuje SRP tím, že kombinuje více odpovìdností, jako je ovìøování a perzistence databáze. To mùže vést k tìsnì propojené tøídì, což ztìžuje testování a je náchylné ke zbyteèným úpravám.

### Øešení
Abychom tento problém vyøešili, mùžeme použít SRP refaktorováním kódu tak, aby byly tyto odpovìdnosti rozdìleny do jednotlivých tøíd.

``` c#
public class UserValidator
{
    public bool ValidateEmail(string email)
    {
        // Validation logic
    }
}
public class UserRepository
{
    public void SaveUser(string username, string email, string password)
    {
        // Database persistence logic
    }
}
public class UserCreator
{
    private readonly UserValidator _validator;
    private readonly UserRepository _repository;
    public UserCreator(UserValidator validator, UserRepository repository)
    {
        _validator = validator;
        _repository = repository;
    }
    public void CreateUser(string username, string email, string password)
    {
        if (!_validator.ValidateEmail(email))
        {
            throw new ArgumentException("Invalid email format.");
        }
        // Business rules
        _repository.SaveUser(username, email, password);
    }
}
```

## Pøíklad 2

```
// Bad example violating SRP
public class Customer
{
    public void AddCustomer()
    {
        // Code to add a customer to the database
    }

    public void SendEmail()
    {
        // Code to send an email to the customer
    }
}

// Good example following SRP
public class CustomerService
{
    public void AddCustomer()
    {
        // Code to add a customer to the database
    }
}

public class EmailService
{
    public void SendEmail()
    {
        // Code to send an email to the customer
    }
}
```

V daném pøíkladu kódu máme tøídu nazvanou Zákazník , která porušuje princip jednotné odpovìdnosti (SRP). Tøída Customer je zodpovìdná jak za pøidání zákazníka do databáze, tak za odeslání e-mailu zákazníkovi. To porušuje SRP , protože tøída má nìkolik dùvodù ke zmìnì – pokud dojde ke zmìnám v databázových operacích nebo logice odesílání e-mailù, tøída Customer bude muset být upravena.

Abychom tento problém vyøešili a øídili se SRP, mùžeme kód refaktorovat rozdìlením odpovìdností do dvou odlišných tøíd: CustomerService a EmailService .

Tøída CustomerService je zodpovìdná za pøidání zákazníka do databáze. Obsahuje metodu nazvanou AddCustomer() , která zpracovává operace související s databází. Oddìlením funkcí souvisejících s databází do vlastní tøídy dodržujeme SRP , protože tøída CustomerService má nyní jedinou odpovìdnost .

Tøída EmailService je zodpovìdná za odesílání e-mailù zákazníkùm. Obsahuje metodu nazvanou SendEmail() , která zpracovává logiku odesílání e-mailù. Pøesunutím funkcí souvisejících s e-mailem do vlastní tøídy je oddìlíme od databázových operací a dodržujeme SRP .

Rozdìlením odpovìdností do samostatných tøíd dosáhneme lepšího oddìlení zájmù , což vede k udržitelnìjšímu a flexibilnìjšímu kódu. Nyní, pokud dojde ke zmìnám v databázových operacích nebo logice odesílání e-mailù, potøebujeme pouze upravit pøíslušnou tøídu, èímž se minimalizuje dopad na ostatní èásti kódové základny.