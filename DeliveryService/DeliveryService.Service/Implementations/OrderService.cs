using DeliveryService.Domain.Response;
using DeliveryService.Domain.ViewModels;
using DeliveryService.Service.Interfaces;
using System.Reflection.Metadata;

namespace DeliveryService.Service.Implementations
{
    public class OrderService : IOrderService<OrderViewModel>
    {
        // Создание заказа
        public async Task<IBaseResponse<OrderViewModel>> Create(OrderViewModel viewModel)
        {
            try
            {
                using (StreamWriter logWriter = new StreamWriter(Properties.Default.DeliveryLog, true))
                {
                    // автосоздание id
                    var lines = File.ReadLines(Properties.Default.AllOrders);
                    int id = 1;
                    if(File.ReadLines(Properties.Default.AllOrders).Count() > 0)
                    {
                        var lastLine = lines.LastOrDefault().Split(' ').ToList();
                        id = int.Parse(lastLine[0]) + id;
                    }
                    
                    viewModel.Id = id;

                    await logWriter.WriteLineAsync($"{DateTime.Now}: Создание заказа с номером {viewModel.Id}...");

                    // Проверка создан ли заказ с таким Id
                    List<string> readText = lines.ToList();

                    string tmp = viewModel.Id.ToString();

                    foreach (string line in readText)
                    {
                        if (line[0].ToString().Equals(tmp))
                        {
                            await logWriter.WriteLineAsync($"{DateTime.Now}: Заказ с номером {viewModel.Id} уже создан");
                            return new BaseResponse<OrderViewModel>()
                            {
                                Description = $"Заказ с номером {viewModel.Id} уже создан",
                                IsSuccess = false,
                            };
                        }
                    }

                    // Если не создан, то записываем
                    using (StreamWriter ordersWriter = new StreamWriter(Properties.Default.AllOrders, true))
                    {
                        await ordersWriter.WriteLineAsync($"{id} {viewModel.Weight} " +
                            $"{viewModel.DistrictId} {viewModel.DeliveryDateTime}");
                    }
                    await logWriter.WriteLineAsync($"{DateTime.Now}: Заказ с номером {viewModel.Id} успешно создан");
                    return new BaseResponse<OrderViewModel>()
                    {
                        Description = $"Заказ с номером {viewModel.Id} успешно создан",
                        IsSuccess = true
                    };
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter sw = new StreamWriter(Properties.Default.DeliveryLog, true))
                {
                    await sw.WriteLineAsync($"{DateTime.Now}: Заказ с номером" +
                        $" {viewModel.Id}. Ошибка: {ex.Message}");
                }
                return new BaseResponse<OrderViewModel>()
                {
                    Description = $"Ошибка: {ex.Message}",
                    IsSuccess = false,
                };
            }
        }

        // Удаение заказа
        public async Task<IBaseResponse<OrderViewModel>> Delete(char id)
        {
            try
            {
                using (StreamWriter logWriter = new StreamWriter(Properties.Default.DeliveryLog, true))
                {
                    await logWriter.WriteLineAsync($"{DateTime.Now}: Удаление заказа с номером {id}...");

                    List<string> readText = File.ReadAllLines(Properties.Default.AllOrders).ToList();

                    foreach (string line in readText)
                    {
                        var items = line.Split(' ').ToList();
                        if (items[0].Equals(id.ToString()))
                        {
                            readText.Remove(line);
                            await logWriter.WriteLineAsync($"{DateTime.Now}: Заказ с номером {id} успешно удален");
                            File.WriteAllLines(Properties.Default.AllOrders, readText);
                            return new BaseResponse<OrderViewModel>()
                            {
                                Description = $"Заказ с номером {id} успешно удален",
                                IsSuccess = true
                            };
                        }
                    }

                    await logWriter.WriteLineAsync($"{DateTime.Now}: Заказ с номером {id} не был найден");
                    return new BaseResponse<OrderViewModel>()
                    {
                        Description = $"Заказ с номером {id} не был найден",
                        IsSuccess = true
                    };
                }
            }

            catch (Exception ex)
            {
                using (StreamWriter sw = new StreamWriter(Properties.Default.DeliveryLog, true))
                {
                    await sw.WriteLineAsync($"{DateTime.Now}: Заказ с номером {id}. Ошибка: {ex.Message}");
                }
                return new BaseResponse<OrderViewModel>()
                {
                    Description = $"Ошибка: {ex.Message}",
                    IsSuccess = false
                };
            }
        }

        public async Task<IBaseResponse<OrderViewModel>> Get(char id)
        {
            try
            {
                using (StreamWriter logWriter = new StreamWriter(Properties.Default.DeliveryLog, true))
                {
                    await logWriter.WriteLineAsync($"{DateTime.Now}: Получение заказа с номером {id}...");

                    List<string> readText = File.ReadAllLines(Properties.Default.AllOrders).ToList();

                    foreach (string line in readText)
                    {
                        var items = line.Split(' ').ToList();
                        if (items[0].Equals(id.ToString()))
                        {
                            await logWriter.WriteLineAsync($"{DateTime.Now}: Заказ с номером {id} успешно получен");
                            string dt = $"{items[3]} {items[4]}";
                            var result = new OrderViewModel()
                            {
                                Id = id,
                                Weight = float.Parse(items[1]),
                                DistrictId = int.Parse(items[2]),
                                DeliveryDateTime = DateTime.Parse(dt)
                            };
                            return new BaseResponse<OrderViewModel>()
                            {
                                Description = $"Заказ с номером {id} успешно получен",
                                IsSuccess = true,
                                Data = new OrderViewModel()
                                {
                                    Id = id,
                                    Weight = float.Parse(items[1]),
                                    DistrictId = int.Parse(items[2]),
                                    DeliveryDateTime = DateTime.Parse(dt)
                                }
                            };
                        }
                    }

                    await logWriter.WriteLineAsync($"{DateTime.Now}: Заказ с номером {id} не был найден");
                    return new BaseResponse<OrderViewModel>()
                    {
                        Description = $"Заказ с номером {id} не был найден",
                        IsSuccess = false
                    };
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter sw = new StreamWriter(Properties.Default.DeliveryLog, true))
                {
                    await sw.WriteLineAsync($"{DateTime.Now}: Заказ с номером {id}. Ошибка: {ex.Message}");
                }
                return new BaseResponse<OrderViewModel>()
                {
                    Description = $"Ошибка: {ex.Message}",
                    IsSuccess = false
                };
            }
        }
    }
}
