using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace ReadBot
{
    public partial class Form1 : Form
    {
        public TelegramBotClient botClient;
        public long chatId = 6052997336; 

        int logCounter = 0;

        void AddLog(string msg)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.BeginInvoke((MethodInvoker)delegate ()
                {
                    AddLog(msg);
                });
            }
            else
            {
                logCounter++;
                if (logCounter > 100)
                {
                    txtLog.Clear();
                    logCounter = 0;
                }
                txtLog.AppendText(msg + "\r\n");
            }
            Console.WriteLine(msg);
        }

        /// <summary>
        /// hàm tạo: ko kiểu, trùng tên với class
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            string token = "6157928842:AAGjOCaEbDfXXeI-YCpTMVjZr9V1W6U70CI";
            botClient = new TelegramBotClient(token);  // Tạo 1 thằng bot 

            CancellationTokenSource cts = new CancellationTokenSource();  

            ReceiverOptions receiverOptions = new ReceiverOptions()
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
            };

            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,  
                pollingErrorHandler: HandlePollingErrorAsync,   
                receiverOptions: receiverOptions, 
                cancellationToken: cts.Token    
                                                
            );

            Task<User> me = botClient.GetMeAsync(); 

            AddLog($"Thằng bot: @{me.Result.Username}");


            async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
            {
                
                bool ok = false;
  
                Telegram.Bot.Types.Message message = null; // dấu ? để có thể gán null 

                
                if (update.Message != null)  
                {

                    message = update.Message;   
                    ok = true;
                }

                if (update.EditedMessage != null)
                {
                    message = update.EditedMessage;
                    ok = true;
                }

                if (!ok || message == null) return; 

                string messageText = message.Text;
                if (messageText == null) return;  

                chatId = message.Chat.Id; 

                AddLog($"{chatId}: {messageText}");  

                string reply = "";  
                string messLow = messageText.ToLower(); 

                // ----------- BẮT ĐẦU XỬ LÝ -----------------------------------------------------------------------------
  
                if (messLow.StartsWith("gv"))
                {
                    reply = "FeedBack Giáo viên:🥲 Môn học lập trình Windows thầy Đỗ Duy Cốp. Giảng quá xá là HAY!😍😍";
                }
                else if (messLow.StartsWith("/thoitiet")){ 
                    string input = messLow.Substring(10);
                    if (input.Contains(",")) {
                        string[] parts = input.Split(',');
                        reply = ThoiTiet.GetThoiTiet(parts[0], parts[1]);
                    }
                    else
                    {
                        reply = "Bạn nên nhập theo cú pháp: Địa điểm + ',' + thời gian";
                    }
                        

                }
                else 
                {
                    reply = "🤡Tôi nói pạn nghe: " + messageText;
                }


                // ----------- KẾT THÚC XỬ LÝ -----------------------------------------------------------------------
                AddLog(reply); 

                Telegram.Bot.Types.Message sentMessage = await botClient.SendTextMessageAsync(
                           
                           chatId: chatId, 
                           text: reply,    
                           parseMode: ParseMode.Html  
                                                      
                      );

                
            }


            Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
            {

                Console.WriteLine("Looi roi anh ouwi");
                AddLog("----       Lỗi rồi -> K rõ lỗi j  -----------");
                return Task.CompletedTask;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Visible = false;
        }
    }
}

