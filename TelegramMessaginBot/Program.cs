using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using static System.Console;


namespace TelegramMessaginBot
{
    internal class Program
    {
        private static TelegramBotClient _telegramBot = new TelegramBotClient("5111567607:AAGND012YSubmD6tfUFklst9iM_AJVsNLQU");
        private static string _fileName = "information.json";
        private static List<UpdateBot> updateBots = new List<UpdateBot>();

        static void Main(string[] args)
        {

            #region Read recived information
            try
            {
                var updatedInfo = System.IO.File.ReadAllText(_fileName);
                var updateBots = JsonConvert.DeserializeObject<List<UpdateBot>>(updatedInfo);
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message);
            }

            #endregion
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new UpdateType[]
                {
                    UpdateType.Message,
                    UpdateType.EditedMessage
                }
            };
            _telegramBot.StartReceiving(UpdateHandler,ErrorHandler,receiverOptions);
            
            ReadLine();
        }

        private static Task ErrorHandler(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw new NotImplementedException();
        }

        private static async Task UpdateHandler(ITelegramBotClient telegramBot, Update update, CancellationToken arg3)
        {
           if(update.Type == UpdateType.Message)
            {
                if (update.Message.Type == MessageType.Text)
                {
                    //Write into JSON File
                    var updateBot = new  UpdateBot
                    {
                        Id = update.Message.Chat.Id,
                        FirstName = update.Message.Chat.FirstName,
                        LastName = update.Message.Chat.LastName,
                        UserName = update.Message.Chat.Username,
                        Description = update.Message.Chat.Description,
                        RecivedText = update.Message.Text

                    };

                    updateBots.Add(updateBot);
                    var botUpdateString = JsonConvert.SerializeObject(updateBots);
                    System.IO.File.WriteAllText(_fileName, botUpdateString);

                    #region Send Message and Photos
                    if (updateBot.RecivedText == "Hello")
                    {
                        await _telegramBot.SendTextMessageAsync(update.Message.Chat.Id, $"Hello {update.Message.Chat.FirstName}");
                    }
                    else if (updateBot.RecivedText == "contact")
                    {
                        await _telegramBot.SendTextMessageAsync(update.Message.Chat.Id, "Contact me on Facebook!");
                    }
                    else if (updateBot.RecivedText.StartsWith("count("))
                    {
                        string counted = "Total number of chars in your string is: ";
                        counted += updateBot.RecivedText.Split('(')[1].Split(')')[0].Length.ToString();
                        await _telegramBot.SendTextMessageAsync(update.Message.Chat.Id, counted);
                    }
                    else if (updateBot.RecivedText == "dog")
                    {
                        _telegramBot.SendPhotoAsync(update.Message.Chat.Id, "https://hips.hearstapps.com/clv.h-cdn.co/assets/17/29/1500566326-gettyimages-512366437-1.jpg");
                    }
                    else if (updateBot.RecivedText == "goat")
                    {
                        _telegramBot.SendPhotoAsync(update.Message.Chat.Id, "https://www.sandfmeatshop.com/wp-content/uploads/2021/10/4488.jpg");
                    }
                    else if (updateBot.RecivedText == "panda")
                    {
                        _telegramBot.SendPhotoAsync(update.Message.Chat.Id, "https://media.istockphoto.com/photos/cute-panda-bear-climbing-in-tree-picture-id523761634?k=20&m=523761634&s=612x612&w=0&h=fycQb31QlRoNLdJWWddooJ94a-54YLYQ3ggTLPkhvmk=");
                    }


                    #endregion
                }
            }
        }
    }
}
