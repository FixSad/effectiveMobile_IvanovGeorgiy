using DeliveryService.DAL.Interfaces;
using DeliveryService.Domain.ViewModels;
using System.IO;
using System.Reflection.Metadata;

namespace DeliveryService.DAL.Repositories
{
    public class OrderRepository : IBaseRepository<OrderViewModel>
    {
        private const string DELIVERY_ORDER = "DeliveryOrder.txt";
        private const string DELIVERY_LOG = "DeliveryLog.txt";
        private const string ALL_ORDERS = "AllOrders.txt";

        // Создание заказа
        public async void Create(OrderViewModel viewModel)
        {
            try
            {
                using (StreamWriter logWriter = new StreamWriter(DELIVERY_LOG, true))
                {
                    await logWriter.WriteLineAsync($"{DateTime.Now}: Создание заказа с номером {viewModel.Id}...");

                    // Проверка создан ли заказ с таким Id
                    List<string> readText = File.ReadAllLines(ALL_ORDERS).ToList();

                    string tmp = viewModel.Id.ToString();

                    foreach (string line in readText)
                    {
                        if (line[0].ToString().Equals(tmp))
                        {
                            await logWriter.WriteLineAsync($"{DateTime.Now}: Заказ с номером {viewModel.Id} уже создан");
                            return;
                        }
                    }

                    // Если не создан, то записываем
                    using (StreamWriter ordersWriter = new StreamWriter(ALL_ORDERS, true))
                    {
                        await ordersWriter.WriteLineAsync($"{viewModel.Id} {viewModel.Weight} " +
                            $"{viewModel.DistrictId} {viewModel.DeliveryDateTime}");
                    }
                    await logWriter.WriteLineAsync($"{DateTime.Now}: Заказ с номером {viewModel.Id} успешно создан");
                }
            }
            catch(Exception ex)
            {
                using (StreamWriter sw = new StreamWriter(DELIVERY_LOG, true))
                {
                    await sw.WriteLineAsync($"{DateTime.Now}: Заказ с номером" +
                        $" {viewModel.Id}. Ошибка: {ex.Message}");
                }
            }
        }

        // Удаение заказа
        public async void Delete(char id)
        {
            try
            {
                using (StreamWriter logWriter = new StreamWriter(DELIVERY_LOG, true))
                {
                    await logWriter.WriteLineAsync($"{DateTime.Now}: Удаление заказа с номером {id}...");

                    List<string> readText = File.ReadAllLines(ALL_ORDERS).ToList();

                    bool flag = false;

                    foreach (string line in readText)
                    {
                        if (line[0] == id)
                        {
                            readText.Remove(line);
                            flag = true;
                            break;
                        }
                    }

                    File.WriteAllLines(ALL_ORDERS, readText);
                    if (flag)
                        await logWriter.WriteLineAsync($"{DateTime.Now}: Заказ с номером {id} успешно удален");
                    else
                        await logWriter.WriteLineAsync($"{DateTime.Now}: Заказ с номером {id} не был найден");
                }
            }

            catch (Exception ex)
            {
                using (StreamWriter sw = new StreamWriter(DELIVERY_LOG, true))
                {
                    await sw.WriteLineAsync($"{DateTime.Now}: Заказ с номером {id}. Ошибка: {ex.Message}");
                }
            }
        }

        public IQueryable<OrderViewModel> Get()
        {
            throw new NotImplementedException();
        }
    }
}
