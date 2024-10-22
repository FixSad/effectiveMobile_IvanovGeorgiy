using DeliveryService;

Properties.Default.DeliveryOrder = "Test";
Properties.Default.Save();

Console.WriteLine($"Hello, {Properties.Default.DeliveryOrder}");
