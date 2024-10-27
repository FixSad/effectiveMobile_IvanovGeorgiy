using DeliveryService.Domain.Response;
using DeliveryService.Domain.ViewModels;
using DeliveryService.Service.Interfaces;
using System.Globalization;
using System.IO;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text;

namespace DeliveryService.Service.Implementations
{
    public class OrderService : IOrderService<OrderViewModel>
    {
        // Создание заказа
        public async Task<IBaseResponse<OrderViewModel>> Create(OrderViewModel viewModel)
        {
            try
            {
                using (StreamWriter logWriter = new StreamWriter(Properties.Default.DeliveryLog + "DeliveryLog.txt", true))
                {
                    // автосоздание id

                    List<string> lines = new List<string>();
                    using (StreamWriter orderWriter = new StreamWriter(Properties.Default.AllOrders, true)){}
                    using (StreamReader ordersReader = new StreamReader(Properties.Default.AllOrders, true))
                    {
                        string? line;
                        while ((line = await ordersReader.ReadLineAsync()) != null)
                        {
                            lines.Add(line);
                        }
                    }

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
                using (StreamWriter sw = new StreamWriter(Properties.Default.DeliveryLog + "DeliveryLog.txt", true))
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
        public async Task<IBaseResponse<OrderViewModel>> Delete(string id)
        {
            try
            {
                using (StreamWriter logWriter = new StreamWriter(Properties.Default.DeliveryLog + "DeliveryLog.txt", true))
                {
                    await logWriter.WriteLineAsync($"{DateTime.Now}: Удаление заказа с номером {id}...");

                    List<string> readText = File.ReadAllLines(Properties.Default.AllOrders).ToList();

                    foreach (string line in readText)
                    {
                        var items = line.Split(' ').ToList();
                        if (items[0].Equals(id))
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
                using (StreamWriter sw = new StreamWriter(Properties.Default.DeliveryLog + "DeliveryLog.txt", true))
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

        public async Task<IBaseResponse<OrderViewModel>> Get(string id)
        {
            try
            {
                using (StreamWriter logWriter = new StreamWriter(Properties.Default.DeliveryLog + "DeliveryLog.txt", true))
                {
                    await logWriter.WriteLineAsync($"{DateTime.Now}: Получение заказа с номером {id}...");

                    List<string> readText = File.ReadAllLines(Properties.Default.AllOrders).ToList();

                    foreach (string line in readText)
                    {
                        var items = line.Split(' ').ToList();
                        if (items[0].Equals(id))
                        {
                            await logWriter.WriteLineAsync($"{DateTime.Now}: Заказ с номером {id} успешно получен");
                            string dt = $"{items[3]} {items[4]}";
                            var result = new OrderViewModel()
                            {
                                Id = int.Parse(id),
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
                                    Id = int.Parse(id),
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
                using (StreamWriter sw = new StreamWriter(Properties.Default.DeliveryLog + "DeliveryLog.txt", true))
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

        public async Task<IBaseResponse<List<OrderViewModel>>> Sort(string districtId, string startDateTime)
        {
            try
            {
                using (StreamWriter logWriter = new StreamWriter(Properties.Default.DeliveryLog + "DeliveryLog.txt", true))
                {
                    DateTime parseDateTime = DateTime.ParseExact(startDateTime, 
                        "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                    await logWriter.WriteLineAsync($"{DateTime.Now}: Сортировка заказов от {parseDateTime}...");

                    List<string> readText = File.ReadAllLines(Properties.Default.AllOrders).ToList();
                    List<OrderViewModel> list = new List<OrderViewModel>();
                    foreach (var line in readText)
                    {
                        var tmp = line.Split(' ').ToList();
                        string dt = $"{tmp[3]} {tmp[4]}";
                        if (tmp[2].Equals(districtId))
                            list.Add(new OrderViewModel()
                            {
                                Id = int.Parse(tmp[0]),
                                Weight = float.Parse(tmp[1]),
                                DistrictId = int.Parse(tmp[2]),
                                DeliveryDateTime = DateTime.Parse(dt)
                            });
                    }
                
                    List<OrderViewModel> res = list.Where(dt => dt.DeliveryDateTime >= parseDateTime 
                            && dt.DeliveryDateTime <= parseDateTime.AddMinutes(30)).ToList();

                    await logWriter.WriteLineAsync($"{DateTime.Now}: Сортировка заказов от {parseDateTime} успешна");

                    using (StreamWriter resultWriter = new StreamWriter(Properties.Default.DeliveryOrder, false))
                    {
                        foreach (var order in res)
                        {
                            await resultWriter.WriteLineAsync($"Id: {order.Id},  Weight: {order.Weight}, " +
                                $"District: {order.DistrictId}, Time: {order.DeliveryDateTime}");
                        }
                    }

                    return new BaseResponse<List<OrderViewModel>>()
                    {
                        IsSuccess = true,
                        Description = $"Результат был записан в файл DeliveryOrder.txt",
                        Data = res
                    };
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter logWriter = new StreamWriter(Properties.Default.DeliveryLog + "DeliveryLog.txt", true))
                {
                    await logWriter.WriteLineAsync($"{DateTime.Now}: Ошибка. {ex.Message}");

                    return new BaseResponse<List<OrderViewModel>>()
                    {
                        Description = $"Ошибка: {ex.Message}",
                        IsSuccess = false
                    };
                }
            }
        }

        public async Task<IBaseResponse<List<OrderViewModel>>> GetAllOrders()
        {
            try
            {
                using (StreamWriter logWriter = new StreamWriter(Properties.Default.DeliveryLog + "DeliveryLog.txt", true))
                {
                    await logWriter.WriteLineAsync($"{DateTime.Now}: Получение всех заказов...");

                    List<string> readText = File.ReadAllLines(Properties.Default.AllOrders).ToList();
                    List<OrderViewModel> list = new List<OrderViewModel>();
                    foreach (var line in readText)
                    {
                        var tmp = line.Split(' ').ToList();
                        string dt = $"{tmp[3]} {tmp[4]}";
                        list.Add(new OrderViewModel()
                        {
                            Id = int.Parse(tmp[0]),
                            Weight = float.Parse(tmp[1]),
                            DistrictId = int.Parse(tmp[2]),
                            DeliveryDateTime = DateTime.Parse(dt)
                        });
                    }

                    await logWriter.WriteLineAsync($"{DateTime.Now}: Заказы получены успешно");

                    return new BaseResponse<List<OrderViewModel>>()
                    {
                        IsSuccess = true,
                        Description = $"Результат был записан в файл DeliveryOrder.txt",
                        Data = list
                    };
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter logWriter = new StreamWriter(Properties.Default.DeliveryLog + "DeliveryLog.txt", true))
                {
                    await logWriter.WriteLineAsync($"{DateTime.Now}: Ошибка. {ex.Message}");

                    return new BaseResponse<List<OrderViewModel>>()
                    {
                        Description = $"Ошибка: {ex.Message}",
                        IsSuccess = false
                    };
                }
            }
        }
    }
}
