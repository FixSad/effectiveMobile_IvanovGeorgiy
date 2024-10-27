using DeliveryService.Domain.ViewModels;
using DeliveryService.Service.Interfaces;
using DeliveryService.Service.Implementations;
using DeliveryService.Service;
using System.Text;
using System.Globalization;

IOrderService<OrderViewModel> orderService = new OrderService();

// На всякий случай настроил переменные из properties.settings, если сбилось
Properties.Default.DeliveryOrder = "DeliveryOrder.txt";
Properties.Default.AllOrders = "AllOrders.txt";
Console.WriteLine("Добро пожаловать! \n" +
            "Файл с результатом и бд будут лежать в DeliveryService\\DeliveryService\\bin\\Debug\\net8.0 \n" +
            "Нажмите любую клавишу для продолжения...");
Console.ReadKey();
Console.Clear();
while (true)
{
	try
	{
        if (Welcome())
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1. Добавить заказ\n" +
                                  "2. Удалить заказ\n" +
                                  "3. Отсортировать заказ\n" +
                                  "4. Выйти\n" +
                                  "Список заказов:");
                Console.WriteLine(GetAllOrders());
                ConsoleKeyInfo key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        Console.WriteLine(CreateOrder());
                        break;
                    case ConsoleKey.D2:
                        Console.WriteLine(DeleteOrder());
                        break;
                    case ConsoleKey.D3:
                        Console.WriteLine(SortOrders());
                        break;
                    case ConsoleKey.D4:
                        Environment.Exit(0);
                        break;
                    default:
                        continue;
                }
                Console.ReadKey();
            }
        }
        else
            continue;

	}
	catch (Exception ex)
	{
        Console.WriteLine(ex.Message);
	}
}

bool Welcome()
{
    Console.Write("Введите путь к папке, где будут храниться логи. Напишите \\ на конце: ");
    string? path = Console.ReadLine();
    if (path.Length <= 0 || !path.EndsWith("\\"))
    {
        Console.WriteLine("Вы не написали путь, либо не закончили его на \\. Нажмите любую клавишу для продолжения...");
        Console.ReadKey(true);
        Console.Clear();
        return false;
    }
    if (!Directory.Exists(path))
    {
        Console.WriteLine("Путь не существует. Нажмите любую клавишу для продолжения...");
        Console.ReadKey(true);
        Console.Clear();
        return false;
    }
    Properties.Default.DeliveryLog = path;
    return true;
}

string GetAllOrders()
{
    var response = orderService.GetAllOrders().Result;
    if (response.IsSuccess)
    {
        List<OrderViewModel> orders = response.Data;

        StringBuilder result = new StringBuilder();

        foreach (var order in orders)
        {
            result.Append($"Id: {order.Id}, Вес: {order.Weight}, Район: {order.DistrictId}, Дата: {order.DeliveryDateTime}\n");
        }

        return result.ToString();
    }
    return response.Description;
}

string DeleteOrder()
{
    Console.Write("Введите id заказа для удаления: ");

    string? id = Console.ReadLine();

    var response = orderService.Delete(id).Result;

    return response.Description;
}

string CreateOrder()
{
    Console.Write("Введите Id района: ");
    string? districtId = Console.ReadLine();
    Console.Write("Введите вес: ");
    string? weight = Console.ReadLine(); 
    Console.Write("Введите дату в формате {yyyy-MM-dd HH:mm:ss}: ");
    string? date = Console.ReadLine();

    try
    {
        var response = orderService.Create(new OrderViewModel()
        {
            DistrictId = int.Parse(districtId),
            Weight = float.Parse(weight),
            DeliveryDateTime = DateTime.ParseExact(date,
                        "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
        }).Result;

        return response.Description ;
    }
    catch(Exception ex)
    {
        return ex.Message;
    }
}

string SortOrders()
{
    Console.Write("Введите Id района: ");
    string? districtId = Console.ReadLine();
    Console.Write("Введите дату начала сортировки в формате {yyyy-MM-dd HH:mm:ss}: ");
    string? startDate = Console.ReadLine();

    var response = orderService.Sort(districtId, startDate).Result;

    return response.Description;
}
