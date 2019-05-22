using DonorPlusLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace DonorPlus.ViewModels
{
    public class ChatPageViewModel : INotifyPropertyChanged
    {
        public bool ShowScrollTap { get; set; } = false;
        public bool LastMessageVisible { get; set; } = true;
        public int PendingMessageCount { get; set; } = 0;
        public bool PendingMessageCountVisible => PendingMessageCount > 0;

        public Queue<Models.Message> DelayedMessages { get; set; } = new Queue<Models.Message>();
        public ObservableCollection<Models.Message> Messages { get; set; } = new ObservableCollection<Models.Message>();
        public string TextToSend { get; set; }
        public ICommand OnSendCommand { get; set; }
        public ICommand MessageAppearingCommand { get; set; }
        public ICommand MessageDisappearingCommand { get; set; }

        private string[] mes =
        {
            "Привет",
            "Добро пожаловать!",
            "Здравствуйте!",
            "Как Ваши дела?",
            "Я всего лишь бот, на что вы рассчитываете?",
            "У Вас проблемы? Пишите 'donorplus.help@gmail.com'",
            "Вы приносите пользу обществу! Я Вами горжусь!!!",
            "У меня всё хорошо! А у Вас?",
            "Рад помочь!",
            "Ну это конечно да",
            "Не уверен :-(",
            "Смешно",
            "Интересненько"
        };
        private readonly Random random = new Random();

        public ChatPageViewModel()
        {
            MessageAppearingCommand = new Command<Models.Message>(OnMessageAppearing);
            MessageDisappearingCommand = new Command<Models.Message>(OnMessageDisappearing);

            if (!Storage.IsChatBot)
            {
                ResultObj result = MessageLog.GetMessages(Storage.User.Id, Storage.Friend.Id);
                if (result.Messages != null)
                {
                    foreach (Message message in result.Messages)
                    {
                        Messages.Insert(0, new Models.Message()
                        {
                            User = message.Id,
                            Text = message.Text,
                            Time = message.Time.ToLongTimeString(),
                            Date = message.Time.ToLongDateString()
                        });
                    }
                }

                OnSendCommand = new Command(() =>
                {
                    if (!string.IsNullOrEmpty(TextToSend))
                    {
                        MessageLog.Add(Storage.User.Id, Storage.Friend.Id, TextToSend.Trim());
                        TextToSend = string.Empty;
                    }
                });

                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    result = MessageLog.GetMessages(Storage.User.Id, Storage.Friend.Id);
                    if (result.Messages != null)
                    {
                        if (result.Messages.Count > Messages.Count)
                        {
                            for (int i = Messages.Count; i < result.Messages.Count; i++)
                            {
                                Models.Message message = new Models.Message()
                                {
                                    User = result.Messages[i].Id,
                                    Text = result.Messages[i].Text,
                                    Time = result.Messages[i].Time.ToLongTimeString(),
                                    Date = result.Messages[i].Time.ToLongDateString()
                                };
                                if (LastMessageVisible)
                                {
                                    Messages.Insert(0, message);
                                }
                                else
                                {
                                    DelayedMessages.Enqueue(message);
                                    PendingMessageCount++;
                                }
                            }
                        }
                    }
                    return Storage.IsChatNow;
                });
            }
            else
            {
                OnSendCommand = new Command(() =>
                {
                    if (!string.IsNullOrEmpty(TextToSend))
                    {
                        Messages.Insert(0, new Models.Message()
                        {
                            Text = TextToSend,
                            User = Storage.User.Id,
                            Date = DateTime.Now.ToLongDateString(),
                            Time = DateTime.Now.ToLongTimeString()
                        });
                        TextToSend = string.Empty;
                    }
                });

                //Code to simulate reveing a new message procces
                Device.StartTimer(TimeSpan.FromSeconds(3), () =>
                {
                    if (LastMessageVisible)
                    {
                        Messages.Insert(0, new Models.Message()
                        {
                            Text = mes[random.Next(mes.Length)],
                            User = -1,
                            Date = DateTime.Now.ToLongDateString(),
                            Time = DateTime.Now.ToLongTimeString()
                        });
                    }
                    else
                    {
                        DelayedMessages.Enqueue(new Models.Message()
                        {
                            Text = mes[random.Next(mes.Length)],
                            User = -1,
                            Date = DateTime.Now.ToLongDateString(),
                            Time = DateTime.Now.ToLongTimeString()
                        });
                        PendingMessageCount++;
                    }
                    return true;
                });
            }
        }

        private void OnMessageAppearing(Models.Message message)
        {
            int idx = Messages.IndexOf(message);
            if (idx <= 6)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    while (DelayedMessages.Count > 0)
                    {
                        Messages.Insert(0, DelayedMessages.Dequeue());
                    }
                    ShowScrollTap = false;
                    LastMessageVisible = true;
                    PendingMessageCount = 0;
                });
            }
        }

        private void OnMessageDisappearing(Models.Message message)
        {
            int idx = Messages.IndexOf(message);
            if (idx >= 6)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    ShowScrollTap = true;
                    LastMessageVisible = false;
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
