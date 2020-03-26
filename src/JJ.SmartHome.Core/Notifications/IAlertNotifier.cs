﻿using System.Threading.Tasks;

namespace JJ.SmartHome.Core.Notifications
{
    public interface IAlertNotifier
    {
        Task Notify(string title, string content);
    }
}